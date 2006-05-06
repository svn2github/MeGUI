using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class AdaptiveMuxWindow : baseMuxWindow
    {
        private MuxProvider muxProvider = new MuxProvider();
        private JobUtil jobUtil;
        public AdaptiveMuxWindow(MeGUI mainForm)
        {
            InitializeComponent();
            jobUtil = new JobUtil(mainForm);
        }

        protected override void fileUpdated()
        {
            updatePossibleContainers();
        }

        private void containerFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.muxedOutput.Text = Path.ChangeExtension(this.muxedOutput.Text, (this.containerFormat.SelectedItem as OutputType).Extension);
        }

        private void getTypes(out AudioType[] audioTypes, out SubtitleType[] subtitleTypes)
        {
            List<AudioType> audioTypesList = new List<AudioType>();
            List<SubtitleType> subTypesList = new List<SubtitleType>();

            foreach (SubStream stream in audioStreams)
            {
                AudioType audioType = VideoUtil.guessAudioType(stream.path);
                if (audioType != null)
                {
                    audioTypesList.Add(audioType);
                }
            }
            foreach (SubStream stream in subtitleStreams)
            {
                SubtitleType subtitleType = VideoUtil.guessSubtitleType(stream.path);
                if (subtitleType != null)
                {
                    subTypesList.Add(subtitleType);
                }
            }
            audioTypes = audioTypesList.ToArray();
            subtitleTypes = subTypesList.ToArray();
        }

        private void getStreams(out SubStream[] audioStreams, out SubStream[] subtitleStreams)
        {
            List<SubStream> audioStreamsList = new List<SubStream>();
            List<SubStream> subtitleStreamList = new List<SubStream>();
            foreach (SubStream stream in this.audioStreams)
            {
                if (!string.IsNullOrEmpty(stream.path))
                    audioStreamsList.Add(stream);
            }
            foreach (SubStream stream in this.subtitleStreams)
            {
                if (!string.IsNullOrEmpty(stream.path))
                    subtitleStreamList.Add(stream);
            }
            audioStreams = audioStreamsList.ToArray();
            subtitleStreams = subtitleStreamList.ToArray();
        }

        protected override void checkIO()
        {
            base.checkIO();
            if (!(containerFormat.SelectedItem is ContainerFileType))
                muxButton.DialogResult = DialogResult.None;
        }

        private void updatePossibleContainers()
        {
            VideoType videoType = VideoUtil.guessVideoType(videoInput.Text);
            if (videoType == null)
            {
                this.containerFormat.Items.Clear();
                this.containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
                this.containerFormat.SelectedIndex = 0;
                return;
            }

            AudioType[] audioTypes;
            SubtitleType[] subTypes;
            getTypes(out audioTypes, out subTypes);

            List<ContainerFileType> supportedOutputTypes = this.muxProvider.GetSupportedContainers(
                videoType, audioTypes, subTypes, audioTypes.Length + subTypes.Length + 1);

            ContainerFileType lastSelectedFileType = null;
            if (containerFormat.SelectedItem is ContainerFileType)
                lastSelectedFileType = containerFormat.SelectedItem as ContainerFileType;

            if (supportedOutputTypes.Count > 0)
            {
                this.containerFormat.Items.Clear();
                this.containerFormat.Items.AddRange(supportedOutputTypes.ToArray());
                this.containerFormat.SelectedIndex = 0;
                if (lastSelectedFileType != null && containerFormat.Items.Contains(lastSelectedFileType))
                    containerFormat.SelectedItem = lastSelectedFileType;
            }
            else
            {
                this.containerFormat.Items.Clear();
                MessageBox.Show("No muxer can be found that supports this input configuration", "Muxing impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public MuxJob[] Jobs
        {
            get
            {
                double framerate = -1;
                if (this.muxFPS.SelectedIndex != -1)
                    framerate = Double.Parse(muxFPS.Text);

                VideoStream myVideo = new VideoStream();
                myVideo.Input = "";
                myVideo.Output = videoInput.Text;
                myVideo.NumberOfFrames = 1000; // Just a guess, since we have no way of actually knowing
                myVideo.Framerate = framerate;
                myVideo.VideoType = VideoUtil.guessVideoType(videoInput.Text);
                myVideo.Settings = new x264Settings();
                myVideo.Settings.NbBframes = 0;

                AudioType[] audioTypes;
                SubtitleType[] subtitleTypes;
                SubStream[] audioStreams, subtitleStreams;
                getTypes(out audioTypes, out subtitleTypes);
                getStreams(out audioStreams, out subtitleStreams);

                int splitSize = -1;
                if (enableSplit.Checked)
                {
                    if (!int.TryParse(this.splitSize.Text, out splitSize))
                        splitSize = -1;
                }

                return jobUtil.GenerateMuxJobs(myVideo, audioStreams, audioTypes, subtitleStreams,
                    subtitleTypes, chaptersInput.Text, (containerFormat.SelectedItem as ContainerFileType).ContainerType, muxedOutput.Text, splitSize);
            }
        }
        /// <summary>
        /// sets the GUI to a minimal mode allowing to configure audio track languages, configure subtitles, and chapters
        /// the rest of the options are deactivated
        /// </summary>
        /// <param name="videoInput">the video input</param>
        /// <param name="framerate">the framerate of the video input</param>
        /// <param name="audioStreams">the audio streams whose languages have to be assigned</param>
        /// <param name="output">the output file</param>
        /// <param name="splitSize">the output split size</param>
        public void setMinimizedMode(string videoInput, double framerate, SubStream[] audioStreams, string output,
            int splitSize, ContainerFileType cft)
        {
            videoGroupbox.Enabled = false;
            this.videoInput.Text = videoInput;
            int fpsIndex = muxFPS.Items.IndexOf(framerate);
            if (fpsIndex != -1)
                muxFPS.SelectedIndex = fpsIndex;
            if (audioStreams.Length == 1) // 1 stream predefined
            {
                preconfigured = new bool[] { true, false };
                this.audioStreams[0] = audioStreams[0];
                audioInputOpenButton.Enabled = false;
                removeAudioTrackButton.Enabled = false;
                audioInput.Text = audioStreams[0].path;
            }
            else if (audioStreams.Length == 2) // both streams are defined, disable audio opening facilities
            {
                preconfigured = new bool[] { true, true };
                this.audioStreams = audioStreams;
                removeAudioTrackButton.Enabled = false;
                audioInput.Text = audioStreams[0].path;
                audioInputOpenButton.Enabled = false;
                removeAudioTrackButton.Enabled = false;
            }
            else // no audio tracks predefined
            {
                preconfigured = new bool[] { false, false };
            }
            muxedOutput.Text = output;
            this.splitSize.Text = splitSize.ToString();
            if (splitSize > 0)
                enableSplit.Checked = true;
            this.muxButton.Text = "Go";
            checkIO();
            if (this.containerFormat.Items.Contains(cft))
                containerFormat.SelectedItem = cft;
        }
        #region filters
        public override string AudioFilter
        {
            get
            {
                return VideoUtil.GenerateCombinedFilter(ContainerManager.GetContainerManager().AudioTypes);
            }
        }
        public override string ChaptersFilter
        {
            get
            {
                return "Chapter files (*.txt)|*.txt";
            }
        }
        public override string OutputFilter
        {
            get
            {
                if (containerFormat.SelectedItem is ContainerFileType)
                {
                    return (containerFormat.SelectedItem as ContainerFileType).OutputFilterString;
                }
                else
                    return "";
            }
        }
        public override string SubtitleFilter
        {
            get
            {
                return VideoUtil.GenerateCombinedFilter(ContainerManager.GetContainerManager().SubtitleTypes);
            }
        }
        public override string VideoInputFilter
        {
            get
            {
                return VideoUtil.GenerateCombinedFilter(ContainerManager.GetContainerManager().VideoTypes);
            }
        }
        #endregion

        public void getAdditionalStreams(out SubStream[] audio, out SubStream[] subtitles, out string chapters, out string output, out ContainerFileType cot)
        {
            cot = (containerFormat.SelectedItem as ContainerFileType);
            output = muxedOutput.Text;
            base.getAdditionalStreams(out audio, out subtitles, out chapters);
        }
    }
}