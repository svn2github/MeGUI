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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip;

using MeGUI.core.util;

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

        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            for (int i = 0; i < profileList.Items.Count; i++)
            {
                profileList.SetItemChecked(i, true);
            } 
        }

        private void checkNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = (ToolStripMenuItem)sender;
            for (int i = 0; i < profileList.Items.Count; i++)
            {
                profileList.SetItemChecked(i, false);
            } 
        }
    }

    public class CancelledException : MeGUIException
    {
        public CancelledException() : base("User cancelled") { }
    }
}

