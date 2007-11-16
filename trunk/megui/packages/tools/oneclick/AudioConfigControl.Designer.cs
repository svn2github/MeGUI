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
            this.label3 = new System.Windows.Forms.Label();
            this.delay = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.delay)).BeginInit();
            this.SuspendLayout();
            // 
            // audioCodecLabel
            // 
            this.audioCodecLabel.AutoSize = true;
            this.audioCodecLabel.Location = new System.Drawing.Point(3, 8);
            this.audioCodecLabel.Name = "audioCodecLabel";
            this.audioCodecLabel.Size = new System.Drawing.Size(38, 13);
            this.audioCodecLabel.TabIndex = 39;
            this.audioCodecLabel.Text = "Codec";
            this.audioCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dontEncodeAudio
            // 
            this.dontEncodeAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dontEncodeAudio.AutoSize = true;
            this.dontEncodeAudio.Location = new System.Drawing.Point(288, 6);
            this.dontEncodeAudio.Name = "dontEncodeAudio";
            this.dontEncodeAudio.Size = new System.Drawing.Size(114, 17);
            this.dontEncodeAudio.TabIndex = 38;
            this.dontEncodeAudio.Text = "Keep original track";
            this.dontEncodeAudio.CheckedChanged += new System.EventHandler(this.dontEncodeAudio_CheckedChanged);
            // 
            // audioCodec
            // 
            this.audioCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioCodec.Location = new System.Drawing.Point(107, 4);
            this.audioCodec.Name = "audioCodec";
            this.audioCodec.Size = new System.Drawing.Size(173, 21);
            this.audioCodec.Sorted = true;
            this.audioCodec.TabIndex = 36;
            // 
            // audioProfileControl
            // 
            this.audioProfileControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioProfileControl.LabelText = "Audio Profile";
            this.audioProfileControl.Location = new System.Drawing.Point(3, 29);
            this.audioProfileControl.Name = "audioProfileControl";
            this.audioProfileControl.Size = new System.Drawing.Size(407, 29);
            this.audioProfileControl.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(249, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "ms";
            // 
            // delay
            // 
            this.delay.Location = new System.Drawing.Point(107, 64);
            this.delay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.delay.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.delay.Name = "delay";
            this.delay.Size = new System.Drawing.Size(136, 20);
            this.delay.TabIndex = 43;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Delay";
            // 
            // AudioConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.delay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.audioProfileControl);
            this.Controls.Add(this.audioCodecLabel);
            this.Controls.Add(this.dontEncodeAudio);
            this.Controls.Add(this.audioCodec);
            this.Name = "AudioConfigControl";
            this.Size = new System.Drawing.Size(416, 90);
            ((System.ComponentModel.ISupportInitialize)(this.delay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MeGUI.core.details.video.ProfileControl audioProfileControl;
        private System.Windows.Forms.Label audioCodecLabel;
        private System.Windows.Forms.CheckBox dontEncodeAudio;
        private System.Windows.Forms.ComboBox audioCodec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown delay;
        private System.Windows.Forms.Label label2;
    }
}
