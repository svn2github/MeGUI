namespace MeGUI
{
    partial class OneClickConfigurationDialog
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
            this.filesizeKB = new System.Windows.Forms.TextBox();
            this.inKBLabel = new System.Windows.Forms.Label();
            this.filesizeComboBox = new System.Windows.Forms.ComboBox();
            this.filesizeLabel = new System.Windows.Forms.Label();
            this.autoDeint = new System.Windows.Forms.CheckBox();
            this.avsProfile = new System.Windows.Forms.ComboBox();
            this.avsProfileLabel = new System.Windows.Forms.Label();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.outputResolutionLabel = new System.Windows.Forms.Label();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.extraGroupbox = new System.Windows.Forms.GroupBox();
            this.audioProfileLabel = new System.Windows.Forms.Label();
            this.audioProfile = new System.Windows.Forms.ComboBox();
            this.dontEncodeAudio = new System.Windows.Forms.CheckBox();
            this.splitOutput = new System.Windows.Forms.CheckBox();
            this.splitSize = new System.Windows.Forms.TextBox();
            this.MBLabel = new System.Windows.Forms.Label();
            this.videoProfile = new System.Windows.Forms.ComboBox();
            this.videoCodecLabel = new System.Windows.Forms.Label();
            this.videoCodec = new System.Windows.Forms.ComboBox();
            this.containerFormatLabel = new System.Windows.Forms.Label();
            this.containerFormat = new System.Windows.Forms.ComboBox();
            this.otherGroupBox = new System.Windows.Forms.GroupBox();
            this.playbackMethodLabel = new System.Windows.Forms.Label();
            this.playbackMethod = new System.Windows.Forms.ComboBox();
            this.profilesGroupBox = new System.Windows.Forms.GroupBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.deleteProfileButton = new System.Windows.Forms.Button();
            this.newProfileButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.extraGroupbox.SuspendLayout();
            this.otherGroupBox.SuspendLayout();
            this.profilesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // filesizeKB
            // 
            this.filesizeKB.Location = new System.Drawing.Point(296, 13);
            this.filesizeKB.Name = "filesizeKB";
            this.filesizeKB.ReadOnly = true;
            this.filesizeKB.Size = new System.Drawing.Size(112, 20);
            this.filesizeKB.TabIndex = 24;
            // 
            // inKBLabel
            // 
            this.inKBLabel.Location = new System.Drawing.Point(240, 17);
            this.inKBLabel.Name = "inKBLabel";
            this.inKBLabel.Size = new System.Drawing.Size(50, 13);
            this.inKBLabel.TabIndex = 26;
            this.inKBLabel.Text = "In KB:";
            // 
            // filesizeComboBox
            // 
            this.filesizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filesizeComboBox.Location = new System.Drawing.Point(120, 13);
            this.filesizeComboBox.Name = "filesizeComboBox";
            this.filesizeComboBox.Size = new System.Drawing.Size(114, 21);
            this.filesizeComboBox.TabIndex = 23;
            this.filesizeComboBox.SelectedIndexChanged += new System.EventHandler(this.filesizeComboBox_SelectedIndexChanged);
            // 
            // filesizeLabel
            // 
            this.filesizeLabel.Location = new System.Drawing.Point(16, 16);
            this.filesizeLabel.Name = "filesizeLabel";
            this.filesizeLabel.Size = new System.Drawing.Size(90, 13);
            this.filesizeLabel.TabIndex = 25;
            this.filesizeLabel.Text = "Filesize";
            // 
            // autoDeint
            // 
            this.autoDeint.AutoSize = true;
            this.autoDeint.Location = new System.Drawing.Point(275, 67);
            this.autoDeint.Name = "autoDeint";
            this.autoDeint.Size = new System.Drawing.Size(138, 17);
            this.autoDeint.TabIndex = 35;
            this.autoDeint.Text = "Automatic Deinterlacing";
            this.autoDeint.UseVisualStyleBackColor = true;
            // 
            // avsProfile
            // 
            this.avsProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.avsProfile.Location = new System.Drawing.Point(120, 65);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.Size = new System.Drawing.Size(149, 21);
            this.avsProfile.Sorted = true;
            this.avsProfile.TabIndex = 34;
            // 
            // avsProfileLabel
            // 
            this.avsProfileLabel.Location = new System.Drawing.Point(16, 68);
            this.avsProfileLabel.Name = "avsProfileLabel";
            this.avsProfileLabel.Size = new System.Drawing.Size(72, 18);
            this.avsProfileLabel.TabIndex = 33;
            this.avsProfileLabel.Text = "AVS profile";
            // 
            // signalAR
            // 
            this.signalAR.Location = new System.Drawing.Point(188, 40);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(76, 24);
            this.signalAR.TabIndex = 32;
            this.signalAR.Text = "Signal AR";
            // 
            // outputResolutionLabel
            // 
            this.outputResolutionLabel.Location = new System.Drawing.Point(16, 44);
            this.outputResolutionLabel.Name = "outputResolutionLabel";
            this.outputResolutionLabel.Size = new System.Drawing.Size(100, 13);
            this.outputResolutionLabel.TabIndex = 30;
            this.outputResolutionLabel.Text = "Output Resolution";
            // 
            // horizontalResolution
            // 
            this.horizontalResolution.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Location = new System.Drawing.Point(120, 40);
            this.horizontalResolution.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.horizontalResolution.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Name = "horizontalResolution";
            this.horizontalResolution.Size = new System.Drawing.Size(64, 20);
            this.horizontalResolution.TabIndex = 27;
            this.horizontalResolution.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // extraGroupbox
            // 
            this.extraGroupbox.Controls.Add(this.audioProfileLabel);
            this.extraGroupbox.Controls.Add(this.audioProfile);
            this.extraGroupbox.Controls.Add(this.dontEncodeAudio);
            this.extraGroupbox.Controls.Add(this.splitOutput);
            this.extraGroupbox.Controls.Add(this.splitSize);
            this.extraGroupbox.Controls.Add(this.MBLabel);
            this.extraGroupbox.Controls.Add(this.videoProfile);
            this.extraGroupbox.Controls.Add(this.videoCodecLabel);
            this.extraGroupbox.Controls.Add(this.videoCodec);
            this.extraGroupbox.Controls.Add(this.containerFormatLabel);
            this.extraGroupbox.Controls.Add(this.containerFormat);
            this.extraGroupbox.Location = new System.Drawing.Point(12, 115);
            this.extraGroupbox.Name = "extraGroupbox";
            this.extraGroupbox.Size = new System.Drawing.Size(424, 97);
            this.extraGroupbox.TabIndex = 36;
            this.extraGroupbox.TabStop = false;
            this.extraGroupbox.Text = "Encoding Setup";
            // 
            // audioProfileLabel
            // 
            this.audioProfileLabel.Location = new System.Drawing.Point(16, 72);
            this.audioProfileLabel.Name = "audioProfileLabel";
            this.audioProfileLabel.Size = new System.Drawing.Size(100, 13);
            this.audioProfileLabel.TabIndex = 27;
            this.audioProfileLabel.Text = "Audio Profile";
            // 
            // audioProfile
            // 
            this.audioProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioProfile.Location = new System.Drawing.Point(120, 69);
            this.audioProfile.Name = "audioProfile";
            this.audioProfile.Size = new System.Drawing.Size(149, 21);
            this.audioProfile.Sorted = true;
            this.audioProfile.TabIndex = 26;
            // 
            // dontEncodeAudio
            // 
            this.dontEncodeAudio.Location = new System.Drawing.Point(275, 69);
            this.dontEncodeAudio.Name = "dontEncodeAudio";
            this.dontEncodeAudio.Size = new System.Drawing.Size(130, 21);
            this.dontEncodeAudio.TabIndex = 25;
            this.dontEncodeAudio.Text = "Don\'t encode audio";
            this.dontEncodeAudio.CheckedChanged += new System.EventHandler(this.dontEncodeAudio_CheckedChanged);
            // 
            // splitOutput
            // 
            this.splitOutput.Location = new System.Drawing.Point(206, 47);
            this.splitOutput.Name = "splitOutput";
            this.splitOutput.Size = new System.Drawing.Size(80, 16);
            this.splitOutput.TabIndex = 24;
            this.splitOutput.Text = "Split Size";
            this.splitOutput.CheckedChanged += new System.EventHandler(this.splitOutput_CheckedChanged);
            // 
            // splitSize
            // 
            this.splitSize.Enabled = false;
            this.splitSize.Location = new System.Drawing.Point(310, 45);
            this.splitSize.Name = "splitSize";
            this.splitSize.Size = new System.Drawing.Size(64, 20);
            this.splitSize.TabIndex = 22;
            this.splitSize.Text = "0";
            // 
            // MBLabel
            // 
            this.MBLabel.Location = new System.Drawing.Point(380, 48);
            this.MBLabel.Name = "MBLabel";
            this.MBLabel.Size = new System.Drawing.Size(25, 16);
            this.MBLabel.TabIndex = 23;
            this.MBLabel.Text = "MB";
            // 
            // videoProfile
            // 
            this.videoProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoProfile.Location = new System.Drawing.Point(206, 16);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.Size = new System.Drawing.Size(202, 21);
            this.videoProfile.Sorted = true;
            this.videoProfile.TabIndex = 1;
            this.videoProfile.SelectedIndexChanged += new System.EventHandler(this.videoProfile_SelectedIndexChanged);
            // 
            // videoCodecLabel
            // 
            this.videoCodecLabel.Location = new System.Drawing.Point(16, 19);
            this.videoCodecLabel.Name = "videoCodecLabel";
            this.videoCodecLabel.Size = new System.Drawing.Size(90, 13);
            this.videoCodecLabel.TabIndex = 17;
            this.videoCodecLabel.Text = "Video Codec";
            // 
            // videoCodec
            // 
            this.videoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoCodec.Enabled = false;
            this.videoCodec.Items.AddRange(new object[] {
            "ASP",
            "AVC",
            "Snow",
            "XviD"});
            this.videoCodec.Location = new System.Drawing.Point(120, 16);
            this.videoCodec.Name = "videoCodec";
            this.videoCodec.Size = new System.Drawing.Size(64, 21);
            this.videoCodec.TabIndex = 0;
            // 
            // containerFormatLabel
            // 
            this.containerFormatLabel.Location = new System.Drawing.Point(16, 48);
            this.containerFormatLabel.Name = "containerFormatLabel";
            this.containerFormatLabel.Size = new System.Drawing.Size(92, 13);
            this.containerFormatLabel.TabIndex = 17;
            this.containerFormatLabel.Text = "Container Format";
            // 
            // containerFormat
            // 
            this.containerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containerFormat.Items.AddRange(new object[] {
            "AVI",
            "MKV",
            "MP4"});
            this.containerFormat.Location = new System.Drawing.Point(120, 43);
            this.containerFormat.Name = "containerFormat";
            this.containerFormat.Size = new System.Drawing.Size(64, 21);
            this.containerFormat.TabIndex = 4;
            this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.containerFormat_SelectedIndexChanged);
            // 
            // otherGroupBox
            // 
            this.otherGroupBox.Controls.Add(this.filesizeLabel);
            this.otherGroupBox.Controls.Add(this.filesizeComboBox);
            this.otherGroupBox.Controls.Add(this.autoDeint);
            this.otherGroupBox.Controls.Add(this.inKBLabel);
            this.otherGroupBox.Controls.Add(this.avsProfile);
            this.otherGroupBox.Controls.Add(this.filesizeKB);
            this.otherGroupBox.Controls.Add(this.avsProfileLabel);
            this.otherGroupBox.Controls.Add(this.signalAR);
            this.otherGroupBox.Controls.Add(this.horizontalResolution);
            this.otherGroupBox.Controls.Add(this.outputResolutionLabel);
            this.otherGroupBox.Location = new System.Drawing.Point(12, 12);
            this.otherGroupBox.Name = "otherGroupBox";
            this.otherGroupBox.Size = new System.Drawing.Size(424, 97);
            this.otherGroupBox.TabIndex = 36;
            this.otherGroupBox.TabStop = false;
            this.otherGroupBox.Text = "Filesize and Avisynth setup";
            // 
            // playbackMethodLabel
            // 
            this.playbackMethodLabel.Location = new System.Drawing.Point(16, 16);
            this.playbackMethodLabel.Name = "playbackMethodLabel";
            this.playbackMethodLabel.Size = new System.Drawing.Size(100, 32);
            this.playbackMethodLabel.TabIndex = 37;
            this.playbackMethodLabel.Text = "Playback method (profile name)";
            // 
            // playbackMethod
            // 
            this.playbackMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playbackMethod.Location = new System.Drawing.Point(120, 13);
            this.playbackMethod.Name = "playbackMethod";
            this.playbackMethod.Size = new System.Drawing.Size(288, 21);
            this.playbackMethod.Sorted = true;
            this.playbackMethod.TabIndex = 38;
            this.playbackMethod.SelectedIndexChanged += new System.EventHandler(this.playbackMethod_SelectedIndexChanged);
            // 
            // profilesGroupBox
            // 
            this.profilesGroupBox.Controls.Add(this.updateButton);
            this.profilesGroupBox.Controls.Add(this.deleteProfileButton);
            this.profilesGroupBox.Controls.Add(this.newProfileButton);
            this.profilesGroupBox.Controls.Add(this.playbackMethodLabel);
            this.profilesGroupBox.Controls.Add(this.playbackMethod);
            this.profilesGroupBox.Location = new System.Drawing.Point(12, 218);
            this.profilesGroupBox.Name = "profilesGroupBox";
            this.profilesGroupBox.Size = new System.Drawing.Size(424, 74);
            this.profilesGroupBox.TabIndex = 39;
            this.profilesGroupBox.TabStop = false;
            this.profilesGroupBox.Text = "Profiles";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(133, 40);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 41;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // deleteProfileButton
            // 
            this.deleteProfileButton.Location = new System.Drawing.Point(333, 40);
            this.deleteProfileButton.Name = "deleteProfileButton";
            this.deleteProfileButton.Size = new System.Drawing.Size(75, 23);
            this.deleteProfileButton.TabIndex = 40;
            this.deleteProfileButton.Text = "Delete";
            this.deleteProfileButton.UseVisualStyleBackColor = true;
            this.deleteProfileButton.Click += new System.EventHandler(this.deleteProfileButton_Click);
            // 
            // newProfileButton
            // 
            this.newProfileButton.Location = new System.Drawing.Point(252, 40);
            this.newProfileButton.Name = "newProfileButton";
            this.newProfileButton.Size = new System.Drawing.Size(75, 23);
            this.newProfileButton.TabIndex = 39;
            this.newProfileButton.Text = "New";
            this.newProfileButton.UseVisualStyleBackColor = true;
            this.newProfileButton.Click += new System.EventHandler(this.newProfileButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(366, 298);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 40;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(264, 298);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 41;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // OneClickConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(453, 328);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.profilesGroupBox);
            this.Controls.Add(this.otherGroupBox);
            this.Controls.Add(this.extraGroupbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OneClickConfigurationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "One Click Configuration Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.extraGroupbox.ResumeLayout(false);
            this.extraGroupbox.PerformLayout();
            this.otherGroupBox.ResumeLayout(false);
            this.otherGroupBox.PerformLayout();
            this.profilesGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox filesizeKB;
        private System.Windows.Forms.Label inKBLabel;
        private System.Windows.Forms.ComboBox filesizeComboBox;
        private System.Windows.Forms.Label filesizeLabel;
        private System.Windows.Forms.CheckBox autoDeint;
        private System.Windows.Forms.ComboBox avsProfile;
        private System.Windows.Forms.Label avsProfileLabel;
        private System.Windows.Forms.CheckBox signalAR;
        private System.Windows.Forms.Label outputResolutionLabel;
        private System.Windows.Forms.NumericUpDown horizontalResolution;
        private System.Windows.Forms.GroupBox extraGroupbox;
        private System.Windows.Forms.Label audioProfileLabel;
        private System.Windows.Forms.ComboBox audioProfile;
        private System.Windows.Forms.CheckBox dontEncodeAudio;
        private System.Windows.Forms.CheckBox splitOutput;
        private System.Windows.Forms.TextBox splitSize;
        private System.Windows.Forms.Label MBLabel;
        private System.Windows.Forms.ComboBox videoProfile;
        private System.Windows.Forms.Label videoCodecLabel;
        private System.Windows.Forms.ComboBox videoCodec;
        private System.Windows.Forms.Label containerFormatLabel;
        private System.Windows.Forms.ComboBox containerFormat;
        private System.Windows.Forms.GroupBox otherGroupBox;
        private System.Windows.Forms.Label playbackMethodLabel;
        private System.Windows.Forms.ComboBox playbackMethod;
        private System.Windows.Forms.GroupBox profilesGroupBox;
        private System.Windows.Forms.Button deleteProfileButton;
        private System.Windows.Forms.Button newProfileButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button updateButton;
    }
}