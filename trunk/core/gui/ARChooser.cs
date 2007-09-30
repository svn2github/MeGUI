using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.util;
using System.ComponentModel;

namespace MeGUI.core.gui
{
    public class ARChooser : StandardAndCustomComboBox
    {
        private static readonly string Later = "Auto-detect later";

        public static readonly Named<Dar>[] ARs = new Named<Dar>[] {
                new Named<Dar>("ITU 16:9 (1.823)", Dar.ITU16x9),
                new Named<Dar>("ITU 4:3 (1.3672)", Dar.ITU4x3),
                new Named<Dar>("1:1", Dar.A1x1) };

        public ARChooser()
            : base("Clear user-selected ARs...", "Select AR...")
        {
            base.Getter = delegate
            {
                decimal result;
                if (NumberChooser.ShowDialog(
                    "Enter your AR:", "Custom AR", 3,
                    0.1M, 10M, (Value ?? Dar.ITU16x9).ar, out result) == DialogResult.OK)
                    return new Named<Dar>(new Dar(result).ToString(), new Dar(result));
                else
                    return null;
            };

            HasLater = true;
        }

        bool hasLater;

        public bool HasLater
        {
            get { return hasLater; }
            set
            {
                hasLater = value;
                if (hasLater)
                {
                    List<object> o = new List<object>();
                    o.Add(Later);
                    o.AddRange(ARs);
                    base.StandardItems = o.ToArray();
                }
                else
                {
                    base.StandardItems = ARs;
                }
            }
        }

        [Browsable(false)]
        public Dar? Value
        {
            get
            {
                if (SelectedObject.Equals(Later))
                    return null;
                return ((Named<Dar>)SelectedObject).Data;
            }
            set
            {
                if (value == null)
                    SelectedObject = Later;
                else
                    SelectedObject = new Named<Dar>(value.ToString(), value.Value);
            }
        }

        [Browsable(false)]
        public Dar RealValue
        {
            get
            {
                if (HasLater) throw new Exception("For OneClicker, use Value");
                return Value.Value;
            }
            set { Value = value; }
        }
    }
}
