using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using MeGUI.core.plugins.implemented;
using MeGUI.core.util;

namespace MeGUI
{
    public delegate void EncoderOutputCallback(string line, int type);

    public abstract class CommandlineVideoEncoder : CommandlineJobProcessor<VideoJob>
    {
        #region variables
        protected int lastStatusUpdateFramePosition = 0;
        ulong numberOfFrames;
        ulong? currentFrameNumber;
        private int hres = 0, vres = 0;
        Dar? dar;
        protected bool usesSAR = false;
        #endregion
        public CommandlineVideoEncoder() : base()
        {
        }
        #region helper methods
        protected override void checkJobIO()
        {
            base.checkJobIO();

            su.Status = "Encoding video...";
            getInputProperties(job);
        }
        /// <summary>
        /// tries to open the video source and gets the number of frames from it, or 
        /// exits with an error
        /// </summary>
        /// <param name="videoSource">the AviSynth script</param>
        /// <param name="error">return parameter for all errors</param>
        /// <returns>true if the file could be opened, false if not</returns>
        protected void getInputProperties(VideoJob job)
        {
            double fps;
            Dar d;
            JobUtil.GetAllInputProperties( out numberOfFrames, out fps, out hres, out vres,out d, job.Input);
            dar = job.DAR;
            if (job.Settings.UsesSAR && job.DAR.HasValue)
            {
                Sar s = job.DAR.Value.ToSar(hres, vres);
                Dar d2 = new Dar(s.ar);
                throw new Exception();
//                job.Commandline = CommandLineGenerator.generateVideoCommandline(job.Settings, job.Input, job.Output, d2);
            }
            su.NbFramesTotal = numberOfFrames;
            su.ClipLength = TimeSpan.FromSeconds((double)numberOfFrames / fps);
        }

        protected override void doExitConfig()
        {
            if (!su.HasError && !su.WasAborted)
                compileFinalStats();
        }
        /// <summary>
        /// compiles final bitrate statistics
        /// </summary>
        private void compileFinalStats()
        {
            try
            {
                if (!string.IsNullOrEmpty(job.Output) && File.Exists(job.Output))
                {
                    FileInfo fi = new FileInfo(job.Output);
                    long size = fi.Length; // size in bytes

                    ulong framecount;
                    double framerate;
                    JobUtil.getInputProperties(out framecount, out framerate, job.Input);

                    double numberOfSeconds = (double)framecount / framerate;
                    long bitrate = (long)((double)(size * 8.0) / (numberOfSeconds * 1000.0));
                    if (job.Settings.EncodingMode != 1)
                        log.Append("desired video bitrate of this job: " + job.Settings.BitrateQuantizer + " kbit/s - obtained video bitrate (approximate): " + bitrate + " kbit/s");
                    else
                        log.Append("This is a CQ job so there's no desired bitrate. Obtained video bitrate: " + bitrate + " kbit/s");
                }
            }
            catch (Exception e)
            {
                log.Append("Exception in compileFinalStats. Message: " + e.Message + " stacktrace: " + e.StackTrace);
            }
        }
        #endregion

        public override void ProcessLine(string line, StreamType stream)
        {
            string frameString = GetFrameString(line, stream);
            string errorString = GetErrorString(line, stream);
            if (!string.IsNullOrEmpty(frameString))
            {
                int currentFrameNumber;
                if (int.TryParse(frameString, out currentFrameNumber))
                    this.currentFrameNumber = (ulong)currentFrameNumber;
            }
            if (!string.IsNullOrEmpty(errorString))
            {
                su.HasError = true;
                if (string.IsNullOrEmpty(su.Error))
                    su.Error = "";
                su.Error += errorString + "\r\n";
                log.AppendLine(errorString);
            }
            if (errorString == null && frameString == null)
            {
                log.AppendLine(line);
            }
        }

        public abstract string GetFrameString(string line, StreamType stream);
        public abstract string GetErrorString(string line, StreamType stream);

        protected override void doStatusCycleOverrides()
        {
            su.NbFramesDone = currentFrameNumber;
        }
    }
}
