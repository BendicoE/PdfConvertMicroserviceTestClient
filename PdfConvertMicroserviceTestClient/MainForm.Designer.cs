namespace PdfConvertMicroserviceTestClient
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndpoint = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtParameters = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lstBodyFiles = new System.Windows.Forms.ListBox();
            this.btnBrowseBodyFiles = new System.Windows.Forms.Button();
            this.btnPostRequest = new System.Windows.Forms.Button();
            this.btnGetRequest = new System.Windows.Forms.Button();
            this.txtJsonResponse = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.btnAuthenticate = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAuthToken = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtApimSubscriptionKey = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyIndicator = new MRG.Controls.UI.LoadingCircle();
            this.cmbEndpointBaseUrl = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Endpoint Base URL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Endpoint";
            // 
            // txtEndpoint
            // 
            this.txtEndpoint.Location = new System.Drawing.Point(12, 92);
            this.txtEndpoint.Name = "txtEndpoint";
            this.txtEndpoint.Size = new System.Drawing.Size(147, 26);
            this.txtEndpoint.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Parameters";
            // 
            // txtParameters
            // 
            this.txtParameters.Location = new System.Drawing.Point(175, 92);
            this.txtParameters.Name = "txtParameters";
            this.txtParameters.Size = new System.Drawing.Size(300, 26);
            this.txtParameters.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 228);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "Body (files to upload)";
            // 
            // lstBodyFiles
            // 
            this.lstBodyFiles.FormattingEnabled = true;
            this.lstBodyFiles.ItemHeight = 20;
            this.lstBodyFiles.Location = new System.Drawing.Point(12, 252);
            this.lstBodyFiles.Name = "lstBodyFiles";
            this.lstBodyFiles.Size = new System.Drawing.Size(464, 324);
            this.lstBodyFiles.TabIndex = 21;
            // 
            // btnBrowseBodyFiles
            // 
            this.btnBrowseBodyFiles.Location = new System.Drawing.Point(12, 582);
            this.btnBrowseBodyFiles.Name = "btnBrowseBodyFiles";
            this.btnBrowseBodyFiles.Size = new System.Drawing.Size(110, 37);
            this.btnBrowseBodyFiles.TabIndex = 14;
            this.btnBrowseBodyFiles.Text = "Browse...";
            this.btnBrowseBodyFiles.UseVisualStyleBackColor = true;
            this.btnBrowseBodyFiles.Click += new System.EventHandler(this.btnBrowseBodyFiles_Click);
            // 
            // btnPostRequest
            // 
            this.btnPostRequest.Location = new System.Drawing.Point(142, 625);
            this.btnPostRequest.Name = "btnPostRequest";
            this.btnPostRequest.Size = new System.Drawing.Size(124, 51);
            this.btnPostRequest.TabIndex = 15;
            this.btnPostRequest.Text = "POST";
            this.btnPostRequest.UseVisualStyleBackColor = true;
            this.btnPostRequest.Click += new System.EventHandler(this.btnPostRequest_Click);
            // 
            // btnGetRequest
            // 
            this.btnGetRequest.Location = new System.Drawing.Point(12, 625);
            this.btnGetRequest.Name = "btnGetRequest";
            this.btnGetRequest.Size = new System.Drawing.Size(124, 51);
            this.btnGetRequest.TabIndex = 16;
            this.btnGetRequest.Text = "GET";
            this.btnGetRequest.UseVisualStyleBackColor = true;
            this.btnGetRequest.Click += new System.EventHandler(this.btnGetRequest_Click);
            // 
            // txtJsonResponse
            // 
            this.txtJsonResponse.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJsonResponse.Location = new System.Drawing.Point(517, 251);
            this.txtJsonResponse.Multiline = true;
            this.txtJsonResponse.Name = "txtJsonResponse";
            this.txtJsonResponse.Size = new System.Drawing.Size(546, 325);
            this.txtJsonResponse.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(513, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 20);
            this.label5.TabIndex = 22;
            this.label5.Text = "Response";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(518, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(518, 36);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(170, 26);
            this.txtUsername.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(694, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(695, 36);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(169, 26);
            this.txtPassword.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(867, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 20);
            this.label8.TabIndex = 6;
            this.label8.Text = "Serial No";
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Location = new System.Drawing.Point(871, 36);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(193, 26);
            this.txtSerialNo.TabIndex = 18;
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(938, 82);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(124, 44);
            this.btnAuthenticate.TabIndex = 9;
            this.btnAuthenticate.Text = "Authenticate";
            this.btnAuthenticate.UseVisualStyleBackColor = true;
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(518, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 20);
            this.label9.TabIndex = 19;
            this.label9.Text = "Auth Token";
            // 
            // txtAuthToken
            // 
            this.txtAuthToken.Location = new System.Drawing.Point(517, 147);
            this.txtAuthToken.Multiline = true;
            this.txtAuthToken.Name = "txtAuthToken";
            this.txtAuthToken.Size = new System.Drawing.Size(546, 78);
            this.txtAuthToken.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(518, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(170, 20);
            this.label10.TabIndex = 7;
            this.label10.Text = "APIM Subscription Key";
            // 
            // txtApimSubscriptionKey
            // 
            this.txtApimSubscriptionKey.Location = new System.Drawing.Point(518, 91);
            this.txtApimSubscriptionKey.Name = "txtApimSubscriptionKey";
            this.txtApimSubscriptionKey.Size = new System.Drawing.Size(346, 26);
            this.txtApimSubscriptionKey.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(441, 640);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 20);
            this.lblStatus.TabIndex = 25;
            // 
            // busyIndicator
            // 
            this.busyIndicator.Active = false;
            this.busyIndicator.Color = System.Drawing.Color.DarkGray;
            this.busyIndicator.InnerCircleRadius = 5;
            this.busyIndicator.Location = new System.Drawing.Point(272, 625);
            this.busyIndicator.Name = "busyIndicator";
            this.busyIndicator.NumberSpoke = 12;
            this.busyIndicator.OuterCircleRadius = 11;
            this.busyIndicator.RotationSpeed = 100D;
            this.busyIndicator.Size = new System.Drawing.Size(83, 51);
            this.busyIndicator.SpokeThickness = 2;
            this.busyIndicator.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.busyIndicator.TabIndex = 26;
            this.busyIndicator.Text = "Busy...";
            this.busyIndicator.Visible = false;
            // 
            // cmbEndpointBaseUrl
            // 
            this.cmbEndpointBaseUrl.FormattingEnabled = true;
            this.cmbEndpointBaseUrl.Location = new System.Drawing.Point(12, 36);
            this.cmbEndpointBaseUrl.Name = "cmbEndpointBaseUrl";
            this.cmbEndpointBaseUrl.Size = new System.Drawing.Size(464, 28);
            this.cmbEndpointBaseUrl.TabIndex = 2;
            this.cmbEndpointBaseUrl.Text = "https://pdfconvertmicroservice2.azurewebsites.net/convertpdf";
            this.cmbEndpointBaseUrl.SelectedIndexChanged += new System.EventHandler(this.cmbEndpointBaseUrl_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 683);
            this.Controls.Add(this.cmbEndpointBaseUrl);
            this.Controls.Add(this.busyIndicator);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtApimSubscriptionKey);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAuthToken);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSerialNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtJsonResponse);
            this.Controls.Add(this.btnGetRequest);
            this.Controls.Add(this.btnAuthenticate);
            this.Controls.Add(this.btnPostRequest);
            this.Controls.Add(this.btnBrowseBodyFiles);
            this.Controls.Add(this.lstBodyFiles);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtParameters);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEndpoint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "PixEdit Cloud API Test Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEndpoint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtParameters;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lstBodyFiles;
        private System.Windows.Forms.Button btnBrowseBodyFiles;
        private System.Windows.Forms.Button btnPostRequest;
        private System.Windows.Forms.Button btnGetRequest;
        private System.Windows.Forms.TextBox txtJsonResponse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Button btnAuthenticate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAuthToken;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtApimSubscriptionKey;
        private System.Windows.Forms.Label lblStatus;
        private MRG.Controls.UI.LoadingCircle busyIndicator;
        private System.Windows.Forms.ComboBox cmbEndpointBaseUrl;
    }
}

