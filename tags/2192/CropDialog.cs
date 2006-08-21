using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public class CropDialog : Form
    {
        public CropDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.containerControl2 = new System.Windows.Forms.ContainerControl();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.containerControl1 = new System.Windows.Forms.ContainerControl();
            this.y2offset = new System.Windows.Forms.NumericUpDown();
            this.x2offset = new System.Windows.Forms.NumericUpDown();
            this.y1offset = new System.Windows.Forms.NumericUpDown();
            this.x1offset = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.containerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.containerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.y2offset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x2offset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.y1offset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x1offset)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 218);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.CropDialog_Paint);
            // 
            // containerControl2
            // 
            this.containerControl2.BackColor = System.Drawing.SystemColors.Control;
            this.containerControl2.Controls.Add(this.button3);
            this.containerControl2.Controls.Add(this.button2);
            this.containerControl2.Controls.Add(this.button1);
            this.containerControl2.Controls.Add(this.trackBar1);
            this.containerControl2.Controls.Add(this.containerControl1);
            this.containerControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.containerControl2.Location = new System.Drawing.Point(0, 375);
            this.containerControl2.Name = "containerControl2";
            this.containerControl2.Size = new System.Drawing.Size(475, 80);
            this.containerControl2.TabIndex = 7;
            this.containerControl2.Text = "containerControl2";
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar1.Location = new System.Drawing.Point(140, 0);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(335, 42);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.TickFrequency = 100;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // containerControl1
            // 
            this.containerControl1.BackColor = System.Drawing.SystemColors.Control;
            this.containerControl1.Controls.Add(this.y2offset);
            this.containerControl1.Controls.Add(this.x2offset);
            this.containerControl1.Controls.Add(this.y1offset);
            this.containerControl1.Controls.Add(this.x1offset);
            this.containerControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.containerControl1.Location = new System.Drawing.Point(0, 0);
            this.containerControl1.Name = "containerControl1";
            this.containerControl1.Size = new System.Drawing.Size(140, 80);
            this.containerControl1.TabIndex = 7;
            this.containerControl1.Text = "containerControl1";
            // 
            // y2offset
            // 
            this.y2offset.Location = new System.Drawing.Point(36, 55);
            this.y2offset.Name = "y2offset";
            this.y2offset.Size = new System.Drawing.Size(67, 20);
            this.y2offset.TabIndex = 9;
            // 
            // x2offset
            // 
            this.x2offset.Location = new System.Drawing.Point(73, 29);
            this.x2offset.Name = "x2offset";
            this.x2offset.Size = new System.Drawing.Size(67, 20);
            this.x2offset.TabIndex = 8;
            // 
            // y1offset
            // 
            this.y1offset.Location = new System.Drawing.Point(36, 3);
            this.y1offset.Name = "y1offset";
            this.y1offset.Size = new System.Drawing.Size(67, 20);
            this.y1offset.TabIndex = 7;
            // 
            // x1offset
            // 
            this.x1offset.Location = new System.Drawing.Point(2, 29);
            this.x1offset.Name = "x1offset";
            this.x1offset.Size = new System.Drawing.Size(67, 20);
            this.x1offset.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(397, 52);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(316, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(155, 52);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Auto Crop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // CropDialog
            // 
            this.ClientSize = new System.Drawing.Size(475, 455);
            this.Controls.Add(this.containerControl2);
            this.Controls.Add(this.panel1);
            this.Name = "CropDialog";
            this.Text = "UNDER CONSTRUCTION YET";
            this.containerControl2.ResumeLayout(false);
            this.containerControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.containerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.y2offset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x2offset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.y1offset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x1offset)).EndInit();
            this.ResumeLayout(false);

        }
        private Panel panel1;
        private ContainerControl containerControl2;
        private TrackBar trackBar1;
        private ContainerControl containerControl1;
        private NumericUpDown y2offset;
        private NumericUpDown x2offset;
        private NumericUpDown y1offset;
        private NumericUpDown x1offset;
        private Button button2;
        private Button button1;
        private Button button3;

        private AvsReader reader = null;

        private void init()
        {
            x1offset.Minimum = x2offset.Minimum = y1offset.Minimum = y2offset.Minimum = 0;
            x1offset.Value = x2offset.Value = y1offset.Value = y2offset.Value = 0;
            x1offset.Maximum = x2offset.Maximum = reader.Width;
            y1offset.Maximum = y2offset.Maximum = reader.Height;
            trackBar1.Minimum = 0;
            trackBar1.Maximum = reader.FrameCount - 1;
            trackBar1.Value = reader.FrameCount / 2;
            panel1.ClientSize = new Size(reader.Width + 2, reader.Height + 2);
           
            int w = panel1.Right + panel1.Left;
            if (w < 600)
            {
                w = 600;
                panel1.Left = (w - panel1.Width) / 2;
            }
            int h = panel1.Bottom + panel1.Top + containerControl2.Height;
            this.ClientSize = new Size(w, h);
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            x1offset.ValueChanged += new System.EventHandler(this.cropValues_Changed);
            x2offset.ValueChanged += new System.EventHandler(this.cropValues_Changed);
            y1offset.ValueChanged += new System.EventHandler(this.cropValues_Changed);
            y2offset.ValueChanged += new System.EventHandler(this.cropValues_Changed);
        }



        static public void Execute(IWin32Window parent, string script)
        {
            using(AvsReader r = AvsReader.ParseScript(script))
            {
                using (CropDialog d = new CropDialog())
                {
                    d.reader = r;
                    d.init();
                    d.ShowDialog(parent);
                }
            }
        }

        //private 

        private static readonly Color lineColor = Color.Cyan;

        private int cropLeft
        {
            get
            {
                return (int)x1offset.Value;
            }
            set
            {
                x1offset.Value =  value;
            }
        }
        private int cropRight
        {
            get
            {
                return (int)x2offset.Value;
            }
            set
            {
                x2offset.Value = value;
            }
        }
        private int cropTop
        {
            get
            {
                return (int)y1offset.Value;
            }
            set
            {
                y1offset.Value = value;
            }
        }
        private int cropBottom
        {
            get
            {
                return (int)y2offset.Value;
            }
            set
            {
                y2offset.Value = value;
            }
        }

        private void drawFrame(Graphics g)
        {
            
            using (Bitmap bmp = reader.ReadFrameBitmap(trackBar1.Value))
            {
                g.FillRectangle(Brushes.Black, 0, 0, bmp.Width + 2, bmp.Height + 2);
                g.DrawImage(bmp, new Point(1,1));
                using (Pen p = new Pen(lineColor))
                {
                    g.DrawLine(p, 0, cropTop, bmp.Width + 1, cropTop);
                    g.DrawLine(p, 0, bmp.Height - cropBottom, bmp.Width + 1, bmp.Height - cropBottom);
                    g.DrawLine(p, cropLeft, 0, cropLeft, bmp.Height+1);
                    g.DrawLine(p, bmp.Width - cropRight, 0, bmp.Width - cropRight, bmp.Height + 1);
                }
            }

        }

        private void drawFrame()
        {
            using (Graphics g = panel1.CreateGraphics())
                drawFrame(g);
        }

        private static int[] dividers = new int[] { 32, 16, 8, 4, 2, 1 };
        private int getmod(int n)
        {
            foreach (int x in dividers)
                if (0 == n % x)
                    return x;
            return 0;
            
        }

        private void showInfo()
        {
            int h = reader.Height - cropTop - cropBottom;
            int w = reader.Width - cropLeft - cropRight;

            this.Text = string.Format("Size: {0}x{1}, Mod: {2}x{3} ", w, h, getmod(w), getmod(h));
        }

        private void CropDialog_Paint(object sender, PaintEventArgs e)
        {
            drawFrame(e.Graphics);
            showInfo();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            drawFrame();
        }

        private void cropValues_Changed(object sender, EventArgs e)
        {
            drawFrame();
            showInfo();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!panel1.Capture)
            {
                CropOrigin o = getOrigin(e.X, e.Y);
                switch (o)
                {
                    case CropOrigin.Bottom:
                    case CropOrigin.Top:
                        panel1.Cursor = Cursors.HSplit;
                        return;
                    case CropOrigin.Left:
                    case CropOrigin.Right:
                        panel1.Cursor = Cursors.VSplit;
                        return;
                    default:
                        panel1.Cursor = Cursors.Default;
                        return;
                }
            }
            else
            {
                switch (currentOrigin)
                {
                    case CropOrigin.Bottom:
                        cropBottom = (panel1.ClientSize.Height - 1) - e.Y;
                        return;
                    case CropOrigin.Top:
                        cropTop = e.Y;
                        return;
                    case CropOrigin.Left:
                        cropLeft = e.X;
                        return;
                    case CropOrigin.Right:
                        cropRight = (panel1.ClientSize.Width - 1) - e.X;
                        return;
                    default:
                        return;
                }                
            }
        }

        private CropOrigin getOrigin(int x, int y)
        {
            if (y == cropTop)
                return CropOrigin.Top;
            if (y == (panel1.ClientSize.Height - 1 - cropBottom))
                return CropOrigin.Bottom;
            if (x == cropLeft)
                return CropOrigin.Left;
            if (x == (panel1.ClientSize.Width - 1 - cropRight))
                return CropOrigin.Right;
            return CropOrigin.None;


        }

        private enum CropOrigin
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }

        private CropOrigin currentOrigin = CropOrigin.None;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            currentOrigin = getOrigin(e.X, e.Y);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CropValues final = VideoUtil.autocrop(reader);
            bool error = (final.left == -1);
            if (!error)
            {
                cropLeft = final.left;
                cropTop = final.top;
                cropRight = final.right;
                cropBottom = final.bottom;
            }
            else
                MessageBox.Show("I'm afraid I was unable to find 3 frames that have matching crop values");

        }


    }
}
