using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.ffmp2
{
    public partial class MP2ConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel
    {
        public MP2ConfigurationPanel(MainForm mainForm, string[] info)
            : base(mainForm, info)
        {
            InitializeComponent();
            comboBox1.Items.AddRange(MP2Settings.SupportedBitrates);
        }
        #region properties

        protected override bool IsMultichanelSupported
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                MP2Settings nas = new MP2Settings();
                nas.Bitrate = (int)comboBox1.SelectedItem;
                return nas;
            }
            set
            {
                MP2Settings nas = value as MP2Settings;
                comboBox1.SelectedItem = nas.Bitrate;
            }
        }
        #endregion
    }
}

