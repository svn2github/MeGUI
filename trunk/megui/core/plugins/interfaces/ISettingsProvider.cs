using System;
using System.Collections.Generic;
using System.Text;

using MeGUI.core.plugins.interfaces;
using MeGUI.core.details.video;
using MeGUI.packages.video.x264;
using MeGUI.packages.video.lmp4;
using MeGUI.packages.video.snow;
using MeGUI.packages.video.xvid;
using MeGUI.packages.audio.naac;
using MeGUI.packages.audio.audx;
using MeGUI.packages.audio.faac;
using MeGUI.packages.audio.ffac3;
using MeGUI.packages.audio.ffmp2;
using MeGUI.packages.audio.lame;
using MeGUI.packages.audio.vorbis;
using MeGUI.packages.audio.waac;
using MeGUI.packages.audio.aften;
using System.Windows.Forms;
using MeGUI.core.util;

namespace MeGUI
{
    public interface ISettingsProvider<TSettings, TInfo, TCodec, TEncoder> : IIDable
        where TSettings : GenericSettings
    {
        /// <summary>
        /// Returns the codec (format, eg AVC, MP3, etc) type
        /// </summary>
        TCodec CodecType { get; }

        /// <summary>
        /// Returns the encoder (eg x264, LAME, etc) type
        /// </summary>
        TEncoder EncoderType { get; }

        /// <summary>
        /// Sets the settings back to default
        /// </summary>
        void LoadDefaults();

        /// <summary>
        /// Returns true if settings is the type that this settings provider supports
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        bool IsSameType(TSettings settings);

        /// <summary>
        /// Returns the current settings
        /// </summary>
        /// <returns></returns>
        TSettings GetCurrentSettings();

        /// <summary>
        /// Sets the Current Settings
        /// </summary>
        /// <param name="settings"></param>
        void LoadSettings(TSettings settings);

        /// <summary>
        /// Opens the configuration dialog for the current settings/profiles
        /// </summary>
        SettingsEditor<TSettings, TInfo> EditSettings { get; }
    }
    public delegate void StringChanged(object sender, string val);
    public delegate void IntChanged(object sender, int val);
    public class VideoInfo
    {
        private string videoInput;
        public event StringChanged VideoInputChanged;
        public string VideoInput
        {
            get { return videoInput; }
            set { videoInput = value; VideoInputChanged(this, value); }
        }

        private string videoOutput;
        public event StringChanged VideoOutputChanged;
        public string VideoOutput
        {
            get { return videoOutput; }
            set { videoOutput = value; VideoOutputChanged(this, value); }
        }

        private Dar? dar = null;

        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
        }
        
        private int introEndFrame;

        public int IntroEndFrame
        {
            get { return introEndFrame; }
            set { introEndFrame = value; }
        }
        private int creditsStartFrame;

        public int CreditsStartFrame
        {
            get { return creditsStartFrame; }
            set { creditsStartFrame = value; }
        }

        public VideoInfo(string videoInput, string videoOutput, int darX, int darY, int creditsStartFrame, int introEndFrame)
        {
            this.videoInput = videoInput;
            this.videoOutput = videoOutput;
            this.creditsStartFrame = creditsStartFrame;
            this.introEndFrame = introEndFrame;
        }

        public VideoInfo()
            : this("", "", -1, -1, -1, -1) { }
    }


    public class SettingsProviderImpl2<TPanel, TInfo, TSettings, TProfileSettings, TCodec, TEncoder>
        : ISettingsProvider<TProfileSettings, TInfo, TCodec, TEncoder>
        where TProfileSettings : GenericSettings
        where TSettings : TProfileSettings, new()
        where TPanel : System.Windows.Forms.Control, Gettable<TProfileSettings>
    {
        private string title;
        private TSettings settings;
        private TCodec codec;
        private TEncoder encoderType;

        public SettingsProviderImpl2(string title, TEncoder encoderType, TCodec codec)
        {
            this.title = title;
            this.encoderType = encoderType;
            this.codec = codec;
            LoadDefaults();
        }

        public string ID
        {
            get
            {
                return title;
            }
        }
        public override string ToString()
        {
            return this.title;
        }

        public TEncoder EncoderType
        {
            get { return encoderType; }
        }

        #region IVideoSettingsProvider Members

        public TCodec CodecType
        {
            get { return this.codec; }
        }

        public void LoadDefaults()
        {
            this.settings = new TSettings();
        }

        public bool IsSameType(TProfileSettings settings)
        {
            return settings is TSettings;
        }

        public TProfileSettings GetCurrentSettings()
        {
            return this.settings;
        }

        public void LoadSettings(TProfileSettings settings)
        {
            this.settings = (TSettings)settings;
        }

        public virtual string EncoderPath(MeGUISettings settings)
        {
            return settings.MencoderPath;
        }

        public SettingsEditor<TProfileSettings, TInfo> EditSettings
        {
            get { return EditorProvider<TPanel, TInfo, TSettings, TProfileSettings>.Create(); }
        }
        #endregion
    }

    public class EditorProvider<TPanel, TInfo, TSettings, TProfileSettings>
        where TProfileSettings : GenericSettings
        where TSettings : TProfileSettings, new()
        where TPanel : System.Windows.Forms.Control, Gettable<TProfileSettings>
    {
        public static SettingsEditor<TProfileSettings, TInfo> Create()
        {
            return new SettingsEditor<TProfileSettings, TInfo>(
                delegate(MainForm mainForm, ref TProfileSettings settings,
                            ref string profileName, TInfo info)
                {
                    TPanel t = (TPanel)System.Activator.CreateInstance(typeof(TPanel), mainForm, info);
                    using (ConfigurationWindow<TSettings, TProfileSettings> scd = 
                        new ConfigurationWindow<TSettings, TProfileSettings>(mainForm.Profiles, t, t, profileName))
                    {
                        scd.Settings = (TSettings)settings; // Set the settings in case there is no profile configured
                        if (scd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            /* Get the selected profile and see if its settings match the current 
                             * configuration of the window. If they do, continue as normal. If not,
                             * ask the user whether he/she wants to overwrite the profile's settings
                             * with the currently configured ones. If the user answers no, then the
                             * settings are returned as an unnamed profile; this is a special case,
                             * and generates a temporary group of settings. */
                            Profile prof = scd.CurrentProfile;
                            if (prof != null && !scd.Settings.Equals(prof.BaseSettings))
                            {
                                if (MessageBox.Show("Profile has been changed. Update the selected profile?",
                                    "Profile update", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    == DialogResult.Yes)
                                    prof.BaseSettings = scd.Settings;
                                else
                                    prof = null;
                            }
                            if (prof == null)
                                profileName = "";
                            else
                                profileName = prof.Name;

                            settings = scd.Settings;
                            return true;
                        }
                        else
                            return false;
                    }
                });
        }
    }

    #region providers
    public class X264SettingsProvider : SettingsProviderImpl2<x264ConfigurationPanel,
        VideoInfo, x264Settings, VideoCodecSettings, VideoCodec, VideoEncoderType>
    {
        public X264SettingsProvider()
            : base("x264", VideoEncoderType.X264, VideoCodec.AVC)
        {
        }
        public override string EncoderPath(MeGUISettings settings)
        {
            return settings.X264Path;
        }
    }
    public class XviDSettingsProvider : SettingsProviderImpl2<xvidConfigurationPanel,
   VideoInfo, xvidSettings, VideoCodecSettings, VideoCodec, VideoEncoderType>
    {
        public XviDSettingsProvider()
            : base("Xvid", VideoEncoderType.XVID, VideoCodec.ASP)
        {
        }
        public override string EncoderPath(MeGUISettings settings)
        {
            return settings.XviDEncrawPath;
        }
    }
    public class LavcSettingsProvider : SettingsProviderImpl2<lavcConfigurationPanel,
   VideoInfo, lavcSettings, VideoCodecSettings, VideoCodec, VideoEncoderType>
    {
        public LavcSettingsProvider()
            : base("LMP4", VideoEncoderType.LMP4, VideoCodec.ASP)
        {
        }
    }
    public class SnowSettingsProvider : SettingsProviderImpl2<snowConfigurationPanel,
   VideoInfo, snowSettings, VideoCodecSettings, VideoCodec, VideoEncoderType>
    {
        public SnowSettingsProvider()
            : base("Snow", VideoEncoderType.SNOW, VideoCodec.SNOW)
        {
        }
    }
    public class NeroAACSettingsProvider : SettingsProviderImpl2<neroConfigurationPanel, string[],
    NeroAACSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public NeroAACSettingsProvider()
            : base("AAC  -  Nero Digital", AudioEncoderType.NAAC, AudioCodec.AAC)
        {
        }
    }
    public class AudXSettingsProvider : SettingsProviderImpl2<AudXConfigurationPanel, string[],
        AudXSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public AudXSettingsProvider()
            : base("MP3  -  Aud-X", AudioEncoderType.AUDX, AudioCodec.MP3)
        { }
    }
    public class faacSettingsProvider : SettingsProviderImpl2<faacConfigurationPanel, string[],
        FaacSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public faacSettingsProvider()
            : base("AAC  -  FAAC", AudioEncoderType.FAAC, AudioCodec.AAC)
        { }
    }
    public class ffac3SettingsProvider : SettingsProviderImpl2<AC3ConfigurationPanel, string[],
   AC3Settings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public ffac3SettingsProvider()
            : base("AC3  -  FFmpeg", AudioEncoderType.FFAC3, AudioCodec.AC3)
        { }
    }
    public class ffmp2SettingsProvider : SettingsProviderImpl2<MP2ConfigurationPanel, string[],
   MP2Settings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public ffmp2SettingsProvider()
            : base("MP2  -  FFmpeg", AudioEncoderType.FFMP2, AudioCodec.MP2)
        { }
    }
    public class lameSettingsProvider : SettingsProviderImpl2<lameConfigurationPanel, string[],
   MP3Settings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public lameSettingsProvider()
            : base("MP3  -  Lame", AudioEncoderType.LAME, AudioCodec.MP3)
        { }
    }
    public class vorbisSettingsProvider : SettingsProviderImpl2<OggVorbisConfigurationPanel, string[],
   OggVorbisSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public vorbisSettingsProvider()
            : base("OGG  -  Xiph", AudioEncoderType.VORBIS, AudioCodec.VORBIS)
        { }
    }
    public class waacSettingsProvider : SettingsProviderImpl2<WinAmpAACConfigurationPanel, string[],
   WinAmpAACSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public waacSettingsProvider()
            : base("AAC  -  Winamp", AudioEncoderType.WAAC, AudioCodec.AAC)
        { }
    }
    public class aftenSettingsProvider : SettingsProviderImpl2<AftenConfigurationPanel, string[],
   AftenSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public aftenSettingsProvider()
            : base("AC3  -  Aften", AudioEncoderType.AFTEN, AudioCodec.AC3)
        { }
    }
    #endregion
}
