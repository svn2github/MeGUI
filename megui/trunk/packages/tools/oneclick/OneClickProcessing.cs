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
                    if (iFileTemp.recommendIndexer(oSettings.IndexerPriority))
                        iFile = iFileTemp;
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

            if (File.Exists(this.strInput) && Path.GetExtension(this.strInput).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".ifo"))
            {
                path = Path.GetDirectoryName(this.strInput);
                videoIFO = this.strInput;
            }
            else if (File.Exists(this.strInput) && Path.GetExtension(this.strInput).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".vob"))
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
                                if (iFileTemp.recommendIndexer(oSettings.IndexerPriority))
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
            string path = Path.Combine(Path.Combine(this.strInput, "BDMV"), "PLAYLIST");
            if (!Directory.Exists(path))
                return false;

            ChapterExtractor ex = new BlurayExtractor();
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

                        foreach (ChapterInfo oChapterInfo in oChapterList)
                        {
                            string strFile = this.strInput + @"\BDMV\PLAYLIST\" + oChapterInfo.SourceName;

                            if (iFile == null && File.Exists(strFile))
                            {
                                iFile = new MediaInfoFile(strFile, ref _log);
                                iFile.recommendIndexer(oSettings.IndexerPriority);
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
                    if (iFileTemp.recommendIndexer(oSettings.IndexerPriority))
                        iFile = iFileTemp;
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
                if (iFile.recommendIndexer(oSettings.IndexerPriority))
                    return getInputIndexerBased(iFile, oSettings);
                else if (iFile.ContainerFileTypeString.Equals("AVS"))
                {
                    iFile.IndexerToUse = FileIndexerWindow.IndexType.NONE;
                    return getInputIndexerBased(iFile, oSettings);
                }
            }

            return false;
        }

        //private bool getInputEac3toBased(OneClickSettings oSettings)
        //{
        //    if (!Directory.Exists(this.strInput))
        //        return false;

        //    oEac3toInfo = new eac3toInfo(strInput, null, _log);
        //    oEac3toInfo.FetchInformationCompleted += new OnFetchInformationCompletedHandler(setInputEac3toBased);
        //    oEac3toInfo.FetchAllInformation();
        //    return true;
        //}

        //private void setInputEac3toBased(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    string x;
        //    if (e.Error != null)
        //        x = e.Error.ToString();

        //    if (oEac3toInfo.Features.Count == 0)
        //    {
        //        continueGet(this._oSettings);
        //        return;
        //    }

        //    foreach (eac3to.Feature oFeature in oEac3toInfo.Features)
        //    {
        //        string strFile = this.strInput + @"\BDMV\PLAYLIST\" + oEac3toInfo.Features[0].Name;
        //        MediaInfoFile iFile = new MediaInfoFile(strFile, ref _log);
        //        oOneClickWindow.setInputData(iFile, new List<OneClickFilesToProcess>());
        //        return;
        //    }

        //    //int maxHorizontalResolution = 0;
        //    //int iHorizontalResolution = 0;
        //    //List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
        //    //foreach (eac3to.Stream oStream in oEac3toInfo.Features[0].Streams)
        //    //{
        //    //    if (oStream.Type == eac3to.StreamType.Video)
        //    //    {
        //    //        //h264/AVC, English, 1280x688 23.975p
        //    //        //MPEG2, 480i60 /1.001 (16:9)
        //    //        string[] info = oStream.Description.Split(',');
        //    //        if (info.Length > 1)
        //    //        {
        //    //            if (info.Length > 2)
        //    //                info = info[2].Trim().Split(' ');
        //    //            else
        //    //                info = info[1].Trim().Split(' ');
        //    //            if (Regex.IsMatch(info[0], "^[0-9999]+x[0-9999]+.+$", RegexOptions.Compiled))
        //    //                iHorizontalResolution = Int32.Parse(info[0].Split('x')[0]);
        //    //            else if (Regex.IsMatch(info[0], "^[0-9999]+[p,i]+[0-9999]+.+$", RegexOptions.Compiled))
        //    //            {
        //    //                iHorizontalResolution = Int32.Parse(info[0].Split(new char[] { 'p', 'i' })[0]);
        //    //                switch (iHorizontalResolution)
        //    //                {
        //    //                    case 480: iHorizontalResolution = 720; break;
        //    //                    case 576: iHorizontalResolution = 720; break;
        //    //                    case 720: iHorizontalResolution = 1280; break;
        //    //                    case 1080: iHorizontalResolution = 1920; break;
        //    //                }
        //    //            }
        //    //            if (iHorizontalResolution > maxHorizontalResolution)
        //    //                maxHorizontalResolution = iHorizontalResolution;
        //    //        }

        //    //        else

        //    //            break;
        //    //    }
        //    //    else if (oStream.Type == eac3to.StreamType.Audio)
        //    //    {
        //    //        AudioTrackInfo oAudio = new AudioTrackInfo(oStream.Language, oStream.Description, oStream.Number);
        //    //        audioTracks.Add(oAudio);
        //    //    }
        //    //    else if (oStream.Type == eac3to.StreamType.Subtitle)
        //    //    {
        //    //        AudioTrackInfo oAudio = new AudioTrackInfo(oStream.Language, oStream.Description, oStream.Number);
        //    //        audioTracks.Add(oAudio);
        //    //    }
        //    //}

            
        //}

        private bool getInputIndexerBased(MediaInfoFile iFile, OneClickSettings oSettings)
        {
            oOneClickWindow.setInputData(iFile, new List<OneClickFilesToProcess>());
            return true;
        }
    }
}
