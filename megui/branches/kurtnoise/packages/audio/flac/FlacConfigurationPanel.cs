// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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

namespace MeGUI.packages.audio.flac
{
    public partial class FlacConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<FlacSettings>
    {
        private MainForm mainform = MainForm.Instance;    

        public FlacConfigurationPanel():base()
        {
            InitializeComponent();
        }

        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                FlacSettings nas = new FlacSettings();
               /* if (rbBitrate.Checked)
                {
                    nas.BitrateMode = BitrateManagementMode.CBR;
                    nas.Bitrate = this.tbBitrate.Value;
                }
                else
                {
                    nas.BitrateMode = BitrateManagementMode.VBR;
                    nas.Quality = this.tbQuality.Value;
                } */
                return nas;
            }
            set
            {
                FlacSettings nas = value as FlacSettings;
               /* switch (nas.BitrateMode)
                {
                    case BitrateManagementMode.VBR: rbQuality.Checked = true; break;
                    case BitrateManagementMode.CBR: rbBitrate.Checked = true; break;
                }
                tbBitrate.Value = Math.Max(Math.Min(nas.Bitrate, tbBitrate.Maximum), tbBitrate.Minimum);
                tbQuality.Value = (int)(nas.Quality);

                target_CheckedChanged(null, null);*/
            }
        }
        #endregion

        #region Editable<FlacSettings> Members

        FlacSettings Editable<FlacSettings>.Settings
        {
            get
            {
                return (FlacSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        private void tbQuality_Scroll(object sender, EventArgs e)
        {
            gbQuality.Text = String.Format("Compression Level ({0})", tbQuality.Value);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open link that was clicked.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void VisitLink()
        {
            //Call the Process.Start method to open the default browser 
            //with a URL:
            System.Diagnostics.Process.Start("http://flac.sourceforge.net");
        }

    }
}




