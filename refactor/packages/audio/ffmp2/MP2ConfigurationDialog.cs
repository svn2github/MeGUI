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
    /// Summary description for MP2ConfigurationDialog.
    /// </summary>
    public class MP2ConfigurationDialog : baseAudioConfigurationDialog
    {
        private Label label2;
        private Label label1;
        private ComboBox comboBox1;
        #region variables

        #endregion
        #region start / stop
        public MP2ConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings,
            string initialProfile)
            : base(profileManager, path, settings, initialProfile)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            performSizeAndLayoutCorrection();
            comboBox1.Items.AddRange(MP2Settings.SupportedBitrates);
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
            this.encoderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.label2);
            this.encoderGroupBox.Controls.Add(this.label1);
            this.encoderGroupBox.Controls.Add(this.comboBox1);
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 50);
            this.encoderGroupBox.Text = "MP2 Options";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(57, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(142, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "kbps";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bitrate";
            // 
            // MP2ConfigurationDialog
            // 
            this.ClientSize = new System.Drawing.Size(376, 502);
            this.Name = "MP2ConfigurationDialog";
            this.Text = "MP2 Encoder Configuration";
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        #region properties

        protected override AudioCodecSettings defaultSettings()
        {
            return new MP2Settings();
        }
        protected override bool IsMultichanelSupported
        {
            get
            {
                return false;
            }
        }

        protected override Type supportedType
        {
            get { return typeof(MP2Settings); }
        }
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                MP2Settings nas = new MP2Settings();
                nas.Bitrate = (int)comboBox1.SelectedItem;
                return nas;
            }
            set
            {
                MP2Settings nas = value as MP2Settings;
                comboBox1.SelectedItem = nas.Bitrate;
            }
        }
        #endregion
    }
}