using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.lmp4
{
    public partial class lavcConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Editable<lavcSettings>
    {
        #region start / stop
        public lavcConfigurationPanel()
            : base()
        {
            InitializeComponent();
        }
        #endregion
        #region adjustments
        private void adjustUpdowns()
        {
            if (lavcNbBframes.Value > 0)
            {
                lavcAvoidHighmotionBframes.Enabled = true;
            }
            else
            {
                lavcAvoidHighmotionBframes.Enabled = false;
                lavcAvoidHighmotionBframes.Checked = false;
            }
        }

        private void adjustCheckboxes()
        {
            if (lavcQpel.Checked)
                lavcSubpelRefinement.Enabled = true;
            else
                lavcSubpelRefinement.Enabled = false;

            if (interlaced.Checked)
            {
                fieldOrder.Enabled = true;
                lavcQpel.Enabled = false;
                if (lavcQpel.Checked)
                    lavcQpel.Checked = false;
            }
            else
            {
                fieldOrder.Enabled = false;
                lavcQpel.Enabled = true;
            }
        }

        private void adjustEncodingMode()
        {
            this.logfileOpenButton.Enabled = false;
            lavcAvoidHighmotionBframes.Enabled = false;
            lavcAvoidHighmotionBframes.Checked = false;
            lavcIPQuantFactor.Enabled = true;
            lavcPBQuantFactor.Enabled = true;
            this.quantizerBlur.Maximum = (decimal)1.0;
            if (this.lastEncodingMode == 1 && lavcEncodingMode.SelectedIndex != 1)
            {
                lavcBitrateQuantLabel.Text = "Bitrate";
                lavcBitrateQuantizer.Text = "800";
            }
            else if (this.lastEncodingMode != 1 && lavcEncodingMode.SelectedIndex == 1)
            {
                lavcBitrateQuantLabel.Text = "Quantizer";
                lavcBitrateQuantizer.Text = "8";
            }
            switch (lavcEncodingMode.SelectedIndex)
            {
                case 0: // cbr
                    lavcTurbo.Checked = false;
                    lavcTurbo.Enabled = false;
                    lavcIPQuantFactor.Enabled = false;
                    lavcPBQuantFactor.Enabled = false;
                    break;
                case 1: // CQ
                    lavcTurbo.Checked = false;
                    lavcTurbo.Enabled = false;
                    lavcIPQuantFactor.Enabled = false;
                    lavcPBQuantFactor.Enabled = false;
                    break;
                case 2: // 2pass first pass
                    lavcTurbo.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    lavcAvoidHighmotionBframes.Enabled = true;
                    break;
                case 3: // 2 pass 2nd pass
                    lavcTurbo.Checked = false;
                    lavcTurbo.Enabled = false;
                    logfileOpenButton.Enabled = true;
                    this.quantizerBlur.Maximum = (decimal)99.0;
                    break;
                case 4: // automated twopass
                    lavcTurbo.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;
                case 5:  // 3 pass first pass
                    lavcTurbo.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    lavcAvoidHighmotionBframes.Enabled = false;
                    this.quantizerBlur.Maximum = (decimal)99.0;
                    break;
                case 6: // 3 pass 2nd pass
                    lavcTurbo.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;
                case 7: // 3 pass 3rd pass
                    lavcTurbo.Checked = false;
                    lavcTurbo.Enabled = false;
                    logfileOpenButton.Enabled = true;
                    break;
                case 8: // automated 3pass
                    lavcTurbo.Enabled = true;
                    logfileOpenButton.Enabled = true;
                    break;
            }
            lastEncodingMode = lavcEncodingMode.SelectedIndex;
        }
        #endregion
        #region codec-specific overload functions
        protected override string getCommandline()
        {
            return mencoderEncoder.genLavcCommandline("input", "output", null, Settings as lavcSettings, null);
        }

        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            adjustEncodingMode();
            adjustCheckboxes();
            adjustUpdowns();
        }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (fourCC.SelectedIndex == -1)
                this.fourCC.SelectedIndex = 0;
            if (lavcEncodingMode.SelectedIndex == -1)
                this.lavcEncodingMode.SelectedIndex = 0; // cbr
            if (lavcMbDecisionAlgo.SelectedIndex == -1)
                this.lavcMbDecisionAlgo.SelectedIndex = 0;
        }

        /// <summary>
        /// Returns whether settings is lavcSettings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is lavcSettings;
        }

        /// <summary>
        /// Returns a new instance of lavcSettings.
        /// </summary>
        /// <returns>A new instance of lavcSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new lavcSettings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public lavcSettings Settings
        {
            get
            {
                lavcSettings ls = new lavcSettings();
                ls.Turbo = lavcTurbo.Checked;
                ls.FourCC = fourCC.SelectedIndex;
                ls.EncodingMode = lavcEncodingMode.SelectedIndex;
                if (!lavcBitrateQuantizer.Text.Equals(""))
                    ls.BitrateQuantizer = Int32.Parse(lavcBitrateQuantizer.Text);
                if (!lavcKeyframeInterval.Text.Equals(""))
                    ls.KeyframeInterval = Int32.Parse(lavcKeyframeInterval.Text);
                ls.NbBframes = (int)lavcNbBframes.Value;
                ls.AvoidHighMoBframes = lavcAvoidHighmotionBframes.Checked;
                ls.MbDecisionAlgo = lavcMbDecisionAlgo.SelectedIndex;
                ls.V4MV = lavcV4MV.Checked;
                ls.SCD = (int)lavcScd.Value;
                ls.QPel = lavcQpel.Checked;
                ls.LumiMasking = lavcLumiMasking.Value;
                ls.DarkMask = lavcDarkMask.Value;
                ls.SubpelRefinement = (int)lavcSubpelRefinement.Value;
                ls.Interlaced = interlaced.Checked;
                ls.FieldOrder = fieldOrder.SelectedIndex - 1;
                ls.GreyScale = greyScale.Checked;
                ls.NbThreads = (int)nbThreads.Value;
                ls.MinQuantizer = (int)lavcMinQuant.Value;
                ls.MaxQuantizer = (int)lavcMaxQuant.Value;
                ls.MaxQuantDelta = (int)lavcMaxQuantDelta.Value;
                ls.CreditsQuantizer = (int)lavcCreditsQuantizer.Value;
                ls.IPFactor = lavcIPQuantFactor.Value;
                ls.PBFactor = lavcPBQuantFactor.Value;
                ls.BQuantFactor = bQuantizerOffset.Value;
                ls.Trellis = lavcTrellisQuant.Checked;
                if (!lavcMinBitrate.Text.Equals(""))
                    ls.MinBitrate = Int32.Parse(lavcMinBitrate.Text);
                if (!lavcMaxBitrate.Text.Equals(""))
                    ls.MaxBitrate = Int32.Parse(lavcMaxBitrate.Text);
                if (!lavcRCBuffersize.Text.Equals(""))
                    ls.BufferSize = Int32.Parse(lavcRCBuffersize.Text);
                ls.FilesizeTolerance = (int)lavcRCTolerance.Value;
                ls.QuantizerBlur = quantizerBlur.Value;
                ls.QuantizerCompression = quantizerCompression.Value;
                ls.IntraMatrix = this.intraMatrix.Text;
                ls.InterMatrix = this.interMatrix.Text;
                ls.MERange = this.meRange.Value;
                ls.InitialBufferOccupancy = this.initialBufferOccupancy.Value;
                ls.BorderMask = this.borderMask.Value;
                ls.SpatialMask = this.spatialMask.Value;
                ls.TemporalMask = this.temporalMask.Value;
                ls.NbMotionPredictors = lavcNBPredictors.Value;
                ls.Logfile = this.logfile.Text;
                return ls;
            }
            set
            {
                lavcSettings ls = value;
                fourCC.SelectedIndex = ls.FourCC;
                lavcEncodingMode.SelectedIndex = ls.EncodingMode;
                lavcBitrateQuantizer.Text = ls.BitrateQuantizer.ToString();
                lavcKeyframeInterval.Text = ls.KeyframeInterval.ToString();
                lavcNbBframes.Value = ls.NbBframes;
                lavcAvoidHighmotionBframes.Checked = ls.AvoidHighMoBframes;
                lavcMbDecisionAlgo.SelectedIndex = ls.MbDecisionAlgo;
                lavcV4MV.Checked = ls.V4MV;
                lavcScd.Value = ls.SCD;
                lavcQpel.Checked = ls.QPel;
                lavcLumiMasking.Value = ls.LumiMasking;
                lavcDarkMask.Value = ls.DarkMask;
                lavcSubpelRefinement.Value = ls.SubpelRefinement;
                greyScale.Checked = ls.GreyScale;
                fieldOrder.SelectedIndex = ls.FieldOrder + 1;
                nbThreads.Value = ls.NbThreads;
                lavcMinQuant.Value = ls.MinQuantizer;
                lavcMaxQuant.Value = ls.MaxQuantizer;
                lavcMaxQuantDelta.Value = ls.MaxQuantDelta;
                lavcCreditsQuantizer.Value = ls.CreditsQuantizer;
                lavcIPQuantFactor.Value = ls.IPFactor;
                lavcPBQuantFactor.Value = ls.PBFactor;
                bQuantizerOffset.Value = ls.BQuantFactor;
                lavcTrellisQuant.Checked = ls.Trellis;
                lavcMinBitrate.Text = ls.MinBitrate.ToString(); ;
                lavcMaxBitrate.Text = ls.MaxBitrate.ToString();
                lavcRCBuffersize.Text = ls.BufferSize.ToString();
                lavcRCTolerance.Value = ls.FilesizeTolerance;
                quantizerBlur.Value = ls.QuantizerBlur;
                quantizerCompression.Value = ls.QuantizerCompression;
                intraMatrix.Text = ls.IntraMatrix;
                interMatrix.Text = ls.InterMatrix;
                this.meRange.Value = ls.MERange;
                this.initialBufferOccupancy.Value = ls.InitialBufferOccupancy;
                this.borderMask.Value = ls.BorderMask;
                this.spatialMask.Value = ls.SpatialMask;
                this.temporalMask.Value = ls.TemporalMask;
                lavcNBPredictors.Value = ls.NbMotionPredictors;
                this.logfile.Text = ls.Logfile;
                lavcTurbo.Checked = ls.Turbo;
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

    }
}





