namespace MeGUI.packages.video.divxavc
{
    partial class DivXAVCConfigurationPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DivXAVCConfigurationPanel));
            this.divxavcGeneralOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.divxavcTurbo = new System.Windows.Forms.CheckBox();
            this.logfile = new System.Windows.Forms.TextBox();
            this.logfileOpenButton = new System.Windows.Forms.Button();
            this.logfileLabel = new System.Windows.Forms.Label();
            this.divxavcBitrateQuantizer = new System.Windows.Forms.NumericUpDown();
            this.divxavcBitrateQuantLabel = new System.Windows.Forms.Label();
            this.divxavcEncodingMode = new System.Windows.Forms.ComboBox();
            this.divxavcModeLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.divxavcMoreOptions = new System.Windows.Forms.GroupBox();
            this.divxavcThreadsLabel = new System.Windows.Forms.Label();
            this.nbThreads = new System.Windows.Forms.NumericUpDown();
            this.divxavcPyramid = new System.Windows.Forms.CheckBox();
            this.divxavcBFramesAsRef = new System.Windows.Forms.CheckBox();
            this.divxavcMaxBFrames = new System.Windows.Forms.NumericUpDown();
            this.lbMaxBFrames = new System.Windows.Forms.Label();
            this.divxavcMaxRefFrames = new System.Windows.Forms.NumericUpDown();
            this.lbMaxRefFrames = new System.Windows.Forms.Label();
            this.divxavcInterlaceMode = new System.Windows.Forms.ComboBox();
            this.lbInterlaceMode = new System.Windows.Forms.Label();
            this.divxavcAQO = new System.Windows.Forms.NumericUpDown();
            this.lbAQO = new System.Windows.Forms.Label();
            this.divxavcGOPLength = new System.Windows.Forms.NumericUpDown();
            this.lbGOPLength = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gradientPanel1 = new MeGUI.GradientPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.mainTabPage.SuspendLayout();
            this.divxavcGeneralOptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcBitrateQuantizer)).BeginInit();
            this.divxavcMoreOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcMaxBFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcMaxRefFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcAQO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcGOPLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // commandline
            // 
            this.commandline.Location = new System.Drawing.Point(0, 490);
            this.commandline.Size = new System.Drawing.Size(399, 86);
            this.commandline.Text = "program ";
            // 
            // tabControl1
            // 
            this.tabControl1.Size = new System.Drawing.Size(396, 392);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.divxavcMoreOptions);
            this.mainTabPage.Controls.Add(this.divxavcGeneralOptionsGroupBox);
            this.mainTabPage.Size = new System.Drawing.Size(388, 366);
            // 
            // divxavcGeneralOptionsGroupBox
            // 
            this.divxavcGeneralOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.divxavcGeneralOptionsGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.divxavcGeneralOptionsGroupBox.Controls.Add(this.divxavcTurbo);
            this.divxavcGeneralOptionsGroupBox.Controls.Add(this.logfile);
            this.divxavcGeneralOptionsGroupBox.Controls.Add(this.logfileOpenButton);
            this.divxavcGeneralOptionsGroupBox.Controls.Add(this.logfileLabel);
            this.divxavcGeneralOptionsGroupBox.Controls.Add(this.divxavcBitrateQuantizer);
            this.divxavcGeneralOptionsGroupBox.Controls.Add(this.divxavcBitrateQuantLabel);
            this.divxavcGeneralOptionsGroupBox.Controls.Add(this.divxavcEncodingMode);
            this.divxavcGeneralOptionsGroupBox.Controls.Add(this.divxavcModeLabel);
            this.divxavcGeneralOptionsGroupBox.Location = new System.Drawing.Point(3, 6);
            this.divxavcGeneralOptionsGroupBox.Name = "divxavcGeneralOptionsGroupBox";
            this.divxavcGeneralOptionsGroupBox.Size = new System.Drawing.Size(382, 120);
            this.divxavcGeneralOptionsGroupBox.TabIndex = 25;
            this.divxavcGeneralOptionsGroupBox.TabStop = false;
            this.divxavcGeneralOptionsGroupBox.Text = "General";
            // 
            // divxavcTurbo
            // 
            this.divxavcTurbo.AutoSize = true;
            this.divxavcTurbo.Location = new System.Drawing.Point(224, 26);
            this.divxavcTurbo.Name = "divxavcTurbo";
            this.divxavcTurbo.Size = new System.Drawing.Size(54, 17);
            this.divxavcTurbo.TabIndex = 17;
            this.divxavcTurbo.Text = "Turbo";
            this.divxavcTurbo.UseVisualStyleBackColor = true;
            this.divxavcTurbo.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // logfile
            // 
            this.logfile.Location = new System.Drawing.Point(70, 80);
            this.logfile.Name = "logfile";
            this.logfile.ReadOnly = true;
            this.logfile.Size = new System.Drawing.Size(269, 20);
            this.logfile.TabIndex = 15;
            this.logfile.Text = "2pass.stats";
            // 
            // logfileOpenButton
            // 
            this.logfileOpenButton.Location = new System.Drawing.Point(345, 78);
            this.logfileOpenButton.Name = "logfileOpenButton";
            this.logfileOpenButton.Size = new System.Drawing.Size(24, 23);
            this.logfileOpenButton.TabIndex = 16;
            this.logfileOpenButton.Text = "...";
            this.logfileOpenButton.Click += new System.EventHandler(this.logfileOpenButton_Click);
            // 
            // logfileLabel
            // 
            this.logfileLabel.Location = new System.Drawing.Point(14, 81);
            this.logfileLabel.Name = "logfileLabel";
            this.logfileLabel.Size = new System.Drawing.Size(46, 20);
            this.logfileLabel.TabIndex = 14;
            this.logfileLabel.Text = "Logfile :";
            // 
            // divxavcBitrateQuantizer
            // 
            this.divxavcBitrateQuantizer.Location = new System.Drawing.Point(70, 49);
            this.divxavcBitrateQuantizer.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.divxavcBitrateQuantizer.Name = "divxavcBitrateQuantizer";
            this.divxavcBitrateQuantizer.Size = new System.Drawing.Size(120, 20);
            this.divxavcBitrateQuantizer.TabIndex = 13;
            this.divxavcBitrateQuantizer.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.divxavcBitrateQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // divxavcBitrateQuantLabel
            // 
            this.divxavcBitrateQuantLabel.Location = new System.Drawing.Point(14, 49);
            this.divxavcBitrateQuantLabel.Name = "divxavcBitrateQuantLabel";
            this.divxavcBitrateQuantLabel.Size = new System.Drawing.Size(50, 24);
            this.divxavcBitrateQuantLabel.TabIndex = 3;
            this.divxavcBitrateQuantLabel.Text = "Bitrate :";
            // 
            // divxavcEncodingMode
            // 
            this.divxavcEncodingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.divxavcEncodingMode.Items.AddRange(new object[] {
            "2pass - 1st pass",
            "2pass - 2nd pass",
            "Automated 2pass"});
            this.divxavcEncodingMode.Location = new System.Drawing.Point(70, 22);
            this.divxavcEncodingMode.Name = "divxavcEncodingMode";
            this.divxavcEncodingMode.Size = new System.Drawing.Size(121, 21);
            this.divxavcEncodingMode.TabIndex = 2;
            this.divxavcEncodingMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // divxavcModeLabel
            // 
            this.divxavcModeLabel.Location = new System.Drawing.Point(14, 25);
            this.divxavcModeLabel.Name = "divxavcModeLabel";
            this.divxavcModeLabel.Size = new System.Drawing.Size(50, 24);
            this.divxavcModeLabel.TabIndex = 0;
            this.divxavcModeLabel.Text = "Mode :";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // divxavcMoreOptions
            // 
            this.divxavcMoreOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.divxavcMoreOptions.Controls.Add(this.divxavcThreadsLabel);
            this.divxavcMoreOptions.Controls.Add(this.nbThreads);
            this.divxavcMoreOptions.Controls.Add(this.divxavcPyramid);
            this.divxavcMoreOptions.Controls.Add(this.divxavcBFramesAsRef);
            this.divxavcMoreOptions.Controls.Add(this.divxavcMaxBFrames);
            this.divxavcMoreOptions.Controls.Add(this.lbMaxBFrames);
            this.divxavcMoreOptions.Controls.Add(this.divxavcMaxRefFrames);
            this.divxavcMoreOptions.Controls.Add(this.lbMaxRefFrames);
            this.divxavcMoreOptions.Controls.Add(this.divxavcInterlaceMode);
            this.divxavcMoreOptions.Controls.Add(this.lbInterlaceMode);
            this.divxavcMoreOptions.Controls.Add(this.divxavcAQO);
            this.divxavcMoreOptions.Controls.Add(this.lbAQO);
            this.divxavcMoreOptions.Controls.Add(this.divxavcGOPLength);
            this.divxavcMoreOptions.Controls.Add(this.lbGOPLength);
            this.divxavcMoreOptions.Location = new System.Drawing.Point(3, 128);
            this.divxavcMoreOptions.Name = "divxavcMoreOptions";
            this.divxavcMoreOptions.Size = new System.Drawing.Size(382, 145);
            this.divxavcMoreOptions.TabIndex = 26;
            this.divxavcMoreOptions.TabStop = false;
            this.divxavcMoreOptions.Text = "More";
            // 
            // divxavcThreadsLabel
            // 
            this.divxavcThreadsLabel.AutoSize = true;
            this.divxavcThreadsLabel.Location = new System.Drawing.Point(247, 58);
            this.divxavcThreadsLabel.Name = "divxavcThreadsLabel";
            this.divxavcThreadsLabel.Size = new System.Drawing.Size(52, 13);
            this.divxavcThreadsLabel.TabIndex = 12;
            this.divxavcThreadsLabel.Text = "Threads :";
            // 
            // nbThreads
            // 
            this.nbThreads.Location = new System.Drawing.Point(304, 55);
            this.nbThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbThreads.Name = "nbThreads";
            this.nbThreads.Size = new System.Drawing.Size(65, 20);
            this.nbThreads.TabIndex = 13;
            this.nbThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbThreads.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // divxavcPyramid
            // 
            this.divxavcPyramid.AutoSize = true;
            this.divxavcPyramid.Enabled = false;
            this.divxavcPyramid.Location = new System.Drawing.Point(224, 108);
            this.divxavcPyramid.Name = "divxavcPyramid";
            this.divxavcPyramid.Size = new System.Drawing.Size(111, 17);
            this.divxavcPyramid.TabIndex = 11;
            this.divxavcPyramid.Text = "Pyramid Encoding";
            this.divxavcPyramid.UseVisualStyleBackColor = true;
            this.divxavcPyramid.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // divxavcBFramesAsRef
            // 
            this.divxavcBFramesAsRef.AutoSize = true;
            this.divxavcBFramesAsRef.Location = new System.Drawing.Point(224, 85);
            this.divxavcBFramesAsRef.Name = "divxavcBFramesAsRef";
            this.divxavcBFramesAsRef.Size = new System.Drawing.Size(137, 17);
            this.divxavcBFramesAsRef.TabIndex = 10;
            this.divxavcBFramesAsRef.Text = "B-Frames as Reference";
            this.divxavcBFramesAsRef.UseVisualStyleBackColor = true;
            this.divxavcBFramesAsRef.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // divxavcMaxBFrames
            // 
            this.divxavcMaxBFrames.Location = new System.Drawing.Point(305, 24);
            this.divxavcMaxBFrames.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.divxavcMaxBFrames.Name = "divxavcMaxBFrames";
            this.divxavcMaxBFrames.Size = new System.Drawing.Size(64, 20);
            this.divxavcMaxBFrames.TabIndex = 9;
            this.divxavcMaxBFrames.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.divxavcMaxBFrames.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbMaxBFrames
            // 
            this.lbMaxBFrames.AutoSize = true;
            this.lbMaxBFrames.Location = new System.Drawing.Point(219, 26);
            this.lbMaxBFrames.Name = "lbMaxBFrames";
            this.lbMaxBFrames.Size = new System.Drawing.Size(80, 13);
            this.lbMaxBFrames.TabIndex = 8;
            this.lbMaxBFrames.Text = "Max B-Frames :";
            // 
            // divxavcMaxRefFrames
            // 
            this.divxavcMaxRefFrames.Location = new System.Drawing.Point(108, 78);
            this.divxavcMaxRefFrames.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.divxavcMaxRefFrames.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.divxavcMaxRefFrames.Name = "divxavcMaxRefFrames";
            this.divxavcMaxRefFrames.Size = new System.Drawing.Size(69, 20);
            this.divxavcMaxRefFrames.TabIndex = 7;
            this.divxavcMaxRefFrames.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.divxavcMaxRefFrames.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbMaxRefFrames
            // 
            this.lbMaxRefFrames.AutoSize = true;
            this.lbMaxRefFrames.Location = new System.Drawing.Point(8, 80);
            this.lbMaxRefFrames.Name = "lbMaxRefFrames";
            this.lbMaxRefFrames.Size = new System.Drawing.Size(90, 13);
            this.lbMaxRefFrames.TabIndex = 6;
            this.lbMaxRefFrames.Text = "Max Ref Frames :";
            // 
            // divxavcInterlaceMode
            // 
            this.divxavcInterlaceMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.divxavcInterlaceMode.FormattingEnabled = true;
            this.divxavcInterlaceMode.Items.AddRange(new object[] {
            "None",
            "MBAFF",
            "Field"});
            this.divxavcInterlaceMode.Location = new System.Drawing.Point(108, 104);
            this.divxavcInterlaceMode.Name = "divxavcInterlaceMode";
            this.divxavcInterlaceMode.Size = new System.Drawing.Size(69, 21);
            this.divxavcInterlaceMode.TabIndex = 5;
            this.divxavcInterlaceMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbInterlaceMode
            // 
            this.lbInterlaceMode.AutoSize = true;
            this.lbInterlaceMode.Location = new System.Drawing.Point(8, 107);
            this.lbInterlaceMode.Name = "lbInterlaceMode";
            this.lbInterlaceMode.Size = new System.Drawing.Size(84, 13);
            this.lbInterlaceMode.TabIndex = 4;
            this.lbInterlaceMode.Text = "Interlace Mode :";
            // 
            // divxavcAQO
            // 
            this.divxavcAQO.Location = new System.Drawing.Point(108, 51);
            this.divxavcAQO.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.divxavcAQO.Name = "divxavcAQO";
            this.divxavcAQO.Size = new System.Drawing.Size(69, 20);
            this.divxavcAQO.TabIndex = 3;
            this.divxavcAQO.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.divxavcAQO.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbAQO
            // 
            this.lbAQO.AutoSize = true;
            this.lbAQO.Location = new System.Drawing.Point(8, 53);
            this.lbAQO.Name = "lbAQO";
            this.lbAQO.Size = new System.Drawing.Size(94, 13);
            this.lbAQO.TabIndex = 2;
            this.lbAQO.Text = "Algo Optimization :";
            // 
            // divxavcGOPLength
            // 
            this.divxavcGOPLength.Location = new System.Drawing.Point(108, 24);
            this.divxavcGOPLength.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.divxavcGOPLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.divxavcGOPLength.Name = "divxavcGOPLength";
            this.divxavcGOPLength.Size = new System.Drawing.Size(69, 20);
            this.divxavcGOPLength.TabIndex = 1;
            this.divxavcGOPLength.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.divxavcGOPLength.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbGOPLength
            // 
            this.lbGOPLength.AutoSize = true;
            this.lbGOPLength.Location = new System.Drawing.Point(8, 26);
            this.lbGOPLength.Name = "lbGOPLength";
            this.lbGOPLength.Size = new System.Drawing.Size(72, 13);
            this.lbGOPLength.TabIndex = 0;
            this.lbGOPLength.Text = "GOP Length :";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(292, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(83, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel1.Controls.Add(this.pictureBox2);
            this.gradientPanel1.Controls.Add(this.label1);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.PageEndColor = System.Drawing.Color.Empty;
            this.gradientPanel1.PageStartColor = System.Drawing.Color.SlateGray;
            this.gradientPanel1.Size = new System.Drawing.Size(399, 90);
            this.gradientPanel1.TabIndex = 43;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(268, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(23, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Adjust your Settings here...";
            // 
            // DivXAVCConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gradientPanel1);
            this.Name = "DivXAVCConfigurationPanel";
            this.Size = new System.Drawing.Size(399, 576);
            this.Controls.SetChildIndex(this.commandline, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.gradientPanel1, 0);
            this.tabControl1.ResumeLayout(false);
            this.mainTabPage.ResumeLayout(false);
            this.divxavcGeneralOptionsGroupBox.ResumeLayout(false);
            this.divxavcGeneralOptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcBitrateQuantizer)).EndInit();
            this.divxavcMoreOptions.ResumeLayout(false);
            this.divxavcMoreOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcMaxBFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcMaxRefFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcAQO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.divxavcGOPLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox divxavcGeneralOptionsGroupBox;
        private System.Windows.Forms.TextBox logfile;
        private System.Windows.Forms.Button logfileOpenButton;
        private System.Windows.Forms.Label logfileLabel;
        private System.Windows.Forms.NumericUpDown divxavcBitrateQuantizer;
        private System.Windows.Forms.Label divxavcBitrateQuantLabel;
        private System.Windows.Forms.ComboBox divxavcEncodingMode;
        private System.Windows.Forms.Label divxavcModeLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.GroupBox divxavcMoreOptions;
        private System.Windows.Forms.Label lbGOPLength;
        private System.Windows.Forms.NumericUpDown divxavcGOPLength;
        private System.Windows.Forms.NumericUpDown divxavcAQO;
        private System.Windows.Forms.Label lbAQO;
        private System.Windows.Forms.Label lbInterlaceMode;
        private System.Windows.Forms.ComboBox divxavcInterlaceMode;
        private System.Windows.Forms.Label lbMaxRefFrames;
        private System.Windows.Forms.NumericUpDown divxavcMaxRefFrames;
        private System.Windows.Forms.NumericUpDown divxavcMaxBFrames;
        private System.Windows.Forms.Label lbMaxBFrames;
        private System.Windows.Forms.CheckBox divxavcBFramesAsRef;
        private System.Windows.Forms.CheckBox divxavcPyramid;
        private System.Windows.Forms.Label divxavcThreadsLabel;
        private System.Windows.Forms.NumericUpDown nbThreads;
        private System.Windows.Forms.CheckBox divxavcTurbo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private GradientPanel gradientPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;


    }
}
