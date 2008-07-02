using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.audx
{
    public partial class AudXConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<AudXSettings>
    {
        public AudXConfigurationPanel():base()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(EnumProxy.CreateArray(typeof(AudXSettings.QualityMode)));
        }
        #region properties
        protected override bool IsMultichanelRequed
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                AudXSettings nas = new AudXSettings();
                nas.Quality = (AudXSettings.QualityMode)(comboBox1.SelectedItem as EnumProxy).RealValue;
                return nas;
            }
            set
            {
                AudXSettings nas = value as AudXSettings;
                comboBox1.SelectedItem = EnumProxy.Create(nas.Quality);
            }
        }
        #endregion

        #region Editable<AudXSettings> Members

        AudXSettings Editable<AudXSettings>.Settings
        {
            get
            {
                return (AudXSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}




