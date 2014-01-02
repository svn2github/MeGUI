// ****************************************************************************
// 
// Copyright (C) 2005-2014 Doom9 & al
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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using MeGUI.core.util;

namespace MeGUI
{
    public class LSMASHIndexer : CommandlineJobProcessor<LSMASHIndexJob>
    {
        public static readonly JobProcessorFactory Factory =
                    new JobProcessorFactory(new ProcessorFactory(init), "LSMASHIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is LSMASHIndexJob) 
                return new LSMASHIndexer(mf.Settings.LSMASH.Path);
            return null;
        }

        public LSMASHIndexer(string executableName)
        {
            UpdateCacher.CheckPackage("lsmash");
            executable = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            //if (Regex.IsMatch(line, "^Indexing, please wait... [0-9]{1,3}%", RegexOptions.Compiled))
            //{
            //    su.PercentageDoneExact = Int32.Parse(line.Substring(25).Split('%')[0]);
            //    su.Status = "Creating LSMASH index...";
            //    return;
            //}

            //if (Regex.IsMatch(line, "^Writing index...", RegexOptions.Compiled))
            //    su.Status = "Writing LSMASH index...";
            base.ProcessLine(line, stream, oType);
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                //if (job.DemuxMode > 0)
                //    sb.Append("-t -1 ");
                //sb.Append("-f \"" + job.Input + "\"");
                //if (!String.IsNullOrEmpty(job.Output))
                //    sb.Append(" \"" + job.Output + "\"");
                sb.Append("/c");
                return sb.ToString();
            }
        }

        protected override void checkJobIO()
        {
            try
            {
                if (!String.IsNullOrEmpty(job.Output))
                {
                    FileUtil.ensureDirectoryExists(Path.GetDirectoryName(job.Output));
                    if (File.Exists(job.Output))
                        File.Delete(job.Output);
                }
            }
            finally
            {
                base.checkJobIO();
            }
            su.Status = "Creating LSMASH index...";
        }

        protected override void doExitConfig()
        {
            if (su.HasError || su.WasAborted)
            {
                base.doExitConfig();
                return;
            }

            string strErrorText;
            StringBuilder strAVSScript = new StringBuilder();
            strAVSScript.Append(VideoUtil.getLSMASHVideoInputLine(job.Input, job.Output, 0));
            if (!VideoUtil.AVSScriptHasVideo(strAVSScript.ToString(), out strErrorText))
            {
                su.HasError = true;
                base.doExitConfig();
                return;
            }

            if (job.DemuxMode > 0 && job.AudioTracks.Count > 0)
            {
                int iTracksFound = 0;
                int iCurrentAudioTrack = -1;
                for (int iCurrentTrack = 0; iCurrentTrack <= 29; iCurrentTrack++) // hard limit to max. 30 tracks
                {
                    strAVSScript = new StringBuilder();
                    strAVSScript.Append(VideoUtil.getLSMASHAudioInputLine(job.Input, job.Output, iCurrentTrack));

                    // is this an audio track?
                    if (AudioUtil.AVSScriptHasAudio(strAVSScript.ToString(), out strErrorText) == false)
                        continue;
                    iCurrentAudioTrack++;

                    foreach (AudioTrackInfo oAudioTrack in job.AudioTracks)
                    {
                        if (oAudioTrack.TrackIndex != iCurrentAudioTrack)
                            continue;

                        // write avs file
                        string strAudioAVSFile;
                        strAudioAVSFile = Path.GetFileNameWithoutExtension(job.Output) + "_track_" + (oAudioTrack.TrackIndex + 1) + "_" + oAudioTrack.Language.ToLower(System.Globalization.CultureInfo.InvariantCulture) + ".avs";
                        strAudioAVSFile = Path.Combine(Path.GetDirectoryName(job.Output), Path.GetFileName(strAudioAVSFile));
                        try
                        {
                            strAVSScript.AppendLine(@"# detected channels: " + oAudioTrack.NbChannels);
                            strAVSScript.Append(@"# detected channel positions: " + oAudioTrack.ChannelPositions);
                            StreamWriter oAVSWriter = new StreamWriter(strAudioAVSFile, false, Encoding.Default);
                            oAVSWriter.Write(strAVSScript);
                            oAVSWriter.Close();
                        }
                        catch (IOException ex)
                        {
                            log.LogValue("Error creating audio AVS file", ex);
                        }
                        break;
                    }
                    if (++iTracksFound == job.AudioTracks.Count)
                        break;
                }
            }
            base.doExitConfig();
        }
    }
}
