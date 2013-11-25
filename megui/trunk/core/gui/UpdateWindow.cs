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

using ICSharpCode.SharpZipLib.Zip;
using SevenZip;

using MeGUI.core.util;


namespace MeGUI
{
    public partial class UpdateWindow : Form
    {
        #region Variables
        private List<string> serverList;
        private MainForm mainForm = null;
        public static MeGUISettings meGUISettings = null;
        private bool continueUpdate = true;
        private iUpgradeableCollection upgradeData = null;
        private Thread updateThread = null;
        private StringBuilder logBuilder = new StringBuilder();
        private System.Threading.ManualResetEvent webUpdate = new ManualResetEvent(false);
        private XmlDocument upgradeXml = null;
        public bool needsRestart = false;
        private bool isOrHasDownloadedUpgradeData = false;
        private LogItem oLog;
        private String ServerAddress;
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
                availableVersions = new Versions(2);

            } // constructor

            // Overrideable methods
            public Version GetLatestVersion()
            {
                Version latest = new Version();
                foreach (Version v in this.availableVersions)
                    if (v.CompareTo(latest) != 0)
                        latest = v;

                return latest;
            }

            public bool HasAvailableVersions
            {
                get
                {
                    Version latest = GetLatestVersion();
                    if (this.name == "neroaacenc")
                    {
                        if (currentVersion.FileVersion != null && currentVersion.FileVersion.Equals(latest.FileVersion))
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
                    case "x265":
                        if (MainForm.Instance.Settings.UseX265)
                        {
                            arrPath.Add(MainForm.Instance.Settings.X265Path);
                            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.X265Path);
                            arrPath.Add(System.IO.Path.Combine(strPath, "avs4x265.exe"));
                        }
                        break;
                    case "x264":
                        {
                            arrPath.Add(MainForm.Instance.Settings.X264Path);
#if x86
                            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.X264Path);
                            if (OSInfo.isWow64())
                            { 
                                arrPath.Add(System.IO.Path.Combine(strPath, "avs4x264mod.exe"));
                                arrPath.Add(System.IO.Path.Combine(strPath, "x264_64.exe"));
                            }
#endif
                            break;
                        }
                    case "x264_10b":
                        if (MainForm.Instance.Settings.Use10bitsX264)
                        {
                            arrPath.Add(MainForm.Instance.Settings.X26410BitsPath);
#if x86
                            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.X26410BitsPath);
                            if (OSInfo.isWow64())
                            {
                                arrPath.Add(System.IO.Path.Combine(strPath, "avs4x264mod.exe"));
                                arrPath.Add(System.IO.Path.Combine(strPath, "x264-10b_64.exe"));
                            }
#endif
                        }
                        break;
                    case "dgindex": 
                        arrPath.Add(MainForm.Instance.Settings.DgIndexPath);
                        strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.DgIndexPath);
                        arrPath.Add(System.IO.Path.Combine(strPath, "DGDecode.dll"));
                        break;
                    case "dgavcindex": 
                        arrPath.Add(MainForm.Instance.Settings.DgavcIndexPath);
                        strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.DgavcIndexPath);
                        arrPath.Add(System.IO.Path.Combine(strPath, "DGAVCDecode.dll")); 
                        break;
                    case "dgindexnv":
                        if (MainForm.Instance.Settings.UseDGIndexNV)
                        {
                            arrPath.Add(MainForm.Instance.Settings.DgnvIndexPath);
                            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.DgnvIndexPath);
                            arrPath.Add(System.IO.Path.Combine(strPath, "DGDecodeNV.dll"));
                        }
                        break;
                    case "ffms":
                        arrPath.Add(MainForm.Instance.Settings.FFMSIndexPath);
                        strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.FFMSIndexPath);
                        arrPath.Add(System.IO.Path.Combine(strPath, "ffms2.dll"));
                        break;
                    case "mp4box": arrPath.Add(MainForm.Instance.Settings.Mp4boxPath); break;
                    case "pgcdemux": arrPath.Add(MainForm.Instance.Settings.PgcDemuxPath); break;
                    case "avimux_gui": arrPath.Add(MainForm.Instance.Settings.AviMuxGUIPath); break;
                    case "tsmuxer": arrPath.Add(MainForm.Instance.Settings.TSMuxerPath); break;
                    case "xvid_encraw": arrPath.Add(MainForm.Instance.Settings.XviDEncrawPath); break;
                    case "mkvmerge":
                        arrPath.Add(MainForm.Instance.Settings.MkvmergePath);
                        arrPath.Add(MainForm.Instance.Settings.MkvExtractPath);
                        break;
                    case "ffmpeg": arrPath.Add(MainForm.Instance.Settings.FFMpegPath); break;
                    case "oggenc2": arrPath.Add(MainForm.Instance.Settings.OggEnc2Path); break;
                    case "yadif": arrPath.Add(MainForm.Instance.Settings.YadifPath); break;
                    case "lame": arrPath.Add(MainForm.Instance.Settings.LamePath); break;
                    case "aften": arrPath.Add(MainForm.Instance.Settings.AftenPath); break;
                    case "flac": arrPath.Add(MainForm.Instance.Settings.FlacPath); break;
                    case "eac3to": arrPath.Add(MainForm.Instance.Settings.EAC3toPath); break;
                    case "qaac":
                        if (MainForm.Instance.Settings.UseQAAC)
                        {
                            arrPath.Add(MainForm.Instance.Settings.QaacPath);
                        }
                        break;
                    case "opus": arrPath.Add(MainForm.Instance.Settings.OpusPath); break;
                    case "libs":
                        {
                            strPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                            arrPath.Add((System.IO.Path.Combine(strPath, @"ICSharpCode.SharpZipLib.dll")));
                            arrPath.Add((System.IO.Path.Combine(strPath, @"MessageBoxExLib.dll")));
                            arrPath.Add((System.IO.Path.Combine(strPath, @"LinqBridge.dll")));
                            break;
                        }
                    case "mediainfo": arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"MediaInfo.dll")); break;
                    case "mediainfowrapper": arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"MediaInfoWrapper.dll")); break;
                    case "sevenzip": arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"7z.dll")); break;
                    case "sevenzipsharp": arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"SevenZipSharp.dll")); break;
                    case "data": arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"Data\ContextHelp.xml")); break;
                    case "avswrapper": arrPath.Add((System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"AvisynthWrapper.dll"))); break;
                    case "updatecopier": arrPath.Add((System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"updatecopier.exe"))); break;
                    case "convolution3dyv12": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"Convolution3DYV12.dll")); break;
                    case "undot": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"UnDot.dll")); break;
                    case "fluxsmooth": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"FluxSmooth.dll")); break;
                    case "eedi2": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"EEDI2.dll")); break;
                    case "decomb": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"Decomb.dll")); break;
                    case "leakkerneldeint": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"LeakKernelDeint.dll")); break;
                    case "tomsmocomp": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"TomsMoComp.dll")); break;
                    case "tdeint": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"TDeint.dll")); break;
                    case "tivtc": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"TIVTC.dll")); break;
                    case "colormatrix": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"ColorMatrix.dll")); break;
                    case "vsfilter": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"VSFilter.dll")); break;
                    case "nicaudio": arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"NicAudio.dll")); break;
                    case "bassaudio":
                        {
                            arrPath.Add(MainForm.Instance.Settings.BassPath);
                            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.BassPath);
                            arrPath.Add(System.IO.Path.Combine(strPath, "bass.dll"));
                            arrPath.Add(System.IO.Path.Combine(strPath, "bass_aac.dll"));
                            break;
                        }
                    case "vobsub": arrPath.Add(MainForm.Instance.Settings.VobSubPath); break;
                    case "besplit": arrPath.Add(MainForm.Instance.Settings.BeSplitPath); break;
                    case "avs":
                        {
                            strPath = Path.GetDirectoryName(MainForm.Instance.Settings.AviSynthPath);
                            arrPath.Add((Path.Combine(strPath, @"avisynth.dll")));
                            arrPath.Add((Path.Combine(strPath, @"directshowsource.dll")));
                            arrPath.Add((Path.Combine(strPath, @"devil.dll")));
                            break;
                        }
                    case "neroaacenc":
                        {
                            if (MainForm.Instance.Settings.UseNeroAacEnc)
                            {
                                arrPath.Add(MainForm.Instance.Settings.NeroAacEncPath);
                                if (File.Exists(MainForm.Instance.Settings.NeroAacEncPath))
                                {
                                    System.Diagnostics.FileVersionInfo finfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(MainForm.Instance.Settings.NeroAacEncPath);
                                    FileInfo fi = new FileInfo(MainForm.Instance.Settings.NeroAacEncPath);
                                    this.currentVersion.FileVersion = finfo.FileMajorPart + "." + finfo.FileMinorPart + "." + finfo.FileBuildPart + "." + finfo.FilePrivatePart;
                                    this.currentVersion.UploadDate = fi.LastWriteTimeUtc;
                                }
                            }
                            break;
                        }
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
                ListViewItem.ListViewSubItem platform = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem status = new ListViewItem.ListViewSubItem();

                myitem.Name = this.Name;

                name.Name = "Name";
                existingVersion.Name = "Existing Version";
                latestVersion.Name = "Latest Version";
                existingDate.Name = "Existing Date";
                latestDate.Name = "Latest Date";
                platform.Name = "Platform";
                status.Name = "Status";

                name.Text = this.DisplayName;

                Version v = GetLatestVersion();
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

                if (this.CurrentVersion != null)
                {
                    existingVersion.Text = this.CurrentVersion.FileVersion;
                    existingDate.Text = this.CurrentVersion.UploadDate.ToShortDateString();
                }
                else
                {
                    existingVersion.Text = "N/A";
                    existingDate.Text = "N/A";
                }

                if (!HasAvailableVersions)
                {
                    if (this.DownloadChecked)
                        status.Text = "Reinstalling";
                    else
                        status.Text = "No Update Available";
                }
                else
                {
                    if (this.AllowUpdate)
                    {
#if DEBUG
                        if (this.Name.Equals("core"))
                        {
                            if ((Int32.Parse(existingVersion.Text)) > (Int32.Parse(latestVersion.Text)))
                                status.Text = "Update Ignored";
                            else
                                status.Text = "Update Available";
                        }
                        else
#endif
                        status.Text = "Update Available";
                    }
                    else
                        status.Text = "Update Ignored";
                }

                if (this.AllowUpdate)
                {
                    if (this.DownloadChecked)
                        myitem.Checked = true;
                    else
                        myitem.Checked = false;
                }

                platform.Text = this.Platform.ToString();

                myitem.SubItems.Add(name);
                myitem.SubItems.Add(existingVersion);
                myitem.SubItems.Add(latestVersion);
                myitem.SubItems.Add(existingDate);
                myitem.SubItems.Add(latestDate);
                myitem.SubItems.Add(platform);
                myitem.SubItems.Add(status);
                return myitem;
            }
            public abstract ErrorState Upgrade();

            
            private bool downloadChecked;
            public bool DownloadChecked
            {
                get { return downloadChecked; }
                set { downloadChecked = value; }
            }

            private string savePath;
            public string SavePath
            {
                get { return savePath; }
                set { savePath = value; }
            }
            private string saveFolder;
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
                    else if (this.isAvailable() == false)
                        currentVersion = new Version();
                    return currentVersion;
                }
                set { currentVersion = value; }
            }

            private Versions availableVersions;
            public Versions AvailableVersions
            {
                get { return availableVersions; }
                set { availableVersions = value; }
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

            public bool Reinstall;

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

            public enum PlatformModes : int
            {
                any = 0,
                x86 = 1,
                x64 = 2
            }

            private PlatformModes platform;
            public PlatformModes Platform
            {
                get { return this.platform; }
                set { this.platform = value; }
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

            internal string treeViewID;
            public string TreeViewID
            {
                get { return this.treeViewID; }
                set { this.treeViewID = value; }
            }

            private bool needsRestartedCopying = false;
            public bool NeedsRestartedCopying
            {
                get { return needsRestartedCopying; }
                set { needsRestartedCopying = value; }
            }

            public virtual bool NeedsInstalling
            {
                get { return false; }
            }

            public virtual ErrorState Install(Stream fileData)
            {
                throw new Exception("This kind of file cannot be installed.");
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

            public ProfilesFile(string treeviewid, string name, MainForm mainForm)
            {
                this.treeViewID = treeviewid;
                this.Name = name;
                this.AllowUpdate = true;
                this.mainForm = mainForm;
            }
            
            private MainForm mainForm;

            public MainForm MainForm
            {
                set { mainForm = value; }
            }

            public override bool NeedsInstalling
            {
                get
                {
                    return true;
                }
            }

            public override ErrorState Install(Stream fileData)
            {
                try
                {
                    mainForm.importProfiles(fileData);
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

            public override ErrorState Upgrade()
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
        public class AviSynthFile : iUpgradeable
        {
            public override void init()
            {
                base.init();
                this.SaveFolder = MainForm.Instance.Settings.AvisynthPluginsPath;
            }
            private AviSynthFile()
            {
                this.SaveFolder = MainForm.Instance.Settings.AvisynthPluginsPath;
            }
            public AviSynthFile(string treeviewid, string name)
            {
                this.Name = name;
                this.AllowUpdate = true;
                this.treeViewID = treeviewid;
                this.SaveFolder = MainForm.Instance.Settings.AvisynthPluginsPath;
            }

            public override ErrorState Upgrade()
            {
                return ErrorState.CouldNotDownloadFile;
            }
        }
        public class MeGUIFile : iUpgradeable
        {
            private MeGUIFile()
            {
            }

            public MeGUIFile(string treeViewID, string name)
            {
                this.Name = name;
                this.AllowUpdate = true;
                this.treeViewID = treeViewID;
                this.SaveFolder = Application.StartupPath;
            }

            public override Version CurrentVersion
            {
                get
                {
                    if (Name == "core")
                    {
                        base.CurrentVersion.FileVersion = new System.Version(Application.ProductVersion).Build.ToString();
                        //FileInfo fi = new FileInfo(System.Windows.Forms.Application.ExecutablePath);
                        //base.CurrentVersion.UploadDate = fi.LastWriteTimeUtc.Date.AddMinutes(Math.Floor(fi.LastWriteTimeUtc.TimeOfDay.TotalMinutes));
                    }
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
                this.SaveFolder = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            }


            public override ErrorState Upgrade()
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
        public class ProgramFile : iUpgradeable
        {
            private ProgramFile()
            {
            }

            public override void init()
            {
                if (MeGUIFilePath == null)
                    throw new FileNotRegisteredYetException(Name);
                SavePath = Path.Combine(Application.StartupPath, MeGUIFilePath);
                // If the file doesn't exist, assume it isn't set up, so put it in the standard install location
                if (!File.Exists(SavePath))
                {
                    string extension = Path.GetExtension(SavePath);
                    string path = Path.Combine(System.Windows.Forms.Application.StartupPath, "tools");
                    path = Path.Combine(path, Name);
                    SavePath = Path.Combine(path,  Name + extension);
                    MeGUIFilePath = SavePath;
                }
            }
            
            public ProgramFile(string treeViewID, string name) // Constructor
            {
                this.Name = name;
                this.AllowUpdate = true;
                this.treeViewID = treeViewID;
                init();
            }

            private string MeGUIFilePath
            {
                get
                {
                    switch (this.Name)
                    {
                        case ("dgindex"):
                            return meGUISettings.DgIndexPath;
                        case ("mkvmerge"):
                            return meGUISettings.MkvmergePath;
                        case ("lame"):
                            return meGUISettings.LamePath;
                        case ("mp4box"):
                            return meGUISettings.Mp4boxPath;
                        case ("pgcdemux"):
                            return meGUISettings.PgcDemuxPath;
                        case ("neroaacenc"):
                            return meGUISettings.NeroAacEncPath;
                        case ("avimux_gui"):
                            return meGUISettings.AviMuxGUIPath;
                        case ("avs"):
                            return meGUISettings.AviSynthPath;
                        case ("x265"):
                            return meGUISettings.X265Path;
                        case ("x264"):
                            return meGUISettings.X264Path;
                        case ("x264_10b"):
                            return meGUISettings.X26410BitsPath;
                        case ("xvid_encraw"):
                            return meGUISettings.XviDEncrawPath;
                        case ("ffmpeg"):
                            return meGUISettings.FFMpegPath;
                        case ("oggenc2"):
                            return meGUISettings.OggEnc2Path;
                        case ("yadif"):
                            return meGUISettings.YadifPath;
                        case ("bassaudio"):
                            return meGUISettings.BassPath;
                        case ("vobsub"):
                            return meGUISettings.VobSubPath;
                        case ("besplit"):
                            return meGUISettings.BeSplitPath;
                        case ("aften"):
                            return meGUISettings.AftenPath;
                        case ("flac"):
                            return meGUISettings.FlacPath;
                        case ("eac3to"):
                            return meGUISettings.EAC3toPath;
                        case ("dgavcindex"):
                            return meGUISettings.DgavcIndexPath;
                        case ("dgindexnv"):
                            return meGUISettings.DgnvIndexPath;
                        case ("ffms"):
                            return meGUISettings.FFMSIndexPath;
                        case ("tsmuxer"):
                            return meGUISettings.TSMuxerPath;
                        case ("qaac"):
                            return meGUISettings.QaacPath;
                        case ("opus"):
                            return meGUISettings.OpusPath;                                 
                        default:
                            return null;
                    }
                }
                set
                {
                    switch (this.Name)
                    {
                        case ("neroaacenc"):
                            meGUISettings.NeroAacEncPath = value;
                            break;
                    }
                }
            }

            public override ErrorState Upgrade()
            {
                return ErrorState.CouldNotDownloadFile;
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
        public class Versions : CollectionBase
        {
            public Versions()
            {
                this.Capacity = 2;
            }
            public Versions(int capacity)
            {
                this.Capacity = capacity;
            }
            public Version this[int index]
            {
                get { return (Version)this.List[index]; }
                set { this.List[index] = value; }
            }
            public void Add(Version item)
            {
                this.InnerList.Add(item);
            }
            public void Remove(Version item)
            {
                this.InnerList.Remove(item);
            }
        }
        #endregion
        #region Delegates and delegate methods
        delegate void BeginParseUpgradeXml(XmlNode node, XmlNode groupNode, string path);
        private delegate void SetLogText();
        private delegate void SetListView(ListViewItem item);
        private delegate void ClearItems(ListView listview);

        private delegate void UpdateProgressBar(int minValue, int maxValue, int currentValue);
        private void SetProgressBar(int minValue, int maxValue, int currentValue)
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
            if (!this.Visible)
                return;
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

            if (item.Index % 2 != 0)
                item.BackColor = Color.White;
            else
                item.BackColor = Color.WhiteSmoke;
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
        #region Enums
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
            [EnumTitle("File cannot be renamed")]
            CouldNotRenameExistingFile,
            [EnumTitle("File cannot be installed")]
            CouldNotInstall,
            [EnumTitle("Update successful")]
            Successful,
            [EnumTitle("File cannot be extracted")]
            CouldNotUnzip,
            [EnumTitle("Update XML is invalid")]
            InvalidXML,
            [EnumTitle("The requirements for this file are not met")]
            RequirementNotMet
        }
        #endregion
        #region con/de struction
        private void UpdateWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Constructor for Updatewindow.
        /// </summary>
        /// <param name="mainForm">MainForm instance</param>
        /// <param name="savedSettings">Current MeGUI settings</param>
        /// <param name="bSilent">whether the update window should be displayed</param>
        public UpdateWindow(MainForm mainForm, bool bSilent)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.oLog = mainForm.UpdateLog;
            LoadComponentSettings();
            this.upgradeData = new iUpgradeableCollection(32); // To avoid unnecessary resizing, start at 32.
            meGUISettings = mainForm.Settings; // Load up the MeGUI settings so i can access filepaths
            if (bSilent)
                return;
            if (mainForm.Settings.AutoUpdateServerLists[mainForm.Settings.AutoUpdateServerSubList].Length <= 1)
            {
                MessageBox.Show("Couldn't run auto-update since there are no servers registered.", "No servers registered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            serverList = getUpdateServerList(mainForm.Settings.AutoUpdateServerLists[mainForm.Settings.AutoUpdateServerSubList]);
            LoadSettings();
        }

        private void LoadComponentSettings()
        {
            // Restore Size/Position of the window
            this.ClientSize = mainForm.Settings.UpdateFormSize;
            this.Location = mainForm.Settings.UpdateFormLocation;
            this.splitContainer2.SplitterDistance = mainForm.Settings.UpdateFormSplitter;

            colUpdate.Width = mainForm.Settings.UpdateFormUpdateColumnWidth;
            colName.Width = mainForm.Settings.UpdateFormNameColumnWidth;
            colExistingVersion.Width = mainForm.Settings.UpdateFormLocalVersionColumnWidth;
            colLatestVersion.Width = mainForm.Settings.UpdateFormServerVersionColumnWidth;
            colExistingDate.Width = mainForm.Settings.UpdateFormLocalDateColumnWidth;
            colLatestDate.Width = mainForm.Settings.UpdateFormServerDateColumnWidth;
            colPlatform.Width = mainForm.Settings.UpdateFormPlatformColumnWidth;
            colStatus.Width = mainForm.Settings.UpdateFormStatusColumnWidth;
        }

        private void SaveComponentSettings()
        {
            mainForm.Settings.UpdateFormUpdateColumnWidth = colUpdate.Width;
            mainForm.Settings.UpdateFormNameColumnWidth = colName.Width;
            mainForm.Settings.UpdateFormLocalVersionColumnWidth = colExistingVersion.Width;
            mainForm.Settings.UpdateFormServerVersionColumnWidth = colLatestVersion.Width;
            mainForm.Settings.UpdateFormLocalDateColumnWidth = colExistingDate.Width;
            mainForm.Settings.UpdateFormServerDateColumnWidth = colLatestDate.Width;
            mainForm.Settings.UpdateFormPlatformColumnWidth = colPlatform.Width;
            mainForm.Settings.UpdateFormStatusColumnWidth = colStatus.Width;
        }

        public static bool isComponentMissing()
        {
            ArrayList arrPath = new ArrayList();
            string strPath;

            // base 
            arrPath.Add(System.Windows.Forms.Application.ExecutablePath);
            // x264
            arrPath.Add(MainForm.Instance.Settings.X264Path);
#if x86
            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.X264Path);
            if (OSInfo.isWow64())
            {
                arrPath.Add(System.IO.Path.Combine(strPath, "avs4x264mod.exe"));
                arrPath.Add(System.IO.Path.Combine(strPath, "x264_64.exe"));
            }
#endif
            //x264 10bit
            if (MainForm.Instance.Settings.Use10bitsX264)
            {
                arrPath.Add(MainForm.Instance.Settings.X26410BitsPath);
#if x86
                strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.X26410BitsPath);
                if (OSInfo.isWow64())
                {
                    arrPath.Add(System.IO.Path.Combine(strPath, "avs4x264mod.exe"));
                    arrPath.Add(System.IO.Path.Combine(strPath, "x264-10b_64.exe"));
                }
#endif
            }

            // x265
            if (MainForm.Instance.Settings.UseX265)
            {
                arrPath.Add(MainForm.Instance.Settings.X265Path);
                strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.X265Path);
                arrPath.Add(System.IO.Path.Combine(strPath, "avs4x265.exe"));
            }

            // dgindex
            arrPath.Add(MainForm.Instance.Settings.DgIndexPath);
            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.DgIndexPath);
            arrPath.Add(System.IO.Path.Combine(strPath, "DGDecode.dll"));
#if x86
            // dgavcindex
            arrPath.Add(MainForm.Instance.Settings.DgavcIndexPath);
            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.DgavcIndexPath);
            arrPath.Add(System.IO.Path.Combine(strPath, "DGAVCDecode.dll"));
#endif
            //ffms
            arrPath.Add(MainForm.Instance.Settings.FFMSIndexPath);
            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.FFMSIndexPath);
            arrPath.Add(System.IO.Path.Combine(strPath, "ffms2.dll"));
            //mp4box
            arrPath.Add(MainForm.Instance.Settings.Mp4boxPath);
            //pgcdemux
            arrPath.Add(MainForm.Instance.Settings.PgcDemuxPath);
            //avimux_gui
            arrPath.Add(MainForm.Instance.Settings.AviMuxGUIPath);
            //tsmuxer
            arrPath.Add(MainForm.Instance.Settings.TSMuxerPath);
            //xvid_encraw
            arrPath.Add(MainForm.Instance.Settings.XviDEncrawPath);
            //mkvmerge
            arrPath.Add(MainForm.Instance.Settings.MkvmergePath);
            arrPath.Add(MainForm.Instance.Settings.MkvExtractPath);
            //ffmpeg
            arrPath.Add(MainForm.Instance.Settings.FFMpegPath);
            //oggenc2
            arrPath.Add(MainForm.Instance.Settings.OggEnc2Path);
            //yadif
            arrPath.Add(MainForm.Instance.Settings.YadifPath);
            //lame
            arrPath.Add(MainForm.Instance.Settings.LamePath);
            //aften
            arrPath.Add(MainForm.Instance.Settings.AftenPath);
            //flac
            arrPath.Add(MainForm.Instance.Settings.FlacPath);
            //eac3to
            arrPath.Add(MainForm.Instance.Settings.EAC3toPath);
            //opus
            arrPath.Add(MainForm.Instance.Settings.OpusPath);
            //libs":
            strPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            arrPath.Add((System.IO.Path.Combine(strPath, @"ICSharpCode.SharpZipLib.dll")));
            arrPath.Add((System.IO.Path.Combine(strPath, @"MessageBoxExLib.dll")));
            arrPath.Add((System.IO.Path.Combine(strPath, @"LinqBridge.dll")));
            //mediainfo
            arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"MediaInfo.dll"));
            //mediainfowrapper
            arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"MediaInfoWrapper.dll"));
            //sevenzip
            arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"7z.dll"));
            //sevenzipsharp
            arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"SevenZipSharp.dll"));
            //data
            arrPath.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"Data\ContextHelp.xml"));
            //avswrapper
            arrPath.Add((System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"AvisynthWrapper.dll")));
            //updatecopier
            arrPath.Add((System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"updatecopier.exe")));
#if x86
            //convolution3dyv12
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"Convolution3DYV12.dll"));
            //fluxsmooth
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"FluxSmooth.dll"));
            //decomb
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"Decomb.dll"));
            //tomsmocomp
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"TomsMoComp.dll"));
            //tdeint
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"TDeint.dll"));
            //tivtc
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"TIVTC.dll"));
            //colormatrix
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"ColorMatrix.dll"));
            //vsfilter
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"VSFilter.dll"));
            //nicaudio
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"NicAudio.dll"));
            //bassaudio
            arrPath.Add(MainForm.Instance.Settings.BassPath);
            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.BassPath);
            arrPath.Add(System.IO.Path.Combine(strPath, "bass.dll"));
            arrPath.Add(System.IO.Path.Combine(strPath, "bass_aac.dll"));
#endif
            //undot
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"UnDot.dll"));
            //eedi2
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"EEDI2.dll"));
            //leakkerneldeint
            arrPath.Add(System.IO.Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, @"LeakKernelDeint.dll"));
            //vobsub
            arrPath.Add(MainForm.Instance.Settings.VobSubPath);
            //besplit
            arrPath.Add(MainForm.Instance.Settings.BeSplitPath);

            //qaac
            if (MainForm.Instance.Settings.UseQAAC)
                arrPath.Add(MainForm.Instance.Settings.QaacPath);

            //neroaacenc
            if (MainForm.Instance.Settings.UseNeroAacEnc)
                arrPath.Add(MainForm.Instance.Settings.NeroAacEncPath);

            // dgindexnv
            if (MainForm.Instance.Settings.UseDGIndexNV)
            {
                arrPath.Add(MainForm.Instance.Settings.DgnvIndexPath);
                strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.DgnvIndexPath);
                arrPath.Add(System.IO.Path.Combine(strPath, "DGDecodeNV.dll"));
            }

            // avisynth
            arrPath.Add(MainForm.Instance.Settings.AviSynthPath);
            strPath = System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.AviSynthPath);
            arrPath.Add(System.IO.Path.Combine(strPath, "directshowsource.dll"));
            arrPath.Add(System.IO.Path.Combine(strPath, "devil.dll"));

            bool bComponentMissing = false;
            foreach (string strAppPath in arrPath)
            {
                if (String.IsNullOrEmpty(strAppPath))
                {
                    MainForm.Instance.UpdateLog.LogEvent("No path to check for missing components!", ImageType.Error);
                    bComponentMissing = true;
                    continue;
                }
                else if (File.Exists(strAppPath) == false)
                {
                    MainForm.Instance.UpdateLog.LogEvent("Component not found: " + strAppPath, ImageType.Error);
                    bComponentMissing = true;
                    continue;
                }
                FileInfo fInfo = new FileInfo(strAppPath);
                if (fInfo.Length == 0)
                {
                    MainForm.Instance.UpdateLog.LogEvent("Component has 0 bytes: " + strAppPath, ImageType.Error);
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
       
            GetUpdateData(false);

            if (VistaStuff.IsVistaOrNot)
                VistaStuff.SetWindowTheme(listViewDetails.Handle, "explorer", null);
        }
        #endregion
        #region load and save
        private void LoadSettings()
        {
            string path = Path.Combine(Application.StartupPath, "AutoUpdate.xml");
            if (File.Exists(path))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(iUpgradeableCollection), new Type[] { typeof(ProgramFile), typeof(AviSynthFile), typeof(ProfilesFile) , typeof(MeGUIFile)});
                    StreamReader settingsReader = new StreamReader(path);
                    iUpgradeableCollection upgradeDataTemp = (iUpgradeableCollection)serializer.Deserialize(settingsReader);
                    settingsReader.Dispose();
                    
                    foreach (iUpgradeable file in upgradeDataTemp)
                    {
                        if (file.Name.Equals("neroaacenc") && !MainForm.Instance.Settings.UseNeroAacEnc)
                            continue;
                        if (file.Name.Equals("dgindexnv") && !MainForm.Instance.Settings.UseDGIndexNV)
                            continue;
                        if (file.Name.Equals("qaac") && !MainForm.Instance.Settings.UseQAAC)
                            continue;
                        if (file.Name.Equals("x265") && !MainForm.Instance.Settings.UseX265)
                            continue;
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

                    return; //settings loaded correctly
                }
                catch(Exception)
                {
                    MessageBox.Show("Error: Could not load previous settings", "Error", MessageBoxButtons.OK);
                    return; // error loading settings
                }
            }
        }
        public void SaveSettings()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(iUpgradeableCollection), new Type[] { typeof(ProgramFile), typeof(AviSynthFile), typeof(ProfilesFile), typeof(MeGUIFile) });
                StreamWriter output = new StreamWriter(Path.Combine(Application.StartupPath, "AutoUpdate.xml"), false);
                serializer.Serialize(output, this.upgradeData);
                output.Dispose();
                return; //settings saved
            }
            catch (Exception)
            {
                MessageBox.Show("Error: Could not save settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion
        #region getting update data
        public void GetUpdateData(bool wait)
        {
            if (!isOrHasDownloadedUpgradeData)
            {
                Thread CreateTreeview = new Thread(new ThreadStart(ProcessUpdateXML));
                CreateTreeview.IsBackground = true;
                CreateTreeview.Start();
                if (wait)
                    webUpdate.WaitOne();
                DisplayItems(chkShowAllFiles.Checked);
            }
        }
        /// <summary>
        /// This method is called to retrieve the update data from the webserver
        /// and then set the relevant information to the grid.
        /// </summary>
        public ErrorState GetUpdateXML(string strServerAddress)
        {
            if (upgradeXml != null) // the update file has already been downloaded and processed
                return ErrorState.Successful;

            string data = null;
            upgradeXml = new XmlDocument();

            if (String.IsNullOrEmpty(strServerAddress))
            {
                // check local file
#if x86
                string strLocalUpdateXML = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "upgrade.xml");
#endif
#if x64
                string strLocalUpdateXML = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "upgrade_x64.xml");
#endif
                if (File.Exists(strLocalUpdateXML))
                {
                    AddTextToLog("Retrieving local update file", ImageType.Information);
                    StreamReader sr = new StreamReader(strLocalUpdateXML);
                    data = sr.ReadToEnd();
                    sr.Close();
                    AddTextToLog("Local update file opened successfully", ImageType.Information);
                }
                else
                {
                    upgradeXml = null;
                    return ErrorState.ServerNotAvailable;
                }
            }
            else
            {
                WebClient serverClient = new WebClient();

                // check for proxy authentication...
                serverClient.Proxy = HttpProxy.GetProxy(meGUISettings);
                
                try
                {
#if x86
                    data = serverClient.DownloadString(strServerAddress + "upgrade.xml?offCache=" + System.Guid.NewGuid().ToString("N"));
#endif
#if x64
                    data = serverClient.DownloadString(strServerAddress + "upgrade_x64.xml?offCache=" + System.Guid.NewGuid().ToString("N"));
#endif
                }
                catch
                {
                    AddTextToLog("Could not connect to server " + strServerAddress, ImageType.Error);
                    upgradeXml = null;
                    return ErrorState.ServerNotAvailable;
                }
            }

            try
            {
                upgradeXml.LoadXml(data);
            }
            catch
            {
                AddTextToLog("Invalid or missing update file. Aborting.", ImageType.Error);
                upgradeXml = null;
                return ErrorState.InvalidXML;
            }
            return ErrorState.Successful;
        }
        /// <summary>
        /// This function downloads the update XML file from the server and then processes it.
        /// </summary>
        private void ProcessUpdateXML()
        {
            isOrHasDownloadedUpgradeData = true;
            ErrorState value = ErrorState.ServerNotAvailable;
            foreach (string serverName in serverList)
            {
                ServerAddress = serverName;
                AddTextToLog("Connecting to server: " + serverName, ImageType.Information);
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
                    return;
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
            RemoveOldFiles();

            int iUpdatesCount = NumUpdatableFiles();
            if (iUpdatesCount > 1)
                AddTextToLog(string.Format("There are {0} files which can be updated.", iUpdatesCount), ImageType.Information);
            else if (iUpdatesCount == 1)
                AddTextToLog("There is 1 file which can be updated.", ImageType.Information);
            else
                AddTextToLog("All files are up to date", ImageType.Information);

            bool bChecked = chkShowAllFiles.Checked;
            if (chkShowAllFiles.InvokeRequired)
                chkShowAllFiles.Invoke(new MethodInvoker(delegate { chkShowAllFiles.Checked = iUpdatesCount == 0; }));
            else
                chkShowAllFiles.Checked = iUpdatesCount == 0;
            if (chkShowAllFiles.Checked == bChecked)
            {
                if (chkShowAllFiles.InvokeRequired)
                    chkShowAllFiles.Invoke(new MethodInvoker(delegate { DisplayItems(chkShowAllFiles.Checked); }));
                else
                    DisplayItems(chkShowAllFiles.Checked);   
            }

            webUpdate.Set();
        }

        private void RemoveOldFiles()
        {
            iUpgradeable iFileToRemove = null;
            XmlNodeList xnList = upgradeXml.SelectNodes("/UpdateableFiles");
            do
            {
                iFileToRemove = null;
                foreach (iUpgradeable iFile in upgradeData)
                {
                    if (FindFileInUpdateXML(xnList, iFile.Name) == false)
                    {
                        iFileToRemove = iFile;
                        break;
                    }
                }
                upgradeData.Remove(iFileToRemove);
            } while (iFileToRemove != null);
        }

        private bool FindFileInUpdateXML(XmlNodeList xnList, string strFile)
        { 
            foreach (XmlNode l1 in xnList)
            {
                if (l1.Attributes["type"].Value.Equals("file"))
                {
                    if (l1.Name.Equals(strFile))
                        return true;
                    continue;
                }
                if (FindFileInUpdateXML(l1.ChildNodes, strFile) == true)
                    return true;
            }
            return false;
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
                    ParseFileData(childnode, groupNode, path);
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
        private void ParseFileData(XmlNode node, XmlNode groupNode, string treePath)
        {
            iUpgradeable file = null;
            Version availableFile = null;
            bool fileAlreadyAdded = false;

            if (node.Name.Equals("neroaacenc") && !MainForm.Instance.Settings.UseNeroAacEnc)
                return;
            if (node.Name.Equals("dgindexnv") && !MainForm.Instance.Settings.UseDGIndexNV)
                return;
            if (node.Name.Equals("qaac") && !MainForm.Instance.Settings.UseQAAC)
                return;
            if (node.Name.Equals("x264_10b") && !MainForm.Instance.Settings.Use10bitsX264)
                return;
            if (node.Name.Equals("x265") && !MainForm.Instance.Settings.UseX265)
                return;

            var nameAttribute = node.Attributes["platform"];
            if (nameAttribute != null)
            {
#if x86
                if (nameAttribute.Value.Equals("x64"))
#endif
#if x64
                if (nameAttribute.Value.Equals("x86"))
#endif
                    return;
            }
            
            if ((file = upgradeData.FindByName(node.Name)) == null) // If this file isn't already in the upgradeData list
            {
                try
                {
                    if (groupNode.Name.Equals("MeGUI"))
                        file = new MeGUIFile(treePath, node.Name);
                    else if (groupNode.Name.Equals("ProgramFile"))
                        file = new ProgramFile(treePath, node.Name);
                    else if (groupNode.Name.Equals("AviSynthFile"))
                        file = new AviSynthFile(treePath, node.Name);
                    else if (groupNode.Name.Equals("ProfilesFile"))
                        file = new ProfilesFile(treePath, node.Name, mainForm);
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
                file.AvailableVersions = new Versions();
                file.DownloadChecked = false;
                file.treeViewID = treePath;
                fileAlreadyAdded = true;
                if (file is ProfilesFile)
                    (file as ProfilesFile).MainForm = mainForm;
            }

            file.Platform = iUpgradeable.PlatformModes.any;
            if (nameAttribute != null)
            {
                if (nameAttribute.Value.Equals("x86"))
                    file.Platform = iUpgradeable.PlatformModes.x86;
                else if (nameAttribute.Value.Equals("x64"))
                    file.Platform = iUpgradeable.PlatformModes.x64;
            }

            file.NeedsRestartedCopying = false;
            nameAttribute = node.Attributes["needsrestart"];
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
            if (nameAttribute != null)
            {
                file.DisplayName = nameAttribute.Value;
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
                    else if (oAttribute.Name.Equals("date"))
                    {
                        DateTime oDate = new DateTime();
                        DateTime.TryParse(filenode.Attributes["date"].Value, new System.Globalization.CultureInfo("en-us"), System.Globalization.DateTimeStyles.None, out oDate);
                        availableFile.UploadDate = oDate;
                    }
                    else if (oAttribute.Name.Equals("url"))
                    {
                        availableFile.Web = filenode.Attributes["url"].Value;
                    }
                }

                file.AvailableVersions.Add(availableFile);
            }
            if (file.GetLatestVersion().CompareTo(file.CurrentVersion) != 0 && file.AllowUpdate && file.HasAvailableVersions)
                file.DownloadChecked = true;

            if (!fileAlreadyAdded)
                upgradeData.Add(file);
        }
        #endregion
        #region GUI
        private void DisplayItems(bool bShowAllFiles)
        {
            ClearListview(this.listViewDetails);

            foreach (iUpgradeable file in upgradeData)
            {
                if (!bShowAllFiles)
                {
                    if (file.HasAvailableVersions || file.DownloadChecked)
                        AddToListview(file.CreateListViewItem());
                }
                else
                {
                    AddToListview(file.CreateListViewItem());
                }
            }
        }
        private void listViewDetails_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListViewItem itm = this.listViewDetails.Items[e.Index];
            // Do not allow checking if there are no updates or it is set to ignore.
            if (itm.SubItems["Status"].Text.Equals("No Update Available")
                || itm.SubItems["Status"].Text.Equals("Update Ignored"))
                e.NewValue = CheckState.Unchecked;

            iUpgradeable file = upgradeData.FindByName(itm.Name);
            if (e.NewValue == CheckState.Checked)
                file.DownloadChecked = true;
            else
                file.DownloadChecked = false;

            if (e.NewValue == CheckState.Unchecked && itm.SubItems["Status"].Text == "Reinstalling")
                itm.SubItems["Status"].Text = file.AllowUpdate ? (file.HasAvailableVersions ? "Update Available" : "No Update Available") : "Update Ignored";
        }

        private void listViewDetails_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewDetails.SelectedItems.Count > 0)
                {
                    ToolStripMenuItem ts = (ToolStripMenuItem)statusToolStrip.Items[0];
                    bool allowupdate = false;
                    foreach (ListViewItem item in listViewDetails.SelectedItems)
                    {
                        allowupdate |= upgradeData.FindByName(item.Name).AllowUpdate;
                    }
                    ts.Checked = !allowupdate;
                    statusToolStrip.Show(Cursor.Position);
                }
            }
        }

        private void setIgnoreValue_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
            {
                iUpgradeable file = upgradeData.FindByName(item.Name);

                file.AllowUpdate = !(ts.Checked);
                Version latest = file.GetLatestVersion();

                if (file.AllowUpdate)
                {
                    if (latest == null && file.CurrentVersion == null)
                    {
                        item.SubItems["Status"].Text = "No Update Available";
                        item.Checked = false;
                    }
                    else if (latest != null && file.CurrentVersion == null)
                    {
                        item.SubItems["Status"].Text = "Update Available";
                        item.Checked = true;
                    }
                    else if (latest.CompareTo(file.CurrentVersion) != 0)
                    {
                        item.SubItems["Status"].Text = "Update Available";
                        item.Checked = true;
                    }
                    else
                        item.SubItems["Status"].Text = "No Update Available";
                }
                else
                {
                    item.Checked = false;
                    item.SubItems["Status"].Text = "Update Ignored";
                }
            }
        }

        public void StartAutoUpdate()
        {
            btnUpdate_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            btnAbort.Enabled = true;
            updateThread = new Thread(new ThreadStart(BeginUpdate));
            updateThread.IsBackground = true;
            updateThread.Start();
        }
        #endregion
        #region updating
        private void InstallFiles(SortedDictionary<uint, List<iUpgradeable>> groups)
        {
            continueUpdate = true;
            int currentFile = 1; //the first file we update is file 1.
            ErrorState result;
            List<iUpgradeable> succeededFiles = new List<iUpgradeable>();
            List<iUpgradeable> failedFiles = new List<iUpgradeable>();
            List<iUpgradeable> missingFiles = new List<iUpgradeable>();
            
            // Count the number of files we can update before we restart
            int updateableFileCount = 0;
            bool needsRestart = false;
            foreach (List<iUpgradeable> group in groups.Values)
                updateableFileCount += group.Count;

            // Now update the files we can
            foreach (List<iUpgradeable> group in groups.Values)
            {
                foreach (iUpgradeable file in group)
                {
                    if (!continueUpdate)
                    {
                        AddTextToLog("Update aborted by user", ImageType.Information);
                        return /* false*/;
                    }

                    AddTextToLog(string.Format("Updating {0}. File {1}/{2}.", file.Name, currentFile, updateableFileCount), ImageType.Information);

                    if (!String.IsNullOrEmpty(file.GetLatestVersion().Web))
                    {
                        string strText;

                        if (file.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals("neroaacenc"))
                        {
                            string strPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\tools\eac3to\neroAacEnc.exe";
                            strText = "MeGUI cannot find " + file.Name + " on your system or it is outdated.\nDue to the licensing the component is not included on the MeGUI update server.\n\nTherefore please download the file on your own and extract neroaacenc.exe to:\n" + strPath + "\n\nIf necessary change the path in the settings:\n\"Settings\\External Program Settings\"\n\nDo you would like to download it now?";
                        }
                        else
                            strText = "MeGUI cannot find " + file.Name + " on your system or it is outdated.\nDue to the licensing the component is not included on the MeGUI update server.\n\nTherefore please download the file on your own, extract it and set the path to the " + file.Name + ".exe in the MeGUI settings\n(\"Settings\\External Program Settings\").\n\nDo you would like to download it now?";

                        if (MessageBox.Show(strText, "Component not found",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(file.GetLatestVersion().Web);
                            succeededFiles.Add(file);
                        }
                        else
                            failedFiles.Add(file); 
                    }
                    else
                    {
                        Stream str;
                        result = UpdateCacher.DownloadFile(file.GetLatestVersion().Url, new Uri(ServerAddress), out str, wc_DownloadProgressChanged, this);
                        if (result != ErrorState.Successful)
                        {
                            failedFiles.Add(file);
                            AddTextToLog(string.Format("Failed to download file {0} with error: {1}.", file.Name, EnumProxy.Create(result).ToString()), ImageType.Error);
                        }
                        else
                        {
                            try
                            {
                                ErrorState state;
                                if (file.NeedsInstalling)
                                    state = Install(file, str);
                                else
                                    state = SaveNewFile(file, str);

                                if (state != ErrorState.Successful)
                                {
                                    if (state != ErrorState.RequirementNotMet)
                                    {
                                        AddTextToLog(string.Format("Failed to install file {0} with error: {1}.", file.Name, EnumProxy.Create(state).ToString()), ImageType.Error);
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
                                }
                            }
                            finally { str.Close(); }
                        }
                    }
                    currentFile++;
                }

                if (currentFile >= updateableFileCount) 
                    break;
            }

            SetProgressBar(0, 1, 1); //make sure progress bar is at 100%.

            if (succeededFiles.Count > 0)
                AddTextToLog("Files which have been successfully updated: " + succeededFiles.Count, ImageType.Information);
            if (failedFiles.Count + missingFiles.Count > 0)
            {
                if (failedFiles.Count == 0)
                    AddTextToLog("Files which have not been successfully updated: " + missingFiles.Count, ImageType.Warning);
                else
                    AddTextToLog("Files which have not been successfully updated: " + (failedFiles.Count + missingFiles.Count), ImageType.Error);
            }
            else
            {
#if x86
                string strLocalUpdateXML = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "upgrade.xml");
#endif
#if x64
                string strLocalUpdateXML = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "upgrade_x64.xml");
#endif
                if (File.Exists(strLocalUpdateXML))
                    File.Delete(strLocalUpdateXML);
            }

            // call function so that avisynth dlls will be copied to the root if needed
            VideoUtil.getAvisynthVersion(null);

            List<string> files = new List<string>();
            foreach (iUpgradeable u in upgradeData)
                files.Add(u.GetLatestVersion().Url);
            UpdateCacher.flushOldCachedFilesAsync(files, this);

            if (needsRestart)
            {
                if (MessageBox.Show("In order to finish the update, MeGUI needs to be restarted. Do you want to restart now?",
                    "Restart now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    mainForm.Restart = true;
                    this.Invoke(new MethodInvoker(delegate { this.Close(); }));
                    mainForm.Invoke(new MethodInvoker(delegate { mainForm.Close(); }));
                    return/* true*/;
                }
            }
            if (MainForm.Instance.Settings.AutoUpdateSession)
            {
                this.Invoke(new MethodInvoker(delegate { this.Close(); }));
                return;
            }
            listViewDetails.Invoke(new MethodInvoker(delegate { DisplayItems(chkShowAllFiles.Checked); }));
            Invoke(new MethodInvoker(delegate
            {
                btnAbort.Enabled = false;
                btnUpdate.Enabled = true;
            }));
        }

        /// <summary>
        /// This function iterates through all the selected files and downloads them
        /// one by one.
        /// </summary>
        private void BeginUpdate()
        {
            // Sort the files to download according to their install priority
            SortedDictionary<uint, List<iUpgradeable>> groups = new SortedDictionary<uint, List<iUpgradeable>>();
            foreach (iUpgradeable file in upgradeData)
            {
                if (file.DownloadChecked)
                {
                    if (!groups.ContainsKey(0))
                        groups[0] = new List<iUpgradeable>();
                    groups[0].Add(file);
                }
            }

            InstallFiles(groups /*, false*/ );
        }

        private ErrorState Install(iUpgradeable file, Stream fileData)
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

            ErrorState state = file.Install(fileData);
            if (state == ErrorState.Successful)
            {
                file.CurrentVersion = file.GetLatestVersion();
                return ErrorState.Successful;
            }

            AddTextToLog(string.Format("Could not install module '{0}'.", file.Name), ImageType.Error);
            return state;
        }

        /// <summary>
        /// This function takes in the byte array containing a downloaded file
        /// and the iUpgradeable file and saves the new file to the disk, it also unzips
        /// the file if necessary.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private ErrorState SaveNewFile(iUpgradeable file, Stream data)
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

            string filepath = null, filename = null;
            if (file.SaveFolder != null)
                filepath = file.SaveFolder;
            else if (file.SavePath != null)
                filepath = Path.GetDirectoryName(file.SavePath);
            else
            {
                AddTextToLog("The path to save " + file.Name + " to is invalid.", ImageType.Error);
                return ErrorState.CouldNotSaveNewFile;
            }
            if (file.SavePath != null)
                filename = file.SavePath;
            else
                filename = filepath + @"\" + Path.GetFileName(file.GetLatestVersion().Url);

            try
            {
                if (!Directory.Exists(filepath))
                    Directory.CreateDirectory(filepath);
            }
            catch (IOException)
            {
                AddTextToLog(string.Format("Could not create directory {0}.", filepath), ImageType.Error);
                return ErrorState.CouldNotSaveNewFile;
            }

            if (file.GetLatestVersion().Url.EndsWith(".zip"))
            {
                try
                {
                    ZipFile zipFile = new ZipFile(Path.Combine(MainForm.Instance.Settings.MeGUIUpdateCache, Path.GetFileName(file.GetLatestVersion().Url)));
                    if (zipFile.TestArchive(true) == false)
                    {
                        AddTextToLog("Could not unzip " + file.Name + ". Deleting file. Please run updater again.", ImageType.Error);
                        UpdateCacher.FlushFile(file.GetLatestVersion().Url, this);
                        return ErrorState.CouldNotUnzip;
                    }

                    using (ZipInputStream zip = new ZipInputStream(data))
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
                            if (mainForm.Settings.AlwaysBackUpFiles)
                            {
                                ErrorState result = manageBackups(filename, file.Name, file.NeedsRestartedCopying);
                                if (result != ErrorState.Successful)
                                    return result;
                            }
                            string oldFileName = null;
                            if (file.NeedsRestartedCopying)
                            {
                                oldFileName = filename;
                                filename += ".tempcopy";
                            }
                            // create the output writer to save the file onto the harddisc
                            using (Stream outputWriter = new FileStream(filename, FileMode.Create))
                            {
                                FileUtil.copyData(zip, outputWriter);
                            }
                            File.SetLastWriteTime(filename, zipentry.DateTime);
                            if (file.NeedsRestartedCopying)
                            {
                                mainForm.AddFileToReplace(file.Name, filename, oldFileName, file.GetLatestVersion().UploadDate.ToString(new System.Globalization.CultureInfo("en-us")));
                                file.CurrentVersion.FileVersion = file.GetLatestVersion().FileVersion;
                                needsRestart = true;
                            }
                        }
                        if (!file.NeedsRestartedCopying)
                            file.CurrentVersion = file.GetLatestVersion(); // the current installed version
                        // is now the latest available version
                    }
                }
                catch
                {
                    AddTextToLog("Could not unzip " + file.Name + ". Deleting file. Please run updater again.", ImageType.Error);
                    UpdateCacher.FlushFile(file.GetLatestVersion().Url, this);
                    return ErrorState.CouldNotUnzip;
                }
            }
            else if (file.GetLatestVersion().Url.EndsWith(".7z"))
            {
                try
                {
                    SevenZipExtractor oArchiveCheck = new SevenZipExtractor(data);
                    if (oArchiveCheck.Check() == false)
                    {
                        AddTextToLog("Could not extract " + file.Name + ". Deleting file. Please run updater again.", ImageType.Error);
                        UpdateCacher.FlushFile(file.GetLatestVersion().Url, this);
                        return ErrorState.CouldNotUnzip;
                    }

                    bool bNeedRestartForCopying = false;
                    using (var oArchive = new SevenZipExtractor(data))
                    {
                        oArchive.Extracting += (s, e) =>
                        {
                            SetProgressBar(0, 100, e.PercentDone);
                        };
                        oArchive.FileExists += (o, e) =>
                        {
                            if (mainForm.Settings.AlwaysBackUpFiles)
                            {
                                ErrorState result = manageBackups(e.FileName, file.Name, file.NeedsRestartedCopying);
                                if (result != ErrorState.Successful)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            if (file.NeedsRestartedCopying)
                            {   
                                mainForm.AddFileToReplace(file.Name, e.FileName + ".tempcopy", e.FileName, file.GetLatestVersion().UploadDate.ToString(new System.Globalization.CultureInfo("en-us")));
                                needsRestart = true;
                                e.FileName += ".tempcopy";
                                bNeedRestartForCopying = true;
                            }
                        };
                        oArchive.ExtractArchive(filepath);
                        if (bNeedRestartForCopying == false)
                            file.CurrentVersion = file.GetLatestVersion();  // the current installed version is now the latest available version
                        else
                            file.CurrentVersion.FileVersion = file.GetLatestVersion().FileVersion; // after the restart the new files will be active
                    }
                }
                catch
                {
                    AddTextToLog("Could not unzip " + file.Name + ". Deleting file. Please run updater again.", ImageType.Error);
                    UpdateCacher.FlushFile(file.GetLatestVersion().Url, this);
                    return ErrorState.CouldNotUnzip;
                }
            }
            else
            {
                if (mainForm.Settings.AlwaysBackUpFiles)
                {
                    ErrorState result = manageBackups(filename, file.Name, file.NeedsRestartedCopying);
                    if (result != ErrorState.Successful)
                        return result;
                }
                string oldFileName = null;
                if (file.NeedsRestartedCopying)
                {
                    oldFileName = filename;
                    filename = filename + ".tempcopy";
                }
                try
                {
                    using (Stream output = File.OpenWrite(filename))
                    {
                        
                        //filename, data);
                        if (file.NeedsRestartedCopying)
                        {
                            mainForm.AddFileToReplace(file.Name, filename, oldFileName, file.GetLatestVersion().UploadDate.ToString(new System.Globalization.CultureInfo("en-us")));
                            file.CurrentVersion.FileVersion = file.GetLatestVersion().FileVersion;
                            needsRestart = true;
                        }
                        else
                            file.CurrentVersion = file.GetLatestVersion(); // current installed version
                        // is now the latest available version
                    }
                }
                catch
                {
                    AddTextToLog("Latest version of " + file.Name + " could not be saved to disk. Check there is enough free space.", ImageType.Error);
                    return ErrorState.CouldNotSaveNewFile;
                }
            }
            return ErrorState.Successful;
        }

        private ErrorState manageBackups(string savePath, string name, bool bCopyFile)
        {
            try
            {
                if (File.Exists(savePath + ".backup"))
                    File.Delete(savePath + ".backup");
            }
            catch
            {
                AddTextToLog("Outdated backup version of " + name + " could not be deleted. Check if it is in use.", ImageType.Error);
                return ErrorState.CouldNotRemoveBackup;
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
                AddTextToLog("Old version of " + name + " could not be backed up correctly. Restart MeGUI and try again.", ImageType.Error);
                return ErrorState.CouldNotRenameExistingFile;
            }
            return ErrorState.Successful;
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
                    if (upgradeable.CurrentVersion.FileVersion != null && upgradeable.CurrentVersion.FileVersion.Equals(upgradeable.GetLatestVersion().FileVersion))
                        upgradeable.GetLatestVersion().UploadDate = upgradeable.CurrentVersion.UploadDate;
                }
                if (upgradeable.AllowUpdate && 
                    upgradeable.GetLatestVersion().CompareTo(upgradeable.CurrentVersion) != 0)
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

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
                item.Checked = true;
        }

        private void uncheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            foreach (ListViewItem item in listViewDetails.SelectedItems)
                item.Checked = false;
        }

        private void reinstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewDetails.SelectedItems)
            {
                item.SubItems["Status"].Text = "Reinstalling";
                item.Checked = true;
            }
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
                mainForm.Settings.UpdateFormLocation = this.Location;
        }

        private void UpdateWindow_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                mainForm.Settings.UpdateFormSize = this.ClientSize;
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                mainForm.Settings.UpdateFormSplitter = this.splitContainer2.SplitterDistance;
        }

        private void listViewDetails_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            SaveComponentSettings();
        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.splitContainer1.Size.Height - 65;
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
            UpdateWindow update = new UpdateWindow(info, false);
            update.ShowDialog();
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
        #endregion
}