// ****************************************************************************
// 
// Copyright (C) 2005-2015 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

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
            this.cbTimeModification = new System.Windows.Forms.ComboBox();
            this.cbSampleRate = new System.Windows.Forms.ComboBox();
            this.primaryDecoding = new System.Windows.Forms.ComboBox();
            this.cbDownmixMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BesweetChannelsLabel = new System.Windows.Forms.Label();
            this.lbSampleRate = new System.Windows.Forms.Label();
            this.autoGain = new System.Windows.Forms.CheckBox();
            this.normalize = new System.Windows.Forms.NumericUpDown();
            this.applyDRC = new System.Windows.Forms.CheckBox();
            this.lbTimeModification = new System.Windows.Forms.Label();
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
            this.encoderGroupBox.Location = new System.Drawing.Point(4, 184);
            this.encoderGroupBox.Name = "encoderGroupBox";
            this.encoderGroupBox.Size = new System.Drawing.Size(402, 100);
            this.encoderGroupBox.TabIndex = 9;
            this.encoderGroupBox.TabStop = false;
            this.encoderGroupBox.Text = "placeholder for encoder options";
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.Controls.Add(this.tableLayoutPanel1);
            this.besweetOptionsGroupbox.Location = new System.Drawing.Point(4, 0);
            this.besweetOptionsGroupbox.Name = "besweetOptionsGroupbox";
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(402, 178);
            this.besweetOptionsGroupbox.TabIndex = 8;
            this.besweetOptionsGroupbox.TabStop = false;
            this.besweetOptionsGroupbox.Text = " Audio Options ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cbTimeModification, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbSampleRate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.primaryDecoding, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbDownmixMode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.BesweetChannelsLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbSampleRate, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.autoGain, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.normalize, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.applyDRC, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbTimeModification, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(396, 159);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // cbTimeModification
            // 
            this.cbTimeModification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbTimeModification, 2);
            this.cbTimeModification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTimeModification.FormattingEnabled = true;
            this.cbTimeModification.Location = new System.Drawing.Point(103, 81);
            this.cbTimeModification.Name = "cbTimeModification";
            this.cbTimeModification.Size = new System.Drawing.Size(290, 21);
            this.cbTimeModification.TabIndex = 17;
            // 
            // cbSampleRate
            // 
            this.cbSampleRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbSampleRate, 2);
            this.cbSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSampleRate.FormattingEnabled = true;
            this.cbSampleRate.Location = new System.Drawing.Point(103, 55);
            this.cbSampleRate.Name = "cbSampleRate";
            this.cbSampleRate.Size = new System.Drawing.Size(290, 21);
            this.cbSampleRate.TabIndex = 15;
            // 
            // primaryDecoding
            // 
            this.primaryDecoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.primaryDecoding, 2);
            this.primaryDecoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.primaryDecoding.FormattingEnabled = true;
            this.primaryDecoding.Location = new System.Drawing.Point(103, 3);
            this.primaryDecoding.Name = "primaryDecoding";
            this.primaryDecoding.Size = new System.Drawing.Size(290, 21);
            this.primaryDecoding.TabIndex = 13;
            // 
            // cbDownmixMode
            // 
            this.cbDownmixMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbDownmixMode, 2);
            this.cbDownmixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDownmixMode.Location = new System.Drawing.Point(103, 29);
            this.cbDownmixMode.Name = "cbDownmixMode";
            this.cbDownmixMode.Size = new System.Drawing.Size(290, 21);
            this.cbDownmixMode.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Preferred Decoder";
            // 
            // BesweetChannelsLabel
            // 
            this.BesweetChannelsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BesweetChannelsLabel.AutoSize = true;
            this.BesweetChannelsLabel.Location = new System.Drawing.Point(3, 39);
            this.BesweetChannelsLabel.Name = "BesweetChannelsLabel";
            this.BesweetChannelsLabel.Size = new System.Drawing.Size(86, 13);
            this.BesweetChannelsLabel.TabIndex = 2;
            this.BesweetChannelsLabel.Text = "Output Channels";
            // 
            // lbSampleRate
            // 
            this.lbSampleRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbSampleRate.AutoSize = true;
            this.lbSampleRate.Location = new System.Drawing.Point(3, 65);
            this.lbSampleRate.Name = "lbSampleRate";
            this.lbSampleRate.Size = new System.Drawing.Size(68, 13);
            this.lbSampleRate.TabIndex = 11;
            this.lbSampleRate.Text = "Sample Rate";
            // 
            // autoGain
            // 
            this.autoGain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.autoGain.AutoSize = true;
            this.autoGain.Location = new System.Drawing.Point(103, 133);
            this.autoGain.Name = "autoGain";
            this.autoGain.Size = new System.Drawing.Size(117, 23);
            this.autoGain.TabIndex = 6;
            this.autoGain.Text = "Normalize Peaks to";
            this.autoGain.CheckedChanged += new System.EventHandler(this.autoGain_CheckedChanged);
            // 
            // normalize
            // 
            this.normalize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.normalize.Location = new System.Drawing.Point(226, 134);
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
            this.applyDRC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applyDRC.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.applyDRC, 2);
            this.applyDRC.Location = new System.Drawing.Point(103, 110);
            this.applyDRC.Name = "applyDRC";
            this.applyDRC.Size = new System.Drawing.Size(194, 17);
            this.applyDRC.TabIndex = 9;
            this.applyDRC.Text = "Apply Dynamic Range Compression";
            this.applyDRC.UseVisualStyleBackColor = true;
            this.applyDRC.CheckedChanged += new System.EventHandler(this.applyDRC_CheckedChanged);
            // 
            // lbTimeModification
            // 
            this.lbTimeModification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbTimeModification.AutoSize = true;
            this.lbTimeModification.Location = new System.Drawing.Point(3, 91);
            this.lbTimeModification.Name = "lbTimeModification";
            this.lbTimeModification.Size = new System.Drawing.Size(90, 13);
            this.lbTimeModification.TabIndex = 18;
            this.lbTimeModification.Text = "Time Modification";
            // 
            // AudioConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.encoderGroupBox);
            this.Controls.Add(this.besweetOptionsGroupbox);
            this.Name = "AudioConfigurationPanel";
            this.Size = new System.Drawing.Size(411, 287);
            this.besweetOptionsGroupbox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox encoderGroupBox;
        private System.Windows.Forms.CheckBox autoGain;
        private System.Windows.Forms.ComboBox cbDownmixMode;
        private System.Windows.Forms.Label BesweetChannelsLabel;
        protected System.Windows.Forms.GroupBox besweetOptionsGroupbox;
        private System.Windows.Forms.CheckBox applyDRC;
        private System.Windows.Forms.NumericUpDown normalize;
        private System.Windows.Forms.Label lbSampleRate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox primaryDecoding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSampleRate;
        private System.Windows.Forms.ComboBox cbTimeModification;
        private System.Windows.Forms.Label lbTimeModification;

    }
}