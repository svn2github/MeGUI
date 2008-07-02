using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class NumberChooser : Form
    {
        private NumberChooser()
        {
            InitializeComponent();
        }



        public static DialogResult ShowDialog(string message, string title, 
            int decimals, decimal min, decimal max,
            decimal defaultNum, out decimal number)
        {
            NumberChooser n = new NumberChooser();
            n.Text = title;
            n.label1.Text = message;
            n.numericUpDown1.DecimalPlaces = decimals;
            n.numericUpDown1.Minimum = min;
            n.numericUpDown1.Maximum = max;
            n.numericUpDown1.Value = defaultNum;

            DialogResult r = n.ShowDialog();
            number = n.numericUpDown1.Value;
            return r;
        }

        private void NumberChooser_Shown(object sender, EventArgs e)
        {
            numericUpDown1.Select(0, stringLength);
            numericUpDown1.Focus();

        }

        private int stringLength
        {
            get
            {
                return Math.Round(numericUpDown1.Value, 0).ToString().Length + 
                    (numericUpDown1.DecimalPlaces > 0 ? 1 : 0) + 
                    numericUpDown1.DecimalPlaces;
            }
        }

    }
}