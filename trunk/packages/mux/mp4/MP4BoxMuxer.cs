using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MeGUI
{
    class MP4BoxMuxer : CommandlineMuxer
    {
        private int numberOfAudioTracks, numberOfSubtitleTracks, trackNumber;
        private enum LineType : int {other = 0, importing, writing, splitting, empty, error };
        private string lastLine;

        public MP4BoxMuxer(string executablePath)
        {
            this.executable = executablePath;
            trackNumber = 0;
            lastLine = "";
        }
        #region setup/start overrides
        public override bool setup(Job job, out string error)
        {
            error = null;
            MuxJob mjob = (MuxJob)job;
            this.numberOfAudioTracks = mjob.Settings.AudioStreams.Count;
            this.numberOfSubtitleTracks = mjob.Settings.SubtitleStreams.Count;
            if (base.setup(job, out error))
            {
                if (!Path.GetExtension(job.Input).ToLower().Equals(".mp4"))
                {
                    double overhead = mjob.Overhead * (double)mjob.NbOfFrames;
                    su.ProjectedFileSize += (int)(overhead / 1024);
                }
                return true;
            }
            else
                return false;
        }
        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            this.MuxerOutputReceived += new MuxerOutputCallback(MP4BoxMuxer_MuxerOutputReceived);
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
        #endregion
        #region line dispatching
        void MP4BoxMuxer_MuxerOutputReceived(string line, int type)
        {
            LineType lineType = getLineType(line);
            int percentage = 0;
            switch (lineType)
            {
                case LineType.empty:
                    if (getLineType(lastLine) == LineType.importing) // moving from one track to another
                        trackNumber++;
                    break;
                case LineType.importing:
                    percentage = getPercentage(line);
                    if (trackNumber == 1) // video
                    {
                        su.FileSize = (videoSize * percentage / 100) / 1024;
                        su.AudioPosition = "importing video";
                    }
                    else if (trackNumber == 2 && numberOfAudioTracks > 0) // first audio track
                    {
                        su.AudioFileSize = (audioSize1 * percentage / 100) / 1024;
                        su.AudioPosition = "importing audio 1";
                    }
                    else if (trackNumber == 3 && numberOfAudioTracks > 1) // second audio track
                    {
                        su.AudioFileSize = (audioSize1 + audioSize2 * percentage / 100) / 1024;
                        su.AudioPosition = "importing audio 2";
                    }
                    else
                        su.AudioPosition = "importing";
                    su.PercentageDoneExact = (double)percentage;
                    su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
                    base.sendStatusUpdateToGUI(su);
                    break;
                case LineType.splitting:
                    percentage = getPercentage(line);
                    su.PercentageDoneExact = (double)percentage;
                    su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
                    su.AudioPosition = "splitting";
                    base.sendStatusUpdateToGUI(su);
                    break;
                case LineType.writing:
                    percentage = getPercentage(line);
                    su.PercentageDoneExact = (double)percentage;
                    su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
                    su.AudioPosition = "writing";
                    base.sendStatusUpdateToGUI(su);
                    break;
                case LineType.other:
                    log.Append(line + "\r\n");
                    break;
                case LineType.error:
                    su.Error = line;
                    su.HasError = true;
                    log.Append(line + "\r\n");
                    break;
            }
            lastLine = line;
        }
        #endregion
        #region line processing
        /// <summary>
        /// looks at a line and returns its type
        /// </summary>
        /// <param name="line">the line to be analyzed</param>
        /// <returns>the line type</returns>
        private LineType getLineType(string line)
        {
            if (line.StartsWith("Importing"))
                return LineType.importing;
            if (line.StartsWith("Writing"))
                return LineType.writing;
            if (line.StartsWith("Splitting"))
                return LineType.splitting;
            if (isEmptyLine(line))
                return LineType.empty;
            if (line.IndexOf("Error") != -1 || line.IndexOf("unknown") != -1)
                return LineType.error;
            return LineType.other;
        }
        /// <summary>
        /// gets the completion percentage of an mp4box line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private int getPercentage(string line)
        {
            try
            {
                int start = line.IndexOf("(") + 1;
                int end = line.IndexOf("/");
                string perc = line.Substring(start, end - start);
                int percentage = Int32.Parse(perc);
                return percentage;
            }
            catch (Exception e)
            {
                log.Append("Exception in getPercentage(" + line + ") " + e.Message);
                return 0;
            }
        }
        /// <summary>
        /// determines if a read line is empty
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool isEmptyLine(string line)
        {
            char[] characters = line.ToCharArray();
            bool isEmpty = true;
            foreach (char c in characters)
            {
                if (c != 32)
                {
                    isEmpty = false;
                    break;
                }
            }
            return isEmpty;
        }
        #endregion
        #region additional stuff
        /// <summary>
        /// compiles mp4 overhead statistics and dumps them to the log and a logfile
        /// </summary>
        public void printStatistics()
        {
            try
            {
                FileInfo fi = new FileInfo(job.Output);
                long finalMP4Size = fi.Length;
                long videoInMP4Size = fi.Length - this.audioSize1 - this.audioSize2 - this.subtitleSize;
                fi = new FileInfo(job.Input);
                if (!Path.GetExtension(job.Input).ToLower().Equals(".mp4"))
                {
                    long rawSize = fi.Length;
                    long overhead = videoInMP4Size - rawSize;
                    double overheadPerFrame = (double)overhead / job.NbOfFrames;
                    log.Append("MP4 muxing info:\r\n");
                    log.Append("Size of audio track 1: " + this.audioSize1 + " bytes\r\n");
                    log.Append("Size of audio track 2: " + this.audioSize2 + " bytes\r\n");
                    log.Append("Size of raw video stream: " + rawSize + " bytes\r\n");
                    log.Append("Size of final MP4 file: " + finalMP4Size + " bytes\r\n");
                    log.Append("Size of video in MP4 file: " + videoInMP4Size + " bytes\r\n");
                    log.Append("Total overhead: " + overhead + " bytes\r\n");
                    log.Append("Overhead per frame: " + overheadPerFrame + " bytes\r\n\r\n");
                    log.Append("source information\r\n");
                    log.Append("codec: " + job.Codec + "\r\n");
                    log.Append("number of b-frames: " + job.NbOfBFrames + "\r\n");
                    log.Append("number of source frames: " + job.NbOfFrames);
                }
            }
            catch (Exception e)
            {
                log.Append("an exception ocurred when trying to read from stdout: " + e.Message);
            }
        }
        #endregion
    }
}
