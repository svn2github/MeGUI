using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class OneClickWindow : Form
    {
        #region Variable Declaration
        private bool beingCalled = false, updateAudioBeingCalled = false;
        private StringBuilder logBuilder;
        private bool outputChosen = false;
        private MainForm mainForm;
        private VideoEncoderProvider videoEncoderProvider;
        private AudioEncoderProvider audioEncoderProvider;
        private JobUtil jobUtil;
        private BitrateCalculator calc;
        private List<string> audioLanguages;
        private VideoUtil vUtil;
        private PartialAudioStream[] audioStreams;
        private MuxProvider muxProvider;
        private int lastSelectedAudioTrackNumber = 0;
        #endregion
        #region init
        public OneClickWindow(MainForm mainForm, int videoIndex, int audioIndex, JobUtil jobUtil, VideoEncoderProvider vProv, AudioEncoderProvider aProv)
        {
            this.mainForm = mainForm;
            this.jobUtil = jobUtil;
            this.videoEncoderProvider = vProv;
            this.audioEncoderProvider = aProv;
            muxProvider = new MuxProvider();
            audioStreams = new PartialAudioStream[2];

            InitializeComponent();

            CodecManager codecs = new CodecManager();
            this.audioCodec.Items.AddRange(CodecManager.ListOfAudioCodecs);
            this.videoCodec.Items.AddRange(CodecManager.ListOfVideoCodecs);
            this.audioCodec.SelectedItem = CodecManager.NAAC;
            this.videoCodec.SelectedItem = CodecManager.X264;

            this.calc = new BitrateCalculator();
            this.filesizeComboBox.Items.AddRange(calc.getPredefinedOutputSizes());
            this.filesizeComboBox.Items.Add("Custom");
            this.filesizeComboBox.Items.Add("Don't care");

            this.filesizeComboBox.SelectedIndex = 2;
            this.arComboBox.SelectedIndex = 0;
            
            this.mainForm = mainForm;
            this.shutdownCheckBox.Checked = mainForm.Settings.Shutdown;
            foreach (string name in mainForm.Profiles.AvsProfiles.Keys)
            {
                this.avsProfile.Items.Add(name);
            }
            this.avsProfile.SelectedIndex = 0;
            foreach (string name in mainForm.Profiles.VideoProfiles.Keys)
            {
                this.videoProfile.Items.Add(name);
            }
            foreach (string name in mainForm.Profiles.AudioProfiles.Keys)
            {
                this.audioProfile.Items.Add(name);
            }
            if (audioIndex >= 0)
            {
                audioProfile.SelectedIndex = audioIndex;
            }

            logBuilder = new StringBuilder();
            this.vUtil = new VideoUtil(this.mainForm);
            audioLanguages = new List<string>();
            containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
            this.containerFormat.SelectedIndex = 0;
            this.playbackMethod.Items.Clear();
            foreach (string name in mainForm.Profiles.OneClickProfiles.Keys)
            {
                this.playbackMethod.Items.Add(name);
            }
            if (playbackMethod.Items.Contains(mainForm.Settings.OneClickProfileName))
                playbackMethod.SelectedItem = mainForm.Settings.OneClickProfileName;
            playbackMethod_SelectedIndexChanged(null, null);
            videoCodec.SelectedIndexChanged += videoCodec_SelectedIndexChanged;
            audioCodec.SelectedIndexChanged += audioCodec_SelectedIndexChanged;
            showAdvancedOptions_CheckedChanged(null, null);
        }
        #endregion
        #region event handlers
        private void openInput(string fileName)
        {
            input.Text = fileName;
            track1.Items.Clear();
            track2.Items.Clear();
            AspectRatio ar;
            int maxHorizontalResolution;
            List<AudioTrackInfo> audioTracks;
            List<SubtitleInfo> subtitles;
            int pgc;
            vUtil.openVideoSource(fileName, out audioTracks, out subtitles, out ar, out maxHorizontalResolution, out pgc);
            track1.Items.AddRange(audioTracks.ToArray());
            track2.Items.AddRange(audioTracks.ToArray());
            foreach (AudioTrackInfo ati in audioTracks)
            {
                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage1.ToLower()))
                {
                    track1.SelectedItem = ati;
                    continue;
                }
                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage2.ToLower()))
                {
                    track2.SelectedItem = ati;
                    continue;
                }
            }
            horizontalResolution.Maximum = maxHorizontalResolution;
            string chapterFile = VideoUtil.getChapterFile(fileName);
            if (File.Exists(chapterFile))
            {
                this.chapterFile.Text = chapterFile;
            }
            audioLanguages.Clear();
            workingDirectory.Text = MainForm.GetDirectoryName(fileName);
            workingName.Text = Path.GetFileNameWithoutExtension(fileName);
            this.updateFilename();
            this.setAspectRatio(ar);
        }
        
        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "VOB Files (*.vob)|*.vob|MPEG-1/2 Program Streams (*.mpg)|*.mpg|Transport Streams " +
                "(*.ts)|*.ts|All DGIndex supported files|*.vob;*.mpg;*.mpeg;*.m2v;*.mpv;*.tp;*.ts" +
                ";*.trp;*.pva;*.vro";
            openFileDialog.FilterIndex = 4;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openInput(openFileDialog.FileName);
            }
        }
        private void selectOutput_Click(object sender, EventArgs e)
        {
            if (outputDialog.ShowDialog() == DialogResult.OK)
            {
                output.Text = outputDialog.FileName;
                outputChosen = true;
            }
        }

        private void clearAudio1Button_Click(object sender, EventArgs e)
        {
            track1.SelectedIndex = -1;
            updateAudioFilenames();
        }
        private void clearAudio2Button_Click(object sender, EventArgs e)
        {
            track2.SelectedIndex = -1;
            updateAudioFilenames();
        }

        private void filesizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.filesizeKB.ReadOnly = true;
            filesizeKB.Text = calc.getOutputSizeKBs(filesizeComboBox.SelectedIndex).ToString();
            if (filesizeComboBox.SelectedIndex == 10) // Custom
                this.filesizeKB.ReadOnly = false;
            if (filesizeComboBox.SelectedIndex == 11) // Don't care
                this.filesizeKB.Text = "-1";
        }

        private void workingDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                workingDirectory.Text = fbd.SelectedPath;
                updateFilename();
            }
        }

        private void chapterFileOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Chapter Files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                chapterFile.Text = openFileDialog.FileName;
            }
        }

        private void workingName_TextChanged(object sender, EventArgs e)
        {
            updateFilename();
        }

        private void arComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            double dar = 0;
            switch (arComboBox.SelectedIndex)
            {
                case 0:
                    dar = 1.7778;
                    break;
                case 1:
                    dar = 1.3333;
                    break;
                case 2:
                    dar = 1;
                    break;
            }
            AR.Text = dar.ToString();
            if (arComboBox.SelectedIndex == 3)
                AR.ReadOnly = false;
            else
                AR.ReadOnly = true;
        }

        private void updatePossibleContainers()
        {
            // Since everything calls everything else, this is just a safeguard to make sure we don't infinitely recurse
            if (beingCalled)
                return;
            beingCalled = true;
            updateAudioStreamsAndGUI();
            List<AudioEncoderType> audioCodecs = new List<AudioEncoderType>();
            List<MuxableType> dictatedOutputTypes = new List<MuxableType>();
            for (int i = 0; i < audioStreams.Length; i++)
            {
                if (audioStreams[i].settings != null && !audioStreams[i].dontEncode && 
                    !string.IsNullOrEmpty(audioStreams[i].input))
                {
                    audioCodecs.Add(audioStreams[i].settings.EncoderType);
                }
                else if (audioStreams[i].dontEncode)
                {
                    string typeString;

                    if (!audioStreams[i].useExternalInput)
                    {
                        if (i == 0 && track1.SelectedIndex >= 0)
                        {
                            AudioTrackInfo ati = track1.SelectedItem as AudioTrackInfo;
                            typeString = "file." + ati.Type;
                        }
                        else if (i == 1 && track2.SelectedIndex >= 0)
                        {
                            AudioTrackInfo ati = track2.SelectedItem as AudioTrackInfo;
                            typeString = "file." + ati.Type;
                        }
                        else
                            continue;
                    }
                    else if (MainForm.verifyInputFile(audioStreams[i].input) == null)
                    {
                        typeString = audioStreams[i].input;
                    }
                    else 
                        continue;
                        
                    if (VideoUtil.guessAudioType(typeString) != null)
                        dictatedOutputTypes.Add(VideoUtil.guessAudioMuxableType(typeString, false));
                }
            }
            List<ContainerFileType> supportedOutputTypes = this.muxProvider.GetSupportedContainers(
                CurrentVideoCodecSettingsProvider.EncoderType, audioCodecs.ToArray(), dictatedOutputTypes.ToArray());
            if (supportedOutputTypes.Count > 0)
            {
                this.containerFormat.Items.Clear();
                this.containerFormat.Items.AddRange(supportedOutputTypes.ToArray());
                this.containerFormat.SelectedIndex = 0;
                this.output.Text = Path.ChangeExtension(output.Text, (this.containerFormat.SelectedItem as ContainerFileType).Extension);
            }
            beingCalled = false;
        }

/*        private void containerFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            chooseOutputName();
        }
        */
        private void splitOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (splitOutput.Checked)
                splitSize.Enabled = true;
            else
            {
                splitSize.Enabled = false;
                splitSize.Text = "0";
            }
        }
        private void playbackMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playbackMethod.SelectedItem != null)
            {
                OneClickSettings settings = this.mainForm.Profiles.OneClickProfiles[playbackMethod.SelectedItem.ToString()].Settings;

                // Do extra defaults config (same code as in OneClickDefaultWindow)
                // strings
                if (audioProfile.Items.Contains(settings.AudioProfileName))
                    audioProfile.SelectedItem = settings.AudioProfileName;
                if (avsProfile.Items.Contains(settings.AvsProfileName))
                    avsProfile.SelectedItem = settings.AvsProfileName;
                if (videoProfile.Items.Contains(settings.VideoProfileName))
                    videoProfile.SelectedItem = settings.VideoProfileName;
                if (containerFormat.Items.Contains(settings.ContainerFormatName))
                    containerFormat.SelectedItem = settings.ContainerFormatName;
                if (filesizeComboBox.Items.Contains(settings.StorageMediumName))
                    filesizeComboBox.SelectedItem = settings.StorageMediumName;

                // bools
                dontEncodeAudio.Checked = settings.DontEncodeAudio;
                signalAR.Checked = settings.SignalAR;
                splitOutput.Checked = settings.Split;
                autoDeint.Checked = settings.AutomaticDeinterlacing;

                // ints
                if (settings.SplitSize > 0)
                    splitSize.Text = settings.SplitSize.ToString();
                if (settings.Filesize > 0)
                    filesizeKB.Text = settings.Filesize.ToString();
                horizontalResolution.Value = settings.OutputResolution;

                // Clean up after those settings were set
                filesizeComboBox_SelectedIndexChanged(null, null);
                containerFormat_SelectedIndexChanged_1(null, null);
                videoProfile_SelectedIndexChanged_1(null, null);
                dontEncodeAudio_CheckedChanged(null, null);
                splitOutput_CheckedChanged(null, null);
            }
        }


        private void goButton_Click(object sender, EventArgs e)
        {
            if ((verifyAudioSettings() == null)
                && (getCurrentVideoCodecSettings() != null)
                && !string.IsNullOrEmpty(input.Text)
                && !string.IsNullOrEmpty(workingName.Text)
                && avsProfile.SelectedIndex != -1)
            {
                long desiredSize;
                try
                {
                    desiredSize = Int64.Parse(filesizeKB.Text) * 1024;
                }
                catch (Exception f)
                {
                    MessageBox.Show("I'm not sure how you want me to reach a target size of <empty>.\r\nWhere I'm from that number doesn't exist.\r\n",
                        "Target size undefined", MessageBoxButtons.OK);
                    Console.Write(f.Message);
                    return;
                }

                string infoFile = VideoUtil.getInfoFileName(input.Text);
                if (infoFile.Length > 0)
                    this.audioLanguages = vUtil.getAudioLanguages(infoFile);
                if (audioLanguages.Count == 0) // add 8 dummy tracks
                    audioLanguages.AddRange(new string[] { "", "", "", "", "", "", "", "" });
                if (!audioStreams[0].dontEncode && track1.SelectedIndex >= 0)
                {
                    audioStreams[0].language = audioLanguages[track1.SelectedIndex];
                }
                if (!audioStreams[1].dontEncode && track2.SelectedIndex >= 0)
                {
                    audioStreams[1].language = audioLanguages[track2.SelectedIndex];
                }
                string d2vName = workingDirectory.Text + @"\" + workingName.Text + ".d2v";
                DGIndexPostprocessingProperties dpp = new DGIndexPostprocessingProperties();
                switch (arComboBox.SelectedIndex)
                {
                    case 0:
                        dpp.AR = AspectRatio.ITU16x9;
                        break;
                    case 1:
                        dpp.AR = AspectRatio.ITU4x3;
                        break;
                    case 2:
                        dpp.AR = AspectRatio.A1x1;
                        break;
                    case 3:
                        dpp.AR = AspectRatio.CUSTOM;
                        dpp.CustomAR = Double.Parse(AR.Text);
                        break;
                    case 4:
                        dpp.AutoDeriveAR = true;
                        break;
                }
                dpp.AudioStreams = audioStreams;
                dpp.AutoDeinterlace = autoDeint.Checked;
                dpp.AviSynthScript = "";
                dpp.AvsSettings = this.mainForm.Profiles.AvsProfiles[avsProfile.SelectedItem.ToString()].Settings;
                dpp.ChapterFile = chapterFile.Text;
                dpp.Container = (ContainerFileType)containerFormat.SelectedItem;
                dpp.FinalOutput = output.Text;
                dpp.HorizontalOutputResolution = (int)horizontalResolution.Value;
                dpp.OutputSize = desiredSize;
                dpp.SignalAR = signalAR.Checked;
                dpp.SplitSize = this.getSplitSize();
                dpp.VideoSettings = this.getCurrentVideoCodecSettings().clone();
                IndexJob job = jobUtil.generateIndexJob(this.input.Text, d2vName, 1,
                    track1.SelectedIndex, track2.SelectedIndex, dpp);
                int jobNumber = mainForm.getFreeJobNumber();
                job.Name = "job" + jobNumber;
                mainForm.addJobToQueue((Job)job);
                this.Close();
            }
            else
                MessageBox.Show("You must select audio and video profile, output name and working directory to continue",
                    "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void shutdownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.Settings.Shutdown = this.shutdownCheckBox.Checked;
        }

        #endregion
        #region helper methods
        /// <summary>
        /// gets the split size for the muxed output
        /// </summary>
        /// <returns></returns>
        public int getSplitSize()
        {
            int splitSize = 0;
            try
            {
                splitSize = Int32.Parse(this.splitSize.Text) * 1024;
            }
            catch (Exception e)
            {
                MessageBox.Show("I'm not sure how you want me to split the output at an undefinied position.\r\nWhere I'm from that number doesn't exist.\r\n" +
                    "I'm going to assume you meant to not split the output", "Split size undefined", MessageBoxButtons.OK);
                Console.Write(e.Message);
            }
            return splitSize;
        }
        private VideoCodecSettings getCurrentVideoCodecSettings()
        {
            return CurrentVideoCodecSettingsProvider.GetCurrentSettings();
        }

/*        private void chooseOutputName()
        {
            if (!outputChosen)
            {
                if (containerFormat.SelectedIndex == 0) //AVI
                    output.Text = workingDirectory.Text + @"\" + workingName.Text + ".avi";
                else if (containerFormat.SelectedIndex == 1)
                    output.Text = workingDirectory.Text + @"\" + workingName.Text + ".mkv";
                else if (containerFormat.SelectedIndex == 2)
                    output.Text = workingDirectory.Text + @"\" + workingName.Text + ".mp4";
            }
        }*/
        private void setAspectRatio(AspectRatio ratio)
        {
            switch (ratio)
            {
                case AspectRatio.ITU16x9:
                    arComboBox.SelectedIndex = 0;
                    arComboBox_SelectedIndexChanged(null, null);
                    break;
                case AspectRatio.ITU4x3:
                    arComboBox.SelectedIndex = 1;
                    arComboBox_SelectedIndexChanged(null, null);
                    break;
                case AspectRatio.A1x1:
                    arComboBox.SelectedIndex = 2;
                    arComboBox_SelectedIndexChanged(null, null);
                    break;
                default:
                    arComboBox.SelectedIndex = 4;
                    arComboBox_SelectedIndexChanged(null, null);
                    break;

            }
        }
        #endregion
        #region Properties
        public string Input
        {
            set
            {
                openInput(value);
            }
        }
        #endregion
        #region profile management
        private void videoCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePossibleContainers();
        }

        private void videoConfigButton_Click(object sender, EventArgs e)
        {
            VideoCodecSettings settings = CurrentVideoCodecSettingsProvider.GetCurrentSettings();
            string selectedProfile;
            if (CurrentVideoCodecSettingsProvider.EditSettings(mainForm.Profiles, mainForm.Settings,
                this.videoProfile.Text, new string[] {"input", "output" }, 
                new int[] { -1, -1 }, out selectedProfile))
            {
                this.videoProfile.Items.Clear();
                foreach (string name in mainForm.Profiles.VideoProfiles.Keys)
                {
                    this.videoProfile.Items.Add(name);
                }
                int index = this.videoProfile.Items.IndexOf(selectedProfile);
                if (index != -1)
                    this.videoProfile.SelectedIndex = index;
            }
            updatePossibleContainers();
        }

        private void videoProfile_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.videoProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                VideoProfile prof = mainForm.Profiles.VideoProfiles[this.videoProfile.SelectedItem.ToString()];
                foreach (IVideoSettingsProvider p in this.videoCodec.Items)
                {
                    if (p.IsSameType(prof.Settings))
                    {
                        p.LoadSettings(prof.Settings);
                        videoCodec.SelectedItem = p;
                        break;
                    }
                }
            }
            updatePossibleContainers();
        }

        private void updateAudioStreamsAndGUI()
        {
            if (updateAudioBeingCalled)
                return;
            updateAudioBeingCalled = true;
            int current = 0;
            if (audioTrack1.Checked) // user switched from track 1 to track 2
            {
                current = 0;
            }
            else if (audioTrack2.Checked) // user switched from track 2 to track 1
            {
                current = 1;
            }
            this.audioStreams[lastSelectedAudioTrackNumber].input = this.audioInput.Text;
            this.audioStreams[lastSelectedAudioTrackNumber].useExternalInput = externalInput.Checked;
            this.audioStreams[lastSelectedAudioTrackNumber].settings = (audioCodec.SelectedItem as IAudioSettingsProvider).GetCurrentSettings();
            this.audioStreams[lastSelectedAudioTrackNumber].dontEncode = dontEncodeAudio.Checked;
            if (this.audioProfile.SelectedIndex >= 0)
                this.audioStreams[lastSelectedAudioTrackNumber].profileItem = audioProfile.SelectedItem;

            this.audioInput.Text = this.audioStreams[current].input;
            this.externalInput.Checked = this.audioStreams[current].useExternalInput;
            this.dontEncodeAudio.Checked = this.audioStreams[current].dontEncode;
            if (audioStreams[current].profileItem != null &&
                audioProfile.Items.Contains(audioStreams[current].profileItem))
                audioProfile.SelectedItem = audioStreams[current].profileItem;
            if (audioStreams[current].settings != null)
            {
                foreach (IAudioSettingsProvider p in this.audioCodec.Items)
                {
                    if (p.IsSameType(audioStreams[current].settings))
                    {
                        p.LoadSettings(audioStreams[current].settings);
                        audioCodec.SelectedItem = p;
                        break;
                    }
                }
            }
            lastSelectedAudioTrackNumber = current;
            updateAudioBeingCalled = false;
        }

        private void audioTrack1_CheckedChanged(object sender, EventArgs e)
        {
            updateAudioStreamsAndGUI();
        }

        private string verifyAudioSettings()
        {
            if (!externalInput.Checked)
                return null;
            return (MainForm.verifyInputFile(audioInput.Text));
        }

        private void audioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePossibleContainers();
        }

        private void configAudioButton_Click(object sender, EventArgs e)
        {
            string selectedProfile;
            if (CurrentAudioSettingsProvider.EditSettings(mainForm.Profiles, mainForm.MeGUIPath,
                mainForm.Settings, this.audioProfile.Text,
                new string[] { audioInput.Text, "output" }, out selectedProfile))
            {
                this.audioProfile.Items.Clear();
                foreach (string name in mainForm.Profiles.AudioProfiles.Keys)
                {
                    this.audioProfile.Items.Add(name);
                }
                int index = audioProfile.Items.IndexOf(selectedProfile);
                if (index != -1)
                    audioProfile.SelectedIndex = index;
                PartialAudioStream stream = this.CurrentAudioStream;
                stream.settings = CurrentAudioSettingsProvider.GetCurrentSettings();
                this.CurrentAudioStream = stream;
            }
            updatePossibleContainers();
        }

        private void audioProfile_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.audioProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                AudioProfile prof = mainForm.Profiles.AudioProfiles[this.audioProfile.SelectedItem.ToString()];
                foreach (IAudioSettingsProvider p in this.audioCodec.Items)
                {
                    if (p.IsSameType(prof.Settings))
                    {
                        p.LoadSettings(prof.Settings);
                        audioCodec.SelectedItem = p;
                        break;
                    }
                }
            }
            updatePossibleContainers();
        }

        private void externalInput_CheckedChanged(object sender, EventArgs e)
        {
            if (externalInput.Checked)
            {
                audioInputOpenButton.Visible = true;
                if (MainForm.verifyInputFile(audioInput.Text)!=null)
                    audioInput.Text = "";
            }
            else
            {
                audioInputOpenButton.Visible = false;
                try
                {
                    if (audioTrack1.Checked)
                        audioInput.Text = track1.SelectedItem.ToString();
                    else
                        audioInput.Text = track2.SelectedItem.ToString();
                }
                catch (NullReferenceException) { } // This will throw if nothing is selected
            }
        }
        #endregion
        #region properties
        private IVideoSettingsProvider CurrentVideoCodecSettingsProvider
        {
            get
            {
                return this.videoCodec.SelectedItem as IVideoSettingsProvider;
            }
        }
        private IAudioSettingsProvider CurrentAudioSettingsProvider
        {
            get
            {
                return this.audioCodec.SelectedItem as IAudioSettingsProvider;
            }
        }
        private PartialAudioStream CurrentAudioStream
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
        #endregion
        public struct PartialAudioStream
        {
            public string input;
            public string language;
            public bool useExternalInput;
            public bool dontEncode;
            public int trackNumber;
            public object profileItem;
            public AudioCodecSettings settings;
        }

        #region updates
        private void dontEncodeAudio_CheckedChanged(object sender, EventArgs e)
        {
            bool aChecked = dontEncodeAudio.Checked;
            audioCodec.Enabled = !aChecked;
            configAudioButton.Enabled = !aChecked;
            audioProfile.Enabled = !aChecked;
            updatePossibleContainers();
        }

        private void containerFormat_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            updateFilename();
        }

        private void updateFilename()
        {
            if (!outputChosen)
            {
                output.Text = Path.Combine(workingDirectory.Text, workingName.Text + "." + 
                    ((ContainerFileType)containerFormat.SelectedItem).Extension);
            }
            else
            {
                output.Text = Path.ChangeExtension(output.Text, ((ContainerFileType)containerFormat.SelectedItem).Extension);
            }
        }

        private void updateAudioFilenames()
        {
            updateAudioStreamsAndGUI();
            if (!audioStreams[0].useExternalInput)
            {
                try
                {
                    audioStreams[0].input = track1.SelectedItem.ToString();
                }
                catch (NullReferenceException)
                {
                    audioStreams[0].input = "";
                }
            }
            if (!audioStreams[1].useExternalInput)
            {
                try
                {
                    audioStreams[1].input = track2.SelectedItem.ToString();
                }
                catch (NullReferenceException)
                {
                    audioStreams[1].input = "";
                }
            }
            audioInput.Text = CurrentAudioStream.input;
            updateAudioStreamsAndGUI();
            updatePossibleContainers();
        }

        private void track1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateAudioFilenames();
        }

        private void audioInputOpenButton_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = this.audioEncoderProvider.GetSupportedInput(this.CurrentAudioSettingsProvider.CodecType);
            this.openFileDialog.Title = "Select your audio input";
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openAudioFile(openFileDialog.FileName);
            }
        }

        private void openAudioFile(string p)
        {
            this.audioInput.Text = p;
            int del = MainForm.getDelay(p);
            AudioCodecSettings settings = (audioCodec.SelectedItem as IAudioSettingsProvider).GetCurrentSettings();
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
        #endregion

        private void deleteAudioButton_Click(object sender, EventArgs e)
        {
            if (audioTrack1.Checked)
                clearAudio1Button_Click(sender, e);
            else
                clearAudio2Button_Click(sender, e);
        }

        private void showAdvancedOptions_CheckedChanged(object sender, EventArgs e)
        {
            if (showAdvancedOptions.Checked)
            {
                if (!tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Add(tabPage2);
                if (!tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Add(encoderConfigTab);
            }
            else
            {
                if (tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Remove(tabPage2);
                if (tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Remove(encoderConfigTab);
            }
        }
    }
}