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

using Utils.MessageBoxExLib;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.gui;
using MeGUI.core.util;

using MediaInfoWrapper;

namespace MeGUI
{
    /// <summary>
    /// AudioUtil is used to perform various audio related tasks
    /// </summary>
    public class AudioUtil
    {

        /// gets ID from audio stream using MediaInfo
        /// </summary>
        /// <param name="infoFile">the file to be analyzed</param>
        /// <param name="count">the counter</param>
        /// <returns>the audio track ID found</returns>
        public static int getIDFromAudioStream(string fileName)
        {
            MediaInfo info;
            int TrackID = 0;
            try
            {
                info = new MediaInfo(fileName);
                if (info.Audio.Count > 0)
                {
                    MediaInfoWrapper.AudioTrack atrack = info.Audio[0];
                    TrackID = Int32.Parse(atrack.ID);
                }
            }
            catch (Exception i)
            {
                MessageBox.Show("The following error ocurred when trying to get Media info for file " + fileName + "\r\n" + i.Message, "Error parsing mediainfo data", MessageBoxButtons.OK);
            }
            return TrackID;
        }

        /// gets SBR/PS flag from AAC streams using MediaInfo
        /// </summary>
        /// <param name="infoFile">the file to be analyzed</param>
        /// <param name="count">the counter</param>
        /// <returns>the flag found</returns>
        public static int getFlagFromAACStream(string fileName)
        {
            MediaInfo info;
            int flag = 0;
            try
            {
                info = new MediaInfo(fileName);
                if (info.Audio.Count > 0)
                {
                    MediaInfoWrapper.AudioTrack atrack = info.Audio[0];
                    if (atrack.Format == "AAC")
                    {
                        if (atrack.FormatSettingsSBR == "Yes")
                        {
                            if (atrack.FormatSettingsPS == "Yes")
                                 flag = 2;
                            else flag = 1;
                        }
                        if (atrack.SamplingRate == "24000")
                        {
                            if ((atrack.Channels == "2") || (atrack.Channels == "1")) // workaround for raw aac
                                flag = 1;
                        }
                    }
                }
            }
            catch (Exception i)
            {
                MessageBox.Show("The following error occurred when trying to get Media info for file " + fileName + "\r\n" + i.Message, "Error parsing mediainfo data", MessageBoxButtons.OK);
            }
            return flag;
        }

        /// <summary>
        /// returns all audio streams that can be encoded or muxed
        /// </summary>
        /// <returns></returns>
        public static AudioJob[] getConfiguredAudioJobs(AudioJob[] audioStreams)
        {
            List<AudioJob> list = new List<AudioJob>();
            foreach (AudioJob stream in audioStreams)
            {
                if (String.IsNullOrEmpty(stream.Input))
                {
                    // no audio is ok, just skip
                    break;
                }
                list.Add(stream);

            }
            return list.ToArray();
        }

        public static bool AVSScriptHasAudio(String strAVSScript)
        {
            try
            {
                using (AviSynthScriptEnvironment env = new AviSynthScriptEnvironment())
                    using (AviSynthClip a = env.ParseScript(strAVSScript))
                        if (a.ChannelsCount == 0)
                            return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AVSFileHasAudio(String strAVSScript)
        {
            try
            {
                using (AviSynthScriptEnvironment env = new AviSynthScriptEnvironment())
                using (AviSynthClip a = env.OpenScriptFile(strAVSScript))
                    if (a.ChannelsCount == 0)
                        return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string getChannelPositionsFromAVSFile(String strAVSFile)
        {
            string strChannelPositions = String.Empty;
            
            try
            {
                string line;
                StreamReader file = new StreamReader(strAVSFile);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.IndexOf(@"# detected channel positions: ") == 0)
                    {
                        strChannelPositions = line.Substring(30);
                        break;
                    }
                }
                file.Close();
                return strChannelPositions;
            }
            catch
            {
                return strChannelPositions;
            }
        }

        public static int getChannelCountFromAVSFile(String strAVSFile)
        {
            int iChannelCount = 0;

            try
            {
                string line;
                StreamReader file = new StreamReader(strAVSFile);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.IndexOf(@"# detected channels: ") == 0)
                    {
                        int.TryParse(line.Substring(21).Split(' ')[0], out iChannelCount);
                        break;
                    }
                }
                file.Close();
                return iChannelCount;
            }
            catch
            {
                return iChannelCount;
            }
        }
    }

    public class AudioTrackInfo
    {
        private string nbChannels, type, samplingRate, containerType, description, channelPositions;
        private int index, trackID;
        public AudioTrackInfo()
            : this(null, null, null, 0)
        {
        }
        public AudioTrackInfo(string language, string nbChannels, string type, int trackID)
        {
            TrackInfo = new TrackInfo(language, null);
            this.nbChannels = nbChannels;
            this.type = type;
            this.trackID = trackID;
        }

        public string Language
        {
            get
            {
                if (TrackInfo == null) return null;
                return TrackInfo.Language;
            }
            set
            {
                if (TrackInfo == null)
                    TrackInfo = new TrackInfo();
                TrackInfo.Language = value;
            }
        }

        public TrackInfo TrackInfo;
        public string TrackIDx
        {
            get { return containerType == "MPEG-TS" ? trackID.ToString("x3") : trackID.ToString("x"); }
            set { trackID = Int32.Parse(value, System.Globalization.NumberStyles.HexNumber); }
        }
        public int TrackID
        {
            get { return trackID; }
            set { trackID = value; }
        }
        public string DgIndexID
        {
            get { return containerType == "MPEG-TS" ? index.ToString() : TrackIDx; }
        }
        public string ContainerType
        {
            get { return containerType; }
            set { containerType = value; }
        }
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string NbChannels
        {
            get { return nbChannels; }
            set { nbChannels = value; }
        }

        public string ChannelPositions
        {
            get { return channelPositions; }
            set { channelPositions = value; }
        }

        public string SamplingRate
        {
            get { return samplingRate; }
            set { samplingRate = value; }
        }

        public override string ToString()
        {
            string fullString = "[" + TrackIDx + "] - " + this.type;
            if (!string.IsNullOrEmpty(nbChannels))
            {
                fullString += " - " + this.nbChannels;
            }
            if (!string.IsNullOrEmpty(samplingRate))
            {
                fullString += " / " + samplingRate;
            }
            if (!string.IsNullOrEmpty(TrackInfo.Language))
            {
                fullString += " / " + TrackInfo.Language;
            }
            return fullString.Trim();
        }
    }
}
