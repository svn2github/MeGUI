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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using MeGUI.core.util;

namespace MeGUI
{
    public partial class UpdateWindow : Form
    {
        #region Variables
        private List<string> serverList;
        private bool continueUpdate = true;
        private iUpgradeableCollection upgradeData = null;
        private Thread updateThread = null;
        private StringBuilder logBuilder = new StringBuilder();
        private System.Threading.ManualResetEvent webUpdate = new ManualResetEvent(false);
        private XmlDocument upgradeXml = null;
        public bool needsRestart = false;
        private LogItem oLog;
        private String ServerAddress;
        private UpdateStep _updateStep;
        private ListViewColumnSorter lvwColumnSorter;
        private enum PackageStatus
        {
            [EnumTitle("no update available")]
            NoUpdateAvailable,
            [EnumTitle("update available")]
            UpdateAvailable,
            [EnumTitle("update ignored")]
            UpdateIgnored,
            [EnumTitle("reinstalling")]
            Reinstall,
            [EnumTitle("package disabled")]
            Disabled
        };
        public enum ErrorState
        {
            [EnumTitle("File cannot be found on the server")]
            FileNotOnServer,
            [EnumTitle("Update server is not available")]
            ServerNotAvailable,
            [EnumTitle("File cannot be downloaded")]
            CouldNotDownloadFile,
            [EnumTitle("Backup file cannot be removed")]
            CouldNotRemoveBackup,
            [EnumTitle("File cannot be saved")]
            CouldNotSaveNewFile,
            [EnumTitle("Backup file cannot be created")]
            CouldNotCreateBackup,
            [EnumTitle("File cannot be installed")]
            CouldNotInstall,
            [EnumTitle("Update successful")]
            Successful,
            [EnumTitle("File cannot be extracted")]
            CouldNotExtract,
            [EnumTitle("Update XML is invalid")]
            InvalidXML,
            [EnumTitle("The requirements for this file are not met")]
            RequirementNotMet
        }
        public enum UpdateStep
        {
            Manual,
            AutomaticCheck,
            AutomaticInstall,
            AutomaticStartup,
            ServerCheckOnly
        }
        #endregion
        #region Classes

        public abstract class iUpgradeable
        {
            /// <summary>
            /// May be overridden in a special init is to be done after the xmlserializer
            /// </summary>
            public virtual void init() { }
            
            internal iUpgradeable()
            {
                availableVersion = new Version();

            } // constructor

            // Overrideable methods
            public bool HasAvailableVersion
            {
                get
                {
                    Version latest = this.availableVersion;
                    if (this.name == "neroaacenc")
                    {
                        if (currentVersion != null && currentVersion.FileVersion != null && currentVersion.FileVersion.Equals(latest.FileVersion))
                            latest.UploadDate = currentVersion.UploadDate;
                    }
                    return latest != null && (latest.CompareTo(currentVersion) != 0);
                }
            }

            public bool isAvailable()
            {
                ArrayList arrPath = new ArrayList();
                string strPath;

                switch (this.name)
                {
                    case "base": arrPath.Add(System.Windows.Forms.Application.ExecutablePath); break;
                    case "libs":
                        strPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                        arrPath.Add((Path.Combine(strPath, @"ICSharpCode.SharpZipLib.dll")));
                        arrPath.Add((Path.Combine(strPath, @"MessageBoxExLib.dll")));
                        arrPath.Add((Path.Combine(strPath, @"LinqBridge.dll")));
                        break;
                    case "mediainfo": arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"MediaInfo.dll")); break;
                    case "mediainfowrapper": arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"MediaInfoWrapper.dll")); break;
                    case "sevenzip": arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"7z.dll")); break;
                    case "sevenzipsharp": arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"SevenZipSharp.dll")); break;
                    case "data": arrPath.Add(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"Data\ContextHelp.xml")); break;
                    case "avswrapper": arrPath.Add((Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"AvisynthWrapper.dll"))); break;
                    case "updatecopier": arrPath.Add((Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"updatecopier.exe"))); break;
                    case "neroaacenc":
                        if (MainForm.Instance.Settings.UseNeroAacEnc)
                        {
                            arrPath.Add(MainForm.Instance.Settings.NeroAacEnc.Path);
                            if (File.Exists(MainForm.Instance.Settings.NeroAacEnc.Path))
                            {
                                System.Diagnostics.FileVersionInfo finfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(MainForm.Instance.Settings.NeroAacEnc.Path);
                                FileInfo fi = new FileInfo(MainForm.Instance.Settings.NeroAacEnc.Path);
                                this.currentVersion.FileVersion = finfo.FileMajorPart + "." + finfo.FileMinorPart + "." + finfo.FileBuildPart + "." + finfo.FilePrivatePart;
                                this.currentVersion.UploadDate = fi.LastWriteTimeUtc;
                            }
                        }
                        break;
                    default:
                        ProgramSettings pSettings = UpdateCacher.GetPackage(this.name);
                        if (pSettings != null && pSettings.UpdateAllowed())
                            arrPath.AddRange(pSettings.Files);
                        break;
                }

                foreach (string strAppPath in arrPath)
                {
                    if (String.IsNullOrEmpty(strAppPath))
                        return false;
                    if (File.Exists(strAppPath) == false)
                        return false;
                    FileInfo fInfo = new FileInfo(strAppPath);
                    if (fInfo.Length == 0)
                        return false;
                }
                return true;
            }

            public ListViewItem CreateListViewItem()
            {
                ListViewItem myitem = new ListViewItem();
                ListViewItem.ListViewSubItem name = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem existingVersion = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem latestVersion = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem existingDate = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem latestDate = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem lastUsed = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem status = new ListViewItem.ListViewSubItem();

                myitem.Name = this.Name;

                name.Name = "Name";
                existingVersion.Name = "Existing Version";
                latestVersion.Name = "Latest Version";
                existingDate.Name = "Existing Date";
                latestDate.Name = "Latest Date";
                lastUsed.Name = "Last Used";
                status.Name = "Status";

                name.Text = this.DisplayName;

                Version v = this.availableVersion;
                if (v != null)
                {
                    latestVersion.Text = v.FileVersion;
                    latestDate.Text = v.UploadDate.ToShortDateString();
                }
                else
                {
                    latestVersion.Text = "unknown";
                    latestDate.Text = "unknown";
                }

                if (this.CurrentVersion != null && !String.IsNullOrEmpty(this.CurrentVersion.FileVersion))
                {
                    existingVersion.Text = this.CurrentVersion.FileVersion;
                    if (this.CurrentVersion.UploadDate.Year > 1)
                        existingDate.Text = this.CurrentVersion.UploadDate.ToShortDateString();
                    else
                        existingDate.Text = "N/A";
                }
                else
                {
                    existingVersion.Text = "N/A";
                    existingDate.Text = "N/A";
                }

                ProgramSettings pSettings = UpdateCacher.GetPackage(this.name);
                if (pSettings != null && !pSettings.UpdateAllowed())
                {
                    status.Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                }
                else if (!this.isAvailable())
                {
                    status.Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    this.DownloadChecked = this.AllowUpdate = true;
                }
                else if (!HasAvailableVersion)
                {
                    if (this.DownloadChecked)
                        status.Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    else
                        status.Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
                }
                else
                {
                    if (this.AllowUpdate)
                        status.Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                    else
                        status.Text = EnumProxy.Create(PackageStatus.UpdateIgnored).ToString();
                }

                if (this.AllowUpdate)
                {
                    if (this.DownloadChecked)
                        myitem.Checked = true;
                    else
                        myitem.Checked = false;
                }

                if (pSettings != null)
                {
                    if (pSettings.LastUsed.Year > 1)
                        lastUsed.Text = pSettings.LastUsed.ToShortDateString();
                    else
                        lastUsed.Text = "N/A";
                }
                else
                    lastUsed.Text = "---";

                myitem.SubItems.Add(name);
                myitem.SubItems.Add(existingVersion);
                myitem.SubItems.Add(latestVersion);
                myitem.SubItems.Add(existingDate);
                myitem.SubItems.Add(latestDate);
                myitem.SubItems.Add(lastUsed);
                myitem.SubItems.Add(status);
                return myitem;
            }
            
            private bool downloadChecked;
            public bool DownloadChecked
            {
                get { return downloadChecked; }
                set { downloadChecked = value; }
            }

            private string savePath;
            [XmlIgnore]
            public string SavePath
            {
                get { return savePath; }
                set { savePath = value; }
            }

            private string saveFolder;
            [XmlIgnore]
            public string SaveFolder
            {
                get { return saveFolder; }
                set { saveFolder = value; }
            }
	
            protected Version currentVersion;
            public virtual Version CurrentVersion
            {
                get
                {
                    if (currentVersion == null)
                        currentVersion = new Version();
                    return currentVersion;
                }
                set { currentVersion = value; }
            }

            private Version availableVersion;
            public Version AvailableVersion
            {
                get { return availableVersion; }
                set { availableVersion = value; }
            }

            private bool allowUpdate;
            public bool AllowUpdate
            {
                get { return allowUpdate; }
                set
                {
                    if (!value)
                        downloadChecked = false;
                    allowUpdate = value;
                }
            }

            private string name;
            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }

            private string displayName;
            public string DisplayName
            {
                get { return this.displayName; }
                set { this.displayName = value; }
            }

            private int requiredBuild;
            public int RequiredBuild
            {
                get { return requiredBuild; }
                set { requiredBuild = value; }
            }

            private string requiredNET;
            public string RequiredNET
            {
                get { return requiredNET; }
                set { requiredNET = value; }
            }

            private bool needsRestartedCopying = false;
            public bool NeedsRestartedCopying
            {
                get { return needsRestartedCopying; }
                set { needsRestartedCopying = value; }
            }

            public virtual ErrorState Install(iUpgradeable file, UpdateWindow oUpdate)
            {
                return UpdateCacher.ExtractFile(file, oUpdate);
            }
        }
        public class iUpgradeableCollection : CollectionBase
        {
            public iUpgradeableCollection() { }
            public iUpgradeableCollection(int capacity)
            {
                this.InnerList.Capacity = capacity;
            }

            public iUpgradeable this[int index]
            {
                get { return (iUpgradeable)this.List[index]; }
                set { this.List[index] = value; }
            }
            public void Add(iUpgradeable item)
            {
                if (FindByName(item.Name) != null)
                    throw new Exception("Can't have multiple upgradeable items with the same name");
                this.InnerList.Add(item);
            }
            public void Remove(iUpgradeable item)
            {
                this.InnerList.Remove(item);
            }
            public iUpgradeable FindByName(string name)
            {
                foreach (iUpgradeable file in this.InnerList)
                {
                    if (file.Name.Equals(name))
                        return file;
                }
                return null;
            }
            public int CountCheckedFiles()
            {
                int count=0;
                foreach (iUpgradeable file in this.InnerList)
                {
                    if (file.DownloadChecked)
                        count++;
                }
                return count;
            }
        }
        public class ProfilesFile : iUpgradeable
        {
            public ProfilesFile()
            {
            }

            public ProfilesFile(string name, MainForm mainForm)
            {
                this.Name = name;
                this.AllowUpdate = true;
                this.mainForm = mainForm;
            }
            
            private MainForm mainForm;

            public MainForm MainForm
            {
                set { mainForm = value; }
            }

            public override ErrorState Install(iUpgradeable file, UpdateWindow oUpdate)
            {
                try
                {
                    string filename = Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, Path.GetFileName(file.AvailableVersion.Url));
                    mainForm.importProfiles(filename, true);
                    if (mainForm.ImportProfileSuccessful == true)
                        return ErrorState.Successful;
                    else
                        return ErrorState.CouldNotInstall;
                }
                catch
                {
                    return ErrorState.CouldNotInstall;
                }
            }
        }

        public class AviSynthFile : iUpgradeable
        {
            public override void init()
            {
                base.init();
            }

            private AviSynthFile()
            {
            }

            public AviSynthFile(string name)
            {
                this.Name = name;
                this.AllowUpdate = true;
            }
        }

        public class MeGUIFile : iUpgradeable
        {
            private MeGUIFile()
            {
            }

            public MeGUIFile(string name)
            {
                this.Name = name;
                this.AllowUpdate = true;
                this.SaveFolder = Application.StartupPath;
            }

            public override Version CurrentVersion
            {
                get
                {
                    if (Name == "core")
                        base.CurrentVersion.FileVersion = new System.Version(Application.ProductVersion).Build.ToString();
                    return base.CurrentVersion;
                }
                set
                {
                    base.CurrentVersion = value;
                }
            }

            public override void init()
            {
                base.init();
                this.SaveFolder = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            }
        }

        public class ProgramFile : iUpgradeable
        {
            private ProgramFile()
            {
            }

            public override void init()
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(this.Name);
                if (pSettings == null || String.IsNullOrEmpty(pSettings.Path))
                    throw new FileNotRegisteredYetException(Name);
                SavePath = pSettings.Path;
            }
            
            public ProgramFile(string name) // Constructor
            {
                this.Name = name;
                this.AllowUpdate = true;
                init();
            }
        }

        public class Version : IComparable<Version>
        {
            public Version()
            {
            }
            public Version(string version, string url)
            {
                this.fileVersion = version;
                this.url = url;
            }
            private string fileVersion;
            private string url;
            private DateTime uploadDate;
            private string web;

            public string FileVersion
            {
                get { return fileVersion; }
                set { fileVersion = value; }
            }
            public string Url
            {
                get { return url; }
                set { url = value; }
            }
            public DateTime UploadDate
            {
                get { return uploadDate;  }
                set { uploadDate = value; }
            }
            public string Web
            {
                get { return web; }
                set { web = value; }
            }

            /// <summary>
            /// Helper method to check if a newer upload date is available
            /// </summary>
            /// <param name="version1">The first version to compare</param>
            /// <param name="version2">The second version to compare</param>
            /// <returns>1 if version1 has a newer upload date</returns>
            private int CompareUploadDate(Version version1, Version version2)
            {
                if (version1 == null && version2 == null)
                    return 0;
                else if (version1 == null)
                    return -1;
                else if (version2 == null)
                    return 1;
                else if (version1.uploadDate != new DateTime() && version2.uploadDate != new DateTime())
                {
                    if (version1.uploadDate > version2.uploadDate)
                        return 1;
                    else if (version1.uploadDate < version2.uploadDate)
                        return -1;
                    else
                        return 0;
                }

                return 1;
            }

            #region IComparable<Version> Members

            public int CompareTo(Version other)
            {
                return CompareUploadDate(this, other);
            }

            #endregion
        }
        #endregion
        #region Delegates and delegate methods
        private delegate void BeginParseUpgradeXml(XmlNode node, XmlNode groupNode, string path);
        private delegate void SetLogText();
        private delegate void SetListView(ListViewItem item);
        private delegate void ClearItems(ListView listview);
        private delegate void UpdateProgressBar(int minValue, int maxValue, int currentValue);
        public void SetProgressBar(int minValue, int maxValue, int currentValue)
        {
            if (this.progressBar.InvokeRequired)
            {
                try
                {
                    UpdateProgressBar d = new UpdateProgressBar(SetProgressBar);
                    progressBar.Invoke(d, minValue, maxValue, currentValue);
                }
                catch (Exception) { }
            }
            else
            {
                this.progressBar.Minimum = (int)minValue;
                this.progressBar.Maximum = (int)maxValue;
                this.progressBar.Value = (int)currentValue;
            }
        }

        public void AddTextToLog(string text, ImageType oLogType)
        {
            oLog.LogEvent(text, oLogType);

            if (oLogType == ImageType.Warning || oLogType == ImageType.Error)
                logBuilder.AppendLine(oLogType + ": " + text);
            else
                logBuilder.AppendLine(text);

            SetLogText d = new SetLogText(UpdateLogText);
            if (this.txtBoxLog.InvokeRequired)
                this.Invoke(d);
            else
                d();
        }

        private void UpdateLogText()
        {
            this.txtBoxLog.Text = logBuilder.ToString();
            this.txtBoxLog.SelectionStart = txtBoxLog.Text.Length;
            this.txtBoxLog.ScrollToCaret();
        }

        private void AddToListview(ListViewItem item)
        {
            if (this.listViewDetails.InvokeRequired)
            {
                SetListView d = new SetListView(AddToListview);
                this.Invoke(d, item);
            }
            else
                this.listViewDetails.Items.Add(item);
        }
        private void ClearListview(ListView listview)
        {
            if (listview.InvokeRequired)
            {
                ClearItems d = new ClearItems(ClearListview);
                this.Invoke(d, listview);
            }
            else
            {
                listview.Items.Clear();
            }
        }
        #endregion

        #region con/de struction
        private void UpdateWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
            SaveSettings();
        }

        /// <summary>
        /// Constructor for Updatewindow.
        /// </summary>
        /// <param name="mainForm">MainForm instance</param>
        /// <param name="savedSettings">Current MeGUI settings</param>
        /// <param name="bSilent">whether the update window should be displayed</param>
        public UpdateWindow()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listViewDetails.ListViewItemSorter = lvwColumnSorter;
            if (MainForm.Instance.UpdateLog == null)
                MainForm.Instance.UpdateLog = MainForm.Instance.Log.Info("Update detection");
            this.oLog = MainForm.Instance.UpdateLog;
            _updateStep = UpdateStep.ServerCheckOnly;
            LoadComponentSettings();
            LoadSettings(); // load upgradeData
        }

        private void LoadComponentSettings()
        {
            // Restore Size/Position of the window
            this.ClientSize = MainForm.Instance.Settings.UpdateFormSize;
            this.Location = MainForm.Instance.Settings.UpdateFormLocation;
            this.splitContainer2.SplitterDistance = MainForm.Instance.Settings.UpdateFormSplitter;

            colUpdate.Width = MainForm.Instance.Settings.UpdateFormUpdateColumnWidth;
            colName.Width = MainForm.Instance.Settings.UpdateFormNameColumnWidth;
            colExistingVersion.Width = MainForm.Instance.Settings.UpdateFormLocalVersionColumnWidth;
            colLatestVersion.Width = MainForm.Instance.Settings.UpdateFormServerVersionColumnWidth;
            colExistingDate.Width = MainForm.Instance.Settings.UpdateFormLocalDateColumnWidth;
            colLatestDate.Width = MainForm.Instance.Settings.UpdateFormServerDateColumnWidth;
            colLastUsed.Width = MainForm.Instance.Settings.UpdateFormLastUsedColumnWidth;
            colStatus.Width = MainForm.Instance.Settings.UpdateFormStatusColumnWidth;
        }

        private void SaveComponentSettings()
        {
            MainForm.Instance.Settings.UpdateFormUpdateColumnWidth = colUpdate.Width;
            MainForm.Instance.Settings.UpdateFormNameColumnWidth = colName.Width;
            MainForm.Instance.Settings.UpdateFormLocalVersionColumnWidth = colExistingVersion.Width;
            MainForm.Instance.Settings.UpdateFormServerVersionColumnWidth = colLatestVersion.Width;
            MainForm.Instance.Settings.UpdateFormLocalDateColumnWidth = colExistingDate.Width;
            MainForm.Instance.Settings.UpdateFormServerDateColumnWidth = colLatestDate.Width;
            MainForm.Instance.Settings.UpdateFormLastUsedColumnWidth = colLastUsed.Width;
            MainForm.Instance.Settings.UpdateFormStatusColumnWidth = colStatus.Width;
        }

        public static bool isComponentMissing()
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
                if (MainForm.Instance.Settings.AutoUpdateSession)
                    image = ImageType.Information;

                if (String.IsNullOrEmpty(strAppPath))
                {
                    MainForm.Instance.UpdateLog.LogEvent("No path to check for missing components!", image);
                    bComponentMissing = true;
                    continue;
                }
                else if (File.Exists(strAppPath) == false)
                {
                    MainForm.Instance.UpdateLog.LogEvent("Component not found: " + strAppPath, image);
                    bComponentMissing = true;
                    continue;
                }
                FileInfo fInfo = new FileInfo(strAppPath);
                if (fInfo.Length == 0)
                {
                    MainForm.Instance.UpdateLog.LogEvent("Component has 0 bytes: " + strAppPath, image);
                    bComponentMissing = true;
                }
            }
            return bComponentMissing;
        }

        private List<string> getUpdateServerList(string[] serverList)
        {
            string lastUpdateServer = MainForm.Instance.Settings.LastUpdateServer;
            List<string> randomServerList = new List<string>();
            List<string> sortedServerList = new List<string>(serverList);
            sortedServerList.RemoveAt(0); // remove header

            if (MainForm.Instance.Settings.LastUpdateCheck.AddHours(4).CompareTo(DateTime.Now.ToUniversalTime()) > 0)
            {
                // update server used within the last 4 hours - therefore no new server will be selected
                if (sortedServerList.Contains(lastUpdateServer))
                {
                    sortedServerList.Remove(lastUpdateServer);
                    randomServerList.Add(lastUpdateServer);
                    lastUpdateServer = String.Empty;
                }
            }
            else
            {
                if (sortedServerList.Contains(lastUpdateServer))
                    sortedServerList.Remove(lastUpdateServer);
            }

            Random r = new Random();
            while (sortedServerList.Count >  0)
            {
                int i = r.Next(0, sortedServerList.Count);
                randomServerList.Add(sortedServerList[i]);
                sortedServerList.RemoveAt(i);
            }

            if (!String.IsNullOrEmpty(lastUpdateServer))
                randomServerList.Add(lastUpdateServer);

            return randomServerList;
        }

        private void UpdateWindow_Load(object sender, EventArgs e)
        {
            // Move window in the visible area of the screen if neccessary
            Size oSizeScreen = Screen.GetWorkingArea(this).Size;
            Point oLocation = Screen.GetWorkingArea(this).Location;
            int iScreenHeight = oSizeScreen.Height - 2 * SystemInformation.FixedFrameBorderSize.Height;
            int iScreenWidth = oSizeScreen.Width - 2 * SystemInformation.FixedFrameBorderSize.Width;

            if (this.Size.Height >= iScreenHeight)
                this.Location = new Point(this.Location.X, oLocation.Y);
            else if (this.Location.Y <= oLocation.Y)
                this.Location = new Point(this.Location.X, oLocation.Y);
            else if (this.Location.Y + this.Size.Height > iScreenHeight)
                this.Location = new Point(this.Location.X, iScreenHeight - this.Size.Height);

            if (this.Size.Width >= iScreenWidth)
                this.Location = new Point(oLocation.X, this.Location.Y);
            else if (this.Location.X <= oLocation.X)
                this.Location = new Point(oLocation.X, this.Location.Y);
            else if (this.Location.X + this.Size.Width > iScreenWidth)
                this.Location = new Point(iScreenWidth - this.Size.Width, this.Location.Y);

            if (VistaStuff.IsVistaOrNot)
                VistaStuff.SetWindowTheme(listViewDetails.Handle, "explorer", null);
        }
        #endregion
        #region load and save
        private void LoadSettings()
        {
            string path = Path.Combine(Application.StartupPath, "AutoUpdate.xml");
            if (!File.Exists(path))
            {
                upgradeData = new iUpgradeableCollection();
                return;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(iUpgradeableCollection), new Type[] { typeof(ProgramFile), typeof(AviSynthFile), typeof(ProfilesFile), typeof(MeGUIFile) });
                StreamReader settingsReader = new StreamReader(path);
                iUpgradeableCollection upgradeDataTemp = (iUpgradeableCollection)serializer.Deserialize(settingsReader);
                settingsReader.Dispose();

                upgradeData = new iUpgradeableCollection();
                foreach (iUpgradeable file in upgradeDataTemp)
                {
                    if (UpdateCacher.IsPackage(file.Name))
                        this.upgradeData.Add(file);
                }

                foreach (iUpgradeable file in upgradeData)
                {
                    try
                    {
                        file.init();
                    }
                    catch (FileNotRegisteredYetException) { }
                }
            }
            catch(Exception)
            {
                upgradeData = new iUpgradeableCollection();
                MessageBox.Show("Error: Could not load previous package settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SaveSettings()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(iUpgradeableCollection), new Type[] { typeof(ProgramFile), typeof(ProfilesFile), typeof(MeGUIFile) });
                StreamWriter output = new StreamWriter(Path.Combine(Application.StartupPath, "AutoUpdate.xml"), false);
                serializer.Serialize(output, this.upgradeData);
                output.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Error: Could not save settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region getting update data
        public void GetUpdateData(bool wait, UpdateStep updateStep)
        {
            if (MainForm.Instance.Settings.AutoUpdateServerLists[MainForm.Instance.Settings.AutoUpdateServerSubList].Length <= 1)
            {
                AddTextToLog("Couldn't run update since there are no servers registered.", ImageType.Error);
                return;
            }

            serverList = getUpdateServerList(MainForm.Instance.Settings.AutoUpdateServerLists[MainForm.Instance.Settings.AutoUpdateServerSubList]);
            _updateStep = updateStep;

            Thread CreateTreeview = new Thread(new ThreadStart(ProcessUpdateXML));
            CreateTreeview.IsBackground = true;
            CreateTreeview.Start();
            if (wait)
            {
                webUpdate.Reset();
                webUpdate.WaitOne();
            }
        }

        /// <summary>
        /// This method is called to retrieve the update data from the webserver
        /// and then set the relevant information to the grid.
        /// </summary>
        public ErrorState GetUpdateXML(string strServerAddress)
        {
            upgradeXml = new XmlDocument();

            if (!String.IsNullOrEmpty(strServerAddress))
            {
                string xmlFile = "upgrade.xml";
#if x64
                xmlFile = "upgrade_x64.xml";
#endif
                ErrorState result = UpdateCacher.DownloadFile(xmlFile, "upgrade.xml", new Uri(strServerAddress), null, this);
                if (result != ErrorState.Successful)
                {
                    AddTextToLog("Could not retrieve update data. " + EnumProxy.Create(result).ToString(), ImageType.Information);
                    upgradeXml = null;
                    return result;
                }
            }

            if (File.Exists(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml")))
            {
                upgradeXml.Load(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, "upgrade.xml"));
                return ErrorState.Successful;
            }
            else
                return ErrorState.ServerNotAvailable;
        }

        /// <summary>
        /// This function downloads the update XML file from the server and then processes it.
        /// </summary>
        private void ProcessUpdateXML()
        {
            // delete cached xml file if the automatic update process is not used
            if (_updateStep != UpdateStep.AutomaticInstall && _updateStep != UpdateStep.AutomaticStartup)
                UpdateCacher.DeleteCacheFile("upgrade.xml", this);

            //if (upgradeXml != null) // the update file has already been downloaded and processed
            //    return ErrorState.Successful;

            ErrorState value = ErrorState.ServerNotAvailable;
            foreach (string serverName in serverList)
            {
                ServerAddress = serverName;
                value = GetUpdateXML(serverName);
                if (value == ErrorState.Successful)
                {
                    MainForm.Instance.Settings.LastUpdateCheck = DateTime.Now.ToUniversalTime();
                    MainForm.Instance.Settings.LastUpdateServer = serverName; 
                    break;
                }
            }

            if (value != ErrorState.Successful)
            {
                value = GetUpdateXML(null);
                if (value != ErrorState.Successful)
                {
                    webUpdate.Set();
                    return;
                }
            }

            // I'd prefer the main thread to parse the upgradeXML as opposed to using this
            // "downloading" thread but i didn't know a better way of doing it other than
            // using a delegate like this.
            BeginParseUpgradeXml d = new BeginParseUpgradeXml(ParseUpgradeXml);
            XmlNode node = this.upgradeXml.SelectSingleNode("/UpdateableFiles");

            if (node != null) // xml file could be dodgy.
            {
                if (listViewDetails.InvokeRequired)
                {
                    listViewDetails.Invoke(d, node, null, node.Name);
                }
                else
                {
                    d(node, null, node.Name);
                }
            }
            
            int iUpdatesCount = NumUpdatableFiles();
            if (iUpdatesCount > 1)
                AddTextToLog(string.Format("There are {0} packages which can be updated.", iUpdatesCount), ImageType.Information);
            else if (iUpdatesCount == 1)
                AddTextToLog("There is 1 package which can be updated.", ImageType.Information);
            else
                AddTextToLog("All packages are up to date", ImageType.Information);

            if (chkShowAllFiles.Checked != (iUpdatesCount == 0))
            {
                if (chkShowAllFiles.InvokeRequired)
                    chkShowAllFiles.Invoke(new MethodInvoker(delegate { chkShowAllFiles.Checked = iUpdatesCount == 0; }));
                else
                    chkShowAllFiles.Checked = iUpdatesCount == 0;
            }
            else
            {
                if (listViewDetails.InvokeRequired)
                    listViewDetails.Invoke(new MethodInvoker(delegate { DisplayItems(chkShowAllFiles.Checked); }));
                else
                    DisplayItems(chkShowAllFiles.Checked);
            }

            webUpdate.Set();
        }

        /// <summary>
        /// Parses the upgrade XML file to populate the upgradeData array. 
        /// It's a recursive algorithm, so it needs to be passed the root node
        /// off the upgrade XML to start off, and it will then recurse
        /// through all the nodes in the file.
        /// </summary>
        /// <param name="currentNode">The node that the function should work on</param>
        private void ParseUpgradeXml(XmlNode currentNode, XmlNode groupNode, string path)
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
                    ParseUpgradeXml(childnode, childnode, newPath);
                else if (childnode.Attributes["type"].Value.Equals("subtree"))
                    ParseUpgradeXml(childnode, groupNode, newPath);
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
            iUpgradeable file = null;
            Version availableFile = null;
            bool fileAlreadyAdded = false;

            if ((file = upgradeData.FindByName(node.Name)) == null) // If this file isn't already in the upgradeData list
            {
                try
                {
                    if (!UpdateCacher.IsPackage(node.Name))
                        return;
                    if (groupNode.Name.Equals("MeGUI"))
                        file = new MeGUIFile(node.Name);
                    else if (groupNode.Name.Equals("ProgramFile"))
                        file = new ProgramFile(node.Name);
                    else if (groupNode.Name.Equals("ProfilesFile"))
                        file = new ProfilesFile(node.Name, MainForm.Instance);
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
                file.AvailableVersion = new Version();
                file.DownloadChecked = false;
                fileAlreadyAdded = true;
                if (file is ProfilesFile)
                    (file as ProfilesFile).MainForm = MainForm.Instance;
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
                availableFile = new Version();
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
                upgradeData.Add(file);
        }
        #endregion
        #region GUI
        private void DisplayItems(bool bShowAllFiles)
        {
            if (!this.Visible)
                return;

            ClearListview(this.listViewDetails);

            foreach (iUpgradeable file in upgradeData)
            {
                if (!bShowAllFiles)
                {
                    ProgramSettings pSettings = UpdateCacher.GetPackage(file.Name);
                    if (file.DownloadChecked)
                        AddToListview(file.CreateListViewItem());
                }
                else
                    AddToListview(file.CreateListViewItem());
            }

            lvwColumnSorter.SortColumn = 1;
            lvwColumnSorter.Order = SortOrder.Ascending;
            listViewDetails.Sort();

            foreach (ListViewItem item in listViewDetails.Items)
            {
                if (item.Index % 2 != 0)
                    item.BackColor = Color.White;
                else
                    item.BackColor = Color.FromArgb(255, 225, 235, 255);
            }
        }

        private void listViewDetails_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListViewItem itm = this.listViewDetails.Items[e.Index];
            // Do not allow checking if there are no updates or it is set to ignore.
            if (itm.SubItems["Status"].Text.Equals(EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString())
                || itm.SubItems["Status"].Text.Equals(EnumProxy.Create(PackageStatus.UpdateIgnored).ToString())
                || itm.SubItems["Status"].Text.Equals(EnumProxy.Create(PackageStatus.Disabled).ToString()))
                e.NewValue = CheckState.Unchecked;

            iUpgradeable file = upgradeData.FindByName(itm.Name);
            if (e.NewValue == CheckState.Checked)
                file.DownloadChecked = file.AllowUpdate = true;
            else
                file.DownloadChecked = false;

            if (e.NewValue == CheckState.Unchecked
                && itm.SubItems["Status"].Text.Equals(EnumProxy.Create(PackageStatus.Reinstall).ToString()))
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(itm.Name);
                if (pSettings != null && !pSettings.UpdateAllowed())
                    itm.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                else if (!file.isAvailable())
                    file.DownloadChecked = file.AllowUpdate = true;
                else if (!file.AllowUpdate && file.HasAvailableVersion)
                    itm.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateIgnored).ToString();
                else if (file.HasAvailableVersion)
                    itm.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                else
                    itm.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
            }
        }

        private void listViewDetails_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || listViewDetails.SelectedItems.Count != 1)
                return;

            // get the program settings
            ProgramSettings pSettings = UpdateCacher.GetPackage(listViewDetails.SelectedItems[0].Name);
            iUpgradeable file = upgradeData.FindByName(listViewDetails.SelectedItems[0].Name);

            // set the enable package value
            ToolStripMenuItem ts = (ToolStripMenuItem)statusToolStrip.Items[0];
            if (pSettings == null)
            {
                ts.Checked = true;
                ts.Enabled = false;
            }
            else
            {
                ts.Checked = pSettings.UpdateAllowed();
                ts.Enabled = !pSettings.Required;
            }

            // set the ignore update data value
            ts = (ToolStripMenuItem)statusToolStrip.Items[1];
            if (pSettings != null && !pSettings.UpdateAllowed())
            {
                ts.Checked = true;
                ts.Enabled = false;
            }
            else if (!file.isAvailable())
            {
                ts.Checked = false;
                ts.Enabled = false;
            }
            else
            {
                ts.Checked = !file.AllowUpdate;
                ts.Enabled = true;
            }

            // set the force reinstall data value
            ts = (ToolStripMenuItem)statusToolStrip.Items[2];
            if (pSettings != null && !pSettings.UpdateAllowed())
            {
                ts.Checked = false;
                ts.Enabled = false;
            }
            else if (!file.isAvailable())
            {
                ts.Checked = true;
                ts.Enabled = false;
            }
            else
            {
                ts.Checked = file.DownloadChecked;
                ts.Enabled = true;
            }

            if (file.HasAvailableVersion)
                ts.Text = "Install";
            else
                ts.Text = "Force reinstall";

            statusToolStrip.Show(Cursor.Position);
        }

        private void setIgnoreValue_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(item.Name);
                iUpgradeable file = upgradeData.FindByName(item.Name);
                Version latest = file.AvailableVersion;
                file.AllowUpdate = !(ts.Checked);

                if (pSettings != null && !pSettings.UpdateAllowed())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                    item.Checked = false;
                }
                else if (!file.isAvailable())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    item.Checked = true;
                    file.AllowUpdate = true;
                }
                else if (latest != null && file.CurrentVersion == null || latest.CompareTo(file.CurrentVersion) != 0)
                {
                    if (!file.AllowUpdate)
                    {
                        item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateIgnored).ToString();
                        item.Checked = false;
                    }
                    else
                    {
                        item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                        item.Checked = true;
                    }
                }
                else
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
                    item.Checked = false;
                }
            }
        }

        private void reinstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(item.Name);
                iUpgradeable file = upgradeData.FindByName(item.Name);
                Version latest = file.AvailableVersion;

                if (pSettings != null && !pSettings.UpdateAllowed())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                    item.Checked = false;
                }
                else if (ts.Checked || (!file.isAvailable()))
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    item.Checked = true;
                }
                else if (latest != null && file.CurrentVersion == null || latest.CompareTo(file.CurrentVersion) != 0)
                {
                    if (!file.AllowUpdate)
                    {
                        item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateIgnored).ToString();
                        item.Checked = false;
                    }
                    else
                    {
                        item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                        item.Checked = false;
                    }
                }
                else
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
                    item.Checked = false;
                }
            }
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
            {
                ProgramSettings pSettings = UpdateCacher.GetPackage(item.Name);
                if (pSettings != null)
                    UpdateCacher.CheckPackage(item.Name, ts.Checked, false);
                iUpgradeable file = upgradeData.FindByName(item.Name);
                Version latest = file.AvailableVersion;

                if (pSettings != null && !pSettings.UpdateAllowed())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Disabled).ToString();
                    item.Checked = false;
                }
                else if (!file.isAvailable())
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.Reinstall).ToString();
                    item.Checked = true;
                }
                else if (latest != null && file.CurrentVersion == null || latest.CompareTo(file.CurrentVersion) != 0)
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.UpdateAvailable).ToString();
                    item.Checked = true;
                }
                else
                {
                    item.SubItems["Status"].Text = EnumProxy.Create(PackageStatus.NoUpdateAvailable).ToString();
                    item.Checked = false;
                }             
            }
        }

        public void StartAutoUpdate()
        {
            this.Visible = true;
            btnUpdate_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            btnAbort.Enabled = true;
            updateThread = new Thread(new ThreadStart(ProcessUpdate));
            updateThread.IsBackground = true;
            updateThread.Start();
        }
        #endregion
        #region updating
        private void ProcessUpdate()
        {
            continueUpdate = true;
            bool needsRestart = false;
            int currentFile = 1; //the first file we update is file 1.
            ErrorState result;
            List<iUpgradeable> succeededFiles = new List<iUpgradeable>();
            List<iUpgradeable> failedFiles = new List<iUpgradeable>();
            List<iUpgradeable> missingFiles = new List<iUpgradeable>();

            // Count the number of files we can update before we restart
            int updateableFileCount = 0;
            foreach (iUpgradeable file in upgradeData)
                if (file.DownloadChecked)
                    updateableFileCount++;

            // Now update the files we can
            foreach (iUpgradeable file in upgradeData)
            {
                if (!continueUpdate)
                {
                    AddTextToLog("Update aborted by user", ImageType.Information);
                    return /* false*/;
                }

                if (!file.DownloadChecked)
                    continue;

                AddTextToLog(string.Format("Updating {0}. Package {1}/{2}.", file.DisplayName, currentFile, updateableFileCount), ImageType.Information);

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
                    result = UpdateCacher.DownloadFile(file.AvailableVersion.Url, null, new Uri(ServerAddress), wc_DownloadProgressChanged, this);
                    if (result != ErrorState.Successful)
                    {
                        failedFiles.Add(file);
                        AddTextToLog(string.Format("Failed to download package {0}: {1}.", file.DisplayName, EnumProxy.Create(result).ToString()), ImageType.Error);
                    }
                    else if (_updateStep != UpdateStep.AutomaticCheck)
                    {
                        // install only if not in full automatic mode and if download successful
                        ErrorState state = Install(file);
                        if (state != ErrorState.Successful)
                        {
                            if (state != ErrorState.RequirementNotMet)
                            {
                                AddTextToLog(string.Format("Failed to install package {0}: {1}.", file.DisplayName, EnumProxy.Create(state).ToString()), ImageType.Error);
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
                
                if (currentFile >= updateableFileCount)
                    break;
                currentFile++;
            }

            if (_updateStep == UpdateStep.AutomaticCheck)
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(delegate { this.Close(); }));
                else
                    this.Close();
                return;
            }

            SetProgressBar(0, 1, 1); //make sure progress bar is at 100%.

            if (succeededFiles.Count > 0)
                AddTextToLog("Packages which have been successfully updated: " + succeededFiles.Count, ImageType.Information);
            if (failedFiles.Count + missingFiles.Count > 0)
            {
                if (failedFiles.Count == 0)
                    AddTextToLog("Packages which have not been successfully updated: " + missingFiles.Count, ImageType.Warning);
                else
                    AddTextToLog("Packages which have not been successfully updated: " + (failedFiles.Count + missingFiles.Count), ImageType.Error);
            }

            UpdateCacher.flushOldCachedFilesAsync(upgradeData, this);

            if (MainForm.Instance.Settings.UpdateMode == UpdateMode.Automatic ||
                (MainForm.Instance.Settings.AutoUpdateSession && (failedFiles.Count + missingFiles.Count) == 0))
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(delegate { this.Close(); }));
                else
                    this.Close();
                return;
            }

            if (needsRestart)
            {
                if (MessageBox.Show("In order to finish the update, MeGUI needs to be restarted. Do you want to restart now?",
                    "Restart now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MainForm.Instance.Restart = true;
                    this.Invoke(new MethodInvoker(delegate { this.Close(); }));
                    MainForm.Instance.Invoke(new MethodInvoker(delegate { MainForm.Instance.Close(); }));
                    return/* true*/;
                }
            }

            int iUpdatesCount = NumUpdatableFiles();
            if (chkShowAllFiles.Checked != (iUpdatesCount == 0))
            {
                if (chkShowAllFiles.InvokeRequired)
                    chkShowAllFiles.Invoke(new MethodInvoker(delegate { chkShowAllFiles.Checked = iUpdatesCount == 0; }));
                else
                    chkShowAllFiles.Checked = iUpdatesCount == 0;
            }
            else
            {
                if (listViewDetails.InvokeRequired)
                    listViewDetails.Invoke(new MethodInvoker(delegate { DisplayItems(chkShowAllFiles.Checked); }));
                else
                    DisplayItems(chkShowAllFiles.Checked);
            }

            Invoke(new MethodInvoker(delegate
            {
                btnAbort.Enabled = false;
                btnUpdate.Enabled = true;
            }));
        }

        private ErrorState Install(iUpgradeable file)
        {
            if (file.RequiredBuild > 0 && new System.Version(Application.ProductVersion).Build < file.RequiredBuild)
            {
                AddTextToLog(string.Format("Could not install module '{0}' as at least MeGUI build {1} is required.", file.Name, file.RequiredBuild), ImageType.Warning);
                return ErrorState.RequirementNotMet;
            }

            if (!String.IsNullOrEmpty(file.RequiredNET) && String.IsNullOrEmpty(OSInfo.GetDotNetVersion(file.RequiredNET)))
            {
                AddTextToLog(string.Format("Could not install module '{0}' as .NET {1} is required.", file.Name, file.RequiredNET), ImageType.Warning);
                return ErrorState.RequirementNotMet;
            }

            ErrorState state = file.Install(file, this);
            if (state == ErrorState.Successful)
            {
                file.CurrentVersion = file.AvailableVersion;
                return ErrorState.Successful;
            }

            AddTextToLog(string.Format("Could not install module '{0}'.", file.Name), ImageType.Error);
            return state;
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive > 0)
                SetProgressBar(0, (int)e.TotalBytesToReceive, (int)e.BytesReceived);
        }

        public bool HasUpdatableFiles()
        {
            return NumUpdatableFiles() > 0;
        }

        public int NumUpdatableFiles()
        {
            int numUpdateableFiles = 0;
            foreach (iUpgradeable upgradeable in upgradeData)
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

        public void UpdateUploadDate(string name, string strDate)
        {
            iUpgradeable up = upgradeData.FindByName(name);
            if (up == null)
                return;

            DateTime oDate;
            bool bReady = DateTime.TryParse(strDate, new System.Globalization.CultureInfo("en-us"), System.Globalization.DateTimeStyles.None, out oDate);
            if (bReady)
                up.CurrentVersion.UploadDate = oDate;
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            updateThread.Abort();
            btnUpdate.Enabled = true;
            btnAbort.Enabled = false;
        }

        private void chkShowAllFiles_CheckedChanged(object sender, EventArgs e)
        {
            DisplayItems(chkShowAllFiles.Checked);
        }

        private void UpdateWindow_Move(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                MainForm.Instance.Settings.UpdateFormLocation = this.Location;
        }

        private void UpdateWindow_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                MainForm.Instance.Settings.UpdateFormSize = this.ClientSize;
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                MainForm.Instance.Settings.UpdateFormSplitter = this.splitContainer2.SplitterDistance;
        }

        private void listViewDetails_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            SaveComponentSettings();
        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.splitContainer1.Size.Height - 65;
        }

        private void UpdateWindow_Shown(object sender, EventArgs e)
        {
            DisplayItems(chkShowAllFiles.Checked);
        }
    }

    #endregion

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }

    public class UpdateOptions : MeGUI.core.plugins.interfaces.IOption
    {

        #region IOption Members

        public string Name
        {
            get { return "Update"; }
        }

        public void Run(MainForm info)
        {
            if (MainForm.Instance.UpdateWindow.InvokeRequired) // as invoke does not work when it comes to making the form visible a new instance is required
                MainForm.Instance.UpdateWindow = new UpdateWindow();

            UpdateWindow _updateWindow = MainForm.Instance.UpdateWindow;
            _updateWindow.GetUpdateData(false, UpdateWindow.UpdateStep.Manual);
            _updateWindow.Visible = true;
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlU }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "update_window"; }
        }

        #endregion
    }

    public class FileNotRegisteredYetException : MeGUIException
    {
        private string name;

        public string Name { get { return name; } }
        public FileNotRegisteredYetException(string name) : base("AutoUpdate file '" + name + "' not registered with MeGUI.")
        {
            this.name = name;
        }
    }
}