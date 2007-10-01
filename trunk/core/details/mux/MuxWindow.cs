using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MeGUI.core.util;

namespace MeGUI
{
    public partial class MuxWindow : baseMuxWindow
    {
        private IMuxing muxer;
        private MainForm mainForm;
        private CommandLineGenerator gen = new CommandLineGenerator();

        public MuxWindow(IMuxing muxer, MainForm mainForm) : base()
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.muxer = muxer;
            if (muxer.GetSupportedAudioTypes().Count == 0)
                audioGroupbox.Enabled = false;
            if (muxer.GetSupportedChapterTypes().Count == 0)
                chaptersGroupbox.Enabled = false;
            if (muxer.GetSupportedSubtitleTypes().Count == 0)
                subtitleGroupbox.Enabled = false;
            if (muxer.GetSupportedContainerInputTypes().Count == 0)
                muxedInputOpenButton.Enabled = false;
            if (muxer.GetSupportedChapterTypes().Count == 0)
                chaptersGroupbox.Enabled = false;
        }
        protected virtual MuxJob generateMuxJob()
        {
            MuxJob job = new MuxJob();
            convertLanguagesToISO();
            foreach (SubStream stream in audioStreams)
            {
                job.Settings.AudioStreams.Add(stream);
            }
            foreach (SubStream stream in subtitleStreams)
            {
                job.Settings.SubtitleStreams.Add(stream);
            }
            job.Settings.ChapterFile = this.chaptersInput.Text;
            job.Settings.VideoName = this.videoName.Text;
            job.Settings.VideoInput = this.videoInput.Text;
            job.Settings.MuxedOutput = muxedOutput.Text;
            job.Settings.MuxedInput = this.muxedInput.Text;
            job.Settings.DAR = base.dar;
            
            if (string.IsNullOrEmpty(job.Settings.VideoInput))
                job.Input = job.Settings.MuxedInput;
            else
                job.Input = job.Settings.VideoInput;

            job.Output = job.Settings.MuxedOutput;
            job.MuxType = muxer.MuxerType;
            job.ContainerType = getContainerType(job.Settings.MuxedOutput);
            if ((!job.Settings.MuxedInput.Equals("") || !job.Settings.VideoInput.Equals(""))
                && !job.Settings.MuxedOutput.Equals(""))
            {
                if (this.muxFPS.SelectedIndex != -1)
                    job.Settings.Framerate = Double.Parse(muxFPS.Text);
                if (this.muxFPS.SelectedIndex != -1 || !isFPSRequired())
                {
                    if (this.enableSplit.Checked && !splitSize.Text.Equals(""))
                        job.Settings.SplitSize = new FileSize(Unit.MB, Int32.Parse(this.splitSize.Text));
                    job.Commandline = CommandLineGenerator.generateMuxCommandline(job.Settings, job.MuxType, mainForm);
                }
            }
            return job;
        }

        public MuxJob Job
        {
            get { return generateMuxJob(); }
            set
            {
                setConfig(value.Settings.VideoInput, value.Settings.MuxedInput, value.Settings.Framerate,
                    value.Settings.AudioStreams.ToArray(), value.Settings.SubtitleStreams.ToArray(),
                    value.Settings.ChapterFile, value.Settings.MuxedOutput, value.Settings.SplitSize,
                    value.Settings.DAR);
            }
        }

        private void setConfig(string videoInput, string muxedInput, double framerate, SubStream[] audioStreams,
            SubStream[] subtitleStreams, string chapterFile, string output, FileSize? splitSize, Dar? dar)
        {
            base.setConfig(videoInput, framerate, audioStreams, subtitleStreams, chapterFile, output, splitSize, dar);
            this.muxedInput.Text = muxedInput;
            this.checkIO();
        }

        #region overriden filters
        public override string AudioFilter
        {
            get
            {
                return muxer.GetAudioInputFilter();
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
                return muxer.GetOutputTypeFilter();
            }
        }
        public override string SubtitleFilter
        {
            get
            {
                return muxer.GetSubtitleInputFilter();
            }
        }
        public override string VideoInputFilter
        {
            get
            {
                return muxer.GetVideoInputFilter();
            }
        }
        public string MuxedInputFilter
        {
            get
            {
                return muxer.GetMuxedInputFilter();
            }
        }
        #endregion

        private ContainerType getContainerType(string outputFilename)
        {
            Debug.Assert(outputFilename != null);
            foreach (ContainerType t in muxer.GetSupportedContainers())
            {
                if (outputFilename.ToLower().EndsWith(t.Extension.ToLower()))
                    return t;
            }
            Debug.Assert(false);
            return null;
        }

        private void muxedInputOpenButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = MuxedInputFilter;
            openFileDialog.Title = "Select your already-muxed file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                muxedInput.Text = openFileDialog.FileName;
                checkIO();
                fileUpdated();
            }
        }

        protected override void checkIO()
        {
            if (videoInput.Text.Equals("") && muxedInput.Text.Equals(""))
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (muxedOutput.Text.Equals(""))
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (muxFPS.SelectedIndex == -1 && isFPSRequired())
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else
                muxButton.DialogResult = DialogResult.OK;
        }

        protected override bool isFPSRequired()
        {
            if (videoInput.Text.Length > 0)
                return base.isFPSRequired();
            else if (muxedInput.Text.Length > 0)
                return false;
            else
                return true;
        }

        protected override void muxButton_Click(object sender, System.EventArgs e)
        {
            if (muxButton.DialogResult != DialogResult.OK)
            {
                if (videoInput.Text.Equals("") && muxedInput.Text.Equals(""))
                {
                    MessageBox.Show("You must configure a video input file", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (muxedOutput.Text.Equals(""))
                {
                    MessageBox.Show("You must configure an output file", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (muxFPS.SelectedIndex == -1 && isFPSRequired())
                {
                    MessageBox.Show("You must select a framerate", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }
    }
}