using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Text;

using MeGUI.core.gui;

namespace MeGUI.core.util
{
    class CustomUserSettings : ApplicationSettingsBase
    {
        private CustomUserSettings() : base() { }

        private static CustomUserSettings defaultInstance = ((CustomUserSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new CustomUserSettings())));

        public static CustomUserSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting]
        public Dar[] CustomDARs
        {
            get
            {
                object o = this["CustomDARs"];
                return (Dar[])this["CustomDARs"];
            }
            set
            {
                this["CustomDARs"] = value; // new ArrayConverter<Named<Dar>, DarConverter>().ToString(value);
                object o = this["CustomDARs"];
            }
        }

        [UserScopedSetting]
        public FileSize[] CustomSizes
        {
            get
            {
                return (FileSize[])this["CustomSizes"];
            }
            set
            {
                this["CustomSizes"] = value;
            }
        }


        [UserScopedSetting]
        public FPS[] CustomFPSs
        {
            get
            {
                return (FPS[])this["CustomFPSs"];
            }
            set
            {
                this["CustomFPSs"] = value;
            }
        }
    }
}
