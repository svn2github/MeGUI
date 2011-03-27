// ****************************************************************************
//
// Copyright (C) 2005-2011  Doom9 & al
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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using MeGUI.core.util;

namespace MeGUI
{
    class MkvInfo
    {
        private bool _bHasChapters;
        private String _strResult, _strFile;
        private List<MkvInfoTrack> _oTracks = new List<MkvInfoTrack>();
        private LogItem _oLog;

        public MkvInfo(String strFile, ref LogItem oLog)
        {
            this._oLog = oLog;
            this._strFile = strFile;
            getInfo();
        }

        public bool HasChapters
        {
            get { return _bHasChapters; }
        }

        public List<MkvInfoTrack> Track
        {
            get { return _oTracks; }
        }

        private void getInfo()
        {
            _strResult = null;
            using (Process mkvinfo = new Process())
            {
                mkvinfo.StartInfo.FileName = MainForm.Instance.Settings.MkvInfoPath;
                mkvinfo.StartInfo.Arguments = string.Format("--ui-language en \"{0}\"", _strFile);
                mkvinfo.StartInfo.CreateNoWindow = true;
                mkvinfo.StartInfo.UseShellExecute = false;
                mkvinfo.StartInfo.RedirectStandardOutput = true;
                mkvinfo.StartInfo.RedirectStandardError = true;
                mkvinfo.StartInfo.ErrorDialog = false;
                mkvinfo.EnableRaisingEvents = true;
                mkvinfo.ErrorDataReceived += new DataReceivedEventHandler(backgroundWorker_ErrorDataReceived);
                mkvinfo.OutputDataReceived += new DataReceivedEventHandler(backgroundWorker_OutputDataReceived);
                try
                {
                    mkvinfo.Start();
                    mkvinfo.BeginErrorReadLine();
                    mkvinfo.BeginOutputReadLine();
                    mkvinfo.WaitForExit();
                    
                    if (mkvinfo.ExitCode != 0)
                        _oLog.LogValue("MkvInfo", _strResult, ImageType.Error);
                    else
                        _oLog.LogValue("MkvInfo", _strResult);
                    parseResult();
                }
                catch (Exception ex)
                {
                    _oLog.LogValue("MkvInfo - Unhandled Error", ex, ImageType.Error);
                }
                finally
                {
                    mkvinfo.ErrorDataReceived -= new DataReceivedEventHandler(backgroundWorker_ErrorDataReceived);
                    mkvinfo.OutputDataReceived -= new DataReceivedEventHandler(backgroundWorker_OutputDataReceived);
                }
            } 
        }

        public bool extractChapters(String strChapterFile)
        {
            _strResult = null;
            bool bResult = false;
            using (Process mkvinfo = new Process())
            {
                mkvinfo.StartInfo.FileName = MainForm.Instance.Settings.MkvExtractPath;
                mkvinfo.StartInfo.Arguments = string.Format("chapters \"{0}\" --ui-language en --simple", _strFile);
                mkvinfo.StartInfo.CreateNoWindow = true;
                mkvinfo.StartInfo.UseShellExecute = false;
                mkvinfo.StartInfo.RedirectStandardOutput = true;
                mkvinfo.StartInfo.RedirectStandardError = true;
                mkvinfo.StartInfo.ErrorDialog = false;
                mkvinfo.EnableRaisingEvents = true;
                mkvinfo.ErrorDataReceived += new DataReceivedEventHandler(backgroundWorker_ErrorDataReceived);
                mkvinfo.OutputDataReceived += new DataReceivedEventHandler(backgroundWorker_OutputDataReceived);
                try
                {
                    mkvinfo.Start();
                    mkvinfo.BeginErrorReadLine();
                    mkvinfo.BeginOutputReadLine();
                    mkvinfo.WaitForExit();

                    if (mkvinfo.ExitCode != 0)
                        _oLog.LogValue("MkvExtract", _strResult, ImageType.Error);
                    else
                    {
                        _oLog.LogValue("MkvExtract", _strResult);
                        try
                        {
                            StreamWriter sr = new StreamWriter(strChapterFile, false);
                            sr.Write(_strResult);
                            sr.Close();
                            bResult = true;
                        }
                        catch (Exception e)
                        {
                            _oLog.LogValue("MkvExtract - Unhandled Error", e, ImageType.Error);
                        }
                    }
                    parseResult();
                }
                catch (Exception ex)
                {
                    _oLog.LogValue("MkvExtract - Unhandled Error", ex, ImageType.Error);
                }
                finally
                {
                    mkvinfo.ErrorDataReceived -= new DataReceivedEventHandler(backgroundWorker_ErrorDataReceived);
                    mkvinfo.OutputDataReceived -= new DataReceivedEventHandler(backgroundWorker_OutputDataReceived);
                }
                return bResult;
            }
        }

        private void parseResult()
        {
            MkvInfoTrack oTempTrack = new MkvInfoTrack(_strFile);
            foreach (String Line in Regex.Split(_strResult, "\r\n"))
            {
                string strLine = Line.Replace("|", "");
                if (strLine.StartsWith("+") || strLine.StartsWith(" +"))
                {
                    // new track start?
                    if (oTempTrack.TrackID != -1)
                        _oTracks.Add(oTempTrack);
                    oTempTrack = new MkvInfoTrack(_strFile);
                }
                
                if (strLine.StartsWith("  + Track number: "))
                {
                    oTempTrack.TrackID = Int32.Parse(strLine.Substring(18));
                }
                else if (strLine.StartsWith("  + Track type: "))
                {
                    if (strLine.Equals("  + Track type: audio"))
                        oTempTrack.Type = MkvInfoTrackType.Audio;
                    else if (strLine.Equals("  + Track type: subtitles"))
                        oTempTrack.Type = MkvInfoTrackType.Subtitle;
                    else if (strLine.Equals("  + Track type: video"))
                        oTempTrack.Type = MkvInfoTrackType.Video;
                }
                else if (strLine.Equals("  + Default flag: 0"))
                {
                    oTempTrack.DefaultTrack = false;
                }
                else if (strLine.Equals("  + Forced flag: 1"))
                {
                    oTempTrack.ForcedTrack = true;
                }
                else if (strLine.StartsWith("  + Codec ID: "))
                {
                    oTempTrack.CodecID = strLine.Substring(14);
                }
                else if (strLine.StartsWith("  + Language: "))
                {
                    oTempTrack.Language = strLine.Substring(14);
                }
                else if (strLine.StartsWith("  + Name: "))
                {
                    oTempTrack.Name = strLine.Substring(10);
                }
                else if (strLine.StartsWith("   + Channels: "))
                {
                    oTempTrack.AudioChannels = strLine.Substring(15) + " Channels";
                }
                else if (strLine.Equals("+ Chapters"))
                {
                    _bHasChapters = true;
                }
                else if (strLine.StartsWith("  + Default duration: "))
                {
                    if (oTempTrack.Type != MkvInfoTrackType.Video)
                        continue;

                    String[] fps = strLine.Split(' ');
                    if (fps.Length < 7)
                        continue;

                    oTempTrack.FPS = decimal.Parse(fps[6].Substring(1), new System.Globalization.CultureInfo("en-us"));
                }
            }
        }

        void backgroundWorker_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
                _strResult += e.Data.Trim() + "\r\n";
        }

        void backgroundWorker_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
                _strResult += e.Data.Trim() + "\r\n";
        }
    }
}
