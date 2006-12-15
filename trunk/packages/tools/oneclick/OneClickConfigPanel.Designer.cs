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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneClickConfigPanel));
            this.otherGroupBox = new System.Windows.Forms.GroupBox();
            this.preprocessVideo = new System.Windows.Forms.CheckBox();
            this.filesizeLabel = new System.Windows.Forms.Label();
            this.filesizeComboBox = new System.Windows.Forms.ComboBox();
            this.autoDeint = new System.Windows.Forms.CheckBox();
            this.splitOutput = new System.Windows.Forms.CheckBox();
            this.inKBLabel = new System.Windows.Forms.Label();
            this.splitSize = new System.Windows.Forms.TextBox();
            this.MBLabel = new System.Windows.Forms.Label();
            this.filesizeKB = new System.Windows.Forms.TextBox();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.outputResolutionLabel = new System.Windows.Forms.Label();
            this.extraGroupbox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.audioProfileLabel = new System.Windows.Forms.Label();
            this.dontEncodeAudio = new System.Windows.Forms.CheckBox();
            this.videoCodecLabel = new System.Windows.Forms.Label();
            this.audioCodec = new System.Windows.Forms.ComboBox();
            this.videoCodec = new System.Windows.Forms.ComboBox();
            this.containerFormatLabel = new System.Windows.Forms.Label();
            this.containerTypeList = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.avsProfileControl = new MeGUI.core.details.video.ProfileControl();
            this.audioProfileControl = new MeGUI.core.details.video.ProfileControl();
            this.videoProfileControl = new MeGUI.core.details.video.ProfileControl();
            this.otherGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.extraGroupbox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // otherGroupBox
            // 
            this.otherGroupBox.AutoSize = true;
            this.otherGroupBox.Controls.Add(this.preprocessVideo);
            this.otherGroupBox.Controls.Add(this.avsProfileControl);
            this.otherGroupBox.Controls.Add(this.filesizeLabel);
            this.otherGroupBox.Controls.Add(this.filesizeComboBox);
            this.otherGroupBox.Controls.Add(this.autoDeint);
            this.otherGroupBox.Controls.Add(this.splitOutput);
            this.otherGroupBox.Controls.Add(this.inKBLabel);
            this.otherGroupBox.Controls.Add(this.splitSize);
            this.otherGroupBox.Controls.Add(this.MBLabel);
            this.otherGroupBox.Controls.Add(this.filesizeKB);
            this.otherGroupBox.Controls.Add(this.signalAR);
            this.otherGroupBox.Controls.Add(this.horizontalResolution);
            this.otherGroupBox.Controls.Add(this.outputResolutionLabel);
            this.otherGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.otherGroupBox.Location = new System.Drawing.Point(3, 3);
            this.otherGroupBox.Name = "otherGroupBox";
            this.otherGroupBox.Size = new System.Drawing.Size(419, 162);
            this.otherGroupBox.TabIndex = 38;
            this.otherGroupBox.TabStop = false;
            this.otherGroupBox.Text = "Filesize and Avisynth setup";
            // 
            // preprocessVideo
            // 
            this.preprocessVideo.AutoSize = true;
            this.preprocessVideo.Location = new System.Drawing.Point(61, 126);
            this.preprocessVideo.Name = "preprocessVideo";
            this.preprocessVideo.Size = new System.Drawing.Size(179, 17);
            this.preprocessVideo.TabIndex = 37;
            this.preprocessVideo.Text = "Prerender video (for slow scripts)";
            this.preprocessVideo.UseVisualStyleBackColor = true;
            // 
            // filesizeLabel
            // 
            this.filesizeLabel.Location = new System.Drawing.Point(6, 16);
            this.filesizeLabel.Name = "filesizeLabel";
            this.filesizeLabel.Size = new System.Drawing.Size(90, 13);
            this.filesizeLabel.TabIndex = 25;
            this.filesizeLabel.Text = "Filesize";
            // 
            // filesizeComboBox
            // 
            this.filesizeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filesizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filesizeComboBox.Location = new System.Drawing.Point(110, 13);
            this.filesizeComboBox.Name = "filesizeComboBox";
            this.filesizeComboBox.Size = new System.Drawing.Size(132, 21);
            this.filesizeComboBox.TabIndex = 23;
            this.filesizeComboBox.SelectedIndexChanged += new System.EventHandler(this.filesizeComboBox_SelectedIndexChanged);
            // 
            // autoDeint
            // 
            this.autoDeint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoDeint.AutoSize = true;
            this.autoDeint.Location = new System.Drawing.Point(274, 126);
            this.autoDeint.Name = "autoDeint";
            this.autoDeint.Size = new System.Drawing.Size(138, 17);
            this.autoDeint.TabIndex = 35;
            this.autoDeint.Text = "Automatic Deinterlacing";
            this.autoDeint.UseVisualStyleBackColor = true;
            // 
            // splitOutput
            // 
            this.splitOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.splitOutput.AutoSize = true;
            this.splitOutput.Location = new System.Drawing.Point(251, 38);
            this.splitOutput.Name = "splitOutput";
            this.splitOutput.Size = new System.Drawing.Size(46, 17);
            this.splitOutput.TabIndex = 24;
            this.splitOutput.Text = "Split";
            this.splitOutput.CheckedChanged += new System.EventHandler(this.splitOutput_CheckedChanged);
            // 
            // inKBLabel
            // 
            this.inKBLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inKBLabel.Location = new System.Drawing.Point(248, 17);
            this.inKBLabel.Name = "inKBLabel";
            this.inKBLabel.Size = new System.Drawing.Size(50, 13);
            this.inKBLabel.TabIndex = 26;
            this.inKBLabel.Text = "In KB:";
            // 
            // splitSize
            // 
            this.splitSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.splitSize.Enabled = false;
            this.splitSize.Location = new System.Drawing.Point(304, 36);
            this.splitSize.Name = "splitSize";
            this.splitSize.Size = new System.Drawing.Size(77, 20);
            this.splitSize.TabIndex = 22;
            this.splitSize.Text = "0";
            // 
            // MBLabel
            // 
            this.MBLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MBLabel.AutoSize = true;
            this.MBLabel.Location = new System.Drawing.Point(389, 39);
            this.MBLabel.Name = "MBLabel";
            this.MBLabel.Size = new System.Drawing.Size(23, 13);
            this.MBLabel.TabIndex = 23;
            this.MBLabel.Text = "MB";
            // 
            // filesizeKB
            // 
            this.filesizeKB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filesizeKB.Location = new System.Drawing.Point(304, 13);
            this.filesizeKB.Name = "filesizeKB";
            this.filesizeKB.ReadOnly = true;
            this.filesizeKB.Size = new System.Drawing.Size(112, 20);
            this.filesizeKB.TabIndex = 24;
            // 
            // signalAR
            // 
            this.signalAR.Location = new System.Drawing.Point(178, 71);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(76, 24);
            this.signalAR.TabIndex = 32;
            this.signalAR.Text = "Signal AR";
            // 
            // horizontalResolution
            // 
            this.horizontalResolution.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Location = new System.Drawing.Point(110, 71);
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
            this.outputResolutionLabel.Location = new System.Drawing.Point(6, 75);
            this.outputResolutionLabel.Name = "outputResolutionLabel";
            this.outputResolutionLabel.Size = new System.Drawing.Size(100, 13);
            this.outputResolutionLabel.TabIndex = 30;
            this.outputResolutionLabel.Text = "Output Resolution";
            // 
            // extraGroupbox
            // 
            this.extraGroupbox.Controls.Add(this.label1);
            this.extraGroupbox.Controls.Add(this.audioProfileControl);
            this.extraGroupbox.Controls.Add(this.videoProfileControl);
            this.extraGroupbox.Controls.Add(this.audioProfileLabel);
            this.extraGroupbox.Controls.Add(this.dontEncodeAudio);
            this.extraGroupbox.Controls.Add(this.videoCodecLabel);
            this.extraGroupbox.Controls.Add(this.audioCodec);
            this.extraGroupbox.Controls.Add(this.videoCodec);
            this.extraGroupbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.extraGroupbox.Location = new System.Drawing.Point(3, 169);
            this.extraGroupbox.Name = "extraGroupbox";
            this.extraGroupbox.Size = new System.Drawing.Size(419, 174);
            this.extraGroupbox.TabIndex = 39;
            this.extraGroupbox.TabStop = false;
            this.extraGroupbox.Text = "Encoding Setup";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(16, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(396, 31);
            this.label1.TabIndex = 29;
            this.label1.Text = "Note: unless changed in the One Click Window, these audio settings will be used f" +
                "or all selected tracks.";
            // 
            // audioProfileLabel
            // 
            this.audioProfileLabel.Location = new System.Drawing.Point(16, 79);
            this.audioProfileLabel.Name = "audioProfileLabel";
            this.audioProfileLabel.Size = new System.Drawing.Size(100, 13);
            this.audioProfileLabel.TabIndex = 27;
            this.audioProfileLabel.Text = "Audio Codec";
            // 
            // dontEncodeAudio
            // 
            this.dontEncodeAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dontEncodeAudio.Location = new System.Drawing.Point(227, 76);
            this.dontEncodeAudio.Name = "dontEncodeAudio";
            this.dontEncodeAudio.Size = new System.Drawing.Size(130, 21);
            this.dontEncodeAudio.TabIndex = 25;
            this.dontEncodeAudio.Text = "Don\'t encode audio";
            this.dontEncodeAudio.CheckedChanged += new System.EventHandler(this.dontEncodeAudio_CheckedChanged);
            // 
            // videoCodecLabel
            // 
            this.videoCodecLabel.Location = new System.Drawing.Point(16, 19);
            this.videoCodecLabel.Name = "videoCodecLabel";
            this.videoCodecLabel.Size = new System.Drawing.Size(90, 13);
            this.videoCodecLabel.TabIndex = 17;
            this.videoCodecLabel.Text = "Video Codec";
            // 
            // audioCodec
            // 
            this.audioCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioCodec.Enabled = false;
            this.audioCodec.Location = new System.Drawing.Point(123, 76);
            this.audioCodec.Name = "audioCodec";
            this.audioCodec.Size = new System.Drawing.Size(98, 21);
            this.audioCodec.TabIndex = 0;
            // 
            // videoCodec
            // 
            this.videoCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoCodec.Enabled = false;
            this.videoCodec.Location = new System.Drawing.Point(123, 16);
            this.videoCodec.Name = "videoCodec";
            this.videoCodec.Size = new System.Drawing.Size(98, 21);
            this.videoCodec.TabIndex = 0;
            // 
            // containerFormatLabel
            // 
            this.containerFormatLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.containerFormatLabel.Location = new System.Drawing.Point(3, 3);
            this.containerFormatLabel.Name = "containerFormatLabel";
            this.containerFormatLabel.Size = new System.Drawing.Size(419, 56);
            this.containerFormatLabel.TabIndex = 17;
            this.containerFormatLabel.Text = resources.GetString("containerFormatLabel.Text");
            // 
            // containerTypeList
            // 
            this.containerTypeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerTypeList.Location = new System.Drawing.Point(3, 59);
            this.containerTypeList.Name = "containerTypeList";
            this.containerTypeList.Size = new System.Drawing.Size(419, 274);
            this.containerTypeList.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(433, 372);
            this.tabControl1.TabIndex = 39;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.otherGroupBox);
            this.tabPage1.Controls.Add(this.extraGroupbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(425, 346);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Encoding Setup";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.containerTypeList);
            this.tabPage2.Controls.Add(this.containerFormatLabel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(425, 346);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Container type";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // avsProfileControl
            // 
            this.avsProfileControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.avsProfileControl.LabelText = "AVS profile";
            this.avsProfileControl.Location = new System.Drawing.Point(6, 91);
            this.avsProfileControl.Name = "avsProfileControl";
            this.avsProfileControl.Size = new System.Drawing.Size(406, 29);
            this.avsProfileControl.TabIndex = 36;
            // 
            // audioProfileControl
            // 
            this.audioProfileControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioProfileControl.LabelText = "Audio Profile";
            this.audioProfileControl.Location = new System.Drawing.Point(19, 103);
            this.audioProfileControl.Name = "audioProfileControl";
            this.audioProfileControl.Size = new System.Drawing.Size(393, 29);
            this.audioProfileControl.TabIndex = 28;
            // 
            // videoProfileControl
            // 
            this.videoProfileControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoProfileControl.LabelText = "Video Profile";
            this.videoProfileControl.Location = new System.Drawing.Point(19, 41);
            this.videoProfileControl.Name = "videoProfileControl";
            this.videoProfileControl.Size = new System.Drawing.Size(393, 29);
            this.videoProfileControl.TabIndex = 28;
            // 
            // OneClickConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "OneClickConfigPanel";
            this.Size = new System.Drawing.Size(433, 372);
            this.otherGroupBox.ResumeLayout(false);
            this.otherGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.extraGroupbox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox otherGroupBox;
        private MeGUI.core.details.video.ProfileControl avsProfileControl;
        private System.Windows.Forms.Label filesizeLabel;
        private System.Windows.Forms.ComboBox filesizeComboBox;
        private System.Windows.Forms.CheckBox autoDeint;
        private System.Windows.Forms.Label inKBLabel;
        private System.Windows.Forms.TextBox filesizeKB;
        private System.Windows.Forms.CheckBox signalAR;
        private System.Windows.Forms.NumericUpDown horizontalResolution;
        private System.Windows.Forms.Label outputResolutionLabel;
        private System.Windows.Forms.GroupBox extraGroupbox;
        private System.Windows.Forms.Label audioProfileLabel;
        private System.Windows.Forms.CheckBox dontEncodeAudio;
        private System.Windows.Forms.CheckBox splitOutput;
        private System.Windows.Forms.TextBox splitSize;
        private System.Windows.Forms.Label MBLabel;
        private System.Windows.Forms.Label videoCodecLabel;
        private System.Windows.Forms.ComboBox videoCodec;
        private System.Windows.Forms.Label containerFormatLabel;
        private MeGUI.core.details.video.ProfileControl audioProfileControl;
        private MeGUI.core.details.video.ProfileControl videoProfileControl;
        private System.Windows.Forms.ComboBox audioCodec;
        private System.Windows.Forms.CheckBox preprocessVideo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox containerTypeList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}
