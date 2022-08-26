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
            this.btnAddBodyFiles = new System.Windows.Forms.Button();
            this.btnSendRequest = new System.Windows.Forms.Button();
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
            this.busyIndicator = new MRG.Controls.UI.LoadingCircle();
            this.cmbEndpointBaseUrl = new System.Windows.Forms.ComboBox();
            this.rbtnGet = new System.Windows.Forms.RadioButton();
            this.rbtnPost = new System.Windows.Forms.RadioButton();
            this.grpRequestType = new System.Windows.Forms.GroupBox();
            this.cmbRequest = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSaveRequest = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRemoveRequest = new System.Windows.Forms.Button();
            this.btnNewRequest = new System.Windows.Forms.Button();
            this.btnRemoveBodyFile = new System.Windows.Forms.Button();
            this.cmbOutputFolder = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnAddOutputFolder = new System.Windows.Forms.Button();
            this.btnClearOutputFolder = new System.Windows.Forms.Button();
            this.btnRemoveOutputFolder = new System.Windows.Forms.Button();
            this.rtxtOutput = new System.Windows.Forms.RichTextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnRunCollection = new System.Windows.Forms.Button();
            this.btnAddBaseUrl = new System.Windows.Forms.Button();
            this.btnRemoveBaseUrl = new System.Windows.Forms.Button();
            this.chkAuthenticate = new System.Windows.Forms.CheckBox();
            this.grpRequestType.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Endpoint Base URL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 148);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Endpoint";
            // 
            // txtEndpoint
            // 
            this.txtEndpoint.Location = new System.Drawing.Point(8, 163);
            this.txtEndpoint.Margin = new System.Windows.Forms.Padding(2);
            this.txtEndpoint.Name = "txtEndpoint";
            this.txtEndpoint.Size = new System.Drawing.Size(99, 20);
            this.txtEndpoint.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(114, 148);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Parameters";
            // 
            // txtParameters
            // 
            this.txtParameters.Location = new System.Drawing.Point(117, 163);
            this.txtParameters.Margin = new System.Windows.Forms.Padding(2);
            this.txtParameters.Name = "txtParameters";
            this.txtParameters.Size = new System.Drawing.Size(201, 20);
            this.txtParameters.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 198);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Body (files to upload)";
            // 
            // lstBodyFiles
            // 
            this.lstBodyFiles.FormattingEnabled = true;
            this.lstBodyFiles.Location = new System.Drawing.Point(8, 214);
            this.lstBodyFiles.Margin = new System.Windows.Forms.Padding(2);
            this.lstBodyFiles.Name = "lstBodyFiles";
            this.lstBodyFiles.Size = new System.Drawing.Size(311, 303);
            this.lstBodyFiles.TabIndex = 21;
            // 
            // btnAddBodyFiles
            // 
            this.btnAddBodyFiles.Location = new System.Drawing.Point(11, 530);
            this.btnAddBodyFiles.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddBodyFiles.Name = "btnAddBodyFiles";
            this.btnAddBodyFiles.Size = new System.Drawing.Size(83, 24);
            this.btnAddBodyFiles.TabIndex = 14;
            this.btnAddBodyFiles.Text = "Add...";
            this.btnAddBodyFiles.UseVisualStyleBackColor = true;
            this.btnAddBodyFiles.Click += new System.EventHandler(this.btnAddBodyFiles_Click);
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Location = new System.Drawing.Point(148, 600);
            this.btnSendRequest.Margin = new System.Windows.Forms.Padding(2);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(83, 33);
            this.btnSendRequest.TabIndex = 15;
            this.btnSendRequest.Text = "SEND";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.btnSendRequest_Click);
            // 
            // txtJsonResponse
            // 
            this.txtJsonResponse.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJsonResponse.Location = new System.Drawing.Point(347, 253);
            this.txtJsonResponse.Margin = new System.Windows.Forms.Padding(2);
            this.txtJsonResponse.Multiline = true;
            this.txtJsonResponse.Name = "txtJsonResponse";
            this.txtJsonResponse.Size = new System.Drawing.Size(365, 304);
            this.txtJsonResponse.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(346, 238);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Response";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(351, 48);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(351, 63);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(115, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(469, 48);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(469, 63);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(114, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(584, 48);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Serial No";
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Location = new System.Drawing.Point(587, 63);
            this.txtSerialNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(130, 20);
            this.txtSerialNo.TabIndex = 18;
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(631, 99);
            this.btnAuthenticate.Margin = new System.Windows.Forms.Padding(2);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(83, 29);
            this.btnAuthenticate.TabIndex = 9;
            this.btnAuthenticate.Text = "Authenticate";
            this.btnAuthenticate.UseVisualStyleBackColor = true;
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(349, 126);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Auth Token";
            // 
            // txtAuthToken
            // 
            this.txtAuthToken.Location = new System.Drawing.Point(349, 141);
            this.txtAuthToken.Margin = new System.Windows.Forms.Padding(2);
            this.txtAuthToken.Multiline = true;
            this.txtAuthToken.Name = "txtAuthToken";
            this.txtAuthToken.Size = new System.Drawing.Size(365, 60);
            this.txtAuthToken.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(348, 89);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "APIM Subscription Key";
            // 
            // txtApimSubscriptionKey
            // 
            this.txtApimSubscriptionKey.Location = new System.Drawing.Point(351, 104);
            this.txtApimSubscriptionKey.Margin = new System.Windows.Forms.Padding(2);
            this.txtApimSubscriptionKey.Name = "txtApimSubscriptionKey";
            this.txtApimSubscriptionKey.Size = new System.Drawing.Size(232, 20);
            this.txtApimSubscriptionKey.TabIndex = 8;
            // 
            // busyIndicator
            // 
            this.busyIndicator.Active = false;
            this.busyIndicator.Color = System.Drawing.Color.DarkGray;
            this.busyIndicator.InnerCircleRadius = 5;
            this.busyIndicator.Location = new System.Drawing.Point(235, 600);
            this.busyIndicator.Margin = new System.Windows.Forms.Padding(2);
            this.busyIndicator.Name = "busyIndicator";
            this.busyIndicator.NumberSpoke = 12;
            this.busyIndicator.OuterCircleRadius = 11;
            this.busyIndicator.RotationSpeed = 100D;
            this.busyIndicator.Size = new System.Drawing.Size(55, 33);
            this.busyIndicator.SpokeThickness = 2;
            this.busyIndicator.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.busyIndicator.TabIndex = 26;
            this.busyIndicator.Text = "Busy...";
            this.busyIndicator.Visible = false;
            // 
            // cmbEndpointBaseUrl
            // 
            this.cmbEndpointBaseUrl.FormattingEnabled = true;
            this.cmbEndpointBaseUrl.Location = new System.Drawing.Point(8, 23);
            this.cmbEndpointBaseUrl.Margin = new System.Windows.Forms.Padding(2);
            this.cmbEndpointBaseUrl.Name = "cmbEndpointBaseUrl";
            this.cmbEndpointBaseUrl.Size = new System.Drawing.Size(409, 21);
            this.cmbEndpointBaseUrl.TabIndex = 2;
            this.cmbEndpointBaseUrl.Text = "https://pdfconvertmicroservice2.azurewebsites.net/convertpdf";
            this.cmbEndpointBaseUrl.SelectedIndexChanged += new System.EventHandler(this.cmbEndpointBaseUrl_SelectedIndexChanged);
            // 
            // rbtnGet
            // 
            this.rbtnGet.AutoSize = true;
            this.rbtnGet.Location = new System.Drawing.Point(7, 16);
            this.rbtnGet.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnGet.Name = "rbtnGet";
            this.rbtnGet.Size = new System.Drawing.Size(60, 26);
            this.rbtnGet.TabIndex = 27;
            this.rbtnGet.Text = "GET";
            this.rbtnGet.UseVisualStyleBackColor = true;
            // 
            // rbtnPost
            // 
            this.rbtnPost.AutoSize = true;
            this.rbtnPost.Checked = true;
            this.rbtnPost.Location = new System.Drawing.Point(7, 36);
            this.rbtnPost.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnPost.Name = "rbtnPost";
            this.rbtnPost.Size = new System.Drawing.Size(67, 26);
            this.rbtnPost.TabIndex = 28;
            this.rbtnPost.TabStop = true;
            this.rbtnPost.Text = "POST";
            this.rbtnPost.UseVisualStyleBackColor = true;
            // 
            // grpRequestType
            // 
            this.grpRequestType.Controls.Add(this.rbtnGet);
            this.grpRequestType.Controls.Add(this.rbtnPost);
            this.grpRequestType.Location = new System.Drawing.Point(11, 568);
            this.grpRequestType.Margin = new System.Windows.Forms.Padding(2);
            this.grpRequestType.Name = "grpRequestType";
            this.grpRequestType.Padding = new System.Windows.Forms.Padding(2);
            this.grpRequestType.Size = new System.Drawing.Size(133, 65);
            this.grpRequestType.TabIndex = 29;
            this.grpRequestType.TabStop = false;
            this.grpRequestType.Text = "Request Type";
            // 
            // cmbRequest
            // 
            this.cmbRequest.DisplayMember = "Name";
            this.cmbRequest.FormattingEnabled = true;
            this.cmbRequest.Location = new System.Drawing.Point(8, 64);
            this.cmbRequest.Margin = new System.Windows.Forms.Padding(2);
            this.cmbRequest.Name = "cmbRequest";
            this.cmbRequest.Size = new System.Drawing.Size(221, 21);
            this.cmbRequest.TabIndex = 30;
            this.cmbRequest.SelectedIndexChanged += new System.EventHandler(this.cmbRequest_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 49);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Request";
            // 
            // btnSaveRequest
            // 
            this.btnSaveRequest.Location = new System.Drawing.Point(232, 92);
            this.btnSaveRequest.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveRequest.Name = "btnSaveRequest";
            this.btnSaveRequest.Size = new System.Drawing.Size(83, 24);
            this.btnSaveRequest.TabIndex = 14;
            this.btnSaveRequest.Text = "Save";
            this.btnSaveRequest.UseVisualStyleBackColor = true;
            this.btnSaveRequest.Click += new System.EventHandler(this.btnSaveRequest_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(232, -208);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(214, 21);
            this.comboBox1.TabIndex = 30;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(229, -223);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Request";
            // 
            // btnRemoveRequest
            // 
            this.btnRemoveRequest.Location = new System.Drawing.Point(232, 120);
            this.btnRemoveRequest.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveRequest.Name = "btnRemoveRequest";
            this.btnRemoveRequest.Size = new System.Drawing.Size(83, 24);
            this.btnRemoveRequest.TabIndex = 14;
            this.btnRemoveRequest.Text = "Remove";
            this.btnRemoveRequest.UseVisualStyleBackColor = true;
            this.btnRemoveRequest.Click += new System.EventHandler(this.btnRemoveRequest_Click);
            // 
            // btnNewRequest
            // 
            this.btnNewRequest.Location = new System.Drawing.Point(232, 64);
            this.btnNewRequest.Margin = new System.Windows.Forms.Padding(2);
            this.btnNewRequest.Name = "btnNewRequest";
            this.btnNewRequest.Size = new System.Drawing.Size(83, 24);
            this.btnNewRequest.TabIndex = 32;
            this.btnNewRequest.Text = "New";
            this.btnNewRequest.UseVisualStyleBackColor = true;
            this.btnNewRequest.Click += new System.EventHandler(this.btnNewRequest_Click);
            // 
            // btnRemoveBodyFile
            // 
            this.btnRemoveBodyFile.Location = new System.Drawing.Point(98, 530);
            this.btnRemoveBodyFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveBodyFile.Name = "btnRemoveBodyFile";
            this.btnRemoveBodyFile.Size = new System.Drawing.Size(83, 24);
            this.btnRemoveBodyFile.TabIndex = 33;
            this.btnRemoveBodyFile.Text = "Remove";
            this.btnRemoveBodyFile.UseVisualStyleBackColor = true;
            this.btnRemoveBodyFile.Click += new System.EventHandler(this.btnRemoveBodyFile_Click);
            // 
            // cmbOutputFolder
            // 
            this.cmbOutputFolder.FormattingEnabled = true;
            this.cmbOutputFolder.Location = new System.Drawing.Point(347, 575);
            this.cmbOutputFolder.Name = "cmbOutputFolder";
            this.cmbOutputFolder.Size = new System.Drawing.Size(272, 21);
            this.cmbOutputFolder.TabIndex = 34;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(346, 559);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "Output Folder";
            // 
            // btnAddOutputFolder
            // 
            this.btnAddOutputFolder.Location = new System.Drawing.Point(629, 573);
            this.btnAddOutputFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddOutputFolder.Name = "btnAddOutputFolder";
            this.btnAddOutputFolder.Size = new System.Drawing.Size(83, 24);
            this.btnAddOutputFolder.TabIndex = 36;
            this.btnAddOutputFolder.Text = "Add...";
            this.btnAddOutputFolder.UseVisualStyleBackColor = true;
            this.btnAddOutputFolder.Click += new System.EventHandler(this.btnAddOutputFolder_Click);
            // 
            // btnClearOutputFolder
            // 
            this.btnClearOutputFolder.Location = new System.Drawing.Point(629, 629);
            this.btnClearOutputFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearOutputFolder.Name = "btnClearOutputFolder";
            this.btnClearOutputFolder.Size = new System.Drawing.Size(83, 24);
            this.btnClearOutputFolder.TabIndex = 36;
            this.btnClearOutputFolder.Text = "Empty";
            this.btnClearOutputFolder.UseVisualStyleBackColor = true;
            this.btnClearOutputFolder.Click += new System.EventHandler(this.btnClearOutputFolder_Click);
            // 
            // btnRemoveOutputFolder
            // 
            this.btnRemoveOutputFolder.Location = new System.Drawing.Point(630, 601);
            this.btnRemoveOutputFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveOutputFolder.Name = "btnRemoveOutputFolder";
            this.btnRemoveOutputFolder.Size = new System.Drawing.Size(83, 24);
            this.btnRemoveOutputFolder.TabIndex = 36;
            this.btnRemoveOutputFolder.Text = "Remove";
            this.btnRemoveOutputFolder.UseVisualStyleBackColor = true;
            this.btnRemoveOutputFolder.Click += new System.EventHandler(this.btnRemoveOutputFolder_Click);
            // 
            // rtxtOutput
            // 
            this.rtxtOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtxtOutput.Location = new System.Drawing.Point(717, 23);
            this.rtxtOutput.Name = "rtxtOutput";
            this.rtxtOutput.Size = new System.Drawing.Size(540, 617);
            this.rtxtOutput.TabIndex = 37;
            this.rtxtOutput.Text = "";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(344, 203);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(73, 13);
            this.lblStatus.TabIndex = 25;
            this.lblStatus.Text = "Authenticated";
            // 
            // btnRunCollection
            // 
            this.btnRunCollection.Location = new System.Drawing.Point(8, 89);
            this.btnRunCollection.Margin = new System.Windows.Forms.Padding(2);
            this.btnRunCollection.Name = "btnRunCollection";
            this.btnRunCollection.Size = new System.Drawing.Size(122, 33);
            this.btnRunCollection.TabIndex = 38;
            this.btnRunCollection.Text = "RUN COLLECTION";
            this.btnRunCollection.UseVisualStyleBackColor = true;
            this.btnRunCollection.Click += new System.EventHandler(this.btnRunCollection_Click);
            // 
            // btnAddBaseUrl
            // 
            this.btnAddBaseUrl.Location = new System.Drawing.Point(423, 23);
            this.btnAddBaseUrl.Name = "btnAddBaseUrl";
            this.btnAddBaseUrl.Size = new System.Drawing.Size(75, 23);
            this.btnAddBaseUrl.TabIndex = 39;
            this.btnAddBaseUrl.Text = "Add";
            this.btnAddBaseUrl.UseVisualStyleBackColor = true;
            this.btnAddBaseUrl.Click += new System.EventHandler(this.btnAddBaseUrl_Click);
            // 
            // btnRemoveBaseUrl
            // 
            this.btnRemoveBaseUrl.Location = new System.Drawing.Point(504, 23);
            this.btnRemoveBaseUrl.Name = "btnRemoveBaseUrl";
            this.btnRemoveBaseUrl.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveBaseUrl.TabIndex = 40;
            this.btnRemoveBaseUrl.Text = "Remove";
            this.btnRemoveBaseUrl.UseVisualStyleBackColor = true;
            this.btnRemoveBaseUrl.Click += new System.EventHandler(this.btnRemoveBaseUrl_Click);
            // 
            // chkAuthenticate
            // 
            this.chkAuthenticate.AutoSize = true;
            this.chkAuthenticate.Location = new System.Drawing.Point(587, 20);
            this.chkAuthenticate.Name = "chkAuthenticate";
            this.chkAuthenticate.Size = new System.Drawing.Size(99, 27);
            this.chkAuthenticate.TabIndex = 41;
            this.chkAuthenticate.Text = "Authenticate";
            this.chkAuthenticate.UseVisualStyleBackColor = true;
            this.chkAuthenticate.CheckedChanged += new System.EventHandler(this.chkAuthenticate_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1269, 732);
            this.Controls.Add(this.chkAuthenticate);
            this.Controls.Add(this.btnRemoveBaseUrl);
            this.Controls.Add(this.btnAddBaseUrl);
            this.Controls.Add(this.btnRunCollection);
            this.Controls.Add(this.rtxtOutput);
            this.Controls.Add(this.btnRemoveOutputFolder);
            this.Controls.Add(this.btnClearOutputFolder);
            this.Controls.Add(this.btnAddOutputFolder);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmbOutputFolder);
            this.Controls.Add(this.btnRemoveBodyFile);
            this.Controls.Add(this.btnNewRequest);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cmbRequest);
            this.Controls.Add(this.grpRequestType);
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
            this.Controls.Add(this.btnAuthenticate);
            this.Controls.Add(this.btnSendRequest);
            this.Controls.Add(this.btnRemoveRequest);
            this.Controls.Add(this.btnSaveRequest);
            this.Controls.Add(this.btnAddBodyFiles);
            this.Controls.Add(this.lstBodyFiles);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtParameters);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEndpoint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "PixEdit Cloud API Test Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpRequestType.ResumeLayout(false);
            this.grpRequestType.PerformLayout();
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
        private System.Windows.Forms.Button btnAddBodyFiles;
        private System.Windows.Forms.Button btnSendRequest;
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
        private MRG.Controls.UI.LoadingCircle busyIndicator;
        private System.Windows.Forms.ComboBox cmbEndpointBaseUrl;
        private System.Windows.Forms.RadioButton rbtnGet;
        private System.Windows.Forms.RadioButton rbtnPost;
        private System.Windows.Forms.GroupBox grpRequestType;
        private System.Windows.Forms.ComboBox cmbRequest;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSaveRequest;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRemoveRequest;
        private System.Windows.Forms.Button btnNewRequest;
        private System.Windows.Forms.Button btnRemoveBodyFile;
        private System.Windows.Forms.ComboBox cmbOutputFolder;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnAddOutputFolder;
        private System.Windows.Forms.Button btnClearOutputFolder;
        private System.Windows.Forms.Button btnRemoveOutputFolder;
        private System.Windows.Forms.RichTextBox rtxtOutput;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnRunCollection;
        private System.Windows.Forms.Button btnAddBaseUrl;
        private System.Windows.Forms.Button btnRemoveBaseUrl;
        private System.Windows.Forms.CheckBox chkAuthenticate;
    }
}

