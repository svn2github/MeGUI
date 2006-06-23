// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace MeGUI
{
	/// <summary>
	/// Summary description for neroConfigurationDialog.
	/// </summary>
	public class FaacConfigurationDialog : baseAudioConfigurationDialog
	{
		#region variables
		private System.Windows.Forms.ComboBox cbrBitrate;
		private System.Windows.Forms.NumericUpDown vbrQuality;
		private System.Windows.Forms.RadioButton cbrBitrateRadioButton;
		private System.Windows.Forms.RadioButton qualityModeRadioButton;
		#endregion
		#region start / stop
		public FaacConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings,
            string initialProfile)
            : base(profileManager, path, settings, initialProfile)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            cbrBitrate.DataSource = FaacSettings.SupportedBitrates;
            cbrBitrate.BindingContext = new BindingContext();
            performSizeAndLayoutCorrection();
        }
        #endregion
        #region Windows Form Designer generated code
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.vbrQuality = new System.Windows.Forms.NumericUpDown();
            this.qualityModeRadioButton = new System.Windows.Forms.RadioButton();
            this.cbrBitrate = new System.Windows.Forms.ComboBox();
            this.cbrBitrateRadioButton = new System.Windows.Forms.RadioButton();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vbrQuality)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.vbrQuality);
            this.encoderGroupBox.Controls.Add(this.qualityModeRadioButton);
            this.encoderGroupBox.Controls.Add(this.cbrBitrate);
            this.encoderGroupBox.Controls.Add(this.cbrBitrateRadioButton);
            this.encoderGroupBox.Location = new System.Drawing.Point(9, 191);
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 87);
            this.encoderGroupBox.Text = "FAAC Options";
            // 
            // vbrQuality
            // 
            this.vbrQuality.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.vbrQuality.Location = new System.Drawing.Point(160, 56);
            this.vbrQuality.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.vbrQuality.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.vbrQuality.Name = "vbrQuality";
            this.vbrQuality.Size = new System.Drawing.Size(120, 21);
            this.vbrQuality.TabIndex = 3;
            this.vbrQuality.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // qualityModeRadioButton
            // 
            this.qualityModeRadioButton.Location = new System.Drawing.Point(8, 56);
            this.qualityModeRadioButton.Name = "qualityModeRadioButton";
            this.qualityModeRadioButton.Size = new System.Drawing.Size(48, 24);
            this.qualityModeRadioButton.TabIndex = 2;
            this.qualityModeRadioButton.Text = "VBR";
            // 
            // cbrBitrate
            // 
            this.cbrBitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbrBitrate.Location = new System.Drawing.Point(160, 24);
            this.cbrBitrate.Name = "cbrBitrate";
            this.cbrBitrate.Size = new System.Drawing.Size(121, 21);
            this.cbrBitrate.TabIndex = 1;
            // 
            // cbrBitrateRadioButton
            // 
            this.cbrBitrateRadioButton.Checked = true;
            this.cbrBitrateRadioButton.Location = new System.Drawing.Point(8, 24);
            this.cbrBitrateRadioButton.Name = "cbrBitrateRadioButton";
            this.cbrBitrateRadioButton.Size = new System.Drawing.Size(56, 24);
            this.cbrBitrateRadioButton.TabIndex = 0;
            this.cbrBitrateRadioButton.TabStop = true;
            this.cbrBitrateRadioButton.Text = "ABR";
            this.cbrBitrateRadioButton.CheckedChanged += new System.EventHandler(this.bitrateModeChanged);
            // 
            // faacConfigurationDialog
            // 
            this.ClientSize = new System.Drawing.Size(376, 368);
            this.Name = "faacConfigurationDialog";
            this.Text = "FAAC Encoder Configuration";
            this.encoderGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vbrQuality)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

	    protected override Type supportedType
	    {
	        get { return typeof(FaacSettings); }
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
    }
}