// ****************************************************************************
// 
// Copyright (C) 2005-2008  Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

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
