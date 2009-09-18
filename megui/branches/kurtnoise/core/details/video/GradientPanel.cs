using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class GradientPanel : System.Windows.Forms.Panel  
    {

        // member variables
        System.Drawing.Color mStartColor;
        System.Drawing.Color mEndColor;

        public GradientPanel()
        {
            InitializeComponent();
            PaintGradient();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
        }


        public System.Drawing.Color PageStartColor
        {
            get
            {
                return mStartColor;
            }
            set
            {
                mStartColor = value;
                PaintGradient();
            }
        }


        public System.Drawing.Color PageEndColor
        {
            get
            {
                return mEndColor;
            }
            set
            {
                mEndColor = value;
                PaintGradient();
            }
        }


        private void PaintGradient()
        {
            System.Drawing.Drawing2D.LinearGradientBrush gradBrush;
            gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0),
            new Point(this.Width, this.Height), PageStartColor, PageEndColor);

            Bitmap bmp = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(gradBrush, new Rectangle(0, 0, this.Width, this.Height));
            this.BackgroundImage = bmp;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

    }
}
