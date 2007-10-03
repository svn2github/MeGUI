using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.core.util
{
    public struct Dar
    {
        public static readonly Dar ITU16x9 = new Dar(1.823M);
        public static readonly Dar ITU4x3 = new Dar(1.3672M);
        public static readonly Dar A1x1 = new Dar(1M);

        public decimal ar;

        public Dar(ulong x, ulong y)
        {
            ar = -1;
            init(x, y);
        }

        public Dar(decimal dar)
        {
            ar = dar;
        }

        public Dar(decimal? dar, ulong width, ulong height)
        {
            ar = -1;
            if (dar.HasValue)
                ar = dar.Value;
            else
                init(width, height);
        }

        private void init(ulong x, ulong y)
        {
            ar = (decimal)x / (decimal)y;
        }

        public Dar(int x, int y, ulong width, ulong height)
        {
            ar = -1;
            if (x > 0 && y > 0)
                init((ulong)x, (ulong)y);
            else
                init(width, height);
        }

        public ulong X
        {
            get
            {
                ulong x, y;
                RatioUtils.approximate(ar, out x, out y);
                return x;
            }
        }

        public ulong Y
        {
            get
            {
                ulong x, y; RatioUtils.approximate(ar, out x, out y);
                return y;
            }
        }



        public override string ToString()
        {
            return ar.ToString("#.###");
        }

        public override bool Equals(object obj)
        {
            if (!( obj is Dar)) return false;
            decimal ar2 = ((Dar)obj).ar;
            
            return (Math.Abs(ar - ar2) < 0.001M * Math.Min(ar, ar2));
        }

        public Sar ToSar(int hres, int vres)
        {
            // sarX
            // ----   must be the amount the video needs to be stretched horizontally.
            // sarY
            //
            //    horizontalResolution
            // --------------------------  is the ratio of the pixels. This must be stretched to equal realAspectRatio
            //  scriptVerticalResolution
            //
            // To work out the stretching amount, we then divide realAspectRatio by the ratio of the pixels:
            // sarX      parX        horizontalResolution        realAspectRatio * scriptVerticalResolution
            // ---- =    ---- /   -------------------------- =  --------------------------------------------   
            // sarY      parY     scriptVerticalResolution               horizontalResolution
            decimal ratio = ar * (decimal)vres / (decimal)hres;
            return new Sar(ratio);
        }
    }

    public struct Sar
    {
        public decimal ar;

        public Sar(ulong x, ulong y)
        {
            ar = (decimal)x / (decimal)y;
        }

        public Sar(decimal sar)
        {
            ar = sar;
        }

        public ulong X
        {
            get
            {
                ulong x, y;
                RatioUtils.approximate(ar, out x, out y);
                return x;
            }
        }

        public ulong Y
        {
            get
            {
                ulong x, y; RatioUtils.approximate(ar, out x, out y);
                return y;
            }
        }

        public Dar ToDar(int hres, int vres)
        {
            return new Dar(ar * (decimal)hres / (decimal)vres);
        }
    }

    struct RatioUtils
    {
        /// <summary>
        /// Puts x and y in simplest form, by dividing by all their factors.
        /// </summary>
        /// <param name="x">First number to reduce</param>
        /// <param name="y">Second number to reduce</param>
        public static void reduce(ref ulong x, ref ulong y)
        {
            ulong g = gcd(x, y);
            x /= g;
            y /= g;
        }

        private static ulong gcd(ulong x, ulong y)
        {
            while (y != 0)
            {
                ulong t = y;
                y = x % y;
                x = t;
            }
            return x;
        }

        public static void approximate(decimal val, out ulong x, out ulong y)
        {
            approximate(val, out x, out y, 5000);
        }

        public static void approximate(decimal val, out ulong x, out ulong y, ulong limit)
        {
            y = limit;
            x = (ulong)((decimal)y * val);
            reduce(ref x, ref y);
        }
    }
}
