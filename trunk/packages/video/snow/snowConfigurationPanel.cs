using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.snow
{
    public partial class snowConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Gettable<VideoCodecSettings>
    {
        #region start / stop
        public snowConfigurationPanel(MainForm mainForm, VideoInfo info)
            : base(mainForm, info)
        {
            InitializeComponent();
        }
        #endregion
        #region adjustments

        private void doCheckBoxAdjustments()
        {
            if (this.snowLosslessMode.Checked)
            {
                this.snowEncodingMode.SelectedIndex = 1; // CQ
                this.snowEncodingMode.Enabled = false;
                this.snowQuantizer.Enabled = false;
                this.snowQuantizer.Minimum = new decimal(0.01);
                this.snowQuantizer.Value = new decimal(0.01);
                this.snowBitrate.Enabled = false;
            }
            else
            {
                this.snowEncodingMode.Enabled = true;
                this.snowQuantizer.Enabled = true;
                this.snowQuantizer.Minimum = new decimal(1);
                if (this.snowQuantizer.Value < 1)
                    this.snowQuantizer.Value = new decimal(5);
            }
        }

        private void doDropDownAdjustments()
        {
            switch (this.snowEncodingMode.SelectedIndex)
            {
                case 0: // cbr
                    this.snowQuantizer.Enabled = false;
                    this.snowBitrate.Enabled = true;
                    this.snowLosslessMode.Enabled = false;
                    break;
                case 1: // cq
                    this.snowQuantizer.Enabled = true;
                    this.snowBitrate.Enabled = false;
                    this.snowLosslessMode.Enabled = true;
                    break;
                case 2: // 2 pass first pass
                    this.snowQuantizer.Enabled = false;
                    this.snowBitrate.Enabled = true;
                    this.snowLosslessMode.Enabled = false;
                    break;
                case 3:
                    this.snowQuantizer.Enabled = false;
                    this.snowBitrate.Enabled = true;
                    this.snowLosslessMode.Enabled = false;
                    break;
                case 4:
                    this.snowQuantizer.Enabled = false;
                    this.snowBitrate.Enabled = true;
                    this.snowLosslessMode.Enabled = false;
                    break;
            }

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
            if (snowEncodingMode.SelectedIndex == -1)
                this.snowEncodingMode.SelectedIndex = 0;
            if (snowPredictionMode.SelectedIndex == -1)
                this.snowPredictionMode.SelectedIndex = 0;
            if (snowMeCmpFullpel.SelectedIndex == -1)
                this.snowMeCmpFullpel.SelectedIndex = 0;
            if (snowMeCmpHpel.SelectedIndex == -1)
                this.snowMeCmpHpel.SelectedIndex = 0;
            if (snowMbCompare.SelectedIndex == -1)
                this.snowMbCompare.SelectedIndex = 0;
        }

        /// <summary>
        /// Returns whether settings is lavcSettings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is snowSettings;
        }

        /// <summary>
        /// Returns a new instance of lavcSettings.
        /// </summary>
        /// <returns>A new instance of lavcSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new snowSettings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public VideoCodecSettings Settings
        {
            get
            {
                snowSettings ss = new snowSettings();
                ss.EncodingMode = snowEncodingMode.SelectedIndex;
                ss.Quantizer = snowQuantizer.Value;
                ss.PredictionMode = snowPredictionMode.SelectedIndex;
                ss.MECompFullpel = snowMeCmpFullpel.SelectedIndex;
                ss.MECompHpel = snowMeCmpHpel.SelectedIndex;
                ss.MBComp = snowMbCompare.SelectedIndex;
                ss.QPel = snowQpel.Checked;
                ss.V4MV = snowV4mv.Checked;
                ss.NbMotionPredictors = snowNBPredictors.Value;
                if (!this.snowBitrate.Text.Equals(""))
                    ss.BitrateQuantizer = Int32.Parse(this.snowBitrate.Text);
                ss.CreditsQuantizer = this.snowCreditsQuantizer.Value;
                ss.LosslessMode = this.snowLosslessMode.Checked;
                ss.Logfile = this.logfile.Text;
                ss.Zones = this.Zones;
                return ss;
            }
            set
            {
                if (!(value is snowSettings))
                    return;
                snowSettings ss = value as snowSettings;
                snowEncodingMode.SelectedIndex = ss.EncodingMode;
                snowQuantizer.Value = ss.Quantizer;
                snowPredictionMode.SelectedIndex = ss.PredictionMode;
                snowMeCmpFullpel.SelectedIndex = ss.MECompFullpel;
                snowMeCmpHpel.SelectedIndex = ss.MECompHpel;
                snowMbCompare.SelectedIndex = ss.MBComp;
                snowQpel.Checked = ss.QPel;
                snowV4mv.Checked = ss.V4MV;
                snowNBPredictors.Value = ss.NbMotionPredictors;
                this.snowBitrate.Text = ss.BitrateQuantizer.ToString(); ;
                this.snowCreditsQuantizer.Value = ss.CreditsQuantizer;
                this.snowLosslessMode.Checked = ss.LosslessMode;
                this.logfile.Text = ss.Logfile;
                this.Zones = ss.Zones;
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

