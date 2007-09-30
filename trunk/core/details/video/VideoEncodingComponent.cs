using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;
using MeGUI.core.details.video;
using MeGUI.packages.video;

namespace MeGUI
{
    public partial class VideoEncodingComponent : UserControl
    {
        private VideoPlayer player; // window that shows a preview of the video
        private MainForm mainForm;

        #region video info
        private VideoInfo info;
        private void initVideoInfo()
        {
            info = new VideoInfo();
            info.VideoInputChanged += new StringChanged(delegate(object _, string val)
            {
                videoInput.Filename = val;
            });
            info.VideoOutputChanged += new StringChanged(delegate(object _, string val)
            {
                videoOutput.Filename = val;
            });
        }
        public VideoInfo Info
        {
            get
            {
                return info;
            }
        }
        #endregion
        #region generic handlers: filetype, profiles and codec. Also, encoder provider
        private FileTypeHandler<VideoType> fileTypeHandler;
        public FileTypeHandler<VideoType> FileTypeHandler
        {
            get { return fileTypeHandler; }
        }


        private ProfilesControlHandler<VideoCodecSettings, VideoInfo> profileHandler;
        public ProfilesControlHandler<VideoCodecSettings, VideoInfo> ProfileHandler
        {
            get { return profileHandler; }
        }

        public VideoCodecSettings CurrentSettings
        {
            get { return codecHandler.Getter(); }
        }
	
        private VideoEncoderProvider videoEncoderProvider = new VideoEncoderProvider();
        public VideoEncodingComponent()
        {
            initVideoInfo();
            InitializeComponent();

        }
        #endregion
        #region wrappers for the above handlers
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> CurrentSettingsProvider
        {
            get { return codecHandler.CurrentSettingsProvider; }
            set { codecHandler.CurrentSettingsProvider = value; }
        }

        private MultipleConfigurersHandler<VideoCodecSettings, VideoInfo, MeGUI.VideoCodec, VideoEncoderType> codecHandler;
        public MultipleConfigurersHandler<VideoCodecSettings, VideoInfo, MeGUI.VideoCodec, VideoEncoderType> CodecHandler
        {
            get { return codecHandler; }
        }
        #endregion
        #region initialization of those handlers
        public  VideoInfo getInfo()
        {
            return Info;
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


            fileTypeHandler = new FileTypeHandler<VideoType>(fileType, videoCodec,
                new FileTypeHandler<VideoType>.SupportedOutputGetter(delegate
            {
                return videoEncoderProvider.GetSupportedOutput(codecHandler.CurrentSettingsProvider.EncoderType);
            }));

            fileTypeHandler.FileTypeChanged += new FileTypeHandler<VideoType>.FileTypeEvent(delegate
            (object sender, VideoType currentType) {
                VideoCodecSettings settings = CurrentSettings;
                this.updateIOConfig();
                if (MainForm.verifyOutputFile(this.VideoOutput) == null)
                    this.VideoOutput = Path.ChangeExtension(this.VideoOutput, currentType.Extension);
            });

            codecHandler =
                new MultipleConfigurersHandler<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>(videoCodec);
            
            profileHandler = new ProfilesControlHandler<VideoCodecSettings, VideoInfo>("Video", mainForm, profileControl1,
                codecHandler.EditSettings, new InfoGetter<VideoInfo>(getInfo),
                codecHandler.Getter, codecHandler.Setter);
            codecHandler.Register(profileHandler);
            fileTypeHandler.RefreshFiletypes();
        }
        #endregion
        #region extra properties
        public MainForm MainForm
        {
            set { mainForm = value; }
        }
        public string VideoInput
        {
            get { return info.VideoInput; }
            set { info.VideoInput = value; }
        }
        public string VideoOutput
        {
            get { return info.VideoOutput; }
            set { info.VideoOutput = value; }
        }
/*        public ComboBox VideoProfile
        {
            get { return null; /* throw new Exception("NOt implemented"); *//*}
        }*/
        public VideoType CurrentVideoOutputType
        {
            get { return this.fileType.SelectedItem as VideoType; }
        }
        public bool PrerenderJob
        {
            get { return addPrerenderJob.Checked; }
            set { addPrerenderJob.Checked = value; }
        }
/*        public ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> CurrentVideoCodecSettingsProvider
        {
            get
            {
                return videoCodec.SelectedItem as ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>;
            }
        }
        */
        #endregion
        #region event handlers
        private void videoInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (!string.IsNullOrEmpty(videoInput.Filename)) openVideoFile(videoInput.Filename);
        }

        private void videoOutput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            info.VideoOutput = videoOutput.Filename;
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
                info.DAR = info.DAR ?? player.File.Info.DAR;
                player.DAR = info.DAR;

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
                if (info.CreditsStartFrame > -1)
                    this.player.CreditsStart = info.CreditsStartFrame;
                if (info.IntroEndFrame > -1)
                    this.player.IntroEnd = info.IntroEndFrame;
            }
        }
/*        private void videoConfigButton_Click(object sender, System.EventArgs e)
        {
#warning fix this
            /*if (player != null)
                player.Hide();
            VideoCodecSettings settings = CurrentVideoCodecSettingsProvider.GetCurrentSettings();
            string selectedProfile;
            if (CurrentVideoCodecSettingsProvider.EditSettings(mainForm, this.VideoProfile.Text,
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
            updateIOConfig();*/
        //}
        private void queueVideoButton_Click(object sender, System.EventArgs e)
        {
            string settingsError = verifyVideoSettings();  // basic input, logfile and output file settings are okay
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            VideoCodecSettings vSettings = this.CurrentVideoCodecSettings.clone();
            mainForm.JobUtil.AddVideoJobs(info.VideoInput, info.VideoOutput, this.CurrentVideoCodecSettings.clone(),
                info.IntroEndFrame, info.CreditsStartFrame, info.DAR, addPrerenderJob.Checked, true);
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
/*        private void VideoProfile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this..SelectedIndex != -1) // if it's -1 it's bogus
            {
                GenericProfile<VideoCodecSettings> prof = (GenericProfile<VideoCodecSettings>)mainForm.Profiles.VideoProfiles[this.VideoProfile.SelectedItem.ToString()];
                foreach (ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> p in this.VideoCodec.Items)
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
        }*/
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
/*            VideoType[] outputTypes = this.videoEncoderProvider.GetSupportedOutput(codecHandler.CurrentSettingsProvider.EncoderType);
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
            VideoCodecSettings settings = CurrentSettings;
            this.updateIOConfig();
            if (MainForm.verifyOutputFile(this.VideoOutput) == null)
                this.VideoOutput = Path.ChangeExtension(this.VideoOutput, currentType.Extension);*/
        }
        /// <summary>
        /// enables / disables output fields depending on the codec configuration
        /// </summary>
        private void updateIOConfig()
        {
            int encodingMode = CurrentSettings.EncodingMode;
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
        #region verification
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
        #endregion
        #region helpers
        /// <summary>
        ///  returns the video codec settings for the currently active video codec
        /// </summary>
        public VideoCodecSettings CurrentVideoCodecSettings
        {
#warning remove
            get
            {
                return CurrentSettings;
            }
/*
            set
            {
                foreach (ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> p in VideoCodec.Items)
                {
                    if (p.IsSameType(value))
                    {
                        p.LoadSettings(value);
                        VideoCodec.SelectedItem = p;
                        break;
                    }
                }
            }*/
        }
        public MuxableType CurrentMuxableVideoType
        {
            get { return new MuxableType(CurrentVideoOutputType, CurrentVideoCodecSettings.Codec); }
        }
        public void openVideoFile(string fileName)
        {
            info.CreditsStartFrame = -1;
            info.IntroEndFrame = -1;
            info.VideoInput = fileName;
            info.DAR = null;
            //reset the zones for all codecs, zones are supposed to be source bound
            foreach (ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> p in (videoCodec.Items))
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
        #endregion
        #region player info

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
            info.DAR = player.DAR;
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
                    info.CreditsStartFrame = frameNumber;
                }
                else
                    player.CreditsStart = -1;
            }
            else
            {
                if (validateIntro(frameNumber))
                {
                    info.IntroEndFrame = frameNumber;
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
        #endregion
        #region misc
        public VideoEncoderProvider VideoEncoderProvider
        {
            get { return videoEncoderProvider; }
        }

        internal void Reset()
        {
            this.VideoInput = "";
            this.VideoOutput = "";
            info.CreditsStartFrame = 0;
        }

        public void RefreshProfiles()
        {
            ProfileHandler.RefreshProfiles();
        }
        #endregion

        private void addAnalysisPass_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(VideoInput))
            {
                MessageBox.Show("Error: Could not add job to queue. Make sure that all the details are entered correctly", "Couldn't create job", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AviSynthJob job = mainForm.JobUtil.generateAvisynthJob(VideoInput);
            mainForm.Jobs.addJobsToQueue(job);
        }

		// added simple drag&drop support
		#region drag&drop
        private void videoInput_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void videoInput_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            Array data = e.Data.GetData("FileDrop") as Array;
            if (data != null)
            {
                if (data.GetValue(0) is String)
                {
                    string filename = ((string[])data)[0];

                    if (Path.GetExtension(filename) == ".avs")
                    {
                        videoInput.Filename = filename;
                        openVideoFile(videoInput.Filename);
                    }
                }
            }
        }
		#endregion
    }
}
