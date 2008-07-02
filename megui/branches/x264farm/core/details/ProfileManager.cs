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
            int index = profileType.IndexOf(':');
            if (index >= 0)
            {
                string start = profileType.Substring(0, index);
                Profile prof = GetSelectedProfile(start);
                if (prof.BaseSettings.matchesType(profileType))
                    return prof.Name;
                return null;
            }

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

        /*if (!path.EndsWith("\\") && !path.EndsWith("/"))
            {
                if (path.Length >= 0)
                    path = path + "\\";
            }
            if (type == typeof(GenericProfile<VideoCodecSettings>))
                return path + @"profiles\video\" + fixName(profName) + ".xml";
            if (type == typeof(GenericProfile<AudioCodecSettings>))
                return path + @"profiles\audio\" + fixName(profName) + ".xml";
            if (type == typeof(GenericProfile<AviSynthSettings>))
                return path + @"profiles\avs\" + fixName(profName) + ".xml";
            if (type == typeof(GenericProfile<OneClickSettings>))
                return path + @"profiles\oneclick\" + fixName(profName) + ".xml";
            throw new ArgumentException("Unknown type");
        }*/
    
        #endregion
        #region old
        #region generic loading and saving

        /*
            foreach (GenericProfile<VideoCodecSettings> vProf in videoProfiles.Values)
            {
                saveVideoProfile(vProf);
            }
            foreach (GenericProfile<AudioCodecSettings> aProf in audioProfiles.Values)
            {
                saveAudioProfile(aProf);
            }
            foreach (GenericProfile<AviSynthSettings> asProf in avsProfiles.Values)
            {
                saveAviSynthProfile(asProf);
            }
            foreach (GenericProfile<OneClickSettings> oProf in oneClickProfiles.Values)
            {
                saveOneClickProfile(oProf);
            }
        }*/

/*        /// <summary>
        /// loads all the profiles from the harddisk.
        /// Every *.xml file in the profile directory is considered to be a profile
        /// the profile dropdown list is then filled with all the loaded elements
        /// </summary>
        /// <summary>
        /// loads all audio, video and global profiles
        /// </summary>
        public void LoadProfiles()
        {
            this.loadVideoProfiles();
            this.loadAudioProfiles();
            this.loadAviSynthProfiles();
            this.loadOneClickProfiles();
        }*/
        #endregion
        /*
        #region video
        /// <summary>
        /// loads all the profiles from the harddisk.
        /// Every *.xml file in the profile directory is considered to be a profile
        /// the profile dropdown list is then filled with all the loaded elements
        /// </summary>
        private void loadVideoProfiles()
        {
            string profilePath = this.path + @"\profiles\video\";
            if (!Directory.Exists(profilePath))
                Directory.CreateDirectory(profilePath);
            DirectoryInfo di = new DirectoryInfo(profilePath);
            FileInfo[] files = di.GetFiles("*.xml");
            foreach (FileInfo fi in files)
            {
                string fileName = fi.FullName;
                if (Path.GetFileNameWithoutExtension(fileName) != "") // additional check to ensure that rogue profiles are not loaded
                {
                    GenericProfile<VideoCodecSettings> prof = this.loadVideoProfile(fileName);
                    if (prof != null)
                    {
                        if (videoProfiles.ContainsKey(prof.Name))
                            MessageBox.Show("Video profile " + prof.Name + " is has already been loaded\nDiscarding duplicate profile " + fi.FullName, "Duplicate profile name",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                        {
                            this.videoProfiles.Add(prof.Name, prof);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// saves a profile to program directory\profiles\profilename.xml
        /// the serializer mechanism used is xml which yields a humanly readable profile
        /// before saving the zones are reset in order not to save them
        /// this is done because zones are assumed to be source dependant whereas a 
        /// profile is generic
        /// </summary>
        /// <param name="prof">the Profile to be saved</param>
        private void saveVideoProfile(GenericProfile<VideoCodecSettings> prof)
        {
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
            XmlSerializer ser = null;
            string fileName = ProfilePath(prof);
            using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    prof.Settings.Zones = new Zone[0];
                    ser = new XmlSerializer(typeof(GenericProfile<VideoCodecSettings>));
                    ser.Serialize(s, prof);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Video Profile " + prof.Name + " could not be saved. Error message: " + e.Message, "Error saving profile", MessageBoxButtons.OK);
                }
            }
        }
        /// <summary>
        /// loads a profile with a given name from program-directory\profiles\profilename.xml
        /// </summary>
        /// <param name="name">name of the profile</param>
        /// <returns>the loaded Profile or null if the profile could not be loaded</returns>
        private GenericProfile<VideoCodecSettings> loadVideoProfile(string name)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    ser = new XmlSerializer(typeof(GenericProfile<VideoCodecSettings>));
                    return (GenericProfile<VideoCodecSettings>)ser.Deserialize(s);
                }
                catch (Exception)
                {
                    MessageBox.Show("Profile " + name + " could not be loaded. Is it a valid profile created by MeGUI?", "Error loading profile", MessageBoxButtons.OK);
                    return null;
                }
            }
        }
        /// <summary>
        /// delets the video profile with the given name
        /// </summary>
        /// <param name="name">the name of the profile to be deleted</param>
        public bool DeleteVideoProfile(string name)
        {
            if (this.videoProfiles.Remove(name))
            {
                string fileName = ProfilePath(name, typeof(GenericProfile<VideoCodecSettings>));
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
        /// adds a new video profile
        /// </summary>
        /// <param name="prof">the new profile to be added</param>
        /// <returns>true if the insertion succeeded, false if not</returns>
        public bool AddVideoProfile(GenericProfile<VideoCodecSettings> prof)
        {
            if (this.videoProfiles.ContainsKey(prof.Name))
                return false;
            else
            {
                this.videoProfiles.Add(prof.Name, prof);
                saveVideoProfile(prof);
                return true;
            }
        }
        #endregion
        #region audio
        /// <summary>
        /// loads all the audio profiles
        /// </summary>
        private void loadAudioProfiles()
		{
			string profilePath = path + @"\profiles\audio\";
			if (!Directory.Exists(profilePath))
				Directory.CreateDirectory(profilePath);
			DirectoryInfo di = new DirectoryInfo(profilePath);
			FileInfo[] files = di.GetFiles("*.xml");
			foreach (FileInfo fi in files)
			{
				string fileName = fi.FullName;
                if (Path.GetFileNameWithoutExtension(fileName) != "") // additional check to ensure that rogue profiles are not loaded
                {
				    GenericProfile<AudioCodecSettings> prof = loadAudioProfile(fileName);
                    if (prof != null)
                    {
                        if (audioProfiles.ContainsKey(prof.Name))
                            MessageBox.Show("Audio profile " + prof.Name + " is has already been loaded\nDiscarding duplicate profile " + fi.FullName, "Duplicate profile name",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                        {
                            this.audioProfiles.Add(prof.Name, prof);
                        }
                    }
				}
			}
		}
        /// <summary>
        /// saves a profile to program directory\profiles\profilename.xml
        /// the serializer mechanism used is xml which yields a humanly readable profile
        /// </summary>
        /// <param name="prof">the Profile to be saved</param>
		private void saveAudioProfile(GenericProfile<AudioCodecSettings> prof)
		{
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
			XmlSerializer ser = null;
			string fileName = ProfilePath(prof);
			using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
			{
				try
				{
					ser = new XmlSerializer(typeof(GenericProfile<AudioCodecSettings>));
					ser.Serialize(s, prof);
				}
				catch (Exception e)
				{
					MessageBox.Show("Audio Profile " + prof.Name + " could not be saved. Error message: " + e.Message, "Error saving profile", MessageBoxButtons.OK);
				}
			}
		}
        /// <summary>
        /// loads a profile with a given name from program-directory\profiles\profilename.xml
        /// </summary>
        /// <param name="name">name of the profile</param>
        /// <returns>the loaded Profile or null if the profile could not be loaded</returns>
		private GenericProfile<AudioCodecSettings> loadAudioProfile(string name)
		{
			XmlSerializer ser = null;
			using (Stream s = File.OpenRead(name))
			{
				try
				{
					ser = new XmlSerializer(typeof(GenericProfile<AudioCodecSettings>));
					return (GenericProfile<AudioCodecSettings>)ser.Deserialize(s);
				}
				catch (Exception)
				{
					MessageBox.Show("Profile " + name + " could not be loaded. Is it a valid profile created by MeGUI?", "Error loading profile", MessageBoxButtons.OK);
					return null;
				}
			}
		}
        /// <summary>
        /// delets the audio profile with the given name
        /// </summary>
        /// <param name="name">the name of the profile to be deleted</param>
        public bool DeleteAudioProfile(string name)
        {
            if (this.audioProfiles.Remove(name))
            {
                string fileName = path + @"\profiles\audio\" + name + ".xml";
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
        public bool AddAudioProfile(GenericProfile<AudioCodecSettings> prof)
        {
            if (this.audioProfiles.ContainsKey(prof.Name))
                return false;
            else
            {
                this.audioProfiles.Add(prof.Name, prof);
                saveAudioProfile(prof);
                return true;
            }
        }
        #endregion
        #region avisynth
        /// <summary>
        /// adds a profile to the dictionary and saves it to the harddisk immediately
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        public bool AddAviSynthProfile(GenericProfile<AviSynthSettings> prof)
        {
            if (this.avsProfiles.ContainsKey(prof.Name))
                return false;
            else
            {
                this.avsProfiles.Add(prof.Name, prof);
                saveAviSynthProfile(prof);
                return true;
            }
        }
        /// <summary>
        /// deletes a profile from the storage and the harddisk
        /// </summary>
        /// <param name="name"></param>
        public bool DeleteAviSynthProfile(string name)
        {
            if (this.avsProfiles.Remove(name))
            {
                string fileName = ProfilePath(name, typeof(GenericProfile<AviSynthSettings>));
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
        /// loads all the AviSynth profiles from your HD
        /// </summary>
        private void loadAviSynthProfiles()
        {
            string profilePath = path + @"\profiles\avs\";
            if (!Directory.Exists(profilePath))
                Directory.CreateDirectory(profilePath);
            DirectoryInfo di = new DirectoryInfo(profilePath);
            FileInfo[] files = di.GetFiles("*.xml");
            foreach (FileInfo fi in files)
            {
                string fileName = fi.FullName;
                GenericProfile<AviSynthSettings> prof = loadAviSynthProfile(fileName);
                if (prof != null)
                {
                    if (avsProfiles.ContainsKey(prof.Name))
                        MessageBox.Show("Audio profile " + prof.Name + " is has already been loaded\nDiscarding duplicate profile " + fi.FullName, "Duplicate profile name",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        this.avsProfiles.Add(prof.Name, prof);
                    }
                }
            }
            if (!this.avsProfiles.ContainsKey(new GenericProfile<AviSynthSettings>().Name))
                this.avsProfiles.Add(new GenericProfile<AviSynthSettings>().Name, new GenericProfile<AviSynthSettings>());
        }
        /// <summary>
        /// saves a given AviSynth profile to the profile path
        /// </summary>
        /// <param name="prof">the profile to be saved</param>
        private void saveAviSynthProfile(GenericProfile<AviSynthSettings> prof)
        {
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
            XmlSerializer ser = null;
            string fileName = ProfilePath(prof);
            using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    ser = new XmlSerializer(typeof(GenericProfile<AviSynthSettings>));
                    ser.Serialize(s, prof);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Avisynth Profile " + prof.Name + " could not be saved. Error message: " + e.Message, "Error saving profile", MessageBoxButtons.OK);
                }
            }
        }
        /// <summary>
        /// loads a profile with a given filename
        /// </summary>
        /// <param name="name">the filename of the path</param>
        /// <returns>the profile loaded or null if no profile has been loaded</returns>
        private GenericProfile<AviSynthSettings> loadAviSynthProfile(string name)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    ser = new XmlSerializer(typeof(GenericProfile<AviSynthSettings>));
                    return (GenericProfile<AviSynthSettings>)ser.Deserialize(s);
                }
                catch (Exception)
                {
                    MessageBox.Show("Profile " + name + " could not be loaded. Is it a valid profile created by MeGUI?", "Error loading profile", MessageBoxButtons.OK);
                    return null;
                }
            }
        }
        #endregion
        #region one click
        /// <summary>
        /// adds a profile to the dictionary and saves it to the harddisk immediately
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        public bool AddOneClickProfile(GenericProfile<OneClickSettings> prof)
        {
            if (this.oneClickProfiles.ContainsKey(prof.Name))
                return false;
            else
            {
                this.oneClickProfiles.Add(prof.Name, prof);
                this.saveOneClickProfile(prof);
                return true;
            }
        }
        /// <summary>
        /// deletes a profile from the storage and the harddisk
        /// </summary>
        /// <param name="name"></param>
        public bool DeleteOneClickProfile(string name)
        {
            if (this.oneClickProfiles.Remove(name))
            {
                string fileName = ProfilePath(name, typeof(GenericProfile<OneClickSettings>));
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
        /// loads all the one click profiles
        /// </summary>
        private void loadOneClickProfiles()
        {
            string profilePath = path + @"\profiles\oneclick\";
            if (!Directory.Exists(profilePath))
                Directory.CreateDirectory(profilePath);
            DirectoryInfo di = new DirectoryInfo(profilePath);
            FileInfo[] files = di.GetFiles("*.xml");
            foreach (FileInfo fi in files)
            {
                string fileName = fi.FullName;
                GenericProfile<OneClickSettings> prof = loadOneClickProfile(fileName);
                if (prof != null)
                {
                    if (oneClickProfiles.ContainsKey(prof.Name))
                        MessageBox.Show("OneClick profile " + prof.Name + " is has already been loaded\nDiscarding duplicate profile " + fi.FullName, "Duplicate profile name",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        this.oneClickProfiles.Add(prof.Name, prof);
                    }
                }
            }
        }
        /// <summary>
        /// loads a given one click profile
        /// </summary>
        /// <param name="name">the filename of the profile</param>
        /// <returns>the profile or null if there was an error loading</returns>
        private GenericProfile<OneClickSettings> loadOneClickProfile(string name)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    ser = new XmlSerializer(typeof(GenericProfile<OneClickSettings>));
                    return (GenericProfile<OneClickSettings>)ser.Deserialize(s);
                }
                catch (Exception)
                {
                    MessageBox.Show("Profile " + name + " could not be loaded. Is it a valid profile created by MeGUI?", "Error loading profile", MessageBoxButtons.OK);
                    return null;
                }
            }
        }
        /// <summary>
        /// saves a one click profile
        /// </summary>
        /// <param name="prof">the profile to be saved</param>
        private void saveOneClickProfile(GenericProfile<OneClickSettings> prof)
        {
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
            XmlSerializer ser = null;
            string fileName = ProfilePath(prof);
            using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    ser = new XmlSerializer(typeof(GenericProfile<OneClickSettings>));
                    ser.Serialize(s, prof);
                }
                catch (Exception e)
                {
                    MessageBox.Show("One click Profile " + prof.Name + " could not be saved. Error message: " + e.Message, "Error saving profile", MessageBoxButtons.OK);
                }
            }
        }
        #endregion
         * */
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
        public IDictionary<string, Profile> Profiles(string type) {
            int index = type.IndexOf(':');
            if (index >= 0)
            {
                string start = type.Substring(0, index);
                string end = type.Substring(index + 1);
                SelectiveDictionary<string, Profile> val =
                    new SelectiveDictionary<string, Profile>(profiles[start]);
                val.Matches = delegate(Profile p) { return p.BaseSettings.matchesType(type); };
                return val;
            }

            return profiles[type]; 
        }
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
/*            List<Profile> profiles = new List<Profile>();
            Profile prof = 
            try
            {
                if (formattedName.StartsWith("Video: "))
                {
                    return new Profile[] { VideoProfiles[formattedName.Substring(7)].baseClone() };
                }
                else if (formattedName.StartsWith("Audio: "))
                {
                    return new Profile[] { AudioProfiles[formattedName.Substring(7)] };
                }
                else if (formattedName.StartsWith("AviSynth: "))
                {
                    return new Profile[] { AvsProfiles[formattedName.Substring(10)] };
                }
                else if (formattedName.StartsWith("OneClick: "))
                {
                    GenericProfile<OneClickSettings> profile = (GenericProfile<OneClickSettings>)OneClickProfiles[formattedName.Substring(10)];
                    if (profile.Settings.DontEncodeAudio)
                        return new Profile[] { profile, VideoProfiles[profile.Settings.VideoProfileName].baseClone() };
                    else
                        return new Profile[] { profile,
                        VideoProfiles[profile.Settings.VideoProfileName].baseClone(),
                        AudioProfiles[profile.Settings.AudioProfileName] };
                }
                else return null;
            }
            catch (KeyNotFoundException) // In this case the profile name is not valid for some reason.
            {
                return null;
            }*/
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

        public List<string> AllRequiredFiles(Profile[] profiles  /*, out Dictionary<string, string> substitutionTable*/)
        {
            /*Dictionary<string, string> fileList = new Dictionary<string,string>();
            substitutionTable = new Dictionary<string, string>();*/
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
        /*
            // Generate a substitution table
            foreach (string file in files)
            {
                string filename = "extra\\" + Path.GetFileName(file);
                string keyName = filename;
                bool fileInserted = false;
                int count = 0;
                while (!fileInserted)
                {
                    if (fileList.ContainsKey(keyName))
                    {
                        if (fileList[keyName] == file)
                        {
                            fileInserted = true;
                        }
                        else
                        {
                            count++;
                            keyName = filename + "_" + count;
                        }
                    }
                    else
                    {
                        substitutionTable.Add(file, keyName);
                        fileList.Add(keyName, file);
                        fileInserted = true;
                    }
                }
            }
            return fileList;
        }*/
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
