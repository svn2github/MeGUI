using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class ProfileExporter : MeGUI.core.gui.ProfilePorter
    {
        private MainForm mainForm;
        private DirectoryInfo tempFolder;

        public ProfileExporter(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            Profiles = mainForm.Profiles.AllProfiles;
        }

        private List<string> getRequiredFiles(List<Profile> ps)
        {
            List<string> files = new List<string>();

            foreach (Profile p in ps)
                files.AddRange(p.BaseSettings.RequiredFiles);

            return Util.Unique(files);
        }



        private void export_Click(object sender, EventArgs e)
        {
            Util.CatchExceptionsAndTellUser("An error occurred when saving the file", delegate
            {
                try
                {
                    string filename = askForFilename();

                    tempFolder = FileUtil.CreateTempDirectory();

                    List<Profile> profs = SelectedAndRequiredProfiles;
                    Dictionary<string, string> subTable =
                        copyExtraFilesToFolder(getRequiredFiles(profs),
                        Path.Combine(tempFolder.FullName, "extra"));

                    subTable = turnValuesToZippedStyleName(subTable);

                    fixFileNames(profs, subTable);

                    ProfileManager.WriteProfiles(tempFolder.FullName, profs);

                    FileUtil.CreateZipFile(tempFolder.FullName, filename);

                    DialogResult = DialogResult.OK;
                    MessageBox.Show("Completed successfully", "Export completed successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (CancelledException)
                {
                    DialogResult = DialogResult.Cancel;
                }
            });
        }

        private Dictionary<string, string> turnValuesToZippedStyleName(Dictionary<string, string> subTable)
        {
            Dictionary<string, string> newTable = new Dictionary<string, string>();
            foreach (string key in subTable.Keys)
                newTable[key] = getZippedExtraFileName(subTable[key]);
            return newTable;
        }

        private static string askForFilename()
        {
            SaveFileDialog outputFilesChooser = new SaveFileDialog();
            outputFilesChooser.Title = "Choose your output file";
            outputFilesChooser.Filter = "Zip archives|*.zip";
            if (outputFilesChooser.ShowDialog() != DialogResult.OK)
                throw new CancelledException();

            return outputFilesChooser.FileName;
        }
    }
}


