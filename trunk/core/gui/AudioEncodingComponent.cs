using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details.video;
using MeGUI.core.plugins.interfaces;

namespace MeGUI
{
    public partial class AudioEncodingComponent : UserControl
    {
        private MainForm mainForm;
        private AudioStream[] audioStreams = new AudioStream[2];
        private AudioEncoderProvider audioEncoderProvider= new AudioEncoderProvider();
        private int lastSelectedAudioTrackNumber = 0;

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


        public AudioEncodingComponent()
        {
            InitializeComponent();
        }
        public void InitializeDropdowns()
        {
            audioCodec.Items.Clear();
            audioCodec.Items.AddRange(mainForm.PackageSystem.AudioSettingsProviders.ValuesArray);
            try { audioCodec.SelectedItem = mainForm.PackageSystem.AudioSettingsProviders["NAAC"]; }
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

            profileHandler = new ProfilesControlHandler<AudioCodecSettings, string[]>("Audio", mainForm, profileControl1, codecHandler.EditSettings,
                new InfoGetter<string[]>(delegate { return new string[] { AudioInput, AudioOutput }; }), codecHandler.Getter, codecHandler.Setter);

            profileHandler.ConfigureCompleted += new EventHandler(profileHandler_ConfigureCompleted);
            codecHandler.Register(profileHandler);
            fileTypeHandler.RefreshFiletypes();
        }

        void profileHandler_ConfigureCompleted(object sender, EventArgs e)
        {
            AudioStream stream = CurrentAudioStream;
            stream.settings = AudCodecSettings;
            CurrentAudioStream = stream;
        }
        #endregion
        public MainForm MainForm
        {
            set { mainForm = value; }
        }
        public AudioEncoderProvider AudioEncoderProvider
        {
            get { return audioEncoderProvider; }
        }
        public string AudioInput
        {
            get { return audioInput.Filename; }
            set { audioInput.Filename = value; }
        }
        public string AudioOutput
        {
            get { return audioOutput.Filename; }
            set { audioOutput.Filename = value; }
        }
        public ComboBox AudioCodec
        {
            get { return audioCodec; }
        }

        /// <summary>
        /// returns the audio streams registered
        /// </summary>
        public AudioStream[] AudioStreams
        {
            get { updateAudioStreams(); return audioStreams; }
        }
        public void setAudioTrack(int trackNumber, string input)
        {
            // Change view to other track
            if (trackNumber == 0)
            {
                audioTrack1.Checked = true;
                audioTrack2.Checked = false;
            }
            else if (trackNumber == 1)
            {
                audioTrack1.Checked = false;
                audioTrack2.Checked = true;
            }
            if (trackNumber >= 0 && trackNumber <= 1)
                this.openAudioFile(input);
        }
        /// <summary>
        /// returns the audio codec settings for the currently active audio codec
        /// </summary>
        public AudioCodecSettings AudCodecSettings
        {
            get
            {
                return codecHandler.CurrentSettingsProvider.GetCurrentSettings();
            }
        }
        public string verifyAudioSettings()
        {
            // test for valid input filename
            int nbEmptyStreams = 0;
            foreach (AudioStream stream in audioStreams)
            {
                if (string.IsNullOrEmpty(stream.path) && string.IsNullOrEmpty(stream.output))
                {
                    nbEmptyStreams++;
                    continue;
                }
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
            }
            if (nbEmptyStreams == audioStreams.Length)
                return "No audio input defined.";
            return null;
        }

        #region local event handlers
        private void audioContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            AudioType currentType = CurrentAudioOutputType;
            audioOutput.Filter = currentType.OutputFilterString;
            AudioOutput = Path.ChangeExtension(AudioOutput, currentType.Extension);
        }
        /// <summary>
        /// deletes the currently selected audio stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAudioButton_Click(object sender, System.EventArgs e)
        {
            this.AudioInput = "";
            this.AudioOutput = "";
            this.cuts.Filename = "";
            int trackNumber = -1;
            if (this.audioTrack1.Checked)
                trackNumber = 0;
            if (this.audioTrack2.Checked)
                trackNumber = 1;
            if (trackNumber != -1)
            {
                this.audioStreams[trackNumber].settings = null;
                this.audioStreams[trackNumber].cutlist = "";
                this.audioStreams[trackNumber].Type = null;
                this.audioStreams[trackNumber].path = "";
                this.audioStreams[trackNumber].output = "";
                this.audioStreams[trackNumber].Delay = 0;
            }
        }
        /// <summary>
        /// handles the user switching from one audio track to another
        /// makes sure the current i/o configuration and the settings are saved so they can be retrieved later on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioTrack_CheckedChanged(object sender, System.EventArgs e)
        {
            int current = 0;
            if (audioTrack2.Checked) // user switched from track 2 to track 1
                current = 1;
            string error = this.verifyAudioSettings();
            if (error != null && !String.IsNullOrEmpty(this.AudioInput))
            {
                MessageBox.Show(error, "Unsupported audio configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (String.IsNullOrEmpty(this.AudioInput))
            {
                this.audioStreams[lastSelectedAudioTrackNumber].path = "";
                this.audioStreams[lastSelectedAudioTrackNumber].output = "";
                this.audioStreams[lastSelectedAudioTrackNumber].Type = null;
                this.audioStreams[lastSelectedAudioTrackNumber].settings = null;
                this.audioStreams[lastSelectedAudioTrackNumber].cutlist = "";
                this.audioStreams[lastSelectedAudioTrackNumber].Delay = 0;
            }
            else
            {
                this.audioStreams[lastSelectedAudioTrackNumber].path = this.AudioInput;
                this.audioStreams[lastSelectedAudioTrackNumber].output = this.AudioOutput;
                this.audioStreams[lastSelectedAudioTrackNumber].Type = CurrentAudioOutputType;
                this.audioStreams[lastSelectedAudioTrackNumber].settings = (audioCodec.SelectedItem as ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>).GetCurrentSettings().clone();
                this.audioStreams[lastSelectedAudioTrackNumber].cutlist = this.cuts.Filename;
                //this.audioStreams[lastSelectedAudioTrackNumber].Delay = this.audioStreams[lastSelectedAudioTrackNumber].settings.Delay;
            }
            this.AudioInput = this.audioStreams[current].path;
            this.AudioOutput = this.audioStreams[current].output;
            this.cuts.Filename = this.audioStreams[current].cutlist;
            if (audioStreams[current].settings != null)
            {
                foreach (ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> p in this.AudioCodec.Items)
                {
                    if (p.IsSameType(audioStreams[current].settings))
                    {
                        p.LoadSettings(audioStreams[current].settings);
                        AudioCodec.SelectedItem = p;
                        break;
                    }
                }
            }
            lastSelectedAudioTrackNumber = current;
        }
        /// <summary>
        /// handles the queue button in the audio tab
        /// generates a new audio job, adds the job to the queue and listView, and if "and encode" is checked, we'll start encoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queueAudioButton_Click(object sender, System.EventArgs e)
        {
            this.updateAudioStreams();
            string settingsError = verifyAudioSettings();
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            AudioCodecSettings aSettings = this.CurrentAudioStream.settings;
            if (this.CurrentAudioStream.Delay != 0)
            {
                aSettings.DelayEnabled = true;
                aSettings.Delay = this.CurrentAudioStream.Delay;
            }
            mainForm.JobUtil.AddAudioJob(this.AudioInput, this.AudioOutput, this.cuts.Filename, aSettings);
        }
        private void audioInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (!string.IsNullOrEmpty(audioInput.Filename)) openAudioFile(audioInput.Filename);
        }
        #endregion
        #region helper methods
        public void openAudioFile(string fileName)
        {
            this.AudioInput = fileName;
            int del = getDelay(fileName);
            AudioCodecSettings settings = (AudioCodec.SelectedItem as ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>).GetCurrentSettings();
            /*if (del != 0) // we have a delay we are interested in
            {
                settings.DelayEnabled = true;
                settings.Delay = del;
            }
            else
            {
                settings.DelayEnabled = false;
            }*/
            string filePath = Path.GetDirectoryName(fileName);
            string fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
            this.AudioOutput = Path.Combine(filePath, fileNameNoExtension) + mainForm.Settings.AudioExtension + ".extension";
            this.AudioOutput = Path.ChangeExtension(this.AudioOutput, this.CurrentAudioOutputType.Extension);
            this.updateAudioStreams();
        }
        /// <summary>
        /// Selects the current audio stream from audioStreams and gets/sets it.
        /// </summary>
        private AudioStream CurrentAudioStream
        {
            get
            {
                if (this.audioTrack1.Checked)
                    return this.audioStreams[0];
                else
                    return this.audioStreams[1];
            }
            set
            {
                if (this.audioTrack1.Checked)
                    this.audioStreams[0] = value;
                else
                    this.audioStreams[1] = value;
            }
        }
        private void updateAudioStreams()
        {
            AudioStream stream = new AudioStream();
            stream.path = this.AudioInput;
            stream.output = this.AudioOutput;
            stream.settings = this.AudCodecSettings.clone();
            stream.Type = (this.audioContainer.SelectedItem as AudioType);
            stream.Delay = getDelay(stream.path);
            stream.cutlist = this.cuts.Filename;
            this.CurrentAudioStream = stream;
        }
        public AudioType CurrentAudioOutputType
        {
            get { return this.audioContainer.SelectedItem as AudioType; }
        }
        #endregion



        /// <summary>
        /// gets the delay from an audio filename
        /// </summary>
        /// <param name="fileName">file name to be analyzed</param>
        /// <returns>the delay in milliseconds</returns>
        public static int getDelay(string fileName)
        {
            int start = fileName.LastIndexOf("DELAY ");
            if (start != -1) // delay is in filename
            {
                try
                {
                    string delay = fileName.Substring(start + 6, fileName.LastIndexOf("ms.") - start - 6);
                    int del = 0;
                    del = Int32.Parse(delay);
                    return del;
                }
                catch (Exception e) // problem parsing, assume 0s delay
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
            return 0;
        }


        internal void Reset()
        {
            this.AudioInput = "";
            this.AudioOutput = "";
            this.audioStreams[0].path = "";
            this.audioStreams[0].output = "";
            this.audioStreams[0].settings = null;
            this.audioStreams[1].path = "";
            this.audioStreams[1].output = "";
            this.audioStreams[1].settings = null;
        }

/*        private void audioProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (audioProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                GenericProfile<AudioCodecSettings> prof = (GenericProfile<AudioCodecSettings>)mainForm.Profiles.AudioProfiles[audioProfile.SelectedItem.ToString()];
                foreach (ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> p in audioCodec.Items)
                {
                    if (p.IsSameType(prof.Settings))
                    {
                        p.LoadSettings(prof.Settings);
                        audioCodec.SelectedItem = p;
                        break;
                    }
                }
            }
        }*/

        internal void RefreshProfiles()
        {
            profileHandler.RefreshProfiles();
        }
    }
}
