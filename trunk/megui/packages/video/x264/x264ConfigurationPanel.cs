using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using MeGUI.core.details.video;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.gui;

namespace MeGUI.packages.video.x264
{
    public partial class x264ConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Editable<x264Settings>
    {
        #region variables
        public static bool levelEnforced; // flag to prevent recursion in EnforceLevels. There's probably a better way to do this.
        private XmlDocument ContextHelp = new XmlDocument();
        #endregion


        #region start / stop
        public x264ConfigurationPanel()
            : base()
        {
            InitializeComponent();
            cqmComboBox1.StandardItems = new string[] { "Flat (none)", "JVT" };
            this.AdvancedToolTips = MainForm.Instance.Settings.UseAdvancedTooltips;
            AVCLevels al = new AVCLevels();
            this.avcLevel.Items.AddRange(al.getLevels());
        }
        #endregion
        #region adjustments
        #region checkboxes
        private void doCheckBoxAdjustments()
        {
            x264AlphaDeblock.Enabled = x264DeblockActive.Checked;
            x264BetaDeblock.Enabled = x264DeblockActive.Checked;
        }
        #endregion
        #region dropdowns
        private void doMacroBlockAdjustments()
        {
            if (macroblockOptions.SelectedIndex == 0) // all
            {
                x264P8x8mv.Checked = true;
                x264P8x8mv.Enabled = false;
                x264I4x4mv.Checked = true;
                x264I4x4mv.Enabled = false;
                x264P4x4mv.Checked = true;
                x264P4x4mv.Enabled = false;
                x264B8x8mv.Checked = true;
                x264B8x8mv.Enabled = false;
                adaptiveDCT.Enabled = false;
                x264I8x8mv.Enabled = false;

                if (avcProfile.SelectedIndex == 2)
                {
                    adaptiveDCT.Checked = true;
                    x264I8x8mv.Checked = true;
                }
                else
                {
                    adaptiveDCT.Checked = false;
                    x264I8x8mv.Checked = false;
                }
            }
            else if (macroblockOptions.SelectedIndex == 1) // none
            {
                x264I4x4mv.Checked = false;
                x264I4x4mv.Enabled = false;
                x264P4x4mv.Checked = false;
                x264P4x4mv.Enabled = false;
                x264P8x8mv.Checked = false;
                x264P8x8mv.Enabled = false;
                x264B8x8mv.Checked = false;
                x264B8x8mv.Enabled = false;
                x264I8x8mv.Checked = false;
                x264I8x8mv.Enabled = false;
                adaptiveDCT.Checked = false;
                adaptiveDCT.Enabled = false;
            }
            else if (macroblockOptions.SelectedIndex == 2) // custom
            {
                x264I4x4mv.Enabled = true;
                x264P8x8mv.Enabled = true;
                x264B8x8mv.Enabled = true;

                if (avcProfile.SelectedIndex == 2)
                {
                    adaptiveDCT.Enabled = true;
                    if (adaptiveDCT.Checked)
                    {
                        x264I8x8mv.Enabled = true;
                        x264I8x8mv.Checked = true;
                    }
                    else
                    {
                        x264I8x8mv.Enabled = false;
                        x264I8x8mv.Checked = false;
                    }
                }
                else
                {
                    adaptiveDCT.Enabled = false;
                    adaptiveDCT.Checked = false;
                    x264I8x8mv.Enabled = false;
                    x264I8x8mv.Checked = false;
                }

                if (!this.x264P8x8mv.Checked) // p8x8 requires p4x4
                {
                    this.x264P4x4mv.Checked = false;
                    this.x264P4x4mv.Enabled = false;
                }
                else
                {
                    this.x264P4x4mv.Enabled = true;
                }
            }
        }
        private void doTrellisAdjustments()
        {
            deadzoneInter.Enabled = (trellis.SelectedIndex == 0);
            deadzoneIntra.Enabled = (trellis.SelectedIndex == 0);
            if (trellis.SelectedIndex != 0)
            {
                deadzoneIntra.Value = 11;
                deadzoneInter.Value = 21;
            }
        }

        #endregion
        #region levels
        private void avcLevelDialog(string title, int verifyResult)
        {
            string invalidLevelHelp = "Should be an informative message here";
            switch (verifyResult)
            {
                case 1:
                    invalidLevelHelp = "P4x4 macroblocks are not allowed in level > 3 or in level 3 with B frames.";
                    break;
                case 2:
                    invalidLevelHelp = "Decoded Picture Buffer Size is too high\n(reduce frame size, no. of references, or no. of B frames)";
                    break;
                case 3:
                    invalidLevelHelp = "Maximum Bitrate is too high";
                    break;
                case 4:
                    invalidLevelHelp = "Maximum buffer size is too high";
                    break;
            }
            MessageBox.Show("Sorry, the selected profile exceeds the AVC Level you have specified\n\n" + invalidLevelHelp, title, MessageBoxButtons.OK);
        }
        private void EnforceLevel(x264Settings inputSettings)
        {
            if (avcLevel.SelectedIndex > -1)
            {
                AVCLevels al = new AVCLevels();
                AVCLevelEnforcementReturn enforcement;
                x264Settings verifiedSettings = al.EnforceSettings(avcLevel.SelectedIndex, inputSettings, BytesPerFrame, out enforcement);
                // Set the correct input enable states
                if (enforcement.EnableP4x4mv)
                    if (this.macroblockOptions.SelectedIndex == 2)
                        this.x264P4x4mv.Enabled = true;
                    else
                        this.x264P4x4mv.Enabled = false;
                else
                    this.x264P4x4mv.Enabled = false;
                if (enforcement.EnableVBVMaxRate)
                    this.x264VBVMaxRate.Enabled = true;
                else
                    this.x264VBVMaxRate.Enabled = false;
                if (enforcement.EnableVBVBufferSize)
                    this.x264VBVBufferSize.Enabled = true;
                else
                    this.x264VBVBufferSize.Enabled = false;
                if (enforcement.Altered)
                {
                    levelEnforced = true;
                    this.Settings = verifiedSettings;
                    levelEnforced = false;

                    if (enforcement.Panic)
                        MessageBox.Show(enforcement.PanicString, "Level Violation", MessageBoxButtons.OK);
                }
            }
        }
        private void doAVCLevelAdjustments()
        {
            AVCLevels avcLevel = new AVCLevels();
            int avcLevelVerify = avcLevel.Verifyx264Settings(this.Settings as x264Settings, this.avcLevel.SelectedIndex, this.BytesPerFrame);
            if (avcLevelVerify != 0)
            {
                avcLevelDialog("Reverting to Unrestrained Level", avcLevelVerify);
                this.avcLevel.SelectedIndex = 15;
            }
        }
        #endregion
        #region dropdowns
        private void setNonQPOptionsEnabled(bool enabled)
        {
            x264MinimimQuantizer.Enabled = enabled;
            x264MaximumQuantizer.Enabled = enabled;
            x264MaxQuantDelta.Enabled = enabled;
            x264IPFrameFactor.Enabled = enabled;
            x264PBFrameFactor.Enabled = enabled;
            x264ChromaQPOffset.Enabled = enabled;
            x264RCGroupbox.Enabled = enabled;
        }
        /// <summary>
        /// Returns whether the given mode is a bitrate or quality-based mode
        /// </summary>
        /// <param name="mode">selected encoding mode</param>
        /// <returns>true if the mode is a bitrate mode, false otherwise</returns>
        private bool isBitrateMode(int mode)
        {
            return !(mode == (int)VideoCodecSettings.Mode.CQ ||
                mode == (int)VideoCodecSettings.Mode.quality);
        }
        private void doEncodingModeAdjustments()
        {

            if (isBitrateMode(x264EncodingMode.SelectedIndex))
            {
                this.x264BitrateQuantizerLabel.Text = "Bitrate";
                x264TempFrameComplexityBlur.Enabled = true;
                x264TempFrameComplexityBlurLabel.Enabled = true;
                x264TempQuantBlur.Enabled = true;
                x264TempQuantBlurLabel.Enabled = true;

                x264BitrateQuantizer.Maximum = 100000;
                x264BitrateQuantizer.Minimum = 10;
                x264BitrateQuantizer.DecimalPlaces = 0;
                x264BitrateQuantizer.Increment = 10;
            }
            else
            {
                x264TempFrameComplexityBlur.Enabled = false;
                x264TempFrameComplexityBlurLabel.Enabled = false;
                x264TempQuantBlur.Enabled = false;
                x264TempQuantBlurLabel.Enabled = false;
                if (x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.Mode.CQ)
                {
                    this.x264BitrateQuantizerLabel.Text = "Quantizer";
                }
                if (x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.Mode.quality)
                {
                    this.x264BitrateQuantizerLabel.Text = "Quality";
                }
              
                x264BitrateQuantizer.Maximum = 64;
                if (x264EncodingMode.SelectedIndex == 9) // crf
                {
                    x264BitrateQuantizer.Minimum = 0.1M;
                    x264BitrateQuantizer.DecimalPlaces = 1;
                    x264BitrateQuantizer.Increment = 0.1M;
                }
                else // qp
                {
                    // This first line makes sure it is an integer, in case we just swapped from crf
                    x264BitrateQuantizer.Value = (int)x264BitrateQuantizer.Value; 
                    x264BitrateQuantizer.Minimum = 1;
                    x264BitrateQuantizer.DecimalPlaces = 0;
                    x264BitrateQuantizer.Increment = 1;
                }

            }
            if (x264EncodingMode.SelectedIndex != (int)VideoCodecSettings.Mode.CQ)
                setNonQPOptionsEnabled(true);
            else
                setNonQPOptionsEnabled(false);

            x264Turbo.Enabled = false;
            x264RateTol.Enabled = true;
            x264RateTolLabel.Enabled = true;
            switch (x264EncodingMode.SelectedIndex)
            {
                case (int)VideoCodecSettings.Mode.CBR: //Actually, ABR
                    x264Turbo.Enabled = false;
                    x264Turbo.Checked = false;
                    x264RateTol.Enabled = true;
                    x264RateTolLabel.Enabled = true;
                    logfileOpenButton.Enabled = false;
                    break;

                case (int)VideoCodecSettings.Mode.CQ:
                    x264Turbo.Enabled = false;
                    x264Turbo.Checked = false;
                    x264RateTol.Enabled = false;
                    x264RateTolLabel.Enabled = false;
                    logfileOpenButton.Enabled = false;
                    break;

                case (int)VideoCodecSettings.Mode.twopass1:
                case (int)VideoCodecSettings.Mode.threepass1:
                    x264Turbo.Enabled = true;
                    x264RateTol.Enabled = true;
                    x264RateTolLabel.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;

                case (int)VideoCodecSettings.Mode.twopass2:
                case (int)VideoCodecSettings.Mode.threepass2:
                case (int)VideoCodecSettings.Mode.threepass3:
                    x264Turbo.Enabled = false;
                    x264Turbo.Checked = false;
                    x264RateTol.Enabled = true;
                    x264RateTolLabel.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;
                case (int)VideoCodecSettings.Mode.twopassAutomated:
                case (int)VideoCodecSettings.Mode.threepassAutomated:
                    x264Turbo.Enabled = true;
                    x264RateTol.Enabled = true;
                    x264RateTolLabel.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;
                case (int)VideoCodecSettings.Mode.quality:
                    x264Turbo.Enabled = false;
                    x264Turbo.Checked = false;
                    logfileOpenButton.Enabled = false;
                    x264RateTol.Enabled = false;
                    x264RateTolLabel.Enabled = false;
                    break;
            }

            // We check whether the bitrate/quality text needs to be changed
            if (isBitrateMode(lastEncodingMode) != isBitrateMode(x264EncodingMode.SelectedIndex))
            {
                if (isBitrateMode(x264EncodingMode.SelectedIndex))
                {
                    this.x264BitrateQuantizer.Value = 700;
                }
                else
                {
                    this.x264BitrateQuantizer.Value = 26;
                }
            }

            lastEncodingMode = x264EncodingMode.SelectedIndex;
        }
        #endregion
        #region level -> mb
        /// <summary>
        /// adjust the mb selection dropdown in function of the selected profile and the activated
        /// mb options
        /// </summary>
        public void doMBOptionsAdjustments()
        {
            if (!x264P8x8mv.Checked)
            {
                // x264P4x4mv.Checked = false;
                x264P4x4mv.Enabled = false;
            }
            switch (avcProfile.SelectedIndex)
            {
                case 0: // BP
                case 1: // MP
                    if (x264P8x8mv.Checked && x264B8x8mv.Checked && x264I4x4mv.Checked && x264P4x4mv.Checked)
                        this.macroblockOptions.SelectedIndex = 0;
                    else if (!x264P8x8mv.Checked && !x264B8x8mv.Checked && !x264I4x4mv.Checked && !x264P4x4mv.Checked)
                        this.macroblockOptions.SelectedIndex = 1;
                    else
                        this.macroblockOptions.SelectedIndex = 2;
                    break;
                case 2: // HP
                    if (x264P8x8mv.Checked && x264B8x8mv.Checked && x264I4x4mv.Checked && x264I8x8mv.Checked && x264P4x4mv.Checked && adaptiveDCT.Checked)
                        this.macroblockOptions.SelectedIndex = 0;
                    else if (!x264P8x8mv.Checked && !x264B8x8mv.Checked && !x264I4x4mv.Checked && !x264I8x8mv.Checked && !x264P4x4mv.Checked && !adaptiveDCT.Checked)
                        this.macroblockOptions.SelectedIndex = 1;
                    else
                        this.macroblockOptions.SelectedIndex = 2;
                    break;
            }
        }
        #endregion
        #endregion
        #region codec-specific overload functions
        protected override string getCommandline()
        {
            return x264Encoder.genCommandline("input", "output", null, -1, -1, Settings as x264Settings);
        }
        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doEncodingModeAdjustments();
            doCheckBoxAdjustments();
            doTrellisAdjustments();
            doAVCLevelAdjustments();
            x264DialogTriStateAdjustment();
            doMacroBlockAdjustments();
        }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (fourCC.SelectedIndex == -1)
                this.fourCC.SelectedIndex = 0;
            if (x264EncodingMode.SelectedIndex == -1)
                this.x264EncodingMode.SelectedIndex = 0;
            if (x264SubpelRefinement.SelectedIndex == -1)
                this.x264SubpelRefinement.SelectedIndex = 4;
            if (x264BframePredictionMode.SelectedIndex == -1)
                this.x264BframePredictionMode.SelectedIndex = 1;
            if (x264METype.SelectedIndex == -1)
                this.x264METype.SelectedIndex = 0;
            if (macroblockOptions.SelectedIndex == -1)
                macroblockOptions.SelectedIndex = 2;
            if (cqmComboBox1.SelectedIndex == -1)
                cqmComboBox1.SelectedIndex = 0; // flat matrix
            if (this.avcProfile.SelectedIndex == -1)
                avcProfile.SelectedIndex = 1; // 
            if (cbAQMode.SelectedIndex == -1)
                cbAQMode.SelectedIndex = 2;

            lastEncodingMode = this.x264EncodingMode.SelectedIndex;
            try
            {
                string p = System.IO.Path.Combine (Application.StartupPath, "Data");
                p = System.IO.Path.Combine (p, "ContextHelp.xml");
                ContextHelp.Load(p);
                SetToolTips();
            }
            catch
            {
                MessageBox.Show("The ContextHelp.xml file could not be found. Please check in the 'Data' directory to see if it exists. Help tooltips will not be available.", "File Not Found", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Returns whether settings is lavcSettings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is x264Settings;
        }

        /// <summary>
        /// Returns a new instance of lavcSettings.
        /// </summary>
        /// <returns>A new instance of lavcSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new x264Settings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public x264Settings Settings
        {
            get
            {
                x264Settings xs = new x264Settings();
                xs.DeadZoneInter = (int)deadzoneInter.Value;
                xs.DeadZoneIntra = (int)deadzoneIntra.Value;
                xs.EncodeInterlaced = interlaced.Checked;
                xs.NoDCTDecimate = this.noDCTDecimateOption.Checked;
                xs.SSIMCalculation = this.ssim.Checked;
                xs.PSNRCalculation = this.psnr.Checked;
                xs.noFastPSkip = this.NoFastPSkip.Checked;
                xs.FourCC = fourCC.SelectedIndex;
                xs.Turbo = this.x264Turbo.Checked;
                xs.MixedRefs = this.x264MixedReferences.Checked;
                xs.EncodingMode = x264EncodingMode.SelectedIndex;
                xs.BitrateQuantizer = (int)x264BitrateQuantizer.Value;
                xs.QuantizerCRF = x264BitrateQuantizer.Value;
                if (!x264KeyframeInterval.Text.Equals(""))
                    xs.KeyframeInterval = Int32.Parse(x264KeyframeInterval.Text);
                xs.NbRefFrames = (int)x264NumberOfRefFrames.Value;
                xs.NbBframes = (int)x264NumberOfBFrames.Value;
                xs.AdaptiveBFrames = x264AdaptiveBframes.Checked;
                xs.BFramePyramid = x264PyramidBframes.Checked;
                xs.Deblock = x264DeblockActive.Checked;
                xs.AlphaDeblock = (int)x264AlphaDeblock.Value;
                xs.BetaDeblock = (int)x264BetaDeblock.Value;
                xs.Cabac = x264CabacEnabled.Checked;
                xs.SubPelRefinement = this.x264SubpelRefinement.SelectedIndex;
                xs.BRDO = bRDO.Checked;
                xs.biME = BiME.Checked;
                xs.WeightedBPrediction = x264WeightedBPrediction.Checked;
                xs.ChromaME = this.x264ChromaMe.Checked;
                xs.X264Trellis = trellis.SelectedIndex;
                xs.P8x8mv = x264P8x8mv.Checked;
                xs.B8x8mv = x264B8x8mv.Checked;
                xs.I4x4mv = x264I4x4mv.Checked;
                xs.I8x8mv = x264I8x8mv.Checked;
                xs.P4x4mv = x264P4x4mv.Checked;
                xs.MinQuantizer = (int)x264MinimimQuantizer.Value;
                xs.MaxQuantizer = (int)x264MaximumQuantizer.Value;
                xs.MaxQuantDelta = (int)x264MaxQuantDelta.Value;
                xs.CreditsQuantizer = (int)this.x264CreditsQuantizer.Value;
                xs.ChromaQPOffset = this.x264ChromaQPOffset.Value;
                xs.IPFactor = x264IPFrameFactor.Value;
                xs.PBFactor = x264PBFrameFactor.Value;
                if (!x264VBVBufferSize.Text.Equals("")) // otherwise it'll be -1 as per the constructor
                    xs.VBVBufferSize = Int32.Parse(x264VBVBufferSize.Text);
                if (!x264VBVMaxRate.Text.Equals("")) // otherwise it'll be -1 as per the constructor
                    xs.VBVMaxBitrate = Int32.Parse(x264VBVMaxRate.Text);
                xs.VBVInitialBuffer = x264VBVInitialBuffer.Value;
                xs.BitrateVariance = x264RateTol.Value;
                xs.QuantCompression = x264QuantizerCompression.Value;
                xs.TempComplexityBlur = (int)x264TempFrameComplexityBlur.Value;
                xs.TempQuanBlurCC = x264TempQuantBlur.Value;
                xs.SCDSensitivity = (int)this.x264SCDSensitivity.Value;
                xs.BframeBias = (int)this.x264BframeBias.Value;
                xs.BframePredictionMode = this.x264BframePredictionMode.SelectedIndex;
                xs.METype = this.x264METype.SelectedIndex;
                xs.MERange = (int)x26MERange.Value;
                xs.NbThreads = (int)x264NbThreads.Value;
                if (!x264MinGOPSize.Text.Equals(""))
                    xs.MinGOPSize = Int32.Parse(x264MinGOPSize.Text);
                xs.Logfile = this.logfile.Text;
                xs.AdaptiveDCT = adaptiveDCT.Checked;
                xs.CustomEncoderOptions = customCommandlineOptions.Text;
                if (cqmComboBox1.SelectedIndex > 1)
                    xs.QuantizerMatrixType = 2;
                else
                    xs.QuantizerMatrixType = cqmComboBox1.SelectedIndex;
                xs.QuantizerMatrix = cqmComboBox1.SelectedText;
                xs.Profile = avcProfile.SelectedIndex;
                xs.Level = avcLevel.SelectedIndex;
                xs.Lossless = x264LosslessMode.Checked;
                if (!NoiseReduction.Text.Equals(""))
                    xs.NoiseReduction = Int32.Parse(NoiseReduction.Text);
                xs.AQmode = (int)cbAQMode.SelectedIndex;
                xs.AQstrength = numAQStrength.Value;
                return xs;
            }
            set
            {  // Warning! The ordering of components matters because of the dependency code!
                x264Settings xs = value;
                deadzoneInter.Value = xs.DeadZoneInter;
                deadzoneIntra.Value = xs.DeadZoneIntra;
                interlaced.Checked = xs.EncodeInterlaced;
                noDCTDecimateOption.Checked = xs.NoDCTDecimate;
                ssim.Checked = xs.SSIMCalculation;
                avcProfile.SelectedIndex = xs.Profile;
                avcLevel.SelectedIndex = xs.Level;
                x264EncodingMode.SelectedIndex = xs.EncodingMode;
                lastEncodingMode = xs.EncodingMode;
                x264NumberOfRefFrames.Value = xs.NbRefFrames;
                x264NumberOfBFrames.Value = xs.NbBframes;
                NoFastPSkip.Checked = xs.noFastPSkip;
                x264MixedReferences.Checked = xs.MixedRefs;
                this.x264SubpelRefinement.SelectedIndex = xs.SubPelRefinement;
                fourCC.SelectedIndex = xs.FourCC;
                x264Turbo.Checked = xs.Turbo;
                x264BitrateQuantizer.Value = (isBitrateMode(xs.EncodingMode) || xs.QuantizerCRF == 0) ? xs.BitrateQuantizer : xs.QuantizerCRF;
                x264KeyframeInterval.Text = xs.KeyframeInterval.ToString() ;
                x264AdaptiveBframes.Checked = xs.AdaptiveBFrames;
                x264DeblockActive.Checked = xs.Deblock;
                x264PyramidBframes.Checked = xs.BFramePyramid;
                x264AlphaDeblock.Value = xs.AlphaDeblock;
                x264BetaDeblock.Value = xs.BetaDeblock;
                x264CabacEnabled.Checked = xs.Cabac;
                bRDO.Checked = xs.BRDO;
                BiME.Checked = xs.biME;
                x264WeightedBPrediction.Checked = xs.WeightedBPrediction;
                x264ChromaMe.Checked = xs.ChromaME;
                trellis.SelectedIndex = xs.X264Trellis;
                adaptiveDCT.Checked = xs.AdaptiveDCT;
                x264P8x8mv.Checked = xs.P8x8mv;
                x264B8x8mv.Checked = xs.B8x8mv;
                x264I4x4mv.Checked = xs.I4x4mv;
                x264I8x8mv.Checked = xs.I8x8mv;
                x264P4x4mv.Checked = xs.P4x4mv;
                x264MinimimQuantizer.Value = xs.MinQuantizer;
                x264MaximumQuantizer.Value = xs.MaxQuantizer;
                x264MaxQuantDelta.Value = xs.MaxQuantDelta;
                x264CreditsQuantizer.Value = xs.CreditsQuantizer;
                x264IPFrameFactor.Value = xs.IPFactor;
                x264PBFrameFactor.Value = xs.PBFactor;
                x264ChromaQPOffset.Value = xs.ChromaQPOffset;
                if (xs.VBVBufferSize > 0)
                    x264VBVBufferSize.Text = xs.VBVBufferSize.ToString();
                else
                    x264VBVBufferSize.Text = "";
                if (xs.VBVMaxBitrate > 0)
                    x264VBVMaxRate.Text = xs.VBVMaxBitrate.ToString();
                else
                    x264VBVBufferSize.Text = "";
                x264VBVInitialBuffer.Value = xs.VBVInitialBuffer;
                x264RateTol.Value = xs.BitrateVariance;
                x264QuantizerCompression.Value = xs.QuantCompression;
                x264TempFrameComplexityBlur.Value = xs.TempComplexityBlur;
                x264TempQuantBlur.Value = xs.TempQuanBlurCC;
                this.x264SCDSensitivity.Value = xs.SCDSensitivity;
                this.x264BframeBias.Value = xs.BframeBias;
                this.x264BframePredictionMode.SelectedIndex = xs.BframePredictionMode;
                this.x264METype.SelectedIndex = xs.METype;
                x26MERange.Value = xs.MERange;
                x264NbThreads.Value = xs.NbThreads;
                x264MinGOPSize.Text = xs.MinGOPSize.ToString();
                customCommandlineOptions.Text = xs.CustomEncoderOptions;
                this.logfile.Text = xs.Logfile;
                cqmComboBox1.SelectedObject = xs.QuantizerMatrix;
                x264LosslessMode.Checked = xs.Lossless;
                psnr.Checked = xs.PSNRCalculation;
                cbAQMode.SelectedIndex = xs.AQmode;
                numAQStrength.Value = xs.AQstrength;
                NoiseReduction.Text = xs.NoiseReduction.ToString(); ;
            }
        }
        #endregion
        #region events
        private void updateEvent(object sender, EventArgs e)
        {
            genericUpdate();
        }
        private void textField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }
        private void logfileOpenButton_Click(object sender, System.EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.logfile.Text = saveFileDialog.FileName;
                this.showCommandLine();
            }
        }
        #endregion
        #region ContextHelp
        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            string xpath = "/ContextHelp/Codec[@name='x264']/" + node;
            XmlNodeList nl = ContextHelp.SelectNodes(xpath); // Return the details for the specified node

            if (nl.Count == 1) // if it finds the required HelpText, count should be 1
            {
                HelpText.Append(nl[0].Attributes["name"].Value);
                HelpText.AppendLine();
                if (AdvancedToolTips)
                    HelpText.AppendLine(nl[0]["Advanced"].InnerText);
                else
                    HelpText.AppendLine(nl[0]["Basic"].InnerText);
                HelpText.AppendLine();
                HelpText.AppendLine("Default Value: " + nl[0]["Default"].InnerText);
                HelpText.AppendLine("Recommended Value: " + nl[0]["Recommended"].InnerText);
            }
            else // If count isn't 1, then theres no valid data.
                HelpText.Append("Error: No data available");

            return (HelpText.ToString());
        }
        private void SetToolTips()
        {
            tooltipHelp.SetToolTip(x264KeyframeInterval, SelectHelpText("keyint"));
            tooltipHelp.SetToolTip(NoiseReduction, SelectHelpText("nr"));
            tooltipHelp.SetToolTip(x264BitrateQuantizer, SelectHelpText("bitrate"));
            tooltipHelp.SetToolTip(BiME, SelectHelpText("bime"));
            tooltipHelp.SetToolTip(x264MinimimQuantizer, SelectHelpText("qpmin"));
            tooltipHelp.SetToolTip(NoFastPSkip, SelectHelpText("no-fast-pskip"));
            tooltipHelp.SetToolTip(bRDO, SelectHelpText("b-rdo"));
            tooltipHelp.SetToolTip(macroblockOptions, SelectHelpText("partitions"));
            tooltipHelp.SetToolTip(x264ChromaMe, SelectHelpText("no-chroma-me"));
            tooltipHelp.SetToolTip(x264WeightedBPrediction, SelectHelpText("weightb"));
            tooltipHelp.SetToolTip(x264SubpelRefinement, SelectHelpText("subme"));
            tooltipHelp.SetToolTip(x264CabacEnabled, SelectHelpText("no-cabac"));
            tooltipHelp.SetToolTip(x264DeblockActive, SelectHelpText("nf"));
            tooltipHelp.SetToolTip(x264AdaptiveBframes, SelectHelpText("no-b-adapt"));
            tooltipHelp.SetToolTip(x264PyramidBframes, SelectHelpText("b-pyramid"));
            tooltipHelp.SetToolTip(x264MixedReferences, SelectHelpText("mixed-refs"));
            tooltipHelp.SetToolTip(x264LosslessMode, SelectHelpText("losslessmode"));
            tooltipHelp.SetToolTip(x264NumberOfRefFrames, SelectHelpText("ref"));
            tooltipHelp.SetToolTip(x264NumberOfBFrames, SelectHelpText("bframes"));
            tooltipHelp.SetToolTip(x264AlphaDeblock, SelectHelpText("filter"));
            tooltipHelp.SetToolTip(x264BetaDeblock, SelectHelpText("filter"));
            tooltipHelp.SetToolTip(x264MaximumQuantizer, SelectHelpText("qpmax"));
            tooltipHelp.SetToolTip(x264MaxQuantDelta, SelectHelpText("qpstep"));
            tooltipHelp.SetToolTip(x264CreditsQuantizer, SelectHelpText("creditsquant"));
            tooltipHelp.SetToolTip(x264IPFrameFactor, SelectHelpText("ipratio"));
            tooltipHelp.SetToolTip(x264PBFrameFactor, SelectHelpText("pbratio"));
            tooltipHelp.SetToolTip(x264ChromaQPOffset, SelectHelpText("chroma-qp-offset"));


            /*************************/
            /* Rate Control Tooltips */
            /*************************/
            tooltipHelp.SetToolTip(x264VBVBufferSize, SelectHelpText("vbv-bufsize"));
            tooltipHelp.SetToolTip(x264VBVMaxRate, SelectHelpText("vbv-maxrate"));
            tooltipHelp.SetToolTip(x264VBVInitialBuffer, SelectHelpText("vbv-init"));
            tooltipHelp.SetToolTip(x264RateTol, SelectHelpText("ratetol"));
            tooltipHelp.SetToolTip(x264QuantizerCompression, SelectHelpText("qcomp"));
            tooltipHelp.SetToolTip(x264TempFrameComplexityBlur, SelectHelpText("cplxblur"));
            tooltipHelp.SetToolTip(x264TempQuantBlur, SelectHelpText("qblur"));

            /**************************/
            /* Other Options Tooltips */
            /**************************/
            tooltipHelp.SetToolTip(x264SCDSensitivity, SelectHelpText("scenecut"));
            tooltipHelp.SetToolTip(x264BframeBias, SelectHelpText("b-bias"));
            tooltipHelp.SetToolTip(x264BframePredictionMode, SelectHelpText("direct"));
            tooltipHelp.SetToolTip(x264METype, SelectHelpText("me"));
            tooltipHelp.SetToolTip(x26MERange, SelectHelpText("merange"));
            tooltipHelp.SetToolTip(x264NbThreads, SelectHelpText("threads"));
            tooltipHelp.SetToolTip(x264MinGOPSize, SelectHelpText("min-keyint"));
            tooltipHelp.SetToolTip(trellis, SelectHelpText("trellis"));
            tooltipHelp.SetToolTip(psnr, SelectHelpText("psnr"));
            tooltipHelp.SetToolTip(ssim, SelectHelpText("ssim"));
            tooltipHelp.SetToolTip(fourCC, SelectHelpText("fourcc"));
            tooltipHelp.SetToolTip(avcLevel, SelectHelpText("level"));
            tooltipHelp.SetToolTip(avcProfile, SelectHelpText("profile"));
            tooltipHelp.SetToolTip(cqmComboBox1, SelectHelpText("cqm"));

            /*************************/
            /* Advanced Tab Tooltips */
            /*************************/
            tooltipHelp.SetToolTip(cbAQMode, SelectHelpText("aqmode"));
            tooltipHelp.SetToolTip(numAQStrength, SelectHelpText("aqstrength"));
            tooltipHelp.SetToolTip(macroblockOptions, SelectHelpText("analyse"));
            tooltipHelp.SetToolTip(adaptiveDCT, SelectHelpText("i8x8dct"));
            tooltipHelp.SetToolTip(x264B8x8mv, SelectHelpText("b8x8mv"));
            tooltipHelp.SetToolTip(x264P8x8mv, SelectHelpText("p8x8mv"));
            tooltipHelp.SetToolTip(x264P4x4mv, SelectHelpText("p4x4mv"));
            tooltipHelp.SetToolTip(x264I4x4mv, SelectHelpText("i4x4mv"));

        }
        #endregion
        #region GUI State adjustment
        private void x264DialogTriStateAdjustment()
        {
            bool turboOptions = this.x264Turbo.Checked &&
                (this.x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.Mode.threepass1 ||
                this.x264EncodingMode.SelectedIndex == (int)VideoCodecSettings.Mode.twopass1);

            // First we do the Profile Adjustments
            #region profile adjustments
            switch (avcProfile.SelectedIndex)
            {
                case 0: // baseline, disable cabac, b-frames and i8x8
                    x264CabacEnabled.Checked = false;
                    x264CabacEnabled.Enabled = false;
                    x264NumberOfBFrames.Value = 0;
                    x264NumberOfBFrames.Enabled = false;
                    x264NumberOfRefFramesLabel.Enabled = false;
                    cqmComboBox1.SelectedIndex = 0;
                    quantizerMatrixGroupbox.Text = "";
                    quantizerMatrixGroupbox.Enabled = false;
                    x264LosslessMode.Checked = false;
                    x264LosslessMode.Enabled = false;
                    break;
                case 1: // main profile, disable i8x8
                    x264CabacEnabled.Enabled = true;
                    x264NumberOfBFrames.Enabled = true;
                    x264NumberOfRefFramesLabel.Enabled = true;
                    cqmComboBox1.SelectedIndex = 0;
                    quantizerMatrixGroupbox.Text = "";
                    quantizerMatrixGroupbox.Enabled = false;
                    x264LosslessMode.Checked = false;
                    x264LosslessMode.Enabled = false;
                    break;
                case 2: // high profile, enable everything
                    x264CabacEnabled.Enabled = true;
                    x264NumberOfBFrames.Enabled = true;
                    x264NumberOfRefFramesLabel.Enabled = true;
                    x264LosslessMode.Enabled = true;
                    if (x264LosslessMode.Checked)
                    {
                        x264BitrateQuantizer.Enabled = false;
                        x264EncodingMode.SelectedIndex = 1;
                        lastEncodingMode = 1;
                        x264EncodingMode.Enabled = false;
                    }
                    else
                    {
                        x264EncodingMode.Enabled = true;
                        x264BitrateQuantizer.Enabled = true;
                    }
                    quantizerMatrixGroupbox.Enabled = true;
                    break;
            }
            #endregion

            // Now we do B frames adjustments
            #region b-frames
            if (this.x264NumberOfBFrames.Value == 0)
            {
                this.x264AdaptiveBframes.Checked = false;
                this.x264AdaptiveBframes.Enabled = false;
                this.x264BframePredictionMode.Enabled = false;
                this.x264BframePredictionModeLabel.Enabled = false;
                this.x264WeightedBPrediction.Checked = false;
                this.x264WeightedBPrediction.Enabled = false;
                this.x264BframeBias.Value = 0;
                this.x264BframeBias.Enabled = false;
                this.x264BframeBiasLabel.Enabled = false;
                this.bRDO.Checked = false;
                this.bRDO.Enabled = false;
                this.BiME.Checked = false;
                this.BiME.Enabled = false;
                this.x264PyramidBframes.Checked = false;
                this.x264PyramidBframes.Enabled = false;
            }
            else
            {
                this.x264AdaptiveBframes.Enabled = true;
                this.x264BframePredictionMode.Enabled = true;
                this.x264BframePredictionModeLabel.Enabled = true;
                // We can enable these if we don't have turbo options
                this.x264WeightedBPrediction.Enabled = !turboOptions;
                this.BiME.Enabled = !turboOptions;
                this.x264BframeBias.Enabled = true;
                this.x264BframeBiasLabel.Enabled = true;
                if (this.x264SubpelRefinement.SelectedIndex > 4 && !turboOptions)
                    this.bRDO.Enabled = true;
                else
                {
                    this.bRDO.Checked = false;
                    this.bRDO.Enabled = false;
                }
                if (this.x264NumberOfBFrames.Value >= 2) // pyramid requires at least two b-frames
                    this.x264PyramidBframes.Enabled = true;
                else
                {
                    this.x264PyramidBframes.Checked = false;
                    this.x264PyramidBframes.Enabled = false;
                }
            }
            #endregion

            // Now we do some additional checks -- ref frames, cabac
            #region extra checks
            if (this.x264NumberOfRefFrames.Value > 1 && !turboOptions) // mixed references require at least two reference frames
            {
                this.x264MixedReferences.Enabled = true;
            }
            else
            {
                this.x264MixedReferences.Checked = false;
                this.x264MixedReferences.Enabled = false;
            }
            if (!this.x264CabacEnabled.Checked || turboOptions) // trellis requires CABAC
            {
                this.trellis.Enabled = false;
                this.trellisLabel.Enabled = false;
                this.trellis.SelectedIndex = 0;
            }
            else
            {
                this.trellis.Enabled = true;
                this.trellisLabel.Enabled = true;
            }
            #endregion

            // And finally, we do turbo adjustmentsn
            #region turbo
            // If we need to make the changes to the GUI required by turbo
            if (turboOptions)
            {
                // Disable everything
                this.x264NumberOfRefFrames.Enabled = false;
                this.x264SubpelRefinement.Enabled = false;
                this.x264METype.Enabled = false;
                this.macroblockOptions.SelectedIndex = 0; // None
                this.bRDO.Enabled = false;
                this.NoFastPSkip.Enabled = false;
                this.macroblockOptions.Enabled = false;

                // Uncheck everything
                this.BiME.Checked = false;
                this.x264NumberOfRefFrames.Value = new decimal(1);
                this.x264SubpelRefinement.SelectedIndex = 0;
                this.x264METype.SelectedIndex = 0;
                this.x264MixedReferences.Checked = false;
                this.bRDO.Checked = false;
                this.NoFastPSkip.Checked = false;
                this.x264WeightedBPrediction.Checked = false;
                this.macroblockOptions.SelectedIndex = 1;
            }
            else
            {
                // Enable everything
                this.x264NumberOfRefFrames.Enabled = true;
                this.x264SubpelRefinement.Enabled = true;
                this.NoFastPSkip.Enabled = true;
                this.macroblockOptions.Enabled = true;
            }
            #endregion
        }
        #endregion

        private void cqmComboBox1_SelectionChanged(object sender, string val)
        {
            genericUpdate();
        }

        private void cbAQMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAQMode.SelectedIndex != 0)
                numAQStrength.Enabled = true;
            else numAQStrength.Enabled = false;
            genericUpdate();
        }

        private void linkx264website_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void VisitLink()
        {
            //Call the Process.Start method to open the default browser 
            //with a URL:
            System.Diagnostics.Process.Start("http://www.videolan.org/developers/x264.html");
        }
    }
}




