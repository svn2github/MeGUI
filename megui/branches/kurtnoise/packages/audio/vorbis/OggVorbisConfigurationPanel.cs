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

namespace MeGUI.packages.audio.vorbis
{
    public partial class OggVorbisConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<OggVorbisSettings>
    {
        public OggVorbisConfigurationPanel():base()
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
                OggVorbisSettings nas = new OggVorbisSettings();
                if (rbBitrate.Checked)
                {
                    nas.BitrateMode = BitrateManagementMode.ABR;
                    nas.Bitrate = this.tbBitrate.Value;
                }
                else
                {
                    nas.BitrateMode = BitrateManagementMode.VBR;
                    nas.Quality = (Decimal)tbQuality.Value * 10.0M / tbQuality.Maximum;
                }
                return nas;
            }
            set
            {
                OggVorbisSettings nas = value as OggVorbisSettings;
                switch (nas.BitrateMode)
                {
                    case BitrateManagementMode.ABR: rbBitrate.Checked = true; break;
                    case BitrateManagementMode.VBR: rbQuality.Checked = true; break;
                }
                tbQuality.Value = (int)(nas.Quality / 10.0M * (Decimal)tbQuality.Maximum);
                tbBitrate.Value = nas.Bitrate;

                target_CheckedChanged(null, null);
            }
        }
        #endregion

        #region Editable<OggVorbisSettings> Members

        OggVorbisSettings Editable<OggVorbisSettings>.Settings
        {
            get
            {
                return (OggVorbisSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        private void tbBitrate_Scroll(object sender, EventArgs e)
        {
            gbBitrate.Text = String.Format("Bitrate ({0} kbps)", tbBitrate.Value); 
        }

        private void tbQuality_Scroll(object sender, EventArgs e)
        {
            gbQuality.Text = String.Format("Quality (Q = {0})", ((Decimal)tbQuality.Value) * 10.0M / tbQuality.Maximum);
        }

        private void target_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBitrate.Checked)
            {
                gbQuality.Enabled = false;
                gbBitrate.Enabled = true;
            }
            else
            {
                gbBitrate.Enabled = false;
                gbQuality.Enabled = true;
            }

            tbBitrate_Scroll(null, null);
            tbQuality_Scroll(null, null);            
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
            System.Diagnostics.Process.Start("http://www.vorbis.com");
        }
    }
}




