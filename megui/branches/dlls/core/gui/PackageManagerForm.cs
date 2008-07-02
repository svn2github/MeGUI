using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI;
using MeGUI.core.details;

namespace MeGUI.core.gui
{
    public partial class PackageManagerForm : Form
    {
        private PackageSystem packages;
        public PackageManagerForm(PackageSystem packages)
        {
            this.packages = packages;
            InitializeComponent();
        }
    }
}