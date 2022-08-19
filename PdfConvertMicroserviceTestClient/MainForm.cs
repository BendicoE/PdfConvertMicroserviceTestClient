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

namespace PdfConvertMicroserviceTestClient
{
    public partial class MainForm : Form
    {
        BindingList<string> uploadFiles = new BindingList<string>();
        bool authenticated = false;

        public MainForm()
        {
            InitializeComponent();
            lstBodyFiles.DataSource = uploadFiles;
        }

        private void btnBrowseBodyFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in dlg.FileNames)
                    uploadFiles.Add(filename);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string[] endpointBaseUrls = Properties.Settings.Default.EndpointBaseUrl.Split(new char[] { ';' });

            txtEndpoint.Text = Properties.Settings.Default.Endpoint;
            txtParameters.Text = Properties.Settings.Default.Parameters;
            txtUsername.Text = Properties.Settings.Default.Username;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtSerialNo.Text = Properties.Settings.Default.SerialNo;
            txtApimSubscriptionKey.Text = Properties.Settings.Default.ApimSubscriptionKey;
            txtAuthToken.Text = Properties.Settings.Default.AuthToken;

            foreach (string endpointBaseUrl in endpointBaseUrls)
                if (!string.IsNullOrEmpty(endpointBaseUrl))
                    cmbEndpointBaseUrl.Items.Add(endpointBaseUrl);

            if (Properties.Settings.Default.EndpointBaseUrlSelectedIndex < cmbEndpointBaseUrl.Items.Count)
                cmbEndpointBaseUrl.SelectedIndex = Properties.Settings.Default.EndpointBaseUrlSelectedIndex;
            else
                cmbEndpointBaseUrl.SelectedIndex = 0;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string endpointBaseUrls = string.Empty;
            foreach (string endpointBaseUrl in cmbEndpointBaseUrl.Items)
            {
                endpointBaseUrls += endpointBaseUrl + ';';
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

            Properties.Settings.Default.Save();
        }

        private void btnPostRequest_Click(object sender, EventArgs e)
        {
            var client = new RestClient(cmbEndpointBaseUrl.Text);
            var request = new RestRequest(txtEndpoint.Text, Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Ocp-Apim-Subscription-Key", txtApimSubscriptionKey.Text);
            request.AddHeader("Authorization", $"Bearer {txtAuthToken.Text}");

            RestResponse response = null;
            Exception ex_ = null;

            try
            {
                if (!string.IsNullOrEmpty(txtParameters.Text) && txtParameters.Text.Contains('='))
                {
                    foreach (string param in txtParameters.Text.Split(new char[] { '?', '&' }))
                    {
                        if (string.IsNullOrWhiteSpace(param))
                            continue;

                        string[] keyValue = param.Split('=');

                        if (keyValue.Length != 2)
                            continue;

                        request.AddQueryParameter(keyValue[0], keyValue[1]);
                    }
                }

                foreach (string filename in uploadFiles)
                {
                    request.AddFile(Path.GetFileName(filename), filename);
                }

                lblStatus.Text = "Sending request, please wait...";
                Cursor.Current = Cursors.WaitCursor;
                busyIndicator.Visible = true;
                busyIndicator.Active = true;

                DateTime startTime = DateTime.Now;

                response = client.Execute(request);

                TimeSpan timeSpent = DateTime.Now - startTime;

                lblStatus.Text = $"Request finished in {timeSpent.Minutes}m {timeSpent.Seconds}.{timeSpent.Milliseconds}s";
                busyIndicator.Active = false;
                busyIndicator.Visible = false;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ex_ = ex;
            }

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
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

                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.FileName = fileName;
                    dlg.OverwritePrompt = true;

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(dlg.FileName, response.RawBytes);
                    }
                }
            }
            else if (response != null && response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                lblStatus.Text = "Internal Server Error";
            }
            else if (response != null && response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
            {
                lblStatus.Text = "Gateway Timeout";
            }
            else if (response != null && response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
            {
                lblStatus.Text = "Request Timeout";
            }
            else
            {
                if (ex_ == null)
                    lblStatus.Text = "Unknown Error";
                else
                    lblStatus.Text = $"An Exception occurred: {ex_.Message}";
            }

        }

        private void btnGetRequest_Click(object sender, EventArgs e)
        {
            var client = new RestClient(cmbEndpointBaseUrl.Text);
            var request = new RestRequest(txtEndpoint.Text, Method.Get);
            var response = client.Execute(request);

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
                                txtJsonResponse.Text = Encoding.UTF8.GetString(ms.ToArray());
                            }
                        }
                    }
                }
                catch { }
            }
        }

        private void UpdateEndpointBaseUrl()
        {
            if (!cmbEndpointBaseUrl.Items.Contains(cmbEndpointBaseUrl.Text))
            {
                cmbEndpointBaseUrl.Items.Add(cmbEndpointBaseUrl.Text);
                cmbEndpointBaseUrl.SelectedIndex = cmbEndpointBaseUrl.Items.Count - 1;
            }
        }

        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            UpdateEndpointBaseUrl();
            Authenticate();
        }

        private void Authenticate()
        {
            authenticated = false;
            EnableButtons();

            lblStatus.Text = "Authenticating, please wait...";
            Cursor.Current = Cursors.WaitCursor;
            busyIndicator.Visible = true;
            busyIndicator.Active = true;

            var client = new RestClient(cmbEndpointBaseUrl.Text);
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
                    authenticated = true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    lblStatus.Text = "NOT Authenticated:-(";
                    authenticated = false;
                    MessageBox.Show("Invalid credentials", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
            }

            Cursor.Current = Cursors.Default;
            busyIndicator.Visible = false;
            busyIndicator.Active = false;

            EnableButtons();
        }

        private void cmbEndpointBaseUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbEndpointBaseUrl.Text = cmbEndpointBaseUrl.SelectedItem.ToString();
            Authenticate();
        }

        private void EnableButtons()
        {
            btnGetRequest.Enabled = authenticated;
            btnPostRequest.Enabled = authenticated;
        }

        private void btnClearBodyFiles_Click(object sender, EventArgs e)
        {
            uploadFiles.Clear();
        }
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
}
