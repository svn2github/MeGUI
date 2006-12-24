using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class AutoUpdateServerConfigWindow : Form
    {
        public AutoUpdateServerConfigWindow()
        {
            InitializeComponent();
        }

        private void addServerButton_Click(object sender, EventArgs e)
        {
            string serverName = Microsoft.VisualBasic.Interaction.InputBox(
                "Please enter the server address", 
                "Please enter the server address",
                "http://yourserver.org/path/to/update/folder/", -1, -1);
            if (serverName == null) return;
            serverName = serverName.Trim();
            if (serverList.Items.Contains(serverName))
            {
                MessageBox.Show("Server already listed. Adding nothing", "Server already listed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!serverName.StartsWith("http://"))
            {
                MessageBox.Show("Only http servers are supported", "Server not http", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            serverList.Items.Add(serverName);
        }

        private void removeSelectedServersButton_Click(object sender, EventArgs e)
        {
            object[] items = new object[serverList.SelectedItems.Count];
            serverList.SelectedItems.CopyTo(items, 0);
            foreach (object o in items)
            {
                serverList.Items.Remove(o);
            }
        }

        public string[] ServerList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (string o in serverList.Items)
                    list.Add(o);
                return list.ToArray();
            }
            set
            {
                serverList.Items.Clear();
                serverList.Items.AddRange(value);
            }
        }
    }
}