using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    class FPSChooser : StandardAndCustomComboBox
    {
        private static readonly object[] Framerates = new object[] { 
            new FPS(23.976M), 
            new FPS(24.0M), 
            new FPS(25.0M), 
            new FPS(29.97M), 
            new FPS(30.0M), 
            new FPS(50.0M), 
            new FPS(59.94M), 
            new FPS(60.0M) };

        public FPSChooser()
            : base("Clear user-selected framerates...", "Select framerate...")
        {
            base.Getter = delegate
            {
                decimal result;
                if (NumberChooser.ShowDialog(
                    "Enter your AR:", "Custom AR", 3,
                    1M, 1000M, Value, out result) == DialogResult.OK)
                    return new FPS(result);
                else
                    return null;
            };
            StandardItems = Framerates;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal Value
        {
            get
            {
                return ((FPS)SelectedObject).val;
            }
            set
            {
                if (value == 0) return;
                SelectedObject = new FPS(value);
            }
        }
    }

    internal struct FPS
    {
        internal FPS(decimal v)
        {
            val = v;
        }

        internal decimal val;

        public override string ToString()
        {
            return val.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FPS)) return false;
            decimal other = ((FPS)obj).val;
            return (Math.Abs(val - other) < MainForm.Instance.Settings.AcceptableFPSError);
        }
    }
}
