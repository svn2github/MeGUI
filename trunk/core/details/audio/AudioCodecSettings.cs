// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using MeGUI.core.plugins.interfaces;
namespace MeGUI
{
	/// <summary>
	/// Summary description for AudioCodecSettings.
	/// </summary>
	/// 
	public enum BitrateManagementMode {CBR, VBR, ABR};
	public enum ChannelMode
	{
	    [EnumTitle("Keep Original Channels")]
	    KeepOriginal, 
	    [EnumTitle("Convert to Mono")]
	    ConvertToMono,  
	    [EnumTitle("Downmix multichannel to Stereo")]
	    StereoDownmix,
	    [EnumTitle("Downmix multichannel to Dolby Pro Logic")]
	    DPLDownmix,
        [EnumTitle("Downmix multichannel to Dolby Pro Logic II")]
        DPLIIDownmix,
        [EnumTitle("Upmix 2 to 5.1 via SuperEQ (slow)")]
	    Upmix,
        [EnumTitle("Upmix 2 to 5.1 via Sox equalizer adjustments")]
        UpmixUsingSoxEq,
        [EnumTitle("Upmix 2 to 5.1 with center channel dialog")]
        UpmixWithCenterChannelDialog
	};

    [
        XmlInclude(typeof(MP2Settings)),
        XmlInclude(typeof(AC3Settings)), 
        XmlInclude(typeof(WinAmpAACSettings)), 
        XmlInclude(typeof(AudXSettings)), 
        XmlInclude(typeof(NeroAACSettings)), 
        XmlInclude(typeof(MP3Settings)), 
        XmlInclude(typeof(FaacSettings)), 
        XmlInclude(typeof(OggVorbisSettings))
    ]
	public class AudioCodecSettings : MeGUI.core.plugins.interfaces.GenericSettings
	{
        public virtual void FixFileNames(Dictionary<string, string> _) {}
        public string getSettingsType()
        {
            return "Audio";
        }
		private ChannelMode downmixMode;
		private BitrateManagementMode bitrateMode;
		private int bitrate, delay;
		private bool delayEnabled, autoGain;
        private bool forceDirectShow;
        private bool improveAccuracy;
        private AudioCodec audioCodec;
        private AudioEncoderType audioEncoderType;

        [XmlIgnore()]
        public AudioCodec Codec
        {
            get { return audioCodec; }
            set { audioCodec = value; }
        }
        
        [XmlIgnore]
        public AudioEncoderType EncoderType
        {
            get { return audioEncoderType; }
            set { audioEncoderType = value; }
        }

		public AudioCodecSettings()
		{
			downmixMode = ChannelMode.KeepOriginal;
			bitrateMode = BitrateManagementMode.CBR;
			bitrate = 128;
			delay = 0;
			delayEnabled = false;
			autoGain = true;
            forceDirectShow = false;
            improveAccuracy = true;
		}

        public bool ImproveAccuracy
        {
            get { return improveAccuracy; }
            set { improveAccuracy = value; }
        }
	
	    public bool ForceDecodingViaDirectShow
	    {
            get { return forceDirectShow; }
            set { forceDirectShow = value; }
	    }
	    
		public ChannelMode DownmixMode
		{
			get {return downmixMode;}
			set {downmixMode = value;}
		}
		public virtual BitrateManagementMode BitrateMode
		{
			get {return bitrateMode;}
			set {bitrateMode = value;}
		}
		public virtual int Bitrate
		{
			get {return bitrate;}
			set {bitrate = value;}
		}
/*		public int Delay
		{
			get {return delay;}
			set {delay = value;}
		}*/
/*		public bool DelayEnabled
		{
			get {return delayEnabled;}
			set {delayEnabled = value;}
		}*/
		public bool AutoGain
		{
			get {return autoGain;}
			set {autoGain = value;}
		}
        /// <summary>
        /// generates a copy of this object
        /// </summary>
        /// <returns>the codec specific settings of this object</returns>
        public GenericSettings baseClone()
        {
            return clone();
        }
        
        public AudioCodecSettings clone()
        {
            // This method is sutable for all known descendants!
            return this.MemberwiseClone() as AudioCodecSettings;
        }

        public override bool Equals(object obj)
        {
            // This works for all known descendants
            return PropertyEqualityTester.Equals(this, obj);
        }

        ///// <summary>
        ///// Must return command line arguments string for command-line audio encoder
        ///// {0} means output file name
        ///// {1} means samplerate in Hz
        ///// {2} means bits per sample
        ///// {3} means channel count
        ///// {4} means samplecount
        ///// {5} means size in bytes
        ///// </summary>
        ///// <returns>command line arguments</returns>
        //public abstract string GetCommandLineArguments();

        ///// <summary>
        ///// Must return logical flag
        ///// </summary>
        //public abstract bool IsRawPcmSupportedByEncoder
        //{
        //    get;
        //}

        ///// <summary>
        ///// Must read executable path from settings
        ///// </summary>
        ///// <param name="from">settings to read path from</param>
        ///// <returns>path to encoder executable</returns>
        //public abstract string GetExecutablePath(MeGUISettings from);


        ///// <summary>
        ///// Return some additional code to be applied to the end of DSP
        ///// Example - 6==Audiochannels(last)?GetChannel(last,2,3,1,6,4,5):last{0}
        ///// </summary>
        //public abstract string AdjasmentAvisynthScript
        //{
        //    get;
        //}




        #region GenericSettings Members


        public string[] RequiredFiles
        {
            get { return new string[0]; }
        }

        public string[] RequiredProfiles
        {
            get { return new string[0]; }
        }

        #endregion
    }
}