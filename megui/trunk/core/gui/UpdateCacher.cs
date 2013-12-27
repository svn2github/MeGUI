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
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip;
using SevenZip;

using MeGUI.core.util;


namespace MeGUI
{
    class UpdateCacher
    {
        public const int REMOVE_PACKAGE_AFTER_DAYS = 60;

        public static void flushOldCachedFilesAsync(List<string> urls, UpdateWindow oUpdate)
        {
            string updateCache = MainForm.Instance.Settings.MeGUIUpdateCache;
            if (string.IsNullOrEmpty(updateCache)
                || !Directory.Exists(updateCache))
                return;

            DirectoryInfo fi = new DirectoryInfo(updateCache);
            FileInfo[] files = fi.GetFiles();

            for (int i = 0; i < urls.Count; ++i)
            {
                urls[i] = urls[i].ToLower(System.Globalization.CultureInfo.InvariantCulture);
            }

            foreach (FileInfo f in files)
            {
                if (urls.IndexOf(f.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture)) < 0)
                {
                    if (DateTime.Now - f.LastWriteTime > new TimeSpan(REMOVE_PACKAGE_AFTER_DAYS, 0, 0, 0, 0))
                    {
                        f.Delete();
                        oUpdate.AddTextToLog("Deleted cached file " + f.Name, ImageType.Information);
                    }
                }
            }
        }

        private static void ensureSensibleCacheFolderExists()
        {
            FileUtil.ensureDirectoryExists(MainForm.Instance.Settings.MeGUIUpdateCache);
        }

        public static UpdateWindow.ErrorState DownloadFile(string url, Uri serverAddress,
            out Stream str, DownloadProgressChangedEventHandler wc_DownloadProgressChanged, UpdateWindow oUpdate)
        {
            ensureSensibleCacheFolderExists();
            UpdateWindow.ErrorState er = UpdateWindow.ErrorState.Successful;
            string updateCache = MainForm.Instance.Settings.MeGUIUpdateCache;
            string localFilename = Path.Combine(updateCache, url);
            bool downloadFile = true;

            if (File.Exists(localFilename))
            {
                FileInfo finfo = new FileInfo(localFilename);
                if (finfo.Length == 0)
                {
                    oUpdate.AddTextToLog(localFilename + " is empty. Deleting file.", ImageType.Information);
                    UpdateCacher.FlushFile(localFilename, oUpdate);
                }

                // check the zip file
                if (localFilename.ToLowerInvariant().EndsWith(".zip"))
                {
                    try
                    {
                        ZipFile zipFile = new ZipFile(localFilename);
                        if (zipFile.TestArchive(true) == false)
                        {
                            oUpdate.AddTextToLog("Could not unzip " + localFilename + ". Deleting file.", ImageType.Information);
                            UpdateCacher.FlushFile(localFilename, oUpdate);
                        }
                        else
                            downloadFile = false;
                    }
                    catch
                    {
                        oUpdate.AddTextToLog("Could not unzip " + localFilename + ". Deleting file.", ImageType.Error);
                        UpdateCacher.FlushFile(localFilename, oUpdate);
                    }
                }
                else if (localFilename.ToLowerInvariant().EndsWith(".7z")) // check the 7-zip file
                {
                    try
                    {
                        SevenZipExtractor oArchive = new SevenZipExtractor(localFilename);
                        if (oArchive.Check() == false)
                        {
                            oUpdate.AddTextToLog("Could not extract " + localFilename + ". Deleting file.", ImageType.Information);
                            UpdateCacher.FlushFile(localFilename, oUpdate);
                        }
                        else
                            downloadFile = false;
                    }
                    catch
                    {
                        oUpdate.AddTextToLog("Could not extract " + localFilename + ". Deleting file.", ImageType.Error);
                        UpdateCacher.FlushFile(localFilename, oUpdate);
                    }
                }
                else
                    downloadFile = false;
            }

            if (downloadFile)
            {
                WebClient wc = new WebClient();

                // check for proxy authentication...
                wc.Proxy = HttpProxy.GetProxy(MainForm.Instance.Settings);

                ManualResetEvent mre = new ManualResetEvent(false);
                wc.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
                {
                    if (e.Error != null)
                        er = UpdateWindow.ErrorState.CouldNotDownloadFile;

                    mre.Set();
                };

                wc.DownloadProgressChanged += wc_DownloadProgressChanged;

                wc.DownloadFileAsync(new Uri(serverAddress, url), localFilename);
                mre.WaitOne();

                if (File.Exists(localFilename))
                {
                    FileInfo finfo = new FileInfo(localFilename);
                    if (finfo.Length == 0)
                        UpdateCacher.FlushFile(localFilename, oUpdate);
                }
            }

            try
            {
                if (File.Exists(localFilename))
                    str = File.OpenRead(localFilename);
                else
                    str = null;
            }
            catch (IOException)
            {
                str = null;
                return UpdateWindow.ErrorState.CouldNotDownloadFile;
            }

            return er;
        }

        public static void FlushFile(string p, UpdateWindow oUpdate)
        {
            string localFilename = Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, p);
            try
            {
                File.Delete(localFilename);
            }
            catch (IOException) 
            {
                oUpdate.AddTextToLog("Could not delete file " + localFilename, ImageType.Error);
            }
        }

        public static bool CheckPackage(string package)
        {
            return CheckPackage(package, true, true);
        }

        public static bool CheckPackage(string package, bool enablePackage, bool forceUpdate)
        {
            switch (package)
            {
                case "aften": return MainForm.Instance.Settings.Aften.Update(package, enablePackage, forceUpdate);
                case "avimux_gui": return MainForm.Instance.Settings.AviMuxGui.Update(package, enablePackage, forceUpdate);
                case "bassaudio": return MainForm.Instance.Settings.BassAudio.Update(package, enablePackage, forceUpdate);
                case "besplit": return MainForm.Instance.Settings.BeSplit.Update(package, enablePackage, forceUpdate);
                case "dgavcindex": return MainForm.Instance.Settings.DGAVCIndex.Update(package, enablePackage, forceUpdate);
                case "dgindex": return MainForm.Instance.Settings.DGIndex.Update(package, enablePackage, forceUpdate);
                case "dgindexnv": return MainForm.Instance.Settings.DGIndexNV.Update(package, enablePackage, forceUpdate);
                case "eac3to": return MainForm.Instance.Settings.Eac3to.Update(package, enablePackage, forceUpdate);
                case "ffmpeg": return MainForm.Instance.Settings.FFmpeg.Update(package, enablePackage, forceUpdate);
                case "ffms": return MainForm.Instance.Settings.FFMS.Update(package, enablePackage, forceUpdate);
                case "flac": return MainForm.Instance.Settings.Flac.Update(package, enablePackage, forceUpdate);
                case "lame": return MainForm.Instance.Settings.Lame.Update(package, enablePackage, forceUpdate);
                case "mkvmerge": return MainForm.Instance.Settings.MkvMerge.Update(package, enablePackage, forceUpdate);
                case "mp4box": return MainForm.Instance.Settings.Mp4Box.Update(package, enablePackage, forceUpdate);
                case "neroaacenc": return MainForm.Instance.Settings.NeroAacEnc.Update(package, enablePackage, forceUpdate);
                case "oggenc2": return MainForm.Instance.Settings.OggEnc.Update(package, enablePackage, forceUpdate);
                case "opus": return MainForm.Instance.Settings.Opus.Update(package, enablePackage, forceUpdate);
                case "pgcdemux": return MainForm.Instance.Settings.PgcDemux.Update(package, enablePackage, forceUpdate);
                case "qaac": return MainForm.Instance.Settings.QAAC.Update(package, enablePackage, forceUpdate);
                case "tsmuxer": return MainForm.Instance.Settings.TSMuxer.Update(package, enablePackage, forceUpdate);
                case "vobsub": return MainForm.Instance.Settings.VobSub.Update(package, enablePackage, forceUpdate);
                case "x264": return MainForm.Instance.Settings.X264.Update(package, enablePackage, forceUpdate);
                case "x264_10b": return MainForm.Instance.Settings.X264_10B.Update(package, enablePackage, forceUpdate);
                case "x265": return MainForm.Instance.Settings.X265.Update(package, enablePackage, forceUpdate);
                case "xvid_encraw": return MainForm.Instance.Settings.XviD.Update(package, enablePackage, forceUpdate);
                case "yadif": return MainForm.Instance.Settings.Yadif.Update(package, enablePackage, forceUpdate);
                default: return false;
            }
        }
    }
}