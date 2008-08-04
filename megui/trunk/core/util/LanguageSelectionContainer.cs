// ****************************************************************************
// 
// Copyright (C) 2005-2008  Doom9 & al
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

            addLanguage("English", "eng");
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
            addLanguage("Albanian", "alb");
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
            addLanguage("Armenian", "arm");
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
            addLanguage("Belarusian", "bel");
            addLanguage("Bosnian", "bos");
            addLanguage("Breton", "bre");
            addLanguage("Bulgarian", "bul");
            addLanguage("Burmese", "bur");
            addLanguage("Catalan", "cat");
            addLanguage("Chinese", "chi");
            addLanguage("Corsican", "cos");
            addLanguage("Croatian", "scr");
            addLanguage("Czech", "cze");
            addLanguage("Danish", "dan");
            addLanguage("Dutch", "dut");            
            addLanguage("Estonian", "est");
            addLanguage("Faroese", "fao");
            addLanguage("Finnish", "fin");
            addLanguage("Français", "fra");
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
            addLanguage("Lithuanian", "lit");
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
            addLanguage("Serbian", "scc");
            addLanguage("Slovak", "slk");
            addLanguage("Slovenian", "slv");
            addLanguage("Spanish", "spa");
            addLanguage("Swahili", "swa");
            addLanguage("Swedish", "swe");
            addLanguage("Thai", "tha");
            addLanguage("Tibetan", "tib");
            addLanguage("Turkish", "tur");
            addLanguage("Ukrainian", "ukr");
            addLanguage("Uzbek", "uzb");
            addLanguage("Vietnamese", "vie");
            addLanguage("Zhuang", "zha");
            addLanguage("Zulu", "zul");
            addLanguage("Zuni", "zun");
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