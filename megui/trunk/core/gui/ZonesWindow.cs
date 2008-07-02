using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class ZonesWindow : Form
    {
        public ZonesWindow(MainForm mf, string input)
        {
            InitializeComponent();
            zonesControl1.MainForm = mf;
            zonesControl1.Input = input;
        }

        public Zone[] Zones
        {
            get { return zonesControl1.Zones; }
            set { zonesControl1.Zones = value ?? new Zone[0]; }
        }
    }
}
