using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net.Mime;
using ContentType = RestSharp.Serializers.ContentType;
using System.Net;
using static PdfConvertMicroserviceTestClient.MainForm;
using System.Xml.Serialization;
using Newtonsoft.Json.Serialization;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using File = System.IO.File;

namespace PdfConvertMicroserviceTestClient
{
    public partial class MainForm : Form
    {
        RestClient _client = null;
        BindingList<Request> _requests = new BindingList<Request>();
        BindingList<string> _outputFolders = new BindingList<string>();
        BackgroundWorker _requestWorker = null;
        BindingList<Server> _servers = new BindingList<Server>();
        List<WorkerArg> _works = new List<WorkerArg>();

        int _selectedRequestIndex = -1;
        Timer _timer = null;
        private Guid _userId = Guid.Empty;
        private BindingList<Job> _activeJobs { get; set; }
        private Dictionary<Guid, DateTime> _jobStartTime = new Dictionary<Guid, DateTime>();
        private Dictionary<Guid, TimeSpan> _jobElapsedTime = new Dictionary<Guid, TimeSpan>();

        public class Server
        {
            public string BaseUrl { get; set; }
            public bool Authenticate { get; set; }
            public bool APIv2 { get; set; }
        }

        public MainForm()
        {
            InitializeComponent();

            _requestWorker = new BackgroundWorker();
            _requestWorker.DoWork += _requestWorker_DoWork;
            _requestWorker.RunWorkerCompleted += _requestWorker_RunWorkerCompleted;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string[] servers = Properties.Settings.Default.EndpointBaseUrl.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string server in servers)
            {
                string[] server_ = server.Split(new char[] { '#' });
                if (server_.Length > 0)
                {
                    bool apiv2 = false;
                    bool.TryParse(server_[2], out apiv2);
                    _servers.Add(new Server
                    {
                        BaseUrl = server_[0],
                        Authenticate = bool.Parse(server_[1]),
                        APIv2 = apiv2
                    });
                }
                else
                {
                    _servers.Add(new Server
                    {
                        BaseUrl = server_[0],
                        Authenticate = true,
                        APIv2 = false
                    });
                }
            }

            cmbEndpointBaseUrl.DisplayMember = "BaseUrl";
            cmbEndpointBaseUrl.DataSource = _servers;
            if (_servers.Count > 0)
                chkAuthenticate.Checked = _servers[0].Authenticate;

            txtEndpoint.Text = Properties.Settings.Default.Endpoint;
            txtParameters.Text = Properties.Settings.Default.Parameters;
            txtUsername.Text = Properties.Settings.Default.Username;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtSerialNo.Text = Properties.Settings.Default.SerialNo;
            txtApimSubscriptionKey.Text = Properties.Settings.Default.ApimSubscriptionKey;
            txtAuthToken.Text = Properties.Settings.Default.AuthToken;

            cmbRequest.DisplayMember = "Name";

            if (!string.IsNullOrEmpty(Properties.Settings.Default.Requests))
            {
                string[] requests_ = Properties.Settings.Default.Requests.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string requestString in requests_)
                {
                    string[] request = requestString.Split(new char[] { '#' });
                    string[] files_ = request[4].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    Request r = new Request
                    {
                        Name = request[0],
                        Type = request[1],
                        Endpoint = request[2],
                        Parameters = request[3],
                        Files = new BindingList<string>(files_.ToList())
                    };

                    _requests.Add(r);
                }

                cmbRequest.DataSource = _requests;

                if (Properties.Settings.Default.SelectedRequestIndex >= 0 && Properties.Settings.Default.SelectedRequestIndex < _requests.Count)
                {
                    _selectedRequestIndex = Properties.Settings.Default.SelectedRequestIndex;
                    cmbRequest.SelectedIndex = _selectedRequestIndex;
                    lstBodyFiles.DataSource = _requests[_selectedRequestIndex].Files;
                }
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.OutputFolders))
            {
                _outputFolders = new BindingList<string>(Properties.Settings.Default.OutputFolders.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
            else
            {
                _outputFolders = new BindingList<string>();
            }

            cmbOutputFolder.DataSource = _outputFolders;

            if (Properties.Settings.Default.EndpointBaseUrlSelectedIndex < cmbEndpointBaseUrl.Items.Count)
            {
                cmbEndpointBaseUrl.SelectedIndex = Properties.Settings.Default.EndpointBaseUrlSelectedIndex;
            }

            _activeJobs = new BindingList<Job>();

            blvJobs.DataSource = _activeJobs;
            blvJobs.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            blvJobs.Columns[0].Width = 200;
            blvJobs.Columns[1].Width = 200;
            blvJobs.Columns[2].Width = 80;
            blvJobs.Columns[3].Width = 120;

            _timer = new Timer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = 3000;
            _timer.Start();
        }

        private static CommandTypesBinder commandTypesBinder = new CommandTypesBinder()
        {
            CommandTypes = new List<Type>()
            {
                typeof(DocProcessStatus),
                typeof(DocProcessStatus.Message)
            }
        };

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            if (_activeJobs.Count > 0)
            {
                try
                {
                    foreach (Job job in _activeJobs)
                    {
                        if (job.Status == "Delivered" || job.Status == "Failed")
                            continue;

                        var request = new RestRequest("JobStatus");
                        request.AddQueryParameter("UserId", _userId.ToString());
                        request.AddQueryParameter("JobId", job.Id);
                        RestResponse response = _client.Execute(request);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (!string.IsNullOrEmpty(response.Content))
                            {
                                DocProcessStatus jobStatus = JsonConvert.DeserializeObject<DocProcessStatus>(response.Content, new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.Objects,
                                    SerializationBinder = commandTypesBinder
                                });

                                Job job_ = _activeJobs.Where<Job>(j => j.Id == jobStatus.JobID).FirstOrDefault();

                                if (job_ != null)
                                {
                                    int i = _activeJobs.IndexOf(job_);
                                    _activeJobs[i].Status = jobStatus.Status.ToString();

                                    if (!job_.ResultSaved)
                                    {
                                        if (!_jobStartTime.ContainsKey(job_.Id))
                                        {
                                            _jobStartTime.Add(job_.Id, DateTime.Now);
                                            _jobElapsedTime.Add(job_.Id, TimeSpan.FromSeconds(0));
                                        }
                                        else
                                        {
                                            _jobElapsedTime[job_.Id] = DateTime.Now - _jobStartTime[job_.Id];
                                            _activeJobs[i].ElapsedTime = _jobElapsedTime[job_.Id];
                                        }
                                    }

                                    if (jobStatus.Status == DocProcessStatus.StatusType.Delivered && !job_.ResultSaved)
                                    {
                                        _activeJobs[i].ResultSaved = true;

                                        if (_jobStartTime.ContainsKey(job_.Id))
                                        {
                                            _jobElapsedTime[job_.Id] = DateTime.Now - _jobStartTime[job_.Id];
                                            _activeJobs[i].ElapsedTime = _jobElapsedTime[job_.Id];
                                        }

                                        WorkerArg work = new WorkerArg
                                        {
                                            Iteration = i,
                                            Name = jobStatus.Title + " - Saving",
                                            Method = Method.Get,
                                            BaseUrl = cmbEndpointBaseUrl.Text,
                                            Endpoint = "JobResult",
                                            Parameters = $"?userId={_userId}&jobId={job_.Id}",
                                            ParametersInQuery = true,
                                            Files = null,
                                            ApimSubscriptionKey = string.Empty,
                                            AuthToken = string.Empty,
                                            OutputFolder = cmbOutputFolder.Text,
                                            APIv2 = true,
                                            Action = WorkerArg.WorkAction.DownloadResult
                                        };
                                        _works.Add(work); // Put in queue
                                    }
                                }
                            }
                            else
                                _activeJobs.Remove(job);
                        }
                        else
                        {
                            //Job job_ = _activeJobs.Where<Job>(j => j.Id == job.Id).FirstOrDefault();

                            //if (job_ != null)
                            //{
                            //    _activeJobs.Remove(job_);
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                   // AddOutput($"A problem occurred: {ex.Message}");
                }
            }

            if (!_requestWorker.IsBusy && _works.Count > 0)
            {
                List<WorkerArg> works = new List<WorkerArg>();
                works.Add(_works[0]);
                _works.RemoveAt(0);
                _requestWorker.RunWorkerAsync(works);
            }

            _timer.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string endpointBaseUrls = string.Empty;
            foreach (Server server in cmbEndpointBaseUrl.Items)
            {
                endpointBaseUrls += $"{server.BaseUrl}#{server.Authenticate}#{server.APIv2};";
            }
            Properties.Settings.Default.EndpointBaseUrlSelectedIndex = cmbEndpointBaseUrl.SelectedIndex;
            Properties.Settings.Default.EndpointBaseUrl = endpointBaseUrls;
            Properties.Settings.Default.Endpoint = txtEndpoint.Text;
            Properties.Settings.Default.Parameters = txtParameters.Text;
            Properties.Settings.Default.Username = txtUsername.Text;
            Properties.Settings.Default.Password = txtPassword.Text;
            Properties.Settings.Default.SerialNo = txtSerialNo.Text;
            Properties.Settings.Default.ApimSubscriptionKey = txtApimSubscriptionKey.Text;
            Properties.Settings.Default.AuthToken = txtAuthToken.Text;

            string requestsString = string.Empty;
            foreach (Request r in _requests)
            {
                string filesString = string.Empty;
                foreach (string f in r.Files)
                    filesString += f + ';';
                filesString.TrimEnd(new char[] { ';' });
                requestsString += $"{r.Name}#{r.Type}#{r.Endpoint}#{r.Parameters}#{filesString}$";
            }
            requestsString.TrimEnd(new char[] { '$' });
            Properties.Settings.Default.Requests = requestsString;

            string outputFoldersString = string.Empty;
            foreach (string s in _outputFolders)
                outputFoldersString += s + ';';
            outputFoldersString.TrimEnd(new char[] { ';' });
            Properties.Settings.Default.OutputFolders = outputFoldersString;

            Properties.Settings.Default.Save();
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            if (_selectedRequestIndex >= 0)
            {
                WorkerArg work = new WorkerArg
                {
                    Iteration = 0,
                    Name = cmbRequest.Text,
                    Method = rbtnPost.Checked ? Method.Post : Method.Get,
                    BaseUrl = cmbEndpointBaseUrl.Text,
                    Endpoint = txtEndpoint.Text,
                    Parameters = txtParameters.Text,
                    ParametersInQuery = true,
                    Files = _requests[_selectedRequestIndex].Files.ToList(),
                    ApimSubscriptionKey = chkAuthenticate.Checked ? txtApimSubscriptionKey.Text : string.Empty,
                    AuthToken = chkAuthenticate.Checked ? txtAuthToken.Text : string.Empty,
                    OutputFolder = cmbOutputFolder.Text,
                    APIv2 = chkAPIv2.Checked,
                    Action = WorkerArg.WorkAction.SendRequest
                };

                _works.Add(work);
            }
        }

        private void btnRunCollection_Click(object sender, EventArgs e)
        {
            if (_requestWorker.IsBusy)
            {
                MessageBox.Show("The request worker is busy, please try again later");
                return;
            }

            for (int i = 0; i < numRunRepeats.Value; i++)
            {
                foreach (Request w in _requests)
                {
                    WorkerArg work = new WorkerArg
                    {
                        Iteration = i,
                        Name = w.Name,
                        Method = w.Type == "POST" ? Method.Post : Method.Get,
                        BaseUrl = cmbEndpointBaseUrl.Text,
                        Endpoint = w.Endpoint,
                        Parameters = w.Parameters,
                        ParametersInQuery = true,
                        Files = w.Files.ToList(),
                        ApimSubscriptionKey = chkAuthenticate.Checked ? txtApimSubscriptionKey.Text : String.Empty,
                        AuthToken = chkAuthenticate.Checked ? txtAuthToken.Text : string.Empty,
                        OutputFolder = cmbOutputFolder.Text, 
                        APIv2 = chkAPIv2.Checked,
                        Action = WorkerArg.WorkAction.SendRequest
                    };

                    _works.Add(work);
                }
            }

            _running = true;
        }

        public struct WorkerArg
        {
            public enum WorkAction { SendRequest, DownloadResult };

            public int Iteration;
            public string Name;
            public Method Method;
            public string BaseUrl;
            public string Endpoint;
            public string Parameters;
            public bool ParametersInQuery;
            public List<string> Files;
            public string ApimSubscriptionKey;
            public string AuthToken;
            public string OutputFolder;
            public RestResponse Response;
            public bool APIv2;
            public Guid JobId;
            public WorkAction Action;
        }

        private void _requestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SetBusy(true);

            List<WorkerArg> work = e.Argument as List<WorkerArg>;

            foreach (WorkerArg w in work)
            {
                WorkerArg wresp = w;

                if (w.Action == WorkerArg.WorkAction.SendRequest)
                {
                    if (work.Count == 1 && string.IsNullOrEmpty(work[0].OutputFolder))
                    {
                        if (work[0].APIv2)
                        {
                            wresp.Response = SendRequestAPIv2(work[0].Name, work[0].Method, work[0].Endpoint, work[0].Parameters, work[0].Files, 0);
                            e.Result = wresp;
                        }
                        else
                        {
                            wresp.Response = SendRequest(work[0].Name, work[0].Method, work[0].Endpoint, work[0].Parameters, work[0].Files, work[0].ApimSubscriptionKey, work[0].AuthToken, work[0].OutputFolder, 0);
                            e.Result = wresp;
                        }
                    }
                    else
                    {
                        if (w.APIv2)
                        {
                            if (_userId != Guid.Empty)
                            {
                                wresp.Response = SendRequestAPIv2(w.Name, w.Method, w.Endpoint, w.Parameters, w.Files, w.Iteration);
                            }
                        }
                        else
                        {
                            wresp.Response = SendRequest(w.Name, w.Method, w.Endpoint, w.Parameters, w.Files, w.ApimSubscriptionKey, w.AuthToken, w.OutputFolder, w.Iteration);
                            if (_abortRun)
                            {
                                AddOutput("***Run aborted***", Color.DarkOrange);
                                break;
                            }
                        }
                        e.Result = wresp;
                    }
                }
                else if (w.Action == WorkerArg.WorkAction.DownloadResult)
                {
                    var request = new RestRequest(w.Endpoint, Method.Get);
//                    request.AlwaysMultipartFormData = true;
//                    request.AddHeader("Content-Type", "multipart/form-data");

                    Exception ex_ = null;

                    try
                    {
                        if (!string.IsNullOrEmpty(w.Parameters) && w.Parameters.Contains('='))
                        {
                            foreach (string param in w.Parameters.Split(new char[] { '?', '&' }))
                            {
                                if (string.IsNullOrWhiteSpace(param))
                                    continue;

                                string[] keyValue = param.Split('=');

                                if (keyValue.Length != 2)
                                    continue;

                                if (w.ParametersInQuery)
                                    request.AddQueryParameter(keyValue[0], keyValue[1]);
                                else
                                    request.AddParameter(keyValue[0], keyValue[1]);
                            }
                        }

                        AddOutput("-----------------------------------------------");
                        AddOutput($"Sending request '{w.Name}':  GET /{w.Endpoint}{w.Parameters}", Color.DarkSlateBlue);
                        DateTime startTime = DateTime.Now;


                        wresp.Response = _client.Execute(request);

                        TimeSpan timeSpent = DateTime.Now - startTime;

                        AddOutput($"Request finished in {timeSpent.Minutes}m {timeSpent.Seconds}.{timeSpent.Milliseconds}s", Color.DarkOliveGreen);

                        if (wresp.Response.ContentType == "application/pdf")
                        {
                            var contentDispositionHeaderValue = wresp.Response.ContentHeaders.FirstOrDefault(x => string.Compare(x.Name, "Content-Disposition", true) == 0)?.Value;
                            string contentDispositionString = Convert.ToString(contentDispositionHeaderValue);
                            string fileName = "Response.pdf";
                            if (!string.IsNullOrWhiteSpace(contentDispositionString))
                            {
                                ContentDisposition contentDisposition = new ContentDisposition(contentDispositionString);
                                fileName = contentDisposition.FileName;
                            }

                            double kB = (double)wresp.Response.RawBytes.Count() / 1024.0;

                            AddOutput($"Response: application/pdf {fileName} {kB:0.00} kB", Color.DarkOliveGreen);

                            if (Directory.Exists(w.OutputFolder))
                            {
                                // Auto Save :-)
                                string saveFilename = Path.Combine(w.OutputFolder, fileName);
                                int i = 1;
                                while (System.IO.File.Exists(saveFilename))
                                {
                                    saveFilename = Path.Combine(w.OutputFolder, $"{Path.GetFileNameWithoutExtension(fileName)} ({i}){Path.GetExtension(fileName)}");
                                    i++;
                                }
                                System.IO.File.WriteAllBytes(saveFilename, wresp.Response.RawBytes);
                                AddOutput($"Auto saved as {Path.GetFileName(saveFilename)} in {w.OutputFolder}", Color.DarkOliveGreen);
                                e.Result = null;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ex_ = ex;
                    }
                }
            }
        }

        private void _requestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _running = false;
            _abortRun = false;

            SetBusy(false);

            if (e.Result != null)
            {
                WorkerArg w = (WorkerArg)e.Result;

                if (w.Response != null && w.Response.ContentType == "application/pdf")
                {
                    var contentDispositionHeaderValue = w.Response.ContentHeaders.FirstOrDefault(x => x.Name == "Content-Disposition")?.Value;
                    string contentDispositionString = Convert.ToString(contentDispositionHeaderValue);
                    string fileName = "Response.pdf";
                    if (!string.IsNullOrWhiteSpace(contentDispositionString))
                    {
                        ContentDisposition contentDisposition = new ContentDisposition(contentDispositionString);
                        fileName = contentDisposition.FileName;
                    }

                    if (string.IsNullOrEmpty(cmbOutputFolder.Text))
                    {
                        SaveFileDialog dlg = new SaveFileDialog();
                        dlg.FileName = fileName;
                        dlg.OverwritePrompt = true;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(dlg.FileName, w.Response.RawBytes);
                        }
                    }
                }
                else if (w.Response != null && w.Response.ContentType == ContentType.Json)
                {
                    string json = (w.Response.Content);

                    var writerOptions = new JsonWriterOptions
                    {
                        Indented = true
                    };

                    try
                    {
                        using (var jsonDoc = JsonDocument.Parse(json))
                        {
                            using (var ms = new MemoryStream())
                            {
                                using (var writer = new Utf8JsonWriter(ms, writerOptions))
                                {
                                    jsonDoc.WriteTo(writer);
                                    writer.Flush();
                                    txtJsonResponse.Text = Encoding.UTF8.GetString(ms.ToArray());

                                    if (Directory.Exists(w.OutputFolder))
                                    {
                                        string saveFilename = Path.Combine(w.OutputFolder, Path.ChangeExtension(w.Endpoint, "json"));
                                        int i = 1;
                                        while (File.Exists(saveFilename))
                                        {
                                            saveFilename = Path.Combine(w.OutputFolder, $"{w.Endpoint} ({i}).json");
                                            i++;
                                        }
                                        File.WriteAllText(saveFilename, Encoding.UTF8.GetString(ms.ToArray()));
                                        AddOutput($"Auto saved as {Path.GetFileName(saveFilename)} in {w.OutputFolder}", Color.DarkOliveGreen);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AddOutput($"An error occurred: {ex.Message}", Color.DarkRed);
                    }
                }
                else if (w.Response != null)
                {
                    AddOutput($"Response: {w.Response.StatusCode.ToString()}", Color.Magenta);
                }
            }
        }

        private RestResponse SendRequest(string name, Method method, string endpoint, string parameters, List<string> files, string apimSubscriptionKey, string authToken, string outputFolder, int iteration)
        {
            if (_client == null)
            {
                AddOutput("Request failed - no connection");
                return null;
            }

            if (method == Method.Post)
            {
                var request = new RestRequest(endpoint, method);
                request.AlwaysMultipartFormData = true;
                request.AddHeader("Content-Type", "multipart/form-data");
                if (!string.IsNullOrEmpty(apimSubscriptionKey))
                    request.AddHeader("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
                if (!string.IsNullOrEmpty(authToken))
                    request.AddHeader("Authorization", $"Bearer {authToken}");

                RestResponse response = null;
                Exception ex_ = null;

                try
                {
                    if (!string.IsNullOrEmpty(parameters) && parameters.Contains('='))
                    {
                        foreach (string param in parameters.Split(new char[] { '?', '&' }))
                        {
                            if (string.IsNullOrWhiteSpace(param))
                                continue;

                            string[] keyValue = param.Split('=');

                            if (keyValue.Length != 2)
                                continue;

                            request.AddQueryParameter(keyValue[0], keyValue[1]);
                        }
                    }

                    foreach (string filename in files)
                    {
                        request.AddFile(Path.GetFileName(filename), filename);
                    }

                    AddOutput("-----------------------------------------------");
                    AddOutput($"Sending request '{name}' (i={iteration}):  POST /{endpoint}{parameters} ({files.Count} files in body)", Color.DarkSlateBlue);

                    DateTime startTime = DateTime.Now;

                    response = _client.Execute(request);

                    TimeSpan timeSpent = DateTime.Now - startTime;

                    AddOutput($"Request finished in {timeSpent.Minutes}m {timeSpent.Seconds}.{timeSpent.Milliseconds}s", Color.DarkOliveGreen);
                }
                catch (Exception ex)
                {
                    ex_ = ex;
                }

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.ContentType == "application/pdf")
                    {
                        var contentDispositionHeaderValue = response.ContentHeaders.FirstOrDefault(x => string.Compare(x.Name, "Content-Disposition", true) == 0)?.Value;
                        string contentDispositionString = Convert.ToString(contentDispositionHeaderValue);
                        string fileName = "Response.pdf";
                        if (!string.IsNullOrWhiteSpace(contentDispositionString))
                        {
                            ContentDisposition contentDisposition = new ContentDisposition(contentDispositionString);
                            fileName = contentDisposition.FileName;
                        }

                        double kB = (double)response.RawBytes.Count() / 1024.0;

                        AddOutput($"Response: application/pdf {fileName} {kB:0.00} kB", Color.DarkOliveGreen);

                        if (Directory.Exists(outputFolder))
                        {
                            // Auto Save :-)
                            string saveFilename = Path.Combine(outputFolder, fileName);
                            int i = 1;
                            while (System.IO.File.Exists(saveFilename))
                            {
                                saveFilename = Path.Combine(outputFolder, $"{Path.GetFileNameWithoutExtension(fileName)} ({i}){Path.GetExtension(fileName)}");
                                i++;
                            }
                            System.IO.File.WriteAllBytes(saveFilename, response.RawBytes);
                            AddOutput($"Auto saved as {Path.GetFileName(saveFilename)} in {outputFolder}", Color.DarkOliveGreen);
                            return null;
                        }
                        else
                        {
                            return response;
                        }
                    }
                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    AddOutput($"Request failed: Internal Server Error ({response.StatusCode})", Color.DarkRed);
                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                {
                    AddOutput($"Request failed: Gateway Timeout ({response.StatusCode})", Color.DarkRed);
                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                {
                    AddOutput($"Request failed: Request Timeout ({response.StatusCode})", Color.DarkRed);
                }
                else
                {
                    if (ex_ == null)
                    {
                        AddOutput($"Request failed: Unknown Error", Color.DarkRed);
                    }
                    else
                    {
                        AddOutput($"Request failed: {ex_.Message}", Color.DarkRed);
                    }
                }
            }
            else if (method == Method.Get)
            {
                try
                {
                    AddOutput("-----------------------------------------------");
                    AddOutput($"Sending request '{name}' (i={iteration}):  GET /{endpoint}{parameters} ({files.Count} files in body)", Color.DarkSlateBlue);

                    DateTime startTime = DateTime.Now;

                    var request = new RestRequest(endpoint, Method.Get);
                    RestResponse response = _client.Execute(request);

                    TimeSpan timeSpent = DateTime.Now - startTime;

                    AddOutput($"Request finished in {timeSpent.Minutes}m {timeSpent.Seconds}.{timeSpent.Milliseconds}s", Color.DarkOliveGreen);

                    if (response != null && response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        AddOutput($"Request failed: Internal Server Error ({response.StatusCode})", Color.DarkRed);
                    }
                    else if (response != null && response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                    {
                        AddOutput($"Request failed: Gateway Timeout ({response.StatusCode})", Color.DarkRed);
                    }
                    else if (response != null && response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                    {
                        AddOutput($"Request failed: Request Timeout ({response.StatusCode})", Color.DarkRed);
                    }

                    if (Directory.Exists(outputFolder))
                    {
                        if (response.ContentType == ContentType.Json)
                        {
                            string json = (response.Content);

                            var writerOptions = new JsonWriterOptions
                            {
                                Indented = true
                            };

                            try
                            {
                                using (var jsonDoc = JsonDocument.Parse(json))
                                {
                                    using (var ms = new MemoryStream())
                                    {
                                        using (var writer = new Utf8JsonWriter(ms, writerOptions))
                                        {
                                            jsonDoc.WriteTo(writer);
                                            writer.Flush();

                                            string saveFilename = Path.Combine(outputFolder, Path.ChangeExtension(endpoint, "json"));
                                            int i = 1;
                                            while (File.Exists(saveFilename))
                                            {
                                                saveFilename = Path.Combine(outputFolder, $"{endpoint} ({i}).json");
                                                i++;
                                            }
                                            File.WriteAllText(saveFilename, Encoding.UTF8.GetString(ms.ToArray()));
                                            AddOutput($"Auto saved as {Path.GetFileName(saveFilename)} in {outputFolder}", Color.DarkOliveGreen);
                                            return null;

                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        return response;
                    }


                    return response;
                }
                catch (Exception ex)
                {
                    AddOutput($"Request failed: {ex.Message}", Color.DarkRed);
                }
            }

            return null;
        }

        private RestResponse SendRequestAPIv2(string name, Method method, string endpoint, string parameters, List<string> files, int iteration)
        {
            if (_client == null)
            {
                AddOutput("Error sending request - no connection");
                return null;
            }

            if (method == Method.Post)
            {
                var request = new RestRequest(endpoint, method);
                request.AlwaysMultipartFormData = true;
                request.AddHeader("Content-Type", "multipart/form-data");

                RestResponse response = null;
                Exception ex_ = null;

                try
                {
                    request.AddQueryParameter("UserId", _userId.ToString());

                    if (!string.IsNullOrEmpty(parameters) && parameters.Contains('='))
                    {
                        foreach (string param in parameters.Split(new char[] { '?', '&' }))
                        {
                            if (string.IsNullOrWhiteSpace(param))
                                continue;

                            string[] keyValue = param.Split('=');

                            if (keyValue.Length != 2)
                                continue;

                            request.AddQueryParameter(keyValue[0], keyValue[1]);
                        }
                    }

                    foreach (string filename in files)
                    {
                        request.AddFile(Path.GetFileName(filename), filename);
                    }

                    AddOutput("-----------------------------------------------");
                    AddOutput($"Sending request '{name}' (i={iteration}):  POST /{endpoint}{parameters} ({files.Count} files in body)", Color.DarkSlateBlue);

                    response = _client.Execute(request);

                    try
                    {
                        using (var jsonDoc = JsonDocument.Parse(response.Content))
                        {
                            JsonElement root = jsonDoc.RootElement;
                            JsonElement e = root.GetProperty("jobId");
                            Guid jobId = Guid.Parse(e.GetString());
                            _activeJobs.Add(new Job
                            {
                                Id = jobId,
                                Name = name,
                                Status = "Created",
                                ElapsedTime = TimeSpan.Zero
                            });

                            if (!_jobStartTime.ContainsKey(jobId))
                            {
                                _jobStartTime.Add(jobId, DateTime.Now);
                                _jobElapsedTime.Add(jobId, TimeSpan.Zero);
                            }
                            else
                            {
                                _jobStartTime[jobId] = DateTime.Now;
                                _jobElapsedTime[jobId] = TimeSpan.Zero;
                            }

                            AddOutput($"Job submitted, ID = {jobId}", Color.DarkOliveGreen);
                        }
                    }
                    catch { }


                }
                catch (Exception ex)
                {
                    ex_ = ex;
                }

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response;
                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    AddOutput($"Request failed: Internal Server Error ({response.StatusCode})", Color.DarkRed);
                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                {
                    AddOutput($"Request failed: Gateway Timeout ({response.StatusCode})", Color.DarkRed);
                }
                else if (response != null && response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                {
                    AddOutput($"Request failed: Request Timeout ({response.StatusCode})", Color.DarkRed);
                }
                else
                {
                    if (ex_ == null)
                    {
                        AddOutput($"Request failed: Unknown Error", Color.DarkRed);
                    }
                    else
                    {
                        AddOutput($"Request failed: {ex_.Message}", Color.DarkRed);
                    }
                }
            }
            else if (method == Method.Get)
            {
                try
                {
                    AddOutput("-----------------------------------------------");
                    AddOutput($"Sending request '{name}' (i={iteration}):  GET /{endpoint}{parameters} ({files.Count} files in body)", Color.DarkSlateBlue);

                    DateTime startTime = DateTime.Now;

                    var request = new RestRequest(endpoint, Method.Get);
                    RestResponse response = _client.Execute(request);

                    TimeSpan timeSpent = DateTime.Now - startTime;

                    AddOutput($"Request finished in {timeSpent.Minutes}m {timeSpent.Seconds}.{timeSpent.Milliseconds}s", Color.DarkOliveGreen);

                    if (response != null && response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        AddOutput($"Request failed: Internal Server Error ({response.StatusCode})", Color.DarkRed);
                    }
                    else if (response != null && response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                    {
                        AddOutput($"Request failed: Gateway Timeout ({response.StatusCode})", Color.DarkRed);
                    }
                    else if (response != null && response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                    {
                        AddOutput($"Request failed: Request Timeout ({response.StatusCode})", Color.DarkRed);
                    }
                    else
                    {
                        return response;
                    }


                    return response;
                }
                catch (Exception ex)
                {
                    AddOutput($"Request failed: {ex.Message}", Color.DarkRed);
                }
            }

            return null;
        }


        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            Authenticate();
        }

        void InitJobList()
        {
            BindingList<Job> activeJobs = new BindingList<Job>();

            RestRequest request = new RestRequest("JobList");
            request.AddParameter("userId", _userId.ToString());

            RestResponse response = _client.Execute(request);

            List<Guid> jobIdList = JsonConvert.DeserializeObject<List<Guid>>(response.Content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = commandTypesBinder
            });

            foreach (Guid jobId in jobIdList)
            {
                request = new RestRequest("JobStatus");
                request.AddParameter("userId", _userId.ToString());
                request.AddParameter("jobId", jobId.ToString());

                response = _client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    DocProcessStatus jobStatus = JsonConvert.DeserializeObject<DocProcessStatus>(response.Content, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        SerializationBinder = commandTypesBinder
                    });

                    if (jobStatus != null)
                    {
                        TimeSpan elapsedTime = TimeSpan.Zero;

                        if (_jobElapsedTime.ContainsKey(jobId))
                        {
                            elapsedTime = _jobElapsedTime[jobId];
                        }

                        activeJobs.Add(new Job
                        {
                            Id = jobId,
                            Name = jobStatus.Title,
                            ResultSaved = jobStatus.Status == DocProcessStatus.StatusType.Delivered,
                            Status = jobStatus.Status.ToString(),
                            ElapsedTime = elapsedTime
                        });
                    }
                }
                else
                {
                    AddOutput($"An error occurred: {response.ErrorMessage}", Color.DarkRed);
                }
            }
            _activeJobs = activeJobs;
            blvJobs.DataSource = _activeJobs;
            blvJobs.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            blvJobs.Columns[0].Width = 200;
            blvJobs.Columns[1].Width = 200;
            blvJobs.Columns[2].Width = 80;
            blvJobs.Columns[3].Width = 120;
            blvJobs.Update();
        }

        private void Authenticate()
        {
            if (cmbEndpointBaseUrl.SelectedIndex < 0 || cmbEndpointBaseUrl.SelectedIndex >= _servers.Count)
                return;

            if (!_servers[cmbEndpointBaseUrl.SelectedIndex].Authenticate)
                return;

            if (_servers[cmbEndpointBaseUrl.SelectedIndex].APIv2)
            {
                try
                {
                    _client = new RestClient(_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl);
                    RestRequest request = new RestRequest();

                    try
                    {
                        RestResponse response = _client.Execute(request);
                        if (!string.IsNullOrEmpty(response.Content))
                        {
                            if (response != null && response.ContentType == ContentType.Json)
                            {
                                string json = (response.Content);

                                var writerOptions = new JsonWriterOptions
                                {
                                    Indented = true
                                };

                                try
                                {
                                    using (var jsonDoc = JsonDocument.Parse(json))
                                    {
                                        using (var ms = new MemoryStream())
                                        {
                                            using (var writer = new Utf8JsonWriter(ms, writerOptions))
                                            {
                                                jsonDoc.WriteTo(writer);
                                                writer.Flush();
                                                txtJsonResponse.Text = Encoding.UTF8.GetString(ms.ToArray());
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AddOutput($"An error occurred: {ex.Message}", Color.DarkRed);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AddOutput($"An error occurred: {ex.Message}", Color.DarkRed);
                    }

                    request = new RestRequest("ValidateUser");
                    request.AddParameter("username", "admin");
                    request.AddParameter("password", "admin");

                    try
                    {
                        RestResponse response = _client.Execute(request);

                        if (string.IsNullOrEmpty(response.Content))
                        {
                            AddOutput($"ValidateUser failed on {_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl}: Status Code = {response.StatusCode}", Color.DarkRed);
                        }
                        else
                        {
                            using (var jsonDoc = JsonDocument.Parse(response.Content))
                            {
                                JsonElement root = jsonDoc.RootElement;
                                JsonElement e = root.GetProperty("userId");
                                _userId = Guid.Parse(e.GetString());

                                InitJobList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AddOutput($"A problem occurred: {ex.Message}", Color.DarkRed);
                    }
                }
                catch (Exception ex)
                {
                    AddOutput($"Authenticating with {_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl} failed: {ex.Message}", Color.DarkRed);
                    _client = null;
                }
            }
            else
            {
                lblStatus.Text = "Authenticating, please wait...";

                SetBusy(true);

                try
                {
                    _client = new RestClient(_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl);
                    var request = new RestRequest("Authenticate", Method.Get);
                    string credentialsB64 = $"{txtUsername.Text}+{txtSerialNo.Text}:{txtPassword.Text}".ToBase64();
                    request.AddHeader("Ocp-Apim-Subscription-Key", txtApimSubscriptionKey.Text);
                    request.AddHeader("Authorization", $"Basic {credentialsB64}");

                    try
                    {
                        var response = _client.Execute(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == ContentType.Json)
                        {
                            txtAuthToken.Text = response.Content;
                            lblStatus.Text = "Authenticated:-)";
                            AddOutput($"Authenticated on '{_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl}' :-)", Color.DarkSeaGreen);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            lblStatus.Text = "NOT Authenticated:-(";
                            AddOutput($"NOT Authenticated on '{_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl}' :-(", Color.DarkRed);
                        }
                        else
                        {
                            lblStatus.Text = "There was a problem authenticating:-(";
                            AddOutput($"There was a problem authenticating on '{_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl}' :-( Status Code: {response.StatusCode} {response.ErrorMessage}", Color.DarkRed);
                        }
                    }
                    catch
                    {
                    }
                }
                catch (Exception ex)
                {
                    AddOutput($"Authenticating with {_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl} failed: {ex.Message}", Color.DarkRed);
                    _client = null;
                }
            }

            SetBusy(false);
        }

        void SetBusy(bool busy)
        {
            Cursor.Current = busy ? Cursors.WaitCursor : Cursors.Default;
            busyIndicator.Invoke((MethodInvoker)(() => {
                busyIndicator.Visible = busy;
                busyIndicator.Active = busy;
            }));
            btnSendRequest.Invoke((MethodInvoker)(() => {
                btnSendRequest.Enabled = !busy;
            }));
        }

        public void AddOutput(string text, bool newline = true)
        {
            rtxtOutput.Invoke((MethodInvoker)(() => {
                rtxtOutput.AppendText(text + (newline ? Environment.NewLine : ""));
                rtxtOutput.SelectionStart = rtxtOutput.Text.Length;
                rtxtOutput.ScrollToCaret();
            }));
        }

        public void AddOutput(string text, Color color, bool newline = true)
        {
            rtxtOutput.Invoke((MethodInvoker)(() => {
                rtxtOutput.AppendText(text + (newline ? Environment.NewLine : ""), color);
                rtxtOutput.SelectionStart = rtxtOutput.Text.Length;
                rtxtOutput.ScrollToCaret();
            }));
        }

        private void cmbEndpointBaseUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEndpointBaseUrl.SelectedIndex > 0)
            {
                chkAuthenticate.Checked = _servers[cmbEndpointBaseUrl.SelectedIndex].Authenticate;
                chkAPIv2.Checked = _servers[cmbEndpointBaseUrl.SelectedIndex].APIv2;
            }

            if (chkAuthenticate.Checked)
                Authenticate();
        }

        private void btnSaveRequest_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbRequest.Text) && _selectedRequestIndex >= 0)
            {
                List<string> files = new List<string>();
                foreach (string file in lstBodyFiles.Items)
                {
                    if (!string.IsNullOrEmpty(file))
                        files.Add(file);
                }

                _requests[_selectedRequestIndex] = new Request
                {
                    Name = cmbRequest.Text,
                    Type = rbtnPost.Checked ? "POST" : "GET",
                    Endpoint = txtEndpoint.Text,
                    Parameters = txtParameters.Text,
                    Files = new BindingList<string>(files)
                };
            }
        }

        private void btnRemoveRequest_Click(object sender, EventArgs e)
        {
            if (_selectedRequestIndex != -1)
            {
                _requests.RemoveAt(_selectedRequestIndex);
            }
        }

        private void cmbRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedRequestIndex = cmbRequest.SelectedIndex;

            if (cmbRequest.Items.Count > 0)
            {
                Request r = _requests[_selectedRequestIndex];
                txtEndpoint.Text = r.Endpoint;
                txtParameters.Text = r.Parameters;
                lstBodyFiles.DataSource = _requests[_selectedRequestIndex].Files;
                rbtnGet.Checked = r.Type == "GET";
                rbtnPost.Checked = r.Type == "POST";
            }
        }

        private void btnNewRequest_Click(object sender, EventArgs e)
        {
            Request r = new Request
            {
                Name = "New Request",
                Type = "POST",
                Endpoint = "Convert",
                Parameters = "?Format=pdfa2b&OCR=false",
                Files = new BindingList<string>()
            };
            _requests.Add(r);
            _selectedRequestIndex = _requests.Count - 1;
            cmbRequest.SelectedIndex = _selectedRequestIndex;
            cmbRequest.SelectAll();
            cmbRequest.Focus();
        }

        private void btnAddBodyFiles_Click(object sender, EventArgs e)
        {
            if (_selectedRequestIndex >= 0)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filename in dlg.FileNames)
                        _requests[_selectedRequestIndex].Files.Add(filename);
                }
            }
        }

        private void btnRemoveBodyFile_Click(object sender, EventArgs e)
        {
            if (lstBodyFiles.SelectedIndex >= 0 && _selectedRequestIndex >= 0)
            {
                _requests[_selectedRequestIndex].Files.RemoveAt(lstBodyFiles.SelectedIndex);
                lstBodyFiles.Update();
            }
        }

        private void btnAddOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!_outputFolders.Contains(dlg.SelectedPath))
                    _outputFolders.Add(dlg.SelectedPath);
            }
        }

        private void btnRemoveOutputFolder_Click(object sender, EventArgs e)
        {
            if (cmbOutputFolder.SelectedIndex >= 0)
                cmbOutputFolder.Items.RemoveAt(cmbOutputFolder.SelectedIndex);
        }

        private void btnClearOutputFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(cmbOutputFolder.SelectedItem as string))
            {
                DirectoryInfo di = new DirectoryInfo(cmbOutputFolder.Text);
                try
                {
                    di.Empty();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was a problem emptying this folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddBaseUrl_Click(object sender, EventArgs e)
        {
            _servers.Add(new Server
            {
                BaseUrl = cmbEndpointBaseUrl.Text,
                Authenticate = chkAuthenticate.Checked,
                APIv2 = chkAPIv2.Checked
            });

            if (chkAPIv2.Checked)
                Authenticate();
        }

        private void btnRemoveBaseUrl_Click(object sender, EventArgs e)
        {
            if (cmbEndpointBaseUrl.SelectedIndex >= 0)
                cmbEndpointBaseUrl.Items.RemoveAt(cmbEndpointBaseUrl.SelectedIndex);
        }

        private void chkAuthenticate_CheckedChanged(object sender, EventArgs e)
        {
            if (cmbEndpointBaseUrl.SelectedIndex >= 0)
                _servers[cmbEndpointBaseUrl.SelectedIndex].Authenticate = chkAuthenticate.Checked;
        }

        private bool _running = false;
        private bool _abortRun = false;

        private void btnAbortRun_Click(object sender, EventArgs e)
        {
            if (_running && !_abortRun)
                _abortRun = true;
        }

        private void chkAPIv2_CheckedChanged(object sender, EventArgs e)
        {
            if (cmbEndpointBaseUrl.SelectedIndex >= 0)
                _servers[cmbEndpointBaseUrl.SelectedIndex].APIv2 = chkAPIv2.Checked;

            if (chkAPIv2.Checked)
            {
                Authenticate();
            }
        }

        private void btnOpenOutputFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbOutputFolder.Text))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = cmbOutputFolder.Text,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }

        private void btnPurgeAllJobs_Click(object sender, EventArgs e)
        {
            if (_userId != Guid.Empty && _client != null)
            {
                RestRequest request = new RestRequest("PurgeAllJobs", Method.Delete);
                request.AddParameter("userId", _userId.ToString());

                try
                {
                    RestResponse response = _client.Execute(request);
                    AddOutput(response.Content);

                    InitJobList();
                }
                catch (Exception ex)
                {
                    AddOutput($"A problem occurred: {ex.Message}", Color.DarkRed);
                }

            }
        }
    }

    public class Job : INotifyPropertyChanged
    {
        public bool ResultSaved = false;

        private Guid id;

        public Guid Id
        {
            get
            {
                return id;
            }

            set 
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged("Id");
                }

            }
        }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string status;

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                if (value != status)
                {
                    status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        private TimeSpan elapsedTime;

        public TimeSpan ElapsedTime
        {
            get
            {
                return elapsedTime;
            }

            set
            {
                if (value != elapsedTime)
                {
                    elapsedTime = value;
                    NotifyPropertyChanged("ElapsedTime");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public class Request
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Endpoint { get; set; }
        public string Parameters { get; set; }
        public BindingList<string> Files { get; set; }
    }

    public static class Base64Encoding
    {
        public static string ToBase64(this string text)
        {
            if (text == null)
                return null;

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }
    }

    static class Extensions
    { 
        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) 
                file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) 
                subDirectory.Delete(true);
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }

    public class DocProcessStatus
    {
        public enum StatusType { Created, InQueue, Processing, Succeeded, Failed, Delivered, Purged };
        public enum ArchiveStatusType { NotArchived, Archiving, Archived, ArchivingFailed };

        public class Message
        {
            public enum MessageType { Info, Process, Error };
            public MessageType Type;
            public string Text;
        }

        public Guid UserID;
        public Guid JobID;
        public string Title;
        public List<string> InputFilenames;
        public List<string> OutputFilenames;
        public StatusType Status;
        public ArchiveStatusType ArchiveStatus;
        public bool IsPurged;
        public DateTime StatusTime;
        public int Retry;
        public int MaxRetries;
        public int CurrentProgress;
        public int TotalProgress;
        public int CurrentDocumentNumber;
        public int TotalDocumentNumber;

        public List<Message> Messages;
        public string InputFilename;
        public Message LastMessage;
        public string LastMessageString;
        public bool JobHasEnded;
    }

    public class CommandTypesBinder : ISerializationBinder
    {
        public IList<Type> CommandTypes { get; set; }

        public Type BindToType(string assemblyName, string typeName)
        {
            return CommandTypes.SingleOrDefault(t => t.Name == typeName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
    }
}
