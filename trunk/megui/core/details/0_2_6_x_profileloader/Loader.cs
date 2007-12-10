using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MeGUI.core.util;
using System.Xml.Serialization;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.core.details._0_2_6_x_profileloader
{
    class Loader
    {
        private static Profile loadProfile<T>(string name)
            where T : GenericSettings
        {
            GenericProfile<T> prof = Util.XmlDeserialize<GenericProfile<T>>(name);
            if (prof == null)
                return null;

            Type t = typeof(GenericProfile<>).MakeGenericType(prof.Settings.GetType());
            return (Profile)Activator.CreateInstance(t, prof.Name, prof.Settings);
        }

        private static List<Profile> getProfiles<T>(string folder)
            where T : GenericSettings
        {
            List<Profile> ps = new List<Profile>();
            if (!Directory.Exists(folder))
                return ps;

            DirectoryInfo di = new DirectoryInfo(folder);
            foreach (FileInfo fi in di.GetFiles("*.xml"))
            {
                Profile p = loadProfile<T>(fi.FullName);
                if (p != null)
                    ps.Add(p);
            }
            return ps;
        }

        public static List<Profile> TryLoadProfiles(string path)
        {
            List<Profile> ps = new List<Profile>();
            ps.AddRange(getProfiles<VideoCodecSettings>(path + @"\profiles\video"));
            ps.AddRange(getProfiles<AudioCodecSettings>(path + @"\profiles\audio"));
            ps.AddRange(getProfiles<AviSynthSettings>(path + @"\profiles\avisynth"));
            ps.AddRange(getProfiles<OneClickSettings>(path + @"\profiles\oneclick"));

            return ps;
        }
    }
}
