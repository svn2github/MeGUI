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
        #region variable declaration
        private bool restart = false;
        private Dictionary<string, CommandlineUpgradeData> filesToReplace = new Dictionary<string, CommandlineUpgradeData>();
        private DialogManager dialogManager;
        private string path; // path the program was started from
        private int jobNr = 1; // number of jobs in the queue
        private bool isEncoding = false, queueEncoding = false, paused = false; // encoding status and whether or not we're in queue encoding mode
        private Dictionary<string, Job> jobs; //storage for all the jobs and profiles known to the system
        private List<Job> skipJobs; // contains jobs to be skipped (chained with a previously errored out job)
        private IJobProcessor currentProcessor;
        private CommandLineGenerator gen; // class that generates commandlines
        private ProgressWindow pw; // window that shows the encoding progress
        private VideoPlayer player; // window that shows a preview of the video
        public StringBuilder logBuilder; // made public so that system jobs can write to it
        private int creditsStartFrame = -1, introEndFrame = -1;
        private int parX = -1, parY = -1;
        private JobUtil jobUtil;
        private int lastSelectedAudioTrackNumber = 0;
        private MuxProvider muxProvider = new MuxProvider();
        private VideoEncoderProvider videoEncoderProvider;
        private AudioEncoderProvider audioEncoderProvider;
        private MeGUISettings settings;
        private Bitmap pauseImage;
        private Bitmap playImage;
        private ProfileManager profileManager;
        private ContextMenuStrip queueContextMenu;
        private ToolStripMenuItem AbortMenuItem;
        private ToolStripMenuItem LoadMenuItem;
        private ToolStripMenuItem DeleteMenuItem;
        private ToolStripMenuItem StatusMenuItem;
        private ToolStripMenuItem PostponedMenuItem;
        private ToolStripMenuItem WaitingMenuItem;
        private System.Windows.Forms.TabPage inputTab;
        private System.Windows.Forms.Button inputOpenButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox videoGroupBox;
        private System.Windows.Forms.TextBox videoInput;
        private System.Windows.Forms.Label videoInputLabel;
        private System.Windows.Forms.ComboBox videoProfile;
        private System.Windows.Forms.Label videoProfileLabel;
        private System.Windows.Forms.TextBox videoOutput;
        private System.Windows.Forms.Label videoOutputLabel;
        private System.Windows.Forms.Button videoOutputOpenButton;
        private System.Windows.Forms.TabPage logTab;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuFileExit;
        private System.Windows.Forms.MenuItem mnuTools;
        private System.Windows.Forms.MenuItem mnuToolsSettings;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button videoConfigButton;
        private System.Windows.Forms.Button queueVideoButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.MenuItem mnuToolsBitrateCalculator;
        private System.Windows.Forms.MenuItem mnuView;
        private System.Windows.Forms.MenuItem mnuViewProcessStatus;
        private System.Windows.Forms.Label videoContainerLabel;
        private System.Windows.Forms.ComboBox fileType;
        private MenuItem mnuViewMinimizeToTray;
        private NotifyIcon trayIcon;
        private BitrateCalculator calc;
        private AudioStream[] audioStreams;
        private Button addAnalysisPass;
        private CheckBox addPrerenderJob;
        private System.Windows.Forms.Label videoCodecLabel;
        private System.Windows.Forms.ComboBox videoCodec;
        private System.Windows.Forms.MenuItem mnuToolsD2VCreator;
        private System.Windows.Forms.MenuItem mnuToolsAviSynth;
        private System.Windows.Forms.MenuItem mnuChapterCreator;
        private System.Windows.Forms.GroupBox audioIOGroupBox;
        private System.Windows.Forms.Button queueAudioButton;
        private System.Windows.Forms.ComboBox audioProfile;
        private System.Windows.Forms.Label audioProfileLabel;
        private System.Windows.Forms.TextBox audioInput;
        private System.Windows.Forms.Button audioInputOpenButton;
        private System.Windows.Forms.Label audioInputLabel;
        private System.Windows.Forms.TextBox audioOutput;
        private System.Windows.Forms.Label audioOutputLabel;
        private System.Windows.Forms.Button audioOutputOpenButton;
        private System.Windows.Forms.RadioButton audioTrack2;
        private System.Windows.Forms.RadioButton audioTrack1;
        private System.Windows.Forms.Button deleteAudioButton;
        private System.Windows.Forms.Button configAudioButton;
        private System.Windows.Forms.Button autoEncodeButton;
        private System.Windows.Forms.ComboBox audioCodec;
        private System.Windows.Forms.Label audioCodecLabel;
        private System.Windows.Forms.MenuItem mnuMuxers;
        private MenuItem mnuToolsOneClick;
        private MenuItem mnuToolsOneClickConfig;
        private MenuItem mnuFileOpen;
        private System.Windows.Forms.MenuItem mnuQuantEditor;
        private System.Windows.Forms.MenuItem mnuAVCLevelValidation;
        private ContextMenuStrip trayMenu;
        private ToolStripMenuItem openMeGUIToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private ToolStripMenuItem abortToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitMeGUIToolStripMenuItem;
        private MenuItem mnuUpdate;

        private CodecManager codecs;
        private ComboBox audioContainer;
        private Label audioContainerLabel;
        private MenuItem mnuFileImport;
        private MenuItem mnuFileExport;
        private Button clearLogButton;
        private MenuItem mnuToolsAdaptiveMuxer;
        private TabPage queueTab;
        private CheckBox shutdownCheckBox;
        private Button pauseButton;
        private Button updateJobButton;
        private Button deleteAllJobsButton;
        private ProgressBar jobProgress;
        private Label progressLabel;
        private Button loadJobButton;
        private Button abortButton;
        private Button startStopButton;
        private Button deleteJobButton;
        private Button downButton;
        private Button upButton;
        private ListView queueListView;
        private ColumnHeader jobColumHeader;
        private ColumnHeader inputColumnHeader;
        private ColumnHeader outputColumnHeader;
        private ColumnHeader codecHeader;
        private ColumnHeader modeHeader;
        private ColumnHeader statusColumn;
        private ColumnHeader startColumn;
        private ColumnHeader endColumn;
        private ColumnHeader fpsColumn;
        private MenuItem mnuHelp;
        private MenuItem mnuGuide;
        private MenuItem mnuChangelog;
        private MenuItem mnuHelpLink;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        #endregion
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
            this.audioIOGroupBox = new System.Windows.Forms.GroupBox();
            this.audioContainer = new System.Windows.Forms.ComboBox();
            this.audioContainerLabel = new System.Windows.Forms.Label();
            this.audioCodecLabel = new System.Windows.Forms.Label();
            this.queueAudioButton = new System.Windows.Forms.Button();
            this.audioProfile = new System.Windows.Forms.ComboBox();
            this.audioProfileLabel = new System.Windows.Forms.Label();
            this.audioInput = new System.Windows.Forms.TextBox();
            this.audioInputOpenButton = new System.Windows.Forms.Button();
            this.audioInputLabel = new System.Windows.Forms.Label();
            this.audioOutput = new System.Windows.Forms.TextBox();
            this.audioOutputLabel = new System.Windows.Forms.Label();
            this.audioOutputOpenButton = new System.Windows.Forms.Button();
            this.audioTrack2 = new System.Windows.Forms.RadioButton();
            this.audioTrack1 = new System.Windows.Forms.RadioButton();
            this.deleteAudioButton = new System.Windows.Forms.Button();
            this.configAudioButton = new System.Windows.Forms.Button();
            this.audioCodec = new System.Windows.Forms.ComboBox();
            this.videoGroupBox = new System.Windows.Forms.GroupBox();
            this.addAnalysisPass = new System.Windows.Forms.Button();
            this.addPrerenderJob = new System.Windows.Forms.CheckBox();
            this.queueVideoButton = new System.Windows.Forms.Button();
            this.videoInput = new System.Windows.Forms.TextBox();
            this.inputOpenButton = new System.Windows.Forms.Button();
            this.videoInputLabel = new System.Windows.Forms.Label();
            this.videoCodecLabel = new System.Windows.Forms.Label();
            this.videoContainerLabel = new System.Windows.Forms.Label();
            this.fileType = new System.Windows.Forms.ComboBox();
            this.videoCodec = new System.Windows.Forms.ComboBox();
            this.videoProfileLabel = new System.Windows.Forms.Label();
            this.videoProfile = new System.Windows.Forms.ComboBox();
            this.videoOutput = new System.Windows.Forms.TextBox();
            this.videoOutputLabel = new System.Windows.Forms.Label();
            this.videoOutputOpenButton = new System.Windows.Forms.Button();
            this.videoConfigButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.queueTab = new System.Windows.Forms.TabPage();
            this.shutdownCheckBox = new System.Windows.Forms.CheckBox();
            this.pauseButton = new System.Windows.Forms.Button();
            this.updateJobButton = new System.Windows.Forms.Button();
            this.deleteAllJobsButton = new System.Windows.Forms.Button();
            this.jobProgress = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.loadJobButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            this.startStopButton = new System.Windows.Forms.Button();
            this.deleteJobButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.queueListView = new System.Windows.Forms.ListView();
            this.jobColumHeader = new System.Windows.Forms.ColumnHeader();
            this.inputColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.outputColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.codecHeader = new System.Windows.Forms.ColumnHeader();
            this.modeHeader = new System.Windows.Forms.ColumnHeader();
            this.statusColumn = new System.Windows.Forms.ColumnHeader();
            this.startColumn = new System.Windows.Forms.ColumnHeader();
            this.endColumn = new System.Windows.Forms.ColumnHeader();
            this.fpsColumn = new System.Windows.Forms.ColumnHeader();
            this.queueContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PostponedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WaitingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logTab = new System.Windows.Forms.TabPage();
            this.clearLogButton = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.TextBox();
            this.mnuUpdate = new System.Windows.Forms.MenuItem();
            this.mnuToolsAviSynth = new System.Windows.Forms.MenuItem();
            this.mnuChapterCreator = new System.Windows.Forms.MenuItem();
            this.mnuToolsD2VCreator = new System.Windows.Forms.MenuItem();
            this.mnuQuantEditor = new System.Windows.Forms.MenuItem();
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
            this.mnuToolsBitrateCalculator = new System.Windows.Forms.MenuItem();
            this.mnuToolsOneClick = new System.Windows.Forms.MenuItem();
            this.mnuToolsOneClickConfig = new System.Windows.Forms.MenuItem();
            this.mnuToolsSettings = new System.Windows.Forms.MenuItem();
            this.mnuAVCLevelValidation = new System.Windows.Forms.MenuItem();
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
            this.tabControl1.SuspendLayout();
            this.inputTab.SuspendLayout();
            this.audioIOGroupBox.SuspendLayout();
            this.videoGroupBox.SuspendLayout();
            this.queueTab.SuspendLayout();
            this.queueContextMenu.SuspendLayout();
            this.logTab.SuspendLayout();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.inputTab);
            this.tabControl1.Controls.Add(this.queueTab);
            this.tabControl1.Controls.Add(this.logTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(480, 406);
            this.tabControl1.TabIndex = 0;
            // 
            // inputTab
            // 
            this.inputTab.Controls.Add(this.autoEncodeButton);
            this.inputTab.Controls.Add(this.audioIOGroupBox);
            this.inputTab.Controls.Add(this.videoGroupBox);
            this.inputTab.Controls.Add(this.resetButton);
            this.inputTab.Location = new System.Drawing.Point(4, 22);
            this.inputTab.Name = "inputTab";
            this.inputTab.Size = new System.Drawing.Size(472, 380);
            this.inputTab.TabIndex = 0;
            this.inputTab.Text = "Input";
            // 
            // autoEncodeButton
            // 
            this.autoEncodeButton.Location = new System.Drawing.Point(376, 330);
            this.autoEncodeButton.Name = "autoEncodeButton";
            this.autoEncodeButton.Size = new System.Drawing.Size(80, 23);
            this.autoEncodeButton.TabIndex = 6;
            this.autoEncodeButton.Text = "AutoEncode";
            this.autoEncodeButton.Click += new System.EventHandler(this.autoEncodeButton_Click);
            // 
            // audioIOGroupBox
            // 
            this.audioIOGroupBox.Controls.Add(this.audioContainer);
            this.audioIOGroupBox.Controls.Add(this.audioContainerLabel);
            this.audioIOGroupBox.Controls.Add(this.audioCodecLabel);
            this.audioIOGroupBox.Controls.Add(this.queueAudioButton);
            this.audioIOGroupBox.Controls.Add(this.audioProfile);
            this.audioIOGroupBox.Controls.Add(this.audioProfileLabel);
            this.audioIOGroupBox.Controls.Add(this.audioInput);
            this.audioIOGroupBox.Controls.Add(this.audioInputOpenButton);
            this.audioIOGroupBox.Controls.Add(this.audioInputLabel);
            this.audioIOGroupBox.Controls.Add(this.audioOutput);
            this.audioIOGroupBox.Controls.Add(this.audioOutputLabel);
            this.audioIOGroupBox.Controls.Add(this.audioOutputOpenButton);
            this.audioIOGroupBox.Controls.Add(this.audioTrack2);
            this.audioIOGroupBox.Controls.Add(this.audioTrack1);
            this.audioIOGroupBox.Controls.Add(this.deleteAudioButton);
            this.audioIOGroupBox.Controls.Add(this.configAudioButton);
            this.audioIOGroupBox.Controls.Add(this.audioCodec);
            this.audioIOGroupBox.Location = new System.Drawing.Point(8, 173);
            this.audioIOGroupBox.Name = "audioIOGroupBox";
            this.audioIOGroupBox.Size = new System.Drawing.Size(456, 136);
            this.audioIOGroupBox.TabIndex = 4;
            this.audioIOGroupBox.TabStop = false;
            this.audioIOGroupBox.Text = "Audio";
            // 
            // audioContainer
            // 
            this.audioContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioContainer.FormattingEnabled = true;
            this.audioContainer.Location = new System.Drawing.Point(339, 74);
            this.audioContainer.Name = "audioContainer";
            this.audioContainer.Size = new System.Drawing.Size(70, 21);
            this.audioContainer.TabIndex = 7;
            this.audioContainer.SelectedIndexChanged += new System.EventHandler(this.audioContainer_SelectedIndexChanged);
            // 
            // audioContainerLabel
            // 
            this.audioContainerLabel.AutoSize = true;
            this.audioContainerLabel.Location = new System.Drawing.Point(286, 78);
            this.audioContainerLabel.Name = "audioContainerLabel";
            this.audioContainerLabel.Size = new System.Drawing.Size(54, 13);
            this.audioContainerLabel.TabIndex = 32;
            this.audioContainerLabel.Text = "Container";
            // 
            // audioCodecLabel
            // 
            this.audioCodecLabel.Location = new System.Drawing.Point(8, 76);
            this.audioCodecLabel.Name = "audioCodecLabel";
            this.audioCodecLabel.Size = new System.Drawing.Size(100, 23);
            this.audioCodecLabel.TabIndex = 31;
            this.audioCodecLabel.Text = "Codec";
            this.audioCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // queueAudioButton
            // 
            this.queueAudioButton.Location = new System.Drawing.Point(374, 100);
            this.queueAudioButton.Name = "queueAudioButton";
            this.queueAudioButton.Size = new System.Drawing.Size(66, 23);
            this.queueAudioButton.TabIndex = 30;
            this.queueAudioButton.Text = "Enqueue";
            this.queueAudioButton.Click += new System.EventHandler(this.queueAudioButton_Click);
            // 
            // audioProfile
            // 
            this.audioProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioProfile.Location = new System.Drawing.Point(152, 101);
            this.audioProfile.Name = "audioProfile";
            this.audioProfile.Size = new System.Drawing.Size(200, 21);
            this.audioProfile.Sorted = true;
            this.audioProfile.TabIndex = 15;
            this.audioProfile.SelectedIndexChanged += new System.EventHandler(this.audioProfile_SelectedIndexChanged);
            // 
            // audioProfileLabel
            // 
            this.audioProfileLabel.Location = new System.Drawing.Point(8, 102);
            this.audioProfileLabel.Name = "audioProfileLabel";
            this.audioProfileLabel.Size = new System.Drawing.Size(100, 23);
            this.audioProfileLabel.TabIndex = 14;
            this.audioProfileLabel.Text = "Audio Profile";
            // 
            // audioInput
            // 
            this.audioInput.Location = new System.Drawing.Point(152, 22);
            this.audioInput.Name = "audioInput";
            this.audioInput.ReadOnly = true;
            this.audioInput.Size = new System.Drawing.Size(256, 21);
            this.audioInput.TabIndex = 6;
            // 
            // audioInputOpenButton
            // 
            this.audioInputOpenButton.Location = new System.Drawing.Point(416, 22);
            this.audioInputOpenButton.Name = "audioInputOpenButton";
            this.audioInputOpenButton.Size = new System.Drawing.Size(24, 23);
            this.audioInputOpenButton.TabIndex = 7;
            this.audioInputOpenButton.Text = "...";
            this.audioInputOpenButton.Click += new System.EventHandler(this.audioInputOpenButton_Click);
            // 
            // audioInputLabel
            // 
            this.audioInputLabel.Location = new System.Drawing.Point(8, 24);
            this.audioInputLabel.Name = "audioInputLabel";
            this.audioInputLabel.Size = new System.Drawing.Size(100, 23);
            this.audioInputLabel.TabIndex = 5;
            this.audioInputLabel.Text = "Audio Input";
            // 
            // audioOutput
            // 
            this.audioOutput.Location = new System.Drawing.Point(152, 48);
            this.audioOutput.Name = "audioOutput";
            this.audioOutput.Size = new System.Drawing.Size(256, 21);
            this.audioOutput.TabIndex = 8;
            // 
            // audioOutputLabel
            // 
            this.audioOutputLabel.Location = new System.Drawing.Point(8, 50);
            this.audioOutputLabel.Name = "audioOutputLabel";
            this.audioOutputLabel.Size = new System.Drawing.Size(100, 23);
            this.audioOutputLabel.TabIndex = 9;
            this.audioOutputLabel.Text = "Audio Output";
            // 
            // audioOutputOpenButton
            // 
            this.audioOutputOpenButton.Location = new System.Drawing.Point(416, 48);
            this.audioOutputOpenButton.Name = "audioOutputOpenButton";
            this.audioOutputOpenButton.Size = new System.Drawing.Size(24, 23);
            this.audioOutputOpenButton.TabIndex = 13;
            this.audioOutputOpenButton.Text = "...";
            this.audioOutputOpenButton.Click += new System.EventHandler(this.audioOutputOpenButton_Click);
            // 
            // audioTrack2
            // 
            this.audioTrack2.Location = new System.Drawing.Point(75, 0);
            this.audioTrack2.Name = "audioTrack2";
            this.audioTrack2.Size = new System.Drawing.Size(24, 16);
            this.audioTrack2.TabIndex = 20;
            this.audioTrack2.Text = "2";
            this.audioTrack2.CheckedChanged += new System.EventHandler(this.audioTrack_CheckedChanged);
            // 
            // audioTrack1
            // 
            this.audioTrack1.Checked = true;
            this.audioTrack1.Location = new System.Drawing.Point(48, 0);
            this.audioTrack1.Name = "audioTrack1";
            this.audioTrack1.Size = new System.Drawing.Size(24, 16);
            this.audioTrack1.TabIndex = 19;
            this.audioTrack1.TabStop = true;
            this.audioTrack1.Text = "1";
            this.audioTrack1.CheckedChanged += new System.EventHandler(this.audioTrack_CheckedChanged);
            // 
            // deleteAudioButton
            // 
            this.deleteAudioButton.Location = new System.Drawing.Point(416, 74);
            this.deleteAudioButton.Name = "deleteAudioButton";
            this.deleteAudioButton.Size = new System.Drawing.Size(24, 23);
            this.deleteAudioButton.TabIndex = 6;
            this.deleteAudioButton.Text = "X";
            this.deleteAudioButton.Click += new System.EventHandler(this.deleteAudioButton_Click);
            // 
            // configAudioButton
            // 
            this.configAudioButton.Location = new System.Drawing.Point(228, 73);
            this.configAudioButton.Name = "configAudioButton";
            this.configAudioButton.Size = new System.Drawing.Size(56, 23);
            this.configAudioButton.TabIndex = 26;
            this.configAudioButton.Text = "Config";
            this.configAudioButton.Click += new System.EventHandler(this.configAudioButton_Click);
            // 
            // audioCodec
            // 
            this.audioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioCodec.Location = new System.Drawing.Point(152, 74);
            this.audioCodec.Name = "audioCodec";
            this.audioCodec.Size = new System.Drawing.Size(70, 21);
            this.audioCodec.TabIndex = 7;
            this.audioCodec.SelectedIndexChanged += new System.EventHandler(this.audioCodec_SelectedIndexChanged);
            // 
            // videoGroupBox
            // 
            this.videoGroupBox.Controls.Add(this.addAnalysisPass);
            this.videoGroupBox.Controls.Add(this.addPrerenderJob);
            this.videoGroupBox.Controls.Add(this.queueVideoButton);
            this.videoGroupBox.Controls.Add(this.videoInput);
            this.videoGroupBox.Controls.Add(this.inputOpenButton);
            this.videoGroupBox.Controls.Add(this.videoInputLabel);
            this.videoGroupBox.Controls.Add(this.videoCodecLabel);
            this.videoGroupBox.Controls.Add(this.videoContainerLabel);
            this.videoGroupBox.Controls.Add(this.fileType);
            this.videoGroupBox.Controls.Add(this.videoCodec);
            this.videoGroupBox.Controls.Add(this.videoProfileLabel);
            this.videoGroupBox.Controls.Add(this.videoProfile);
            this.videoGroupBox.Controls.Add(this.videoOutput);
            this.videoGroupBox.Controls.Add(this.videoOutputLabel);
            this.videoGroupBox.Controls.Add(this.videoOutputOpenButton);
            this.videoGroupBox.Controls.Add(this.videoConfigButton);
            this.videoGroupBox.Location = new System.Drawing.Point(8, 8);
            this.videoGroupBox.Name = "videoGroupBox";
            this.videoGroupBox.Size = new System.Drawing.Size(456, 159);
            this.videoGroupBox.TabIndex = 1;
            this.videoGroupBox.TabStop = false;
            this.videoGroupBox.Text = "Video";
            // 
            // addAnalysisPass
            // 
            this.addAnalysisPass.Location = new System.Drawing.Point(19, 128);
            this.addAnalysisPass.Name = "addAnalysisPass";
            this.addAnalysisPass.Size = new System.Drawing.Size(133, 23);
            this.addAnalysisPass.TabIndex = 17;
            this.addAnalysisPass.Text = "Queue analysis pass";
            this.addAnalysisPass.UseVisualStyleBackColor = true;
            this.addAnalysisPass.Click += new System.EventHandler(this.addAnalysisPass_Click);
            // 
            // addPrerenderJob
            // 
            this.addPrerenderJob.AutoSize = true;
            this.addPrerenderJob.Location = new System.Drawing.Point(239, 132);
            this.addPrerenderJob.Name = "addPrerenderJob";
            this.addPrerenderJob.Size = new System.Drawing.Size(132, 17);
            this.addPrerenderJob.TabIndex = 16;
            this.addPrerenderJob.Text = "Add pre-rendering job";
            this.addPrerenderJob.UseVisualStyleBackColor = true;
            // 
            // queueVideoButton
            // 
            this.queueVideoButton.Location = new System.Drawing.Point(380, 128);
            this.queueVideoButton.Name = "queueVideoButton";
            this.queueVideoButton.Size = new System.Drawing.Size(62, 23);
            this.queueVideoButton.TabIndex = 15;
            this.queueVideoButton.Text = "Enqueue";
            this.queueVideoButton.Click += new System.EventHandler(this.queueVideoButton_Click);
            // 
            // videoInput
            // 
            this.videoInput.Location = new System.Drawing.Point(152, 22);
            this.videoInput.Name = "videoInput";
            this.videoInput.ReadOnly = true;
            this.videoInput.Size = new System.Drawing.Size(256, 21);
            this.videoInput.TabIndex = 3;
            this.videoInput.DoubleClick += new System.EventHandler(this.videoInput_DoubleClick);
            // 
            // inputOpenButton
            // 
            this.inputOpenButton.Location = new System.Drawing.Point(418, 22);
            this.inputOpenButton.Name = "inputOpenButton";
            this.inputOpenButton.Size = new System.Drawing.Size(24, 23);
            this.inputOpenButton.TabIndex = 4;
            this.inputOpenButton.Text = "...";
            this.inputOpenButton.Click += new System.EventHandler(this.inputOpenButton_Click);
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.Location = new System.Drawing.Point(16, 24);
            this.videoInputLabel.Name = "videoInputLabel";
            this.videoInputLabel.Size = new System.Drawing.Size(100, 23);
            this.videoInputLabel.TabIndex = 0;
            this.videoInputLabel.Text = "AviSynth Script";
            // 
            // videoCodecLabel
            // 
            this.videoCodecLabel.Location = new System.Drawing.Point(16, 75);
            this.videoCodecLabel.Name = "videoCodecLabel";
            this.videoCodecLabel.Size = new System.Drawing.Size(100, 23);
            this.videoCodecLabel.TabIndex = 7;
            this.videoCodecLabel.Text = "Codec";
            this.videoCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // videoContainerLabel
            // 
            this.videoContainerLabel.Location = new System.Drawing.Point(330, 78);
            this.videoContainerLabel.Name = "videoContainerLabel";
            this.videoContainerLabel.Size = new System.Drawing.Size(54, 13);
            this.videoContainerLabel.TabIndex = 1;
            this.videoContainerLabel.Text = "Container";
            this.videoContainerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileType
            // 
            this.fileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileType.Location = new System.Drawing.Point(387, 74);
            this.fileType.Name = "fileType";
            this.fileType.Size = new System.Drawing.Size(56, 21);
            this.fileType.TabIndex = 4;
            this.fileType.SelectedIndexChanged += new System.EventHandler(this.fileType_SelectedIndexChanged);
            // 
            // videoCodec
            // 
            this.videoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoCodec.Location = new System.Drawing.Point(152, 74);
            this.videoCodec.Name = "videoCodec";
            this.videoCodec.Size = new System.Drawing.Size(70, 21);
            this.videoCodec.TabIndex = 8;
            this.videoCodec.SelectedIndexChanged += new System.EventHandler(this.codec_SelectedIndexChanged);
            // 
            // videoProfileLabel
            // 
            this.videoProfileLabel.Location = new System.Drawing.Point(16, 104);
            this.videoProfileLabel.Name = "videoProfileLabel";
            this.videoProfileLabel.Size = new System.Drawing.Size(92, 23);
            this.videoProfileLabel.TabIndex = 7;
            this.videoProfileLabel.Text = "Video Profile";
            // 
            // videoProfile
            // 
            this.videoProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoProfile.Location = new System.Drawing.Point(153, 101);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.Size = new System.Drawing.Size(290, 21);
            this.videoProfile.Sorted = true;
            this.videoProfile.TabIndex = 8;
            this.videoProfile.SelectedIndexChanged += new System.EventHandler(this.videoProfile_SelectedIndexChanged);
            // 
            // videoOutput
            // 
            this.videoOutput.Location = new System.Drawing.Point(152, 48);
            this.videoOutput.Name = "videoOutput";
            this.videoOutput.Size = new System.Drawing.Size(256, 21);
            this.videoOutput.TabIndex = 13;
            // 
            // videoOutputLabel
            // 
            this.videoOutputLabel.Location = new System.Drawing.Point(16, 53);
            this.videoOutputLabel.Name = "videoOutputLabel";
            this.videoOutputLabel.Size = new System.Drawing.Size(100, 16);
            this.videoOutputLabel.TabIndex = 13;
            this.videoOutputLabel.Text = "Video Output";
            // 
            // videoOutputOpenButton
            // 
            this.videoOutputOpenButton.Location = new System.Drawing.Point(418, 48);
            this.videoOutputOpenButton.Name = "videoOutputOpenButton";
            this.videoOutputOpenButton.Size = new System.Drawing.Size(24, 23);
            this.videoOutputOpenButton.TabIndex = 13;
            this.videoOutputOpenButton.Text = "...";
            this.videoOutputOpenButton.Click += new System.EventHandler(this.videoOutputOpenButton_Click);
            // 
            // videoConfigButton
            // 
            this.videoConfigButton.Location = new System.Drawing.Point(228, 73);
            this.videoConfigButton.Name = "videoConfigButton";
            this.videoConfigButton.Size = new System.Drawing.Size(56, 23);
            this.videoConfigButton.TabIndex = 14;
            this.videoConfigButton.Text = "Config";
            this.videoConfigButton.Click += new System.EventHandler(this.videoConfigButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(312, 330);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(48, 23);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Reset";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // queueTab
            // 
            this.queueTab.Controls.Add(this.shutdownCheckBox);
            this.queueTab.Controls.Add(this.pauseButton);
            this.queueTab.Controls.Add(this.updateJobButton);
            this.queueTab.Controls.Add(this.deleteAllJobsButton);
            this.queueTab.Controls.Add(this.jobProgress);
            this.queueTab.Controls.Add(this.progressLabel);
            this.queueTab.Controls.Add(this.loadJobButton);
            this.queueTab.Controls.Add(this.abortButton);
            this.queueTab.Controls.Add(this.startStopButton);
            this.queueTab.Controls.Add(this.deleteJobButton);
            this.queueTab.Controls.Add(this.downButton);
            this.queueTab.Controls.Add(this.upButton);
            this.queueTab.Controls.Add(this.queueListView);
            this.queueTab.Location = new System.Drawing.Point(4, 22);
            this.queueTab.Name = "queueTab";
            this.queueTab.Size = new System.Drawing.Size(472, 380);
            this.queueTab.TabIndex = 5;
            this.queueTab.Text = "Queue";
            // 
            // shutdownCheckBox
            // 
            this.shutdownCheckBox.AutoSize = true;
            this.shutdownCheckBox.Location = new System.Drawing.Point(6, 321);
            this.shutdownCheckBox.Name = "shutdownCheckBox";
            this.shutdownCheckBox.Size = new System.Drawing.Size(167, 17);
            this.shutdownCheckBox.TabIndex = 14;
            this.shutdownCheckBox.Text = "Shutdown at end of encoding";
            this.shutdownCheckBox.UseVisualStyleBackColor = true;
            this.shutdownCheckBox.CheckedChanged += new System.EventHandler(this.shutdownCheckBox_CheckedChanged);
            // 
            // pauseButton
            // 
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(52, 288);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(24, 24);
            this.pauseButton.TabIndex = 13;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // updateJobButton
            // 
            this.updateJobButton.Location = new System.Drawing.Point(184, 289);
            this.updateJobButton.Name = "updateJobButton";
            this.updateJobButton.Size = new System.Drawing.Size(50, 23);
            this.updateJobButton.TabIndex = 12;
            this.updateJobButton.Text = "Update";
            this.updateJobButton.Click += new System.EventHandler(this.updateJobButton_Click);
            // 
            // deleteAllJobsButton
            // 
            this.deleteAllJobsButton.Location = new System.Drawing.Point(362, 288);
            this.deleteAllJobsButton.Name = "deleteAllJobsButton";
            this.deleteAllJobsButton.Size = new System.Drawing.Size(48, 23);
            this.deleteAllJobsButton.TabIndex = 11;
            this.deleteAllJobsButton.Text = "Clear";
            this.deleteAllJobsButton.Click += new System.EventHandler(this.deleteAllJobsButton_Click);
            // 
            // jobProgress
            // 
            this.jobProgress.Location = new System.Drawing.Point(59, 344);
            this.jobProgress.Name = "jobProgress";
            this.jobProgress.Size = new System.Drawing.Size(405, 23);
            this.jobProgress.TabIndex = 8;
            // 
            // progressLabel
            // 
            this.progressLabel.Location = new System.Drawing.Point(3, 348);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(50, 15);
            this.progressLabel.TabIndex = 7;
            this.progressLabel.Text = "Progress";
            // 
            // loadJobButton
            // 
            this.loadJobButton.Location = new System.Drawing.Point(138, 288);
            this.loadJobButton.Name = "loadJobButton";
            this.loadJobButton.Size = new System.Drawing.Size(40, 23);
            this.loadJobButton.TabIndex = 6;
            this.loadJobButton.Text = "Load";
            this.loadJobButton.Click += new System.EventHandler(this.loadJobButton_Click);
            // 
            // abortButton
            // 
            this.abortButton.Enabled = false;
            this.abortButton.Location = new System.Drawing.Point(82, 288);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(42, 23);
            this.abortButton.TabIndex = 5;
            this.abortButton.Text = "Abort";
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // startStopButton
            // 
            this.startStopButton.Location = new System.Drawing.Point(6, 288);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(40, 23);
            this.startStopButton.TabIndex = 4;
            this.startStopButton.Text = "Start";
            this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
            // 
            // deleteJobButton
            // 
            this.deleteJobButton.Location = new System.Drawing.Point(416, 288);
            this.deleteJobButton.Name = "deleteJobButton";
            this.deleteJobButton.Size = new System.Drawing.Size(48, 23);
            this.deleteJobButton.TabIndex = 3;
            this.deleteJobButton.Text = "Delete";
            this.deleteJobButton.Click += new System.EventHandler(this.deleteJobButton_Click);
            // 
            // downButton
            // 
            this.downButton.Location = new System.Drawing.Point(296, 288);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(44, 23);
            this.downButton.TabIndex = 2;
            this.downButton.Text = "Down";
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.Location = new System.Drawing.Point(250, 288);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(40, 24);
            this.upButton.TabIndex = 1;
            this.upButton.Text = "Up";
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // queueListView
            // 
            this.queueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.jobColumHeader,
            this.inputColumnHeader,
            this.outputColumnHeader,
            this.codecHeader,
            this.modeHeader,
            this.statusColumn,
            this.startColumn,
            this.endColumn,
            this.fpsColumn});
            this.queueListView.ContextMenuStrip = this.queueContextMenu;
            this.queueListView.FullRowSelect = true;
            this.queueListView.HideSelection = false;
            this.queueListView.Location = new System.Drawing.Point(0, 16);
            this.queueListView.Name = "queueListView";
            this.queueListView.Size = new System.Drawing.Size(464, 256);
            this.queueListView.TabIndex = 0;
            this.queueListView.UseCompatibleStateImageBehavior = false;
            this.queueListView.View = System.Windows.Forms.View.Details;
            this.queueListView.DoubleClick += new System.EventHandler(this.queueListView_DoubleClick);
            // 
            // jobColumHeader
            // 
            this.jobColumHeader.Text = "Name";
            this.jobColumHeader.Width = 40;
            // 
            // inputColumnHeader
            // 
            this.inputColumnHeader.Text = "Input";
            this.inputColumnHeader.Width = 89;
            // 
            // outputColumnHeader
            // 
            this.outputColumnHeader.Text = "Output";
            this.outputColumnHeader.Width = 89;
            // 
            // codecHeader
            // 
            this.codecHeader.Text = "Codec";
            this.codecHeader.Width = 43;
            // 
            // modeHeader
            // 
            this.modeHeader.Text = "Mode";
            this.modeHeader.Width = 75;
            // 
            // statusColumn
            // 
            this.statusColumn.Text = "Status";
            this.statusColumn.Width = 51;
            // 
            // startColumn
            // 
            this.startColumn.Text = "Start";
            this.startColumn.Width = 55;
            // 
            // endColumn
            // 
            this.endColumn.Text = "End";
            this.endColumn.Width = 55;
            // 
            // fpsColumn
            // 
            this.fpsColumn.Text = "FPS";
            this.fpsColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fpsColumn.Width = 35;
            // 
            // queueContextMenu
            // 
            this.queueContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteMenuItem,
            this.StatusMenuItem,
            this.AbortMenuItem,
            this.LoadMenuItem});
            this.queueContextMenu.Name = "queueContextMenu";
            this.queueContextMenu.Size = new System.Drawing.Size(156, 92);
            this.queueContextMenu.Opened += new System.EventHandler(this.queueContextMenu_Opened);
            // 
            // DeleteMenuItem
            // 
            this.DeleteMenuItem.Name = "DeleteMenuItem";
            this.DeleteMenuItem.ShortcutKeyDisplayString = "";
            this.DeleteMenuItem.Size = new System.Drawing.Size(155, 22);
            this.DeleteMenuItem.Text = "&Delete";
            this.DeleteMenuItem.ToolTipText = "Delete this job";
            this.DeleteMenuItem.Click += new System.EventHandler(this.deleteMenuItem_Click);
            // 
            // StatusMenuItem
            // 
            this.StatusMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PostponedMenuItem,
            this.WaitingMenuItem});
            this.StatusMenuItem.Name = "StatusMenuItem";
            this.StatusMenuItem.Size = new System.Drawing.Size(155, 22);
            this.StatusMenuItem.Text = "&Change status";
            // 
            // PostponedMenuItem
            // 
            this.PostponedMenuItem.Name = "PostponedMenuItem";
            this.PostponedMenuItem.Size = new System.Drawing.Size(136, 22);
            this.PostponedMenuItem.Text = "&Postponed";
            this.PostponedMenuItem.Click += new System.EventHandler(this.postponeMenuItem_Click);
            // 
            // WaitingMenuItem
            // 
            this.WaitingMenuItem.Name = "WaitingMenuItem";
            this.WaitingMenuItem.Size = new System.Drawing.Size(136, 22);
            this.WaitingMenuItem.Text = "&Waiting";
            this.WaitingMenuItem.Click += new System.EventHandler(this.waitingMenuItem_Click);
            // 
            // AbortMenuItem
            // 
            this.AbortMenuItem.Name = "AbortMenuItem";
            this.AbortMenuItem.ShortcutKeyDisplayString = "";
            this.AbortMenuItem.Size = new System.Drawing.Size(155, 22);
            this.AbortMenuItem.Text = "&Abort";
            this.AbortMenuItem.ToolTipText = "Abort this job";
            this.AbortMenuItem.Click += new System.EventHandler(this.abortMenuItem_Click);
            // 
            // LoadMenuItem
            // 
            this.LoadMenuItem.Name = "LoadMenuItem";
            this.LoadMenuItem.ShortcutKeyDisplayString = "";
            this.LoadMenuItem.Size = new System.Drawing.Size(155, 22);
            this.LoadMenuItem.Text = "&Load";
            this.LoadMenuItem.ToolTipText = "Load into MeGUI";
            this.LoadMenuItem.Click += new System.EventHandler(this.loadMenuItem_Click);
            // 
            // logTab
            // 
            this.logTab.Controls.Add(this.clearLogButton);
            this.logTab.Controls.Add(this.log);
            this.logTab.Location = new System.Drawing.Point(4, 22);
            this.logTab.Name = "logTab";
            this.logTab.Size = new System.Drawing.Size(472, 380);
            this.logTab.TabIndex = 10;
            this.logTab.Text = "Log";
            // 
            // clearLogButton
            // 
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
            this.log.Size = new System.Drawing.Size(472, 348);
            this.log.TabIndex = 0;
            // 
            // mnuUpdate
            // 
            this.mnuUpdate.Index = 9;
            this.mnuUpdate.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.mnuUpdate.Text = "&Update";
            this.mnuUpdate.Click += new System.EventHandler(this.mnuUpdate_Click);
            // 
            // mnuToolsAviSynth
            // 
            this.mnuToolsAviSynth.Index = 0;
            this.mnuToolsAviSynth.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.mnuToolsAviSynth.Text = "AviSynth Script C&reator";
            this.mnuToolsAviSynth.Click += new System.EventHandler(this.mnuToolsAviSynth_Click);
            // 
            // mnuChapterCreator
            // 
            this.mnuChapterCreator.Index = 3;
            this.mnuChapterCreator.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            this.mnuChapterCreator.Text = "C&hapter Creator";
            this.mnuChapterCreator.Click += new System.EventHandler(this.mnuChapterCreator_Click);
            // 
            // mnuToolsD2VCreator
            // 
            this.mnuToolsD2VCreator.Index = 6;
            this.mnuToolsD2VCreator.Shortcut = System.Windows.Forms.Shortcut.Ctrl2;
            this.mnuToolsD2VCreator.Text = "D&2V Creator";
            this.mnuToolsD2VCreator.Click += new System.EventHandler(this.mnuToolsD2VCreator_Click);
            // 
            // mnuQuantEditor
            // 
            this.mnuQuantEditor.Index = 1;
            this.mnuQuantEditor.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
            this.mnuQuantEditor.Text = "AVC &Quant Matrix Editor";
            this.mnuQuantEditor.Click += new System.EventHandler(this.mnuQuantEditor_Click);
            // 
            // mnuMuxers
            // 
            this.mnuMuxers.Index = 7;
            this.mnuMuxers.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuToolsAdaptiveMuxer});
            this.mnuMuxers.Text = "&Muxer";
            // 
            // mnuToolsAdaptiveMuxer
            // 
            this.mnuToolsAdaptiveMuxer.Index = 0;
            this.mnuToolsAdaptiveMuxer.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.mnuToolsAdaptiveMuxer.Text = "Adaptive &Muxer";
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
            this.mnuToolsAviSynth,
            this.mnuQuantEditor,
            this.mnuToolsBitrateCalculator,
            this.mnuChapterCreator,
            this.mnuToolsOneClick,
            this.mnuToolsOneClickConfig,
            this.mnuToolsD2VCreator,
            this.mnuMuxers,
            this.mnuToolsSettings,
            this.mnuUpdate,
            this.mnuAVCLevelValidation});
            this.mnuTools.Text = "&Tools";
            // 
            // mnuToolsBitrateCalculator
            // 
            this.mnuToolsBitrateCalculator.Index = 2;
            this.mnuToolsBitrateCalculator.Shortcut = System.Windows.Forms.Shortcut.CtrlB;
            this.mnuToolsBitrateCalculator.Text = "&Bitrate Calculator";
            this.mnuToolsBitrateCalculator.Click += new System.EventHandler(this.mnuToolsBitrateCalculator_Click);
            // 
            // mnuToolsOneClick
            // 
            this.mnuToolsOneClick.Index = 4;
            this.mnuToolsOneClick.Shortcut = System.Windows.Forms.Shortcut.Ctrl1;
            this.mnuToolsOneClick.Text = "One Click Encoder";
            this.mnuToolsOneClick.Click += new System.EventHandler(this.mnuToolsOneClick_Click);
            // 
            // mnuToolsOneClickConfig
            // 
            this.mnuToolsOneClickConfig.Index = 5;
            this.mnuToolsOneClickConfig.Text = "One Click Profile Setup";
            this.mnuToolsOneClickConfig.Click += new System.EventHandler(this.mnuToolsOneClickConfig_Click);
            // 
            // mnuToolsSettings
            // 
            this.mnuToolsSettings.Index = 8;
            this.mnuToolsSettings.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuToolsSettings.Text = "&Settings";
            this.mnuToolsSettings.Click += new System.EventHandler(this.mnuToolsSettings_Click);
            // 
            // mnuAVCLevelValidation
            // 
            this.mnuAVCLevelValidation.Index = 10;
            this.mnuAVCLevelValidation.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            this.mnuAVCLevelValidation.Text = "Validate AVC &Level";
            this.mnuAVCLevelValidation.Click += new System.EventHandler(this.mnuAVCLevelValidation_Click);
            this.mnuAVCLevelValidation.Popup += new System.EventHandler(this.mnuAVCLevelValidation_Popup);
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
            this.trayMenu.Size = new System.Drawing.Size(147, 126);
            // 
            // openMeGUIToolStripMenuItem
            // 
            this.openMeGUIToolStripMenuItem.Name = "openMeGUIToolStripMenuItem";
            this.openMeGUIToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openMeGUIToolStripMenuItem.Text = "Open MeGUI";
            this.openMeGUIToolStripMenuItem.Click += new System.EventHandler(this.openMeGUIToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startStopButton_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // abortToolStripMenuItem
            // 
            this.abortToolStripMenuItem.Name = "abortToolStripMenuItem";
            this.abortToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.abortToolStripMenuItem.Text = "Abort";
            this.abortToolStripMenuItem.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // exitMeGUIToolStripMenuItem
            // 
            this.exitMeGUIToolStripMenuItem.Name = "exitMeGUIToolStripMenuItem";
            this.exitMeGUIToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitMeGUIToolStripMenuItem.Text = "Exit MeGUI";
            this.exitMeGUIToolStripMenuItem.Click += new System.EventHandler(this.exitMeGUIToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(480, 406);
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
            this.audioIOGroupBox.ResumeLayout(false);
            this.audioIOGroupBox.PerformLayout();
            this.videoGroupBox.ResumeLayout(false);
            this.videoGroupBox.PerformLayout();
            this.queueTab.ResumeLayout(false);
            this.queueTab.PerformLayout();
            this.queueContextMenu.ResumeLayout(false);
            this.logTab.ResumeLayout(false);
            this.logTab.PerformLayout();
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        #region start and end
        public void handleCommandline(CommandlineParser parser)
        {
            foreach (string file in parser.failedUpgrades)
                MessageBox.Show("Failed to upgrade '" + file + "'.", "Upgrade failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public MainForm()
        {
            InitializeComponent();

            System.Reflection.Assembly myAssembly = this.GetType().Assembly;
            string name = this.GetType().Namespace + ".";
#if CSC
			name = "";
#endif
            string[] resources = myAssembly.GetManifestResourceNames();

            this.codecs = new CodecManager();
            this.pauseImage = new Bitmap(myAssembly.GetManifestResourceStream(name + "pause.ico"));
            this.playImage = new Bitmap(myAssembly.GetManifestResourceStream(name + "play.ico"));
            this.pauseButton.Image = (Image)pauseImage;
            this.pauseButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.trayIcon.Icon = new Icon(myAssembly.GetManifestResourceStream(name + "App.ico"));
            this.gen = new CommandLineGenerator();
            this.path = Application.StartupPath;
            this.jobs = new Dictionary<string, Job>();
            this.skipJobs = new List<Job>();
            this.logBuilder = new StringBuilder();
            this.jobUtil = new JobUtil(this);
            this.settings = new MeGUISettings();
            this.calc = new BitrateCalculator();
            audioStreams = new AudioStream[2];
            audioStreams[0].path = "";
            audioStreams[0].output = "";
            audioStreams[0].settings = null;
            audioStreams[1].path = "";
            audioStreams[1].output = "";
            audioStreams[1].settings = null;
            this.TitleText = Application.ProductName + " " + Application.ProductVersion;
            this.videoEncoderProvider = new VideoEncoderProvider();
            this.audioEncoderProvider = new AudioEncoderProvider();
            this.initializeDropdowns();
            this.profileManager = new ProfileManager(this.path);
            this.profileManager.LoadProfiles(videoProfile, audioProfile);
            this.loadSettings();
            this.shutdownCheckBox.Checked = this.settings.Shutdown;
            this.loadJobs();
            this.dialogManager = new DialogManager(this);

            int index = mnuMuxers.MenuItems.Count;
            foreach (IMuxing muxer in muxProvider.GetRegisteredMuxers())
            {
                MenuItem newMenuItem = new MenuItem();
                newMenuItem.Text = muxer.Name;
                newMenuItem.Tag = muxer;
                newMenuItem.Index = index;
                index++;
                mnuMuxers.MenuItems.Add(newMenuItem);
                newMenuItem.Click += new System.EventHandler(this.mnuMuxer_Click);
            }

            //MessageBox.Show(String.Join("|", this.GetType().Assembly.GetManifestResourceNames()));
        }

        private static Mutex mySingleInstanceMutex = new Mutex(true, "MeGUI_D9D0C224154B489784998BF97B9C9414");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            if (!mySingleInstanceMutex.WaitOne(0, false))
            {
                if (DialogResult.Yes != MessageBox.Show("Running MeGUI instance detected!\n\rThere's not really much point in running multiple copies of MeGUI, and it can cause problems.\n\rDo You still want to run yet another MeGUI instance?", "Running MeGUI instance detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    return;
            }
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            CommandlineParser parser = new CommandlineParser();
            parser.Parse(args);
            MainForm mainForm = new MainForm();
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
        /// <summary>
        /// initializes all the dropdown elements in the GUI to their default values
        /// </summary>
        private void initializeDropdowns()
        {
            this.videoCodec.Items.AddRange(CodecManager.ListOfVideoCodecs);
            this.videoCodec.SelectedItem = CodecManager.X264;
            this.audioCodec.Items.AddRange(CodecManager.ListOfAudioCodecs);
            this.audioCodec.SelectedItem = CodecManager.NAAC;
        }
        /// <summary>
        /// handles the GUI closing event
        /// saves all jobs, stops the currently active job and saves all profiles as well
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.isEncoding)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to quit?", "Job in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                    e.Cancel = true; // abort closing
                else
                    abort();
            }
            if (!e.Cancel)
            {
                this.profileManager.SaveProfiles();
                this.saveSettings();
                this.saveJobs();
                this.saveLog();
                this.runRestarter();
            }
            base.OnClosing(e);
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
            if (!proc.Start())
                MessageBox.Show("Couldn't run updater.", "Couldn't run updater.", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #endregion
        #region button actions
        #region I/O
        #region Input tab
        /// <summary>
        /// handles the ... button to select an input script
        /// if a file has been selected, the input field is updated along with the commandline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputOpenButton_Click(object sender, System.EventArgs e)
        {
            this.openFileDialog.Filter = "AviSynth scripts|*.avs|All files|*.*";
            this.openFileDialog.Title = "Open AviSynth script";
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileName.ToLower().EndsWith(".avs"))
                    openVideoFile(openFileDialog.FileName);
                else
                {
                    string newFileName = VideoUtil.createSimpleAvisynthScript(openFileDialog.FileName);
                    if (newFileName != null)
                        openVideoFile(newFileName);
                }
            }
        }
        /// <summary>
        /// opens the AviSynth preview for a given AviSynth script
        /// gets the properties of the video, registers the callbacks, updates the video bitrate (we know the lenght of the video now) and updates the commandline
        /// with the scriptname
        /// </summary>
        /// <param name="fileName">the AviSynth scrip to be opened</param>
        private void openAvisynthScript(string fileName)
        {
            if (this.player != null) // make sure only one preview window is open at all times
                player.Close();
            player = new VideoPlayer();
            bool videoLoaded = player.loadVideo(fileName, PREVIEWTYPE.CREDITS, true);
            if (videoLoaded)
            {
                if (parX < 1 || parY < 1)
                {
                    parX = player.Reader.DARX;
                    parY = player.Reader.DARY;
                }
                player.PARX = parX;
                player.PARY = parY;
                player.IntroCreditsFrameSet += new IntroCreditsFrameSetCallback(player_IntroCreditsFrameSet);
                player.Closed += new EventHandler(player_Closed);
                player.Show();
            }
        }

        /// <summary>
        /// handles the ... button to select an outupt file
        /// depending on the output type selected, the file filter will be changed
        /// upon selection of a file, the GUI field and commandline will be updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videoOutputOpenButton_Click(object sender, System.EventArgs e)
        {
            this.saveFileDialog.Title = "Enter name of output";
            this.saveFileDialog.FileName = "";
            VideoType currentType = CurrentVideoOutputType;
            this.saveFileDialog.Filter = currentType.OutputFilterString;
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.videoOutput.Text = Path.ChangeExtension(this.saveFileDialog.FileName, currentType.Extension);
            }
        }

        private void videoInput_DoubleClick(object sender, System.EventArgs e)
        {
            if (!videoInput.Text.Equals(""))
            {
                this.openAvisynthScript(videoInput.Text);
                if (this.creditsStartFrame > -1)
                    this.player.CreditsStart = creditsStartFrame;
                if (this.introEndFrame > -1)
                    this.player.IntroEnd = introEndFrame;
            }
        }
        #endregion
        #region codec configuration
        private void videoConfigButton_Click(object sender, System.EventArgs e)
        {
            if (player != null)
                player.Hide();
            VideoCodecSettings settings = CurrentVideoCodecSettingsProvider.GetCurrentSettings();
            string selectedProfile;
            if (CurrentVideoCodecSettingsProvider.EditSettings(this.profileManager, this.settings, this.videoProfile.Text,
                this.VideoIO, new int[] { this.introEndFrame, this.creditsStartFrame }, out selectedProfile))
            {
                this.videoProfile.Items.Clear();
                foreach (string name in this.profileManager.VideoProfiles.Keys)
                {
                    this.videoProfile.Items.Add(name);
                }
                int index = this.videoProfile.Items.IndexOf(selectedProfile);
                if (index != -1)
                    this.videoProfile.SelectedIndex = index;
            }
            if (player != null)
                player.Show();
            updateIOConfig();
        }
        private void configAudioButton_Click(object sender, System.EventArgs e)
        {
            if (player != null)
                player.Hide();
            string selectedProfile;
            if (CurrentAudioSettingsProvider.EditSettings(this.profileManager, this.path, this.settings, this.audioProfile.Text,
                new string[] { audioInput.Text, audioOutput.Text }, out selectedProfile))
            {
                this.audioProfile.Items.Clear();
                foreach (string name in this.profileManager.AudioProfiles.Keys)
                {
                    this.audioProfile.Items.Add(name);
                }
                int index = audioProfile.Items.IndexOf(selectedProfile);
                if (index != -1)
                    audioProfile.SelectedIndex = index;
                AudioStream stream = this.CurrentAudioStream;
                stream.settings = CurrentAudioSettingsProvider.GetCurrentSettings();
                this.CurrentAudioStream = stream;
            }
            if (player != null)
                player.Show();
        }
        #endregion
        #region reset
        private void resetButton_Click(object sender, System.EventArgs e)
        {
            this.videoInput.Text = "";
            this.videoOutput.Text = "";
            this.creditsStartFrame = 0;
            this.audioInput.Text = "";
            this.audioOutput.Text = "";
            this.audioStreams[0].path = "";
            this.audioStreams[0].output = "";
            this.audioStreams[0].settings = null;
            this.audioStreams[1].path = "";
            this.audioStreams[1].output = "";
            this.audioStreams[1].settings = null;
        }
        #endregion
        #region auto encoding
        private void autoEncodeButton_Click(object sender, System.EventArgs e)
        {
            // normal video verification
            string error = null;
            // update the current audio stream with the latest data
            updateAudioStreams();
            if ((error = verifyVideoSettings()) != null)
            {
                MessageBox.Show(error, "Unsupported video configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if ((error = verifyAudioSettings()) != null)
            {
                MessageBox.Show(error, "Unsupported audio configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (CurrentVideoCodecSettings.EncodingMode == 2 || CurrentVideoCodecSettings.EncodingMode == 5)
            {
                MessageBox.Show("First pass encoding is not supported for automated encoding as no output is generated.\nPlease choose another encoding mode", "Improper configuration",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            VideoCodecSettings vSettings = this.CurrentVideoCodecSettings.clone();
            bool cont = jobUtil.getFinalZoneConfiguration(vSettings, this.introEndFrame, this.creditsStartFrame);
            if (cont)
            {
                int length = 0;
                double framerate = 0.0;
                VideoStream myVideo = new VideoStream();
                jobUtil.getInputProperties(out length, out framerate, videoInput.Text);
                myVideo.Input = VideoIO[0];
                myVideo.Output = VideoIO[1];
                myVideo.NumberOfFrames = length;
                myVideo.Framerate = framerate;
                myVideo.ParX = this.parX;
                myVideo.ParY = this.parY;
                myVideo.VideoType = CurrentMuxableVideoType;
                myVideo.Settings = vSettings;
                AutoEncodeWindow aew = new AutoEncodeWindow(myVideo, this.audioStreams, this, this.addPrerenderJob.Checked);
                if (aew.init())
                    aew.ShowDialog();
                else
                    MessageBox.Show("The currently selected combination of video and audio output cannot be muxed", "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region audio

        private void updateAudioStreams()
        {
            AudioStream stream = new AudioStream();
            stream.path = this.audioInput.Text;
            stream.output = this.audioOutput.Text;
            stream.settings = this.CurrentAudioSettingsProvider.GetCurrentSettings();
            stream.Type = (this.audioContainer.SelectedItem as AudioType);
            this.CurrentAudioStream = stream;
        }

        /// <summary>
        /// gets the delay from an audio filename
        /// </summary>
        /// <param name="fileName">file name to be analyzed</param>
        /// <returns>the delay in milliseconds</returns>
        public static int getDelay(string fileName)
        {
            int start = fileName.LastIndexOf("DELAY ");
            if (start != -1) // delay is in filename
            {
                try
                {
                    string delay = fileName.Substring(start + 6, fileName.LastIndexOf("ms.") - start - 6);
                    int del = 0;
                    del = Int32.Parse(delay);
                    return del;
                }
                catch (Exception e) // problem parsing, assume 0s delay
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
            return 0;
        }
        /// <summary>
        /// user has pressed the open audio button
        /// present it with a fileopen dialogue and set the proper file selection filters
        /// if the input is not .aac/.mp4, enable audio encoding facilities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioInputOpenButton_Click(object sender, System.EventArgs e)
        {
            this.openFileDialog.Filter = this.audioEncoderProvider.GetSupportedInput(this.CurrentAudioSettingsProvider.CodecType);
            this.openFileDialog.Title = "Select your audio input";
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openAudioFile(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// user has clicked on the audio output selection button, present him with a file save dialogue
        /// and update the commandline after file selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioOutputOpenButton_Click(object sender, System.EventArgs e)
        {
            this.saveFileDialog.Title = "Enter name of output";
            this.saveFileDialog.FileName = "";
            AudioType currentType = CurrentAudioOutputType;
            this.saveFileDialog.Filter = currentType.OutputFilterString;
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.audioOutput.Text = Path.ChangeExtension(this.saveFileDialog.FileName, currentType.Extension);
            }
        }
        /// <summary>
        /// deletes the currently selected audio stream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAudioButton_Click(object sender, System.EventArgs e)
        {
            this.audioInput.Text = "";
            this.audioOutput.Text = "";
            int trackNumber = -1;
            if (this.audioTrack1.Checked)
                trackNumber = 0;
            if (this.audioTrack2.Checked)
                trackNumber = 1;
            if (trackNumber != -1)
            {
                this.audioStreams[trackNumber].settings = null;
                this.audioStreams[trackNumber].Type = null;
                this.audioStreams[trackNumber].path = "";
                this.audioStreams[trackNumber].output = "";
            }
        }
        #endregion
        #region queueing
        private void queueVideoButton_Click(object sender, System.EventArgs e)
        {
            string settingsError = verifyVideoSettings();  // basic input, logfile and output file settings are okay
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            VideoCodecSettings vSettings = this.CurrentVideoCodecSettings.clone();
            bool start = settings.AutoStartQueue;
            start &= jobUtil.AddVideoJobs(this.VideoIO[0], this.VideoIO[1], this.CurrentVideoCodecSettings.clone(),
                this.introEndFrame, this.creditsStartFrame, parX, parY, addPrerenderJob.Checked, true);
            if (start)
                this.startNextJobInQueue();
        }
        /// <summary>
        /// handles the queue button in the audio tab
        /// generates a new audio job, adds the job to the queue and listView, and if "and encode" is checked, we'll start encoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queueAudioButton_Click(object sender, System.EventArgs e)
        {
            string settingsError = verifyAudioSettings();
            if (settingsError != null)
            {
                MessageBox.Show(settingsError, "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            AudioCodecSettings aSettings = this.CurrentAudioSettingsProvider.GetCurrentSettings();
            bool start = this.settings.AutoStartQueue;
            start &= jobUtil.AddAudioJob(this.audioInput.Text, this.audioOutput.Text, aSettings);
            if (start)
                this.startNextJobInQueue();
        }
        #endregion
        #endregion
        #region queue
        /// <summary>
        /// moves the job one position up in the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upButton_Click(object sender, System.EventArgs e)
        {
            if (queueListView.SelectedItems.Count > 0)
            {
                MoveListViewItem(ref this.queueListView, true);
                updateJobPositions();
            }
        }
        /// <summary>
        /// moves a job one position down in the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downButton_Click(object sender, System.EventArgs e)
        {
            if (queueListView.SelectedItems.Count > 0)
            {
                MoveListViewItem(ref this.queueListView, false);
                updateJobPositions();
            }
        }
        /// <summary>
        /// deletes a job from the job queue
        /// if a line in the job queue is selected, and its status is not processing (status processing means we're currently
        /// encoding), the job is removed from the listview and the hashtable containing the jobs
        /// in addition, of the job exists as a file in the jobs directory, it is being deleted as well
        /// deleting of a job that is currently being encoded is not supported and shows an error message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteJobButton_Click(object sender, System.EventArgs e)
        {
            if (this.queueListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in this.queueListView.SelectedItems)
                {
                    if (!jobs.ContainsKey(item.Text)) // Check if it has already been deleted
                        continue;
                    Job job = jobs[item.Text];
                    if (job != null)
                    {
                        if (job.Status != JobStatus.PROCESSING)
                        {
                            if (job.Next != null || job.Previous != null)
                            {
                                DialogResult dr = MessageBox.Show("This job is part of a series of jobs. Deleting it alone will cause corruption of the dependant jobs\r\n"
                                    + "Press Yes to delete all dependant jobs, No to delete just this job or Cancel to abort.", "Job dependency detected", MessageBoxButtons.YesNoCancel);
                                switch (dr)
                                {
                                    case DialogResult.Yes:
                                        this.removeJobFromQueue(job);
                                        Job prev = null;
                                        if (job.Previous != null)
                                            prev = jobs[job.Previous];
                                        while (prev != null)
                                        {
                                            removeJobFromQueue(prev);
                                            if (prev.Previous != null)
                                                prev = jobs[prev.Previous];
                                            else
                                                prev = null;
                                        }
                                        Job next = null;
                                        if (job.Next != null)
                                            next = jobs[job.Next];
                                        while (next != null)
                                        {
                                            removeJobFromQueue(next);
                                            if (next.Next != null)
                                                next = jobs[next.Next];
                                            else
                                                next = null;
                                        }
                                        break;
                                    case DialogResult.No:
                                        removeJobFromQueue(job);
                                        break;
                                    case DialogResult.Cancel: // do nothing
                                        break;
                                }
                            }
                            else // no dependent jobs
                            {
                                removeJobFromQueue(job);
                            }
                        }
                        else
                            MessageBox.Show("You cannot delete a job while it is being processed.", "Deleting job failed", MessageBoxButtons.OK);
                    }
                }
                updateJobPositions();
            }
        }
        /// <summary>
        /// goes through all jobs in the listview and updates their position
        /// this is called when moving or deleting jobs
        /// </summary>
        private void updateJobPositions()
        {
            foreach (ListViewItem item in this.queueListView.Items) // go through all remaining jobs and update their position
            {
                Job job = jobs[item.Text];
                job.Position = item.Index;
            }
        }
        /// <summary>
        /// loads a job listed in the job queue
        /// if a line is selected in the queue, its associated job is extracted from the jobs hashtable, then its settings
        /// are being loaded. In addition, the commandline stored in the job is displayed, overriding the automatically
        /// generated commandline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadJobButton_Click(object sender, System.EventArgs e)
        {
            if (this.queueListView.SelectedItems.Count > 0) // otherwise we have something bogus on our hands
            {
                int position = this.queueListView.SelectedItems[0].Index;
                Job job = jobs[this.queueListView.Items[position].Text];
                if (job is VideoJob)
                {
                    VideoJob vjob = (VideoJob)job;
                    this.videoInput.Text = vjob.Input;
                    parX = vjob.DARX;
                    parY = vjob.DARY;
                    CurrentVideoCodecSettings = vjob.Settings;
                    this.videoOutput.Text = vjob.Output;
                    this.tabControl1.SelectedIndex = 0;
                    this.videoProfile.SelectedIndex = -1;
                }
                if (job is AudioJob)
                {
                    AudioJob ajob = (AudioJob)job;
                    foreach (IAudioSettingsProvider p in audioCodec.Items)
                    {
                        if (p.IsSameType(ajob.Settings))
                        {
                            p.LoadSettings(ajob.Settings);
                            audioCodec.SelectedItem = p;
                            break;
                        }
                    }
                    this.audioInput.Text = job.Input;
                    this.audioOutput.Text = job.Output;
                    this.tabControl1.SelectedIndex = 0;
                    this.audioProfile.SelectedIndex = -1;
                }
                if (job is MuxJob)
                {
                    MuxJob mjob = (MuxJob)job;
                    MuxWindow mw = new MuxWindow(muxProvider.GetMuxer(mjob.MuxType));
                    SubStream[] subtitleStreams = mjob.Settings.SubtitleStreams.ToArray();
                    mw.Job = mjob;
                    if (mw.ShowDialog() == DialogResult.OK)
                    {
                        MuxJob newJob = mw.Job;
                        transferJobSettings(mjob, newJob);
                        jobs.Remove(job.Name);
                        jobs.Add(job.Name, newJob);
                    }
                }
                else if (job is IndexJob)
                {
                    IndexJob ijob = (IndexJob)job;
                    if (ijob.PostprocessingProperties == null)
                    {
                        VobinputWindow viw = new VobinputWindow(this);
                        viw.setConfig(ijob.Input, ijob.Output, ijob.DemuxMode, ijob.AudioTrackID1, ijob.AudioTrackID2,
                            false, true, ijob.LoadSources, true);
                        if (viw.ShowDialog() == DialogResult.OK)
                        {
                            IndexJob newJob = viw.Job;
                            transferJobSettings(ijob, newJob);
                            jobs.Remove(job.Name);
                            jobs.Add(job.Name, newJob);
                        }
                    }
                    else
                        MessageBox.Show("Loading of OneClick index jobs not supported", "Load not possible", MessageBoxButtons.OK);
                }
            }
            else
                MessageBox.Show("You need to select a job first.", "No job selected", MessageBoxButtons.OK);
        }
        /// <summary>
        /// updates a selected job in the queue with what's currently configured in the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateJobButton_Click(object sender, System.EventArgs e)
        {
            if (this.queueListView.SelectedItems.Count > 0) // otherwise we have something bogus on our hands
            {
                int position = this.queueListView.SelectedItems[0].Index;
                ListViewItem item = this.queueListView.SelectedItems[0];
                Job job = jobs[this.queueListView.Items[position].Text];
                if (job is VideoJob)
                {
                    VideoJob vjob = (VideoJob)job;
                    if (vjob.Settings.GetType() == this.CurrentVideoCodecSettings.GetType())
                    {
                        if (this.CurrentVideoCodecSettings.EncodingMode != 4 && this.CurrentVideoCodecSettings.EncodingMode != 8)
                        {
                            vjob.Input = VideoIO[0];
                            vjob.Output = VideoIO[1];
                            vjob.Settings = this.CurrentVideoCodecSettings.clone();
                            item.SubItems[1].Text = vjob.InputFileName;
                            item.SubItems[2].Text = vjob.OutputFileName;
                            item.SubItems[4].Text = vjob.EncodingMode;
                        }
                        else
                        {
                            MessageBox.Show("You cannot turn a single job into a series of jobs", "Unsupported update", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                        MessageBox.Show("You cannot change the codec of an existing job.\nIf you want to change codecs, delete the job and create a new one",
                            "Unsupported update", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else if (job is AudioJob)
                {
                    AudioJob ajob = (AudioJob)job;
                    if (ajob.Settings.GetType() == this.CurrentAudioSettingsProvider.GetCurrentSettings().GetType())
                    {
                        ajob.Input = this.audioInput.Text;
                        ajob.Output = this.audioOutput.Text;
                        ajob.Settings = this.CurrentAudioSettingsProvider.GetCurrentSettings();
                        item.SubItems[1].Text = ajob.InputFileName;
                        item.SubItems[2].Text = ajob.OutputFileName;
                        item.SubItems[4].Text = ajob.EncodingMode;
                    }
                    else
                        MessageBox.Show("You cannot change the change the codec of an existing job.\nIf you want to change codecs, delete the job and create a new one",
                            "Unsupported update", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
                else if (job is MuxJob)
                {
                    MessageBox.Show("To update a mux job, simply select it and press config.\nThen make changes in the window that pops up and press\nthe Update button to update the job.",
                        "Not supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (job is IndexJob)
                {
                    MessageBox.Show("You cannot update an indexing job", "Not supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void shutdownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.Shutdown = this.shutdownCheckBox.Checked;
        }
        #endregion
        #region queue buttons
        /// <summary>
        /// handles the start/stop button in the queue tab
        /// if we're already encoding in queue mode, the button acts as a stop button
        /// if we're encoding but are not in queue mode, the queue mode will be started
        /// if we're not encoding, and there is at least one job in the queue, encoding is started
        /// if we're not encoding and there's no job in the queue, a warning is displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startStopButton_Click(object sender, System.EventArgs e)
        {
            if (this.isEncoding) // we're already encoding
            {
                this.queueEncoding = !this.queueEncoding;
            }
            else // we're not encoding yet
            {
                if (this.queueListView.Items.Count > 0) // we can't start encoding if there are no jobs
                {
                    if (this.player != null)
                    {
                        player.Close();
                        player = null;
                    }
                    this.skipJobs.Clear();
                    this.queueEncoding = true;
                    int retval = startNextJobInQueue();
                    if (retval == 1)
                        MessageBox.Show("Couldn't start processing. Please consult the log for more details", "Processing failed", MessageBoxButtons.OK);
                    else if (retval == 2)
                        MessageBox.Show("No jobs are waiting. Nothing to do", "No jobs waiting", MessageBoxButtons.OK);
                }
                else
                    MessageBox.Show("Please give me something to do", "No jobs found", MessageBoxButtons.OK);
            }
            updateProcessingStatus();
        }

        /// <summary>
        /// handles the abort button in the queue tab
        /// if we're currently encoding), the encoder is stopped and the GUI reset to non encoding mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void abortButton_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to abort encoding?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
                abort();
        }
        /// <summary>
        /// deletes all the jobs from the jobs queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAllJobsButton_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to clear the queue?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            foreach (ListViewItem item in this.queueListView.Items)
            {
                Job job = jobs[item.Text];
                if (job.Status != JobStatus.PROCESSING)
                    this.removeJobFromQueue(job);
                else
                    MessageBox.Show("You cannot delete a job while it is being encoded.", "Deleting job failed", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// handles the pause button
        /// enables / disables the main encoding thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseButton_Click(object sender, System.EventArgs e)
        {
            if (!this.paused) // we're encoding
            {
                string error;
                if (currentProcessor != null)
                {
                    if (currentProcessor.pause(out error))
                    {
                        paused = true;
                    }
                    else
                        addToLog("Error when trying to pause encoding: " + error);
                }
            }
            else
            {
                string error;
                if (currentProcessor != null)
                {
                    if (currentProcessor.resume(out error))
                    {
                        paused = false;
                        this.pauseButton.Image = (Image)this.pauseImage;
                    }
                    else
                        addToLog("Error when trying to resume encoding: " + error);
                }
            }
            updateProcessingStatus();
        }
        #endregion
        private void clearLogButton_Click(object sender, EventArgs e)
        {
            saveLog();
            logBuilder = new StringBuilder();
            log.Text = "";
        }
        #endregion
        #region dropdown action
        #region IO tab
        /// <summary>
        /// handles changes in the file type selected
        /// enforces the proper extension for the selected container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            VideoType vot = CurrentVideoOutputType;
            this.videoOutput.Text = Path.ChangeExtension(this.videoOutput.Text, vot.Extension);
        }
        /// <summary>
        /// hangles changes in the audio container selection
        /// enforces the proper extension for the selected container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioContainer_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AudioType aot = CurrentAudioOutputType;
            this.audioOutput.Text = Path.ChangeExtension(this.audioOutput.Text, aot.Extension);
        }
        #endregion
        #region codec selection
        /// <summary>
        /// handles changes in the codec selection
        /// enables / disabled the proper GUI fields
        /// and changes the available fourCCs
        /// at the end, the proper encodingmode_changed method is triggered to ensure a proper GUI update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void codec_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            VideoType[] outputTypes = this.videoEncoderProvider.GetSupportedOutput(this.CurrentVideoCodecSettingsProvider.EncoderType);
            VideoType currentType = null;
            if (CurrentVideoOutputType != null)
                currentType = CurrentVideoOutputType;
            else
                currentType = outputTypes[0];
            this.fileType.Items.Clear();
            this.fileType.Items.AddRange(outputTypes);
            // now select the previously selected type again if possible
            bool selected = false;
            foreach (VideoType t in outputTypes)
            {
                if (currentType == t)
                {
                    this.fileType.SelectedItem = t;
                    currentType = t;
                    selected = true;
                    break;
                }
            }
            if (!selected)
            {
                currentType = outputTypes[0];
                this.fileType.SelectedItem = outputTypes[0];
            }
            VideoCodecSettings settings = CurrentVideoCodecSettingsProvider.GetCurrentSettings();
            this.updateIOConfig();
            if (verifyOutputFile(this.videoOutput.Text) == null)
                this.videoOutput.Text = Path.ChangeExtension(this.videoOutput.Text, currentType.Extension);
        }

        private void audioCodec_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AudioType[] outputTypes = this.audioEncoderProvider.GetSupportedOutput(this.CurrentAudioSettingsProvider.EncoderType);
            AudioType currentType = null;
            if (this.audioContainer.SelectedItem != null)
                currentType = this.audioContainer.SelectedItem as AudioType;
            else
                currentType = outputTypes[0];
            this.audioContainer.Items.Clear();
            this.audioContainer.Items.AddRange(outputTypes);
            // now select the previously selected type again if possible
            bool selected = false;
            foreach (AudioType t in outputTypes)
            {
                if (currentType == t)
                {
                    this.audioContainer.SelectedItem = t;
                    currentType = t;
                    selected = true;
                    break;
                }
            }
            if (!selected)
            {
                currentType = outputTypes[0];
                this.audioContainer.SelectedItem = outputTypes[0];
            }
            AudioCodecSettings settings = CurrentAudioSettingsProvider.GetCurrentSettings();
            if (verifyOutputFile(this.audioOutput.Text) == null)
                this.audioOutput.Text = Path.ChangeExtension(this.audioOutput.Text, currentType.Extension);
        }
        #endregion
        #region profiles
        /// <summary>
        /// handles the selection of a profile from the list
        /// the profile is looked up from the profiles Hashtable (it uses the name as unique key), then
        /// the settings from the new profile are displayed in the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videoProfile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.videoProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                VideoProfile prof = this.profileManager.VideoProfiles[this.videoProfile.SelectedItem.ToString()];
                foreach (IVideoSettingsProvider p in this.videoCodec.Items)
                {
                    if (p.IsSameType(prof.Settings))
                    {
                        p.LoadSettings(prof.Settings);
                        videoCodec.SelectedItem = p;
                        break;
                    }
                }
            }
            updateIOConfig();
        }
        private void audioProfile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.audioProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                AudioProfile prof = this.profileManager.AudioProfiles[this.audioProfile.SelectedItem.ToString()];
                foreach (IAudioSettingsProvider p in this.audioCodec.Items)
                {
                    if (p.IsSameType(prof.Settings))
                    {
                        p.LoadSettings(prof.Settings);
                        audioCodec.SelectedItem = p;
                        break;
                    }
                }
            }
        }
        #endregion
        #endregion
        #region listview action
        /// <summary>
        /// moves the currently selected listviewitem up/down
        /// code by Less Smith @ KnotDot.Net
        /// </summary>
        /// <param name="lv">reference to ListView</param>
        /// <param name="moveUp">whether the currently selected item should be moved up or down</param>
        private void MoveListViewItem(ref ListView lv, bool moveUp)
        {
            string cache;
            int selIdx;

            selIdx = lv.SelectedItems[0].Index;
            lv.Items[selIdx].Selected = false;
            if (moveUp)
            {
                // ignore moveup of row(0)
                if (selIdx == 0)
                    return;

                // move the subitems for the previous row
                // to cache to make room for the selected row
                for (int i = 0; i < lv.Items[selIdx].SubItems.Count; i++)
                {
                    cache = lv.Items[selIdx - 1].SubItems[i].Text;
                    lv.Items[selIdx - 1].SubItems[i].Text =
                        lv.Items[selIdx].SubItems[i].Text;
                    lv.Items[selIdx].SubItems[i].Text = cache;
                }
                lv.Items[selIdx - 1].Selected = true;
                lv.Refresh();
            }
            else
            {
                // ignore movedown of last item
                if (selIdx == lv.Items.Count - 1)
                    return;
                // move the subitems for the next row
                // to cache so we can move the selected row down
                for (int i = 0; i < lv.Items[selIdx].SubItems.Count; i++)
                {
                    cache = lv.Items[selIdx + 1].SubItems[i].Text;
                    lv.Items[selIdx + 1].SubItems[i].Text =
                        lv.Items[selIdx].SubItems[i].Text;
                    lv.Items[selIdx].SubItems[i].Text = cache;
                }
                lv.Items[selIdx + 1].Selected = true;
                lv.Refresh();
            }
        }
        /// <summary>
        /// handles the doubleclick event for the listview
        /// changes the job status from waiting -> postponed to waiting
        /// from done -> waiting -> postponed -> waiting
        /// from error -> waiting -> postponed -> waiting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queueListView_DoubleClick(object sender, EventArgs e)
        {
            if (this.queueListView.SelectedItems.Count > 0) // otherwise 
            {
                int position = this.queueListView.SelectedItems[0].Index;
                Job job = jobs[this.queueListView.SelectedItems[0].Text];
                if (job.Status == JobStatus.POSTPONED || job.Status == JobStatus.ERROR || job.Status == JobStatus.ABORTED || job.Status == JobStatus.DONE) // postponed/error/aborted/done -> waiting
                    job.Status = JobStatus.WAITING;
                else if (job.Status == JobStatus.WAITING) // waiting -> postponed
                    job.Status = JobStatus.POSTPONED;
                else if (job.Status == JobStatus.PROCESSING && !this.isEncoding) // b0rked processing job
                    job.Status = JobStatus.WAITING;
                this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
            }
        }

        private void queueContextMenu_Opened(object sender, EventArgs e)
        {
            AbortMenuItem.Enabled = AllJobsHaveStatus(JobStatus.PROCESSING);
            AbortMenuItem.Checked = AllJobsHaveStatus(JobStatus.ABORTED);

            LoadMenuItem.Enabled = this.queueListView.SelectedItems.Count == 1;
            LoadMenuItem.Checked = false;

            bool canModifySelectedJobs = !AnyJobsHaveStatus(JobStatus.PROCESSING) && this.queueListView.SelectedItems.Count > 0;
            DeleteMenuItem.Enabled = PostponedMenuItem.Enabled = WaitingMenuItem.Enabled = canModifySelectedJobs;

            DeleteMenuItem.Checked = false;
            PostponedMenuItem.Checked = AllJobsHaveStatus(JobStatus.POSTPONED);
            WaitingMenuItem.Checked = AllJobsHaveStatus(JobStatus.WAITING);

            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                item.SubItems[5].Text = (jobs[item.Text]).StatusString;
            }
        }

        /// <summary>
        /// Returns true if all selected jobs have the requested status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool AllJobsHaveStatus(JobStatus status)
        {
            if (this.queueListView.SelectedItems.Count <= 0)
            {
                return false;
            }
            bool allHaveStatus = true;
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                Job job = jobs[item.Text];
                allHaveStatus &= (job.Status == status);
            }
            return allHaveStatus;
        }

        /// <summary>
        /// Returns true if any selected jobs have the requested status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool AnyJobsHaveStatus(JobStatus status)
        {
            bool anyHaveStatus = false;
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                Job job = jobs[item.Text];
                anyHaveStatus |= (job.Status == status);
            }
            return anyHaveStatus;
        }

        private void abortMenuItem_Click(object sender, EventArgs e)
        {
            abortButton_Click(sender, e);
        }

        void deleteMenuItem_Click(object sender, EventArgs e)
        {
            deleteJobButton_Click(sender, e);
        }

        private void loadMenuItem_Click(object sender, EventArgs e)
        {
            loadJobButton_Click(sender, e);
        }

        private void postponeMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                int position = item.Index;
                Job job = jobs[item.Text];
                if (job.Status != JobStatus.PROCESSING)
                {
                    job.Status = JobStatus.POSTPONED;
                    this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
                }
                else
                {
                    Debug.Assert(false, "shouldn't be able to postpone an active job");
                    return;
                }
            }
        }

        private void waitingMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                int position = item.Index;
                Job job = jobs[item.Text];
                if (job.Status != JobStatus.PROCESSING)
                {
                    job.Status = JobStatus.WAITING;
                    this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
                }
                else
                {
                    Debug.Assert(false, "shouldn't be able to set an active job back to waiting");
                    return;
                }
            }
        }

        private int getJobPosition(string name)
        {
            foreach (ListViewItem item in this.queueListView.Items)
            {
                if (item.SubItems[0].Text.Equals(name))
                {
                    return item.Index;
                }
            }
            return -1;
        }

        #endregion
        #region job management
        #region I/O verification
        /// <summary>
        /// verifies the input, output and logfile configuration
        /// based on the codec and encoding mode certain fields must be filled out
        /// </summary>
        /// <returns>null if no error; otherwise string error message</returns>
        public string verifyVideoSettings()
        {
            // test for valid input filename
            string fileErr = verifyInputFile(this.videoInput.Text);
            if (fileErr != null)
            {
                return "Problem with video input filename:\n" + fileErr;
            }

            // test for valid output filename (not needed if just doing 1st pass)
            if (!isFirstPass())
            {
                fileErr = verifyOutputFile(this.videoOutput.Text);
                if (fileErr != null)
                {
                    return "Problem with video output filename:\n" + fileErr;
                }

                VideoType vot = CurrentVideoOutputType;
                // test output file extension
                if (!Path.GetExtension(this.videoOutput.Text).Replace(".", "").Equals(vot.Extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "Video output filename does not have the correct extension.\nBased on current settings, it should be "
                        + vot.Extension;
                }
            }
            return null;
        }

        public string verifyAudioSettings()
        {
            // test for valid input filename
            foreach (AudioStream stream in audioStreams)
            {
                if (string.IsNullOrEmpty(stream.path) && string.IsNullOrEmpty(stream.output))
                    continue;
                string fileErr = verifyInputFile(this.audioInput.Text);
                if (fileErr != null)
                {
                    return "Problem with audio input filename:\n" + fileErr;
                }

                fileErr = verifyOutputFile(this.audioOutput.Text);
                if (fileErr != null)
                {
                    return "Problem with audio output filename:\n" + fileErr;
                }
                AudioType aot = this.audioContainer.SelectedItem as AudioType;
                // test output file extension
                if (!Path.GetExtension(this.audioOutput.Text).Replace(".", "").Equals(aot.Extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    return "Audio output filename does not have the correct extension.\nBased on current settings, it should be "
                        + aot.Extension;
                }
            }
            return null;
        }

        private bool isFirstPass()
        {
            VideoCodecSettings settings = CurrentVideoCodecSettings;
            if (settings.EncodingMode == 2 || settings.EncodingMode == 5)
                return true;
            else
                return false;
        }

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
        #region update queue
        /// <summary>
        /// marks job currently marked as processing as aborted
        /// </summary>
        private void markJobAborted()
        {
            foreach (ListViewItem item in this.queueListView.Items)
            {
                Job job = jobs[item.Text];
                if (job.Status == JobStatus.PROCESSING) // processing -> done
                {
                    job.Status = JobStatus.ABORTED;
                    DateTime now = DateTime.Now;
                    job.End = now;
                    item.SubItems[5].Text = job.StatusString;
                    item.SubItems[7].Text = job.End.ToShortDateString();
                    if (settings.DeleteAbortedOutput)
                    {
                        logBuilder.Append("Job aborted, deleting output file...");
                        try
                        {
                            File.Delete(job.Output);
                            logBuilder.Append("Deletion successful.\r\n");
                        }
                        catch (Exception)
                        {
                            logBuilder.Append("Deletion failed.\r\n");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// marks the first job found in "processing" state in the job queue as done and returns its name
        /// </summary>
        /// <returns>name of the job marked as done</returns>
        private Job markJobDone(StatusUpdate su)
        {
            Job job = null;
            foreach (ListViewItem item in this.queueListView.Items)
            {
                job = jobs[item.Text];
                if (su.JobName == job.Name)
                {
                    if (job.Status == JobStatus.PROCESSING) // processing -> done
                    {
                        if (su.HasError)
                        {
                            job.Status = JobStatus.ERROR;
                            item.SubItems[5].Text = "error";
                        }
                        else if (su.WasAborted)
                        {
                            job.Status = JobStatus.ABORTED;
                            item.SubItems[5].Text = "aborted";
                        }
                        else
                        {
                            job.Status = JobStatus.DONE;
                            item.SubItems[5].Text = "done";

                        }
                        DateTime now = DateTime.Now;
                        job.End = now;
                        if (su.JobType == JobTypes.VIDEO) // video job
                        {
                            job.FPS = su.FPS;
                            item.SubItems[8].Text = su.FPS.ToString("##.##");
                        }
                        item.SubItems[7].Text = job.End.ToLongTimeString();
                    }
                    break;
                }
            }
            if (job != null && job.Status == JobStatus.DONE && job.Next == null && settings.DeleteCompletedJobs)
                removeCompletedJob(job);
            return job;
        }
        /// <summary>
        /// removes this job, and any previous jobs that belong to a series of jobs from the
        /// queue, then update the queue positions
        /// </summary>
        /// <param name="job">the job to be removed</param>
        private void removeCompletedJob(Job job)
        {
            ArrayList jobs = new ArrayList();
            jobs.Add(job);
            Job j = job;
            while (j.Previous != null) // find all previous jobs
            {
                jobs.Add(this.jobs[j.Previous]);
                j = this.jobs[j.Previous];
            }
            ListViewItem item2Flush = null;
            foreach (object o in jobs)
            {
                j = (Job)o;
                this.jobs.Remove(j.Name);
                foreach (ListViewItem item in this.queueListView.Items)
                {
                    if (item.Text.Equals(j.Name))
                    {
                        item2Flush = item;
                        break;
                    }
                }
                this.queueListView.Items.Remove(item2Flush);
            }
            updateJobPositions();
        }
        #endregion
        #region starting jobs
        /// <summary>
        /// starts the job provided as parameters
        /// </summary>
        /// <param name="job">the Job object containing all the parameters</param>
        /// <returns>success / failure indicator</returns>
        public bool startEncoding(Job job)
        {
            if (this.player != null)
            {
                player.Close();
                player = null;
            }
            bool retval = false;
            if (this.isEncoding) // we're already encoding and can't start another job
                return false;
            string error;
            logBuilder.Append("Starting job " + job.Name + " at " + DateTime.Now.ToLongTimeString() + "\r\n");
            if (job is VideoJob)
                currentProcessor = new VideoEncoder(Settings);
            else if (job is AudioJob)
                currentProcessor = new AudioEncoder(Settings);
            else if (job is MuxJob)
                currentProcessor = new Muxer(Settings);
            else if (job is AviSynthJob)
                currentProcessor = new AviSynthProcessor();
            else if (job is IndexJob)
                currentProcessor = new DGIndexer(Settings.DgIndexPath);
            else
            {
                addToLog("Unknown job type found. No job started.\r\n");
                return false;
            }
            if (currentProcessor.setup(job, out error))
            {
                logBuilder.AppendLine(" encoder commandline:\r\n" + job.Commandline);
                ListViewItem item = this.queueListView.Items[getJobPosition(job.Name)];
                item.SubItems[5].Text = "processing";
                item.SubItems[6].Text = DateTime.Now.ToLongTimeString();
                item.SubItems[7].Text = "";
                item.SubItems[8].Text = "";
                job.Status = JobStatus.PROCESSING;
                currentProcessor.StatusUpdate += new JobProcessingStatusUpdateCallback(enc_StatusUpdate);
                pw = new ProgressWindow(job.JobType);
                pw.WindowClosed += new WindowClosedCallback(pw_WindowClosed);
                pw.Abort += new AbortCallback(pw_Abort);
                pw.setPriority(job.Priority);
                pw.PriorityChanged += new PriorityChangedCallback(pw_PriorityChanged);
                if (this.settings.OpenProgressWindow && this.Visible)
                {
                    mnuViewProcessStatus.Checked = true;
                    pw.Show();
                }
                if (currentProcessor.start(out error))
                {
                    this.isEncoding = true;
                    retval = true;
                    logBuilder.Append("successfully started encoding\r\n");
                }
                else
                {
                    logBuilder.Append("starting encoder failed with error " + error + "\r\n");
                    this.isEncoding = false;
                    retval = false;
                }
            }
            else // setup failed, probably a program is missing
            {
                logBuilder.Append("calling setup failed with error " + error + "\r\n");
                this.isEncoding = false;
                currentProcessor = null;
                retval = false;
            }

            updateProcessingStatus();
            this.log.Text = logBuilder.ToString();
            return retval;
        }
        /// <summary>
        /// starts the next job in status "waiting" from the queue list
        /// all items in the job queue are analyzed and the first found in status waiting is changed to 
        /// status processing, then a new Encoder object is created for it, the statusupdate callbacks
        /// are registered, a new progresswindow is created and finally the encoding is startred.
        /// if there's no remaining job in status waiting, the internal status indicating whether we're encoding
        /// is set back to false, as well as the queue encoding indicator
        /// </summary>
        /// <returns>0 = successfully started nex tjob, 1 = starting the next job failed, 2 = no jobs with status pending</returns>
        private int startNextJobInQueue()
        {
            ListViewItem i = null;
            Job job = null;
            foreach (ListViewItem item in this.queueListView.Items)
            {
                if (item.SubItems[5].Text.Equals("waiting")) // this is an item to be encoded
                {
                    i = item;
                    job = jobs[item.SubItems[0].Text];
                    if (this.skipJobs.Contains(job)) // this job is to be skipped
                        continue;
                    if (startEncoding(job))
                    {
                        i.SubItems[5].Text = "processing";
                        job.Start = DateTime.Now;
                        i.SubItems[6].Text = job.Start.ToLongTimeString();
                        job.Status = JobStatus.PROCESSING;
                        return 0;
                    }
                    else
                        return 1;
                }
            }
            updateProcessingStatus();
            return 2;
        }
        #endregion
        #region saving jobs
        /// <summary>
        /// saves all the jobs in the queue
        /// </summary>
        public void saveJobs()
        {
            int position = 0;
            foreach (ListViewItem item in this.queueListView.Items)
            {
                Job job = (Job)jobs[item.Text];
                job.Position = position;
                this.jobUtil.saveJob(job, this.path);
                position++;
            }
        }
        #endregion
        #region loading jobs
        /// <summary>
        /// loads all the jobs from the harddisk
        /// upon loading, the jobs are ordered according to their position field
        /// so that the order in which the jobs were previously shown in the GUI is preserved
        /// </summary>
        private void loadJobs()
        {
            string jobsPath = path + "\\jobs\\";
            if (!Directory.Exists(jobsPath))
                Directory.CreateDirectory(jobsPath);
            DirectoryInfo di = new DirectoryInfo(path + "\\jobs\\");
            FileInfo[] files = di.GetFiles("*.xml");
            foreach (FileInfo fi in files)
            {
                string fileName = fi.FullName;
                Job job = this.jobUtil.loadJob(fileName);
                if (job != null)
                {
                    if (jobs.ContainsKey(job.Name))
                        MessageBox.Show("A job named " + job.Name + " is already in the queue.\nThe job defined in " + fileName + "\nwill be discarded", "Duplicate job name detected",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        jobs.Add(job.Name, job);
                }
            }
            Dictionary<string, Job> remaining = new Dictionary<string, Job>(jobs);
            Job next = null;
            while (remaining.Count > 0)
            {
                foreach (Job job in remaining.Values)
                {
                    if (next == null || 
                        job.Position < next.Position)
                    {
                        next = job;
                    }
                }
                remaining.Remove(next.Name);
                string codec = "", encodingMode = "", start = "", end = "", fps = "";
                ListViewItem item = new ListViewItem(new string[] { next.Name, next.InputFileName, next.OutputFileName });
                if (next is VideoJob)
                {
                    codec = ((VideoJob)next).CodecString;
                    encodingMode = ((VideoJob)next).EncodingMode;
                    if (next.Status == JobStatus.DONE)
                        fps = next.FPS.ToString();
                }
                else if (next is AudioJob)
                {
                    codec = ((AudioJob)next).CodecString;
                    encodingMode = ((AudioJob)next).EncodingMode;
                }
                else if (next is MuxJob)
                {
                    encodingMode = "mux";
                }
                else if (next is IndexJob)
                {
                    encodingMode = "idx";
                }
                else if (next is AviSynthJob)
                {
                    encodingMode = "avs";
                }
                switch (next.Status)
                {
                    case JobStatus.ABORTED:
                    case JobStatus.ERROR:
                    case JobStatus.DONE:
                        start = next.Start.ToLongTimeString();
                        end = next.End.ToLongTimeString();
                        break;
                    case JobStatus.PROCESSING:
                        start = next.Start.ToLongTimeString();
                        break;
                }
                item.SubItems.AddRange(new string[] { codec, encodingMode, next.StatusString, start, end, fps });
                this.queueListView.Items.Add(item);
                next = null;
            }
        }
        #endregion
        #region misc
        /// <summary>
        /// looks up the first free job number
        /// </summary>
        /// <returns>the job number that can be attributed to the next job to be added to the queue</returns>
        public int getFreeJobNumber()
        {
            jobNr = 1;
            string name = "job" + this.jobNr;
            while (jobs.ContainsKey(name) || jobs.ContainsKey(name + "-1") || jobs.ContainsKey(name + "-2") || jobs.ContainsKey(name + "-3") || jobs.ContainsKey(name + "-4")
                || jobs.ContainsKey(name + "-5"))
            {
                name = "job" + this.jobNr;
                jobNr++;
            }
            if (jobNr > 1)
                return jobNr - 1;
            else
                return jobNr;
        }
        #endregion
        #region adding jobs to queue
        /// <summary>
        /// adds a job to the Queue (Hashtable) and the listview for graphical display
        /// </summary>
        /// <param name="job">the Job to be added to the next free spot in the queue</param>
        public void addJobToQueue(Job job)
        {
            job.Position = this.jobs.Count;
            ListViewItem item;
            if (job is VideoJob)
            {
                jobs.Add(job.Name, job); // adds job to the queue
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
							((VideoJob)job).CodecString, ((VideoJob)job).EncodingMode, "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
            if (job is AudioJob)
            {
                jobs.Add(job.Name, job); // adds job to the queue
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
														 ((AudioJob)job).CodecString, ((AudioJob)job).EncodingMode, "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
            if (job is MuxJob)
            {
                jobs.Add(job.Name, job);
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
						"", "mux", "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
            if (job is IndexJob)
            {
                jobs.Add(job.Name, job);
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
														 "", "idx", "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
            if (job is AviSynthJob)
            {
                jobs.Add(job.Name, job);
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
														 "", "avs", "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
        }
        private void addAnalysisPass_Click(object sender, EventArgs e)
        {
            // Prevents exception being thrown when trying to add a job with no data.
            if (!(videoInput.Text.Equals("")))
            {
                AviSynthJob job = jobUtil.generateAvisynthJob(VideoIO[0]);
                job.Name = "job" + getFreeJobNumber(); ;
                addJobToQueue(job);
            }
            else
            {
                MessageBox.Show("Error: Could not add job to queue. Make sure that all the details are entered correctly", "Couldn't create job", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region deleting jobs
        /// <summary>
        /// removes the given job from the job queue, the listview and from the harddisk if the xml job file exists
        /// </summary>
        /// <param name="job">the job to be removed</param>
        private void removeJobFromQueue(Job job)
        {
            this.queueListView.BeginUpdate();
            jobs.Remove(job.Name);
            int position = getJobPosition(job.Name);
            this.queueListView.Items[position].Remove();
            this.queueListView.Refresh();
            this.queueListView.EndUpdate();
            string fileName = path + "\\jobs\\" + job.Name + ".xml";
            if (File.Exists(fileName))
                File.Delete(fileName);
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
            settings.AudioProfileName = this.audioProfile.Text;
            settings.VideoProfileName = this.videoProfile.Text;
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
                        if (!settings.AudioProfileName.Equals(""))
                        {
                            if (this.profileManager.AudioProfiles.ContainsKey(settings.AudioProfileName))
                            {
                                int pos = this.audioProfile.Items.IndexOf(settings.AudioProfileName);
                                this.audioProfile.SelectedIndex = pos;
                            }
                        }
                        // modify PATH so that n00bs don't complain because they forgot to put dgdecode.dll in the MeGUI dir
                        string pathEnv = Environment.GetEnvironmentVariable("PATH");
                        pathEnv = MainForm.GetDirectoryName(settings.DgIndexPath) + ";" + pathEnv;
                        Environment.SetEnvironmentVariable("PATH", pathEnv);
                        if (!settings.VideoProfileName.Equals(""))
                        {
                            if (this.profileManager.VideoProfiles.ContainsKey(settings.VideoProfileName))
                            {
                                int pos = this.videoProfile.Items.IndexOf(settings.VideoProfileName);
                                this.videoProfile.SelectedIndex = pos;
                            }
                        }
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
        #region event routing
        /// <summary>
        /// event callback for the encoder
        /// Each time the encoder has a statusupdate, it fires an event that is catched here
        /// The actual GUI update is triggered via an Invoke on the current form, which ensures
        /// that the update is made in the GUI thread rather than the encoder thread, which could cause
        /// access problems
        /// </summary>
        /// <param name="su">StatusUpdate object that contains the current encoding statuts</param>
        private void enc_StatusUpdate(StatusUpdate su)
        {
            this.Invoke(new UpdateGUIStatusCallback(this.UpdateGUIStatus), new object[] { su });
        }
        /// <summary>
        /// event callback for the audio encoder, routes events to the GUI thread
        /// </summary>
        /// <param name="su"></param>
        private void aEnc_StatusUpdate(StatusUpdate su)
        {
            this.Invoke(new UpdateGUIStatusCallback(this.UpdateGUIStatus), new object[] { su });
        }
        /// <summary>
        /// event callback for the muxer, routes events to the GUI thread
        /// </summary>
        /// <param name="su"></param>
        private void muxer_StatusUpdate(StatusUpdate su)
        {
            this.Invoke(new UpdateGUIStatusCallback(this.UpdateGUIStatus), new object[] { su });
        }
        /// <summary>
        /// event callback for the indexer, routes events to the GUI thread
        /// </summary>
        /// <param name="su"></param>
        private void indexer_StatusUpdate(StatusUpdate su)
        {
            this.Invoke(new UpdateGUIStatusCallback(this.UpdateGUIStatus), new object[] { su });
        }
        /// <summary>
        /// event callback for the avisynth processor, rountes events to the GUI thread
        /// </summary>
        /// <param name="su"></param>
        private void avsProcessor_StatusUpdate(StatusUpdate su)
        {
            this.Invoke(new UpdateGUIStatusCallback(this.UpdateGUIStatus), new object[] { su });
        }
        #endregion
        #region job postprocessing
        /// <summary>
        /// Attempts to delete all files listed in job.FilesToDelete if settings.DeleteIntermediateFiles is checked
        /// </summary>
        /// <param name="job">the job which should just have been completed</param>
        private void deleteIntermediateFiles(Job job)
        {
            if (settings.DeleteIntermediateFiles)
            {
                addToLog("Job completed successfully and deletion of intermediate files is activated\r\n");
                foreach (string file in job.FilesToDelete)
                {
                    try
                    {
                        addToLog("Found intermediate output file '" + ((string)file)
                            + "', deleting...\r\n");
                        File.Delete(file);
                        addToLog("Deletion succeeded.");
                    }
                    catch (Exception)
                    {
                        addToLog("Deletion failed.");
                    }
                }
            }
        }
        /// <summary>
        /// postprocesses an audio job followed by a video job
        /// this constellation happens in automated or one click encoding where we have an audio job linked
        /// to a video job
        /// first, any audio jobs previous to the audio job in question will be located
        /// then we get the size of all audio tracks
        /// from the desired final output size stored in the first video job, we calculate the video bitrate
        /// we have to use to obtain the final file size, taking container overhead into account
        /// the calculated bitrate is then applied to all video jobs
        /// </summary>
        /// <param name="firstAudio">the audio job that is linked to a video job</param>
        /// <param name="firstpass">the video job to which the audio job is linked</param>
        private bool postprocessAudioJob(AudioJob firstAudio, VideoJob firstpass)
        {
            if (firstpass.DesiredSizeBytes <= 0) // No desired filesize
            {
                logBuilder.Append("No desired size. Continuing without making any changes.\r\n");
                return true;
            }
            logBuilder.Append("We have an audio job followed by a video job\r\n");
            logBuilder.AppendFormat("The audio job is named {0}. The first video job is named {1}.", firstAudio.Name, firstpass.Name);
            List<VideoJob> allVideoJobs = new List<VideoJob>();
            allVideoJobs.Add(firstpass);
            VideoJob currentJob = firstpass;
            while (jobs.ContainsKey(currentJob.Next) && jobs[currentJob.Next] is VideoJob)
            {
                currentJob = (VideoJob)jobs[currentJob.Next];
                logBuilder.AppendFormat("Found another video job: {0}.{1}", currentJob.Name,
                    Environment.NewLine);
                allVideoJobs.Add(currentJob);
            }
            logBuilder.Append("The audio job is named " + firstAudio.Name + " the first pass " + firstpass.Name + ".\r\n");
            logBuilder.Append("The video job has a desired final output size of " + firstpass.DesiredSizeBytes
                + " bytes and video bitrate of " +
                firstpass.Settings.BitrateQuantizer + " kbit/s\r\n");
            List<AudioStream> audioStreams = new List<AudioStream>();
            AudioJob currentAudio = firstAudio;
            while (true)
            {
                logBuilder.Append("Found a preceding audio job: '" + currentAudio.Name + "'.");
                if (currentAudio.Status == JobStatus.DONE)
                    logBuilder.AppendFormat("{0} completed successfully, taking size into account...", currentAudio.Name);
                else
                {
                    logBuilder.AppendFormat("{0} didn't complete successfully, ignoring...{1}", currentAudio.Name, Environment.NewLine);
                    continue;
                }

                try
                {
                    FileInfo fi = new FileInfo(currentAudio.Output);
                    long audioSize = fi.Length;
                    AudioType audioType = VideoUtil.guessAudioType(currentAudio.Output);
                    if (audioType == null)
                    {
                        logBuilder.Append("Unable to determine audio type of file, ignoring.\r\n");
                        continue;
                    }
                    AudioStream audioStream = new AudioStream();
                    audioStream.Type = audioType;
                    audioStream.SizeBytes = audioSize;
                    audioStreams.Add(audioStream);
                    logBuilder.AppendFormat("The size is of this track is {0} bytes, and the type is {1}. Taking this into account in the bitrate calculation.{2}",
                        audioSize, audioType, Environment.NewLine);

                }
                catch (IOException e)
                {
                    logBuilder.AppendFormat("IOException when trying to get size of audio. Error message: {0}. Ignoring audio file.{1}",
                        e.Message, Environment.NewLine);
                    continue;
                }
                if (!string.IsNullOrEmpty(currentAudio.Previous) && 
                    jobs.ContainsKey(currentAudio.Previous) && jobs[currentAudio.Previous] is AudioJob)
                    currentAudio = (AudioJob)jobs[currentAudio.Previous];
                else
                    break;
            }

            MuxJob mux;
            Job lastJob = firstpass;
            while (!string.IsNullOrEmpty(lastJob.Next) && jobs.ContainsKey(lastJob.Next))
                lastJob = jobs[lastJob.Next];
            if (!(lastJob is MuxJob))
            {
                logBuilder.AppendFormat("For some reason the last job is not a mux job. This is a programming error, please report it. Aborting calculation now.\r\n");
                return false;
            }
            mux = (MuxJob)lastJob;

            int bitrateKBits = 0;
            VideoCodec vCodec = firstpass.Settings.Codec;
            long videoSizeKB = 0;
            bitrateKBits = calc.CalculateBitrateKBits(vCodec, firstpass.Settings.NbBframes > 0, mux.ContainerType,
                audioStreams.ToArray(), firstpass.DesiredSizeBytes, firstpass.NumberOfFrames,
                firstpass.Framerate, out videoSizeKB);

            logBuilder.Append("Desired video size after substracting audio size is " + videoSizeKB + "KBs. ");
            logBuilder.Append("Setting the desired bitrate of the subsequent video jobs to " + bitrateKBits + " kbit/s.\r\n");
            foreach (VideoJob job in allVideoJobs)
                jobUtil.updateVideoBitrate(job, bitrateKBits);
            return true;
        }
        #endregion
        /// <summary>
        /// updates the actual GUI with the status information received as parameter
        /// If the StatusUpdate indicates that the job has ended, the Progress window is closed
        /// and the logging messages from the StatusUpdate object are added to the log tab
        /// if the job mentioned in the statusupdate has a next job name defined, the job is looked
        /// up and processing of that job starts - this applies even in queue encoding mode
        /// the linked jobs will always be encoded first, regardless of their position in the queue
        /// If we're in queue encoding mode, the next nob in the queue is also started
        /// </summary>
        /// <param name="su">StatusUpdate object containing the current encoder stats</param>
        private void UpdateGUIStatus(StatusUpdate su)
        {
            if (su.IsComplete)
            {
                this.TitleText = Application.ProductName + " " + Application.ProductVersion;
                Job job = markJobDone(su);
                currentProcessor = null;
                this.jobProgress.Value = 0;
                if (pw != null)
                {
                    pw.IsUserAbort = false; // ensures that the window will be closed
                    pw.Close();
                    pw = null;
                }
                logBuilder.Append("Processing ended at " + DateTime.Now.ToLongTimeString() + "\r\n");
                logBuilder.Append("----------------------------------------------------------------------------------------------------------" +
                    "\r\n\r\nLog for job " + su.JobName + "\r\n\r\n" + su.Log +
                    "\r\n----------------------------------------------------------------------------------------------------------\r\n");
                log.Text = logBuilder.ToString();
                log.ScrollToCaret();
                bool jobCompletedSuccessfully = true; // true = no errors; false = some reason to stop
                if (su.WasAborted)
                {
                    logBuilder.Append("The current job was aborted. Stopping queue mode\r\n");
                    jobCompletedSuccessfully = false;
                }
                if (su.HasError)
                {
                    logBuilder.Append("The current job contains errors. Skipping chained jobs\r\n");
                    Job j = null;
                    jobCompletedSuccessfully = false;
                    if (job.Next != null)
                    {
                        j = job;
                        while (j.Next != null) // find all chained jobs
                        {
                            j = jobs[j.Next];
                            this.skipJobs.Add(j);
                        }
                    }
                }
                if (jobCompletedSuccessfully)
                    deleteIntermediateFiles(job);
                if (job is IndexJob && jobCompletedSuccessfully) // only process if there was no abort/error
                {
                    jobUtil.postprocessIndexJob((IndexJob)job);
                }
                this.isEncoding = false;
                if (job.Next != null && jobCompletedSuccessfully) // try finding a chained job
                {
                    Job next = jobs[job.Next];
                    logBuilder.Append("job " + job.Name + " has been processed. This job is linked to the next job: " + next.Name + "\r\n");
                    if (next is VideoJob && job is AudioJob) // fully automatic encoding, update bitrate
                    {
                        this.postprocessAudioJob((AudioJob)job, (VideoJob)next);
                    }
                    log.Text = logBuilder.ToString();
                }
                int nextJobStart = 0;
                if (this.queueEncoding)
                {
                    nextJobStart = startNextJobInQueue(); //new with the return value to check if there was another job
                }
                else
                {
                    nextJobStart = 2;
                }
                if (nextJobStart == 2)
                {	//new test if this was the last job or a job was stoped
                    this.isEncoding = false;	//moved out the else before
                    this.queueEncoding = false;
                    this.startStopButton.Text = "Start";
                    this.abortButton.Enabled = false;
                    this.shutdown();
                }
            }
            else // job is not complete yet
            {
                try
                {
                    if (pw.IsHandleCreated) // the window is there, send the update to the window
                    {
                        pw.Invoke(new UpdateStatusCallback(pw.UpdateStatus), new object[] { su });
                    }
                }
                catch (Exception e)
                {
                    logBuilder.Append("Exception when trying to update status while a job is running. Text: " + e.Message + " stacktrace: " + e.StackTrace);
                    this.log.Text = logBuilder.ToString();
                }
                string percentage = su.PercentageDoneExact.ToString("##.##");
                if (percentage.IndexOf(".") != -1 && percentage.Substring(percentage.IndexOf(".")).Length == 1)
                    percentage += "0";
                this.TitleText = "MeGUI " + su.JobName + " " + percentage + "% ";
                if (this.settings.Shutdown)
                    this.TitleText += "- SHUTDOWN after encode";
                this.jobProgress.Value = su.PercentageDone;
            }
        }
        #region button callbacks
        /// <summary>
        /// callback for the Progress Window
        /// This is called when the progress window has been closed and ensures that
        /// no futher attempt is made to send a statusupdate to the progress window
        /// </summary>
        private void pw_WindowClosed(bool hideOnly)
        {
            mnuViewProcessStatus.Checked = false;
            if (!hideOnly)
                pw = null;
        }
        /// <summary>
        /// callback for the progress window
        /// this method is called if the abort button in the progress window is called
        /// it stops the encoder cold
        /// </summary>
        private void pw_Abort()
        {
            abort();
        }
        /// <summary>
        /// catches the ChangePriority event from the progresswindow and forward it to the encoder class
        /// </summary>
        /// <param name="priority"></param>
        private void pw_PriorityChanged(ProcessPriority priority)
        {
            string error;
            if (!currentProcessor.changePriority(priority, out error))
            {
                addToLog("Error when attempting to change priority: " + error);
            }
        }
        #endregion
        #endregion
        #region radiobutton action
        #region audio
        /// <summary>
        /// handles the user switching from one audio track to another
        /// makes sure the current i/o configuration and the settings are saved so they can be retrieved later on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioTrack_CheckedChanged(object sender, System.EventArgs e)
        {
            int current = 0;
            if (audioTrack2.Checked) // user switched from track 2 to track 1
                current = 1;
            string error = this.verifyAudioSettings();
            if (error != null && !String.IsNullOrEmpty(this.audioInput.Text))
            {
                MessageBox.Show(error, "Unsupported audio configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (String.IsNullOrEmpty(this.audioInput.Text))
            {
                this.audioStreams[lastSelectedAudioTrackNumber].path = "";
                this.audioStreams[lastSelectedAudioTrackNumber].output = "";
                this.audioStreams[lastSelectedAudioTrackNumber].Type = null;
                this.audioStreams[lastSelectedAudioTrackNumber].settings = null;
            }
            else
            {
                this.audioStreams[lastSelectedAudioTrackNumber].path = this.audioInput.Text;
                this.audioStreams[lastSelectedAudioTrackNumber].output = this.audioOutput.Text;
                this.audioStreams[lastSelectedAudioTrackNumber].Type = CurrentAudioOutputType;
                this.audioStreams[lastSelectedAudioTrackNumber].settings = (audioCodec.SelectedItem as IAudioSettingsProvider).GetCurrentSettings();
            }
            this.audioInput.Text = this.audioStreams[current].path;
            this.audioOutput.Text = this.audioStreams[current].output;
            if (audioStreams[current].settings != null)
            {
                foreach (IAudioSettingsProvider p in this.audioCodec.Items)
                {
                    if (p.IsSameType(audioStreams[current].settings))
                    {
                        p.LoadSettings(audioStreams[current].settings);
                        audioCodec.SelectedItem = p;
                        break;
                    }
                }
            }
            lastSelectedAudioTrackNumber = current;
        }
        #endregion
        #endregion
        #region helper methods
        private void updateProcessingStatus()
        {
            if (this.isEncoding)
            {
                if (this.paused)
                {
                    pauseButton.Image = playImage;
                    pauseButton.Enabled = true;
                    pauseToolStripMenuItem.Image = playImage;
                    pauseToolStripMenuItem.Text = "Resume";
                }
                else
                {
                    pauseButton.Image = pauseImage;
                    pauseButton.Enabled = true;
                    pauseToolStripMenuItem.Image = pauseImage;
                    pauseToolStripMenuItem.Text = "Pause";
                    pauseToolStripMenuItem.Enabled = true;
                }
                if (this.queueEncoding)
                {
                    startStopButton.Text = "Stop";
                    startToolStripMenuItem.Text = "Stop";
                }
                else
                {
                    startStopButton.Text = "Start";
                    startToolStripMenuItem.Text = "Start";
                }
                abortToolStripMenuItem.Enabled = true;
                abortButton.Enabled = true;
            }
            else
            {
                startStopButton.Text = "Start";
                startToolStripMenuItem.Text = "Start";
                pauseButton.Image = pauseImage;
                pauseButton.Enabled = false;
                pauseToolStripMenuItem.Image = pauseImage;
                pauseToolStripMenuItem.Text = "Pause";
                pauseToolStripMenuItem.Enabled = false;
                abortToolStripMenuItem.Enabled = false;
                abortButton.Enabled = false;
            }
        }

        private string TitleText
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

        #region IO
        /// <summary>
        /// A wrapper for MeGUI.GetDirectoryName which ensures that it has no trailing / or \
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string filename)
        {
            return Path.GetDirectoryName(filename).TrimEnd(new char[] { '\\', '/' });
        }
        #endregion
        /// <summary>
        /// shuts down the PC if the shutdown option is set
        /// also saves all profiles, jobs and the log as MeGUI is killed
        /// via the shutdown so the appropriate methods in the OnClosing are not called
        /// </summary>
        private void shutdown()
        {
            if (this.settings.Shutdown)
            {
                this.profileManager.SaveProfiles();
                this.saveSettings();
                this.saveJobs();
                this.saveLog();
                bool succ = Shutdown.shutdown();
                if (!succ)
                    logBuilder.Append("Tried shutting down system at " + DateTime.Now.ToShortTimeString() + " but the call failed");
                else
                    logBuilder.Append("Shutdown initiated at " + DateTime.Now.ToShortTimeString());
            }
        }
        /// <summary>
        /// aborts the currently active job
        /// </summary>
        private void abort()
        {
            string error;
            if (this.isEncoding)
            {
                if (!currentProcessor.stop(out error))
                {
                    logBuilder.AppendLine("Error when trying to stop processing: " + error);
                }
            }
            this.markJobAborted();
            this.isEncoding = false;
            this.queueEncoding = false;
            if (this.paused) // aborting directly causes problems so prevent it
            {
                this.paused = false;
                if (currentProcessor.resume(out error))
                {
                    logBuilder.AppendLine("Error when trying to resume processing: " + error);
                }
            }
            this.shutdownCheckBox.Checked = false;
            this.settings.Shutdown = false;
            updateProcessingStatus();
        }
        /// <summary>
        /// adds a string to the log
        /// </summary>
        /// <param name="logEntry"></param>
        public void addToLog(string logEntry)
        {
            this.logBuilder.Append(logEntry);
            this.log.Text = logBuilder.ToString();
        }
        /// <summary>
        /// callback for the video player window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void player_Closed(object sender, EventArgs e)
        {
            parX = player.PARX;
            parY = player.PARY;
            this.player = null;
        }
        /// <summary>
        /// iterates through all zones and makes sure we get no intersection by applying the current credits settings
        /// </summary>
        /// <param name="creditsStartFrame">the credits start frame</param>
        /// <returns>returns true if there is no intersetion between zones and credits and false if there is an intersection</returns>
        private bool validateCredits(int creditsStartFrame)
        {
            VideoCodecSettings settings = this.CurrentVideoCodecSettings;
            foreach (Zone z in settings.Zones)
            {
                if (creditsStartFrame <= z.endFrame) // credits start before end of this zone -> intersection
                {
                    MessageBox.Show("The start of the end credits intersects with an already configured zone\ngoing from frame " + z.startFrame + " to frame " + z.endFrame +
                        "\nPlease select another credits start frame or reconfigure the zone in the codec configuration.", "Zone intersection detected",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// iteratees through all zones and makes sure we get no intersection by applying the current intro settings
        /// </summary>
        /// <param name="introEndFrame">the frame where the intro ends</param>
        /// <returns>true if the intro zone does not interesect with a zone, false otherwise</returns>
        private bool validateIntro(int introEndFrame)
        {
            VideoCodecSettings settings = this.CurrentVideoCodecSettings;
            foreach (Zone z in settings.Zones)
            {
                if (introEndFrame >= z.startFrame)
                {
                    MessageBox.Show("The end of the intro intersects with an already configured zone\ngoing from frame " + z.startFrame + " to frame " + z.endFrame +
                        "\nPlease select another credits start frame or reconfigure the zone in the codec configuration.", "Zone intersection detected",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// sets the intro end / credits start frame
        /// </summary>
        /// <param name="frameNumber">the frame number</param>
        /// <param name="isCredits">true if the credits start frame is to be set, false if the intro end is to be set</param>
        private void player_IntroCreditsFrameSet(int frameNumber, bool isCredits)
        {
            if (isCredits)
            {
                if (validateCredits(frameNumber))
                {
                    player.CreditsStart = frameNumber;
                    this.creditsStartFrame = frameNumber;
                }
                else
                    player.CreditsStart = -1;
            }
            else
            {
                if (validateIntro(frameNumber))
                {
                    this.introEndFrame = frameNumber;
                    player.IntroEnd = frameNumber;
                }
                else
                    player.IntroEnd = -1;
            }
        }
        /// <summary>
        /// enables / disables output fields depending on the codec configuration
        /// </summary>
        private void updateIOConfig()
        {
            int encodingMode = CurrentVideoCodecSettingsProvider.GetCurrentSettings().EncodingMode;
            if (encodingMode == 2 || encodingMode == 5) // first pass
            {
                videoOutputOpenButton.Enabled = false;
                videoOutput.Text = "";
                videoOutput.Enabled = false;
                fileType.Enabled = false;
            }
            else if (!videoOutputOpenButton.Enabled)
            {
                videoOutputOpenButton.Enabled = true;
                videoOutput.Enabled = true;
                fileType.Enabled = true;
            }
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
        public void setAudioTrack(int trackNumber, string input)
        {
            // Change view to other track
            if (trackNumber == 0)
            {
                audioTrack1.Checked = true;
                audioTrack2.Checked = false;
            }
            else if (trackNumber == 1)
            {
                audioTrack1.Checked = false;
                audioTrack2.Checked = true;
            }
            if (trackNumber >= 0 && trackNumber <= 1)
                this.openAudioFile(input);
        }
        /// <summary>
        /// transfers common settings like name, position, linking and status from one job to another
        /// </summary>
        /// <param name="job">the old job</param>
        /// <param name="newJob">the new job</param>
        private void transferJobSettings(Job job, Job newJob)
        {
            newJob.End = job.End;
            newJob.FPS = job.FPS;
            newJob.Name = job.Name;
            newJob.Next = job.Next;
            newJob.Position = job.Position;
            newJob.Previous = job.Previous;
            newJob.Priority = this.Settings.DefaultPriority;
            newJob.Start = job.Start;
            newJob.Status = job.Status;
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
            if (pw != null)
                pw.Hide();
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
                    this.shutdownCheckBox.Checked = this.settings.Shutdown;
                    this.saveSettings();
                }
            }
        }
        private void mnuToolsD2VCreator_Click(object sender, System.EventArgs e)
        {
            VobinputWindow vobInput = new VobinputWindow(this);
            vobInput.ShowDialog();
        }

        private void mnuToolsAviSynth_Click(object sender, System.EventArgs e)
        {
            if (this.player != null) // make sure only one preview window is open at all times
                player.Close();
            using (AviSynthWindow asw = new AviSynthWindow(this))
            {
                asw.OpenScript += new OpenScriptCallback(openVideoFile);
                asw.ShowDialog(this);
            }
        }
        private void mnuQuantEditor_Click(object sender, System.EventArgs e)
        {
            using (QuantizerMatrixDialog qmd = new QuantizerMatrixDialog())
            {
                qmd.ShowDialog();
            }
        }
        private void mnuAVCLevelValidation_Click(object sender, System.EventArgs e)
        {
            if (this.videoInput.Text.Equals(""))
                MessageBox.Show("You first need to load an AviSynth script", "No video configured",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                int compliantLevel = 15;
                x264Settings currentX264Settings = (x264Settings)CodecManager.X264.GetCurrentSettings();
                bool succ = jobUtil.validateAVCLevel(videoInput.Text, currentX264Settings, out compliantLevel);
                if (succ)
                    MessageBox.Show("This file matches the criteria for the level chosen", "Video validated",
                        MessageBoxButtons.OK);
                else
                {
                    if (compliantLevel == -1)
                        MessageBox.Show("Unable to open video", "Test failed", MessageBoxButtons.OK);
                    else
                    {
                        AVCLevels al = new AVCLevels();
                        string[] levels = al.getLevels();
                        string levelRequired = levels[compliantLevel];
                        string message = "This video source cannot be encoded to comply with the chosen level.\n"
                            + "You need at least level " + levelRequired + " for this source. Do you want\n"
                            + "to increase the level automatically now?";
                        DialogResult dr = MessageBox.Show(message, "Test failed", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                            currentX264Settings.Level = compliantLevel;
                    }
                }
            }
        }

        private void mnuAVCLevelValidation_Popup(object sender, System.EventArgs e)
        {
            if (videoInput.Text.Equals(""))
                mnuAVCLevelValidation.Enabled = false;
            else
                mnuAVCLevelValidation.Enabled = true;
        }
        private void mnuChapterCreator_Click(object sender, System.EventArgs e)
        {
            using (ChapterCreator cc = new ChapterCreator())
            {
                cc.VideoInput = this.videoInput.Text;
                cc.CreditsStartFrame = this.creditsStartFrame;
                cc.IntroEndFrame = this.introEndFrame;
                cc.ShowDialog();
            }
        }
        private void mnuMuxer_Click(object sender, System.EventArgs e)
        {
            if ((!(sender is System.Windows.Forms.MenuItem)) || (!((sender as MenuItem).Tag is IMuxing)))
                return;
            using (MuxWindow mw = new MuxWindow((IMuxing)((sender as MenuItem).Tag)))
            {
                if (mw.ShowDialog() == DialogResult.OK)
                {
                    MuxJob job = mw.Job;
                    int freeJobNumber = getFreeJobNumber();
                    job.Name = "job" + freeJobNumber;
                    addJobToQueue(job);
                    if (Settings.AutoStartQueue)
                        startEncoding(job);
                }
            }
        }

        private void mnuToolsBitrateCalculator_Click(object sender, System.EventArgs e)
        {
            using (Calculator calc = new Calculator())
            {
                int nbFrames = 0;
                double framerate = 0.0;
                audioTrack_CheckedChanged(null, null);
                if (!videoInput.Text.Equals(""))
                    jobUtil.getInputProperties(out nbFrames, out framerate, videoInput.Text);
                calc.setDefaults(nbFrames, framerate, (IVideoSettingsProvider)videoCodec.SelectedItem, audioStreams[0], audioStreams[1]);

                DialogResult dr = calc.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    videoCodec.SelectedItem = calc.getSelectedCodec();
                    VideoCodecSettings settings = CurrentVideoCodecSettingsProvider.GetCurrentSettings();
                    if (settings.EncodingMode == 1 || settings.EncodingMode == 9)
                    {
                        settings.EncodingMode = 0;
                    }
                    settings.BitrateQuantizer = calc.getBitrate();
                }
            }
        }

        private void mnuToolsOneClick_Click(object sender, EventArgs e)
        {
            using (OneClickWindow ocmt = new OneClickWindow(this, videoProfile.SelectedIndex,
                audioProfile.SelectedIndex, jobUtil, videoEncoderProvider, audioEncoderProvider))
            {
                ocmt.ShowDialog();
            }
        }
        private void mnuView_Popup(object sender, System.EventArgs e)
        {
            if (pw != null)
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
            if (pw != null)
            {
                if (mnuViewProcessStatus.Checked)
                {
                    mnuViewProcessStatus.Checked = false;
                    pw.Hide();
                }
                else
                {
                    mnuViewProcessStatus.Checked = true;
                    pw.Show();
                }
            }
            else
            {
                Debug.Assert(false, "ProgressWindow should not be null if we can get here");
            }
        }

        private void mnuToolsOneClickConfig_Click(object sender, EventArgs e)
        {
            using (OneClickConfigurationDialog dialog = new OneClickConfigurationDialog(this.videoProfile.SelectedIndex, this.audioProfile.SelectedIndex, this))
            {
                dialog.ShowDialog();
            }
        }
        /// <summary>
        /// launches the adaptive muxer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuToolsAdaptiveMuxer_Click(object sender, EventArgs e)
        {
            AdaptiveMuxWindow amw = new AdaptiveMuxWindow(this);
            if (amw.ShowDialog() == DialogResult.OK)
            {
                MuxJob[] jobs = amw.Jobs;
                int freeJobNumber = getFreeJobNumber();
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
                foreach (MuxJob job in jobs)
                    addJobToQueue(job);
                if (Settings.AutoStartQueue)
                    startEncoding(jobs[0]);
            }

        }
        /// <summary>
        /// starts the updater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuUpdate_Click(object sender, EventArgs e)
        {
            UpdateWindow update = new UpdateWindow(this, this.Settings);
            update.ShowDialog();
        }
        /// <summary>
        /// launches the megui wiki in the default browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGuide_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.project357.com/MeGUIwiki/index.php?title=Main_Page");
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
        #endregion
        #region properties
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
        /// gets / sets if we are in queue encoding mode
        /// </summary>
        public bool QueueEncoding
        {
            get { return queueEncoding; }
            set { queueEncoding = value; }
        }

        /// <summary>
        /// returns video input and output configuration
        /// </summary>
        public string[] VideoIO
        {
            get
            {
                return new string[] { videoInput.Text, videoOutput.Text };
            }
        }
        /// <summary>
        /// gets the path from where MeGUI was launched
        /// </summary>
        public string MeGUIPath
        {
            get { return this.path; }
        }
        /// <summary>
        /// gets  / sets the currently selected audiostream
        /// </summary>
        public Dictionary<string, AviSynthProfile> AvsProfiles
        {
            get { return this.profileManager.AvsProfiles; }
        }

        private AudioStream CurrentAudioStream
        {
            get
            {
                if (this.audioTrack1.Checked)
                    return this.audioStreams[0];
                else
                    return this.audioStreams[1];
            }
            set
            {
                if (this.audioTrack1.Checked)
                    this.audioStreams[0] = value;
                else
                    this.audioStreams[1] = value;
            }
        }
        /// <summary>
        ///  returns the video codec settings for the currently active video codec
        /// </summary>
        public VideoCodecSettings CurrentVideoCodecSettings
        {
            get
            {
                return CurrentVideoCodecSettingsProvider.GetCurrentSettings();
            }

            set
            {
                foreach (IVideoSettingsProvider p in videoCodec.Items)
                {
                    if (p.IsSameType(value))
                    {
                        p.LoadSettings(value);
                        videoCodec.SelectedItem = p;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// returns the audio codec settings for the currently active audio codec
        /// </summary>
        public AudioCodecSettings AudCodecSettings
        {
            get
            {
                return (this.audioCodec.SelectedItem as IAudioSettingsProvider).GetCurrentSettings();
            }
        }

        public MuxableType CurrentMuxableVideoType
        {
            get { return new MuxableType(CurrentVideoOutputType, CurrentVideoCodecSettings.Codec); }
        }

        public VideoType CurrentVideoOutputType
        {
            get { return this.fileType.SelectedItem as VideoType; }
        }
        public AudioType CurrentAudioOutputType
        {
            get { return this.audioContainer.SelectedItem as AudioType; }
        }
        public MeGUISettings Settings
        {
            get { return settings; }
        }
        private IVideoSettingsProvider CurrentVideoCodecSettingsProvider
        {
            get
            {
                return this.videoCodec.SelectedItem as IVideoSettingsProvider;
            }
        }
        private IAudioSettingsProvider CurrentAudioSettingsProvider
        {
            get
            {
                return this.audioCodec.SelectedItem as IAudioSettingsProvider;
            }
        }
        #endregion
        #region tray action
        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            if (pw != null && mnuViewProcessStatus.Checked)
                pw.Show();
            trayIcon.Visible = false;
        }
        private void openMeGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            if (pw != null && mnuViewProcessStatus.Checked)
                pw.Show();
            trayIcon.Visible = false;
        }

        #endregion
        #region file opening
        public void openVideoFile(string fileName)
        {
            this.creditsStartFrame = -1;
            this.introEndFrame = -1;
            this.videoInput.Text = fileName;
            parX = parY = -1;
            //reset the zones for all codecs, zones are supposed to be source bound
            foreach (IVideoSettingsProvider p in (videoCodec.Items))
            {
                VideoCodecSettings s = p.GetCurrentSettings();
                s.Zones = new Zone[0];
                p.LoadSettings(s);
            }
            if (settings.AutoOpenScript)
                openAvisynthScript(fileName);
            string filePath = Path.GetDirectoryName(fileName);
            string fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
            this.videoOutput.Text = Path.Combine(filePath, fileNameNoExtension) + this.settings.VideoExtension + ".extension";
            this.videoOutput.Text = Path.ChangeExtension(this.videoOutput.Text, this.CurrentVideoOutputType.Extension);
            updateIOConfig();
        }
        public void openAudioFile(string fileName)
        {
            this.audioInput.Text = fileName;
            int del = getDelay(fileName);
            AudioCodecSettings settings = (audioCodec.SelectedItem as IAudioSettingsProvider).GetCurrentSettings();
            if (del != 0) // we have a delay we are interested in
            {
                settings.DelayEnabled = true;
                settings.Delay = del;
            }
            else
            {
                settings.DelayEnabled = false;
            }
            this.audioOutput.Text = Path.ChangeExtension(fileName, this.CurrentAudioOutputType.Extension);
        }
        public void openOtherVideoFile(string fileName)
        {
            AviSynthWindow asw = new AviSynthWindow(this, fileName);
            asw.OpenScript += new OpenScriptCallback(openVideoFile);
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
            OneClickWindow ocmt = new OneClickWindow(this, videoProfile.SelectedIndex,
                audioProfile.SelectedIndex, jobUtil, videoEncoderProvider, audioEncoderProvider);
            ocmt.Input = fileName;
            ocmt.ShowDialog();
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
                    openVideoFile(file);
                    break;
                case FileType.AUDIOINPUT:
                    openAudioFile(file);
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
            videoProfile.Items.Clear();
            foreach (string name in this.profileManager.VideoProfiles.Keys)
            {
                this.videoProfile.Items.Add(name);
            }
            audioProfile.Items.Clear();
            foreach (string name in this.profileManager.AudioProfiles.Keys)
            {
                this.audioProfile.Items.Add(name);
            }
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
            videoProfile.Items.Clear();
            foreach (string name in this.profileManager.VideoProfiles.Keys)
            {
                this.videoProfile.Items.Add(name);
            }
            audioProfile.Items.Clear();
            foreach (string name in this.profileManager.AudioProfiles.Keys)
            {
                this.audioProfile.Items.Add(name);
            }
        }

        private void mnuFileImport_Click(object sender, EventArgs e)
        {
            ProfilePorter importer = new ProfilePorter(profileManager, true, this);
            importer.ShowDialog();
            videoProfile.Items.Clear();
            foreach (string name in this.profileManager.VideoProfiles.Keys)
            {
                this.videoProfile.Items.Add(name);
            }
            audioProfile.Items.Clear();
            foreach (string name in this.profileManager.AudioProfiles.Keys)
            {
                this.audioProfile.Items.Add(name);
            }
        }

        private void mnuFileExport_Click(object sender, EventArgs e)
        {
            ProfilePorter exporter = new ProfilePorter(profileManager, false, this);
            exporter.ShowDialog();
        }
        #endregion
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
    }
}
    public class CommandlineUpgradeData
    {
        public List<string> filename = new List<string>();
        public List<string> tempFilename = new List<string>();
        public string newVersion;
    }
