using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class InputBox : Form
    {
        private InputBox()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        internal static string Show(string message, string title, string defaultText)
        {
            using (InputBox box = new InputBox())
            {
                box.lblMessage.Text = message;
                box.text.Text = defaultText;
                box.Text = title;
                if (box.ShowDialog() == DialogResult.OK)
                    return box.text.Text;
                return null;
            }
        }
    }
}