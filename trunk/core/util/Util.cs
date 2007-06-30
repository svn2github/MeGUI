using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MeGUI.core.util
{
    // The base class for MeGUI-triggered exceptions
    public class MeGUIException : Exception
    {}

    public class Pair<T1, T2>
    {
        public T1 fst;
        public T2 snd;
        public Pair(T1 f, T2 s)
        {
            fst = f;
            snd = s;
        }
    }

    public class Util
    {
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

        public static string ToString(ulong? u)
        {
            if (!u.HasValue) return null;
            return u.Value.ToString();
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
