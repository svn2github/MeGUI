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
            this.profileControl1 = new MeGUI.core.details.video.ProfileControl();
            this.audioOutput = new MeGUI.FileBar();
            this.audioInput = new MeGUI.FileBar();
            this.audioContainer = new System.Windows.Forms.ComboBox();
            this.audioContainerLabel = new System.Windows.Forms.Label();
            this.audioCodecLabel = new System.Windows.Forms.Label();
            this.queueAudioButton = new System.Windows.Forms.Button();
            this.audioInputLabel = new System.Windows.Forms.Label();
            this.audioOutputLabel = new System.Windows.Forms.Label();
            this.audioTrack2 = new System.Windows.Forms.RadioButton();
            this.audioTrack1 = new System.Windows.Forms.RadioButton();
            this.deleteAudioButton = new System.Windows.Forms.Button();
            this.audioCodec = new System.Windows.Forms.ComboBox();
            this.audioIOGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // audioIOGroupBox
            // 
            this.audioIOGroupBox.Controls.Add(this.profileControl1);
            this.audioIOGroupBox.Controls.Add(this.audioOutput);
            this.audioIOGroupBox.Controls.Add(this.audioInput);
            this.audioIOGroupBox.Controls.Add(this.audioContainer);
            this.audioIOGroupBox.Controls.Add(this.audioContainerLabel);
            this.audioIOGroupBox.Controls.Add(this.audioCodecLabel);
            this.audioIOGroupBox.Controls.Add(this.queueAudioButton);
            this.audioIOGroupBox.Controls.Add(this.audioInputLabel);
            this.audioIOGroupBox.Controls.Add(this.audioOutputLabel);
            this.audioIOGroupBox.Controls.Add(this.audioTrack2);
            this.audioIOGroupBox.Controls.Add(this.audioTrack1);
            this.audioIOGroupBox.Controls.Add(this.deleteAudioButton);
            this.audioIOGroupBox.Controls.Add(this.audioCodec);
            this.audioIOGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioIOGroupBox.Location = new System.Drawing.Point(0, 0);
            this.audioIOGroupBox.Name = "audioIOGroupBox";
            this.audioIOGroupBox.Size = new System.Drawing.Size(466, 162);
            this.audioIOGroupBox.TabIndex = 5;
            this.audioIOGroupBox.TabStop = false;
            this.audioIOGroupBox.Text = "Audio";
            // 
            // profileControl1
            // 
            this.profileControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profileControl1.LabelText = "Audio profile";
            this.profileControl1.Location = new System.Drawing.Point(11, 97);
            this.profileControl1.Name = "profileControl1";
            this.profileControl1.Size = new System.Drawing.Size(449, 29);
            this.profileControl1.TabIndex = 36;
            // 
            // audioOutput
            // 
            this.audioOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioOutput.Filename = "";
            this.audioOutput.Filter = null;
            this.audioOutput.FolderMode = false;
            this.audioOutput.Location = new System.Drawing.Point(115, 41);
            this.audioOutput.Name = "audioOutput";
            this.audioOutput.ReadOnly = false;
            this.audioOutput.SaveMode = true;
            this.audioOutput.Size = new System.Drawing.Size(348, 29);
            this.audioOutput.TabIndex = 34;
            this.audioOutput.Title = "Enter name of output";
            // 
            // audioInput
            // 
            this.audioInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.audioInput.Filename = "";
            this.audioInput.Filter = null;
            this.audioInput.FolderMode = false;
            this.audioInput.Location = new System.Drawing.Point(115, 16);
            this.audioInput.Name = "audioInput";
            this.audioInput.ReadOnly = true;
            this.audioInput.SaveMode = false;
            this.audioInput.Size = new System.Drawing.Size(348, 29);
            this.audioInput.TabIndex = 33;
            this.audioInput.Title = "Select your audio input";
            this.audioInput.FileSelected += new MeGUI.FileBarEventHandler(this.audioInput_FileSelected);
            // 
            // audioContainer
            // 
            this.audioContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audioContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioContainer.FormattingEnabled = true;
            this.audioContainer.Location = new System.Drawing.Point(330, 73);
            this.audioContainer.Name = "audioContainer";
            this.audioContainer.Size = new System.Drawing.Size(103, 21);
            this.audioContainer.TabIndex = 7;
            this.audioContainer.SelectedIndexChanged += new System.EventHandler(this.audioContainer_SelectedIndexChanged);
            // 
            // audioContainerLabel
            // 
            this.audioContainerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.audioContainerLabel.AutoSize = true;
            this.audioContainerLabel.Location = new System.Drawing.Point(272, 76);
            this.audioContainerLabel.Name = "audioContainerLabel";
            this.audioContainerLabel.Size = new System.Drawing.Size(52, 13);
            this.audioContainerLabel.TabIndex = 32;
            this.audioContainerLabel.Text = "Container";
            // 
            // audioCodecLabel
            // 
            this.audioCodecLabel.AutoSize = true;
            this.audioCodecLabel.Location = new System.Drawing.Point(8, 76);
            this.audioCodecLabel.Name = "audioCodecLabel";
            this.audioCodecLabel.Size = new System.Drawing.Size(38, 13);
            this.audioCodecLabel.TabIndex = 31;
            this.audioCodecLabel.Text = "Codec";
            this.audioCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // queueAudioButton
            // 
            this.queueAudioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.queueAudioButton.AutoSize = true;
            this.queueAudioButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.queueAudioButton.Location = new System.Drawing.Point(400, 133);
            this.queueAudioButton.Name = "queueAudioButton";
            this.queueAudioButton.Size = new System.Drawing.Size(60, 23);
            this.queueAudioButton.TabIndex = 30;
            this.queueAudioButton.Text = "Enqueue";
            this.queueAudioButton.Click += new System.EventHandler(this.queueAudioButton_Click);
            // 
            // audioInputLabel
            // 
            this.audioInputLabel.AutoSize = true;
            this.audioInputLabel.Location = new System.Drawing.Point(8, 19);
            this.audioInputLabel.Name = "audioInputLabel";
            this.audioInputLabel.Size = new System.Drawing.Size(61, 13);
            this.audioInputLabel.TabIndex = 5;
            this.audioInputLabel.Text = "Audio Input";
            // 
            // audioOutputLabel
            // 
            this.audioOutputLabel.AutoSize = true;
            this.audioOutputLabel.Location = new System.Drawing.Point(9, 44);
            this.audioOutputLabel.Name = "audioOutputLabel";
            this.audioOutputLabel.Size = new System.Drawing.Size(69, 13);
            this.audioOutputLabel.TabIndex = 9;
            this.audioOutputLabel.Text = "Audio Output";
            // 
            // audioTrack2
            // 
            this.audioTrack2.AutoSize = true;
            this.audioTrack2.Location = new System.Drawing.Point(75, 0);
            this.audioTrack2.Name = "audioTrack2";
            this.audioTrack2.Size = new System.Drawing.Size(31, 17);
            this.audioTrack2.TabIndex = 20;
            this.audioTrack2.Text = "2";
            this.audioTrack2.CheckedChanged += new System.EventHandler(this.audioTrack_CheckedChanged);
            // 
            // audioTrack1
            // 
            this.audioTrack1.AutoSize = true;
            this.audioTrack1.Checked = true;
            this.audioTrack1.Location = new System.Drawing.Point(48, 0);
            this.audioTrack1.Name = "audioTrack1";
            this.audioTrack1.Size = new System.Drawing.Size(31, 17);
            this.audioTrack1.TabIndex = 19;
            this.audioTrack1.TabStop = true;
            this.audioTrack1.Text = "1";
            this.audioTrack1.CheckedChanged += new System.EventHandler(this.audioTrack_CheckedChanged);
            // 
            // deleteAudioButton
            // 
            this.deleteAudioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteAudioButton.Location = new System.Drawing.Point(439, 73);
            this.deleteAudioButton.Name = "deleteAudioButton";
            this.deleteAudioButton.Size = new System.Drawing.Size(24, 23);
            this.deleteAudioButton.TabIndex = 6;
            this.deleteAudioButton.Text = "X";
            this.deleteAudioButton.Click += new System.EventHandler(this.deleteAudioButton_Click);
            // 
            // audioCodec
            // 
            this.audioCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioCodec.Location = new System.Drawing.Point(115, 73);
            this.audioCodec.Name = "audioCodec";
            this.audioCodec.Size = new System.Drawing.Size(95, 21);
            this.audioCodec.TabIndex = 7;
            // 
            // AudioEncodingComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.audioIOGroupBox);
            this.Name = "AudioEncodingComponent";
            this.Size = new System.Drawing.Size(466, 162);
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
        private System.Windows.Forms.Label audioInputLabel;
        private System.Windows.Forms.Label audioOutputLabel;
        private System.Windows.Forms.RadioButton audioTrack2;
        private System.Windows.Forms.RadioButton audioTrack1;
        private System.Windows.Forms.Button deleteAudioButton;
        private System.Windows.Forms.ComboBox audioCodec;
        private FileBar audioOutput;
        private FileBar audioInput;
        private MeGUI.core.details.video.ProfileControl profileControl1;
    }
}
