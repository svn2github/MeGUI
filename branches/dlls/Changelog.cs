using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class Changelog : Form
    {
        public Changelog()
        {
            InitializeComponent();
        }

        private void Changelog_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Changelog_Load(object sender, EventArgs e)
        {
            using (System.IO.TextReader r = new System.IO.StreamReader(this.GetType().Assembly.GetManifestResourceStream("MeGUI.Changelog.txt")))
            {
                txtChangelog.Text = r.ReadToEnd();
                txtChangelog.Select(0, 0);
            }
        }
    }
}