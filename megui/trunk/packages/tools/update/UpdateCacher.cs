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

        public static void flushOldCachedFilesAsync(UpdateWindow.iUpgradeableCollection upgradeData, UpdateWindow oUpdate)
        {
            if (MainForm.Instance.Settings.AlwaysBackUpFiles)
                return;

            string updateCache = MainForm.Instance.Settings.MeGUIUpdateCache;
            if (String.IsNullOrEmpty(updateCache) || !Directory.Exists(updateCache))
                return;

            List<string> urls = new List<string>();
            foreach (UpdateWindow.iUpgradeable u in upgradeData)
            {
                if (!String.IsNullOrEmpty(u.AvailableVersion.Url))
                    urls.Add(u.AvailableVersion.Url.ToLowerInvariant());
                if (!urls.Contains(u.CurrentVersion.Url) && !String.IsNullOrEmpty(u.CurrentVersion.Url))
                    urls.Add(u.CurrentVersion.Url.ToLowerInvariant());
            }

            DirectoryInfo fi = new DirectoryInfo(updateCache);
            FileInfo[] files = fi.GetFiles();
            foreach (FileInfo f in files)
            {
                if (urls.IndexOf(f.Name.ToLowerInvariant()) >= 0)
                    continue;

                if (DateTime.Now - f.LastWriteTime > new TimeSpan(REMOVE_PACKAGE_AFTER_DAYS, 0, 0, 0, 0))
                {
                    f.Delete();
                    oUpdate.AddTextToLog("Deleted cached file " + f.Name, ImageType.Information);
                }
            }
        }

        public static UpdateWindow.ErrorState DownloadFile(string url, string fileName, Uri serverAddress,
            DownloadProgressChangedEventHandler wc_DownloadProgressChanged, UpdateWindow oUpdate)
        {
            FileUtil.ensureDirectoryExists(MainForm.Instance.Settings.MeGUIUpdateCache);
            UpdateWindow.ErrorState err = UpdateWindow.ErrorState.Successful;
            string updateCache = MainForm.Instance.Settings.MeGUIUpdateCache;
            string localFilename = Path.Combine(updateCache, url);
            if (!String.IsNullOrEmpty(fileName))
                localFilename = Path.Combine(updateCache, fileName);

            if (!VerifyLocalCacheFile(localFilename, oUpdate, ref err))
            {
                err = UpdateWindow.ErrorState.Successful;
                WebClient wc = new WebClient();

                // check for proxy authentication...
                wc.Proxy = HttpProxy.GetProxy(MainForm.Instance.Settings);

                ManualResetEvent mre = new ManualResetEvent(false);
                wc.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
                {
                    if (e.Error != null)
                    {
                        if (e.Error is WebException)
                        {
                            WebException webex = (WebException)e.Error;
                            if (webex.Response != null && ((HttpWebResponse)webex.Response).StatusCode == HttpStatusCode.NotFound)
                                err = UpdateWindow.ErrorState.FileNotOnServer;
                            else
                                err = UpdateWindow.ErrorState.ServerNotAvailable;
                        }
                        else
                            err = UpdateWindow.ErrorState.CouldNotDownloadFile;
                    }
                    mre.Set();
                };

                if (wc_DownloadProgressChanged != null)
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new Uri(serverAddress, url), localFilename);
                mre.WaitOne();

                VerifyLocalCacheFile(localFilename, oUpdate, ref err);

                if (localFilename.ToLowerInvariant().EndsWith(".xml"))
                {
                    if (err == UpdateWindow.ErrorState.Successful)
                        oUpdate.AddTextToLog("Connected to server: " + serverAddress, ImageType.Information);
                    else
                        oUpdate.AddTextToLog("Cannot connect to server: " + serverAddress + ". Reason: " + EnumProxy.Create(err).ToString(), ImageType.Information);
                }
                if (err == UpdateWindow.ErrorState.Successful)
                    oUpdate.AddTextToLog("File downloaded: " + Path.GetFileName(localFilename), ImageType.Information);
            }
            else if (localFilename.ToLowerInvariant().EndsWith(".xml"))
                oUpdate.AddTextToLog("Using cached update config and server: " + serverAddress, ImageType.Information);

            return err;
        }

        public static UpdateWindow.ErrorState ExtractFile(MeGUI.UpdateWindow.iUpgradeable file, UpdateWindow oUpdate)
        {
            string filepath = null, filename = null;
            UpdateWindow.ErrorState extractResult = UpdateWindow.ErrorState.Successful;

            if (file.SaveFolder != null)
                filepath = file.SaveFolder;
            else if (file.SavePath != null)
                filepath = Path.GetDirectoryName(file.SavePath);
            else
            {
                oUpdate.AddTextToLog("The path to save " + file.Name + " to is invalid.", ImageType.Error);
                return UpdateWindow.ErrorState.CouldNotSaveNewFile;
            }
            if (file.SavePath != null)
                filename = file.SavePath;
            else
                filename = filepath + @"\" + Path.GetFileName(file.AvailableVersion.Url);

            try
            {
                if (!Directory.Exists(filepath))
                    Directory.CreateDirectory(filepath);
            }
            catch (IOException)
            {
                oUpdate.AddTextToLog(string.Format("Could not create directory {0}.", filepath), ImageType.Error);
                return UpdateWindow.ErrorState.CouldNotSaveNewFile;
            }

            if (file.AvailableVersion.Url.EndsWith(".7z"))
            {
                try
                {
                    using (var oArchive = new SevenZipExtractor(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, Path.GetFileName(file.AvailableVersion.Url))))
                    {
                        oArchive.Extracting += (s, e) =>
                        {
                            oUpdate.SetProgressBar(0, 100, e.PercentDone);
                        };
                        oArchive.FileExists += (o, e) =>
                        {
                            if (MainForm.Instance.Settings.AlwaysBackUpFiles)
                            {
                                extractResult = ManageBackups(e.FileName, file.Name, file.NeedsRestartedCopying, oUpdate);
                                if (extractResult != UpdateWindow.ErrorState.Successful)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        };

                        // To-Do: add restarted copying to 7z handler
                        //if (!file.NeedsRestartedCopying)
                        //{
                        //    string targetPath = filepath;
                        //    string sourcePath = Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, file.Name);

                        //    oArchive.ExtractArchive(sourcePath);
                        //    extractResult = ManageRestartedCopying(sourcePath, targetPath, file, oUpdate);

                        //    //oArchive.FileExtractionStarted += (o, e) =>
                        //    //{
                        //    //    MainForm.Instance.AddFileToReplace(file.Name, e.FileInfo.FileName, file.AvailableVersion.UploadDate.ToString(new System.Globalization.CultureInfo("en-us")));
                        //    //    e.FileInfo.FileName += ".tempcopy";
                        //    //};
                        //}
                        //else
                            oArchive.ExtractArchive(filepath);
                    }

                    if (extractResult != UpdateWindow.ErrorState.Successful)
                        return extractResult;

                    if (!file.NeedsRestartedCopying)
                        file.CurrentVersion = file.AvailableVersion;  // the current installed version is now the latest available version
                    else
                        file.CurrentVersion.FileVersion = file.AvailableVersion.FileVersion; // after the restart the new files will be active
                }
                catch
                {
                    oUpdate.AddTextToLog("Could not extract " + file.Name + ". Deleting file. Please run updater again.", ImageType.Error);
                    DeleteCacheFile(file.AvailableVersion.Url, oUpdate);
                    return UpdateWindow.ErrorState.CouldNotExtract;
                }
            }
            else if (file.AvailableVersion.Url.EndsWith(".zip"))
            {
                try
                {
                    Stream str = File.OpenRead(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, Path.GetFileName(file.AvailableVersion.Url)));
                    using (ZipInputStream zip = new ZipInputStream(str))
                    {
                        ZipEntry zipentry;
                        while ((zipentry = zip.GetNextEntry()) != null)
                        {
                            filename = Path.Combine(filepath, zipentry.Name);
                            if (zipentry.IsDirectory)
                            {
                                if (!Directory.Exists(filename))
                                    Directory.CreateDirectory(filename);
                                continue;
                            }
                            if (MainForm.Instance.Settings.AlwaysBackUpFiles)
                            {
                                UpdateWindow.ErrorState result = ManageBackups(filename, file.Name, file.NeedsRestartedCopying, oUpdate);
                                if (result != UpdateWindow.ErrorState.Successful)
                                    return result;
                            }
                            if (file.NeedsRestartedCopying)
                            {
                                MainForm.Instance.AddFileToReplace(file.Name, filename, file.AvailableVersion.UploadDate.ToString(new System.Globalization.CultureInfo("en-us")));
                                filename += ".tempcopy";
                            }

                            // create the output writer to save the file onto the harddisc
                            using (Stream outputWriter = new FileStream(filename, FileMode.Create))
                                FileUtil.copyData(zip, outputWriter);
                            File.SetLastWriteTime(filename, zipentry.DateTime);
                        }
                        if (!file.NeedsRestartedCopying)
                            file.CurrentVersion = file.AvailableVersion; // the current installed version is now the latest available version
                        else
                            file.CurrentVersion.FileVersion = file.AvailableVersion.FileVersion; // after the restart the new files will be active
                    }
                }
                catch
                {
                    oUpdate.AddTextToLog("Could not extract " + file.Name + ". Deleting file. Please run updater again.", ImageType.Error);
                    DeleteCacheFile(file.AvailableVersion.Url, oUpdate);
                    return UpdateWindow.ErrorState.CouldNotExtract;
                }
            }
            else
            {
                oUpdate.AddTextToLog("Package " + file.Name + " could not be extracted.", ImageType.Error);
                return UpdateWindow.ErrorState.CouldNotExtract;
            }

            return extractResult;
        }

        private static UpdateWindow.ErrorState ManageRestartedCopying(string sourcePath, string targetPath, UpdateWindow.iUpgradeable file, UpdateWindow oUpdate)
        {
            DirectoryInfo fi = new DirectoryInfo(sourcePath);
            FileInfo[] files = fi.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo f in files)
            {
                string directory = Path.Combine(targetPath, f.DirectoryName.Substring(MainForm.Instance.Settings.MeGUIUpdateCache.Length + file.Name.Length + 1));

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                f.MoveTo(Path.Combine(directory, f.Name + ".tempcopy"));
                MainForm.Instance.AddFileToReplace(file.Name, f.FullName.Remove(f.FullName.Length - 9), file.AvailableVersion.UploadDate.ToString(new System.Globalization.CultureInfo("en-us")));
            }

            
            //try
            //{
            //    if (File.Exists(savePath))
            //    {
            //        if (bCopyFile == false)
            //            File.Move(savePath, (savePath + ".backup"));
            //        else
            //            File.Copy(savePath, (savePath + ".backup"));
            //    }
            //}
            //catch
            //{
            //    oUpdate.AddTextToLog("Old version of " + package + " could not be backed up correctly.", ImageType.Error);
            //    return UpdateWindow.ErrorState.CouldNotCreateBackup;
            //}

            return UpdateWindow.ErrorState.Successful;
        }

        private static UpdateWindow.ErrorState ManageBackups(string savePath, string name, bool bCopyFile, UpdateWindow oUpdate)
        {
            try
            {
                if (File.Exists(savePath + ".backup"))
                    File.Delete(savePath + ".backup");
            }
            catch
            {
                oUpdate.AddTextToLog("Outdated backup version of " + name + " could not be deleted. Check if it is in use.", ImageType.Error);
                return UpdateWindow.ErrorState.CouldNotRemoveBackup;
            }

            try
            {
                if (File.Exists(savePath))
                {
                    if (bCopyFile == false)
                        File.Move(savePath, (savePath + ".backup"));
                    else
                        File.Copy(savePath, (savePath + ".backup"));
                }
            }
            catch
            {
                oUpdate.AddTextToLog("Old version of " + name + " could not be backed up correctly.", ImageType.Error);
                return UpdateWindow.ErrorState.CouldNotCreateBackup;
            }

            return UpdateWindow.ErrorState.Successful;
        }

        public static void DeleteCacheFile(string p, UpdateWindow oUpdate)
        {
            string localFilename = Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, p);
            try
            {
                if (File.Exists(localFilename))
                    File.Delete(localFilename);
            }
            catch (IOException)
            {
                oUpdate.AddTextToLog("Could not delete file " + localFilename, ImageType.Error);
            }
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

        public static bool IsPackage(string package)
        {
            if (UpdateCacher.GetPackage(package) != null)
                return true;

            switch (package)
            {
                case "audio":
                case "TXviD":
                case "core":
                case "libs":
                case "mediainfo":
                case "mediainfowrapper":
                case "sevenzip":
                case "sevenzipsharp":
                case "data":
                case "avswrapper":
                case "updatecopier": return true;
                default: return false;
            }
        }

        public static bool CheckPackage(string package)
        {
            return CheckPackage(package, true, true);
        }

        public static bool CheckPackage(string package, bool enablePackage, bool forceUpdate)
        {
            ProgramSettings pSettings = GetPackage(package);
            if (pSettings != null)
                return pSettings.Update(enablePackage, forceUpdate);
            return false;
        }

        /// <summary>
        /// Checks the local update cache file
        /// </summary>
        /// <param name="file">full file name</param>
        /// <param name="oUpdate">update window</param>
        /// <returns>true if file is ok, false if not</returns>
        private static bool VerifyLocalCacheFile(string file, UpdateWindow oUpdate, ref UpdateWindow.ErrorState err)
        {
            if (err == UpdateWindow.ErrorState.Successful)
                err = UpdateWindow.ErrorState.CouldNotDownloadFile;

            if (!File.Exists(file))
                return false;

            FileInfo finfo = new FileInfo(file);
            if (finfo.Length == 0)
            {
                DeleteCacheFile(file, oUpdate);
                return false;
            }

            if (file.ToLowerInvariant().EndsWith(".7z"))
            {
                // check the 7-zip file
                err = UpdateWindow.ErrorState.CouldNotExtract;
                try
                {
                    using (SevenZipExtractor oArchive = new SevenZipExtractor(file))
                    {
                        if (oArchive.Check() == false)
                        {
                            oUpdate.AddTextToLog("Could not extract " + file + ". Deleting file.", ImageType.Information);
                            DeleteCacheFile(file, oUpdate);
                            return false;
                        }
                    }
                }
                catch
                {
                    oUpdate.AddTextToLog("Could not extract " + file + ". Deleting file.", ImageType.Error);
                    DeleteCacheFile(file, oUpdate);
                    return false;
                }
            }
            else if (file.ToLowerInvariant().EndsWith(".zip"))
            {
                // check the zip file
                err = UpdateWindow.ErrorState.CouldNotExtract;
                try
                {
                    using (ZipFile zipFile = new ZipFile(file))
                    {
                        if (zipFile.TestArchive(true) == false)
                        {
                            zipFile.Close();
                            oUpdate.AddTextToLog("Could not unzip " + file + ". Deleting file.", ImageType.Information);
                            DeleteCacheFile(file, oUpdate);
                            return false;
                        }
                    }
                }
                catch
                {
                    oUpdate.AddTextToLog("Could not unzip " + file + ". Deleting file.", ImageType.Error);
                    DeleteCacheFile(file, oUpdate);
                    return false;
                }
            }
            else if (file.ToLowerInvariant().EndsWith(".xml"))
            {
                // check the xml file
                err = UpdateWindow.ErrorState.InvalidXML;
                System.Xml.XmlDocument upgradeXml = new System.Xml.XmlDocument();
                try
                {
                    upgradeXml.Load(file);
                }
                catch
                {
                    oUpdate.AddTextToLog("Invalid or missing update file.", ImageType.Error);
                    DeleteCacheFile(file, oUpdate);
                    return false;
                }
            }

            err = UpdateWindow.ErrorState.Successful;
            return true;
        }
    }
}