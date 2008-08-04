using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MeGUI.packages.tools.besplitter
{
    public class AudioJoinJob : Job
    {
        private TimeSpan cliplength;

        public TimeSpan ClipLength
        {
            get { return cliplength; }
            set { cliplength = value; }
        }


        private string[] inputFiles;

        public string[] InputFiles
        {
            get { return inputFiles; }
            set { inputFiles = value; }
        }

        public string generateJoinCommandline(string tempFilename)
        {
            return string.Format("-core ( -input \"{0}\" -prefix \"{1}\" -type {2} -join )",
                tempFilename, Output, Path.GetExtension(Output).Substring(1));
        }

        public AudioJoinJob() { }

        public AudioJoinJob(string[] inputFiles, string output)
        {
            Debug.Assert(inputFiles.Length > 0);
            this.inputFiles = inputFiles;
            this.Output = output;
            this.Input = inputFiles[0];
        }

        public override string CodecString
        {
            get { return "cut"; }
        }

        public override string EncodingMode
        {
            get { return "join"; }
        }

    }
}
