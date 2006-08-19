namespace MeGUI
{
    partial class AudioEncodingComponent
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
            this.audioIOGroupBox = new System.Windows.Forms.GroupBox();
            this.fileBar1 = new MeGUI.FileBar();
            this.audioOutput = new MeGUI.FileBar();
            this.audioInput = new MeGUI.FileBar();
            this.audioContainer = new System.Windows.Forms.ComboBox();
            this.audioContainerLabel = new System.Windows.Forms.Label();
            this.audioCodecLabel = new System.Windows.Forms.Label();
            this.queueAudioButton = new System.Windows.Forms.Button();
            this.audioProfile = new System.Windows.Forms.ComboBox();
            this.audioProfileLabel = new System.Windows.Forms.Label();
            this.audioInputLabel = new System.Windows.Forms.Label();
            this.audioOutputLabel = new System.Windows.Forms.Label();
            this.audioTrack2 = new System.Windows.Forms.RadioButton();
            this.audioTrack1 = new System.Windows.Forms.RadioButton();
            this.deleteAudioButton = new System.Windows.Forms.Button();
            this.configAudioButton = new System.Windows.Forms.Button();
            this.audioCodec = new System.Windows.Forms.ComboBox();
            this.audioIOGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // audioIOGroupBox
            // 
            this.audioIOGroupBox.Controls.Add(this.fileBar1);
            this.audioIOGroupBox.Controls.Add(this.audioOutput);
            this.audioIOGroupBox.Controls.Add(this.audioInput);
            this.audioIOGroupBox.Controls.Add(this.audioContainer);
            this.audioIOGroupBox.Controls.Add(this.audioContainerLabel);
            this.audioIOGroupBox.Controls.Add(this.audioCodecLabel);
            this.audioIOGroupBox.Controls.Add(this.queueAudioButton);
            this.audioIOGroupBox.Controls.Add(this.audioProfile);
            this.audioIOGroupBox.Controls.Add(this.audioProfileLabel);
            this.audioIOGroupBox.Controls.Add(this.audioInputLabel);
            this.audioIOGroupBox.Controls.Add(this.audioOutputLabel);
            this.audioIOGroupBox.Controls.Add(this.audioTrack2);
            this.audioIOGroupBox.Controls.Add(this.audioTrack1);
            this.audioIOGroupBox.Controls.Add(this.deleteAudioButton);
            this.audioIOGroupBox.Controls.Add(this.configAudioButton);
            this.audioIOGroupBox.Controls.Add(this.audioCodec);
            this.audioIOGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioIOGroupBox.Location = new System.Drawing.Point(0, 0);
            this.audioIOGroupBox.Name = "audioIOGroupBox";
            this.audioIOGroupBox.Size = new System.Drawing.Size(496, 162);
            this.audioIOGroupBox.TabIndex = 5;
            this.audioIOGroupBox.TabStop = false;
            this.audioIOGroupBox.Text = "Audio";
            // 
            // fileBar1
            // 
            this.fileBar1.Filename = "";
            this.fileBar1.Filter = null;
            this.fileBar1.FolderMode = false;
            this.fileBar1.Location = new System.Drawing.Point(592, 36);
            this.fileBar1.Name = "fileBar1";
            this.fileBar1.ReadOnly = true;
            this.fileBar1.SaveMode = false;
            this.fileBar1.Size = new System.Drawing.Size(269, 26);
            this.fileBar1.TabIndex = 35;
            this.fileBar1.Title = null;
            // 
            // audioOutput
            // 
            this.audioOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioOutput.Filename = "";
            this.audioOutput.Filter = null;
            this.audioOutput.FolderMode = false;
            this.audioOutput.Location = new System.Drawing.Point(152, 41);
            this.audioOutput.Name = "audioOutput";
            this.audioOutput.ReadOnly = false;
            this.audioOutput.SaveMode = false;
            this.audioOutput.Size = new System.Drawing.Size(338, 26);
            this.audioOutput.TabIndex = 34;
            this.audioOutput.Title = "Enter name of output";
            // 
            // audioInput
            // 
            this.audioInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioInput.Filename = "";
            this.audioInput.Filter = null;
            this.audioInput.FolderMode = false;
            this.audioInput.Location = new System.Drawing.Point(152, 16);
            this.audioInput.Name = "audioInput";
            this.audioInput.ReadOnly = true;
            this.audioInput.SaveMode = false;
            this.audioInput.Size = new System.Drawing.Size(338, 26);
            this.audioInput.TabIndex = 33;
            this.audioInput.Title = "Select your audio input";
            this.audioInput.FileSelected += new MeGUI.FileBarEventHandler(this.audioInput_FileSelected);
            // 
            // audioContainer
            // 
            this.audioContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audioContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioContainer.FormattingEnabled = true;
            this.audioContainer.Location = new System.Drawing.Point(401, 73);
            this.audioContainer.Name = "audioContainer";
            this.audioContainer.Size = new System.Drawing.Size(59, 21);
            this.audioContainer.TabIndex = 7;
            this.audioContainer.SelectedIndexChanged += new System.EventHandler(this.audioContainer_SelectedIndexChanged);
            // 
            // audioContainerLabel
            // 
            this.audioContainerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audioContainerLabel.AutoSize = true;
            this.audioContainerLabel.Location = new System.Drawing.Point(343, 77);
            this.audioContainerLabel.Name = "audioContainerLabel";
            this.audioContainerLabel.Size = new System.Drawing.Size(52, 13);
            this.audioContainerLabel.TabIndex = 32;
            this.audioContainerLabel.Text = "Container";
            // 
            // audioCodecLabel
            // 
            this.audioCodecLabel.Location = new System.Drawing.Point(8, 76);
            this.audioCodecLabel.Name = "audioCodecLabel";
            this.audioCodecLabel.Size = new System.Drawing.Size(100, 23);
            this.audioCodecLabel.TabIndex = 31;
            this.audioCodecLabel.Text = "Codec";
            this.audioCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // queueAudioButton
            // 
            this.queueAudioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.queueAudioButton.Location = new System.Drawing.Point(424, 101);
            this.queueAudioButton.Name = "queueAudioButton";
            this.queueAudioButton.Size = new System.Drawing.Size(66, 23);
            this.queueAudioButton.TabIndex = 30;
            this.queueAudioButton.Text = "Enqueue";
            this.queueAudioButton.Click += new System.EventHandler(this.queueAudioButton_Click);
            // 
            // audioProfile
            // 
            this.audioProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioProfile.Location = new System.Drawing.Point(152, 101);
            this.audioProfile.Name = "audioProfile";
            this.audioProfile.Size = new System.Drawing.Size(200, 21);
            this.audioProfile.Sorted = true;
            this.audioProfile.TabIndex = 15;
            this.audioProfile.SelectedIndexChanged += new System.EventHandler(this.audioProfile_SelectedIndexChanged);
            // 
            // audioProfileLabel
            // 
            this.audioProfileLabel.Location = new System.Drawing.Point(8, 102);
            this.audioProfileLabel.Name = "audioProfileLabel";
            this.audioProfileLabel.Size = new System.Drawing.Size(100, 23);
            this.audioProfileLabel.TabIndex = 14;
            this.audioProfileLabel.Text = "Audio Profile";
            // 
            // audioInputLabel
            // 
            this.audioInputLabel.Location = new System.Drawing.Point(8, 24);
            this.audioInputLabel.Name = "audioInputLabel";
            this.audioInputLabel.Size = new System.Drawing.Size(100, 23);
            this.audioInputLabel.TabIndex = 5;
            this.audioInputLabel.Text = "Audio Input";
            // 
            // audioOutputLabel
            // 
            this.audioOutputLabel.Location = new System.Drawing.Point(8, 50);
            this.audioOutputLabel.Name = "audioOutputLabel";
            this.audioOutputLabel.Size = new System.Drawing.Size(100, 23);
            this.audioOutputLabel.TabIndex = 9;
            this.audioOutputLabel.Text = "Audio Output";
            // 
            // audioTrack2
            // 
            this.audioTrack2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioTrack2.Location = new System.Drawing.Point(75, 0);
            this.audioTrack2.Name = "audioTrack2";
            this.audioTrack2.Size = new System.Drawing.Size(24, 16);
            this.audioTrack2.TabIndex = 20;
            this.audioTrack2.Text = "2";
            this.audioTrack2.CheckedChanged += new System.EventHandler(this.audioTrack_CheckedChanged);
            // 
            // audioTrack1
            // 
            this.audioTrack1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioTrack1.Checked = true;
            this.audioTrack1.Location = new System.Drawing.Point(48, 0);
            this.audioTrack1.Name = "audioTrack1";
            this.audioTrack1.Size = new System.Drawing.Size(24, 16);
            this.audioTrack1.TabIndex = 19;
            this.audioTrack1.TabStop = true;
            this.audioTrack1.Text = "1";
            this.audioTrack1.CheckedChanged += new System.EventHandler(this.audioTrack_CheckedChanged);
            // 
            // deleteAudioButton
            // 
            this.deleteAudioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteAudioButton.Location = new System.Drawing.Point(466, 73);
            this.deleteAudioButton.Name = "deleteAudioButton";
            this.deleteAudioButton.Size = new System.Drawing.Size(24, 23);
            this.deleteAudioButton.TabIndex = 6;
            this.deleteAudioButton.Text = "X";
            this.deleteAudioButton.Click += new System.EventHandler(this.deleteAudioButton_Click);
            // 
            // configAudioButton
            // 
            this.configAudioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.configAudioButton.Location = new System.Drawing.Point(281, 72);
            this.configAudioButton.Name = "configAudioButton";
            this.configAudioButton.Size = new System.Drawing.Size(56, 23);
            this.configAudioButton.TabIndex = 26;
            this.configAudioButton.Text = "Config";
            this.configAudioButton.Click += new System.EventHandler(this.configAudioButton_Click);
            // 
            // audioCodec
            // 
            this.audioCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioCodec.Location = new System.Drawing.Point(152, 74);
            this.audioCodec.Name = "audioCodec";
            this.audioCodec.Size = new System.Drawing.Size(123, 21);
            this.audioCodec.TabIndex = 7;
            this.audioCodec.SelectedIndexChanged += new System.EventHandler(this.audioCodec_SelectedIndexChanged);
            // 
            // AudioEncodingComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.audioIOGroupBox);
            this.Name = "AudioEncodingComponent";
            this.Size = new System.Drawing.Size(496, 162);
            this.audioIOGroupBox.ResumeLayout(false);
            this.audioIOGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox audioIOGroupBox;
        private System.Windows.Forms.ComboBox audioContainer;
        private System.Windows.Forms.Label audioContainerLabel;
        private System.Windows.Forms.Label audioCodecLabel;
        private System.Windows.Forms.Button queueAudioButton;
        private System.Windows.Forms.ComboBox audioProfile;
        private System.Windows.Forms.Label audioProfileLabel;
        private System.Windows.Forms.Label audioInputLabel;
        private System.Windows.Forms.Label audioOutputLabel;
        private System.Windows.Forms.RadioButton audioTrack2;
        private System.Windows.Forms.RadioButton audioTrack1;
        private System.Windows.Forms.Button deleteAudioButton;
        private System.Windows.Forms.Button configAudioButton;
        private System.Windows.Forms.ComboBox audioCodec;
        private FileBar audioOutput;
        private FileBar audioInput;
        private FileBar fileBar1;
    }
}
