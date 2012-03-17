// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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

        public static bool AVSScriptHasAudio(String strAVSScript, out string strErrorText)
        {
            try
            {
                strErrorText = String.Empty;
                using (AviSynthScriptEnvironment env = new AviSynthScriptEnvironment())
                    using (AviSynthClip a = env.ParseScript(strAVSScript))
                        if (a.ChannelsCount == 0)
                            return false;
                return true;
            }
            catch (Exception ex)
            {
                strErrorText = ex.Message;
                return false;
            }
        }

        public static bool AVSFileHasAudio(String strAVSScript)
        {
            try
            {
                if (!Path.GetExtension(strAVSScript).ToLower().Equals(".avs"))
                    return false;
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

        public static int AVSFileChannelCount(String strAVSScript)
        {
            try
            {
                if (!Path.GetExtension(strAVSScript).ToLower().Equals(".avs"))
                    return 0;
                using (AviSynthScriptEnvironment env = new AviSynthScriptEnvironment())
                    using (AviSynthClip a = env.OpenScriptFile(strAVSScript))
                        return a.ChannelsCount;
            }
            catch
            {
                return 0;
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
        private string nbChannels, type, samplingRate, containerType, channelPositions, language, name;
        private int index, trackID, aacFlag, mmgTrackID;
        public AudioTrackInfo()
            : this(null, null, null, 0)
        {
        }
        public AudioTrackInfo(string language, string nbChannels, string type, int trackID)
        {
            this.language = language;
            this.nbChannels = nbChannels;     
            this.type = type;
            this.trackID = trackID;
            aacFlag = -1;
            mmgTrackID = 0;   
        }

        public string Language
        {
            get { return language; } 
            set { language = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

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
        public int MMGTrackID
        {
            get { return mmgTrackID; }
            set { mmgTrackID = value; }
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

        public int AACFlag
        {
            get { return aacFlag; }
            set { aacFlag = value; }
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
            if (!string.IsNullOrEmpty(language))
            {
                fullString += " / " + language;
            }
            return fullString.Trim();
        }
    }
}
