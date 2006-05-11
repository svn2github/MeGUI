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

namespace MeGUI
{
    public partial class ProfilePorter : Form
    {
        public const int BUFFER_SIZE = 2 * 1024 * 1024; // 2 MBs
        private ProfileManager profiles;
        private MeGUI mainForm;
        private bool importMode;

        private ProfileManager importedProfiles;
        private string inputFileName = null;
        private ZipFile inputFile;
        private Dictionary<string, ZipEntry> extraFiles;

        public ProfilePorter(ProfileManager profiles, bool importMode, MeGUI mainForm)
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
        }

        public ProfilePorter(ProfileManager profiles, string file, MeGUI mainForm)
            : this(profiles, true, mainForm)
        {
            this.inputFileName = file;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (importMode)
            {
                IList list_selectedProfiles = profileListBox.SelectedItems;
                int i = 0;
                string[] selectedProfiles = new string[list_selectedProfiles.Count];
                foreach (object o in list_selectedProfiles)
                {
                    selectedProfiles[i] = (string)o;
                    i++;
                }

                List<string> failedProfiles ;
                Profile[] profileList = importedProfiles.AllProfiles(selectedProfiles, out failedProfiles);
                
                Dictionary<string, string> substitutionTable = new Dictionary<string, string>();
                Dictionary<string, string> fileList = importedProfiles.AllRequiredFiles(profileList, out substitutionTable);

                foreach (string file in fileList.Values)
                {
                    if (!extraFiles.ContainsKey(file))
                        failedProfiles.Add(file);
                    else
                    {
                        string filename = Path.GetFileName(file);
                        string pathname = mainForm.MeGUIPath + "\\extra\\" + filename;
                        int count = 0;
                        while (File.Exists(pathname))
                        {
                            count++;
                            pathname = mainForm.MeGUIPath + "\\extra\\" + count + "_" + filename;
                        }
                        if (substitutionTable.ContainsKey(file))
                            substitutionTable.Remove(file);
                        substitutionTable.Add(file, pathname);
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
                }
                ProfileManager.FixFileNames(profileList, substitutionTable);
                profiles.AddAll(profileList, mainForm.DialogManager);
                this.Close();
            }
            else // export mode
            {
                SaveFileDialog outputFilesChooser = new SaveFileDialog();
                outputFilesChooser.Title = "Choose your output file";
                outputFilesChooser.Filter = "Zip archives|*.zip";
                if (outputFilesChooser.ShowDialog() != DialogResult.OK)
                    return;
                string filename = outputFilesChooser.FileName;

                IList list_selectedProfiles = profileListBox.SelectedItems;
                int i = 0;
                string[] selectedProfiles = new string[list_selectedProfiles.Count];
                foreach (object o in list_selectedProfiles)
                {
                    selectedProfiles[i] = (string)o;
                    i++;
                }
                List<string> failedProfiles;
                Profile[] profileList = profiles.AllProfiles(selectedProfiles, out failedProfiles);
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
                    ZipEntry newEntry = new ZipEntry(ProfileManager.ProfilePath(prof.Name, prof.GetType(), "").TrimStart(new char[] { '\\' }));
                    outputFile.PutNextEntry(newEntry);
                    XmlSerializer outputter = new XmlSerializer(prof.GetType());
                    outputter.Serialize(outputFile, prof);
                }
                outputFile.Close();
                MessageBox.Show("Completed successfully", "Export completed successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private static void copyData(Stream input, Stream output)
        {
            int count = -1;
            byte[] data = new byte[BUFFER_SIZE];
            while ((count = input.Read(data, 0, BUFFER_SIZE)) > 0)
            {
                output.Write(data, 0, count);
            }
        }

        private void ProfileExporter_Load(object sender, EventArgs e)
        {
            if (!importMode)
            {
                profileListBox.DataSource = profiles.AllProfileNames;
                return;
            }

            extraFiles = new Dictionary<string, ZipEntry>();
            if (inputFileName == null || inputFileName.Length == 0)
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
            
            importedProfiles = new ProfileManager("");
            XmlSerializer videoSerializer, audioSerializer, avsSerializer, oneclickSerializer;
            videoSerializer = new XmlSerializer(typeof(VideoProfile));
            audioSerializer = new XmlSerializer(typeof(AudioProfile));
            avsSerializer = new XmlSerializer(typeof(AviSynthProfile));
            oneclickSerializer = new XmlSerializer(typeof(OneClickProfile));

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
                               (VideoProfile)videoSerializer.Deserialize(inputFile.GetInputStream(entry)));
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
                            importedProfiles.AddAudioProfile(
                               (AudioProfile)audioSerializer.Deserialize(inputFile.GetInputStream(entry)));
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
                            importedProfiles.AddAviSynthProfile(
                               (AviSynthProfile)avsSerializer.Deserialize(inputFile.GetInputStream(entry)));
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
                            importedProfiles.AddOneClickProfile(
                               (OneClickProfile)oneclickSerializer.Deserialize(inputFile.GetInputStream(entry)));
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
            profileListBox.DataSource = importedProfiles.AllProfileNames;
        }
    }
}