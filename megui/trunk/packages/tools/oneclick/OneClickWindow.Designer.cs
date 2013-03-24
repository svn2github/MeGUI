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

namespace MeGUI
{
    partial class OneClickWindow
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneClickWindow));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.outputTab = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.outputLabel = new System.Windows.Forms.Label();
            this.videoTab = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.inputLabel = new System.Windows.Forms.Label();
            this.audioTab = new System.Windows.Forms.TabControl();
            this.audioPage0 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.subtitlesTab = new System.Windows.Forms.TabControl();
            this.subPage0 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.subtitleMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.subtitleAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitleRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audioMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.audioAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audioRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.encoderConfigTab = new System.Windows.Forms.TabPage();
            this.avsBox = new System.Windows.Forms.GroupBox();
            this.keepInputResolution = new System.Windows.Forms.CheckBox();
            this.autoCrop = new System.Windows.Forms.CheckBox();
            this.autoDeint = new System.Windows.Forms.CheckBox();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.outputResolutionLabel = new System.Windows.Forms.Label();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ARLabel = new System.Windows.Forms.Label();
            this.locationGroupBox = new System.Windows.Forms.GroupBox();
            this.chapterLabel = new System.Windows.Forms.Label();
            this.workingDirectoryLabel = new System.Windows.Forms.Label();
            this.workingName = new System.Windows.Forms.TextBox();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filesizeLabel = new System.Windows.Forms.Label();
            this.devicetype = new System.Windows.Forms.ComboBox();
            this.deviceLabel = new System.Windows.Forms.Label();
            this.containerFormatLabel = new System.Windows.Forms.Label();
            this.containerFormat = new System.Windows.Forms.ComboBox();
            this.videoGroupBox = new System.Windows.Forms.GroupBox();
            this.chkDontEncodeVideo = new System.Windows.Forms.CheckBox();
            this.usechaptersmarks = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.addPrerenderJob = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.goButton = new System.Windows.Forms.Button();
            this.openOnQueue = new System.Windows.Forms.CheckBox();
            this.cbGUIMode = new System.Windows.Forms.ComboBox();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.oneclickProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.output = new MeGUI.FileBar();
            this.input = new MeGUI.core.gui.FileSCBox();
            this.oneClickAudioStreamControl1 = new MeGUI.OneClickStreamControl();
            this.oneClickSubtitleStreamControl1 = new MeGUI.OneClickStreamControl();
            this.avsProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            this.ar = new MeGUI.core.gui.ARChooser();
            this.chapterFile = new MeGUI.FileBar();
            this.workingDirectory = new MeGUI.FileBar();
            this.fileSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.splitting = new MeGUI.core.gui.TargetSizeSCBox();
            this.videoProfile = new MeGUI.core.gui.ConfigableProfilesControl();
            label1 = new System.Windows.Forms.Label();
            this.tabPage1.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.videoTab.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.audioTab.SuspendLayout();
            this.audioPage0.SuspendLayout();
            this.subtitlesTab.SuspendLayout();
            this.subPage0.SuspendLayout();
            this.subtitleMenu.SuspendLayout();
            this.audioMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.encoderConfigTab.SuspendLayout();
            this.avsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.locationGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.videoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 47);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(45, 13);
            label1.TabIndex = 39;
            label1.Text = "Splitting";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.outputTab);
            this.tabPage1.Controls.Add(this.videoTab);
            this.tabPage1.Controls.Add(this.audioTab);
            this.tabPage1.Controls.Add(this.subtitlesTab);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(464, 487);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // outputTab
            // 
            this.outputTab.Controls.Add(this.tabPage4);
            this.outputTab.Location = new System.Drawing.Point(6, 379);
            this.outputTab.Name = "outputTab";
            this.outputTab.SelectedIndex = 0;
            this.outputTab.Size = new System.Drawing.Size(452, 95);
            this.outputTab.TabIndex = 22;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.oneclickProfile);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.output);
            this.tabPage4.Controls.Add(this.outputLabel);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(444, 69);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Output";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "OneClick profile";
            // 
            // outputLabel
            // 
            this.outputLabel.Location = new System.Drawing.Point(3, 14);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(100, 13);
            this.outputLabel.TabIndex = 28;
            this.outputLabel.Text = "Output file";
            // 
            // videoTab
            // 
            this.videoTab.Controls.Add(this.tabPage3);
            this.videoTab.Location = new System.Drawing.Point(6, 6);
            this.videoTab.Name = "videoTab";
            this.videoTab.SelectedIndex = 0;
            this.videoTab.Size = new System.Drawing.Size(452, 65);
            this.videoTab.TabIndex = 21;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.input);
            this.tabPage3.Controls.Add(this.inputLabel);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(444, 39);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Video";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // inputLabel
            // 
            this.inputLabel.Location = new System.Drawing.Point(8, 14);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(100, 13);
            this.inputLabel.TabIndex = 49;
            this.inputLabel.Text = "Input";
            // 
            // audioTab
            // 
            this.audioTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audioTab.Controls.Add(this.audioPage0);
            this.audioTab.Controls.Add(this.tabPage2);
            this.audioTab.Location = new System.Drawing.Point(6, 77);
            this.audioTab.Name = "audioTab";
            this.audioTab.SelectedIndex = 0;
            this.audioTab.Size = new System.Drawing.Size(452, 175);
            this.audioTab.TabIndex = 20;
            this.audioTab.SelectedIndexChanged += new System.EventHandler(this.audioTab_SelectedIndexChanged);
            this.audioTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.audioTab_KeyUp);
            this.audioTab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.audioTab_MouseClick);
            this.audioTab.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.audioTab_MouseDoubleClick);
            // 
            // audioPage0
            // 
            this.audioPage0.Controls.Add(this.oneClickAudioStreamControl1);
            this.audioPage0.Location = new System.Drawing.Point(4, 22);
            this.audioPage0.Name = "audioPage0";
            this.audioPage0.Padding = new System.Windows.Forms.Padding(3);
            this.audioPage0.Size = new System.Drawing.Size(444, 149);
            this.audioPage0.TabIndex = 2;
            this.audioPage0.Text = "Audio 1";
            this.audioPage0.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(444, 149);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "   +";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // subtitlesTab
            // 
            this.subtitlesTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitlesTab.Controls.Add(this.subPage0);
            this.subtitlesTab.Controls.Add(this.tabPage5);
            this.subtitlesTab.Location = new System.Drawing.Point(6, 258);
            this.subtitlesTab.Name = "subtitlesTab";
            this.subtitlesTab.SelectedIndex = 0;
            this.subtitlesTab.Size = new System.Drawing.Size(452, 115);
            this.subtitlesTab.TabIndex = 19;
            this.subtitlesTab.SelectedIndexChanged += new System.EventHandler(this.subtitlesTab_SelectedIndexChanged);
            this.subtitlesTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.subtitlesTab_KeyUp);
            this.subtitlesTab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.subtitlesTab_MouseClick);
            this.subtitlesTab.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.subtitlesTab_MouseDoubleClick);
            // 
            // subPage0
            // 
            this.subPage0.Controls.Add(this.oneClickSubtitleStreamControl1);
            this.subPage0.Location = new System.Drawing.Point(4, 22);
            this.subPage0.Name = "subPage0";
            this.subPage0.Padding = new System.Windows.Forms.Padding(3);
            this.subPage0.Size = new System.Drawing.Size(444, 89);
            this.subPage0.TabIndex = 2;
            this.subPage0.Text = "Subtitle 1";
            this.subPage0.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(444, 89);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "   +";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // subtitleMenu
            // 
            this.subtitleMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subtitleAddTrack,
            this.subtitleRemoveTrack});
            this.subtitleMenu.Name = "subtitleMenu";
            this.subtitleMenu.Size = new System.Drawing.Size(150, 48);
            this.subtitleMenu.Opening += new System.ComponentModel.CancelEventHandler(this.subtitleMenu_Opening);
            // 
            // subtitleAddTrack
            // 
            this.subtitleAddTrack.Name = "subtitleAddTrack";
            this.subtitleAddTrack.Size = new System.Drawing.Size(149, 22);
            this.subtitleAddTrack.Text = "Add Track";
            this.subtitleAddTrack.Click += new System.EventHandler(this.subtitleAddTrack_Click);
            // 
            // subtitleRemoveTrack
            // 
            this.subtitleRemoveTrack.Name = "subtitleRemoveTrack";
            this.subtitleRemoveTrack.Size = new System.Drawing.Size(149, 22);
            this.subtitleRemoveTrack.Text = "Remove Track";
            this.subtitleRemoveTrack.Click += new System.EventHandler(this.subtitleRemoveTrack_Click);
            // 
            // audioMenu
            // 
            this.audioMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioAddTrack,
            this.audioRemoveTrack});
            this.audioMenu.Name = "audioMenu";
            this.audioMenu.Size = new System.Drawing.Size(150, 48);
            this.audioMenu.Opening += new System.ComponentModel.CancelEventHandler(this.audioMenu_Opening);
            // 
            // audioAddTrack
            // 
            this.audioAddTrack.Name = "audioAddTrack";
            this.audioAddTrack.Size = new System.Drawing.Size(149, 22);
            this.audioAddTrack.Text = "Add Track";
            this.audioAddTrack.Click += new System.EventHandler(this.audioAddTrack_Click);
            // 
            // audioRemoveTrack
            // 
            this.audioRemoveTrack.Name = "audioRemoveTrack";
            this.audioRemoveTrack.Size = new System.Drawing.Size(149, 22);
            this.audioRemoveTrack.Text = "Remove Track";
            this.audioRemoveTrack.Click += new System.EventHandler(this.audioRemoveTrack_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.encoderConfigTab);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(472, 513);
            this.tabControl1.TabIndex = 0;
            // 
            // encoderConfigTab
            // 
            this.encoderConfigTab.Controls.Add(this.avsBox);
            this.encoderConfigTab.Controls.Add(this.locationGroupBox);
            this.encoderConfigTab.Controls.Add(this.groupBox1);
            this.encoderConfigTab.Controls.Add(this.videoGroupBox);
            this.encoderConfigTab.Location = new System.Drawing.Point(4, 22);
            this.encoderConfigTab.Name = "encoderConfigTab";
            this.encoderConfigTab.Padding = new System.Windows.Forms.Padding(3);
            this.encoderConfigTab.Size = new System.Drawing.Size(464, 487);
            this.encoderConfigTab.TabIndex = 2;
            this.encoderConfigTab.Text = "Advanced Config";
            this.encoderConfigTab.UseVisualStyleBackColor = true;
            // 
            // avsBox
            // 
            this.avsBox.Controls.Add(this.keepInputResolution);
            this.avsBox.Controls.Add(this.autoCrop);
            this.avsBox.Controls.Add(this.avsProfile);
            this.avsBox.Controls.Add(this.ar);
            this.avsBox.Controls.Add(this.autoDeint);
            this.avsBox.Controls.Add(this.signalAR);
            this.avsBox.Controls.Add(this.outputResolutionLabel);
            this.avsBox.Controls.Add(this.horizontalResolution);
            this.avsBox.Controls.Add(this.label2);
            this.avsBox.Controls.Add(this.ARLabel);
            this.avsBox.Location = new System.Drawing.Point(6, 102);
            this.avsBox.Name = "avsBox";
            this.avsBox.Size = new System.Drawing.Size(452, 144);
            this.avsBox.TabIndex = 44;
            this.avsBox.TabStop = false;
            this.avsBox.Text = " AviSynth Settings ";
            // 
            // keepInputResolution
            // 
            this.keepInputResolution.AutoSize = true;
            this.keepInputResolution.Location = new System.Drawing.Point(120, 42);
            this.keepInputResolution.Name = "keepInputResolution";
            this.keepInputResolution.Size = new System.Drawing.Size(246, 17);
            this.keepInputResolution.TabIndex = 25;
            this.keepInputResolution.Text = "Keep Input Resolution (disable Crop && Resize)";
            this.keepInputResolution.UseVisualStyleBackColor = true;
            this.keepInputResolution.CheckedChanged += new System.EventHandler(this.keepInputResolution_CheckedChanged);
            // 
            // autoCrop
            // 
            this.autoCrop.AutoSize = true;
            this.autoCrop.Checked = true;
            this.autoCrop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCrop.Location = new System.Drawing.Point(202, 19);
            this.autoCrop.Name = "autoCrop";
            this.autoCrop.Size = new System.Drawing.Size(72, 17);
            this.autoCrop.TabIndex = 24;
            this.autoCrop.Text = "AutoCrop";
            this.autoCrop.UseVisualStyleBackColor = true;
            // 
            // autoDeint
            // 
            this.autoDeint.AutoSize = true;
            this.autoDeint.Location = new System.Drawing.Point(119, 120);
            this.autoDeint.Name = "autoDeint";
            this.autoDeint.Size = new System.Drawing.Size(139, 17);
            this.autoDeint.TabIndex = 20;
            this.autoDeint.Text = "Automatic Deinterlacing";
            this.autoDeint.UseVisualStyleBackColor = true;
            // 
            // signalAR
            // 
            this.signalAR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signalAR.AutoSize = true;
            this.signalAR.Location = new System.Drawing.Point(282, 19);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(112, 17);
            this.signalAR.TabIndex = 5;
            this.signalAR.Text = "Anamorph Output";
            // 
            // outputResolutionLabel
            // 
            this.outputResolutionLabel.Location = new System.Drawing.Point(6, 20);
            this.outputResolutionLabel.Name = "outputResolutionLabel";
            this.outputResolutionLabel.Size = new System.Drawing.Size(100, 13);
            this.outputResolutionLabel.TabIndex = 3;
            this.outputResolutionLabel.Text = "Output Resolution";
            // 
            // horizontalResolution
            // 
            this.horizontalResolution.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Location = new System.Drawing.Point(120, 16);
            this.horizontalResolution.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.horizontalResolution.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.horizontalResolution.Name = "horizontalResolution";
            this.horizontalResolution.Size = new System.Drawing.Size(64, 21);
            this.horizontalResolution.TabIndex = 0;
            this.horizontalResolution.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Avisynth Profile";
            // 
            // ARLabel
            // 
            this.ARLabel.AutoSize = true;
            this.ARLabel.Location = new System.Drawing.Point(6, 69);
            this.ARLabel.Name = "ARLabel";
            this.ARLabel.Size = new System.Drawing.Size(105, 13);
            this.ARLabel.TabIndex = 4;
            this.ARLabel.Text = "Display Aspect Ratio";
            // 
            // locationGroupBox
            // 
            this.locationGroupBox.Controls.Add(this.chapterFile);
            this.locationGroupBox.Controls.Add(this.workingDirectory);
            this.locationGroupBox.Controls.Add(this.chapterLabel);
            this.locationGroupBox.Controls.Add(this.workingDirectoryLabel);
            this.locationGroupBox.Controls.Add(this.workingName);
            this.locationGroupBox.Controls.Add(this.projectNameLabel);
            this.locationGroupBox.Location = new System.Drawing.Point(6, 359);
            this.locationGroupBox.Name = "locationGroupBox";
            this.locationGroupBox.Size = new System.Drawing.Size(452, 122);
            this.locationGroupBox.TabIndex = 43;
            this.locationGroupBox.TabStop = false;
            this.locationGroupBox.Text = " Extra IO ";
            // 
            // chapterLabel
            // 
            this.chapterLabel.Location = new System.Drawing.Point(6, 53);
            this.chapterLabel.Name = "chapterLabel";
            this.chapterLabel.Size = new System.Drawing.Size(100, 13);
            this.chapterLabel.TabIndex = 36;
            this.chapterLabel.Text = "Chapter File";
            // 
            // workingDirectoryLabel
            // 
            this.workingDirectoryLabel.Location = new System.Drawing.Point(6, 21);
            this.workingDirectoryLabel.Name = "workingDirectoryLabel";
            this.workingDirectoryLabel.Size = new System.Drawing.Size(100, 13);
            this.workingDirectoryLabel.TabIndex = 32;
            this.workingDirectoryLabel.Text = "Working Directory";
            // 
            // workingName
            // 
            this.workingName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workingName.Location = new System.Drawing.Point(119, 79);
            this.workingName.Name = "workingName";
            this.workingName.Size = new System.Drawing.Size(329, 21);
            this.workingName.TabIndex = 30;
            this.workingName.TextChanged += new System.EventHandler(this.workingName_TextChanged);
            // 
            // projectNameLabel
            // 
            this.projectNameLabel.Location = new System.Drawing.Point(6, 82);
            this.projectNameLabel.Name = "projectNameLabel";
            this.projectNameLabel.Size = new System.Drawing.Size(73, 16);
            this.projectNameLabel.TabIndex = 31;
            this.projectNameLabel.Text = "Project Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileSize);
            this.groupBox1.Controls.Add(this.filesizeLabel);
            this.groupBox1.Controls.Add(this.devicetype);
            this.groupBox1.Controls.Add(this.deviceLabel);
            this.groupBox1.Controls.Add(this.containerFormatLabel);
            this.groupBox1.Controls.Add(this.containerFormat);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.splitting);
            this.groupBox1.Location = new System.Drawing.Point(6, 252);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 100);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Output Settings ";
            // 
            // filesizeLabel
            // 
            this.filesizeLabel.Location = new System.Drawing.Point(6, 21);
            this.filesizeLabel.Name = "filesizeLabel";
            this.filesizeLabel.Size = new System.Drawing.Size(90, 13);
            this.filesizeLabel.TabIndex = 45;
            this.filesizeLabel.Text = "Filesize";
            // 
            // devicetype
            // 
            this.devicetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.devicetype.FormattingEnabled = true;
            this.devicetype.Location = new System.Drawing.Point(351, 69);
            this.devicetype.Name = "devicetype";
            this.devicetype.Size = new System.Drawing.Size(95, 21);
            this.devicetype.TabIndex = 44;
            // 
            // deviceLabel
            // 
            this.deviceLabel.AutoSize = true;
            this.deviceLabel.Location = new System.Drawing.Point(279, 72);
            this.deviceLabel.Name = "deviceLabel";
            this.deviceLabel.Size = new System.Drawing.Size(66, 13);
            this.deviceLabel.TabIndex = 43;
            this.deviceLabel.Text = "Device Type";
            // 
            // containerFormatLabel
            // 
            this.containerFormatLabel.AutoSize = true;
            this.containerFormatLabel.Location = new System.Drawing.Point(6, 72);
            this.containerFormatLabel.Name = "containerFormatLabel";
            this.containerFormatLabel.Size = new System.Drawing.Size(91, 13);
            this.containerFormatLabel.TabIndex = 42;
            this.containerFormatLabel.Text = "Container Format";
            // 
            // containerFormat
            // 
            this.containerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containerFormat.Location = new System.Drawing.Point(120, 69);
            this.containerFormat.Name = "containerFormat";
            this.containerFormat.Size = new System.Drawing.Size(95, 21);
            this.containerFormat.TabIndex = 41;
            this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.containerFormat_SelectedIndexChanged);
            // 
            // videoGroupBox
            // 
            this.videoGroupBox.Controls.Add(this.chkDontEncodeVideo);
            this.videoGroupBox.Controls.Add(this.usechaptersmarks);
            this.videoGroupBox.Controls.Add(this.label4);
            this.videoGroupBox.Controls.Add(this.videoProfile);
            this.videoGroupBox.Controls.Add(this.addPrerenderJob);
            this.videoGroupBox.Location = new System.Drawing.Point(6, 6);
            this.videoGroupBox.Name = "videoGroupBox";
            this.videoGroupBox.Size = new System.Drawing.Size(452, 90);
            this.videoGroupBox.TabIndex = 41;
            this.videoGroupBox.TabStop = false;
            this.videoGroupBox.Text = " Video Settings ";
            // 
            // chkDontEncodeVideo
            // 
            this.chkDontEncodeVideo.AutoSize = true;
            this.chkDontEncodeVideo.Location = new System.Drawing.Point(119, 16);
            this.chkDontEncodeVideo.Name = "chkDontEncodeVideo";
            this.chkDontEncodeVideo.Size = new System.Drawing.Size(118, 17);
            this.chkDontEncodeVideo.TabIndex = 40;
            this.chkDontEncodeVideo.Text = "Don\'t encode video";
            this.chkDontEncodeVideo.UseVisualStyleBackColor = true;
            this.chkDontEncodeVideo.CheckedChanged += new System.EventHandler(this.chkDontEncodeVideo_CheckedChanged);
            // 
            // usechaptersmarks
            // 
            this.usechaptersmarks.AutoSize = true;
            this.usechaptersmarks.Location = new System.Drawing.Point(120, 67);
            this.usechaptersmarks.Name = "usechaptersmarks";
            this.usechaptersmarks.Size = new System.Drawing.Size(234, 17);
            this.usechaptersmarks.TabIndex = 39;
            this.usechaptersmarks.Text = "Force using Key-Frames for chapters marks";
            this.usechaptersmarks.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Encoder Settings";
            // 
            // addPrerenderJob
            // 
            this.addPrerenderJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addPrerenderJob.AutoSize = true;
            this.addPrerenderJob.Location = new System.Drawing.Point(-507, 47);
            this.addPrerenderJob.Name = "addPrerenderJob";
            this.addPrerenderJob.Size = new System.Drawing.Size(132, 17);
            this.addPrerenderJob.TabIndex = 16;
            this.addPrerenderJob.Text = "Add pre-rendering job";
            this.addPrerenderJob.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "IFO Files|*.ifo|VOB Files (*.vob)|*.vob|MPEG-1/2 Program Streams (*.mpg)|*.mpg|Tr" +
    "ansport Streams (*.ts)|*.ts";
            // 
            // goButton
            // 
            this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.goButton.Location = new System.Drawing.Point(385, 519);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 29;
            this.goButton.Text = "Go!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // openOnQueue
            // 
            this.openOnQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openOnQueue.Checked = true;
            this.openOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openOnQueue.Location = new System.Drawing.Point(273, 520);
            this.openOnQueue.Name = "openOnQueue";
            this.openOnQueue.Size = new System.Drawing.Size(106, 24);
            this.openOnQueue.TabIndex = 33;
            this.openOnQueue.Text = "close after Go!";
            // 
            // cbGUIMode
            // 
            this.cbGUIMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbGUIMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGUIMode.FormattingEnabled = true;
            this.cbGUIMode.Location = new System.Drawing.Point(104, 521);
            this.cbGUIMode.Name = "cbGUIMode";
            this.cbGUIMode.Size = new System.Drawing.Size(145, 21);
            this.cbGUIMode.TabIndex = 34;
            this.cbGUIMode.SelectedIndexChanged += new System.EventHandler(this.cbGUIMode_SelectedIndexChanged);
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpButton1.ArticleName = "One Click Encoder";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(12, 519);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 32;
            // 
            // oneclickProfile
            // 
            this.oneclickProfile.Location = new System.Drawing.Point(99, 40);
            this.oneclickProfile.Name = "oneclickProfile";
            this.oneclickProfile.ProfileSet = "OneClick";
            this.oneclickProfile.Size = new System.Drawing.Size(332, 22);
            this.oneclickProfile.TabIndex = 31;
            this.oneclickProfile.SelectedProfileChanged += new System.EventHandler(this.OneClickProfileChanged);
            // 
            // output
            // 
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.output.Filename = "";
            this.output.Filter = "MP4 Files|*.mp4";
            this.output.FilterIndex = 0;
            this.output.FolderMode = false;
            this.output.Location = new System.Drawing.Point(99, 8);
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.SaveMode = true;
            this.output.Size = new System.Drawing.Size(333, 26);
            this.output.TabIndex = 29;
            this.output.Title = null;
            this.output.FileSelected += new MeGUI.FileBarEventHandler(this.output_FileSelected);
            // 
            // input
            // 
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input.Filter = "All files (*.*)|*.*";
            this.input.Location = new System.Drawing.Point(63, 6);
            this.input.MaximumSize = new System.Drawing.Size(1000, 29);
            this.input.MinimumSize = new System.Drawing.Size(64, 29);
            this.input.Name = "input";
            this.input.SelectedIndex = -1;
            this.input.SelectedItem = null;
            this.input.Size = new System.Drawing.Size(368, 29);
            this.input.TabIndex = 50;
            this.input.Type = MeGUI.core.gui.FileSCBox.FileSCBoxType.OC_FILE_AND_FOLDER;
            this.input.SelectionChanged += new MeGUI.StringChanged(this.input_SelectionChanged);
            // 
            // oneClickAudioStreamControl1
            // 
            this.oneClickAudioStreamControl1.CustomStreams = new object[0];
            this.oneClickAudioStreamControl1.Filter = "All files (*.*)|*.*";
            this.oneClickAudioStreamControl1.Location = new System.Drawing.Point(0, 0);
            this.oneClickAudioStreamControl1.Name = "oneClickAudioStreamControl1";
            this.oneClickAudioStreamControl1.SelectedStreamIndex = 0;
            this.oneClickAudioStreamControl1.ShowDefaultStream = false;
            this.oneClickAudioStreamControl1.ShowDelay = true;
            this.oneClickAudioStreamControl1.ShowForceStream = false;
            this.oneClickAudioStreamControl1.Size = new System.Drawing.Size(434, 149);
            this.oneClickAudioStreamControl1.StandardStreams = new object[0];
            this.oneClickAudioStreamControl1.TabIndex = 0;
            this.oneClickAudioStreamControl1.FileUpdated += new System.EventHandler(this.oneClickAudioStreamControl_FileUpdated);
            // 
            // oneClickSubtitleStreamControl1
            // 
            this.oneClickSubtitleStreamControl1.CustomStreams = new object[0];
            this.oneClickSubtitleStreamControl1.Filter = "All files (*.*)|*.*";
            this.oneClickSubtitleStreamControl1.Location = new System.Drawing.Point(0, 0);
            this.oneClickSubtitleStreamControl1.Name = "oneClickSubtitleStreamControl1";
            this.oneClickSubtitleStreamControl1.SelectedStreamIndex = 0;
            this.oneClickSubtitleStreamControl1.ShowDefaultStream = true;
            this.oneClickSubtitleStreamControl1.ShowDelay = true;
            this.oneClickSubtitleStreamControl1.ShowForceStream = true;
            this.oneClickSubtitleStreamControl1.Size = new System.Drawing.Size(434, 90);
            this.oneClickSubtitleStreamControl1.StandardStreams = new object[0];
            this.oneClickSubtitleStreamControl1.TabIndex = 0;
            this.oneClickSubtitleStreamControl1.FileUpdated += new System.EventHandler(this.oneClickSubtitleStreamControl_FileUpdated);
            // 
            // avsProfile
            // 
            this.avsProfile.Location = new System.Drawing.Point(119, 92);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.ProfileSet = "AviSynth";
            this.avsProfile.Size = new System.Drawing.Size(330, 22);
            this.avsProfile.TabIndex = 23;
            // 
            // ar
            // 
            this.ar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ar.CustomDARs = new MeGUI.core.util.Dar[0];
            this.ar.HasLater = true;
            this.ar.Location = new System.Drawing.Point(119, 61);
            this.ar.MaximumSize = new System.Drawing.Size(1000, 29);
            this.ar.MinimumSize = new System.Drawing.Size(64, 29);
            this.ar.Name = "ar";
            this.ar.SelectedIndex = 0;
            this.ar.Size = new System.Drawing.Size(330, 29);
            this.ar.TabIndex = 22;
            // 
            // chapterFile
            // 
            this.chapterFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chapterFile.Filename = "";
            this.chapterFile.Filter = "Chapter files (*.txt)|*.txt";
            this.chapterFile.FilterIndex = 0;
            this.chapterFile.FolderMode = false;
            this.chapterFile.Location = new System.Drawing.Point(120, 47);
            this.chapterFile.Name = "chapterFile";
            this.chapterFile.ReadOnly = true;
            this.chapterFile.SaveMode = false;
            this.chapterFile.Size = new System.Drawing.Size(326, 26);
            this.chapterFile.TabIndex = 39;
            this.chapterFile.Title = null;
            // 
            // workingDirectory
            // 
            this.workingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workingDirectory.Filename = "";
            this.workingDirectory.Filter = null;
            this.workingDirectory.FilterIndex = 0;
            this.workingDirectory.FolderMode = true;
            this.workingDirectory.Location = new System.Drawing.Point(120, 15);
            this.workingDirectory.Name = "workingDirectory";
            this.workingDirectory.ReadOnly = true;
            this.workingDirectory.SaveMode = false;
            this.workingDirectory.Size = new System.Drawing.Size(326, 26);
            this.workingDirectory.TabIndex = 38;
            this.workingDirectory.Title = null;
            this.workingDirectory.FileSelected += new MeGUI.FileBarEventHandler(this.workingDirectory_FileSelected);
            // 
            // fileSize
            // 
            this.fileSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileSize.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.fileSize.Location = new System.Drawing.Point(120, 13);
            this.fileSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.fileSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.fileSize.Name = "fileSize";
            this.fileSize.NullString = "Don\'t Care";
            this.fileSize.SaveCustomValues = false;
            this.fileSize.SelectedIndex = 0;
            this.fileSize.Size = new System.Drawing.Size(326, 29);
            this.fileSize.TabIndex = 46;
            // 
            // splitting
            // 
            this.splitting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitting.AutoSize = true;
            this.splitting.CustomSizes = new MeGUI.core.util.FileSize[0];
            this.splitting.Location = new System.Drawing.Point(120, 40);
            this.splitting.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitting.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitting.Name = "splitting";
            this.splitting.NullString = "No splitting";
            this.splitting.SaveCustomValues = false;
            this.splitting.SelectedIndex = 0;
            this.splitting.Size = new System.Drawing.Size(326, 29);
            this.splitting.TabIndex = 40;
            // 
            // videoProfile
            // 
            this.videoProfile.Location = new System.Drawing.Point(119, 39);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.ProfileSet = "Video";
            this.videoProfile.Size = new System.Drawing.Size(327, 22);
            this.videoProfile.TabIndex = 17;
            this.videoProfile.SelectedProfileChanged += new System.EventHandler(this.ProfileChanged);
            // 
            // OneClickWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 550);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.openOnQueue);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.cbGUIMode);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OneClickWindow";
            this.Text = "MeGUI - One Click Encoder";
            this.Shown += new System.EventHandler(this.OneClickWindow_Shown);
            this.tabPage1.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.videoTab.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.audioTab.ResumeLayout(false);
            this.audioPage0.ResumeLayout(false);
            this.subtitlesTab.ResumeLayout(false);
            this.subPage0.ResumeLayout(false);
            this.subtitleMenu.ResumeLayout(false);
            this.audioMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.encoderConfigTab.ResumeLayout(false);
            this.avsBox.ResumeLayout(false);
            this.avsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.locationGroupBox.ResumeLayout(false);
            this.locationGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.videoGroupBox.ResumeLayout(false);
            this.videoGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage encoderConfigTab;
        private System.Windows.Forms.Button goButton;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.CheckBox openOnQueue;
        private System.Windows.Forms.TabControl subtitlesTab;
        private System.Windows.Forms.ContextMenuStrip subtitleMenu;
        private System.Windows.Forms.ToolStripMenuItem subtitleAddTrack;
        private System.Windows.Forms.ToolStripMenuItem subtitleRemoveTrack;
        private System.Windows.Forms.TabPage subPage0;
        private OneClickStreamControl oneClickSubtitleStreamControl1;
        private System.Windows.Forms.TabControl audioTab;
        private System.Windows.Forms.ContextMenuStrip audioMenu;
        private System.Windows.Forms.ToolStripMenuItem audioAddTrack;
        private System.Windows.Forms.ToolStripMenuItem audioRemoveTrack;
        private System.Windows.Forms.TabPage audioPage0;
        private OneClickStreamControl oneClickAudioStreamControl1;
        private System.Windows.Forms.TabControl outputTab;
        private System.Windows.Forms.TabPage tabPage4;
        private core.gui.ConfigableProfilesControl oneclickProfile;
        private System.Windows.Forms.Label label3;
        private FileBar output;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.TabControl videoTab;
        private System.Windows.Forms.TabPage tabPage3;
        private core.gui.FileSCBox input;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.GroupBox videoGroupBox;
        private System.Windows.Forms.CheckBox chkDontEncodeVideo;
        private System.Windows.Forms.CheckBox usechaptersmarks;
        private System.Windows.Forms.Label label4;
        private core.gui.ConfigableProfilesControl videoProfile;
        private System.Windows.Forms.CheckBox addPrerenderJob;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox devicetype;
        private System.Windows.Forms.Label deviceLabel;
        private System.Windows.Forms.Label containerFormatLabel;
        private System.Windows.Forms.ComboBox containerFormat;
        private core.gui.TargetSizeSCBox splitting;
        private System.Windows.Forms.GroupBox locationGroupBox;
        private FileBar chapterFile;
        private FileBar workingDirectory;
        private System.Windows.Forms.Label chapterLabel;
        private System.Windows.Forms.Label workingDirectoryLabel;
        private System.Windows.Forms.TextBox workingName;
        private System.Windows.Forms.Label projectNameLabel;
        private System.Windows.Forms.GroupBox avsBox;
        private System.Windows.Forms.CheckBox keepInputResolution;
        private System.Windows.Forms.CheckBox autoCrop;
        private core.gui.ConfigableProfilesControl avsProfile;
        private core.gui.ARChooser ar;
        private System.Windows.Forms.CheckBox autoDeint;
        private System.Windows.Forms.CheckBox signalAR;
        private System.Windows.Forms.Label outputResolutionLabel;
        private System.Windows.Forms.NumericUpDown horizontalResolution;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ARLabel;
        private core.gui.TargetSizeSCBox fileSize;
        private System.Windows.Forms.Label filesizeLabel;
        private System.Windows.Forms.ComboBox cbGUIMode;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage5;
    }
}
