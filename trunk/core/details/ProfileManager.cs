using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Windows.Forms;
using MeGUI.core.plugins.interfaces;

namespace MeGUI
{
    public class ProfileManager
    {
        string path;
/*        Dictionary<string, GenericProfile<AudioCodecSettings>> audioProfiles;
        Dictionary<string, GenericProfile<AviSynthSettings>> avsProfiles;
        Dictionary<string, GenericProfile<OneClickSettings>> oneClickProfiles;
        Dictionary<string, GenericProfile<VideoCodecSettings>> videoProfiles;*/
        Dictionary<string, GenericSettings> settingsTypes = new Dictionary<string,GenericSettings>();
        Dictionary<string, Type> profileTypes = new Dictionary<string,Type>();
        Dictionary<string, Dictionary<string, Profile>> profiles = new Dictionary<string,Dictionary<string,Profile>>();

        public bool Register(GenericSettings settingsType, Type type)
        {
            string name = settingsType.getSettingsType();
            if (settingsTypes.ContainsKey(name)) return false;
            System.Diagnostics.Debug.Assert((!profileTypes.ContainsKey(name)) && (!profiles.ContainsKey(name)));
            settingsTypes[name] = settingsType;
            profileTypes[name] = type;
            profiles[name] = new Dictionary<string, Profile>();
            return true;
        }

        public ProfileManager(string path)
        {
            this.path = path;
            Register(new x264Settings(), typeof(GenericProfile<VideoCodecSettings>));
            Register(new AudioCodecSettings(), typeof(GenericProfile<AudioCodecSettings>));
            Register(new OneClickSettings(), typeof(GenericProfile<OneClickSettings>));
            Register(new AviSynthSettings(), typeof(GenericProfile<OneClickSettings>));
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
        }
        private void loadProfiles()
        {
            foreach (GenericSettings type in settingsTypes.Values)
            {
                string profilePath = path + @"\profiles\" + type.getSettingsType();
                if (!Directory.Exists(profilePath))
                    Directory.CreateDirectory(profilePath);
                DirectoryInfo di = new DirectoryInfo(profilePath);
                FileInfo[] files = di.GetFiles("*.xml");
                foreach (FileInfo fi in files)
                {
                    string fileName = fi.FullName;
                    if (Path.GetFileNameWithoutExtension(fileName) != "") // additional check to ensure that rogue profiles are not loaded
                    {
                        Profile prof = loadProfile(fileName, type.getSettingsType());
                        if (prof != null)
                        {
                            if (profiles[type.getSettingsType()].ContainsKey(prof.Name))
                                MessageBox.Show(type.getSettingsType() + " profile " + prof.Name + " is has already been loaded\nDiscarding duplicate profile " + fi.FullName, "Duplicate profile name",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            else
                            {
                                profiles[type.getSettingsType()].Add(prof.Name, prof);
                            }
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
		private void saveProfile(Profile prof)
		{
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
			XmlSerializer ser = null;
			string fileName = ProfilePath(prof);
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
					ser = new XmlSerializer(profileTypes[type]);
					return (Profile)ser.Deserialize(s);
				}
				catch (Exception)
				{
					MessageBox.Show("Profile " + name + " could not be loaded. Is it a valid profile created by MeGUI?", "Error loading profile", MessageBoxButtons.OK);
					return null;
				}
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
            if (profiles[name].Remove(name))
            {
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
        public Dictionary<string, Profile> Profiles(string type) { return profiles[type]; }
        #endregion
        #region Profile Listing
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
        public Profile[] AllDependantProfiles(string formattedName)
        {
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
            }
        }
        public Profile[] AllProfiles(string[] formattedProfileNames, out List<string> failedProfiles)
        {
            failedProfiles = new List<string>();

            // List all the profiles
            List<Profile> profiles = new List<Profile>();
            foreach (string formattedProfileName in formattedProfileNames)
            {
                Profile[] relatedProfiles = AllDependantProfiles(formattedProfileName);
                if (relatedProfiles == null)
                    failedProfiles.Add(formattedProfileName);
                else
                    profiles.AddRange(relatedProfiles);
            }

            // Cull the repeated ones.
            for (int i = 0; i < profiles.Count; i++)
            {
                for (int j = i + 1; j < profiles.Count; j++)
                {
                    if (profiles[i] == profiles[j])
                    {
                        profiles.RemoveAt(j);
                        j--;
                    }
                }
            }
    

            return profiles.ToArray();
        }
        public string[] RequiredFiles(Profile profile)
        {
            if (profile is GenericProfile<VideoCodecSettings>)
            {
                GenericProfile<VideoCodecSettings> vProfile = profile as GenericProfile<VideoCodecSettings>;
                if (vProfile.Settings is x264Settings)
                {
                    if ((vProfile.Settings as x264Settings).QuantizerMatrixType == 2) // Custom profile
                        return new string[] { (vProfile.Settings as x264Settings).QuantizerMatrix };
                }
                if (vProfile.Settings is xvidSettings)
                {
                    if ((vProfile.Settings as xvidSettings).QuantType == 2) // CQM
                        return new string[] { (vProfile.Settings as xvidSettings).CustomQuantizerMatrix };
                }
            }
            return new string[] { };
        }
        public Dictionary<string, string> AllRequiredFiles(Profile[] profiles, out Dictionary<string, string> substitutionTable)
        {
            Dictionary<string, string> fileList = new Dictionary<string,string>();
            substitutionTable = new Dictionary<string, string>();
            List<string> files = new List<string>();
            foreach (Profile prof in profiles)
                files.AddRange(RequiredFiles(prof));

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
        }
        public static void FixFileNames(Profile[] profileList, Dictionary<string, string> substitutionTable)
        {
            foreach (Profile prof in profileList)
            {
                FixFileNames(prof, substitutionTable);
            }
        }

        public static void FixFileNames(Profile profile, Dictionary<string, string> substitutionTable)
        {
            if (profile is GenericProfile<VideoCodecSettings>)
            {
                GenericProfile<VideoCodecSettings> vProf = profile as GenericProfile<VideoCodecSettings>;
                if (vProf.Settings is x264Settings)
                {
                    x264Settings xSettings = vProf.Settings as x264Settings;
                    if (xSettings.QuantizerMatrixType == 2) // CQM
                    {
                        if (substitutionTable.ContainsKey(xSettings.QuantizerMatrix))
                            xSettings.QuantizerMatrix = substitutionTable[xSettings.QuantizerMatrix];
                    }
                }
                if (vProf.Settings is xvidSettings)
                {
                    xvidSettings xSettings = vProf.Settings as xvidSettings;
                    if (xSettings.QuantType == 2) // CQM
                    {
                        if (substitutionTable.ContainsKey(xSettings.CustomQuantizerMatrix))
                            xSettings.CustomQuantizerMatrix = substitutionTable[xSettings.CustomQuantizerMatrix];
                    }
                }
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
