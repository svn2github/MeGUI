using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MeGUI
{
    public class ProfileManager
    {
        string path;
        Dictionary<string, AudioProfile> audioProfiles;
        Dictionary<string, AviSynthProfile> avsProfiles;
        Dictionary<string, OneClickProfile> oneClickProfiles;
        Dictionary<string, VideoProfile> videoProfiles;

        public ProfileManager(string path)
        {
            this.path = path;
            audioProfiles = new Dictionary<string, AudioProfile>();
            avsProfiles = new Dictionary<string, AviSynthProfile>();
            oneClickProfiles = new Dictionary<string, OneClickProfile>();
            videoProfiles = new Dictionary<string, VideoProfile>();
        } 
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
            return ProfilePath(prof.Name, prof.GetType());
        }

        public string ProfilePath(string profName, Type type)
        {
            return ProfilePath(profName, type, this.path);
        }

        public static string ProfilePath(string profName, Type type, string path)
        {
            if (!path.EndsWith("\\") && !path.EndsWith("/"))
            {
                if (path.Length >= 0)
                    path = path + "\\";
            }
            if (type == typeof(VideoProfile))
                return path + @"profiles\video\" + fixName(profName) + ".xml";
            if (type == typeof(AudioProfile))
                return path + @"profiles\audio\" + fixName(profName) + ".xml";
            if (type == typeof(AviSynthProfile))
                return path + @"profiles\avs\" + fixName(profName) + ".xml";
            if (type == typeof(OneClickProfile))
                return path + @"profiles\oneclick\" + fixName(profName) + ".xml";
            throw new ArgumentException("Unknown type");
        }
    
        #endregion
        #region generic loading and saving
        /// <summary>
        /// saves all the profiles
        /// this is called when the program exists and ensures that all
        /// currently defined profiles are saved, overwriting currently existing ones
        /// </summary>
        public void SaveProfiles()
        {
            foreach (VideoProfile vProf in videoProfiles.Values)
            {
                saveVideoProfile(vProf);
            }
            foreach (AudioProfile aProf in audioProfiles.Values)
            {
                saveAudioProfile(aProf);
            }
            foreach (AviSynthProfile asProf in avsProfiles.Values)
            {
                saveAviSynthProfile(asProf);
            }
            foreach (OneClickProfile oProf in oneClickProfiles.Values)
            {
                saveOneClickProfile(oProf);
            }
        }
        public void LoadProfiles(ComboBox video, ComboBox audio)
        {
            LoadProfiles();
            foreach (VideoProfile vProf in VideoProfiles.Values)
            {
                video.Items.Add(vProf.Name);
            }
            foreach (AudioProfile aProf in AudioProfiles.Values)
            {
                audio.Items.Add(aProf.Name);
            }
        }
        /// <summary>
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
        }
        #endregion
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
                    VideoProfile prof = this.loadVideoProfile(fileName);
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
        private void saveVideoProfile(VideoProfile prof)
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
                    ser = new XmlSerializer(typeof(VideoProfile));
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
        private VideoProfile loadVideoProfile(string name)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    ser = new XmlSerializer(typeof(VideoProfile));
                    return (VideoProfile)ser.Deserialize(s);
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
                string fileName = ProfilePath(name, typeof(VideoProfile));
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
        public bool AddVideoProfile(VideoProfile prof)
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
				    AudioProfile prof = loadAudioProfile(fileName);
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
		private void saveAudioProfile(AudioProfile prof)
		{
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
			XmlSerializer ser = null;
			string fileName = ProfilePath(prof);
			using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
			{
				try
				{
					ser = new XmlSerializer(typeof(AudioProfile));
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
		private AudioProfile loadAudioProfile(string name)
		{
			XmlSerializer ser = null;
			using (Stream s = File.OpenRead(name))
			{
				try
				{
					ser = new XmlSerializer(typeof(AudioProfile));
					return (AudioProfile)ser.Deserialize(s);
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
        public bool AddAudioProfile(AudioProfile prof)
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
        public bool AddAviSynthProfile(AviSynthProfile prof)
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
                string fileName = ProfilePath(name, typeof(AviSynthProfile));
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
                AviSynthProfile prof = loadAviSynthProfile(fileName);
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
            if (!this.avsProfiles.ContainsKey(new AviSynthProfile().Name))
                this.avsProfiles.Add(new AviSynthProfile().Name, new AviSynthProfile());
        }
        /// <summary>
        /// saves a given AviSynth profile to the profile path
        /// </summary>
        /// <param name="prof">the profile to be saved</param>
        private void saveAviSynthProfile(AviSynthProfile prof)
        {
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
            XmlSerializer ser = null;
            string fileName = ProfilePath(prof);
            using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    ser = new XmlSerializer(typeof(AviSynthProfile));
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
        private AviSynthProfile loadAviSynthProfile(string name)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    ser = new XmlSerializer(typeof(AviSynthProfile));
                    return (AviSynthProfile)ser.Deserialize(s);
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
        public bool AddOneClickProfile(OneClickProfile prof)
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
                string fileName = ProfilePath(name, typeof(OneClickProfile));
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
                OneClickProfile prof = loadOneClickProfile(fileName);
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
        private OneClickProfile loadOneClickProfile(string name)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    ser = new XmlSerializer(typeof(OneClickProfile));
                    return (OneClickProfile)ser.Deserialize(s);
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
        private void saveOneClickProfile(OneClickProfile prof)
        {
            if (prof.Name == "")
                return; // redundant check to eliminate rogue profiles
            XmlSerializer ser = null;
            string fileName = ProfilePath(prof);
            using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    ser = new XmlSerializer(typeof(OneClickProfile));
                    ser.Serialize(s, prof);
                }
                catch (Exception e)
                {
                    MessageBox.Show("One click Profile " + prof.Name + " could not be saved. Error message: " + e.Message, "Error saving profile", MessageBoxButtons.OK);
                }
            }
        }
        #endregion
        #region properties
        public Dictionary<string, AudioProfile> AudioProfiles
        {
            get { return audioProfiles; }
            set { audioProfiles = value; }
        }
        public Dictionary<string, AviSynthProfile> AvsProfiles
        {
            get { return avsProfiles; }
            set { avsProfiles = value; }
        }
        public Dictionary<string, OneClickProfile> OneClickProfiles
        {
            get { return oneClickProfiles; }
            set { oneClickProfiles = value; }
        }
        public Dictionary<string, VideoProfile> VideoProfiles
        {
            get { return videoProfiles; }
            set { videoProfiles = value; }
        }
        #endregion
        #region Profile Listing
        public string[] AllProfileNames
        {
            get
            {
                string[] profileList = new string[AudioProfiles.Count
                    + VideoProfiles.Count + OneClickProfiles.Count
                    + AvsProfiles.Count];

                int i = 0;
                foreach (string key in VideoProfiles.Keys)
                {
                    profileList[i] = "Video: " + key;
                    i++;
                }
                foreach (string key in AudioProfiles.Keys)
                {
                    profileList[i] = "Audio: " + key;
                    i++;
                }
                foreach (string key in AvsProfiles.Keys)
                {
                    profileList[i] = "AviSynth: " + key;
                    i++;
                }
                foreach (string key in OneClickProfiles.Keys)
                {
                    profileList[i] = "OneClick: " + key;
                    i++;
                }
                return profileList;
            }
        }
        public Profile[] AllDependantProfiles(string formattedName)
        {
            try
            {
                if (formattedName.StartsWith("Video: "))
                {
                    return new Profile[] { VideoProfiles[formattedName.Substring(7)].clone() };
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
                    OneClickProfile profile = OneClickProfiles[formattedName.Substring(10)];
                    if (profile.Settings.DontEncodeAudio)
                        return new Profile[] { profile, VideoProfiles[profile.Settings.VideoProfileName].clone() };
                    else
                        return new Profile[] { profile,
                        VideoProfiles[profile.Settings.VideoProfileName].clone(),
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
            if (profile is VideoProfile)
            {
                VideoProfile vProfile = profile as VideoProfile;
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
            if (profile is VideoProfile)
            {
                VideoProfile vProf = profile as VideoProfile;
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
            bool succeeded = false;
            if (prof is VideoProfile)
                succeeded = AddVideoProfile(prof as VideoProfile);
            else if (prof is AudioProfile)
                succeeded = AddAudioProfile(prof as AudioProfile);
            else if (prof is AviSynthProfile)
                succeeded = AddAviSynthProfile(prof as AviSynthProfile);
            else if (prof is OneClickProfile)
                succeeded = AddOneClickProfile(prof as OneClickProfile);
            if (!succeeded)
            {
                if (!asker.overwriteProfile(prof.Name))
                    return;
                else
                {
                    RemoveProfileWithName(prof);
                    Add(prof, asker);
                }
            }
        }
        private void RemoveProfileWithName(Profile prof)
        {
            if (prof is VideoProfile)
                DeleteVideoProfile(prof.Name);
            else if (prof is AudioProfile)
                DeleteAudioProfile(prof.Name);
            else if (prof is AviSynthProfile)
                DeleteAviSynthProfile(prof.Name);
            else if (prof is OneClickProfile)
                DeleteOneClickProfile(prof.Name);
        }
        #endregion

    }
}
