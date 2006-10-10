namespace MeGUI
{
    partial class VideoEncodingComponent
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
            this.videoOutputLabel = new System.Windows.Forms.Label();
            this.videoCodec = new System.Windows.Forms.ComboBox();
            this.fileType = new System.Windows.Forms.ComboBox();
            this.videoCodecLabel = new System.Windows.Forms.Label();
            this.videoInputLabel = new System.Windows.Forms.Label();
            this.queueVideoButton = new System.Windows.Forms.Button();
            this.addPrerenderJob = new System.Windows.Forms.CheckBox();
            this.addAnalysisPass = new System.Windows.Forms.Button();
            this.videoInput = new MeGUI.FileBar();
            this.videoOutput = new MeGUI.FileBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.profileControl1 = new MeGUI.core.details.video.ProfileControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // videoOutputLabel
            // 
            this.videoOutputLabel.AutoSize = true;
            this.videoOutputLabel.Location = new System.Drawing.Point(6, 40);
            this.videoOutputLabel.Name = "videoOutputLabel";
            this.videoOutputLabel.Size = new System.Drawing.Size(69, 13);
            this.videoOutputLabel.TabIndex = 24;
            this.videoOutputLabel.Text = "Video Output";
            // 
            // videoCodec
            // 
            this.videoCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoCodec.Location = new System.Drawing.Point(71, 67);
            this.videoCodec.Name = "videoCodec";
            this.videoCodec.Size = new System.Drawing.Size(58, 21);
            this.videoCodec.TabIndex = 22;
            this.videoCodec.SelectedIndexChanged += new System.EventHandler(this.codec_SelectedIndexChanged);
            // 
            // fileType
            // 
            this.fileType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileType.Location = new System.Drawing.Point(365, 66);
            this.fileType.Name = "fileType";
            this.fileType.Size = new System.Drawing.Size(56, 21);
            this.fileType.TabIndex = 19;
            this.fileType.SelectedIndexChanged += new System.EventHandler(this.fileType_SelectedIndexChanged);
            // 
            // videoCodecLabel
            // 
            this.videoCodecLabel.AutoSize = true;
            this.videoCodecLabel.Location = new System.Drawing.Point(6, 67);
            this.videoCodecLabel.Name = "videoCodecLabel";
            this.videoCodecLabel.Size = new System.Drawing.Size(38, 13);
            this.videoCodecLabel.TabIndex = 21;
            this.videoCodecLabel.Text = "Codec";
            this.videoCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.AutoSize = true;
            this.videoInputLabel.Location = new System.Drawing.Point(6, 16);
            this.videoInputLabel.Name = "videoInputLabel";
            this.videoInputLabel.Size = new System.Drawing.Size(79, 13);
            this.videoInputLabel.TabIndex = 18;
            this.videoInputLabel.Text = "AviSynth Script";
            // 
            // queueVideoButton
            // 
            this.queueVideoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.queueVideoButton.Location = new System.Drawing.Point(358, 120);
            this.queueVideoButton.Name = "queueVideoButton";
            this.queueVideoButton.Size = new System.Drawing.Size(62, 23);
            this.queueVideoButton.TabIndex = 26;
            this.queueVideoButton.Text = "Enqueue";
            this.queueVideoButton.Click += new System.EventHandler(this.queueVideoButton_Click);
            // 
            // addPrerenderJob
            // 
            this.addPrerenderJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addPrerenderJob.AutoSize = true;
            this.addPrerenderJob.Location = new System.Drawing.Point(217, 124);
            this.addPrerenderJob.Name = "addPrerenderJob";
            this.addPrerenderJob.Size = new System.Drawing.Size(127, 17);
            this.addPrerenderJob.TabIndex = 27;
            this.addPrerenderJob.Text = "Add pre-rendering job";
            this.addPrerenderJob.UseVisualStyleBackColor = true;
            // 
            // addAnalysisPass
            // 
            this.addAnalysisPass.Location = new System.Drawing.Point(9, 120);
            this.addAnalysisPass.Name = "addAnalysisPass";
            this.addAnalysisPass.Size = new System.Drawing.Size(133, 23);
            this.addAnalysisPass.TabIndex = 28;
            this.addAnalysisPass.Text = "Queue analysis pass";
            this.addAnalysisPass.UseVisualStyleBackColor = true;
            // 
            // videoInput
            // 
            this.videoInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoInput.Filename = "";
            this.videoInput.Filter = "AviSynth files (*.avs)|*.avs|All files (*.*)|*.*";
            this.videoInput.FolderMode = false;
            this.videoInput.Location = new System.Drawing.Point(139, 8);
            this.videoInput.Name = "videoInput";
            this.videoInput.ReadOnly = true;
            this.videoInput.SaveMode = false;
            this.videoInput.Size = new System.Drawing.Size(282, 26);
            this.videoInput.TabIndex = 29;
            this.videoInput.Title = "Open AviSynth script";
            this.videoInput.FileSelected += new MeGUI.FileBarEventHandler(this.videoInput_FileSelected);
            // 
            // videoOutput
            // 
            this.videoOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoOutput.Filename = "";
            this.videoOutput.Filter = null;
            this.videoOutput.FolderMode = false;
            this.videoOutput.Location = new System.Drawing.Point(139, 34);
            this.videoOutput.Name = "videoOutput";
            this.videoOutput.ReadOnly = false;
            this.videoOutput.SaveMode = false;
            this.videoOutput.Size = new System.Drawing.Size(281, 26);
            this.videoOutput.TabIndex = 29;
            this.videoOutput.Title = "Enter name of output";
            this.videoOutput.FileSelected += new MeGUI.FileBarEventHandler(this.videoOutput_FileSelected);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.profileControl1);
            this.groupBox1.Controls.Add(this.videoInputLabel);
            this.groupBox1.Controls.Add(this.videoOutput);
            this.groupBox1.Controls.Add(this.videoInput);
            this.groupBox1.Controls.Add(this.videoOutputLabel);
            this.groupBox1.Controls.Add(this.addAnalysisPass);
            this.groupBox1.Controls.Add(this.addPrerenderJob);
            this.groupBox1.Controls.Add(this.queueVideoButton);
            this.groupBox1.Controls.Add(this.videoCodec);
            this.groupBox1.Controls.Add(this.fileType);
            this.groupBox1.Controls.Add(this.videoCodecLabel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 195);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Video encoding";
            // 
            // profileControl1
            // 
            this.profileControl1.Location = new System.Drawing.Point(0, 89);
            this.profileControl1.Name = "profileControl1";
            this.profileControl1.Size = new System.Drawing.Size(426, 29);
            this.profileControl1.TabIndex = 30;
            // 
            // VideoEncodingComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "VideoEncodingComponent";
            this.Size = new System.Drawing.Size(426, 195);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label videoOutputLabel;
        private System.Windows.Forms.ComboBox videoCodec;
        private System.Windows.Forms.ComboBox fileType;
        private System.Windows.Forms.Label videoCodecLabel;
        private System.Windows.Forms.Label videoInputLabel;
        private System.Windows.Forms.Button queueVideoButton;
        private System.Windows.Forms.CheckBox addPrerenderJob;
        private System.Windows.Forms.Button addAnalysisPass;
        private FileBar videoInput;
        private FileBar videoOutput;
        private System.Windows.Forms.GroupBox groupBox1;
        private MeGUI.core.details.video.ProfileControl profileControl1;

    }
}
