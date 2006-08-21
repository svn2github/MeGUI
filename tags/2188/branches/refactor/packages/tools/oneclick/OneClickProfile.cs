using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class OneClickProfile : Profile
    {
		private OneClickSettings oneClickSettings;
		public OneClickSettings Settings
		{
			get {return oneClickSettings;}
			set {oneClickSettings = value;}
		}

		public OneClickProfile()
		{
			this.Name = "Default Profile";
			this.Settings = new OneClickSettings();
		}

		public OneClickProfile(string name, OneClickSettings settings)
		{
			this.Settings = settings;
			this.Name = name;
		}
    }
}