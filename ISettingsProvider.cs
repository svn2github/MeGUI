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
    public class SnowSettingsProvider : Object, IVideoSettingsProvider
    {
        snowSettings settings;
        public SnowSettingsProvider()
        {
            this.LoadDefaults();
        }
        public override string ToString()
        {
            return "Snow";
        }

        #region IVideoSettingsProvider Members

        public void LoadDefaults()
        {
            this.settings = new snowSettings();
        }

        public bool IsSameType(VideoCodecSettings settings)
        {
            if (settings is snowSettings)
                return true;
            else
                return false;
        }

        public VideoCodecSettings GetCurrentSettings()
        {
            return settings;
        }

        public void LoadSettings(VideoCodecSettings settings)
        {
            this.settings = (snowSettings)settings;
        }

        public bool EditSettings(ProfileManager profileManager, MeGUISettings settings, string initialProfile, string[] videoIO, int[] creditsAndIntroFrames, out string selectedProfile)
        {
            selectedProfile = null;
            using (snowConfigurationDialog scd = new snowConfigurationDialog(profileManager, initialProfile, settings.SafeProfileAlteration))
            {
                scd.Input = videoIO[0];
                scd.Output = videoIO[1];
                scd.EncoderPath = settings.MencoderPath;
                scd.Settings = this.settings;
                scd.IntroEndFrame = creditsAndIntroFrames[0];
                scd.CreditsStartFrame = creditsAndIntroFrames[1];
                if (scd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (snowSettings)scd.Settings;
                    selectedProfile = scd.CurrentProfile;
                    return true;
                }
                else
                    return false;
            }
        }

        public VideoCodec CodecType
        {
            get { return VideoCodec.SNOW; }
        }

        #endregion
    }
    public class LavcSettingsProvider : Object, IVideoSettingsProvider
    {
        lavcSettings settings;
        public LavcSettingsProvider()
        {
            this.LoadDefaults();
        }
        public override string ToString()
        {
            return "LMP4";
        }

        #region IVideoSettingsProvider Members

        public VideoCodec CodecType
        {
            get { return VideoCodec.LMP4; }
        }

        public void LoadDefaults()
        {
            this.settings = new lavcSettings();
        }

        public bool IsSameType(VideoCodecSettings settings)
        {
            if (settings is lavcSettings)
                return true;
            else
                return false;
        }

        public VideoCodecSettings GetCurrentSettings()
        {
            return settings;
        }

        public void LoadSettings(VideoCodecSettings settings)
        {
            this.settings = (lavcSettings)settings;
        }

        public bool EditSettings(ProfileManager profileManager, MeGUISettings settings, string initialProfile, string[] videoIO, int[] creditsAndIntroFrames, out string selectedProfile)
        {
            selectedProfile = null;
            using (lavcConfigurationDialog lcd = new lavcConfigurationDialog(profileManager, initialProfile, settings.SafeProfileAlteration))
            {
                lcd.Input = videoIO[0];
                lcd.Output = videoIO[1];
                lcd.EncoderPath = settings.MencoderPath;
                lcd.Settings = this.settings;
                lcd.IntroEndFrame = creditsAndIntroFrames[0];
                lcd.CreditsStartFrame = creditsAndIntroFrames[1];
                if (lcd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (lavcSettings)lcd.Settings;
                    selectedProfile = lcd.CurrentProfile;
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion
    }
    public class X264SettingsProvider : Object, IVideoSettingsProvider
    {
        x264Settings settings;
        public X264SettingsProvider()
        {
            this.LoadDefaults();
        }
        public override string ToString()
        {
            return "x264";
        }

        #region IVideoSettingsProvider Members

        public VideoCodec CodecType
        {
            get { return VideoCodec.X264; }
        }

        public void LoadDefaults()
        {
            this.settings = new x264Settings();
        }

        public bool IsSameType(VideoCodecSettings settings)
        {
            if (settings is x264Settings)
                return true;
            else
                return false;
        }

        public VideoCodecSettings GetCurrentSettings()
        {
            return settings;
        }

        public void LoadSettings(VideoCodecSettings settings)
        {
            this.settings = (x264Settings)settings;
        }

        public bool EditSettings(ProfileManager profileManager, MeGUISettings settings, string initialProfile, string[] videoIO, int[] creditsAndIntroFrames, out string selectedProfile)
        {
            selectedProfile = null;
            int hres, vres, nFrames, darX, darY;
            double framerate;
            if (videoIO[0] == "")
                hres = vres = 0;
            else
                JobUtil.GetAllInputProperties(out nFrames, out framerate, out hres, out vres, out darX, out darY, videoIO[0]);
            AVCLevels al = new AVCLevels();
            double BPF = al.bytesPerFrame(hres, vres);
            using (x264ConfigurationDialog xcd = new x264ConfigurationDialog(profileManager, initialProfile, settings.SafeProfileAlteration))
            {
                xcd.BytesPerFrame = BPF;
                xcd.Input = videoIO[0];
                xcd.Output = videoIO[1];
                xcd.EncoderPath = settings.X264Path;
                xcd.Settings = this.settings;
                xcd.IntroEndFrame = creditsAndIntroFrames[0];
                xcd.CreditsStartFrame = creditsAndIntroFrames[1];
                xcd.AdvancedToolTips = settings.UseAdvancedTooltips;
                if (xcd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (x264Settings)xcd.Settings;
                    selectedProfile = xcd.CurrentProfile;
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion
    }
    public class XviDSettingsProvider : Object, IVideoSettingsProvider
    {
        xvidSettings settings;
        public XviDSettingsProvider()
        {
            this.LoadDefaults();
        }
        public override string ToString()
        {
            return "XviD";
        }

        #region IVideoSettingsProvider Members

        public VideoCodec CodecType
        {
            get { return VideoCodec.XVID; }
        }

        public void LoadDefaults()
        {
            this.settings = new xvidSettings();
        }

        public bool IsSameType(VideoCodecSettings settings)
        {
            if (settings is xvidSettings)
                return true;
            else
                return false;
        }

        public VideoCodecSettings GetCurrentSettings()
        {
            return settings;
        }

        public void LoadSettings(VideoCodecSettings settings)
        {
            this.settings = (xvidSettings)settings;
        }

        public bool EditSettings(ProfileManager profileManager, MeGUISettings settings, string initialProfile, string[] videoIO, int[] creditsAndIntroFrames, out string selectedProfile)
        {
            selectedProfile = null;
            using (xvidConfigurationDialog xcd = new xvidConfigurationDialog(profileManager, initialProfile, settings.SafeProfileAlteration))
            {
                xcd.Input = videoIO[0];
                xcd.Output = videoIO[1];
                xcd.EncoderPath = settings.XviDEncrawPath;
                xcd.Settings = this.settings;
                xcd.IntroEndFrame = creditsAndIntroFrames[0];
                xcd.CreditsStartFrame = creditsAndIntroFrames[1];
                if (xcd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (xvidSettings)xcd.Settings;
                    selectedProfile = xcd.CurrentProfile;
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion
    }
    public class FaacSettingsProvider : Object, IAudioSettingsProvider
    {
        private FaacSettings settings;
        public FaacSettingsProvider()
        {
            LoadDefaults();
        }
        public override string ToString()
        {
            return "FAAC";
        }

        #region IAudioSettingsProvider Members
        public AudioCodec CodecType
        {
            get { return AudioCodec.AAC; }
        }


        public void LoadDefaults()
        {
            this.settings = new FaacSettings();
        }

        public bool IsSameType(AudioCodecSettings settings)
        {
            if (settings is FaacSettings)
                return true;
            else
                return false;
        }

        public AudioCodecSettings GetCurrentSettings()
        {
            return settings;
        }

        public void LoadSettings(AudioCodecSettings settings)
        {
            this.settings = (FaacSettings)settings;
        }

        public bool EditSettings(ProfileManager profileManager, string path, MeGUISettings settings, string initialProfile, string[] audioIO, 
            out string selectedProfile)
        {
            selectedProfile = null;
            using (faacConfigurationDialog fcc = new faacConfigurationDialog(profileManager, path, settings, initialProfile))
            {
                fcc.Input = audioIO[0];
                fcc.Output = audioIO[1];
                fcc.Settings = this.settings;
                if (fcc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (FaacSettings)fcc.Settings;
                    selectedProfile = fcc.CurrentProfile;
                    return true;
                }
                return false;
            }
        }

        #endregion
    }
    public class NeroAACSettingsProvider : Object, IAudioSettingsProvider
    {
        private NeroAACSettings settings;
        public NeroAACSettingsProvider()
        {
            LoadDefaults();
        }
        public override string ToString()
        {
            return "NAAC";
        }

        #region IAudioSettingsProvider Members

        public AudioCodec CodecType
        {
            get { return AudioCodec.AAC; }
        }

        public void LoadDefaults()
        {
            this.settings = new NeroAACSettings();
        }

        public bool IsSameType(AudioCodecSettings settings)
        {
            if (settings is NeroAACSettings)
                return true;
            else
                return false;
        }

        public AudioCodecSettings GetCurrentSettings()
        {
            return settings;
        }

        public void LoadSettings(AudioCodecSettings settings)
        {
            this.settings = (NeroAACSettings)settings;
        }

        public bool EditSettings(ProfileManager profileManager, string path, MeGUISettings settings, string initialProfile, string[] audioIO,
            out string selectedProfile)
        {
            selectedProfile = null;
            using (neroConfigurationDialog ncd = new neroConfigurationDialog(profileManager, path,
                    settings, initialProfile))
            {
                ncd.Input = audioIO[0];
                ncd.Output = audioIO[1];
                ncd.Settings = this.settings;
                if (ncd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (NeroAACSettings)ncd.Settings;
                    selectedProfile = ncd.CurrentProfile;
                    return true;
                }
                return false;
            }
        }

        #endregion
    }
    public class LameMP3SettingsProvider : Object, IAudioSettingsProvider
    {
        MP3Settings settings;

        public LameMP3SettingsProvider()
        {
            LoadDefaults();
        }

        public override string ToString()
        {
            return "MP3";
        }

        #region IAudioSettingsProvider Members

        public AudioCodec CodecType
        {
            get { return AudioCodec.MP3; }
        }

        public void LoadDefaults()
        {
            this.settings = new MP3Settings();
        }

        public bool IsSameType(AudioCodecSettings settings)
        {
            if (settings is MP3Settings)
                return true;
            else
                return false;
        }

        public AudioCodecSettings GetCurrentSettings()
        {
            return settings;
        }

        public void LoadSettings(AudioCodecSettings settings)
        {
            this.settings = (MP3Settings)settings;
        }

        public bool EditSettings(ProfileManager profileManager, string path, MeGUISettings settings, string initialProfile, string[] audioIO,
            out string selectedProfile)
        {
            selectedProfile = null;
            using (lameConfigurationDialog lcd = new lameConfigurationDialog(profileManager, path,
                    settings, initialProfile))
            {
                lcd.Input = audioIO[0];
                lcd.Output = audioIO[1];
                lcd.Settings = this.settings;
                if (lcd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.settings = (MP3Settings)lcd.Settings;
                    selectedProfile = lcd.CurrentProfile;
                    return true;
                }
                return false;
            }
        }

        #endregion
    }
}
