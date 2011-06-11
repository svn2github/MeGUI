namespace MeGUI.packages.tools.oneclick
{
    partial class OneClickConfigPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label3;
            this.otherGroupBox = new System.Windows.Forms.GroupBox();
            this.keepInputResolution = new System.Windows.Forms.CheckBox();
            this.autoCrop = new System.Windows.Forms.CheckBox();
            this.preprocessVideo = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.filesizeLabel = new System.Windows.Forms.Label();
            this.autoDeint = new System.Windows.Forms.CheckBox();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.outputResolutionLabel = new System.Windows.Forms.Label();
            this.containerFormatLabel = new System.Windows.Forms.Label();
            this.containerTypeList = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.extraGroupbox = new System.Windows.Forms.GroupBox();
            this.chkDontEncodeVideo = new System.Windows.Forms.CheckBox();
            this.usechaptersmarks = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.audioProfileLabel = new System.Windows.Forms.Label();
            this.videoCodecLabel = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbAudioEncoding = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.avsProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.splitSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.fileSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.audioProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.videoProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            label3 = new System.Windows.Forms.Label();
            this.otherGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.extraGroupbox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(5, 170);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(78, 13);
            label3.TabIndex = 40;
            label3.Text = "Avisynth profile";
            // 
            // otherGroupBox
            // 
            this.otherGroupBox.Controls.Add(this.keepInputResolution);
            this.otherGroupBox.Controls.Add(this.autoCrop);
            this.otherGroupBox.Controls.Add(label3);
            this.otherGroupBox.Controls.Add(this.avsProfile);
            this.otherGroupBox.Controls.Add(this.splitSize);
            this.otherGroupBox.Controls.Add(this.fileSize);
            this.otherGroupBox.Controls.Add(this.preprocessVideo);
            this.otherGroupBox.Controls.Add(this.label2);
            this.otherGroupBox.Controls.Add(this.filesizeLabel);
            this.otherGroupBox.Controls.Add(this.autoDeint);
            this.otherGroupBox.Controls.Add(this.signalAR);
            this.otherGroupBox.Controls.Add(this.horizontalResolution);
            this.otherGroupBox.Controls.Add(this.outputResolutionLabel);
            this.otherGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.otherGroupBox.Location = new System.Drawing.Point(3, 3);
            this.otherGroupBox.Name = "otherGroupBox";
            this.otherGroupBox.Size = new System.Drawing.Size(419, 224);
            this.otherGroupBox.TabIndex = 38;
            this.otherGroupBox.TabStop = false;
            this.otherGroupBox.Text = "Filesize and Avisynth setup";
            // 
            // keepInputResolution
            // 
            this.keepInputResolution.AutoSize = true;
            this.keepInputResolution.Location = new System.Drawing.Point(110, 122);
            this.keepInputResolution.Name = "keepInputResolution";
            this.keepInputResolution.Size = new System.Drawing.Size(242, 17);
            this.keepInputResolution.TabIndex = 42;
            this.keepInputResolution.Text = "Keep Input Resolution (disable Crop && Resize)";
            this.keepInputResolution.UseVisualStyleBackColor = true;
            this.keepInputResolution.CheckedChanged += new System.EventHandler(this.keepInputResolution_CheckedChanged);
            // 
            // autoCrop
            // 
            this.autoCrop.AutoSize = true;
            this.autoCrop.Checked = true;
            this.autoCrop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCrop.Location = new System.Drawing.Point(189, 99);
            this.autoCrop.Name = "autoCrop";
            this.autoCrop.Size = new System.Drawing.Size(70, 17);
            this.autoCrop.TabIndex = 41;
            this.autoCrop.Text = "AutoCrop";
            this.autoCrop.UseVisualStyleBackColor = true;
            // 
            // preprocessVideo
            // 
            this.preprocessVideo.AutoSize = true;
            this.preprocessVideo.Location = new System.Drawing.Point(110, 194);
            this.preprocessVideo.Name = "preprocessVideo";
            this.preprocessVideo.Size = new System.Drawing.Size(101, 17);
            this.preprocessVideo.TabIndex = 37;
            this.preprocessVideo.Text = "Prerender video";
            this.preprocessVideo.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Splitting:";
            // 
            // filesizeLabel
            // 
            this.filesizeLabel.Location = new System.Drawing.Point(6, 27);
            this.filesizeLabel.Name = "filesizeLabel";
            this.filesizeLabel.Size = new System.Drawing.Size(90, 13);
            this.filesizeLabel.TabIndex = 25;
            this.filesizeLabel.Text = "Filesize";
            // 
            // autoDeint
            // 
            this.autoDeint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoDeint.AutoSize = true;
            this.autoDeint.Location = new System.Drawing.Point(273, 194);
            this.autoDeint.Name = "autoDeint";
            this.autoDeint.Size = new System.Drawing.Size(138, 17);
            this.autoDeint.TabIndex = 35;
            this.autoDeint.Text = "Automatic Deinterlacing";
            this.autoDeint.UseVisualStyleBackColor = true;
            // 
            // signalAR
            // 
            this.signalAR.AutoSize = true;
            this.signalAR.Location = new System.Drawing.Point(273, 99);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(73, 17);
            this.signalAR.TabIndex = 32;
            this.signalAR.Text = "Signal AR";
            this.signalAR.UseVisualStyleBackColor = true;
            // 
            // horizontalResolution
            // 
            this.horizontalResolution.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Location = new System.Drawing.Point(110, 98);
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
            // outputResolutionLabel
            // 
            this.outputResolutionLabel.Location = new System.Drawing.Point(6, 102);
            this.outputResolutionLabel.Name = "outputResolutionLabel";
            this.outputResolutionLabel.Size = new System.Drawing.Size(100, 13);
            this.outputResolutionLabel.TabIndex = 30;
            this.outputResolutionLabel.Text = "Output Resolution";
            // 
            // containerFormatLabel
            // 
            this.containerFormatLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerFormatLabel.Location = new System.Drawing.Point(3, 3);
            this.containerFormatLabel.Name = "containerFormatLabel";
            this.containerFormatLabel.Size = new System.Drawing.Size(419, 56);
            this.containerFormatLabel.TabIndex = 17;
            this.containerFormatLabel.Text = "Text change later for resource behavior reasons";
            // 
            // containerTypeList
            // 
            this.containerTypeList.CheckOnClick = true;
            this.containerTypeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerTypeList.Location = new System.Drawing.Point(3, 59);
            this.containerTypeList.Name = "containerTypeList";
            this.containerTypeList.Size = new System.Drawing.Size(419, 168);
            this.containerTypeList.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(433, 256);
            this.tabControl1.TabIndex = 39;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.otherGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(425, 230);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Filesize and Avisynth Setup";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.extraGroupbox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(425, 230);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Encoding Setup";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // extraGroupbox
            // 
            this.extraGroupbox.Controls.Add(this.label4);
            this.extraGroupbox.Controls.Add(this.cbAudioEncoding);
            this.extraGroupbox.Controls.Add(this.chkDontEncodeVideo);
            this.extraGroupbox.Controls.Add(this.usechaptersmarks);
            this.extraGroupbox.Controls.Add(this.label1);
            this.extraGroupbox.Controls.Add(this.audioProfile);
            this.extraGroupbox.Controls.Add(this.videoProfile);
            this.extraGroupbox.Controls.Add(this.audioProfileLabel);
            this.extraGroupbox.Controls.Add(this.videoCodecLabel);
            this.extraGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extraGroupbox.Location = new System.Drawing.Point(3, 3);
            this.extraGroupbox.Name = "extraGroupbox";
            this.extraGroupbox.Size = new System.Drawing.Size(419, 224);
            this.extraGroupbox.TabIndex = 40;
            this.extraGroupbox.TabStop = false;
            this.extraGroupbox.Text = "Encoding Setup";
            // 
            // chkDontEncodeVideo
            // 
            this.chkDontEncodeVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDontEncodeVideo.AutoSize = true;
            this.chkDontEncodeVideo.Location = new System.Drawing.Point(292, 23);
            this.chkDontEncodeVideo.Name = "chkDontEncodeVideo";
            this.chkDontEncodeVideo.Size = new System.Drawing.Size(119, 17);
            this.chkDontEncodeVideo.TabIndex = 41;
            this.chkDontEncodeVideo.Text = "Don\'t encode video";
            this.chkDontEncodeVideo.CheckedChanged += new System.EventHandler(this.chkDontEncodeVideo_CheckedChanged);
            // 
            // usechaptersmarks
            // 
            this.usechaptersmarks.AutoSize = true;
            this.usechaptersmarks.Location = new System.Drawing.Point(89, 47);
            this.usechaptersmarks.Name = "usechaptersmarks";
            this.usechaptersmarks.Size = new System.Drawing.Size(229, 17);
            this.usechaptersmarks.TabIndex = 40;
            this.usechaptersmarks.Text = "Force using Key-Frames for chapters marks";
            this.usechaptersmarks.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(16, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(396, 31);
            this.label1.TabIndex = 29;
            this.label1.Text = "Note: unless changed in the One Click Window, these audio settings will be used f" +
    "or all selected tracks.";
            // 
            // audioProfileLabel
            // 
            this.audioProfileLabel.Location = new System.Drawing.Point(5, 104);
            this.audioProfileLabel.Name = "audioProfileLabel";
            this.audioProfileLabel.Size = new System.Drawing.Size(100, 13);
            this.audioProfileLabel.TabIndex = 27;
            this.audioProfileLabel.Text = "Audio Codec";
            // 
            // videoCodecLabel
            // 
            this.videoCodecLabel.Location = new System.Drawing.Point(6, 23);
            this.videoCodecLabel.Name = "videoCodecLabel";
            this.videoCodecLabel.Size = new System.Drawing.Size(90, 13);
            this.videoCodecLabel.TabIndex = 17;
            this.videoCodecLabel.Text = "Video Preset";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.containerTypeList);
            this.tabPage2.Controls.Add(this.containerFormatLabel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(425, 230);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Container Type";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbAudioEncoding
            // 
            this.cbAudioEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAudioEncoding.FormattingEnabled = true;
            this.cbAudioEncoding.Items.AddRange(new object[] {
            "always",
            "if codec does not match",
            "never"});
            this.cbAudioEncoding.Location = new System.Drawing.Point(170, 128);
            this.cbAudioEncoding.Name = "cbAudioEncoding";
            this.cbAudioEncoding.Size = new System.Drawing.Size(148, 21);
            this.cbAudioEncoding.TabIndex = 42;
            this.cbAudioEncoding.SelectedIndexChanged += new System.EventHandler(this.dontEncodeAudio_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(86, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 43;
            this.label4.Text = "encode audio: ";
            // 
            // avsProfile
            // 
            this.avsProfile.Location = new System.Drawing.Point(109, 165);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.ProfileSet = "AviSynth";
            this.avsProfile.Size = new System.Drawing.Size(298, 22);
            this.avsProfile.TabIndex = 39;
            // 
            // splitSize
            // 
            this.splitSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.splitSize.Location = new System.Drawing.Point(110, 45);
            this.splitSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitSize.Name = "splitSize";
            this.splitSize.NullString = "Dont split";
            this.splitSize.SelectedIndex = 0;
            this.splitSize.Size = new System.Drawing.Size(208, 29);
            this.splitSize.TabIndex = 38;
            // 
            // fileSize
            // 
            this.fileSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.fileSize.Location = new System.Drawing.Point(110, 19);
            this.fileSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.fileSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.fileSize.Name = "fileSize";
            this.fileSize.NullString = "Don\'t care";
            this.fileSize.SelectedIndex = 0;
            this.fileSize.Size = new System.Drawing.Size(208, 29);
            this.fileSize.TabIndex = 38;
            // 
            // audioProfile
            // 
            this.audioProfile.Location = new System.Drawing.Point(88, 100);
            this.audioProfile.Name = "audioProfile";
            this.audioProfile.ProfileSet = "Audio";
            this.audioProfile.Size = new System.Drawing.Size(230, 22);
            this.audioProfile.TabIndex = 39;
            // 
            // videoProfile
            // 
            this.videoProfile.Location = new System.Drawing.Point(88, 19);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.ProfileSet = "Video";
            this.videoProfile.Size = new System.Drawing.Size(199, 22);
            this.videoProfile.TabIndex = 39;
            // 
            // OneClickConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "OneClickConfigPanel";
            this.Size = new System.Drawing.Size(433, 256);
            this.otherGroupBox.ResumeLayout(false);
            this.otherGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.extraGroupbox.ResumeLayout(false);
            this.extraGroupbox.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox otherGroupBox;
        private System.Windows.Forms.Label filesizeLabel;
        private System.Windows.Forms.CheckBox autoDeint;
        private System.Windows.Forms.CheckBox signalAR;
        private System.Windows.Forms.NumericUpDown horizontalResolution;
        private System.Windows.Forms.Label outputResolutionLabel;
        private System.Windows.Forms.Label containerFormatLabel;
        private System.Windows.Forms.CheckBox preprocessVideo;
        private System.Windows.Forms.CheckedListBox containerTypeList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MeGUI.core.gui.TargetSizeSCBox splitSize;
        private MeGUI.core.gui.TargetSizeSCBox fileSize;
        private System.Windows.Forms.Label label2;
        private MeGUI.core.gui.ConfigableProfilesControl avsProfile;
        private System.Windows.Forms.CheckBox autoCrop;
        private System.Windows.Forms.CheckBox keepInputResolution;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox extraGroupbox;
        private System.Windows.Forms.CheckBox chkDontEncodeVideo;
        private System.Windows.Forms.CheckBox usechaptersmarks;
        private System.Windows.Forms.Label label1;
        private core.gui.ConfigableProfilesControl audioProfile;
        private core.gui.ConfigableProfilesControl videoProfile;
        private System.Windows.Forms.Label audioProfileLabel;
        private System.Windows.Forms.Label videoCodecLabel;
        private System.Windows.Forms.ComboBox cbAudioEncoding;
        private System.Windows.Forms.Label label4;
    }
}
