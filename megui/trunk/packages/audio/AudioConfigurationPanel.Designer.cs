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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.normalize = new System.Windows.Forms.NumericUpDown();
            this.applyDRC = new System.Windows.Forms.CheckBox();
            this.lbSampleRate = new System.Windows.Forms.Label();
            this.besweetDownmixMode = new System.Windows.Forms.ComboBox();
            this.autoGain = new System.Windows.Forms.CheckBox();
            this.BesweetChannelsLabel = new System.Windows.Forms.Label();
            this.primaryDecoding = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSampleRate = new System.Windows.Forms.ComboBox();
            this.besweetOptionsGroupbox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.besweetOptionsGroupbox.Controls.Add(this.tableLayoutPanel1);
            this.besweetOptionsGroupbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.besweetOptionsGroupbox.Location = new System.Drawing.Point(0, 0);
            this.besweetOptionsGroupbox.Name = "besweetOptionsGroupbox";
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(394, 149);
            this.besweetOptionsGroupbox.TabIndex = 8;
            this.besweetOptionsGroupbox.TabStop = false;
            this.besweetOptionsGroupbox.Text = "Audio Options";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cbSampleRate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.normalize, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.autoGain, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.primaryDecoding, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.BesweetChannelsLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbSampleRate, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.applyDRC, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.besweetDownmixMode, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(388, 130);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // normalize
            // 
            this.normalize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.normalize.Location = new System.Drawing.Point(126, 107);
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
            this.applyDRC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.applyDRC.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.applyDRC, 2);
            this.applyDRC.Location = new System.Drawing.Point(3, 82);
            this.applyDRC.Name = "applyDRC";
            this.applyDRC.Size = new System.Drawing.Size(194, 17);
            this.applyDRC.TabIndex = 9;
            this.applyDRC.Text = "Apply Dynamic Range Compression";
            this.applyDRC.UseVisualStyleBackColor = true;
            this.applyDRC.CheckedChanged += new System.EventHandler(this.applyDRC_CheckedChanged);
            // 
            // lbSampleRate
            // 
            this.lbSampleRate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbSampleRate.AutoSize = true;
            this.lbSampleRate.Location = new System.Drawing.Point(3, 58);
            this.lbSampleRate.Name = "lbSampleRate";
            this.lbSampleRate.Size = new System.Drawing.Size(65, 13);
            this.lbSampleRate.TabIndex = 11;
            this.lbSampleRate.Text = "SampleRate";
            // 
            // besweetDownmixMode
            // 
            this.besweetDownmixMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.besweetDownmixMode, 2);
            this.besweetDownmixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.besweetDownmixMode.Location = new System.Drawing.Point(126, 29);
            this.besweetDownmixMode.Name = "besweetDownmixMode";
            this.besweetDownmixMode.Size = new System.Drawing.Size(259, 21);
            this.besweetDownmixMode.TabIndex = 3;
            // 
            // autoGain
            // 
            this.autoGain.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.autoGain.AutoSize = true;
            this.autoGain.Location = new System.Drawing.Point(3, 108);
            this.autoGain.Name = "autoGain";
            this.autoGain.Size = new System.Drawing.Size(117, 17);
            this.autoGain.TabIndex = 6;
            this.autoGain.Text = "Normalize Peaks to";
            this.autoGain.CheckedChanged += new System.EventHandler(this.autoGain_CheckedChanged);
            // 
            // BesweetChannelsLabel
            // 
            this.BesweetChannelsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BesweetChannelsLabel.AutoSize = true;
            this.BesweetChannelsLabel.Location = new System.Drawing.Point(3, 32);
            this.BesweetChannelsLabel.Name = "BesweetChannelsLabel";
            this.BesweetChannelsLabel.Size = new System.Drawing.Size(86, 13);
            this.BesweetChannelsLabel.TabIndex = 2;
            this.BesweetChannelsLabel.Text = "Output Channels";
            // 
            // primaryDecoding
            // 
            this.primaryDecoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.primaryDecoding.FormattingEnabled = true;
            this.primaryDecoding.Location = new System.Drawing.Point(126, 3);
            this.primaryDecoding.Name = "primaryDecoding";
            this.primaryDecoding.Size = new System.Drawing.Size(121, 21);
            this.primaryDecoding.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Preferred Decoder";
            // 
            // cbSampleRate
            // 
            this.cbSampleRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbSampleRate, 2);
            this.cbSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSampleRate.FormattingEnabled = true;
            this.cbSampleRate.Items.AddRange(new object[] {
            "Keep Original",
            "Change to   8000 Hz",
            "Change to 11025 Hz",
            "Change to 22050 Hz",
            "Change to 44100 Hz",
            "Change to 48000 Hz",
            "Speed-up (23.976 to 25)",
            "Slow-down (25 to 23.976)",
            "Speed-up (24 to 25)",
            "Slow-down (25 to 24)"});
            this.cbSampleRate.Location = new System.Drawing.Point(126, 55);
            this.cbSampleRate.Name = "cbSampleRate";
            this.cbSampleRate.Size = new System.Drawing.Size(259, 21);
            this.cbSampleRate.TabIndex = 15;
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox encoderGroupBox;
        private System.Windows.Forms.CheckBox autoGain;
        private System.Windows.Forms.ComboBox besweetDownmixMode;
        private System.Windows.Forms.Label BesweetChannelsLabel;
        protected System.Windows.Forms.GroupBox besweetOptionsGroupbox;
        private System.Windows.Forms.CheckBox applyDRC;
        private System.Windows.Forms.NumericUpDown normalize;
        private System.Windows.Forms.Label lbSampleRate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox primaryDecoding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSampleRate;

    }
}
