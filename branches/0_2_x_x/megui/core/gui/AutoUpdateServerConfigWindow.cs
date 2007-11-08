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
        private string[][] serverLists;
        private string oldTitle = null;
        private int oldIndex = -1;

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

        public int ServerListIndex
        {
            get
            {
                return subList.SelectedIndex;
            }
            set
            {
                subList.SelectedIndex = value;
            }
        }

        public string[][] ServerList
        {
            get
            {
                subList_SelectedIndexChanged(null, null);
                return serverLists;
            }
            set
            {
                serverLists = value;
                oldIndex = -1;
                subList.Items.Clear();
                
                foreach (string[] sub in value)
                    subList.Items.Add(sub[0]);

                subList_SelectedIndexChanged(null, null);
            }
        }

        private string[] littleServerList
        {
            get
            {
                List<string> list = new List<string>();
                list.Add(oldTitle);
                foreach (string o in serverList.Items)
                    list.Add(o);
                return list.ToArray();
            }
            set
            {
                oldTitle = value[0];
                serverList.Items.Clear();
                serverList.Items.AddRange(value);
                serverList.Items.RemoveAt(0);
            }
        }

        private void subList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldIndex > -1)
                serverLists[oldIndex] = littleServerList;
            oldIndex = subList.SelectedIndex;
            if (oldIndex > -1) 
                littleServerList = serverLists[subList.SelectedIndex];
        }
    }
}