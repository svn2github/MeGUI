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
using System.Windows.Forms;

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

    /*    public interface IAudioSettingsProvider : ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> {}
        public interface IVideoSettingsProvider : ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> { }*/

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
        private int darX;

        public int DARX
        {
            get { return darX; }
            set { darX = value; }
        }
        private int darY;

        public int DARY
        {
            get { return darY; }
            set { darY = value; }
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
            this.darX = darX;
            this.darY = darY;
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

    #region old
    /*
    /// <summary>
    /// Generic IVideoSettingsProvider implementation
    /// </summary>
    /// <typeparam name="TSettings">Video Codec settings</typeparam>
    /// <typeparam name="TConfigurationDialog">Dialog to configure Video Codec settings</typeparam>
    public class VideoSettingsProviderImpl<TSettings, TConfigurationDialog> : ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>
        where TSettings : VideoCodecSettings, new()
        where TConfigurationDialog : VideoConfigurationDialog
    {
        private string title;
        private TSettings settings;
        private VideoEncoderType encoderType;
        public VideoSettingsProviderImpl(string title, VideoEncoderType encoderType)
        {
            this.title = title;
            this.encoderType = encoderType;
            (this as ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>).LoadDefaults();
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

        public VideoEncoderType EncoderType
        {
            get { return encoderType; }
        }

        #region IVideoSettingsProvider Members

        VideoCodec ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>.CodecType
        {
            get { return this.settings.Codec; }
        }

        void ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>.LoadDefaults()
        {
            this.settings = new TSettings();
        }

        bool ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>.IsSameType(VideoCodecSettings settings)
        {
            return settings is TSettings;
        }

        VideoCodecSettings ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>.GetCurrentSettings()
        {
            return this.settings;
        }

        void ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>.LoadSettings(VideoCodecSettings settings)
        {
            this.settings = (TSettings)settings;
        }

        public virtual string EncoderPath(MeGUISettings settings)
        {
            return settings.MencoderPath;
        }

        bool ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>.EditSettings(MainForm mainForm, string initialProfile, string[] videoIO, int[] creditsAndIntroFrames, out string selectedProfile)
        {
            selectedProfile = null;
            using (TConfigurationDialog scd = (TConfigurationDialog)System.Activator.CreateInstance( typeof(TConfigurationDialog),mainForm, initialProfile, mainForm.Settings.SafeProfileAlteration))
            {
                scd.Input = videoIO[0];
                scd.Output = videoIO[1];
                scd.EncoderPath = EncoderPath(mainForm.Settings);
                scd.Settings = this.settings;
                scd.IntroEndFrame = creditsAndIntroFrames[0];
                scd.CreditsStartFrame = creditsAndIntroFrames[1];
                if (scd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (TSettings)scd.Settings;
                    selectedProfile = scd.CurrentProfile;
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion
    }

*/
    #endregion
    /*    public class SnowSettingsProvider : SettingsProviderImpl<snowSettings, snowConfigurationDialog>
    {
        public SnowSettingsProvider():base("Snow", VideoEncoderType.SNOW)
        {
        }
    }
    public class LavcSettingsProvider : VideoSettingsProviderImpl<lavcSettings, lavcConfigurationDialog>
    {
        public LavcSettingsProvider(): base("LMP4", VideoEncoderType.LMP4)
        {
        }
    }*/
    #region video providers
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
            : base("XviD", VideoEncoderType.XVID, VideoCodec.ASP)
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
            : base("ND AAC", AudioEncoderType.NAAC, AudioCodec.AAC)
        {
        }
    }
    public class AudXSettingsProvider : SettingsProviderImpl2<AudXConfigurationPanel, string[],
        AudXSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public AudXSettingsProvider()
            : base("Aud-X MP3", AudioEncoderType.AUDX, AudioCodec.MP3)
        { }
    }
    public class faacSettingsProvider : SettingsProviderImpl2<faacConfigurationPanel, string[],
        FaacSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public faacSettingsProvider()
            : base("FAAC", AudioEncoderType.FAAC, AudioCodec.AAC)
        { }
    }
    public class ffac3SettingsProvider : SettingsProviderImpl2<AC3ConfigurationPanel, string[],
   AC3Settings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public ffac3SettingsProvider()
            : base("FFMPEG AC-3", AudioEncoderType.FFAC3, AudioCodec.AAC)
        { }
    }
    public class ffmp2SettingsProvider : SettingsProviderImpl2<MP2ConfigurationPanel, string[],
   MP2Settings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public ffmp2SettingsProvider()
            : base("FFMPEG MP2", AudioEncoderType.FFMP2, AudioCodec.MP2)
        { }
    }
    public class lameSettingsProvider : SettingsProviderImpl2<lameConfigurationPanel, string[],
   MP3Settings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public lameSettingsProvider()
            : base("LAME MP3", AudioEncoderType.LAME, AudioCodec.MP3)
        { }
    }
    public class vorbisSettingsProvider : SettingsProviderImpl2<OggVorbisConfigurationPanel, string[],
   OggVorbisSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public vorbisSettingsProvider()
            : base("Ogg Vorbis", AudioEncoderType.VORBIS, AudioCodec.VORBIS)
        { }
    }
    public class waacSettingsProvider : SettingsProviderImpl2<WinAmpAACConfigurationPanel, string[],
   WinAmpAACSettings, AudioCodecSettings, AudioCodec, AudioEncoderType>
    {
        public waacSettingsProvider()
            : base("WinAmp AAC", AudioEncoderType.WAAC, AudioCodec.AAC)
        { }
    }

    #endregion
    #region old
    /*
    public class XviDSettingsProvider : VideoSettingsProviderImpl<xvidSettings, xvidConfigurationDialog>
    {
        public XviDSettingsProvider():base("XviD", VideoEncoderType.XVID)
        {
        }
        public override string EncoderPath(MeGUISettings settings)
        {
            return settings.XviDEncrawPath;
        }
    }

    /// <summary>
    /// Generic IAudioSettingsProvider implementation
    /// </summary>
    /// <typeparam name="TSettings">Audio Codec settings</typeparam>
    /// <typeparam name="TConfigurationDialog">Dialog to configure Audio Codec settings</typeparam>
    public class AudioSettingsProviderImpl<TSettings, TConfigurationDialog>:ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>
        where TSettings: AudioCodecSettings, new()
        where TConfigurationDialog: baseAudioConfigurationDialog
    {

        private TSettings settings;
        private string title;
        private AudioEncoderType encoderType;

        public AudioSettingsProviderImpl(string title, AudioEncoderType encoderType)
        {
            ((ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>)this).LoadDefaults();
            this.title = title;
            this.encoderType = encoderType;
        }
        public string ID
        {
            get { return title; }
        }
        public override string ToString()
        {
            return this.title;
        }

        #region IAudioSettingsProvider Members

        public AudioCodec CodecType
        {
            get { return this.settings.Codec; }
        }

        public AudioEncoderType EncoderType
        {
            get { return this.encoderType; }
        }

        public void LoadDefaults()
        {
            this.settings = new TSettings();
        }

        public bool IsSameType(AudioCodecSettings settings)
        {
            return settings is TSettings;
        }

        public AudioCodecSettings GetCurrentSettings()
        {
            return this.settings;
        }

        public void LoadSettings(AudioCodecSettings settings)
        {
            this.settings = (TSettings)settings;
        }

        public bool EditSettings(ProfileManager profileManager, string path, MeGUISettings settings, string initialProfile, string[] audioIO, out string selectedProfile)
        {
            selectedProfile = null;
            using (TConfigurationDialog fcc = (TConfigurationDialog)System.Activator.CreateInstance(typeof(TConfigurationDialog), profileManager, path, settings, initialProfile))
            {
                fcc.Input = audioIO[0];
                fcc.Output = audioIO[1];
                fcc.Settings = this.settings;
                if (fcc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (TSettings)fcc.Settings;
                    selectedProfile = fcc.CurrentProfile;
                    return true;
                }
                return false;
            }
        }

        #endregion
    }
    */
    #endregion

    /*    public class MP2SettingsProvider : AudioSettingsProviderImpl<MP2Settings, MP2ConfigurationDialog>
    {
        public MP2SettingsProvider():base("MP2", AudioEncoderType.FFMP2)
        {
        }
    }
    public class AC3SettingsProvider : AudioSettingsProviderImpl<AC3Settings, AC3ConfigurationDialog>
    {

        public AC3SettingsProvider():base("AC3", AudioEncoderType.FFAC3)
        {
        }
    }
    public class WinAmpAACSettingsProvider : AudioSettingsProviderImpl<WinAmpAACSettings, WinAmpAACConfigurationDialog>
    {

        public WinAmpAACSettingsProvider():base("CT AAC", AudioEncoderType.WAAC)
        {
        }
    }
    public class AudXSettingsProvider : AudioSettingsProviderImpl<AudXSettings, AudXConfigurationDialog>
    {

        public AudXSettingsProvider(): base("AudX 5.1", AudioEncoderType.AUDX)
        {
        }
    }
    public class OggVorbisSettingsProvider : AudioSettingsProviderImpl<OggVorbisSettings, OggVorbisConfigurationDialog>
    {

        public OggVorbisSettingsProvider():base("Vorbis", AudioEncoderType.VORBIS)
        {
        }
    }
        public class LameMP3SettingsProvider : AudioSettingsProviderImpl<MP3Settings,lameConfigurationDialog>
    {
        public LameMP3SettingsProvider():base("MP3", AudioEncoderType.LAME)
        {
        }
    }
    public class FaacSettingsProvider : AudioSettingsProviderImpl<FaacSettings, FaacConfigurationDialog>
    {
        public FaacSettingsProvider():base("FAAC", AudioEncoderType.FAAC)
        {
        }
    }*/


}
