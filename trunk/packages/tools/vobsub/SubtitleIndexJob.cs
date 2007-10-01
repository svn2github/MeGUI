using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class SubtitleIndexJob : Job
    {
        private bool indexAllTracks;
        private List<int> trackIDs;
        private int pgc;
        private string scriptFile;
        public SubtitleIndexJob()
            : base()
		{
            indexAllTracks = true;
            trackIDs = new List<int>();
            pgc = 1;
		}

        public SubtitleIndexJob(string input, string output,
            bool indexAllTracks, List<int> trackIDs, int pgc)
        {
            Input = input;
            Output = output;
            IndexAllTracks = indexAllTracks;
            TrackIDs = trackIDs;
            PGC = pgc;
        }

        
        public bool IndexAllTracks
        {
            get { return indexAllTracks; }
            set { indexAllTracks = value; }
        }
        public List<int> TrackIDs
        {
            get { return trackIDs; }
            set { trackIDs = value; }
        }
        public int PGC
        {
            get { return pgc; }
            set { pgc = value; }
        }

        public override string CodecString
        {
            get { return ""; }
        }

        public override string EncodingMode
        {
            get { return "sub"; }
        }
    }
}
