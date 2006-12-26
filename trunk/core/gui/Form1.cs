// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
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
using System.Threading;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Text;
using MeGUI.core.details;
using MeGUI.core.plugins.interfaces;
using System.Reflection;

namespace MeGUI
{
    public delegate void UpdateGUIStatusCallback(StatusUpdate su); // catches the UpdateGUI events fired from the encoder
    public enum FileType
    {
        VIDEOINPUT, AUDIOINPUT, DGINDEX, OTHERVIDEO, ZIPPED_PROFILES, NONE
    };
    public enum ProcessingStatus
    {
        DOINGNOTHING, RUNNING, PAUSED, STOPPING
    }

    /// <summary>
    /// Form1 is the main GUI of the program
    /// it contains all the elements required to start encoding and contains the application intelligence as well.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        // This instance is to be used by the serializers that can't be passed a MainForm as a parameter
        public static MainForm Instance;

        #region variable declaration
        //        private MeGUIInfo info;
        private System.Windows.Forms.TabPage inputTab;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage logTab;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuFileExit;
        private System.Windows.Forms.MenuItem mnuTools;
        private System.Windows.Forms.MenuItem mnuToolsSettings;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.MenuItem mnuView;
        private System.Windows.Forms.MenuItem mnuViewProcessStatus;
        private MenuItem mnuViewMinimizeToTray;
        private NotifyIcon trayIcon;
        private System.Windows.Forms.Button autoEncodeButton;
        private System.Windows.Forms.MenuItem mnuMuxers;
        private MenuItem mnuFileOpen;
        private ContextMenuStrip trayMenu;
        private ToolStripMenuItem openMeGUIToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private ToolStripMenuItem abortToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitMeGUIToolStripMenuItem;
        private MenuItem mnuFileImport;
        private MenuItem mnuFileExport;
        private Button clearLogButton;
        private MenuItem mnuToolsAdaptiveMuxer;
        #endregion
        private AudioEncodingComponent audioEncodingComponent1;
        private VideoEncodingComponent videoEncodingComponent1;
        private BitrateCalculator bitrateCalculator = new BitrateCalculator();
        private TabPage tabPage2;
        private JobControl jobControl1;
        private MenuItem mnuHelp;
        private MenuItem mnuGuide;
        private MenuItem mnuChangelog;
        private MenuItem mnuHelpLink;



        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
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
            this.autoEncodeButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.logTab = new System.Windows.Forms.TabPage();
            this.clearLogButton = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.TextBox();
            this.mnuMuxers = new System.Windows.Forms.MenuItem();
            this.mnuToolsAdaptiveMuxer = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuFileOpen = new System.Windows.Forms.MenuItem();
            this.mnuFileImport = new System.Windows.Forms.MenuItem();
            this.mnuFileExport = new System.Windows.Forms.MenuItem();
            this.mnuFileExit = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuViewProcessStatus = new System.Windows.Forms.MenuItem();
            this.mnuViewMinimizeToTray = new System.Windows.Forms.MenuItem();
            this.mnuTools = new System.Windows.Forms.MenuItem();
            this.mnuToolsSettings = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuChangelog = new System.Windows.Forms.MenuItem();
            this.mnuGuide = new System.Windows.Forms.MenuItem();
            this.mnuHelpLink = new System.Windows.Forms.MenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMeGUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMeGUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioEncodingComponent1 = new MeGUI.AudioEncodingComponent();
            this.videoEncodingComponent1 = new MeGUI.VideoEncodingComponent();
            this.jobControl1 = new MeGUI.core.details.JobControl();
            this.tabControl1.SuspendLayout();
            this.inputTab.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(478, 369);
            this.tabControl1.TabIndex = 0;
            // 
            // inputTab
            // 
            this.inputTab.BackColor = System.Drawing.SystemColors.Control;
            this.inputTab.Controls.Add(this.audioEncodingComponent1);
            this.inputTab.Controls.Add(this.videoEncodingComponent1);
            this.inputTab.Controls.Add(this.autoEncodeButton);
            this.inputTab.Controls.Add(this.resetButton);
            this.inputTab.Location = new System.Drawing.Point(4, 22);
            this.inputTab.Name = "inputTab";
            this.inputTab.Size = new System.Drawing.Size(470, 343);
            this.inputTab.TabIndex = 0;
            this.inputTab.Text = "Input";
            // 
            // autoEncodeButton
            // 
            this.autoEncodeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.autoEncodeButton.Location = new System.Drawing.Point(384, 330);
            this.autoEncodeButton.Name = "autoEncodeButton";
            this.autoEncodeButton.Size = new System.Drawing.Size(80, 23);
            this.autoEncodeButton.TabIndex = 6;
            this.autoEncodeButton.Text = "AutoEncode";
            this.autoEncodeButton.Click += new System.EventHandler(this.autoEncodeButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.resetButton.Location = new System.Drawing.Point(330, 330);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(48, 23);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Reset";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.jobControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(470, 364);
            this.tabPage2.TabIndex = 12;
            this.tabPage2.Text = "Queue";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // logTab
            // 
            this.logTab.Controls.Add(this.clearLogButton);
            this.logTab.Controls.Add(this.log);
            this.logTab.Location = new System.Drawing.Point(4, 22);
            this.logTab.Name = "logTab";
            this.logTab.Size = new System.Drawing.Size(470, 364);
            this.logTab.TabIndex = 10;
            this.logTab.Text = "Log";
            this.logTab.UseVisualStyleBackColor = true;
            // 
            // clearLogButton
            // 
            this.clearLogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.clearLogButton.Location = new System.Drawing.Point(8, 3);
            this.clearLogButton.Name = "clearLogButton";
            this.clearLogButton.Size = new System.Drawing.Size(75, 23);
            this.clearLogButton.TabIndex = 1;
            this.clearLogButton.Text = "Clear Log";
            this.clearLogButton.UseVisualStyleBackColor = true;
            this.clearLogButton.Click += new System.EventHandler(this.clearLogButton_Click);
            // 
            // log
            // 
            this.log.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.log.Location = new System.Drawing.Point(0, 32);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log.Size = new System.Drawing.Size(470, 332);
            this.log.TabIndex = 0;
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
            this.mnuTools,
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
            this.mnuFileImport.Text = "&Import Profiles";
            this.mnuFileImport.Click += new System.EventHandler(this.mnuFileImport_Click);
            // 
            // mnuFileExport
            // 
            this.mnuFileExport.Index = 2;
            this.mnuFileExport.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.mnuFileExport.Text = "&Export Profiles";
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
            this.mnuViewProcessStatus,
            this.mnuViewMinimizeToTray});
            this.mnuView.Text = "&View";
            this.mnuView.Popup += new System.EventHandler(this.mnuView_Popup);
            // 
            // mnuViewProcessStatus
            // 
            this.mnuViewProcessStatus.Index = 0;
            this.mnuViewProcessStatus.Text = "&Process Status";
            this.mnuViewProcessStatus.Click += new System.EventHandler(this.mnuViewProcessStatus_Click);
            // 
            // mnuViewMinimizeToTray
            // 
            this.mnuViewMinimizeToTray.Index = 1;
            this.mnuViewMinimizeToTray.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.mnuViewMinimizeToTray.Text = "&Minimize to Tray";
            this.mnuViewMinimizeToTray.Click += new System.EventHandler(this.mnuViewMinimizeToTray_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.Index = 2;
            this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuMuxers,
            this.mnuToolsSettings});
            this.mnuTools.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            this.mnuTools.Text = "&Tools";
            // 
            // mnuToolsSettings
            // 
            this.mnuToolsSettings.Index = 1;
            this.mnuToolsSettings.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuToolsSettings.Text = "Settings";
            this.mnuToolsSettings.Click += new System.EventHandler(this.mnuToolsSettings_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 3;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuChangelog,
            this.mnuGuide,
            this.mnuHelpLink});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuChangelog
            // 
            this.mnuChangelog.Index = 0;
            this.mnuChangelog.Text = "Changelog";
            this.mnuChangelog.Click += new System.EventHandler(this.mnuChangelog_Click);
            // 
            // mnuGuide
            // 
            this.mnuGuide.Index = 1;
            this.mnuGuide.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
            this.mnuGuide.Text = "&Guide";
            this.mnuGuide.Click += new System.EventHandler(this.mnuGuide_Click);
            // 
            // mnuHelpLink
            // 
            this.mnuHelpLink.Index = 2;
            this.mnuHelpLink.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            this.mnuHelpLink.Text = "Help";
            this.mnuHelpLink.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Text = "MeGUI";
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMeGUIToolStripMenuItem,
            this.toolStripSeparator1,
            this.startToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.abortToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitMeGUIToolStripMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(153, 126);
            // 
            // openMeGUIToolStripMenuItem
            // 
            this.openMeGUIToolStripMenuItem.Name = "openMeGUIToolStripMenuItem";
            this.openMeGUIToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openMeGUIToolStripMenuItem.Text = "Open MeGUI";
            this.openMeGUIToolStripMenuItem.Click += new System.EventHandler(this.openMeGUIToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startToolStripMenuItem.Text = "Start";
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            // 
            // abortToolStripMenuItem
            // 
            this.abortToolStripMenuItem.Name = "abortToolStripMenuItem";
            this.abortToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.abortToolStripMenuItem.Text = "Abort";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // exitMeGUIToolStripMenuItem
            // 
            this.exitMeGUIToolStripMenuItem.Name = "exitMeGUIToolStripMenuItem";
            this.exitMeGUIToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitMeGUIToolStripMenuItem.Text = "Exit MeGUI";
            this.exitMeGUIToolStripMenuItem.Click += new System.EventHandler(this.exitMeGUIToolStripMenuItem_Click);
            // 
            // audioEncodingComponent1
            // 
            this.audioEncodingComponent1.AudioInput = "";
            this.audioEncodingComponent1.AudioOutput = "";
            this.audioEncodingComponent1.Dock = System.Windows.Forms.DockStyle.Top;
            this.audioEncodingComponent1.Location = new System.Drawing.Point(0, 153);
            this.audioEncodingComponent1.MaximumSize = new System.Drawing.Size(0, 162);
            this.audioEncodingComponent1.MinimumSize = new System.Drawing.Size(400, 162);
            this.audioEncodingComponent1.Name = "audioEncodingComponent1";
            this.audioEncodingComponent1.Size = new System.Drawing.Size(470, 162);
            this.audioEncodingComponent1.TabIndex = 8;
            // 
            // videoEncodingComponent1
            // 
            this.videoEncodingComponent1.Dock = System.Windows.Forms.DockStyle.Top;
            this.videoEncodingComponent1.Location = new System.Drawing.Point(0, 0);
            this.videoEncodingComponent1.MaximumSize = new System.Drawing.Size(0, 153);
            this.videoEncodingComponent1.MinimumSize = new System.Drawing.Size(415, 153);
            this.videoEncodingComponent1.Name = "videoEncodingComponent1";
            this.videoEncodingComponent1.PrerenderJob = false;
            this.videoEncodingComponent1.Size = new System.Drawing.Size(470, 153);
            this.videoEncodingComponent1.TabIndex = 7;
            this.videoEncodingComponent1.VideoInput = "";
            this.videoEncodingComponent1.VideoOutput = "";
            // 
            // jobControl1
            // 
            this.jobControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jobControl1.Location = new System.Drawing.Point(0, 0);
            this.jobControl1.Name = "jobControl1";
            this.jobControl1.Size = new System.Drawing.Size(470, 364);
            this.jobControl1.TabIndex = 0;
            this.jobControl1.Load += new System.EventHandler(this.jobControl1_Load);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(478, 369);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MeGUI_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MeGUI_DragEnter);
            this.Load += new System.EventHandler(this.MeGUI_Load);
            this.tabControl1.ResumeLayout(false);
            this.inputTab.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.logTab.ResumeLayout(false);
            this.logTab.PerformLayout();
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
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
            System.Diagnostics.Process.Start("http://forum.doom9.org/forumdisplay.php?f=78");
        }
        /// <summary>
        /// shows the changelog dialog window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuChangelog_Click(object sender, EventArgs e)
        {
            using (Changelog cl = new Changelog())
            {
                cl.ShowDialog();
            }
        }

        public MainForm()
        {
            Instance = this;
            InitializeComponent();

            System.Reflection.Assembly myAssembly = this.GetType().Assembly;
            string name = this.GetType().Namespace + ".";
#if CSC
			name = "";
#endif
            string[] resources = myAssembly.GetManifestResourceNames();
            this.trayIcon.Icon = new Icon(myAssembly.GetManifestResourceStream(name + "App.ico"));
            constructMeGUIInfo();
            this.TitleText = Application.ProductName + " " + Application.ProductVersion;
            Jobs.showAfterEncodingStatus(Settings);
        }

        #region GUI properties
        public BitrateCalculator BitrateCalculator
        {
            get { return bitrateCalculator; }
        }
        public JobControl Jobs
        {
            get { return jobControl1; }
        }
        public bool ProcessStatusChecked
        {
            get { return mnuViewProcessStatus.Checked; }
            set { mnuViewProcessStatus.Checked = value; }
        }
        public ToolStripMenuItem PauseMenuItemTS
        {
            get { return pauseToolStripMenuItem; }
        }
        public ToolStripMenuItem StartStopMenuItemTS
        {
            get { return startToolStripMenuItem; }
        }
        public ToolStripMenuItem AbortMenuItemTS
        {
            get { return abortToolStripMenuItem; }
        }
        public VideoEncodingComponent Video
        {
            get { return videoEncodingComponent1; }
        }
        public AudioEncodingComponent Audio
        {
            get { return audioEncodingComponent1; }
        }
        
/*        public Menu ToolsMenu
        {
            get { return mnuTools; }
        }
        public Menu FileMenu
        {
            get { return mnuFile; }
        }
        public TextBox Changelog
        {
            get { return txtChangelog; }
        }
        public TextBox Log
        {
            get { return log; }
        }*/
        #endregion
        /// <summary>
        /// initializes all the dropdown elements in the GUI to their default values
        /// </summary>

        /// <summary>
        /// handles the GUI closing event
        /// saves all jobs, stops the currently active job and saves all profiles as well
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (jobControl1.IsEncoding)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to quit?", "Job in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                    e.Cancel = true; // abort closing
                else
                    jobControl1.Abort();
            }
            if (!e.Cancel)
            {
                CloseSilent();
            }
            base.OnClosing(e);
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

/*        internal void AttachInfo(MeGUIInfo info)
        {
            this.info = info;
            info.MainForm = this;
        }*/
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
                MessageBox.Show("Required tool, '" + p + "', not found.");
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
        private void clearLogButton_Click(object sender, EventArgs e)
        {
            saveLog();
            logBuilder = new StringBuilder();
            log.Text = "";
        }
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
                    Console.Write(e.Message);
                }
            }
        }
        /// <summary>
        /// loads the global settings
        /// </summary>
        public void loadSettings()
        {
            string fileName = path + "\\settings.xml";
            if (File.Exists(fileName))
            {
                XmlSerializer ser = null;
                using (Stream s = File.OpenRead(fileName))
                {
                    ser = new XmlSerializer(typeof(MeGUISettings));
                    try
                    {
                        this.settings = (MeGUISettings)ser.Deserialize(s);

                        // modify PATH so that n00bs don't complain because they forgot to put dgdecode.dll in the MeGUI dir
                        string pathEnv = Environment.GetEnvironmentVariable("PATH");
                        pathEnv = Path.GetDirectoryName(settings.DgIndexPath) + ";" + pathEnv;
                        Environment.SetEnvironmentVariable("PATH", pathEnv);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Settings could not be loaded.", "Error loading profile", MessageBoxButtons.OK);
                        Console.Write(e.Message);
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
            if (settings.AfterEncoding == AfterEncoding.DoNothing) return;
            this.profileManager.SaveProfiles();
            this.saveSettings();
            jobControl1.saveJobs();
            this.saveLog();

            if (settings.AfterEncoding == AfterEncoding.Shutdown)
            {
                bool succ = Shutdown.shutdown();
                if (!succ)
                    addToLog("Tried shutting down system at " + DateTime.Now.ToShortTimeString() + " but the call failed");
                else
                    addToLog("Shutdown initiated at " + DateTime.Now.ToShortTimeString());
            }
            else
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
/*                try { File.Delete(filename); }
                catch (IOException) { }*/

            }
        }
        /// <summary>
        /// adds a string to the log
        /// </summary>
        /// <param name="logEntry"></param>
        public void addToLog(string logEntry)
        {
            logBuilder.Append(logEntry);
            this.log.Text = logBuilder.ToString();
        }
        /// <summary>
        /// saves the whole content of the log into a logfile
        /// </summary>
        public void saveLog()
        {
            if (this.logBuilder.Length > 0)
            {
                StreamWriter sw = null;
                try
                {
                    string logDirectory = path + @"\logs";
                    if (!Directory.Exists(logDirectory))
                        Directory.CreateDirectory(logDirectory);
                    string fileName = logDirectory + @"\logfile-" + DateTime.Now.ToString("yy'-'MM'-'dd'_'HH'-'mm'-'ss") + ".log";
                    sw = new StreamWriter(fileName, true);
                    sw.WriteLine(this.logBuilder.ToString());
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                finally
                {
                    if (sw != null)
                    {
                        try
                        {
                            sw.Close();
                        }
                        catch (Exception f)
                        {
                            Console.Write(f.Message);
                        }
                    }
                }
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
            openFileDialog.Filter = "AviSynth Scripts (*.avs)|*.avs|" +
                "Audio Files (*.ac3, *.mp2, *.mpa, *.wav)|*.ac3;*.mp2;*.mpa;*.wav|" +
                "MPEG-2 Files (*.vob, *.mpg, *.mpeg, *.m2v, *.mpv, *.tp, *.ts, *.trp, *.pva, *.vro)|" +
                "*.vob;*.mpg;*.mpeg;*.m2v;*.mpv;*.tp;*.ts;*.trp;*.pva;*.vro|" +
                "Other Video Files (*.d2v, *.avi, *.mp4, *.mkv, *.rmvb)|*.d2v;*.avi;*.mp4;*.mkv;*.rmvb|" +
                "All supported encodable files|" +
                "*.avs;*.ac3;*.mp2;*.mpa;*.wav;*.vob;*.mpg;*.mpeg;*.m2v;*.mpv;*.tp;*.ts;*.trp;*.pva;*.vro;*.d2v;*.avi;*.mp4;*.mkv;*.rmvb|" +
                "All files|*.*";
            openFileDialog.Title = "Select your input file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                openFile(openFileDialog.FileName);
        }
        private void mnuViewMinimizeToTray_Click(object sender, EventArgs e)
        {
            Jobs.HideProcessWindow();
            this.Hide();
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
/*        private void mnuAVCLevelValidation_Popup(object sender, System.EventArgs e)
        {
            if (Video.VideoInput.Equals(""))
                mnuAVCLevelValidation.Enabled = false;
            else
                mnuAVCLevelValidation.Enabled = true;
        }*/
        private void mnuTool_Click(object sender, System.EventArgs e)
        {
            if ((!(sender is System.Windows.Forms.MenuItem)) || (!((sender as MenuItem).Tag is ITool)))
                return;
            ((ITool)(sender as MenuItem).Tag).Run(this);
        }

        private void mnuMuxer_Click(object sender, System.EventArgs e)
        {
            if ((!(sender is System.Windows.Forms.MenuItem)) || (!((sender as MenuItem).Tag is IMuxing)))
                return;
            using (MuxWindow mw = new MuxWindow((IMuxing)((sender as MenuItem).Tag),this))
            {
                if (mw.ShowDialog() == DialogResult.OK)
                {
                    MuxJob job = mw.Job;
                    int freeJobNumber = Jobs.getFreeJobNumber();
                    job.Name = "job" + freeJobNumber;
                    Jobs.addJobsToQueue(job);
                }
            }
        }

        private void mnuView_Popup(object sender, System.EventArgs e)
        {
            if (Jobs.ProcessWindowAccessible)
            {
                mnuViewProcessStatus.Enabled = true;
            }
            else
            {
                mnuViewProcessStatus.Enabled = false;
            }
        }
        private void mnuViewProcessStatus_Click(object sender, System.EventArgs e)
        {
            if (Jobs.ProcessWindowAccessible)
            {
                if (mnuViewProcessStatus.Checked)
                {
                    mnuViewProcessStatus.Checked = false;
                    Jobs.HideProcessWindow();
                }
                else
                {
                    mnuViewProcessStatus.Checked = true;
                    Jobs.ShowProcessWindow();
                }
            }
            else
            {
                Debug.Assert(false, "ProgressWindow should not be null if we can get here");
            }
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
            this.Show();
            if (mnuViewProcessStatus.Checked)
                Jobs.ShowProcessWindow();
            trayIcon.Visible = false;
        }
        private void openMeGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            if (mnuViewProcessStatus.Checked)
                Jobs.ShowProcessWindow();
            trayIcon.Visible = false;
        }

        #endregion
        #region file opening
        public void openOtherVideoFile(string fileName)
        {
            AviSynthWindow asw = new AviSynthWindow(this, fileName);
            asw.OpenScript += new OpenScriptCallback(Video.openVideoFile);
            asw.Show();
        }
        public void openDGIndexFile(string fileName)
        {
            if (DialogManager.useOneClick())
                openOneClickFile(fileName);
            else
                openD2VCreatorFile(fileName);
        }
        public void openOneClickFile(string fileName)
        {
#warning fix bitrate calculator
/*            OneClickWindow ocmt = new OneClickWindow(this, VideoProfile.SelectedIndex,
                GenericProfile<AudioCodecSettings>.SelectedIndex, jobUtil, 
                videoEncodingComponent1.VideoEncoderProvider, 
                audioEncodingComponent1.AudioEncoderProvider);
            ocmt.Input = fileName;
            ocmt.ShowDialog();*/
        }
        public void openD2VCreatorFile(string fileName)
        {
            VobinputWindow mpegInput = new VobinputWindow(this, fileName);
            mpegInput.setConfig(fileName, Path.ChangeExtension(fileName, ".d2v"), 2, 0, -1,
                true, true, true, false);
            mpegInput.ShowDialog();
        }
        private FileType getFileType(string fileName)
        {
            switch (Path.GetExtension(fileName.ToLower()))
            {
                case ".avs":
                    return FileType.VIDEOINPUT;
                case ".ac3":
                case ".mp2":
                case ".mpa":
                case ".wav":
                    return FileType.AUDIOINPUT;

                case ".vob":
                case ".mpg":
                case ".mpeg":
                case ".m2v":
                case ".mpv":
                case ".tp":
                case ".ts":
                case ".trp":
                case ".pva":
                case ".vro":
                    return FileType.DGINDEX;

                case ".zip":
                    return FileType.ZIPPED_PROFILES;

                default:
                    return FileType.OTHERVIDEO;
            }
        }
        public void openFile(string file)
        {
            switch (getFileType(file))
            {
                case FileType.VIDEOINPUT:
                    Video.openVideoFile(file);
                    break;
                case FileType.AUDIOINPUT:
                    audioEncodingComponent1.openAudioFile(file);
                    break;

                case FileType.DGINDEX:
                    openDGIndexFile(file);
                    break;

                case FileType.OTHERVIDEO:
                    openOtherVideoFile(file);
                    break;

                case FileType.ZIPPED_PROFILES:
                    importProfiles(file);
                    break;
            }
        }

        private void importProfiles(string file)
        {
            ProfilePorter importer = new ProfilePorter(profileManager, file, this);
            importer.ShowDialog();
            Video.RefreshProfiles();
            Audio.RefreshProfiles();
        }
        #endregion
        #region Drag 'n' Drop
        private void MeGUI_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            Invoke(new MethodInvoker(delegate
            {
                openFile(files[0]);
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
        public void importProfiles(byte[] data)
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { importProfiles(data); }));
                return;
            }
            ProfilePorter importer = new ProfilePorter(profileManager, this, data);
            importer.ShowDialog();
            Video.RefreshProfiles();
            Audio.RefreshProfiles();
        }

        private void mnuFileImport_Click(object sender, EventArgs e)
        {
            ProfilePorter importer = new ProfilePorter(profileManager, true, this);
            importer.ShowDialog();
            Video.RefreshProfiles();
            Audio.RefreshProfiles();
        }

        private void mnuFileExport_Click(object sender, EventArgs e)
        {
            ProfilePorter exporter = new ProfilePorter(profileManager, false, this);
            exporter.ShowDialog();
        }
        #endregion

        private void mnuToolsAdaptiveMuxer_Click(object sender, EventArgs e)
        {
            AdaptiveMuxWindow amw = new AdaptiveMuxWindow(this);
            if (amw.ShowDialog() == DialogResult.OK)
            {
                MuxJob[] jobs = amw.Jobs;
                int freeJobNumber = Jobs.getFreeJobNumber();
                int subNumber = 1;
                if (jobs.Length > 1)
                {
                    foreach (MuxJob job in jobs)
                    {
                        job.Name = "job" + freeJobNumber + "-" + subNumber;
                        subNumber++;
                    }
                }
                else
                {
                    if (jobs.Length == 1)
                        jobs[0].Name = "job" + freeJobNumber;
                    else
                        return;
                }
                Jobs.addJobsToQueue(jobs);
            }

        }

        private void MeGUI_Load(object sender, EventArgs e)
        {
            if (settings.AutoUpdate)
            {
                // Need a seperate thread to run the updater to stop internet lookups from freezing the app.
                Thread updateCheck = new Thread(new ThreadStart(beginUpdateCheck));
                updateCheck.IsBackground = true;
                updateCheck.Start();
            }
        }

        private void beginUpdateCheck()
        {
            UpdateWindow update = new UpdateWindow(this, this.Settings);
            update.GetUpdateData(true);
            if (update.HasUpdatableFiles()) // If there are updated files, display the window
            {
                if (MessageBox.Show("There are updated files available. Do you wish to update to the latest versions?",
                    "Updates Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    update.ShowDialog();
            }
        }

        private void mnuUpdate_Click(object sender, EventArgs e)
        {
            UpdateWindow update = new UpdateWindow(this, this.Settings);
            update.ShowDialog();
        }

        internal void AddFileToReplace(string iUpgradeableName, string tempFilename, string filename, string newVersion)
        {
            CommandlineUpgradeData data = new CommandlineUpgradeData();
            data.filename.Add(filename);
            data.tempFilename.Add(tempFilename);
            data.newVersion = newVersion;
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
            jobControl1.saveJobs();
            this.saveLog();
            this.runRestarter();
        }

        public void OpenVideoFile(string p)
        {
            if (p.ToLower().EndsWith(".avs"))
                Video.openVideoFile(p);
            else
            {
#warning should use mediafactory to generate scripts here.
                string newFileName = VideoUtil.createSimpleAvisynthScript(p);
                if (newFileName != null)
                    Video.openVideoFile(newFileName);
            }
        }
        #region MeGUIInfo
        #region variable declaration
        private bool restart = false;
        private Dictionary<string, CommandlineUpgradeData> filesToReplace = new Dictionary<string, CommandlineUpgradeData>();
        private DialogManager dialogManager;
        private string path; // path the program was started from
        private CommandLineGenerator gen; // class that generates commandlines
        private StringBuilder logBuilder; // made public so that system jobs can write to it
        private MediaFileFactory mediaFileFactory;
        private PackageSystem packageSystem = new PackageSystem();
        private JobUtil jobUtil;
        private MuxProvider muxProvider;
        private MeGUISettings settings = new MeGUISettings();
        private ProfileManager profileManager;
        private BitrateCalculator calc;

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
#warning turn the UpdateWindow here into an ITool
            if (parser.upgradeData.Count > 0)
            {
                UpdateWindow update = new UpdateWindow(this, Settings);
                foreach (string file in parser.upgradeData.Keys)
                    update.UpdateVersionNumber(file, parser.upgradeData[file]);
                update.SaveSettings();
            }
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
            this.gen = new CommandLineGenerator();
            this.path = System.Windows.Forms.Application.StartupPath;
            this.logBuilder = new StringBuilder();
            this.jobUtil = new JobUtil(this);
            this.settings = new MeGUISettings();
            this.calc = new BitrateCalculator();
            addPackages();
            fillMenus();
            videoEncodingComponent1.MainForm = this;
            audioEncodingComponent1.MainForm = this;
            this.profileManager = new ProfileManager(this.path);
            this.profileManager.LoadProfiles();
            this.mediaFileFactory = new MediaFileFactory(this);
            videoEncodingComponent1.InitializeDropdowns();
            audioEncodingComponent1.InitializeDropdowns();
            this.loadSettings();
            jobControl1.MainForm = this;
            jobControl1.loadJobs();
            this.dialogManager = new DialogManager(this);

            //MessageBox.Show(String.Join("|", this.GetType().Assembly.GetManifestResourceNames()));
        }

        private void fillMenus()
        {
            // Fill the muxing menu
            mnuMuxers.MenuItems.Clear();
            mnuMuxers.MenuItems.Add(mnuToolsAdaptiveMuxer);
            int index = 1;
            foreach (IMuxing muxer in PackageSystem.MuxerProviders.Values)
            {
                MenuItem newMenuItem = new MenuItem();
                newMenuItem.Text = muxer.Name;
                newMenuItem.Tag = muxer;
                newMenuItem.Index = index;
                index++;
                mnuMuxers.MenuItems.Add(newMenuItem);
                newMenuItem.Click += new System.EventHandler(this.mnuMuxer_Click);
            }

            // Fill the tools menu
            mnuTools.MenuItems.Clear();
            List<MenuItem> toolsItems = new List<MenuItem>();
            List<Shortcut> usedShortcuts = new List<Shortcut>();
            toolsItems.Add(mnuMuxers);
            toolsItems.Add(mnuToolsSettings);
            usedShortcuts.Add(mnuMuxers.Shortcut);
            usedShortcuts.Add(mnuToolsSettings.Shortcut);
            
            foreach (ITool tool in PackageSystem.Tools.Values)
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
                    addToLog("Shortcut for '" + tool.Name + "' is already used. No shortcut selected.\r\n");
                toolsItems.Add(newMenuItem);
            }

            toolsItems.Sort(new Comparison<MenuItem>(delegate(MenuItem a, MenuItem b) { return (a.Text.CompareTo(b.Text)); }));
            index = 0;
            foreach (MenuItem m in toolsItems)
            {
                m.Index = index;
                index++;
                mnuTools.MenuItems.Add(m);
            }
        }

        private void addPackages()
        {
            PackageSystem.JobProcessors.Register(VideoEncoder.Factory);
            PackageSystem.JobProcessors.Register(AudioEncoder.Factory);
            PackageSystem.JobProcessors.Register(Muxer.Factory);
            PackageSystem.JobProcessors.Register(AviSynthProcessor.Factory);
            PackageSystem.JobProcessors.Register(DGIndexer.Factory);
            PackageSystem.JobProcessors.Register(VobSubIndexer.Factory);
            PackageSystem.MuxerProviders.Register(new MKVMergeMuxerProvider());
            PackageSystem.MuxerProviders.Register(new MP4BoxMuxerProvider());
            PackageSystem.MuxerProviders.Register(new AVC2AVIMuxerProvider());
            PackageSystem.MuxerProviders.Register(new DivXMuxProvider());
            PackageSystem.Tools.Register(new AviSynthWindowTool());
            PackageSystem.Tools.Register(new AutoEncodeTool());
            PackageSystem.Tools.Register(new CQMEditorTool());
            PackageSystem.Tools.Register(new CalculatorTool());
            PackageSystem.Tools.Register(new ChapterCreatorTool());
            PackageSystem.Tools.Register(new UpdateTool());
//            PackageSystem.Tools.Register(new OneClickConfigTool());
            PackageSystem.Tools.Register(new OneClickTool());
            PackageSystem.Tools.Register(new D2VCreatorTool());
            PackageSystem.Tools.Register(new AVCLevelTool());
            PackageSystem.Tools.Register(new VobSubTool());
            PackageSystem.VideoSettingsProviders.Register(new X264SettingsProvider());
            PackageSystem.VideoSettingsProviders.Register(new XviDSettingsProvider());
            PackageSystem.VideoSettingsProviders.Register(new SnowSettingsProvider());
            PackageSystem.VideoSettingsProviders.Register(new LavcSettingsProvider());
            PackageSystem.AudioSettingsProviders.Register(new NeroAACSettingsProvider());
            PackageSystem.AudioSettingsProviders.Register(new AudXSettingsProvider());
            PackageSystem.AudioSettingsProviders.Register(new faacSettingsProvider());
            PackageSystem.AudioSettingsProviders.Register(new ffac3SettingsProvider());
            PackageSystem.AudioSettingsProviders.Register(new ffmp2SettingsProvider());
            PackageSystem.AudioSettingsProviders.Register(new lameSettingsProvider());
            PackageSystem.AudioSettingsProviders.Register(new vorbisSettingsProvider());
            PackageSystem.AudioSettingsProviders.Register(new waacSettingsProvider());
            PackageSystem.MediaFileTypes.Register(new AvsFileFactory());
            PackageSystem.MediaFileTypes.Register(new d2vFileFactory());
            PackageSystem.MediaFileTypes.Register(new MediaInfoFileFactory());
            PackageSystem.JobPreProcessors.Register(JobUtil.CalculationProcessor);
            PackageSystem.JobPostProcessors.Register(OneClickPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(IndexJobPostProcessor.PostProcessor);
            PackageSystem.JobPostProcessors.Register(JobControl.DeleteIntermediateFilesPostProcessor);
        }

        private static Mutex mySingleInstanceMutex = new Mutex(true, "MeGUI_D9D0C224154B489784998BF97B9C9414");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            if (!mySingleInstanceMutex.WaitOne(0, false))
            {
                if (DialogResult.Yes != MessageBox.Show("Running MeGUI instance detected!\n\rThere's not really much point in running multiple copies of MeGUI, and it can cause problems.\n\rDo You still want to run yet another MeGUI instance?", "Running MeGUI instance detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    return;
            }
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            CommandlineParser parser = new CommandlineParser();
            parser.Parse(args);
//            MeGUIInfo info = new MeGUIInfo();
            MainForm mainForm = new MainForm();
//            mainForm.AttachInfo(info);
            mainForm.handleCommandline(parser);
            if (parser.start)
                Application.Run(mainForm);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("MeGUI encountered a fatal error and may not be able to proceed. Reason: " + e.Exception.Message
                + " Source of exception: " + e.Exception.Source + " stacktrace: " + e.Exception.StackTrace, "Fatal error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void runRestarter()
        {
            if (filesToReplace.Keys.Count == 0)
                return;
            Process proc = new Process();
            ProcessStartInfo pstart = new ProcessStartInfo();
            pstart.FileName = Path.Combine(Application.StartupPath, "updatecopier.exe");
            foreach (string file in filesToReplace.Keys)
            {
                pstart.Arguments += string.Format("--component \"{0}\" \"{1}\" ", file, filesToReplace[file].newVersion);
                for (int i = 0; i < filesToReplace[file].filename.Count; i++)
                {
                    pstart.Arguments += string.Format("\"{0}\" \"{1}\" ",
                       filesToReplace[file].filename[i],
                       filesToReplace[file].tempFilename[i]);
                }
            }
            if (restart)
                pstart.Arguments += "--restart ";
            else
                pstart.Arguments += "--app ";
            pstart.Arguments += "\"" + Application.ExecutablePath + "\"";

            pstart.CreateNoWindow = true;
            pstart.UseShellExecute = false;
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
/*        public MeGUISettings Settings
        {
            get { return settings; }
        }*/

        /// <summary>
        /// gets the path from where MeGUI was launched
        /// </summary>
        public string MeGUIPath
        {
            get { return this.path; }
        }
/*        /// <summary>
        /// gets  / sets the currently selected audiostream
        /// </summary>
        public ProfileManager Profiles
        {
            get { return profileManager; }
        }*/
        #endregion
#endregion

        internal void ClosePlayer()
        {
            videoEncodingComponent1.ClosePlayer();
        }

        internal void setAudioTrack(int counter, string p)
        {
            audioEncodingComponent1.setAudioTrack(counter, p);
        }

        internal void hidePlayer()
        {
            videoEncodingComponent1.hidePlayer();
        }

        internal void showPlayer()
        {
            videoEncodingComponent1.showPlayer();
        }

        private void jobControl1_Load(object sender, EventArgs e)
        {

        }
    }
    public class CommandlineUpgradeData
    {
        public List<string> filename = new List<string>();
        public List<string> tempFilename = new List<string>();
        public string newVersion;
    }
}
        #endregion