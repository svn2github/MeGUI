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
        private string oldName;
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

        bool raiseEvent = true;

        public string Filename
        {
            get { return filename.Text; }
            set
            {
                raiseEvent = false;
                filename.Text = value;
                oldName = value;
                raiseEvent = false;
            }
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
                    oldName = filename.Text;
                    filename.Text = dialog.SelectedPath;
                    FileSelected(this, new FileBarEventArgs(oldName, filename.Text));
                    oldName = filename.Text;
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
                    oldName = filename.Text;
                    filename.Text = dialog.FileName;
                    if (FileSelected != null) FileSelected(this, new FileBarEventArgs(oldName, filename.Text));
                    oldName = filename.Text;
                }
            }
        }

        private void filename_TextChanged(object sender, EventArgs e)
        {
            if (raiseEvent && FileSelected != null) FileSelected(this, new FileBarEventArgs(oldName, filename.Text));
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
