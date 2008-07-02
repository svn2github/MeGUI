using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.util;
using System.Collections;
using System.IO;

namespace MeGUI.core.gui
{
    public partial class ProfilePorter : Form
    {
        public ProfilePorter()
        {
            InitializeComponent();
        }

        private static object[] profilesToObjects(Profile[] ps)
        {
            return Array.ConvertAll<Profile, object>(ps,
            delegate(Profile p) { return new Named<Profile>(p.FQName, p); });
        }

        private static Profile[] objectsToProfiles(IEnumerable objects)
        {
            return Array.ConvertAll<object, Profile>(
                Util.ToArray(objects), delegate (object o) { return ((Named<Profile>)o).Data; });
        }

        protected Profile[] Profiles
        {
            get { return objectsToProfiles(profileList.Items); }
            set
            {
                profileList.Items.Clear();
                profileList.Items.AddRange(profilesToObjects(value));
            }
        }

        protected List<Profile> SelectedAndRequiredProfiles
        {
            get
            {
                Profile[] allProfs = Profiles;
                List<Profile> profs = new List<Profile>(objectsToProfiles(profileList.CheckedItems));

                while (true)
                {
                    int oldCount = profs.Count;

                    List<Profile> newProfs = new List<Profile>();

                    foreach (Profile p in profs)
                    {
                        // add the profiles we don't already have
                        newProfs.AddRange(Array.ConvertAll<string, Profile>(p.BaseSettings.RequiredProfiles,
                            delegate(string s) { return Util.ByID(allProfs, s); }));
                    }

                    profs.AddRange(newProfs);
                    profs = Util.RemoveDuds(profs);

                    if (oldCount == profs.Count)
                        break;
                }

                return profs;
            }
        }

        protected Dictionary<string, string> copyExtraFilesToFolder(List<string> extraFiles, string folder)
        {
            Dictionary<string, string> subTable = new Dictionary<string, string>();
            FileUtil.ensureDirectoryExists(folder);

            foreach (string file in extraFiles)
            {
                string filename = Path.GetFileName(file);
                string pathname = Path.Combine(folder, filename);
                subTable[file] = pathname;

                // Copy the file
                if (File.Exists(pathname))
                    File.Delete(pathname);

                File.Copy(file, pathname);
            }

            return subTable;
        }

        protected static void fixFileNames(List<Profile> ps, Dictionary<string, string> subTable)
        {
            foreach (Profile p in ps)
                p.BaseSettings.FixFileNames(subTable);
        }

        protected static string getZippedExtraFileName(string p)
        {
            return Path.Combine("extra", p);
        }

    }
}
