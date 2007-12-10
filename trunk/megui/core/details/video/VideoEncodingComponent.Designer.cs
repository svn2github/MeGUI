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
            this.fileType = new System.Windows.Forms.ComboBox();
            this.videoInputLabel = new System.Windows.Forms.Label();
            this.queueVideoButton = new System.Windows.Forms.Button();
            this.addPrerenderJob = new System.Windows.Forms.CheckBox();
            this.addAnalysisPass = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.videoProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.label1 = new System.Windows.Forms.Label();
            this.videoOutput = new MeGUI.FileBar();
            this.videoInput = new MeGUI.FileBar();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // videoOutputLabel
            // 
            this.videoOutputLabel.AutoSize = true;
            this.videoOutputLabel.Location = new System.Drawing.Point(6, 54);
            this.videoOutputLabel.Name = "videoOutputLabel";
            this.videoOutputLabel.Size = new System.Drawing.Size(69, 13);
            this.videoOutputLabel.TabIndex = 2;
            this.videoOutputLabel.Text = "Video Output";
            // 
            // fileType
            // 
            this.fileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileType.Location = new System.Drawing.Point(113, 110);
            this.fileType.Name = "fileType";
            this.fileType.Size = new System.Drawing.Size(143, 21);
            this.fileType.TabIndex = 7;
            this.fileType.SelectedIndexChanged += new System.EventHandler(this.fileType_SelectedIndexChanged);
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.AutoSize = true;
            this.videoInputLabel.Location = new System.Drawing.Point(6, 26);
            this.videoInputLabel.Name = "videoInputLabel";
            this.videoInputLabel.Size = new System.Drawing.Size(79, 13);
            this.videoInputLabel.TabIndex = 0;
            this.videoInputLabel.Text = "AviSynth Script";
            // 
            // queueVideoButton
            // 
            this.queueVideoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.queueVideoButton.Location = new System.Drawing.Point(422, 137);
            this.queueVideoButton.Name = "queueVideoButton";
            this.queueVideoButton.Size = new System.Drawing.Size(62, 23);
            this.queueVideoButton.TabIndex = 11;
            this.queueVideoButton.Text = "Enqueue";
            this.queueVideoButton.Click += new System.EventHandler(this.queueVideoButton_Click);
            // 
            // addPrerenderJob
            // 
            this.addPrerenderJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addPrerenderJob.AutoSize = true;
            this.addPrerenderJob.Location = new System.Drawing.Point(289, 141);
            this.addPrerenderJob.Name = "addPrerenderJob";
            this.addPrerenderJob.Size = new System.Drawing.Size(127, 17);
            this.addPrerenderJob.TabIndex = 10;
            this.addPrerenderJob.Text = "Add pre-rendering job";
            this.addPrerenderJob.UseVisualStyleBackColor = true;
            // 
            // addAnalysisPass
            // 
            this.addAnalysisPass.Location = new System.Drawing.Point(9, 137);
            this.addAnalysisPass.Name = "addAnalysisPass";
            this.addAnalysisPass.Size = new System.Drawing.Size(133, 23);
            this.addAnalysisPass.TabIndex = 9;
            this.addAnalysisPass.Text = "Queue analysis pass";
            this.addAnalysisPass.UseVisualStyleBackColor = true;
            this.addAnalysisPass.Click += new System.EventHandler(this.addAnalysisPass_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.videoProfile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.videoInputLabel);
            this.groupBox1.Controls.Add(this.videoOutput);
            this.groupBox1.Controls.Add(this.videoInput);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.videoOutputLabel);
            this.groupBox1.Controls.Add(this.addAnalysisPass);
            this.groupBox1.Controls.Add(this.addPrerenderJob);
            this.groupBox1.Controls.Add(this.queueVideoButton);
            this.groupBox1.Controls.Add(this.fileType);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(490, 168);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Video encoding";
            // 
            // videoProfile
            // 
            this.videoProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoProfile.Location = new System.Drawing.Point(113, 79);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.ProfileSet = "Video";
            this.videoProfile.Size = new System.Drawing.Size(371, 22);
            this.videoProfile.TabIndex = 12;
            this.videoProfile.SelectedProfileChanged += new System.EventHandler(this.videoProfile_SelectedProfileChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "File format";
            // 
            // videoOutput
            // 
            this.videoOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoOutput.Filename = "";
            this.videoOutput.Filter = null;
            this.videoOutput.FolderMode = false;
            this.videoOutput.Location = new System.Drawing.Point(113, 48);
            this.videoOutput.Name = "videoOutput";
            this.videoOutput.ReadOnly = false;
            this.videoOutput.SaveMode = true;
            this.videoOutput.Size = new System.Drawing.Size(371, 29);
            this.videoOutput.TabIndex = 3;
            this.videoOutput.Title = "Enter name of output";
            this.videoOutput.FileSelected += new MeGUI.FileBarEventHandler(this.videoOutput_FileSelected);
            // 
            // videoInput
            // 
            this.videoInput.AllowDrop = true;
            this.videoInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoInput.Filename = "";
            this.videoInput.Filter = "AviSynth files (*.avs)|*.avs|All files (*.*)|*.*";
            this.videoInput.FolderMode = false;
            this.videoInput.Location = new System.Drawing.Point(113, 20);
            this.videoInput.Name = "videoInput";
            this.videoInput.ReadOnly = true;
            this.videoInput.SaveMode = false;
            this.videoInput.Size = new System.Drawing.Size(371, 29);
            this.videoInput.TabIndex = 1;
            this.videoInput.Title = "Open AviSynth script";
            this.videoInput.FileSelected += new MeGUI.FileBarEventHandler(this.videoInput_FileSelected);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Encoder settings";
            // 
            // VideoEncodingComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "VideoEncodingComponent";
            this.Size = new System.Drawing.Size(490, 168);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label videoOutputLabel;
        private System.Windows.Forms.ComboBox fileType;
        private System.Windows.Forms.Label videoInputLabel;
        private System.Windows.Forms.Button queueVideoButton;
        private System.Windows.Forms.CheckBox addPrerenderJob;
        private System.Windows.Forms.Button addAnalysisPass;
        private FileBar videoInput;
        private FileBar videoOutput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private MeGUI.core.gui.ConfigableProfilesControl videoProfile;
        private System.Windows.Forms.Label label2;

    }
}
