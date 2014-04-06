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
using System.Xml;
using System.Xml.Serialization;

using ICSharpCode.SharpZipLib.Zip;
using SevenZip;

using MeGUI.core.util;

namespace MeGUI
{
    public class UpdateHandler
    {
        private string _updateServerURL;
        private LogItem _updateLog;
        private StringBuilder _updateLogText = new StringBuilder();
        private XmlDocument _updateXML;
        private UpdateWindow _updateWindow;
        private int _updateCheckEveryXHours;
        private Dictionary<string, CommandlineUpgradeData> filesToReplace = new Dictionary<string, CommandlineUpgradeData>();
        private UpdateWindow.iUpgradeableCollection _updateData = null;
        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        private bool _abortUpdate, _updateRunning;
        private UpdateMode _updateMode;
        private string _forcePackage;
        private WebClient wc;

        public UpdateMode UpdateMode
        {
            get { return _updateMode; }
            set { _updateMode = value; }
        }

        public string ForcePackageInstallation
        {
            get { return _forcePackage; }
            set { _forcePackage = value; }
        }

        public UpdateWindow.iUpgradeableCollection UpdateData
        {
            get { return _updateData; }
            set { _updateData = value; }
        }

        public StringBuilder UpdateLOG
        {
            get { return _updateLogText; }
        }

        public bool AbortUpdate
        {
            get { return _abortUpdate; }
            set 
            {
                _abortUpdate = value;
                if (value == true && wc != null && wc.IsBusy)
                    wc.CancelAsync();
            }
        }

        public Dictionary<string, CommandlineUpgradeData> FilesToReplace
        {
            get { return filesToReplace; }
        }

        public UpdateHandler()
        {
            _updateXML = null;
            if (_updateLog == null)
                _updateLog = MainForm.Instance.Log.Info("Update detection");
            _updateServerURL = MainForm.Instance.Settings.LastUpdateServer;
            _updateMode = MainForm.Instance.Settings.UpdateMode;

            // load saved _updateData
            LoadSettings();

            // start initial update check
            GetUpdateInformation(false);

            if (_updateMode == UpdateMode.Disabled)
                _updateLog.LogEvent("Automatic update is disabled");
        }

        public void GetUpdateInformation(bool bWait)
        {
            while (_updateRunning)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            _updateRunning = true;

            // refresh update data
            Thread updateCheck = new Thread(new ThreadStart(RefreshUpdateData));
            updateCheck.IsBackground = true;
            updateCheck.Start();
            while (bWait && updateCheck.IsAlive)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
        }

        public void RefreshUpdateData()
        {
            lock (this)
            {
                GetUpdateData();        // check if new update information is available
                ParseUpdateXML();       // finally update the data
                WriteUpdateStatus();    // write the missing update status into the log
                _updateRunning = false;
            }
        }

        private void WriteUpdateStatus()
        {
            int iUpdatesCount = NumUpdatableFiles();
            string text = string.Empty;


            if (iUpdatesCount > 0)
            {
                foreach (UpdateWindow.iUpgradeable upgradeable in _updateData)
                {
                    if (upgradeable.DownloadChecked)
                        text += upgradeable.DisplayName + ", ";
                }
            }

            if (iUpdatesCount > 1)
                AddTextToLog(string.Format("There are {0} packages which can be updated: " + text.Substring(0, text.Length - 2), iUpdatesCount), ImageType.Information, false);
            else if (iUpdatesCount == 1)
                AddTextToLog("There is 1 package which can be updated: " + text.Substring(0, text.Length - 2), ImageType.Information, false);
            else
                AddTextToLog("No package requires an update", ImageType.Information, false);

        }

        public UpdateWindow.ErrorState DownloadFile(string url, string fileName, string packageName)
        {
            string updateCache = MainForm.Instance.Settings.MeGUIUpdateCache;
            string localFilename = Path.Combine(updateCache, url);
            
            UpdateWindow.ErrorState result = UpdateWindow.ErrorState.Successful;
            FileUtil.ensureDirectoryExists(updateCache);
            
            if (!String.IsNullOrEmpty(fileName))
                localFilename = Path.Combine(updateCache, fileName);

            if (UpdateCacher.VerifyLocalCacheFile(localFilename, ref result))
                return result;

            lock (this)
            {
                ManualResetEvent mre = new ManualResetEvent(false);
                result = UpdateWindow.ErrorState.Successful;
                //_updateDownloadPackageInProgress = packageName;
                wc = new WebClient();
                // check for proxy authentication...
                wc.Proxy = HttpProxy.GetProxy(MainForm.Instance.Settings);
                wc.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
                {
                    if (e.Error != null)
                    {
                        if (e.Error is WebException && !_abortUpdate)
                        {
                            WebException webex = (WebException)e.Error;
                            if (webex.Response != null && ((HttpWebResponse)webex.Response).StatusCode == HttpStatusCode.NotFound)
                                result = UpdateWindow.ErrorState.FileNotOnServer;
                            else
                                result = UpdateWindow.ErrorState.ServerNotAvailable;
                        }
                        else
                            result = UpdateWindow.ErrorState.CouldNotDownloadFile;
                    }
                    sw.Reset();
                    mre.Set();
                };
                wc.DownloadProgressChanged += delegate(object sender, DownloadProgressChangedEventArgs e)
                {
                    if (_updateWindow != null)
                    {
                        string text = string.Format("Downloading {0} - {1} kb / {2} kb", packageName, (e.BytesReceived / 1024d).ToString("##,#"), (e.TotalBytesToReceive / 1024d).ToString("##,#"));
                        text += string.Format(" ({0} kb/s)", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("##,#"));
                        _updateWindow.SetProgressBar(0, (int)e.TotalBytesToReceive, (int)e.BytesReceived, text);
                    }
                };
                if (_updateWindow != null)
                    _updateWindow.SetProgressBar(0, 1, 0, "Downloading " + packageName);
                sw.Start();
                wc.DownloadFileAsync(new Uri(new Uri(_updateServerURL), url), localFilename);
                mre.WaitOne();
                if (_updateWindow != null)
                {
                    System.Threading.Thread.Sleep(1000);
                    _updateWindow.SetProgressBar(0, 10, 0, string.Empty);
                }
            }

            if (result == UpdateWindow.ErrorState.Successful)
                UpdateCacher.VerifyLocalCacheFile(localFilename, ref result);
            return result;
        }

        public UpdateWindow.ErrorState ExtractFile(MeGUI.UpdateWindow.iUpgradeable file)
        {
            string filepath = null, filename = null;
            UpdateWindow.ErrorState extractResult = UpdateWindow.ErrorState.Successful;

            if (file.SaveFolder != null)
                filepath = file.SaveFolder;
            else if (file.SavePath != null)
                filepath = Path.GetDirectoryName(file.SavePath);
            else
            {
                AddTextToLog("The path to save " + file.Name + " to is invalid.", ImageType.Error, true);
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
                AddTextToLog(string.Format("Could not create directory {0}.", filepath), ImageType.Error, true);
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
                            if (_updateWindow != null)
                                _updateWindow.SetProgressBar(0, 100, e.PercentDone, "Extracting " + file.DisplayName);
                        };
                        oArchive.FileExists += (o, e) =>
                        {
                            if (MainForm.Instance.Settings.AlwaysBackUpFiles)
                            {
                                extractResult = UpdateCacher.ManageBackups(e.FileName, file.Name, file.NeedsRestartedCopying);
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

                    if (_updateWindow != null)
                        _updateWindow.SetProgressBar(0, 11, 0, string.Empty);

                    if (extractResult != UpdateWindow.ErrorState.Successful)
                        return extractResult;

                    if (!file.NeedsRestartedCopying)
                        file.CurrentVersion = file.AvailableVersion;  // the current installed version is now the latest available version
                    else
                        file.CurrentVersion.FileVersion = file.AvailableVersion.FileVersion; // after the restart the new files will be active
                }
                catch
                {
                    AddTextToLog("Could not extract " + file.Name + ". Deleting file. Please run updater again.", ImageType.Error, true);
                    UpdateCacher.DeleteCacheFile(file.AvailableVersion.Url);
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
                                UpdateWindow.ErrorState result = UpdateCacher.ManageBackups(filename, file.Name, file.NeedsRestartedCopying);
                                if (result != UpdateWindow.ErrorState.Successful)
                                    return result;
                            }
                            if (file.NeedsRestartedCopying)
                            {
                                AddFileToReplace(file.Name, filename, file.AvailableVersion.UploadDate.ToString(new System.Globalization.CultureInfo("en-us")));
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
                    AddTextToLog("Could not extract " + file.Name + ". Deleting file. Please run updater again.", ImageType.Error, true);
                    UpdateCacher.DeleteCacheFile(file.AvailableVersion.Url);
                    return UpdateWindow.ErrorState.CouldNotExtract;
                }
            }
            else
            {
                AddTextToLog("Package " + file.Name + " could not be extracted.", ImageType.Error, true);
                return UpdateWindow.ErrorState.CouldNotExtract;
            }

            return extractResult;
        }

        public void AddTextToLog(string text, ImageType oLogType, bool bAddToWindow)
        {
            _updateLog.LogEvent(text, oLogType);

            if (!bAddToWindow)
                return;

            if (oLogType == ImageType.Warning || oLogType == ImageType.Error)
                _updateLogText.AppendLine(oLogType + ": " + text);
            else
                _updateLogText.AppendLine(text);

            if (_updateWindow != null)
                _updateWindow.RefreshText();
        }

        public void BeginUpdateCheck()
        {
            while (_updateRunning)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }

            bool bIsComponentMissing = UpdateCacher.IsComponentMissing();
            if (!bIsComponentMissing && NumUpdatableFiles() == 0)
                return;

            // there are updated or missing files, display the window
            if (bIsComponentMissing)
            {
                if (AskToInstallComponents(filesToReplace.Keys.Count > 0))
                {
                    if (filesToReplace.Keys.Count > 0) // restart required
                    {
                        MainForm.Instance.Restart = true;
                        MainForm.Instance.Invoke(new MethodInvoker(delegate { MainForm.Instance.Close(); }));
                        return;
                    }
                    else
                        ShowUpdateWindow(true, false);
                }
                else
                    return;
            }
            else if (MainForm.Instance.DialogManager.AskAboutUpdates())
                ShowUpdateWindow(true, false);

            if (UpdateCacher.IsComponentMissing() && !MainForm.Instance.Restart)
            {
                if (AskToInstallComponents(filesToReplace.Keys.Count > 0))
                {
                    if (filesToReplace.Keys.Count > 0) // restart required
                    {
                        MainForm.Instance.Restart = true;
                        MainForm.Instance.Invoke(new MethodInvoker(delegate { MainForm.Instance.Close(); }));
                        return;
                    }
                    else
                        BeginUpdateCheck();
                }
            }
        }

        public int NumUpdatableFiles()
        {
            int numUpdateableFiles = 0;
            foreach (UpdateWindow.iUpgradeable upgradeable in _updateData)
            {
                if (upgradeable.Name.Equals("neroaacenc"))
                {
                    if (upgradeable.CurrentVersion.FileVersion != null && upgradeable.CurrentVersion.FileVersion.Equals(upgradeable.AvailableVersion.FileVersion))
                        upgradeable.AvailableVersion.UploadDate = upgradeable.CurrentVersion.UploadDate;
                }
                if (upgradeable.DownloadChecked)
                    numUpdateableFiles++;
            }
            return numUpdateableFiles;
        }

        private bool AskToInstallComponents(bool bRestartRequired)
        {
            string strQuestionText;

            if (bRestartRequired)
                strQuestionText = "MeGUI cannot find at least one required component. Without these components, MeGUI will not run properly (e.g. no job can be started).\n\nDo you want to restart MeGUI now?";
            else
                strQuestionText = "MeGUI cannot find at least one required component. Without these components, MeGUI will not run properly (e.g. no job can be started).\n\nDo you want to search now online for updates?";

            if (MessageBox.Show(strQuestionText, "MeGUI component(s) missing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                return true;
            else
                return false;
        }

        public void AddFileToReplace(string iUpgradeableName, string filename, string newUploadDate)
        {
            CommandlineUpgradeData data = new CommandlineUpgradeData();
            data.filename.Add(filename);
            data.tempFilename.Add(filename + ".tempcopy");
            data.newUploadDate = newUploadDate;
            if (filesToReplace.ContainsKey(iUpgradeableName))
            {
                filesToReplace[iUpgradeableName].tempFilename.Add(filename + ".tempcopy");
                filesToReplace[iUpgradeableName].filename.Add(filename);
                return;
            }
            filesToReplace.Add(iUpgradeableName, data);
        }

        public void ProcessUpdate()
        {
            bool needsRestart = false;
            int currentFile = 1; //the first file we update is file 1.
            UpdateWindow.ErrorState result;
            List<UpdateWindow.iUpgradeable> succeededFiles = new List<UpdateWindow.iUpgradeable>();
            List<UpdateWindow.iUpgradeable> failedFiles = new List<UpdateWindow.iUpgradeable>();
            List<UpdateWindow.iUpgradeable> missingFiles = new List<UpdateWindow.iUpgradeable>();

            while (_updateRunning)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            _updateRunning = true;

            // Count the number of files we can update before we restart
            int updateableFileCount = 0;
            foreach (UpdateWindow.iUpgradeable file in UpdateData)
                if ((String.IsNullOrEmpty(_forcePackage) && file.DownloadChecked) || file.Name.Equals(_forcePackage))
                    updateableFileCount++;

            _abortUpdate = false;

            // Now update the files we can
            foreach (UpdateWindow.iUpgradeable file in UpdateData)
            {
                if ((!String.IsNullOrEmpty(_forcePackage) || !file.DownloadChecked) && !file.Name.Equals(_forcePackage))
                    continue;

                AddTextToLog(string.Format("{1}/{2} - updating package {0}", file.DisplayName, currentFile, updateableFileCount), ImageType.Information, true);

                if (!String.IsNullOrEmpty(file.AvailableVersion.Web))
                {
                    string strText;
                    if (file.Name.ToLowerInvariant().Equals("neroaacenc"))
                    {
                        string strPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\tools\eac3to\neroAacEnc.exe";
                        strText = "MeGUI cannot find " + file.DisplayName + " on your system or it is outdated.\nDue to the licensing the component is not included on the MeGUI update server.\n\nTherefore please download the file on your own and extract neroaacenc.exe to:\n" + strPath + "\n\nIf necessary change the path in the settings:\n\"Settings\\External Program Settings\"\n\nWould you like to download it now?";
                    }
                    else
                        strText = "MeGUI cannot find " + file.DisplayName + " on your system or it is outdated.\nDue to the licensing the component is not included on the MeGUI update server.\n\nTherefore please download the file on your own, extract it and set the path to the " + file.Name + ".exe in the MeGUI settings\n(\"Settings\\External Program Settings\").\n\nWould you like to download it now?";

                    if (MessageBox.Show(strText, "Component not found", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(file.AvailableVersion.Web);
                        succeededFiles.Add(file);
                    }
                    else
                        failedFiles.Add(file);
                }
                else
                {
                    result = DownloadFile(file.AvailableVersion.Url, null, file.DisplayName);
                    if (result != UpdateWindow.ErrorState.Successful || _abortUpdate)
                    {
                        failedFiles.Add(file);
                        AddTextToLog(string.Format("Failed to download package {0}: {1}.", file.DisplayName, EnumProxy.Create(result).ToString()), ImageType.Error, true);
                    }
                    else if ((_updateMode != UpdateMode.Automatic || !String.IsNullOrEmpty(_forcePackage)) && !_abortUpdate)
                    {
                        // install only if not in full automatic mode and if download successful
                        UpdateWindow.ErrorState state = Install(file);
                        if (state != UpdateWindow.ErrorState.Successful)
                        {
                            if (state != UpdateWindow.ErrorState.RequirementNotMet)
                            {
                                AddTextToLog(string.Format("Failed to install package {0}: {1}.", file.DisplayName, EnumProxy.Create(state).ToString()), ImageType.Error, true);
                                failedFiles.Add(file);
                            }
                            else
                                missingFiles.Add(file);
                        }
                        else
                        {
                            succeededFiles.Add(file);
                            file.DownloadChecked = false;
                            if (file.NeedsRestartedCopying)
                                needsRestart = true;

                            if (file.Name.Equals("ffmpeg") || file.Name.StartsWith("x26")
                                || file.Name.Equals("xvid_encraw"))
                            {
                                if (MainForm.Instance.Settings.PortableAviSynth)
                                    FileUtil.PortableAviSynthActions(false);
                                if (!MainForm.Instance.Settings.AviSynthPlus)
                                    FileUtil.LSMASHFileActions(false);
                            }
                        }
                    }
                }

                if (currentFile >= updateableFileCount || _abortUpdate)
                    break;

                currentFile++;
            }

            if (_abortUpdate)
                AddTextToLog("Update aborted", ImageType.Information, true);

            if (_updateMode == UpdateMode.Automatic)
            {
                if (_updateWindow != null)
                    _updateWindow.CloseWindow();
                _updateRunning = false;
                return;
            }

            if (_updateWindow != null)
                _updateWindow.SetProgressBar(0, 12, 0, string.Empty);

            if (succeededFiles.Count > 0)
                AddTextToLog("Packages which have been successfully updated: " + succeededFiles.Count, ImageType.Information, true);
            if (failedFiles.Count + missingFiles.Count > 0)
            {
                if (failedFiles.Count == 0)
                    AddTextToLog("Packages which have not been successfully updated: " + missingFiles.Count, ImageType.Information, true);
                else
                    AddTextToLog("Packages which have not been successfully updated: " + (failedFiles.Count + missingFiles.Count), ImageType.Error, true);
            }

            UpdateCacher.flushOldCachedFilesAsync(UpdateData);

            if (_updateMode == UpdateMode.Automatic && (failedFiles.Count + missingFiles.Count) == 0)
            {
                if (_updateWindow != null)
                    _updateWindow.CloseWindow();
                _updateRunning = false;
                return;
            }

            if (needsRestart)
            {
                if (MessageBox.Show("In order to finish the update MeGUI needs to be restarted. Do you want to restart now?",
                    "Restart now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_updateWindow != null)
                        _updateWindow.CloseWindow();
                    _updateRunning = false;
                    MainForm.Instance.Restart = true;
                    MainForm.Instance.Invoke(new MethodInvoker(delegate { MainForm.Instance.Close(); }));
                    return;
                }
            }

            _updateRunning = false;
        }

        private UpdateWindow.ErrorState Install(UpdateWindow.iUpgradeable file)
        {
            if (file.RequiredBuild > 0 && new System.Version(Application.ProductVersion).Build < file.RequiredBuild)
            {
                AddTextToLog(string.Format("Could not install module '{0}' as at least MeGUI build {1} is required.", file.Name, file.RequiredBuild), ImageType.Information, true);
                return UpdateWindow.ErrorState.RequirementNotMet;
            }

            if (!String.IsNullOrEmpty(file.RequiredNET) && String.IsNullOrEmpty(OSInfo.GetDotNetVersion(file.RequiredNET)))
            {
                AddTextToLog(string.Format("Could not install module '{0}' as .NET {1} is required.", file.Name, file.RequiredNET), ImageType.Warning, true);
                return UpdateWindow.ErrorState.RequirementNotMet;
            }

            UpdateWindow.ErrorState state = file.Install(file);
            if (state == UpdateWindow.ErrorState.Successful)
            {
                file.CurrentVersion = file.AvailableVersion;
                return UpdateWindow.ErrorState.Successful;
            }

            AddTextToLog(string.Format("Could not install module '{0}'.", file.Name), ImageType.Error, true);
            return state;
        }

        public void ShowUpdateWindow(bool bWait, bool bAutoUpdate)
        {
            if (_updateWindow != null)
                _updateWindow.CloseWindow();
            if (_updateMode == UpdateMode.Disabled)
                _updateMode = UpdateMode.Manual;

            _updateWindow = new UpdateWindow();
            
            // reset update log
            _updateLogText = new StringBuilder();

            // show the update form
            _updateWindow.Show();

            // refresh update information
            if (!_updateRunning)
            {
                GetUpdateInformation(true);
                _updateWindow.RefreshGUI();
                _updateWindow.EnableUpdateButton();
            }
            else
                AddTextToLog("Update already in progress - please wait", ImageType.Information, true);

            if (bAutoUpdate)
            {
                bool bUpdateRunning = _updateRunning;
                while (_updateRunning)
                {
                    System.Threading.Thread.Sleep(100);
                    Application.DoEvents();
                }
                if (bUpdateRunning)
                {
                    GetUpdateInformation(true);
                    _updateWindow.RefreshGUI();
                    _updateWindow.EnableUpdateButton();
                }
                _updateMode = UpdateMode.Automatic;
                _updateWindow.StartUpdate();
                _updateWindow.CloseWindow();
            }
            else if (bWait)
            {
                while (_updateWindow.Visible)
                {
                    System.Threading.Thread.Sleep(100);
                    Application.DoEvents();
                }
            }

            if (bAutoUpdate)
                _updateMode = MainForm.Instance.Settings.UpdateMode;
        }

        #region refresh update data

        /// <summary>
        /// Retrieves if neccessary the update data from the update server
        /// and creates the update XML data
        /// </summary>
        private UpdateWindow.ErrorState GetUpdateData()
        {
            // get the value how often the update server can be checked
            // check more often when the development update server is selected
            _updateCheckEveryXHours = 120;
            if (MainForm.Instance.Settings.AutoUpdateServerSubList > 0)
                _updateCheckEveryXHours = 24;
#if DEBUG
            _updateCheckEveryXHours = 0;
#endif

            UpdateWindow.ErrorState result = UpdateWindow.ErrorState.Successful;
            if (MainForm.Instance.Settings.LastUpdateCheck.AddHours(_updateCheckEveryXHours).CompareTo(DateTime.Now.ToUniversalTime()) > 0)
            {
                // update server used within the last _updateCheckEveryXHours hours - therefore no new check if possible
                if (_updateXML != null)
                {
                    // update data already available
                    return UpdateWindow.ErrorState.Successful;
                }
                else
                {
                    // update data not available
                    if (UpdateCacher.VerifyLocalCacheFile(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml"), ref result))
                    {
                        // local xml file can be used
                        AddTextToLog("Using cached update config and server: " + _updateServerURL, ImageType.Information, false);
                        _updateXML = new XmlDocument();
                        _updateXML.Load(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml"));
                        return UpdateWindow.ErrorState.Successful;
                    }
                }
            }

            // new upgrade.xml must be downloaded
            // get the current list of update servers to try
            List<string> _updateServerList = GetUpdateServerList();
            if (_updateServerList.Count == 0)
            {
                _updateXML = null;
                AddTextToLog("No update servers are registered!", ImageType.Error, true);
                return UpdateWindow.ErrorState.ServerNotAvailable;
            }

            // get the upgrade.xml from the update server
            result = UpdateWindow.ErrorState.ServerNotAvailable;
            if (_updateMode != UpdateMode.Disabled)
            {
                foreach (string serverName in _updateServerList)
                {
                    _updateServerURL = serverName;
                    result = GetUpdateXML(_updateServerURL, false);
                    if (result == UpdateWindow.ErrorState.Successful)
                    {
                        MainForm.Instance.Settings.LastUpdateCheck = DateTime.Now.ToUniversalTime();
                        MainForm.Instance.Settings.LastUpdateServer = _updateServerURL;
                        AddTextToLog("Connected to server: " + _updateServerURL, ImageType.Information, false);
                        break;
                    }
                    else
                        AddTextToLog("Cannot use update server " + _updateServerURL + ". Reason: " + EnumProxy.Create(result).ToString(), ImageType.Information, true);
                }
            }

            if (result != UpdateWindow.ErrorState.Successful)
            {
                // update server not available
                if (UpdateCacher.VerifyLocalCacheFile(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml"), ref result))
                {
                    // local xml file can be used
                    AddTextToLog("Using cached update config and server: " + _updateServerURL, ImageType.Information, false);
                    _updateXML = new XmlDocument();
                    _updateXML.Load(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml"));
                    return UpdateWindow.ErrorState.Successful;
                }
                else
                    _updateXML = null;
            }
            else if (File.Exists(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade_new.xml")))
            {
                // delete old file if available
                File.Delete(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml"));
                File.Move(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade_new.xml"), Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml"));
                _updateXML = new XmlDocument();
                _updateXML.Load(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml"));
                return UpdateWindow.ErrorState.Successful;
            }
            else
                _updateXML = null;

            return result;
        }

        /// <summary>
        /// This method is called to retrieve the update data from the webserver
        /// </summary>
        public UpdateWindow.ErrorState GetUpdateXML(string strServerAddress, bool bCheckOnly)
        {
            if (String.IsNullOrEmpty(strServerAddress))
                return UpdateWindow.ErrorState.ServerNotAvailable;

            string xmlFile = "upgrade.xml";
#if x64
            xmlFile = "upgrade_x64.xml";
#endif
            string targetFile = "upgrade_new.xml";
            if (bCheckOnly)
                targetFile = "upgrade_temp.xml";

            UpdateCacher.DeleteCacheFile(targetFile);
            UpdateWindow.ErrorState result = DownloadFile(xmlFile, targetFile, "update definition");
            if (result == UpdateWindow.ErrorState.Successful && bCheckOnly)
                UpdateCacher.DeleteCacheFile(targetFile);

            return result;
        }

        /// <summary>
        /// Gets the (random) update server list. It will only be populated once at startup and if the auto update settings change
        /// </summary>
        public List<string> GetUpdateServerList()
        {
            if (MainForm.Instance.Settings.AutoUpdateServerLists[MainForm.Instance.Settings.AutoUpdateServerSubList].Length <= 1)
                return new List<string>();

            string lastUpdateServer = MainForm.Instance.Settings.LastUpdateServer;
            List<string> randomServerList = new List<string>();
            List<string> sortedServerList = new List<string>(MainForm.Instance.Settings.AutoUpdateServerLists[MainForm.Instance.Settings.AutoUpdateServerSubList]);
            sortedServerList.RemoveAt(0); // remove header

            if (MainForm.Instance.Settings.LastUpdateCheck.AddHours(_updateCheckEveryXHours).CompareTo(DateTime.Now.ToUniversalTime()) > 0)
            {
                // update server used within the last _updateCheckEveryXHours hours
                // therefore no new server will be selected - at first the current one must be tried
                if (sortedServerList.Contains(lastUpdateServer))
                {
                    sortedServerList.Remove(lastUpdateServer);
                    randomServerList.Add(lastUpdateServer);
                    lastUpdateServer = String.Empty;
                }
            }
            else
            {
                // remove the current update server from the list and add it lateron to the end of the list
                if (sortedServerList.Contains(lastUpdateServer))
                    sortedServerList.Remove(lastUpdateServer);
            }

            Random r = new Random();
            while (sortedServerList.Count > 0)
            {
                int i = r.Next(0, sortedServerList.Count);
                randomServerList.Add(sortedServerList[i]);
                sortedServerList.RemoveAt(i);
            }

            if (!String.IsNullOrEmpty(lastUpdateServer))
                randomServerList.Add(lastUpdateServer);

            return randomServerList;
        }

        private void ParseUpdateXML()
        {
            if (_updateXML == null)
                return;

            XmlNode node = _updateXML.SelectSingleNode("/UpdateableFiles");
            if (node != null) // xml file could be dodgy.
                ParseUpdateXml(node, null, node.Name);
        }

        /// <summary>
        /// Parses the upgrade XML file to populate the upgradeData array. 
        /// It's a recursive algorithm, so it needs to be passed the root node
        /// off the upgrade XML to start off, and it will then recurse
        /// through all the nodes in the file.
        /// </summary>
        /// <param name="currentNode">The node that the function should work on</param>
        private void ParseUpdateXml(XmlNode currentNode, XmlNode groupNode, string path)
        {
            foreach (XmlNode childnode in currentNode.ChildNodes)
            {
                if (childnode.Attributes["type"].Value.Equals("file"))
                {
                    ParseFileData(childnode, groupNode);
                    continue;
                }

                string newPath = path + "." + childnode.Name;
                if (childnode.Attributes["type"].Value.Equals("tree"))
                    ParseUpdateXml(childnode, childnode, newPath);
                else if (childnode.Attributes["type"].Value.Equals("subtree"))
                    ParseUpdateXml(childnode, groupNode, newPath);
            }
        }

        /// <summary>
        /// Once a "file" is found in the upgrade XML file, the files node is passed
        /// to this function which generates the correct iUpgradeable filetype (i.e. MeGUIFile
        /// or AviSynthFile) and then fills in all the relevant data.
        /// </summary>
        /// <param name="node"></param>
        private void ParseFileData(XmlNode node, XmlNode groupNode)
        {
            UpdateWindow.iUpgradeable file = null;
            UpdateWindow.Version availableFile = null;
            bool fileAlreadyAdded = false;

            if ((file = _updateData.FindByName(node.Name)) == null) // If this file isn't already in the upgradeData list
            {
                try
                {
                    if (!UpdateCacher.IsPackage(node.Name))
                        return;
                    if (groupNode.Name.Equals("MeGUI"))
                        file = new UpdateWindow.MeGUIFile(node.Name);
                    else if (groupNode.Name.Equals("ProgramFile"))
                        file = new UpdateWindow.ProgramFile(node.Name);
                    else if (groupNode.Name.Equals("ProfilesFile"))
                        file = new UpdateWindow.ProfilesFile(node.Name, MainForm.Instance);
                    else
                        return;
                }
                catch (FileNotRegisteredYetException)
                {
                    return;
                }
            }
            else
            {
                file.AvailableVersion = new UpdateWindow.Version();
                file.DownloadChecked = false;
                fileAlreadyAdded = true;
                if (file is UpdateWindow.ProfilesFile)
                    (file as UpdateWindow.ProfilesFile).MainForm = MainForm.Instance;
            }

            file.NeedsRestartedCopying = false;
            var nameAttribute = node.Attributes["needsrestart"];
            if (nameAttribute != null)
            {
                if (nameAttribute.Value.Equals("true"))
                    file.NeedsRestartedCopying = true;
            }

            file.RequiredBuild = 0;
            nameAttribute = node.Attributes["requiredbuild"];
            if (nameAttribute != null)
            {
                file.RequiredBuild = Int32.Parse(nameAttribute.Value);
            }

            file.DisplayName = node.Name;
            nameAttribute = node.Attributes["name"];
            ProgramSettings pSettings = UpdateCacher.GetPackage(file.Name);
            if (nameAttribute != null)
            {
                file.DisplayName = nameAttribute.Value;
                if (pSettings != null)
                    pSettings.DisplayName = file.DisplayName;
            }
            else
            {
                if (pSettings != null)
                    file.DisplayName = pSettings.DisplayName;
            }

            file.RequiredNET = String.Empty;
            nameAttribute = node.Attributes["net"];
            if (nameAttribute != null)
            {
                file.RequiredNET = nameAttribute.Value;
            }

            foreach (XmlNode filenode in node.ChildNodes) // each filenode contains the upgrade url and version
            {
                availableFile = new UpdateWindow.Version();
                availableFile.Url = filenode.FirstChild.Value;

                foreach (XmlAttribute oAttribute in filenode.Attributes)
                {
                    if (oAttribute.Name.Equals("version"))
                        availableFile.FileVersion = filenode.Attributes["version"].Value;
                    else if (oAttribute.Name.Equals("url"))
                        availableFile.Web = filenode.Attributes["url"].Value;
                    else if (oAttribute.Name.Equals("date"))
                    {
                        DateTime oDate = new DateTime();
                        DateTime.TryParse(filenode.Attributes["date"].Value, new System.Globalization.CultureInfo("en-us"), System.Globalization.DateTimeStyles.None, out oDate);
                        availableFile.UploadDate = oDate;
                    }
                }

                file.AvailableVersion = availableFile;
            }

            if ((!file.isAvailable() && (pSettings == null || pSettings.UpdateAllowed()))
                || (file.AllowUpdate && file.HasAvailableVersion && (pSettings == null || pSettings.UpdateAllowed())))
                file.DownloadChecked = true;

            if (!fileAlreadyAdded)
                _updateData.Add(file);
        }

        #endregion

        #region load and save
        private void LoadSettings()
        {
            string path = Path.Combine(Application.StartupPath, "AutoUpdate.xml");
            if (!File.Exists(path))
            {
                _updateData = new UpdateWindow.iUpgradeableCollection();
                return;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UpdateWindow.iUpgradeableCollection), new Type[] { typeof(UpdateWindow.ProgramFile), typeof(UpdateWindow.AviSynthFile), typeof(UpdateWindow.ProfilesFile), typeof(UpdateWindow.MeGUIFile) });
                StreamReader settingsReader = new StreamReader(path);
                UpdateWindow.iUpgradeableCollection upgradeDataTemp = (UpdateWindow.iUpgradeableCollection)serializer.Deserialize(settingsReader);
                settingsReader.Dispose();

                _updateData = new UpdateWindow.iUpgradeableCollection();
                foreach (UpdateWindow.iUpgradeable file in upgradeDataTemp)
                {
                    if (UpdateCacher.IsPackage(file.Name))
                        _updateData.Add(file);
                }

                foreach (UpdateWindow.iUpgradeable file in _updateData)
                {
                    try
                    {
                        file.init();
                    }
                    catch (FileNotRegisteredYetException) { }
                }
            }
            catch (Exception)
            {
                _updateData = new UpdateWindow.iUpgradeableCollection();
                AddTextToLog("Could not restore previous update package settings", ImageType.Error, true);
            }
        }

        public void SaveSettings()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UpdateWindow.iUpgradeableCollection), new Type[] { typeof(UpdateWindow.ProgramFile), typeof(UpdateWindow.ProfilesFile), typeof(UpdateWindow.MeGUIFile) });
                StreamWriter output = new StreamWriter(Path.Combine(Application.StartupPath, "AutoUpdate.xml"), false);
                serializer.Serialize(output, _updateData);
                output.Dispose();
            }
            catch (Exception)
            {
                AddTextToLog("Could not save update package settings", ImageType.Error, true);
            }
        }

        public void UpdateUploadDate(string name, string strDate)
        {
            UpdateWindow.iUpgradeable up = UpdateData.FindByName(name);
            if (up == null)
                return;

            DateTime oDate;
            bool bReady = DateTime.TryParse(strDate, new System.Globalization.CultureInfo("en-us"), System.Globalization.DateTimeStyles.None, out oDate);
            if (bReady)
                up.CurrentVersion.UploadDate = oDate;
        }

        #endregion
    }
}