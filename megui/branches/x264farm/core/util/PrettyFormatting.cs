using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

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

        /// <summary>
        /// gets the delay from an audio filename
        /// </summary>
        /// <param name="fileName">file name to be analyzed</param>
        /// <returns>the delay in milliseconds</returns>
        public static int getDelay(string fileName)
        {
            int start = fileName.LastIndexOf("DELAY ");
            if (start != -1) // delay is in filename
            {
                try
                {
                    string delay = fileName.Substring(start + 6, fileName.LastIndexOf("ms.") - start - 6);
                    int del = 0;
                    del = Int32.Parse(delay);
                    return del;
                }
                catch (Exception e) // problem parsing, assume 0s delay
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
            return 0;
        }

    }
}
