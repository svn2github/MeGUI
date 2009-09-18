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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.besweetOptionsGroupbox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.forceDShowDecoding = new System.Windows.Forms.CheckBox();
            this.normalize = new System.Windows.Forms.NumericUpDown();
            this.cbSampleRate = new System.Windows.Forms.ComboBox();
            this.applyDRC = new System.Windows.Forms.CheckBox();
            this.lbSampleRate = new System.Windows.Forms.Label();
            this.besweetDownmixMode = new System.Windows.Forms.ComboBox();
            this.autoGain = new System.Windows.Forms.CheckBox();
            this.BesweetChannelsLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.besweetOptionsGroupbox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalize)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(4, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(402, 315);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(394, 289);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Encoder";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.besweetOptionsGroupbox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(394, 289);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DSP";
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.besweetOptionsGroupbox.BackColor = System.Drawing.SystemColors.Control;
            this.besweetOptionsGroupbox.Controls.Add(this.tableLayoutPanel1);
            this.besweetOptionsGroupbox.Location = new System.Drawing.Point(5, 4);
            this.besweetOptionsGroupbox.Name = "besweetOptionsGroupbox";
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(385, 212);
            this.besweetOptionsGroupbox.TabIndex = 13;
            this.besweetOptionsGroupbox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.forceDShowDecoding, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.normalize, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbSampleRate, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.applyDRC, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbSampleRate, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.besweetDownmixMode, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.autoGain, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.BesweetChannelsLabel, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(374, 163);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // forceDShowDecoding
            // 
            this.forceDShowDecoding.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.forceDShowDecoding.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.forceDShowDecoding, 2);
            this.forceDShowDecoding.Location = new System.Drawing.Point(3, 7);
            this.forceDShowDecoding.Name = "forceDShowDecoding";
            this.forceDShowDecoding.Size = new System.Drawing.Size(177, 17);
            this.forceDShowDecoding.TabIndex = 8;
            this.forceDShowDecoding.Text = "Force Decoding via DirectShow";
            this.forceDShowDecoding.UseVisualStyleBackColor = true;
            // 
            // normalize
            // 
            this.normalize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.normalize.Location = new System.Drawing.Point(126, 135);
            this.normalize.Name = "normalize";
            this.normalize.Size = new System.Drawing.Size(52, 20);
            this.normalize.TabIndex = 10;
            this.normalize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
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
            "Slow-down (25 to 23.976)"});
            this.cbSampleRate.Location = new System.Drawing.Point(126, 101);
            this.cbSampleRate.Name = "cbSampleRate";
            this.cbSampleRate.Size = new System.Drawing.Size(245, 21);
            this.cbSampleRate.TabIndex = 12;
            // 
            // applyDRC
            // 
            this.applyDRC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.applyDRC.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.applyDRC, 2);
            this.applyDRC.Location = new System.Drawing.Point(3, 39);
            this.applyDRC.Name = "applyDRC";
            this.applyDRC.Size = new System.Drawing.Size(194, 17);
            this.applyDRC.TabIndex = 9;
            this.applyDRC.Text = "Apply Dynamic Range Compression";
            this.applyDRC.UseVisualStyleBackColor = true;
            // 
            // lbSampleRate
            // 
            this.lbSampleRate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbSampleRate.AutoSize = true;
            this.lbSampleRate.Location = new System.Drawing.Point(3, 105);
            this.lbSampleRate.Name = "lbSampleRate";
            this.lbSampleRate.Size = new System.Drawing.Size(71, 13);
            this.lbSampleRate.TabIndex = 11;
            this.lbSampleRate.Text = "SampleRate :";
            // 
            // besweetDownmixMode
            // 
            this.besweetDownmixMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.besweetDownmixMode, 2);
            this.besweetDownmixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.besweetDownmixMode.Location = new System.Drawing.Point(126, 69);
            this.besweetDownmixMode.Name = "besweetDownmixMode";
            this.besweetDownmixMode.Size = new System.Drawing.Size(245, 21);
            this.besweetDownmixMode.TabIndex = 3;
            // 
            // autoGain
            // 
            this.autoGain.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.autoGain.AutoSize = true;
            this.autoGain.Location = new System.Drawing.Point(3, 137);
            this.autoGain.Name = "autoGain";
            this.autoGain.Size = new System.Drawing.Size(117, 17);
            this.autoGain.TabIndex = 6;
            this.autoGain.Text = "Normalize Peaks to";
            // 
            // BesweetChannelsLabel
            // 
            this.BesweetChannelsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BesweetChannelsLabel.AutoSize = true;
            this.BesweetChannelsLabel.Location = new System.Drawing.Point(3, 73);
            this.BesweetChannelsLabel.Name = "BesweetChannelsLabel";
            this.BesweetChannelsLabel.Size = new System.Drawing.Size(92, 13);
            this.BesweetChannelsLabel.TabIndex = 2;
            this.BesweetChannelsLabel.Text = "Output Channels :";
            // 
            // AudioConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "AudioConfigurationPanel";
            this.Size = new System.Drawing.Size(409, 443);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.besweetOptionsGroupbox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        protected System.Windows.Forms.GroupBox besweetOptionsGroupbox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox forceDShowDecoding;
        private System.Windows.Forms.NumericUpDown normalize;
        private System.Windows.Forms.ComboBox cbSampleRate;
        private System.Windows.Forms.CheckBox applyDRC;
        private System.Windows.Forms.Label lbSampleRate;
        private System.Windows.Forms.ComboBox besweetDownmixMode;
        private System.Windows.Forms.CheckBox autoGain;
        private System.Windows.Forms.Label BesweetChannelsLabel;
        protected System.Windows.Forms.TabPage tabPage1;
        protected System.Windows.Forms.TabControl tabControl1;

    }
}
