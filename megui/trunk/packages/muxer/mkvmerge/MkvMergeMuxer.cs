// ****************************************************************************
// 
// Copyright (C) 2005-2011  Doom9 & al
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
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;

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
            get { return true; }
        }

        public override void ProcessLine(string line, StreamType stream)
        {
            if (line.StartsWith("Progress: ")) //status update
                su.PercentageDoneExact = getPercentage(line);
            else if (line.StartsWith("Error: "))
            {
                log.LogValue("An error occurred", line, ImageType.Error);
                su.HasError = true;
            }
            else if (line.StartsWith("Warning: "))
                log.LogValue("A warning occurred", line, ImageType.Warning);
            else
                base.ProcessLine(line, stream);
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                MuxSettings settings = job.Settings;
                int trackID;
                
                sb.Append("-o \"" + settings.MuxedOutput + "\"");

                if (!string.IsNullOrEmpty(settings.VideoInput))
                {
                    MediaInfoFile oVideoInfo = new MediaInfoFile(settings.VideoInput, ref log);
                    if (oVideoInfo.ContainerFileType == ContainerType.MP4 || oVideoInfo.ContainerFileType == ContainerType.MKV)
                        trackID = oVideoInfo.GetFirstVideoTrackID();
                    else
                        trackID = 0;

                    sb.Append(" --engage keep_bitstream_ar_info"); // assuming that SAR info is already in the stream...
                    if (!string.IsNullOrEmpty(settings.VideoName))
                        sb.Append(" --track-name \"" + trackID + ":" + settings.VideoName.Replace("\"","\\\"") + "\"");
                    if (settings.Framerate.HasValue)
                        sb.Append(" --default-duration " + trackID + ":" + PrettyFormatting.ReplaceFPSValue(settings.Framerate.Value.ToString()) + "fps");
                    sb.Append(" \"--compression\" \"" + trackID + ":none\"");
                    sb.Append(" -d \"" + trackID + "\" --no-chapters -A -S \"" + settings.VideoInput + "\"");                    
                }
                else if(!string.IsNullOrEmpty(settings.MuxedInput))
                {
                    MediaInfoFile oVideoInfo = new MediaInfoFile(settings.MuxedInput, ref log);
                    if (oVideoInfo.ContainerFileType == ContainerType.MP4 || oVideoInfo.ContainerFileType == ContainerType.MKV)
                        trackID = oVideoInfo.GetFirstVideoTrackID();
                    else
                        trackID = 0;

                    if (settings.DAR.HasValue)
                        sb.Append(" --aspect-ratio " + trackID + ":" + settings.DAR.Value.X + "/" + settings.DAR.Value.Y);
                    else
                        sb.Append(" --engage keep_bitstream_ar_info");
                    if (!string.IsNullOrEmpty(settings.VideoName))
                        sb.Append(" --track-name \"" + trackID + ":" + settings.VideoName.Replace("\"", "\\\"") + "\"");
                    if (settings.Framerate.HasValue)
                        sb.Append(" --default-duration " + trackID + ":" + PrettyFormatting.ReplaceFPSValue(settings.Framerate.Value.ToString()) + "fps");
                    sb.Append(" \"--compression\" \"" + trackID + ":none\"");
                    sb.Append(" -d \"" + trackID + "\" -A -S \"" + settings.MuxedInput + "\""); 
                }

                foreach (object o in settings.AudioStreams)
                {
                    MuxStream stream = (MuxStream)o;
                    MediaInfoFile oAudioInfo = new MediaInfoFile(stream.path, ref log);

                    if (!oAudioInfo.HasAudio)
                    {
                        log.Error("No audio track found: " + stream.path);
                        continue;
                    }

                    if (oAudioInfo.ContainerFileType == ContainerType.MP4 || oAudioInfo.ContainerFileType == ContainerType.MKV)
                        trackID = oAudioInfo.GetFirstAudioTrackID();
                    else
                        trackID = 0;

                    int heaac_flag = 0;
                    if (oAudioInfo.ContainerFileType == ContainerType.MP4)
                    {
                        heaac_flag = -1;
                        if (oAudioInfo.AudioTracks.Count > 0)
                        {
                            heaac_flag = oAudioInfo.AudioTracks[0].AACFlag;
                        }
                        if (heaac_flag == 1)
                            sb.Append(" --aac-is-sbr "+ trackID + ":1");
                        else if (heaac_flag == 0)
                            sb.Append(" --aac-is-sbr " + trackID + ":0");
                    }
                    else if (oAudioInfo.ACodecs[0] == AudioCodec.AAC)
                    {
                        heaac_flag = -1;
                        if (oAudioInfo.AudioTracks.Count > 0)
                            heaac_flag = oAudioInfo.AudioTracks[0].AACFlag;
                        if (heaac_flag == 1)
                            sb.Append(" --aac-is-sbr 0:1");
                        else if (heaac_flag == 0)
                            sb.Append(" --aac-is-sbr 0");
                    }

                    if (!string.IsNullOrEmpty(stream.language))
                    {
                        foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                        {
                            if (stream.language.ToLower().Equals(strLanguage.Key.ToLower()))
                            {
                                sb.Append(" --language " + trackID + ":" + strLanguage.Value);
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(stream.name))
                        sb.Append(" --track-name \"" + trackID + ":" + stream.name.Replace("\"", "\\\"") + "\"");
                    if (stream.delay != 0)
                        sb.AppendFormat(" --sync {0}:{1}ms", trackID, stream.delay);
                    sb.Append(" \"--compression\" \"" + trackID + ":none\"");
                    sb.Append(" -a " + trackID + " --no-chapters -D -S \"" + stream.path + "\"");
                }

                foreach (object o in settings.SubtitleStreams)
                {
                    MuxStream stream = (MuxStream)o;

                    trackID = 0;
                    if (System.IO.File.Exists(stream.path))
                    {
                        MediaInfoFile oSubtitleInfo = new MediaInfoFile(stream.path, ref log);
                        if (oSubtitleInfo.ContainerFileType == ContainerType.MP4 || oSubtitleInfo.ContainerFileType == ContainerType.MKV)
                            trackID = oSubtitleInfo.GetFirstSubtitleTrackID();      
                    }

                    if (stream.MuxOnlyInfo != null)
                    {
                        trackID = stream.MuxOnlyInfo.TrackID - 1;
                        if (!string.IsNullOrEmpty(stream.MuxOnlyInfo.Language))
                            sb.Append(" --language " + trackID + ":" + stream.MuxOnlyInfo.Language);
                        if (!string.IsNullOrEmpty(stream.MuxOnlyInfo.Name))
                            sb.Append(" --track-name \"" + trackID + ":" + stream.MuxOnlyInfo.Name.Replace("\"", "\\\"") + "\"");
                        if (stream.delay != 0)
                            sb.AppendFormat(" --sync {0}:{1}ms", trackID, stream.delay);
                        if (stream.MuxOnlyInfo.DefaultTrack)
                            sb.Append(" --default-track \"" + trackID + ":yes\"");
                        else
                            sb.Append(" --default-track \"" + trackID + ":no\"");
                        if (stream.MuxOnlyInfo.ForcedTrack)
                            sb.Append(" --forced-track \"" + trackID + ":yes\"");
                        else
                            sb.Append(" --forced-track \"" + trackID + ":no\"");
                        sb.Append(" -s " + trackID + " -D -A -T --no-global-tags --no-chapters \"" + stream.MuxOnlyInfo.InputFile + "\"");
                    }
                    else if (stream.path.ToLower().EndsWith(".idx"))
                    {
                        List<SubtitleInfo> subTracks;
                        idxReader.readFileProperties(stream.path, out subTracks);
                        foreach (SubtitleInfo strack in subTracks)
                        {
                            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                            {  
                                if (trackID == 0 && !string.IsNullOrEmpty(stream.language) 
                                    && stream.language.ToLower().Equals(strLanguage.Key.ToLower()))
                                {
                                    sb.Append(" --language " + trackID + ":" + strLanguage.Value);
                                    break;
                                }
                                else if (((trackID == 0 && string.IsNullOrEmpty(stream.language)) || trackID > 0) 
                                    && LanguageSelectionContainer.Short2FullLanguageName(strack.Name).ToLower().Equals(strLanguage.Key.ToLower()))
                                {
                                    sb.Append(" --language " + trackID + ":" + strLanguage.Value);
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(stream.name))
                                sb.Append(" --track-name \"" + trackID + ":" + stream.name.Replace("\"", "\\\"") + "\"");
                            if (stream.delay != 0)
                                sb.AppendFormat(" --sync {0}:{1}ms", trackID, stream.delay);
                            if (stream.bDefaultTrack && trackID == 0)
                                sb.Append(" --default-track 0:yes");
                            else
                                sb.Append(" --default-track " + trackID + ":no");
                            if (stream.bForceTrack)
                                sb.Append(" --forced-track " + trackID + ":yes");
                            else
                                sb.Append(" --forced-track " + trackID + ":no");
                            ++trackID;
                        }
                        trackID = 0;
                        sb.Append(" -s ");
                        foreach (SubtitleInfo strack in subTracks)
                        {
                            if (trackID > 0)
                                sb.Append("," + trackID);
                            else 
                                sb.Append("0");
                            ++trackID;
                        }
                        sb.Append(" -D -A \"" + stream.path + "\"");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(stream.language))
                        {
                            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                            {
                                if (stream.language.ToLower().Equals(strLanguage.Key.ToLower()))
                                {
                                    sb.Append(" --language " + trackID + ":" + strLanguage.Value);
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(stream.name))
                            sb.Append(" --track-name \"" + trackID + ":" + stream.name.Replace("\"", "\\\"") + "\"");
                        if (stream.delay != 0)
                            sb.AppendFormat(" --sync {0}:{1}ms", trackID, stream.delay);
                        if (stream.bDefaultTrack)
                            sb.Append(" --default-track \"" + trackID + ":yes\"");
                        else
                            sb.Append(" --default-track \"" + trackID + ":no\"");
                        if (stream.bForceTrack)
                            sb.Append(" --forced-track \"" + trackID + ":yes\"");
                        else
                            sb.Append(" --forced-track \"" + trackID + ":no\"");
                        sb.Append(" -s " + trackID + " -D -A \"" + stream.path + "\"");
                    }
                }
                if (!string.IsNullOrEmpty(settings.ChapterFile)) // a chapter file is defined
                    sb.Append(" --chapters \"" + settings.ChapterFile + "\"");

                if (settings.SplitSize.HasValue)
                    sb.Append(" --split " + (settings.SplitSize.Value.MB) + "M");

                sb.Append(" --ui-language en");

                return sb.ToString();
            }
        }
    }
}
