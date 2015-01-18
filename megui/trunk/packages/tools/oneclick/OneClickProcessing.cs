// ****************************************************************************
// 
// Copyright (C) 2005-2015 Doom9 & al
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
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.packages.tools.oneclick
{
    public class OneClickProcessing
    {
        private OneClickWindow oOneClickWindow;
        private string strInput;
        private LogItem _log;
        private OneClickSettings _oSettings;

        public OneClickProcessing(OneClickWindow oWindow, String strFileOrFolderName, OneClickSettings oSettings, LogItem oLog)
        {
            this.oOneClickWindow = oWindow;
            this.strInput = strFileOrFolderName;
            this._log = oLog;
            this._oSettings = oSettings;

            if (!getInputDVDBased(oSettings))
                if (!getInputBluRayBased(oSettings))
                    if (!getInputFolderBased(oSettings))
                        if (!getInputFileBased(oSettings))
                            this.oOneClickWindow.setOpenFailure();   
        }

        // batch processing
        public OneClickProcessing(OneClickWindow oWindow, List<OneClickFilesToProcess> arrFilesToProcess, OneClickSettings oSettings, LogItem oLog)
        {
            this.oOneClickWindow = oWindow;
            this._log = oLog;

            List<OneClickFilesToProcess> arrFilesToProcessNew = new List<OneClickFilesToProcess>();
            MediaInfoFile iFile = null;

            foreach (OneClickFilesToProcess oFileToProcess in arrFilesToProcess)
            {
                if (iFile == null)
                {
                    MediaInfoFile iFileTemp = new MediaInfoFile(oFileToProcess.FilePath, ref _log, oFileToProcess.TrackNumber);
                    if (iFileTemp.recommendIndexer(oSettings.IndexerPriority, true))
                        iFile = iFileTemp;
                    else if (iFileTemp.ContainerFileTypeString.Equals("AVS"))
                    {
                        iFile = iFileTemp;
                        iFile.IndexerToUse = FileIndexerWindow.IndexType.NONE;
                    }
                    else
                        _log.LogEvent(oFileToProcess.FilePath + " cannot be processed as no indexer can be used. skipping...");
                }
                else
                    arrFilesToProcessNew.Add(oFileToProcess);
            }
            if (iFile != null)
                oOneClickWindow.setInputData(iFile, arrFilesToProcessNew);
            else
                oOneClickWindow.setInputData(null, new List<OneClickFilesToProcess>()); // not demuxable
        }

        /// <summary>
        /// checks if the files/folders can be processed as DVD
        /// </summary>
        /// <returns>true if the files/folder can be processed as DVD, false otherwise</returns>
        private bool getInputDVDBased(OneClickSettings oSettings)
        {
            string videoIFO;
            string path;

            if (File.Exists(this.strInput) && Path.GetExtension(this.strInput).ToLowerInvariant().Equals(".ifo"))
            {
                path = Path.GetDirectoryName(this.strInput);
                videoIFO = this.strInput;
            }
            else if (File.Exists(this.strInput) && Path.GetExtension(this.strInput).ToLowerInvariant().Equals(".vob"))
            {
                path = Path.GetDirectoryName(this.strInput);
                if (Path.GetFileName(this.strInput).ToUpper(System.Globalization.CultureInfo.InvariantCulture).Substring(0, 4) == "VTS_")
                    videoIFO = this.strInput.Substring(0, this.strInput.LastIndexOf("_")) + "_0.IFO";
                else
                    videoIFO = Path.ChangeExtension(this.strInput, ".IFO");
                if (!File.Exists(videoIFO))
                    return false;
                else
                    this.strInput = videoIFO;
            }
            else if (Directory.Exists(this.strInput) && Directory.GetFiles(this.strInput, "*.ifo").Length > 0)
            {
                path = this.strInput;
                videoIFO = Path.Combine(path, "VIDEO_TS.IFO");
            }
            else if (Directory.Exists(Path.Combine(this.strInput, "VIDEO_TS")) && Directory.GetFiles(Path.Combine(this.strInput, "VIDEO_TS"), "*.IFO").Length > 0)
            {
                path = Path.Combine(this.strInput, "VIDEO_TS");
                videoIFO = Path.Combine(path, "VIDEO_TS.IFO");
            }
            else
                return false;

            ChapterExtractor ex = new DvdExtractor();
            using (frmStreamSelect frm = new frmStreamSelect(ex, SelectionMode.MultiExtended))
            {
                frm.Text = "Select your Titles";
                ex.GetStreams(this.strInput);
                if (frm.ChapterCount == 1 || (frm.ChapterCount > 1 && frm.ShowDialog() == DialogResult.OK))
                {
                    List<ChapterInfo> oChapterList = frm.SelectedMultipleChapterInfo;
                    if (oChapterList.Count > 0)
                    {
                        List<OneClickFilesToProcess> arrFilesToProcess = new List<OneClickFilesToProcess>();
                        MediaInfoFile iFile = null;
                        int iTitleNumber = 1;

                        foreach (ChapterInfo oChapterInfo in oChapterList)
                        {
                            string strVOBFile = Path.Combine(path, oChapterInfo.Title + "_1.VOB");

                            if (iFile == null && File.Exists(strVOBFile))
                            {
                                MediaInfoFile iFileTemp = new MediaInfoFile(strVOBFile, ref _log, oChapterInfo.TitleNumber);
                                if (iFileTemp.recommendIndexer(oSettings.IndexerPriority, false))
                                {
                                    iFile = iFileTemp;
                                    iTitleNumber = oChapterInfo.TitleNumber;
                                }
                                else
                                    _log.LogEvent(strVOBFile + " cannot be processed as no indexer can be used. skipping...");
                            }
                            else
                                arrFilesToProcess.Add(new OneClickFilesToProcess(strVOBFile, oChapterInfo.TitleNumber));
                        }
                        if (iFile != null)
                        {
                            oOneClickWindow.setInputData(iFile, arrFilesToProcess);
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// checks if the files/folders can be processed as BluRay
        /// </summary>
        /// <returns>true if the files/folder can be processed as BluRay, false otherwise</returns>
        private bool getInputBluRayBased(OneClickSettings oSettings)
        {
            string path = this.strInput;
            while (!Path.GetFullPath(path).Equals(Path.GetPathRoot(path)))
            {
                string bdmvPath = Path.Combine(path, "BDMV\\PLAYLIST");
                if (Directory.Exists(bdmvPath))
                    break;

                DirectoryInfo pathInfo = new DirectoryInfo(path);
                path = pathInfo.Parent.FullName;
            }

            if (!Directory.Exists(Path.Combine(path, "BDMV\\PLAYLIST")))
                return false;

            ChapterExtractor ex = new BlurayExtractor();
            using (frmStreamSelect frm = new frmStreamSelect(ex, SelectionMode.MultiExtended))
            {
                frm.Text = "Select your Titles";
                ex.GetStreams(path);
                if (frm.ChapterCount == 1 || (frm.ChapterCount > 1 && frm.ShowDialog() == DialogResult.OK))
                {
                    List<ChapterInfo> oChapterList = frm.SelectedMultipleChapterInfo;
                    if (oChapterList.Count > 0)
                    {
                        List<OneClickFilesToProcess> arrFilesToProcess = new List<OneClickFilesToProcess>();
                        MediaInfoFile iFile = null;

                        foreach (ChapterInfo oChapterInfo in oChapterList)
                        {
                            string strFile = path + @"\BDMV\PLAYLIST\" + oChapterInfo.SourceName;

                            if (iFile == null && File.Exists(strFile))
                            {
                                iFile = new MediaInfoFile(strFile, ref _log);
                                iFile.recommendIndexer(oSettings.IndexerPriority, false);
                            }
                            else
                                arrFilesToProcess.Add(new OneClickFilesToProcess(strFile, 1));
                        }
                        if (iFile != null)
                        {
                            oOneClickWindow.setInputData(iFile, arrFilesToProcess);
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// checks if the input folder can be processed
        /// </summary>
        /// <returns>true if the folder can be processed, false otherwise</returns>
        private bool getInputFolderBased(OneClickSettings oSettings)
        {
            List<OneClickFilesToProcess> arrFilesToProcess = new List<OneClickFilesToProcess>();
            MediaInfoFile iFile = null;

            if (!Directory.Exists(this.strInput))
                return false;

            foreach (string strFileName in Directory.GetFiles(this.strInput))
            {
                if (iFile == null)
                {
                    MediaInfoFile iFileTemp = new MediaInfoFile(strFileName, ref _log);
                    if (iFileTemp.recommendIndexer(oSettings.IndexerPriority, true))
                        iFile = iFileTemp;
                    else if (iFileTemp.ContainerFileTypeString.Equals("AVS"))
                    {
                        iFile = iFileTemp;
                        iFile.IndexerToUse = FileIndexerWindow.IndexType.NONE;
                    }
                    else
                        _log.LogEvent(strFileName + " cannot be processed as no indexer can be used. skipping...");
                }
                else
                    arrFilesToProcess.Add(new OneClickFilesToProcess(strFileName, 1));
            }
            if (iFile != null)
            {
                oOneClickWindow.setInputData(iFile, arrFilesToProcess);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// checks if the input file can be processed
        /// </summary>
        /// <returns>true if the file can be processed, false otherwise</returns>
        private bool getInputFileBased(OneClickSettings oSettings)
        {
            if (File.Exists(this.strInput))
            {
                MediaInfoFile iFile = new MediaInfoFile(this.strInput, ref this._log);
                if (iFile.recommendIndexer(oSettings.IndexerPriority, true))
                    return getInputIndexerBased(iFile, oSettings);
                else if (iFile.ContainerFileTypeString.Equals("AVS"))
                {
                    iFile.IndexerToUse = FileIndexerWindow.IndexType.NONE;
                    return getInputIndexerBased(iFile, oSettings);
                }
            }

            return false;
        }

        private bool getInputIndexerBased(MediaInfoFile iFile, OneClickSettings oSettings)
        {
            oOneClickWindow.setInputData(iFile, new List<OneClickFilesToProcess>());
            return true;
        }
    }
}
