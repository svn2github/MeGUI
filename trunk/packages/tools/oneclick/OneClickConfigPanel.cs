using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;



using MeGUI.core.details.video;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

namespace MeGUI.packages.tools.oneclick
{
    public partial class OneClickConfigPanel : UserControl, MeGUI.core.plugins.interfaces.Gettable<OneClickSettings>
    {
        BitrateCalculator calc = new BitrateCalculator();
        private MainForm mainForm;
        #region profiles
        #region AVS profiles
        ISettingsProvider<AviSynthSettings, MeGUI.core.details.video.Empty, int, int> avsSettingsProvider = new SettingsProviderImpl2<
    MeGUI.core.gui.AviSynthProfileConfigPanel, MeGUI.core.details.video.Empty, AviSynthSettings, AviSynthSettings, int, int>("AviSynth", 0, 0);
        ProfilesControlHandler<AviSynthSettings, Empty> avsProfileHandler;
        private void initAvsHandler()
        {
            // Init AVS handlers
            avsProfileHandler = new ProfilesControlHandler<AviSynthSettings, Empty>(
    "AviSynth", mainForm, avsProfileControl, avsSettingsProvider.EditSettings, Empty.Getter,
    new SettingsGetter<AviSynthSettings>(avsSettingsProvider.GetCurrentSettings), new SettingsSetter<AviSynthSettings>(avsSettingsProvider.LoadSettings));
            SingleConfigurerHandler<AviSynthSettings, Empty, int, int> configurerHandler = new SingleConfigurerHandler<AviSynthSettings, Empty, int, int>(avsProfileHandler, avsSettingsProvider);
        }
        #endregion
        #region Video profiles
        MultipleConfigurersHandler<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> videoCodecHandler;
        ProfilesControlHandler<VideoCodecSettings, VideoInfo> videoProfileHandler;
        private void initVideoHandler()
        {
            this.videoCodec.Items.AddRange(mainForm.PackageSystem.VideoSettingsProviders.ValuesArray);
            try
            {
                this.videoCodec.SelectedItem = mainForm.PackageSystem.VideoSettingsProviders["x264"];
            }
            catch (Exception) { }
            // Init Video handlers
            videoCodecHandler =
                new MultipleConfigurersHandler<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>(videoCodec);
            videoProfileHandler =
                new ProfilesControlHandler<VideoCodecSettings, VideoInfo>("Video", mainForm, videoProfileControl,
                videoCodecHandler.EditSettings, new InfoGetter<VideoInfo>(delegate() { return new VideoInfo(); }),
                videoCodecHandler.Getter, videoCodecHandler.Setter);
            videoCodecHandler.Register(videoProfileHandler);
        }
        #endregion
        #region Audio profiles
        MultipleConfigurersHandler<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> audioCodecHandler;
        ProfilesControlHandler<AudioCodecSettings, string[]> audioProfileHandler;
        private void initAudioHandler()
        {
            this.audioCodec.Items.AddRange(mainForm.PackageSystem.AudioSettingsProviders.ValuesArray);
            try
            {
                this.audioCodec.SelectedItem = mainForm.PackageSystem.AudioSettingsProviders["ND AAC"];
            }
            catch (Exception) { }

            // Init audio handlers
            audioCodecHandler = new MultipleConfigurersHandler<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>(audioCodec);
            audioProfileHandler = new ProfilesControlHandler<AudioCodecSettings, string[]>("Audio", mainForm, audioProfileControl, audioCodecHandler.EditSettings,
                new InfoGetter<string[]>(delegate { return new string[] { "input", "output" }; }), audioCodecHandler.Getter, audioCodecHandler.Setter);
            audioCodecHandler.Register(audioProfileHandler);
        }
        #endregion
        #endregion
        
        public OneClickConfigPanel(MainForm mainForm, MeGUI.core.details.video.Empty dummy) 
        {
            this.mainForm = mainForm;
            InitializeComponent();

            // We do this because the designer will attempt to put such a long string in the resources otherwise
            containerFormatLabel.Text = "Since the possible output filetypes are not known until the input is configured, the output type cannot be configured in a profile. Instead, here is a list of known file-types. You choose which you are happy with, and MeGUI will attempt to encode to one of those on the list.";

            foreach (ContainerType t in mainForm.MuxProvider.GetSupportedContainers())
                containerTypeList.Items.Add(t.ToString());
            this.filesizeComboBox.Items.AddRange(calc.getPredefinedOutputSizes());
            this.filesizeComboBox.Items.Add("Custom");
            this.filesizeComboBox.Items.Add("Don't care");
            this.filesizeComboBox.SelectedIndex = 2;
            initAudioHandler();
            initAvsHandler();
            initVideoHandler();
        }

        #region Gettable<OneClickSettings> Members

        public OneClickSettings Settings
        {
            get
            {
                OneClickSettings val = new OneClickSettings();
                val.AudioProfileName = audioProfileHandler.SelectedProfile;
                val.AutomaticDeinterlacing = autoDeint.Checked;
                val.AvsProfileName = avsProfileHandler.SelectedProfile;
                val.ContainerCandidates = ContainerCandidates;
                val.DontEncodeAudio = dontEncodeAudio.Checked;
                val.Filesize = long.Parse(filesizeKB.Text);
                val.OutputResolution = (long)horizontalResolution.Value;
                val.PrerenderVideo = preprocessVideo.Checked;
                val.SignalAR = signalAR.Checked;
                val.Split = splitOutput.Checked;
                if (val.Split) val.SplitSize = new FileSize(Unit.KB, long.Parse(splitSize.Text));
                val.StorageMediumName = filesizeComboBox.SelectedItem.ToString();
                val.VideoProfileName = videoProfileHandler.SelectedProfile;
                return val;
            }
            set
            {
                try { audioProfileHandler.SelectedProfile = value.AudioProfileName; } catch (ProfileCouldntBeSelectedException) { }
                autoDeint.Checked = value.AutomaticDeinterlacing;
                try { avsProfileHandler.SelectedProfile = value.AvsProfileName; } catch (ProfileCouldntBeSelectedException) { }
                ContainerCandidates = value.ContainerCandidates;
                dontEncodeAudio.Checked = value.DontEncodeAudio;
                filesizeKB.Text = value.Filesize.ToString();
                horizontalResolution.Value = value.OutputResolution;
                preprocessVideo.Checked = value.PrerenderVideo;
                signalAR.Checked = value.SignalAR;
                splitOutput.Checked = value.Split;
                splitSize.Text = value.SplitSize.ToString();
                try { filesizeComboBox.SelectedItem = value.StorageMediumName; } catch (Exception) { }
                try { videoProfileHandler.SelectedProfile = value.VideoProfileName; } catch (ProfileCouldntBeSelectedException) { }
            }
        }

        #endregion

        private void dontEncodeAudio_CheckedChanged(object sender, EventArgs e)
        {
            audioProfileControl.Enabled = !dontEncodeAudio.Checked;
            audioCodec.Enabled = !dontEncodeAudio.Checked;
        }

        private void splitOutput_CheckedChanged(object sender, EventArgs e)
        {
            splitSize.Enabled = splitOutput.Checked;
        }

        private string[] ContainerCandidates
        {
            get
            {
                string[] val = new string[containerTypeList.CheckedItems.Count];
                containerTypeList.CheckedItems.CopyTo(val, 0);
                return val;
            }
            set
            {
                for (int i = 0; i < containerTypeList.Items.Count; i++)
                    containerTypeList.SetItemChecked(i, false);

                foreach (string val in value)
                {
                    int index = containerTypeList.Items.IndexOf(val);
                    if (index > -1)
                        containerTypeList.SetItemChecked(index, true);
                }
            }
        }

        private void filesizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.filesizeKB.ReadOnly = true;
            filesizeKB.Text = calc.getOutputSizeKBs(filesizeComboBox.SelectedIndex).ToString();
            if (filesizeComboBox.SelectedIndex == 10) // Custom
                this.filesizeKB.ReadOnly = false;
            if (filesizeComboBox.SelectedIndex == 11) // Don't care
                this.filesizeKB.Text = "-1";
        }
    }
}
