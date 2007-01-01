using System;
using System.Text;
using MeGUI.core.plugins.interfaces;
namespace MeGUI
{
	/// <summary>
	/// Summary description for AviSynthSettings.
	/// </summary>
    public sealed class AviSynthSettings : GenericSettings
	{
        public void FixFileNames(System.Collections.Generic.Dictionary<string, string> _) { }

        public override bool Equals(object obj)
        {
            return PropertyEqualityTester.Equals(this, obj);
        }

        public string getSettingsType()
        {
            return "AviSynth";
        }
        private string template;
        private ResizeFilterType resizeMethod;
        private DenoiseFilterType denoiseMethod;
        private mod16Method mod16Method;
		private bool deinterlace, denoise, ivtc, mpeg2deblock, colourCorrect;
        private bool resize;

        public GenericSettings baseClone()
        {
            return clone();
        }
        
        public AviSynthSettings clone()
        {
            return this.MemberwiseClone() as AviSynthSettings;
        }

        public mod16Method Mod16Method
        {
            get { return mod16Method; }
            set { mod16Method = value; }
        }
        
        public bool Resize
        {
            get { return resize; }
            set { resize = value; }
        }


		public string Template
		{
			get {return template;}
			set {
				string[] lines = value.Split('\r', '\n');
				StringBuilder script = new StringBuilder();
				script.EnsureCapacity(value.Length);
				foreach (string line in lines)
				{
					if (line.Length>0)
					{
						script.Append(line);
						script.Append("\r\n");
					}
				}
				template = script.ToString();}
		}
		public ResizeFilterType ResizeMethod
		{
			get {return resizeMethod;}
			set {resizeMethod = value;}
		}
		public DenoiseFilterType DenoiseMethod
		{
			get {return denoiseMethod;}
			set {denoiseMethod = value;}
		}
		public bool Deinterlace
		{
			get {return deinterlace;}
			set {deinterlace = value;}
		}
		public bool Denoise
		{
			get {return denoise;}
			set {denoise = value;}
		}
		public bool IVTC
		{
			get {return ivtc;}
			set {ivtc = value;}
		}
		public bool MPEG2Deblock
		{
			get {return mpeg2deblock;}
			set {mpeg2deblock = value;}
		}
		public bool ColourCorrect
		{
			get {return colourCorrect;}
			set {colourCorrect = value;}
		}

		public AviSynthSettings()
		{
			this.Template = "<input>\r\n<deinterlace>\r\n<crop>\r\n<resize>\r\n<denoise>\r\n"; // Default -- will act as it did before avs profiles
			this.ResizeMethod = ResizeFilterType.Lanczos; // Lanczos
			this.DenoiseMethod = 0; // UnDot
			this.Deinterlace = false;
			this.Denoise = false;
            this.resize = true;
			this.IVTC = false;
			this.MPEG2Deblock = false;
			this.ColourCorrect = true;
            this.Mod16Method = mod16Method.none;
		}

		public AviSynthSettings(string template, ResizeFilterType resizeMethod, bool resize,
			DenoiseFilterType denoiseMethod, bool denoise, bool mpeg2deblock, bool colourCorrect, mod16Method method)
		{
			this.Template = template;
			this.ResizeMethod = resizeMethod;
			this.DenoiseMethod = denoiseMethod;
			this.Deinterlace = deinterlace;
			this.Denoise = denoise;
			this.IVTC = ivtc;
			this.MPEG2Deblock = mpeg2deblock;
			this.ColourCorrect = colourCorrect;
            this.Mod16Method = method;
		}

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
