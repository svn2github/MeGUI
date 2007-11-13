using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace MeGUI.core.util
{
    public class PrettyFormatting
    {
        public static string ExtractWorkingName(string fileName)
        {
            string A = Path.GetFileNameWithoutExtension(fileName); // In case they all fail

            int count = 0;
            while (Path.GetDirectoryName(fileName).Length > 0 && count < 3)
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

                beginningOfWord = !char.IsLetter(chars[i]);
            }

            A = new string(chars);
            return A;
        }

        private static readonly Regex delayRegex = new Regex("(?<match>-?[0-9]+)ms");
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
