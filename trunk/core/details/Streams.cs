using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.util;

namespace MeGUI.core.details
{
    public class BitrateCalculationStream
    {
        public BitrateCalculationStream(string filename)
        {
            this.Filename = filename;
            if (Filename != null) fillInfo();
        }

        public string Filename;
        public FileSize? Size;
        public OutputType Type;

        public virtual void fillInfo()
        {
            Size = FileSize.Of2(Filename);
        }

    }

    public class AudioBitrateCalculationStream : BitrateCalculationStream
    {
        public AudioBitrateCalculationStream() : this(null) { }

        public AudioBitrateCalculationStream(string filename)
            : base(filename) {}


        public AudioType AType;

        public override void fillInfo()
        {
            base.fillInfo();
            Type = AType = VideoUtil.guessAudioType(Filename);
        }
    }


}

