// ****************************************************************************
// 
// Copyright (C) 2005-2012  Doom9 & al
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.audio.qaac
{
    public partial class qaacConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<QaacSettings>
    {

        public qaacConfigurationPanel():base()
        {
            InitializeComponent();
            cbMode.Items.AddRange(EnumProxy.CreateArray(QaacSettings.SupportedModes));
            cbProfile.Items.AddRange(EnumProxy.CreateArray(QaacSettings.SupportedProfiles));
            trackBar1_ValueChanged(null, null);
            cbProfile_SelectedIndexChanged(null, null);
        }

        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.cbProfile);
            this.encoderGroupBox.Controls.Add(this.label3);
            this.encoderGroupBox.Controls.Add(this.trackBar);
            this.encoderGroupBox.Controls.Add(this.cbMode);
            this.encoderGroupBox.Controls.Add(this.label2);
            this.encoderGroupBox.Size = new System.Drawing.Size(390, 131);
            this.encoderGroupBox.Text = "QAAC Options";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mode";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(88, 23);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(121, 21);
            this.cbMode.TabIndex = 1;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(6, 50);
            this.trackBar.Maximum = 127;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(387, 45);
            this.trackBar.TabIndex = 2;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar.Value = 90;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Profile";
            // 
            // cbProfile
            // 
            this.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Location = new System.Drawing.Point(88, 95);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(121, 21);
            this.cbProfile.TabIndex = 4;
            this.cbProfile.SelectedIndexChanged += new System.EventHandler(this.cbProfile_SelectedIndexChanged);
            // 
            // qaacConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "qaacConfigurationPanel";
            this.Size = new System.Drawing.Size(394, 300);
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #region Editable<QaacSettings> Members

        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                QaacSettings qas = new QaacSettings();
                if (cbMode.SelectedIndex == 0) qas.BitrateMode = BitrateManagementMode.VBR;
                if (cbMode.SelectedIndex == 1) qas.BitrateMode = BitrateManagementMode.VBR;
                if (cbMode.SelectedIndex == 2) qas.BitrateMode = BitrateManagementMode.ABR;
                if (cbMode.SelectedIndex == 3) qas.BitrateMode = BitrateManagementMode.CBR;
                qas.Mode = (QaacMode)(cbMode.SelectedItem as EnumProxy).RealValue;
                qas.Profile = (QaacProfile)(cbProfile.SelectedItem as EnumProxy).RealValue;
                if (qas.Mode == QaacMode.TVBR) 
                     qas.Quality = (Int16)trackBar.Value;
                else 
                    qas.Bitrate = (int)trackBar.Value;
                return qas;
            }
            set
            {
                QaacSettings qas = value as QaacSettings;
                if (cbMode.SelectedIndex == 0) qas.BitrateMode = BitrateManagementMode.VBR;
                if (cbMode.SelectedIndex == 1) qas.BitrateMode = BitrateManagementMode.VBR;
                if (cbMode.SelectedIndex == 2) qas.BitrateMode = BitrateManagementMode.ABR;
                if (cbMode.SelectedIndex == 3) qas.BitrateMode = BitrateManagementMode.CBR;
                cbMode.SelectedItem = EnumProxy.Create(qas.Mode);
                cbProfile.SelectedItem = EnumProxy.Create(qas.Profile);
                if (cbMode.SelectedIndex == 0)
                    trackBar.Value = Math.Max(Math.Min(qas.Quality, trackBar.Maximum), trackBar.Minimum);  
                else
                    trackBar.Value = Math.Max(Math.Min(qas.Bitrate, trackBar.Maximum), trackBar.Minimum);            
            }
        }
        #endregion

        QaacSettings Editable<QaacSettings>.Settings
        {
            get
            {
                return (QaacSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            switch (cbMode.SelectedIndex)
            {
                case 0 : // TVBR
                    trackBar.Minimum = 0;
                    trackBar.Maximum = 127;
                    trackBar.TickFrequency = 1;
                    encoderGroupBox.Text =  String.Format("QAAC Options - (Q={0})", trackBar.Value);
                    break;
                case 1 : // CVBR
                    trackBar.Minimum = 0;
                    trackBar.Maximum = 320;
                    trackBar.TickFrequency = 20;
                    encoderGroupBox.Text = String.Format("QAAC Options - Constrained Variable Bitrate @ {0} kbit/s", trackBar.Value);
                    break;
                case 2 : // ABR
                    trackBar.Minimum = 0;
                    trackBar.Maximum = 320;
                    trackBar.TickFrequency = 20;
                    encoderGroupBox.Text = String.Format("QAAC Options - Average Bitrate @ {0} kbit/s", trackBar.Value);
                    break;
                case 3 : // CBR
                    trackBar.Minimum = 0;
                    trackBar.Maximum = 320;
                    trackBar.TickFrequency = 20;
                    encoderGroupBox.Text = String.Format("QAAC Options - Constant Bitrate  @ {0} kbit/s", trackBar.Value);
                    break;
            }
            if (cbProfile.SelectedIndex == 2) encoderGroupBox.Text = String.Format("QAAC Options");
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackBar1_ValueChanged(sender, e);
        }

        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbProfile.SelectedIndex)
            {
                case 2: trackBar.Enabled = false; cbMode.Enabled = false; break;
                default: trackBar.Enabled = true; cbMode.Enabled = true; break;
            }
            cbMode_SelectedIndexChanged(sender, e);
        }

    }
}
