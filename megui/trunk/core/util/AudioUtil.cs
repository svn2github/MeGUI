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

using System;
using System.Collections.Generic;
using System.IO;

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
}
