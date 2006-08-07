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
    /// Summary description for OggVorbisConfigurationDialog.
    /// </summary>
    public class OggVorbisConfigurationDialog : baseAudioConfigurationDialog
    {
        private Label label1;
        public TrackBar vQuality;
        #region variables

        #endregion
        #region start / stop
        public OggVorbisConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings,
            string initialProfile)
            : base(profileManager, path, settings, initialProfile)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            performSizeAndLayoutCorrection();
            vQuality_ValueChanged(null, null);
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
            this.label1 = new System.Windows.Forms.Label();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.vQuality);
            this.encoderGroupBox.Controls.Add(this.label1);
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 88);
            this.encoderGroupBox.Text = "OggVorbis Options";
            // 
            // vQuality
            // 
            this.vQuality.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.vQuality.Location = new System.Drawing.Point(3, 43);
            this.vQuality.Maximum = 1000;
            this.vQuality.Minimum = -200;
            this.vQuality.Name = "vQuality";
            this.vQuality.Size = new System.Drawing.Size(349, 42);
            this.vQuality.TabIndex = 22;
            this.vQuality.TickFrequency = 25;
            this.vQuality.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.vQuality.ValueChanged += new System.EventHandler(this.vQuality_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "label1";
            // 
            // OggVorbisConfigurationDialog
            // 
            this.ClientSize = new System.Drawing.Size(376, 502);
            this.Name = "OggVorbisConfigurationDialog";
            this.Text = "OggVorbis Encoder Configuration";
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        #region properties
        protected override Type supportedType
        {
            get { return typeof(OggVorbisSettings); }
        }
        /// <summary>
        /// gets / sets the settings that are being shown in this configuration dialog
        /// </summary>
        protected override AudioCodecSettings CodecSettings
        {
            get
            {
                OggVorbisSettings nas = new OggVorbisSettings();
                nas.Quality = (Decimal)vQuality.Value*10.0M / vQuality.Maximum;
                return nas;
            }
            set
            {
                OggVorbisSettings nas = value as OggVorbisSettings;
                vQuality.Value = (int)(nas.Quality/10.0M * (Decimal)vQuality.Maximum);
            }
        }
        #endregion


        private void vQuality_ValueChanged(object sender, EventArgs e)
        {
            Decimal q = ((Decimal)vQuality.Value) *10.0M/ vQuality.Maximum;
            label1.Text = String.Format("Variable Bitrate (Q={0}) ", q);
        }
    }
}