using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.gui;
using MeGUI.core.util;
using MeGUI.packages.reader.vstrip;

namespace MeGUI.packages.tools.dvddecrypter
{
    public partial class DVDDecrypterTool : Form
    {
        public DVDDecrypterTool()
        {
            InitializeComponent();

            DriveInfo[] drives = Array.FindAll(DriveInfo.GetDrives(),
                delegate(DriveInfo i) { return i.DriveType == DriveType.CDRom; });
            driveLetter.Items.AddRange(drives);
            if (drives.Length == 0)
            {
                MessageBox.Show("No DVD drives found. DVD Decrypter cannot run", "No DVD drives found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Abort;
                return;
            }
            driveLetter.SelectedIndex = 0;
        }

        internal DVDJob[] Jobs
        {
            get
            {
                List<DVDJob> jobs = new List<DVDJob>();
                string driveletter = this.driveLetter.SelectedItem.ToString();
                string outputfolder = this.outputFolder.Filename;


                foreach (object o in pgcs.CheckedItems)
                {
                    Named<Tuple<TimeSpan, int, int>> n = (Named<Tuple<TimeSpan, int, int>>)o;
                    jobs.Add(new DVDJob(driveletter, outputfolder, n.Data.b, n.Data.c));
                }

                return jobs.ToArray();
            }
            set
            {
                throw new Exception();
            }
        }

        private void driveLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriveInfo d = (DriveInfo)driveLetter.SelectedItem;
            try
            {
                volumeLabel.Text = d.VolumeLabel;
                pgcs.Items.AddRange(getPGCs());
                selectMultiPGCs();
            }
            catch (IOException)
            {
                volumeLabel.Text = "unknown";
                pgcs.Items.Clear();
            }
        }

        private string selectedDrive
        {
            get { return driveLetter.SelectedItem.ToString(); }
        }

        private object[] getPGCs()
        {
            string[] ifos = Directory.GetFiles(Path.Combine(selectedDrive, "VIDEO_TS"), "VTS_??_0.IFO");
            List<Named<Tuple<TimeSpan, int, int>>> pgcs = new List<Named<Tuple<TimeSpan, int, int>>>();
            foreach (string s in ifos)
            {
                int vts;
                try
                {
                    vts = int.Parse(s.Substring(s.Length - 8, 2));
                }
                catch (FormatException) { continue; }

                DVDFile d = DVDFile.Open(s);
                int pgc = 1;
                foreach (PGC p in d.PGCs)
                {
                    pgcs.Add(new Named<Tuple<TimeSpan, int, int>>(
                        string.Format("VTS {0} PGC {1}: {2}", vts, pgc, Util.ToString(p.Length)),
                        new Tuple<TimeSpan, int, int>(p.Length, vts, pgc)));
                    pgc++;
                }
            }
            return pgcs.ToArray();
        }

        private void selectOnePGC()
        {
            if (pgcs.Items.Count == 0)
                return;

            TimeSpan maxLength = TimeSpan.Zero;
            int maxIndex = -1;
            int i = 0;
            foreach (object o in pgcs.Items)
            {
                Named<Tuple<TimeSpan, int, int>> pgc = (Named<Tuple<TimeSpan, int, int>>)o;
                if (pgc.Data.a > maxLength)
                {
                    maxLength = pgc.Data.a;
                    maxIndex = i;
                }

                ++i;
            }
        }

        private int selectMultiPGCs()
        {
            TimeSpan oneHour = new TimeSpan(1, 0, 0);
            TimeSpan tenMinutes = new TimeSpan(0, 10, 0);
            List<int> pgcsOverTenMinutes = new List<int>();
            List<int> pgcsOverOneHour = new List<int>();

            int index = 0;
            foreach (object o in pgcs.Items)
            {
                Named<Tuple<TimeSpan, int, int>> pgc = (Named<Tuple<TimeSpan, int, int>>)o;
                if (pgc.Data.a > oneHour)
                    pgcsOverOneHour.Add(index);
                if (pgc.Data.a > tenMinutes)
                    pgcsOverTenMinutes.Add(index);
                ++index;
            }

            if (pgcsOverOneHour.Count > 0)
            {
                select(pgcsOverOneHour.ToArray());
                return pgcsOverOneHour.Count;
            }
            else if (pgcsOverTenMinutes.Count > 0)
            {
                select(pgcsOverTenMinutes.ToArray());
                return pgcsOverTenMinutes.Count;
            }
            else
            {
                selectOnePGC();
                return 1;
            }
        }

        private void select(params int[] p)
        {
            for (int j = 0; j < pgcs.Items.Count; ++j)
                pgcs.SetItemChecked(j, Array.IndexOf(p, j) > -1);
        }

        private void queueButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(outputFolder.Filename) || !Directory.Exists(outputFolder.Filename))
            {
                MessageBox.Show("You must select a valid output folder", "Invalid output folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pgcs.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must select at least one PGC", "No PGCs selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }


    public class DVDDecrypterFactory : ITool
    {
        #region ITool Members

        string ITool.Name
        {
            get { return "DVD Decrypter"; }
        }

        void ITool.Run(MainForm info)
        {
            DVDDecrypterTool d = new DVDDecrypterTool();
            if (d.ShowDialog() == DialogResult.OK)
                info.Jobs.addJobsToQueue(d.Jobs);
        }

        Shortcut[] ITool.Shortcuts
        {
            get { return new Shortcut[] { Shortcut.Ctrl0 }; }
        }

        #endregion

        #region IIDable Members

        string IIDable.ID
        {
            get { return (this as ITool).Name; }
        }

        #endregion
    }
}