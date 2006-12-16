// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
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

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace MeGUI
{
    /// <summary>
    /// Summary description for WinAmpAACConfigurationDialog.
    /// </summary>
    public class WinAmpAACConfigurationDialog : baseAudioConfigurationDialog
    {
        private Label label1;
        private Label label2;
        private ComboBox comboBox2;
        private CheckBox checkBox2;
        private Label label3;
        public TrackBar vBitrate;
        private ComboBox comboBox1;
        #region variables

        #endregion
        #region start / stop
        public WinAmpAACConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings,
            string initialProfile)
            : base(profileManager, path, settings, initialProfile)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            performSizeAndLayoutCorrection();
            comboBox1.Items.AddRange(EnumProxy.CreateArray( WinAmpAACSettings.SupportedProfiles));
            comboBox2.Items.AddRange(EnumProxy.CreateArray(typeof(WinAmpAACSettings.AacStereoMode)));
            vBitrate_ValueChanged(null, null);
        }


        #endregion
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.vBitrate = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vBitrate)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.label3);
            this.encoderGroupBox.Controls.Add(this.vBitrate);
            this.encoderGroupBox.Controls.Add(this.checkBox2);
            this.encoderGroupBox.Controls.Add(this.label2);
            this.encoderGroupBox.Controls.Add(this.comboBox2);
            this.encoderGroupBox.Controls.Add(this.label1);
            this.encoderGroupBox.Controls.Add(this.comboBox1);
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 172);
            this.encoderGroupBox.Text = "CT AAC Options";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(93, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(253, 21);
            this.comboBox1.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Profile";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Channel Mode";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(93, 44);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(253, 21);
            this.comboBox2.TabIndex = 26;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(15, 71);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(236, 17);
            this.checkBox2.TabIndex = 28;
            this.checkBox2.Text = "Produce MPEG2 AAC isntead of MPEG4 AAC";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // vBitrate
            // 
            this.vBitrate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.vBitrate.Location = new System.Drawing.Point(3, 127);
            this.vBitrate.Maximum = 320;
            this.vBitrate.Minimum = 16;
            this.vBitrate.Name = "vBitrate";
            this.vBitrate.Size = new System.Drawing.Size(349, 42);
            this.vBitrate.TabIndex = 29;
            this.vBitrate.TickFrequency = 8;
            this.vBitrate.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.vBitrate.Value = 16;
            this.vBitrate.ValueChanged += new System.EventHandler(this.vBitrate_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "label3";
            // 
            // WinAmpAACConfigurationDialog
            // 
            this.ClientSize = new System.Drawing.Size(376, 502);
            this.Name = "WinAmpAACConfigurationDialog";
            this.Text = "CT AAC Encoder Configuration";
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vBitrate)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        #region properties
        protected override AudioCodecSettings defaultSettings()
        {
            return new WinAmpAACSettings();
        }
        protected override Type supportedType
        {
            get { return typeof(WinAmpAACSettings); }
        }
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                WinAmpAACSettings nas = new WinAmpAACSettings();
                nas.Mpeg2AAC = checkBox2.Checked;
                nas.Profile = (AacProfile)(comboBox1.SelectedItem as EnumProxy).RealValue;
                nas.StereoMode = (WinAmpAACSettings.AacStereoMode)(comboBox2.SelectedItem as EnumProxy).RealValue;
                nas.Bitrate = vBitrate.Value;
                return nas;
            }
            set
            {
                WinAmpAACSettings nas = value as WinAmpAACSettings;
                checkBox2.Checked = nas.Mpeg2AAC;
                comboBox1.SelectedItem = EnumProxy.Create(nas.Profile);
                comboBox2.SelectedItem = EnumProxy.Create(nas.StereoMode);
                vBitrate.Value = Math.Max(Math.Min(nas.Bitrate, vBitrate.Maximum), vBitrate.Minimum);
            }
        }
        #endregion

        private void vBitrate_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = "CBR @ " + vBitrate.Value + " kbps";
        }



    }
}