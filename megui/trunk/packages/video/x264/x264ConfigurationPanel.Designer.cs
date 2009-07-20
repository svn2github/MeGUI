namespace MeGUI.packages.video.x264
{
    partial class x264ConfigurationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(x264ConfigurationPanel));
            this.x264GeneralMiscGroupbox = new System.Windows.Forms.GroupBox();
            this.ssim = new System.Windows.Forms.CheckBox();
            this.psnr = new System.Windows.Forms.CheckBox();
            this.x264NbThreadsLabel = new System.Windows.Forms.Label();
            this.x264NbThreads = new System.Windows.Forms.NumericUpDown();
            this.avcLevelGroupbox = new System.Windows.Forms.GroupBox();
            this.avcLevel = new System.Windows.Forms.ComboBox();
            this.avcProfileGroupbox = new System.Windows.Forms.GroupBox();
            this.avcProfile = new System.Windows.Forms.ComboBox();
            this.x264CodecToolsGroupbox = new System.Windows.Forms.GroupBox();
            this.x264BetaDeblock = new System.Windows.Forms.NumericUpDown();
            this.x264AlphaDeblock = new System.Windows.Forms.NumericUpDown();
            this.x264DeblockActive = new System.Windows.Forms.CheckBox();
            this.x264BetaDeblockLabel = new System.Windows.Forms.Label();
            this.x264AlphaDeblockLabel = new System.Windows.Forms.Label();
            this.x264CodecGeneralGroupbox = new System.Windows.Forms.GroupBox();
            this.qpfile = new System.Windows.Forms.TextBox();
            this.qpfileOpenButton = new System.Windows.Forms.Button();
            this.useQPFile = new System.Windows.Forms.CheckBox();
            this.x264BitrateQuantizer = new System.Windows.Forms.NumericUpDown();
            this.logfile = new System.Windows.Forms.TextBox();
            this.x264EncodingMode = new System.Windows.Forms.ComboBox();
            this.logfileLabel = new System.Windows.Forms.Label();
            this.logfileOpenButton = new System.Windows.Forms.Button();
            this.x264Turbo = new System.Windows.Forms.CheckBox();
            this.x264EncodingModeLabel = new System.Windows.Forms.Label();
            this.x264BitrateQuantizerLabel = new System.Windows.Forms.Label();
            this.x264LosslessMode = new System.Windows.Forms.CheckBox();
            this.rateControlTabPage = new System.Windows.Forms.TabPage();
            this.x264OtherOptionsGroupbox = new System.Windows.Forms.GroupBox();
            this.x264SubpelRefinement = new System.Windows.Forms.ComboBox();
            this.x264SubpelRefinementLabel = new System.Windows.Forms.Label();
            this.x264ChromaMe = new System.Windows.Forms.CheckBox();
            this.x264MERangeLabel = new System.Windows.Forms.Label();
            this.x264METypeLabel = new System.Windows.Forms.Label();
            this.x264METype = new System.Windows.Forms.ComboBox();
            this.x264MERange = new System.Windows.Forms.NumericUpDown();
            this.x264SCDSensitivity = new System.Windows.Forms.NumericUpDown();
            this.x264SCDSensitivityLabel = new System.Windows.Forms.Label();
            this.x264QuantOptionsGroupbox = new System.Windows.Forms.GroupBox();
            this.PsyTrellisLabel = new System.Windows.Forms.Label();
            this.PsyTrellis = new System.Windows.Forms.NumericUpDown();
            this.PsyRDLabel = new System.Windows.Forms.Label();
            this.PsyRD = new System.Windows.Forms.NumericUpDown();
            this.x264MixedReferences = new System.Windows.Forms.CheckBox();
            this.x264NumberOfRefFrames = new System.Windows.Forms.NumericUpDown();
            this.x264CabacEnabled = new System.Windows.Forms.CheckBox();
            this.noDCTDecimateOption = new System.Windows.Forms.CheckBox();
            this.NoFastPSkip = new System.Windows.Forms.CheckBox();
            this.trellis = new System.Windows.Forms.ComboBox();
            this.x264NumberOfRefFramesLabel = new System.Windows.Forms.Label();
            this.trellisLabel = new System.Windows.Forms.Label();
            this.x264RateControlMiscGroupbox = new System.Windows.Forms.GroupBox();
            this.x264FullRange = new System.Windows.Forms.CheckBox();
            this.interlaced = new System.Windows.Forms.CheckBox();
            this.NoiseReduction = new System.Windows.Forms.TextBox();
            this.NoiseReductionLabel = new System.Windows.Forms.Label();
            this.x264KeyframeIntervalLabel = new System.Windows.Forms.Label();
            this.x264KeyframeInterval = new System.Windows.Forms.TextBox();
            this.x264MinGOPSize = new System.Windows.Forms.TextBox();
            this.x264MinGOPSizeLabel = new System.Windows.Forms.Label();
            this.x264RCGroupbox = new System.Windows.Forms.GroupBox();
            this.x264RateTolLabel = new System.Windows.Forms.Label();
            this.x264VBVInitialBuffer = new System.Windows.Forms.NumericUpDown();
            this.x264VBVInitialBufferLabel = new System.Windows.Forms.Label();
            this.x264VBVMaxRate = new System.Windows.Forms.TextBox();
            this.x264TempQuantBlur = new System.Windows.Forms.NumericUpDown();
            this.x264TempFrameComplexityBlur = new System.Windows.Forms.NumericUpDown();
            this.x264QuantizerCompression = new System.Windows.Forms.NumericUpDown();
            this.x264VBVBufferSize = new System.Windows.Forms.TextBox();
            this.x264TempQuantBlurLabel = new System.Windows.Forms.Label();
            this.x264TempFrameComplexityBlurLabel = new System.Windows.Forms.Label();
            this.x264QuantizerCompressionLabel = new System.Windows.Forms.Label();
            this.x264VBVMaxRateLabel = new System.Windows.Forms.Label();
            this.x264VBVBufferSizeLabel = new System.Windows.Forms.Label();
            this.x264RateTol = new System.Windows.Forms.NumericUpDown();
            this.quantizationTabPage = new System.Windows.Forms.TabPage();
            this.gbx264CustomCmd = new System.Windows.Forms.GroupBox();
            this.customCommandlineOptions = new System.Windows.Forms.TextBox();
            this.gbAQ = new System.Windows.Forms.GroupBox();
            this.numAQStrength = new System.Windows.Forms.NumericUpDown();
            this.lbAQStrength = new System.Windows.Forms.Label();
            this.cbAQMode = new System.Windows.Forms.ComboBox();
            this.lbAQMode = new System.Windows.Forms.Label();
            this.x264GeneralBFramesgGroupbox = new System.Windows.Forms.GroupBox();
            this.x264AdaptiveBframesLabel = new System.Windows.Forms.Label();
            this.x264NewAdaptiveBframes = new System.Windows.Forms.ComboBox();
            this.x264BframePredictionMode = new System.Windows.Forms.ComboBox();
            this.x264BframeBias = new System.Windows.Forms.NumericUpDown();
            this.x264WeightedBPrediction = new System.Windows.Forms.CheckBox();
            this.x264BframeBiasLabel = new System.Windows.Forms.Label();
            this.x264BframePredictionModeLabel = new System.Windows.Forms.Label();
            this.x264NumberOfBFramesLabel = new System.Windows.Forms.Label();
            this.x264NumberOfBFrames = new System.Windows.Forms.NumericUpDown();
            this.x264PyramidBframes = new System.Windows.Forms.CheckBox();
            this.quantizerMatrixGroupbox = new System.Windows.Forms.GroupBox();
            this.cqmComboBox1 = new MeGUI.core.gui.FileSCBox();
            this.x264MBGroupbox = new System.Windows.Forms.GroupBox();
            this.macroblockOptions = new System.Windows.Forms.ComboBox();
            this.adaptiveDCT = new System.Windows.Forms.CheckBox();
            this.x264I4x4mv = new System.Windows.Forms.CheckBox();
            this.x264I8x8mv = new System.Windows.Forms.CheckBox();
            this.x264P4x4mv = new System.Windows.Forms.CheckBox();
            this.x264B8x8mv = new System.Windows.Forms.CheckBox();
            this.x264P8x8mv = new System.Windows.Forms.CheckBox();
            this.x264QuantizerGroupBox = new System.Windows.Forms.GroupBox();
            this.deadzoneIntra = new System.Windows.Forms.NumericUpDown();
            this.deadzoneInter = new System.Windows.Forms.NumericUpDown();
            this.lbx264DeadZones = new System.Windows.Forms.Label();
            this.x264PBFrameFactor = new System.Windows.Forms.NumericUpDown();
            this.x264IPFrameFactor = new System.Windows.Forms.NumericUpDown();
            this.lbQuantizersRatio = new System.Windows.Forms.Label();
            this.x264CreditsQuantizer = new System.Windows.Forms.NumericUpDown();
            this.x264CreditsQuantizerLabel = new System.Windows.Forms.Label();
            this.x264ChromaQPOffset = new System.Windows.Forms.NumericUpDown();
            this.x264ChromaQPOffsetLabel = new System.Windows.Forms.Label();
            this.x264MaxQuantDelta = new System.Windows.Forms.NumericUpDown();
            this.x264MaximumQuantizer = new System.Windows.Forms.NumericUpDown();
            this.x264MinimimQuantizer = new System.Windows.Forms.NumericUpDown();
            this.x264MinimimQuantizerLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.customCommandlineOptionsLabel = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.x264PicBox = new System.Windows.Forms.PictureBox();
            this.linkx264website = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.mainTabPage.SuspendLayout();
            this.x264GeneralMiscGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264NbThreads)).BeginInit();
            this.avcLevelGroupbox.SuspendLayout();
            this.avcProfileGroupbox.SuspendLayout();
            this.x264CodecToolsGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264BetaDeblock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264AlphaDeblock)).BeginInit();
            this.x264CodecGeneralGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264BitrateQuantizer)).BeginInit();
            this.rateControlTabPage.SuspendLayout();
            this.x264OtherOptionsGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264MERange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264SCDSensitivity)).BeginInit();
            this.x264QuantOptionsGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PsyTrellis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsyRD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264NumberOfRefFrames)).BeginInit();
            this.x264RateControlMiscGroupbox.SuspendLayout();
            this.x264RCGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264VBVInitialBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264TempQuantBlur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264TempFrameComplexityBlur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264QuantizerCompression)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264RateTol)).BeginInit();
            this.quantizationTabPage.SuspendLayout();
            this.gbx264CustomCmd.SuspendLayout();
            this.gbAQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAQStrength)).BeginInit();
            this.x264GeneralBFramesgGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264BframeBias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264NumberOfBFrames)).BeginInit();
            this.quantizerMatrixGroupbox.SuspendLayout();
            this.x264MBGroupbox.SuspendLayout();
            this.x264QuantizerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneIntra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneInter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264PBFrameFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264IPFrameFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264CreditsQuantizer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264ChromaQPOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264MaxQuantDelta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264MaximumQuantizer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264MinimimQuantizer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264PicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.rateControlTabPage);
            this.tabControl1.Controls.Add(this.quantizationTabPage);
            this.tabControl1.Size = new System.Drawing.Size(510, 429);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Controls.SetChildIndex(this.quantizationTabPage, 0);
            this.tabControl1.Controls.SetChildIndex(this.rateControlTabPage, 0);
            this.tabControl1.Controls.SetChildIndex(this.mainTabPage, 0);
            // 
            // commandline
            // 
            this.commandline.Location = new System.Drawing.Point(0, 431);
            this.commandline.Size = new System.Drawing.Size(507, 89);
            this.commandline.TabIndex = 1;
            this.commandline.Text = " ";
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.linkx264website);
            this.mainTabPage.Controls.Add(this.x264PicBox);
            this.mainTabPage.Controls.Add(this.helpButton1);
            this.mainTabPage.Controls.Add(this.x264GeneralMiscGroupbox);
            this.mainTabPage.Controls.Add(this.avcLevelGroupbox);
            this.mainTabPage.Controls.Add(this.avcProfileGroupbox);
            this.mainTabPage.Controls.Add(this.x264CodecToolsGroupbox);
            this.mainTabPage.Controls.Add(this.x264CodecGeneralGroupbox);
            this.mainTabPage.Size = new System.Drawing.Size(502, 403);
            // 
            // x264GeneralMiscGroupbox
            // 
            this.x264GeneralMiscGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264GeneralMiscGroupbox.Controls.Add(this.ssim);
            this.x264GeneralMiscGroupbox.Controls.Add(this.psnr);
            this.x264GeneralMiscGroupbox.Controls.Add(this.x264NbThreadsLabel);
            this.x264GeneralMiscGroupbox.Controls.Add(this.x264NbThreads);
            this.x264GeneralMiscGroupbox.Location = new System.Drawing.Point(321, 3);
            this.x264GeneralMiscGroupbox.Name = "x264GeneralMiscGroupbox";
            this.x264GeneralMiscGroupbox.Size = new System.Drawing.Size(175, 159);
            this.x264GeneralMiscGroupbox.TabIndex = 1;
            this.x264GeneralMiscGroupbox.TabStop = false;
            this.x264GeneralMiscGroupbox.Text = "Misc";
            // 
            // ssim
            // 
            this.ssim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ssim.Location = new System.Drawing.Point(7, 43);
            this.ssim.Name = "ssim";
            this.ssim.Padding = new System.Windows.Forms.Padding(3);
            this.ssim.Size = new System.Drawing.Size(160, 23);
            this.ssim.TabIndex = 1;
            this.ssim.Text = "Enable SSIM calculation";
            this.ssim.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // psnr
            // 
            this.psnr.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.psnr.Location = new System.Drawing.Point(7, 15);
            this.psnr.Name = "psnr";
            this.psnr.Padding = new System.Windows.Forms.Padding(3);
            this.psnr.Size = new System.Drawing.Size(160, 23);
            this.psnr.TabIndex = 0;
            this.psnr.Text = "Enable PSNR calculation";
            this.psnr.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264NbThreadsLabel
            // 
            this.x264NbThreadsLabel.AutoSize = true;
            this.x264NbThreadsLabel.Location = new System.Drawing.Point(7, 73);
            this.x264NbThreadsLabel.Margin = new System.Windows.Forms.Padding(3);
            this.x264NbThreadsLabel.Name = "x264NbThreadsLabel";
            this.x264NbThreadsLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264NbThreadsLabel.Size = new System.Drawing.Size(101, 19);
            this.x264NbThreadsLabel.TabIndex = 2;
            this.x264NbThreadsLabel.Text = "Threads (0 = Auto)";
            this.x264NbThreadsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264NbThreads
            // 
            this.x264NbThreads.Location = new System.Drawing.Point(127, 72);
            this.x264NbThreads.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.x264NbThreads.Name = "x264NbThreads";
            this.x264NbThreads.Size = new System.Drawing.Size(40, 20);
            this.x264NbThreads.TabIndex = 3;
            this.x264NbThreads.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // avcLevelGroupbox
            // 
            this.avcLevelGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.avcLevelGroupbox.Controls.Add(this.avcLevel);
            this.avcLevelGroupbox.Location = new System.Drawing.Point(321, 214);
            this.avcLevelGroupbox.Name = "avcLevelGroupbox";
            this.avcLevelGroupbox.Size = new System.Drawing.Size(175, 44);
            this.avcLevelGroupbox.TabIndex = 4;
            this.avcLevelGroupbox.TabStop = false;
            this.avcLevelGroupbox.Text = "AVC Level";
            // 
            // avcLevel
            // 
            this.avcLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.avcLevel.Location = new System.Drawing.Point(10, 16);
            this.avcLevel.Name = "avcLevel";
            this.avcLevel.Size = new System.Drawing.Size(157, 21);
            this.avcLevel.TabIndex = 0;
            this.avcLevel.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // avcProfileGroupbox
            // 
            this.avcProfileGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.avcProfileGroupbox.Controls.Add(this.avcProfile);
            this.avcProfileGroupbox.Location = new System.Drawing.Point(321, 164);
            this.avcProfileGroupbox.Name = "avcProfileGroupbox";
            this.avcProfileGroupbox.Size = new System.Drawing.Size(175, 44);
            this.avcProfileGroupbox.TabIndex = 3;
            this.avcProfileGroupbox.TabStop = false;
            this.avcProfileGroupbox.Text = "AVC Profiles";
            // 
            // avcProfile
            // 
            this.avcProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.avcProfile.Items.AddRange(new object[] {
            "Baseline Profile",
            "Main Profile",
            "High Profile",
            "Autoguess"});
            this.avcProfile.Location = new System.Drawing.Point(10, 16);
            this.avcProfile.Name = "avcProfile";
            this.avcProfile.Size = new System.Drawing.Size(157, 21);
            this.avcProfile.TabIndex = 0;
            this.avcProfile.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264CodecToolsGroupbox
            // 
            this.x264CodecToolsGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.x264CodecToolsGroupbox.Controls.Add(this.x264BetaDeblock);
            this.x264CodecToolsGroupbox.Controls.Add(this.x264AlphaDeblock);
            this.x264CodecToolsGroupbox.Controls.Add(this.x264DeblockActive);
            this.x264CodecToolsGroupbox.Controls.Add(this.x264BetaDeblockLabel);
            this.x264CodecToolsGroupbox.Controls.Add(this.x264AlphaDeblockLabel);
            this.x264CodecToolsGroupbox.Location = new System.Drawing.Point(0, 164);
            this.x264CodecToolsGroupbox.Name = "x264CodecToolsGroupbox";
            this.x264CodecToolsGroupbox.Size = new System.Drawing.Size(314, 95);
            this.x264CodecToolsGroupbox.TabIndex = 2;
            this.x264CodecToolsGroupbox.TabStop = false;
            this.x264CodecToolsGroupbox.Text = "Deblocking";
            // 
            // x264BetaDeblock
            // 
            this.x264BetaDeblock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264BetaDeblock.Location = new System.Drawing.Point(268, 66);
            this.x264BetaDeblock.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.x264BetaDeblock.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.x264BetaDeblock.Name = "x264BetaDeblock";
            this.x264BetaDeblock.Size = new System.Drawing.Size(40, 20);
            this.x264BetaDeblock.TabIndex = 4;
            this.x264BetaDeblock.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264AlphaDeblock
            // 
            this.x264AlphaDeblock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264AlphaDeblock.Location = new System.Drawing.Point(268, 43);
            this.x264AlphaDeblock.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.x264AlphaDeblock.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.x264AlphaDeblock.Name = "x264AlphaDeblock";
            this.x264AlphaDeblock.Size = new System.Drawing.Size(40, 20);
            this.x264AlphaDeblock.TabIndex = 2;
            this.x264AlphaDeblock.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264DeblockActive
            // 
            this.x264DeblockActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.x264DeblockActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x264DeblockActive.Checked = true;
            this.x264DeblockActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x264DeblockActive.Location = new System.Drawing.Point(6, 15);
            this.x264DeblockActive.Name = "x264DeblockActive";
            this.x264DeblockActive.Padding = new System.Windows.Forms.Padding(3);
            this.x264DeblockActive.Size = new System.Drawing.Size(308, 23);
            this.x264DeblockActive.TabIndex = 0;
            this.x264DeblockActive.Text = "Enable Deblocking";
            this.x264DeblockActive.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264BetaDeblockLabel
            // 
            this.x264BetaDeblockLabel.AutoSize = true;
            this.x264BetaDeblockLabel.Location = new System.Drawing.Point(6, 65);
            this.x264BetaDeblockLabel.Margin = new System.Windows.Forms.Padding(3);
            this.x264BetaDeblockLabel.Name = "x264BetaDeblockLabel";
            this.x264BetaDeblockLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264BetaDeblockLabel.Size = new System.Drawing.Size(117, 19);
            this.x264BetaDeblockLabel.TabIndex = 3;
            this.x264BetaDeblockLabel.Text = "Deblocking Threshold";
            this.x264BetaDeblockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264AlphaDeblockLabel
            // 
            this.x264AlphaDeblockLabel.AutoSize = true;
            this.x264AlphaDeblockLabel.Location = new System.Drawing.Point(6, 42);
            this.x264AlphaDeblockLabel.Margin = new System.Windows.Forms.Padding(3);
            this.x264AlphaDeblockLabel.Name = "x264AlphaDeblockLabel";
            this.x264AlphaDeblockLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264AlphaDeblockLabel.Size = new System.Drawing.Size(110, 19);
            this.x264AlphaDeblockLabel.TabIndex = 1;
            this.x264AlphaDeblockLabel.Text = "Deblocking Strength";
            this.x264AlphaDeblockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264CodecGeneralGroupbox
            // 
            this.x264CodecGeneralGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.x264CodecGeneralGroupbox.Controls.Add(this.qpfile);
            this.x264CodecGeneralGroupbox.Controls.Add(this.qpfileOpenButton);
            this.x264CodecGeneralGroupbox.Controls.Add(this.useQPFile);
            this.x264CodecGeneralGroupbox.Controls.Add(this.x264BitrateQuantizer);
            this.x264CodecGeneralGroupbox.Controls.Add(this.logfile);
            this.x264CodecGeneralGroupbox.Controls.Add(this.x264EncodingMode);
            this.x264CodecGeneralGroupbox.Controls.Add(this.logfileLabel);
            this.x264CodecGeneralGroupbox.Controls.Add(this.logfileOpenButton);
            this.x264CodecGeneralGroupbox.Controls.Add(this.x264Turbo);
            this.x264CodecGeneralGroupbox.Controls.Add(this.x264EncodingModeLabel);
            this.x264CodecGeneralGroupbox.Controls.Add(this.x264BitrateQuantizerLabel);
            this.x264CodecGeneralGroupbox.Controls.Add(this.x264LosslessMode);
            this.x264CodecGeneralGroupbox.Location = new System.Drawing.Point(0, 3);
            this.x264CodecGeneralGroupbox.Name = "x264CodecGeneralGroupbox";
            this.x264CodecGeneralGroupbox.Size = new System.Drawing.Size(314, 159);
            this.x264CodecGeneralGroupbox.TabIndex = 0;
            this.x264CodecGeneralGroupbox.TabStop = false;
            this.x264CodecGeneralGroupbox.Text = "General";
            // 
            // qpfile
            // 
            this.qpfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.qpfile.Enabled = false;
            this.qpfile.Location = new System.Drawing.Point(96, 119);
            this.qpfile.Name = "qpfile";
            this.qpfile.ReadOnly = true;
            this.qpfile.Size = new System.Drawing.Size(182, 20);
            this.qpfile.TabIndex = 10;
            this.qpfile.Text = ".qp";
            // 
            // qpfileOpenButton
            // 
            this.qpfileOpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.qpfileOpenButton.Enabled = false;
            this.qpfileOpenButton.Location = new System.Drawing.Point(284, 118);
            this.qpfileOpenButton.Name = "qpfileOpenButton";
            this.qpfileOpenButton.Size = new System.Drawing.Size(24, 23);
            this.qpfileOpenButton.TabIndex = 11;
            this.qpfileOpenButton.Text = "...";
            this.qpfileOpenButton.Click += new System.EventHandler(this.qpfileOpenButton_Click);
            // 
            // useQPFile
            // 
            this.useQPFile.AutoSize = true;
            this.useQPFile.Location = new System.Drawing.Point(9, 122);
            this.useQPFile.Name = "useQPFile";
            this.useQPFile.Size = new System.Drawing.Size(79, 17);
            this.useQPFile.TabIndex = 9;
            this.useQPFile.Text = "Use qp File";
            this.useQPFile.UseVisualStyleBackColor = true;
            this.useQPFile.CheckedChanged += new System.EventHandler(this.useQPFile_CheckedChanged);
            // 
            // x264BitrateQuantizer
            // 
            this.x264BitrateQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264BitrateQuantizer.Location = new System.Drawing.Point(242, 44);
            this.x264BitrateQuantizer.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.x264BitrateQuantizer.Name = "x264BitrateQuantizer";
            this.x264BitrateQuantizer.Size = new System.Drawing.Size(66, 20);
            this.x264BitrateQuantizer.TabIndex = 5;
            this.x264BitrateQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // logfile
            // 
            this.logfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.logfile.Location = new System.Drawing.Point(96, 83);
            this.logfile.Name = "logfile";
            this.logfile.ReadOnly = true;
            this.logfile.Size = new System.Drawing.Size(182, 20);
            this.logfile.TabIndex = 7;
            this.logfile.Text = "2pass.stats";
            this.logfile.TextChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264EncodingMode
            // 
            this.x264EncodingMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264EncodingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x264EncodingMode.Items.AddRange(new object[] {
            "ABR",
            "Const. Quantizer",
            "2pass - 1st pass",
            "2pass - 2nd pass",
            "Automated 2pass",
            "3pass - 1st pass",
            "3pass - 2nd pass",
            "3pass - 3rd pass",
            "Automated 3pass",
            "Const. Quality"});
            this.x264EncodingMode.Location = new System.Drawing.Point(187, 16);
            this.x264EncodingMode.Name = "x264EncodingMode";
            this.x264EncodingMode.Size = new System.Drawing.Size(121, 21);
            this.x264EncodingMode.TabIndex = 2;
            this.x264EncodingMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // logfileLabel
            // 
            this.logfileLabel.Location = new System.Drawing.Point(6, 82);
            this.logfileLabel.Margin = new System.Windows.Forms.Padding(3);
            this.logfileLabel.Name = "logfileLabel";
            this.logfileLabel.Padding = new System.Windows.Forms.Padding(3);
            this.logfileLabel.Size = new System.Drawing.Size(75, 23);
            this.logfileLabel.TabIndex = 6;
            this.logfileLabel.Text = "Logfile:";
            this.logfileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logfileOpenButton
            // 
            this.logfileOpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logfileOpenButton.Location = new System.Drawing.Point(284, 82);
            this.logfileOpenButton.Name = "logfileOpenButton";
            this.logfileOpenButton.Size = new System.Drawing.Size(24, 23);
            this.logfileOpenButton.TabIndex = 8;
            this.logfileOpenButton.Text = "...";
            this.logfileOpenButton.Click += new System.EventHandler(this.logfileOpenButton_Click);
            // 
            // x264Turbo
            // 
            this.x264Turbo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264Turbo.AutoSize = true;
            this.x264Turbo.Location = new System.Drawing.Point(126, 15);
            this.x264Turbo.Name = "x264Turbo";
            this.x264Turbo.Padding = new System.Windows.Forms.Padding(3);
            this.x264Turbo.Size = new System.Drawing.Size(60, 23);
            this.x264Turbo.TabIndex = 1;
            this.x264Turbo.Text = "Turbo";
            this.x264Turbo.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264EncodingModeLabel
            // 
            this.x264EncodingModeLabel.Location = new System.Drawing.Point(6, 15);
            this.x264EncodingModeLabel.Margin = new System.Windows.Forms.Padding(3);
            this.x264EncodingModeLabel.Name = "x264EncodingModeLabel";
            this.x264EncodingModeLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264EncodingModeLabel.Size = new System.Drawing.Size(109, 23);
            this.x264EncodingModeLabel.TabIndex = 0;
            this.x264EncodingModeLabel.Text = "Mode";
            this.x264EncodingModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264BitrateQuantizerLabel
            // 
            this.x264BitrateQuantizerLabel.Location = new System.Drawing.Point(6, 43);
            this.x264BitrateQuantizerLabel.Margin = new System.Windows.Forms.Padding(3);
            this.x264BitrateQuantizerLabel.Name = "x264BitrateQuantizerLabel";
            this.x264BitrateQuantizerLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264BitrateQuantizerLabel.Size = new System.Drawing.Size(109, 23);
            this.x264BitrateQuantizerLabel.TabIndex = 3;
            this.x264BitrateQuantizerLabel.Text = "Bitrate";
            this.x264BitrateQuantizerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264LosslessMode
            // 
            this.x264LosslessMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264LosslessMode.AutoSize = true;
            this.x264LosslessMode.Location = new System.Drawing.Point(126, 43);
            this.x264LosslessMode.Name = "x264LosslessMode";
            this.x264LosslessMode.Padding = new System.Windows.Forms.Padding(3);
            this.x264LosslessMode.Size = new System.Drawing.Size(72, 23);
            this.x264LosslessMode.TabIndex = 4;
            this.x264LosslessMode.Text = "Lossless";
            this.x264LosslessMode.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // rateControlTabPage
            // 
            this.rateControlTabPage.Controls.Add(this.x264OtherOptionsGroupbox);
            this.rateControlTabPage.Controls.Add(this.x264QuantOptionsGroupbox);
            this.rateControlTabPage.Controls.Add(this.x264RateControlMiscGroupbox);
            this.rateControlTabPage.Controls.Add(this.x264RCGroupbox);
            this.rateControlTabPage.Location = new System.Drawing.Point(4, 22);
            this.rateControlTabPage.Name = "rateControlTabPage";
            this.rateControlTabPage.Size = new System.Drawing.Size(502, 403);
            this.rateControlTabPage.TabIndex = 3;
            this.rateControlTabPage.Text = "RC and ME";
            this.rateControlTabPage.UseVisualStyleBackColor = true;
            // 
            // x264OtherOptionsGroupbox
            // 
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264SubpelRefinement);
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264SubpelRefinementLabel);
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264ChromaMe);
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264MERangeLabel);
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264METypeLabel);
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264METype);
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264MERange);
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264SCDSensitivity);
            this.x264OtherOptionsGroupbox.Controls.Add(this.x264SCDSensitivityLabel);
            this.x264OtherOptionsGroupbox.Location = new System.Drawing.Point(3, 209);
            this.x264OtherOptionsGroupbox.Name = "x264OtherOptionsGroupbox";
            this.x264OtherOptionsGroupbox.Size = new System.Drawing.Size(284, 166);
            this.x264OtherOptionsGroupbox.TabIndex = 4;
            this.x264OtherOptionsGroupbox.TabStop = false;
            this.x264OtherOptionsGroupbox.Text = "M.E.";
            // 
            // x264SubpelRefinement
            // 
            this.x264SubpelRefinement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264SubpelRefinement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x264SubpelRefinement.Items.AddRange(new object[] {
            "1 - QPel SAD",
            "2 - QPel SATD",
            "3 - HPel on MB then QPel",
            "4 - Always QPel",
            "5 - QPel & Bidir ME",
            "6 - RD on I/P frames",
            "7 - RD on all frames",
            "8 - RD refinement on I/P frames",
            "9 - RD refinement on all frames"});
            this.x264SubpelRefinement.Location = new System.Drawing.Point(125, 117);
            this.x264SubpelRefinement.Name = "x264SubpelRefinement";
            this.x264SubpelRefinement.Size = new System.Drawing.Size(154, 21);
            this.x264SubpelRefinement.TabIndex = 8;
            this.x264SubpelRefinement.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264SubpelRefinementLabel
            // 
            this.x264SubpelRefinementLabel.AutoSize = true;
            this.x264SubpelRefinementLabel.Location = new System.Drawing.Point(8, 117);
            this.x264SubpelRefinementLabel.Name = "x264SubpelRefinementLabel";
            this.x264SubpelRefinementLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264SubpelRefinementLabel.Size = new System.Drawing.Size(110, 19);
            this.x264SubpelRefinementLabel.TabIndex = 7;
            this.x264SubpelRefinementLabel.Text = "Subpixel Refinement";
            this.x264SubpelRefinementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264ChromaMe
            // 
            this.x264ChromaMe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.x264ChromaMe.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x264ChromaMe.Checked = true;
            this.x264ChromaMe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x264ChromaMe.Location = new System.Drawing.Point(8, 15);
            this.x264ChromaMe.Name = "x264ChromaMe";
            this.x264ChromaMe.Padding = new System.Windows.Forms.Padding(3);
            this.x264ChromaMe.Size = new System.Drawing.Size(271, 23);
            this.x264ChromaMe.TabIndex = 0;
            this.x264ChromaMe.Text = "Chroma M.E.";
            this.x264ChromaMe.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264MERangeLabel
            // 
            this.x264MERangeLabel.AutoSize = true;
            this.x264MERangeLabel.Location = new System.Drawing.Point(8, 40);
            this.x264MERangeLabel.Name = "x264MERangeLabel";
            this.x264MERangeLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264MERangeLabel.Size = new System.Drawing.Size(70, 19);
            this.x264MERangeLabel.TabIndex = 1;
            this.x264MERangeLabel.Text = "M.E. Range";
            this.x264MERangeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264METypeLabel
            // 
            this.x264METypeLabel.AutoSize = true;
            this.x264METypeLabel.Location = new System.Drawing.Point(8, 92);
            this.x264METypeLabel.Name = "x264METypeLabel";
            this.x264METypeLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264METypeLabel.Size = new System.Drawing.Size(81, 19);
            this.x264METypeLabel.TabIndex = 5;
            this.x264METypeLabel.Text = "M.E. Algorithm";
            this.x264METypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264METype
            // 
            this.x264METype.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264METype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x264METype.Items.AddRange(new object[] {
            "Diamond",
            "Hexagon",
            "Multi hex",
            "Exhaustive",
            "SATD Exhaustive"});
            this.x264METype.Location = new System.Drawing.Point(170, 91);
            this.x264METype.Name = "x264METype";
            this.x264METype.Size = new System.Drawing.Size(109, 21);
            this.x264METype.TabIndex = 6;
            this.x264METype.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264MERange
            // 
            this.x264MERange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264MERange.Location = new System.Drawing.Point(231, 40);
            this.x264MERange.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.x264MERange.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.x264MERange.Name = "x264MERange";
            this.x264MERange.Size = new System.Drawing.Size(48, 20);
            this.x264MERange.TabIndex = 2;
            this.x264MERange.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.x264MERange.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264SCDSensitivity
            // 
            this.x264SCDSensitivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264SCDSensitivity.Location = new System.Drawing.Point(231, 66);
            this.x264SCDSensitivity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.x264SCDSensitivity.Name = "x264SCDSensitivity";
            this.x264SCDSensitivity.Size = new System.Drawing.Size(48, 20);
            this.x264SCDSensitivity.TabIndex = 4;
            this.x264SCDSensitivity.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.x264SCDSensitivity.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264SCDSensitivityLabel
            // 
            this.x264SCDSensitivityLabel.AutoSize = true;
            this.x264SCDSensitivityLabel.Location = new System.Drawing.Point(8, 67);
            this.x264SCDSensitivityLabel.Name = "x264SCDSensitivityLabel";
            this.x264SCDSensitivityLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264SCDSensitivityLabel.Size = new System.Drawing.Size(134, 19);
            this.x264SCDSensitivityLabel.TabIndex = 3;
            this.x264SCDSensitivityLabel.Text = "Scene Change Sensitivity";
            this.x264SCDSensitivityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264QuantOptionsGroupbox
            // 
            this.x264QuantOptionsGroupbox.Controls.Add(this.PsyTrellisLabel);
            this.x264QuantOptionsGroupbox.Controls.Add(this.PsyTrellis);
            this.x264QuantOptionsGroupbox.Controls.Add(this.PsyRDLabel);
            this.x264QuantOptionsGroupbox.Controls.Add(this.PsyRD);
            this.x264QuantOptionsGroupbox.Controls.Add(this.x264MixedReferences);
            this.x264QuantOptionsGroupbox.Controls.Add(this.x264NumberOfRefFrames);
            this.x264QuantOptionsGroupbox.Controls.Add(this.x264CabacEnabled);
            this.x264QuantOptionsGroupbox.Controls.Add(this.noDCTDecimateOption);
            this.x264QuantOptionsGroupbox.Controls.Add(this.NoFastPSkip);
            this.x264QuantOptionsGroupbox.Controls.Add(this.trellis);
            this.x264QuantOptionsGroupbox.Controls.Add(this.x264NumberOfRefFramesLabel);
            this.x264QuantOptionsGroupbox.Controls.Add(this.trellisLabel);
            this.x264QuantOptionsGroupbox.Location = new System.Drawing.Point(293, 154);
            this.x264QuantOptionsGroupbox.Name = "x264QuantOptionsGroupbox";
            this.x264QuantOptionsGroupbox.Size = new System.Drawing.Size(206, 221);
            this.x264QuantOptionsGroupbox.TabIndex = 0;
            this.x264QuantOptionsGroupbox.TabStop = false;
            this.x264QuantOptionsGroupbox.Text = "Quant Options";
            // 
            // PsyTrellisLabel
            // 
            this.PsyTrellisLabel.AutoSize = true;
            this.PsyTrellisLabel.Location = new System.Drawing.Point(5, 147);
            this.PsyTrellisLabel.Name = "PsyTrellisLabel";
            this.PsyTrellisLabel.Padding = new System.Windows.Forms.Padding(3);
            this.PsyTrellisLabel.Size = new System.Drawing.Size(103, 19);
            this.PsyTrellisLabel.TabIndex = 11;
            this.PsyTrellisLabel.Text = "Psy-Trellis Strength";
            // 
            // PsyTrellis
            // 
            this.PsyTrellis.DecimalPlaces = 1;
            this.PsyTrellis.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.PsyTrellis.Location = new System.Drawing.Point(153, 147);
            this.PsyTrellis.Name = "PsyTrellis";
            this.PsyTrellis.Size = new System.Drawing.Size(48, 20);
            this.PsyTrellis.TabIndex = 10;
            this.PsyTrellis.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // PsyRDLabel
            // 
            this.PsyRDLabel.AutoSize = true;
            this.PsyRDLabel.Location = new System.Drawing.Point(5, 122);
            this.PsyRDLabel.Name = "PsyRDLabel";
            this.PsyRDLabel.Padding = new System.Windows.Forms.Padding(3);
            this.PsyRDLabel.Size = new System.Drawing.Size(92, 19);
            this.PsyRDLabel.TabIndex = 9;
            this.PsyRDLabel.Text = "Psy-RD Strength";
            // 
            // PsyRD
            // 
            this.PsyRD.DecimalPlaces = 1;
            this.PsyRD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.PsyRD.Location = new System.Drawing.Point(153, 121);
            this.PsyRD.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.PsyRD.Name = "PsyRD";
            this.PsyRD.Size = new System.Drawing.Size(48, 20);
            this.PsyRD.TabIndex = 8;
            this.PsyRD.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.PsyRD.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264MixedReferences
            // 
            this.x264MixedReferences.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x264MixedReferences.Enabled = false;
            this.x264MixedReferences.Location = new System.Drawing.Point(3, 66);
            this.x264MixedReferences.Name = "x264MixedReferences";
            this.x264MixedReferences.Padding = new System.Windows.Forms.Padding(3);
            this.x264MixedReferences.Size = new System.Drawing.Size(200, 24);
            this.x264MixedReferences.TabIndex = 4;
            this.x264MixedReferences.Text = "Mixed Reference frames";
            this.x264MixedReferences.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264NumberOfRefFrames
            // 
            this.x264NumberOfRefFrames.Location = new System.Drawing.Point(161, 43);
            this.x264NumberOfRefFrames.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.x264NumberOfRefFrames.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264NumberOfRefFrames.Name = "x264NumberOfRefFrames";
            this.x264NumberOfRefFrames.Size = new System.Drawing.Size(40, 20);
            this.x264NumberOfRefFrames.TabIndex = 3;
            this.x264NumberOfRefFrames.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264NumberOfRefFrames.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264CabacEnabled
            // 
            this.x264CabacEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x264CabacEnabled.Checked = true;
            this.x264CabacEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x264CabacEnabled.Location = new System.Drawing.Point(3, 18);
            this.x264CabacEnabled.Name = "x264CabacEnabled";
            this.x264CabacEnabled.Padding = new System.Windows.Forms.Padding(3);
            this.x264CabacEnabled.Size = new System.Drawing.Size(200, 23);
            this.x264CabacEnabled.TabIndex = 5;
            this.x264CabacEnabled.Text = "CABAC";
            this.x264CabacEnabled.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // noDCTDecimateOption
            // 
            this.noDCTDecimateOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.noDCTDecimateOption.Location = new System.Drawing.Point(5, 170);
            this.noDCTDecimateOption.Name = "noDCTDecimateOption";
            this.noDCTDecimateOption.Padding = new System.Windows.Forms.Padding(3);
            this.noDCTDecimateOption.Size = new System.Drawing.Size(198, 23);
            this.noDCTDecimateOption.TabIndex = 6;
            this.noDCTDecimateOption.Text = "No Dct Decimation";
            this.noDCTDecimateOption.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // NoFastPSkip
            // 
            this.NoFastPSkip.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NoFastPSkip.Location = new System.Drawing.Point(5, 193);
            this.NoFastPSkip.Name = "NoFastPSkip";
            this.NoFastPSkip.Padding = new System.Windows.Forms.Padding(3);
            this.NoFastPSkip.Size = new System.Drawing.Size(198, 23);
            this.NoFastPSkip.TabIndex = 7;
            this.NoFastPSkip.Text = "No Fast P-Skip";
            this.NoFastPSkip.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // trellis
            // 
            this.trellis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trellis.Items.AddRange(new object[] {
            "0 - None",
            "1 - Final MB",
            "2 - Always"});
            this.trellis.Location = new System.Drawing.Point(83, 94);
            this.trellis.Name = "trellis";
            this.trellis.Size = new System.Drawing.Size(118, 21);
            this.trellis.TabIndex = 1;
            this.trellis.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264NumberOfRefFramesLabel
            // 
            this.x264NumberOfRefFramesLabel.AutoSize = true;
            this.x264NumberOfRefFramesLabel.Location = new System.Drawing.Point(4, 44);
            this.x264NumberOfRefFramesLabel.Name = "x264NumberOfRefFramesLabel";
            this.x264NumberOfRefFramesLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264NumberOfRefFramesLabel.Size = new System.Drawing.Size(152, 19);
            this.x264NumberOfRefFramesLabel.TabIndex = 2;
            this.x264NumberOfRefFramesLabel.Text = "Number of Reference Frames";
            this.x264NumberOfRefFramesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trellisLabel
            // 
            this.trellisLabel.AutoSize = true;
            this.trellisLabel.Location = new System.Drawing.Point(5, 93);
            this.trellisLabel.Name = "trellisLabel";
            this.trellisLabel.Padding = new System.Windows.Forms.Padding(3);
            this.trellisLabel.Size = new System.Drawing.Size(40, 19);
            this.trellisLabel.TabIndex = 0;
            this.trellisLabel.Text = "Trellis";
            this.trellisLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264RateControlMiscGroupbox
            // 
            this.x264RateControlMiscGroupbox.Controls.Add(this.x264FullRange);
            this.x264RateControlMiscGroupbox.Controls.Add(this.interlaced);
            this.x264RateControlMiscGroupbox.Controls.Add(this.NoiseReduction);
            this.x264RateControlMiscGroupbox.Controls.Add(this.NoiseReductionLabel);
            this.x264RateControlMiscGroupbox.Controls.Add(this.x264KeyframeIntervalLabel);
            this.x264RateControlMiscGroupbox.Controls.Add(this.x264KeyframeInterval);
            this.x264RateControlMiscGroupbox.Controls.Add(this.x264MinGOPSize);
            this.x264RateControlMiscGroupbox.Controls.Add(this.x264MinGOPSizeLabel);
            this.x264RateControlMiscGroupbox.Location = new System.Drawing.Point(293, 3);
            this.x264RateControlMiscGroupbox.Name = "x264RateControlMiscGroupbox";
            this.x264RateControlMiscGroupbox.Size = new System.Drawing.Size(206, 145);
            this.x264RateControlMiscGroupbox.TabIndex = 1;
            this.x264RateControlMiscGroupbox.TabStop = false;
            this.x264RateControlMiscGroupbox.Text = "Misc";
            // 
            // x264FullRange
            // 
            this.x264FullRange.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x264FullRange.Location = new System.Drawing.Point(8, 116);
            this.x264FullRange.Name = "x264FullRange";
            this.x264FullRange.Size = new System.Drawing.Size(193, 24);
            this.x264FullRange.TabIndex = 7;
            this.x264FullRange.Text = "Full Range";
            this.x264FullRange.UseVisualStyleBackColor = true;
            this.x264FullRange.CheckedChanged += new System.EventHandler(this.x264FullRange_CheckedChanged);
            // 
            // interlaced
            // 
            this.interlaced.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.interlaced.Location = new System.Drawing.Point(8, 91);
            this.interlaced.Name = "interlaced";
            this.interlaced.Size = new System.Drawing.Size(193, 24);
            this.interlaced.TabIndex = 6;
            this.interlaced.Text = "Encode interlaced";
            this.interlaced.UseVisualStyleBackColor = true;
            this.interlaced.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // NoiseReduction
            // 
            this.NoiseReduction.Location = new System.Drawing.Point(150, 67);
            this.NoiseReduction.MaxLength = 6;
            this.NoiseReduction.Name = "NoiseReduction";
            this.NoiseReduction.Size = new System.Drawing.Size(48, 20);
            this.NoiseReduction.TabIndex = 5;
            this.NoiseReduction.TextChanged += new System.EventHandler(this.updateEvent);
            this.NoiseReduction.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // NoiseReductionLabel
            // 
            this.NoiseReductionLabel.AutoSize = true;
            this.NoiseReductionLabel.Location = new System.Drawing.Point(5, 67);
            this.NoiseReductionLabel.Name = "NoiseReductionLabel";
            this.NoiseReductionLabel.Padding = new System.Windows.Forms.Padding(3);
            this.NoiseReductionLabel.Size = new System.Drawing.Size(92, 19);
            this.NoiseReductionLabel.TabIndex = 4;
            this.NoiseReductionLabel.Text = "Noise Reduction";
            this.NoiseReductionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264KeyframeIntervalLabel
            // 
            this.x264KeyframeIntervalLabel.AutoSize = true;
            this.x264KeyframeIntervalLabel.Location = new System.Drawing.Point(5, 18);
            this.x264KeyframeIntervalLabel.Name = "x264KeyframeIntervalLabel";
            this.x264KeyframeIntervalLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264KeyframeIntervalLabel.Size = new System.Drawing.Size(95, 19);
            this.x264KeyframeIntervalLabel.TabIndex = 0;
            this.x264KeyframeIntervalLabel.Text = "Keyframe Interval";
            this.x264KeyframeIntervalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264KeyframeInterval
            // 
            this.x264KeyframeInterval.Location = new System.Drawing.Point(150, 17);
            this.x264KeyframeInterval.MaxLength = 6;
            this.x264KeyframeInterval.Name = "x264KeyframeInterval";
            this.x264KeyframeInterval.Size = new System.Drawing.Size(48, 20);
            this.x264KeyframeInterval.TabIndex = 1;
            this.x264KeyframeInterval.Text = "250";
            this.x264KeyframeInterval.TextChanged += new System.EventHandler(this.updateEvent);
            this.x264KeyframeInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // x264MinGOPSize
            // 
            this.x264MinGOPSize.Location = new System.Drawing.Point(150, 42);
            this.x264MinGOPSize.Name = "x264MinGOPSize";
            this.x264MinGOPSize.Size = new System.Drawing.Size(48, 20);
            this.x264MinGOPSize.TabIndex = 3;
            this.x264MinGOPSize.TextChanged += new System.EventHandler(this.updateEvent);
            this.x264MinGOPSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // x264MinGOPSizeLabel
            // 
            this.x264MinGOPSizeLabel.AutoSize = true;
            this.x264MinGOPSizeLabel.Location = new System.Drawing.Point(5, 43);
            this.x264MinGOPSizeLabel.Name = "x264MinGOPSizeLabel";
            this.x264MinGOPSizeLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264MinGOPSizeLabel.Size = new System.Drawing.Size(82, 19);
            this.x264MinGOPSizeLabel.TabIndex = 2;
            this.x264MinGOPSizeLabel.Text = "Min. GOP Size";
            this.x264MinGOPSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264RCGroupbox
            // 
            this.x264RCGroupbox.Controls.Add(this.x264RateTolLabel);
            this.x264RCGroupbox.Controls.Add(this.x264VBVInitialBuffer);
            this.x264RCGroupbox.Controls.Add(this.x264VBVInitialBufferLabel);
            this.x264RCGroupbox.Controls.Add(this.x264VBVMaxRate);
            this.x264RCGroupbox.Controls.Add(this.x264TempQuantBlur);
            this.x264RCGroupbox.Controls.Add(this.x264TempFrameComplexityBlur);
            this.x264RCGroupbox.Controls.Add(this.x264QuantizerCompression);
            this.x264RCGroupbox.Controls.Add(this.x264VBVBufferSize);
            this.x264RCGroupbox.Controls.Add(this.x264TempQuantBlurLabel);
            this.x264RCGroupbox.Controls.Add(this.x264TempFrameComplexityBlurLabel);
            this.x264RCGroupbox.Controls.Add(this.x264QuantizerCompressionLabel);
            this.x264RCGroupbox.Controls.Add(this.x264VBVMaxRateLabel);
            this.x264RCGroupbox.Controls.Add(this.x264VBVBufferSizeLabel);
            this.x264RCGroupbox.Controls.Add(this.x264RateTol);
            this.x264RCGroupbox.Location = new System.Drawing.Point(0, 3);
            this.x264RCGroupbox.Name = "x264RCGroupbox";
            this.x264RCGroupbox.Size = new System.Drawing.Size(287, 200);
            this.x264RCGroupbox.TabIndex = 0;
            this.x264RCGroupbox.TabStop = false;
            this.x264RCGroupbox.Text = "Rate Control";
            // 
            // x264RateTolLabel
            // 
            this.x264RateTolLabel.AutoSize = true;
            this.x264RateTolLabel.Location = new System.Drawing.Point(8, 93);
            this.x264RateTolLabel.Name = "x264RateTolLabel";
            this.x264RateTolLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264RateTolLabel.Size = new System.Drawing.Size(88, 19);
            this.x264RateTolLabel.TabIndex = 6;
            this.x264RateTolLabel.Text = "Bitrate Variance";
            this.x264RateTolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264VBVInitialBuffer
            // 
            this.x264VBVInitialBuffer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264VBVInitialBuffer.DecimalPlaces = 1;
            this.x264VBVInitialBuffer.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x264VBVInitialBuffer.Location = new System.Drawing.Point(229, 67);
            this.x264VBVInitialBuffer.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264VBVInitialBuffer.Name = "x264VBVInitialBuffer";
            this.x264VBVInitialBuffer.Size = new System.Drawing.Size(48, 20);
            this.x264VBVInitialBuffer.TabIndex = 5;
            this.x264VBVInitialBuffer.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            this.x264VBVInitialBuffer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264VBVInitialBufferLabel
            // 
            this.x264VBVInitialBufferLabel.AutoSize = true;
            this.x264VBVInitialBufferLabel.Location = new System.Drawing.Point(8, 68);
            this.x264VBVInitialBufferLabel.Name = "x264VBVInitialBufferLabel";
            this.x264VBVInitialBufferLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264VBVInitialBufferLabel.Size = new System.Drawing.Size(92, 19);
            this.x264VBVInitialBufferLabel.TabIndex = 4;
            this.x264VBVInitialBufferLabel.Text = "VBV Initial Buffer";
            this.x264VBVInitialBufferLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264VBVMaxRate
            // 
            this.x264VBVMaxRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264VBVMaxRate.Enabled = false;
            this.x264VBVMaxRate.Location = new System.Drawing.Point(229, 42);
            this.x264VBVMaxRate.MaxLength = 5;
            this.x264VBVMaxRate.Name = "x264VBVMaxRate";
            this.x264VBVMaxRate.Size = new System.Drawing.Size(48, 20);
            this.x264VBVMaxRate.TabIndex = 3;
            this.x264VBVMaxRate.TextChanged += new System.EventHandler(this.updateEvent);
            this.x264VBVMaxRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // x264TempQuantBlur
            // 
            this.x264TempQuantBlur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264TempQuantBlur.DecimalPlaces = 1;
            this.x264TempQuantBlur.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.x264TempQuantBlur.Location = new System.Drawing.Point(229, 167);
            this.x264TempQuantBlur.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.x264TempQuantBlur.Name = "x264TempQuantBlur";
            this.x264TempQuantBlur.Size = new System.Drawing.Size(48, 20);
            this.x264TempQuantBlur.TabIndex = 13;
            this.x264TempQuantBlur.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.x264TempQuantBlur.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264TempFrameComplexityBlur
            // 
            this.x264TempFrameComplexityBlur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264TempFrameComplexityBlur.Location = new System.Drawing.Point(229, 142);
            this.x264TempFrameComplexityBlur.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.x264TempFrameComplexityBlur.Name = "x264TempFrameComplexityBlur";
            this.x264TempFrameComplexityBlur.Size = new System.Drawing.Size(48, 20);
            this.x264TempFrameComplexityBlur.TabIndex = 11;
            this.x264TempFrameComplexityBlur.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.x264TempFrameComplexityBlur.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264QuantizerCompression
            // 
            this.x264QuantizerCompression.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264QuantizerCompression.DecimalPlaces = 1;
            this.x264QuantizerCompression.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x264QuantizerCompression.Location = new System.Drawing.Point(229, 117);
            this.x264QuantizerCompression.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264QuantizerCompression.Name = "x264QuantizerCompression";
            this.x264QuantizerCompression.Size = new System.Drawing.Size(48, 20);
            this.x264QuantizerCompression.TabIndex = 9;
            this.x264QuantizerCompression.Value = new decimal(new int[] {
            6,
            0,
            0,
            65536});
            this.x264QuantizerCompression.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264VBVBufferSize
            // 
            this.x264VBVBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264VBVBufferSize.Location = new System.Drawing.Point(229, 17);
            this.x264VBVBufferSize.MaxLength = 58;
            this.x264VBVBufferSize.Name = "x264VBVBufferSize";
            this.x264VBVBufferSize.Size = new System.Drawing.Size(48, 20);
            this.x264VBVBufferSize.TabIndex = 1;
            this.x264VBVBufferSize.TextChanged += new System.EventHandler(this.updateEvent);
            this.x264VBVBufferSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // x264TempQuantBlurLabel
            // 
            this.x264TempQuantBlurLabel.AutoSize = true;
            this.x264TempQuantBlurLabel.Location = new System.Drawing.Point(8, 168);
            this.x264TempQuantBlurLabel.Name = "x264TempQuantBlurLabel";
            this.x264TempQuantBlurLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264TempQuantBlurLabel.Size = new System.Drawing.Size(149, 19);
            this.x264TempQuantBlurLabel.TabIndex = 12;
            this.x264TempQuantBlurLabel.Text = "Temp. Blur of Quant after CC";
            this.x264TempQuantBlurLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264TempFrameComplexityBlurLabel
            // 
            this.x264TempFrameComplexityBlurLabel.AutoSize = true;
            this.x264TempFrameComplexityBlurLabel.Location = new System.Drawing.Point(8, 143);
            this.x264TempFrameComplexityBlurLabel.Name = "x264TempFrameComplexityBlurLabel";
            this.x264TempFrameComplexityBlurLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264TempFrameComplexityBlurLabel.Size = new System.Drawing.Size(180, 19);
            this.x264TempFrameComplexityBlurLabel.TabIndex = 10;
            this.x264TempFrameComplexityBlurLabel.Text = "Temp. Blur of est. Frame complexity";
            this.x264TempFrameComplexityBlurLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264QuantizerCompressionLabel
            // 
            this.x264QuantizerCompressionLabel.AutoSize = true;
            this.x264QuantizerCompressionLabel.Location = new System.Drawing.Point(8, 118);
            this.x264QuantizerCompressionLabel.Name = "x264QuantizerCompressionLabel";
            this.x264QuantizerCompressionLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264QuantizerCompressionLabel.Size = new System.Drawing.Size(121, 19);
            this.x264QuantizerCompressionLabel.TabIndex = 8;
            this.x264QuantizerCompressionLabel.Text = "Quantizer Compression";
            this.x264QuantizerCompressionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264VBVMaxRateLabel
            // 
            this.x264VBVMaxRateLabel.AutoSize = true;
            this.x264VBVMaxRateLabel.Enabled = false;
            this.x264VBVMaxRateLabel.Location = new System.Drawing.Point(8, 43);
            this.x264VBVMaxRateLabel.Name = "x264VBVMaxRateLabel";
            this.x264VBVMaxRateLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264VBVMaxRateLabel.Size = new System.Drawing.Size(114, 19);
            this.x264VBVMaxRateLabel.TabIndex = 2;
            this.x264VBVMaxRateLabel.Text = "VBV Maximum Bitrate";
            this.x264VBVMaxRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264VBVBufferSizeLabel
            // 
            this.x264VBVBufferSizeLabel.AutoSize = true;
            this.x264VBVBufferSizeLabel.Location = new System.Drawing.Point(8, 18);
            this.x264VBVBufferSizeLabel.Name = "x264VBVBufferSizeLabel";
            this.x264VBVBufferSizeLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264VBVBufferSizeLabel.Size = new System.Drawing.Size(88, 19);
            this.x264VBVBufferSizeLabel.TabIndex = 0;
            this.x264VBVBufferSizeLabel.Text = "VBV Buffer Size";
            this.x264VBVBufferSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264RateTol
            // 
            this.x264RateTol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264RateTol.DecimalPlaces = 1;
            this.x264RateTol.Location = new System.Drawing.Point(229, 92);
            this.x264RateTol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x264RateTol.Name = "x264RateTol";
            this.x264RateTol.Size = new System.Drawing.Size(48, 20);
            this.x264RateTol.TabIndex = 7;
            this.x264RateTol.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.x264RateTol.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // quantizationTabPage
            // 
            this.quantizationTabPage.Controls.Add(this.gbx264CustomCmd);
            this.quantizationTabPage.Controls.Add(this.gbAQ);
            this.quantizationTabPage.Controls.Add(this.x264GeneralBFramesgGroupbox);
            this.quantizationTabPage.Controls.Add(this.quantizerMatrixGroupbox);
            this.quantizationTabPage.Controls.Add(this.x264MBGroupbox);
            this.quantizationTabPage.Controls.Add(this.x264QuantizerGroupBox);
            this.quantizationTabPage.Location = new System.Drawing.Point(4, 22);
            this.quantizationTabPage.Name = "quantizationTabPage";
            this.quantizationTabPage.Size = new System.Drawing.Size(502, 403);
            this.quantizationTabPage.TabIndex = 4;
            this.quantizationTabPage.Text = "Advanced";
            this.quantizationTabPage.UseVisualStyleBackColor = true;
            // 
            // gbx264CustomCmd
            // 
            this.gbx264CustomCmd.Controls.Add(this.customCommandlineOptions);
            this.gbx264CustomCmd.Location = new System.Drawing.Point(0, 341);
            this.gbx264CustomCmd.Name = "gbx264CustomCmd";
            this.gbx264CustomCmd.Size = new System.Drawing.Size(499, 58);
            this.gbx264CustomCmd.TabIndex = 8;
            this.gbx264CustomCmd.TabStop = false;
            this.gbx264CustomCmd.Text = "Custom Command Line";
            // 
            // customCommandlineOptions
            // 
            this.customCommandlineOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customCommandlineOptions.Location = new System.Drawing.Point(12, 25);
            this.customCommandlineOptions.Name = "customCommandlineOptions";
            this.customCommandlineOptions.Size = new System.Drawing.Size(473, 20);
            this.customCommandlineOptions.TabIndex = 0;
            this.customCommandlineOptions.TextChanged += new System.EventHandler(this.updateEvent);
            // 
            // gbAQ
            // 
            this.gbAQ.Controls.Add(this.numAQStrength);
            this.gbAQ.Controls.Add(this.lbAQStrength);
            this.gbAQ.Controls.Add(this.cbAQMode);
            this.gbAQ.Controls.Add(this.lbAQMode);
            this.gbAQ.Location = new System.Drawing.Point(0, 192);
            this.gbAQ.Name = "gbAQ";
            this.gbAQ.Size = new System.Drawing.Size(291, 78);
            this.gbAQ.TabIndex = 7;
            this.gbAQ.TabStop = false;
            this.gbAQ.Text = "Adaptive Quantizers";
            // 
            // numAQStrength
            // 
            this.numAQStrength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numAQStrength.DecimalPlaces = 1;
            this.numAQStrength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numAQStrength.Location = new System.Drawing.Point(229, 46);
            this.numAQStrength.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numAQStrength.Name = "numAQStrength";
            this.numAQStrength.Size = new System.Drawing.Size(48, 20);
            this.numAQStrength.TabIndex = 3;
            this.numAQStrength.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numAQStrength.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbAQStrength
            // 
            this.lbAQStrength.AutoSize = true;
            this.lbAQStrength.Location = new System.Drawing.Point(12, 48);
            this.lbAQStrength.Name = "lbAQStrength";
            this.lbAQStrength.Size = new System.Drawing.Size(50, 13);
            this.lbAQStrength.TabIndex = 2;
            this.lbAQStrength.Text = "Strength ";
            this.lbAQStrength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbAQMode
            // 
            this.cbAQMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAQMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAQMode.FormattingEnabled = true;
            this.cbAQMode.Items.AddRange(new object[] {
            "Disabled",
            "Variance AQ (complexity mask)",
            "Auto-variance AQ (experimental)"});
            this.cbAQMode.Location = new System.Drawing.Point(48, 19);
            this.cbAQMode.Name = "cbAQMode";
            this.cbAQMode.Size = new System.Drawing.Size(229, 21);
            this.cbAQMode.TabIndex = 1;
            this.cbAQMode.SelectedIndexChanged += new System.EventHandler(this.cbAQMode_SelectedIndexChanged);
            // 
            // lbAQMode
            // 
            this.lbAQMode.AutoSize = true;
            this.lbAQMode.Location = new System.Drawing.Point(12, 22);
            this.lbAQMode.Name = "lbAQMode";
            this.lbAQMode.Size = new System.Drawing.Size(34, 13);
            this.lbAQMode.TabIndex = 0;
            this.lbAQMode.Text = "Mode";
            this.lbAQMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264GeneralBFramesgGroupbox
            // 
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264AdaptiveBframesLabel);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264NewAdaptiveBframes);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264BframePredictionMode);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264BframeBias);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264WeightedBPrediction);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264BframeBiasLabel);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264BframePredictionModeLabel);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264NumberOfBFramesLabel);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264NumberOfBFrames);
            this.x264GeneralBFramesgGroupbox.Controls.Add(this.x264PyramidBframes);
            this.x264GeneralBFramesgGroupbox.Location = new System.Drawing.Point(297, 131);
            this.x264GeneralBFramesgGroupbox.Name = "x264GeneralBFramesgGroupbox";
            this.x264GeneralBFramesgGroupbox.Size = new System.Drawing.Size(202, 204);
            this.x264GeneralBFramesgGroupbox.TabIndex = 6;
            this.x264GeneralBFramesgGroupbox.TabStop = false;
            this.x264GeneralBFramesgGroupbox.Text = "B-Frames";
            // 
            // x264AdaptiveBframesLabel
            // 
            this.x264AdaptiveBframesLabel.AutoSize = true;
            this.x264AdaptiveBframesLabel.Location = new System.Drawing.Point(6, 48);
            this.x264AdaptiveBframesLabel.Name = "x264AdaptiveBframesLabel";
            this.x264AdaptiveBframesLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264AdaptiveBframesLabel.Size = new System.Drawing.Size(102, 19);
            this.x264AdaptiveBframesLabel.TabIndex = 12;
            this.x264AdaptiveBframesLabel.Text = "Adaptive B-Frames";
            // 
            // x264NewAdaptiveBframes
            // 
            this.x264NewAdaptiveBframes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x264NewAdaptiveBframes.Items.AddRange(new object[] {
            "0-Off",
            "1-Fast",
            "2-Optimal"});
            this.x264NewAdaptiveBframes.Location = new System.Drawing.Point(120, 47);
            this.x264NewAdaptiveBframes.Name = "x264NewAdaptiveBframes";
            this.x264NewAdaptiveBframes.Size = new System.Drawing.Size(76, 21);
            this.x264NewAdaptiveBframes.TabIndex = 11;
            this.x264NewAdaptiveBframes.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264BframePredictionMode
            // 
            this.x264BframePredictionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x264BframePredictionMode.Items.AddRange(new object[] {
            "None",
            "Spatial",
            "Temporal",
            "Auto"});
            this.x264BframePredictionMode.Location = new System.Drawing.Point(120, 141);
            this.x264BframePredictionMode.Name = "x264BframePredictionMode";
            this.x264BframePredictionMode.Size = new System.Drawing.Size(76, 21);
            this.x264BframePredictionMode.TabIndex = 8;
            this.x264BframePredictionMode.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264BframeBias
            // 
            this.x264BframeBias.Location = new System.Drawing.Point(149, 175);
            this.x264BframeBias.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.x264BframeBias.Name = "x264BframeBias";
            this.x264BframeBias.Size = new System.Drawing.Size(47, 20);
            this.x264BframeBias.TabIndex = 10;
            this.x264BframeBias.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264WeightedBPrediction
            // 
            this.x264WeightedBPrediction.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x264WeightedBPrediction.Enabled = false;
            this.x264WeightedBPrediction.Location = new System.Drawing.Point(4, 104);
            this.x264WeightedBPrediction.Name = "x264WeightedBPrediction";
            this.x264WeightedBPrediction.Padding = new System.Windows.Forms.Padding(3);
            this.x264WeightedBPrediction.Size = new System.Drawing.Size(192, 23);
            this.x264WeightedBPrediction.TabIndex = 5;
            this.x264WeightedBPrediction.Text = "Weighted B-Prediction";
            this.x264WeightedBPrediction.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264BframeBiasLabel
            // 
            this.x264BframeBiasLabel.AutoSize = true;
            this.x264BframeBiasLabel.Location = new System.Drawing.Point(6, 174);
            this.x264BframeBiasLabel.Name = "x264BframeBiasLabel";
            this.x264BframeBiasLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264BframeBiasLabel.Size = new System.Drawing.Size(71, 19);
            this.x264BframeBiasLabel.TabIndex = 9;
            this.x264BframeBiasLabel.Text = "B-frame bias";
            this.x264BframeBiasLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264BframePredictionModeLabel
            // 
            this.x264BframePredictionModeLabel.AutoSize = true;
            this.x264BframePredictionModeLabel.Location = new System.Drawing.Point(6, 141);
            this.x264BframePredictionModeLabel.Name = "x264BframePredictionModeLabel";
            this.x264BframePredictionModeLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264BframePredictionModeLabel.Size = new System.Drawing.Size(78, 19);
            this.x264BframePredictionModeLabel.TabIndex = 7;
            this.x264BframePredictionModeLabel.Text = "B-frame mode";
            this.x264BframePredictionModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264NumberOfBFramesLabel
            // 
            this.x264NumberOfBFramesLabel.AutoSize = true;
            this.x264NumberOfBFramesLabel.Location = new System.Drawing.Point(6, 19);
            this.x264NumberOfBFramesLabel.Name = "x264NumberOfBFramesLabel";
            this.x264NumberOfBFramesLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264NumberOfBFramesLabel.Size = new System.Drawing.Size(106, 19);
            this.x264NumberOfBFramesLabel.TabIndex = 0;
            this.x264NumberOfBFramesLabel.Text = "Number of B-frames";
            this.x264NumberOfBFramesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264NumberOfBFrames
            // 
            this.x264NumberOfBFrames.Location = new System.Drawing.Point(149, 20);
            this.x264NumberOfBFrames.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.x264NumberOfBFrames.Name = "x264NumberOfBFrames";
            this.x264NumberOfBFrames.Size = new System.Drawing.Size(47, 20);
            this.x264NumberOfBFrames.TabIndex = 1;
            this.x264NumberOfBFrames.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.x264NumberOfBFrames.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264PyramidBframes
            // 
            this.x264PyramidBframes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.x264PyramidBframes.Enabled = false;
            this.x264PyramidBframes.Location = new System.Drawing.Point(4, 75);
            this.x264PyramidBframes.Name = "x264PyramidBframes";
            this.x264PyramidBframes.Padding = new System.Windows.Forms.Padding(3);
            this.x264PyramidBframes.Size = new System.Drawing.Size(192, 23);
            this.x264PyramidBframes.TabIndex = 3;
            this.x264PyramidBframes.Text = "B-Pyramid";
            this.x264PyramidBframes.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // quantizerMatrixGroupbox
            // 
            this.quantizerMatrixGroupbox.Controls.Add(this.cqmComboBox1);
            this.quantizerMatrixGroupbox.Location = new System.Drawing.Point(0, 276);
            this.quantizerMatrixGroupbox.Name = "quantizerMatrixGroupbox";
            this.quantizerMatrixGroupbox.Size = new System.Drawing.Size(291, 59);
            this.quantizerMatrixGroupbox.TabIndex = 2;
            this.quantizerMatrixGroupbox.TabStop = false;
            this.quantizerMatrixGroupbox.Text = "Quantizer Matrices";
            // 
            // cqmComboBox1
            // 
            this.cqmComboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cqmComboBox1.Filter = "Quantizer matrix files (*.cfg)|*.cfg|All Files (*.*)|*.*";
            this.cqmComboBox1.Location = new System.Drawing.Point(12, 19);
            this.cqmComboBox1.MaximumSize = new System.Drawing.Size(1000, 29);
            this.cqmComboBox1.MinimumSize = new System.Drawing.Size(64, 29);
            this.cqmComboBox1.Name = "cqmComboBox1";
            this.cqmComboBox1.SelectedIndex = 0;
            this.cqmComboBox1.Size = new System.Drawing.Size(265, 29);
            this.cqmComboBox1.TabIndex = 5;
            this.cqmComboBox1.SelectionChanged += new MeGUI.StringChanged(this.cqmComboBox1_SelectionChanged);
            // 
            // x264MBGroupbox
            // 
            this.x264MBGroupbox.Controls.Add(this.label1);
            this.x264MBGroupbox.Controls.Add(this.macroblockOptions);
            this.x264MBGroupbox.Controls.Add(this.adaptiveDCT);
            this.x264MBGroupbox.Controls.Add(this.x264I4x4mv);
            this.x264MBGroupbox.Controls.Add(this.x264I8x8mv);
            this.x264MBGroupbox.Controls.Add(this.x264P4x4mv);
            this.x264MBGroupbox.Controls.Add(this.x264B8x8mv);
            this.x264MBGroupbox.Controls.Add(this.x264P8x8mv);
            this.x264MBGroupbox.Location = new System.Drawing.Point(297, 3);
            this.x264MBGroupbox.Name = "x264MBGroupbox";
            this.x264MBGroupbox.Size = new System.Drawing.Size(202, 123);
            this.x264MBGroupbox.TabIndex = 1;
            this.x264MBGroupbox.TabStop = false;
            this.x264MBGroupbox.Text = "Macroblock Options";
            // 
            // macroblockOptions
            // 
            this.macroblockOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.macroblockOptions.Items.AddRange(new object[] {
            "All",
            "None",
            "Custom",
            "Default"});
            this.macroblockOptions.Location = new System.Drawing.Point(66, 16);
            this.macroblockOptions.Name = "macroblockOptions";
            this.macroblockOptions.Size = new System.Drawing.Size(111, 21);
            this.macroblockOptions.TabIndex = 0;
            this.macroblockOptions.SelectedIndexChanged += new System.EventHandler(this.updateEvent);
            // 
            // adaptiveDCT
            // 
            this.adaptiveDCT.Checked = true;
            this.adaptiveDCT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.adaptiveDCT.Location = new System.Drawing.Point(9, 40);
            this.adaptiveDCT.Name = "adaptiveDCT";
            this.adaptiveDCT.Size = new System.Drawing.Size(104, 24);
            this.adaptiveDCT.TabIndex = 1;
            this.adaptiveDCT.Text = "Adaptive DCT";
            this.adaptiveDCT.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264I4x4mv
            // 
            this.x264I4x4mv.Checked = true;
            this.x264I4x4mv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x264I4x4mv.Location = new System.Drawing.Point(9, 67);
            this.x264I4x4mv.Name = "x264I4x4mv";
            this.x264I4x4mv.Size = new System.Drawing.Size(56, 24);
            this.x264I4x4mv.TabIndex = 2;
            this.x264I4x4mv.Text = "I4x4";
            this.x264I4x4mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264I8x8mv
            // 
            this.x264I8x8mv.Checked = true;
            this.x264I8x8mv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x264I8x8mv.Location = new System.Drawing.Point(9, 93);
            this.x264I8x8mv.Name = "x264I8x8mv";
            this.x264I8x8mv.Size = new System.Drawing.Size(56, 24);
            this.x264I8x8mv.TabIndex = 4;
            this.x264I8x8mv.Text = "I8x8";
            this.x264I8x8mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264P4x4mv
            // 
            this.x264P4x4mv.Location = new System.Drawing.Point(66, 67);
            this.x264P4x4mv.Name = "x264P4x4mv";
            this.x264P4x4mv.Size = new System.Drawing.Size(64, 24);
            this.x264P4x4mv.TabIndex = 3;
            this.x264P4x4mv.Text = "P4x4";
            this.x264P4x4mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264B8x8mv
            // 
            this.x264B8x8mv.Checked = true;
            this.x264B8x8mv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x264B8x8mv.Location = new System.Drawing.Point(121, 93);
            this.x264B8x8mv.Name = "x264B8x8mv";
            this.x264B8x8mv.Size = new System.Drawing.Size(56, 24);
            this.x264B8x8mv.TabIndex = 6;
            this.x264B8x8mv.Text = "B8x8";
            this.x264B8x8mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264P8x8mv
            // 
            this.x264P8x8mv.Checked = true;
            this.x264P8x8mv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x264P8x8mv.Location = new System.Drawing.Point(66, 93);
            this.x264P8x8mv.Name = "x264P8x8mv";
            this.x264P8x8mv.Size = new System.Drawing.Size(64, 24);
            this.x264P8x8mv.TabIndex = 5;
            this.x264P8x8mv.Text = "P8x8";
            this.x264P8x8mv.CheckedChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264QuantizerGroupBox
            // 
            this.x264QuantizerGroupBox.Controls.Add(this.deadzoneIntra);
            this.x264QuantizerGroupBox.Controls.Add(this.deadzoneInter);
            this.x264QuantizerGroupBox.Controls.Add(this.lbx264DeadZones);
            this.x264QuantizerGroupBox.Controls.Add(this.x264PBFrameFactor);
            this.x264QuantizerGroupBox.Controls.Add(this.x264IPFrameFactor);
            this.x264QuantizerGroupBox.Controls.Add(this.lbQuantizersRatio);
            this.x264QuantizerGroupBox.Controls.Add(this.x264CreditsQuantizer);
            this.x264QuantizerGroupBox.Controls.Add(this.x264CreditsQuantizerLabel);
            this.x264QuantizerGroupBox.Controls.Add(this.x264ChromaQPOffset);
            this.x264QuantizerGroupBox.Controls.Add(this.x264ChromaQPOffsetLabel);
            this.x264QuantizerGroupBox.Controls.Add(this.x264MaxQuantDelta);
            this.x264QuantizerGroupBox.Controls.Add(this.x264MaximumQuantizer);
            this.x264QuantizerGroupBox.Controls.Add(this.x264MinimimQuantizer);
            this.x264QuantizerGroupBox.Controls.Add(this.x264MinimimQuantizerLabel);
            this.x264QuantizerGroupBox.Location = new System.Drawing.Point(0, 3);
            this.x264QuantizerGroupBox.Name = "x264QuantizerGroupBox";
            this.x264QuantizerGroupBox.Size = new System.Drawing.Size(291, 183);
            this.x264QuantizerGroupBox.TabIndex = 0;
            this.x264QuantizerGroupBox.TabStop = false;
            this.x264QuantizerGroupBox.Text = "Quantizers";
            // 
            // deadzoneIntra
            // 
            this.deadzoneIntra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deadzoneIntra.Location = new System.Drawing.Point(229, 84);
            this.deadzoneIntra.Name = "deadzoneIntra";
            this.deadzoneIntra.Size = new System.Drawing.Size(48, 20);
            this.deadzoneIntra.TabIndex = 17;
            this.deadzoneIntra.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // deadzoneInter
            // 
            this.deadzoneInter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deadzoneInter.Location = new System.Drawing.Point(175, 84);
            this.deadzoneInter.Name = "deadzoneInter";
            this.deadzoneInter.Size = new System.Drawing.Size(48, 20);
            this.deadzoneInter.TabIndex = 15;
            this.deadzoneInter.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbx264DeadZones
            // 
            this.lbx264DeadZones.AutoSize = true;
            this.lbx264DeadZones.Location = new System.Drawing.Point(12, 86);
            this.lbx264DeadZones.Name = "lbx264DeadZones";
            this.lbx264DeadZones.Size = new System.Drawing.Size(123, 13);
            this.lbx264DeadZones.TabIndex = 21;
            this.lbx264DeadZones.Text = "Deadzones (Inter / Intra)";
            this.lbx264DeadZones.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264PBFrameFactor
            // 
            this.x264PBFrameFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264PBFrameFactor.DecimalPlaces = 1;
            this.x264PBFrameFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x264PBFrameFactor.Location = new System.Drawing.Point(229, 49);
            this.x264PBFrameFactor.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.x264PBFrameFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264PBFrameFactor.Name = "x264PBFrameFactor";
            this.x264PBFrameFactor.Size = new System.Drawing.Size(48, 20);
            this.x264PBFrameFactor.TabIndex = 20;
            this.x264PBFrameFactor.Value = new decimal(new int[] {
            13,
            0,
            0,
            65536});
            this.x264PBFrameFactor.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264IPFrameFactor
            // 
            this.x264IPFrameFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264IPFrameFactor.DecimalPlaces = 1;
            this.x264IPFrameFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.x264IPFrameFactor.Location = new System.Drawing.Point(175, 49);
            this.x264IPFrameFactor.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.x264IPFrameFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264IPFrameFactor.Name = "x264IPFrameFactor";
            this.x264IPFrameFactor.Size = new System.Drawing.Size(48, 20);
            this.x264IPFrameFactor.TabIndex = 19;
            this.x264IPFrameFactor.Value = new decimal(new int[] {
            14,
            0,
            0,
            65536});
            this.x264IPFrameFactor.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // lbQuantizersRatio
            // 
            this.lbQuantizersRatio.AutoSize = true;
            this.lbQuantizersRatio.Location = new System.Drawing.Point(12, 51);
            this.lbQuantizersRatio.Name = "lbQuantizersRatio";
            this.lbQuantizersRatio.Size = new System.Drawing.Size(135, 13);
            this.lbQuantizersRatio.TabIndex = 18;
            this.lbQuantizersRatio.Text = "Quantizers Ratio (I:P / P:B)";
            this.lbQuantizersRatio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264CreditsQuantizer
            // 
            this.x264CreditsQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264CreditsQuantizer.Location = new System.Drawing.Point(229, 149);
            this.x264CreditsQuantizer.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.x264CreditsQuantizer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264CreditsQuantizer.Name = "x264CreditsQuantizer";
            this.x264CreditsQuantizer.Size = new System.Drawing.Size(48, 20);
            this.x264CreditsQuantizer.TabIndex = 7;
            this.x264CreditsQuantizer.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.x264CreditsQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264CreditsQuantizerLabel
            // 
            this.x264CreditsQuantizerLabel.Location = new System.Drawing.Point(9, 149);
            this.x264CreditsQuantizerLabel.Name = "x264CreditsQuantizerLabel";
            this.x264CreditsQuantizerLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264CreditsQuantizerLabel.Size = new System.Drawing.Size(122, 17);
            this.x264CreditsQuantizerLabel.TabIndex = 6;
            this.x264CreditsQuantizerLabel.Text = "Credits Quantizer";
            this.x264CreditsQuantizerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264ChromaQPOffset
            // 
            this.x264ChromaQPOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264ChromaQPOffset.Location = new System.Drawing.Point(229, 119);
            this.x264ChromaQPOffset.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.x264ChromaQPOffset.Minimum = new decimal(new int[] {
            12,
            0,
            0,
            -2147483648});
            this.x264ChromaQPOffset.Name = "x264ChromaQPOffset";
            this.x264ChromaQPOffset.Size = new System.Drawing.Size(48, 20);
            this.x264ChromaQPOffset.TabIndex = 13;
            this.x264ChromaQPOffset.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264ChromaQPOffsetLabel
            // 
            this.x264ChromaQPOffsetLabel.Location = new System.Drawing.Point(9, 119);
            this.x264ChromaQPOffsetLabel.Name = "x264ChromaQPOffsetLabel";
            this.x264ChromaQPOffsetLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264ChromaQPOffsetLabel.Size = new System.Drawing.Size(122, 17);
            this.x264ChromaQPOffsetLabel.TabIndex = 12;
            this.x264ChromaQPOffsetLabel.Text = "Chroma QP Offset";
            this.x264ChromaQPOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // x264MaxQuantDelta
            // 
            this.x264MaxQuantDelta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264MaxQuantDelta.Location = new System.Drawing.Point(229, 17);
            this.x264MaxQuantDelta.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.x264MaxQuantDelta.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264MaxQuantDelta.Name = "x264MaxQuantDelta";
            this.x264MaxQuantDelta.Size = new System.Drawing.Size(48, 20);
            this.x264MaxQuantDelta.TabIndex = 5;
            this.x264MaxQuantDelta.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.x264MaxQuantDelta.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264MaximumQuantizer
            // 
            this.x264MaximumQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264MaximumQuantizer.Location = new System.Drawing.Point(175, 17);
            this.x264MaximumQuantizer.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.x264MaximumQuantizer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264MaximumQuantizer.Name = "x264MaximumQuantizer";
            this.x264MaximumQuantizer.Size = new System.Drawing.Size(48, 20);
            this.x264MaximumQuantizer.TabIndex = 3;
            this.x264MaximumQuantizer.Value = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.x264MaximumQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264MinimimQuantizer
            // 
            this.x264MinimimQuantizer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264MinimimQuantizer.Location = new System.Drawing.Point(121, 17);
            this.x264MinimimQuantizer.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.x264MinimimQuantizer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.x264MinimimQuantizer.Name = "x264MinimimQuantizer";
            this.x264MinimimQuantizer.Size = new System.Drawing.Size(48, 20);
            this.x264MinimimQuantizer.TabIndex = 1;
            this.x264MinimimQuantizer.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.x264MinimimQuantizer.ValueChanged += new System.EventHandler(this.updateEvent);
            // 
            // x264MinimimQuantizerLabel
            // 
            this.x264MinimimQuantizerLabel.Location = new System.Drawing.Point(9, 16);
            this.x264MinimimQuantizerLabel.Name = "x264MinimimQuantizerLabel";
            this.x264MinimimQuantizerLabel.Padding = new System.Windows.Forms.Padding(3);
            this.x264MinimimQuantizerLabel.Size = new System.Drawing.Size(93, 18);
            this.x264MinimimQuantizerLabel.TabIndex = 0;
            this.x264MinimimQuantizerLabel.Text = "Min/Max/Delta";
            this.x264MinimimQuantizerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "Intra luma quantization deadzone";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Inter luma quantization deadzone";
            // 
            // customCommandlineOptionsLabel
            // 
            this.customCommandlineOptionsLabel.Location = new System.Drawing.Point(6, 301);
            this.customCommandlineOptionsLabel.Name = "customCommandlineOptionsLabel";
            this.customCommandlineOptionsLabel.Size = new System.Drawing.Size(167, 13);
            this.customCommandlineOptionsLabel.TabIndex = 1;
            this.customCommandlineOptionsLabel.Text = "Custom Commandline Options";
            this.customCommandlineOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton1.ArticleName = "Video configuration dialog/X264 Configuration";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(448, 348);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 10;
            // 
            // x264PicBox
            // 
            this.x264PicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.x264PicBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.x264PicBox.Image = ((System.Drawing.Image)(resources.GetObject("x264PicBox.Image")));
            this.x264PicBox.Location = new System.Drawing.Point(53, 282);
            this.x264PicBox.Name = "x264PicBox";
            this.x264PicBox.Size = new System.Drawing.Size(204, 63);
            this.x264PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.x264PicBox.TabIndex = 11;
            this.x264PicBox.TabStop = false;
            // 
            // linkx264website
            // 
            this.linkx264website.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkx264website.AutoSize = true;
            this.linkx264website.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkx264website.Location = new System.Drawing.Point(96, 348);
            this.linkx264website.Name = "linkx264website";
            this.linkx264website.Size = new System.Drawing.Size(107, 13);
            this.linkx264website.TabIndex = 12;
            this.linkx264website.TabStop = true;
            this.linkx264website.Text = "Official x264 Website";
            this.linkx264website.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkx264website_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Partitions";
            // 
            // x264ConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "x264ConfigurationPanel";
            this.Size = new System.Drawing.Size(510, 523);
            this.tabControl1.ResumeLayout(false);
            this.mainTabPage.ResumeLayout(false);
            this.mainTabPage.PerformLayout();
            this.x264GeneralMiscGroupbox.ResumeLayout(false);
            this.x264GeneralMiscGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264NbThreads)).EndInit();
            this.avcLevelGroupbox.ResumeLayout(false);
            this.avcProfileGroupbox.ResumeLayout(false);
            this.x264CodecToolsGroupbox.ResumeLayout(false);
            this.x264CodecToolsGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264BetaDeblock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264AlphaDeblock)).EndInit();
            this.x264CodecGeneralGroupbox.ResumeLayout(false);
            this.x264CodecGeneralGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264BitrateQuantizer)).EndInit();
            this.rateControlTabPage.ResumeLayout(false);
            this.x264OtherOptionsGroupbox.ResumeLayout(false);
            this.x264OtherOptionsGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264MERange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264SCDSensitivity)).EndInit();
            this.x264QuantOptionsGroupbox.ResumeLayout(false);
            this.x264QuantOptionsGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PsyTrellis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PsyRD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264NumberOfRefFrames)).EndInit();
            this.x264RateControlMiscGroupbox.ResumeLayout(false);
            this.x264RateControlMiscGroupbox.PerformLayout();
            this.x264RCGroupbox.ResumeLayout(false);
            this.x264RCGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264VBVInitialBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264TempQuantBlur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264TempFrameComplexityBlur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264QuantizerCompression)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264RateTol)).EndInit();
            this.quantizationTabPage.ResumeLayout(false);
            this.gbx264CustomCmd.ResumeLayout(false);
            this.gbx264CustomCmd.PerformLayout();
            this.gbAQ.ResumeLayout(false);
            this.gbAQ.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAQStrength)).EndInit();
            this.x264GeneralBFramesgGroupbox.ResumeLayout(false);
            this.x264GeneralBFramesgGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x264BframeBias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264NumberOfBFrames)).EndInit();
            this.quantizerMatrixGroupbox.ResumeLayout(false);
            this.x264MBGroupbox.ResumeLayout(false);
            this.x264MBGroupbox.PerformLayout();
            this.x264QuantizerGroupBox.ResumeLayout(false);
            this.x264QuantizerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneIntra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deadzoneInter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264PBFrameFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264IPFrameFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264CreditsQuantizer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264ChromaQPOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264MaxQuantDelta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264MaximumQuantizer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264MinimimQuantizer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x264PicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox x264GeneralMiscGroupbox;
        private System.Windows.Forms.CheckBox psnr;
        private System.Windows.Forms.Label x264NbThreadsLabel;
        private System.Windows.Forms.NumericUpDown x264NbThreads;
        private System.Windows.Forms.GroupBox avcLevelGroupbox;
        private System.Windows.Forms.ComboBox avcLevel;
        private System.Windows.Forms.GroupBox avcProfileGroupbox;
        private System.Windows.Forms.ComboBox avcProfile;
        private System.Windows.Forms.GroupBox x264CodecToolsGroupbox;
        private System.Windows.Forms.NumericUpDown x264BetaDeblock;
        private System.Windows.Forms.NumericUpDown x264AlphaDeblock;
        private System.Windows.Forms.CheckBox x264DeblockActive;
        private System.Windows.Forms.Label x264BetaDeblockLabel;
        private System.Windows.Forms.Label x264AlphaDeblockLabel;
        private System.Windows.Forms.GroupBox x264CodecGeneralGroupbox;
        private System.Windows.Forms.TextBox logfile;
        private System.Windows.Forms.ComboBox x264EncodingMode;
        private System.Windows.Forms.Label logfileLabel;
        private System.Windows.Forms.Button logfileOpenButton;
        private System.Windows.Forms.CheckBox x264Turbo;
        private System.Windows.Forms.Label x264EncodingModeLabel;
        private System.Windows.Forms.Label x264BitrateQuantizerLabel;
        private System.Windows.Forms.CheckBox x264LosslessMode;
        private System.Windows.Forms.TabPage rateControlTabPage;
        private System.Windows.Forms.TabPage quantizationTabPage;
        private System.Windows.Forms.GroupBox x264RateControlMiscGroupbox;
        private System.Windows.Forms.TextBox NoiseReduction;
        private System.Windows.Forms.Label NoiseReductionLabel;
        private System.Windows.Forms.Label x264KeyframeIntervalLabel;
        private System.Windows.Forms.TextBox x264KeyframeInterval;
        private System.Windows.Forms.TextBox x264MinGOPSize;
        private System.Windows.Forms.Label x264MinGOPSizeLabel;
        private System.Windows.Forms.GroupBox x264RCGroupbox;
        private System.Windows.Forms.Label x264RateTolLabel;
        private System.Windows.Forms.NumericUpDown x264VBVInitialBuffer;
        private System.Windows.Forms.Label x264VBVInitialBufferLabel;
        private System.Windows.Forms.TextBox x264VBVMaxRate;
        private System.Windows.Forms.NumericUpDown x264TempQuantBlur;
        private System.Windows.Forms.NumericUpDown x264TempFrameComplexityBlur;
        private System.Windows.Forms.NumericUpDown x264QuantizerCompression;
        private System.Windows.Forms.TextBox x264VBVBufferSize;
        private System.Windows.Forms.Label x264TempQuantBlurLabel;
        private System.Windows.Forms.Label x264TempFrameComplexityBlurLabel;
        private System.Windows.Forms.Label x264QuantizerCompressionLabel;
        private System.Windows.Forms.Label x264VBVMaxRateLabel;
        private System.Windows.Forms.Label x264VBVBufferSizeLabel;
        private System.Windows.Forms.NumericUpDown x264RateTol;
        private System.Windows.Forms.GroupBox quantizerMatrixGroupbox;
        private System.Windows.Forms.GroupBox x264MBGroupbox;
        private System.Windows.Forms.ComboBox macroblockOptions;
        private System.Windows.Forms.CheckBox adaptiveDCT;
        private System.Windows.Forms.CheckBox x264I4x4mv;
        private System.Windows.Forms.CheckBox x264I8x8mv;
        private System.Windows.Forms.CheckBox x264P4x4mv;
        private System.Windows.Forms.CheckBox x264B8x8mv;
        private System.Windows.Forms.CheckBox x264P8x8mv;
        private System.Windows.Forms.GroupBox x264QuantizerGroupBox;
        private System.Windows.Forms.NumericUpDown x264CreditsQuantizer;
        private System.Windows.Forms.Label x264CreditsQuantizerLabel;
        private System.Windows.Forms.NumericUpDown x264ChromaQPOffset;
        private System.Windows.Forms.Label x264ChromaQPOffsetLabel;
        private System.Windows.Forms.NumericUpDown x264MaxQuantDelta;
        private System.Windows.Forms.NumericUpDown x264MaximumQuantizer;
        private System.Windows.Forms.NumericUpDown x264MinimimQuantizer;
        private System.Windows.Forms.Label x264MinimimQuantizerLabel;
        private System.Windows.Forms.Label customCommandlineOptionsLabel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox ssim;
        private System.Windows.Forms.CheckBox interlaced;
        private System.Windows.Forms.GroupBox x264QuantOptionsGroupbox;
        private System.Windows.Forms.NumericUpDown x264NumberOfRefFrames;
        private System.Windows.Forms.CheckBox x264CabacEnabled;
        private System.Windows.Forms.CheckBox x264MixedReferences;
        private System.Windows.Forms.CheckBox noDCTDecimateOption;
        private System.Windows.Forms.CheckBox NoFastPSkip;
        private System.Windows.Forms.ComboBox trellis;
        private System.Windows.Forms.Label x264NumberOfRefFramesLabel;
        private System.Windows.Forms.Label trellisLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown deadzoneIntra;
        private System.Windows.Forms.NumericUpDown deadzoneInter;
        private System.Windows.Forms.NumericUpDown x264BitrateQuantizer;
        private MeGUI.core.gui.FileSCBox cqmComboBox1;
        private System.Windows.Forms.GroupBox x264GeneralBFramesgGroupbox;
        private System.Windows.Forms.ComboBox x264BframePredictionMode;
        private System.Windows.Forms.NumericUpDown x264BframeBias;
        private System.Windows.Forms.CheckBox x264WeightedBPrediction;
        private System.Windows.Forms.Label x264BframeBiasLabel;
        private System.Windows.Forms.Label x264BframePredictionModeLabel;
        private System.Windows.Forms.Label x264NumberOfBFramesLabel;
        private System.Windows.Forms.NumericUpDown x264NumberOfBFrames;
        private System.Windows.Forms.CheckBox x264PyramidBframes;
        private System.Windows.Forms.NumericUpDown x264IPFrameFactor;
        private System.Windows.Forms.Label lbQuantizersRatio;
        private System.Windows.Forms.NumericUpDown x264PBFrameFactor;
        private System.Windows.Forms.Label lbx264DeadZones;
        private System.Windows.Forms.GroupBox gbAQ;
        private System.Windows.Forms.NumericUpDown numAQStrength;
        private System.Windows.Forms.Label lbAQStrength;
        private System.Windows.Forms.ComboBox cbAQMode;
        private System.Windows.Forms.Label lbAQMode;
        private System.Windows.Forms.GroupBox gbx264CustomCmd;
        private System.Windows.Forms.TextBox customCommandlineOptions;
        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.LinkLabel linkx264website;
        private System.Windows.Forms.PictureBox x264PicBox;
        private System.Windows.Forms.GroupBox x264OtherOptionsGroupbox;
        private System.Windows.Forms.ComboBox x264SubpelRefinement;
        private System.Windows.Forms.Label x264SubpelRefinementLabel;
        private System.Windows.Forms.CheckBox x264ChromaMe;
        private System.Windows.Forms.Label x264MERangeLabel;
        private System.Windows.Forms.Label x264METypeLabel;
        private System.Windows.Forms.ComboBox x264METype;
        private System.Windows.Forms.NumericUpDown x264MERange;
        private System.Windows.Forms.NumericUpDown x264SCDSensitivity;
        private System.Windows.Forms.Label x264SCDSensitivityLabel;
        private System.Windows.Forms.Label x264AdaptiveBframesLabel;
        private System.Windows.Forms.ComboBox x264NewAdaptiveBframes;
        private System.Windows.Forms.NumericUpDown PsyRD;
        private System.Windows.Forms.Label PsyRDLabel;
        private System.Windows.Forms.Label PsyTrellisLabel;
        private System.Windows.Forms.NumericUpDown PsyTrellis;
        private System.Windows.Forms.TextBox qpfile;
        private System.Windows.Forms.Button qpfileOpenButton;
        private System.Windows.Forms.CheckBox useQPFile;
        private System.Windows.Forms.CheckBox x264FullRange;
        private System.Windows.Forms.Label label1;
    }
}
