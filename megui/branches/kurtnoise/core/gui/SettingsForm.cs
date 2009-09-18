// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MeGUI
{
	/// <summary>
	/// Summary description for SettingsForm.
	/// </summary>
	public class SettingsForm : System.Windows.Forms.Form
	{
		#region variables
        private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
        private MeGUISettings internalSettings = new MeGUISettings();
        private Button resetDialogs;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private SourceDetectorSettings sdSettings;
		private System.Windows.Forms.CheckBox autoForceFilm;
		private System.Windows.Forms.NumericUpDown forceFilmPercentage;
        private System.Windows.Forms.Label percentLabel;
		private System.Windows.Forms.GroupBox vobGroupBox;
		private System.Windows.Forms.Label defaultAudioTrack1Label;
		private System.Windows.Forms.ComboBox defaultLanguage1;
		private System.Windows.Forms.Label defaultAudioTrack2Label;
        private System.Windows.Forms.ComboBox defaultLanguage2;
        private System.Windows.Forms.FolderBrowserDialog openFolderDialog;
        private CheckBox chkboxUseAdvancedTooltips;
        private Button configSourceDetector;
        private System.Windows.Forms.GroupBox otherGroupBox;
		private System.Windows.Forms.ComboBox priority;
		private System.Windows.Forms.OpenFileDialog openExecutableDialog;
        private System.Windows.Forms.CheckBox autostartQueue;
		private System.Windows.Forms.CheckBox openScript;
		private System.Windows.Forms.Label priorityLabel;
		private System.Windows.Forms.CheckBox deleteCompletedJobs;
		private System.Windows.Forms.CheckBox deleteAbortedOutput;
		private System.Windows.Forms.CheckBox deleteIntermediateFiles;
        private System.Windows.Forms.CheckBox openProgressWindow;
        private NumericUpDown acceptableAspectError;
        private Label acceptableAspectErrorLabel;
        private AutoEncodeDefaultsSettings autoEncodeDefaults;
        private TabPage tabPage3;
        private GroupBox autoUpdateGroupBox;
        private CheckBox useAutoUpdateCheckbox;
        private GroupBox outputExtensions;
        private TextBox videoExtension;
        private Label label11;
        private Label label12;
        private TextBox audioExtension;
        private GroupBox autoModeGroupbox;
        private Label label13;
        private NumericUpDown nbPasses;
        private Label audioExtLabel;
        private Label videoExtLabel;
        private Button autoEncodeDefaultsButton;
        private TextBox command;
        private RadioButton runCommand;
        private RadioButton shutdown;
        private RadioButton donothing;
        private Button configureServersButton;
        private Label label14;
        private NumericUpDown maxServersToTry;
        private NumericUpDown acceptableFPSError;
        private Label label15;
        private NumericUpDown audiosamplesperupdate;
        private Label label6;
        private MeGUI.core.gui.HelpButton helpButton1;
        private TabPage tabPage2;
        private TabControl tabControl2;
        private TabPage tabPage4;
        private Label xvidEncrawLabel;
        private Button selectXvidEncrawButton;
        private TextBox xvidEncrawPath;
        private Button selectMencoderExecutableButton;
        private TextBox mencoderPath;
        private Label mencoderPathLabel;
        private Label x264ExePathLabel;
        private TextBox x264ExePath;
        private Button selectX264ExecutableButton;
        private TabPage tabPage5;
        private TabPage tabPage7;
        private Button button9;
        private Button selectAvisynthPluginsDir;
        private TextBox textBox9;
        private Label label16;
        private TextBox avisynthPluginsDir;
        private TextBox dgIndexPath;
        private Label dgIndexLabel;
        private Button selectDGIndexExecutable;
        private Label avisynthPluginsLabel;
        private TabPage tabPage6;
        private TextBox aviMuxGUIPath;
        private TextBox mp4boxPath;
        private Button selectMkvmergeExecutableButton;
        private Button selectAviMuxGUIExecutableButton;
        private TextBox mkvmergePath;
        private Button selectMP4boxExecutableButton;
        private Label mp4boxPathLabel;
        private Label mkvmergePathLabel;
        private Label avimuxguiPathLabel;
        private TextBox tbEncAacPlus;
        private Label label9;
        private Button button6;
        private TextBox tbOggEnc;
        private Label label7;
        private Button button4;
        private TextBox tbLame;
        private Label label3;
        private Button button3;
        private TextBox tbNeroAacEnc;
        private Label label2;
        private Button button2;
        private CheckBox keep2ndPassOutput;
        private CheckBox keep2ndPassLogFile;
        private Label label17;
        private Button button10;
        private TextBox meguiUpdateCache;
        private Button configAutoEncodeDefaults;
        private TextBox textBox8;
        private Label besplit;
        private Button button8;
        private TextBox tbAften;
        private Label lbAften;
        private Button selectAftenExecutableButton;
        private GroupBox gbVideoPreview;
        private CheckBox chAlwaysOnTop;
        private GroupBox groupBox2;
        private TextBox txt_httpproxyuid;
        private TextBox txt_httpproxyaddress;
        private Label label21;
        private Label label20;
        private Label label19;
        private Label label18;
        private CheckBox cbx_usehttpproxy;
        private TextBox txt_httpproxypwd;
        private TextBox txt_httpproxyport;
        private CheckBox cbAddTimePos;
        private TextBox dgavcIndexPath;
        private Label DGAIndex;
        private Button selectDGAIndexExecutable;
        private TextBox dgvc1IndexPath;
        private Label label5;
        private Button selectDGVIndexExecutable;
        private TextBox dgmpgIndexPath;
        private Label label22;
        private Button selectDGMIndexExecutable;
        private TextBox eac3toPath;
        private Label label23;
        private Button selectEAC3toExecutable;
        private TextBox tsmuxerPath;
        private Label tsmuxerPathLabel;
        private Button selectTSMuxerExecutableButton;
        private CheckBox backupfiles;
        private CheckBox forcerawavcuse;
        private Label label1;
        private TextBox DivXAVCPath;
        private Button selectDivXAVCExecButton;
        private GroupBox gbDefaultOutput;
        private Button clearDefaultOutputDir;
        private FileBar defaultOutputDir;
        private TextBox tbFlac;
        private Label label4;
        private Button selectFlacExecutableButton;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region start / stop
		public SettingsForm()
		{
			InitializeComponent();
            List<string> keys = new List<string>(LanguageSelectionContainer.Languages.Keys);
            defaultLanguage2.DataSource = defaultLanguage1.DataSource = keys;
            defaultLanguage2.BindingContext = new BindingContext();
            defaultLanguage1.BindingContext = new BindingContext();
            this.avisynthPluginsDir.Text = "" + MeGUISettings.AvisynthPluginsPath;
            this.meguiUpdateCache.Text = "" + MeGUISettings.MeGUIUpdateCache;
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.GroupBox groupBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.command = new System.Windows.Forms.TextBox();
            this.runCommand = new System.Windows.Forms.RadioButton();
            this.shutdown = new System.Windows.Forms.RadioButton();
            this.donothing = new System.Windows.Forms.RadioButton();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.otherGroupBox = new System.Windows.Forms.GroupBox();
            this.forcerawavcuse = new System.Windows.Forms.CheckBox();
            this.audiosamplesperupdate = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.acceptableFPSError = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.acceptableAspectError = new System.Windows.Forms.NumericUpDown();
            this.acceptableAspectErrorLabel = new System.Windows.Forms.Label();
            this.resetDialogs = new System.Windows.Forms.Button();
            this.configSourceDetector = new System.Windows.Forms.Button();
            this.chkboxUseAdvancedTooltips = new System.Windows.Forms.CheckBox();
            this.openProgressWindow = new System.Windows.Forms.CheckBox();
            this.deleteIntermediateFiles = new System.Windows.Forms.CheckBox();
            this.deleteAbortedOutput = new System.Windows.Forms.CheckBox();
            this.deleteCompletedJobs = new System.Windows.Forms.CheckBox();
            this.openScript = new System.Windows.Forms.CheckBox();
            this.autostartQueue = new System.Windows.Forms.CheckBox();
            this.priority = new System.Windows.Forms.ComboBox();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.vobGroupBox = new System.Windows.Forms.GroupBox();
            this.defaultLanguage2 = new System.Windows.Forms.ComboBox();
            this.defaultAudioTrack2Label = new System.Windows.Forms.Label();
            this.defaultLanguage1 = new System.Windows.Forms.ComboBox();
            this.defaultAudioTrack1Label = new System.Windows.Forms.Label();
            this.percentLabel = new System.Windows.Forms.Label();
            this.forceFilmPercentage = new System.Windows.Forms.NumericUpDown();
            this.autoForceFilm = new System.Windows.Forms.CheckBox();
            this.openExecutableDialog = new System.Windows.Forms.OpenFileDialog();
            this.openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gbDefaultOutput = new System.Windows.Forms.GroupBox();
            this.defaultOutputDir = new MeGUI.FileBar();
            this.clearDefaultOutputDir = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_httpproxyport = new System.Windows.Forms.TextBox();
            this.txt_httpproxypwd = new System.Windows.Forms.TextBox();
            this.txt_httpproxyuid = new System.Windows.Forms.TextBox();
            this.txt_httpproxyaddress = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cbx_usehttpproxy = new System.Windows.Forms.CheckBox();
            this.gbVideoPreview = new System.Windows.Forms.GroupBox();
            this.cbAddTimePos = new System.Windows.Forms.CheckBox();
            this.chAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.autoUpdateGroupBox = new System.Windows.Forms.GroupBox();
            this.backupfiles = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.maxServersToTry = new System.Windows.Forms.NumericUpDown();
            this.configureServersButton = new System.Windows.Forms.Button();
            this.useAutoUpdateCheckbox = new System.Windows.Forms.CheckBox();
            this.outputExtensions = new System.Windows.Forms.GroupBox();
            this.videoExtension = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.audioExtension = new System.Windows.Forms.TextBox();
            this.autoModeGroupbox = new System.Windows.Forms.GroupBox();
            this.configAutoEncodeDefaults = new System.Windows.Forms.Button();
            this.keep2ndPassLogFile = new System.Windows.Forms.CheckBox();
            this.keep2ndPassOutput = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.nbPasses = new System.Windows.Forms.NumericUpDown();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.DivXAVCPath = new System.Windows.Forms.TextBox();
            this.selectDivXAVCExecButton = new System.Windows.Forms.Button();
            this.xvidEncrawLabel = new System.Windows.Forms.Label();
            this.selectXvidEncrawButton = new System.Windows.Forms.Button();
            this.xvidEncrawPath = new System.Windows.Forms.TextBox();
            this.selectMencoderExecutableButton = new System.Windows.Forms.Button();
            this.mencoderPath = new System.Windows.Forms.TextBox();
            this.mencoderPathLabel = new System.Windows.Forms.Label();
            this.x264ExePathLabel = new System.Windows.Forms.Label();
            this.x264ExePath = new System.Windows.Forms.TextBox();
            this.selectX264ExecutableButton = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tbAften = new System.Windows.Forms.TextBox();
            this.lbAften = new System.Windows.Forms.Label();
            this.selectAftenExecutableButton = new System.Windows.Forms.Button();
            this.tbEncAacPlus = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.tbOggEnc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.tbLame = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.tbNeroAacEnc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tsmuxerPath = new System.Windows.Forms.TextBox();
            this.tsmuxerPathLabel = new System.Windows.Forms.Label();
            this.selectTSMuxerExecutableButton = new System.Windows.Forms.Button();
            this.aviMuxGUIPath = new System.Windows.Forms.TextBox();
            this.mp4boxPath = new System.Windows.Forms.TextBox();
            this.avimuxguiPathLabel = new System.Windows.Forms.Label();
            this.selectMkvmergeExecutableButton = new System.Windows.Forms.Button();
            this.mkvmergePathLabel = new System.Windows.Forms.Label();
            this.selectAviMuxGUIExecutableButton = new System.Windows.Forms.Button();
            this.mp4boxPathLabel = new System.Windows.Forms.Label();
            this.mkvmergePath = new System.Windows.Forms.TextBox();
            this.selectMP4boxExecutableButton = new System.Windows.Forms.Button();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.eac3toPath = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.selectEAC3toExecutable = new System.Windows.Forms.Button();
            this.dgmpgIndexPath = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.selectDGMIndexExecutable = new System.Windows.Forms.Button();
            this.dgvc1IndexPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.selectDGVIndexExecutable = new System.Windows.Forms.Button();
            this.dgavcIndexPath = new System.Windows.Forms.TextBox();
            this.DGAIndex = new System.Windows.Forms.Label();
            this.selectDGAIndexExecutable = new System.Windows.Forms.Button();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.besplit = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.avisynthPluginsLabel = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.selectAvisynthPluginsDir = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.meguiUpdateCache = new System.Windows.Forms.TextBox();
            this.avisynthPluginsDir = new System.Windows.Forms.TextBox();
            this.dgIndexPath = new System.Windows.Forms.TextBox();
            this.dgIndexLabel = new System.Windows.Forms.Label();
            this.selectDGIndexExecutable = new System.Windows.Forms.Button();
            this.audioExtLabel = new System.Windows.Forms.Label();
            this.videoExtLabel = new System.Windows.Forms.Label();
            this.autoEncodeDefaultsButton = new System.Windows.Forms.Button();
            this.tbFlac = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.selectFlacExecutableButton = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            this.otherGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audiosamplesperupdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableFPSError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableAspectError)).BeginInit();
            this.vobGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forceFilmPercentage)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbDefaultOutput.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbVideoPreview.SuspendLayout();
            this.autoUpdateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxServersToTry)).BeginInit();
            this.outputExtensions.SuspendLayout();
            this.autoModeGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPasses)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.command);
            groupBox1.Controls.Add(this.runCommand);
            groupBox1.Controls.Add(this.shutdown);
            groupBox1.Controls.Add(this.donothing);
            groupBox1.Location = new System.Drawing.Point(4, 187);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(217, 117);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "After encoding";
            // 
            // command
            // 
            this.command.Enabled = false;
            this.command.Location = new System.Drawing.Point(10, 89);
            this.command.Name = "command";
            this.command.Size = new System.Drawing.Size(197, 21);
            this.command.TabIndex = 3;
            // 
            // runCommand
            // 
            this.runCommand.AutoSize = true;
            this.runCommand.Location = new System.Drawing.Point(11, 66);
            this.runCommand.Name = "runCommand";
            this.runCommand.Size = new System.Drawing.Size(96, 17);
            this.runCommand.TabIndex = 2;
            this.runCommand.Text = "Run command:";
            this.runCommand.UseVisualStyleBackColor = true;
            this.runCommand.CheckedChanged += new System.EventHandler(this.runCommand_CheckedChanged);
            // 
            // shutdown
            // 
            this.shutdown.AutoSize = true;
            this.shutdown.Location = new System.Drawing.Point(11, 43);
            this.shutdown.Name = "shutdown";
            this.shutdown.Size = new System.Drawing.Size(73, 17);
            this.shutdown.TabIndex = 1;
            this.shutdown.Text = "Shutdown";
            this.shutdown.UseVisualStyleBackColor = true;
            // 
            // donothing
            // 
            this.donothing.AutoSize = true;
            this.donothing.Checked = true;
            this.donothing.Location = new System.Drawing.Point(11, 20);
            this.donothing.Name = "donothing";
            this.donothing.Size = new System.Drawing.Size(77, 17);
            this.donothing.TabIndex = 0;
            this.donothing.TabStop = true;
            this.donothing.Text = "Do nothing";
            this.donothing.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(359, 418);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(48, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(430, 418);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            // 
            // otherGroupBox
            // 
            this.otherGroupBox.Controls.Add(this.forcerawavcuse);
            this.otherGroupBox.Controls.Add(this.audiosamplesperupdate);
            this.otherGroupBox.Controls.Add(this.label6);
            this.otherGroupBox.Controls.Add(this.acceptableFPSError);
            this.otherGroupBox.Controls.Add(this.label15);
            this.otherGroupBox.Controls.Add(this.acceptableAspectError);
            this.otherGroupBox.Controls.Add(this.acceptableAspectErrorLabel);
            this.otherGroupBox.Controls.Add(this.resetDialogs);
            this.otherGroupBox.Controls.Add(this.configSourceDetector);
            this.otherGroupBox.Controls.Add(this.chkboxUseAdvancedTooltips);
            this.otherGroupBox.Controls.Add(this.openProgressWindow);
            this.otherGroupBox.Controls.Add(this.deleteIntermediateFiles);
            this.otherGroupBox.Controls.Add(this.deleteAbortedOutput);
            this.otherGroupBox.Controls.Add(this.deleteCompletedJobs);
            this.otherGroupBox.Controls.Add(this.openScript);
            this.otherGroupBox.Controls.Add(this.autostartQueue);
            this.otherGroupBox.Controls.Add(this.priority);
            this.otherGroupBox.Controls.Add(this.priorityLabel);
            this.otherGroupBox.Location = new System.Drawing.Point(2, 85);
            this.otherGroupBox.Name = "otherGroupBox";
            this.otherGroupBox.Size = new System.Drawing.Size(467, 231);
            this.otherGroupBox.TabIndex = 1;
            this.otherGroupBox.TabStop = false;
            this.otherGroupBox.Text = "Other";
            // 
            // forcerawavcuse
            // 
            this.forcerawavcuse.Location = new System.Drawing.Point(13, 174);
            this.forcerawavcuse.Name = "forcerawavcuse";
            this.forcerawavcuse.Size = new System.Drawing.Size(258, 17);
            this.forcerawavcuse.TabIndex = 18;
            this.forcerawavcuse.Text = "Force Video File Extension for QT compatibility";
            this.forcerawavcuse.UseVisualStyleBackColor = true;
            // 
            // audiosamplesperupdate
            // 
            this.audiosamplesperupdate.Location = new System.Drawing.Point(357, 17);
            this.audiosamplesperupdate.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.audiosamplesperupdate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.audiosamplesperupdate.Name = "audiosamplesperupdate";
            this.audiosamplesperupdate.Size = new System.Drawing.Size(95, 21);
            this.audiosamplesperupdate.TabIndex = 3;
            this.audiosamplesperupdate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(249, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 42);
            this.label6.TabIndex = 2;
            this.label6.Text = "Samples between audio progress updates";
            // 
            // acceptableFPSError
            // 
            this.acceptableFPSError.DecimalPlaces = 3;
            this.acceptableFPSError.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.acceptableFPSError.Location = new System.Drawing.Point(150, 70);
            this.acceptableFPSError.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.acceptableFPSError.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.acceptableFPSError.Name = "acceptableFPSError";
            this.acceptableFPSError.Size = new System.Drawing.Size(79, 21);
            this.acceptableFPSError.TabIndex = 7;
            this.acceptableFPSError.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(9, 70);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(130, 32);
            this.label15.TabIndex = 6;
            this.label15.Text = "Acceptable FPS rounding error (bitrate calculator)";
            // 
            // acceptableAspectError
            // 
            this.acceptableAspectError.Location = new System.Drawing.Point(175, 43);
            this.acceptableAspectError.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.acceptableAspectError.Name = "acceptableAspectError";
            this.acceptableAspectError.Size = new System.Drawing.Size(54, 21);
            this.acceptableAspectError.TabIndex = 5;
            this.acceptableAspectError.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // acceptableAspectErrorLabel
            // 
            this.acceptableAspectErrorLabel.AutoSize = true;
            this.acceptableAspectErrorLabel.Location = new System.Drawing.Point(8, 45);
            this.acceptableAspectErrorLabel.Name = "acceptableAspectErrorLabel";
            this.acceptableAspectErrorLabel.Size = new System.Drawing.Size(145, 13);
            this.acceptableAspectErrorLabel.TabIndex = 4;
            this.acceptableAspectErrorLabel.Text = "Acceptable Aspect Error (%)";
            // 
            // resetDialogs
            // 
            this.resetDialogs.Location = new System.Drawing.Point(13, 197);
            this.resetDialogs.Name = "resetDialogs";
            this.resetDialogs.Size = new System.Drawing.Size(149, 23);
            this.resetDialogs.TabIndex = 16;
            this.resetDialogs.Text = "Reset All Dialogs";
            this.resetDialogs.UseVisualStyleBackColor = true;
            this.resetDialogs.Click += new System.EventHandler(this.resetDialogs_Click);
            // 
            // configSourceDetector
            // 
            this.configSourceDetector.Location = new System.Drawing.Point(300, 197);
            this.configSourceDetector.Name = "configSourceDetector";
            this.configSourceDetector.Size = new System.Drawing.Size(154, 23);
            this.configSourceDetector.TabIndex = 17;
            this.configSourceDetector.Text = "Configure Source Detector";
            this.configSourceDetector.UseVisualStyleBackColor = true;
            this.configSourceDetector.Click += new System.EventHandler(this.configSourceDetector_Click);
            // 
            // chkboxUseAdvancedTooltips
            // 
            this.chkboxUseAdvancedTooltips.Location = new System.Drawing.Point(13, 105);
            this.chkboxUseAdvancedTooltips.Name = "chkboxUseAdvancedTooltips";
            this.chkboxUseAdvancedTooltips.Size = new System.Drawing.Size(152, 17);
            this.chkboxUseAdvancedTooltips.TabIndex = 8;
            this.chkboxUseAdvancedTooltips.Text = "Use Advanced ToolTips";
            // 
            // openProgressWindow
            // 
            this.openProgressWindow.Checked = true;
            this.openProgressWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openProgressWindow.Location = new System.Drawing.Point(300, 174);
            this.openProgressWindow.Name = "openProgressWindow";
            this.openProgressWindow.Size = new System.Drawing.Size(144, 17);
            this.openProgressWindow.TabIndex = 15;
            this.openProgressWindow.Text = "Open Progress Window";
            // 
            // deleteIntermediateFiles
            // 
            this.deleteIntermediateFiles.Location = new System.Drawing.Point(300, 151);
            this.deleteIntermediateFiles.Name = "deleteIntermediateFiles";
            this.deleteIntermediateFiles.Size = new System.Drawing.Size(152, 17);
            this.deleteIntermediateFiles.TabIndex = 13;
            this.deleteIntermediateFiles.Text = "Delete intermediate files";
            // 
            // deleteAbortedOutput
            // 
            this.deleteAbortedOutput.Location = new System.Drawing.Point(13, 151);
            this.deleteAbortedOutput.Name = "deleteAbortedOutput";
            this.deleteAbortedOutput.Size = new System.Drawing.Size(184, 17);
            this.deleteAbortedOutput.TabIndex = 12;
            this.deleteAbortedOutput.Text = "Delete Output of aborted jobs";
            // 
            // deleteCompletedJobs
            // 
            this.deleteCompletedJobs.Location = new System.Drawing.Point(300, 128);
            this.deleteCompletedJobs.Name = "deleteCompletedJobs";
            this.deleteCompletedJobs.Size = new System.Drawing.Size(144, 17);
            this.deleteCompletedJobs.TabIndex = 11;
            this.deleteCompletedJobs.Text = "Delete completed Jobs";
            // 
            // openScript
            // 
            this.openScript.Location = new System.Drawing.Point(13, 128);
            this.openScript.Name = "openScript";
            this.openScript.Size = new System.Drawing.Size(248, 17);
            this.openScript.TabIndex = 10;
            this.openScript.Text = "Open Preview after AviSynth script selection";
            // 
            // autostartQueue
            // 
            this.autostartQueue.Location = new System.Drawing.Point(300, 105);
            this.autostartQueue.Name = "autostartQueue";
            this.autostartQueue.Size = new System.Drawing.Size(112, 17);
            this.autostartQueue.TabIndex = 9;
            this.autostartQueue.Text = "Autostart Queue";
            // 
            // priority
            // 
            this.priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priority.Items.AddRange(new object[] {
            "Low",
            "Below Normal",
            "Normal",
            "Above Normal",
            "High"});
            this.priority.Location = new System.Drawing.Point(150, 16);
            this.priority.Name = "priority";
            this.priority.Size = new System.Drawing.Size(80, 21);
            this.priority.TabIndex = 1;
            // 
            // priorityLabel
            // 
            this.priorityLabel.Location = new System.Drawing.Point(8, 19);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(88, 13);
            this.priorityLabel.TabIndex = 0;
            this.priorityLabel.Text = "Default Priority";
            this.priorityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vobGroupBox
            // 
            this.vobGroupBox.Controls.Add(this.defaultLanguage2);
            this.vobGroupBox.Controls.Add(this.defaultAudioTrack2Label);
            this.vobGroupBox.Controls.Add(this.defaultLanguage1);
            this.vobGroupBox.Controls.Add(this.defaultAudioTrack1Label);
            this.vobGroupBox.Controls.Add(this.percentLabel);
            this.vobGroupBox.Controls.Add(this.forceFilmPercentage);
            this.vobGroupBox.Controls.Add(this.autoForceFilm);
            this.vobGroupBox.Location = new System.Drawing.Point(2, 2);
            this.vobGroupBox.Name = "vobGroupBox";
            this.vobGroupBox.Size = new System.Drawing.Size(467, 77);
            this.vobGroupBox.TabIndex = 0;
            this.vobGroupBox.TabStop = false;
            this.vobGroupBox.Text = "DGIndex";
            // 
            // defaultLanguage2
            // 
            this.defaultLanguage2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultLanguage2.Location = new System.Drawing.Point(126, 47);
            this.defaultLanguage2.Name = "defaultLanguage2";
            this.defaultLanguage2.Size = new System.Drawing.Size(104, 21);
            this.defaultLanguage2.TabIndex = 6;
            // 
            // defaultAudioTrack2Label
            // 
            this.defaultAudioTrack2Label.Location = new System.Drawing.Point(8, 50);
            this.defaultAudioTrack2Label.Name = "defaultAudioTrack2Label";
            this.defaultAudioTrack2Label.Size = new System.Drawing.Size(112, 13);
            this.defaultAudioTrack2Label.TabIndex = 5;
            this.defaultAudioTrack2Label.Text = "Default Audio Track 2";
            this.defaultAudioTrack2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // defaultLanguage1
            // 
            this.defaultLanguage1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultLanguage1.Location = new System.Drawing.Point(126, 20);
            this.defaultLanguage1.Name = "defaultLanguage1";
            this.defaultLanguage1.Size = new System.Drawing.Size(104, 21);
            this.defaultLanguage1.TabIndex = 1;
            // 
            // defaultAudioTrack1Label
            // 
            this.defaultAudioTrack1Label.Location = new System.Drawing.Point(8, 24);
            this.defaultAudioTrack1Label.Name = "defaultAudioTrack1Label";
            this.defaultAudioTrack1Label.Size = new System.Drawing.Size(112, 13);
            this.defaultAudioTrack1Label.TabIndex = 0;
            this.defaultAudioTrack1Label.Text = "Default Audio Track 1";
            this.defaultAudioTrack1Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // percentLabel
            // 
            this.percentLabel.Location = new System.Drawing.Point(411, 24);
            this.percentLabel.Margin = new System.Windows.Forms.Padding(3);
            this.percentLabel.Name = "percentLabel";
            this.percentLabel.Size = new System.Drawing.Size(50, 13);
            this.percentLabel.TabIndex = 4;
            this.percentLabel.Text = "Percent";
            this.percentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // forceFilmPercentage
            // 
            this.forceFilmPercentage.Location = new System.Drawing.Point(365, 20);
            this.forceFilmPercentage.Name = "forceFilmPercentage";
            this.forceFilmPercentage.Size = new System.Drawing.Size(40, 21);
            this.forceFilmPercentage.TabIndex = 3;
            this.forceFilmPercentage.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            // 
            // autoForceFilm
            // 
            this.autoForceFilm.Location = new System.Drawing.Point(239, 22);
            this.autoForceFilm.Name = "autoForceFilm";
            this.autoForceFilm.Size = new System.Drawing.Size(120, 17);
            this.autoForceFilm.TabIndex = 2;
            this.autoForceFilm.Text = "Auto Force Film at";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(483, 413);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gbDefaultOutput);
            this.tabPage1.Controls.Add(this.vobGroupBox);
            this.tabPage1.Controls.Add(this.otherGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(475, 387);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gbDefaultOutput
            // 
            this.gbDefaultOutput.Controls.Add(this.defaultOutputDir);
            this.gbDefaultOutput.Controls.Add(this.clearDefaultOutputDir);
            this.gbDefaultOutput.Location = new System.Drawing.Point(2, 322);
            this.gbDefaultOutput.Name = "gbDefaultOutput";
            this.gbDefaultOutput.Size = new System.Drawing.Size(467, 57);
            this.gbDefaultOutput.TabIndex = 8;
            this.gbDefaultOutput.TabStop = false;
            this.gbDefaultOutput.Text = "Default Output Directory";
            // 
            // defaultOutputDir
            // 
            this.defaultOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultOutputDir.Filename = "";
            this.defaultOutputDir.Filter = null;
            this.defaultOutputDir.FilterIndex = 0;
            this.defaultOutputDir.FolderMode = true;
            this.defaultOutputDir.Location = new System.Drawing.Point(6, 20);
            this.defaultOutputDir.Name = "defaultOutputDir";
            this.defaultOutputDir.ReadOnly = true;
            this.defaultOutputDir.SaveMode = false;
            this.defaultOutputDir.Size = new System.Drawing.Size(417, 26);
            this.defaultOutputDir.TabIndex = 42;
            this.defaultOutputDir.Title = null;
            // 
            // clearDefaultOutputDir
            // 
            this.clearDefaultOutputDir.Location = new System.Drawing.Point(429, 21);
            this.clearDefaultOutputDir.Name = "clearDefaultOutputDir";
            this.clearDefaultOutputDir.Size = new System.Drawing.Size(24, 23);
            this.clearDefaultOutputDir.TabIndex = 41;
            this.clearDefaultOutputDir.Text = "x";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.gbVideoPreview);
            this.tabPage3.Controls.Add(groupBox1);
            this.tabPage3.Controls.Add(this.autoUpdateGroupBox);
            this.tabPage3.Controls.Add(this.outputExtensions);
            this.tabPage3.Controls.Add(this.autoModeGroupbox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(475, 387);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Extra config";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_httpproxyport);
            this.groupBox2.Controls.Add(this.txt_httpproxypwd);
            this.groupBox2.Controls.Add(this.txt_httpproxyuid);
            this.groupBox2.Controls.Add(this.txt_httpproxyaddress);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.cbx_usehttpproxy);
            this.groupBox2.Location = new System.Drawing.Point(227, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 191);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auto Update Http Proxy:";
            // 
            // txt_httpproxyport
            // 
            this.txt_httpproxyport.Enabled = false;
            this.txt_httpproxyport.Location = new System.Drawing.Point(191, 43);
            this.txt_httpproxyport.Name = "txt_httpproxyport";
            this.txt_httpproxyport.Size = new System.Drawing.Size(43, 21);
            this.txt_httpproxyport.TabIndex = 8;
            // 
            // txt_httpproxypwd
            // 
            this.txt_httpproxypwd.Enabled = false;
            this.txt_httpproxypwd.Location = new System.Drawing.Point(55, 99);
            this.txt_httpproxypwd.Name = "txt_httpproxypwd";
            this.txt_httpproxypwd.PasswordChar = '*';
            this.txt_httpproxypwd.Size = new System.Drawing.Size(179, 21);
            this.txt_httpproxypwd.TabIndex = 7;
            // 
            // txt_httpproxyuid
            // 
            this.txt_httpproxyuid.Enabled = false;
            this.txt_httpproxyuid.Location = new System.Drawing.Point(55, 72);
            this.txt_httpproxyuid.Name = "txt_httpproxyuid";
            this.txt_httpproxyuid.Size = new System.Drawing.Size(179, 21);
            this.txt_httpproxyuid.TabIndex = 6;
            // 
            // txt_httpproxyaddress
            // 
            this.txt_httpproxyaddress.Enabled = false;
            this.txt_httpproxyaddress.Location = new System.Drawing.Point(55, 43);
            this.txt_httpproxyaddress.Name = "txt_httpproxyaddress";
            this.txt_httpproxyaddress.Size = new System.Drawing.Size(103, 21);
            this.txt_httpproxyaddress.TabIndex = 5;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 102);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(31, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Pwd:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 75);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(36, 13);
            this.label20.TabIndex = 3;
            this.label20.Text = "Login:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(164, 45);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(31, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Port:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 45);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(43, 13);
            this.label18.TabIndex = 1;
            this.label18.Text = "Server:";
            // 
            // cbx_usehttpproxy
            // 
            this.cbx_usehttpproxy.AutoSize = true;
            this.cbx_usehttpproxy.Location = new System.Drawing.Point(9, 21);
            this.cbx_usehttpproxy.Name = "cbx_usehttpproxy";
            this.cbx_usehttpproxy.Size = new System.Drawing.Size(75, 17);
            this.cbx_usehttpproxy.TabIndex = 0;
            this.cbx_usehttpproxy.Text = "Use Proxy";
            this.cbx_usehttpproxy.UseVisualStyleBackColor = true;
            this.cbx_usehttpproxy.CheckedChanged += new System.EventHandler(this.cbx_usehttpproxy_CheckedChanged);
            // 
            // gbVideoPreview
            // 
            this.gbVideoPreview.Controls.Add(this.cbAddTimePos);
            this.gbVideoPreview.Controls.Add(this.chAlwaysOnTop);
            this.gbVideoPreview.Location = new System.Drawing.Point(4, 309);
            this.gbVideoPreview.Name = "gbVideoPreview";
            this.gbVideoPreview.Size = new System.Drawing.Size(217, 69);
            this.gbVideoPreview.TabIndex = 4;
            this.gbVideoPreview.TabStop = false;
            this.gbVideoPreview.Text = "Video Preview";
            // 
            // cbAddTimePos
            // 
            this.cbAddTimePos.AutoSize = true;
            this.cbAddTimePos.Checked = true;
            this.cbAddTimePos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAddTimePos.Location = new System.Drawing.Point(8, 40);
            this.cbAddTimePos.Name = "cbAddTimePos";
            this.cbAddTimePos.Size = new System.Drawing.Size(110, 17);
            this.cbAddTimePos.TabIndex = 1;
            this.cbAddTimePos.Text = "Add Time Position";
            this.cbAddTimePos.UseVisualStyleBackColor = true;
            // 
            // chAlwaysOnTop
            // 
            this.chAlwaysOnTop.AutoSize = true;
            this.chAlwaysOnTop.Location = new System.Drawing.Point(8, 17);
            this.chAlwaysOnTop.Name = "chAlwaysOnTop";
            this.chAlwaysOnTop.Size = new System.Drawing.Size(169, 17);
            this.chAlwaysOnTop.TabIndex = 0;
            this.chAlwaysOnTop.Text = "Set the Form \"Always on Top\"";
            this.chAlwaysOnTop.UseVisualStyleBackColor = true;
            // 
            // autoUpdateGroupBox
            // 
            this.autoUpdateGroupBox.Controls.Add(this.backupfiles);
            this.autoUpdateGroupBox.Controls.Add(this.label14);
            this.autoUpdateGroupBox.Controls.Add(this.maxServersToTry);
            this.autoUpdateGroupBox.Controls.Add(this.configureServersButton);
            this.autoUpdateGroupBox.Controls.Add(this.useAutoUpdateCheckbox);
            this.autoUpdateGroupBox.Location = new System.Drawing.Point(227, 82);
            this.autoUpdateGroupBox.Name = "autoUpdateGroupBox";
            this.autoUpdateGroupBox.Size = new System.Drawing.Size(240, 99);
            this.autoUpdateGroupBox.TabIndex = 3;
            this.autoUpdateGroupBox.TabStop = false;
            this.autoUpdateGroupBox.Text = "Auto Update";
            // 
            // backupfiles
            // 
            this.backupfiles.AutoSize = true;
            this.backupfiles.Checked = true;
            this.backupfiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backupfiles.Location = new System.Drawing.Point(9, 76);
            this.backupfiles.Name = "backupfiles";
            this.backupfiles.Size = new System.Drawing.Size(187, 17);
            this.backupfiles.TabIndex = 4;
            this.backupfiles.Text = "Always backup files when needed";
            this.backupfiles.UseVisualStyleBackColor = true;
            this.backupfiles.CheckedChanged += new System.EventHandler(this.backupfiles_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 53);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(152, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Max number of servers to try:";
            // 
            // maxServersToTry
            // 
            this.maxServersToTry.Location = new System.Drawing.Point(190, 51);
            this.maxServersToTry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxServersToTry.Name = "maxServersToTry";
            this.maxServersToTry.Size = new System.Drawing.Size(44, 21);
            this.maxServersToTry.TabIndex = 3;
            this.maxServersToTry.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // configureServersButton
            // 
            this.configureServersButton.AutoSize = true;
            this.configureServersButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.configureServersButton.Location = new System.Drawing.Point(119, 18);
            this.configureServersButton.Name = "configureServersButton";
            this.configureServersButton.Size = new System.Drawing.Size(115, 23);
            this.configureServersButton.TabIndex = 1;
            this.configureServersButton.Text = "Configure servers...";
            this.configureServersButton.UseVisualStyleBackColor = true;
            this.configureServersButton.Click += new System.EventHandler(this.configureServersButton_Click);
            // 
            // useAutoUpdateCheckbox
            // 
            this.useAutoUpdateCheckbox.AutoSize = true;
            this.useAutoUpdateCheckbox.Location = new System.Drawing.Point(9, 22);
            this.useAutoUpdateCheckbox.Name = "useAutoUpdateCheckbox";
            this.useAutoUpdateCheckbox.Size = new System.Drawing.Size(105, 17);
            this.useAutoUpdateCheckbox.TabIndex = 0;
            this.useAutoUpdateCheckbox.Text = "Use AutoUpdate";
            this.useAutoUpdateCheckbox.UseVisualStyleBackColor = true;
            // 
            // outputExtensions
            // 
            this.outputExtensions.Controls.Add(this.videoExtension);
            this.outputExtensions.Controls.Add(this.label11);
            this.outputExtensions.Controls.Add(this.label12);
            this.outputExtensions.Controls.Add(this.audioExtension);
            this.outputExtensions.Location = new System.Drawing.Point(3, 82);
            this.outputExtensions.Name = "outputExtensions";
            this.outputExtensions.Size = new System.Drawing.Size(218, 99);
            this.outputExtensions.TabIndex = 1;
            this.outputExtensions.TabStop = false;
            this.outputExtensions.Text = "Optional output extensions";
            // 
            // videoExtension
            // 
            this.videoExtension.Location = new System.Drawing.Point(11, 20);
            this.videoExtension.Name = "videoExtension";
            this.videoExtension.Size = new System.Drawing.Size(120, 21);
            this.videoExtension.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(137, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Audio";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(137, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Video";
            // 
            // audioExtension
            // 
            this.audioExtension.Location = new System.Drawing.Point(11, 48);
            this.audioExtension.Name = "audioExtension";
            this.audioExtension.Size = new System.Drawing.Size(120, 21);
            this.audioExtension.TabIndex = 2;
            // 
            // autoModeGroupbox
            // 
            this.autoModeGroupbox.Controls.Add(this.configAutoEncodeDefaults);
            this.autoModeGroupbox.Controls.Add(this.keep2ndPassLogFile);
            this.autoModeGroupbox.Controls.Add(this.keep2ndPassOutput);
            this.autoModeGroupbox.Controls.Add(this.label13);
            this.autoModeGroupbox.Controls.Add(this.nbPasses);
            this.autoModeGroupbox.Location = new System.Drawing.Point(4, 3);
            this.autoModeGroupbox.Name = "autoModeGroupbox";
            this.autoModeGroupbox.Size = new System.Drawing.Size(463, 73);
            this.autoModeGroupbox.TabIndex = 0;
            this.autoModeGroupbox.TabStop = false;
            this.autoModeGroupbox.Text = "Automated Encoding";
            // 
            // configAutoEncodeDefaults
            // 
            this.configAutoEncodeDefaults.AutoSize = true;
            this.configAutoEncodeDefaults.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.configAutoEncodeDefaults.Location = new System.Drawing.Point(11, 43);
            this.configAutoEncodeDefaults.Name = "configAutoEncodeDefaults";
            this.configAutoEncodeDefaults.Size = new System.Drawing.Size(179, 23);
            this.configAutoEncodeDefaults.TabIndex = 5;
            this.configAutoEncodeDefaults.Text = "Configure AutoEncode defaults...";
            this.configAutoEncodeDefaults.UseVisualStyleBackColor = true;
            this.configAutoEncodeDefaults.Click += new System.EventHandler(this.autoEncodeDefaultsButton_Click);
            // 
            // keep2ndPassLogFile
            // 
            this.keep2ndPassLogFile.AutoSize = true;
            this.keep2ndPassLogFile.Checked = true;
            this.keep2ndPassLogFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keep2ndPassLogFile.Location = new System.Drawing.Point(232, 22);
            this.keep2ndPassLogFile.Name = "keep2ndPassLogFile";
            this.keep2ndPassLogFile.Size = new System.Drawing.Size(176, 17);
            this.keep2ndPassLogFile.TabIndex = 4;
            this.keep2ndPassLogFile.Text = "Overwrite Stats File in 3rd Pass";
            this.keep2ndPassLogFile.UseVisualStyleBackColor = true;
            // 
            // keep2ndPassOutput
            // 
            this.keep2ndPassOutput.AutoSize = true;
            this.keep2ndPassOutput.Checked = true;
            this.keep2ndPassOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keep2ndPassOutput.Location = new System.Drawing.Point(232, 47);
            this.keep2ndPassOutput.Name = "keep2ndPassOutput";
            this.keep2ndPassOutput.Size = new System.Drawing.Size(207, 17);
            this.keep2ndPassOutput.TabIndex = 3;
            this.keep2ndPassOutput.Text = "Keep 2nd pass Output in 3 pass mode";
            this.keep2ndPassOutput.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(11, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Number of passes";
            // 
            // nbPasses
            // 
            this.nbPasses.Location = new System.Drawing.Point(117, 20);
            this.nbPasses.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nbPasses.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nbPasses.Name = "nbPasses";
            this.nbPasses.Size = new System.Drawing.Size(40, 21);
            this.nbPasses.TabIndex = 1;
            this.nbPasses.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(475, 387);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Program Paths";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Location = new System.Drawing.Point(8, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(459, 310);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.DivXAVCPath);
            this.tabPage4.Controls.Add(this.selectDivXAVCExecButton);
            this.tabPage4.Controls.Add(this.xvidEncrawLabel);
            this.tabPage4.Controls.Add(this.selectXvidEncrawButton);
            this.tabPage4.Controls.Add(this.xvidEncrawPath);
            this.tabPage4.Controls.Add(this.selectMencoderExecutableButton);
            this.tabPage4.Controls.Add(this.mencoderPath);
            this.tabPage4.Controls.Add(this.mencoderPathLabel);
            this.tabPage4.Controls.Add(this.x264ExePathLabel);
            this.tabPage4.Controls.Add(this.x264ExePath);
            this.tabPage4.Controls.Add(this.selectX264ExecutableButton);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(451, 284);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Video";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "DivXAVC";
            // 
            // DivXAVCPath
            // 
            this.DivXAVCPath.Location = new System.Drawing.Point(81, 97);
            this.DivXAVCPath.Name = "DivXAVCPath";
            this.DivXAVCPath.ReadOnly = true;
            this.DivXAVCPath.Size = new System.Drawing.Size(330, 21);
            this.DivXAVCPath.TabIndex = 13;
            this.DivXAVCPath.Text = "DivXAVC.exe";
            // 
            // selectDivXAVCExecButton
            // 
            this.selectDivXAVCExecButton.Location = new System.Drawing.Point(417, 96);
            this.selectDivXAVCExecButton.Name = "selectDivXAVCExecButton";
            this.selectDivXAVCExecButton.Size = new System.Drawing.Size(24, 23);
            this.selectDivXAVCExecButton.TabIndex = 14;
            this.selectDivXAVCExecButton.Text = "...";
            this.selectDivXAVCExecButton.Click += new System.EventHandler(this.selectDivXAVCExecButton_Click);
            // 
            // xvidEncrawLabel
            // 
            this.xvidEncrawLabel.Location = new System.Drawing.Point(6, 72);
            this.xvidEncrawLabel.Name = "xvidEncrawLabel";
            this.xvidEncrawLabel.Size = new System.Drawing.Size(69, 16);
            this.xvidEncrawLabel.TabIndex = 9;
            this.xvidEncrawLabel.Text = "xvid_encraw";
            // 
            // selectXvidEncrawButton
            // 
            this.selectXvidEncrawButton.Location = new System.Drawing.Point(417, 69);
            this.selectXvidEncrawButton.Name = "selectXvidEncrawButton";
            this.selectXvidEncrawButton.Size = new System.Drawing.Size(24, 23);
            this.selectXvidEncrawButton.TabIndex = 11;
            this.selectXvidEncrawButton.Text = "...";
            this.selectXvidEncrawButton.Click += new System.EventHandler(this.selectXvidEncrawButton_Click);
            // 
            // xvidEncrawPath
            // 
            this.xvidEncrawPath.Location = new System.Drawing.Point(81, 70);
            this.xvidEncrawPath.Name = "xvidEncrawPath";
            this.xvidEncrawPath.ReadOnly = true;
            this.xvidEncrawPath.Size = new System.Drawing.Size(330, 21);
            this.xvidEncrawPath.TabIndex = 10;
            this.xvidEncrawPath.Text = "xvid_encraw.exe";
            // 
            // selectMencoderExecutableButton
            // 
            this.selectMencoderExecutableButton.Location = new System.Drawing.Point(417, 15);
            this.selectMencoderExecutableButton.Name = "selectMencoderExecutableButton";
            this.selectMencoderExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMencoderExecutableButton.TabIndex = 2;
            this.selectMencoderExecutableButton.Text = "...";
            this.selectMencoderExecutableButton.Click += new System.EventHandler(this.selectMencoderExecutableButton_Click);
            // 
            // mencoderPath
            // 
            this.mencoderPath.Location = new System.Drawing.Point(81, 16);
            this.mencoderPath.Name = "mencoderPath";
            this.mencoderPath.ReadOnly = true;
            this.mencoderPath.Size = new System.Drawing.Size(330, 21);
            this.mencoderPath.TabIndex = 1;
            this.mencoderPath.Text = "mencoder.exe";
            // 
            // mencoderPathLabel
            // 
            this.mencoderPathLabel.Location = new System.Drawing.Point(6, 16);
            this.mencoderPathLabel.Name = "mencoderPathLabel";
            this.mencoderPathLabel.Size = new System.Drawing.Size(57, 18);
            this.mencoderPathLabel.TabIndex = 0;
            this.mencoderPathLabel.Text = "mencoder";
            // 
            // x264ExePathLabel
            // 
            this.x264ExePathLabel.Location = new System.Drawing.Point(6, 46);
            this.x264ExePathLabel.Name = "x264ExePathLabel";
            this.x264ExePathLabel.Size = new System.Drawing.Size(38, 16);
            this.x264ExePathLabel.TabIndex = 6;
            this.x264ExePathLabel.Text = "x264";
            // 
            // x264ExePath
            // 
            this.x264ExePath.Location = new System.Drawing.Point(81, 43);
            this.x264ExePath.Name = "x264ExePath";
            this.x264ExePath.ReadOnly = true;
            this.x264ExePath.Size = new System.Drawing.Size(330, 21);
            this.x264ExePath.TabIndex = 7;
            this.x264ExePath.Text = "x264.exe";
            // 
            // selectX264ExecutableButton
            // 
            this.selectX264ExecutableButton.Location = new System.Drawing.Point(417, 42);
            this.selectX264ExecutableButton.Name = "selectX264ExecutableButton";
            this.selectX264ExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectX264ExecutableButton.TabIndex = 8;
            this.selectX264ExecutableButton.Text = "...";
            this.selectX264ExecutableButton.Click += new System.EventHandler(this.selectX264ExecutableButton_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tbFlac);
            this.tabPage5.Controls.Add(this.label4);
            this.tabPage5.Controls.Add(this.selectFlacExecutableButton);
            this.tabPage5.Controls.Add(this.tbAften);
            this.tabPage5.Controls.Add(this.lbAften);
            this.tabPage5.Controls.Add(this.selectAftenExecutableButton);
            this.tabPage5.Controls.Add(this.tbEncAacPlus);
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.button6);
            this.tabPage5.Controls.Add(this.tbOggEnc);
            this.tabPage5.Controls.Add(this.label7);
            this.tabPage5.Controls.Add(this.button4);
            this.tabPage5.Controls.Add(this.tbLame);
            this.tabPage5.Controls.Add(this.label3);
            this.tabPage5.Controls.Add(this.button3);
            this.tabPage5.Controls.Add(this.tbNeroAacEnc);
            this.tabPage5.Controls.Add(this.label2);
            this.tabPage5.Controls.Add(this.button2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(451, 284);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Audio";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tbAften
            // 
            this.tbAften.Location = new System.Drawing.Point(91, 87);
            this.tbAften.Name = "tbAften";
            this.tbAften.ReadOnly = true;
            this.tbAften.Size = new System.Drawing.Size(322, 21);
            this.tbAften.TabIndex = 26;
            this.tbAften.Text = "aften.exe";
            // 
            // lbAften
            // 
            this.lbAften.Location = new System.Drawing.Point(5, 91);
            this.lbAften.Name = "lbAften";
            this.lbAften.Size = new System.Drawing.Size(80, 13);
            this.lbAften.TabIndex = 25;
            this.lbAften.Text = "aften";
            // 
            // selectAftenExecutableButton
            // 
            this.selectAftenExecutableButton.Location = new System.Drawing.Point(419, 86);
            this.selectAftenExecutableButton.Name = "selectAftenExecutableButton";
            this.selectAftenExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectAftenExecutableButton.TabIndex = 27;
            this.selectAftenExecutableButton.Text = "...";
            this.selectAftenExecutableButton.Click += new System.EventHandler(this.selectAftenExecutableButton_Click);
            // 
            // tbEncAacPlus
            // 
            this.tbEncAacPlus.Location = new System.Drawing.Point(91, 60);
            this.tbEncAacPlus.Name = "tbEncAacPlus";
            this.tbEncAacPlus.ReadOnly = true;
            this.tbEncAacPlus.Size = new System.Drawing.Size(322, 21);
            this.tbEncAacPlus.TabIndex = 13;
            this.tbEncAacPlus.Text = "enc_aacplus.exe";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(5, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "enc_aacPlus";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(419, 59);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(24, 23);
            this.button6.TabIndex = 14;
            this.button6.Text = "...";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // tbOggEnc
            // 
            this.tbOggEnc.Location = new System.Drawing.Point(91, 114);
            this.tbOggEnc.Name = "tbOggEnc";
            this.tbOggEnc.ReadOnly = true;
            this.tbOggEnc.Size = new System.Drawing.Size(322, 21);
            this.tbOggEnc.TabIndex = 22;
            this.tbOggEnc.Text = "oggenc2.exe";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(5, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "oggenc";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(419, 113);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 23);
            this.button4.TabIndex = 23;
            this.button4.Text = "...";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // tbLame
            // 
            this.tbLame.Location = new System.Drawing.Point(91, 33);
            this.tbLame.Name = "tbLame";
            this.tbLame.ReadOnly = true;
            this.tbLame.Size = new System.Drawing.Size(322, 21);
            this.tbLame.TabIndex = 7;
            this.tbLame.Text = "lame.exe";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "lame";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(419, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "...";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbNeroAacEnc
            // 
            this.tbNeroAacEnc.Location = new System.Drawing.Point(91, 6);
            this.tbNeroAacEnc.Name = "tbNeroAacEnc";
            this.tbNeroAacEnc.ReadOnly = true;
            this.tbNeroAacEnc.Size = new System.Drawing.Size(322, 21);
            this.tbNeroAacEnc.TabIndex = 4;
            this.tbNeroAacEnc.Text = "neroAacEnc.exe";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "neroAacEnc";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(419, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "...";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.tsmuxerPath);
            this.tabPage6.Controls.Add(this.tsmuxerPathLabel);
            this.tabPage6.Controls.Add(this.selectTSMuxerExecutableButton);
            this.tabPage6.Controls.Add(this.aviMuxGUIPath);
            this.tabPage6.Controls.Add(this.mp4boxPath);
            this.tabPage6.Controls.Add(this.avimuxguiPathLabel);
            this.tabPage6.Controls.Add(this.selectMkvmergeExecutableButton);
            this.tabPage6.Controls.Add(this.mkvmergePathLabel);
            this.tabPage6.Controls.Add(this.selectAviMuxGUIExecutableButton);
            this.tabPage6.Controls.Add(this.mp4boxPathLabel);
            this.tabPage6.Controls.Add(this.mkvmergePath);
            this.tabPage6.Controls.Add(this.selectMP4boxExecutableButton);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(451, 284);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Muxer";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tsmuxerPath
            // 
            this.tsmuxerPath.Location = new System.Drawing.Point(89, 97);
            this.tsmuxerPath.Name = "tsmuxerPath";
            this.tsmuxerPath.ReadOnly = true;
            this.tsmuxerPath.Size = new System.Drawing.Size(322, 21);
            this.tsmuxerPath.TabIndex = 10;
            this.tsmuxerPath.Text = "tsmuxer.exe";
            // 
            // tsmuxerPathLabel
            // 
            this.tsmuxerPathLabel.Location = new System.Drawing.Point(6, 100);
            this.tsmuxerPathLabel.Name = "tsmuxerPathLabel";
            this.tsmuxerPathLabel.Size = new System.Drawing.Size(57, 17);
            this.tsmuxerPathLabel.TabIndex = 9;
            this.tsmuxerPathLabel.Text = "tsmuxer";
            // 
            // selectTSMuxerExecutableButton
            // 
            this.selectTSMuxerExecutableButton.Location = new System.Drawing.Point(417, 96);
            this.selectTSMuxerExecutableButton.Name = "selectTSMuxerExecutableButton";
            this.selectTSMuxerExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectTSMuxerExecutableButton.TabIndex = 11;
            this.selectTSMuxerExecutableButton.Text = "...";
            this.selectTSMuxerExecutableButton.Click += new System.EventHandler(this.selectTSMuxerExecutableButton_Click);
            // 
            // aviMuxGUIPath
            // 
            this.aviMuxGUIPath.Location = new System.Drawing.Point(89, 70);
            this.aviMuxGUIPath.Name = "aviMuxGUIPath";
            this.aviMuxGUIPath.ReadOnly = true;
            this.aviMuxGUIPath.Size = new System.Drawing.Size(322, 21);
            this.aviMuxGUIPath.TabIndex = 7;
            this.aviMuxGUIPath.Text = "avimux_gui.exe";
            // 
            // mp4boxPath
            // 
            this.mp4boxPath.Location = new System.Drawing.Point(89, 16);
            this.mp4boxPath.Name = "mp4boxPath";
            this.mp4boxPath.ReadOnly = true;
            this.mp4boxPath.Size = new System.Drawing.Size(322, 21);
            this.mp4boxPath.TabIndex = 1;
            this.mp4boxPath.Text = "mp4box.exe";
            // 
            // avimuxguiPathLabel
            // 
            this.avimuxguiPathLabel.Location = new System.Drawing.Point(6, 72);
            this.avimuxguiPathLabel.Name = "avimuxguiPathLabel";
            this.avimuxguiPathLabel.Size = new System.Drawing.Size(70, 17);
            this.avimuxguiPathLabel.TabIndex = 6;
            this.avimuxguiPathLabel.Text = "avimux_gui";
            // 
            // selectMkvmergeExecutableButton
            // 
            this.selectMkvmergeExecutableButton.Location = new System.Drawing.Point(417, 42);
            this.selectMkvmergeExecutableButton.Name = "selectMkvmergeExecutableButton";
            this.selectMkvmergeExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMkvmergeExecutableButton.TabIndex = 5;
            this.selectMkvmergeExecutableButton.Text = "...";
            this.selectMkvmergeExecutableButton.Click += new System.EventHandler(this.selectMkvmergeExecutableButton_Click);
            // 
            // mkvmergePathLabel
            // 
            this.mkvmergePathLabel.Location = new System.Drawing.Point(6, 46);
            this.mkvmergePathLabel.Name = "mkvmergePathLabel";
            this.mkvmergePathLabel.Size = new System.Drawing.Size(57, 17);
            this.mkvmergePathLabel.TabIndex = 3;
            this.mkvmergePathLabel.Text = "mkvmerge";
            // 
            // selectAviMuxGUIExecutableButton
            // 
            this.selectAviMuxGUIExecutableButton.Location = new System.Drawing.Point(417, 69);
            this.selectAviMuxGUIExecutableButton.Name = "selectAviMuxGUIExecutableButton";
            this.selectAviMuxGUIExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectAviMuxGUIExecutableButton.TabIndex = 8;
            this.selectAviMuxGUIExecutableButton.Text = "...";
            this.selectAviMuxGUIExecutableButton.Click += new System.EventHandler(this.selectAviMuxGUIExecutableButton_Click);
            // 
            // mp4boxPathLabel
            // 
            this.mp4boxPathLabel.Location = new System.Drawing.Point(6, 19);
            this.mp4boxPathLabel.Name = "mp4boxPathLabel";
            this.mp4boxPathLabel.Size = new System.Drawing.Size(57, 17);
            this.mp4boxPathLabel.TabIndex = 0;
            this.mp4boxPathLabel.Text = "mp4Box";
            // 
            // mkvmergePath
            // 
            this.mkvmergePath.Location = new System.Drawing.Point(89, 43);
            this.mkvmergePath.Name = "mkvmergePath";
            this.mkvmergePath.ReadOnly = true;
            this.mkvmergePath.Size = new System.Drawing.Size(322, 21);
            this.mkvmergePath.TabIndex = 4;
            this.mkvmergePath.Text = "mkvmerge.exe";
            // 
            // selectMP4boxExecutableButton
            // 
            this.selectMP4boxExecutableButton.Location = new System.Drawing.Point(417, 15);
            this.selectMP4boxExecutableButton.Name = "selectMP4boxExecutableButton";
            this.selectMP4boxExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMP4boxExecutableButton.TabIndex = 2;
            this.selectMP4boxExecutableButton.Text = "...";
            this.selectMP4boxExecutableButton.Click += new System.EventHandler(this.selectMP4boxExecutableButton_Click);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.eac3toPath);
            this.tabPage7.Controls.Add(this.label23);
            this.tabPage7.Controls.Add(this.selectEAC3toExecutable);
            this.tabPage7.Controls.Add(this.dgmpgIndexPath);
            this.tabPage7.Controls.Add(this.label22);
            this.tabPage7.Controls.Add(this.selectDGMIndexExecutable);
            this.tabPage7.Controls.Add(this.dgvc1IndexPath);
            this.tabPage7.Controls.Add(this.label5);
            this.tabPage7.Controls.Add(this.selectDGVIndexExecutable);
            this.tabPage7.Controls.Add(this.dgavcIndexPath);
            this.tabPage7.Controls.Add(this.DGAIndex);
            this.tabPage7.Controls.Add(this.selectDGAIndexExecutable);
            this.tabPage7.Controls.Add(this.textBox8);
            this.tabPage7.Controls.Add(this.besplit);
            this.tabPage7.Controls.Add(this.button8);
            this.tabPage7.Controls.Add(this.label17);
            this.tabPage7.Controls.Add(this.avisynthPluginsLabel);
            this.tabPage7.Controls.Add(this.button9);
            this.tabPage7.Controls.Add(this.button10);
            this.tabPage7.Controls.Add(this.selectAvisynthPluginsDir);
            this.tabPage7.Controls.Add(this.textBox9);
            this.tabPage7.Controls.Add(this.label16);
            this.tabPage7.Controls.Add(this.meguiUpdateCache);
            this.tabPage7.Controls.Add(this.avisynthPluginsDir);
            this.tabPage7.Controls.Add(this.dgIndexPath);
            this.tabPage7.Controls.Add(this.dgIndexLabel);
            this.tabPage7.Controls.Add(this.selectDGIndexExecutable);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(451, 284);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "Others";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // eac3toPath
            // 
            this.eac3toPath.Location = new System.Drawing.Point(96, 244);
            this.eac3toPath.Name = "eac3toPath";
            this.eac3toPath.ReadOnly = true;
            this.eac3toPath.Size = new System.Drawing.Size(315, 21);
            this.eac3toPath.TabIndex = 37;
            this.eac3toPath.Text = "eac3to.exe";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(6, 247);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(80, 13);
            this.label23.TabIndex = 36;
            this.label23.Text = "EAC3to";
            // 
            // selectEAC3toExecutable
            // 
            this.selectEAC3toExecutable.Location = new System.Drawing.Point(417, 242);
            this.selectEAC3toExecutable.Name = "selectEAC3toExecutable";
            this.selectEAC3toExecutable.Size = new System.Drawing.Size(24, 23);
            this.selectEAC3toExecutable.TabIndex = 38;
            this.selectEAC3toExecutable.Text = "...";
            this.selectEAC3toExecutable.Click += new System.EventHandler(this.selectEAC3toExecutable_Click);
            // 
            // dgmpgIndexPath
            // 
            this.dgmpgIndexPath.Location = new System.Drawing.Point(96, 214);
            this.dgmpgIndexPath.Name = "dgmpgIndexPath";
            this.dgmpgIndexPath.ReadOnly = true;
            this.dgmpgIndexPath.Size = new System.Drawing.Size(315, 21);
            this.dgmpgIndexPath.TabIndex = 34;
            this.dgmpgIndexPath.Text = "dgmpgindex.exe";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(4, 217);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(94, 19);
            this.label22.TabIndex = 33;
            this.label22.Text = "DGMPGIndex(NV)";
            // 
            // selectDGMIndexExecutable
            // 
            this.selectDGMIndexExecutable.Location = new System.Drawing.Point(417, 213);
            this.selectDGMIndexExecutable.Name = "selectDGMIndexExecutable";
            this.selectDGMIndexExecutable.Size = new System.Drawing.Size(24, 23);
            this.selectDGMIndexExecutable.TabIndex = 35;
            this.selectDGMIndexExecutable.Text = "...";
            this.selectDGMIndexExecutable.Click += new System.EventHandler(this.selectDGMIndexExecutable_Click);
            // 
            // dgvc1IndexPath
            // 
            this.dgvc1IndexPath.Location = new System.Drawing.Point(96, 183);
            this.dgvc1IndexPath.Name = "dgvc1IndexPath";
            this.dgvc1IndexPath.ReadOnly = true;
            this.dgvc1IndexPath.Size = new System.Drawing.Size(315, 21);
            this.dgvc1IndexPath.TabIndex = 31;
            this.dgvc1IndexPath.Text = "dgvc1index.exe";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 19);
            this.label5.TabIndex = 30;
            this.label5.Text = "DGVC1Index(NV)";
            // 
            // selectDGVIndexExecutable
            // 
            this.selectDGVIndexExecutable.Location = new System.Drawing.Point(417, 182);
            this.selectDGVIndexExecutable.Name = "selectDGVIndexExecutable";
            this.selectDGVIndexExecutable.Size = new System.Drawing.Size(24, 23);
            this.selectDGVIndexExecutable.TabIndex = 32;
            this.selectDGVIndexExecutable.Text = "...";
            this.selectDGVIndexExecutable.Click += new System.EventHandler(this.selectDGVIndexExecutable_Click);
            // 
            // dgavcIndexPath
            // 
            this.dgavcIndexPath.Location = new System.Drawing.Point(96, 153);
            this.dgavcIndexPath.Name = "dgavcIndexPath";
            this.dgavcIndexPath.ReadOnly = true;
            this.dgavcIndexPath.Size = new System.Drawing.Size(315, 21);
            this.dgavcIndexPath.TabIndex = 28;
            this.dgavcIndexPath.Text = "dgavcindex.exe";
            // 
            // DGAIndex
            // 
            this.DGAIndex.Location = new System.Drawing.Point(4, 156);
            this.DGAIndex.Name = "DGAIndex";
            this.DGAIndex.Size = new System.Drawing.Size(94, 19);
            this.DGAIndex.TabIndex = 27;
            this.DGAIndex.Text = "DGAVCIndex(NV)";
            // 
            // selectDGAIndexExecutable
            // 
            this.selectDGAIndexExecutable.Location = new System.Drawing.Point(417, 152);
            this.selectDGAIndexExecutable.Name = "selectDGAIndexExecutable";
            this.selectDGAIndexExecutable.Size = new System.Drawing.Size(24, 23);
            this.selectDGAIndexExecutable.TabIndex = 29;
            this.selectDGAIndexExecutable.Text = "...";
            this.selectDGAIndexExecutable.Click += new System.EventHandler(this.selectDGAIndexExecutable_Click);
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(96, 126);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(315, 21);
            this.textBox8.TabIndex = 22;
            this.textBox8.Text = "besplit.exe";
            // 
            // besplit
            // 
            this.besplit.Location = new System.Drawing.Point(6, 129);
            this.besplit.Name = "besplit";
            this.besplit.Size = new System.Drawing.Size(80, 13);
            this.besplit.TabIndex = 21;
            this.besplit.Text = "BeSplit";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(417, 124);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(24, 23);
            this.button8.TabIndex = 23;
            this.button8.Text = "...";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 101);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "Update cache";
            // 
            // avisynthPluginsLabel
            // 
            this.avisynthPluginsLabel.AutoSize = true;
            this.avisynthPluginsLabel.Location = new System.Drawing.Point(6, 74);
            this.avisynthPluginsLabel.Name = "avisynthPluginsLabel";
            this.avisynthPluginsLabel.Size = new System.Drawing.Size(84, 13);
            this.avisynthPluginsLabel.TabIndex = 6;
            this.avisynthPluginsLabel.Text = "avisynth plugins";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(417, 42);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(24, 23);
            this.button9.TabIndex = 5;
            this.button9.Text = "...";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(417, 96);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(24, 23);
            this.button10.TabIndex = 8;
            this.button10.Text = "...";
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // selectAvisynthPluginsDir
            // 
            this.selectAvisynthPluginsDir.Location = new System.Drawing.Point(417, 69);
            this.selectAvisynthPluginsDir.Name = "selectAvisynthPluginsDir";
            this.selectAvisynthPluginsDir.Size = new System.Drawing.Size(24, 23);
            this.selectAvisynthPluginsDir.TabIndex = 8;
            this.selectAvisynthPluginsDir.Text = "...";
            this.selectAvisynthPluginsDir.Click += new System.EventHandler(this.selectAvisynthDir_Click);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(96, 43);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(315, 21);
            this.textBox9.TabIndex = 4;
            this.textBox9.Text = "avisynth plugins Directory";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 47);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(45, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "yadif.dll";
            // 
            // meguiUpdateCache
            // 
            this.meguiUpdateCache.Location = new System.Drawing.Point(96, 97);
            this.meguiUpdateCache.Name = "meguiUpdateCache";
            this.meguiUpdateCache.ReadOnly = true;
            this.meguiUpdateCache.Size = new System.Drawing.Size(315, 21);
            this.meguiUpdateCache.TabIndex = 7;
            this.meguiUpdateCache.Text = "MeGUI update cache";
            // 
            // avisynthPluginsDir
            // 
            this.avisynthPluginsDir.Location = new System.Drawing.Point(96, 70);
            this.avisynthPluginsDir.Name = "avisynthPluginsDir";
            this.avisynthPluginsDir.ReadOnly = true;
            this.avisynthPluginsDir.Size = new System.Drawing.Size(315, 21);
            this.avisynthPluginsDir.TabIndex = 7;
            this.avisynthPluginsDir.Text = "avisynth plugins Directory";
            // 
            // dgIndexPath
            // 
            this.dgIndexPath.Location = new System.Drawing.Point(96, 16);
            this.dgIndexPath.Name = "dgIndexPath";
            this.dgIndexPath.ReadOnly = true;
            this.dgIndexPath.Size = new System.Drawing.Size(315, 21);
            this.dgIndexPath.TabIndex = 1;
            this.dgIndexPath.Text = "dgindex.exe";
            // 
            // dgIndexLabel
            // 
            this.dgIndexLabel.Location = new System.Drawing.Point(6, 19);
            this.dgIndexLabel.Name = "dgIndexLabel";
            this.dgIndexLabel.Size = new System.Drawing.Size(60, 15);
            this.dgIndexLabel.TabIndex = 0;
            this.dgIndexLabel.Text = "DGIndex";
            // 
            // selectDGIndexExecutable
            // 
            this.selectDGIndexExecutable.Location = new System.Drawing.Point(417, 15);
            this.selectDGIndexExecutable.Name = "selectDGIndexExecutable";
            this.selectDGIndexExecutable.Size = new System.Drawing.Size(24, 23);
            this.selectDGIndexExecutable.TabIndex = 2;
            this.selectDGIndexExecutable.Text = "...";
            this.selectDGIndexExecutable.Click += new System.EventHandler(this.selectDGIndexExecutable_Click);
            // 
            // audioExtLabel
            // 
            this.audioExtLabel.AutoSize = true;
            this.audioExtLabel.Location = new System.Drawing.Point(137, 51);
            this.audioExtLabel.Name = "audioExtLabel";
            this.audioExtLabel.Size = new System.Drawing.Size(34, 13);
            this.audioExtLabel.TabIndex = 24;
            this.audioExtLabel.Text = "Audio";
            // 
            // videoExtLabel
            // 
            this.videoExtLabel.AutoSize = true;
            this.videoExtLabel.Location = new System.Drawing.Point(137, 24);
            this.videoExtLabel.Name = "videoExtLabel";
            this.videoExtLabel.Size = new System.Drawing.Size(34, 13);
            this.videoExtLabel.TabIndex = 23;
            this.videoExtLabel.Text = "Video";
            // 
            // autoEncodeDefaultsButton
            // 
            this.autoEncodeDefaultsButton.Location = new System.Drawing.Point(11, 51);
            this.autoEncodeDefaultsButton.Name = "autoEncodeDefaultsButton";
            this.autoEncodeDefaultsButton.Size = new System.Drawing.Size(114, 23);
            this.autoEncodeDefaultsButton.TabIndex = 4;
            this.autoEncodeDefaultsButton.Text = "Configure Defaults";
            this.autoEncodeDefaultsButton.UseVisualStyleBackColor = true;
            // 
            // tbFlac
            // 
            this.tbFlac.Location = new System.Drawing.Point(91, 141);
            this.tbFlac.Name = "tbFlac";
            this.tbFlac.ReadOnly = true;
            this.tbFlac.Size = new System.Drawing.Size(322, 21);
            this.tbFlac.TabIndex = 29;
            this.tbFlac.Text = "flac.exe";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "flac";
            // 
            // selectFlacExecutableButton
            // 
            this.selectFlacExecutableButton.Location = new System.Drawing.Point(419, 140);
            this.selectFlacExecutableButton.Name = "selectFlacExecutableButton";
            this.selectFlacExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectFlacExecutableButton.TabIndex = 30;
            this.selectFlacExecutableButton.Text = "...";
            this.selectFlacExecutableButton.Click += new System.EventHandler(this.selectFlacExecutableButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(483, 446);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::MeGUI.Properties.Settings.Default, "SettingsFormSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = global::MeGUI.Properties.Settings.Default.SettingsFormSize;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.otherGroupBox.ResumeLayout(false);
            this.otherGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audiosamplesperupdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableFPSError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableAspectError)).EndInit();
            this.vobGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.forceFilmPercentage)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gbDefaultOutput.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbVideoPreview.ResumeLayout(false);
            this.gbVideoPreview.PerformLayout();
            this.autoUpdateGroupBox.ResumeLayout(false);
            this.autoUpdateGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxServersToTry)).EndInit();
            this.outputExtensions.ResumeLayout(false);
            this.outputExtensions.PerformLayout();
            this.autoModeGroupbox.ResumeLayout(false);
            this.autoModeGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPasses)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.ResumeLayout(false);

		}
        #endregion
        #region button handlers
        private bool selectExe(string exe)
        {
            openExecutableDialog.Filter = exe + " executable|" + exe +"*.exe|Any executable|*.exe";
            openExecutableDialog.DefaultExt = "exe";
            openExecutableDialog.FileName = exe + ".exe";
            openExecutableDialog.Title = "Select " + exe + " executable";
            return openExecutableDialog.ShowDialog() == DialogResult.OK;
        }

        private void selectMencoderExecutableButton_Click(object sender, System.EventArgs e)
		{
			if (selectExe("mencoder"))
			{
				mencoderPath.Text = openExecutableDialog.FileName;
			}
		}
        private void selectAvisynthDir_Click(object sender, EventArgs e)
        {
            openFolderDialog.RootFolder = Environment.SpecialFolder.Desktop;
            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                avisynthPluginsDir.Text = openFolderDialog.SelectedPath;
                MeGUISettings.AvisynthPluginsPath = avisynthPluginsDir.Text;
            }
        }

		private void selectMP4boxExecutableButton_Click(object sender, System.EventArgs e)
		{
			if (selectExe("mp4box"))
			{
				mp4boxPath.Text = openExecutableDialog.FileName;
			}
		}

		private void selectDGIndexExecutable_Click(object sender, System.EventArgs e)
		{
			if (selectExe("DGIndex"))
			{
				dgIndexPath.Text = openExecutableDialog.FileName;
			}
            // modify PATH so that n00bs don't complain because they forgot to put dgdecode.dll in the MeGUI dir
            string pathEnv = Environment.GetEnvironmentVariable("PATH");
            pathEnv = Path.GetDirectoryName(dgIndexPath.Text) + ";" + pathEnv;
            Environment.SetEnvironmentVariable("PATH", pathEnv); // Only available in .NET 2.0 !
		}
		private void selectMkvmergeExecutableButton_Click(object sender, System.EventArgs e)
		{
            if (selectExe("mkvmerge"))
			{
				mkvmergePath.Text = openExecutableDialog.FileName;
			}
		}
        private void configSourceDetector_Click(object sender, EventArgs e)
        {
            SourceDetectorConfigWindow sdcWindow = new SourceDetectorConfigWindow();
            sdcWindow.Settings = sdSettings;
            if (sdcWindow.ShowDialog() == DialogResult.OK)
                sdSettings = sdcWindow.Settings;
        }
        private void selectXvidEncrawButton_Click(object sender, EventArgs e)
        {
            if (selectExe("xvid_encraw"))
            {
                xvidEncrawPath.Text = openExecutableDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectExe("neroAacEnc"))
            {
                tbNeroAacEnc.Text = openExecutableDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectExe("lame"))
            {
                tbLame.Text = openExecutableDialog.FileName;
            }
        }
		private void selectX264ExecutableButton_Click(object sender, System.EventArgs e)
		{
			if (selectExe("x264"))
			{
				x264ExePath.Text = openExecutableDialog.FileName;
			}
		}

        private void selectAviMuxGUIExecutableButton_Click(object sender, EventArgs e)
        {
            if (selectExe("avimux_gui"))
            {
                aviMuxGUIPath.Text = openExecutableDialog.FileName;
            }
        }
   
        private void resetDialogs_Click(object sender, EventArgs e)
        {
            internalSettings.DialogSettings = new DialogSettings();
            MessageBox.Show(this, "Successfully reset all dialogs", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectExe("oggenc2"))
            {
                tbOggEnc.Text = openExecutableDialog.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (selectExe("enc_aacPlus"))
            {
                tbEncAacPlus.Text = openExecutableDialog.FileName;
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (selectExe("besplit"))
            {
                textBox8.Text = openExecutableDialog.FileName;
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            openFolderDialog.RootFolder = Environment.SpecialFolder.Desktop;
            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                meguiUpdateCache.Text = openFolderDialog.SelectedPath;
                MeGUISettings.MeGUIUpdateCache = meguiUpdateCache.Text;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "yadif library|yadif.dll|Any library|*.dll";
            d.DefaultExt = "dll";
            d.FileName = "yadif.dll";
            d.Title = "Select yadif library";
            if (d.ShowDialog() == DialogResult.OK)
                textBox9.Text = d.FileName;
        }

        private void selectAftenExecutableButton_Click(object sender, EventArgs e)
        {
            if (selectExe("aften"))
            {
                tbAften.Text = openExecutableDialog.FileName;
            }
        }

        private void selectDGAIndexExecutable_Click(object sender, EventArgs e)
        {
            if ((selectExe("DGAVCIndex")) || (selectExe("DGAVCIndexNV")))
            {
                dgavcIndexPath.Text = openExecutableDialog.FileName;
            }
        }

        private void selectDGVIndexExecutable_Click(object sender, EventArgs e)
        {
            if (selectExe("DGVC1IndexNV"))
            {
                dgvc1IndexPath.Text = openExecutableDialog.FileName;
            }
        }

        private void selectDGMIndexExecutable_Click(object sender, EventArgs e)
        {
            if (selectExe("DGMPGIndexNV"))
            {
                dgmpgIndexPath.Text = openExecutableDialog.FileName;
            }
        }

        private void selectEAC3toExecutable_Click(object sender, EventArgs e)
        {
            if (selectExe("eac3to"))
            {
                eac3toPath.Text = openExecutableDialog.FileName;
            }
        }

        private void selectTSMuxerExecutableButton_Click(object sender, EventArgs e)
        {
            if (selectExe("tsmuxer"))
            {
                tsmuxerPath.Text = openExecutableDialog.FileName;
            }
        }

        private void selectDivXAVCExecButton_Click(object sender, EventArgs e)
        {
            if (selectExe("DivX264"))
            {
                DivXAVCPath.Text = openExecutableDialog.FileName;
            }
        }

        private void selectFlacExecutableButton_Click(object sender, EventArgs e)
        {
            if (selectExe("flac"))
            {
                tbFlac.Text = openExecutableDialog.FileName;
            }
        }

        private void runCommand_CheckedChanged(object sender, EventArgs e)
        {
            command.Enabled = runCommand.Checked;
        }
        /// <summary>
        /// launches the autoencode default settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoEncodeDefaultsButton_Click(object sender, EventArgs e)
        {
            using (AutoEncodeDefaults aed = new AutoEncodeDefaults())
            {
                aed.Settings = this.autoEncodeDefaults;
                DialogResult dr = aed.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    this.autoEncodeDefaults = aed.Settings;
                }
            }
        }

        /// <summary>
        /// Launches configuration of auto-update servers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configureServersButton_Click(object sender, EventArgs e)
        {
            using (MeGUI.core.gui.AutoUpdateServerConfigWindow w = new MeGUI.core.gui.AutoUpdateServerConfigWindow())
            {
                w.ServerList = internalSettings.AutoUpdateServerLists;
                w.ServerListIndex = internalSettings.AutoUpdateServerSubList;
                if (w.ShowDialog() == DialogResult.OK)
                {
                    internalSettings.AutoUpdateServerLists = w.ServerList;
                    internalSettings.AutoUpdateServerSubList = w.ServerListIndex;
                }
            }
        }

        private void cbx_usehttpproxy_CheckedChanged(object sender, EventArgs e)
        {
            txt_httpproxyaddress.Enabled = cbx_usehttpproxy.Checked;
            txt_httpproxyport.Enabled = cbx_usehttpproxy.Checked;
            txt_httpproxyuid.Enabled = cbx_usehttpproxy.Checked;
            txt_httpproxypwd.Enabled = cbx_usehttpproxy.Checked;
        }

        private void clearDefaultOutputDir_Click(object sender, EventArgs e)
        {
            defaultOutputDir.Filename = "";
        }

		#endregion
		#region properties
		public MeGUISettings Settings
		{
			get 
			{
                MeGUISettings settings = internalSettings;
                settings.YadifPath = textBox9.Text;
                settings.AudioSamplesPerUpdate = (ulong)audiosamplesperupdate.Value;
                settings.AcceptableFPSError = acceptableFPSError.Value; 
                settings.MaxServersToTry = (int)maxServersToTry.Value;
                settings.AutoUpdate = useAutoUpdateCheckbox.Checked;
                settings.AcceptableAspectErrorPercent = (int)acceptableAspectError.Value;
				settings.MencoderPath = mencoderPath.Text;
                settings.SourceDetectorSettings = sdSettings;
                settings.NeroAacEncPath = tbNeroAacEnc.Text;
                settings.LamePath = tbLame.Text;
				settings.Mp4boxPath = mp4boxPath.Text;
                settings.AviMuxGUIPath = aviMuxGUIPath.Text;
				settings.MkvmergePath = mkvmergePath.Text;
                settings.XviDEncrawPath = xvidEncrawPath.Text;
                settings.VideoExtension = videoExtension.Text;
                settings.AudioExtension = audioExtension.Text;
                settings.UseAdvancedTooltips = chkboxUseAdvancedTooltips.Checked;
                settings.BeSplitPath = textBox8.Text;
				settings.X264Path = x264ExePath.Text;
				settings.DgIndexPath = dgIndexPath.Text;
				settings.DefaultLanguage1 = defaultLanguage1.Text;
				settings.DefaultLanguage2 = defaultLanguage2.Text;
				settings.AutoForceFilm = autoForceFilm.Checked;
				settings.ForceFilmThreshold = forceFilmPercentage.Value;
				settings.DefaultPriority = (ProcessPriority)priority.SelectedIndex;
				settings.AutoStartQueue = this.autostartQueue.Checked;
                if (donothing.Checked) settings.AfterEncoding = AfterEncoding.DoNothing;
                else if (shutdown.Checked) settings.AfterEncoding = AfterEncoding.Shutdown;
                else
                {
                    settings.AfterEncoding = AfterEncoding.RunCommand;
                    settings.AfterEncodingCommand = command.Text;
                }
                settings.AutoOpenScript = openScript.Checked;
				settings.DeleteCompletedJobs = deleteCompletedJobs.Checked;
				settings.DeleteIntermediateFiles = deleteIntermediateFiles.Checked;
				settings.DeleteAbortedOutput = deleteAbortedOutput.Checked;
				settings.OpenProgressWindow = openProgressWindow.Checked;
				settings.Keep2of3passOutput = keep2ndPassOutput.Checked;
				settings.OverwriteStats = keep2ndPassLogFile.Checked;
				settings.NbPasses = (int)nbPasses.Value;
                settings.OggEnc2Path = tbOggEnc.Text;
                settings.EncAacPlusPath = tbEncAacPlus.Text;
                settings.AftenPath = tbAften.Text;
                settings.AedSettings = this.autoEncodeDefaults;
                settings.AlwaysOnTop = chAlwaysOnTop.Checked;
                settings.UseHttpProxy = cbx_usehttpproxy.Checked;
                settings.HttpProxyAddress = txt_httpproxyaddress.Text;
                settings.HttpProxyPort = txt_httpproxyport.Text;
                settings.HttpProxyUid = txt_httpproxyuid.Text;
                settings.HttpProxyPwd = txt_httpproxypwd.Text;
                settings.DefaultOutputDir = defaultOutputDir.Filename;
                settings.AddTimePosition = cbAddTimePos.Checked;
                settings.DgavcIndexPath = dgavcIndexPath.Text;
                settings.Dgvc1IndexPath = dgvc1IndexPath.Text;
                settings.DgmpgIndexPath = dgmpgIndexPath.Text;
                settings.EAC3toPath = eac3toPath.Text;
                settings.TSMuxerPath = tsmuxerPath.Text;
                settings.AlwaysBackUpFiles = backupfiles.Checked;
                settings.ForceRawAVCExtension = forcerawavcuse.Checked;
                settings.DivXAVCPath = DivXAVCPath.Text;
                settings.FlacPath = tbFlac.Text;
				return settings;
			}
			set
			{
                internalSettings = value;
                MeGUISettings settings = value;
                textBox9.Text = settings.YadifPath;
                audiosamplesperupdate.Value = settings.AudioSamplesPerUpdate;
                acceptableFPSError.Value = settings.AcceptableFPSError;
                maxServersToTry.Value = settings.MaxServersToTry;
                useAutoUpdateCheckbox.Checked = settings.AutoUpdate;
                acceptableAspectError.Value = (decimal)settings.AcceptableAspectErrorPercent;
				mencoderPath.Text = settings.MencoderPath;      
                tbNeroAacEnc.Text = settings.NeroAacEncPath;
                tbLame.Text = settings.LamePath;
                textBox8.Text = settings.BeSplitPath;
                sdSettings = settings.SourceDetectorSettings;
                chkboxUseAdvancedTooltips.Checked = settings.UseAdvancedTooltips;
				mp4boxPath.Text = settings.Mp4boxPath;
				mkvmergePath.Text = settings.MkvmergePath;
                xvidEncrawPath.Text = settings.XviDEncrawPath;
                aviMuxGUIPath.Text = settings.AviMuxGUIPath;
                videoExtension.Text = settings.VideoExtension;
                audioExtension.Text = settings.AudioExtension;
				x264ExePath.Text = settings.X264Path;
				dgIndexPath.Text = settings.DgIndexPath;
				int index = this.defaultLanguage1.Items.IndexOf(settings.DefaultLanguage1);
				if (index != -1)
					defaultLanguage1.SelectedIndex = index;
				index = defaultLanguage2.Items.IndexOf(settings.DefaultLanguage2);
				if (index != -1)
					defaultLanguage2.SelectedIndex = index;
				autoForceFilm.Checked = settings.AutoForceFilm;
				forceFilmPercentage.Value = settings.ForceFilmThreshold;
				priority.SelectedIndex = (int)settings.DefaultPriority;
				autostartQueue.Checked = settings.AutoStartQueue;
                donothing.Checked = settings.AfterEncoding == AfterEncoding.DoNothing;
                shutdown.Checked = settings.AfterEncoding == AfterEncoding.Shutdown;
                runCommand.Checked = settings.AfterEncoding == AfterEncoding.RunCommand;
                command.Text = settings.AfterEncodingCommand;
				deleteCompletedJobs.Checked = settings.DeleteCompletedJobs;
                openScript.Checked = settings.AutoOpenScript;
				deleteIntermediateFiles.Checked = settings.DeleteIntermediateFiles;
				deleteAbortedOutput.Checked = settings.DeleteAbortedOutput;
				openProgressWindow.Checked = settings.OpenProgressWindow;
				keep2ndPassOutput.Checked = settings.Keep2of3passOutput;
				keep2ndPassLogFile.Checked = settings.OverwriteStats;
				nbPasses.Value = (decimal)settings.NbPasses;
                tbOggEnc.Text = settings.OggEnc2Path;
                tbEncAacPlus.Text = settings.EncAacPlusPath;
                tbAften.Text = settings.AftenPath;
                this.autoEncodeDefaults = settings.AedSettings;
                chAlwaysOnTop.Checked = settings.AlwaysOnTop;
                cbx_usehttpproxy.Checked = settings.UseHttpProxy;
                txt_httpproxyaddress.Text = settings.HttpProxyAddress;
                txt_httpproxyport.Text = settings.HttpProxyPort;
                txt_httpproxyuid.Text = settings.HttpProxyUid;
                txt_httpproxypwd.Text = settings.HttpProxyPwd;
                defaultOutputDir.Filename = settings.DefaultOutputDir;
                cbAddTimePos.Checked = settings.AddTimePosition;
                dgavcIndexPath.Text = settings.DgavcIndexPath;
                dgvc1IndexPath.Text = settings.Dgvc1IndexPath;
                dgmpgIndexPath.Text = settings.DgmpgIndexPath;
                eac3toPath.Text = settings.EAC3toPath;
                tsmuxerPath.Text = settings.TSMuxerPath;
                backupfiles.Checked = settings.AlwaysBackUpFiles;
                forcerawavcuse.Checked = settings.ForceRawAVCExtension;
                DivXAVCPath.Text = settings.DivXAVCPath;
                tbFlac.Text = settings.FlacPath;
			}
		}
		#endregion

        private void backupfiles_CheckedChanged(object sender, EventArgs e)
        {
            if (!backupfiles.Checked)
            {
                string meguiToolsFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\')) + "\\tools\\";
                string meguiAvisynthFolder = MeGUISettings.AvisynthPluginsPath + "\\";
                if (Directory.Exists(meguiToolsFolder))
                {
                    try
                    {  // remove all backup files found
                        Array.ForEach(Directory.GetFiles(meguiToolsFolder, "*.backup", SearchOption.AllDirectories),
                          delegate(string path) { File.Delete(path); });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (Directory.Exists(meguiAvisynthFolder))
                {
                    try
                    {  // remove all backup files found
                        Array.ForEach(Directory.GetFiles(meguiAvisynthFolder, "*.backup", SearchOption.AllDirectories),
                          delegate(string path) { File.Delete(path); });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
	}
}
