using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class ConfigableProfilesControl : MeGUI.core.gui.SimpleProfilesControl
    {
        public ConfigableProfilesControl()
        {
            InitializeComponent();
        }

        private void config_Click(object sender, EventArgs e)
        {
            Manager.Configure(SelectedProfile);
            //RefreshProfiles();
        }
    }
}

