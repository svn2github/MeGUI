using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class HelpButton : UserControl
    {
        private static readonly string BaseURL = "http://mewiki.project357.com/wiki/MeGUI:";

        public HelpButton()
        {
            InitializeComponent();
        }

        void HelpButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(BaseURL + articleName.Replace(' ', '_'));
        }

        private string articleName;

        public string ArticleName
        {
            get { return articleName; }
            set { articleName = value; }
        }

    }
}
