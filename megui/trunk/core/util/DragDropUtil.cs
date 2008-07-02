using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.util
{
    public delegate void SingleFileReceiver(string file);
    public delegate void MultiFileReceiver(string[] files);

    public class DragDropUtil
    {
        public static void RegisterSingleFileDragDrop(Control c, SingleFileReceiver r)
        {
            RegisterSingleFileDragDrop(c, r, "*.*");
        }

        public static void RegisterSingleFileDragDrop(Control c, SingleFileReceiver r, string filter)
        {
            RegisterSingleFileDragDrop(c, r, delegate() { return filter; });
        }

        public static void RegisterSingleFileDragDrop(Control c, SingleFileReceiver r, Getter<string> filter)
        {
            c.AllowDrop = true;
            c.DragEnter += delegate(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (files.Length == 1 && FileUtil.MatchesFilter(filter(), files[0]))
                    e.Effect = DragDropEffects.All;
            }
        };

            c.DragDrop += delegate(object sender, DragEventArgs e)
        {
            r(((string[])e.Data.GetData(DataFormats.FileDrop, false))[0]);
        };

        }

        public static void RegisterMultiFileDragDrop(Control c, MultiFileReceiver r)
        {
            RegisterMultiFileDragDrop(c, r, "*.*");
        }

        public static void RegisterMultiFileDragDrop(Control c, MultiFileReceiver r, string filter)
        {
            RegisterMultiFileDragDrop(c, r, delegate() { return filter; });
        }

        public static void RegisterMultiFileDragDrop(Control c, MultiFileReceiver r, Getter<string> filter)
        {
            c.AllowDrop = true;
            c.DragEnter += delegate(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (files.Length > 0 &&
                    Array.Exists<string>(files, delegate(string s)
                {
                    return FileUtil.MatchesFilter(filter(), s);
                }))
                    e.Effect = DragDropEffects.All;
            }
        };

            c.DragDrop += delegate(object sender, DragEventArgs e)
        {
            r(Array.FindAll<string>(((string[])e.Data.GetData(DataFormats.FileDrop, false)),
                delegate(string s)
                {
                    return FileUtil.MatchesFilter(filter(), s);
                }));
        };

        }
    }
}
