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

        public override JobTypes JobType
        {
            get { return JobTypes.VOBSUB; }
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
        public string ScriptFile
        {
            get { return scriptFile; }
            set { scriptFile = value; }
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
