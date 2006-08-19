using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    class MkvMergeMuxer : CommandlineMuxer
    {
        public MkvMergeMuxer(string executablePath)
        {
            this.executable = executablePath;
        }

        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            this.MuxerOutputReceived += new MuxerOutputCallback(MkvMergeMuxer_MuxerOutputReceived);
            try
            {
                bool started = proc.Start();
                new MethodInvoker(this.readStdOut).BeginInvoke(null, null);
                new MethodInvoker(this.readStdErr).BeginInvoke(null, null);
                this.changePriority(job.Priority, out error);
                return true;
            }
            catch (Exception e)
            {
                error = "Exception starting the process: " + e.Message;
                return false;
            }
        }
        #region line processing
        void MkvMergeMuxer_MuxerOutputReceived(string line, int type)
        {
            if (line.StartsWith("progress: ")) //status update
            {
                int percentage = getPercentage(line);
                su.PercentageDoneExact = percentage;
                su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
                su.FileSize = (long)(videoSize * percentage / 100) / 1024;
                su.AudioFileSize = (long)(audioSize1 * percentage / 100) / 1024;
                base.sendStatusUpdateToGUI(su);
            }
            else if (line.IndexOf("Error") != -1)
            {
                log.Append(line + "\r\n");
                su.HasError = true;
                su.Error = line;
            }
            else
                log.Append(line + "\r\n");
        }

        /// <summary>
        /// gets the framenumber from an mkvmerge status update line
        /// </summary>
        /// <param name="line">mkvmerge commandline output</param>
        /// <returns>the framenumber included in the line</returns>
        public int getPercentage(string line)
        {
            try
            {
                int percentageStart = 10;
                int percentageEnd = line.IndexOf("%");
                string frameNumber = line.Substring(percentageStart, percentageEnd - percentageStart).Trim();
                return Int32.Parse(frameNumber);
            }
            catch (Exception e)
            {
                log.Append("Exception in getPercentage(" + line + ") " + e.Message);
                return 0;
            }
        }
        #endregion
    }
}