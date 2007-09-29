using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MeGUI.core.gui
{
    
    public partial class NiceComboBox : UserControl
    {
        public readonly List<NiceComboBoxItem> Items = new List<NiceComboBoxItem>();

        private NiceComboBoxItem selectedItem;

        public event StringChanged SelectionChanged;

        public int SelectedIndex
        {
            get
            {
                if (selectedItem == null)
                    return -1;
                return (Items.IndexOf(selectedItem));
            }
            set
            {
                if (value == -1)
                    SelectedItem = null;
                else
                    SelectedItem = Items[value];
            }
        }

        public NiceComboBoxItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != null)
                    selectedItem.Ticked = false;
                if (value != null)
                    value.Ticked = true;
                
                selectedItem = value;
                
                if (value != null)
                    textBox1.Text = value.Name;
                else
                    textBox1.Text = "";

                if (SelectionChanged != null)
                    SelectionChanged(this, textBox1.Text);
            }
        }

        public string SelectedText
        {
            get { return textBox1.Text; }
            set
            {
                textBox1.Text = value;
                selectedItem = null;

                if (SelectionChanged != null)
                    SelectionChanged(this, textBox1.Text);
            }
        }

        public NiceComboBox()
        {
            InitializeComponent();
        }

        private void dropDownButton_Click(object sender, EventArgs e)
        {
            ContextMenuStrip s = new ContextMenuStrip();
            s.Items.AddRange(createMenu(Items));
            s.Show(dropDownButton, 0, dropDownButton.Height);
        }

        private ToolStripItem[] createMenu(List<NiceComboBoxItem> items)
        {
            ToolStripItem[] result = new ToolStripItem[items.Count];

            int index = 0;
            foreach (NiceComboBoxItem i in items)
            {
                if (i is NiceComboBoxSeparator)
                    result[index] = new ToolStripSeparator();
                else
                {
                    ToolStripMenuItem t = new ToolStripMenuItem(i.Name);
                    t.Checked = i.Ticked;
                    t.Tag = i;
                    if (i is NiceComboBoxNormalItem)
                        t.Click += new EventHandler(item_Click);
                    else if (i is NiceComboBoxSubMenuItem)
                    {
                        t.DropDownItems.AddRange(
                            createMenu(((NiceComboBoxSubMenuItem)i).SubItems));
                    }
                    else Debug.Assert(false);

                    result[index] = t;
                }
                ++index;
            }
            return result;
        }
        
        void item_Click(object sender, EventArgs e)
        {
            ToolStripItem i = (ToolStripItem)sender;
            NiceComboBoxNormalItem item = (NiceComboBoxNormalItem)i.Tag;
            if (item.Selectable)
                SelectedItem = item;

            item.OnClick();
        }
    }

    public delegate void NiceComboBoxItemClicked(NiceComboBoxNormalItem item, EventArgs e);

    public class NiceComboBoxItem
    {
        public string Name;
        public object Tag;
        public bool Ticked = false;

        public NiceComboBoxItem(string name, object tag)
        {
            Name = name;
            Tag = tag;
        }
    }

    public class NiceComboBoxNormalItem : NiceComboBoxItem
    {
        public bool Selectable = true;
        public event NiceComboBoxItemClicked ItemClicked;

        internal void OnClick()
        {
            if (ItemClicked != null) ItemClicked(this, new EventArgs());
        }

        public NiceComboBoxNormalItem(string name, object tag)
            :base(name,tag)
        {
        }

        public NiceComboBoxNormalItem(string name, object tag, NiceComboBoxItemClicked handler)
            :this(name, tag)
        {
            ItemClicked += handler;
            Selectable = false;
        }

        public NiceComboBoxNormalItem(object stringableObject)
            : this(stringableObject.ToString(), stringableObject) { }

        public NiceComboBoxNormalItem(object stringableObject, NiceComboBoxItemClicked handler)
            : this(stringableObject.ToString(), stringableObject, handler) { }
    }

    public class NiceComboBoxSubMenuItem : NiceComboBoxItem
    {
        public List<NiceComboBoxItem> SubItems;

        public NiceComboBoxSubMenuItem(string name, object tag, params NiceComboBoxItem[] subItems)
            :base(name, tag)
        {
            SubItems = new List<NiceComboBoxItem>(subItems);
        }

        public NiceComboBoxSubMenuItem(object stringableObject, params NiceComboBoxItem[] subItems)
            : this(stringableObject.ToString(), stringableObject, subItems) { }
    }

    public class NiceComboBoxSeparator : NiceComboBoxItem
    {
        public NiceComboBoxSeparator() : base(null, null) { }
    }
}
