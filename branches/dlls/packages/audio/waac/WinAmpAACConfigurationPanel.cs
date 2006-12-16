using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.waac
{
    public partial class WinAmpAACConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel
    {
        public WinAmpAACConfigurationPanel(MainForm mainForm, string[] info)
            : base(mainForm, info)
        {
            InitializeComponent();
            comboBox1.Items.AddRange(EnumProxy.CreateArray(WinAmpAACSettings.SupportedProfiles));
            comboBox2.Items.AddRange(EnumProxy.CreateArray(typeof(WinAmpAACSettings.AacStereoMode)));
            vBitrate_ValueChanged(null, null);
        }
        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                WinAmpAACSettings nas = new WinAmpAACSettings();
                nas.Mpeg2AAC = checkBox2.Checked;
                nas.Profile = (AacProfile)(comboBox1.SelectedItem as EnumProxy).RealValue;
                nas.StereoMode = (WinAmpAACSettings.AacStereoMode)(comboBox2.SelectedItem as EnumProxy).RealValue;
                nas.Bitrate = vBitrate.Value;
                return nas;
            }
            set
            {
                WinAmpAACSettings nas = value as WinAmpAACSettings;
                checkBox2.Checked = nas.Mpeg2AAC;
                comboBox1.SelectedItem = EnumProxy.Create(nas.Profile);
                comboBox2.SelectedItem = EnumProxy.Create(nas.StereoMode);
                vBitrate.Value = Math.Max(Math.Min(nas.Bitrate, vBitrate.Maximum), vBitrate.Minimum);
            }
        }
        #endregion

        private void vBitrate_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = "CBR @ " + vBitrate.Value + " kbps";
        }


    }
}

