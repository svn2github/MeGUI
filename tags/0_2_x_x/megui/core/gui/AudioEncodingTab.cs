using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.details.video;
using System.IO;
using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class AudioEncodingTab : UserControl
    {
        private AudioEncoderProvider audioEncoderProvider = new AudioEncoderProvider();

        /// <summary>
        /// This delegate is called when a job has been successfully configured and the queue button is pressed
        /// </summary>
        public Setter<AudioJob> QueueJob;

        #region handlers
        private FileTypeHandler<AudioType> fileTypeHandler;
        public FileTypeHandler<AudioType> FileTypeHandler
        {
            get { return fileTypeHandler; }
        }

        private ProfilesControlHandler<AudioCodecSettings, string[]> profileHandler;
        public ProfilesControlHandler<AudioCodecSettings, string[]> ProfileHandler
        {
            get { return profileHandler; }
        }

        private MultipleConfigurersHandler<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> codecHandler;
        public MultipleConfigurersHandler<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> CodecHandler
        {
            get { return codecHandler; }
        }
        #endregion
        
        #region init
        public void InitializeDropdowns()
        {
            audioCodec.Items.Clear();
            audioCodec.Items.AddRange(MainForm.Instance.PackageSystem.AudioSettingsProviders.ValuesArray);
            try { audioCodec.SelectedItem = MainForm.Instance.PackageSystem.AudioSettingsProviders["NAAC"]; }
            catch (Exception)
            {
                try { audioCodec.SelectedIndex = 0; }
                catch (Exception) { MessageBox.Show("No valid audio codecs are set up", "No valid audio codecs", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }

            fileTypeHandler = new FileTypeHandler<AudioType>(audioContainer, audioCodec, new FileTypeHandler<AudioType>.SupportedOutputGetter(delegate
            {
                return audioEncoderProvider.GetSupportedOutput(codecHandler.CurrentSettingsProvider.EncoderType);
            }));

            codecHandler = new MultipleConfigurersHandler<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>(audioCodec);

            profileHandler = new ProfilesControlHandler<AudioCodecSettings, string[]>("Audio", MainForm.Instance, profileControl1, codecHandler.EditSettings,
                new InfoGetter<string[]>(delegate { return new string[] { AudioInput, AudioOutput }; }), codecHandler.Getter, codecHandler.Setter);

            codecHandler.Register(profileHandler);
            fileTypeHandler.RefreshFiletypes();
        }
        #endregion

        public string QueueButtonText
        {
            get { return queueAudioButton.Text; }
            set { queueAudioButton.Text = value; }
        }

        public AudioEncoderProvider AudioEncoderProvider
        {
            get { return audioEncoderProvider; }
        }
        private string AudioInput
        {
            get { return audioInput.Filename; }
            set { audioInput.Filename = value; }
        }
        private string AudioOutput
        {
            get { return audioOutput.Filename; }
            set { audioOutput.Filename = value; }
        }

        /// <summary>
        /// returns the audio codec settings for the currently active audio codec
        /// </summary>
        public AudioCodecSettings AudCodecSettings
        {
            get
            {
                if (string.IsNullOrEmpty(profileHandler.SelectedProfile))
                    return codecHandler.CurrentSettingsProvider.GetCurrentSettings();
                else
                    return (AudioCodecSettings)profileHandler.CurrentProfile.BaseSettings;
            }
            private set
            {
                codecHandler.Setter(value);
            }
        }

        public string verifyAudioSettings()
        {
            AudioJob stream;

            try
            {
                stream = AudioJob;
            }
            catch (MeGUIException m)
            {
                return m.Message;
            }

            if (stream == null)
                return "Audio input, audio output, and audio settings must all be configured";

            string fileErr = MainForm.verifyInputFile(this.AudioInput);
            if (fileErr != null)
            {
                return "Problem with audio input filename:\n" + fileErr;
            }

            fileErr = MainForm.verifyOutputFile(this.AudioOutput);
            if (fileErr != null)
            {
                return "Problem with audio output filename:\n" + fileErr;
            }
            AudioType aot = this.audioContainer.SelectedItem as AudioType;
            // test output file extension
            if (!Path.GetExtension(this.AudioOutput).Replace(".", "").Equals(aot.Extension, StringComparison.InvariantCultureIgnoreCase))
            {
                return "Audio output filename does not have the correct extension.\nBased on current settings, it should be "
                    + aot.Extension;
            }
            return null;
        }
        
        
        public AudioEncodingTab()
        {
            InitializeComponent();
        }


        private void audioInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (!string.IsNullOrEmpty(audioInput.Filename)) openAudioFile(audioInput.Filename);
        }

        private void audioContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            AudioType currentType = (AudioType)audioContainer.SelectedItem;
            audioOutput.Filter = currentType.OutputFilterString;
            AudioOutput = Path.ChangeExtension(AudioOutput, currentType.Extension);
        }

        private void deleteAudioButton_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void queueAudioButton_Click(object sender, EventArgs e)
        {
            string settingsError = verifyAudioSettings();
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            QueueJob(AudioJob);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioJob AudioJob
        {
            get
            {
                if (string.IsNullOrEmpty(AudioInput) ||
                    string.IsNullOrEmpty(AudioOutput) ||
                    AudCodecSettings == null)
                    return null;

                return new AudioJob(this.AudioInput, this.AudioOutput, this.cuts.Filename,
                    this.AudCodecSettings, (int)delay.Value);
            }
            set
            {
                AudioInput = value.Input;
                AudioOutput = value.Output;
                AudCodecSettings = value.Settings;
                cuts.Filename = value.CutFile;
                delay.Value = value.Delay;
            }
        }

        #region helper methods
        public void openAudioFile(string fileName)
        {
            AudioInput = fileName;
            delay.Value = PrettyFormatting.getDelay(fileName);

            try
            {
                AudioOutput = FileUtil.AddToFileName(fileName, MainForm.Instance.Settings.AudioExtension);
            }
            catch (Exception e)
            {
                throw new ApplicationException("The value detected as delay in your filename seems to be too high/low for MeGUI." +
                                              "Try to recreate it with the appropriate tools." + e.Message, e);
            }
                
            audioContainer_SelectedIndexChanged(null, null);              
        }

        internal Size FileTypeComboBoxSize
        {
            get { return audioContainer.Size; }
            set { audioContainer.Size = value; }
        }


        internal void Reset()
        {
            this.AudioInput = "";
            this.cuts.Filename = "";
            this.AudioOutput = "";
            this.delay.Value = 0;
        }

        internal void RefreshProfiles()
        {
            profileHandler.RefreshProfiles();
        }
        #endregion

    }
}
