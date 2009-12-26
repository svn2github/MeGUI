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

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

using MeGUI.core.util;

namespace MeGUI
{
    public class dgiFileFactory : IMediaFileFactory
    {

        #region IMediaFileFactory Members

        public IMediaFile Open(string file)
        {
            return new dgiFile(file);
        }

        public int HandleLevel(string file)
        {
            if (file.ToLower().EndsWith(".dgi"))
                return 12;
            return -1;
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "dgi"; }
        }

        #endregion
    }

    /// <summary>
    /// Summary description for dgiReader.
    /// </summary>
    public class dgiFile : IMediaFile
    {
        private AvsFile reader;
        private string fileName;
        private MediaFileInfo info;

        /// <summary>
        /// initializes the dgi reader
        /// </summary>
        /// <param name="fileName">the DGNVIndex project file that this reader will process</param>
        public dgiFile(string fileName)
        {
            this.fileName = fileName;
            string strPath = Path.GetDirectoryName(MainForm.Instance.Settings.DgnvIndexPath);
            if (MainForm.Instance.Settings.UseCUVIDserver == true)
            {
                string strDLL = Path.Combine(strPath, "DGDecodeNV.dll");
                reader = AvsFile.ParseScript("LoadPlugin(\"" + strDLL + "\")\r\nDGSource(\"" + this.fileName + "\")");
            }
            else
            {
                string strDLL = Path.Combine(strPath, "DGMultiDecodeNV.dll");
                reader = AvsFile.ParseScript("LoadPlugin(\"" + strDLL + "\")\r\nDGMultiSource(\"" + this.fileName + "\")");
            }
            this.readFileProperties();
        }

        /// <summary>
        /// reads the dgi file, which is essentially a text file
        /// </summary>
        private void readFileProperties()
        {
            info = reader.Info.Clone();
            Dar dar = new Dar(reader.Info.Width, reader.Info.Height);

            info.DAR = dar;
        }
        #region properties
        public MediaFileInfo Info
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

