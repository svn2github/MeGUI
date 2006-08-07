using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class VideoEncodingComponent : UserControl
    {
        private VideoPlayer player; // window that shows a preview of the video
        private MainForm mainForm;
        private int creditsStartFrame = -1, introEndFrame = -1;
        private int parX = -1, parY = -1;
        private VideoEncoderProvider videoEncoderProvider = new VideoEncoderProvider();
        public VideoEncodingComponent()
        {
            InitializeComponent();
        }
        public void InitializeDropdowns()
        {
            videoCodec.Items.Clear();
            videoCodec.Items.AddRange(mainForm.PackageSystem.VideoSettingsProviders.ValuesArray);
            try { videoCodec.SelectedItem = mainForm.PackageSystem.VideoSettingsProviders["x264"]; }
            catch (Exception)
            {
                try { videoCodec.SelectedIndex = 0; }
                catch (Exception) { MessageBox.Show("No valid video codecs are set up", "No valid video codecs", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
        public MainForm MainForm
        {
            set { mainForm = value; }
        }
        public ComboBox VideoCodec
        {
            get { return videoCodec; }
        }
        public string VideoInput
        {
            get { return videoInput.Filename; }
            set { videoInput.Filename = value; }
        }
        public string VideoOutput
        {
            get { return videoOutput.Filename; }
            set { videoOutput.Filename = value; }
        }
        public ComboBox VideoProfile
        {
            get { return videoProfile; }
        }
        public VideoType CurrentVideoOutputType
        {
            get { return this.fileType.SelectedItem as VideoType; }
        }
        public int PARX
        {
            get { return parX; }
            set { parX = value; }
        }
        public int PARY
        {
            get { return parY; }
            set { parY = value; }
        }
        public bool PrerenderJob
        {
            get { return addPrerenderJob.Checked; }
            set { addPrerenderJob.Checked = value; }
        }
        public int IntroEndFrame
        {
            get { return introEndFrame; }
            set { introEndFrame = value; }
        }
        public int CreditsStartFrame
        {
            get { return creditsStartFrame; }
            set { creditsStartFrame = value; }
        }
        public IVideoSettingsProvider CurrentVideoCodecSettingsProvider
        {
            get
            {
                return videoCodec.SelectedItem as IVideoSettingsProvider;
            }
        }

        #region event handlers
        private void videoInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            openVideoFile(videoInput.Filename);
        }

        private void videoOutput_FileSelected(FileBar sender, FileBarEventArgs args)
        {

        }
        /// <summary>
        /// opens the AviSynth preview for a given AviSynth script
        /// gets the properties of the video, registers the callbacks, updates the video bitrate (we know the lenght of the video now) and updates the commandline
        /// with the scriptname
        /// </summary>
        /// <param name="fileName">the AviSynth scrip to be opened</param>
        private void openAvisynthScript(string fileName)
        {
            if (this.player != null) // make sure only one preview window is open at all times
                player.Close();
            player = new VideoPlayer();
            bool videoLoaded = player.loadVideo(mainForm, fileName, PREVIEWTYPE.CREDITS, true);
            if (videoLoaded)
            {
                if (parX < 1 || parY < 1)
                {
                    parX = player.File.DARX;
                    parY = player.File.DARY;
                }
                player.PARX = parX;
                player.PARY = parY;
                player.IntroCreditsFrameSet += new IntroCreditsFrameSetCallback(player_IntroCreditsFrameSet);
                player.Closed += new EventHandler(player_Closed);
                player.Show();
            }
        }


        private void VideoInput_DoubleClick(object sender, System.EventArgs e)
        {
            if (!VideoInput.Equals(""))
            {
                this.openAvisynthScript(VideoInput);
                if (this.creditsStartFrame > -1)
                    this.player.CreditsStart = creditsStartFrame;
                if (this.introEndFrame > -1)
                    this.player.IntroEnd = introEndFrame;
            }
        }
        private void videoConfigButton_Click(object sender, System.EventArgs e)
        {
            if (player != null)
                player.Hide();
            VideoCodecSettings settings = CurrentVideoCodecSettingsProvider.GetCurrentSettings();
            string selectedProfile;
            if (CurrentVideoCodecSettingsProvider.EditSettings(mainForm.Profiles, mainForm.Settings, this.VideoProfile.Text,
                this.VideoIO, new int[] { this.introEndFrame, this.creditsStartFrame }, out selectedProfile))
            {
                this.VideoProfile.Items.Clear();
                foreach (string name in mainForm.Profiles.VideoProfiles.Keys)
                {
                    this.VideoProfile.Items.Add(name);
                }
                int index = this.VideoProfile.Items.IndexOf(selectedProfile);
                if (index != -1)
                    this.VideoProfile.SelectedIndex = index;
            }
            if (player != null)
                player.Show();
            updateIOConfig();
        }
        private void queueVideoButton_Click(object sender, System.EventArgs e)
        {
            string settingsError = verifyVideoSettings();  // basic input, logfile and output file settings are okay
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            VideoCodecSettings vSettings = this.CurrentVideoCodecSettings.clone();
            bool start = mainForm.Settings.AutoStartQueue;
            start &= mainForm.JobUtil.AddVideoJobs(this.VideoIO[0], this.VideoIO[1], this.CurrentVideoCodecSettings.clone(),
                this.introEndFrame, this.creditsStartFrame, parX, parY, addPrerenderJob.Checked, true);
            if (start)
                mainForm.Jobs.startNextJobInQueue();
        }
        private void fileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            VideoType currentType = CurrentVideoOutputType;
            videoOutput.Filter = currentType.OutputFilterString;
            this.VideoOutput = Path.ChangeExtension(this.VideoOutput, currentType.Extension);
        }
        /// <summary>
        /// handles the selection of a profile from the list
        /// the profile is looked up from the profiles Hashtable (it uses the name as unique key), then
        /// the settings from the new profile are displayed in the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoProfile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.VideoProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                VideoProfile prof = mainForm.Profiles.VideoProfiles[this.VideoProfile.SelectedItem.ToString()];
                foreach (IVideoSettingsProvider p in this.VideoCodec.Items)
                {
                    if (p.IsSameType(prof.Settings))
                    {
                        p.LoadSettings(prof.Settings);
                        VideoCodec.SelectedItem = p;
                        break;
                    }
                }
            }
            updateIOConfig();
        }
        /// <summary>
        /// handles changes in the codec selection
        /// enables / disabled the proper GUI fields
        /// and changes the available fourCCs
        /// at the end, the proper encodingmode_changed method is triggered to ensure a proper GUI update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void codec_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            VideoType[] outputTypes = this.videoEncoderProvider.GetSupportedOutput(this.CurrentVideoCodecSettingsProvider.EncoderType);
            VideoType currentType = null;
            if (CurrentVideoOutputType != null)
                currentType = CurrentVideoOutputType;
            else
                currentType = outputTypes[0];
            this.fileType.Items.Clear();
            this.fileType.Items.AddRange(outputTypes);
            // now select the previously selected type again if possible
            bool selected = false;
            foreach (VideoType t in outputTypes)
            {
                if (currentType == t)
                {
                    this.fileType.SelectedItem = t;
                    currentType = t;
                    selected = true;
                    break;
                }
            }
            if (!selected)
            {
                currentType = outputTypes[0];
                this.fileType.SelectedItem = outputTypes[0];
            }
            VideoCodecSettings settings = CurrentVideoCodecSettingsProvider.GetCurrentSettings();
            this.updateIOConfig();
            if (MainForm.verifyOutputFile(this.VideoOutput) == null)
                this.VideoOutput = Path.ChangeExtension(this.VideoOutput, currentType.Extension);
        }
        /// <summary>
        /// enables / disables output fields depending on the codec configuration
        /// </summary>
        private void updateIOConfig()
        {
            int encodingMode = CurrentVideoCodecSettingsProvider.GetCurrentSettings().EncodingMode;
            if (encodingMode == 2 || encodingMode == 5) // first pass
            {
                videoOutput.Enabled = false;
            }
            else if (!videoOutput.Enabled)
            {
                videoOutput.Enabled = true;
            }
        }
        #endregion

        /// <summary>
        /// verifies the input, output and logfile configuration
        /// based on the codec and encoding mode certain fields must be filled out
        /// </summary>
        /// <returns>null if no error; otherwise string error message</returns>
        public string verifyVideoSettings()
        {
            // test for valid input filename
            string fileErr = MainForm.verifyInputFile(this.VideoInput);
            if (fileErr != null)
            {
                return "Problem with video input filename:\n" + fileErr;
            }

            // test for valid output filename (not needed if just doing 1st pass)
            if (!isFirstPass())
            {
                fileErr = MainForm.verifyOutputFile(this.VideoOutput);
                if (fileErr != null)
                {
                    return "Problem with video output filename:\n" + fileErr;
                }

                VideoType vot = CurrentVideoOutputType;
                // test output file extension
                if (!Path.GetExtension(this.VideoOutput).Replace(".", "").Equals(vot.Extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "Video output filename does not have the correct extension.\nBased on current settings, it should be "
                        + vot.Extension;
                }
            }
            return null;
        }
        /// <summary>
        ///  returns the video codec settings for the currently active video codec
        /// </summary>
        public VideoCodecSettings CurrentVideoCodecSettings
        {
            get
            {
                return CurrentVideoCodecSettingsProvider.GetCurrentSettings();
            }

            set
            {
                foreach (IVideoSettingsProvider p in VideoCodec.Items)
                {
                    if (p.IsSameType(value))
                    {
                        p.LoadSettings(value);
                        VideoCodec.SelectedItem = p;
                        break;
                    }
                }
            }
        }
        public MuxableType CurrentMuxableVideoType
        {
            get { return new MuxableType(CurrentVideoOutputType, CurrentVideoCodecSettings.Codec); }
        }
        public void openVideoFile(string fileName)
        {
            this.creditsStartFrame = -1;
            this.introEndFrame = -1;
            this.VideoInput = fileName;
            parX = parY = -1;
            //reset the zones for all codecs, zones are supposed to be source bound
            foreach (IVideoSettingsProvider p in (VideoCodec.Items))
            {
                VideoCodecSettings s = p.GetCurrentSettings();
                s.Zones = new Zone[0];
                p.LoadSettings(s);
            }
            if (mainForm.Settings.AutoOpenScript)
                openAvisynthScript(fileName);
            string filePath = Path.GetDirectoryName(fileName);
            string fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
            this.VideoOutput = Path.Combine(filePath, fileNameNoExtension) + mainForm.Settings.VideoExtension + ".extension";
            this.VideoOutput = Path.ChangeExtension(this.VideoOutput, this.CurrentVideoOutputType.Extension);
            updateIOConfig();
        }
        private bool isFirstPass()
        {
            VideoCodecSettings settings = CurrentVideoCodecSettings;
            if (settings.EncodingMode == 2 || settings.EncodingMode == 5)
                return true;
            else
                return false;
        }


        internal void ClosePlayer()
        {
            if (this.player != null)
            {
                player.Close();
                player = null;
            }
        }

        internal void hidePlayer()
        {
            if (player != null)
                player.Hide();
        }

        internal void showPlayer()
        {
            if (player != null)
                player.Show();
        }
        /// <summary>
        /// callback for the video player window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void player_Closed(object sender, EventArgs e)
        {
            parX = player.PARX;
            parY = player.PARY;
            this.player = null;
        }
        /// <summary>
        /// sets the intro end / credits start frame
        /// </summary>
        /// <param name="frameNumber">the frame number</param>
        /// <param name="isCredits">true if the credits start frame is to be set, false if the intro end is to be set</param>
        private void player_IntroCreditsFrameSet(int frameNumber, bool isCredits)
        {
            if (isCredits)
            {
                if (validateCredits(frameNumber))
                {
                    player.CreditsStart = frameNumber;
                    this.creditsStartFrame = frameNumber;
                }
                else
                    player.CreditsStart = -1;
            }
            else
            {
                if (validateIntro(frameNumber))
                {
                    this.introEndFrame = frameNumber;
                    player.IntroEnd = frameNumber;
                }
                else
                    player.IntroEnd = -1;
            }
        }
        /// <summary>
        /// iterates through all zones and makes sure we get no intersection by applying the current credits settings
        /// </summary>
        /// <param name="creditsStartFrame">the credits start frame</param>
        /// <returns>returns true if there is no intersetion between zones and credits and false if there is an intersection</returns>
        private bool validateCredits(int creditsStartFrame)
        {
            VideoCodecSettings settings = this.CurrentVideoCodecSettings;
            foreach (Zone z in settings.Zones)
            {
                if (creditsStartFrame <= z.endFrame) // credits start before end of this zone -> intersection
                {
                    MessageBox.Show("The start of the end credits intersects with an already configured zone\ngoing from frame " + z.startFrame + " to frame " + z.endFrame +
                        "\nPlease select another credits start frame or reconfigure the zone in the codec configuration.", "Zone intersection detected",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// iteratees through all zones and makes sure we get no intersection by applying the current intro settings
        /// </summary>
        /// <param name="introEndFrame">the frame where the intro ends</param>
        /// <returns>true if the intro zone does not interesect with a zone, false otherwise</returns>
        private bool validateIntro(int introEndFrame)
        {
            VideoCodecSettings settings = this.CurrentVideoCodecSettings;
            foreach (Zone z in settings.Zones)
            {
                if (introEndFrame >= z.startFrame)
                {
                    MessageBox.Show("The end of the intro intersects with an already configured zone\ngoing from frame " + z.startFrame + " to frame " + z.endFrame +
                        "\nPlease select another credits start frame or reconfigure the zone in the codec configuration.", "Zone intersection detected",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// returns video input and output configuration
        /// </summary>
        public string[] VideoIO
        {
            get
            {
                return new string[] { VideoInput, VideoOutput };
            }
        }
        public VideoEncoderProvider VideoEncoderProvider
        {
            get { return videoEncoderProvider; }
        }

        internal void Reset()
        {
            this.VideoInput = "";
            this.VideoOutput = "";
            this.creditsStartFrame = 0;
        }
    }
}
