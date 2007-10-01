using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.util;

namespace MeGUI
{
    public partial class AutoEncodeDefaults : Form
    {
        private BitrateCalculator calc;
        public AutoEncodeDefaults()
        {
            InitializeComponent();
            calc = new BitrateCalculator();
            sizeSelection.Items.AddRange(calc.getPredefinedOutputSizes());
#warning muxprovider needs to be added here
//            MuxProvider muxProvider = new MuxProvider();
//            this.container.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
        }

        public AutoEncodeDefaultsSettings Settings
        {
            get
            {
                AutoEncodeDefaultsSettings defaults = new AutoEncodeDefaultsSettings();
                defaults.AddAdditionalContent = addSubsNChapters.Checked;
                defaults.SplitSize = null;
                if (splitOutput.Checked)
                {
                    try
                    {
                        int size = Int32.Parse(splitSize.Text);
                        defaults.SplitSize = new FileSize(Unit.MB, size);
                    }
                    catch (Exception)
                    {

                    }
                }
                defaults.FileSizeMode = FileSizeRadio.Checked;
                if (defaults.FileSizeMode)
                {
                    try
                    {
                        int size = Int32.Parse(muxedSizeMBs.Text);
                        defaults.FileSize = new FileSize(Unit.MB, size);
                    }
                    catch (Exception)
                    {

                    }
                }
                defaults.BitrateMode = averageBitrateRadio.Checked;
                if (defaults.BitrateMode)
                {
                    try
                    {
                        int bitrate = Int32.Parse(projectedBitrateKBits.Text);
                        defaults.Bitrate = bitrate;
                    }
                    catch (Exception)
                    {

                    }
                }
                defaults.NoTargetSizeMode = noTargetRadio.Checked;
                defaults.Container = container.SelectedItem.ToString();
                return defaults;
            }
            set
            {
                AutoEncodeDefaultsSettings defaults = value;
                addSubsNChapters.Checked = defaults.AddAdditionalContent;
                splitOutput.Checked = defaults.SplitSize.HasValue;
                muxedSizeMBs.Text = defaults.SplitSize.ToString();
                FileSizeRadio.Checked = defaults.FileSizeMode;
                muxedSizeMBs.Text = defaults.FileSize.ToString();
                averageBitrateRadio.Checked = defaults.BitrateMode;
                projectedBitrateKBits.Text = defaults.Bitrate.ToString();
                noTargetRadio.Checked = defaults.NoTargetSizeMode;
                foreach (object o in container.Items) // I know this is ugly, but using the ContainerType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(defaults.Container))
                    {
                        container.SelectedItem = o;
                        break;
                    }
                }
            }
        }

        private void splitOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (this.splitOutput.Checked)
                splitSize.Enabled = true;
            else
                splitSize.Enabled = false;
        }

        private void sizeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sizeMBs = calc.getOutputSizeKBs(sizeSelection.SelectedIndex) / 1024;
            this.muxedSizeMBs.Text = sizeMBs.ToString();
        }

        private void FileSizeRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (averageBitrateRadio.Checked)
            {
                muxedSizeMBs.Enabled = false;
                this.projectedBitrateKBits.Enabled = true;
                this.sizeSelection.Enabled = false;
            }
            else if (noTargetRadio.Checked)
            {
                muxedSizeMBs.Enabled = false;
                this.projectedBitrateKBits.Enabled = false;
                this.sizeSelection.Enabled = false;
            }
            else
            {
                muxedSizeMBs.Enabled = true;
                this.projectedBitrateKBits.Enabled = false;
                this.sizeSelection.Enabled = true;
            }
        }
        private void textField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }
    }
}