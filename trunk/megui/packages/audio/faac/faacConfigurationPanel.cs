using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.faac
{
    public partial class faacConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<FaacSettings>
    {
        public faacConfigurationPanel():base()
        {
            InitializeComponent();
            cbrBitrate.DataSource = FaacSettings.SupportedBitrates;
            cbrBitrate.BindingContext = new BindingContext();
        }
	    #region properties
		/// <summary>
		/// gets / sets the settings that are being shown in this configuration dialog
		/// </summary>
		protected override AudioCodecSettings CodecSettings
		{
			get
			{
				FaacSettings fas = new FaacSettings();
                fas.BitrateMode = qualityModeRadioButton.Checked ? BitrateManagementMode.VBR : BitrateManagementMode.ABR;
                fas.Bitrate = (int)cbrBitrate.SelectedItem;
				fas.Quality = vbrQuality.Value;
				return fas;
			}
			set
			{
                FaacSettings fas = value as FaacSettings;
                cbrBitrate.SelectedItem = Array.IndexOf(FaacSettings.SupportedBitrates, fas.Bitrate) < 0 ? FaacSettings.SupportedBitrates[0] : fas.Bitrate;
                vbrQuality.Value = fas.Quality;
                qualityModeRadioButton.Checked = !(cbrBitrateRadioButton.Checked = (fas.BitrateMode != BitrateManagementMode.VBR));
                bitrateModeChanged(null, null);
			}
		}
		#endregion
        #region checkboxes
        private void bitrateModeChanged(object sender, EventArgs e)
        {
            cbrBitrate.Enabled = !(vbrQuality.Enabled = qualityModeRadioButton.Checked);
        }
        #endregion

        #region Editable<FaacSettings> Members

        FaacSettings Editable<FaacSettings>.Settings
        {
            get
            {
                return (FaacSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}




