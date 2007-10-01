using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.core.details
{
    public class CleanupJob : Job
    {
        public static JobChain AddAfter(JobChain other, IEnumerable<string> files)
        {
            return AddAfter(other, null, files);
        }

        public static JobChain AddAfter(JobChain other, string type, IEnumerable<string> files)
        { throw new Exception(); }



        public override string CodecString
        {
            get { return "cleanup"; }
        }

        private string type;
        public override string EncodingMode
        {
            get { return type; }
        }
    }
}
