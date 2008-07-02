using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.aften
{
    public partial class AftenConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<AftenSettings>
    {
       public AftenConfigurationPanel():base()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(AftenSettings.SupportedBitrates);
        }

        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                AftenSettings nas = new AftenSettings();
                nas.Bitrate = (int)comboBox1.SelectedItem;
                return nas;
            }
            set
            {
                AftenSettings nas = value as AftenSettings;
                comboBox1.SelectedItem = nas.Bitrate;
            }
        }
        #endregion

        #region Editable<AftenSettings> Members

        AftenSettings Editable<AftenSettings>.Settings
        {
            get
            {
                return (AftenSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}




