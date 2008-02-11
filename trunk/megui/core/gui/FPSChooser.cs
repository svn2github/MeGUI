using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using MeGUI.core.util;

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
            CustomFPSs = CustomUserSettings.Default.CustomFPSs;
        }

        protected override void Dispose(bool disposing)
        {
            CustomUserSettings.Default.CustomFPSs = CustomFPSs;
            base.Dispose(disposing);
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

        private FPS[] CustomFPSs
        {
            get { return Util.CastAll<FPS>(CustomItems); }
            set { CustomItems = Util.CastAll<FPS, object>(value); }
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

    [TypeConverter(typeof(FPSConverter))]
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

        public static FPS Parse(string s)
        {
            return new FPS(decimal.Parse(s));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FPS)) return false;
            decimal other = ((FPS)obj).val;
            return (Math.Abs(val - other) < MainForm.Instance.Settings.AcceptableFPSError);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    class FPSConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
                return FPS.Parse((string)value);

            throw new Exception();
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return value.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
