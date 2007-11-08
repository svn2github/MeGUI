using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using MeGUI.core.details.video;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.packages.video.x264farm
{
    public partial class x264farmConfigurationPanel : MeGUI.core.details.video.VideoConfigurationPanel, Gettable<VideoCodecSettings>
    {
        class ProfileLister : IProfileList
        {
            MainForm mainForm;
            #region IProfileList Members

            public Profile[] Profiles
            {
                get
                {
                    List<Profile> profiles = new List<Profile>();
                    foreach (Profile prof in mainForm.Profiles.Profiles("Video").Values)
                    {
                        if (prof.BaseSettings is x264Settings)
                            profiles.Add(prof);
                    }
                    return profiles.ToArray();
                }
            }

            public Profile ByName(string name)
            {
                return mainForm.Profiles.Profiles("Video")[name];
            }

            public ProfileLister(MainForm mf)
            {
                this.mainForm = mf;
            }
            #endregion
        }

        private MainForm mainForm;
        private VideoInfo info;
        private XmlDocument ContextHelp = new XmlDocument();

        #region x264 profiles

        private x264Settings x264LocalSettings
        {
            set
            {
                this.x264MinimimQuantizer.Value = value.MinQuantizer;
                this.x264MaximumQuantizer.Value = value.MaxQuantizer;
                this.x264MaxQuantDelta.Value = value.MaxQuantDelta;
                this.x264IPFrameFactor.Value = value.IPFactor;
                this.x264PBFrameFactor.Value = value.PBFactor;
                this.x264QuantizerCompression.Value = value.QuantCompression;
                this.x264TempFrameComplexityBlur.Value = value.TempComplexityBlur;
                this.x264TempQuantBlur.Value = value.TempQuantBlur;
                this.Zones = value.Zones;

                //SPW make Default, to Omit from passes for x264farm
                x264Settings x264defaultSettings = new x264Settings();

                value.MinQuantizer = x264defaultSettings.MinQuantizer;
                value.MaxQuantizer = x264defaultSettings.MaxQuantizer;
                value.MaxQuantDelta = x264defaultSettings.MaxQuantDelta;
                value.IPFactor = x264defaultSettings.IPFactor;
                value.PBFactor = x264defaultSettings.PBFactor;
                value.QuantCompression = x264defaultSettings.QuantCompression;
                value.TempComplexityBlur = x264defaultSettings.TempComplexityBlur;
                value.TempQuantBlur = x264defaultSettings.TempQuantBlur;
                value.Zones = x264defaultSettings.Zones;
                x264defaultSettings = value.clone() as x264Settings;

                bool turbo = value.Turbo;

                //SPW generate secondpass settings.
                //x264defaultSettings.EncodingMode = 4;
                //value.EncodingMode = (int)VideoCodecSettings.Mode.twopassAutomated;
                this.secondPass.Text = x264Encoder.generateX264CLICommandlineMid(x264defaultSettings, null, 0, 0);

                //SPW generate firstpass settings.
                //value.EncodingMode = (int)VideoCodecSettings.Mode.twopass1;
                x264defaultSettings.EncodingMode = 2;
                x264defaultSettings.Turbo = turbo;
                this.firstPass.Text = "--crf 18 " + x264Encoder.generateX264CLICommandlineMid(x264defaultSettings, null, 0, 0);
            }
        }

        static ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> x264settingsProvider = CodecManager.X264;
        SingleConfigurerHandler<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> x264configHandler;
        ProfilesControlHandler<VideoCodecSettings, VideoInfo> x264profileHandler;
        private void initX264Handler()
        {
            x264profileHandler = new ProfilesControlHandler<VideoCodecSettings, VideoInfo>(
                "Video:X264", this.mainForm, profileControl1, x264settingsProvider.EditSettings, new InfoGetter<VideoInfo>(delegate { return info; }),
                new SettingsGetter<VideoCodecSettings>(delegate() { return x264settingsProvider.GetCurrentSettings(); }),
                new SettingsSetter<VideoCodecSettings>(delegate(VideoCodecSettings s) { x264settingsProvider.LoadSettings(s); }));
            x264configHandler =
                new SingleConfigurerHandler<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>(
                x264profileHandler, x264settingsProvider);
            x264configHandler.ProfileChanged += new SelectedProfileChangedEvent(configHandler_ProfileChanged);
        }

        void configHandler_ProfileChanged(object sender, Profile prof)
        {
            if (MessageBox.Show("Do you want to replace your Viedo settings?", "Update x264 Settnigs", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                x264LocalSettings = (x264settingsProvider.GetCurrentSettings() as x264Settings);
            }
        }

        #endregion

        /// <summary>
        /// x264farm configuration component.
        /// This will configure controller's parameters.
        /// </summary>
        public void init()
        {
            initX264Handler();
        }


        protected override string getCommandline()
        {
            return x264farmEncoder.genCommandline(this.input, this.output, null, -1, -1, (x264farmSettings)this.Settings);
        }

        #region start / stop
        public x264farmConfigurationPanel(MainForm mainForm, VideoInfo info)
            : base(mainForm, info)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            this.AdvancedToolTips = mainForm.Settings.UseAdvancedTooltips;
            this.info = info;
            init();
        }
        #endregion

        #region adjustments
        #region checkboxes
        private void doCheckBoxAdjustments()
        {
            gopsReEnc.Enabled = !cbgopsReEnc.Checked;
            configExtPath.Enabled = configExt.Checked;
            threshFPResplit.Enabled = !cbtreshFPReSplit.Checked;
        }
        #endregion
        #endregion
        #region codec-specific overload functions
        /// <summary>
        /// Does all the necessary adjustments after a GUI change has been made.
        /// </summary>
        protected override void doCodecSpecificAdjustments()
        {
            doCheckBoxAdjustments();
        }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected override void doCodecSpecificLoadAdjustments()
        {
            if (bitrateMod.SelectedIndex == -1)
                this.bitrateMod.SelectedIndex = 0;
            try
            {
                ContextHelp.Load(Application.StartupPath + "\\Data\\ContextHelp.xml");
                SetToolTips();
            }
            catch
            {
                MessageBox.Show("The ContextHelp.xml file could not be found. Please check in the 'Data' directory to see if it exists. Help tooltips will not be available.", "File Not Found", MessageBoxButtons.OK);
            }
        }

        protected override bool isValidSettings(VideoCodecSettings settings)
        {
            return settings is x264farmSettings;
        }

        /// <summary>
        /// Returns a new instance of lfarmSettings.
        /// </summary>
        /// <returns>A new instance of lfarmSettings</returns>
        protected override VideoCodecSettings defaultSettings()
        {
            return new x264farmSettings();
        }

        /// <summary>
        /// gets / sets the settings currently displayed on the GUI
        /// </summary>
        public VideoCodecSettings Settings
        {
            get
            {
                x264farmSettings xs = new x264farmSettings();
                xs.BitrateQuantizer = (int)this.bitrate.Value;
                xs.EncodingMode = this.bitrateMod.SelectedIndex;
                xs.FirstPassSettings = this.firstPass.Text;
                xs.SecondPassSettings = this.secondPass.Text;
                xs.FirstPassAVSFast = this.fastAVS.Text;
                xs.FirstPassAVS = this.firstAVS.Text;
                xs.PreseekFrames = (int)this.preseekFrames.Value;
                xs.ForceSPReEnc = this.forceReEnc.Checked;
                xs.RestartTotal = this.restartTotal.Checked;
                xs.SavediskOnMerge = this.savediskOnMerge.Checked;
                xs.NoCompression = this.noCompression.Checked;
                xs.ConfigExt = this.configExt.Checked;
                xs.ConfigPath = this.configExtPath.Text;
                xs.BatchFPSize = (int)this.batchFPSize.Value;
                xs.BatchFPMulti = this.batchFPMulti.Value;
                xs.SplitFPFrames = (int)this.splitFPFrames.Value;
                xs.ThreshFPSplit = this.threshFPSplit.Value;
                xs.ThreshFPReSplitDont = this.cbtreshFPReSplit.Checked;
                xs.ThreshFPReSplit = this.threshFPResplit.Value;
                xs.ThreshTPAccuracy = this.threshTPAccuracy.Value;
                xs.GopsReEncDont = this.cbgopsReEnc.Checked;
                if (!this.gopsReEnc.Text.Equals(""))
                    xs.GopsReEnc = int.Parse(this.gopsReEnc.Text);
                xs.RatioTPGopsReEnc = this.ratioTPGopsReEnc.Value;
                xs.ReRateControlGopsReEnc = (int)this.reRateControlGopsReEnc.Value;
                xs.SizePrecision = this.sizePrecision.Value;
                xs.Zones = this.Zones;
                xs.X264Seek = (int)this.x264Seek.Value;
                xs.X264Frames = (int)this.x264Frames.Value;
                xs.X264MinQuantizer = (int)this.x264MinimimQuantizer.Value;
                xs.X264MaxQuantizer = (int)this.x264MaximumQuantizer.Value;
                xs.X264MaxQuantDelta = (int)this.x264MaxQuantDelta.Value;
                xs.X264QuantCompression = this.x264QuantizerCompression.Value;
                xs.X264TempComplexityBlur = this.x264TempFrameComplexityBlur.Value;
                xs.X264TempQuanBlurCC = this.x264TempQuantBlur.Value;
                xs.X264IPFactor = this.x264IPFrameFactor.Value;
                xs.X264PBFactor = this.x264PBFrameFactor.Value;
                xs.CustomEncoderOptions = this.customCommandlineOptions.Text;
                xs.Logfile = this.logFile.Text;
                return xs;
            }
            set
            {  // Warning! The ordering of components matters because of the dependency code!
                x264farmSettings xs = (x264farmSettings)value;
                this.bitrate.Value = xs.BitrateQuantizer;
                this.bitrateMod.SelectedIndex = xs.EncodingMode;
# warning Maybe workaround for normal x264Settings Container... but.. later...
                this.firstPass.Text = xs.FirstPassSettings;
                this.secondPass.Text = xs.SecondPassSettings;

                this.fastAVS.Text = xs.FirstPassAVSFast;
                this.firstAVS.Text = xs.FirstPassAVS;

                this.preseekFrames.Value = xs.PreseekFrames;
                this.forceReEnc.Checked = xs.ForceSPReEnc;
                this.restartTotal.Checked = xs.RestartTotal;
                this.savediskOnMerge.Checked = xs.SavediskOnMerge;
                this.noCompression.Checked = xs.NoCompression;
                this.configExt.Checked = xs.ConfigExt;
                this.configExtPath.Text = xs.ConfigPath;
                this.batchFPSize.Value = xs.BatchFPSize;
                this.batchFPMulti.Value = xs.BatchFPMulti;
                this.splitFPFrames.Value = xs.SplitFPFrames;
                this.threshFPSplit.Value = xs.ThreshFPSplit;
                this.cbtreshFPReSplit.Checked = xs.ThreshFPReSplitDont;
                this.threshFPResplit.Enabled = !xs.ThreshFPReSplitDont;
                this.threshFPResplit.Value = xs.ThreshFPReSplit;
                this.threshTPAccuracy.Value = xs.ThreshTPAccuracy;
                this.cbgopsReEnc.Checked = xs.GopsReEncDont;
                this.gopsReEnc.Enabled = !xs.GopsReEncDont;
                if (xs.GopsReEnc > 0)
                    this.gopsReEnc.Text = xs.GopsReEnc.ToString();
                else
                    this.gopsReEnc.Text = "";

                this.ratioTPGopsReEnc.Value = xs.RatioTPGopsReEnc;
                this.reRateControlGopsReEnc.Value = xs.ReRateControlGopsReEnc;
                this.sizePrecision.Value = xs.SizePrecision;
                this.Zones = xs.Zones;
                this.x264Seek.Value = xs.X264Seek;
                this.x264Frames.Value = xs.X264Frames;
                this.x264MinimimQuantizer.Value = xs.X264MinQuantizer;
                this.x264MaximumQuantizer.Value = xs.X264MaxQuantizer;
                this.x264MaxQuantDelta.Value = xs.X264MaxQuantDelta;
                this.x264QuantizerCompression.Value = xs.X264QuantCompression;
                this.x264TempFrameComplexityBlur.Value = xs.X264TempComplexityBlur;
                this.x264TempQuantBlur.Value = xs.X264TempQuanBlurCC;
                this.x264IPFrameFactor.Value = xs.X264IPFactor;
                this.x264PBFrameFactor.Value = xs.X264PBFactor;
                this.customCommandlineOptions.Text = xs.CustomEncoderOptions;
                this.logFile.Text = xs.Logfile;
            }
        }
        #endregion
        #region events
        private void updateEvent(object sender, EventArgs e)
        {
            genericUpdate();
        }
        private void textField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }
    
        #endregion
        #region ContextHelp
        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            string xpath = "/ContextHelp/Codec[@name='x264farm']/" + node;
            XmlNodeList nl = ContextHelp.SelectNodes(xpath); // Return the details for the specified node

            if (nl.Count == 1) // if it finds the required HelpText, count should be 1
            {
                HelpText.Append(nl[0].Attributes["name"].Value);
                HelpText.AppendLine();
                if (AdvancedToolTips)
                    HelpText.AppendLine(nl[0]["Advanced"].InnerText);
                else
                    HelpText.AppendLine(nl[0]["Basic"].InnerText);
                HelpText.AppendLine();
                HelpText.AppendLine("Default Value: " + nl[0]["Default"].InnerText);
                HelpText.AppendLine("Recommended Value: " + nl[0]["Recommended"].InnerText);
            }
            else // If count isn't 1, then theres no valid data.
                HelpText.Append("Error: No data available");

            return (HelpText.ToString());
        }
        private void SetToolTips()
        {
            tooltipHelp.SetToolTip(firstPass, SelectHelpText("firstpass"));
            tooltipHelp.SetToolTip(secondPass, SelectHelpText("secondpass"));
            tooltipHelp.SetToolTip(bitrate, SelectHelpText("bitrate"));
            tooltipHelp.SetToolTip(bitrateMod, SelectHelpText("bitrate-mod"));
            tooltipHelp.SetToolTip(sizePrecision, SelectHelpText("size-precision"));
            tooltipHelp.SetToolTip(preseekFrames, SelectHelpText("preseek"));
            tooltipHelp.SetToolTip(noCompression, SelectHelpText("no-compression"));
            tooltipHelp.SetToolTip(savediskOnMerge, SelectHelpText("save-disk"));
            tooltipHelp.SetToolTip(restartTotal, SelectHelpText("restart"));
            tooltipHelp.SetToolTip(forceReEnc, SelectHelpText("force"));

            tooltipHelp.SetToolTip(configExt, SelectHelpText("config"));
            tooltipHelp.SetToolTip(configExtPath, SelectHelpText("config-text"));

            tooltipHelp.SetToolTip(firstAVS, SelectHelpText("firstavs"));
            tooltipHelp.SetToolTip(fastAVS, SelectHelpText("fastavs"));

            tooltipHelp.SetToolTip(batchFPSize, SelectHelpText("batch-fp-size"));
            tooltipHelp.SetToolTip(batchFPMulti, SelectHelpText("batch-fp-multi"));
            tooltipHelp.SetToolTip(splitFPFrames, SelectHelpText("split-fp-frames"));
            tooltipHelp.SetToolTip(threshFPSplit, SelectHelpText("split-fp-thresh"));
            tooltipHelp.SetToolTip(threshFPResplit, SelectHelpText("split-fp-rethresh"));
            tooltipHelp.SetToolTip(threshTPAccuracy, SelectHelpText("thresh-tp-accuracy"));

            tooltipHelp.SetToolTip(gopsReEnc, SelectHelpText("gops-reencode"));
            tooltipHelp.SetToolTip(ratioTPGopsReEnc, SelectHelpText("ratio-tp-gops-reencode"));
            tooltipHelp.SetToolTip(reRateControlGopsReEnc, SelectHelpText("re-ratecontrol-gops"));

        }
        #endregion
    }
}
