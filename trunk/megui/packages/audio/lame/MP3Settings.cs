using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MP3Settings.
	/// </summary>
	public class MP3Settings : AudioCodecSettings
	{
        public static readonly string ID = "LAME MP3";
		int quality;
		public MP3Settings():base(ID)
		{
			quality = 50;
            Codec = AudioCodec.MP3;
            EncoderType = AudioEncoderType.LAME;
		}
		/// <summary>
		/// gets / sets the quality for vbr mode
		/// </summary>
		public int Quality
		{
			get {return quality;}
			set {quality = value;}
		}
	}
}
