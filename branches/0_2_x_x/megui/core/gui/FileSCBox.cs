using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public class FileSCBox : StandardAndCustomComboBox
    {
        public FileSCBox() : base("Clear user-selected files...", "Select file...")
        {
            base.Getter = new Getter<object>(getter);
        }

        OpenFileDialog ofd = new OpenFileDialog();

        private object getter()
        {
            if (ofd.ShowDialog() == DialogResult.OK)
                return ofd.FileName;
            return null;
        }

        public string Filter
        {
            get { return ofd.Filter; }
            set { ofd.Filter = value; }
        }

    }
}
