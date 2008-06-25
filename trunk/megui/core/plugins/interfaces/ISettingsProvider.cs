using System;
using System.Collections.Generic;
using System.Text;

using MeGUI.core.plugins.interfaces;
using MeGUI.core.details.video;
using MeGUI.packages.video.x264;
using MeGUI.packages.video.lmp4;
using MeGUI.packages.video.snow;
using MeGUI.packages.video.xvid;
using MeGUI.packages.audio.naac;
using MeGUI.packages.audio.audx;
using MeGUI.packages.audio.faac;
using MeGUI.packages.audio.ffac3;
using MeGUI.packages.audio.ffmp2;
using MeGUI.packages.audio.lame;
using MeGUI.packages.audio.vorbis;
using MeGUI.packages.audio.waac;
using MeGUI.packages.audio.aften;
using System.Windows.Forms;
using MeGUI.core.util;

namespace MeGUI
{

    public delegate void StringChanged(object sender, string val);
    public delegate void IntChanged(object sender, int val);
    public class VideoInfo
    {
        private string videoInput;
        public event StringChanged VideoInputChanged;
        public string VideoInput
        {
            get { return videoInput; }
            set { videoInput = value; VideoInputChanged(this, value); }
        }

        private string videoOutput;
        public event StringChanged VideoOutputChanged;
        public string VideoOutput
        {
            get { return videoOutput; }
            set { videoOutput = value; VideoOutputChanged(this, value); }
        }

        private Dar? dar = null;

        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
        }
        
        private int introEndFrame;

        public int IntroEndFrame
        {
            get { return introEndFrame; }
            set { introEndFrame = value; }
        }
        private int creditsStartFrame;

        public int CreditsStartFrame
        {
            get { return creditsStartFrame; }
            set { creditsStartFrame = value; }
        }

        private Zone[] zones;
        public Zone[] Zones
        {
            get { return zones; }
            set { zones = value ?? new Zone[0]; }
        }


        public VideoInfo(string videoInput, string videoOutput, int darX, int darY, int creditsStartFrame, int introEndFrame, Zone[] zones)
        {
            this.videoInput = videoInput;
            this.videoOutput = videoOutput;
            this.creditsStartFrame = creditsStartFrame;
            this.introEndFrame = introEndFrame;
            this.zones = zones;
        }

        public VideoInfo()
            : this("", "", -1, -1, -1, -1, null) { }

        internal VideoInfo Clone()
        {
            return (VideoInfo)this.MemberwiseClone();
        }
    }
}
