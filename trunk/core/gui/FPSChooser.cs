using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public class FPSChooser : StandardAndCustomComboBox
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
                    "Enter your framerate:", "Custom framerate", 3,
                    1M, 1000M, Value ?? 25M, out result) == DialogResult.OK)
                    return new FPS(result);
                else
                    return null;
            };
            StandardItems = Framerates;
        }

        private string nullString;
        /// <summary>
        /// String to display which represents "null" filesize. If NullString is set to null, then
        /// there is no option not to select a filesize.
        /// </summary>
        public string NullString
        {
            get { return nullString; }
            set { nullString = value; fillStandard(); }
        }
        
        private void fillStandard()
        {
            List<object> objects = new List<object>();
            if (!string.IsNullOrEmpty(NullString))
                objects.Add(NullString);
            objects.AddRange(Framerates);
            base.StandardItems = objects.ToArray();

        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal? Value
        {
            get
            {
                if (SelectedObject.Equals(NullString))
                    return null;
                return ((FPS)SelectedObject).val;
            }
            set
            {
                if (value == null)
                {
                    SelectedObject = NullString;
                    return;
                }
                if (value == 0) return;
                SelectedObject = new FPS(value.Value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal CertainValue
        {
            get
            {
                return Value.Value;
            }
            set
            {
                Value = value;
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
