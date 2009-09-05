using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.divxavc
{
    public partial class DivXAVCConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Editable<DivXAVCSettings>
    {
        public DivXAVCConfigurationPanel()
            : base()
        {
            InitializeComponent();
        }

        private void doCheckBoxAdjustments()
        {
            if (divxavcBFramesAsRef.Checked)
                divxavcPyramid.Enabled = true;
            else
            {
                divxavcPyramid.Enabled = false;
                divxavcPyramid.Checked = false;
            }
        }

        private void doDropDownAdjustments()
        {
            lastEncodingMode = this.divxavcEncodingMode.SelectedIndex;
        }

        protected override string getCommandline()
        {
            return DivXAVCEncoder.genCommandline("input", "output", null, Settings as DivXAVCSettings, 1, 1, null);
        }

        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doDropDownAdjustments();
            doCheckBoxAdjustments();
            divxAVCDialogTriStateAdjustment();
        }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (divxavcEncodingMode.SelectedIndex == -1)
                this.divxavcEncodingMode.SelectedIndex = 0; // 1pass
            if (divxavcInterlaceMode.SelectedIndex == -1)
                this.divxavcInterlaceMode.SelectedIndex = 0; // none
        }

        private void divxAVCDialogTriStateAdjustment()
        {
            bool turboOptions = this.divxavcTurbo.Checked &&
                (this.divxavcEncodingMode.SelectedIndex == (int)VideoCodecSettings.Mode.twopass1);

            if (this.divxavcMaxRefFrames.Value == 1)
                this.divxavcMaxBFrames.Value = 0;

            if (turboOptions)
            {
                this.divxavcPyramid.Checked = false;
                this.divxavcPyramid.Enabled = false;
                this.divxavcBFramesAsRef.Checked = false;
                this.divxavcAQO.Value = 0;
                this.divxavcMaxRefFrames.Value = 1;
                this.divxavcMaxBFrames.Value = 0;
            }
            else
            {
                this.divxavcPyramid.Enabled = true;
            }
        }


        /// <summary>
        /// Returns whether settings is divxAVCSettings
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is DivXAVCSettings;
        }

        /// <summary>
        /// Returns a new instance of divxAVCSettings.
        /// </summary>
        /// <returns>A new instance of divxAVCSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new DivXAVCSettings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public DivXAVCSettings Settings
        {
            get
            {
                DivXAVCSettings xs = new DivXAVCSettings();
                xs.EncodingMode = this.divxavcEncodingMode.SelectedIndex;
                xs.BitrateQuantizer = (int)divxavcBitrateQuantizer.Value;
                xs.AQO = (int)divxavcAQO.Value;
                xs.GOPLength = (int)divxavcGOPLength.Value;
                xs.InterlaceMode = (int)divxavcInterlaceMode.SelectedIndex;
                xs.BasRef = this.divxavcBFramesAsRef.Checked;
                xs.MaxBFrames = (int)this.divxavcMaxBFrames.Value;
                xs.MaxRefFrames = (int)this.divxavcMaxRefFrames.Value;
                xs.Pyramid = this.divxavcPyramid.Checked;
                xs.Logfile = this.logfile.Text;
                xs.NbThreads = (int)nbThreads.Value;
                xs.Turbo = this.divxavcTurbo.Checked;
                return xs;
            }
            set
            {
                DivXAVCSettings xs = value;
                this.divxavcEncodingMode.SelectedIndex = xs.EncodingMode;
                lastEncodingMode = divxavcEncodingMode.SelectedIndex;
                divxavcBitrateQuantizer.Value = xs.BitrateQuantizer;
                divxavcAQO.Value = xs.AQO;
                divxavcGOPLength.Value = xs.GOPLength;
                divxavcInterlaceMode.SelectedIndex = xs.InterlaceMode;
                divxavcPyramid.Checked = xs.Pyramid;
                divxavcMaxRefFrames.Value = xs.MaxRefFrames;
                divxavcBFramesAsRef.Checked = xs.BasRef;
                divxavcMaxBFrames.Value = xs.MaxBFrames;
                this.logfile.Text = xs.Logfile;
                this.nbThreads.Value = xs.NbThreads;
                this.divxavcTurbo.Checked = xs.Turbo;
            }
        }

        private void logfileOpenButton_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.logfile.Text = saveFileDialog.FileName;
                this.showCommandLine();
            }
        }

        private void updateEvent(object sender, EventArgs e)
        {
            genericUpdate();
        }
    }
}
