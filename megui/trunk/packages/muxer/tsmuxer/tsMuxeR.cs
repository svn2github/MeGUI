// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
    class tsMuxeR : CommandlineMuxer
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "TSMuxer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MuxJob && (j as MuxJob).MuxType == MuxerType.TSMUXER)
                return new tsMuxeR(mf.Settings.TSMuxerPath);
            return null;
        }

        private int numberOfAudioTracks, numberOfSubtitleTracks, trackNumber;
        private string metaFile = null;
        private string lastLine;

        public tsMuxeR(string executablePath)
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
            generateMetaFile();
            Util.ensureExists(metaFile);
            base.checkJobIO();
        }

        #endregion


        protected override string Commandline
        {
            get
            {
                return " \"" + metaFile + "\"" + " \"" + job.Output + "\"";
            }
        }
        
        /// <summary>
        /// gets the completion percentage of an tsmuxer line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private decimal? getPercentage(string line)
        {
            try
            {
                int percentageStart = 0;
                int percentageEnd = line.IndexOf(".");
                string frameNumber = line.Substring(percentageStart, percentageEnd - percentageStart).Trim();
                return Int32.Parse(frameNumber);
            }
            catch (Exception e)
            {
                log.LogValue("Exception in getPercentage(" + line + ")", e, MeGUI.core.util.ImageType.Warning);
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

        private void generateMetaFile()
        {
            metaFile = Path.ChangeExtension(job.Output, ".meta");
            MuxSettings settings = job.Settings;
            CultureInfo ci = new CultureInfo("en-us");

            using (StreamWriter sw = new StreamWriter(metaFile, false, Encoding.Default))
            {
                string vcodecID = "";
                string extra = "";

                sw.Write("MUXOPT --no-pcr-on-video-pid --new-audio-pes --vbr --vbv-len=500"); // mux options
                if (settings.SplitSize.HasValue)
                    sw.Write(" --split-size " + settings.SplitSize.Value.MB + "MB");

                if (!string.IsNullOrEmpty(settings.DeviceType) && settings.DeviceType != "Standard")
                {
                    switch (settings.DeviceType)
                    {
                        case "Blu-ray": sw.Write(" --blu-ray"); break;
                        case "AVCHD": sw.Write(" --avchd"); break;
                    }

                    if (!string.IsNullOrEmpty(settings.ChapterFile)) // a chapter file is defined
                    {
                        string chapterTimeLine = VideoUtil.getChapterTimeLine(settings.ChapterFile);
                        sw.Write(" --custom-chapters" + chapterTimeLine);
                    }

                    job.Output = Path.ChangeExtension(job.Output, ""); // remove m2ts file extension - use folder name only with this mode
                }

                if (!string.IsNullOrEmpty(settings.VideoInput))
                {
                    if (VideoUtil.detecAVCStreamFromFile(settings.VideoInput) == true)
                    {
                        vcodecID = "V_MPEG4/ISO/AVC";
                        extra = " insertSEI, contSPS";
                        if (settings.VideoInput.ToLower().EndsWith(".mp4"))
                            extra += " , track=1";
                    }
                    else if (settings.VideoInput.ToLower().EndsWith(".m2v"))
                        vcodecID = "V_MPEG2";
                    else vcodecID = "V_MS/VFW/WVC1";
                    sw.Write("\n" + vcodecID + ", ");

                    sw.Write("\"" + settings.VideoInput + "\"");
                }

                if (!string.IsNullOrEmpty(settings.MuxedInput))
                {
                    if (VideoUtil.detecAVCStreamFromFile(settings.VideoInput) == true)
                    {
                        vcodecID = "V_MPEG4/ISO/AVC";
                        extra = " insertSEI, contSPS";
                        if (settings.VideoInput.ToLower().EndsWith(".mp4"))
                            extra += " , track=1";
                    }
                    else if (settings.MuxedInput.ToLower().EndsWith(".m2v"))
                        vcodecID = "V_MPEG2";
                    else vcodecID = "V_MS/VFW/WVC1";
                    sw.Write(vcodecID + ", ");

                    sw.Write("\"" + settings.MuxedInput + "\"");
                }

                if (settings.Framerate.HasValue)
                {
                    string fpsString = settings.Framerate.Value.ToString(ci);
                    sw.Write(", fps=" + fpsString);
                }

                if (extra != "") sw.Write(" ," + extra);

                foreach (object o in settings.AudioStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    string acodecID = "";

                    if (stream.path.ToLower().EndsWith(".ac3")) 
                        acodecID = "A_AC3";
                    else if (stream.path.ToLower().EndsWith(".aac"))
                        acodecID = "A_AAC";
                    else if (stream.path.ToLower().EndsWith(".dts"))
                        acodecID = "A_DTS";

                    sw.Write("\n" + acodecID + ", ");
                    sw.Write("\"" + stream.path + "\"");
                    
                    if (stream.delay != 0)
                       sw.Write(", timeshift={0}ms", stream.delay);

                    if (!string.IsNullOrEmpty(stream.language))
                       sw.Write(", lang=" + stream.language);
                }

                foreach (object o in settings.SubtitleStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    string scodecID = "";

                    if (stream.path.ToLower().EndsWith(".srt"))
                        scodecID = "S_TEXT/UTF8";
                    else scodecID = "S_HDMV/PGS"; // sup files

                    sw.Write("\n" + scodecID + ", ");
                    sw.Write("\"" + stream.path + "\"");

                    if (stream.delay != 0)
                        sw.Write(", timeshift={0}ms", stream.delay);

                    if (!string.IsNullOrEmpty(stream.language))
                        sw.Write(", lang=" + stream.language);
                }                

                job.FilesToDelete.Add(metaFile);
            }
        }
    }
}
