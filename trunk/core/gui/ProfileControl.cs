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
    public partial class ProfileControl : UserControl
    {
        public string LabelText
        {
            get { return avsProfileLabel.Text; }
            set { avsProfileLabel.Text = value; }
        }

        public ProfileControl()
        {
            InitializeComponent();
        }
        public event EventHandler ConfigClick;
        public event EventHandler ProfileIndexChanged;

        private void avsConfigButton_Click(object sender, EventArgs e)
        {
            if (ConfigClick != null) ConfigClick(sender, e);
        }

        private void avsProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProfileIndexChanged != null) ProfileIndexChanged(sender, e);
        }


    }
    #region helpers
    public struct Empty
    {
        public static readonly InfoGetter<Empty> Getter = delegate { return new Empty(); };
    }

    public class Getter<TSettings> where TSettings : GenericSettings
    {
        public static SettingsGetter<TSettings> FromGettable(Gettable<TSettings> input)
        {
            return new SettingsGetter<TSettings>(delegate { return input.Settings; });
        }
    }

    public class DefaultGetter<TSettings> where TSettings : GenericSettings, new()
    {
        public static SettingsGetter<TSettings> Create()
        {
            return new SettingsGetter<TSettings>(delegate { return new TSettings(); });
        }
    }
    public class DefaultSetter<TSettings> where TSettings : GenericSettings
    {
        public static SettingsSetter<TSettings> Create()
        {
            return new SettingsSetter<TSettings>(delegate(TSettings p) { /*Do nothing */ });
        }
    }
    #endregion
    #region delegates
    public delegate void SelectedProfileChangedEvent(object sender, Profile prof);
    
    public delegate TSettings SettingsGetter<TSettings>() where TSettings : GenericSettings;
    public delegate void SettingsSetter<TSettings>(TSettings _) where TSettings : GenericSettings;
    
    public delegate bool SettingsEditor<TSettings, TInfo>(
        MainForm mainForm, ref TSettings settings, ref string profile, TInfo extra)
        where TSettings : GenericSettings;
    
    public delegate TInfo InfoGetter<TInfo>();
    #endregion

    public class ProfilesControlHandler<TSettings, TInfo>
        where TSettings : GenericSettings
    {
        private string profileType;
        private MainForm mainForm;
        private ProfileControl impl;


        private SettingsGetter<TSettings> GetCurrentSettings;
        private SettingsSetter<TSettings> SetCurrentSettings;
        private SettingsEditor<TSettings, TInfo> EditSettings;
        private InfoGetter<TInfo> GetInfo;
        public event EventHandler ConfigureCompleted;
        public event SelectedProfileChangedEvent ProfileChanged;

        public string SelectedProfile
        {
            get { try { return impl.avsProfile.SelectedItem.ToString(); } catch (Exception) { return null; } }
            set
            {
                if (string.IsNullOrEmpty(value)) impl.avsProfile.SelectedIndex = -1;
                impl.avsProfile.SelectedItem = value;
            }
        }

        public Profile[] Profiles
        {
            get
            {
                Profile[] val = new Profile[mainForm.Profiles.Profiles(profileType).Values.Count];
                mainForm.Profiles.Profiles(profileType).Values.CopyTo(val, 0);
                return val;
            }
        }

        public void RefreshProfiles()
        {
            impl.avsProfile.Items.Clear();
            foreach (string name in mainForm.Profiles.Profiles(profileType).Keys)
            {
                impl.avsProfile.Items.Add(name);
            }
            try { SelectedProfile = mainForm.Profiles.GetSelectedProfile(profileType).Name; }
            catch (NullReferenceException) { }
        }

        private void avsConfigButton_Click(object sender, System.EventArgs e)
        {
            
            TInfo info = GetInfo();
            TSettings settings = GetCurrentSettings();
            string profile = impl.avsProfile.Text;
            if (EditSettings(mainForm, ref settings, ref profile, info))
            {
                RefreshProfiles();
                int index = impl.avsProfile.Items.IndexOf(profile);
                impl.avsProfile.SelectedIndex = index;
                if (index == -1)
                    SetCurrentSettings(settings);
            }
            if (ConfigureCompleted != null) ConfigureCompleted(this, null);
        }

        private void avsProfile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            mainForm.Profiles.SetSelectedProfile(profileType, SelectedProfile);
            if (ProfileChanged != null)
            {
                if (impl.avsProfile.SelectedIndex >= 0)
                {
                    Profile prof = this.mainForm.Profiles.Profiles(profileType)[impl.avsProfile.SelectedItem.ToString()];
                    ProfileChanged(this, prof);
                }
                else
                    ProfileChanged(this, null);
            }
        }

        public ProfilesControlHandler(string profileType, MainForm mainForm, ProfileControl p,
            SettingsEditor<TSettings, TInfo> editor, 
            InfoGetter<TInfo> g2, 
            SettingsGetter<TSettings> getter, 
            SettingsSetter<TSettings> setter)
        {
            this.profileType = profileType;
            this.SetCurrentSettings = setter;
            this.GetCurrentSettings = getter;
            this.EditSettings = editor;
            this.GetInfo = g2;
            this.mainForm = mainForm; 
            p.ProfileIndexChanged += new EventHandler(avsProfile_SelectedIndexChanged);
            p.ConfigClick += new EventHandler(avsConfigButton_Click);
            impl = p;
            RefreshProfiles();
        }
    }

    public class SingleConfigurerHandler<TProfileSettings, TInfo, TCodec, TEncoder>
        where TProfileSettings : GenericSettings
    {
        ISettingsProvider<TProfileSettings, TInfo, TCodec, TEncoder> settingsProvider;
        public SingleConfigurerHandler(ProfilesControlHandler<TProfileSettings, TInfo> pHandler,
            ISettingsProvider<TProfileSettings, TInfo, TCodec, TEncoder> sP)
        {
            settingsProvider = sP;
            pHandler.ProfileChanged += new SelectedProfileChangedEvent(profileChanged);
            pHandler.RefreshProfiles();
        }

        private void profileChanged(object sender, Profile prof)
        {
            if (prof == null) return;
            settingsProvider.LoadSettings((TProfileSettings)prof.BaseSettings);
            if (ProfileChanged != null) ProfileChanged(this, prof);
        }
        public event SelectedProfileChangedEvent ProfileChanged;


    }
    
    public class MultipleConfigurersHandler<TProfileSettings, TInfo, TCodec, TEncoder>
        where TProfileSettings : GenericSettings
    {
        ProfilesControlHandler<TProfileSettings, TInfo> pHandler;
        public MultipleConfigurersHandler(ComboBox impl)
        {
            this.impl = impl;
            impl.SelectedIndexChanged += new EventHandler(impl_SelectedIndexChanged);
        }

        void impl_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Profile p in pHandler.Profiles)
            {
                if (CurrentSettingsProvider.IsSameType((TProfileSettings)p.BaseSettings))
                {
                    pHandler.SelectedProfile = p.Name;
                    return;
                }
            }
            pHandler.SelectedProfile = null;
        }

        public void Register(ProfilesControlHandler<TProfileSettings, TInfo> pHandler)
        {
            this.pHandler = pHandler;
            pHandler.ProfileChanged += new SelectedProfileChangedEvent(ProfileChanged);
            pHandler.RefreshProfiles();
        }

        private void ProfileChanged(object sender, Profile prof)
        {
            if (prof == null) return;
            foreach (ISettingsProvider<TProfileSettings, TInfo, TCodec, TEncoder> p in impl.Items)
            {
                if (p.IsSameType((TProfileSettings)prof.BaseSettings))
                {
                    p.LoadSettings((TProfileSettings)prof.BaseSettings);
                    impl.SelectedItem = p;
                    break;
                }
            }
        }

        public ISettingsProvider<TProfileSettings, TInfo, TCodec, TEncoder> CurrentSettingsProvider
        {
            get
            {
                return (ISettingsProvider<TProfileSettings, TInfo, TCodec, TEncoder>)impl.SelectedItem;
            }
            set
            {
                impl.SelectedItem = value;
            }
        }

        private ComboBox impl;
        public SettingsGetter<TProfileSettings> Getter
        {
            get 
            { 
                return new SettingsGetter<TProfileSettings>(delegate {
                    return CurrentSettingsProvider.GetCurrentSettings();
                });
            }
        }

        public SettingsSetter<TProfileSettings> Setter
        {
            get
            {
                return new SettingsSetter<TProfileSettings>(delegate(TProfileSettings settings)
                {
                    CurrentSettingsProvider.LoadSettings(settings);
                });
            }
        }

        public SettingsEditor<TProfileSettings, TInfo> EditSettings
        {
            get
            {
                return new SettingsEditor<TProfileSettings, TInfo>(
              delegate(MainForm a, ref TProfileSettings b, ref string c, TInfo d)
              {
                  return CurrentSettingsProvider.EditSettings(a, ref b, ref c, d);
              });
            }
        }
    }


    public class FileTypeHandler<TType>
    {

        private ComboBox fileType;
        public FileTypeHandler(ComboBox fileType, ComboBox codecBox, SupportedOutputGetter getter)
        {
            this.GetOutput = getter;
            this.fileType = fileType;
            codecBox.SelectedIndexChanged += new EventHandler(codec_SelectedIndexChanged);
        }

        public delegate TType[] SupportedOutputGetter();

        private SupportedOutputGetter GetOutput;

        public TType CurrentType
        {
            get { return (TType)fileType.SelectedItem; }
        }

        public delegate void FileTypeEvent(object Sender, TType newType);
        public event FileTypeEvent FileTypeChanged;

        private void codec_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TType[] outputTypes = GetOutput();
            TType currentType = CurrentType;

            if (currentType == null ||
                Array.IndexOf<TType>(outputTypes, currentType) < 0)
                currentType = outputTypes[0];

            this.fileType.Items.Clear();
            foreach (TType t in outputTypes) fileType.Items.Add(t);

            fileType.SelectedItem = currentType;
            if (FileTypeChanged != null) FileTypeChanged(this, currentType);
/*            // now select the previously selected type again if possible
            bool selected = false;
            foreach (TType t in outputTypes)
            {
                if (currentType == t)
                {
                    fileType.SelectedItem = t;
                    currentType = t;
                    selected = true;
                    break;
                }
            }
            if (!selected)
            {
                currentType = outputTypes[0];
                this.fileType.SelectedItem = outputTypes[0];
            }*/
/*            VideoCodecSettings settings = CurrentSettings;
            this.updateIOConfig();
            if (MainForm.verifyOutputFile(this.VideoOutput) == null)
                this.VideoOutput = Path.ChangeExtension(this.VideoOutput, currentType.Extension);*/
        }

        public void RefreshFiletypes()
        {
            codec_SelectedIndexChanged(null, null);
        }
    }

}
