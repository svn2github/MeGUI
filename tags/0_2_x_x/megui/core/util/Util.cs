using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;

namespace MeGUI.core.util
{

    public class Pair<T1, T2>
    {
        public T1 fst;
        public T2 snd;
        public Pair(T1 f, T2 s)
        {
            fst = f;
            snd = s;
        }

        public Pair() { }
    }

    public delegate T Getter<T>();
    public delegate void Setter<T>(T thing);

    public class Util
    {
        public static void SetSize(Form f, Size s, FormWindowState state)
        {
            f.WindowState = state;
            if (f.WindowState == FormWindowState.Normal)
                f.ClientSize = s;
        }

        public static void SaveSize(Form f, Setter<Size> size, Setter<FormWindowState> state)
        {
            size(f.ClientSize);
            state(f.WindowState);
        }


        public static void ThreadSafeRun(Control c, MethodInvoker m)
        {
            if (c.InvokeRequired)
                c.Invoke(m);
            else
                m();
        }

        public static void XmlSerialize<T>(T t, string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (Stream s = File.Open(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    ser.Serialize(s, t);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }

        public static T XmlDeserializeOrDefault<T>(string path)
            where T : class, new()
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            if (File.Exists(path))
            {
                using (Stream s = File.OpenRead(path))
                {
                    try
                    {
                        return (T)ser.Deserialize(s);
                    }
                    catch (Exception e)
                    {
                        DialogResult r = MessageBox.Show("File '" + path + "' could not be loaded. Delete?", "Error loading Job", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (r == DialogResult.Yes)
                        {
                            try { s.Close(); File.Delete(path); }
                            catch (Exception) { }
                        }
                        Console.Write(e.Message);
                        return null;
                    }
                }
            }
            else return new T();
        }
        
        public static T XmlDeserialize<T>(string path)
            where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            if (File.Exists(path))
            {
                using (Stream s = File.OpenRead(path))
                {
                    try
                    {
                        return (T)ser.Deserialize(s);
                    }
                    catch (Exception e)
                    {
                        DialogResult r = MessageBox.Show("File '" + path + "' could not be loaded. Delete?", "Error loading Job", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (r == DialogResult.Yes)
                        {
                            try { s.Close(); File.Delete(path); }
                            catch (Exception) { }
                        }
                        Console.Write(e.Message);
                        return null;
                    }
                }
            }
            else return null;
        }


        private static readonly System.Text.RegularExpressions.Regex _cleanUpStringRegex = new System.Text.RegularExpressions.Regex(@"\n[^\n]+\r", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);
        public static string cleanUpString(string s)
        {
            return _cleanUpStringRegex.Replace(s.Replace(Environment.NewLine, "\n"), Environment.NewLine);
        }

        public static void ensureExists(string file)
        {
            if (file == null || !System.IO.File.Exists(file))
                throw new MissingFileException(file);
        }

        public static void ensureExistsIfNeeded(string file)
        {
            if (!string.IsNullOrEmpty(file)) ensureExists(file);
        }

        public static string ToString(TimeSpan? t1)
        {
            if (!t1.HasValue) return null;
            TimeSpan t = t1.Value;
            return (new TimeSpan(t.Hours, t.Minutes, t.Seconds)).ToString();
        }

        /// <summary>
        /// Formats the decimal according to what looks nice in MeGUI (ensures consistency
        /// and not too many decimal places)
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToString(decimal? d)
        {
            if (!d.HasValue) return null;
            return d.Value.ToString("#####.##");
        }

        public static string ToStringOrNull<T>(T t)
            where T : class
        {
            if (t == null) return null;
            return t.ToString();
        }

        public static string ToStringOrNull<T>(T? t)
            where T : struct
        {
            if (t == null) return null;
            return t.Value.ToString();
        }
/*        public static string ToStringOrNull<T>(T? t)
            where T : class
        {
            if (!t.HasValue) return null;
            return t.Value.ToString();
        }*/

        public static int CountStrings(string src, char find)
        {
            int ret = 0;
            foreach (char s in src)
            {
                if (s == find)
                {
                    ++ret;
                }
            }
            return ret;
        }        
        
        #region range clamping
        public static void clampedSet(NumericUpDown box, decimal value)
        {
            box.Value = clamp(value, box.Minimum, box.Maximum);
        }

        public static decimal clamp(decimal val, decimal min, decimal max)
        {
            Debug.Assert(max >= min);
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        public static int clamp(int val, int min, int max)
        {
            return (int)clamp((decimal)val, min, max);
        }

        public static uint clamp(uint val, uint min, uint max)
        {
            return (uint)clamp((decimal)val, min, max);
        }

        public static ulong clamp(ulong val, ulong min, ulong max)
        {
            return (ulong)clamp((decimal)val, min, max);
        }

        public static long clamp(long val, long min, long max)
        {
            return (long)clamp((decimal)val, min, max);
        }

        public static ulong clamp(long val, ulong min, ulong max)
        {
            return (ulong)clamp((decimal)val, min, max);
        }

        public static ulong clampPositive(long val)
        {
            if (val < 0) return 0u;
            return (ulong)val;
        }
        public static uint clampPositive(int val)
        {
            return (uint)clampPositive((long)val);
        }
        #endregion
    }
}
