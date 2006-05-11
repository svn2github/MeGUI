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
        private DialogSettings dialogSettings;
        private Button resetDialogs;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private SourceDetectorSettings sdSettings;
		private System.Windows.Forms.TextBox mp4boxPath;
		private System.Windows.Forms.Label mp4boxPathLabel;
		private System.Windows.Forms.Button selectMP4boxExecutableButton;
		private System.Windows.Forms.TextBox mkvmergePath;
		private System.Windows.Forms.Label mkvmergePathLabel;
		private System.Windows.Forms.Button selectMkvmergeExecutableButton;
		private System.Windows.Forms.TextBox besweetPath;
		private System.Windows.Forms.Label besweetPathLabel;
		private System.Windows.Forms.Button selectBesweetExecutableLabelButton;
		private System.Windows.Forms.Label dgIndexLabel;
		private System.Windows.Forms.TextBox dgIndexPath;
		private System.Windows.Forms.Button selectDGIndexExecutable;
		private System.Windows.Forms.CheckBox autoForceFilm;
		private System.Windows.Forms.NumericUpDown forceFilmPercentage;
        private System.Windows.Forms.Label percentLabel;
		private System.Windows.Forms.GroupBox vobGroupBox;
		private System.Windows.Forms.Label defaultAudioTrack1Label;
		private System.Windows.Forms.ComboBox defaultLanguage1;
		private System.Windows.Forms.Label defaultAudioTrack2Label;
		private System.Windows.Forms.ComboBox defaultLanguage2;
		private System.Windows.Forms.GroupBox autoModeGroupbox;
		private System.Windows.Forms.NumericUpDown nbPasses;
		private System.Windows.Forms.Label nbPassesLabel;
		private System.Windows.Forms.CheckBox keep2ndPassLogfile;
		private System.Windows.Forms.CheckBox keep2ndPassOutput;
        private System.Windows.Forms.FolderBrowserDialog openFolderDialog;
        private System.Windows.Forms.GroupBox outputExtensions;
        private System.Windows.Forms.TextBox videoExtension;
        private System.Windows.Forms.Label audioExtLabel;
        private System.Windows.Forms.Label videoExtLabel;
        private System.Windows.Forms.TextBox audioExtension;
        private System.Windows.Forms.Label avisynthPluginsLabel;
        private System.Windows.Forms.Button selectAvisynthPluginsDir;
        private System.Windows.Forms.TextBox avisynthPluginsDir;
        private CheckBox chkboxUseAdvancedTooltips;
        private Button configSourceDetector;
        private Label xvidEncrawLabel;
        private Button selectXvidEncrawButton;
        private TextBox xvidEncrawPath;
		private System.Windows.Forms.TextBox mencoderPath;
		private System.Windows.Forms.Label mencoderPathLabel;
		private System.Windows.Forms.Button selectMencoderExecutableButton;
		private System.Windows.Forms.TextBox x264ExePath;
		private System.Windows.Forms.Label x264ExePathLabel;
		private System.Windows.Forms.Button selectX264ExecutableButton;
        private System.Windows.Forms.GroupBox otherGroupBox;
        private System.Windows.Forms.CheckBox safeProfileAlteration;
		private System.Windows.Forms.ComboBox priority;
		private System.Windows.Forms.OpenFileDialog openExecutableDialog;
		private System.Windows.Forms.CheckBox autostartQueue;
		private System.Windows.Forms.CheckBox shutdown;
		private System.Windows.Forms.CheckBox openScript;
		private System.Windows.Forms.Label priorityLabel;
		private System.Windows.Forms.CheckBox deleteCompletedJobs;
		private System.Windows.Forms.CheckBox deleteAbortedOutput;
		private System.Windows.Forms.CheckBox deleteIntermediateFiles;
		private System.Windows.Forms.CheckBox openProgressWindow;
		private System.Windows.Forms.CheckBox autosetNbThreads;
        private TextBox textBox3;
        private Label label3;
        private Button button3;
        private TextBox textBox2;
        private Label label2;
        private Button button2;
        private TextBox textBox1;
        private Label label1;
        private Button button1;
        private CheckBox usingNero6;
        private NumericUpDown acceptableAspectError;
        private Label acceptableAspectErrorLabel;
        private Button selectAvc2AviExecutableButton;
        private Button selectAviMuxGUIExecutableButton;
        private TextBox avc2aviPath;
        private TextBox aviMuxGUIPath;
        private Label label5;
        private Label label4;
        private Button divxMuxSelectExeButton;
        private TextBox divxMuxPath;
        private Label label6;
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
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.xvidEncrawLabel = new System.Windows.Forms.Label();
            this.selectXvidEncrawButton = new System.Windows.Forms.Button();
            this.xvidEncrawPath = new System.Windows.Forms.TextBox();
            this.selectAvisynthPluginsDir = new System.Windows.Forms.Button();
            this.avisynthPluginsDir = new System.Windows.Forms.TextBox();
            this.avisynthPluginsLabel = new System.Windows.Forms.Label();
            this.dgIndexPath = new System.Windows.Forms.TextBox();
            this.dgIndexLabel = new System.Windows.Forms.Label();
            this.selectMP4boxExecutableButton = new System.Windows.Forms.Button();
            this.selectMkvmergeExecutableButton = new System.Windows.Forms.Button();
            this.selectBesweetExecutableLabelButton = new System.Windows.Forms.Button();
            this.selectMencoderExecutableButton = new System.Windows.Forms.Button();
            this.mp4boxPath = new System.Windows.Forms.TextBox();
            this.mkvmergePath = new System.Windows.Forms.TextBox();
            this.besweetPath = new System.Windows.Forms.TextBox();
            this.mencoderPath = new System.Windows.Forms.TextBox();
            this.mp4boxPathLabel = new System.Windows.Forms.Label();
            this.mkvmergePathLabel = new System.Windows.Forms.Label();
            this.besweetPathLabel = new System.Windows.Forms.Label();
            this.mencoderPathLabel = new System.Windows.Forms.Label();
            this.x264ExePathLabel = new System.Windows.Forms.Label();
            this.x264ExePath = new System.Windows.Forms.TextBox();
            this.selectX264ExecutableButton = new System.Windows.Forms.Button();
            this.selectDGIndexExecutable = new System.Windows.Forms.Button();
            this.otherGroupBox = new System.Windows.Forms.GroupBox();
            this.acceptableAspectError = new System.Windows.Forms.NumericUpDown();
            this.acceptableAspectErrorLabel = new System.Windows.Forms.Label();
            this.resetDialogs = new System.Windows.Forms.Button();
            this.configSourceDetector = new System.Windows.Forms.Button();
            this.chkboxUseAdvancedTooltips = new System.Windows.Forms.CheckBox();
            this.safeProfileAlteration = new System.Windows.Forms.CheckBox();
            this.openProgressWindow = new System.Windows.Forms.CheckBox();
            this.autosetNbThreads = new System.Windows.Forms.CheckBox();
            this.deleteIntermediateFiles = new System.Windows.Forms.CheckBox();
            this.deleteAbortedOutput = new System.Windows.Forms.CheckBox();
            this.deleteCompletedJobs = new System.Windows.Forms.CheckBox();
            this.openScript = new System.Windows.Forms.CheckBox();
            this.shutdown = new System.Windows.Forms.CheckBox();
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
            this.autoModeGroupbox = new System.Windows.Forms.GroupBox();
            this.keep2ndPassLogfile = new System.Windows.Forms.CheckBox();
            this.nbPassesLabel = new System.Windows.Forms.Label();
            this.nbPasses = new System.Windows.Forms.NumericUpDown();
            this.keep2ndPassOutput = new System.Windows.Forms.CheckBox();
            this.outputExtensions = new System.Windows.Forms.GroupBox();
            this.videoExtension = new System.Windows.Forms.TextBox();
            this.audioExtLabel = new System.Windows.Forms.Label();
            this.videoExtLabel = new System.Windows.Forms.Label();
            this.audioExtension = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.usingNero6 = new System.Windows.Forms.CheckBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.selectAvc2AviExecutableButton = new System.Windows.Forms.Button();
            this.selectAviMuxGUIExecutableButton = new System.Windows.Forms.Button();
            this.avc2aviPath = new System.Windows.Forms.TextBox();
            this.aviMuxGUIPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.divxMuxPath = new System.Windows.Forms.TextBox();
            this.divxMuxSelectExeButton = new System.Windows.Forms.Button();
            this.otherGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableAspectError)).BeginInit();
            this.vobGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forceFilmPercentage)).BeginInit();
            this.autoModeGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPasses)).BeginInit();
            this.outputExtensions.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(410, 501);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(48, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(356, 501);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            // 
            // xvidEncrawLabel
            // 
            this.xvidEncrawLabel.Location = new System.Drawing.Point(8, 200);
            this.xvidEncrawLabel.Name = "xvidEncrawLabel";
            this.xvidEncrawLabel.Size = new System.Drawing.Size(84, 16);
            this.xvidEncrawLabel.TabIndex = 19;
            this.xvidEncrawLabel.Text = "xvid_encraw";
            // 
            // selectXvidEncrawButton
            // 
            this.selectXvidEncrawButton.Location = new System.Drawing.Point(424, 194);
            this.selectXvidEncrawButton.Name = "selectXvidEncrawButton";
            this.selectXvidEncrawButton.Size = new System.Drawing.Size(24, 23);
            this.selectXvidEncrawButton.TabIndex = 18;
            this.selectXvidEncrawButton.Text = "...";
            this.selectXvidEncrawButton.Click += new System.EventHandler(this.selectXvidEncrawButton_Click);
            // 
            // xvidEncrawPath
            // 
            this.xvidEncrawPath.Location = new System.Drawing.Point(120, 195);
            this.xvidEncrawPath.Name = "xvidEncrawPath";
            this.xvidEncrawPath.ReadOnly = true;
            this.xvidEncrawPath.Size = new System.Drawing.Size(296, 21);
            this.xvidEncrawPath.TabIndex = 17;
            this.xvidEncrawPath.Text = "xvid_encraw.exe";
            // 
            // selectAvisynthPluginsDir
            // 
            this.selectAvisynthPluginsDir.Location = new System.Drawing.Point(424, 336);
            this.selectAvisynthPluginsDir.Name = "selectAvisynthPluginsDir";
            this.selectAvisynthPluginsDir.Size = new System.Drawing.Size(24, 23);
            this.selectAvisynthPluginsDir.TabIndex = 16;
            this.selectAvisynthPluginsDir.Text = "...";
            this.selectAvisynthPluginsDir.Click += new System.EventHandler(this.selectAvisynthDir_Click);
            // 
            // avisynthPluginsDir
            // 
            this.avisynthPluginsDir.Location = new System.Drawing.Point(120, 336);
            this.avisynthPluginsDir.Name = "avisynthPluginsDir";
            this.avisynthPluginsDir.ReadOnly = true;
            this.avisynthPluginsDir.Size = new System.Drawing.Size(296, 21);
            this.avisynthPluginsDir.TabIndex = 15;
            this.avisynthPluginsDir.Text = "avisynth plugins Directory";
            // 
            // avisynthPluginsLabel
            // 
            this.avisynthPluginsLabel.AutoSize = true;
            this.avisynthPluginsLabel.Location = new System.Drawing.Point(11, 344);
            this.avisynthPluginsLabel.Name = "avisynthPluginsLabel";
            this.avisynthPluginsLabel.Size = new System.Drawing.Size(84, 13);
            this.avisynthPluginsLabel.TabIndex = 14;
            this.avisynthPluginsLabel.Text = "avisynth plugins";
            // 
            // dgIndexPath
            // 
            this.dgIndexPath.Location = new System.Drawing.Point(120, 219);
            this.dgIndexPath.Name = "dgIndexPath";
            this.dgIndexPath.ReadOnly = true;
            this.dgIndexPath.Size = new System.Drawing.Size(296, 21);
            this.dgIndexPath.TabIndex = 11;
            this.dgIndexPath.Text = "dgindex.exe";
            // 
            // dgIndexLabel
            // 
            this.dgIndexLabel.Location = new System.Drawing.Point(8, 224);
            this.dgIndexLabel.Name = "dgIndexLabel";
            this.dgIndexLabel.Size = new System.Drawing.Size(100, 23);
            this.dgIndexLabel.TabIndex = 10;
            this.dgIndexLabel.Text = "DGIndex";
            // 
            // selectMP4boxExecutableButton
            // 
            this.selectMP4boxExecutableButton.Location = new System.Drawing.Point(424, 53);
            this.selectMP4boxExecutableButton.Name = "selectMP4boxExecutableButton";
            this.selectMP4boxExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMP4boxExecutableButton.TabIndex = 8;
            this.selectMP4boxExecutableButton.Text = "...";
            this.selectMP4boxExecutableButton.Click += new System.EventHandler(this.selectMP4boxExecutableButton_Click);
            // 
            // selectMkvmergeExecutableButton
            // 
            this.selectMkvmergeExecutableButton.Location = new System.Drawing.Point(424, 77);
            this.selectMkvmergeExecutableButton.Name = "selectMkvmergeExecutableButton";
            this.selectMkvmergeExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMkvmergeExecutableButton.TabIndex = 8;
            this.selectMkvmergeExecutableButton.Text = "...";
            this.selectMkvmergeExecutableButton.Click += new System.EventHandler(this.selectMkvmergeExecutableButton_Click);
            // 
            // selectBesweetExecutableLabelButton
            // 
            this.selectBesweetExecutableLabelButton.Location = new System.Drawing.Point(424, 29);
            this.selectBesweetExecutableLabelButton.Name = "selectBesweetExecutableLabelButton";
            this.selectBesweetExecutableLabelButton.Size = new System.Drawing.Size(24, 23);
            this.selectBesweetExecutableLabelButton.TabIndex = 7;
            this.selectBesweetExecutableLabelButton.Text = "...";
            this.selectBesweetExecutableLabelButton.Click += new System.EventHandler(this.selectBesweetExecutableLabelButton_Click);
            // 
            // selectMencoderExecutableButton
            // 
            this.selectMencoderExecutableButton.Location = new System.Drawing.Point(424, 5);
            this.selectMencoderExecutableButton.Name = "selectMencoderExecutableButton";
            this.selectMencoderExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectMencoderExecutableButton.TabIndex = 6;
            this.selectMencoderExecutableButton.Text = "...";
            this.selectMencoderExecutableButton.Click += new System.EventHandler(this.selectMencoderExecutableButton_Click);
            // 
            // mp4boxPath
            // 
            this.mp4boxPath.Location = new System.Drawing.Point(120, 53);
            this.mp4boxPath.Name = "mp4boxPath";
            this.mp4boxPath.ReadOnly = true;
            this.mp4boxPath.Size = new System.Drawing.Size(296, 21);
            this.mp4boxPath.TabIndex = 5;
            this.mp4boxPath.Text = "mp4box.exe";
            // 
            // mkvmergePath
            // 
            this.mkvmergePath.Location = new System.Drawing.Point(120, 77);
            this.mkvmergePath.Name = "mkvmergePath";
            this.mkvmergePath.ReadOnly = true;
            this.mkvmergePath.Size = new System.Drawing.Size(296, 21);
            this.mkvmergePath.TabIndex = 5;
            this.mkvmergePath.Text = "mkvmerge.exe";
            // 
            // besweetPath
            // 
            this.besweetPath.Location = new System.Drawing.Point(120, 29);
            this.besweetPath.Name = "besweetPath";
            this.besweetPath.ReadOnly = true;
            this.besweetPath.Size = new System.Drawing.Size(296, 21);
            this.besweetPath.TabIndex = 4;
            this.besweetPath.Text = "besweet.exe";
            // 
            // mencoderPath
            // 
            this.mencoderPath.Location = new System.Drawing.Point(120, 5);
            this.mencoderPath.Name = "mencoderPath";
            this.mencoderPath.ReadOnly = true;
            this.mencoderPath.Size = new System.Drawing.Size(296, 21);
            this.mencoderPath.TabIndex = 3;
            this.mencoderPath.Text = "mencoder.exe";
            // 
            // mp4boxPathLabel
            // 
            this.mp4boxPathLabel.Location = new System.Drawing.Point(8, 58);
            this.mp4boxPathLabel.Name = "mp4boxPathLabel";
            this.mp4boxPathLabel.Size = new System.Drawing.Size(100, 23);
            this.mp4boxPathLabel.TabIndex = 2;
            this.mp4boxPathLabel.Text = "mp4Box";
            // 
            // mkvmergePathLabel
            // 
            this.mkvmergePathLabel.Location = new System.Drawing.Point(8, 82);
            this.mkvmergePathLabel.Name = "mkvmergePathLabel";
            this.mkvmergePathLabel.Size = new System.Drawing.Size(100, 23);
            this.mkvmergePathLabel.TabIndex = 2;
            this.mkvmergePathLabel.Text = "mkvmerge";
            // 
            // besweetPathLabel
            // 
            this.besweetPathLabel.Location = new System.Drawing.Point(8, 32);
            this.besweetPathLabel.Name = "besweetPathLabel";
            this.besweetPathLabel.Size = new System.Drawing.Size(100, 23);
            this.besweetPathLabel.TabIndex = 1;
            this.besweetPathLabel.Text = "BeSweet";
            // 
            // mencoderPathLabel
            // 
            this.mencoderPathLabel.Location = new System.Drawing.Point(8, 10);
            this.mencoderPathLabel.Name = "mencoderPathLabel";
            this.mencoderPathLabel.Size = new System.Drawing.Size(100, 23);
            this.mencoderPathLabel.TabIndex = 0;
            this.mencoderPathLabel.Text = "mencoder";
            // 
            // x264ExePathLabel
            // 
            this.x264ExePathLabel.Location = new System.Drawing.Point(8, 174);
            this.x264ExePathLabel.Name = "x264ExePathLabel";
            this.x264ExePathLabel.Size = new System.Drawing.Size(64, 16);
            this.x264ExePathLabel.TabIndex = 9;
            this.x264ExePathLabel.Text = "x264";
            // 
            // x264ExePath
            // 
            this.x264ExePath.Location = new System.Drawing.Point(120, 171);
            this.x264ExePath.Name = "x264ExePath";
            this.x264ExePath.ReadOnly = true;
            this.x264ExePath.Size = new System.Drawing.Size(296, 21);
            this.x264ExePath.TabIndex = 9;
            this.x264ExePath.Text = "x264.exe";
            // 
            // selectX264ExecutableButton
            // 
            this.selectX264ExecutableButton.Location = new System.Drawing.Point(424, 171);
            this.selectX264ExecutableButton.Name = "selectX264ExecutableButton";
            this.selectX264ExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectX264ExecutableButton.TabIndex = 9;
            this.selectX264ExecutableButton.Text = "...";
            this.selectX264ExecutableButton.Click += new System.EventHandler(this.selectX264ExecutableButton_Click);
            // 
            // selectDGIndexExecutable
            // 
            this.selectDGIndexExecutable.Location = new System.Drawing.Point(424, 219);
            this.selectDGIndexExecutable.Name = "selectDGIndexExecutable";
            this.selectDGIndexExecutable.Size = new System.Drawing.Size(24, 23);
            this.selectDGIndexExecutable.TabIndex = 3;
            this.selectDGIndexExecutable.Text = "...";
            this.selectDGIndexExecutable.Click += new System.EventHandler(this.selectDGIndexExecutable_Click);
            // 
            // otherGroupBox
            // 
            this.otherGroupBox.Controls.Add(this.acceptableAspectError);
            this.otherGroupBox.Controls.Add(this.acceptableAspectErrorLabel);
            this.otherGroupBox.Controls.Add(this.resetDialogs);
            this.otherGroupBox.Controls.Add(this.configSourceDetector);
            this.otherGroupBox.Controls.Add(this.chkboxUseAdvancedTooltips);
            this.otherGroupBox.Controls.Add(this.safeProfileAlteration);
            this.otherGroupBox.Controls.Add(this.openProgressWindow);
            this.otherGroupBox.Controls.Add(this.autosetNbThreads);
            this.otherGroupBox.Controls.Add(this.deleteIntermediateFiles);
            this.otherGroupBox.Controls.Add(this.deleteAbortedOutput);
            this.otherGroupBox.Controls.Add(this.deleteCompletedJobs);
            this.otherGroupBox.Controls.Add(this.openScript);
            this.otherGroupBox.Controls.Add(this.shutdown);
            this.otherGroupBox.Controls.Add(this.autostartQueue);
            this.otherGroupBox.Controls.Add(this.priority);
            this.otherGroupBox.Controls.Add(this.priorityLabel);
            this.otherGroupBox.Location = new System.Drawing.Point(2, 88);
            this.otherGroupBox.Name = "otherGroupBox";
            this.otherGroupBox.Size = new System.Drawing.Size(456, 197);
            this.otherGroupBox.TabIndex = 3;
            this.otherGroupBox.TabStop = false;
            this.otherGroupBox.Text = "Other";
            // 
            // acceptableAspectError
            // 
            this.acceptableAspectError.Location = new System.Drawing.Point(194, 43);
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
            this.resetDialogs.Location = new System.Drawing.Point(12, 162);
            this.resetDialogs.Name = "resetDialogs";
            this.resetDialogs.Size = new System.Drawing.Size(149, 23);
            this.resetDialogs.TabIndex = 26;
            this.resetDialogs.Text = "Reset All Dialogs";
            this.resetDialogs.UseVisualStyleBackColor = true;
            this.resetDialogs.Click += new System.EventHandler(this.resetDialogs_Click);
            // 
            // configSourceDetector
            // 
            this.configSourceDetector.Location = new System.Drawing.Point(287, 162);
            this.configSourceDetector.Name = "configSourceDetector";
            this.configSourceDetector.Size = new System.Drawing.Size(154, 23);
            this.configSourceDetector.TabIndex = 6;
            this.configSourceDetector.Text = "Configure Source Detector";
            this.configSourceDetector.UseVisualStyleBackColor = true;
            this.configSourceDetector.Click += new System.EventHandler(this.configSourceDetector_Click);
            // 
            // chkboxUseAdvancedTooltips
            // 
            this.chkboxUseAdvancedTooltips.Location = new System.Drawing.Point(7, 64);
            this.chkboxUseAdvancedTooltips.Name = "chkboxUseAdvancedTooltips";
            this.chkboxUseAdvancedTooltips.Size = new System.Drawing.Size(152, 24);
            this.chkboxUseAdvancedTooltips.TabIndex = 23;
            this.chkboxUseAdvancedTooltips.Text = "Use Advanced ToolTips";
            // 
            // safeProfileAlteration
            // 
            this.safeProfileAlteration.Location = new System.Drawing.Point(289, 132);
            this.safeProfileAlteration.Name = "safeProfileAlteration";
            this.safeProfileAlteration.Size = new System.Drawing.Size(152, 24);
            this.safeProfileAlteration.TabIndex = 22;
            this.safeProfileAlteration.Text = "Safe Profile Alteration";
            // 
            // openProgressWindow
            // 
            this.openProgressWindow.Checked = true;
            this.openProgressWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openProgressWindow.Location = new System.Drawing.Point(289, 112);
            this.openProgressWindow.Name = "openProgressWindow";
            this.openProgressWindow.Size = new System.Drawing.Size(144, 24);
            this.openProgressWindow.TabIndex = 21;
            this.openProgressWindow.Text = "Open Progress Window";
            // 
            // autosetNbThreads
            // 
            this.autosetNbThreads.Location = new System.Drawing.Point(7, 136);
            this.autosetNbThreads.Name = "autosetNbThreads";
            this.autosetNbThreads.Size = new System.Drawing.Size(208, 24);
            this.autosetNbThreads.TabIndex = 20;
            this.autosetNbThreads.Text = "Automatically set number of Threads";
            // 
            // deleteIntermediateFiles
            // 
            this.deleteIntermediateFiles.Location = new System.Drawing.Point(289, 88);
            this.deleteIntermediateFiles.Name = "deleteIntermediateFiles";
            this.deleteIntermediateFiles.Size = new System.Drawing.Size(152, 24);
            this.deleteIntermediateFiles.TabIndex = 19;
            this.deleteIntermediateFiles.Text = "Delete intermediate files";
            // 
            // deleteAbortedOutput
            // 
            this.deleteAbortedOutput.Location = new System.Drawing.Point(7, 112);
            this.deleteAbortedOutput.Name = "deleteAbortedOutput";
            this.deleteAbortedOutput.Size = new System.Drawing.Size(184, 24);
            this.deleteAbortedOutput.TabIndex = 18;
            this.deleteAbortedOutput.Text = "Delete Output of aborted jobs";
            // 
            // deleteCompletedJobs
            // 
            this.deleteCompletedJobs.Location = new System.Drawing.Point(289, 64);
            this.deleteCompletedJobs.Name = "deleteCompletedJobs";
            this.deleteCompletedJobs.Size = new System.Drawing.Size(144, 24);
            this.deleteCompletedJobs.TabIndex = 17;
            this.deleteCompletedJobs.Text = "Delete completed Jobs";
            // 
            // openScript
            // 
            this.openScript.Location = new System.Drawing.Point(7, 88);
            this.openScript.Name = "openScript";
            this.openScript.Size = new System.Drawing.Size(248, 24);
            this.openScript.TabIndex = 16;
            this.openScript.Text = "Open Preview after AviSynth script selection";
            // 
            // shutdown
            // 
            this.shutdown.Location = new System.Drawing.Point(289, 16);
            this.shutdown.Name = "shutdown";
            this.shutdown.Size = new System.Drawing.Size(152, 24);
            this.shutdown.TabIndex = 15;
            this.shutdown.Text = "Shutdown after encoding";
            // 
            // autostartQueue
            // 
            this.autostartQueue.Location = new System.Drawing.Point(289, 40);
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
            this.priority.Location = new System.Drawing.Point(128, 16);
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
            this.vobGroupBox.Location = new System.Drawing.Point(2, 0);
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
            // autoModeGroupbox
            // 
            this.autoModeGroupbox.Controls.Add(this.keep2ndPassLogfile);
            this.autoModeGroupbox.Controls.Add(this.nbPassesLabel);
            this.autoModeGroupbox.Controls.Add(this.nbPasses);
            this.autoModeGroupbox.Controls.Add(this.keep2ndPassOutput);
            this.autoModeGroupbox.Location = new System.Drawing.Point(4, 291);
            this.autoModeGroupbox.Name = "autoModeGroupbox";
            this.autoModeGroupbox.Size = new System.Drawing.Size(456, 78);
            this.autoModeGroupbox.TabIndex = 5;
            this.autoModeGroupbox.TabStop = false;
            this.autoModeGroupbox.Text = "Automated Encoding";
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
            this.nbPasses.Size = new System.Drawing.Size(40, 21);
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
            // outputExtensions
            // 
            this.outputExtensions.Controls.Add(this.videoExtension);
            this.outputExtensions.Controls.Add(this.audioExtLabel);
            this.outputExtensions.Controls.Add(this.videoExtLabel);
            this.outputExtensions.Controls.Add(this.audioExtension);
            this.outputExtensions.Location = new System.Drawing.Point(3, 375);
            this.outputExtensions.Name = "outputExtensions";
            this.outputExtensions.Size = new System.Drawing.Size(218, 78);
            this.outputExtensions.TabIndex = 4;
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
            this.videoExtLabel.Size = new System.Drawing.Size(33, 13);
            this.videoExtLabel.TabIndex = 23;
            this.videoExtLabel.Text = "Video";
            // 
            // audioExtension
            // 
            this.audioExtension.Location = new System.Drawing.Point(6, 48);
            this.audioExtension.Name = "audioExtension";
            this.audioExtension.Size = new System.Drawing.Size(120, 21);
            this.audioExtension.TabIndex = 22;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(468, 487);
            this.tabControl1.TabIndex = 28;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.outputExtensions);
            this.tabPage1.Controls.Add(this.autoModeGroupbox);
            this.tabPage1.Controls.Add(this.vobGroupBox);
            this.tabPage1.Controls.Add(this.otherGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(460, 461);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.usingNero6);
            this.tabPage2.Controls.Add(this.textBox3);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.xvidEncrawLabel);
            this.tabPage2.Controls.Add(this.selectXvidEncrawButton);
            this.tabPage2.Controls.Add(this.xvidEncrawPath);
            this.tabPage2.Controls.Add(this.selectAvisynthPluginsDir);
            this.tabPage2.Controls.Add(this.avisynthPluginsDir);
            this.tabPage2.Controls.Add(this.avisynthPluginsLabel);
            this.tabPage2.Controls.Add(this.dgIndexPath);
            this.tabPage2.Controls.Add(this.dgIndexLabel);
            this.tabPage2.Controls.Add(this.selectMP4boxExecutableButton);
            this.tabPage2.Controls.Add(this.selectAvc2AviExecutableButton);
            this.tabPage2.Controls.Add(this.divxMuxSelectExeButton);
            this.tabPage2.Controls.Add(this.selectAviMuxGUIExecutableButton);
            this.tabPage2.Controls.Add(this.selectMkvmergeExecutableButton);
            this.tabPage2.Controls.Add(this.selectBesweetExecutableLabelButton);
            this.tabPage2.Controls.Add(this.selectMencoderExecutableButton);
            this.tabPage2.Controls.Add(this.mp4boxPath);
            this.tabPage2.Controls.Add(this.avc2aviPath);
            this.tabPage2.Controls.Add(this.divxMuxPath);
            this.tabPage2.Controls.Add(this.aviMuxGUIPath);
            this.tabPage2.Controls.Add(this.mkvmergePath);
            this.tabPage2.Controls.Add(this.besweetPath);
            this.tabPage2.Controls.Add(this.mencoderPath);
            this.tabPage2.Controls.Add(this.mp4boxPathLabel);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.mkvmergePathLabel);
            this.tabPage2.Controls.Add(this.besweetPathLabel);
            this.tabPage2.Controls.Add(this.mencoderPathLabel);
            this.tabPage2.Controls.Add(this.x264ExePathLabel);
            this.tabPage2.Controls.Add(this.x264ExePath);
            this.tabPage2.Controls.Add(this.selectX264ExecutableButton);
            this.tabPage2.Controls.Add(this.selectDGIndexExecutable);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(460, 461);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Program Paths";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // usingNero6
            // 
            this.usingNero6.AutoSize = true;
            this.usingNero6.Location = new System.Drawing.Point(120, 289);
            this.usingNero6.Name = "usingNero6";
            this.usingNero6.Size = new System.Drawing.Size(196, 17);
            this.usingNero6.TabIndex = 29;
            this.usingNero6.Text = "I\'m using Nero6 AAC Dll\'s to encode";
            this.usingNero6.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(120, 312);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(296, 21);
            this.textBox3.TabIndex = 28;
            this.textBox3.Text = "dgindex.exe";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 27;
            this.label3.Text = "lame";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(424, 312);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 23);
            this.button3.TabIndex = 26;
            this.button3.Text = "...";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(120, 267);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(296, 21);
            this.textBox2.TabIndex = 25;
            this.textBox2.Text = "dgindex.exe";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 24;
            this.label2.Text = "neroraw";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(424, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "...";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, 243);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(296, 21);
            this.textBox1.TabIndex = 22;
            this.textBox1.Text = "dgindex.exe";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 21;
            this.label1.Text = "faac";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(424, 243);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "...";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // selectAvc2AviExecutableButton
            // 
            this.selectAvc2AviExecutableButton.Location = new System.Drawing.Point(424, 146);
            this.selectAvc2AviExecutableButton.Name = "selectAvc2AviExecutableButton";
            this.selectAvc2AviExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectAvc2AviExecutableButton.TabIndex = 8;
            this.selectAvc2AviExecutableButton.Text = "...";
            this.selectAvc2AviExecutableButton.Click += new System.EventHandler(this.selectAvc2AviExecutableButton_Click);
            // 
            // selectAviMuxGUIExecutableButton
            // 
            this.selectAviMuxGUIExecutableButton.Location = new System.Drawing.Point(424, 100);
            this.selectAviMuxGUIExecutableButton.Name = "selectAviMuxGUIExecutableButton";
            this.selectAviMuxGUIExecutableButton.Size = new System.Drawing.Size(24, 23);
            this.selectAviMuxGUIExecutableButton.TabIndex = 8;
            this.selectAviMuxGUIExecutableButton.Text = "...";
            this.selectAviMuxGUIExecutableButton.Click += new System.EventHandler(this.selectAviMuxGUIExecutableButton_Click);
            // 
            // avc2aviPath
            // 
            this.avc2aviPath.Location = new System.Drawing.Point(120, 146);
            this.avc2aviPath.Name = "avc2aviPath";
            this.avc2aviPath.ReadOnly = true;
            this.avc2aviPath.Size = new System.Drawing.Size(296, 21);
            this.avc2aviPath.TabIndex = 5;
            this.avc2aviPath.Text = "avc2avi.exe";
            // 
            // aviMuxGUIPath
            // 
            this.aviMuxGUIPath.Location = new System.Drawing.Point(120, 100);
            this.aviMuxGUIPath.Name = "aviMuxGUIPath";
            this.aviMuxGUIPath.ReadOnly = true;
            this.aviMuxGUIPath.Size = new System.Drawing.Size(296, 21);
            this.aviMuxGUIPath.TabIndex = 5;
            this.aviMuxGUIPath.Text = "avimux_gui.exe";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "avc2avi";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "avimux_gui";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 2;
            this.label6.Text = "avimux_gui";
            // 
            // divxMuxPath
            // 
            this.divxMuxPath.Location = new System.Drawing.Point(120, 123);
            this.divxMuxPath.Name = "divxMuxPath";
            this.divxMuxPath.ReadOnly = true;
            this.divxMuxPath.Size = new System.Drawing.Size(296, 21);
            this.divxMuxPath.TabIndex = 5;
            this.divxMuxPath.Text = "divxmux.exe";
            // 
            // divxMuxSelectExeButton
            // 
            this.divxMuxSelectExeButton.Location = new System.Drawing.Point(424, 123);
            this.divxMuxSelectExeButton.Name = "divxMuxSelectExeButton";
            this.divxMuxSelectExeButton.Size = new System.Drawing.Size(24, 23);
            this.divxMuxSelectExeButton.TabIndex = 8;
            this.divxMuxSelectExeButton.Text = "...";
            this.divxMuxSelectExeButton.Click += new System.EventHandler(this.divxMuxSelectExeButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(468, 536);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.otherGroupBox.ResumeLayout(false);
            this.otherGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acceptableAspectError)).EndInit();
            this.vobGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.forceFilmPercentage)).EndInit();
            this.autoModeGroupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nbPasses)).EndInit();
            this.outputExtensions.ResumeLayout(false);
            this.outputExtensions.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

		}
        #endregion
        #region button handlers
        private bool selectExe(string exe)
        {
            openExecutableDialog.Filter = exe + " executable|" + exe +".exe|Any executable|*.exe";
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
		private void selectBesweetExecutableLabelButton_Click(object sender, System.EventArgs e)
		{
			if (selectExe("besweet"))
			{
				besweetPath.Text = openExecutableDialog.FileName;
				string fileName = Path.GetFileName(openExecutableDialog.FileName);
				string path = openExecutableDialog.FileName.Substring(0, openExecutableDialog.FileName.LastIndexOf(fileName));
				if (!File.Exists(path + "\\azid.dll"))
					MessageBox.Show("azid.dll is missing from your BeSweet directory. Make sure you place azid.dll where besweet.exe is located or AC3 input cannot be processed", 
						"Component not found",
						MessageBoxButtons.OK);
				if (!File.Exists(path + "\\shibatch.dll"))
					MessageBox.Show("shibatch.dll is missing from your BeSweet directory. Make sure you place shibatch.dll where besweet.exe is located", "Component not found",
						MessageBoxButtons.OK);
				if (!File.Exists(path + "\\hip.dll"))
					MessageBox.Show("hip.dll is missing from your BeSweet directory. Make sure you place hip.dll where besweet.exe is located or MP2/MP3 and MPA files cannot be processed", 
						"Component not found",
						MessageBoxButtons.OK);
				if (!File.Exists(path + "\\aacenc32.dll") || !File.Exists(path + "\\aac.dll") || !File.Exists(path + "\\bsn.dll"))
					MessageBox.Show("Make sure both bsn.dll, aacenc32.dll and aac.dll are in your BeSweet directory, or AAC encoding using Nero will not be possible", "Component not found",
						MessageBoxButtons.OK);
				if (!File.Exists(path + @"\bse_FAAC.dll") || !File.Exists(path + @"\faac.exe"))
					MessageBox.Show("Make sure both bse_FAAC.dll and faac.exe are in your BeSweet directory, or AAC encoding using FAAC will not be possible", "Component not found",
						MessageBoxButtons.OK);
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
            pathEnv = MeGUI.GetDirectoryName(dgIndexPath.Text) + ";" + pathEnv;
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
            if (selectExe("neroraw"))
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
		#endregion
		#region properties
		public MeGUISettings Settings
		{
			get 
			{
				MeGUISettings settings = new MeGUISettings();
                settings.DialogSettings = dialogSettings;
                settings.AcceptableAspectErrorPercent = (int)acceptableAspectError.Value;
				settings.MencoderPath = mencoderPath.Text;
                settings.UsingNero6 = usingNero6.Checked;
                settings.SourceDetectorSettings = sdSettings;
                settings.FaacPath = textBox1.Text;
                settings.NerorawPath = textBox2.Text;
                settings.LamePath = textBox3.Text;
			    settings.BesweetPath = besweetPath.Text;
				settings.Mp4boxPath = mp4boxPath.Text;
                settings.Avc2aviPath = avc2aviPath.Text;
                settings.AviMuxGUIPath = aviMuxGUIPath.Text;
				settings.MkvmergePath = mkvmergePath.Text;
                settings.XviDEncrawPath = xvidEncrawPath.Text;
                settings.VideoExtension = videoExtension.Text;
                settings.AudioExtension = audioExtension.Text;
                settings.UseAdvancedTooltips = chkboxUseAdvancedTooltips.Checked;
				settings.X264Path = x264ExePath.Text;
				settings.DgIndexPath = dgIndexPath.Text;
				settings.DefaultLanguage1 = defaultLanguage1.Text;
				settings.DefaultLanguage2 = defaultLanguage2.Text;
				settings.AutoForceFilm = autoForceFilm.Checked;
				settings.ForceFilmThreshold = forceFilmPercentage.Value;
				settings.DefaultPriority = (ProcessPriority)priority.SelectedIndex;
				settings.AutoStartQueue = this.autostartQueue.Checked;
				settings.Shutdown = this.shutdown.Checked;
				settings.AutoOpenScript = this.openScript.Checked;
				settings.DeleteCompletedJobs = deleteCompletedJobs.Checked;
				settings.DeleteIntermediateFiles = deleteIntermediateFiles.Checked;
				settings.DeleteAbortedOutput = deleteAbortedOutput.Checked;
				settings.OpenProgressWindow = openProgressWindow.Checked;
                settings.SafeProfileAlteration = safeProfileAlteration.Checked;
				settings.AutoSetNbThreads = autosetNbThreads.Checked;
				settings.Keep2of3passOutput = keep2ndPassOutput.Checked;
				settings.OverwriteStats = keep2ndPassLogfile.Checked;
                settings.DivXMuxPath = divxMuxPath.Text;
				settings.NbPasses = (int)nbPasses.Value;
				return settings;
			}
			set
			{
				MeGUISettings settings = value;
                acceptableAspectError.Value = (decimal)settings.AcceptableAspectErrorPercent;
                dialogSettings = settings.DialogSettings;
				mencoderPath.Text = settings.MencoderPath;      
                usingNero6.Checked = settings.UsingNero6;
                textBox1.Text = settings.FaacPath;
                textBox2.Text = settings.NerorawPath;
                textBox3.Text = settings.LamePath;
                sdSettings = settings.SourceDetectorSettings;
                chkboxUseAdvancedTooltips.Checked = settings.UseAdvancedTooltips;
				besweetPath.Text = settings.BesweetPath;
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
				shutdown.Checked = settings.Shutdown;
				deleteCompletedJobs.Checked = settings.DeleteCompletedJobs;
				openScript.Checked = settings.AutoOpenScript;
				deleteIntermediateFiles.Checked = settings.DeleteIntermediateFiles;
				deleteAbortedOutput.Checked = settings.DeleteAbortedOutput;
				openProgressWindow.Checked = settings.OpenProgressWindow;
                safeProfileAlteration.Checked = settings.SafeProfileAlteration;
				autosetNbThreads.Checked = settings.AutoSetNbThreads;
				keep2ndPassOutput.Checked = settings.Keep2of3passOutput;
				keep2ndPassLogfile.Checked = settings.OverwriteStats;
                divxMuxPath.Text = settings.DivXMuxPath;
				nbPasses.Value = (decimal)settings.NbPasses;
			}
		}
		#endregion

        private void resetDialogs_Click(object sender, EventArgs e)
        {
            dialogSettings = new DialogSettings();
            MessageBox.Show(this, "Successfully reset all dialogs", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void divxMuxSelectExeButton_Click(object sender, EventArgs e)
        {
            if (selectExe("divxmux"))
            {
                divxMuxPath.Text = openExecutableDialog.FileName;
            }
        }

	}
}
