using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.ffac3
{
    public partial class AC3ConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel
    {
        public AC3ConfigurationPanel(MainForm mainForm, string[] info)
            : base(mainForm, info)
        {
            InitializeComponent();
            comboBox1.Items.AddRange(AC3Settings.SupportedBitrates);
        }

        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                AC3Settings nas = new AC3Settings();
                nas.Bitrate = (int)comboBox1.SelectedItem;
                return nas;
            }
            set
            {
                AC3Settings nas = value as AC3Settings;
                comboBox1.SelectedItem = nas.Bitrate;
            }
        }
        #endregion
    }
}

