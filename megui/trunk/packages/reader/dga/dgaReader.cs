// ****************************************************************************
// 
// Copyright (C) 2005-2013 Doom9 & al
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
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

using MeGUI.core.util;

namespace MeGUI
{
    public class dgaFileFactory : IMediaFileFactory
    {

        #region IMediaFileFactory Members

        public IMediaFile Open(string file)
        {
            return new dgaFile(file);
        }

        public int HandleLevel(string file)
        {
            if (file.ToLower(System.Globalization.CultureInfo.InvariantCulture).EndsWith(".dga"))
                return 11;
            return -1;
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "dga"; }
        }

        #endregion
    }

/// <summary>
	/// Summary description for dgaReader.
	/// </summary>
    public class dgaFile : IMediaFile
    {
        private AvsFile reader;
        private string fileName;
        private VideoInformation info;

        /// <summary>
        /// initializes the dga reader
        /// </summary>
        /// <param name="fileName">the DGAVCIndex project file that this reader will process</param>
        public dgaFile(string fileName)
        {
            UpdateCacher.CheckPackage("dgavcindex");
            this.fileName = fileName;
            string strPath = Path.GetDirectoryName(MainForm.Instance.Settings.DGAVCIndex.Path);
            string strDLL = Path.Combine(strPath, "DGAVCDecode.dll");
            reader = AvsFile.ParseScript("LoadPlugin(\"" + strDLL + "\")\r\nAVCSource(\"" + this.fileName + "\")");
            this.readFileProperties();
        }

        /// <summary>
        /// reads the dga file, which is essentially a text file
        /// </summary>
        private void readFileProperties()
        {
            info = reader.VideoInfo.Clone();
            using (StreamReader sr = new StreamReader(fileName))
            {
                int iLineCount = 0;
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    iLineCount++;
                    if (iLineCount == 3)
                    {
                        string strSourceFile = line;
                        if (File.Exists(strSourceFile))
                        {
                            MediaInfoFile oInfo = new MediaInfoFile(strSourceFile);
                            info.DAR = oInfo.VideoInfo.DAR;
                        }
                        break;
                    }
                }
            }
        }
        #region properties
        public VideoInformation VideoInfo
        {
            get { return info; }
        }
        #endregion

        #region IMediaFile Members

        public bool CanReadVideo
        {
            get { return reader.CanReadVideo; }
        }

        public bool CanReadAudio
        {
            get { return false; }
        }

        public IVideoReader GetVideoReader()
        {
            return reader.GetVideoReader();
        }

        public IAudioReader GetAudioReader(int track)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            reader.Dispose();
        }

        #endregion
    }
}
