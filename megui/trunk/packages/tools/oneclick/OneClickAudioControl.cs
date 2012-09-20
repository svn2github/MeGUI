// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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
using System.Windows.Forms;

using MeGUI.core.gui;
using MeGUI.core.util;

namespace MeGUI
{
    public partial class OneClickAudioControl : UserControl
    {
        public event EventHandler LanguageChanged;

        public OneClickAudioControl()
        {
            InitializeComponent();
            language.Items.AddRange(new List<string>(LanguageSelectionContainer.Languages.Keys).ToArray());
            if (MainForm.Instance != null)
                encoderProfile.Manager = MainForm.Instance.Profiles;
        }

        public void SetProfileNameOrWarn(string fqname)
        {
            encoderProfile.SetProfileNameOrWarn(fqname);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioEncodingMode EncodingMode
        {
            get
            {
                return (AudioEncodingMode)cbEncodingMode.SelectedIndex;
            }
            set
            {
                cbEncodingMode.SelectedIndex = (int)value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Profile EncoderProfile
        {
            get { return encoderProfile.SelectedProfile; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Language
        {
            get { return language.Text; }
            set { language.SelectedItem = value; }
        }

        private void cbEncodingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            encoderProfile.Enabled = !cbEncodingMode.SelectedText.Equals("never");
        }

        private void language_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LanguageChanged != null)
                LanguageChanged(this, null);
        }

        public void SetDefault()
        {
            language.Enabled = false;
            language.Items.Add("[default]");
            language.SelectedIndex = 0;
        }
    }

    public class OneClickAudioSettings
    {
        public string Language;
        public string Profile;

        public OneClickAudioSettings()
        {

        }

        public OneClickAudioSettings(string language, string profile, AudioEncodingMode mode)
        {
            this.Language = language;
            this.Profile = profile;
            this.audioEncodingMode = mode;
        }

        private AudioEncodingMode audioEncodingMode;
        [System.Xml.Serialization.XmlIgnore()]
        public AudioEncodingMode AudioEncodingMode
        {
            get { return audioEncodingMode; }
            set { audioEncodingMode = value; }
        }

        // for profile import/export in case the enum changes
        public string AudioEncodingModeString
        {
            get { return audioEncodingMode.ToString(); }
            set
            {
                if (value.Equals("Never"))
                    audioEncodingMode = AudioEncodingMode.Never;
                else if (value.Equals("IfCodecDoesNotMatch"))
                    audioEncodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
                else
                    audioEncodingMode = AudioEncodingMode.Always;
            }
        }
    }
}