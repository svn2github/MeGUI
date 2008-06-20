using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.details;

namespace MeGUI
{
    class MkvMergeMuxer : CommandlineMuxer
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "MkvMergeMuxer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MuxJob && (j as MuxJob).MuxType == MuxerType.MKVMERGE)
                return new MkvMergeMuxer(mf.Settings.MkvmergePath);
            return null;
        }
        
        public MkvMergeMuxer(string executablePath)
        {
            this.executable = executablePath;
        }


        #region line processing
        /// <summary>
        /// gets the framenumber from an mkvmerge status update line
        /// </summary>
        /// <param name="line">mkvmerge commandline output</param>
        /// <returns>the framenumber included in the line</returns>
        public decimal? getPercentage(string line)
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
                log.LogValue("Exception in getPercentage(" + line + ")", e, MeGUI.core.util.ImageType.Warning);
                return null;
            }
        }
        #endregion

        protected override bool checkExitCode
        {
            get { return false; }
        }

        public override void ProcessLine(string line, StreamType stream)
        {
            if (line.StartsWith("progress: ")) //status update
                su.PercentageDoneExact = getPercentage(line);
            else if (line.IndexOf("Error") != -1)
            {
                log.LogValue("An error occurred", line, MeGUI.core.util.ImageType.Error);
                su.HasError = true;
            }
            else
                base.ProcessLine(line, stream);
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                MuxSettings settings = job.Settings;
                sb.Append("-o \"" + settings.MuxedOutput + "\"");

                if (settings.DAR.HasValue)
                    sb.Append(" --aspect-ratio 0:" + settings.DAR.Value.X + "/" + settings.DAR.Value.Y);
                else
                    sb.Append(" --engage keep_bitstream_ar_info");

                if (settings.VideoName.Length > 0)
                    sb.Append(" --track-name \"0:" + settings.VideoName + "\"");

                if (settings.MuxedInput.Length > 0)
                    sb.Append(" \"" + settings.MuxedInput + "\"");

                if (settings.VideoInput.Length > 0)
                    sb.Append(" -A -S \"" + settings.VideoInput + "\"");

                foreach (object o in settings.AudioStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    int trackID = 0;
                    if (stream.path.ToLower().EndsWith(".mp4") || stream.path.ToLower().EndsWith(".m4a"))
                        trackID = 1;
                    if (!string.IsNullOrEmpty(stream.language))
                        sb.Append(" --language " + trackID + ":" + stream.language);
                    if (stream.name != null && !stream.name.Equals(""))
                        sb.Append(" --track-name \"" + trackID + ":" + stream.name + "\"");
                    if (stream.delay != 0)
                        sb.AppendFormat(" --delay {0}:{1}ms", trackID, stream.delay);

                    sb.Append(" -a " + trackID + " -D -S \"" + stream.path + "\"");
                }

                foreach (object o in settings.SubtitleStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    int trackID = 0;
                    if (!string.IsNullOrEmpty(stream.language))
                        sb.Append(" --language 0:" + stream.language);
                    if (stream.name != null && !stream.name.Equals(""))
                        sb.Append(" --track-name \"" + trackID + ":" + stream.name + "\"");

                    sb.Append(" -s 0 -D -A \"" + stream.path + "\"");
                }
                if (!string.IsNullOrEmpty(settings.ChapterFile)) // a chapter file is defined
                    sb.Append(" --chapters \"" + settings.ChapterFile + "\"");

                if (settings.SplitSize.HasValue)
                    sb.Append(" --split " + (settings.SplitSize.Value.MB) + "M");

                sb.Append(" --no-clusters-in-meta-seek"); // ensures lower overhead

                return sb.ToString();
            }
        }
    }
}
