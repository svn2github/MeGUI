using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using MeGUI;

namespace MeGUI
{
	/// <summary>
	/// Summary description for lameConfigurationDialog.
	/// </summary>
	public class lameConfigurationDialog : baseAudioConfigurationDialog
	{
		#region variables
		private System.Windows.Forms.NumericUpDown bitrate;
		private System.Windows.Forms.ComboBox encodingMode;
		private System.Windows.Forms.Label encodingModeLabel;
		private System.Windows.Forms.Label bitrateLabel;
		private System.Windows.Forms.NumericUpDown quality;
		private System.Windows.Forms.Label qualityLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		#endregion
		#region start / stop
		/// <summary>
		/// default constructor
		/// initializes the GUI components and profiles
		/// </summary>
		/// <param name="audioProfiles">all audio profiles known</param>
		/// <param name="path">path where MeGUI was started from</param>
		/// <param name="initialProfile">the name of the currently selected audio profile</param>
        public lameConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings, 
			string initialProfile):
                base(profileManager, path, settings, initialProfile)

		{
			//
			// Required for Windows Form Designer support
			//
            InitializeComponent();
			this.encodingMode.Items.Add(BitrateManagementMode.CBR);
            this.encodingMode.Items.Add(BitrateManagementMode.VBR);
            this.encodingMode.Items.Add(BitrateManagementMode.ABR);
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
            this.qualityLabel = new System.Windows.Forms.Label();
            this.quality = new System.Windows.Forms.NumericUpDown();
            this.bitrateLabel = new System.Windows.Forms.Label();
            this.encodingModeLabel = new System.Windows.Forms.Label();
            this.encodingMode = new System.Windows.Forms.ComboBox();
            this.bitrate = new System.Windows.Forms.NumericUpDown();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bitrate)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.qualityLabel);
            this.encoderGroupBox.Controls.Add(this.quality);
            this.encoderGroupBox.Controls.Add(this.bitrateLabel);
            this.encoderGroupBox.Controls.Add(this.encodingModeLabel);
            this.encoderGroupBox.Controls.Add(this.encodingMode);
            this.encoderGroupBox.Controls.Add(this.bitrate);
            this.encoderGroupBox.Location = new System.Drawing.Point(9, 169);
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 105);
            this.encoderGroupBox.Text = "Lame Options";
            // 
            // qualityLabel
            // 
            this.qualityLabel.Location = new System.Drawing.Point(8, 70);
            this.qualityLabel.Name = "qualityLabel";
            this.qualityLabel.Size = new System.Drawing.Size(100, 18);
            this.qualityLabel.TabIndex = 5;
            this.qualityLabel.Text = "Quality";
            // 
            // quality
            // 
            this.quality.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.quality.Location = new System.Drawing.Point(232, 66);
            this.quality.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.quality.Name = "quality";
            this.quality.Size = new System.Drawing.Size(48, 21);
            this.quality.TabIndex = 4;
            this.quality.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // bitrateLabel
            // 
            this.bitrateLabel.Location = new System.Drawing.Point(8, 46);
            this.bitrateLabel.Name = "bitrateLabel";
            this.bitrateLabel.Size = new System.Drawing.Size(100, 23);
            this.bitrateLabel.TabIndex = 3;
            this.bitrateLabel.Text = "Bitrate";
            // 
            // encodingModeLabel
            // 
            this.encodingModeLabel.Location = new System.Drawing.Point(8, 22);
            this.encodingModeLabel.Name = "encodingModeLabel";
            this.encodingModeLabel.Size = new System.Drawing.Size(100, 23);
            this.encodingModeLabel.TabIndex = 2;
            this.encodingModeLabel.Text = "Encoding Mode";
            // 
            // encodingMode
            // 
            this.encodingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodingMode.Location = new System.Drawing.Point(160, 18);
            this.encodingMode.Name = "encodingMode";
            this.encodingMode.Size = new System.Drawing.Size(121, 21);
            this.encodingMode.TabIndex = 1;
            this.encodingMode.SelectedIndexChanged += new System.EventHandler(this.encodingMode_SelectedIndexChanged);
            // 
            // bitrate
            // 
            this.bitrate.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.bitrate.Location = new System.Drawing.Point(232, 42);
            this.bitrate.Maximum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.bitrate.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.bitrate.Name = "bitrate";
            this.bitrate.Size = new System.Drawing.Size(48, 21);
            this.bitrate.TabIndex = 0;
            this.bitrate.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // lameConfigurationDialog
            // 
            this.ClientSize = new System.Drawing.Size(376, 368);
            this.Name = "lameConfigurationDialog";
            this.Text = "MP3 Encoder Configuration";
            this.encoderGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.quality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bitrate)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		#region properties
        protected override AudioCodecSettings defaultSettings()
        {
            return new MP3Settings();
        }
        protected override bool IsMultichanelSupported
        {
            get
            {
                return false;
            }
        }

        protected override Type supportedType
        {
            get { return typeof(MP3Settings); }
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
	}
}