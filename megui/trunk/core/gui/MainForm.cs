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
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

using MeGUI.core.details;
using MeGUI.core.gui;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

namespace MeGUI
{
    public delegate void UpdateGUIStatusCallback(StatusUpdate su); // catches the UpdateGUI events fired from the encoder
    public enum ProcessingStatus
    {
        DOINGNOTHING, RUNNING, PAUSED, STOPPING
    }

    /// <summary>
    /// MainForm is the main GUI of the program
    /// it contains all the elements required to start encoding and contains the application intelligence as well.
    /// </summary>
    public partial class MainForm : Form
    {
        // This instance is to be used by the serializers that can't be passed a MainForm as a parameter
        public static MainForm Instance;

        #region variable declaration
        private List<string> filesToDeleteOnClosing = new List<string>();
        private List<Form> allForms = new List<Form>();
        private List<Form> formsToReopen = new List<Form>();
        private ITaskbarList3 taskbarItem;
        private Icon taskbarIcon;
        private string strLogFile;
        private Semaphore logLock;
        private int avsLock;
        private LogItem _oneClickLog;
        private LogItem _aVSScriptCreatorLog;
        private LogItem _fileIndexerLog;
        private LogItem _updateLog;
        private LogItem _eac3toLog;
        public bool IsHiddenMode { get { return trayIcon.Visible; } }
        public bool IsOverlayIconActive { get { return taskbarIcon != null; } }
        public string LogFile { get { return strLogFile; } }
        public Semaphore LogLock { get { return logLock; } set { logLock = value; } }
        public int AvsLock { get { return avsLock; } set { avsLock = value; } }
        public LogItem OneClickLog { get { return _oneClickLog; } set { _oneClickLog = value; } }
        public LogItem AVSScriptCreatorLog { get { return _aVSScriptCreatorLog; } set { _aVSScriptCreatorLog = value; } }
        public LogItem FileIndexerLog { get { return _fileIndexerLog; } set { _fileIndexerLog = value; } }
        public LogItem UpdateLog { get { return _updateLog; } set { _updateLog = value; } }
        public LogItem Eac3toLog { get { return _eac3toLog; } set { _eac3toLog = value; } }
        #endregion

        public void RegisterForm(Form f)
        {
        }

        public void DeleteOnClosing(string file)
        {
            filesToDeleteOnClosing.Add(file);
        }

        /// <summary>
        /// launches the megui wiki in the default browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGuide_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://mewiki.project357.com/wiki/Main_Page");
        }
        /// <summary>
        /// launches the encoder gui forum in the default browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem2_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// shows the changelog dialog window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuChangelog_Click(object sender, EventArgs e)
        {
            string strChangeLog = Path.Combine(Application.StartupPath, "changelog.txt");

            if (File.Exists(strChangeLog))
            {
                try
                {
                    Process oProcess = new Process();
                    oProcess.StartInfo.FileName = strChangeLog;
                    oProcess.StartInfo.UseShellExecute = true;
                    oProcess.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(strChangeLog + " cannot be opened:\r\n" + ex.Message, "Process error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(strChangeLog + " not found", "Changelog not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public MainForm()
        {
            // Log File Handling
            string strMeGUILogPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\logs";
            FileUtil.ensureDirectoryExists(strMeGUILogPath);
            strLogFile = strMeGUILogPath + @"\logfile-" + DateTime.Now.ToString("yy'-'MM'-'dd'_'HH'-'mm'-'ss") + ".log";
            logLock = new Semaphore(1, 1);
            avsLock = 0;
            try
            {
                logLock.WaitOne(10000, false);
                File.WriteAllText(strLogFile, "Preliminary log file only. During closing of MeGUI the well formed log file will be written.\r\n\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Log File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            logLock.Release();
            Instance = this;
            constructMeGUIInfo();
            InitializeComponent();
            System.Reflection.Assembly myAssembly = this.GetType().Assembly;
            string name = this.GetType().Namespace + ".";
#if CSC
			name = "";
#endif
            string[] resources = myAssembly.GetManifestResourceNames();
            this.trayIcon.Icon = new Icon(myAssembly.GetManifestResourceStream(name + "App.ico"));
            this.Icon = trayIcon.Icon;
            this.TitleText = Application.ProductName + " " + new System.Version(Application.ProductVersion).Build;
#if x64
            this.TitleText += " x64";
#endif
            if (MainForm.Instance.Settings.AutoUpdate == true && MainForm.Instance.Settings.AutoUpdateServerSubList == 1)
                this.TitleText += " DEVELOPMENT UPDATE SERVER";
            setGUIInfo();
            Jobs.showAfterEncodingStatus(Settings);
            this.videoEncodingComponent1.FileType = MainForm.Instance.Settings.MainFileFormat;

            this.ClientSize = settings.MainFormSize;
            this.Location = settings.MainFormLocation;
            this.splitContainer2.SplitterDistance = (int)(0.42 * (this.splitContainer2.Panel1.Height + this.splitContainer2.Panel2.Height));

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
        }

        #region GUI properties
        public JobControl Jobs
        {
            get { return jobControl1; }
        }
        public bool ProcessStatusChecked
        {
            get { return progressMenu.Checked; }
            set { progressMenu.Checked = value; }
        }
        public VideoEncodingComponent Video
        {
            get { return videoEncodingComponent1; }
        }
        public AudioEncodingComponent Audio
        {
            get { return audioEncodingComponent1; }
        }
        #endregion
        /// <summary>
        /// initializes all the dropdown elements in the GUI to their default values
        /// </summary>

        /// <summary>
        /// handles the GUI closing event
        /// saves all jobs, stops the currently active job and saves all profiles as well
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (jobControl1.IsAnyWorkerEncoding)
            {
                e.Cancel = true; // abort closing
                MessageBox.Show("Please close running jobs before you close MeGUI.", "Job in progress", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (!e.Cancel)
            {
                CloseSilent();
            }
            base.OnClosing(e);
        }
        #region reset
        private void resetButton_Click(object sender, System.EventArgs e)
        {
            videoEncodingComponent1.Reset();
            audioEncodingComponent1.Reset();
        }
        #endregion
        #region auto encoding
        private void autoEncodeButton_Click(object sender, System.EventArgs e)
        {
            RunTool("AutoEncode");
        }

        private void RunTool(string p)
        {
            try
            {
                ITool tool = PackageSystem.Tools[p];
                tool.Run(this);
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Required tool, '" + p + "', not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region job management
        #region I/O verification
        /// <summary>
        /// Test whether a filename is suitable for writing to
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Error message if problem, null if ok</returns>
        public static string verifyOutputFile(string filename)
        {
            try
            {
                filename = Path.GetFullPath(filename);  // this will throw ArgumentException if invalid
                if (File.Exists(filename))
                {
                    FileStream fs = File.OpenWrite(filename);  // this will throw if we'll have problems writing
                    fs.Close();
                }
                else
                {
                    FileStream fs = File.Create(filename);  // this will throw if we'll have problems writing
                    fs.Close();
                    File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        /// <summary>
        /// Test whether a filename is suitable for reading from
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Error message if problem, null if ok</returns>
        public static string verifyInputFile(string filename)
        {
            try
            {
                filename = Path.GetFullPath(filename);  // this will throw ArgumentException if invalid
                FileStream fs = File.OpenRead(filename);  // this will throw if we'll have problems reading
                fs.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
        #endregion
        #endregion
        #region settings
        /// <summary>
        /// saves the global GUI settings to settings.xml
        /// </summary>
        public void saveSettings()
        {
            XmlSerializer ser = null;
            string fileName = this.path + @"\settings.xml";
            using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    ser = new XmlSerializer(typeof(MeGUISettings));
                    ser.Serialize(s, this.settings);
                }
                catch (Exception e)
                {
                    LogItem _oLog = MainForm.Instance.Log.Info("Error");
                    _oLog.LogValue("saveSettings", e, ImageType.Error);
                }
            }
        }
        /// <summary>
        /// loads the global settings
        /// </summary>
        public void loadSettings()
        {
            string fileName = Path.Combine(path, "settings.xml");
            if (File.Exists(fileName))
            {
                XmlSerializer ser = null;
                using (Stream s = File.OpenRead(fileName))
                {
                    ser = new XmlSerializer(typeof(MeGUISettings));
                    try
                    {
                        this.settings = (MeGUISettings)ser.Deserialize(s);
                    }
                    catch
                    {
                        MessageBox.Show("MeGUI settings could not be loaded. Default values will be applied now.", "Error loading MeGUI settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion
        #region GUI updates
        #region job postprocessing
        #endregion
        #region helper methods

        public JobUtil JobUtil
        {
            get { return jobUtil; }
        }

        public string TitleText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
                trayIcon.Text = value;
            }
        }

        /// <summary>
        /// shuts down the PC if the shutdown option is set
        /// also saves all profiles, jobs and the log as MeGUI is killed
        /// via the shutdown so the appropriate methods in the OnClosing are not called
        /// </summary>
        public void runAfterEncodingCommands()
        {
            if (Jobs.CurrentAfterEncoding == AfterEncoding.DoNothing) return;
            this.profileManager.SaveProfiles();
            this.saveSettings();
            jobControl1.saveJobs();
            this.saveLog();

            if (Jobs.CurrentAfterEncoding == AfterEncoding.Shutdown)
            {
                using (CountdownWindow countdown = new CountdownWindow(30))
                {
                    if (countdown.ShowDialog() == DialogResult.OK)
                    {
                        bool succ = Shutdown.shutdown();
                        if (!succ)
                            Log.LogEvent("Tried and failed to shut down system");
                        else
                            Log.LogEvent("Shutdown initiated");
                    }
                    else
                        Log.LogEvent("User aborted shutdown");

                }
            }
            else if (Jobs.CurrentAfterEncoding == AfterEncoding.CloseMeGUI)
            {
                this.CloseSilent();
                Application.Exit();
            }
            else if (Jobs.CurrentAfterEncoding == AfterEncoding.RunCommand && !String.IsNullOrEmpty(settings.AfterEncodingCommand))
            {
                string filename = MeGUIPath + @"\after_encoding.bat";
                try
                {
                    using (StreamWriter s = new StreamWriter(File.OpenWrite(filename)))
                    {
                        s.WriteLine(settings.AfterEncodingCommand);
                    }
                    ProcessStartInfo psi = new ProcessStartInfo(filename);
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    Process p = new Process();
                    p.StartInfo = psi;
                    p.Start();
                }
                catch (IOException e) { MessageBox.Show("Error when attempting to run command: " + e.Message, "Run command failed", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        public LogItem Log
        {
            get { return logTree1.Log; }
        }

        /// <summary>
        /// saves the whole content of the log into a logfile
        /// </summary>
        public void saveLog()
        {
            string text = Log.ToString();
            try
            {
                logLock.WaitOne(10000, false);
                File.WriteAllText(strLogFile, text);
            }
            catch (Exception)
            {
                MessageBox.Show("Log file cannot be saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                logLock.Release();
            }
        }
        private void exitMeGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// returns the profile manager to whomever might require it
        /// </summary>
        public ProfileManager Profiles
        {
            get
            {
                return this.profileManager;
            }
        }
        #endregion
        #region menu actions
        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "All files|*.*";
            openFileDialog.Title = "Select your input file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                openFile(openFileDialog.FileName, false);
        }
        private void mnuViewMinimizeToTray_Click(object sender, EventArgs e)
        {
            formsToReopen.Clear();
            this.Visible = false;
            trayIcon.Visible = true;
        }

        private void mnuFileExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void mnuToolsSettings_Click(object sender, System.EventArgs e)
        {
            using (SettingsForm sform = new SettingsForm())
            {
                sform.Settings = this.settings;
                if (sform.ShowDialog() == DialogResult.OK)
                {
                    this.settings = sform.Settings;
                    this.saveSettings();
                    Jobs.showAfterEncodingStatus(settings);
                }
            }
        }
        private void mnuTool_Click(object sender, System.EventArgs e)
        {
            if ((!(sender is System.Windows.Forms.MenuItem)) || (!((sender as MenuItem).Tag is ITool)))
                return;
            ((ITool)(sender as MenuItem).Tag).Run(this);
        }

        private void mnuOptions_Click(object sender, System.EventArgs e)
        {
            if ((!(sender is System.Windows.Forms.MenuItem)) || (!((sender as MenuItem).Tag is IOption)))
                return;
            ((IOption)(sender as MenuItem).Tag).Run(this);
        }

        private void mnuMuxer_Click(object sender, System.EventArgs e)
        {
            if ((!(sender is System.Windows.Forms.MenuItem)) || (!((sender as MenuItem).Tag is IMuxing)))
                return;
            MuxWindow mw = new MuxWindow((IMuxing)((sender as MenuItem).Tag), this);
            mw.Show();
        }

        private void mnuView_Popup(object sender, System.EventArgs e)
        {
            List<Pair<string, bool>> workers = Jobs.ListProgressWindows();
            progressMenu.MenuItems.Clear();
            progressMenu.MenuItems.Add(showAllProgressWindows);
            progressMenu.MenuItems.Add(hideAllProgressWindows);
            progressMenu.MenuItems.Add(separator2);

            foreach (Pair<string, bool> p in workers)
            {
                MenuItem i = new MenuItem(p.fst);
                i.Checked = p.snd;
                i.Click += new EventHandler(mnuProgress_Click);
                progressMenu.MenuItems.Add(i);
            }

            if (workers.Count == 0)
            {
                MenuItem i = new MenuItem("(No progress windows to show)");
                i.Enabled = false;
                progressMenu.MenuItems.Add(i);
            }
        }

        void mnuProgress_Click(object sender, EventArgs e)
        {
            MenuItem i = (MenuItem)sender;
            if (i.Checked)
                Jobs.HideProgressWindow(i.Text);
            else
                Jobs.ShowProgressWindow(i.Text);
        }
        private void mnuViewProcessStatus_Click(object sender, System.EventArgs e)
        {
        }

        #endregion


        public MeGUISettings Settings
        {
            get { return settings; }
        }

        public MediaFileFactory MediaFileFactory
        {
            get { return mediaFileFactory; }
        }
        #region tray action
        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Activate the form.
            this.Show(); this.Activate();

            if (progressMenu.Checked)
                Jobs.ShowAllProcessWindows();
            trayIcon.Visible = false;
        }
        private void openMeGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            this.Visible = true;
        }

        #endregion
        #region file opening
        private void openOtherVideoFile(string fileName)
        {
            AviSynthWindow asw = new AviSynthWindow(this, fileName);
            asw.OpenScript += new OpenScriptCallback(Video.openVideoFile);
            asw.Show();
        }
        private void openIndexableFile(string fileName)
        {
            if (DialogManager.useOneClick())
            {
                OneClickWindow ocmt = new OneClickWindow(this);
                ocmt.setInput(fileName);
                ocmt.ShowDialog();
            }
            else
            {
                FileIndexerWindow mpegInput = new FileIndexerWindow(this);
                mpegInput.setConfig(fileName, null, 2, true, true, true, false);
                mpegInput.Show();
            }
        }
        public bool openFile(string file, bool openVideo)
        {
            if (Path.GetExtension(file.ToLowerInvariant()).Equals(".zip"))
            {
                importProfiles(file);
                return false;
            }

            MediaInfoFile iFile = new MediaInfoFile(file);
            if (iFile.HasVideo)
            {
                if (iFile.isD2VIndexable() || iFile.isDGIIndexable() || iFile.isDGAIndexable() || iFile.isFFMSIndexable())
                {
                    openIndexableFile(file);
                }
                else
                {
                    if (iFile.HasAudio)
                        audioEncodingComponent1.openAudioFile(file);
                    if (iFile.ContainerFileTypeString.Equals("AVS"))
                    {
                        Video.openVideoFile(file);
                        if (openVideo)
                            return true;
                    }
                    else
                        openOtherVideoFile(file);
                }
            }
            else if (iFile.HasAudio)
                audioEncodingComponent1.openAudioFile(file);
            else if (Path.GetExtension(iFile.FileName).ToLowerInvariant().Equals(".avs"))
            {
                try
                {
                    using (AvsFile avi = AvsFile.OpenScriptFile(iFile.FileName))
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error parsing avs file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("This file cannot be opened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void importProfiles(string file)
        {
            new ProfileImporter(this, file, false).ShowDialog();
        }
        #endregion
        #region Drag 'n' Drop
        private void MeGUI_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            Invoke(new MethodInvoker(delegate
            {
                openFile(files[0], false);
            }));
            this.tabControl1.SelectedIndex = 0;
        }

        private void MeGUI_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (files.Length > 0)
                    e.Effect = DragDropEffects.All;
            }
        }
        #endregion
        #region importing
        public void importProfiles(Stream data)
        {
            Util.ThreadSafeRun(this, delegate
            {
                ProfileImporter importer = new ProfileImporter(this, data, true);
                if (importer.ErrorDuringInit())
                    return;
                if (MainForm.Instance.settings.AutoUpdateSession)
                {
                    importer.AutoImport();
                }
                else
                {
                    importer.Show();
                    while (importer.Visible == true)    // wait until the profiles have been imported
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
            });
        }

        private bool bImportProfileSuccessful = false;
        public bool ImportProfileSuccessful
        {
            get { return bImportProfileSuccessful; }
            set { bImportProfileSuccessful = value; }
        }

        private void mnuFileImport_Click(object sender, EventArgs e)
        {
            try
            {
                new ProfileImporter(this, false).ShowDialog();
            }
            catch (CancelledException) { }
        }

        private void mnuFileExport_Click(object sender, EventArgs e)
        {
            new ProfileExporter(this).ShowDialog();
        }
        #endregion

        private void mnuToolsAdaptiveMuxer_Click(object sender, EventArgs e)
        {
            AdaptiveMuxWindow amw = new AdaptiveMuxWindow(this);
            amw.Show();
        }

        private void MeGUI_Load(object sender, EventArgs e)
        {
            RegisterForm(this);
        }

        private void beginUpdateCheck()
        {
#if x86
            string strLocalUpdateXML = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "upgrade.xml");
#endif
#if x64
            string strLocalUpdateXML = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "upgrade_x64.xml");
#endif
            if (File.Exists(strLocalUpdateXML))
                MainForm.Instance.Settings.AutoUpdateSession = true;

            UpdateWindow update = new UpdateWindow(this, this.Settings, false);
            update.GetUpdateData(true);
            bool bIsComponentMissing = UpdateWindow.isComponentMissing();
            if (bIsComponentMissing || update.HasUpdatableFiles()) // If there are updated or missing files, display the window
            {
                if (MainForm.Instance.Settings.AutoUpdateSession)
                {
                    update.Visible = true;
                    update.StartAutoUpdate();
                    while (update.Visible == true)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
                else
                {
                    if (bIsComponentMissing)
                    {
                        if (AskToInstallComponents(filesToReplace.Keys.Count > 0) == true)
                        {
                            if (filesToReplace.Keys.Count > 0) // restart required
                            {
                                this.Restart = true;
                                this.Invoke(new MethodInvoker(delegate { this.Close(); }));
                                return;
                            }
                        }
                        else
                            return;
                    }
                    if (MessageBox.Show("There are updated files available that may be necessary for MeGUI to work correctly. Some of them are binary files subject to patents, so they could be in violation of your local laws if you live e.g. in US, Japan and some countries in Europe. MeGUI will let you choose what files to update but please check your local laws about patents before proceeding. By clicking on the 'Yes' button you declare you have read this warning. Do you wish to proceed reviewing the updates?",
                            "Updates Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        update.ShowDialog();
                }
                if (UpdateWindow.isComponentMissing() && !this.Restart)
                {
                    if (AskToInstallComponents(filesToReplace.Keys.Count > 0) == true)
                    {
                        if (filesToReplace.Keys.Count > 0) // restart required
                        {
                            this.Restart = true;
                            this.Invoke(new MethodInvoker(delegate { this.Close(); }));
                            return;
                        }
                        else
                            beginUpdateCheck();
                    }
                }
            }
            else
            {
                if (File.Exists(strLocalUpdateXML))
                    File.Delete(strLocalUpdateXML);
            }
        }

        private void mnuUpdate_Click(object sender, EventArgs e)
        {
            UpdateWindow update = new UpdateWindow(this, this.Settings, false);
            update.ShowDialog();
        }

        internal void AddFileToReplace(string iUpgradeableName, string tempFilename, string filename, string newUploadDate)
        {
            CommandlineUpgradeData data = new CommandlineUpgradeData();
            data.filename.Add(filename);
            data.tempFilename.Add(tempFilename);
            data.newUploadDate = newUploadDate;
            if (filesToReplace.ContainsKey(iUpgradeableName))
            {
                filesToReplace[iUpgradeableName].tempFilename.Add(tempFilename);
                filesToReplace[iUpgradeableName].filename.Add(filename);
                return;
            }
            filesToReplace.Add(iUpgradeableName, data);
        }

        internal void CloseSilent()
        {
            this.profileManager.SaveProfiles();
            this.saveSettings();
            this.saveApplicationSettings();
            jobControl1.saveJobs();
            this.saveLog();
            deleteFiles();
            this.runRestarter();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            MeGUI.Properties.Settings.Default.Save();
        }

        private void saveApplicationSettings()
        {
        }

        private void deleteFiles()
        {
            foreach (string file in filesToDeleteOnClosing)
            {
                try
                {
                    FileUtil.DeleteDirectoryIfExists(file, true);
                    if (File.Exists(file)) File.Delete(file);
                }
                catch { }
            }
        }

        #region MeGUIInfo
        #region variable declaration
        private bool restart = false;
        private Dictionary<string, CommandlineUpgradeData> filesToReplace = new Dictionary<string, CommandlineUpgradeData>();
        private DialogManager dialogManager;
        private string path; // path the program was started from
        private MediaFileFactory mediaFileFactory;
        private PackageSystem packageSystem = new PackageSystem();
        private JobUtil jobUtil;
        private MuxProvider muxProvider;
        private MeGUISettings settings = new MeGUISettings();
        private ProfileManager profileManager;

        //public MeGUISettings Settings
        //{
        //    get { return settings; }
        //}

        public MuxProvider MuxProvider
        {
            get { return muxProvider; }
        }

        private CodecManager codecs;
        #endregion
        #region start and end
        public void handleCommandline(CommandlineParser parser)
        {
            foreach (string file in parser.failedUpgrades)
                System.Windows.Forms.MessageBox.Show("Failed to upgrade '" + file + "'.", "Upgrade failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (parser.upgradeData.Count > 0)
            {
                UpdateWindow update = new UpdateWindow(this, Settings, false);
                foreach (string file in parser.upgradeData.Keys)
                    update.UpdateUploadDate(file, parser.upgradeData[file]);
                update.SaveSettings();
            }
        }

        public void setGUIInfo()
        {
            fillMenus();
            jobControl1.MainForm = this;
            jobControl1.loadJobs();

        }

        /// <summary>
        /// default constructor
        /// initializes all the GUI components, initializes the internal objects and makes a default selection for all the GUI dropdowns
        /// In addition, all the jobs and profiles are being loaded from the harddisk
        /// </summary>
        public void constructMeGUIInfo()
        {
            muxProvider = new MuxProvider(this);
            this.codecs = new CodecManager();
            this.path = System.Windows.Forms.Application.StartupPath;
            this.jobUtil = new JobUtil(this);
            this.settings = new MeGUISettings();
            addPackages();
            this.profileManager = new ProfileManager(this.path);
            this.profileManager.LoadProfiles();
            this.mediaFileFactory = new MediaFileFactory(this);
            this.loadSettings();
            this.dialogManager = new DialogManager(this);
        }

        private void fillMenus()
        {
            // Fill the muxing menu
            mnuMuxers.MenuItems.Clear();
            mnuToolsAdaptiveMuxer.Shortcut = Shortcut.Ctrl1;
            mnuMuxers.MenuItems.Add(mnuToolsAdaptiveMuxer);
            int index = 1;
            foreach (IMuxing muxer in PackageSystem.MuxerProviders.Values)
            {
                MenuItem newMenuItem = new MenuItem();
                newMenuItem.Text = muxer.Name;
                newMenuItem.Tag = muxer;
                newMenuItem.Index = index;
                newMenuItem.Shortcut = muxer.Shortcut;
                index++;
                mnuMuxers.MenuItems.Add(newMenuItem);
                newMenuItem.Click += new System.EventHandler(this.mnuMuxer_Click);
            }

            // Fill the tools menu
            mnuTools.MenuItems.Clear();
            List<MenuItem> toolsItems = new List<MenuItem>();
            List<Shortcut> usedShortcuts = new List<Shortcut>();
            toolsItems.Add(mnutoolsD2VCreator);
            toolsItems.Add(mnuMuxers);
            usedShortcuts.Add(mnuMuxers.Shortcut);

            foreach (ITool tool in PackageSystem.Tools.Values)
            {
                if (tool.Name != "File Indexer")
                {
                    MenuItem newMenuItem = new MenuItem();
                    newMenuItem.Text = tool.Name;
                    newMenuItem.Tag = tool;
                    newMenuItem.Click += new System.EventHandler(this.mnuTool_Click);
                    bool shortcutAttempted = false;
                    foreach (Shortcut s in tool.Shortcuts)
                    {
                        shortcutAttempted = true;
                        Debug.Assert(s != Shortcut.None);
                        if (!usedShortcuts.Contains(s))
                        {
                            usedShortcuts.Add(s);
                            newMenuItem.Shortcut = s;
                            break;
                        }
                    }

                    if (shortcutAttempted && newMenuItem.Shortcut == Shortcut.None)
                        Log.Warn("Shortcut for '" + tool.Name + "' is already used. No shortcut selected.");
                    toolsItems.Add(newMenuItem);
                }
            }

            toolsItems.Sort(new Comparison<MenuItem>(delegate(MenuItem a, MenuItem b) { return (a.Text.CompareTo(b.Text)); }));
            index = 0;
            foreach (MenuItem m in toolsItems)
            {
                m.Index = index;
                index++;
                mnuTools.MenuItems.Add(m);
            }

            // Fill the Options Menu
            mnuOptions.MenuItems.Clear();
            List<MenuItem> optionsItems = new List<MenuItem>();
            List<Shortcut> usedShortcuts2 = new List<Shortcut>();
            optionsItems.Add(mnuOptionsSettings);
            usedShortcuts2.Add(mnuOptionsSettings.Shortcut);
            foreach (IOption option in PackageSystem.Options.Values)
            {
                MenuItem newMenuItem = new MenuItem();
                newMenuItem.Text = option.Name;
                newMenuItem.Tag = option;
                newMenuItem.Click += new System.EventHandler(this.mnuOptions_Click);
                bool shortcutAttempted = false;
                foreach (Shortcut s in option.Shortcuts)
                {
                    shortcutAttempted = true;
                    Debug.Assert(s != Shortcut.None);
                    if (!usedShortcuts.Contains(s))
                    {
                        usedShortcuts2.Add(s);
                        newMenuItem.Shortcut = s;
                        break;
                    }
                }
                if (shortcutAttempted && newMenuItem.Shortcut == Shortcut.None)
                    Log.Warn("Shortcut for '" + option.Name + "' is already used. No shortcut selected.");
                optionsItems.Add(newMenuItem);
            }

            optionsItems.Sort(new Comparison<MenuItem>(delegate(MenuItem a, MenuItem b) { return (a.Text.CompareTo(b.Text)); }));
            index = 0;
            foreach (MenuItem m in optionsItems)
            {
                m.Index = index;
                index++;
                mnuOptions.MenuItems.Add(m);
            }
        }

        private void addPackages()
        {
            PackageSystem.JobProcessors.Register(AviSynthAudioEncoder.Factory);

            PackageSystem.JobProcessors.Register(ffmpegEncoder.Factory);
            PackageSystem.JobProcessors.Register(x264Encoder.Factory);
            PackageSystem.JobProcessors.Register(XviDEncoder.Factory);

            PackageSystem.JobProcessors.Register(MkvMergeMuxer.Factory);
            PackageSystem.JobProcessors.Register(MP4BoxMuxer.Factory);
            PackageSystem.JobProcessors.Register(AMGMuxer.Factory);
            PackageSystem.JobProcessors.Register(tsMuxeR.Factory);

            PackageSystem.JobProcessors.Register(MkvExtract.Factory);
            PackageSystem.JobProcessors.Register(PgcDemux.Factory);
            PackageSystem.JobProcessors.Register(OneClickPostProcessing.Factory);
            PackageSystem.JobProcessors.Register(CleanupJobRunner.Factory);

            PackageSystem.JobProcessors.Register(AviSynthProcessor.Factory);
            PackageSystem.JobProcessors.Register(D2VIndexer.Factory);
            PackageSystem.JobProcessors.Register(DGAIndexer.Factory);
            PackageSystem.JobProcessors.Register(DGIIndexer.Factory);
            PackageSystem.JobProcessors.Register(FFMSIndexer.Factory);
            PackageSystem.JobProcessors.Register(VobSubIndexer.Factory);
            PackageSystem.JobProcessors.Register(MeGUI.packages.tools.besplitter.Joiner.Factory);
            PackageSystem.JobProcessors.Register(MeGUI.packages.tools.besplitter.Splitter.Factory);
            PackageSystem.JobProcessors.Register(HDStreamExtractorIndexer.Factory);
            PackageSystem.MuxerProviders.Register(new AVIMuxGUIMuxerProvider());
            PackageSystem.MuxerProviders.Register(new TSMuxerProvider());
            PackageSystem.MuxerProviders.Register(new MKVMergeMuxerProvider());
            PackageSystem.MuxerProviders.Register(new MP4BoxMuxerProvider());
            PackageSystem.Tools.Register(new MeGUI.packages.tools.cutter.CutterTool());
            PackageSystem.Tools.Register(new AviSynthWindowTool());
            PackageSystem.Tools.Register(new AutoEncodeTool());
            PackageSystem.Tools.Register(new CQMEditorTool());
            PackageSystem.Tools.Register(new CalculatorTool());
            PackageSystem.Tools.Register(new ChapterCreatorTool());
            PackageSystem.Options.Register(new UpdateOptions());
            PackageSystem.Tools.Register(new MeGUI.packages.tools.besplitter.BesplitterTool());
            PackageSystem.Tools.Register(new OneClickTool());
            PackageSystem.Tools.Register(new D2VCreatorTool());
            PackageSystem.Tools.Register(new AVCLevelTool());
            PackageSystem.Tools.Register(new VobSubTool());
            PackageSystem.Tools.Register(new MeGUI.packages.tools.hdbdextractor.HdBdExtractorTool());
            PackageSystem.MediaFileTypes.Register(new AvsFileFactory());
            PackageSystem.MediaFileTypes.Register(new d2vFileFactory());
            PackageSystem.MediaFileTypes.Register(new dgaFileFactory());
            PackageSystem.MediaFileTypes.Register(new dgiFileFactory());
            PackageSystem.MediaFileTypes.Register(new ffmsFileFactory());
            PackageSystem.MediaFileTypes.Register(new MediaInfoFileFactory());
            PackageSystem.JobPreProcessors.Register(BitrateCalculatorPreProcessor.CalculationProcessor);
            PackageSystem.JobPostProcessors.Register(d2vIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(dgaIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(dgiIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(ffmsIndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(CleanupJobRunner.DeleteIntermediateFilesPostProcessor);
            PackageSystem.JobConfigurers.Register(MuxWindow.Configurer);
            PackageSystem.JobConfigurers.Register(AudioEncodingWindow.Configurer);
        }

        private static Mutex mySingleInstanceMutex = new Mutex(true, "MeGUI_mutex");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Parse for the need to run the program elevated
            Boolean bRunElevated = false, bForceAdmin = false;
            foreach (string strParam in args)
            {
                if (strParam.Equals("-elevate"))
                    bRunElevated = true;
                else if (strParam.Equals("-forceadmin"))
                    bForceAdmin = true;
            }

            // Check if the program can write to the program dir
            if (!FileUtil.IsDirWriteable(Path.GetDirectoryName(Application.ExecutablePath)))
                bForceAdmin = true;

            // If needed run as elevated process
            if (bForceAdmin && !bRunElevated)
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = Application.ExecutablePath;
                    p.StartInfo.Arguments = "-elevate";
                    p.StartInfo.Verb = "runas";
                    p.Start();
                    return;
                }
                catch
                {
                }
            }

            System.Windows.Forms.Application.EnableVisualStyles();
#if !DEBUG
            if (!mySingleInstanceMutex.WaitOne(0, false))
            {
                if (DialogResult.Yes != MessageBox.Show("Running MeGUI instance detected!\n\rThere's not really much point in running multiple copies of MeGUI, and it can cause problems.\n\rDo You still want to run yet another MeGUI instance?", "Running MeGUI instance detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    return;
            }
#endif
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            CommandlineParser parser = new CommandlineParser();
            parser.Parse(args);
            MainForm mainForm = new MainForm();
            mainForm.handleCommandline(parser);
            if (parser.start)
                Application.Run(mainForm);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException((Exception)e.ExceptionObject);
        }

        static void HandleUnhandledException(Exception e)
        {
            LogItem i = MainForm.Instance.Log.Error("Unhandled error");
            i.LogValue("Exception message", e.Message);
            i.LogValue("Stacktrace", e.StackTrace);
            i.LogValue("Inner exception", e.InnerException);
            foreach (System.Collections.DictionaryEntry info in e.Data)
                i.LogValue(info.Key.ToString(), info.Value);

            MessageBox.Show("MeGUI encountered a fatal error and may not be able to proceed. Reason: " + e.Message
                , "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
        }

        private void runRestarter()
        {
            if (filesToReplace.Keys.Count == 0 /*&& otherFilesToInstall.Count == 0*/)
                return;
            Process proc = new Process();
            ProcessStartInfo pstart = new ProcessStartInfo();
            pstart.FileName = Path.Combine(Application.StartupPath, "updatecopier.exe");
            foreach (string file in filesToReplace.Keys)
            {
                pstart.Arguments += string.Format("--component \"{0}\" \"{1}\" ", file, filesToReplace[file].newUploadDate);
                for (int i = 0; i < filesToReplace[file].filename.Count; i++)
                {
                    pstart.Arguments += string.Format("\"{0}\" \"{1}\" ",
                       filesToReplace[file].filename[i],
                       filesToReplace[file].tempFilename[i]);
                }
            }
            if (restart)
                pstart.Arguments += "--restart";
            else
                pstart.Arguments += "--no-restart";

            // Check if the program can write to the program dir
            if (FileUtil.IsDirWriteable(Path.GetDirectoryName(Application.ExecutablePath)))
            {
                pstart.CreateNoWindow = true;
                pstart.UseShellExecute = false;
            }
            else
            {
                // Need admin permissions
                proc.StartInfo.Verb = "runas";
                pstart.UseShellExecute = true;
            }

            proc.StartInfo = pstart;
            try
            {
                if (!proc.Start())
                    MessageBox.Show("Couldn't run updater.", "Couldn't run updater.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("Couldn't run updater.", "Couldn't run updater.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region properties
        public PackageSystem PackageSystem
        {
            get { return packageSystem; }
        }
        public bool Restart
        {
            get { return restart; }
            set { restart = value; }
        }
        public DialogManager DialogManager
        {
            get { return dialogManager; }
        }
        /// <summary>
        /// gets the path from where MeGUI was launched
        /// </summary>
        public string MeGUIPath
        {
            get { return this.path; }
        }
        #endregion
        #endregion

        internal void ClosePlayer()
        {
            videoEncodingComponent1.ClosePlayer();
        }

        internal void hidePlayer()
        {
            videoEncodingComponent1.hidePlayer();
        }

        internal void showPlayer()
        {
            videoEncodingComponent1.showPlayer();
        }


        private void showAllWorkers_Click(object sender, EventArgs e)
        {
            Jobs.ShowAllWorkers();
        }

        private void hideAllWorkers_Click(object sender, EventArgs e)
        {
            Jobs.HideAllWorkers();
        }

        private void showAllWorkers_Popup(object sender, EventArgs e)
        {
            viewSummary.Checked = Jobs.SummaryVisible;

            List<Pair<string, bool>> workers = Jobs.ListWorkers();
            workersMenu.MenuItems.Clear();
            workersMenu.MenuItems.Add(showAllWorkers);
            workersMenu.MenuItems.Add(hideAllWorkers);
            workersMenu.MenuItems.Add(separator);

            foreach (Pair<string, bool> p in workers)
            {
                MenuItem i = new MenuItem(p.fst);
                i.Checked = p.snd;
                i.Click += new EventHandler(mnuWorker_Click);
                workersMenu.MenuItems.Add(i);
            }

            if (workers.Count == 0)
            {
                MenuItem i = new MenuItem("(No workers to show)");
                i.Enabled = false;
                workersMenu.MenuItems.Add(i);
            }
        }

        void mnuWorker_Click(object sender1, EventArgs e)
        {
            MenuItem sender = (MenuItem)sender1;
            Jobs.SetWorkerVisible(sender.Text, !sender.Checked);
        }

        private void viewSummary_Click(object sender, EventArgs e)
        {
            if (viewSummary.Checked)
            {
                viewSummary.Checked = false;
                Jobs.HideSummary();
            }
            else
            {
                viewSummary.Checked = true;
                Jobs.ShowSummary();
            }
        }

        private void createNewWorker_Click(object sender, EventArgs e)
        {
            Jobs.RequestNewWorker();
        }

        private void showAllProgressWindows_Click(object sender, EventArgs e)
        {
            Jobs.ShowAllProcessWindows();
        }

        private void hideAllProgressWindows_Click(object sender, EventArgs e)
        {
            Jobs.HideAllProcessWindows();
        }

        private void mnuForum_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.doom9.org/forumdisplay.php?f=78");
        }

        private void mnuHome_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/megui");
        }

        private void mnuBugTracker_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/tracker/?group_id=156112&atid=798476");
        }

        private void mnuFeaturesReq_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/tracker/?group_id=156112&atid=798479");
        }

        private void mnuDoc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://mewiki.project357.com/wiki/Main_Page");
        }

        private void mnuOptionsSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm sform = new SettingsForm())
            {
                sform.Settings = this.settings;
                if (sform.ShowDialog() == DialogResult.OK)
                {
                    this.settings = sform.Settings;
                    this.saveSettings();
                    Jobs.showAfterEncodingStatus(settings);
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.ClientSize = settings.MainFormSize;

            if ((Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1) || Environment.OSVersion.Version.Major > 6)
                taskbarItem = (ITaskbarList3)new ProgressTaskbar();

            getVersionInformation();

            if (_updateLog == null)
                _updateLog = Log.Info("Update detection");
            if (settings.AutoUpdate)
                startUpdateCheck();
            else
                _updateLog.LogEvent("Automatic update is disabled");

            if (settings.AutoStartQueueStartup)
                jobControl1.StartAll(false);
        }

        public void startUpdateCheck()
        {
            // Need a seperate thread to run the updater to stop internet lookups from freezing the app.
            Thread updateCheck = new Thread(new ThreadStart(beginUpdateCheck));
            updateCheck.IsBackground = true;
            updateCheck.Start();
        }

        private void getVersionInformation()
        {
            LogItem i = Log.Info("Versions");
#if x86
            i.LogValue("MeGUI", new System.Version(Application.ProductVersion).Build);
#endif
#if x64
            i.LogValue("MeGUI Version ", new System.Version(Application.ProductVersion).Build + " x64");
#endif
            i.LogValue("Operating System", string.Format("{0}{1} ({2}.{3}.{4}.{5})", OSInfo.GetOSName(), OSInfo.GetOSServicePack(), OSInfo.OSMajorVersion, OSInfo.OSMinorVersion, OSInfo.OSRevisionVersion, OSInfo.OSBuildVersion));
            i.LogValue(".Net Framework", string.Format("{0}", OSInfo.DotNetVersionFormated(OSInfo.FormatDotNetVersion())));

            VideoUtil.getAvisynthVersion(i);

            string devil = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "devil.dll");
            if (File.Exists(devil))
                FileUtil.GetFileInformation("DevIL", devil, ref i);
            FileUtil.GetFileInformation("AvisynthWrapper", Path.GetDirectoryName(Application.ExecutablePath) + @"\AvisynthWrapper.dll", ref i);

            // check if Haali Matroska Splitter is properly installed
            try
            {
                // A28F324B-DDC5-4999-AA25-D3A7E25EF7A8 = Haali Matroska Splitter x86
                // 55DA30FC-F16B-49FC-BAA5-AE59FC65F82D = Haali Matroska Splitter x64
#if x86
                Type comtype = Type.GetTypeFromCLSID(new Guid("A28F324B-DDC5-4999-AA25-D3A7E25EF7A8"));
                string fileName = "splitter.ax";
#endif
#if x64
                Type comtype = Type.GetTypeFromCLSID(new Guid("55DA30FC-F16B-49FC-BAA5-AE59FC65F82D"));
                string fileName = "splitter.x64.ax";
#endif
                object comobj = Activator.CreateInstance(comtype);
                FileUtil.GetFileInformation("Haali Matroska Splitter", Path.Combine(MeGUISettings.HaaliMSPath, fileName), ref i);
            }
            catch (Exception)
            {
                i.LogEvent("Haali Matroska Splitter not installed properly.", ImageType.Warning);
                i.LogEvent("Therefore DSS2() and certain functions of FFVideoSource() and the HD Streams Extractor may also not work.", ImageType.Warning);
                i.LogEvent("Please download and install it from http://haali.su/mkv/", ImageType.Warning);
            }

            FileUtil.GetFileInformation("Haali DSS2", Path.Combine(MeGUISettings.HaaliMSPath, "avss.dll"), ref i);
            FileUtil.GetFileInformation("ICSharpCode.SharpZipLib", Path.GetDirectoryName(Application.ExecutablePath) + @"\ICSharpCode.SharpZipLib.dll", ref i);
            FileUtil.GetFileInformation("LinqBridge", Path.GetDirectoryName(Application.ExecutablePath) + @"\LinqBridge.dll", ref i);
            FileUtil.GetFileInformation("MediaInfo", Path.GetDirectoryName(Application.ExecutablePath) + @"\MediaInfo.dll", ref i);
            FileUtil.GetFileInformation("MediaInfoWrapper", Path.GetDirectoryName(Application.ExecutablePath) + @"\MediaInfoWrapper.dll", ref i);
            FileUtil.GetFileInformation("MessageBoxExLib", Path.GetDirectoryName(Application.ExecutablePath) + @"\MessageBoxExLib.dll", ref i);
            FileUtil.GetFileInformation("SevenZipSharp", Path.GetDirectoryName(Application.ExecutablePath) + @"\SevenZipSharp.dll", ref i);
            FileUtil.GetFileInformation("7z", Path.GetDirectoryName(Application.ExecutablePath) + @"\7z.dll", ref i);
        }

        public void setOverlayIcon(Icon oIcon)
        {
            if ((Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0)
                || Environment.OSVersion.Version.Major < 6)
                return;

            if (oIcon == null)
            {
                // remove the overlay icon
                Util.ThreadSafeRun(this, delegate { taskbarItem.SetOverlayIcon(this.Handle, IntPtr.Zero, null); });
                taskbarIcon = null;
                return;
            }

            if (taskbarIcon != null && oIcon.Handle == taskbarIcon.Handle)
                return;

            if (oIcon == System.Drawing.SystemIcons.Warning && taskbarIcon == System.Drawing.SystemIcons.Error)
                return;

            if (taskbarItem != null)
            {
                Util.ThreadSafeRun(this, delegate { taskbarItem.SetOverlayIcon(this.Handle, oIcon.Handle, null); });
                taskbarIcon = oIcon;
            }
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

        private void OneClickEncButton_Click(object sender, EventArgs e)
        {
            RunTool("one_click");
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://mewiki.project357.com/wiki/Main_Page");
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            FileIndexerWindow d2vc = new FileIndexerWindow(this);
            d2vc.Show();
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                settings.MainFormLocation = this.Location;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true)
                settings.MainFormSize = this.ClientSize;
        }
    }
    public class CommandlineUpgradeData
    {
        public List<string> filename = new List<string>();
        public List<string> tempFilename = new List<string>();
        public string newUploadDate;
    }
}
        #endregion