using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public class ARChooser : StandardAndCustomComboBox
    {

        protected override void Dispose(bool disposing)
        {
            CustomUserSettings.Default.CustomDARs = CustomDARs;
            base.Dispose(disposing);
        }

        private static readonly string Later = "Auto-detect later";

        public static readonly Named<Dar>[] ARs = new Named<Dar>[] {
                new Named<Dar>("ITU 16:9 PAL (1.823361)", Dar.ITU16x9PAL),
                new Named<Dar>("ITU 4:3 PAL (1.367521)", Dar.ITU4x3PAL),
                new Named<Dar>("ITU 16:9 NTSC (1.822784)", Dar.ITU16x9NTSC),
                new Named<Dar>("ITU 4:3 NTSC (1.367088)", Dar.ITU4x3NTSC),
                new Named<Dar>("1:1", Dar.A1x1) };

        public ARChooser()
            : base("Clear user-selected ARs...", "Select AR...")
        {
            base.Getter = delegate
            {
                decimal result;
                if (NumberChooser.ShowDialog(
                    "Enter your AR:", "Custom AR", 3,
                    0.1M, 10M, (Value ?? Dar.ITU16x9PAL).ar, out result) == DialogResult.OK)
                    return new Dar(result);
                else
                    return null;
            };

            HasLater = true;
            CustomDARs = CustomUserSettings.Default.CustomDARs;
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dar? Value
        {
            get
            {
                if (SelectedObject.Equals(Later))
                    return null;
                if (SelectedObject is Named<Dar>)
                    return ((Named<Dar>)SelectedObject).Data;
                else
                    return ((Dar)SelectedObject);
            }
            set
            {
                if (value == null)
                    SelectedObject = Later;
                else
                    SelectedObject = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dar RealValue
        {
            get
            {
                if (HasLater) throw new Exception("For OneClicker, use Value");
                return Value.Value;
            }
            set { Value = value; }
        }

        public Dar[] CustomDARs
        {
            get
            {
                return Util.CastAll<Dar>(CustomItems);
            }
            set
            {
                if (value == null)
                    return;
                base.CustomItems = Util.CastAll<Dar, object>(value);
            }
        }
    }
    
}
