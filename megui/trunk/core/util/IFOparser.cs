// ****************************************************************************
// 
// Copyright (C) 2009  Kurtnoise (kurtnoise@free.fr)
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
using System.Collections;
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
            if (vobFiles.Length == 0) return null;            

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

        public static byte[] GetFileBlock(string strFile, long pos, int count)
        {
            using (FileStream stream = new FileStream(strFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] buf = new byte[count];
                stream.Seek(pos, SeekOrigin.Begin);
                if (stream.Read(buf, 0, count) != count)
                    return buf;
                return buf;
            }
        }

        public static short ToInt16(byte[] bytes) { return (short)((bytes[0] << 8) + bytes[1]); }
        public static uint ToInt32(byte[] bytes) { return (uint)((bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + bytes[3]); }
        public static short ToShort(byte[] bytes) { return ToInt16(bytes); }
        public static long ToFilePosition(byte[] bytes) { return ToInt32(bytes) * 0x800L; }

        public static long GetTotalFrames(TimeSpan time, int fps)
        {
            return (long)Math.Round(fps * time.TotalSeconds);
        }

        static string TwoLong(int val) { return string.Format("{0:D2}", val); }

        static int AsHex(int val)
        {
            int ret;
            int.TryParse(string.Format("{0:X2}", val), out ret);
            return ret;
        }

        internal static short? GetFrames(byte val)
        {
            int byte0_high = val >> 4;
            int byte0_low = val & 0x0F;
            if (byte0_high > 11)
                return (short)(((byte0_high - 12) * 10) + byte0_low);
            if ((byte0_high <= 3) || (byte0_high >= 8))
                return null;
            return (short)(((byte0_high - 4) * 10) + byte0_low);
        }

        internal static int GetFrames(TimeSpan time, int fps)
        {
            return (int)Math.Round(fps * time.Milliseconds / 1000.0);
        }
        internal static long GetPCGIP_Position(string ifoFile)
        {
            return ToFilePosition(GetFileBlock(ifoFile, 0xCC, 4));
        }

        internal static int GetProgramChains(string ifoFile, long pcgitPosition)
        {
            return ToInt16(GetFileBlock(ifoFile, pcgitPosition, 2));
        }

        internal static uint GetChainOffset(string ifoFile, long pcgitPosition, int programChain)
        {
            return ToInt32(GetFileBlock(ifoFile, (pcgitPosition + (8 * programChain)) + 4, 4));
        }

        internal static int GetNumberOfPrograms(string ifoFile, long pcgitPosition, uint chainOffset)
        {
            return GetFileBlock(ifoFile, (pcgitPosition + chainOffset) + 2, 1)[0];
        }

        internal static TimeSpan? ReadTimeSpan(string ifoFile, long pcgitPosition, uint chainOffset, out double fps)
        {
            return ReadTimeSpan(GetFileBlock(ifoFile, (pcgitPosition + chainOffset) + 4, 4), out fps);
        }

        internal static TimeSpan? ReadTimeSpan(byte[] playbackBytes, out double fps)
        {
            short? frames = GetFrames(playbackBytes[3]);
            int fpsMask = playbackBytes[3] >> 6;
            fps = fpsMask == 0x01 ? 25D : fpsMask == 0x03 ? (30D / 1.001D) : 0;
            if (frames == null)
                return null;

            try
            {
                int hours = AsHex(playbackBytes[0]);
                int minutes = AsHex(playbackBytes[1]);
                int seconds = AsHex(playbackBytes[2]);
                TimeSpan ret = new TimeSpan(hours, minutes, seconds);
                if (fps != 0)
                    ret = ret.Add(TimeSpan.FromSeconds((double)frames / fps));
                return ret;
            }
            catch { return null; }
        }

        /// <summary>
        /// get Audio Language from the IFO file
        /// </summary>
        /// <param name="fileName">name of the IFO file</param>
        /// <param name="count">the audio stream number</param>
        /// <returns>Language as String</returns>
        public static string getAudioLanguage(string FileName, int count)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Stream sr = br.BaseStream;

            // go to audio stream number
            sr.Seek(0x203, SeekOrigin.Begin);
            byte a = br.ReadByte();
            sr.Seek(2, SeekOrigin.Current);
            if (count > 0) sr.Seek(8*count, SeekOrigin.Current);
            byte[] buff = new byte[2];
            br.Read(buff, 0, 2);
            string ShortLangCode = String.Format("{0}{1}", (char)buff[0], (char)buff[1]);
            string audioLang = LanguageSelectionContainer.Short2FullLanguageName(ShortLangCode);
            fs.Close();
            return audioLang;
        }

        /// <summary>
        /// get several Subtitles Informations from the IFO file
        /// </summary>
        /// <param name="fileName">name of the IFO file</param>
        /// <returns>several infos as String</returns>       
        public static string[] GetSubtitlesStreamsInfos(string FileName, int iPGC, bool bGetAllStreams)
        {
            byte[] buff = new byte[4];
            byte s = 0;
            string[] subdesc = new string[s];
            string[] substreams = new string[s];

            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                Stream sr = br.BaseStream;

                // go to the substream #1
                sr.Seek(0x255, SeekOrigin.Begin);

                s = br.ReadByte();
                if (s > 32 || bGetAllStreams)
                    s = 32; // force the max #. According to the specs 32 is the max value for subtitles streams.

                subdesc = new string[s];

                // go to the Language Code
                sr.Seek(2, SeekOrigin.Current);

                for (int i = 0; i < s; i++)
                {
                    // Presence (1 bit), Coding Mode (1bit), Short Language Code (2bits), Language Extension (1bit), Sub Picture Caption Type (1bit)
                    br.Read(buff, 0, 2);

                    if (buff[0] == 0 && buff[1] == 0)
                    {
                        subdesc[i] = "unknown";
                    }
                    else
                    {
                        string ShortLangCode = String.Format("{0}{1}", (char)buff[0], (char)buff[1]);
                        subdesc[i] = LanguageSelectionContainer.Short2FullLanguageName(ShortLangCode);
                    }

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

                // find the PGC starting address of the requested PGC number
                sr.Seek(0x1000 + 0x0C + (iPGC - 1) * 0x08, SeekOrigin.Begin);
                br.Read(buff, 0, 4);

                // go to the starting address of the requested PGC number
                sr.Seek(0x1000 + buff[3] + buff[2] * 256 + buff[1] * 256 ^ 2 + buff[0] * 256 ^ 3, SeekOrigin.Begin);

                // go to the subtitle starting address
                sr.Seek(0x1B, SeekOrigin.Current);

                substreams = new string[32];
                for (int i = 0; i < 32; i++)
                {
                    if (i >= subdesc.Length)
                        break;

                    br.Read(buff, 0, 4);

                    if (buff[0] == 0)
                        continue;

                    // match the stream number with the stream ID
                    if (buff[1] == buff[2])
                    {
                        if (String.IsNullOrEmpty(substreams[buff[1]]))
                            substreams[buff[1]] = "[" + String.Format("{0:00}", buff[1]) + "]  - " + subdesc[i];
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(substreams[buff[1]]))
                            substreams[buff[1]] = "[" + String.Format("{0:00}", buff[1]) + "]  - " + subdesc[i] + " wide";
                        if (String.IsNullOrEmpty(substreams[buff[2]]))
                            substreams[buff[2]] = "[" + String.Format("{0:00}", buff[2]) + "]  - " + subdesc[i] + " letterbox";
                    }
                }

                if (bGetAllStreams)
                {
                    for (int i = 0; i < 32; i++)
                    {
                        substreams[i] = "[" + String.Format("{0:00}", i) + "]  - not detected";
                    }
                }
                else
                {
                    ArrayList arrList = new ArrayList();
                    foreach (string strItem in substreams)
                        if (!String.IsNullOrEmpty(strItem))
                            arrList.Add(strItem);
                    substreams = new string[arrList.Count];
                    for (int i = 0; i < arrList.Count; i++)
                        substreams[i] = arrList[i].ToString();
                }

                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return substreams;
        }

        /// <summary>
        /// get number of PGCs
        /// </summary>
        /// <param name="fileName">name of the IFO file</param>
        /// <returns>number of PGS as unsigned integer</returns>
        public static uint getPGCnb(string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Stream sr = br.BaseStream;

            sr.Seek(0xCC, SeekOrigin.Begin);
            uint buf = ReadUInt32(br);									// Read PGC offset
            sr.Seek(2048 * buf + 0x1, SeekOrigin.Begin);			// Move to beginning of PGC
            long VTS_PGCITI_start_position = sr.Position - 1;
            byte nPGCs = br.ReadByte();									// Number of PGCs
            fs.Close();

            return nPGCs;
        }

        private static uint ReadUInt32(BinaryReader br)
        {
            uint val = (
                ((uint)br.ReadByte()) << 24 |
                ((uint)br.ReadByte()) << 16 |
                ((uint)br.ReadByte()) << 8 |
                ((uint)br.ReadByte()));
            return val;
        }
    }
}
