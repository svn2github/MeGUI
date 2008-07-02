using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MeGUI.core.util;
using System.Text.RegularExpressions;

namespace MeGUI.packages.reader.vstrip
{
    public class DVDFile
    {
        public PGC[] PGCs;
        public string Video;
        public string[] Audio;
        public string[] Subtitles;


        public static DVDFile Open(string file)
        {
            try
            {
                DVDFile d = new DVDFile();

                IntPtr p = ifoOpen(file, FioFlags.None);
                if (p == IntPtr.Zero) return null;

                d.PGCs = new PGC[ifoGetNumPGCI(p)];
                for (uint i = 0; i < d.PGCs.Length; ++i)
                {
                    Time t;
                    int numCells = ifoGetPGCIInfo(p, i, out t);
                    VobCellID[] cells = new VobCellID[numCells];
                    bool cellSuccess = ifoGetPGCICells(p, i, cells);
                    d.PGCs[i].Cells = cells;
                    d.PGCs[i].Length = t;
                }

                // "MPEG2 720x576 PAL 16:9 letbox "
                d.Video = ifoGetVideoDesc(p);

                // "English (AC3 6ch, 0xBD 0x80) [0]"
                d.Audio = new string[ifoGetNumAudio(p)];
                for (int i = 0; i < d.Audio.Length; ++i)
                    d.Audio[i] = ifoGetAudioDesc(p, i);

                d.Subtitles = new string[ifoGetNumSubPic(p)];
                for (int i = 0; i < d.Subtitles.Length; ++i)
                    d.Subtitles[i] = ifoGetSubPicDesc(p, i);

                //int n string[] a;
                bool b = ifoClose(p);

                return d;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region vStrip details
        private enum FioFlags
        {
            None = 0x00,
            Append = 0x01,
            Writeable = 0x02,
            UseAspi = 0x04,
            PreferAspi = 0x08,
            Support1GB = 0x10,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct Time
        {
            public byte hour;
            public byte minute;
            public byte second;
            public byte frame;

            public static implicit operator TimeSpan(Time t)
            {
                byte DVD_TIME_AND = 0x3f;	// to and the frame_u member with
                decimal fps = 25M;
                if ((t.frame & DVD_TIME_AND) == 192)
                    fps = 30M;

                int ms = (int)((decimal)(t.frame & DVD_TIME_AND) / fps * 1000M);
                return new TimeSpan(0, t.hour, t.minute, t.second, ms);
            }
        }


        [DllImport("vStrip.dll")]
        static extern IntPtr ifoOpen(string s, [MarshalAs(UnmanagedType.U4)] FioFlags flags);

        [DllImport("vStrip.dll")]
        static extern bool ifoClose(IntPtr handle);

        [DllImport("vStrip.dll")]
        static extern int ifoGetNumPGCI(IntPtr handle);

        [DllImport("vStrip.dll")]
        static extern int ifoGetPGCIInfo(IntPtr handle, [MarshalAs(UnmanagedType.U4)] uint title, out Time time);

        [DllImport("vStrip.dll")]
        unsafe static extern bool ifoGetPGCICells(IntPtr handle, [MarshalAs(UnmanagedType.U4)] uint title,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] VobCellID[] info /*VobCellID* info*/ );

        [DllImport("vStrip.dll")]
        static extern string ifoGetVideoDesc(IntPtr handle);

        [DllImport("vStrip.dll")]
        static extern int ifoGetNumAudio(IntPtr handle);

        [DllImport("vStrip.dll")]
        static extern string ifoGetAudioDesc(IntPtr handle, int index);

        [DllImport("vStrip.dll")]
        static extern int ifoGetNumSubPic(IntPtr handle);

        [DllImport("vStrip.dll")]
        static extern string ifoGetSubPicDesc(IntPtr handle, int index);
        #endregion

    }

    
    public struct PGC
    {
        public TimeSpan Length;
        public VobCellID[] Cells;
    }

    public struct IFOAudioInfo
    { }

    public struct IFOVideoInfo {
        string Codec;
        bool Pal;
        Dar Dar;
        uint Width;
        uint Height;
    }

    public struct IFOSubtitleInfo { }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VobCellID
    {
        public uint StartLBA;
        public uint EndLBA;
        public ushort VobId;
        public byte CellId;
        public byte Angle;
        public byte Chapter;

        private DVDFile.Time time;

        public TimeSpan Length
        {
            get { return time; }
        }
    }

    class vStripDLL
    {

        #region VOB reading
        enum ErrorCode
        {
            Ok = 0, Done = 1, UserFuncExit = 2, InitFailed = 3, CantOpenInput = 4,
            CantCreateOutput = 5, CantWriteOutput = 6, CantCrack = 7, LostSync = 8,
        }

        struct VSData { 
        }
        struct StreamFlags { }

        [DllImport("vStrip.dll")]
        [return: MarshalAs(UnmanagedType.U4)]
        static extern ErrorCode vs_init(
            VSData vsd, StreamFlags streams, StreamFlags substreams);

        #endregion
    }
}
