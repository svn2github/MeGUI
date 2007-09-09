using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

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
/*        private string[][] autoUpdateServers;
        private int autoUpdateIndex;
        private DialogSettings dialogSettings;*/
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
        private System.Windows.Forms.CheckBox autosetNbThreads;
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
        private CheckBox checkBox3;
        private Label label13;
        private NumericUpDown numericUpDown1;
        private CheckBox checkBox4;
        private Label audioExtLabel;
        private Label videoExtLabel;
        private Button autoEncodeDefaultsButton;
        private CheckBox keep2ndPassLogfile;
        private Label nbPassesLabel;
        private NumericUpDown nbPasses;
        private CheckBox keep2ndPassOutput;
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
        private Button selectAvc2AviExecutableButton;
        private Button selectMencoderExecutableButton;
        private TextBox avc2aviPath;
        private TextBox mencoderPath;
        private Label label5;
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
        private Label label4;
        private TextBox textBox8;
        private Label besplit;
        private TextBox textBox7;
        private Button button8;
        private Label label10;
        private Button button7;
        private CheckBox checkBox1;
        private TextBox textBox6;
        private Label label9;
        private Button button6;
        private TextBox textBox5;
        private Label label8;
        private Button button5;
        private TextBox textBox4;
        private Label label7;
        private Button button4;
        private TextBox textBox3;
        private Label label3;
        private Button button3;
        private TextBox textBox2;
        private Label label2;
        private Button button2;
        private TextBox textBox1;
        private Label label1;
        private Button button1;
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
            this.command = new System.Windows.Forms.TextBox();
            this.runCommand = new System.Windows.Forms.RadioButton();
            this.shutdown = new System.Windows.Forms.RadioButton();
            this.donothing = new System.Windows.Forms.RadioButton();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.otherGroupBox = new System.Windows.Forms.GroupBox();
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
            this.autosetNbThreads = new System.Windows.Forms.CheckBox();
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
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.autoUpdateGroupBox = new System.Windows.Forms.GroupBox();
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
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.xvidEncrawLabel = new System.Windows.Forms.Label();
            this.selectXvidEncrawButton = new System.Windows.Forms.Button();
            this.xvidEncrawPath = new System.Windows.Forms.TextBox();
            this.selectAvc2AviExecutableButton = new System.Windows.Forms.Button();
            this.selectMencoderExecutableButton = new System.Windows.Forms.Button();
            this.avc2aviPath = new System.Windows.Forms.TextBox();
            this.mencoderPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mencoderPathLabel = new System.Windows.Forms.Label();
            this.x264ExePathLabel = new System.Windows.Forms.Label();
            this.x264ExePath = new System.Windows.Forms.TextBox();
            this.selectX264ExecutableButton = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.button9 = new System.Windows.Forms.Button();
            this.selectAvisynthPluginsDir = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.avisynthPluginsDir = new System.Windows.Forms.TextBox();
            this.dgIndexPath = new System.Windows.Forms.TextBox();
            this.dgIndexLabel = new System.Windows.Forms.Label();
            this.selectDGIndexExecutable = new System.Windows.Forms.Button();
            this.audioExtLabel = new System.Windows.Forms.Label();
            this.videoExtLabel = new System.Windows.Forms.Label();
            this.autoEncodeDefaultsButton = new System.Windows.Forms.Button();
            this.keep2ndPassLogfile = new System.Windows.Forms.CheckBox();
            this.nbPassesLabel = new System.Windows.Forms.Label();
            this.nbPasses = new System.Windows.Forms.NumericUpDown();
            this.keep2ndPassOutput = new System.Windows.Forms.CheckBox();
            this.avisynthPluginsLabel = new System.Windows.Forms.Label();
            this.mkvmergePathLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mp4boxPathLabel = new System.Windows.Forms.Label();
            this.mkvmergePath = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.aviMuxGUIPath = new System.Windows.Forms.TextBox();
            this.mp4boxPath = new System.Windows.Forms.TextBox();
            this.selectMkvmergeExecutableButton = new System.Windows.Forms.Button();
            this.selectAviMuxGUIExecutableButton = new System.Windows.Forms.Button();
            this.selectMP4boxExecutableButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button7 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.besplit = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
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
            this.tabPage3.SuspendLayout();
            this.autoUpdateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxServersToTry)).BeginInit();
            this.outputExtensions.SuspendLayout();
            this.autoModeGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPasses)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.command);
            groupBox1.Controls.Add(this.runCommand);
            groupBox1.Controls.Add(this.shutdown);
            groupBox1.Controls.Add(this.donothing);
            groupBox1.Location = new System.Drawing.Point(4, 171);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(217, 119);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "After encoding";
            // 
            // command
            // 
            this.command.Enabled = false;
            this.command.Location = new System.Drawing.Point(42, 89);
            this.command.Name = "command";
            this.command.Size = new System.Drawing.Size(160, 21);
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
            this.saveButton.Location = new System.Drawing.Point(355, 402);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(48, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(414, 402);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            // 
            // otherGroupBox
            // 
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
            this.otherGroupBox.Controls.Add(this.autosetNbThreads);
            this.otherGroupBox.Controls.Add(this.deleteIntermediateFiles);
            this.otherGroupBox.Controls.Add(this.deleteAbortedOutput);
            this.otherGroupBox.Controls.Add(this.deleteCompletedJobs);
            this.otherGroupBox.Controls.Add(this.openScript);
            this.otherGroupBox.Controls.Add(this.autostartQueue);
            this.otherGroupBox.Controls.Add(this.priority);
            this.otherGroupBox.Controls.Add(this.priorityLabel);
            this.otherGroupBox.Location = new System.Drawing.Point(2, 90);
            this.otherGroupBox.Name = "otherGroupBox";
            this.otherGroupBox.Size = new System.Drawing.Size(456, 253);
            this.otherGroupBox.TabIndex = 3;
            this.otherGroupBox.TabStop = false;
            this.otherGroupBox.Text = "Other";
            // 
            // audiosamplesperupdate
            // 
            this.audiosamplesperupdate.Location = new System.Drawing.Point(342, 20);
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
            this.audiosamplesperupdate.TabIndex = 32;
            this.audiosamplesperupdate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(241, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 42);
            this.label6.TabIndex = 31;
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
            this.acceptableFPSError.Location = new System.Drawing.Point(145, 72);
            this.acceptableFPSError.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.acceptableFPSError.Name = "acceptableFPSError";
            this.acceptableFPSError.Size = new System.Drawing.Size(79, 21);
            this.acceptableFPSError.TabIndex = 30;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(9, 72);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(130, 32);
            this.label15.TabIndex = 29;
            this.label15.Text = "Acceptable FPS rounding error (bitrate calculator)";
            // 
            // acceptableAspectError
            // 
            this.acceptableAspectError.Location = new System.Drawing.Point(170, 43);
            this.acceptableAspectError.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.acceptableAspectError.Name = "acceptableAspectError";
            this.acceptableAspectError.Size = new System.Drawing.Size(54, 21);
            this.acceptableAspectError.TabIndex = 28;
            this.acceptableAspectError.Value = new decimal(new int[] {
            5,
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
            this.acceptableAspectErrorLabel.TabIndex = 27;
            this.acceptableAspectErrorLabel.Text = "Acceptable Aspect Error (%)";
            // 
            // resetDialogs
            // 
            this.resetDialogs.Location = new System.Drawing.Point(17, 224);
            this.resetDialogs.Name = "resetDialogs";
            this.resetDialogs.Size = new System.Drawing.Size(149, 23);
            this.resetDialogs.TabIndex = 26;
            this.resetDialogs.Text = "Reset All Dialogs";
            this.resetDialogs.UseVisualStyleBackColor = true;
            this.resetDialogs.Click += new System.EventHandler(this.resetDialogs_Click);
            // 
            // configSourceDetector
            // 
            this.configSourceDetector.Location = new System.Drawing.Point(293, 224);
            this.configSourceDetector.Name = "configSourceDetector";
            this.configSourceDetector.Size = new System.Drawing.Size(154, 23);
            this.configSourceDetector.TabIndex = 6;
            this.configSourceDetector.Text = "Configure Source Detector";
            this.configSourceDetector.UseVisualStyleBackColor = true;
            this.configSourceDetector.Click += new System.EventHandler(this.configSourceDetector_Click);
            // 
            // chkboxUseAdvancedTooltips
            // 
            this.chkboxUseAdvancedTooltips.Location = new System.Drawing.Point(13, 122);
            this.chkboxUseAdvancedTooltips.Name = "chkboxUseAdvancedTooltips";
            this.chkboxUseAdvancedTooltips.Size = new System.Drawing.Size(152, 24);
            this.chkboxUseAdvancedTooltips.TabIndex = 23;
            this.chkboxUseAdvancedTooltips.Text = "Use Advanced ToolTips";
            // 
            // openProgressWindow
            // 
            this.openProgressWindow.Checked = true;
            this.openProgressWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openProgressWindow.Location = new System.Drawing.Point(293, 194);
            this.openProgressWindow.Name = "openProgressWindow";
            this.openProgressWindow.Size = new System.Drawing.Size(144, 24);
            this.openProgressWindow.TabIndex = 21;
            this.openProgressWindow.Text = "Open Progress Window";
            // 
            // autosetNbThreads
            // 
            this.autosetNbThreads.Location = new System.Drawing.Point(13, 194);
            this.autosetNbThreads.Name = "autosetNbThreads";
            this.autosetNbThreads.Size = new System.Drawing.Size(208, 24);
            this.autosetNbThreads.TabIndex = 20;
            this.autosetNbThreads.Text = "Automatically set number of Threads";
            // 
            // deleteIntermediateFiles
            // 
            this.deleteIntermediateFiles.Location = new System.Drawing.Point(293, 170);
            this.deleteIntermediateFiles.Name = "deleteIntermediateFiles";
            this.deleteIntermediateFiles.Size = new System.Drawing.Size(152, 24);
            this.deleteIntermediateFiles.TabIndex = 19;
            this.deleteIntermediateFiles.Text = "Delete intermediate files";
            // 
            // deleteAbortedOutput
            // 
            this.deleteAbortedOutput.Location = new System.Drawing.Point(13, 170);
            this.deleteAbortedOutput.Name = "deleteAbortedOutput";
            this.deleteAbortedOutput.Size = new System.Drawing.Size(184, 24);
            this.deleteAbortedOutput.TabIndex = 18;
            this.deleteAbortedOutput.Text = "Delete Output of aborted jobs";
            // 
            // deleteCompletedJobs
            // 
            this.deleteCompletedJobs.Location = new System.Drawing.Point(293, 146);
            this.deleteCompletedJobs.Name = "deleteCompletedJobs";
            this.deleteCompletedJobs.Size = new System.Drawing.Size(144, 24);
            this.deleteCompletedJobs.TabIndex = 17;
            this.deleteCompletedJobs.Text = "Delete completed Jobs";
            // 
            // openScript
            // 
            this.openScript.Location = new System.Drawing.Point(13, 146);
            this.openScript.Name = "openScript";
            this.openScript.Size = new System.Drawing.Size(248, 24);
            this.openScript.TabIndex = 16;
            this.openScript.Text = "Open Preview after AviSynth script selection";
            // 
            // autostartQueue
            // 
            this.autostartQueue.Location = new System.Drawing.Point(293, 122);
            this.autostartQueue.Name = "autostartQueue";
            this.autostartQueue.Size = new System.Drawing.Size(112, 24);
            this.autostartQueue.TabIndex = 14;
            this.autostartQueue.Text = "Autostart Queue";
            // 
            // priority
            // 
            this.priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priority.Items.AddRange(new object[] {
            "Low",
            "Normal",
            "High"});
            this.priority.Location = new System.Drawing.Point(144, 16);
            this.priority.Name = "priority";
            this.priority.Size = new System.Drawing.Size(80, 21);
            this.priority.TabIndex = 13;
            // 
            // priorityLabel
            // 
            this.priorityLabel.Location = new System.Drawing.Point(8, 16);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(88, 23);
            this.priorityLabel.TabIndex = 12;
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
            this.vobGroupBox.Size = new System.Drawing.Size(456, 85);
            this.vobGroupBox.TabIndex = 4;
            this.vobGroupBox.TabStop = false;
            this.vobGroupBox.Text = "DGIndex";
            // 
            // defaultLanguage2
            // 
            this.defaultLanguage2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultLanguage2.Location = new System.Drawing.Point(120, 53);
            this.defaultLanguage2.Name = "defaultLanguage2";
            this.defaultLanguage2.Size = new System.Drawing.Size(104, 21);
            this.defaultLanguage2.Sorted = true;
            this.defaultLanguage2.TabIndex = 6;
            // 
            // defaultAudioTrack2Label
            // 
            this.defaultAudioTrack2Label.Location = new System.Drawing.Point(8, 51);
            this.defaultAudioTrack2Label.Name = "defaultAudioTrack2Label";
            this.defaultAudioTrack2Label.Size = new System.Drawing.Size(112, 24);
            this.defaultAudioTrack2Label.TabIndex = 5;
            this.defaultAudioTrack2Label.Text = "Default Audio Track 2";
            this.defaultAudioTrack2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // defaultLanguage1
            // 
            this.defaultLanguage1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultLanguage1.Location = new System.Drawing.Point(120, 24);
            this.defaultLanguage1.Name = "defaultLanguage1";
            this.defaultLanguage1.Size = new System.Drawing.Size(104, 21);
            this.defaultLanguage1.Sorted = true;
            this.defaultLanguage1.TabIndex = 4;
            // 
            // defaultAudioTrack1Label
            // 
            this.defaultAudioTrack1Label.Location = new System.Drawing.Point(8, 22);
            this.defaultAudioTrack1Label.Name = "defaultAudioTrack1Label";
            this.defaultAudioTrack1Label.Size = new System.Drawing.Size(112, 24);
            this.defaultAudioTrack1Label.TabIndex = 3;
            this.defaultAudioTrack1Label.Text = "Default Audio Track 1";
            this.defaultAudioTrack1Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // percentLabel
            // 
            this.percentLabel.Location = new System.Drawing.Point(397, 21);
            this.percentLabel.Margin = new System.Windows.Forms.Padding(3);
            this.percentLabel.Name = "percentLabel";
            this.percentLabel.Size = new System.Drawing.Size(50, 24);
            this.percentLabel.TabIndex = 2;
            this.percentLabel.Text = "Percent";
            this.percentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // forceFilmPercentage
            // 
            this.forceFilmPercentage.Location = new System.Drawing.Point(349, 23);
            this.forceFilmPercentage.Name = "forceFilmPercentage";
            this.forceFilmPercentage.Size = new System.Drawing.Size(40, 21);
            this.forceFilmPercentage.TabIndex = 1;
            this.forceFilmPercentage.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            // 
            // autoForceFilm
            // 
            this.autoForceFilm.Location = new System.Drawing.Point(234, 23);
            this.autoForceFilm.Name = "autoForceFilm";
            this.autoForceFilm.Size = new System.Drawing.Size(120, 24);
            this.autoForceFilm.TabIndex = 0;
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
            this.tabControl1.Size = new System.Drawing.Size(468, 394);
            this.tabControl1.TabIndex = 28;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.vobGroupBox);
            this.tabPage1.Controls.Add(this.otherGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(460, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(groupBox1);
            this.tabPage3.Controls.Add(this.autoUpdateGroupBox);
            this.tabPage3.Controls.Add(this.outputExtensions);
            this.tabPage3.Controls.Add(this.autoModeGroupbox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(460, 368);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Extra config";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // autoUpdateGroupBox
            // 
            this.autoUpdateGroupBox.Controls.Add(this.label14);
            this.autoUpdateGroupBox.Controls.Add(this.maxServersToTry);
            this.autoUpdateGroupBox.Controls.Add(this.configureServersButton);
            this.autoUpdateGroupBox.Controls.Add(this.useAutoUpdateCheckbox);
            this.autoUpdateGroupBox.Location = new System.Drawing.Point(227, 87);
            this.autoUpdateGroupBox.Name = "autoUpdateGroupBox";
            this.autoUpdateGroupBox.Size = new System.Drawing.Size(231, 203);
            this.autoUpdateGroupBox.TabIndex = 9;
            this.autoUpdateGroupBox.TabStop = false;
            this.autoUpdateGroupBox.Text = "Auto Update";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 84);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(176, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Maximum number of servers to try:";
            // 
            // maxServersToTry
            // 
            this.maxServersToTry.Location = new System.Drawing.Point(24, 104);
            this.maxServersToTry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxServersToTry.Name = "maxServersToTry";
            this.maxServersToTry.Size = new System.Drawing.Size(120, 21);
            this.maxServersToTry.TabIndex = 4;
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
            this.configureServersButton.Location = new System.Drawing.Point(9, 46);
            this.configureServersButton.Name = "configureServersButton";
            this.configureServersButton.Size = new System.Drawing.Size(115, 23);
            this.configureServersButton.TabIndex = 3;
            this.configureServersButton.Text = "Configure servers...";
            this.configureServersButton.UseVisualStyleBackColor = true;
            this.configureServersButton.Click += new System.EventHandler(this.configureServersButton_Click);
            // 
            // useAutoUpdateCheckbox
            // 
            this.useAutoUpdateCheckbox.AutoSize = true;
            this.useAutoUpdateCheckbox.Location = new System.Drawing.Point(9, 23);
            this.useAutoUpdateCheckbox.Name = "useAutoUpdateCheckbox";
            this.useAutoUpdateCheckbox.Size = new System.Drawing.Size(105, 17);
            this.useAutoUpdateCheckbox.TabIndex = 2;
            this.useAutoUpdateCheckbox.Text = "Use AutoUpdate";
            this.useAutoUpdateCheckbox.UseVisualStyleBackColor = true;
            // 
            // outputExtensions
            // 
            this.outputExtensions.Controls.Add(this.videoExtension);
            this.outputExtensions.Controls.Add(this.label11);
            this.outputExtensions.Controls.Add(this.label12);
            this.outputExtensions.Controls.Add(this.audioExtension);
            this.outputExtensions.Location = new System.Drawing.Point(3, 87);
            this.outputExtensions.Name = "outputExtensions";
            this.outputExtensions.Size = new System.Drawing.Size(218, 78);
            this.outputExtensions.TabIndex = 7;
            this.outputExtensions.TabStop = false;
            this.outputExtensions.Text = "Optional output extensions";
            // 
            // videoExtension
            // 
            this.videoExtension.Location = new System.Drawing.Point(6, 20);
            this.videoExtension.Name = "videoExtension";
            this.videoExtension.Size = new System.Drawing.Size(120, 21);
            this.videoExtension.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(137, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Audio";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(137, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "Video";
            // 
            // audioExtension
            // 
            this.audioExtension.Location = new System.Drawing.Point(6, 48);
            this.audioExtension.Name = "audioExtension";
            this.audioExtension.Size = new System.Drawing.Size(120, 21);
            this.audioExtension.TabIndex = 22;
            // 
            // autoModeGroupbox
            // 
            this.autoModeGroupbox.Controls.Add(this.checkBox3);
            this.autoModeGroupbox.Controls.Add(this.label13);
            this.autoModeGroupbox.Controls.Add(this.numericUpDown1);
            this.autoModeGroupbox.Controls.Add(this.checkBox4);
            this.autoModeGroupbox.Location = new System.Drawing.Point(4, 3);
            this.autoModeGroupbox.Name = "autoModeGroupbox";
            this.autoModeGroupbox.Size = new System.Drawing.Size(454, 78);
            this.autoModeGroupbox.TabIndex = 8;
            this.autoModeGroupbox.TabStop = false;
            this.autoModeGroupbox.Text = "Automated Encoding";
            // 
            // checkBox3
            // 
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(232, 16);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(208, 24);
            this.checkBox3.TabIndex = 3;
            this.checkBox3.Text = "Overwrite Stats File in 3rd pass";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 23);
            this.label13.TabIndex = 2;
            this.label13.Text = "Number of passes";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(120, 22);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(40, 21);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // checkBox4
            // 
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(232, 48);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(216, 24);
            this.checkBox4.TabIndex = 0;
            this.checkBox4.Text = "Keep 2nd pass Output in 3 pass mode";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(460, 368);
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
            this.tabControl2.Size = new System.Drawing.Size(446, 296);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.xvidEncrawLabel);
            this.tabPage4.Controls.Add(this.selectXvidEncrawButton);
            this.tabPage4.Controls.Add(this.xvidEncrawPath);
            this.tabPage4.Controls.Add(this.selectAvc2AviExecutableButton);
            this.tabPage4.Controls.Add(this.selectMencoderExecutableButton);
            this.tabPage4.Controls.Add(this.avc2aviPath);
            this.tabPage4.Controls.Add(this.mencoderPath);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.mencoderPathLabel);
            this.tabPage4.Controls.Add(this.x264ExePathLabel);
            this.tabPage4.Controls.Add(this.x264ExePath);
            this.tabPage4.Controls.Add(this.selectX264ExecutableButton);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(438, 270);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Video";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // xvidEncrawLabel
            // 
            this.xvidEncrawLabel.Location = new System.Drawing.Point(2, 99);
            this.xvidEncrawLabel.Name = "xvidEncrawLabel";
            this.xvidEncrawLabel.Size = new System.Drawing.Size(84, 16);
            this.xvidEncrawLabel.TabIndex = 71;
            this.xvidEncrawLabel.Text = "xvid_encraw";
            // 
            // selectXvidEncrawButton
            // 
            this.selectXvidEncrawButton.Location = new System.Drawing.Point(408, 96);
            this.selectXvidEncrawButton.Name = "selectXvidEncrawButton";
            this.selectXvidEncrawButton.Size = new System.Drawing.Size(24, 23);
            this.selectXvidEncrawButton.TabIndex = 70;
            this.selectXvidEncrawButton.Text = "...";
            // 
            // xvidEncrawPath
            // 
            this.xvidEncrawPath.Location = new System.Drawing.Point(89, 96);
            this.xvidEncrawPath.Name = "xvidEncrawPath";
            this.xvidEncrawPath.ReadOnly = true;
            this.xvidEncrawPath.Size = new System.Drawing.Size(308, 21);
            this.xvidEncrawPath.TabIndex = 69;
            this.xvidEncrawPath.Text = "xvid_encraw.exe";
            // 
            // selectAvc2AviExecutableButton
            // 
            this.selectAvc2AviExecutableButton.Location = new System.Drawing.Point(408, 42);
            this.selectAvc2AviExecutableButton.Name = "selectAvc2AviExecutableButton";
            this.selectAvc2AviExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectAvc2AviExecutableButton.TabIndex = 57;
            this.selectAvc2AviExecutableButton.Text = "...";
            // 
            // selectMencoderExecutableButton
            // 
            this.selectMencoderExecutableButton.Location = new System.Drawing.Point(408, 16);
            this.selectMencoderExecutableButton.Name = "selectMencoderExecutableButton";
            this.selectMencoderExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMencoderExecutableButton.TabIndex = 53;
            this.selectMencoderExecutableButton.Text = "...";
            // 
            // avc2aviPath
            // 
            this.avc2aviPath.Location = new System.Drawing.Point(89, 42);
            this.avc2aviPath.Name = "avc2aviPath";
            this.avc2aviPath.ReadOnly = true;
            this.avc2aviPath.Size = new System.Drawing.Size(308, 21);
            this.avc2aviPath.TabIndex = 51;
            this.avc2aviPath.Text = "avc2avi.exe";
            // 
            // mencoderPath
            // 
            this.mencoderPath.Location = new System.Drawing.Point(89, 16);
            this.mencoderPath.Name = "mencoderPath";
            this.mencoderPath.ReadOnly = true;
            this.mencoderPath.Size = new System.Drawing.Size(308, 21);
            this.mencoderPath.TabIndex = 47;
            this.mencoderPath.Text = "mencoder.exe";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(26, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 18);
            this.label5.TabIndex = 46;
            this.label5.Text = "avc2avi";
            // 
            // mencoderPathLabel
            // 
            this.mencoderPathLabel.Location = new System.Drawing.Point(16, 19);
            this.mencoderPathLabel.Name = "mencoderPathLabel";
            this.mencoderPathLabel.Size = new System.Drawing.Size(57, 18);
            this.mencoderPathLabel.TabIndex = 42;
            this.mencoderPathLabel.Text = "mencoder";
            // 
            // x264ExePathLabel
            // 
            this.x264ExePathLabel.Location = new System.Drawing.Point(35, 72);
            this.x264ExePathLabel.Name = "x264ExePathLabel";
            this.x264ExePathLabel.Size = new System.Drawing.Size(38, 16);
            this.x264ExePathLabel.TabIndex = 58;
            this.x264ExePathLabel.Text = "x264";
            // 
            // x264ExePath
            // 
            this.x264ExePath.Location = new System.Drawing.Point(89, 69);
            this.x264ExePath.Name = "x264ExePath";
            this.x264ExePath.ReadOnly = true;
            this.x264ExePath.Size = new System.Drawing.Size(308, 21);
            this.x264ExePath.TabIndex = 59;
            this.x264ExePath.Text = "x264.exe";
            // 
            // selectX264ExecutableButton
            // 
            this.selectX264ExecutableButton.Location = new System.Drawing.Point(408, 69);
            this.selectX264ExecutableButton.Name = "selectX264ExecutableButton";
            this.selectX264ExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectX264ExecutableButton.TabIndex = 60;
            this.selectX264ExecutableButton.Text = "...";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.textBox8);
            this.tabPage5.Controls.Add(this.besplit);
            this.tabPage5.Controls.Add(this.textBox7);
            this.tabPage5.Controls.Add(this.button8);
            this.tabPage5.Controls.Add(this.label10);
            this.tabPage5.Controls.Add(this.button7);
            this.tabPage5.Controls.Add(this.checkBox1);
            this.tabPage5.Controls.Add(this.textBox6);
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.button6);
            this.tabPage5.Controls.Add(this.textBox5);
            this.tabPage5.Controls.Add(this.label8);
            this.tabPage5.Controls.Add(this.button5);
            this.tabPage5.Controls.Add(this.textBox4);
            this.tabPage5.Controls.Add(this.label7);
            this.tabPage5.Controls.Add(this.button4);
            this.tabPage5.Controls.Add(this.textBox3);
            this.tabPage5.Controls.Add(this.label3);
            this.tabPage5.Controls.Add(this.button3);
            this.tabPage5.Controls.Add(this.textBox2);
            this.tabPage5.Controls.Add(this.label2);
            this.tabPage5.Controls.Add(this.button2);
            this.tabPage5.Controls.Add(this.textBox1);
            this.tabPage5.Controls.Add(this.label1);
            this.tabPage5.Controls.Add(this.button1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(438, 270);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Audio";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.avisynthPluginsLabel);
            this.tabPage7.Controls.Add(this.button9);
            this.tabPage7.Controls.Add(this.selectAvisynthPluginsDir);
            this.tabPage7.Controls.Add(this.textBox9);
            this.tabPage7.Controls.Add(this.label16);
            this.tabPage7.Controls.Add(this.avisynthPluginsDir);
            this.tabPage7.Controls.Add(this.dgIndexPath);
            this.tabPage7.Controls.Add(this.dgIndexLabel);
            this.tabPage7.Controls.Add(this.selectDGIndexExecutable);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(438, 270);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "Others";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(408, 43);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(24, 23);
            this.button9.TabIndex = 75;
            this.button9.Text = "...";
            // 
            // selectAvisynthPluginsDir
            // 
            this.selectAvisynthPluginsDir.Location = new System.Drawing.Point(408, 70);
            this.selectAvisynthPluginsDir.Name = "selectAvisynthPluginsDir";
            this.selectAvisynthPluginsDir.Size = new System.Drawing.Size(24, 23);
            this.selectAvisynthPluginsDir.TabIndex = 76;
            this.selectAvisynthPluginsDir.Text = "...";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(99, 43);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(298, 21);
            this.textBox9.TabIndex = 73;
            this.textBox9.Text = "avisynth plugins Directory";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(45, 47);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(45, 13);
            this.label16.TabIndex = 72;
            this.label16.Text = "yadif.dll";
            // 
            // avisynthPluginsDir
            // 
            this.avisynthPluginsDir.Location = new System.Drawing.Point(99, 70);
            this.avisynthPluginsDir.Name = "avisynthPluginsDir";
            this.avisynthPluginsDir.ReadOnly = true;
            this.avisynthPluginsDir.Size = new System.Drawing.Size(298, 21);
            this.avisynthPluginsDir.TabIndex = 74;
            this.avisynthPluginsDir.Text = "avisynth plugins Directory";
            // 
            // dgIndexPath
            // 
            this.dgIndexPath.Location = new System.Drawing.Point(99, 16);
            this.dgIndexPath.Name = "dgIndexPath";
            this.dgIndexPath.ReadOnly = true;
            this.dgIndexPath.Size = new System.Drawing.Size(298, 21);
            this.dgIndexPath.TabIndex = 71;
            this.dgIndexPath.Text = "dgindex.exe";
            // 
            // dgIndexLabel
            // 
            this.dgIndexLabel.Location = new System.Drawing.Point(35, 19);
            this.dgIndexLabel.Name = "dgIndexLabel";
            this.dgIndexLabel.Size = new System.Drawing.Size(58, 21);
            this.dgIndexLabel.TabIndex = 70;
            this.dgIndexLabel.Text = "DGIndex";
            // 
            // selectDGIndexExecutable
            // 
            this.selectDGIndexExecutable.Location = new System.Drawing.Point(408, 16);
            this.selectDGIndexExecutable.Name = "selectDGIndexExecutable";
            this.selectDGIndexExecutable.Size = new System.Drawing.Size(24, 23);
            this.selectDGIndexExecutable.TabIndex = 69;
            this.selectDGIndexExecutable.Text = "...";
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
            // keep2ndPassLogfile
            // 
            this.keep2ndPassLogfile.Checked = true;
            this.keep2ndPassLogfile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keep2ndPassLogfile.Location = new System.Drawing.Point(232, 16);
            this.keep2ndPassLogfile.Name = "keep2ndPassLogfile";
            this.keep2ndPassLogfile.Size = new System.Drawing.Size(208, 24);
            this.keep2ndPassLogfile.TabIndex = 3;
            this.keep2ndPassLogfile.Text = "Overwrite Stats File in 3rd pass";
            // 
            // nbPassesLabel
            // 
            this.nbPassesLabel.Location = new System.Drawing.Point(8, 24);
            this.nbPassesLabel.Name = "nbPassesLabel";
            this.nbPassesLabel.Size = new System.Drawing.Size(100, 23);
            this.nbPassesLabel.TabIndex = 2;
            this.nbPassesLabel.Text = "Number of passes";
            // 
            // nbPasses
            // 
            this.nbPasses.Location = new System.Drawing.Point(120, 22);
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
            this.nbPasses.Size = new System.Drawing.Size(40, 20);
            this.nbPasses.TabIndex = 1;
            this.nbPasses.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // keep2ndPassOutput
            // 
            this.keep2ndPassOutput.Checked = true;
            this.keep2ndPassOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keep2ndPassOutput.Location = new System.Drawing.Point(232, 48);
            this.keep2ndPassOutput.Name = "keep2ndPassOutput";
            this.keep2ndPassOutput.Size = new System.Drawing.Size(216, 24);
            this.keep2ndPassOutput.TabIndex = 0;
            this.keep2ndPassOutput.Text = "Keep 2nd pass Output in 3 pass mode";
            // 
            // avisynthPluginsLabel
            // 
            this.avisynthPluginsLabel.AutoSize = true;
            this.avisynthPluginsLabel.Location = new System.Drawing.Point(6, 75);
            this.avisynthPluginsLabel.Name = "avisynthPluginsLabel";
            this.avisynthPluginsLabel.Size = new System.Drawing.Size(84, 13);
            this.avisynthPluginsLabel.TabIndex = 77;
            this.avisynthPluginsLabel.Text = "avisynth plugins";
            // 
            // mkvmergePathLabel
            // 
            this.mkvmergePathLabel.Location = new System.Drawing.Point(25, 45);
            this.mkvmergePathLabel.Name = "mkvmergePathLabel";
            this.mkvmergePathLabel.Size = new System.Drawing.Size(58, 21);
            this.mkvmergePathLabel.TabIndex = 58;
            this.mkvmergePathLabel.Text = "mkvmerge";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 21);
            this.label4.TabIndex = 57;
            this.label4.Text = "avimux_gui";
            // 
            // mp4boxPathLabel
            // 
            this.mp4boxPathLabel.Location = new System.Drawing.Point(37, 19);
            this.mp4boxPathLabel.Name = "mp4boxPathLabel";
            this.mp4boxPathLabel.Size = new System.Drawing.Size(46, 20);
            this.mp4boxPathLabel.TabIndex = 59;
            this.mp4boxPathLabel.Text = "mp4Box";
            // 
            // mkvmergePath
            // 
            this.mkvmergePath.Location = new System.Drawing.Point(89, 43);
            this.mkvmergePath.Name = "mkvmergePath";
            this.mkvmergePath.ReadOnly = true;
            this.mkvmergePath.Size = new System.Drawing.Size(308, 21);
            this.mkvmergePath.TabIndex = 62;
            this.mkvmergePath.Text = "mkvmerge.exe";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.aviMuxGUIPath);
            this.tabPage6.Controls.Add(this.mp4boxPath);
            this.tabPage6.Controls.Add(this.label4);
            this.tabPage6.Controls.Add(this.selectMkvmergeExecutableButton);
            this.tabPage6.Controls.Add(this.mkvmergePathLabel);
            this.tabPage6.Controls.Add(this.selectAviMuxGUIExecutableButton);
            this.tabPage6.Controls.Add(this.mp4boxPathLabel);
            this.tabPage6.Controls.Add(this.mkvmergePath);
            this.tabPage6.Controls.Add(this.selectMP4boxExecutableButton);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(438, 270);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Muxer";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // aviMuxGUIPath
            // 
            this.aviMuxGUIPath.Location = new System.Drawing.Point(89, 70);
            this.aviMuxGUIPath.Name = "aviMuxGUIPath";
            this.aviMuxGUIPath.ReadOnly = true;
            this.aviMuxGUIPath.Size = new System.Drawing.Size(308, 21);
            this.aviMuxGUIPath.TabIndex = 60;
            this.aviMuxGUIPath.Text = "avimux_gui.exe";
            // 
            // mp4boxPath
            // 
            this.mp4boxPath.Location = new System.Drawing.Point(89, 16);
            this.mp4boxPath.Name = "mp4boxPath";
            this.mp4boxPath.ReadOnly = true;
            this.mp4boxPath.Size = new System.Drawing.Size(308, 21);
            this.mp4boxPath.TabIndex = 61;
            this.mp4boxPath.Text = "mp4box.exe";
            // 
            // selectMkvmergeExecutableButton
            // 
            this.selectMkvmergeExecutableButton.Location = new System.Drawing.Point(408, 43);
            this.selectMkvmergeExecutableButton.Name = "selectMkvmergeExecutableButton";
            this.selectMkvmergeExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMkvmergeExecutableButton.TabIndex = 63;
            this.selectMkvmergeExecutableButton.Text = "...";
            // 
            // selectAviMuxGUIExecutableButton
            // 
            this.selectAviMuxGUIExecutableButton.Location = new System.Drawing.Point(408, 70);
            this.selectAviMuxGUIExecutableButton.Name = "selectAviMuxGUIExecutableButton";
            this.selectAviMuxGUIExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectAviMuxGUIExecutableButton.TabIndex = 64;
            this.selectAviMuxGUIExecutableButton.Text = "...";
            // 
            // selectMP4boxExecutableButton
            // 
            this.selectMP4boxExecutableButton.Location = new System.Drawing.Point(408, 16);
            this.selectMP4boxExecutableButton.Name = "selectMP4boxExecutableButton";
            this.selectMP4boxExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMP4boxExecutableButton.TabIndex = 65;
            this.selectMP4boxExecutableButton.Text = "...";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(408, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 91;
            this.button1.Text = "...";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(50, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 18);
            this.label1.TabIndex = 92;
            this.label1.Text = "faac";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(89, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(308, 21);
            this.textBox1.TabIndex = 93;
            this.textBox1.Text = "faac.exe";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(408, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 23);
            this.button2.TabIndex = 94;
            this.button2.Text = "...";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 21);
            this.label2.TabIndex = 95;
            this.label2.Text = "neroAacEnc";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(89, 43);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(308, 21);
            this.textBox2.TabIndex = 96;
            this.textBox2.Text = "neroAacEnc.exe";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(408, 70);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 23);
            this.button3.TabIndex = 97;
            this.button3.Text = "...";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(47, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 21);
            this.label3.TabIndex = 98;
            this.label3.Text = "lame";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(89, 70);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(308, 21);
            this.textBox3.TabIndex = 99;
            this.textBox3.Text = "lame.exe";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(408, 205);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 23);
            this.button4.TabIndex = 100;
            this.button4.Text = "...";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(29, 208);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 21);
            this.label7.TabIndex = 101;
            this.label7.Text = "oggenc2";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(89, 205);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(308, 21);
            this.textBox4.TabIndex = 102;
            this.textBox4.Text = "oggenc2.exe";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(408, 97);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(24, 23);
            this.button5.TabIndex = 103;
            this.button5.Text = "...";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 21);
            this.label8.TabIndex = 104;
            this.label8.Text = "enc_AudX_CLI";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(89, 97);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(308, 21);
            this.textBox5.TabIndex = 105;
            this.textBox5.Text = "enc_AudX_CLI.exe";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(408, 124);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(24, 23);
            this.button6.TabIndex = 106;
            this.button6.Text = "...";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(14, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 21);
            this.label9.TabIndex = 107;
            this.label9.Text = "enc_aacPlus";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(89, 124);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(308, 21);
            this.textBox6.TabIndex = 108;
            this.textBox6.Text = "enc_aacplus.exe";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(110, 232);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(177, 17);
            this.checkBox1.TabIndex = 109;
            this.checkBox1.Text = "I\'m using OggEnc2 v2.8 or later";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(408, 149);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(24, 23);
            this.button7.TabIndex = 111;
            this.button7.Text = "...";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(34, 154);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 23);
            this.label10.TabIndex = 113;
            this.label10.Text = "ffmpeg";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(408, 178);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(24, 23);
            this.button8.TabIndex = 110;
            this.button8.Text = "...";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(89, 151);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(308, 21);
            this.textBox7.TabIndex = 115;
            this.textBox7.Text = "dgindex.exe";
            // 
            // besplit
            // 
            this.besplit.Location = new System.Drawing.Point(34, 183);
            this.besplit.Name = "besplit";
            this.besplit.Size = new System.Drawing.Size(49, 16);
            this.besplit.TabIndex = 112;
            this.besplit.Text = "besplit";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(89, 178);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(308, 21);
            this.textBox8.TabIndex = 114;
            this.textBox8.Text = "dgindex.exe";
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton1.ArticleName = "Settings window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(12, 402);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 29;
            // 
            // SettingsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(468, 437);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::MeGUI.Properties.Settings.Default, "SettingsFormSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
            this.tabPage3.ResumeLayout(false);
            this.autoUpdateGroupBox.ResumeLayout(false);
            this.autoUpdateGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxServersToTry)).EndInit();
            this.outputExtensions.ResumeLayout(false);
            this.outputExtensions.PerformLayout();
            this.autoModeGroupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPasses)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private void button1_Click(object sender, EventArgs e)
        {
            if (selectExe("faac"))
            {
                textBox1.Text = openExecutableDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectExe("neroAacEnc"))
            {
                textBox2.Text = openExecutableDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectExe("lame"))
            {
                textBox3.Text = openExecutableDialog.FileName;
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

        private void selectAvc2AviExecutableButton_Click(object sender, EventArgs e)
        {
            if (selectExe("avc2avi"))
            {
                avc2aviPath.Text = openExecutableDialog.FileName;
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
                textBox4.Text = openExecutableDialog.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (selectExe("enc_AudX_CLI"))
            {
                textBox5.Text = openExecutableDialog.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (selectExe("enc_aacPlus"))
            {
                textBox6.Text = openExecutableDialog.FileName;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (selectExe("ffmpeg"))
            {
                textBox7.Text = openExecutableDialog.FileName;
            }
        }


        private void button8_Click(object sender, EventArgs e)
        {
            if (selectExe("besplit"))
            {
                textBox8.Text = openExecutableDialog.FileName;
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
                settings.FaacPath = textBox1.Text;
                settings.NeroAacEncPath = textBox2.Text;
                settings.LamePath = textBox3.Text;
				settings.Mp4boxPath = mp4boxPath.Text;
                settings.Avc2aviPath = avc2aviPath.Text;
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
				settings.AutoOpenScript = this.openScript.Checked;
				settings.DeleteCompletedJobs = deleteCompletedJobs.Checked;
				settings.DeleteIntermediateFiles = deleteIntermediateFiles.Checked;
				settings.DeleteAbortedOutput = deleteAbortedOutput.Checked;
				settings.OpenProgressWindow = openProgressWindow.Checked;
				settings.AutoSetNbThreads = autosetNbThreads.Checked;
				settings.Keep2of3passOutput = keep2ndPassOutput.Checked;
				settings.OverwriteStats = keep2ndPassLogfile.Checked;
				settings.NbPasses = (int)nbPasses.Value;
                settings.OggEnc2Path = textBox4.Text;
                settings.EncAudXPath = textBox5.Text;
                settings.EncAacPlusPath = textBox6.Text;
                settings.FreshOggEnc2 = checkBox1.Checked;
                settings.FFMpegPath = textBox7.Text;
                settings.AedSettings = this.autoEncodeDefaults;
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
                textBox1.Text = settings.FaacPath;
                textBox2.Text = settings.NeroAacEncPath;
                textBox3.Text = settings.LamePath;
                textBox8.Text = settings.BeSplitPath;
                sdSettings = settings.SourceDetectorSettings;
                chkboxUseAdvancedTooltips.Checked = settings.UseAdvancedTooltips;
				mp4boxPath.Text = settings.Mp4boxPath;
				mkvmergePath.Text = settings.MkvmergePath;
                xvidEncrawPath.Text = settings.XviDEncrawPath;
                avc2aviPath.Text = settings.Avc2aviPath;
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
				autosetNbThreads.Checked = settings.AutoSetNbThreads;
				keep2ndPassOutput.Checked = settings.Keep2of3passOutput;
				keep2ndPassLogfile.Checked = settings.OverwriteStats;
				nbPasses.Value = (decimal)settings.NbPasses;
                textBox4.Text = settings.OggEnc2Path;
                textBox5.Text = settings.EncAudXPath;
                textBox6.Text = settings.EncAacPlusPath;
                checkBox1.Checked = settings.FreshOggEnc2;
                textBox7.Text = settings.FFMpegPath;
                this.autoEncodeDefaults = settings.AedSettings;
			}
		}
		#endregion
	}
}
