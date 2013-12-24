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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using MeGUI.core.util;

namespace MeGUI
{
    class MkvInfo
    {
        private bool _bHasChapters, _bMuxable;
        private String _strResult, _strFile;
        private List<TrackInfo> _oTracks = new List<TrackInfo>();
        private LogItem _oLog;

        public MkvInfo(String strFile, ref LogItem oLog)
        {
            if (oLog == null)
                this._oLog = MainForm.Instance.Log.Info("MkvInfo");
            else
                this._oLog = oLog;
            this._strFile = strFile;
            _bMuxable = true;
            getInfo();
        }

        public bool HasChapters
        {
            get { return _bHasChapters; }
        }

        public bool IsMuxable
        {
            get { return _bMuxable; }
        }

        private void getInfo()
        {
            _strResult = null;
            using (Process mkvinfo = new Process())
            {
                UpdateCacher.CheckPackage("mkvmerge");
                mkvinfo.StartInfo.FileName = MainForm.Instance.Settings.MkvMerge.Path;
                mkvinfo.StartInfo.Arguments = string.Format("--ui-language en --identify-verbose \"{0}\"", _strFile);
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
                    while (!mkvinfo.HasExited) // wait until the process has terminated without locking the GUI
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                    mkvinfo.WaitForExit();

                    _oLog.LogValue("MkvInfo", _strResult);
                    if (mkvinfo.ExitCode != 0)
                    {
                        _bMuxable = false;
                        _bHasChapters = false;
                    }
                    else
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
                UpdateCacher.CheckPackage("mkvmerge");
                mkvinfo.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.MkvMerge.Path), "mkvextract.exe");
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
                    while (!mkvinfo.HasExited) // wait until the process has terminated without locking the GUI
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
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
            foreach (String Line in Regex.Split(_strResult, "\r\n"))
            {
                if (Line.StartsWith("Chapters:"))
                    _bHasChapters = true;
                else if (Line.Contains("(unsupported "))
                    _bMuxable = false;
                else if (Line.Contains("unsupported container"))
                    _bMuxable = false;
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
