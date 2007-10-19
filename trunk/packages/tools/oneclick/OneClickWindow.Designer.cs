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
            System.Windows.Forms.TabPage trackTabPage2;
            System.Windows.Forms.Label label1;
            this.audio2 = new MeGUI.packages.tools.oneclick.AudioConfigControl();
            this.trackTabPage1 = new System.Windows.Forms.TabPage();
            this.audio1 = new MeGUI.packages.tools.oneclick.AudioConfigControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitting = new MeGUI.core.gui.TargetSizeSCBox();
            this.locationGroupBox = new System.Windows.Forms.GroupBox();
            this.chapterFile = new MeGUI.FileBar();
            this.workingDirectory = new MeGUI.FileBar();
            this.chapterLabel = new System.Windows.Forms.Label();
            this.workingDirectoryLabel = new System.Windows.Forms.Label();
            this.workingName = new System.Windows.Forms.TextBox();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.avsBox = new System.Windows.Forms.GroupBox();
            this.ar = new MeGUI.core.gui.ARChooser();
            this.profileControl1 = new MeGUI.core.details.video.ProfileControl();
            this.autoDeint = new System.Windows.Forms.CheckBox();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.outputResolutionLabel = new System.Windows.Forms.Label();
            this.horizontalResolution = new System.Windows.Forms.NumericUpDown();
            this.ARLabel = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.IOGroupbox = new System.Windows.Forms.GroupBox();
            this.output = new MeGUI.FileBar();
            this.input = new MeGUI.FileBar();
            this.outputLabel = new System.Windows.Forms.Label();
            this.inputLabel = new System.Windows.Forms.Label();
            this.targetGroupBox = new System.Windows.Forms.GroupBox();
            this.optionalTargetSizeBox1 = new MeGUI.core.gui.TargetSizeSCBox();
            this.profileControl2 = new MeGUI.core.details.video.ProfileControl();
            this.filesizeLabel = new System.Windows.Forms.Label();
            this.audioGroupbox = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.audioTrack1 = new MeGUI.core.gui.FileSCBox();
            this.track2Label = new System.Windows.Forms.Label();
            this.audioTrack2 = new MeGUI.core.gui.FileSCBox();
            this.track1Label = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.encoderConfigTab = new System.Windows.Forms.TabPage();
            this.containerFormatLabel = new System.Windows.Forms.Label();
            this.containerFormat = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.videoGroupBox = new System.Windows.Forms.GroupBox();
            this.videoProfileControl = new MeGUI.core.details.video.ProfileControl();
            this.addPrerenderJob = new System.Windows.Forms.CheckBox();
            this.videoCodecLabel = new System.Windows.Forms.Label();
            this.videoCodec = new System.Windows.Forms.ComboBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.showAdvancedOptions = new System.Windows.Forms.CheckBox();
            this.goButton = new System.Windows.Forms.Button();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            trackTabPage2 = new System.Windows.Forms.TabPage();
            label1 = new System.Windows.Forms.Label();
            trackTabPage2.SuspendLayout();
            this.trackTabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.locationGroupBox.SuspendLayout();
            this.avsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.IOGroupbox.SuspendLayout();
            this.targetGroupBox.SuspendLayout();
            this.audioGroupbox.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.encoderConfigTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.videoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackTabPage2
            // 
            trackTabPage2.Controls.Add(this.audio2);
            trackTabPage2.Location = new System.Drawing.Point(4, 22);
            trackTabPage2.Name = "trackTabPage2";
            trackTabPage2.Padding = new System.Windows.Forms.Padding(3);
            trackTabPage2.Size = new System.Drawing.Size(423, 94);
            trackTabPage2.TabIndex = 1;
            trackTabPage2.Text = "Audio track 2";
            trackTabPage2.UseVisualStyleBackColor = true;
            // 
            // audio2
            // 
            this.audio2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audio2.Location = new System.Drawing.Point(3, 3);
            this.audio2.Name = "audio2";
            this.audio2.Size = new System.Drawing.Size(417, 88);
            this.audio2.TabIndex = 0;
            this.audio2.SomethingChanged += new System.EventHandler(this.audio1_SomethingChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(24, 245);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(47, 13);
            label1.TabIndex = 37;
            label1.Text = "Splitting:";
            // 
            // trackTabPage1
            // 
            this.trackTabPage1.Controls.Add(this.audio1);
            this.trackTabPage1.Location = new System.Drawing.Point(4, 22);
            this.trackTabPage1.Name = "trackTabPage1";
            this.trackTabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.trackTabPage1.Size = new System.Drawing.Size(423, 94);
            this.trackTabPage1.TabIndex = 0;
            this.trackTabPage1.Text = "Audio track 1";
            this.trackTabPage1.UseVisualStyleBackColor = true;
            // 
            // audio1
            // 
            this.audio1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audio1.Location = new System.Drawing.Point(3, 3);
            this.audio1.Name = "audio1";
            this.audio1.Size = new System.Drawing.Size(417, 88);
            this.audio1.TabIndex = 0;
            this.audio1.SomethingChanged += new System.EventHandler(this.audio1_SomethingChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(label1);
            this.tabPage2.Controls.Add(this.splitting);
            this.tabPage2.Controls.Add(this.locationGroupBox);
            this.tabPage2.Controls.Add(this.avsBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(453, 272);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced Config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitting
            // 
            this.splitting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitting.Location = new System.Drawing.Point(128, 237);
            this.splitting.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitting.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitting.Name = "splitting";
            this.splitting.NullString = "No splitting";
            this.splitting.SelectedIndex = 0;
            this.splitting.Size = new System.Drawing.Size(221, 29);
            this.splitting.TabIndex = 38;
            // 
            // locationGroupBox
            // 
            this.locationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.locationGroupBox.Controls.Add(this.chapterFile);
            this.locationGroupBox.Controls.Add(this.workingDirectory);
            this.locationGroupBox.Controls.Add(this.chapterLabel);
            this.locationGroupBox.Controls.Add(this.workingDirectoryLabel);
            this.locationGroupBox.Controls.Add(this.workingName);
            this.locationGroupBox.Controls.Add(this.projectNameLabel);
            this.locationGroupBox.Location = new System.Drawing.Point(8, 6);
            this.locationGroupBox.Name = "locationGroupBox";
            this.locationGroupBox.Size = new System.Drawing.Size(437, 95);
            this.locationGroupBox.TabIndex = 23;
            this.locationGroupBox.TabStop = false;
            this.locationGroupBox.Text = "Extra IO";
            // 
            // chapterFile
            // 
            this.chapterFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chapterFile.Filename = "";
            this.chapterFile.Filter = "Chapter files (*.txt)|*.txt";
            this.chapterFile.FolderMode = false;
            this.chapterFile.Location = new System.Drawing.Point(120, 38);
            this.chapterFile.Name = "chapterFile";
            this.chapterFile.ReadOnly = true;
            this.chapterFile.SaveMode = false;
            this.chapterFile.Size = new System.Drawing.Size(301, 26);
            this.chapterFile.TabIndex = 39;
            this.chapterFile.Title = null;
            // 
            // workingDirectory
            // 
            this.workingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.workingDirectory.Filename = "";
            this.workingDirectory.Filter = null;
            this.workingDirectory.FolderMode = true;
            this.workingDirectory.Location = new System.Drawing.Point(120, 8);
            this.workingDirectory.Name = "workingDirectory";
            this.workingDirectory.ReadOnly = true;
            this.workingDirectory.SaveMode = false;
            this.workingDirectory.Size = new System.Drawing.Size(301, 26);
            this.workingDirectory.TabIndex = 38;
            this.workingDirectory.Title = null;
            this.workingDirectory.FileSelected += new MeGUI.FileBarEventHandler(this.workingDirectory_FileSelected);
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
            // workingName
            // 
            this.workingName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.workingName.Location = new System.Drawing.Point(120, 64);
            this.workingName.Name = "workingName";
            this.workingName.Size = new System.Drawing.Size(301, 20);
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
            this.avsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.avsBox.Controls.Add(this.ar);
            this.avsBox.Controls.Add(this.profileControl1);
            this.avsBox.Controls.Add(this.autoDeint);
            this.avsBox.Controls.Add(this.signalAR);
            this.avsBox.Controls.Add(this.outputResolutionLabel);
            this.avsBox.Controls.Add(this.horizontalResolution);
            this.avsBox.Controls.Add(this.ARLabel);
            this.avsBox.Location = new System.Drawing.Point(8, 107);
            this.avsBox.Name = "avsBox";
            this.avsBox.Size = new System.Drawing.Size(437, 124);
            this.avsBox.TabIndex = 23;
            this.avsBox.TabStop = false;
            this.avsBox.Text = "AviSynth setup";
            // 
            // ar
            // 
            this.ar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ar.HasLater = true;
            this.ar.Location = new System.Drawing.Point(120, 42);
            this.ar.MaximumSize = new System.Drawing.Size(1000, 29);
            this.ar.MinimumSize = new System.Drawing.Size(64, 29);
            this.ar.Name = "ar";
            this.ar.SelectedIndex = 0;
            this.ar.Size = new System.Drawing.Size(213, 29);
            this.ar.TabIndex = 22;
            // 
            // profileControl1
            // 
            this.profileControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profileControl1.LabelText = "AVS profile";
            this.profileControl1.Location = new System.Drawing.Point(19, 69);
            this.profileControl1.Name = "profileControl1";
            this.profileControl1.Size = new System.Drawing.Size(402, 29);
            this.profileControl1.TabIndex = 21;
            // 
            // autoDeint
            // 
            this.autoDeint.AutoSize = true;
            this.autoDeint.Location = new System.Drawing.Point(123, 101);
            this.autoDeint.Name = "autoDeint";
            this.autoDeint.Size = new System.Drawing.Size(138, 17);
            this.autoDeint.TabIndex = 20;
            this.autoDeint.Text = "Automatic Deinterlacing";
            this.autoDeint.UseVisualStyleBackColor = true;
            // 
            // signalAR
            // 
            this.signalAR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signalAR.Location = new System.Drawing.Point(339, 43);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(82, 24);
            this.signalAR.TabIndex = 5;
            this.signalAR.Text = "Signal AR";
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
            // ARLabel
            // 
            this.ARLabel.Location = new System.Drawing.Point(16, 48);
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
            this.tabPage1.Size = new System.Drawing.Size(453, 272);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic IO";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // IOGroupbox
            // 
            this.IOGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.IOGroupbox.Controls.Add(this.output);
            this.IOGroupbox.Controls.Add(this.input);
            this.IOGroupbox.Controls.Add(this.outputLabel);
            this.IOGroupbox.Controls.Add(this.inputLabel);
            this.IOGroupbox.Location = new System.Drawing.Point(8, 6);
            this.IOGroupbox.Name = "IOGroupbox";
            this.IOGroupbox.Size = new System.Drawing.Size(437, 76);
            this.IOGroupbox.TabIndex = 14;
            this.IOGroupbox.TabStop = false;
            this.IOGroupbox.Text = "Input/Output";
            // 
            // output
            // 
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.output.Filename = "";
            this.output.Filter = "MP4 Files|*.mp4";
            this.output.FolderMode = false;
            this.output.Location = new System.Drawing.Point(123, 42);
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.SaveMode = true;
            this.output.Size = new System.Drawing.Size(298, 26);
            this.output.TabIndex = 4;
            this.output.Title = null;
            this.output.FileSelected += new MeGUI.FileBarEventHandler(this.output_FileSelected);
            // 
            // input
            // 
            this.input.AllowDrop = true;
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.input.Filename = "";
            this.input.Filter = "VOB Files (*.vob)|*.vob|MPEG-1/2 Program Streams (*.mpg)|*.mpg|Transport Streams " +
                "(*.m2ts,*.ts)|*.m2ts;*.ts|All DGIndex supported files|*.vob;*.mpg;*.mpeg;*.m2ts;" +
                "*.m2v;*.mpv;*.tp;*.ts;*.trp;*.pva;*.vro";
            this.input.FolderMode = false;
            this.input.Location = new System.Drawing.Point(123, 13);
            this.input.Name = "input";
            this.input.ReadOnly = true;
            this.input.SaveMode = false;
            this.input.Size = new System.Drawing.Size(298, 26);
            this.input.TabIndex = 3;
            this.input.Title = null;
            this.input.FileSelected += new MeGUI.FileBarEventHandler(this.input_FileSelected);
            // 
            // outputLabel
            // 
            this.outputLabel.Location = new System.Drawing.Point(16, 48);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(100, 13);
            this.outputLabel.TabIndex = 0;
            this.outputLabel.Text = "Output file";
            // 
            // inputLabel
            // 
            this.inputLabel.Location = new System.Drawing.Point(16, 22);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(100, 13);
            this.inputLabel.TabIndex = 2;
            this.inputLabel.Text = "Input file";
            // 
            // targetGroupBox
            // 
            this.targetGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.targetGroupBox.Controls.Add(this.optionalTargetSizeBox1);
            this.targetGroupBox.Controls.Add(this.profileControl2);
            this.targetGroupBox.Controls.Add(this.filesizeLabel);
            this.targetGroupBox.Location = new System.Drawing.Point(8, 184);
            this.targetGroupBox.Name = "targetGroupBox";
            this.targetGroupBox.Size = new System.Drawing.Size(437, 82);
            this.targetGroupBox.TabIndex = 18;
            this.targetGroupBox.TabStop = false;
            this.targetGroupBox.Text = "Target";
            this.targetGroupBox.Enter += new System.EventHandler(this.targetGroupBox_Enter);
            // 
            // optionalTargetSizeBox1
            // 
            this.optionalTargetSizeBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionalTargetSizeBox1.Location = new System.Drawing.Point(125, 43);
            this.optionalTargetSizeBox1.MaximumSize = new System.Drawing.Size(1000, 29);
            this.optionalTargetSizeBox1.MinimumSize = new System.Drawing.Size(64, 29);
            this.optionalTargetSizeBox1.Name = "optionalTargetSizeBox1";
            this.optionalTargetSizeBox1.NullString = "Don\'t Care";
            this.optionalTargetSizeBox1.SelectedIndex = 0;
            this.optionalTargetSizeBox1.Size = new System.Drawing.Size(298, 29);
            this.optionalTargetSizeBox1.TabIndex = 24;
            // 
            // profileControl2
            // 
            this.profileControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profileControl2.LabelText = "One Click profile";
            this.profileControl2.Location = new System.Drawing.Point(21, 12);
            this.profileControl2.Name = "profileControl2";
            this.profileControl2.Size = new System.Drawing.Size(402, 29);
            this.profileControl2.TabIndex = 23;
            // 
            // filesizeLabel
            // 
            this.filesizeLabel.Location = new System.Drawing.Point(18, 51);
            this.filesizeLabel.Name = "filesizeLabel";
            this.filesizeLabel.Size = new System.Drawing.Size(90, 13);
            this.filesizeLabel.TabIndex = 21;
            this.filesizeLabel.Text = "Filesize";
            // 
            // audioGroupbox
            // 
            this.audioGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioGroupbox.ContextMenuStrip = this.contextMenuStrip1;
            this.audioGroupbox.Controls.Add(this.panel1);
            this.audioGroupbox.Location = new System.Drawing.Point(8, 88);
            this.audioGroupbox.Name = "audioGroupbox";
            this.audioGroupbox.Size = new System.Drawing.Size(437, 91);
            this.audioGroupbox.TabIndex = 5;
            this.audioGroupbox.TabStop = false;
            this.audioGroupbox.Text = "Audio";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTrackToolStripMenuItem,
            this.removeTrackToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addTrackToolStripMenuItem
            // 
            this.addTrackToolStripMenuItem.Name = "addTrackToolStripMenuItem";
            this.addTrackToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.addTrackToolStripMenuItem.Text = "Add track";
            this.addTrackToolStripMenuItem.Click += new System.EventHandler(this.addTrackToolStripMenuItem_Click);
            // 
            // removeTrackToolStripMenuItem
            // 
            this.removeTrackToolStripMenuItem.Name = "removeTrackToolStripMenuItem";
            this.removeTrackToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.removeTrackToolStripMenuItem.Text = "Remove track";
            this.removeTrackToolStripMenuItem.Click += new System.EventHandler(this.removeTrackToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.audioTrack1);
            this.panel1.Controls.Add(this.track2Label);
            this.panel1.Controls.Add(this.audioTrack2);
            this.panel1.Controls.Add(this.track1Label);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 72);
            this.panel1.TabIndex = 21;
            // 
            // audioTrack1
            // 
            this.audioTrack1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioTrack1.Filter = "All files (*.*)|*.*";
            this.audioTrack1.Location = new System.Drawing.Point(120, 3);
            this.audioTrack1.MaximumSize = new System.Drawing.Size(1000, 29);
            this.audioTrack1.MinimumSize = new System.Drawing.Size(64, 29);
            this.audioTrack1.Name = "audioTrack1";
            this.audioTrack1.SelectedIndex = 0;
            this.audioTrack1.Size = new System.Drawing.Size(298, 29);
            this.audioTrack1.TabIndex = 19;
            this.audioTrack1.SelectionChanged += new MeGUI.StringChanged(this.audioTrack1_SelectionChanged);
            // 
            // track2Label
            // 
            this.track2Label.AutoSize = true;
            this.track2Label.Location = new System.Drawing.Point(13, 41);
            this.track2Label.Name = "track2Label";
            this.track2Label.Size = new System.Drawing.Size(44, 13);
            this.track2Label.TabIndex = 16;
            this.track2Label.Text = "Track 2";
            // 
            // audioTrack2
            // 
            this.audioTrack2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioTrack2.Filter = "All files (*.*)|*.*";
            this.audioTrack2.Location = new System.Drawing.Point(120, 38);
            this.audioTrack2.MaximumSize = new System.Drawing.Size(1000, 29);
            this.audioTrack2.MinimumSize = new System.Drawing.Size(64, 29);
            this.audioTrack2.Name = "audioTrack2";
            this.audioTrack2.SelectedIndex = 0;
            this.audioTrack2.Size = new System.Drawing.Size(298, 29);
            this.audioTrack2.TabIndex = 20;
            this.audioTrack2.SelectionChanged += new MeGUI.StringChanged(this.audioTrack1_SelectionChanged);
            // 
            // track1Label
            // 
            this.track1Label.AutoSize = true;
            this.track1Label.Location = new System.Drawing.Point(13, 11);
            this.track1Label.Name = "track1Label";
            this.track1Label.Size = new System.Drawing.Size(44, 13);
            this.track1Label.TabIndex = 15;
            this.track1Label.Text = "Track 1";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.encoderConfigTab);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(461, 298);
            this.tabControl1.TabIndex = 0;
            // 
            // encoderConfigTab
            // 
            this.encoderConfigTab.Controls.Add(this.containerFormatLabel);
            this.encoderConfigTab.Controls.Add(this.containerFormat);
            this.encoderConfigTab.Controls.Add(this.groupBox1);
            this.encoderConfigTab.Controls.Add(this.videoGroupBox);
            this.encoderConfigTab.Location = new System.Drawing.Point(4, 22);
            this.encoderConfigTab.Name = "encoderConfigTab";
            this.encoderConfigTab.Padding = new System.Windows.Forms.Padding(3);
            this.encoderConfigTab.Size = new System.Drawing.Size(453, 272);
            this.encoderConfigTab.TabIndex = 2;
            this.encoderConfigTab.Text = "Encoder Config";
            this.encoderConfigTab.UseVisualStyleBackColor = true;
            // 
            // containerFormatLabel
            // 
            this.containerFormatLabel.Location = new System.Drawing.Point(15, 242);
            this.containerFormatLabel.Name = "containerFormatLabel";
            this.containerFormatLabel.Size = new System.Drawing.Size(92, 13);
            this.containerFormatLabel.TabIndex = 38;
            this.containerFormatLabel.Text = "Container Format";
            // 
            // containerFormat
            // 
            this.containerFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.containerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containerFormat.Location = new System.Drawing.Point(124, 238);
            this.containerFormat.Name = "containerFormat";
            this.containerFormat.Size = new System.Drawing.Size(135, 21);
            this.containerFormat.TabIndex = 37;
            this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.containerFormat_SelectedIndexChanged_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.groupBox1.Controls.Add(this.tabControl2);
            this.groupBox1.Location = new System.Drawing.Point(8, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 139);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Audio";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.trackTabPage1);
            this.tabControl2.Controls.Add(trackTabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 16);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(431, 120);
            this.tabControl2.TabIndex = 32;
            // 
            // videoGroupBox
            // 
            this.videoGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoGroupBox.Controls.Add(this.videoProfileControl);
            this.videoGroupBox.Controls.Add(this.addPrerenderJob);
            this.videoGroupBox.Controls.Add(this.videoCodecLabel);
            this.videoGroupBox.Controls.Add(this.videoCodec);
            this.videoGroupBox.Location = new System.Drawing.Point(8, 6);
            this.videoGroupBox.Name = "videoGroupBox";
            this.videoGroupBox.Size = new System.Drawing.Size(437, 77);
            this.videoGroupBox.TabIndex = 31;
            this.videoGroupBox.TabStop = false;
            this.videoGroupBox.Text = "Video";
            // 
            // videoProfileControl
            // 
            this.videoProfileControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoProfileControl.LabelText = "Video Profile";
            this.videoProfileControl.Location = new System.Drawing.Point(12, 42);
            this.videoProfileControl.Name = "videoProfileControl";
            this.videoProfileControl.Size = new System.Drawing.Size(415, 29);
            this.videoProfileControl.TabIndex = 17;
            // 
            // addPrerenderJob
            // 
            this.addPrerenderJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addPrerenderJob.AutoSize = true;
            this.addPrerenderJob.Location = new System.Drawing.Point(259, 22);
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
            this.videoCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoCodec.Location = new System.Drawing.Point(116, 20);
            this.videoCodec.Name = "videoCodec";
            this.videoCodec.Size = new System.Drawing.Size(130, 21);
            this.videoCodec.TabIndex = 8;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "IFO Files|*.ifo|VOB Files (*.vob)|*.vob|MPEG-1/2 Program Streams (*.mpg)|*.mpg|Tr" +
                "ansport Streams (*.ts)|*.ts";
            // 
            // showAdvancedOptions
            // 
            this.showAdvancedOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showAdvancedOptions.AutoSize = true;
            this.showAdvancedOptions.Location = new System.Drawing.Point(76, 304);
            this.showAdvancedOptions.Name = "showAdvancedOptions";
            this.showAdvancedOptions.Size = new System.Drawing.Size(144, 17);
            this.showAdvancedOptions.TabIndex = 31;
            this.showAdvancedOptions.Text = "Show Advanced Options";
            this.showAdvancedOptions.UseVisualStyleBackColor = true;
            this.showAdvancedOptions.CheckedChanged += new System.EventHandler(this.showAdvancedOptions_CheckedChanged);
            // 
            // goButton
            // 
            this.goButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.goButton.Location = new System.Drawing.Point(374, 304);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 29;
            this.goButton.Text = "Go!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpButton1.ArticleName = "One click encoder";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(12, 300);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 32;
            // 
            // OneClickWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 335);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.showAdvancedOptions);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.goButton);
            this.Name = "OneClickWindow";
            this.Text = "MeGUI - One Click Encoder";
            trackTabPage2.ResumeLayout(false);
            this.trackTabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.locationGroupBox.ResumeLayout(false);
            this.locationGroupBox.PerformLayout();
            this.avsBox.ResumeLayout(false);
            this.avsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResolution)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.IOGroupbox.ResumeLayout(false);
            this.targetGroupBox.ResumeLayout(false);
            this.audioGroupbox.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.encoderConfigTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.videoGroupBox.ResumeLayout(false);
            this.videoGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox audioGroupbox;
        private System.Windows.Forms.Label track2Label;
        private System.Windows.Forms.Label track1Label;
        private System.Windows.Forms.GroupBox IOGroupbox;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.GroupBox avsBox;
        private System.Windows.Forms.CheckBox signalAR;
        private System.Windows.Forms.Label outputResolutionLabel;
        private System.Windows.Forms.NumericUpDown horizontalResolution;
        private System.Windows.Forms.Label ARLabel;
        private System.Windows.Forms.GroupBox locationGroupBox;
        private System.Windows.Forms.Label workingDirectoryLabel;
        private System.Windows.Forms.TextBox workingName;
        private System.Windows.Forms.Label projectNameLabel;
        private System.Windows.Forms.GroupBox targetGroupBox;
        private System.Windows.Forms.Label filesizeLabel;
        private System.Windows.Forms.Label chapterLabel;
        private System.Windows.Forms.CheckBox autoDeint;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage encoderConfigTab;
        private System.Windows.Forms.GroupBox videoGroupBox;
        private System.Windows.Forms.CheckBox addPrerenderJob;
        private System.Windows.Forms.Label videoCodecLabel;
        private System.Windows.Forms.ComboBox videoCodec;
        private System.Windows.Forms.CheckBox showAdvancedOptions;
        private System.Windows.Forms.Button goButton;
        private MeGUI.core.details.video.ProfileControl profileControl1;
        private MeGUI.core.details.video.ProfileControl videoProfileControl;
        private FileBar input;
        private FileBar output;
        private FileBar workingDirectory;
        private FileBar chapterFile;
        private MeGUI.core.details.video.ProfileControl profileControl2;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.TabControl tabControl2;
        private MeGUI.packages.tools.oneclick.AudioConfigControl audio1;
        private MeGUI.packages.tools.oneclick.AudioConfigControl audio2;
        private MeGUI.core.gui.FileSCBox audioTrack2;
        private MeGUI.core.gui.FileSCBox audioTrack1;
        private MeGUI.core.gui.TargetSizeSCBox optionalTargetSizeBox1;
        private MeGUI.core.gui.ARChooser ar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage trackTabPage1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addTrackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTrackToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private MeGUI.core.gui.TargetSizeSCBox splitting;
        private System.Windows.Forms.Label containerFormatLabel;
        private System.Windows.Forms.ComboBox containerFormat;


    }
}