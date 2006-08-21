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
	public class NeroAACConfigurationDialog : baseAudioConfigurationDialog
	{
        public CheckBox cbxCreateHintTrack;
        public TrackBar vQuality;
        public RadioButton rbtnVBR;
        public TrackBar vBitrate;
        public RadioButton rbtnCBR;
        private ComboBox comboBox1;
        private Label label1;
        public RadioButton rbtnABR;
		#region variables

        #endregion
        #region start / stop
        public NeroAACConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings, 
			string initialProfile)
            : base(profileManager, path, settings, initialProfile)

		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            comboBox1.Items.AddRange(EnumProxy.CreateArray(typeof(AacProfile)));
            performSizeAndLayoutCorrection();
            rbtnABR_CheckedChanged(null, null);
            vBitrate_ValueChanged(null, null);
		}


		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.vQuality = new System.Windows.Forms.TrackBar();
            this.rbtnVBR = new System.Windows.Forms.RadioButton();
            this.vBitrate = new System.Windows.Forms.TrackBar();
            this.rbtnCBR = new System.Windows.Forms.RadioButton();
            this.rbtnABR = new System.Windows.Forms.RadioButton();
            this.cbxCreateHintTrack = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vBitrate)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.label1);
            this.encoderGroupBox.Controls.Add(this.comboBox1);
            this.encoderGroupBox.Controls.Add(this.cbxCreateHintTrack);
            this.encoderGroupBox.Controls.Add(this.vQuality);
            this.encoderGroupBox.Controls.Add(this.rbtnVBR);
            this.encoderGroupBox.Controls.Add(this.vBitrate);
            this.encoderGroupBox.Controls.Add(this.rbtnCBR);
            this.encoderGroupBox.Controls.Add(this.rbtnABR);
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 235);
            this.encoderGroupBox.Text = "NeroDigital AAC Options";
            // 
            // vQuality
            // 
            this.vQuality.Dock = System.Windows.Forms.DockStyle.Top;
            this.vQuality.Location = new System.Drawing.Point(3, 131);
            this.vQuality.Maximum = 100;
            this.vQuality.Name = "vQuality";
            this.vQuality.Size = new System.Drawing.Size(349, 42);
            this.vQuality.TabIndex = 22;
            this.vQuality.TickFrequency = 5;
            this.vQuality.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.vQuality.ValueChanged += new System.EventHandler(this.vBitrate_ValueChanged);
            // 
            // rbtnVBR
            // 
            this.rbtnVBR.Dock = System.Windows.Forms.DockStyle.Top;
            this.rbtnVBR.Location = new System.Drawing.Point(3, 107);
            this.rbtnVBR.Name = "rbtnVBR";
            this.rbtnVBR.Size = new System.Drawing.Size(349, 24);
            this.rbtnVBR.TabIndex = 19;
            this.rbtnVBR.Text = "Variable bit rate";
            this.rbtnVBR.CheckedChanged += new System.EventHandler(this.rbtnABR_CheckedChanged);
            // 
            // vBitrate
            // 
            this.vBitrate.Dock = System.Windows.Forms.DockStyle.Top;
            this.vBitrate.Location = new System.Drawing.Point(3, 65);
            this.vBitrate.Maximum = 320;
            this.vBitrate.Minimum = 16;
            this.vBitrate.Name = "vBitrate";
            this.vBitrate.Size = new System.Drawing.Size(349, 42);
            this.vBitrate.TabIndex = 20;
            this.vBitrate.TickFrequency = 8;
            this.vBitrate.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.vBitrate.Value = 16;
            this.vBitrate.ValueChanged += new System.EventHandler(this.vBitrate_ValueChanged);
            // 
            // rbtnCBR
            // 
            this.rbtnCBR.Dock = System.Windows.Forms.DockStyle.Top;
            this.rbtnCBR.Location = new System.Drawing.Point(3, 41);
            this.rbtnCBR.Name = "rbtnCBR";
            this.rbtnCBR.Size = new System.Drawing.Size(349, 24);
            this.rbtnCBR.TabIndex = 18;
            this.rbtnCBR.Text = "Constant bit rate";
            this.rbtnCBR.CheckedChanged += new System.EventHandler(this.rbtnABR_CheckedChanged);
            // 
            // rbtnABR
            // 
            this.rbtnABR.Dock = System.Windows.Forms.DockStyle.Top;
            this.rbtnABR.Location = new System.Drawing.Point(3, 17);
            this.rbtnABR.Name = "rbtnABR";
            this.rbtnABR.Size = new System.Drawing.Size(349, 24);
            this.rbtnABR.TabIndex = 21;
            this.rbtnABR.Text = "Adaptive bit rate";
            this.rbtnABR.CheckedChanged += new System.EventHandler(this.rbtnABR_CheckedChanged);
            // 
            // cbxCreateHintTrack
            // 
            this.cbxCreateHintTrack.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbxCreateHintTrack.Location = new System.Drawing.Point(3, 200);
            this.cbxCreateHintTrack.Name = "cbxCreateHintTrack";
            this.cbxCreateHintTrack.Size = new System.Drawing.Size(349, 32);
            this.cbxCreateHintTrack.TabIndex = 23;
            this.cbxCreateHintTrack.Text = "Create hint track (for streaming server)";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(106, 179);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(240, 21);
            this.comboBox1.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "AAC Profile";
            // 
            // neroConfigurationDialog
            // 
            this.ClientSize = new System.Drawing.Size(376, 502);
            this.Name = "neroConfigurationDialog";
            this.Text = "NeroDigital AAC Encoder Configuration";
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vBitrate)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		#region properties
        protected override AudioCodecSettings defaultSettings()
        {
            return new NeroAACSettings();
        }
        protected override Type supportedType
        {
            get { return typeof(NeroAACSettings); }
        }
		/// <summary>
		/// gets / sets the settings that are being shown in this configuration dialog
		/// </summary>
		protected override AudioCodecSettings CodecSettings
		{
			get
			{
				NeroAACSettings nas = new NeroAACSettings();
                if (rbtnABR.Checked) nas.BitrateMode = BitrateManagementMode.ABR;
                if (rbtnCBR.Checked) nas.BitrateMode = BitrateManagementMode.CBR;
                if (rbtnVBR.Checked) nas.BitrateMode = BitrateManagementMode.VBR;
                nas.Bitrate = vBitrate.Value;
                nas.Quality= (Decimal)vQuality.Value/vQuality.Maximum;
                nas.CreateHintTrack = cbxCreateHintTrack.Checked;
                nas.Profile = (AacProfile)(comboBox1.SelectedItem as EnumProxy).RealValue;
				return nas;
			}
			set
			{
                NeroAACSettings nas = value as NeroAACSettings;
                rbtnABR.Checked = nas.BitrateMode == BitrateManagementMode.ABR;
                rbtnCBR.Checked = nas.BitrateMode == BitrateManagementMode.CBR;
                rbtnVBR.Checked = nas.BitrateMode == BitrateManagementMode.VBR;
                vBitrate.Value = Math.Max(Math.Min(nas.Bitrate, vBitrate.Maximum), vBitrate.Minimum);
                vQuality.Value = (int)(nas.Quality * (Decimal)vQuality.Maximum);
                cbxCreateHintTrack.Checked = nas.CreateHintTrack;
                comboBox1.SelectedItem = EnumProxy.Create(nas.Profile);
			}
		}
		#endregion

        private void rbtnABR_CheckedChanged(object sender, EventArgs e)
        {
            vBitrate.Enabled = !(vQuality.Enabled = rbtnVBR.Checked);
        }

        private void vBitrate_ValueChanged(object sender, EventArgs e)
        {
            rbtnABR.Text = String.Format("Adaptive Bitrate @ {0} kbit/s", vBitrate.Value);
            rbtnCBR.Text = String.Format("Constant Bitrate @ {0} kbit/s", vBitrate.Value);
            Decimal q = ((Decimal)vQuality.Value) / vQuality.Maximum;
            rbtnVBR.Text = String.Format("Variable Bitrate (Q={0}) ", q);
        }
	}
}