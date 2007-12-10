using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.packages.audio.vorbis
{
    public partial class OggVorbisConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<OggVorbisSettings>
    {
        public OggVorbisConfigurationPanel():base()
        {
            InitializeComponent();
            vQuality_ValueChanged(null, null);
        }
        #region properties
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                OggVorbisSettings nas = new OggVorbisSettings();
                nas.Quality = (Decimal)vQuality.Value * 10.0M / vQuality.Maximum;
                return nas;
            }
            set
            {
                OggVorbisSettings nas = value as OggVorbisSettings;
                vQuality.Value = (int)(nas.Quality / 10.0M * (Decimal)vQuality.Maximum);
            }
        }
        #endregion
        private void vQuality_ValueChanged(object sender, EventArgs e)
        {
            Decimal q = ((Decimal)vQuality.Value) * 10.0M / vQuality.Maximum;
            label1.Text = String.Format("Variable Bitrate (Q={0}) ", q);
        }

        #region Editable<OggVorbisSettings> Members

        OggVorbisSettings Editable<OggVorbisSettings>.Settings
        {
            get
            {
                return (OggVorbisSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion
    }
}




