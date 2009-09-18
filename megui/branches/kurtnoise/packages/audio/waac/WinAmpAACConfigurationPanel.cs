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

namespace MeGUI.packages.audio.waac
{
    public partial class WinAmpAACConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<WinAmpAACSettings>
    {
        public WinAmpAACConfigurationPanel():base()
        {
            InitializeComponent();
            cbProfile.Items.AddRange(EnumProxy.CreateArray(WinAmpAACSettings.SupportedProfiles));
            cbChannelMode.Items.AddRange(EnumProxy.CreateArray(typeof(WinAmpAACSettings.AacStereoMode)));
        }
        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                WinAmpAACSettings nas = new WinAmpAACSettings();
                nas.Mpeg2AAC = chmpeg2.Checked;
                nas.Profile = (AacProfile)(cbProfile.SelectedItem as EnumProxy).RealValue;
                nas.StereoMode = (WinAmpAACSettings.AacStereoMode)(cbChannelMode.SelectedItem as EnumProxy).RealValue;
                nas.Bitrate = tbBitrate.Value;
                return nas;
            }
            set
            {
                WinAmpAACSettings nas = value as WinAmpAACSettings;
                chmpeg2.Checked = nas.Mpeg2AAC;
                cbProfile.SelectedItem = EnumProxy.Create(nas.Profile);
                cbChannelMode.SelectedItem = EnumProxy.Create(nas.StereoMode);
                tbBitrate.Value = Math.Max(Math.Min(nas.Bitrate, tbBitrate.Maximum), tbBitrate.Minimum);

                tbBitrate_Scroll(null, null);
            }
        }
        #endregion

        #region Editable<WinAmpAACSettings> Members
        WinAmpAACSettings Editable<WinAmpAACSettings>.Settings
        {
            get
            {
                return (WinAmpAACSettings)Settings;
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
            System.Diagnostics.Process.Start("http://www.winamp.com");
        }
    }
}




