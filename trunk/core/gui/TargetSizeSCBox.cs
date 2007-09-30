using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.util;
using System.Windows.Forms;
using System.Diagnostics;

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

        private static readonly string DontCare = "Don't care";



        public TargetSizeSCBox() : base("Clear user-selected sizes...", "Select size...")
        {
            base.Getter = new Getter<object>(getter);
            List<object> objects = new List<object>();
            objects.Add(DontCare);
            objects.AddRange(TargetSizeSCBox.PredefinedFilesizes);
            base.StandardItems = objects.ToArray();
        }

        FileSizeDialog ofd = new FileSizeDialog();

        private object getter()
        {
            ofd.Value = Value ?? new FileSize(Unit.MB, 700);
            if (ofd.ShowDialog() == DialogResult.OK)
                return new Named<FileSize>(ofd.Value.ToString(), ofd.Value);
            return null;
        }

        /// <summary>
        /// Gets / sets the target, or null if the user doesn't care about filesize
        /// </summary>
        public FileSize? Value
        {
            get
            {
                object o = base.SelectedObject;
                if (DontCare.Equals(o))
                    return null;
                Debug.Assert(o is Named<FileSize>);
                return ((Named<FileSize>)o).Data;
            }

            set
            {
                if (value.HasValue)
                    base.SelectedObject = new Named<FileSize>(value.HasValue.ToString(), value.Value);
                else
                    base.SelectedObject = DontCare;
            }
        }
    }
}
