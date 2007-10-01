using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.xvid
{
    public partial class xvidConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Gettable<VideoCodecSettings>
    {

        #region start / stop
        public xvidConfigurationPanel(MainForm mainForm, VideoInfo info)
            : base(mainForm, info)
        {
            InitializeComponent();
            cqmComboBox1.StandardItems = new string[] { xvidSettings.H263Matrix, xvidSettings.MPEGMatrix };
        }
        #endregion
        #region adjustments
        private void doCheckBoxAdjustments()
        {
            if ((int)xvidNbBFrames.Value > 0)
            {
                xvidUseVHQForBframes.Enabled = true;
                xvidMinBQuant.Enabled = true;
                xvidMaxBQuant.Enabled = true;
                xvidBframeQuantRatio.Enabled = true;
                xvidBframeQuantOffset.Enabled = true;
                xvidBframeThreshold.Enabled = true;
                xvidFrameDropRatio.Enabled = true;
            }
            else // no b-frames
            {
                xvidUseVHQForBframes.Enabled = false;
                xvidUseVHQForBframes.Checked = false;
                xvidMinBQuant.Enabled = false;
                xvidMaxBQuant.Enabled = false;
                xvidBframeQuantRatio.Enabled = false;
                xvidBframeQuantOffset.Enabled = false;
                xvidBframeThreshold.Enabled = false;
                xvidFrameDropRatio.Enabled = false;
            }
        }

        private void doDropDownAdjustments()
        {
            logfileOpenButton.Enabled = false;
            if (this.lastEncodingMode == 1 && xvidEncodingMode.SelectedIndex != 1)
            {
                xvidBitrateQuantLabel.Text = "Bitrate";
                xvidBitrateQuantizer.Maximum = 10000;
                xvidBitrateQuantizer.Minimum = 1;
                xvidBitrateQuantizer.Increment = 1;
                xvidBitrateQuantizer.DecimalPlaces = 0;
                xvidBitrateQuantizer.Value = 700;
            }
            else if (lastEncodingMode != 1 && xvidEncodingMode.SelectedIndex == 1)
            {
                xvidBitrateQuantLabel.Text = "Quantizer";
                xvidBitrateQuantizer.Maximum = 31;
                xvidBitrateQuantizer.Minimum = 1;
                xvidBitrateQuantizer.Increment = 0.1M;
                xvidBitrateQuantizer.DecimalPlaces = 1;
                xvidBitrateQuantizer.Value = 8;
            }
            switch (this.xvidEncodingMode.SelectedIndex)
            {
                case 0: // cbr
                    xvidTurbo.Checked = false;
                    xvidTurbo.Enabled = false;
                    xvidRCGroupbox.Enabled = false;
                    xvidCBRRcGroupBox.Enabled = true;
                    break;
                case 1: // CQ
                    xvidTurbo.Checked = false;
                    xvidTurbo.Enabled = false;
                    xvidRCGroupbox.Enabled = false;
                    xvidCBRRcGroupBox.Enabled = true;
                    break;
                case 2: // 2pass first pass
                    xvidTurbo.Enabled = true;
                    xvidRCGroupbox.Enabled = true;
                    xvidCBRRcGroupBox.Enabled = false;
                    logfileOpenButton.Enabled = true;
                    break;
                case 3: // 2 pass 2nd pass
                    xvidTurbo.Enabled = true;
                    xvidRCGroupbox.Enabled = true;
                    xvidCBRRcGroupBox.Enabled = false;
                    logfileOpenButton.Enabled = true;
                    break;
                case 4: // automated 2pass
                    xvidTurbo.Enabled = true;
                    xvidRCGroupbox.Enabled = true;
                    xvidCBRRcGroupBox.Enabled = false;
                    logfileOpenButton.Enabled = true;
                    break;
            }
            lastEncodingMode = this.xvidEncodingMode.SelectedIndex;

            if (xvidVHQ.SelectedIndex == 0)
            {
                xvidUseVHQForBframes.Checked = false;
                xvidUseVHQForBframes.Enabled = false;
            }
            else
                xvidUseVHQForBframes.Enabled = true;
        }

        #endregion
        #region codec-specific overload functions
        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doDropDownAdjustments();
            doCheckBoxAdjustments();
        }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (fourCC.SelectedIndex == -1)
                this.fourCC.SelectedIndex = 0;
            if (xvidEncodingMode.SelectedIndex == -1)
                this.xvidEncodingMode.SelectedIndex = 0; // cbr
            if (xvidMotionSearchPrecision.SelectedIndex == -1)
                this.xvidMotionSearchPrecision.SelectedIndex = 6;
            if (xvidVHQ.SelectedIndex == -1)
                this.xvidVHQ.SelectedIndex = 1;
            if (cqmComboBox1.SelectedIndex == -1)
                cqmComboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// Returns whether settings is lavcSettings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is xvidSettings;
        }

        /// <summary>
        /// Returns a new instance of lavcSettings.
        /// </summary>
        /// <returns>A new instance of lavcSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new xvidSettings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public VideoCodecSettings Settings
        {
            get
            {
                xvidSettings xs = new xvidSettings();
                xs.FourCC = fourCC.SelectedIndex;
                xs.Turbo = this.xvidTurbo.Checked;
                xs.EncodingMode = this.xvidEncodingMode.SelectedIndex;
                xs.Quantizer = xvidBitrateQuantizer.Value;
                xs.BitrateQuantizer = (int)xvidBitrateQuantizer.Value;

                if (!xvidKeyframeInterval.Text.Equals(""))
                    xs.KeyframeInterval = Int32.Parse(this.xvidKeyframeInterval.Text);
                xs.NbBframes = (int)xvidNbBFrames.Value;
                xs.PackedBitstream = xvidPackedBitstream.Checked;
                xs.MotionSearchPrecision = xvidMotionSearchPrecision.SelectedIndex;
                xs.VHQMode = xvidVHQ.SelectedIndex;
                xs.VHQForBframes = xvidUseVHQForBframes.Checked;
                xs.QPel = xvidQpel.Checked;
                xs.GMC = xvidGMC.Checked;
                xs.NbThreads = (int)nbThreads.Value;
//                xs.CartoonMode = xvidCartoonMode.Checked;
                xs.ChromaMotion = xvidChromaMotion.Checked;
                xs.ClosedGOP = xvidClosedGop.Checked;
                xs.Greyscale = xvidGreyScale.Checked;
                xs.Interlaced = xvidInterlaced.Checked;
                xs.MinQuantizer = (int)xvidMinIQuant.Value;
                xs.MaxQuantizer = (int)xvidMaxIQuant.Value;
                xs.MinPQuant = (int)xvidMinPQuant.Value;
                xs.MaxPQuant = (int)xvidMaxPQuant.Value;
                xs.MinBQuant = (int)xvidMinBQuant.Value;
                xs.MaxBQuant = (int)xvidMaxBQuant.Value;
                xs.CreditsQuantizer = (int)xvidCreditsQuantizer.Value;
                xs.Trellis = xvidTrellisQuant.Checked;
                xs.AdaptiveQuant = xvidAdaptiveQuant.Checked;
                xs.BQuantRatio = (int)xvidBframeQuantRatio.Value;
                xs.BQuantOffset = (int)xvidBframeQuantOffset.Value;
                xs.KeyFrameBoost = (int)xvidIframeBoost.Value;
                if (!xvidKeyframeTreshold.Text.Equals(""))
                    xs.KeyframeThreshold = Int32.Parse(xvidKeyframeTreshold.Text);
                xs.KeyframeReduction = (int)xvidKeyframeReduction.Value;
                xs.OverflowControlStrength = (int)xvidOverflowControlStrength.Value;
                xs.MaxOverflowImprovement = (int)xvidMaxOverflowImprovement.Value;
                xs.MaxOverflowDegradation = (int)xvidMaxOverflowDegradation.Value;
                xs.HighBitrateDegradation = (int)xvidHighBitrateDegradation.Value;
                xs.LowBitrateImprovement = (int)xvidLowBitrateImprovement.Value;
                if (!xvidRCDelayFactor.Text.Equals(""))
                    xs.ReactionDelayFactor = Int32.Parse(xvidRCDelayFactor.Text);
                if (!xvidRCAveragingPeriod.Text.Equals(""))
                    xs.AveragingPeriod = Int32.Parse(xvidRCAveragingPeriod.Text);
                if (!xvidRCBufferSize.Text.Equals("") && !xvidRCBufferSize.Text.Equals("0"))
                    xs.RateControlBuffer = Int32.Parse(xvidRCBufferSize.Text);
                xs.BframeThreshold = this.xvidBframeThreshold.Value;
//                xs.ChromaOptimizer = this.xvidChromaOptimizer.Checked;
//                xs.HQAC = this.xvidHQAC.Checked;
                xs.FrameDropRatio = (int)xvidFrameDropRatio.Value;
                xs.QuantizerMatrix = cqmComboBox1.SelectedText;
                xs.CustomEncoderOptions = xvidCustomCommandlineOptions.Text;
                xs.Logfile = this.logfile.Text;
                xs.Zones = Zones;
                return xs;
            }
            set
            {
                if (!(value is xvidSettings))
                    return;
                xvidSettings xs = (xvidSettings)value;
                fourCC.SelectedIndex = xs.FourCC;
                this.xvidTurbo.Checked = xs.Turbo;
                this.xvidEncodingMode.SelectedIndex = xs.EncodingMode;
                lastEncodingMode = xvidEncodingMode.SelectedIndex;
                if (xs.EncodingMode == 1) // CQ
                    xvidBitrateQuantizer.Value = xs.Quantizer;
                else
                    xvidBitrateQuantizer.Value = xs.BitrateQuantizer;
                this.nbThreads.Value = xs.NbThreads;
                this.xvidKeyframeInterval.Text = xs.KeyframeInterval.ToString(); ;
                xvidNbBFrames.Value = xs.NbBframes;
                xvidPackedBitstream.Checked = xs.PackedBitstream;
                xvidMotionSearchPrecision.SelectedIndex = xs.MotionSearchPrecision;
                xvidVHQ.SelectedIndex = xs.VHQMode;
                xvidUseVHQForBframes.Checked = xs.VHQForBframes;
                xvidQpel.Checked = xs.QPel;
                xvidGMC.Checked = xs.GMC;
//                xvidCartoonMode.Checked = xs.CartoonMode;
                xvidChromaMotion.Checked = xs.ChromaMotion;
                xvidClosedGop.Checked = xs.ClosedGOP;
                xvidGreyScale.Checked = xs.Greyscale;
                xvidInterlaced.Checked = xs.Interlaced;
                xvidMinIQuant.Value = xs.MinQuantizer;
                xvidMaxIQuant.Value = xs.MaxQuantizer;
                xvidMinPQuant.Value = xs.MinPQuant;
                xvidMaxPQuant.Value = xs.MaxPQuant;
                xvidMinBQuant.Value = xs.MinBQuant;
                xvidMaxBQuant.Value = xs.MaxBQuant;
                xvidCreditsQuantizer.Value = xs.CreditsQuantizer;
                xvidTrellisQuant.Checked = xs.Trellis;
                xvidAdaptiveQuant.Checked = xs.AdaptiveQuant;
                xvidBframeQuantRatio.Value = xs.BQuantRatio;
                xvidBframeQuantOffset.Value = xs.BQuantOffset;
                xvidIframeBoost.Value = xs.KeyFrameBoost;
                xvidKeyframeTreshold.Text = xs.KeyframeThreshold.ToString();
                xvidKeyframeReduction.Value = xs.KeyframeReduction;
                xvidOverflowControlStrength.Value = xs.OverflowControlStrength;
                xvidMaxOverflowImprovement.Value = xs.MaxOverflowImprovement;
                xvidMaxOverflowDegradation.Value = xs.MaxOverflowDegradation;
                xvidHighBitrateDegradation.Value = xs.HighBitrateDegradation;
                xvidLowBitrateImprovement.Value = xs.LowBitrateImprovement;
                xvidRCDelayFactor.Text = xs.ReactionDelayFactor.ToString();
                xvidRCAveragingPeriod.Text = xs.AveragingPeriod.ToString();
                if (xs.RateControlBuffer > 0)
                    xvidRCBufferSize.Text = xs.RateControlBuffer.ToString();
                else
                    xvidRCBufferSize.Text = "";
                this.xvidBframeThreshold.Value = xs.BframeThreshold;
//                this.xvidChromaOptimizer.Checked = xs.ChromaOptimizer;
//                this.xvidHQAC.Checked = xs.HQAC;
                xvidFrameDropRatio.Value = (decimal)xs.FrameDropRatio;
                cqmComboBox1.SelectedObject = xs.QuantizerMatrix;
                xvidCustomCommandlineOptions.Text = xs.CustomEncoderOptions;
                this.logfile.Text = xs.Logfile;
                this.Zones = xs.Zones;
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

        private void cqmComboBox1_SelectionChanged(object sender, string val)
        {
            genericUpdate();
        }
    }
}

