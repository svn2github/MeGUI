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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.locationGroupBox = new System.Windows.Forms.GroupBox();
            this.chapterFileOpen = new System.Windows.Forms.Button();
            this.chapterFile = new System.Windows.Forms.TextBox();
            this.chapterLabel = new System.Windows.Forms.Label();
            this.workingDirectoryLabel = new System.Windows.Forms.Label();
            this.workingDirectory = new System.Windows.Forms.TextBox();
            this.workingDirectoryButton = new System.Windows.Forms.Button();
            this.workingName = new System.Windows.Forms.TextBox();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.avsBox = new System.Windows.Forms.GroupBox();
            this.autoDeint = new System.Windows.Forms.CheckBox();
            this.avsProfile = new System.Windows.Forms.ComboBox();
            this.avsProfileLabel = new System.Windows.Forms.Label();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.AR = new System.Windows.Forms.TextBox();
            this.outputResolutionLabel = new System.Windows.Forms.Label();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.arComboBox = new System.Windows.Forms.ComboBox();
            this.ARLabel = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.IOGroupbox = new System.Windows.Forms.GroupBox();
            this.selectOutput = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.TextBox();
            this.outputLabel = new System.Windows.Forms.Label();
            this.input = new System.Windows.Forms.TextBox();
            this.inputLabel = new System.Windows.Forms.Label();
            this.openButton = new System.Windows.Forms.Button();
            this.targetGroupBox = new System.Windows.Forms.GroupBox();
            this.filesize = new System.Windows.Forms.TextBox();
            this.inKBLabel = new System.Windows.Forms.Label();
            this.filesizeComboBox = new System.Windows.Forms.ComboBox();
            this.filesizeLabel = new System.Windows.Forms.Label();
            this.playbackMethodLabel = new System.Windows.Forms.Label();
            this.playbackMethod = new System.Windows.Forms.ComboBox();
            this.audioGroupbox = new System.Windows.Forms.GroupBox();
            this.clearAudio2Button = new System.Windows.Forms.Button();
            this.clearAudio1Button = new System.Windows.Forms.Button();
            this.track2Label = new System.Windows.Forms.Label();
            this.track1Label = new System.Windows.Forms.Label();
            this.track2 = new System.Windows.Forms.ComboBox();
            this.track1 = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.encoderConfigTab = new System.Windows.Forms.TabPage();
            this.audioIOGroupBox = new System.Windows.Forms.GroupBox();
            this.externalInput = new System.Windows.Forms.CheckBox();
            this.audioCodecLabel = new System.Windows.Forms.Label();
            this.audioProfile = new System.Windows.Forms.ComboBox();
            this.dontEncodeAudio = new System.Windows.Forms.CheckBox();
            this.audioProfileLabel = new System.Windows.Forms.Label();
            this.audioInput = new System.Windows.Forms.TextBox();
            this.audioInputOpenButton = new System.Windows.Forms.Button();
            this.audioTrack2 = new System.Windows.Forms.RadioButton();
            this.audioTrack1 = new System.Windows.Forms.RadioButton();
            this.deleteAudioButton = new System.Windows.Forms.Button();
            this.configAudioButton = new System.Windows.Forms.Button();
            this.audioCodec = new System.Windows.Forms.ComboBox();
            this.videoGroupBox = new System.Windows.Forms.GroupBox();
            this.addPrerenderJob = new System.Windows.Forms.CheckBox();
            this.videoCodecLabel = new System.Windows.Forms.Label();
            this.videoCodec = new System.Windows.Forms.ComboBox();
            this.videoProfileLabel = new System.Windows.Forms.Label();
            this.videoProfile = new System.Windows.Forms.ComboBox();
            this.videoConfigButton = new System.Windows.Forms.Button();
            this.splitOutput = new System.Windows.Forms.CheckBox();
            this.splitSize = new System.Windows.Forms.TextBox();
            this.containerFormatLabel = new System.Windows.Forms.Label();
            this.containerFormat = new System.Windows.Forms.ComboBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.outputDialog = new System.Windows.Forms.SaveFileDialog();
            this.showAdvancedOptions = new System.Windows.Forms.CheckBox();
            this.shutdownCheckBox = new System.Windows.Forms.CheckBox();
            this.goButton = new System.Windows.Forms.Button();
            this.tabPage2.SuspendLayout();
            this.locationGroupBox.SuspendLayout();
            this.avsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.IOGroupbox.SuspendLayout();
            this.targetGroupBox.SuspendLayout();
            this.audioGroupbox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.encoderConfigTab.SuspendLayout();
            this.audioIOGroupBox.SuspendLayout();
            this.videoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.locationGroupBox);
            this.tabPage2.Controls.Add(this.avsBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(440, 252);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced Config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // locationGroupBox
            // 
            this.locationGroupBox.Controls.Add(this.chapterFileOpen);
            this.locationGroupBox.Controls.Add(this.chapterFile);
            this.locationGroupBox.Controls.Add(this.chapterLabel);
            this.locationGroupBox.Controls.Add(this.workingDirectoryLabel);
            this.locationGroupBox.Controls.Add(this.workingDirectory);
            this.locationGroupBox.Controls.Add(this.workingDirectoryButton);
            this.locationGroupBox.Controls.Add(this.workingName);
            this.locationGroupBox.Controls.Add(this.projectNameLabel);
            this.locationGroupBox.Location = new System.Drawing.Point(8, 6);
            this.locationGroupBox.Name = "locationGroupBox";
            this.locationGroupBox.Size = new System.Drawing.Size(424, 95);
            this.locationGroupBox.TabIndex = 23;
            this.locationGroupBox.TabStop = false;
            this.locationGroupBox.Text = "Extra IO";
            // 
            // chapterFileOpen
            // 
            this.chapterFileOpen.Location = new System.Drawing.Point(384, 36);
            this.chapterFileOpen.Name = "chapterFileOpen";
            this.chapterFileOpen.Size = new System.Drawing.Size(24, 23);
            this.chapterFileOpen.TabIndex = 35;
            this.chapterFileOpen.Text = "...";
            this.chapterFileOpen.Click += new System.EventHandler(this.chapterFileOpen_Click);
            // 
            // chapterFile
            // 
            this.chapterFile.Location = new System.Drawing.Point(120, 38);
            this.chapterFile.Name = "chapterFile";
            this.chapterFile.ReadOnly = true;
            this.chapterFile.Size = new System.Drawing.Size(256, 20);
            this.chapterFile.TabIndex = 37;
            // 
            // chapterLabel
            // 
            this.chapterLabel.Location = new System.Drawing.Point(16, 41);
            this.chapterLabel.Name = "chapterLabel";
            this.chapterLabel.Size = new System.Drawing.Size(100, 13);
            this.chapterLabel.TabIndex = 36;
            this.chapterLabel.Text = "Chapter file";
            // 
            // workingDirectoryLabel
            // 
            this.workingDirectoryLabel.Location = new System.Drawing.Point(16, 15);
            this.workingDirectoryLabel.Name = "workingDirectoryLabel";
            this.workingDirectoryLabel.Size = new System.Drawing.Size(100, 13);
            this.workingDirectoryLabel.TabIndex = 32;
            this.workingDirectoryLabel.Text = "Working Directory";
            // 
            // workingDirectory
            // 
            this.workingDirectory.Location = new System.Drawing.Point(120, 12);
            this.workingDirectory.Name = "workingDirectory";
            this.workingDirectory.ReadOnly = true;
            this.workingDirectory.Size = new System.Drawing.Size(256, 20);
            this.workingDirectory.TabIndex = 34;
            // 
            // workingDirectoryButton
            // 
            this.workingDirectoryButton.Location = new System.Drawing.Point(384, 12);
            this.workingDirectoryButton.Name = "workingDirectoryButton";
            this.workingDirectoryButton.Size = new System.Drawing.Size(24, 23);
            this.workingDirectoryButton.TabIndex = 33;
            this.workingDirectoryButton.Text = "...";
            this.workingDirectoryButton.Click += new System.EventHandler(this.workingDirectoryButton_Click);
            // 
            // workingName
            // 
            this.workingName.Location = new System.Drawing.Point(120, 64);
            this.workingName.Name = "workingName";
            this.workingName.Size = new System.Drawing.Size(288, 20);
            this.workingName.TabIndex = 30;
            this.workingName.TextChanged += new System.EventHandler(this.workingName_TextChanged);
            // 
            // projectNameLabel
            // 
            this.projectNameLabel.Location = new System.Drawing.Point(16, 67);
            this.projectNameLabel.Name = "projectNameLabel";
            this.projectNameLabel.Size = new System.Drawing.Size(73, 16);
            this.projectNameLabel.TabIndex = 31;
            this.projectNameLabel.Text = "Project Name";
            // 
            // avsBox
            // 
            this.avsBox.Controls.Add(this.autoDeint);
            this.avsBox.Controls.Add(this.avsProfile);
            this.avsBox.Controls.Add(this.avsProfileLabel);
            this.avsBox.Controls.Add(this.signalAR);
            this.avsBox.Controls.Add(this.AR);
            this.avsBox.Controls.Add(this.outputResolutionLabel);
            this.avsBox.Controls.Add(this.horizontalResolution);
            this.avsBox.Controls.Add(this.arComboBox);
            this.avsBox.Controls.Add(this.ARLabel);
            this.avsBox.Location = new System.Drawing.Point(8, 107);
            this.avsBox.Name = "avsBox";
            this.avsBox.Size = new System.Drawing.Size(424, 72);
            this.avsBox.TabIndex = 23;
            this.avsBox.TabStop = false;
            this.avsBox.Text = "AviSynth setup";
            // 
            // autoDeint
            // 
            this.autoDeint.AutoSize = true;
            this.autoDeint.Location = new System.Drawing.Point(270, 44);
            this.autoDeint.Name = "autoDeint";
            this.autoDeint.Size = new System.Drawing.Size(138, 17);
            this.autoDeint.TabIndex = 20;
            this.autoDeint.Text = "Automatic Deinterlacing";
            this.autoDeint.UseVisualStyleBackColor = true;
            // 
            // avsProfile
            // 
            this.avsProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.avsProfile.Location = new System.Drawing.Point(120, 41);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.Size = new System.Drawing.Size(144, 21);
            this.avsProfile.Sorted = true;
            this.avsProfile.TabIndex = 19;
            // 
            // avsProfileLabel
            // 
            this.avsProfileLabel.Location = new System.Drawing.Point(16, 44);
            this.avsProfileLabel.Name = "avsProfileLabel";
            this.avsProfileLabel.Size = new System.Drawing.Size(72, 18);
            this.avsProfileLabel.TabIndex = 18;
            this.avsProfileLabel.Text = "AVS profile";
            // 
            // signalAR
            // 
            this.signalAR.Location = new System.Drawing.Point(342, 16);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(82, 24);
            this.signalAR.TabIndex = 5;
            this.signalAR.Text = "Signal AR";
            // 
            // AR
            // 
            this.AR.Location = new System.Drawing.Point(296, 16);
            this.AR.Name = "AR";
            this.AR.ReadOnly = true;
            this.AR.Size = new System.Drawing.Size(40, 20);
            this.AR.TabIndex = 2;
            this.AR.Text = "1.778";
            // 
            // outputResolutionLabel
            // 
            this.outputResolutionLabel.Location = new System.Drawing.Point(16, 20);
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
            this.horizontalResolution.Size = new System.Drawing.Size(64, 20);
            this.horizontalResolution.TabIndex = 0;
            this.horizontalResolution.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // arComboBox
            // 
            this.arComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.arComboBox.Items.AddRange(new object[] {
            "16:9",
            "4:3",
            "1:1",
            "Custom",
            "Auto-detect later"});
            this.arComboBox.Location = new System.Drawing.Point(224, 16);
            this.arComboBox.Name = "arComboBox";
            this.arComboBox.Size = new System.Drawing.Size(64, 21);
            this.arComboBox.TabIndex = 1;
            this.arComboBox.SelectedIndexChanged += new System.EventHandler(this.arComboBox_SelectedIndexChanged);
            // 
            // ARLabel
            // 
            this.ARLabel.Location = new System.Drawing.Point(192, 20);
            this.ARLabel.Name = "ARLabel";
            this.ARLabel.Size = new System.Drawing.Size(24, 13);
            this.ARLabel.TabIndex = 4;
            this.ARLabel.Text = "AR:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.IOGroupbox);
            this.tabPage1.Controls.Add(this.targetGroupBox);
            this.tabPage1.Controls.Add(this.audioGroupbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(440, 252);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic IO";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // IOGroupbox
            // 
            this.IOGroupbox.Controls.Add(this.selectOutput);
            this.IOGroupbox.Controls.Add(this.output);
            this.IOGroupbox.Controls.Add(this.outputLabel);
            this.IOGroupbox.Controls.Add(this.input);
            this.IOGroupbox.Controls.Add(this.inputLabel);
            this.IOGroupbox.Controls.Add(this.openButton);
            this.IOGroupbox.Location = new System.Drawing.Point(8, 6);
            this.IOGroupbox.Name = "IOGroupbox";
            this.IOGroupbox.Size = new System.Drawing.Size(424, 76);
            this.IOGroupbox.TabIndex = 14;
            this.IOGroupbox.TabStop = false;
            this.IOGroupbox.Text = "Input/Output";
            // 
            // selectOutput
            // 
            this.selectOutput.Location = new System.Drawing.Point(384, 44);
            this.selectOutput.Name = "selectOutput";
            this.selectOutput.Size = new System.Drawing.Size(24, 23);
            this.selectOutput.TabIndex = 0;
            this.selectOutput.Text = "...";
            this.selectOutput.Click += new System.EventHandler(this.selectOutput_Click);
            // 
            // output
            // 
            this.output.Location = new System.Drawing.Point(120, 45);
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.Size = new System.Drawing.Size(256, 20);
            this.output.TabIndex = 1;
            // 
            // outputLabel
            // 
            this.outputLabel.Location = new System.Drawing.Point(16, 48);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(100, 13);
            this.outputLabel.TabIndex = 0;
            this.outputLabel.Text = "Output file";
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(120, 19);
            this.input.Name = "input";
            this.input.ReadOnly = true;
            this.input.Size = new System.Drawing.Size(256, 20);
            this.input.TabIndex = 4;
            // 
            // inputLabel
            // 
            this.inputLabel.Location = new System.Drawing.Point(16, 22);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(100, 13);
            this.inputLabel.TabIndex = 2;
            this.inputLabel.Text = "Input file";
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(384, 19);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(24, 23);
            this.openButton.TabIndex = 3;
            this.openButton.Text = "...";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // targetGroupBox
            // 
            this.targetGroupBox.Controls.Add(this.filesize);
            this.targetGroupBox.Controls.Add(this.inKBLabel);
            this.targetGroupBox.Controls.Add(this.filesizeComboBox);
            this.targetGroupBox.Controls.Add(this.filesizeLabel);
            this.targetGroupBox.Controls.Add(this.playbackMethodLabel);
            this.targetGroupBox.Controls.Add(this.playbackMethod);
            this.targetGroupBox.Location = new System.Drawing.Point(6, 176);
            this.targetGroupBox.Name = "targetGroupBox";
            this.targetGroupBox.Size = new System.Drawing.Size(424, 71);
            this.targetGroupBox.TabIndex = 18;
            this.targetGroupBox.TabStop = false;
            this.targetGroupBox.Text = "Target";
            // 
            // filesize
            // 
            this.filesize.Location = new System.Drawing.Point(298, 41);
            this.filesize.Name = "filesize";
            this.filesize.ReadOnly = true;
            this.filesize.Size = new System.Drawing.Size(112, 20);
            this.filesize.TabIndex = 20;
            // 
            // inKBLabel
            // 
            this.inKBLabel.Location = new System.Drawing.Point(242, 45);
            this.inKBLabel.Name = "inKBLabel";
            this.inKBLabel.Size = new System.Drawing.Size(50, 13);
            this.inKBLabel.TabIndex = 22;
            this.inKBLabel.Text = "In KB:";
            // 
            // filesizeComboBox
            // 
            this.filesizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filesizeComboBox.Location = new System.Drawing.Point(122, 41);
            this.filesizeComboBox.Name = "filesizeComboBox";
            this.filesizeComboBox.Size = new System.Drawing.Size(114, 21);
            this.filesizeComboBox.TabIndex = 19;
            this.filesizeComboBox.SelectedIndexChanged += new System.EventHandler(this.filesizeComboBox_SelectedIndexChanged);
            // 
            // filesizeLabel
            // 
            this.filesizeLabel.Location = new System.Drawing.Point(18, 44);
            this.filesizeLabel.Name = "filesizeLabel";
            this.filesizeLabel.Size = new System.Drawing.Size(90, 13);
            this.filesizeLabel.TabIndex = 21;
            this.filesizeLabel.Text = "Filesize";
            // 
            // playbackMethodLabel
            // 
            this.playbackMethodLabel.Location = new System.Drawing.Point(18, 20);
            this.playbackMethodLabel.Name = "playbackMethodLabel";
            this.playbackMethodLabel.Size = new System.Drawing.Size(100, 13);
            this.playbackMethodLabel.TabIndex = 3;
            this.playbackMethodLabel.Text = "Playback method";
            // 
            // playbackMethod
            // 
            this.playbackMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playbackMethod.Location = new System.Drawing.Point(122, 17);
            this.playbackMethod.Name = "playbackMethod";
            this.playbackMethod.Size = new System.Drawing.Size(288, 21);
            this.playbackMethod.Sorted = true;
            this.playbackMethod.TabIndex = 13;
            this.playbackMethod.SelectedIndexChanged += new System.EventHandler(this.playbackMethod_SelectedIndexChanged);
            // 
            // audioGroupbox
            // 
            this.audioGroupbox.Controls.Add(this.clearAudio2Button);
            this.audioGroupbox.Controls.Add(this.clearAudio1Button);
            this.audioGroupbox.Controls.Add(this.track2Label);
            this.audioGroupbox.Controls.Add(this.track1Label);
            this.audioGroupbox.Controls.Add(this.track2);
            this.audioGroupbox.Controls.Add(this.track1);
            this.audioGroupbox.Location = new System.Drawing.Point(6, 88);
            this.audioGroupbox.Name = "audioGroupbox";
            this.audioGroupbox.Size = new System.Drawing.Size(424, 82);
            this.audioGroupbox.TabIndex = 5;
            this.audioGroupbox.TabStop = false;
            this.audioGroupbox.Text = "Audio";
            // 
            // clearAudio2Button
            // 
            this.clearAudio2Button.Location = new System.Drawing.Point(386, 48);
            this.clearAudio2Button.Name = "clearAudio2Button";
            this.clearAudio2Button.Size = new System.Drawing.Size(24, 23);
            this.clearAudio2Button.TabIndex = 18;
            this.clearAudio2Button.Text = "X";
            this.clearAudio2Button.Click += new System.EventHandler(this.clearAudio2Button_Click);
            // 
            // clearAudio1Button
            // 
            this.clearAudio1Button.Location = new System.Drawing.Point(386, 24);
            this.clearAudio1Button.Name = "clearAudio1Button";
            this.clearAudio1Button.Size = new System.Drawing.Size(24, 23);
            this.clearAudio1Button.TabIndex = 17;
            this.clearAudio1Button.Text = "X";
            this.clearAudio1Button.Click += new System.EventHandler(this.clearAudio1Button_Click);
            // 
            // track2Label
            // 
            this.track2Label.Location = new System.Drawing.Point(18, 51);
            this.track2Label.Name = "track2Label";
            this.track2Label.Size = new System.Drawing.Size(72, 23);
            this.track2Label.TabIndex = 16;
            this.track2Label.Text = "Track 2";
            // 
            // track1Label
            // 
            this.track1Label.Location = new System.Drawing.Point(18, 27);
            this.track1Label.Name = "track1Label";
            this.track1Label.Size = new System.Drawing.Size(72, 23);
            this.track1Label.TabIndex = 15;
            this.track1Label.Text = "Track 1";
            // 
            // track2
            // 
            this.track2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.track2.Location = new System.Drawing.Point(122, 48);
            this.track2.Name = "track2";
            this.track2.Size = new System.Drawing.Size(256, 21);
            this.track2.TabIndex = 14;
            this.track2.SelectedIndexChanged += new System.EventHandler(this.track1_SelectedIndexChanged);
            // 
            // track1
            // 
            this.track1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.track1.Location = new System.Drawing.Point(122, 24);
            this.track1.Name = "track1";
            this.track1.Size = new System.Drawing.Size(256, 21);
            this.track1.TabIndex = 13;
            this.track1.SelectedIndexChanged += new System.EventHandler(this.track1_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.encoderConfigTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(448, 278);
            this.tabControl1.TabIndex = 0;
            // 
            // encoderConfigTab
            // 
            this.encoderConfigTab.Controls.Add(this.audioIOGroupBox);
            this.encoderConfigTab.Controls.Add(this.videoGroupBox);
            this.encoderConfigTab.Controls.Add(this.splitOutput);
            this.encoderConfigTab.Controls.Add(this.splitSize);
            this.encoderConfigTab.Controls.Add(this.containerFormatLabel);
            this.encoderConfigTab.Controls.Add(this.containerFormat);
            this.encoderConfigTab.Location = new System.Drawing.Point(4, 22);
            this.encoderConfigTab.Name = "encoderConfigTab";
            this.encoderConfigTab.Padding = new System.Windows.Forms.Padding(3);
            this.encoderConfigTab.Size = new System.Drawing.Size(440, 252);
            this.encoderConfigTab.TabIndex = 2;
            this.encoderConfigTab.Text = "Encoder Config";
            this.encoderConfigTab.UseVisualStyleBackColor = true;
            // 
            // audioIOGroupBox
            // 
            this.audioIOGroupBox.Controls.Add(this.externalInput);
            this.audioIOGroupBox.Controls.Add(this.audioCodecLabel);
            this.audioIOGroupBox.Controls.Add(this.audioProfile);
            this.audioIOGroupBox.Controls.Add(this.dontEncodeAudio);
            this.audioIOGroupBox.Controls.Add(this.audioProfileLabel);
            this.audioIOGroupBox.Controls.Add(this.audioInput);
            this.audioIOGroupBox.Controls.Add(this.audioInputOpenButton);
            this.audioIOGroupBox.Controls.Add(this.audioTrack2);
            this.audioIOGroupBox.Controls.Add(this.audioTrack1);
            this.audioIOGroupBox.Controls.Add(this.deleteAudioButton);
            this.audioIOGroupBox.Controls.Add(this.configAudioButton);
            this.audioIOGroupBox.Controls.Add(this.audioCodec);
            this.audioIOGroupBox.Location = new System.Drawing.Point(6, 89);
            this.audioIOGroupBox.Name = "audioIOGroupBox";
            this.audioIOGroupBox.Size = new System.Drawing.Size(426, 104);
            this.audioIOGroupBox.TabIndex = 32;
            this.audioIOGroupBox.TabStop = false;
            this.audioIOGroupBox.Text = "Audio";
            // 
            // externalInput
            // 
            this.externalInput.AutoSize = true;
            this.externalInput.Location = new System.Drawing.Point(9, 21);
            this.externalInput.Name = "externalInput";
            this.externalInput.Size = new System.Drawing.Size(90, 17);
            this.externalInput.TabIndex = 32;
            this.externalInput.Text = "External input";
            this.externalInput.UseVisualStyleBackColor = true;
            this.externalInput.CheckedChanged += new System.EventHandler(this.externalInput_CheckedChanged);
            // 
            // audioCodecLabel
            // 
            this.audioCodecLabel.AutoSize = true;
            this.audioCodecLabel.Location = new System.Drawing.Point(6, 50);
            this.audioCodecLabel.Name = "audioCodecLabel";
            this.audioCodecLabel.Size = new System.Drawing.Size(38, 13);
            this.audioCodecLabel.TabIndex = 31;
            this.audioCodecLabel.Text = "Codec";
            this.audioCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // audioProfile
            // 
            this.audioProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioProfile.Location = new System.Drawing.Point(119, 71);
            this.audioProfile.Name = "audioProfile";
            this.audioProfile.Size = new System.Drawing.Size(159, 21);
            this.audioProfile.Sorted = true;
            this.audioProfile.TabIndex = 15;
            this.audioProfile.SelectedIndexChanged += new System.EventHandler(this.audioProfile_SelectedIndexChanged_1);
            // 
            // dontEncodeAudio
            // 
            this.dontEncodeAudio.Location = new System.Drawing.Point(256, 47);
            this.dontEncodeAudio.Name = "dontEncodeAudio";
            this.dontEncodeAudio.Size = new System.Drawing.Size(122, 21);
            this.dontEncodeAudio.TabIndex = 30;
            this.dontEncodeAudio.Text = "Keep original track";
            this.dontEncodeAudio.CheckedChanged += new System.EventHandler(this.dontEncodeAudio_CheckedChanged);
            // 
            // audioProfileLabel
            // 
            this.audioProfileLabel.AutoSize = true;
            this.audioProfileLabel.Location = new System.Drawing.Point(6, 74);
            this.audioProfileLabel.Name = "audioProfileLabel";
            this.audioProfileLabel.Size = new System.Drawing.Size(66, 13);
            this.audioProfileLabel.TabIndex = 14;
            this.audioProfileLabel.Text = "Audio Profile";
            // 
            // audioInput
            // 
            this.audioInput.Location = new System.Drawing.Point(119, 19);
            this.audioInput.Name = "audioInput";
            this.audioInput.ReadOnly = true;
            this.audioInput.Size = new System.Drawing.Size(256, 20);
            this.audioInput.TabIndex = 6;
            // 
            // audioInputOpenButton
            // 
            this.audioInputOpenButton.Location = new System.Drawing.Point(383, 19);
            this.audioInputOpenButton.Name = "audioInputOpenButton";
            this.audioInputOpenButton.Size = new System.Drawing.Size(24, 23);
            this.audioInputOpenButton.TabIndex = 7;
            this.audioInputOpenButton.Text = "...";
            this.audioInputOpenButton.Visible = false;
            this.audioInputOpenButton.Click += new System.EventHandler(this.audioInputOpenButton_Click);
            // 
            // audioTrack2
            // 
            this.audioTrack2.Location = new System.Drawing.Point(75, 0);
            this.audioTrack2.Name = "audioTrack2";
            this.audioTrack2.Size = new System.Drawing.Size(24, 16);
            this.audioTrack2.TabIndex = 20;
            this.audioTrack2.Text = "2";
            this.audioTrack2.CheckedChanged += new System.EventHandler(this.audioTrack1_CheckedChanged);
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
            this.audioTrack1.CheckedChanged += new System.EventHandler(this.audioTrack1_CheckedChanged);
            // 
            // deleteAudioButton
            // 
            this.deleteAudioButton.Location = new System.Drawing.Point(383, 71);
            this.deleteAudioButton.Name = "deleteAudioButton";
            this.deleteAudioButton.Size = new System.Drawing.Size(24, 23);
            this.deleteAudioButton.TabIndex = 6;
            this.deleteAudioButton.Text = "X";
            this.deleteAudioButton.Click += new System.EventHandler(this.deleteAudioButton_Click);
            // 
            // configAudioButton
            // 
            this.configAudioButton.Location = new System.Drawing.Point(194, 45);
            this.configAudioButton.Name = "configAudioButton";
            this.configAudioButton.Size = new System.Drawing.Size(56, 23);
            this.configAudioButton.TabIndex = 26;
            this.configAudioButton.Text = "Config";
            this.configAudioButton.Click += new System.EventHandler(this.configAudioButton_Click);
            // 
            // audioCodec
            // 
            this.audioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioCodec.Location = new System.Drawing.Point(119, 45);
            this.audioCodec.Name = "audioCodec";
            this.audioCodec.Size = new System.Drawing.Size(70, 21);
            this.audioCodec.TabIndex = 7;
            // 
            // videoGroupBox
            // 
            this.videoGroupBox.Controls.Add(this.addPrerenderJob);
            this.videoGroupBox.Controls.Add(this.videoCodecLabel);
            this.videoGroupBox.Controls.Add(this.videoCodec);
            this.videoGroupBox.Controls.Add(this.videoProfileLabel);
            this.videoGroupBox.Controls.Add(this.videoProfile);
            this.videoGroupBox.Controls.Add(this.videoConfigButton);
            this.videoGroupBox.Location = new System.Drawing.Point(3, 6);
            this.videoGroupBox.Name = "videoGroupBox";
            this.videoGroupBox.Size = new System.Drawing.Size(429, 77);
            this.videoGroupBox.TabIndex = 31;
            this.videoGroupBox.TabStop = false;
            this.videoGroupBox.Text = "Video";
            // 
            // addPrerenderJob
            // 
            this.addPrerenderJob.AutoSize = true;
            this.addPrerenderJob.Location = new System.Drawing.Point(259, 21);
            this.addPrerenderJob.Name = "addPrerenderJob";
            this.addPrerenderJob.Size = new System.Drawing.Size(127, 17);
            this.addPrerenderJob.TabIndex = 16;
            this.addPrerenderJob.Text = "Add pre-rendering job";
            this.addPrerenderJob.UseVisualStyleBackColor = true;
            // 
            // videoCodecLabel
            // 
            this.videoCodecLabel.Location = new System.Drawing.Point(9, 18);
            this.videoCodecLabel.Name = "videoCodecLabel";
            this.videoCodecLabel.Size = new System.Drawing.Size(100, 23);
            this.videoCodecLabel.TabIndex = 7;
            this.videoCodecLabel.Text = "Codec";
            this.videoCodecLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // videoCodec
            // 
            this.videoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoCodec.Location = new System.Drawing.Point(122, 17);
            this.videoCodec.Name = "videoCodec";
            this.videoCodec.Size = new System.Drawing.Size(70, 21);
            this.videoCodec.TabIndex = 8;
            // 
            // videoProfileLabel
            // 
            this.videoProfileLabel.Location = new System.Drawing.Point(9, 47);
            this.videoProfileLabel.Name = "videoProfileLabel";
            this.videoProfileLabel.Size = new System.Drawing.Size(92, 23);
            this.videoProfileLabel.TabIndex = 7;
            this.videoProfileLabel.Text = "Video Profile";
            // 
            // videoProfile
            // 
            this.videoProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoProfile.Location = new System.Drawing.Point(122, 46);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.Size = new System.Drawing.Size(290, 21);
            this.videoProfile.Sorted = true;
            this.videoProfile.TabIndex = 8;
            this.videoProfile.SelectedIndexChanged += new System.EventHandler(this.videoProfile_SelectedIndexChanged_1);
            // 
            // videoConfigButton
            // 
            this.videoConfigButton.Location = new System.Drawing.Point(197, 17);
            this.videoConfigButton.Name = "videoConfigButton";
            this.videoConfigButton.Size = new System.Drawing.Size(56, 23);
            this.videoConfigButton.TabIndex = 14;
            this.videoConfigButton.Text = "Config";
            this.videoConfigButton.Click += new System.EventHandler(this.videoConfigButton_Click);
            // 
            // splitOutput
            // 
            this.splitOutput.AutoSize = true;
            this.splitOutput.Location = new System.Drawing.Point(247, 199);
            this.splitOutput.Name = "splitOutput";
            this.splitOutput.Size = new System.Drawing.Size(69, 17);
            this.splitOutput.TabIndex = 29;
            this.splitOutput.Text = "Split Size";
            // 
            // splitSize
            // 
            this.splitSize.Enabled = false;
            this.splitSize.Location = new System.Drawing.Point(351, 197);
            this.splitSize.Name = "splitSize";
            this.splitSize.Size = new System.Drawing.Size(64, 20);
            this.splitSize.TabIndex = 28;
            this.splitSize.Text = "0";
            // 
            // containerFormatLabel
            // 
            this.containerFormatLabel.Location = new System.Drawing.Point(21, 204);
            this.containerFormatLabel.Name = "containerFormatLabel";
            this.containerFormatLabel.Size = new System.Drawing.Size(92, 13);
            this.containerFormatLabel.TabIndex = 27;
            this.containerFormatLabel.Text = "Container Format";
            // 
            // containerFormat
            // 
            this.containerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containerFormat.Location = new System.Drawing.Point(125, 199);
            this.containerFormat.Name = "containerFormat";
            this.containerFormat.Size = new System.Drawing.Size(116, 21);
            this.containerFormat.TabIndex = 26;
            this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.containerFormat_SelectedIndexChanged_1);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "IFO Files|*.ifo|VOB Files (*.vob)|*.vob|MPEG-1/2 Program Streams (*.mpg)|*.mpg|Tr" +
                "ansport Streams (*.ts)|*.ts";
            // 
            // outputDialog
            // 
            this.outputDialog.Filter = "MP4 Files|*.mp4";
            // 
            // showAdvancedOptions
            // 
            this.showAdvancedOptions.AutoSize = true;
            this.showAdvancedOptions.Location = new System.Drawing.Point(4, 306);
            this.showAdvancedOptions.Name = "showAdvancedOptions";
            this.showAdvancedOptions.Size = new System.Drawing.Size(144, 17);
            this.showAdvancedOptions.TabIndex = 31;
            this.showAdvancedOptions.Text = "Show Advanced Options";
            this.showAdvancedOptions.UseVisualStyleBackColor = true;
            this.showAdvancedOptions.CheckedChanged += new System.EventHandler(this.showAdvancedOptions_CheckedChanged);
            // 
            // shutdownCheckBox
            // 
            this.shutdownCheckBox.AutoSize = true;
            this.shutdownCheckBox.Location = new System.Drawing.Point(4, 283);
            this.shutdownCheckBox.Name = "shutdownCheckBox";
            this.shutdownCheckBox.Size = new System.Drawing.Size(145, 17);
            this.shutdownCheckBox.TabIndex = 30;
            this.shutdownCheckBox.Text = "Shutdown after encoding";
            this.shutdownCheckBox.UseVisualStyleBackColor = true;
            this.shutdownCheckBox.CheckedChanged += new System.EventHandler(this.shutdownCheckBox_CheckedChanged);
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(351, 283);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 29;
            this.goButton.Text = "Go!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // OneClickWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 335);
            this.Controls.Add(this.showAdvancedOptions);
            this.Controls.Add(this.shutdownCheckBox);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OneClickWindow";
            this.Text = "One Click Encoder";
            this.tabPage2.ResumeLayout(false);
            this.locationGroupBox.ResumeLayout(false);
            this.locationGroupBox.PerformLayout();
            this.avsBox.ResumeLayout(false);
            this.avsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.IOGroupbox.ResumeLayout(false);
            this.IOGroupbox.PerformLayout();
            this.targetGroupBox.ResumeLayout(false);
            this.targetGroupBox.PerformLayout();
            this.audioGroupbox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.encoderConfigTab.ResumeLayout(false);
            this.encoderConfigTab.PerformLayout();
            this.audioIOGroupBox.ResumeLayout(false);
            this.audioIOGroupBox.PerformLayout();
            this.videoGroupBox.ResumeLayout(false);
            this.videoGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox audioGroupbox;
        private System.Windows.Forms.Button clearAudio2Button;
        private System.Windows.Forms.Button clearAudio1Button;
        private System.Windows.Forms.Label track2Label;
        private System.Windows.Forms.Label track1Label;
        private System.Windows.Forms.ComboBox track2;
        private System.Windows.Forms.ComboBox track1;
        private System.Windows.Forms.GroupBox IOGroupbox;
        private System.Windows.Forms.Button selectOutput;
        private System.Windows.Forms.TextBox output;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.GroupBox avsBox;
        private System.Windows.Forms.CheckBox signalAR;
        private System.Windows.Forms.TextBox AR;
        private System.Windows.Forms.Label outputResolutionLabel;
        private System.Windows.Forms.NumericUpDown horizontalResolution;
        private System.Windows.Forms.ComboBox arComboBox;
        private System.Windows.Forms.Label ARLabel;
        private System.Windows.Forms.GroupBox locationGroupBox;
        private System.Windows.Forms.Label workingDirectoryLabel;
        private System.Windows.Forms.TextBox workingDirectory;
        private System.Windows.Forms.Button workingDirectoryButton;
        private System.Windows.Forms.TextBox workingName;
        private System.Windows.Forms.Label projectNameLabel;
        private System.Windows.Forms.GroupBox targetGroupBox;
        private System.Windows.Forms.Label playbackMethodLabel;
        private System.Windows.Forms.ComboBox playbackMethod;
        private System.Windows.Forms.TextBox filesize;
        private System.Windows.Forms.Label inKBLabel;
        private System.Windows.Forms.ComboBox filesizeComboBox;
        private System.Windows.Forms.Label filesizeLabel;
        private System.Windows.Forms.Button chapterFileOpen;
        private System.Windows.Forms.TextBox chapterFile;
        private System.Windows.Forms.Label chapterLabel;
        private System.Windows.Forms.CheckBox autoDeint;
        private System.Windows.Forms.ComboBox avsProfile;
        private System.Windows.Forms.Label avsProfileLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog outputDialog;
        private System.Windows.Forms.TabPage encoderConfigTab;
        private System.Windows.Forms.CheckBox dontEncodeAudio;
        private System.Windows.Forms.CheckBox splitOutput;
        private System.Windows.Forms.TextBox splitSize;
        private System.Windows.Forms.Label containerFormatLabel;
        private System.Windows.Forms.ComboBox containerFormat;
        private System.Windows.Forms.GroupBox videoGroupBox;
        private System.Windows.Forms.CheckBox addPrerenderJob;
        private System.Windows.Forms.Label videoCodecLabel;
        private System.Windows.Forms.ComboBox videoCodec;
        private System.Windows.Forms.Label videoProfileLabel;
        private System.Windows.Forms.ComboBox videoProfile;
        private System.Windows.Forms.Button videoConfigButton;
        private System.Windows.Forms.GroupBox audioIOGroupBox;
        private System.Windows.Forms.Label audioCodecLabel;
        private System.Windows.Forms.ComboBox audioProfile;
        private System.Windows.Forms.Label audioProfileLabel;
        private System.Windows.Forms.TextBox audioInput;
        private System.Windows.Forms.Button audioInputOpenButton;
        private System.Windows.Forms.RadioButton audioTrack2;
        private System.Windows.Forms.RadioButton audioTrack1;
        private System.Windows.Forms.Button deleteAudioButton;
        private System.Windows.Forms.Button configAudioButton;
        private System.Windows.Forms.ComboBox audioCodec;
        private System.Windows.Forms.CheckBox externalInput;
        private System.Windows.Forms.CheckBox showAdvancedOptions;
        private System.Windows.Forms.CheckBox shutdownCheckBox;
        private System.Windows.Forms.Button goButton;


    }
}