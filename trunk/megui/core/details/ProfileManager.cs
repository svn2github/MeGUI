using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

namespace MeGUI
{
    public class ProfileManager
    {
        string path;
        Dictionary<string, string> selectedProfiles = new Dictionary<string, string>();
        List<string> settingsTypes = new List<string>();
        Dictionary<string, Type> profileTypes = new Dictionary<string,Type>();
        Dictionary<string, Dictionary<string, Profile>> profiles = new Dictionary<string,Dictionary<string,Profile>>();

        public bool Register(string name, Type type)
        {
            if (settingsTypes.Contains(name)) return false;
            System.Diagnostics.Debug.Assert((!profileTypes.ContainsKey(name)) && (!profiles.ContainsKey(name)));
            settingsTypes.Add(name);
            profileTypes[name] = type;
            profiles[name] = new Dictionary<string, Profile>();
            selectedProfiles[name] = "";
            return true;
        }

        public ProfileManager(string path)
        {
            this.path = path;
            Register(new x264Settings().getSettingsType(), typeof(GenericProfile<VideoCodecSettings>));
            Register(new AudioCodecSettings().getSettingsType(), typeof(GenericProfile<AudioCodecSettings>));
            Register(new OneClickSettings().getSettingsType(), typeof(GenericProfile<OneClickSettings>));
            Register(new AviSynthSettings().getSettingsType(), typeof(GenericProfile<AviSynthSettings>));
        }
        #region loading and saving
        public void LoadProfiles()
        {
            loadProfiles();
        }
        /// <summary>
        /// saves all the profiles
        /// this is called when the program exists and ensures that all
        /// currently defined profiles are saved, overwriting currently existing ones
        /// </summary>
        public void SaveProfiles()
        {
            foreach (Dictionary<string, Profile> profileSet in profiles.Values)
            {
                foreach (Profile prof in profileSet.Values)
                {
                    saveProfile(prof);
                }
            }
            saveSelectedProfiles();
        }

        private void saveSelectedProfiles()
        {
            string filename = path + @"\profiles\SelectedProfiles.xml";
            XmlSerializer ser = null;
            using (Stream s = File.Open(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    // Hacky workaround because serialization of dictionaries isn't possible
                    string[] keys = new string[selectedProfiles.Count];
                    string[] values = new string[selectedProfiles.Count];
                    selectedProfiles.Keys.CopyTo(keys, 0);
                    for (int i = 0; i < keys.Length; i++)
                    {
                        values[i] = selectedProfiles[keys[i]];
                    }
                    string[][] data = new string[][] { keys, values };
                    ser = new XmlSerializer(data.GetType());
                    ser.Serialize(s, data);
                }
                catch (Exception e)
                {
                    MessageBox.Show("List of selected profiles could not be saved. Error message: " + e.Message, "Error saving profile", MessageBoxButtons.OK);
                }
            }
        }

        private void loadSelectedProfiles()
        {
            string file = path + @"\profiles\SelectedProfiles.xml";
            if (!File.Exists(file)) return;
            using (Stream s = File.OpenRead(path + @"\profiles\SelectedProfiles.xml"))
            {
                try
                {
                    // Hacky workaround because serialization of dictionaries isn't possible
                    XmlSerializer ser = new XmlSerializer(typeof(string[][]));
                    string[][] data = (string[][])ser.Deserialize(s);
                    string[] keys = data[0];
                    string[] values = data[1];
                    System.Diagnostics.Debug.Assert(keys.Length == values.Length);
                    for (int i = 0; i < keys.Length; i++)
                    {
                        selectedProfiles[keys[i]] = values[i];
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("List of selected profiles could not be loaded.", "Error loading profile", MessageBoxButtons.OK);
                }
            }
        }

        private void loadProfiles()
        {
            foreach (string type in settingsTypes)
            {
                string profilePath = path + @"\profiles\" + type;
                if (!Directory.Exists(profilePath))
                    Directory.CreateDirectory(profilePath);
                DirectoryInfo di = new DirectoryInfo(profilePath);
                FileInfo[] files = di.GetFiles("*.xml");
                foreach (FileInfo fi in files)
                {
                    string fileName = fi.FullName;
                    if (Path.GetFileNameWithoutExtension(fileName) != "") // additional check to ensure that rogue profiles are not loaded
                    {
                        Profile prof = loadProfile(fileName, type);
                        if (prof != null)
                        {
                            if (profiles[type].ContainsKey(prof.Name))
                                MessageBox.Show(type + " profile " + prof.Name + " is has already been loaded\nDiscarding duplicate profile " + fi.FullName, "Duplicate profile name",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            else
                            {
                                profiles[type].Add(prof.Name, prof);
                            }
                        }
                    }
                }                
            }
            loadSelectedProfiles();
		}
        /// <summary>
        /// saves a profile to program directory\profiles\profilename.xml
        /// the serializer mechanism used is xml which yields a humanly readable profile
        /// </summary>
        /// <param name="prof">the Profile to be saved</param>
		private void saveProfile(Profile prof)
		{
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
			XmlSerializer ser = null;
			string fileName = ProfilePath(prof);
            FileUtil.ensureDirectoryExists(Path.GetDirectoryName(fileName));
			using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
			{
				try
				{
					ser = new XmlSerializer(prof.GetType());
					ser.Serialize(s, prof);
				}
				catch (Exception e)
				{
					MessageBox.Show("Profile " + prof.Name + " could not be saved. Error message: " + e.Message, "Error saving profile", MessageBoxButtons.OK);
                    s.Close();
                    try { File.Delete(fileName); }
                    catch (IOException) { }
				}
			}
		}

        /// <summary>
        /// loads a profile with a given name from program-directory\profiles\profilename.xml
        /// </summary>
        /// <param name="name">name of the profile</param>
        /// <returns>the loaded Profile or null if the profile could not be loaded</returns>
        private Profile loadProfile(string name, string type)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    try
                    {
                        ser = new XmlSerializer(profileTypes[type]);
                        return (Profile)ser.Deserialize(s);
                    }
                    catch (InvalidOperationException)
                    {
                        s.Close();
                        return updateProfile(name, type); // If this fails, it will throw to the catch below
                    }
                }
                catch (Exception)
                {
                    DialogResult r = MessageBox.Show("Profile " + name + " could not be loaded. Delete?", "Error loading profile", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private Profile updateProfile(string name, string type)
        {
            string[] data = File.ReadAllLines(name);
            if (new Regex("GenericProfileOf[a-zA-Z]+").IsMatch(data[1])) // It's already a new profile
                return null;
            Regex matchLine2 = new Regex(@"(?<=\<)[a-zA-Z]+");
            string replaceName = "GenericProfileOf" + profileTypes[type].GetGenericArguments()[0].Name;
            data[1] = matchLine2.Replace(data[1], replaceName);
            Regex matchLastLine = new Regex(@"(?<=\</)[a-zA-Z]+");
            data[data.Length - 1] = matchLastLine.Replace(data[data.Length - 1], replaceName);
            File.WriteAllLines(name, data);

            using (Stream s = File.OpenRead(name))
            {
                XmlSerializer ser = new XmlSerializer(profileTypes[type]);
                return (Profile)ser.Deserialize(s);
            }
        }
        #endregion
        #region individual setting and getting
        /// <summary>
        /// delets the audio profile with the given name
        /// </summary>
        /// <param name="name">the name of the profile to be deleted</param>
        public bool DeleteProfile(string name, string type)
        {
            if (profiles[type].Remove(name))
            {
                if (selectedProfiles[type] == name) selectedProfiles[type] = "";
                string fileName = path + @"\profiles\" + type + "\\" + name + ".xml";
                if (File.Exists(fileName))
                    File.Delete(fileName);
                return true;
            }
            else
            {
                MessageBox.Show("Profile " + name + " could not be found and can thus not be deleted.", "Error loading profile", MessageBoxButtons.OK);
                return false;
            }
        }
        /// <summary>
        /// adds a new audio profile
        /// </summary>
        /// <param name="prof">the new profile to be added</param>
        /// <returns>true if the insertion succeeded, false if not</returns>
        public bool AddProfile(Profile prof, string type)
        {
            if (profiles[type].ContainsKey(prof.Name))
                return false;
            else
            {
                profiles[type].Add(prof.Name, prof);
                saveProfile(prof);
                return true;
            }
        }

        public string GetSelectedProfileName(string profileType)
        {
            return selectedProfiles[profileType];
        }

        public Profile GetSelectedProfile(string profileType)
        {
            string name = GetSelectedProfileName(profileType);
            if (string.IsNullOrEmpty(name)) return null;
            try { return profiles[profileType][name]; }
            catch { }
            return null;
        }

        public void SetSelectedProfile(Profile prof)
        {
            SetSelectedProfile(prof.BaseSettings.getSettingsType(), prof.Name);
        }

        public void SetSelectedProfile(string profileType, string name)
        {
            selectedProfiles[profileType] = name;
        }
        #endregion
        #region helper methods
        /// <summary>
        /// eliminates non allowed characters and replaced them with an underscore
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string fixName(string name)
        {
            name = name.Replace("\"", "_hc_");
            name = name.Replace("*", "_st_");
            name = name.Replace("/", "_sl_");
            name = name.Replace(":", "_dp_");
            name = name.Replace("<", "_lt_");
            name = name.Replace(">", "_rt_");
            name = name.Replace("?", "_qm_");
            name = name.Replace("\\", "_bs_");
            name = name.Replace("|", "_vl_");
            return name;
        }

        public string ProfilePath(Profile prof)
        {
            return ProfilePath(prof, this.path);
        }

        public static string ProfilePath(Profile prof, string path)
        {
            return Path.Combine(path, @"profiles\" + prof.BaseSettings.getSettingsType() + "\\" + fixName(prof.Name) + ".xml");
        }
        #endregion

        #region my hasher
        class MyHashTable<TSettings>
            where TSettings : GenericSettings
        {
            private Dictionary<string, Profile> impl;
            public GenericProfile<TSettings> this[string key]
            {
                get
                {
                    return (GenericProfile<TSettings>)impl[key];
                }
                set
                {
                    impl[key] = value;
                }
            }
            MyHashTable(Dictionary<string, Profile> impl)
            {
                this.impl = impl;
            }
        }
        #endregion
        #region properties
#warning look at properties here. Work out how to do the types
        public Dictionary<string, Profile> AudioProfiles
        {
            get { return profiles[new AudioCodecSettings().getSettingsType()]; }
            set { profiles[new AudioCodecSettings().getSettingsType()] = value; }
        }
        public Dictionary<string, Profile> AvsProfiles
        {
            get { return profiles[new AviSynthSettings().getSettingsType()]; }
            set { profiles[new AviSynthSettings().getSettingsType()] = value; }
        }
        public Dictionary<string, Profile> OneClickProfiles
        {
            get { return profiles[new OneClickSettings().getSettingsType()]; }
            set { profiles[new OneClickSettings().getSettingsType()] = value; }
        }
        public Dictionary<string, Profile> VideoProfiles
        {
            get { return profiles[new x264Settings().getSettingsType()]; }
            set { profiles[new x264Settings().getSettingsType()] = value; }
        }
        public Dictionary<string, Profile> Profiles(string type) { return profiles[type]; }
        #endregion
        #region Profile Listing
        public Profile ByFormattedName(string formattedName)
        {
            string type = formattedName.Substring(0, formattedName.IndexOf(':'));
            System.Diagnostics.Debug.Assert(formattedName.StartsWith(type + ": "));
            string profileName = formattedName.Substring(type.Length + 2);
            return Profiles(type)[profileName];
        }

        public string[] AllProfileNames
        {
            get
            {
                List<string> profileList = new List<string>();
                foreach (string key in profiles.Keys)
                {
                    Dictionary<string, Profile> profileSet = profiles[key];
                    foreach (string name in profileSet.Keys)
                    {
                        profileList.Add(key + ": " + name);
                    }
                }
                return profileList.ToArray();
            }
        }

        public void AddDependantProfiles(string formattedName, Dictionary<string, Profile> includedProfiles)
        {
            if (includedProfiles.ContainsKey(formattedName)) return; // This profile has already been added
            
            Profile prof = ByFormattedName(formattedName);
            includedProfiles[formattedName] = prof;
            
            string[] formattedNames = prof.BaseSettings.RequiredProfiles;
            foreach (string profileName in formattedNames)
            {
                AddDependantProfiles(profileName, includedProfiles);
            }
        }
        public Profile[] AllProfiles(string[] formattedProfileNames)
        {
            Dictionary<string, Profile> profileList = new Dictionary<string, Profile>();
            foreach (string formattedProfileName in formattedProfileNames)
            {
                AddDependantProfiles(formattedProfileName, profileList);
            }
            Profile[] array = new Profile[profileList.Count];
            profileList.Values.CopyTo(array, 0);
            return array;
        }

        public List<string> AllRequiredFiles(Profile[] profiles)
        {
            List<string> files = new List<string>();
            
            // Generate the list of files
            foreach (Profile prof in profiles)
                files.AddRange(prof.BaseSettings.RequiredFiles);

            // Make it unique
            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                int index;
                while ((index = files.LastIndexOf(file)) > i)
                {
                    files.RemoveAt(index);
                }
            }

            return files;
        }
     public static void FixFileNames(Profile[] profileList, Dictionary<string, string> substitutionTable)
        {
            foreach (Profile prof in profileList)
            {
                prof.BaseSettings.FixFileNames(substitutionTable);
            }
        }

        public void AddAll(Profile[] profiles, DialogManager asker)
        {
            foreach (Profile prof in profiles)
            {
                Add(prof, asker);
            }
        }
        public void Add(Profile prof, DialogManager asker)
        {
            if (!AddProfile(prof, prof.BaseSettings.getSettingsType()))
            {
                if (!asker.overwriteProfile(prof.Name))
                    return;
                else
                {
                    DeleteProfile(prof);
                    Add(prof, asker);
                }
            }
        }
        public bool DeleteProfile(Profile prof)
        {
            return DeleteProfile(prof.Name, prof.BaseSettings.getSettingsType());
        }
        #endregion

        public bool AddProfile(Profile prof)
        {
            return AddProfile(prof, prof.BaseSettings.getSettingsType());
        }

        public bool AddVideoProfile(GenericProfile<VideoCodecSettings> prof)
        {
            return AddProfile(prof, prof.BaseSettings.getSettingsType());
        }

    }
}
