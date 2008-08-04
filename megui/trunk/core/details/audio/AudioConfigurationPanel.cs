using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.details.audio
{
    public partial class AudioConfigurationPanel : UserControl
    {
        private EnumProxy[] _avisynthChannelSet;
	    
        #region start / stop

	    public AudioConfigurationPanel()
	    {
            _avisynthChannelSet =
                EnumProxy.CreateArray(
                    this.IsMultichanelSupported
                    ?
                    (
                    this.IsMultichanelRequed
                    ?
                    new object[]{
                                ChannelMode.Upmix,
                                ChannelMode.UpmixUsingSoxEq,
                                ChannelMode.UpmixWithCenterChannelDialog
                    }
                    :
                    new object[]{
                                ChannelMode.KeepOriginal,
                                ChannelMode.StereoDownmix,
                                ChannelMode.DPLDownmix,
                                ChannelMode.DPLIIDownmix,
                                ChannelMode.ConvertToMono,
                                ChannelMode.Upmix,
                                ChannelMode.UpmixUsingSoxEq,
                                ChannelMode.UpmixWithCenterChannelDialog
                    }

                    )
                                :
                    new object[]{
                                ChannelMode.StereoDownmix,
                                ChannelMode.DPLDownmix,
                                ChannelMode.DPLIIDownmix,
                                ChannelMode.ConvertToMono
                    }
                    );

            InitializeComponent();
            this.besweetDownmixMode.DataSource = _avisynthChannelSet;
            this.besweetDownmixMode.BindingContext = new BindingContext();
        }
	    
		#endregion
		#region dropdowns
			
		#endregion
		#region checkboxes

        protected void showCommandLine()
	    {
	        
	    }

		#endregion
		#region properties


	    protected virtual bool IsMultichanelSupported
	    {
	        get
	        {
                return true;
	        }
	    }

        protected virtual bool IsMultichanelRequed
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Must collect data from UI / Fill UI from Data
        /// </summary>
        [Browsable(false)]
        protected virtual AudioCodecSettings CodecSettings
        {
            get
            {
                throw new NotImplementedException("Must be overridden");
            }
            set
            {
                throw new NotImplementedException("Must be overridden");
            }
        }

	    
		/// <summary>
		/// gets / sets the settings that are being shown in this configuration dialog
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AudioCodecSettings Settings
		{
			get
			{
                AudioCodecSettings fas = CodecSettings;
                fas.ImproveAccuracy = improvedAccuracy.Checked;
                fas.ForceDecodingViaDirectShow = forceDShowDecoding.Checked;
                EnumProxy o = besweetDownmixMode.SelectedItem as EnumProxy;
			    if(o!=null)
				    fas.DownmixMode = (ChannelMode) o.RealValue ;
				fas.AutoGain = autoGain.Checked;
				return fas;
			}
			set
			{
				AudioCodecSettings fas = value;
                besweetDownmixMode.SelectedItem = EnumProxy.Create(fas.DownmixMode);
                improvedAccuracy.Checked = fas.ImproveAccuracy;
                forceDShowDecoding.Checked = fas.ForceDecodingViaDirectShow;
				autoGain.Checked = fas.AutoGain;
                CodecSettings = fas;
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
		#region commandline
		private void besweetDelay_TextChanged(object sender, System.EventArgs e)
		{
		}
		#endregion
    }
}
