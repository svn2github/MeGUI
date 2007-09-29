using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class CQMComboBox : MeGUI.core.gui.NiceComboBox
    {
        private NiceComboBoxNormalItem clearItem;

        public CQMComboBox() : base()
        {
            InitializeComponent();
            
            clearItem = new NiceComboBoxNormalItem(
                "Clear user-selected files...",
                delegate(NiceComboBoxNormalItem i, EventArgs e)
                {
                    clearCustomCQMs();
                });

            Items.Add(new NiceComboBoxNormalItem(
                "Select file...", null,
                delegate(NiceComboBoxNormalItem i, EventArgs e)
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        SelectCQM(ofd.FileName);
                    }
                }));
        }

        public string Filter
        {
            get { return ofd.Filter; }
            set { ofd.Filter = value; }
        }

        int numStandardCQMs, numCustomCQMs;

        public void AddStandardCQM(string name)
        {
            if (numStandardCQMs == 0)
                Items.Insert(0, new NiceComboBoxSeparator());

            Items.Insert(numStandardCQMs, 
                new NiceComboBoxNormalItem(new CQMName(name, true)));
            numStandardCQMs++;
        }

        private void AddCustomCQM(string name)
        {
            if (numCustomCQMs == 0)
            {
                Items.Insert(Items.Count - 1, new NiceComboBoxSeparator());
                Items.Insert(Items.Count - 1, clearItem);
            }

            Items.Insert(Items.Count - 3, 
                new NiceComboBoxNormalItem(new CQMName(name, false)));
            numCustomCQMs++;
        }

        private void clearCustomCQMs()
        {
            int start = getCustomCQMStart();

            if (SelectedIndex >= start)
                SelectedIndex = 0;

            Items.RemoveRange(start, numCustomCQMs);
            if (numCustomCQMs > 0)
            {
                Items.RemoveAt(start);
                Items.RemoveAt(start);
            }

            numCustomCQMs = 0;
        }

        private void clearStandardCQMs()
        {
            if (numStandardCQMs == 0) return;

            Items.RemoveRange(0, numStandardCQMs + 1);
            numStandardCQMs = 0;
        }

        private int getCustomCQMStart()
        {
            if (numStandardCQMs > 0)
                return numStandardCQMs + 1;
            return 0;
        }

        public string[] CustomCQMs
        {
            get
            {
                string[] res = new string[numCustomCQMs];
                int start = getCustomCQMStart();

                for (int i = 0; i < numCustomCQMs; ++i)
                {
                    res[i] = Items[start+i].Name;
                }
                return res;
            }
            set
            {
                clearCustomCQMs();
                foreach (string s in value)
                    AddCustomCQM(s);
            }
        }

        public void SelectCQM(string CQM)
        {
            if (string.IsNullOrEmpty(CQM))
                return;

            foreach (NiceComboBoxItem i in Items)
            {
                if (i.Name == CQM)
                {
                    SelectedItem = i;
                    return;
                }
            }
            AddCustomCQM(CQM);
            SelectCQM(CQM);
        }

        public string[] StandardCQMs
        {
            get
            {
                string[] res = new string[numStandardCQMs];

                for (int i = 0; i < numStandardCQMs; ++i)
                {
                    res[i] = Items[i].Name;
                }
                return res;
            }
            set {
                clearStandardCQMs();
                foreach (string s in value)
                    AddStandardCQM(s);
            }
        }

    }

    public class CQMName
    {
        string Name;
        bool IsStandard;

        public CQMName(string name, bool isStandard)
        {
            Name = name;
            IsStandard = isStandard;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

