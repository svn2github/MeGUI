using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public interface IAudioSettingsProvider
    {
        AudioCodec CodecType
        {
            get;
        }

        AudioEncoderType EncoderType
        {
            get;
        }
        /// <summary>
        /// loads the default settings
        /// </summary>
        void LoadDefaults();
        /// <summary>
        /// checks if the type of the object given as an argument is the same as the one of the provider
        /// this check is used to see if a settings object can be loaded into the current provider
        /// </summary>
        /// <param name="settings">the settings object to be checked</param>
        /// <returns>true if the settings is of the same type, false if not</returns>
        bool IsSameType(AudioCodecSettings settings);
        /// <summary>
        /// returns the underlying settings object
        /// </summary>
        /// <returns></returns>
        AudioCodecSettings GetCurrentSettings();
        /// <summary>
        /// loads the settings into the provider
        /// </summary>
        /// <param name="settings"></param>
        void LoadSettings(AudioCodecSettings settings);
        /// <summary>
        /// allows to edit the settings object via GUI
        /// </summary>
        /// <param name="profileManager">the profile manager containing all profiles</param>
        /// <param name="path">the start path of the application</param>
        /// <param name="settings">the settings of the application</param>
        /// <param name="initialProfile">the profile that's currently selected</param>
        /// <param name="selectedProfile">the profile selected after the configuration dialog has been closed</param>
        /// <returns></returns>
        bool EditSettings(ProfileManager profileManager, string path, MeGUISettings settings, string initialProfile, string[] audioIO, out string selectedProfile);
    }
    public interface IVideoSettingsProvider
    {
        VideoCodec CodecType
        {
            get;
        }

        VideoEncoderType EncoderType
        {
            get;
        }
        /// <summary>
        /// loads the default settings
        /// </summary>
        void LoadDefaults();
        /// <summary>
        /// checks if the type of the object given as an argument is the same as the one of the provider
        /// this check is used to see if a settings object can be loaded into the current provider
        /// </summary>
        /// <param name="settings">the settings object to be checked</param>
        /// <returns>true if the settings is of the same type, false if not</returns>
        bool IsSameType(VideoCodecSettings settings);
        /// <summary>
        /// returns the underlying settings object
        /// </summary>
        /// <returns></returns>
        VideoCodecSettings GetCurrentSettings();
        /// <summary>
        /// loads the settings into the provider
        /// </summary>
        /// <param name="settings"></param>
        void LoadSettings(VideoCodecSettings settings);
        bool EditSettings(ProfileManager profileManager, MeGUISettings settings, string initialProfile, string[] videoIO, int[] creditsAndIntroFrames, out string selectedProfile);
    }

    /// <summary>
    /// Generic IVideoSettingsProvider implementation
    /// </summary>
    /// <typeparam name="TSettings">Video Codec settings</typeparam>
    /// <typeparam name="TConfigurationDialog">Dialog to configure Video Codec settings</typeparam>
    public class VideoSettingsProviderImpl<TSettings, TConfigurationDialog> : IVideoSettingsProvider
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
            (this as IVideoSettingsProvider).LoadDefaults();
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

        VideoCodec IVideoSettingsProvider.CodecType
        {
            get { return this.settings.Codec; }
        }

        void IVideoSettingsProvider.LoadDefaults()
        {
            this.settings = new TSettings();
        }

        bool IVideoSettingsProvider.IsSameType(VideoCodecSettings settings)
        {
            return settings is TSettings;
        }

        VideoCodecSettings IVideoSettingsProvider.GetCurrentSettings()
        {
            return this.settings;
        }

        void IVideoSettingsProvider.LoadSettings(VideoCodecSettings settings)
        {
            this.settings = (TSettings)settings;
        }

        public virtual string EncoderPath(MeGUISettings settings)
        {
            return settings.MencoderPath;
        }

        bool IVideoSettingsProvider.EditSettings(ProfileManager profileManager, MeGUISettings settings, string initialProfile, string[] videoIO, int[] creditsAndIntroFrames, out string selectedProfile)
        {
            selectedProfile = null;
            using (TConfigurationDialog scd = (TConfigurationDialog)System.Activator.CreateInstance(typeof(TConfigurationDialog),profileManager, initialProfile, settings.SafeProfileAlteration))
            {
                scd.Input = videoIO[0];
                scd.Output = videoIO[1];
                scd.EncoderPath = EncoderPath(settings);
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

    public class SnowSettingsProvider : VideoSettingsProviderImpl<snowSettings, snowConfigurationDialog>
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
    }
    public class X264SettingsProvider : VideoSettingsProviderImpl<x264Settings,x264ConfigurationDialog>
    {
        public X264SettingsProvider():base("x264", VideoEncoderType.X264)
        {
        }
        public override string EncoderPath(MeGUISettings settings)
        {
            return settings.X264Path;
        }
    }
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
    public class AudioSettingsProviderImpl<TSettings, TConfigurationDialog>:IAudioSettingsProvider
        where TSettings: AudioCodecSettings, new()
        where TConfigurationDialog: baseAudioConfigurationDialog
    {

        private TSettings settings;
        private string title;
        private AudioEncoderType encoderType;

        public AudioSettingsProviderImpl(string title, AudioEncoderType encoderType)
        {
            ((IAudioSettingsProvider)this).LoadDefaults();
            this.title = title;
            this.encoderType = encoderType;
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

    public class MP2SettingsProvider : AudioSettingsProviderImpl<MP2Settings, MP2ConfigurationDialog>
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

        public OggVorbisSettingsProvider():base("OggVorbis", AudioEncoderType.VORBIS)
        {
        }
    }

    public class FaacSettingsProvider : AudioSettingsProviderImpl<FaacSettings, FaacConfigurationDialog>
    {
        public FaacSettingsProvider():base("FAAC", AudioEncoderType.FAAC)
        {
        }
    }
    public class NeroAACSettingsProvider : AudioSettingsProviderImpl<NeroAACSettings, NeroAACConfigurationDialog>
    {
        public NeroAACSettingsProvider():base("ND AAC", AudioEncoderType.NAAC)
        {
        }
    }
    public class LameMP3SettingsProvider : AudioSettingsProviderImpl<MP3Settings,lameConfigurationDialog>
    {
        public LameMP3SettingsProvider():base("MP3", AudioEncoderType.LAME)
        {
        }
    }
}
