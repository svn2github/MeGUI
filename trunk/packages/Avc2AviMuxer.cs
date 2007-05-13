using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
    /// offers raw avc to avi muxing based on avc2avi
    /// </summary>
    class Avc2AviMuxer : CommandlineMuxer
    {
        public Avc2AviMuxer(string executablePath)
        {
            this.executable = executablePath;
        }
        /// <summary>
        /// sets up the muxer
        /// checks the input for validity and sets some internal variables
        /// </summary>
        /// <param name="job">the job to be processed</param>
        /// <param name="error">feedback variable for errors</param>
        /// <returns>true if the setup succeeded, false if it failed</returns>
        public override bool setup(Job job, out string error)
        {
            error = null;
            MuxJob mjob = (MuxJob)job;
            if (base.setup(job, out error))
            {
                double overhead = mjob.Overhead * (double)mjob.NbOfFrames;
                if (su.ProjectedFileSize.HasValue)
                    su.ProjectedFileSize += new FileSize((ulong)overhead);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// starts the muxing process
        /// </summary>
        /// <param name="error">feedback variable for potential errors</param>
        /// <returns>true if everything was started okay, false if not</returns>
        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            this.MuxerOutputReceived += new MuxerOutputCallback(Avc2AviMuxer_MuxerOutputReceived);
            try
            {
                bool started = proc.Start();
                new MethodInvoker(this.readStdOut).BeginInvoke(null, null);
                new MethodInvoker(this.readStdErr).BeginInvoke(null, null);
                this.changePriority(job.Priority, out error);
                new Thread(new ThreadStart(this.sendStatusUpdate)).Start();
                return true;
            }
            catch (Exception e)
            {
                error = "Exception starting the process: " + e.Message;
                return false;
            }
        }
        /// <summary>
        /// processes commandline output from avc2avi
        /// </summary>
        /// <param name="line">the line received from avc2avi</param>
        /// <param name="type">the type of line (stdout/stderr/internal)</param>
        void Avc2AviMuxer_MuxerOutputReceived(string line, int type)
        {
            log.AppendLine(line);
        }
        /// <summary>
        /// sets the completion percentage based on the projected output size (raw filesize + 
        /// overhead projection)
        /// </summary>
        private void setPercentage()
        {
            if (File.Exists(this.job.Output) && (su.ProjectedFileSize ?? FileSize.Empty) > FileSize.Empty
                && this.job.NbOfFrames > 0)
            { // we need those properties so that we can calculate a proper estimate
                su.PercentageDoneExact = (100M * FileSize.Of(job.Output) / su.ProjectedFileSize) ?? 0M;
                su.FileSize = FileSize.Of(job.Output);
            }
        }
        /// <summary>
        /// sends a status update to the GUI every second
        /// this is done because avc2avi gives no commandline output so we have to derive progress
        /// based on filesize
        /// </summary>
        private void sendStatusUpdate()
        {
            Thread.Sleep(1000);
            setPercentage();
            base.sendStatusUpdateToGUI(su);
        }
    }
}
