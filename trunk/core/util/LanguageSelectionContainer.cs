using System;
using System.Collections;
using System.Collections.Generic;

namespace MeGUI
{
	/// <summary>
	/// A container for the selectable languages in MeGUI
	/// </summary>
	public class LanguageSelectionContainer
	{
        private static readonly Dictionary<string,string> languages;
        private static readonly Dictionary<string, string> languagesReverse;

        public static Dictionary<string, string> Languages
        {
            get
            {
                return languages;
            }
        }


        private static void addLanguage(string name, string iso)
        {
            languages.Add(name, iso);
            languagesReverse.Add(iso, name);
        }

        static LanguageSelectionContainer()
        {
            languages = new Dictionary<string, string>();
            languagesReverse = new Dictionary<string, string>();

            addLanguage("Abkhazian", "abk");
            addLanguage("Achinese", "ace");
            addLanguage("Acoli", "ach");
            addLanguage("Adangme", "ada");
            addLanguage("Adygei (Adyghe)", "ady");
            addLanguage("Afar", "aar");
            addLanguage("Afrihili", "afh");
            addLanguage("Afrikaans", "afr");
            addLanguage("Afro-Asiatic (Other)", "afa");
            addLanguage("Akan", "aka");
            addLanguage("Akkadian", "akk");

            addLanguage("Albanian", "alb/sqi");
            addLanguage("Aleut", "ale");
            addLanguage("Algonquian languages", "alg");
            addLanguage("Altaic (Other)", "tut");
            addLanguage("Apache languages", "apa");
            addLanguage("Arabic", "ara");
            addLanguage("Aragonese", "arg");
            addLanguage("Aramaic", "arc");
            addLanguage("Arapaho", "arp");
            addLanguage("Araucanian", "arn");
            addLanguage("Arawak", "arw");
            addLanguage("Armenian", "arm/hye");
            addLanguage("Assamese", "ast");
            addLanguage("Athapascan languages", "art");
            addLanguage("Australian languages", "aus");
            addLanguage("Austronesian (Other)", "map");
            addLanguage("Avaric", "ava");
            addLanguage("Avestan", "ave");
            addLanguage("Awadhi", "awa");
            addLanguage("Aymara", "aym");
            addLanguage("Azerbaijani", "aze");

            addLanguage("Balinese", "ban");
            addLanguage("Bantu", "bnt");
            addLanguage("Basque", "baq");
            addLanguage("Breton", "bre");
            addLanguage("Bulgarian", "bul");
            addLanguage("Burmese", "bur");
            addLanguage("Catalan", "cat");
            addLanguage("Chinese", "chi");
            addLanguage("Corsican", "cos");
            addLanguage("Croatian", "hr");
            addLanguage("Czech", "ces");
            addLanguage("Danish", "dan");
            addLanguage("Dutch", "dut");
            addLanguage("English", "eng");
            addLanguage("Estonian", "est");
            addLanguage("Faroese", "fao");
            addLanguage("Finnish", "fin");
            addLanguage("French", "fre");
            addLanguage("Georgian", "geo");
            addLanguage("German", "ger");
            addLanguage("Greek", "gre");
            addLanguage("Hebrew", "heb");
            addLanguage("Hindi", "hin");
            addLanguage("Hungarian", "hun");
            addLanguage("Icelandic", "ice");
            addLanguage("Indonesian", "ind");
            addLanguage("Irish", "gai");
            addLanguage("Italian", "ita");
            addLanguage("Japanese", "jpn");
            addLanguage("Kashmiri", "kas");
            addLanguage("Kongo", "kon");
            addLanguage("Korean", "kor");
            addLanguage("Latvian", "lav");
            addLanguage("Macedonian", "mac");
            addLanguage("Maltese", "mlt");
            addLanguage("Moldavian", "mol");
            addLanguage("Mongolian", "mon");
            addLanguage("Norwegian", "nor");
            addLanguage("Persian", "per");
            addLanguage("Polish", "pol");
            addLanguage("Portuguese", "por");
            addLanguage("Romanian", "ron");
            addLanguage("Russian", "rus");
            addLanguage("Serbian", "sr");
            addLanguage("Serbo-Croatian", "sh");
            addLanguage("Slovak", "slk");
            addLanguage("Slovenian", "slv");
            addLanguage("Spanish", "spa");
            addLanguage("Swedish", "swe");
            addLanguage("Thai", "tha");
            addLanguage("Tibetan", "tib");
            addLanguage("Turkish", "tur");
            addLanguage("Uzbek", "uzb");
            addLanguage("Vietnamese", "vie");
            addLanguage("Zulu", "zun");


        }

		private LanguageSelectionContainer()
		{
		}

        public static string lookupISOCode(string code)
		{
            if (languagesReverse.ContainsKey(code))
                return languagesReverse[code];
            else
                return "";
		}
        /// <summary>
        /// takes an ISO639.2 3 letter language code and returns
        /// a 2 letter ISO639.1 language code
        /// </summary>
        /// <param name="iso639dot2"></param>
        /// <returns></returns>
        public static string getISO639dot1(string iso639dot2)
        {
            foreach (System.Globalization.CultureInfo ci in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures))
            {
                if (ci.ThreeLetterISOLanguageName == iso639dot2) // we found our language
                {
                    if (ci.TwoLetterISOLanguageName.Length == 2) // sometimes we get 3 letter codes here, divxmux can't handle those
                        return ci.TwoLetterISOLanguageName;
                }
            }
            return null;
        }
	}
}