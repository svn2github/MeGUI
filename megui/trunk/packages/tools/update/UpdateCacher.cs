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
using System.Collections;
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

        public static void flushOldCachedFilesAsync(UpdateWindow.iUpgradeableCollection upgradeData)
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
            FileInfo[] files = fi.GetFiles("*.zip");
            foreach (FileInfo f in files)
            {
                if (urls.IndexOf(f.Name.ToLowerInvariant()) >= 0)
                    continue;

                if (DateTime.Now - f.LastWriteTime > new TimeSpan(REMOVE_PACKAGE_AFTER_DAYS, 0, 0, 0, 0))
                {
                    f.Delete();
                    MainForm.Instance.UpdateHandler.AddTextToLog("Deleted cached file " + f.Name, ImageType.Information, false);
                }
            }
            files = fi.GetFiles("*.7z");
            foreach (FileInfo f in files)
            {
                if (urls.IndexOf(f.Name.ToLowerInvariant()) >= 0)
                    continue;

                if (DateTime.Now - f.LastWriteTime > new TimeSpan(REMOVE_PACKAGE_AFTER_DAYS, 0, 0, 0, 0))
                {
                    f.Delete();
                    MainForm.Instance.UpdateHandler.AddTextToLog("Deleted cached file " + f.Name, ImageType.Information, false);
                }
            }
        }

        private static UpdateWindow.ErrorState ManageRestartedCopying(string sourcePath, string targetPath, UpdateWindow.iUpgradeable file)
        {
            DirectoryInfo fi = new DirectoryInfo(sourcePath);
            FileInfo[] files = fi.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo f in files)
            {
                string directory = Path.Combine(targetPath, f.DirectoryName.Substring(MainForm.Instance.Settings.MeGUIUpdateCache.Length + file.Name.Length + 1));

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                f.MoveTo(Path.Combine(directory, f.Name + ".tempcopy"));
                MainForm.Instance.UpdateHandler.AddFileToReplace(file.Name, f.FullName.Remove(f.FullName.Length - 9), file.AvailableVersion.UploadDate.ToString(new System.Globalization.CultureInfo("en-us")));
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

        public static UpdateWindow.ErrorState ManageBackups(string savePath, string name, bool bCopyFile)
        {
            try
            {
                if (File.Exists(savePath + ".backup"))
                    File.Delete(savePath + ".backup");
            }
            catch
            {
                MainForm.Instance.UpdateHandler.AddTextToLog("Outdated backup version of " + name + " could not be deleted. Check if it is in use.", ImageType.Error, true);
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
                MainForm.Instance.UpdateHandler.AddTextToLog("Old version of " + name + " could not be backed up correctly.", ImageType.Error, true);
                return UpdateWindow.ErrorState.CouldNotCreateBackup;
            }

            return UpdateWindow.ErrorState.Successful;
        }

        public static void DeleteCacheFile(string p)
        {
            string localFilename = Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, p);
            try
            {
                if (File.Exists(localFilename))
                    File.Delete(localFilename);
            }
            catch (IOException)
            {
                MainForm.Instance.UpdateHandler.AddTextToLog("Could not delete file " + localFilename, ImageType.Error, true);
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

        public static bool IsComponentMissing()
        {
            ArrayList arrPath = new ArrayList();
            string strPath;

            //base 
            arrPath.Add(System.Windows.Forms.Application.ExecutablePath);
            //libs":
            strPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            arrPath.Add((Path.Combine(strPath, @"ICSharpCode.SharpZipLib.dll")));
            arrPath.Add((Path.Combine(strPath, @"MessageBoxExLib.dll")));
            arrPath.Add((Path.Combine(strPath, @"LinqBridge.dll")));
            //mediainfo
            arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"MediaInfo.dll"));
            //mediainfowrapper
            arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"MediaInfoWrapper.dll"));
            //sevenzip
            arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"7z.dll"));
            //sevenzipsharp
            arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"SevenZipSharp.dll"));
            //data
            arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"Data\ContextHelp.xml"));
            //avswrapper
            arrPath.Add((Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"AvisynthWrapper.dll")));
            //updatecopier
            arrPath.Add((Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"updatecopier.exe")));

            foreach (ProgramSettings pSettings in MainForm.Instance.ProgramSettings)
            {
                if (!pSettings.UpdateAllowed())
                    continue;
                arrPath.AddRange(pSettings.Files);
            }

            bool bComponentMissing = false;
            foreach (string strAppPath in arrPath)
            {
                ImageType image = ImageType.Error;
                if (MainForm.Instance.UpdateHandler.UpdateMode == UpdateMode.Automatic)
                    image = ImageType.Information;

                if (String.IsNullOrEmpty(strAppPath))
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("No path to check for missing components!", image, false);
                    bComponentMissing = true;
                    continue;
                }
                else if (File.Exists(strAppPath) == false)
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Component not found: " + strAppPath, image, false);
                    bComponentMissing = true;
                    continue;
                }
                FileInfo fInfo = new FileInfo(strAppPath);
                if (fInfo.Length == 0)
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Component has 0 bytes: " + strAppPath, image, false);
                    bComponentMissing = true;
                }
            }
            return bComponentMissing;
        }

        /// <summary>
        /// Checks the local update cache file
        /// </summary>
        /// <param name="file">full file name</param>
        /// <param name="err">reference to the error result</param>
        /// <returns>true if file is ok, false if not</returns>
        public static bool VerifyLocalCacheFile(string file, ref UpdateWindow.ErrorState err)
        {
            if (err == UpdateWindow.ErrorState.Successful)
                err = UpdateWindow.ErrorState.CouldNotDownloadFile;

            if (!File.Exists(file))
                return false;

            FileInfo finfo = new FileInfo(file);
            if (finfo.Length == 0)
            {
                DeleteCacheFile(file);
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
                            MainForm.Instance.UpdateHandler.AddTextToLog("Could not extract " + file + ". Deleting file.", ImageType.Information, true);
                            DeleteCacheFile(file);
                            return false;
                        }
                    }
                }
                catch
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Could not extract " + file + ". Deleting file.", ImageType.Error, true);
                    DeleteCacheFile(file);
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
                            MainForm.Instance.UpdateHandler.AddTextToLog("Could not extract " + file + ". Deleting file.", ImageType.Information, true);
                            DeleteCacheFile(file);
                            return false;
                        }
                    }
                }
                catch
                {
                    MainForm.Instance.UpdateHandler.AddTextToLog("Could not extract " + file + ". Deleting file.", ImageType.Error, true);
                    DeleteCacheFile(file);
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
                    //MainForm.Instance.UpdateHandler.AddTextToLog("Invalid or missing update file.", ImageType.Error);
                    DeleteCacheFile(file);
                    return false;
                }
            }

            err = UpdateWindow.ErrorState.Successful;
            return true;
        }
    }
}