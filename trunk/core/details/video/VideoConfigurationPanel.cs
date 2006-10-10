using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;

namespace MeGUI.core.details.video
{
    public class VideoConfigurationPanel : UserControl
    {
        #region variables
        private bool updating = false;
        private double bytesPerFrame;
        private bool advancedToolTips;
        protected int lastEncodingMode = 0;

        private bool loaded;
        private int introEndFrame = 0, creditsStartFrame = 0;
        protected string input = "", output = "", encoderPath = "";
        #endregion
        protected ToolTip tooltipHelp;
        private IContainer components;
        #region start / stop
        public VideoConfigurationPanel()
            : this(null, new VideoInfo())
        { }


        public VideoConfigurationPanel(MainForm mainForm, VideoInfo info)
        {
            loaded = false;
            InitializeComponent();
            zonesControl.UpdateGUIEvent += new ZonesControl.UpdateConfigGUI(genericUpdate);
            zonesControl.MainForm = mainForm;

            input = info.videoIO[0];
            output = info.videoIO[1];
            zonesControl.IntroEndFrame = info.creditsAndIntroFrames[0];
            zonesControl.CreditsStartFrame = info.creditsAndIntroFrames[1];
        }

        private void VideoConfigurationPanel_Load(object sender, EventArgs e)
        {
            loaded = true;
            doCodecSpecificLoadAdjustments();
            genericUpdate();
        }

        private void VideoConfigurationPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible) zonesControl.closePlayer();
        }

        #endregion
        #region codec specific adjustments
        /// <summary>
        /// The method by which codecs can do their pre-commandline generation adjustments (eg tri-state adjustment).
        /// </summary>
        protected virtual void doCodecSpecificAdjustments() { }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected virtual void doCodecSpecificLoadAdjustments() { }
        
        /// <summary>
        /// Returns whether settings is a valid settings object for this instance. Should be implemented by one line:
        /// return (settings is xxxxSettings);
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected virtual bool isValidSettings(VideoCodecSettings settings)
        {
            throw new Exception("A bug in the program -- ProfilableConfigurationDialog.isValidSettings(GenericSettings) is not overridden");
        }

        /// <summary>
        /// Returns a new instance of the codec settings. This must be specific to the type of the config dialog, so
        /// that it can be set with the Settings.set property.
        /// </summary>
        /// <returns>A new instance of xxxSettings</returns>
        protected virtual VideoCodecSettings defaultSettings()
        {
            throw new Exception("A bug in the program -- ProfilableConfigurationDialog.defaultSettings() is not overridden");
        }
        #endregion
        #region showCommandline
        protected virtual string getCommandline()
        {
            return null;
            // CommandLineGenerator.generateVideoCommandline((VideoCodecSettings)this.Settings, this.input, this.output, -1, -1);
        }
        protected void showCommandLine()
        {
            if (!loaded)
                return;
            if (updating)
                return;
            updating = true;

            doCodecSpecificAdjustments();

            this.commandline.Text = encoderPath + " " + getCommandline();
            updating = false;
        }
        #endregion
        #region GUI events
        protected void genericUpdate()
        {
            showCommandLine();
        }
        #endregion
        #region properties

        public bool AdvancedToolTips
        {
            get { return advancedToolTips; }
            set { advancedToolTips = value; }
        }

        public double BytesPerFrame
        {
            get { return bytesPerFrame; }
            set { bytesPerFrame = value; }
        }

        /// <summary>
        /// sets the video input (for commandline generation and zone previews)
        /// </summary>
        public string Input
        {
            set
            {
                this.input = value;
                zonesControl.Input = value;
            }
        }
        /// <summary>
        ///  sets the video output (for commandline generation)
        /// </summary>
        public string Output
        {
            set { this.output = value; }
        }
        /// <summary>
        /// sets the path of besweet
        /// </summary>
        public string EncoderPath
        {
            set { this.encoderPath = value; }
        }
        /// <summary>
        /// gets / sets the start frame of the credits
        /// </summary>
        public int CreditsStartFrame
        {
            get { return this.creditsStartFrame; }
            set { creditsStartFrame = value; }
        }
        /// <summary>
        /// gets / sets the end frame of the intro
        /// </summary>
        public int IntroEndFrame
        {
            get { return this.introEndFrame; }
            set { introEndFrame = value; }
        }
        /// <summary>
        /// gets / sets the zones of the video
        /// </summary>
        public Zone[] Zones
        {
            get { return zonesControl.Zones; }
            set { zonesControl.Zones = value; }
        }
        #endregion
        
        
        
        
        protected TabControl tabControl1;
        protected TabPage zoneTabPage;
        protected TextBox commandline;
        protected TabPage mainTabPage;
        private ZonesControl zonesControl;
    

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.zoneTabPage = new System.Windows.Forms.TabPage();
            this.zonesControl = new MeGUI.ZonesControl();
            this.commandline = new System.Windows.Forms.TextBox();
            this.tooltipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.zoneTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.mainTabPage);
            this.tabControl1.Controls.Add(this.zoneTabPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(327, 344);
            this.tabControl1.TabIndex = 39;
            // 
            // mainTabPage
            // 
            this.mainTabPage.Location = new System.Drawing.Point(4, 22);
            this.mainTabPage.Name = "mainTabPage";
            this.mainTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mainTabPage.Size = new System.Drawing.Size(319, 318);
            this.mainTabPage.TabIndex = 0;
            this.mainTabPage.Text = "Main";
            this.mainTabPage.UseVisualStyleBackColor = true;
            // 
            // zoneTabPage
            // 
            this.zoneTabPage.Controls.Add(this.zonesControl);
            this.zoneTabPage.Location = new System.Drawing.Point(4, 22);
            this.zoneTabPage.Name = "zoneTabPage";
            this.zoneTabPage.Size = new System.Drawing.Size(319, 318);
            this.zoneTabPage.TabIndex = 2;
            this.zoneTabPage.Text = "Zones";
            this.zoneTabPage.UseVisualStyleBackColor = true;
            // 
            // zonesControl
            // 
            this.zonesControl.CreditsStartFrame = 0;
            this.zonesControl.Input = "";
            this.zonesControl.IntroEndFrame = 0;
            this.zonesControl.Location = new System.Drawing.Point(0, 3);
            this.zonesControl.MainForm = null;
            this.zonesControl.Name = "zonesControl";
            this.zonesControl.Size = new System.Drawing.Size(310, 305);
            this.zonesControl.TabIndex = 0;
            this.zonesControl.Zones = new MeGUI.Zone[0];
            // 
            // commandline
            // 
            this.commandline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.commandline.Location = new System.Drawing.Point(3, 350);
            this.commandline.Multiline = true;
            this.commandline.Name = "commandline";
            this.commandline.ReadOnly = true;
            this.commandline.Size = new System.Drawing.Size(324, 59);
            this.commandline.TabIndex = 41;
            // 
            // tooltipHelp
            // 
            this.tooltipHelp.AutoPopDelay = 30000;
            this.tooltipHelp.InitialDelay = 500;
            this.tooltipHelp.IsBalloon = true;
            this.tooltipHelp.ReshowDelay = 100;
            this.tooltipHelp.ShowAlways = true;
            // 
            // VideoConfigurationPanel
            // 
            this.Controls.Add(this.commandline);
            this.Controls.Add(this.tabControl1);
            this.Name = "VideoConfigurationPanel";
            this.Size = new System.Drawing.Size(331, 409);
            this.VisibleChanged += new System.EventHandler(this.VideoConfigurationPanel_VisibleChanged);
            this.Load += new System.EventHandler(this.VideoConfigurationPanel_Load);
            this.tabControl1.ResumeLayout(false);
            this.zoneTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
