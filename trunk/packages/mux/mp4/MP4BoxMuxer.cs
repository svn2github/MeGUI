using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MeGUI.core.util;
using System.Globalization;
using MeGUI.core.details;

namespace MeGUI
{
    class MP4BoxMuxer : CommandlineMuxer
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "MP4BoxMuxer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MuxJob && (j as MuxJob).MuxType == MuxerType.MP4BOX)
                return new MP4BoxMuxer(mf.Settings.Mp4boxPath);
            return null;
        }
        
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
        protected override void checkJobIO()
        {
            this.numberOfAudioTracks = job.Settings.AudioStreams.Count;
            this.numberOfSubtitleTracks = job.Settings.SubtitleStreams.Count;
            
            base.checkJobIO();
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
            if (line.StartsWith("ISO File Writing"))
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
        private decimal? getPercentage(string line)
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
                return null;
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
                FileSize len = FileSize.Of(job.Output);
                FileSize Empty = FileSize.Empty;
                if (!Path.GetExtension(job.Input).ToLower().Equals(".mp4"))
                {
                    FileSize rawSize = FileSize.Of(job.Input);
                    log.Append("MP4 muxing info:\r\n");
                    log.AppendFormat(
                        "Size of raw video stream: {1}{0}" +
                        "Size of final MP4 file: {2}{0}" +
                        "source information{0}" +
                        "codec: {3}{0}" +
                        "number of b-frames: {4}{0}" +
                        "number of source frames: {5}{0}",
                        Environment.NewLine,
                        rawSize,
                        len,
                        job.Codec,
                        job.NbOfBFrames,
                        job.NbOfFrames);
                }
            }
            catch (Exception e)
            {
                log.Append("an exception ocurred when trying to read from stdout: " + e.Message);
            }
        }
        #endregion

        public override void ProcessLine(string line, StreamType stream)
        {
            switch (getLineType(line))
            {
                case LineType.empty:
                    if (getLineType(lastLine) == LineType.importing) // moving from one track to another
                        trackNumber++;
                    break;
                case LineType.importing:
                    su.PercentageDoneExact = getPercentage(line);
                    if (trackNumber == 1) // video
                        su.Status = "importing video";
                    else if (trackNumber == 2 && numberOfAudioTracks > 0) // first audio track
                        su.Status = "importing audio 1";
                    else if (trackNumber == 3 && numberOfAudioTracks > 1) // second audio track
                        su.Status = "importing audio 2";
                    else
                        su.Status = "importing";
                    break;

                case LineType.splitting:
                    su.PercentageDoneExact = getPercentage(line);
                    break;

                case LineType.writing:
                    su.PercentageDoneExact = getPercentage(line);
                    su.Status = "writing";
                    break;

                case LineType.other:
                    log.AppendLine(line);
                    break;

                case LineType.error:
                    su.Error = line;
                    su.HasError = true;
                    log.AppendLine(line);
                    break;
            }
            lastLine = line;
        }

        protected override string Commandline
        {
            get
            {
                MuxSettings settings = job.Settings;
                CultureInfo ci = new CultureInfo("en-us");
                StringBuilder sb = new StringBuilder();
                if (settings.VideoInput.Length > 0)
                {
                    sb.Append("-add \"" + settings.VideoInput);
                    if (!settings.VideoName.Equals(""))
                        sb.Append(":name=" + settings.VideoName);
                    sb.Append("\"");
                }
                if (settings.MuxedInput.Length > 0)
                {
                    sb.Append(" -add \"" + settings.MuxedInput);
                    if (!settings.VideoName.Equals(""))
                        sb.Append(":name=" + settings.VideoName);
                    sb.Append("\"");
                }
                foreach (object o in settings.AudioStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    sb.Append(" -add \"" + stream.path);
                    if (stream.language != null && !stream.language.Equals(""))
                        sb.Append(":lang=" + stream.language);
                    if (stream.name != null && !stream.name.Equals(""))
                        sb.Append(":name=" + stream.name);
                    if (stream.delay != 0)
                        sb.AppendFormat(":delay={0}", stream.delay);
                    sb.Append("\"");
                }
                foreach (object o in settings.SubtitleStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    sb.Append(" -add \"" + stream.path);
                    if (!stream.language.Equals(""))
                        sb.Append(":lang=" + stream.language);
                    if (stream.name != null && !stream.name.Equals(""))
                        sb.Append(":name=" + stream.name);
                    sb.Append("\"");
                }

                if (!settings.ChapterFile.Equals("")) // a chapter file is defined
                    sb.Append(" -chap \"" + settings.ChapterFile + "\"");

                if (settings.SplitSize.HasValue)
                    sb.Append(" -splits " + settings.SplitSize.Value.KB);

                if (settings.Framerate > 0)
                {
                    string fpsString = settings.Framerate.ToString(ci);
                    sb.Append(" -fps " + fpsString);
                }
                /*   sb.AppendFormat(" -tmp {0}", Path.GetDirectoryName(settings.MuxedOutput)); */
                sb.Append(" -new \"" + settings.MuxedOutput + "\"");
                return sb.ToString();
            }
        }
    }
}
