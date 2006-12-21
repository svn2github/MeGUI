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
            this.improvedAccuracy = new System.Windows.Forms.CheckBox();
            this.forceDShowDecoding = new System.Windows.Forms.CheckBox();
            this.negativeDelay = new System.Windows.Forms.CheckBox();
            this.autoGain = new System.Windows.Forms.CheckBox();
            this.besweetDelay = new System.Windows.Forms.TextBox();
            this.besweetDelayCorrection = new System.Windows.Forms.CheckBox();
            this.besweetDownmixMode = new System.Windows.Forms.ComboBox();
            this.besweetDelayLabel = new System.Windows.Forms.Label();
            this.BesweetChannelsLabel = new System.Windows.Forms.Label();
            this.besweetOptionsGroupbox.SuspendLayout();
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
            this.besweetOptionsGroupbox.Controls.Add(this.improvedAccuracy);
            this.besweetOptionsGroupbox.Controls.Add(this.forceDShowDecoding);
            this.besweetOptionsGroupbox.Controls.Add(this.negativeDelay);
            this.besweetOptionsGroupbox.Controls.Add(this.autoGain);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDelay);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDelayCorrection);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDownmixMode);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDelayLabel);
            this.besweetOptionsGroupbox.Controls.Add(this.BesweetChannelsLabel);
            this.besweetOptionsGroupbox.Location = new System.Drawing.Point(0, 3);
            this.besweetOptionsGroupbox.Name = "besweetOptionsGroupbox";
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(394, 149);
            this.besweetOptionsGroupbox.TabIndex = 8;
            this.besweetOptionsGroupbox.TabStop = false;
            this.besweetOptionsGroupbox.Text = "Audio Options";
            // 
            // improvedAccuracy
            // 
            this.improvedAccuracy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.improvedAccuracy.AutoSize = true;
            this.improvedAccuracy.Location = new System.Drawing.Point(16, 93);
            this.improvedAccuracy.Name = "improvedAccuracy";
            this.improvedAccuracy.Size = new System.Drawing.Size(267, 17);
            this.improvedAccuracy.TabIndex = 12;
            this.improvedAccuracy.Text = "Improve Accuracy using 32bit && Float computations";
            this.improvedAccuracy.UseVisualStyleBackColor = true;
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
            // negativeDelay
            // 
            this.negativeDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.negativeDelay.Enabled = false;
            this.negativeDelay.Location = new System.Drawing.Point(168, 118);
            this.negativeDelay.Name = "negativeDelay";
            this.negativeDelay.Size = new System.Drawing.Size(32, 24);
            this.negativeDelay.TabIndex = 7;
            this.negativeDelay.Text = "-";
            // 
            // autoGain
            // 
            this.autoGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoGain.Location = new System.Drawing.Point(16, 63);
            this.autoGain.Name = "autoGain";
            this.autoGain.Size = new System.Drawing.Size(184, 24);
            this.autoGain.TabIndex = 6;
            this.autoGain.Text = "Increase Volume automatically";
            // 
            // besweetDelay
            // 
            this.besweetDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.besweetDelay.Enabled = false;
            this.besweetDelay.Location = new System.Drawing.Point(200, 119);
            this.besweetDelay.Name = "besweetDelay";
            this.besweetDelay.Size = new System.Drawing.Size(48, 20);
            this.besweetDelay.TabIndex = 5;
            this.besweetDelay.Text = "0";
            // 
            // besweetDelayCorrection
            // 
            this.besweetDelayCorrection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.besweetDelayCorrection.Location = new System.Drawing.Point(16, 123);
            this.besweetDelayCorrection.Name = "besweetDelayCorrection";
            this.besweetDelayCorrection.Size = new System.Drawing.Size(112, 16);
            this.besweetDelayCorrection.TabIndex = 4;
            this.besweetDelayCorrection.Text = "Delay Correction";
            this.besweetDelayCorrection.CheckedChanged += new System.EventHandler(this.besweetDelayCorrection_CheckedChanged);
            // 
            // besweetDownmixMode
            // 
            this.besweetDownmixMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.besweetDownmixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.besweetDownmixMode.Location = new System.Drawing.Point(107, 39);
            this.besweetDownmixMode.Name = "besweetDownmixMode";
            this.besweetDownmixMode.Size = new System.Drawing.Size(278, 21);
            this.besweetDownmixMode.TabIndex = 3;
            // 
            // besweetDelayLabel
            // 
            this.besweetDelayLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.besweetDelayLabel.Location = new System.Drawing.Point(256, 121);
            this.besweetDelayLabel.Name = "besweetDelayLabel";
            this.besweetDelayLabel.Size = new System.Drawing.Size(24, 23);
            this.besweetDelayLabel.TabIndex = 2;
            this.besweetDelayLabel.Text = "ms";
            // 
            // BesweetChannelsLabel
            // 
            this.BesweetChannelsLabel.Location = new System.Drawing.Point(13, 42);
            this.BesweetChannelsLabel.Name = "BesweetChannelsLabel";
            this.BesweetChannelsLabel.Size = new System.Drawing.Size(96, 16);
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
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox encoderGroupBox;
        private System.Windows.Forms.CheckBox improvedAccuracy;
        private System.Windows.Forms.CheckBox forceDShowDecoding;
        private System.Windows.Forms.CheckBox negativeDelay;
        private System.Windows.Forms.CheckBox autoGain;
        private System.Windows.Forms.TextBox besweetDelay;
        private System.Windows.Forms.CheckBox besweetDelayCorrection;
        private System.Windows.Forms.ComboBox besweetDownmixMode;
        private System.Windows.Forms.Label besweetDelayLabel;
        private System.Windows.Forms.Label BesweetChannelsLabel;
        protected System.Windows.Forms.GroupBox besweetOptionsGroupbox;

    }
}
