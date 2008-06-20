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
using MeGUI.core.util;

namespace MeGUI
{
    public partial class VideoEncodingComponent : UserControl
    {
        private VideoPlayer player; // window that shows a preview of the video
        private MainForm mainForm = MainForm.Instance;

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


        public VideoCodecSettings CurrentSettings
        {
            get
            {
                return (VideoCodecSettings)videoProfile.SelectedProfile.BaseSettings;
            }
        }
	
        private VideoEncoderProvider videoEncoderProvider = new VideoEncoderProvider();
        public VideoEncodingComponent()
        {
            initVideoInfo();
            InitializeComponent();
            videoProfile.Manager = MainForm.Instance.Profiles;

        }
        #endregion
        #region extra properties
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
        public VideoType CurrentVideoOutputType
        {
            get { return this.fileType.SelectedItem as VideoType; }
        }
        public bool PrerenderJob
        {
            get { return addPrerenderJob.Checked; }
            set { addPrerenderJob.Checked = value; }
        }
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
        /// <param name="fileName">the AviSynth script to be opened</param>
        private void openAvisynthScript(string fileName)
        {
            if (this.player != null) // make sure only one preview window is open at all times
                player.Close();
            player = new VideoPlayer();
            info.DAR = null; // to be sure to initialize DAR values
            bool videoLoaded = player.loadVideo(mainForm, fileName, PREVIEWTYPE.CREDITS, true);
            if (videoLoaded)
            {
                info.DAR = info.DAR ?? player.File.Info.DAR;
                player.DAR = info.DAR;

                player.IntroCreditsFrameSet += new IntroCreditsFrameSetCallback(player_IntroCreditsFrameSet);
                player.Closed += new EventHandler(player_Closed);
                player.Show();
                if (mainForm.Settings.AlwaysOnTop) player.TopMost = true;
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
        private void queueVideoButton_Click(object sender, System.EventArgs e)
        {
            string settingsError = verifyVideoSettings();  // basic input, logfile and output file settings are okay
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            VideoCodecSettings vSettings = this.CurrentSettings.Clone();
            mainForm.JobUtil.AddVideoJobs(info.VideoInput, info.VideoOutput, this.CurrentSettings.Clone(),
                info.IntroEndFrame, info.CreditsStartFrame, info.DAR, PrerenderJob, true);
        }
        private void fileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            videoOutput.Filter = CurrentVideoOutputType.OutputFilterString;
            this.VideoOutput = Path.ChangeExtension(this.VideoOutput, CurrentVideoOutputType.Extension);
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
        public MuxableType CurrentMuxableVideoType
        {
            get { return new MuxableType(CurrentVideoOutputType, CurrentSettings.Codec); }
        }
        public void openVideoFile(string fileName)
        {
            info.CreditsStartFrame = -1;
            info.IntroEndFrame = -1;
            info.VideoInput = fileName;
            info.DAR = null;

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
            VideoCodecSettings settings = CurrentSettings;
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
            VideoCodecSettings settings = this.CurrentSettings;
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
            VideoCodecSettings settings = this.CurrentSettings;
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
        #endregion

        private void addAnalysisPass_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(VideoInput))
            {
                MessageBox.Show("Error: Could not add job to queue. Make sure that all the details are entered correctly", "Couldn't create job", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AviSynthJob job = new AviSynthJob(VideoInput);
            mainForm.Jobs.addJobsToQueue(new AviSynthJob(VideoInput));
        }

        VideoEncoderType lastCodec = null;
        private void videoProfile_SelectedProfileChanged(object sender, EventArgs e)
        {
            if (CurrentSettings.EncoderType == lastCodec)
                return;

            lastCodec = CurrentSettings.EncoderType;
            Util.ChangeItemsKeepingSelectedSame(fileType, videoEncoderProvider.GetSupportedOutput(lastCodec));

        }
    }
}
