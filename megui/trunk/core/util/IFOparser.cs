// ****************************************************************************
// 
// Copyright (C) 2008  Kurtnoise (kurtnoise@free.fr)
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

// Some Functions used in this class come from SubtitleCreator.
// http://subtitlecreator.svn.sourceforge.net/viewvc/subtitlecreator/trunk/DVDinfo.cs?view=markup
// Thanks to the author for that.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.util
{
    public sealed class IFOparser
    {
        /// <summary>
        /// Determine the IFO file that contains the menu: although it often is the largest
        /// IFO, this is not always the case, especially with elaborate DVDs with many extras.
        /// Therefore, look for the largest VOBs, and determine the IFO based on that.
        /// </summary>
        /// <param name="inputPath">Path that contains the DVD</param>
        /// <returns>Filename of the IFO that contains the movie</returns>
        public static string DetermineMovieIFO(string inputPath)
        {
            // The first 7 characters are the same for each VOB set, e.g.
            // VTS_24_0.VOB, VTS_24_1.VOB etc.
            string[] vobFiles = Directory.GetFiles(inputPath, "vts*.vob");

            // Look for the largest VOB set
            string vtsNameCurrent;
            string vtsNamePrevious = Path.GetFileName(vobFiles[0]).Substring(0, 7);
            long vtsSizeLargest = 0;
            long vtsSize = 0;
            string vtsNumber = "01";
            foreach (string file in vobFiles)
            {
                vtsNameCurrent = Path.GetFileName(file).Substring(0, 7);
                if (vtsNameCurrent.Equals(vtsNamePrevious))
                    vtsSize += new FileInfo(file).Length;
                else
                {
                    if (vtsSize > vtsSizeLargest)
                    {
                        vtsSizeLargest = vtsSize;
                        vtsNumber = vtsNamePrevious.Substring(4, 2);
                    }
                    vtsNamePrevious = vtsNameCurrent;
                    vtsSize = new FileInfo(file).Length;
                }
            }
            // Check whether the last one isn't the largest
            if (vtsSize > vtsSizeLargest)
                vtsNumber = vtsNamePrevious.Substring(4, 2);

            string ifoFile = inputPath + Path.DirectorySeparatorChar + "VTS_" + vtsNumber + "_0.IFO";
            // Name of largest VOB set is the name of the IFO, so we can now create the IFO file
            return ifoFile;
        }

        public static string[] GetAudioStreamsInfos(string FileName)
        {
            byte[] buff = new byte[2];
            byte a = 0;
            string[] audiodesc = new string[a];
            string codingMode = "";
            string LanguageType = "";
            string ApplicationMode = "";
            string quantization = "";
            string SamplingRate = "";
            byte trackID = 0x80;
            bool Multichannel_Ext = false;
            bool DRC = true;

            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                Stream sr = br.BaseStream;

                // got to audio stream #1
                sr.Seek(0x203, SeekOrigin.Begin);

                a = br.ReadByte();
                if (a > 8)
                    a = 8; // force the max #. According to the specs 8 is the max value for audio streams.

                audiodesc = new string[a];

                for (int i = 0; i < a; i++)
                {
                    switch (( a & 0xD0) >> 5)
                    {
                        case 0: codingMode = "AC3"; trackID = (byte)(0x80 + i); break;
                        case 1: codingMode = "Unknown"; break;
                        case 2: codingMode = "Mpeg-1"; break;
                        case 3: codingMode = "Mpeg-2 Ext"; break;
                        case 4: codingMode = "LPCM"; trackID = (byte)(0xA0 + i); break;
                        case 5: codingMode = "Unknown"; break;
                        case 6: codingMode = "DTS"; trackID = (byte)(0x88 + i); break;
                        case 7: codingMode = "SDDS"; break;
                    }

                    if (((a & 0x20) >> 4) == 0)
                         Multichannel_Ext = false;
                    else Multichannel_Ext = true;

                    if (((a & 0x0C) >> 2) == 0)
                         LanguageType = "Not Present";
                    else LanguageType = "Present";

                    switch (a & 0x03)
                    {
                        case 0: ApplicationMode = "Unspecified"; break;
                        case 1: ApplicationMode = "Karaoke"; break;
                        case 2: ApplicationMode = "Surround"; break;
                    }

                    byte ee = br.ReadByte(); // byte 1
                    switch ((ee & 0xC0) >> 6)
                    {
                        case 0: quantization = "16 Bits"; break;
                        case 1: quantization = "20 Bits"; break;
                        case 2: quantization = "24 Bits"; break;
                    }

                    // Several cases
                    if (codingMode != "LPCM") quantization = "16 Bits";
                    if ((codingMode == "Mpeg-1") || (codingMode == "Mpeg-2 Ext"))
                         DRC = false;
                    else DRC = true;

                    if (((ee & 0x30) >> 4) == 0)
                        SamplingRate = "48 Khz";

                    int Channels = (ee & 0x07) + 1;

                    ee = br.ReadByte();  // byte 2
                    br.Read(buff, 0, 2); 
                    string ShortLangCode = String.Format("{0}{1}", (char)buff[0], (char)buff[1]);

                    audiodesc[i] = "[" + trackID.ToString("X") + "]  - " + codingMode + " / " + SamplingRate + " / " + LanguageSelectionContainer.Short2FullLanguageName(ShortLangCode);
/*                   
                    sr.Seek(1, SeekOrigin.Current);
                    ee = br.ReadByte(); // byte 5
                    switch (ee & 0x0F)
                    {
                        case 0: audiodesc[i] += " Unspecified"; break;
                        case 1: audiodesc[i] += " Normal"; break;
                        case 2: audiodesc[i] += " For Visually Impaired"; break;
                        case 3: audiodesc[i] += " Director's Comments"; break;
                        case 4: audiodesc[i] += " Alternate Director's Comments"; break;
                    }
*/                    
                    sr.Seek(4, SeekOrigin.Current); // next audio stream
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return audiodesc;

        }
             
        
        public static string[] GetSubtitlesStreamsInfos(string FileName)
        {
            byte[] buff = new byte[2];
            byte s = 0;
            string[] subdesc = new string[s];

            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                Stream sr = br.BaseStream;

                // go to the substream #1
                sr.Seek(0x255, SeekOrigin.Begin);

                s = br.ReadByte();
                if (s > 32)
                    s = 32; // force the max #. According to the specs 32 is the max value for subtitles streams.

                subdesc = new string[s];

                // go to the Language Code
                sr.Seek(2, SeekOrigin.Current);

                for (int i = 0; i < s; i++)
                {
                    // Presence (1 bit), Coding Mode (1bit), Short Language Code (2bits), Language Extension (1bit), Sub Picture Caption Type (1bit)
                    br.Read(buff, 0, 2);
                    string ShortLangCode = String.Format("{0}{1}", (char)buff[0], (char)buff[1]);

                    subdesc[i] = "[" + String.Format("{0:00}", i) + "]  - " + LanguageSelectionContainer.Short2FullLanguageName(ShortLangCode);

                    // Go to Code Extension
                    sr.Seek(1, SeekOrigin.Current);
                    buff[0] = br.ReadByte();

                    switch (buff[0] & 0x0F)
                    {
                        // from http://dvd.sourceforge.net/dvdinfo/sprm.html
                        case 1: subdesc[i] += " (Caption/Normal Size Char)"; break;
                        case 2: subdesc[i] += " (Caption/Large Size Char)"; break;
                        case 3: subdesc[i] += " (Caption For Children)"; break;
                        case 5: subdesc[i] += " (Closed Caption/Normal Size Char)"; break;
                        case 6: subdesc[i] += " (Closed Caption/Large Size Char)"; break;
                        case 7: subdesc[i] += " (Closed Caption For Children)"; break;
                        case 9: subdesc[i] += " (Forced Caption)"; break;
                        case 13: subdesc[i] += " (Director Comments/Normal Size Char)"; break;
                        case 14: subdesc[i] += " (Director Comments/Large Size Char)"; break;
                        case 15: subdesc[i] += " (Director Comments for Children)"; break;
                    }

                    if (buff[0] == 0) buff[0] = 1;

                    // go to the next sub stream
                    sr.Seek(2, SeekOrigin.Current);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           return subdesc;
        }
    }
}
