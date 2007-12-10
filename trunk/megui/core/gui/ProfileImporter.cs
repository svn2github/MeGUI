using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using MeGUI.core.util;
using System.IO;

namespace MeGUI.core.gui
{
    public partial class ProfileImporter : MeGUI.core.gui.ProfilePorter
    {
        private static string askForZipFile()
        {
            OpenFileDialog inputChooser = new OpenFileDialog();
            inputChooser.Filter = "Zip archives|*.zip";
            inputChooser.Title = "Select your input file";

            if (inputChooser.ShowDialog() != DialogResult.OK)
                throw new CancelledException();
            
            return inputChooser.FileName;
        }

        private DirectoryInfo tempFolder;
        private DirectoryInfo extraFiles;
        private MainForm mainForm;

        public ProfileImporter(MainForm mf)
            :this(mf, askForZipFile())
        {}

        public ProfileImporter(MainForm mf, string filename)
            : this(mf, File.OpenRead(filename))
        {}

        public ProfileImporter(MainForm mf, Stream s)
        {
            InitializeComponent();
            
            mainForm = mf;

            tempFolder = FileUtil.CreateTempDirectory();
            FileUtil.ExtractZipFile(s, tempFolder.FullName);

            extraFiles = FileUtil.ensureDirectoryExists(Path.Combine(tempFolder.FullName, "extra"));
            List<Profile> ps = ProfileManager.ReadAllProfiles(tempFolder.FullName);
            fixFileNames(ps, createInitSubTable());

            Profiles = ps.ToArray();
        }

        private Dictionary<string, string> createInitSubTable()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            foreach (FileInfo f in extraFiles.GetFiles())
                d[getZippedExtraFileName(f.Name)] = f.FullName;

            return d;
        }

        private List<string> extraFilesList
        {
            get { return new List<FileInfo>(extraFiles.GetFiles()).ConvertAll<string>(delegate(FileInfo f) { return f.FullName; }); }
        }

        private void import_Click(object sender, EventArgs e)
        {
            Util.CatchExceptionsAndTellUser("Error importing file", delegate
            {
                List<Profile> ps = SelectedAndRequiredProfiles;
                fixFileNames(ps,
                    copyExtraFilesToFolder(extraFilesList, Path.Combine(mainForm.MeGUIPath, "extra")));

                mainForm.Profiles.AddAll(ps.ToArray(), mainForm.DialogManager);

                DialogResult = DialogResult.OK;
            });
        }
    }

    public class CancelledException : MeGUIException
    {
        public CancelledException() : base("User cancelled") { }
    }
}

