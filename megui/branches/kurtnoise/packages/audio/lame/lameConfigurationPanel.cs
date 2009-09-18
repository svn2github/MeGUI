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

namespace MeGUI.packages.audio.lame
{
    public partial class lameConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<MP3Settings>
    {
        public lameConfigurationPanel():base()
        {
            InitializeComponent();
        }
		#region properties
        protected override bool IsMultichanelSupported
        {
            get
            {
                return false;
            }
        }
	    /// <summary>
	    /// gets / sets the settings that are being shown in this configuration dialog
	    /// </summary>
	    protected override AudioCodecSettings CodecSettings
	    {
	        get
	        {
                MP3Settings ms = new MP3Settings();
                if (rbBitrate.Checked)
                {
                    if (cbCBR.Checked)
                        ms.BitrateMode = BitrateManagementMode.CBR;
                    else
                        ms.BitrateMode = BitrateManagementMode.ABR;
                    ms.Bitrate = (int)this.tbBitrate.Value;
                }
                else
                {
                    ms.BitrateMode = BitrateManagementMode.VBR;
                    ms.Quality = (int)this.tbQuality.Value;
                }
                return ms;
	        }
	        set
	        {
                MP3Settings ms = value as MP3Settings;
                switch (ms.BitrateMode)
                {
                    case BitrateManagementMode.VBR: rbQuality.Checked = true; cbCBR.Checked = false; break;
                    case BitrateManagementMode.ABR: rbBitrate.Checked = true; cbCBR.Checked = false; break;
                    case BitrateManagementMode.CBR: rbBitrate.Checked = true; cbCBR.Checked = true; break;
                }
                tbQuality.Value = ms.Quality;
                tbBitrate.Value = ms.Bitrate;

                target_CheckedChanged(null, null);
	        }
	    }
		#endregion

        #region Editable<MP3Settings> Members

        MP3Settings Editable<MP3Settings>.Settings
        {
            get
            {
                return (MP3Settings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        #region Bitrate Scrollbar
        private void tbBitrate_Scroll(object sender, EventArgs e)
        {
            gbBitrate.Text = String.Format("Bitrate ({0} kbps)", tbBitrate.Value);
        }
        #endregion

        #region Quality Scrollbar
        private void tbQuality_Scroll(object sender, EventArgs e)
        {
            gbQuality.Text = String.Format("Quality (Q = {0}0)", tbQuality.Value);

            switch (tbQuality.Value)
            {
                case 10: tbBitrate.Value = 245; break; //preset fast extreme
                case  9: tbBitrate.Value = 225; break;
                case  8: tbBitrate.Value = 190; break; //preset fast standard 
                case  7: tbBitrate.Value = 175; break;
                case  6: tbBitrate.Value = 165; break; //preset fast medium
                case  5: tbBitrate.Value = 130; break;
                case  4: tbBitrate.Value = 115; break;
                case  3: tbBitrate.Value = 100; break;
                case  2: tbBitrate.Value = 85;  break;
                case  1: tbBitrate.Value = 65;  break;
            }

            tbBitrate_Scroll(sender, e);
        }
        #endregion

        #region Target
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
        #endregion

        #region website link
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
            System.Diagnostics.Process.Start("http://lame.sourceforge.net");
        }
        #endregion

    }
}




