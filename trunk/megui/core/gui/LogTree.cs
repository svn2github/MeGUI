using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.util;
using System.IO;

namespace MeGUI.core.gui
{
    public partial class LogTree : UserControl
    {
        public LogTree()
        {
            InitializeComponent();

            ImageList i = new ImageList();
            i.Images.Add(System.Drawing.SystemIcons.Error);
            i.Images.Add(System.Drawing.SystemIcons.Warning);
            i.Images.Add(System.Drawing.SystemIcons.Information);
            treeView.ImageList = i;

            Log.SubItemAdded += delegate(object sender, EventArgs<LogItem> args)
            {
                Util.ThreadSafeRun(treeView, delegate { treeView.Nodes.Add(register(args.Data)); });
            };
        }


        public readonly LogItem Log = new LogItem("Log", ImageType.NoImage);


        private TreeNode register(LogItem log)
        {
            List<TreeNode> subNodes = log.SubEvents.ConvertAll<TreeNode>(delegate(LogItem e)
            {
                return register(e);
            });

            TreeNode node = new TreeNode(log.Text, (int)log.Type, (int)log.Type, subNodes.ToArray());
            node.Tag = log;

            log.SubItemAdded += delegate(object sender, EventArgs<LogItem> args)
            {
                Util.ThreadSafeRun(treeView, delegate { node.Nodes.Add(register(args.Data)); });
            };

            log.TypeChanged += delegate(object sender, EventArgs<ImageType> args)
            {
                Util.ThreadSafeRun(treeView, delegate { node.SelectedImageIndex = node.ImageIndex = (int)args.Data; });
            };
            log.Expanded += delegate(object sender, EventArgs e)
            {
                Util.ThreadSafeRun(treeView, delegate { node.Expand(); });
            };
            log.Collapsed += delegate(object sender, EventArgs e)
            {
                Util.ThreadSafeRun(treeView, delegate { node.Collapse(); });
            };

            return node;
        }

        private void ofIndividualNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            show(selectedLogItem, false);
        }

        private void ofBranchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            show(selectedLogItem, true);
        }

        private void editLog_Click(object sender, EventArgs e)
        {
            show(Log, true);
        }

        private LogItem selectedLogItem
        {
            get
            {
                if (treeView.SelectedNode == null)
                    return null;

                return (treeView.SelectedNode.Tag as LogItem);
            }
        }

        private void show(LogItem l, bool subnodes)
        {
            if (l == null)
                return;

            TextViewer t = new TextViewer();
            t.Contents = l.ToString(subnodes);
            t.Wrap = false;
            t.ShowDialog();
        }

        private void saveLog_Click(object sender, EventArgs e)
        {
            save(Log);
        }

        private void saveBranch_Click(object sender, EventArgs e)
        {
            LogItem i = selectedLogItem;
            if (i == null)
            {
                MessageBox.Show("No log branch selected", "Can't save file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            save(i);
        }

        private void save(LogItem i)
        {
            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                File.WriteAllText(saveDialog.FileName, i.ToString());
                MessageBox.Show("File saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch (IOException ie)
            {
                MessageBox.Show("Error saving file: " + ie.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
