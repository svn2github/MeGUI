// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.audio.naac
{
    public partial class neroConfigurationPanel : MeGUI.core.details.audio.AudioConfigurationPanel, Editable<NeroAACSettings>
    {
        #region start / stop
        public neroConfigurationPanel():base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            cbProfile.Items.AddRange(EnumProxy.CreateArray(NeroAACSettings.SupportedProfiles));
		}
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(neroConfigurationPanel));
            this.gradientPanel1 = new MeGUI.GradientPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gbQuality = new System.Windows.Forms.GroupBox();
            this.tbQuality = new System.Windows.Forms.TrackBar();
            this.gbBitrate = new System.Windows.Forms.GroupBox();
            this.cbCBR = new System.Windows.Forms.CheckBox();
            this.tbBitrate = new System.Windows.Forms.TrackBar();
            this.gbTarget = new System.Windows.Forms.GroupBox();
            this.rbQuality = new System.Windows.Forms.RadioButton();
            this.rbBitrate = new System.Windows.Forms.RadioButton();
            this.gbProfile = new System.Windows.Forms.GroupBox();
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.gbQuality.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).BeginInit();
            this.gbBitrate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBitrate)).BeginInit();
            this.gbTarget.SuspendLayout();
            this.gbProfile.SuspendLayout();
            this.SuspendLayout();
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(392, 212);
            this.besweetOptionsGroupbox.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.gbProfile);
            this.tabPage1.Controls.Add(this.gbQuality);
            this.tabPage1.Controls.Add(this.gbBitrate);
            this.tabPage1.Controls.Add(this.gbTarget);
            this.tabPage1.Size = new System.Drawing.Size(394, 298);
            this.tabPage1.UseVisualStyleBackColor = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(4, 96);
            this.tabControl1.Size = new System.Drawing.Size(402, 324);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientPanel1.Controls.Add(this.pictureBox2);
            this.gradientPanel1.Controls.Add(this.label3);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.PageEndColor = System.Drawing.Color.Empty;
            this.gradientPanel1.PageStartColor = System.Drawing.Color.SlateGray;
            this.gradientPanel1.Size = new System.Drawing.Size(409, 90);
            this.gradientPanel1.TabIndex = 45;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(283, 21);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 47);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(23, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Adjust your Settings here...";
            // 
            // gbQuality
            // 
            this.gbQuality.Controls.Add(this.tbQuality);
            this.gbQuality.Location = new System.Drawing.Point(6, 139);
            this.gbQuality.Name = "gbQuality";
            this.gbQuality.Size = new System.Drawing.Size(385, 75);
            this.gbQuality.TabIndex = 5;
            this.gbQuality.TabStop = false;
            this.gbQuality.Text = "Quality";
            // 
            // tbQuality
            // 
            this.tbQuality.Location = new System.Drawing.Point(6, 24);
            this.tbQuality.Maximum = 100;
            this.tbQuality.Name = "tbQuality";
            this.tbQuality.Size = new System.Drawing.Size(373, 45);
            this.tbQuality.TabIndex = 0;
            this.tbQuality.TickFrequency = 5;
            this.tbQuality.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbQuality.Scroll += new System.EventHandler(this.tbQuality_Scroll);
            // 
            // gbBitrate
            // 
            this.gbBitrate.Controls.Add(this.cbCBR);
            this.gbBitrate.Controls.Add(this.tbBitrate);
            this.gbBitrate.Location = new System.Drawing.Point(6, 58);
            this.gbBitrate.Name = "gbBitrate";
            this.gbBitrate.Size = new System.Drawing.Size(385, 75);
            this.gbBitrate.TabIndex = 4;
            this.gbBitrate.TabStop = false;
            this.gbBitrate.Text = "Bitrate";
            // 
            // cbCBR
            // 
            this.cbCBR.AutoSize = true;
            this.cbCBR.Location = new System.Drawing.Point(41, 52);
            this.cbCBR.Name = "cbCBR";
            this.cbCBR.Size = new System.Drawing.Size(195, 17);
            this.cbCBR.TabIndex = 1;
            this.cbCBR.Text = "Restrict Encoder to Constant Bitrate";
            this.cbCBR.UseVisualStyleBackColor = true;
            // 
            // tbBitrate
            // 
            this.tbBitrate.Location = new System.Drawing.Point(6, 19);
            this.tbBitrate.Maximum = 640;
            this.tbBitrate.Minimum = 16;
            this.tbBitrate.Name = "tbBitrate";
            this.tbBitrate.Size = new System.Drawing.Size(373, 45);
            this.tbBitrate.TabIndex = 0;
            this.tbBitrate.TickFrequency = 16;
            this.tbBitrate.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbBitrate.Value = 16;
            this.tbBitrate.Scroll += new System.EventHandler(this.tbBitrate_Scroll);
            // 
            // gbTarget
            // 
            this.gbTarget.Controls.Add(this.rbQuality);
            this.gbTarget.Controls.Add(this.rbBitrate);
            this.gbTarget.Location = new System.Drawing.Point(6, 6);
            this.gbTarget.Name = "gbTarget";
            this.gbTarget.Size = new System.Drawing.Size(385, 46);
            this.gbTarget.TabIndex = 3;
            this.gbTarget.TabStop = false;
            this.gbTarget.Text = "Target";
            // 
            // rbQuality
            // 
            this.rbQuality.AutoSize = true;
            this.rbQuality.Checked = true;
            this.rbQuality.Location = new System.Drawing.Point(240, 19);
            this.rbQuality.Name = "rbQuality";
            this.rbQuality.Size = new System.Drawing.Size(57, 17);
            this.rbQuality.TabIndex = 21;
            this.rbQuality.TabStop = true;
            this.rbQuality.Text = "Quality";
            this.rbQuality.UseVisualStyleBackColor = true;
            this.rbQuality.CheckedChanged += new System.EventHandler(this.target_CheckedChanged);
            // 
            // rbBitrate
            // 
            this.rbBitrate.AutoSize = true;
            this.rbBitrate.Location = new System.Drawing.Point(117, 19);
            this.rbBitrate.Name = "rbBitrate";
            this.rbBitrate.Size = new System.Drawing.Size(55, 17);
            this.rbBitrate.TabIndex = 20;
            this.rbBitrate.Text = "Bitrate";
            this.rbBitrate.UseVisualStyleBackColor = true;
            this.rbBitrate.CheckedChanged += new System.EventHandler(this.target_CheckedChanged);
            // 
            // gbProfile
            // 
            this.gbProfile.Controls.Add(this.cbProfile);
            this.gbProfile.Location = new System.Drawing.Point(6, 220);
            this.gbProfile.Name = "gbProfile";
            this.gbProfile.Size = new System.Drawing.Size(149, 75);
            this.gbProfile.TabIndex = 6;
            this.gbProfile.TabStop = false;
            this.gbProfile.Text = "Profile";
            // 
            // cbProfile
            // 
            this.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Location = new System.Drawing.Point(13, 30);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(121, 21);
            this.cbProfile.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(198, 253);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(169, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Nero Digital Audio Official Website";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // neroConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.gradientPanel1);
            this.Name = "neroConfigurationPanel";
            this.Size = new System.Drawing.Size(409, 433);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.gradientPanel1, 0);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.gbQuality.ResumeLayout(false);
            this.gbQuality.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).EndInit();
            this.gbBitrate.ResumeLayout(false);
            this.gbBitrate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBitrate)).EndInit();
            this.gbTarget.ResumeLayout(false);
            this.gbTarget.PerformLayout();
            this.gbProfile.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		#region properties
		/// <summary>
		/// gets / sets the settings that are being shown in this configuration dialog. These will be added to by the base class
		/// </summary>
		protected override AudioCodecSettings CodecSettings
		{
			get
			{
				NeroAACSettings nas = new NeroAACSettings();
                if (rbBitrate.Checked)
                {
                    if (cbCBR.Checked)
                        nas.BitrateMode = BitrateManagementMode.CBR;
                    else
                        nas.BitrateMode = BitrateManagementMode.ABR;
                    nas.Bitrate = this.tbBitrate.Value;
                }
                else
                {
                    nas.BitrateMode = BitrateManagementMode.VBR;
                    nas.Quality = (Decimal)this.tbQuality.Value / this.tbQuality.Maximum;
                } 
                nas.Profile = (AacProfile)(cbProfile.SelectedItem as EnumProxy).RealValue;
				return nas;
			}
			set
			{
                NeroAACSettings nas = (NeroAACSettings)value;
                switch (nas.BitrateMode)
                {
                    case BitrateManagementMode.VBR: rbQuality.Checked = true; cbCBR.Checked = false; break;
                    case BitrateManagementMode.ABR: rbBitrate.Checked = true; cbCBR.Checked = false; break;
                    case BitrateManagementMode.CBR: rbBitrate.Checked = true; cbCBR.Checked = true; break;
                }
                tbBitrate.Value = Math.Max(Math.Min(nas.Bitrate, tbBitrate.Maximum), tbBitrate.Minimum);
                tbQuality.Value = (int)(nas.Quality * (Decimal)tbQuality.Maximum);
                cbProfile.SelectedItem = EnumProxy.Create(nas.Profile);

                target_CheckedChanged(null, null);
			}
		}
		#endregion

        #region Editable<NeroAACSettings> Members

        NeroAACSettings Editable<NeroAACSettings>.Settings
        {
            get
            {
                return (NeroAACSettings)Settings;
            }
            set
            {
                Settings = value;
            }
        }

        #endregion

        private void tbBitrate_Scroll(object sender, EventArgs e)
        {
            gbBitrate.Text = String.Format("Bitrate ({0} kbps)", tbBitrate.Value); 
        }

        private void tbQuality_Scroll(object sender, EventArgs e)
        {
            gbQuality.Text = String.Format("Quality (Q = {0})", ((Decimal)tbQuality.Value / tbQuality.Maximum));
        }

        private void target_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBitrate.Checked)
            {
                gbQuality.Enabled = false;
                gbBitrate.Enabled = true;
            }
            else
            {
                gbBitrate.Enabled = false;
                gbQuality.Enabled = true;
            }

            tbBitrate_Scroll(null, null);
            tbQuality_Scroll(null, null);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open link that was clicked.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void VisitLink()
        {
            //Call the Process.Start method to open the default browser 
            //with a URL:
            System.Diagnostics.Process.Start("http://www.nero.com/enu/technologies-aac-codec.html");
        }

    }
}





