// ****************************************************************************
// 
// Copyright (C) 2005-2008  Doom9 & al
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
using System.Windows.Forms;

namespace MeGUI.core.util
{
    public class PrettyFormatting
    {
        public static string ExtractWorkingName(string fileName)
        {
            string A = Path.GetFileNameWithoutExtension(fileName); // In case they all fail

            int count = 0;
            while (!string.IsNullOrEmpty(Path.GetDirectoryName(fileName)) && count < 3)
            {
                string temp = Path.GetFileNameWithoutExtension(fileName).ToLower();
                if (!temp.Contains("vts") && !temp.Contains("video") && !temp.Contains("audio"))
                {
                    A = temp;
                    break;
                }
                fileName = Path.GetDirectoryName(fileName);
                count++;
            }

            // Format it nicely:
            char[] chars = A.ToCharArray();
            bool beginningOfWord = true;
            for (int i = 0; i < chars.Length; i++)
            {
                // Capitalize the beginning of words
                if (char.IsLetter(chars[i]) && beginningOfWord) chars[i] = char.ToUpper(chars[i]);
                // Turn '_' into ' '
                if (chars[i] == '_') chars[i] = ' ';

                beginningOfWord = !char.IsLetter(chars[i]) && !(chars[i] == '\'');
            }

            A = new string(chars);
            return A;
        }

        private static readonly Regex delayRegex = new Regex("(?<match>-?[0-9]+)ms");

        /// <summary>
        /// Gets the delay from the filename, but warns the user if this delay is larger than
        /// 10 seconds.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>The delay, or null if no valid delay was found</returns>
        public static int? getDelayAndCheck(string filename)
        {
            int? delay = getDelay(filename);
            
            if (delay.HasValue && Math.Abs(delay.Value) > 10000)
            {
                if (MessageBox.Show(string.Format("Your input filename suggests the delay is {0}ms ({1}s), " +
                    "which is surprisingly large. Try checking the tool used to create this file to see " +
                    "if it got the delay wrong.\n\nAre you sure this delay is correct?", delay, (delay / 1000)),
                    "Very large delay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    delay = null;
            }
            
            return delay;
        }

        /// <summary>
        /// gets the delay from an audio filename
        /// </summary>
        /// <param name="fileName">file name to be analyzed</param>
        /// <returns>the delay in milliseconds</returns>
        public static int? getDelay(string fileName)
        {
            try
            {
                return int.Parse(delayRegex.Match(fileName).Groups["match"].Value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// replaces the delay in the audio filename with a new delay
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string ReplaceDelay(string fileName, int delay)
        {
            return delayRegex.Replace(fileName, delay + "ms", 1);
        }
    }
}
