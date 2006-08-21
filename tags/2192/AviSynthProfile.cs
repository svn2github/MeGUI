using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for AviSynthProfile.
	/// </summary>
	public class AviSynthProfile : Profile
	{
		private AviSynthSettings avsSettings;
		public AviSynthSettings Settings
		{
			get {return avsSettings;}
			set {avsSettings = value;}
		}

		public AviSynthProfile()
		{
			this.Name = "Default Profile";
			this.Settings = new AviSynthSettings();
		}

		public AviSynthProfile(string name, AviSynthSettings settings)
		{
			this.Settings = settings;
			this.Name = name;
		}

	}
}