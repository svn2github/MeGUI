// ****************************************************************************
// 
// Copyright (C) 2005-2013 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;

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
                log.LogValue("Exception in getPercentage(" + line + ") ", e, ImageType.Warning);
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
                if (!Path.GetExtension(job.Input).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".mp4"))
                {
                    FileSize rawSize = FileSize.Of(job.Input);
                    LogItem i = new LogItem("MP4 Muxing statistics");
                    i.LogValue("Size of raw video stream", rawSize);
                    i.LogValue("Size of final MP4 file", len);
                    i.LogValue("Codec", job.Codec);
                    i.LogValue("Number of b-frames", job.NbOfBFrames);
                    i.LogValue("Number of source frames", job.NbOfFrames);
                    log.Add(i);
                }
            }
            catch (Exception e)
            {
                log.LogValue("An exception occurred when printing mux statistics", e, ImageType.Warning);
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
                    {
                        if (String.IsNullOrEmpty(su.Status) || !su.Status.Equals("Importing Video Track..."))
                        {
                            su.Status = "Importing Video Track...";
                            startTime = DateTime.Now;
                        }
                    }
                    else if (trackNumber == 2 && numberOfAudioTracks > 0) // first audio track
                    {
                        if (String.IsNullOrEmpty(su.Status) || !su.Status.Equals("Importing Audio Track 1..."))
                        {
                            su.Status = "Importing Audio Track 1...";
                            startTime = DateTime.Now;
                        }
                    }
                    else if (trackNumber == 3 && numberOfAudioTracks > 1) // second audio track
                    {
                        if (String.IsNullOrEmpty(su.Status) || !su.Status.Equals("Importing Audio Track 2..."))
                        {
                            su.Status = "Importing Audio Track 2...";
                            startTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(su.Status) || !su.Status.Equals("Importing Tracks..."))
                        {
                            su.Status = "Importing Tracks...";
                            startTime = DateTime.Now;
                        }
                    }
                    break;

                case LineType.splitting:
                    su.PercentageDoneExact = getPercentage(line);
                    su.Status = "Splitting...";
                    break;

                case LineType.writing:
                    su.PercentageDoneExact = getPercentage(line);
                    if (String.IsNullOrEmpty(su.Status) || !su.Status.Equals("Writing..."))
                    {
                        su.Status = "Writing...";
                        startTime = DateTime.Now;
                    }
                    break;

                case LineType.other:
                    base.ProcessLine(line, stream);
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
 
                if (!String.IsNullOrEmpty(settings.VideoInput) || !String.IsNullOrEmpty(settings.MuxedInput))
                {
                    string strInput;
                    if (!String.IsNullOrEmpty(settings.VideoInput))
                        strInput = settings.VideoInput;
                    else
                        strInput = settings.MuxedInput;

                    if (settings.DeviceType != "Standard")
                    {
                        switch (settings.DeviceType)
                        {
                            case "iPod": sb.Append("-ipod -brand M4V  "); break;
                            case "iPhone": sb.Append("-ipod -brand M4VP:1 "); break;
                            case "iPad":
                            case "Apple TV": sb.Append("-ipod -brand M4VH "); break;
                            case "ISMA": sb.Append("-isma "); break;
                            case "PSP": sb.Append("-psp "); break;
                        }
                    }

                    MediaInfoFile oVideoInfo = new MediaInfoFile(strInput, ref log);
                    sb.Append("-add \"" + strInput);

                    int trackID = 1;
                    if (oVideoInfo.HasVideo && oVideoInfo.ContainerFileType == ContainerType.MP4)
                        trackID = oVideoInfo.VideoInfo.Track.TrackID;
                    sb.Append("#trackID=" + trackID);

                    if (settings.Framerate.HasValue)
                    {
                        string fpsString = settings.Framerate.Value.ToString(ci);
                        sb.Append(":fps=" + fpsString);
                    }
                    if (!string.IsNullOrEmpty(settings.VideoName))
                        sb.Append(":name=" + settings.VideoName);
                    sb.Append("\"");
                }
                int trackCount = 0;
                foreach (object o in settings.AudioStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    MediaInfoFile oInfo = new MediaInfoFile(stream.path, ref log);

                    if (!oInfo.HasAudio)
                    {
                        log.Error("No audio track found: " + stream.path);
                        continue;
                    }

                    sb.Append(" -add \"" + stream.path);

                    int trackID = 1;
                    int heaac_flag = -1;
                    if (oInfo.AudioInfo.Tracks.Count > 0)
                    {
                        if (oInfo.ContainerFileType == ContainerType.MP4)
                            trackID = oInfo.AudioInfo.Tracks[0].TrackID;
                        heaac_flag = oInfo.AudioInfo.Tracks[0].AACFlag;
                    }
                    sb.Append("#trackID=" + trackID);

                    if (oInfo.ContainerFileType == ContainerType.MP4 || oInfo.AudioInfo.Codecs[0] == AudioCodec.AAC)
                    {
                        switch (heaac_flag)
                        {
                            case 1: sb.Append(":sbr"); break;
                            case 2: sb.Append(":ps"); break;
                            default: sb.Append(""); break;
                        }
                    }
                    if (!string.IsNullOrEmpty(stream.language))
                    {
                        foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.LanguagesTerminology)
                        {
                            if (stream.language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(strLanguage.Key.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                            {
                                sb.Append(":lang=" + LanguageSelectionContainer.getISO639dot1(strLanguage.Value));
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(stream.name))
                        sb.Append(":name=" + stream.name);
                    if (stream.delay != 0)
                        sb.AppendFormat(":delay={0}", stream.delay);
                    if (settings.DeviceType == "iPod" || settings.DeviceType == "iPhone" || settings.DeviceType == "iPad" || settings.DeviceType == "Apple TV")
                    {
                        sb.Append(":group=1");
                        if (trackCount > 0)
                            sb.Append(":disable");
                    }
                    sb.Append("\"");
                    trackCount++;
                }
                trackCount = 0;
                foreach (object o in settings.SubtitleStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    sb.Append(" -add \"" + stream.path + "#trackID=1");
                    if (!string.IsNullOrEmpty(stream.language))
                    {
                        foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.LanguagesTerminology)
                        {
                            if (stream.language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(strLanguage.Key.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                            {
                                sb.Append(":lang=" + LanguageSelectionContainer.getISO639dot1(strLanguage.Value));
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(stream.name))
                        sb.Append(":name=" + stream.name);
                    if (settings.DeviceType == "iPod" || settings.DeviceType == "iPhone" || settings.DeviceType == "iPad" || settings.DeviceType == "Apple TV")
                    {
                        sb.Append(":hdlr=sbtl:layout=-1:group=2");
                        if (trackCount > 0)
                            sb.Append(":disable");
                    }
                    sb.Append("\"");
                    trackCount++;
                }

                if (!string.IsNullOrEmpty(settings.ChapterFile))
                {
                    // Add Apple Devices Chapter format
                    if (settings.DeviceType == "iPod" || settings.DeviceType == "iPhone" || settings.DeviceType == "iPad" || settings.DeviceType == "Apple TV")
                    {
                        FileUtil.CreateXMLFromOGGChapFile(settings.ChapterFile);
                        sb.Append(" -add \"" + Path.Combine(Path.GetDirectoryName(settings.ChapterFile), Path.GetFileNameWithoutExtension(settings.ChapterFile) + ".xml:name=:chap") + "\"");
                        job.FilesToDelete.Add(Path.GetFileNameWithoutExtension(settings.ChapterFile) + ".xml");
                    }

                    // Add Nero Style Chapters - this doesn't break Apple Devices playback  - just for better interoperability with other tools
                    sb.Append(" -chap \"" + settings.ChapterFile + "\"");

                }

                if (settings.SplitSize.HasValue)
                    sb.Append(" -splits " + settings.SplitSize.Value.KB);

                // tmp directory
                if (!String.IsNullOrEmpty(MainForm.Instance.Settings.TempDirMP4) && Directory.Exists(MainForm.Instance.Settings.TempDirMP4))
                    sb.AppendFormat(" -tmp \"{0}\"", MainForm.Instance.Settings.TempDirMP4.Replace("\\","\\\\"));
                else if (!Path.GetPathRoot(settings.MuxedOutput).Equals(settings.MuxedOutput, StringComparison.CurrentCultureIgnoreCase))
                    sb.AppendFormat(" -tmp \"{0}\"", Path.GetDirectoryName(settings.MuxedOutput).Replace("\\", "\\\\"));

                if (settings.DeviceType == "iPod" || settings.DeviceType == "iPhone" || settings.DeviceType == "iPad" || settings.DeviceType == "Apple TV")
                {
                    if (!string.IsNullOrEmpty(settings.VideoInput))
                        settings.MuxedOutput = Path.ChangeExtension(settings.MuxedOutput, ".m4v");
                    if (string.IsNullOrEmpty(settings.VideoInput) && !string.IsNullOrEmpty(settings.AudioStreams.ToString()))
                        settings.MuxedOutput = Path.ChangeExtension(settings.MuxedOutput, ".m4a");
                }

                // force to create a new output file
                sb.Append(" -new \"" + settings.MuxedOutput + "\"");
                return sb.ToString();
            }
        }
    }
}
