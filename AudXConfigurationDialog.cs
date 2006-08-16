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
    /// Summary description for AudXConfigurationDialog.
    /// </summary>
    public class AudXConfigurationDialog : baseAudioConfigurationDialog
    {
        private Label label1;
        private ComboBox comboBox1;
        #region variables

        #endregion
        #region start / stop
        public AudXConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings,
            string initialProfile)
            : base(profileManager, path, settings, initialProfile)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            performSizeAndLayoutCorrection();
            comboBox1.Items.AddRange(EnumProxy.CreateArray(typeof(AudXSettings.QualityMode)));
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
            this.encoderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.label1);
            this.encoderGroupBox.Controls.Add(this.comboBox1);
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 50);
            this.encoderGroupBox.Text = "AudX Options";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(68, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(278, 21);
            this.comboBox1.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Quality";
            // 
            // AudXConfigurationDialog
            // 
            this.ClientSize = new System.Drawing.Size(376, 502);
            this.Name = "AudXConfigurationDialog";
            this.Text = "AudX Encoder Configuration";
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        #region properties
        protected override AudioCodecSettings defaultSettings()
        {
            return new AudXSettings();
        }
        protected override bool IsMultichanelRequed
        {
            get
            {
                return true;
            }
        }

        protected override Type supportedType
        {
            get { return typeof(AudXSettings); }
        }
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                AudXSettings nas = new AudXSettings();
                nas.Quality = (AudXSettings.QualityMode)(comboBox1.SelectedItem as EnumProxy).RealValue;
                return nas;
            }
            set
            {
                AudXSettings nas = value as AudXSettings;
                comboBox1.SelectedItem = EnumProxy.Create(nas.Quality);
            }
        }
        #endregion
    }
}