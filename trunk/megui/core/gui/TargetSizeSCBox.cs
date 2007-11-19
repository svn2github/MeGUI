using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.util;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace MeGUI.core.gui
{
    public class TargetSizeSCBox : StandardAndCustomComboBox
    {   
        public static readonly Named<FileSize>[] PredefinedFilesizes = new Named<FileSize>[] {
            new Named<FileSize>("1/4 CD (175MB)", new FileSize(Unit.MB, 175)),
            new Named<FileSize>("1/2 CD (350MB)", new FileSize(Unit.MB, 350)),
            new Named<FileSize>(  "1 CD (700MB)", new FileSize(Unit.MB, 700)),
            new Named<FileSize>("2 CDs (1400MB)", new FileSize(Unit.MB, 1400)),
            new Named<FileSize>("3 CDs (2100MB)", new FileSize(Unit.MB, 2100)),
            new Named<FileSize>("1/3 DVD (1493MB)", new FileSize(Unit.MB, 1493)),
            new Named<FileSize>("1/4 DVD (1120MB)", new FileSize(Unit.MB, 1120)),
            new Named<FileSize>("1/5 DVD (896MB)", new FileSize(Unit.MB, 896)),
            new Named<FileSize>("1 DVD (4479MB)", new FileSize(Unit.MB, 4479)),
            new Named<FileSize>("1 DVD-9 (8138MB)", new FileSize(Unit.MB, 8138)) };

        protected override void Dispose(bool disposing)
        {
            CustomUserSettings.Default.CustomSizes = CustomSizes;
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

        private FileSize minSize = FileSize.MinNonZero;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileSize MinimumFileSize
        {
            get { return minSize; }
            set { minSize = value; }
        }

        private FileSize? maxSize = null;
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileSize? MaximumFileSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }

        public TargetSizeSCBox() : base("Clear user-selected sizes...", "Select size...")
        {
            base.Getter = new Getter<object>(getter);
            CustomSizes = CustomUserSettings.Default.CustomSizes;
        }

        private void fillStandard()
        {
            List<object> objects = new List<object>();
            if (!string.IsNullOrEmpty(NullString))
                objects.Add(NullString);
            objects.AddRange(TargetSizeSCBox.PredefinedFilesizes);
            base.StandardItems = objects.ToArray();

        }

        FileSizeDialog ofd = new FileSizeDialog();

        private object getter()
        {
            ofd.Value = Value ?? new FileSize(Unit.MB, 700);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.Value >= minSize &&
                   maxSize == null || ofd.Value <= maxSize)
                    return ofd.Value;
                else
                    MessageBox.Show(genRestrictions(), "Invalid filesize", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return null;
        }

        public FileSize[] CustomSizes
        {
            get
            {
                return Util.CastAll<FileSize>(CustomItems);
            }
            set
            {
                CustomItems = Util.CastAll<FileSize, object>(value);
            }
        }


        private string genRestrictions()
        {
            if (maxSize.HasValue)
                return string.Format("Filesize must be between {0} and {1}.", minSize, maxSize);
            else
                return string.Format("Filesize must be at least {0}.", minSize);
        }

        /// <summary>
        /// Gets / sets the target, or null if the user doesn't care about filesize
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileSize? Value
        {
            get
            {
                object o = base.SelectedObject;
                if (o.Equals(NullString))
                    return null;
                if (o is Named<FileSize>)
                    return ((Named<FileSize>)o).Data;
                else
                    return (FileSize)o;
            }

            set
            {
                if (value.HasValue)
                    base.SelectedObject = value.Value;
                else
                    base.SelectedObject = NullString;
            }
        }

        /// <summary>
        /// Gets / sets the target, or null if the user doesn't care about filesize
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileSize CertainValue
        {
            get { return Value.Value; }
            set { Value = value; }
        }

    }
}
