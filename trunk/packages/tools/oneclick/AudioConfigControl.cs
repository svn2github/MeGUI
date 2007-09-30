using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.details.video;

namespace MeGUI.packages.tools.oneclick
{
    public partial class AudioConfigControl : UserControl
    {
        MainForm mainForm = MainForm.Instance;

        #region Audio profiles
        MultipleConfigurersHandler<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> audioCodecHandler;
        ProfilesControlHandler<AudioCodecSettings, string[]> audioProfileHandler;
        private void initAudioHandler()
        {
            this.audioCodec.Items.AddRange(mainForm.PackageSystem.AudioSettingsProviders.ValuesArray);
            try
            {
                this.audioCodec.SelectedItem = mainForm.PackageSystem.AudioSettingsProviders["ND AAC"];
            }
            catch (Exception) { }

            // Init audio handlers
            audioCodecHandler = new MultipleConfigurersHandler<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>(audioCodec);
            audioProfileHandler = new ProfilesControlHandler<AudioCodecSettings, string[]>("Audio", mainForm, audioProfileControl, audioCodecHandler.EditSettings,
                new InfoGetter<string[]>(delegate { return new string[] { "input", "output" }; }), audioCodecHandler.Getter, audioCodecHandler.Setter);
            audioCodecHandler.Register(audioProfileHandler);
            audioProfileHandler.ProfileChanged += new SelectedProfileChangedEvent(ProfileChanged); ;
            audioCodec.SelectedIndexChanged += new EventHandler(audioCodec_SelectedIndexChanged);
        }

        void audioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            raiseEvent();
        }
        #endregion

        private void ProfileChanged(object o, Profile p) 
        {
            raiseEvent();
        }

        public event EventHandler SomethingChanged;

        private void dontEncodeAudio_CheckedChanged(object sender, EventArgs e)
        {
            bool aChecked = dontEncodeAudio.Checked;
            audioCodec.Enabled = !aChecked;
            audioProfileControl.Enabled = !aChecked;
            raiseEvent();
        }

        private void raiseEvent()
        {
            if (SomethingChanged != null) SomethingChanged(this, null);
        }

        private ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> AudioSettingsProvider
        {
            get { return audioCodecHandler.CurrentSettingsProvider; }
        }

        public AudioCodecSettings Settings
        {
            get { return AudioSettingsProvider.GetCurrentSettings(); }
        }
        
        public void openAudioFile(string p)
        {
            int del = AudioEncodingComponent.getDelay(p);
            AudioCodecSettings settings = (audioCodec.SelectedItem as ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>).GetCurrentSettings();
            if (del != 0) // we have a delay we are interested in
            {
                settings.DelayEnabled = true;
                settings.Delay = del;
            }
            else
            {
                settings.DelayEnabled = false;
            }
        }

        public AudioConfigControl()
        {
            InitializeComponent();
        }

        internal void initHandler()
        {
            initAudioHandler();
        }

        internal void RefreshProfiles()
        {
            audioProfileHandler.RefreshProfiles();
        }

        public bool DontEncode
        {
            get
            {
                return dontEncodeAudio.Checked;
            }
            set
            {
                dontEncodeAudio.Checked = value;
            }
        }

        public string SelectedProfile
        {
            get
            {
                return audioProfileHandler.SelectedProfile;
            }
            set
            {
                audioProfileHandler.SelectedProfile = value;
            }
        }
    }
}
