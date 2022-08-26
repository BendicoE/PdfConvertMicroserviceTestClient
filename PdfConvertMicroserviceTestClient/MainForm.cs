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

namespace PdfConvertMicroserviceTestClient
{
    public partial class MainForm : Form
    {
        BindingList<Request> _requests = new BindingList<Request>();
        BindingList<string> _outputFolders = new BindingList<string>();
        BackgroundWorker _requestWorker = null;
        BindingList<Server> _servers = new BindingList<Server>();
        int _selectedRequestIndex = -1;

        public class Server
        {
            public string BaseUrl { get; set; }
            public bool Authenticate { get; set; }
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
                if (server_.Length > 1)
                {
                    _servers.Add(new Server
                    {
                        BaseUrl = server_[0],
                        Authenticate = bool.Parse(server_[1])
                    });
                }
                else
                {
                    _servers.Add(new Server
                    {
                        BaseUrl = server_[0],
                        Authenticate = true
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
                cmbEndpointBaseUrl.SelectedIndex = Properties.Settings.Default.EndpointBaseUrlSelectedIndex;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string endpointBaseUrls = string.Empty;
            foreach (Server server in cmbEndpointBaseUrl.Items)
            {
                endpointBaseUrls += $"{server.BaseUrl}#{server.Authenticate};";
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
            Cursor.Current = Cursors.WaitCursor;
            busyIndicator.Visible = true;
            busyIndicator.Active = true;
            btnSendRequest.Enabled = false;

            if (_selectedRequestIndex >= 0)
            {
                RequestArg request = new RequestArg
                {
                    Iteration = 0,
                    Name = cmbRequest.Text,
                    Method = rbtnPost.Checked ? Method.Post : Method.Get,
                    BaseUrl = cmbEndpointBaseUrl.Text,
                    Endpoint = txtEndpoint.Text,
                    Parameters = txtParameters.Text,
                    Files = _requests[_selectedRequestIndex].Files.ToList(),
                    ApimSubscriptionKey = chkAuthenticate.Checked ? txtApimSubscriptionKey.Text : String.Empty,
                    AuthToken = chkAuthenticate.Checked ? txtAuthToken.Text : string.Empty,
                    OutputFolder = cmbOutputFolder.Text
                };

                List<RequestArg> requests = new List<RequestArg>();
                requests.Add(request);

                _requestWorker.RunWorkerAsync(requests);
            }
        }

        private void btnRunCollection_Click(object sender, EventArgs e)
        {
            List<RequestArg> requests = new List<RequestArg>();

            for (int i = 0; i < numRunRepeats.Value; i++)
            {
                foreach (Request r in _requests)
                {
                    RequestArg request = new RequestArg
                    {
                        Iteration = i,
                        Name = r.Name,
                        Method = r.Type == "POST" ? Method.Post : Method.Get,
                        BaseUrl = cmbEndpointBaseUrl.Text,
                        Endpoint = r.Endpoint,
                        Parameters = r.Parameters,
                        Files = r.Files.ToList(),
                        ApimSubscriptionKey = chkAuthenticate.Checked ? txtApimSubscriptionKey.Text : String.Empty,
                        AuthToken = chkAuthenticate.Checked ? txtAuthToken.Text : string.Empty,
                        OutputFolder = cmbOutputFolder.Text
                    };

                    requests.Add(request);
                }
            }

            _running = true;

            _requestWorker.RunWorkerAsync(requests);
        }

        public struct RequestArg
        {
            public int Iteration;
            public string Name;
            public Method Method;
            public string BaseUrl;
            public string Endpoint;
            public string Parameters;
            public List<string> Files;
            public string ApimSubscriptionKey;
            public string AuthToken;
            public string OutputFolder;
        }

        private void _requestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<RequestArg> requests = e.Argument as List<RequestArg>;

            if (requests.Count == 1 && string.IsNullOrEmpty(requests[0].OutputFolder))
            {
                RestResponse response = SendRequest(requests[0].Name, requests[0].Method, requests[0].BaseUrl, requests[0].Endpoint, requests[0].Parameters, requests[0].Files, requests[0].ApimSubscriptionKey, requests[0].AuthToken, requests[0].OutputFolder, 0);
                e.Result = response;
            }
            else
            {
                foreach (RequestArg request in requests)
                {
                    SendRequest(request.Name, request.Method, request.BaseUrl, request.Endpoint, request.Parameters, request.Files, request.ApimSubscriptionKey, request.AuthToken, request.OutputFolder, request.Iteration);
                    if (_abortRun)
                    {
                        AddOutput("***Run aborted***", Color.DarkOrange);
                        break;
                    }
                }
                e.Result = null;
            }
        }
        
        private void _requestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _running = false;
            _abortRun = false;

            Cursor.Current = Cursors.Default;
            busyIndicator.Visible = false;
            busyIndicator.Active = false;
            btnSendRequest.Enabled = true;

            if (e.Result != null)
            {
                RestResponse response = e.Result as RestResponse;

                if (response.ContentType == "application/pdf")
                {
                    var contentDispositionHeaderValue = response.ContentHeaders.FirstOrDefault(x => x.Name == "Content-Disposition")?.Value;
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
                            File.WriteAllBytes(dlg.FileName, response.RawBytes);
                        }
                    }
                }
                else if (response.ContentType == ContentType.Json)
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
                    catch { }
                }
            }
        }

        private RestResponse SendRequest(string name, Method method, string baseUrl, string endpoint, string parameters, List<string> files, string apimSubscriptionKey, string authToken, string outputFolder, int iteration)
        {
            var client = new RestClient(baseUrl);
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

                    response = client.Execute(request);

                    TimeSpan timeSpent = DateTime.Now - startTime;

                    AddOutput($"Request finished in {timeSpent.Minutes}m {timeSpent.Seconds}.{timeSpent.Milliseconds}s", Color.DarkOliveGreen);
                }
                catch (Exception ex)
                {
                    ex_ = ex;
                }

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
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
                            while (File.Exists(saveFilename))
                            {
                                saveFilename = Path.Combine(outputFolder, $"{Path.GetFileNameWithoutExtension(fileName)} ({i}){Path.GetExtension(fileName)}");
                                i++;
                            }
                            File.WriteAllBytes(saveFilename, response.RawBytes);
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
                    RestResponse response = client.Execute(request);

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

        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            Authenticate();
        }

        private void Authenticate()
        {
            if (cmbEndpointBaseUrl.SelectedIndex < 0 || cmbEndpointBaseUrl.SelectedIndex >= _servers.Count)
                return;

            if (!_servers[cmbEndpointBaseUrl.SelectedIndex].Authenticate)
                return;

            lblStatus.Text = "Authenticating, please wait...";
            Cursor.Current = Cursors.WaitCursor;
            busyIndicator.Visible = true;
            busyIndicator.Active = true;

            var client = new RestClient(_servers[cmbEndpointBaseUrl.SelectedIndex].BaseUrl);
            var request = new RestRequest("Authenticate", Method.Get);
            string credentialsB64 = $"{txtUsername.Text}+{txtSerialNo.Text}:{txtPassword.Text}".ToBase64();
            request.AddHeader("Ocp-Apim-Subscription-Key", txtApimSubscriptionKey.Text);
            request.AddHeader("Authorization", $"Basic {credentialsB64}");

            try
            {
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == ContentType.Json)
                {
                    txtAuthToken.Text = response.Content;
                    lblStatus.Text = "Authenticated:-)";
                    AddOutput("Authenticated:-)");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    lblStatus.Text = "NOT Authenticated:-(";
                    AddOutput("NOT Authenticated:-(");
                }
                else
                {
                    lblStatus.Text = "There was a problem authenticating:-(";
                    AddOutput($"There was a problem authenticating:-( Status Code: {response.StatusCode} {response.ErrorMessage}");
                }
            }
            catch
            {
            }

            Cursor.Current = Cursors.Default;
            busyIndicator.Visible = false;
            busyIndicator.Active = false;
        }

        public void AddOutput(string text, bool newline = true)
        {
            rtxtOutput.Invoke((MethodInvoker)(() => rtxtOutput.AppendText(text + (newline ? Environment.NewLine : ""))));
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
                chkAuthenticate.Checked = _servers[cmbEndpointBaseUrl.SelectedIndex].Authenticate;
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
                _requests[_selectedRequestIndex].Files.RemoveAt(lstBodyFiles.SelectedIndex);
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
            cmbEndpointBaseUrl.Items.Add(cmbEndpointBaseUrl.Text);
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
}
