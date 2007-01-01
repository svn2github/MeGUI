using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Windows.Forms;
using MeGUI.core.util;

/*                    try
                    {
                    }
                    catch (IOException ioe)
                    {
                        MessageBox.Show(
                            string.Format("Error opening selected file: {0}{1}{0}Import cancelled", Environment.NewLine, ioe.Message),
                            "Error opening file",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
*/
namespace MeGUI
{
    public partial class ProfilePorter : Form
    {
        private ProfileManager profiles;
        private MainForm mainForm;
        private bool importMode;
        private string path;

        private ProfileManager importedProfiles;
        private string inputFileName = null;
        private ZipFile inputFile;
        private ZipOutputStream outputFile;
        private Dictionary<string, ZipEntry> extraFiles;

        public ProfilePorter(ProfileManager profiles, MainForm mainForm, byte[] data)
            : this (profiles, true, mainForm)
        {
            inputFile = new ZipFile(new MemoryStream(data));
        }

        public ProfilePorter(ProfileManager profiles, bool importMode, MainForm mainForm)
        {
            InitializeComponent();
            this.importMode = importMode;
            if (importMode)
            {
                okButton.Text = "Import";
                this.Text = "Profile Importer";
                label1.Text = "Select the profiles you want to import (Ctrl or Shift might help):";
            }
            this.profiles = profiles;
            this.mainForm = mainForm;
            this.path = Path.Combine(mainForm.MeGUIPath, "ProfilePorter.temp");
            try 
            {
                // This will fail unless the folder is inexistant or empty
                if (Directory.Exists(path)) Directory.Delete(path);
                Directory.CreateDirectory(path);
            }
            catch (IOException ioe) {
                throw new Exception("Could not start porter because of inability to access " + path, ioe);
            }
        }

        public ProfilePorter(ProfileManager profiles, string file, MainForm mainForm)
            : this(profiles, true, mainForm)
        {
            this.inputFileName = file;
        }

        private void moveProfiles(ProfileManager from, ProfileManager to)
        {
            // Gather the list of selected profiles and convert them to strings
            IList list_selectedProfiles = profileListBox.SelectedItems;
            int i = 0;
            string[] selectedProfiles = new string[list_selectedProfiles.Count];
            foreach (object o in list_selectedProfiles)
            {
                selectedProfiles[i] = (string)o;
                i++;
            }

            // List the profiles to be imported
            Profile[] profileList = from.AllProfiles(selectedProfiles);
            // List the extra files to be copied
            List<string> fileList = from.AllRequiredFiles(profileList);
            // Copy the extra files, generating a list of the new files
            Dictionary<string, string> subTable = copyExtraFiles(fileList);
            // Update the profiles to show the new files
            ProfileManager.FixFileNames(profileList, subTable);
            //Add the profiles to 'to'
            to.AddAll(profileList, mainForm.DialogManager);

            this.Close();
        }

        /// <summary>
        /// Copies the extra files needed by the profiles to the relevant place. Returns a substitution table
        /// for the new locations of the extra files
        /// </summary>
        /// <param name="fileList"></param>
        private Dictionary<string, string> copyExtraFiles(List<string> fileList)
        {
            Dictionary<string, string> substitutionTable = new Dictionary<string, string>();
            if (importMode)
            {
                // These files come from a *.zip file and move to MeGUIPath/extra
                /* If there's already a file there with the same name, overwrite it -- 
                 * it's probably an older version of the same file. It is the user's responsibility
                 * to make sure that different 'extra' files have different names. (The other alternative
                 * is that we generate a new name if it already exists. The problem is that we do mostly
                 * actually want to overwrite it, and if we don't, we end up with multiple copies of the
                 * same file, under names like file.cfg, 0_file.cfg, 1_file.cfg */
                
                if (!Directory.Exists(mainForm.MeGUIPath + "\\extra"))
                    Directory.CreateDirectory(mainForm.MeGUIPath + "\\extra");

                foreach (string file in fileList)
                {
                    string filename = Path.GetFileName(file);
                    string pathname = mainForm.MeGUIPath + "\\extra\\" + filename;
                    substitutionTable[file] = pathname;
                        
                    // Copy the file
                    File.Move(Path.Combine(path, file), pathname);
                }
            }
            else // Export mode
            {
                List<string> addedFilenames = new List<string>();
                foreach (string file in fileList)
                {
                    string zipFileName = FileUtil.getUniqueFilename("extra\\" + Path.GetFileName(file), addedFilenames);
                    addedFilenames.Add(zipFileName);
                    ZipEntry newEntry = new ZipEntry(zipFileName);
                    outputFile.PutNextEntry(newEntry);
                    FileStream input = File.OpenRead(file);
                    FileUtil.copyData(input, outputFile);
                    input.Close();
                    substitutionTable[file] = zipFileName;
                }
            }
            return substitutionTable;
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            if (importMode)
                moveProfiles(importedProfiles, profiles);
            else
            {
                // Choose output filename
                SaveFileDialog outputFilesChooser = new SaveFileDialog();
                outputFilesChooser.Title = "Choose your output file";
                outputFilesChooser.Filter = "Zip archives|*.zip";
                if (outputFilesChooser.ShowDialog() != DialogResult.OK)
                    return;
                string filename = outputFilesChooser.FileName;
                
                // This outputfile is accessed by moveProfiles, so must be initialised beforehand
                outputFile = new ZipOutputStream(File.OpenWrite(filename));

                ProfileManager to = new ProfileManager(path);
                moveProfiles(profiles, to);
                to.SaveProfiles();

                foreach (string file in FileUtil.AllFiles(path))
                {
                    ZipEntry newEntry = new ZipEntry(file.Substring(path.Length).TrimStart('\\', '/'));
                    outputFile.PutNextEntry(newEntry);
                    FileStream input = File.OpenRead(file);
                    FileUtil.copyData(input, outputFile);
                    input.Close();
                }
                outputFile.Close();
                MessageBox.Show("Completed successfully", "Export completed successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
#if UNUSED
            if (importMode)
            {
                // Gather the list of selected profiles and convert them to strings
                IList list_selectedProfiles = profileListBox.SelectedItems;
                int i = 0;
                string[] selectedProfiles = new string[list_selectedProfiles.Count];
                foreach (object o in list_selectedProfiles)
                {
                    selectedProfiles[i] = (string)o;
                    i++;
                }

                // List the profiles to be imported
                Profile[] profileList = importedProfiles.AllProfiles(selectedProfiles);
                // List the extra files to be copied
                List<string> fileList = importedProfiles.AllRequiredFiles(profileList);

                // Copy the files and store where they have been moved to (in the substitution table)
                #region copying
                Dictionary<string, string> substitutionTable = new Dictionary<string, string>();
                foreach (string file in fileList)
                {
                    string filename = Path.GetFileName(file);
                    string pathname = mainForm.MeGUIPath + "\\extra\\" + filename;
                    int count = 0;
                    while (File.Exists(pathname))
                    {
                        pathname = mainForm.MeGUIPath + "\\extra\\" + count + "_" + filename;
                        count++;
                    }
                    substitutionTable[file] = pathname;
                    /*                    if (substitutionTable.ContainsKey(file))
                                            substitutionTable.Remove(file);
                                        substitutionTable.Add(file, pathname);*/
                    try
                    {
                        if (!Directory.Exists(mainForm.MeGUIPath + "\\extra"))
                            Directory.CreateDirectory(mainForm.MeGUIPath + "\\extra");
                        Stream outputStream = File.OpenWrite(pathname);
                        copyData(inputFile.GetInputStream(extraFiles[file]), outputStream);
                        outputStream.Close();
                    }
                    catch (IOException ioe)
                    {
                        MessageBox.Show(
                            string.Format("Error opening selected file: {0}{1}{0}Import cancelled", Environment.NewLine, ioe.Message),
                            "Error opening file",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
                #endregion

                // Fix up the filenames with the substitution table
                ProfileManager.FixFileNames(profileList, substitutionTable);
                // Add the profiles to the main manager
                profiles.AddAll(profileList, mainForm.DialogManager);
                this.Close();
            }
            else // export mode
            {
                // Choose output filename
                SaveFileDialog outputFilesChooser = new SaveFileDialog();
                outputFilesChooser.Title = "Choose your output file";
                outputFilesChooser.Filter = "Zip archives|*.zip";
                if (outputFilesChooser.ShowDialog() != DialogResult.OK)
                    return;
                string filename = outputFilesChooser.FileName;

                // List profile names
                IList list_selectedProfiles = profileListBox.SelectedItems;
                int i = 0;
                string[] selectedProfiles = new string[list_selectedProfiles.Count];
                foreach (object o in list_selectedProfiles)
                {
                    selectedProfiles[i] = (string)o;
                    i++;
                }
                // List profiles
                Profile[] profileList = profiles.AllProfiles(selectedProfiles);
                Dictionary<string, string> substitutionTable = new Dictionary<string, string>();
                Dictionary<string, string> fileList = profiles.AllRequiredFiles(profileList, out substitutionTable);

                ProfileManager.FixFileNames(profileList, substitutionTable);

                ZipOutputStream outputFile = new ZipOutputStream(File.OpenWrite(filename));

                foreach (string zipFilename in fileList.Keys)
                {
                    ZipEntry newEntry = new ZipEntry(zipFilename);
                    outputFile.PutNextEntry(newEntry);
                    FileStream input = File.OpenRead(fileList[zipFilename]);
                    copyData(input, outputFile);
                    input.Close();
                }

                foreach (Profile prof in profileList)
                {
                    ZipEntry newEntry = new ZipEntry(ProfileManager.ProfilePath(prof, "").TrimStart(new char[] { '\\' }));
                    outputFile.PutNextEntry(newEntry);
                    XmlSerializer outputter = new XmlSerializer(prof.GetType());
                    outputter.Serialize(outputFile, prof);
                }
                outputFile.Close();
                MessageBox.Show("Completed successfully", "Export completed successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
#endif           
        }

        protected override void OnClosed(EventArgs e)
        {
            try { Directory.Delete(path, true); }
            catch { }
            base.OnClosed(e);
        }



        private void ProfileExporter_Load(object sender, EventArgs e)
        {
            if (!importMode)
            {
                profileListBox.DataSource = profiles.AllProfileNames;
                return;
            }

            if (inputFile == null)
            {
                if (string.IsNullOrEmpty(inputFileName))
                {
                    OpenFileDialog inputChooser = new OpenFileDialog();
                    inputChooser.Filter = "Zip archives|*.zip";
                    inputChooser.Title = "Select your input file";

                    if (inputChooser.ShowDialog() != DialogResult.OK)
                    {
                        Close();
                        return;
                    }
                    inputFileName = inputChooser.FileName;
                }
                try
                {
                    inputFile = new ZipFile(File.OpenRead(inputFileName));
                }
                catch (IOException ioe)
                {
                    MessageBox.Show(
                        string.Format("Error opening selected file: {0}{1}{0}Import cancelled", Environment.NewLine, ioe.Message),
                        "Error opening file",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
            }

            foreach (ZipEntry entry in inputFile)
            {
                string pathname = Path.Combine(path, entry.Name);
                if (entry.IsDirectory)
                {
                    Directory.CreateDirectory(pathname);
                }
                else // entry.isFile
                {
                    System.Diagnostics.Debug.Assert(entry.IsFile);
                    FileUtil.ensureDirectoryExists(Path.GetDirectoryName(pathname));
                    Stream outputStream = File.OpenWrite(pathname);
                    FileUtil.copyData(inputFile.GetInputStream(entry), outputStream);
                    outputStream.Close();
                }
            }
            inputFile.Close();
            inputFile = null;

            importedProfiles = new ProfileManager(path);
            importedProfiles.LoadProfiles();
            #region old code
            /*
            XmlSerializer videoSerializer, audioSerializer, avsSerializer, oneclickSerializer;
            videoSerializer = new XmlSerializer(typeof(GenericProfile<VideoCodecSettings>));
            audioSerializer = new XmlSerializer(typeof(GenericProfile<AudioCodecSettings>));
            avsSerializer = new XmlSerializer(typeof(GenericProfile<AviSynthSettings>));
            oneclickSerializer = new XmlSerializer(typeof(GenericProfile<OneClickSettings>));

#warning We are generating a list of failed attempts, but we aren't doing anything with it (below).
            List<string> failedEntries = new List<string>(); 
            foreach (ZipEntry entry in inputFile)
            {
                // Check if the entry is in the video/audio/avs/oneclick/extra folders.
                // Try to deserialize if it is a profile, list the files in the extra folders
                #region terribly boring cases
                if (entry.IsFile)
                {
                    if (entry.Name.ToLower().StartsWith("profiles\\video")
                        || entry.Name.ToLower().StartsWith("profiles/video"))
                    {
                        try
                        {
                            importedProfiles.AddVideoProfile(
                               (GenericProfile<VideoCodecSettings>)videoSerializer.Deserialize(inputFile.GetInputStream(entry)));
                        }
                        catch (Exception)
                        {
                            failedEntries.Add(entry.Name);
                        }
                    }

                    else if (entry.Name.ToLower().StartsWith("profiles\\audio")
                        || entry.Name.ToLower().StartsWith("profiles/audio"))
                    {
                        try
                        {
                            importedProfiles.AddProfile(
                               (GenericProfile<AudioCodecSettings>)audioSerializer.Deserialize(inputFile.GetInputStream(entry)));
                        }
                        catch (Exception)
                        {
                            failedEntries.Add(entry.Name);
                        }
                    }

                    else if (entry.Name.ToLower().StartsWith("profiles\\avs")
                        || entry.Name.ToLower().StartsWith("profiles/avs"))
                    {
                        try
                        {
                            importedProfiles.AddProfile(
                               (GenericProfile<AviSynthSettings>)avsSerializer.Deserialize(inputFile.GetInputStream(entry)));
                        }
                        catch (Exception)
                        {
                            failedEntries.Add(entry.Name);
                        }
                    }

                    else if (entry.Name.ToLower().StartsWith("profiles\\oneclick")
                        || entry.Name.ToLower().StartsWith("profiles/oneclick"))
                    {
                        try
                        {
                            importedProfiles.AddProfile(
                               (GenericProfile<OneClickSettings>)oneclickSerializer.Deserialize(inputFile.GetInputStream(entry)));
                        }
                        catch (Exception)
                        {
                            failedEntries.Add(entry.Name);
                        }
                    }
                    else if (entry.Name.ToLower().StartsWith("extra\\")
                        || entry.Name.ToLower().StartsWith("extra/"))
                    {
                        extraFiles.Add(entry.Name, entry);
                    }
                }
                #endregion
            }
            */
            #endregion
            profileListBox.DataSource = importedProfiles.AllProfileNames;
        }
    }
}