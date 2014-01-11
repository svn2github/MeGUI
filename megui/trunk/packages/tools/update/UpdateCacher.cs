// ****************************************************************************
// 
// Copyright (C) 2005-2014 Doom9 & al
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

        public static ProgramSettings GetPackage(string package)
        {
            foreach (ProgramSettings pSettings in MainForm.Instance.ProgramSettings)
            {
                if (!package.Equals(pSettings.Name))
                    continue;
                return pSettings;
            }

            return null;
        }

        public static bool CheckPackage(string package, bool enablePackage, bool forceUpdate)
        {
            ProgramSettings pSettings = GetPackage(package);
            if (pSettings != null)
                return pSettings.Update(enablePackage, forceUpdate);
            return false;
        }
    }
}