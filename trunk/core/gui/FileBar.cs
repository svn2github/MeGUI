using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{

    public delegate void FileBarEventHandler(FileBar sender, FileBarEventArgs args);
    
    public partial class FileBar : UserControl
    {
        public FileBar()
        {
            InitializeComponent();
        }

        private bool saveMode = false;
        public bool SaveMode
        {
            get { return saveMode; }
            set { saveMode = value; }
        }

        public bool ReadOnly
        {
            get { return filename.ReadOnly; }
            set { filename.ReadOnly = value; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        public string Filename
        {
            get { return filename.Text; }
            set { filename.Text = value; }
        }

        private bool folderMode;

        public bool FolderMode
        {
            get { return folderMode; }
            set { folderMode = value; }
        }

        private string filter;

        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        public event FileBarEventHandler FileSelected;

        private void openButton_Click(object sender, EventArgs e)
        {
            if (folderMode)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string oldFilename = filename.Text;
                    filename.Text = dialog.SelectedPath;
                    FileSelected(this, new FileBarEventArgs(oldFilename, filename.Text));
                }
            }
            else
            {
                FileDialog dialog;
                if (saveMode)
                {
                    dialog = new SaveFileDialog();
                }
                else
                {
                    dialog = new OpenFileDialog();
                }
                dialog.Filter = filter;
                dialog.Title = title;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string oldFilename = filename.Text;
                    filename.Text = dialog.FileName;
                    FileSelected(this, new FileBarEventArgs(oldFilename, filename.Text));
                }
            }
        }
    }
    public class FileBarEventArgs : EventArgs
    {
        public readonly string OldFileName;
        public readonly string NewFileName;
        public FileBarEventArgs(string oldName, string newName)
            : base()
        {
            OldFileName = oldName;
            NewFileName = newName;
        }
    }

}
