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
using System.Text;
using System.Text.RegularExpressions;

using MeGUI.core.util;

namespace MeGUI
{
    class MkvExtract : CommandlineJobProcessor<MkvExtractJob>
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "MkvExtract");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MkvExtractJob) 
                return new MkvExtract(mf.Settings.MkvExtractPath);
            return null;
        }

        public MkvExtract(string executablePath)
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

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (line.StartsWith("Progress: ")) //status update
            {
                su.PercentageDoneExact = getPercentage(line);
                su.Status = "Extracting Tracks...";
                return;
            }

            if (!line.ToLower(System.Globalization.CultureInfo.InvariantCulture).StartsWith("extracting track"))
            {
                if (line.ToLower(System.Globalization.CultureInfo.InvariantCulture).StartsWith("error"))
                    oType = ImageType.Error;
                else if (line.ToLower(System.Globalization.CultureInfo.InvariantCulture).StartsWith("warning"))
                    oType = ImageType.Warning;
            }
            base.ProcessLine(line, stream, oType);
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                // verify track IDs
                MediaInfoFile oFile = new MediaInfoFile(job.Input, ref log);
                foreach (TrackInfo oTrack in job.MkvTracks)
                {
                    if (oTrack.TrackType == TrackType.Audio)
                    {
                        bool bFound = false;
                        foreach (AudioTrackInfo oAudioInfo in oFile.AudioInfo.Tracks)
                        {
                            if (oAudioInfo.MMGTrackID == oTrack.MMGTrackID)
                                bFound = true;
                        }

                        if (!bFound)
                            oTrack.MMGTrackID = oFile.AudioInfo.Tracks[oTrack.TrackIndex].MMGTrackID;
                    }
                    else if (oTrack.TrackType == TrackType.Subtitle)
                    {
                        bool bFound = false;
                        foreach (SubtitleTrackInfo oSubtitleInfo in oFile.SubtitleInfo.Tracks)
                        {
                            if (oSubtitleInfo.MMGTrackID == oTrack.MMGTrackID)
                                bFound = true;
                        }

                        if (!bFound)
                            oTrack.MMGTrackID = oFile.SubtitleInfo.Tracks[oTrack.TrackIndex].MMGTrackID;
                    }

                }

                // Input File
                sb.Append("tracks \"" + job.Input + "\" --ui-language en");

                // Tracks to extract
                foreach (TrackInfo oTrack in job.MkvTracks)
                {
                    // Extract only audio/subtitle tracks
                    if (oTrack.TrackType != TrackType.Audio && oTrack.TrackType != TrackType.Subtitle)
                        continue;

                    sb.Append(" " + oTrack.MMGTrackID + ":\"" + job.OutputPath + "\\" + oTrack.DemuxFileName + "\"");
                }

                return sb.ToString();
            }
        }
    }
}
