using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.util;
using System.IO;

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

        NotifyCounter raiseEvent = new NotifyCounter();

        public string Filename
        {
            get { return filename.Text; }
            set
            {
                using (IDisposable wrapper = raiseEvent.Wrap())
                {
                    filename.Text = value;
                    oldName = value;
                }
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
                    setFilename(dialog.SelectedPath);
            }
            else
            {
                FileDialog dialog = saveMode ?
                    (FileDialog)new SaveFileDialog() :
                    (FileDialog)new OpenFileDialog();

                dialog.Filter = filter;
                dialog.Title = title;
                if (dialog.ShowDialog() == DialogResult.OK)
                    setFilename(dialog.FileName);
            }
        }

        private void setFilename(string filename)
        {
            oldName = this.filename.Text;
            using (IDisposable a = raiseEvent.Wrap())
            {
                this.filename.Text = filename;
            }
            triggerEvent();
        }

        private void triggerEvent()
        {
            if (raiseEvent.Ready && FileSelected != null) FileSelected(this, new FileBarEventArgs(oldName, filename.Text));
            oldName = filename.Text;
        }

        private void filename_TextChanged(object sender, EventArgs e)
        {
            setFilename(filename.Text);
            triggerEvent();
        }

        private void filename_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            
            if (SaveMode)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (files.Length == 1 && FileUtil.MatchesFilter(Filter, files[0]))
                    e.Effect = DragDropEffects.All;
            }
        }

        private void filename_DragDrop(object sender, DragEventArgs e)
        {
            setFilename(((string[])e.Data.GetData(DataFormats.FileDrop, false))[0]);
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
