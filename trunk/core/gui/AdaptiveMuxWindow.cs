using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.util;
using MeGUI.core.details;
using MeGUI.core.details.mux;

namespace MeGUI
{
    public partial class AdaptiveMuxWindow : baseMuxWindow
    {
        private MuxProvider muxProvider;
        private JobUtil jobUtil;
        private bool minimizedMode = false;
        private VideoEncoderType knownVideoType;
        private AudioEncoderType[] knownAudioTypes;

        public AdaptiveMuxWindow(MainForm mainForm)
        {
            InitializeComponent();
            jobUtil = new JobUtil(mainForm);
            muxProvider = mainForm.MuxProvider;

            audioTracks[0].Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.AudioTypes.ValuesArray);
            subtitleTracks[0].Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.SubtitleTypes.ValuesArray);
            vInput.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.VideoTypes.ValuesArray);
        }

        protected override void fileUpdated()
        {
            updatePossibleContainers();
        }

        private void containerFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            output.Filename = Path.ChangeExtension(output.Filename, (this.containerFormat.SelectedItem as ContainerType).Extension);

            if (containerFormat.SelectedItem is ContainerType)
                output.Filter = (containerFormat.SelectedItem as ContainerType).OutputFilterString;
            else
                output.Filter = "";
        }

        private void getTypes(out AudioEncoderType[] aCodec, out MuxableType[] audioTypes, out MuxableType[] subtitleTypes)
        {
            List<MuxableType> audioTypesList = new List<MuxableType>();
            List<MuxableType> subTypesList = new List<MuxableType>();
            List<AudioEncoderType> audioCodecList = new List<AudioEncoderType>();
            
            int counter = 0;
            foreach (MuxStreamControl c in audioTracks)
            {
                if (minimizedMode && knownAudioTypes.Length > counter)
                {
                    audioCodecList.Add(knownAudioTypes[counter]);
                }
                else if (c.Stream != null)
                {
                    AudioType audioType = VideoUtil.guessAudioType(c.Stream.path);
                    if (audioType != null)
                    {
                        subTypesList.Add(new MuxableType(audioType, null));
                    }
                }
                counter++;
            }
            foreach (MuxStreamControl c in subtitleTracks)
            {
                if (c.Stream == null) continue;
                SubtitleType subtitleType = VideoUtil.guessSubtitleType(c.Stream.path);
                if (subtitleType != null)
                {
                    subTypesList.Add(new MuxableType(subtitleType, null));
                }
            }
            audioTypes = audioTypesList.ToArray();
            subtitleTypes = subTypesList.ToArray();
            aCodec = audioCodecList.ToArray();
        }


        protected override void checkIO()
        {
            base.checkIO();
            if (!(containerFormat.SelectedItem is ContainerType))
                muxButton.DialogResult = DialogResult.None;
        }

        private void updatePossibleContainers()
        {
            MuxableType videoType;
            if (minimizedMode)
                videoType = null;
            else
            {
                videoType = VideoUtil.guessVideoMuxableType(vInput.Filename, true);
                if (videoType != null && (videoType.codec == null || videoType.outputType == null))
                {
                    MessageBox.Show("Unable to determine type of input video. Mux-path finding cannot continue. Your video could well be corrupt.", "Determining type failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!minimizedMode && videoType == null)
            {
                this.containerFormat.Items.Clear();
                this.containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
                this.containerFormat.SelectedIndex = 0;
                return;
            }

            MuxableType[] audioTypes;
            MuxableType[] subTypes;
            AudioEncoderType[] audioCodecs;
            getTypes(out audioCodecs, out audioTypes, out subTypes);

            List<MuxableType> allTypes = new List<MuxableType>();
            if (videoType != null)
                allTypes.Add(videoType);
            allTypes.AddRange(audioTypes);
            allTypes.AddRange(subTypes);

            List<ContainerType> supportedOutputTypes;

            if (minimizedMode)
            {
                supportedOutputTypes = this.muxProvider.GetSupportedContainers(knownVideoType, audioCodecs,
                    allTypes.ToArray());
            }
            else
            {
                supportedOutputTypes = this.muxProvider.GetSupportedContainers(allTypes.ToArray());
            }

            ContainerType lastSelectedFileType = null;
            if (containerFormat.SelectedItem is ContainerType)
                lastSelectedFileType = containerFormat.SelectedItem as ContainerType;

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

        public JobChain Jobs
        {
            get
            {
                if (minimizedMode)
                    throw new Exception("Jobs property not accessible in minimized mode");
                
                VideoStream myVideo = new VideoStream();
                myVideo.Input = "";
                myVideo.Output = vInput.Filename;
                myVideo.NumberOfFrames = 1000; // Just a guess, since we have no way of actually knowing
                myVideo.VideoType = VideoUtil.guessVideoMuxableType(vInput.Filename, true);
                myVideo.Settings = new x264Settings();
                myVideo.Settings.NbBframes = 0;

                MuxableType[] audioTypes;
                MuxableType[] subtitleTypes;
                AudioEncoderType[] audioCodecs;
                MuxStream[] audioStreams, subtitleStreams;
                getTypes(out audioCodecs, out audioTypes, out subtitleTypes);
                string chapters;
                getAdditionalStreams(out audioStreams, out subtitleStreams, out chapters);

                FileSize? splitSize = splitting.Value;

                MuxableType chapterInputType = null;
                if (!String.IsNullOrEmpty(this.chapters.Filename))
                {
                    ChapterType type = VideoUtil.guessChapterType(this.chapters.Filename);
                    if (type != null)
                        chapterInputType = new MuxableType(type, null);
                }

                return jobUtil.GenerateMuxJobs(myVideo, fps.Value, audioStreams, audioTypes, subtitleStreams,
                    subtitleTypes, this.chapters.Filename, chapterInputType, (containerFormat.SelectedItem as ContainerType), output.Filename, splitSize, false);
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
        public void setMinimizedMode(string videoInput, VideoEncoderType videoType, double framerate, MuxStream[] audioStreams, AudioEncoderType[] audioTypes, string output,
            FileSize? splitSize, ContainerType cft)
        {
            base.setConfig(videoInput, (decimal)framerate, audioStreams, new MuxStream[0], null, output, splitSize, null);

            minimizedMode = true;
            knownVideoType = videoType;
            knownAudioTypes = audioTypes;

            // disable everything
            videoGroupbox.Enabled = false;

            for (int i = 0; i < audioStreams.Length; ++i)
                audioTracks[i].Enabled = false;

            this.output.Filename = output;
            this.splitting.Value = splitSize;
            this.muxButton.Text = "Go";
            updatePossibleContainers();
            if (this.containerFormat.Items.Contains(cft))
                containerFormat.SelectedItem = cft;
            checkIO();
        }

        public void getAdditionalStreams(out MuxStream[] audio, out MuxStream[] subtitles, out string chapters, out string output, out ContainerType cot)
        {
            cot = (containerFormat.SelectedItem as ContainerType);
            output = this.output.Filename;
            base.getAdditionalStreams(out audio, out subtitles, out chapters);
        }
    }
}