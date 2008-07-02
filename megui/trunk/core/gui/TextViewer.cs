using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class TextViewer : Form
    {
        public TextViewer()
        {
            InitializeComponent();
        }

        public string Contents
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public bool Wrap
        {
            get { return textBox1.WordWrap; }
            set { textBox1.WordWrap = value; }
        }
    }
}
