namespace MeGUI.core.details.audio
{
    partial class AudioConfigurationPanel
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
            this.encoderGroupBox = new System.Windows.Forms.GroupBox();
            this.besweetOptionsGroupbox = new System.Windows.Forms.GroupBox();
            this.cbSampleRate = new System.Windows.Forms.ComboBox();
            this.lbSampleRate = new System.Windows.Forms.Label();
            this.normalize = new System.Windows.Forms.NumericUpDown();
            this.applyDRC = new System.Windows.Forms.CheckBox();
            this.forceDShowDecoding = new System.Windows.Forms.CheckBox();
            this.autoGain = new System.Windows.Forms.CheckBox();
            this.besweetDownmixMode = new System.Windows.Forms.ComboBox();
            this.BesweetChannelsLabel = new System.Windows.Forms.Label();
            this.besweetOptionsGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalize)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.encoderGroupBox.Location = new System.Drawing.Point(1, 209);
            this.encoderGroupBox.Name = "encoderGroupBox";
            this.encoderGroupBox.Size = new System.Drawing.Size(390, 68);
            this.encoderGroupBox.TabIndex = 9;
            this.encoderGroupBox.TabStop = false;
            this.encoderGroupBox.Text = "placeholder for encoder options";
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.besweetOptionsGroupbox.Controls.Add(this.cbSampleRate);
            this.besweetOptionsGroupbox.Controls.Add(this.lbSampleRate);
            this.besweetOptionsGroupbox.Controls.Add(this.normalize);
            this.besweetOptionsGroupbox.Controls.Add(this.applyDRC);
            this.besweetOptionsGroupbox.Controls.Add(this.forceDShowDecoding);
            this.besweetOptionsGroupbox.Controls.Add(this.autoGain);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDownmixMode);
            this.besweetOptionsGroupbox.Controls.Add(this.BesweetChannelsLabel);
            this.besweetOptionsGroupbox.Location = new System.Drawing.Point(0, 3);
            this.besweetOptionsGroupbox.Name = "besweetOptionsGroupbox";
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(394, 149);
            this.besweetOptionsGroupbox.TabIndex = 8;
            this.besweetOptionsGroupbox.TabStop = false;
            this.besweetOptionsGroupbox.Text = "Audio Options";
            // 
            // cbSampleRate
            // 
            this.cbSampleRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSampleRate.FormattingEnabled = true;
            this.cbSampleRate.Items.AddRange(new object[] {
            "Keep Original",
            "Change to 44100 Hz",
            "Change to 48000 Hz",
            "Speed-up (23.976 to 25)",
            "Slow-down (25 to 23.976)"});
            this.cbSampleRate.Location = new System.Drawing.Point(107, 92);
            this.cbSampleRate.Name = "cbSampleRate";
            this.cbSampleRate.Size = new System.Drawing.Size(278, 21);
            this.cbSampleRate.TabIndex = 12;
            // 
            // lbSampleRate
            // 
            this.lbSampleRate.AutoSize = true;
            this.lbSampleRate.Location = new System.Drawing.Point(13, 95);
            this.lbSampleRate.Name = "lbSampleRate";
            this.lbSampleRate.Size = new System.Drawing.Size(65, 13);
            this.lbSampleRate.TabIndex = 11;
            this.lbSampleRate.Text = "SampleRate";
            // 
            // normalize
            // 
            this.normalize.Location = new System.Drawing.Point(139, 122);
            this.normalize.Name = "normalize";
            this.normalize.Size = new System.Drawing.Size(52, 20);
            this.normalize.TabIndex = 10;
            this.normalize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // applyDRC
            // 
            this.applyDRC.AutoSize = true;
            this.applyDRC.Location = new System.Drawing.Point(16, 39);
            this.applyDRC.Name = "applyDRC";
            this.applyDRC.Size = new System.Drawing.Size(194, 17);
            this.applyDRC.TabIndex = 9;
            this.applyDRC.Text = "Apply Dynamic Range Compression";
            this.applyDRC.UseVisualStyleBackColor = true;
            this.applyDRC.CheckedChanged += new System.EventHandler(this.applyDRC_CheckedChanged);
            // 
            // forceDShowDecoding
            // 
            this.forceDShowDecoding.AutoSize = true;
            this.forceDShowDecoding.Location = new System.Drawing.Point(16, 16);
            this.forceDShowDecoding.Name = "forceDShowDecoding";
            this.forceDShowDecoding.Size = new System.Drawing.Size(177, 17);
            this.forceDShowDecoding.TabIndex = 8;
            this.forceDShowDecoding.Text = "Force Decoding via DirectShow";
            this.forceDShowDecoding.UseVisualStyleBackColor = true;
            // 
            // autoGain
            // 
            this.autoGain.AutoSize = true;
            this.autoGain.Location = new System.Drawing.Point(16, 123);
            this.autoGain.Name = "autoGain";
            this.autoGain.Size = new System.Drawing.Size(117, 17);
            this.autoGain.TabIndex = 6;
            this.autoGain.Text = "Normalize Peaks to";
            this.autoGain.CheckedChanged += new System.EventHandler(this.autoGain_CheckedChanged);
            // 
            // besweetDownmixMode
            // 
            this.besweetDownmixMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.besweetDownmixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.besweetDownmixMode.Location = new System.Drawing.Point(107, 66);
            this.besweetDownmixMode.Name = "besweetDownmixMode";
            this.besweetDownmixMode.Size = new System.Drawing.Size(278, 21);
            this.besweetDownmixMode.TabIndex = 3;
            // 
            // BesweetChannelsLabel
            // 
            this.BesweetChannelsLabel.AutoSize = true;
            this.BesweetChannelsLabel.Location = new System.Drawing.Point(13, 69);
            this.BesweetChannelsLabel.Name = "BesweetChannelsLabel";
            this.BesweetChannelsLabel.Size = new System.Drawing.Size(86, 13);
            this.BesweetChannelsLabel.TabIndex = 2;
            this.BesweetChannelsLabel.Text = "Output Channels";
            // 
            // AudioConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.encoderGroupBox);
            this.Controls.Add(this.besweetOptionsGroupbox);
            this.Name = "AudioConfigurationPanel";
            this.Size = new System.Drawing.Size(394, 280);
            this.besweetOptionsGroupbox.ResumeLayout(false);
            this.besweetOptionsGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox encoderGroupBox;
        private System.Windows.Forms.CheckBox forceDShowDecoding;
        private System.Windows.Forms.CheckBox autoGain;
        private System.Windows.Forms.ComboBox besweetDownmixMode;
        private System.Windows.Forms.Label BesweetChannelsLabel;
        protected System.Windows.Forms.GroupBox besweetOptionsGroupbox;
        private System.Windows.Forms.CheckBox applyDRC;
        private System.Windows.Forms.NumericUpDown normalize;
        private System.Windows.Forms.ComboBox cbSampleRate;
        private System.Windows.Forms.Label lbSampleRate;

    }
}
