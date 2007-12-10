using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.lame
{
    public partial class lameConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<MP3Settings>
    {
        public lameConfigurationPanel():base()
        {
            InitializeComponent();
            this.encodingMode.Items.Add(BitrateManagementMode.CBR);
            this.encodingMode.Items.Add(BitrateManagementMode.VBR);
            this.encodingMode.Items.Add(BitrateManagementMode.ABR);
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
                MP3Settings ms = new MP3Settings();
                ms.BitrateMode = (BitrateManagementMode)encodingMode.SelectedItem;
                switch (ms.BitrateMode)
                {
                    case BitrateManagementMode.CBR:
                    case BitrateManagementMode.ABR:
                        ms.Bitrate = (int)this.bitrate.Value;
                        break;
                    case BitrateManagementMode.VBR:
                        ms.Quality = (int)this.quality.Value;
                        break;
                }
                return ms;
	        }
	        set
	        {
                MP3Settings ms = value as MP3Settings;
                encodingMode.SelectedItem = ms.BitrateMode;
                bitrate.Value = ms.Bitrate;
                quality.Value = ms.Quality;
                encodingMode_SelectedIndexChanged(null, null);	            
	        }
	    }
		#endregion
		#region buttons
		/// <summary>
		/// handles entires into textfiels, blocks entry of non digit characters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textField_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (! char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
				e.Handled = true;
		}
		#endregion
		#region updown controls
		private void encodingMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (encodingMode.SelectedIndex >= 0) // else it's bogus
			{
                if ((BitrateManagementMode)encodingMode.SelectedItem != BitrateManagementMode.VBR) // cbr or abr
				{
					bitrate.Enabled = true;
					quality.Enabled = false;
				}
				else // vbr
				{
					bitrate.Enabled = false;
					quality.Enabled = true;
				}
			}
		}

		#endregion

        #region Editable<MP3Settings> Members

        MP3Settings Editable<MP3Settings>.Settings
        {
            get
            {
                return (MP3Settings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}




