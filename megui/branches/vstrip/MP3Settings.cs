using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MP3Settings.
	/// </summary>
	public class MP3Settings : AudioCodecSettings
	{
		int quality;
		public MP3Settings():base()
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