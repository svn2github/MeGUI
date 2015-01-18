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

namespace MeGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.inputTab = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.videoEncodingComponent1 = new MeGUI.VideoEncodingComponent();
            this.audioEncodingComponent1 = new MeGUI.AudioEncodingComponent();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.resetButton = new System.Windows.Forms.Button();
            this.helpButton1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.autoEncodeButton = new System.Windows.Forms.Button();
            this.OneClickEncButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.jobControl1 = new MeGUI.core.details.JobControl();
            this.logTab = new System.Windows.Forms.TabPage();
            this.logTree1 = new MeGUI.core.gui.LogTree();
            this.mnuMuxers = new System.Windows.Forms.MenuItem();
            this.mnuToolsAdaptiveMuxer = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuFileOpen = new System.Windows.Forms.MenuItem();
            this.mnuFileImport = new System.Windows.Forms.MenuItem();
            this.mnuFileExport = new System.Windows.Forms.MenuItem();
            this.mnuFileExit = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.progressMenu = new System.Windows.Forms.MenuItem();
            this.showAllProgressWindows = new System.Windows.Forms.MenuItem();
            this.hideAllProgressWindows = new System.Windows.Forms.MenuItem();
            this.separator2 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mnuViewMinimizeToTray = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.createNewWorker = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.workersMenu = new System.Windows.Forms.MenuItem();
            this.showAllWorkers = new System.Windows.Forms.MenuItem();
            this.hideAllWorkers = new System.Windows.Forms.MenuItem();
            this.separator = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.viewSummary = new System.Windows.Forms.MenuItem();
            this.mnuTools = new System.Windows.Forms.MenuItem();
            this.mnutoolsD2VCreator = new System.Windows.Forms.MenuItem();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.mnuOptionsSettings = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuChangelog = new System.Windows.Forms.MenuItem();
            this.mnuDoc = new System.Windows.Forms.MenuItem();
            this.mnuWebsite = new System.Windows.Forms.MenuItem();
            this.mnuHome = new System.Windows.Forms.MenuItem();
            this.mnuForum = new System.Windows.Forms.MenuItem();
            this.mnuBugTracker = new System.Windows.Forms.MenuItem();
            this.mnuFeaturesReq = new System.Windows.Forms.MenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMeGUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMeGUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.inputTab.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.logTab.SuspendLayout();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.inputTab);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.logTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(508, 499);
            this.tabControl1.TabIndex = 0;
            // 
            // inputTab
            // 
            this.inputTab.BackColor = System.Drawing.Color.Transparent;
            this.inputTab.Controls.Add(this.splitContainer2);
            this.inputTab.Controls.Add(this.splitContainer1);
            this.inputTab.Location = new System.Drawing.Point(4, 22);
            this.inputTab.Name = "inputTab";
            this.inputTab.Size = new System.Drawing.Size(500, 473);
            this.inputTab.TabIndex = 0;
            this.inputTab.Text = "Input";
            this.inputTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.videoEncodingComponent1);
            this.splitContainer2.Panel1MinSize = 168;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.audioEncodingComponent1);
            this.splitContainer2.Panel2MinSize = 150;
            this.splitContainer2.Size = new System.Drawing.Size(500, 441);
            this.splitContainer2.SplitterDistance = 168;
            this.splitContainer2.TabIndex = 4;
            // 
            // videoEncodingComponent1
            // 
            this.videoEncodingComponent1.BackColor = System.Drawing.SystemColors.Control;
            this.videoEncodingComponent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoEncodingComponent1.FileType = "";
            this.videoEncodingComponent1.Location = new System.Drawing.Point(0, 0);
            this.videoEncodingComponent1.MinimumSize = new System.Drawing.Size(500, 168);
            this.videoEncodingComponent1.Name = "videoEncodingComponent1";
            this.videoEncodingComponent1.PrerenderJob = false;
            this.videoEncodingComponent1.Size = new System.Drawing.Size(500, 168);
            this.videoEncodingComponent1.TabIndex = 0;
            this.videoEncodingComponent1.VideoInput = "";
            this.videoEncodingComponent1.VideoOutput = "";
            // 
            // audioEncodingComponent1
            // 
            this.audioEncodingComponent1.AutoScroll = true;
            this.audioEncodingComponent1.BackColor = System.Drawing.SystemColors.Control;
            this.audioEncodingComponent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioEncodingComponent1.Location = new System.Drawing.Point(0, 0);
            this.audioEncodingComponent1.MinimumSize = new System.Drawing.Size(400, 192);
            this.audioEncodingComponent1.Name = "audioEncodingComponent1";
            this.audioEncodingComponent1.Size = new System.Drawing.Size(500, 269);
            this.audioEncodingComponent1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 441);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(500, 32);
            this.splitContainer1.SplitterDistance = 244;
            this.splitContainer1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Controls.Add(this.resetButton);
            this.flowLayoutPanel1.Controls.Add(this.helpButton1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(244, 32);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // resetButton
            // 
            this.resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resetButton.AutoSize = true;
            this.resetButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.resetButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.resetButton.Location = new System.Drawing.Point(3, 3);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(45, 23);
            this.resetButton.TabIndex = 4;
            this.resetButton.Text = "Reset";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(54, 3);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 5;
            this.helpButton1.Text = "Help";
            this.helpButton1.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel2.Controls.Add(this.autoEncodeButton);
            this.flowLayoutPanel2.Controls.Add(this.OneClickEncButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(252, 32);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // autoEncodeButton
            // 
            this.autoEncodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoEncodeButton.AutoSize = true;
            this.autoEncodeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.autoEncodeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.autoEncodeButton.Location = new System.Drawing.Point(174, 3);
            this.autoEncodeButton.Name = "autoEncodeButton";
            this.autoEncodeButton.Size = new System.Drawing.Size(75, 23);
            this.autoEncodeButton.TabIndex = 2;
            this.autoEncodeButton.Text = "AutoEncode";
            this.autoEncodeButton.Click += new System.EventHandler(this.autoEncodeButton_Click);
            // 
            // OneClickEncButton
            // 
            this.OneClickEncButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OneClickEncButton.AutoSize = true;
            this.OneClickEncButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.OneClickEncButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OneClickEncButton.Location = new System.Drawing.Point(106, 3);
            this.OneClickEncButton.Name = "OneClickEncButton";
            this.OneClickEncButton.Size = new System.Drawing.Size(62, 23);
            this.OneClickEncButton.TabIndex = 3;
            this.OneClickEncButton.Text = "One-Click";
            this.OneClickEncButton.Click += new System.EventHandler(this.OneClickEncButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.jobControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(500, 473);
            this.tabPage2.TabIndex = 12;
            this.tabPage2.Text = "Queue";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // jobControl1
            // 
            this.jobControl1.BackColor = System.Drawing.SystemColors.Control;
            this.jobControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jobControl1.Location = new System.Drawing.Point(0, 0);
            this.jobControl1.Name = "jobControl1";
            this.jobControl1.Size = new System.Drawing.Size(500, 473);
            this.jobControl1.TabIndex = 0;
            // 
            // logTab
            // 
            this.logTab.Controls.Add(this.logTree1);
            this.logTab.Location = new System.Drawing.Point(4, 22);
            this.logTab.Name = "logTab";
            this.logTab.Size = new System.Drawing.Size(500, 473);
            this.logTab.TabIndex = 13;
            this.logTab.Text = "Log";
            this.logTab.UseVisualStyleBackColor = true;
            // 
            // logTree1
            // 
            this.logTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTree1.Location = new System.Drawing.Point(0, 0);
            this.logTree1.Name = "logTree1";
            this.logTree1.Size = new System.Drawing.Size(500, 473);
            this.logTree1.TabIndex = 0;
            // 
            // mnuMuxers
            // 
            this.mnuMuxers.Index = 0;
            this.mnuMuxers.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuToolsAdaptiveMuxer});
            this.mnuMuxers.Text = "Muxer";
            // 
            // mnuToolsAdaptiveMuxer
            // 
            this.mnuToolsAdaptiveMuxer.Index = 0;
            this.mnuToolsAdaptiveMuxer.Text = "Adaptive Muxer";
            this.mnuToolsAdaptiveMuxer.Click += new System.EventHandler(this.mnuToolsAdaptiveMuxer_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuView,
            this.menuItem1,
            this.mnuTools,
            this.mnuOptions,
            this.mnuHelp});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileOpen,
            this.mnuFileImport,
            this.mnuFileExport,
            this.mnuFileExit});
            this.mnuFile.Text = "&File";
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Index = 0;
            this.mnuFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuFileOpen.Text = "&Open";
            this.mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
            // 
            // mnuFileImport
            // 
            this.mnuFileImport.Index = 1;
            this.mnuFileImport.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.mnuFileImport.Text = "&Import Presets";
            this.mnuFileImport.Click += new System.EventHandler(this.mnuFileImport_Click);
            // 
            // mnuFileExport
            // 
            this.mnuFileExport.Index = 2;
            this.mnuFileExport.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.mnuFileExport.Text = "&Export Presets";
            this.mnuFileExport.Click += new System.EventHandler(this.mnuFileExport_Click);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Index = 3;
            this.mnuFileExit.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuView
            // 
            this.mnuView.Index = 1;
            this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.progressMenu,
            this.mnuViewMinimizeToTray});
            this.mnuView.Text = "&View";
            this.mnuView.Popup += new System.EventHandler(this.mnuView_Popup);
            // 
            // progressMenu
            // 
            this.progressMenu.Index = 0;
            this.progressMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.showAllProgressWindows,
            this.hideAllProgressWindows,
            this.separator2,
            this.menuItem7});
            this.progressMenu.Text = "&Process Status";
            // 
            // showAllProgressWindows
            // 
            this.showAllProgressWindows.Index = 0;
            this.showAllProgressWindows.Text = "Show all";
            this.showAllProgressWindows.Click += new System.EventHandler(this.showAllProgressWindows_Click);
            // 
            // hideAllProgressWindows
            // 
            this.hideAllProgressWindows.Index = 1;
            this.hideAllProgressWindows.Text = "Hide all";
            this.hideAllProgressWindows.Click += new System.EventHandler(this.hideAllProgressWindows_Click);
            // 
            // separator2
            // 
            this.separator2.Index = 2;
            this.separator2.Text = "-";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            this.menuItem7.Text = "(List of progress windows goes here)";
            // 
            // mnuViewMinimizeToTray
            // 
            this.mnuViewMinimizeToTray.Index = 1;
            this.mnuViewMinimizeToTray.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.mnuViewMinimizeToTray.Text = "&Minimize to Tray";
            this.mnuViewMinimizeToTray.Click += new System.EventHandler(this.mnuViewMinimizeToTray_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.createNewWorker,
            this.menuItem6,
            this.workersMenu,
            this.viewSummary});
            this.menuItem1.Text = "Workers";
            this.menuItem1.Popup += new System.EventHandler(this.showAllWorkers_Popup);
            // 
            // createNewWorker
            // 
            this.createNewWorker.Index = 0;
            this.createNewWorker.Text = "Create new worker";
            this.createNewWorker.Click += new System.EventHandler(this.createNewWorker_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "-";
            // 
            // workersMenu
            // 
            this.workersMenu.Index = 2;
            this.workersMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.showAllWorkers,
            this.hideAllWorkers,
            this.separator,
            this.menuItem2,
            this.menuItem3});
            this.workersMenu.Text = "Workers";
            // 
            // showAllWorkers
            // 
            this.showAllWorkers.Index = 0;
            this.showAllWorkers.Text = "Show all";
            this.showAllWorkers.Click += new System.EventHandler(this.showAllWorkers_Click);
            // 
            // hideAllWorkers
            // 
            this.hideAllWorkers.Index = 1;
            this.hideAllWorkers.Text = "Hide all";
            this.hideAllWorkers.Click += new System.EventHandler(this.hideAllWorkers_Click);
            // 
            // separator
            // 
            this.separator.Index = 2;
            this.separator.Text = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "(List of workers goes here)";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 4;
            this.menuItem3.Text = "ao";
            // 
            // viewSummary
            // 
            this.viewSummary.Index = 3;
            this.viewSummary.Text = "Worker summary";
            this.viewSummary.Click += new System.EventHandler(this.viewSummary_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.Index = 3;
            this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuMuxers,
            this.mnutoolsD2VCreator});
            this.mnuTools.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            this.mnuTools.Text = "&Tools";
            // 
            // mnutoolsD2VCreator
            // 
            this.mnutoolsD2VCreator.Index = 1;
            this.mnutoolsD2VCreator.Shortcut = System.Windows.Forms.Shortcut.CtrlF2;
            this.mnutoolsD2VCreator.Text = "File Indexer";
            this.mnutoolsD2VCreator.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Index = 4;
            this.mnuOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOptionsSettings});
            this.mnuOptions.Text = "&Options";
            // 
            // mnuOptionsSettings
            // 
            this.mnuOptionsSettings.Index = 0;
            this.mnuOptionsSettings.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuOptionsSettings.Text = "Settings";
            this.mnuOptionsSettings.Click += new System.EventHandler(this.mnuOptionsSettings_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 5;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuChangelog,
            this.mnuDoc,
            this.mnuWebsite});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuChangelog
            // 
            this.mnuChangelog.Index = 0;
            this.mnuChangelog.Text = "Changelog";
            this.mnuChangelog.Click += new System.EventHandler(this.mnuChangelog_Click);
            // 
            // mnuDoc
            // 
            this.mnuDoc.Index = 1;
            this.mnuDoc.Text = "Wiki - User Guides";
            this.mnuDoc.Click += new System.EventHandler(this.mnuDoc_Click);
            // 
            // mnuWebsite
            // 
            this.mnuWebsite.Index = 2;
            this.mnuWebsite.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuHome,
            this.mnuForum,
            this.mnuBugTracker,
            this.mnuFeaturesReq});
            this.mnuWebsite.Text = "Website";
            // 
            // mnuHome
            // 
            this.mnuHome.Index = 0;
            this.mnuHome.Text = "Homepage";
            this.mnuHome.Click += new System.EventHandler(this.mnuHome_Click);
            // 
            // mnuForum
            // 
            this.mnuForum.Index = 1;
            this.mnuForum.Text = "Forum";
            this.mnuForum.Click += new System.EventHandler(this.mnuForum_Click);
            // 
            // mnuBugTracker
            // 
            this.mnuBugTracker.Index = 2;
            this.mnuBugTracker.Text = "Bugs Tracker";
            this.mnuBugTracker.Click += new System.EventHandler(this.mnuBugTracker_Click);
            // 
            // mnuFeaturesReq
            // 
            this.mnuFeaturesReq.Index = 3;
            this.mnuFeaturesReq.Text = "Feature Requests";
            this.mnuFeaturesReq.Click += new System.EventHandler(this.mnuFeaturesReq_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipText = "meGUI is still working...";
            this.trayIcon.BalloonTipTitle = "meGUI";
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Text = "MeGUI";
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMeGUIToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitMeGUIToolStripMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.trayMenu.Size = new System.Drawing.Size(143, 54);
            // 
            // openMeGUIToolStripMenuItem
            // 
            this.openMeGUIToolStripMenuItem.Name = "openMeGUIToolStripMenuItem";
            this.openMeGUIToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.openMeGUIToolStripMenuItem.Text = "Open MeGUI";
            this.openMeGUIToolStripMenuItem.Click += new System.EventHandler(this.openMeGUIToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // exitMeGUIToolStripMenuItem
            // 
            this.exitMeGUIToolStripMenuItem.Name = "exitMeGUIToolStripMenuItem";
            this.exitMeGUIToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.exitMeGUIToolStripMenuItem.Text = "Exit MeGUI";
            this.exitMeGUIToolStripMenuItem.Click += new System.EventHandler(this.exitMeGUIToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(508, 499);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(524, 537);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.MeGUI_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MeGUI_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MeGUI_DragEnter);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabControl1.ResumeLayout(false);
            this.inputTab.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.logTab.ResumeLayout(false);
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TabPage inputTab;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuFileExit;
        private System.Windows.Forms.MenuItem mnuTools;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.MenuItem mnuView;
        private System.Windows.Forms.MenuItem progressMenu;
        private System.Windows.Forms.MenuItem mnuViewMinimizeToTray;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.MenuItem mnuMuxers;
        private System.Windows.Forms.MenuItem mnuFileOpen;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem openMeGUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitMeGUIToolStripMenuItem;
        private System.Windows.Forms.MenuItem mnuFileImport;
        private System.Windows.Forms.MenuItem mnuFileExport;
        private System.Windows.Forms.MenuItem mnuToolsAdaptiveMuxer;
        private AudioEncodingComponent audioEncodingComponent1;
        private VideoEncodingComponent videoEncodingComponent1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuItem mnuHelp;
        private System.Windows.Forms.MenuItem mnuChangelog;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem createNewWorker;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem workersMenu;
        private System.Windows.Forms.MenuItem showAllWorkers;
        private System.Windows.Forms.MenuItem hideAllWorkers;
        private System.Windows.Forms.MenuItem separator;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem viewSummary;
        private System.Windows.Forms.MenuItem showAllProgressWindows;
        private System.Windows.Forms.MenuItem hideAllProgressWindows;
        private System.Windows.Forms.MenuItem separator2;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem mnuDoc;
        private System.Windows.Forms.MenuItem mnuWebsite;
        private System.Windows.Forms.MenuItem mnuHome;
        private System.Windows.Forms.MenuItem mnuForum;
        private System.Windows.Forms.MenuItem mnuBugTracker;
        private System.Windows.Forms.MenuItem mnuFeaturesReq;
        private System.Windows.Forms.MenuItem mnuOptions;
        private System.Windows.Forms.MenuItem mnuOptionsSettings;
        private System.Windows.Forms.TabPage logTab;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button autoEncodeButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button OneClickEncButton;
        private System.Windows.Forms.Button helpButton1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.MenuItem mnutoolsD2VCreator;
        private MeGUI.core.details.JobControl jobControl1;
        private MeGUI.core.gui.LogTree logTree1;
    }
}