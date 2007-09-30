namespace MeGUI.packages.tools.oneclick
{
    partial class AudioConfigControl
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
            this.audioCodecLabel = new System.Windows.Forms.Label();
            this.dontEncodeAudio = new System.Windows.Forms.CheckBox();
            this.audioCodec = new System.Windows.Forms.ComboBox();
            this.audioProfileControl = new MeGUI.core.details.video.ProfileControl();
            this.SuspendLayout();
            // 
            // audioCodecLabel
            // 
            this.audioCodecLabel.AutoSize = true;
            this.audioCodecLabel.Location = new System.Drawing.Point(3, 7);
            this.audioCodecLabel.Name = "audioCodecLabel";
            this.audioCodecLabel.Size = new System.Drawing.Size(38, 13);
            this.audioCodecLabel.TabIndex = 39;
            this.audioCodecLabel.Text = "Codec";
            this.audioCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dontEncodeAudio
            // 
            this.dontEncodeAudio.Location = new System.Drawing.Point(288, 4);
            this.dontEncodeAudio.Name = "dontEncodeAudio";
            this.dontEncodeAudio.Size = new System.Drawing.Size(122, 21);
            this.dontEncodeAudio.TabIndex = 38;
            this.dontEncodeAudio.Text = "Keep original track";
            this.dontEncodeAudio.CheckedChanged += new System.EventHandler(this.dontEncodeAudio_CheckedChanged);
            // 
            // audioCodec
            // 
            this.audioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioCodec.Location = new System.Drawing.Point(109, 4);
            this.audioCodec.Name = "audioCodec";
            this.audioCodec.Size = new System.Drawing.Size(173, 21);
            this.audioCodec.TabIndex = 36;
            // 
            // audioProfileControl
            // 
            this.audioProfileControl.LabelText = "Audio Profile";
            this.audioProfileControl.Location = new System.Drawing.Point(3, 29);
            this.audioProfileControl.Name = "audioProfileControl";
            this.audioProfileControl.Size = new System.Drawing.Size(407, 29);
            this.audioProfileControl.TabIndex = 41;
            // 
            // AudioConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.audioProfileControl);
            this.Controls.Add(this.audioCodecLabel);
            this.Controls.Add(this.dontEncodeAudio);
            this.Controls.Add(this.audioCodec);
            this.Name = "AudioConfigControl";
            this.Size = new System.Drawing.Size(415, 58);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MeGUI.core.details.video.ProfileControl audioProfileControl;
        private System.Windows.Forms.Label audioCodecLabel;
        private System.Windows.Forms.CheckBox dontEncodeAudio;
        private System.Windows.Forms.ComboBox audioCodec;
    }
}
