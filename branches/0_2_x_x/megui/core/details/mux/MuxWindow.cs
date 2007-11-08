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
using MeGUI.core.details;

namespace MeGUI
{
    public partial class MuxWindow : baseMuxWindow
    {
        public static readonly IDable<ReconfigureJob> Configurer = new IDable<ReconfigureJob>("mux_reconfigure", new ReconfigureJob(ReconfigureJob));

        private static Job ReconfigureJob(Job j)
        {
            if (!(j is MuxJob))
                return null;

            MuxJob m = (MuxJob)j;
            MuxWindow w = new MuxWindow(
                MainForm.Instance.MuxProvider.GetMuxer(m.MuxType), 
                MainForm.Instance);

            w.Job = m;
            if (w.ShowDialog() == DialogResult.OK)
                return w.Job;
            else
                return m;
        }

        private IMuxing muxer;
        private MainForm mainForm;

        public MuxWindow(IMuxing muxer, MainForm mainForm)
            : base()
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.muxer = muxer;
            if (muxer.GetSupportedAudioTypes().Count == 0)
                audio.Enabled = false;
            if (muxer.GetSupportedChapterTypes().Count == 0)
                chaptersGroupbox.Enabled = false;
            if (muxer.GetSupportedSubtitleTypes().Count == 0)
                subtitles.Enabled = false;
            if (muxer.GetSupportedChapterTypes().Count == 0)
                chaptersGroupbox.Enabled = false;
            muxedInput.Filter = muxer.GetMuxedInputFilter();

            audioTracks[0].Filter = muxer.GetAudioInputFilter();
            output.Filter = muxer.GetOutputTypeFilter();
            subtitleTracks[0].Filter = muxer.GetSubtitleInputFilter();
            vInput.Filter = muxer.GetVideoInputFilter();
        }

        protected virtual MuxJob generateMuxJob()
        {
            MuxJob job = new MuxJob();
            string chapters;
            MuxStream[] aStreams, sStreams;
            getAdditionalStreams(out aStreams, out sStreams, out chapters);
            job.Settings.AudioStreams.AddRange(aStreams);
            job.Settings.SubtitleStreams.AddRange(sStreams);
            job.Settings.ChapterFile = this.chapters.Filename;
            job.Settings.VideoName = this.videoName.Text;
            job.Settings.VideoInput = vInput.Filename;
            job.Settings.MuxedOutput = output.Filename;
            job.Settings.MuxedInput = this.muxedInput.Filename;
            job.Settings.DAR = base.dar;
            
            if (string.IsNullOrEmpty(job.Settings.VideoInput))
                job.Input = job.Settings.MuxedInput;
            else
                job.Input = job.Settings.VideoInput;

            job.Output = job.Settings.MuxedOutput;
            job.MuxType = muxer.MuxerType;
            job.ContainerType = getContainerType(job.Settings.MuxedOutput);
            job.Settings.Framerate = fps.Value;
            
            Debug.Assert(!splitting.Value.HasValue || splitting.Value.Value >= new FileSize(Unit.MB, 1));
            job.Settings.SplitSize = splitting.Value;
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

        private void setConfig(string videoInput, string muxedInput, decimal? framerate, MuxStream[] audioStreams,
            MuxStream[] subtitleStreams, string chapterFile, string output, FileSize? splitSize, Dar? dar)
        {
            base.setConfig(videoInput, framerate, audioStreams, subtitleStreams, chapterFile, output, splitSize, dar);
            this.muxedInput.Filename = muxedInput;
            this.checkIO();
        }

        protected override void ChangeOutputExtension()
        {
            foreach (ContainerType t in muxer.GetSupportedContainers())
            {
                if (output.Filename.ToLower().EndsWith(t.Extension.ToLower()))
                    return;
            }
            output.Filename = Path.ChangeExtension(output.Filename, muxer.GetSupportedContainers()[0].Extension);
        }

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

        protected override bool isFPSRequired()
        {
            if (vInput.Filename.Length > 0)
                return base.isFPSRequired();
            else if (muxedInput.Filename.Length > 0)
                return false;
            else
                return true;
        }

        private void muxedInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            checkIO();
            fileUpdated();
        }
    }
}