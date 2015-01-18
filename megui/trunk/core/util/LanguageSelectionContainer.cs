// ****************************************************************************
// 
// Copyright (C) 2005-2015 Doom9 & al
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
        // used by all tools except MP4box
        private static readonly Dictionary<string, string> languagesBibliographic; 
        private static readonly Dictionary<string, string> languagesReverseBibliographic;

        // used by MP4box
        private static readonly Dictionary<string, string> languagesTerminology;
        private static readonly Dictionary<string, string> languagesReverseTerminology;

        /// <summary>
        /// uses the ISO 639-2/B language codes
        /// </summary>
        public static Dictionary<string, string> Languages
        {
            get
            {
                return languagesBibliographic;
            }
        }

        /// <summary>
        /// uses the ISO 639-2/T language codes
        /// </summary>
        public static Dictionary<string, string> LanguagesTerminology
        {
            get
            {
                return languagesTerminology;
            }
        }

        private static void addLanguageB(string name, string iso)
        {
            languagesBibliographic.Add(name, iso);
            languagesReverseBibliographic.Add(iso, name);
        }

        private static void addLanguageT(string name, string iso)
        {
            languagesTerminology.Add(name, iso);
            languagesReverseTerminology.Add(iso, name);
        }

        static LanguageSelectionContainer()
        {
            // http://www.loc.gov/standards/iso639-2/php/code_list.php

            languagesBibliographic = new Dictionary<string, string>();
            languagesReverseBibliographic = new Dictionary<string, string>();

            languagesTerminology = new Dictionary<string, string>();
            languagesReverseTerminology = new Dictionary<string, string>();

            addLanguageB("Abkhazian", "abk");
            addLanguageB("Achinese", "ace");
            addLanguageB("Acoli", "ach");
            addLanguageB("Adangme", "ada");
            addLanguageB("Adygei (Adyghe)", "ady");
            addLanguageB("Afar", "aar");
            addLanguageB("Afrihili", "afh");
            addLanguageB("Afrikaans", "afr");
            addLanguageB("Afro-Asiatic (Other)", "afa");
            addLanguageB("Akan", "aka");
            addLanguageB("Akkadian", "akk");
            addLanguageB("Albanian", "alb");
            addLanguageB("Aleut", "ale");
            addLanguageB("Algonquian languages", "alg");
            addLanguageB("Altaic (Other)", "tut");
            addLanguageB("Apache languages", "apa");
            addLanguageB("Arabic", "ara");
            addLanguageB("Aragonese", "arg");
            addLanguageB("Aramaic", "arc");
            addLanguageB("Arapaho", "arp");
            addLanguageB("Araucanian", "arn");
            addLanguageB("Arawak", "arw");
            addLanguageB("Armenian", "arm");
            addLanguageB("Assamese", "ast");
            addLanguageB("Athapascan languages", "art");
            addLanguageB("Australian languages", "aus");
            addLanguageB("Austronesian (Other)", "map");
            addLanguageB("Avaric", "ava");
            addLanguageB("Avestan", "ave");
            addLanguageB("Awadhi", "awa");
            addLanguageB("Aymara", "aym");
            addLanguageB("Azerbaijani", "aze");
            addLanguageB("Balinese", "ban");
            addLanguageB("Bantu", "bnt");
            addLanguageB("Basque", "baq");
            addLanguageB("Belarusian", "bel");
            addLanguageB("Bosnian", "bos");
            addLanguageB("Breton", "bre");
            addLanguageB("Bulgarian", "bul");
            addLanguageB("Burmese", "bur");
            addLanguageB("Catalan", "cat");
            addLanguageB("Chinese", "chi");
            addLanguageB("Corsican", "cos");
            addLanguageB("Croatian", "hrv");
            addLanguageB("Czech", "cze");
            addLanguageB("Danish", "dan");
            addLanguageB("Dutch", "dut");            
            addLanguageB("Estonian", "est");
            addLanguageB("Faroese", "fao");
            addLanguageB("Finnish", "fin");
            addLanguageB("French", "fre");
            addLanguageB("Georgian", "geo");
            addLanguageB("German", "ger");
            addLanguageB("Greek", "gre");
            addLanguageB("Hebrew", "heb");
            addLanguageB("Hindi", "hin");
            addLanguageB("Hungarian", "hun");
            addLanguageB("English", "eng");
            addLanguageB("Icelandic", "ice");
            addLanguageB("Indonesian", "ind");
            addLanguageB("Irish", "gai");
            addLanguageB("Italian", "ita");
            addLanguageB("Japanese", "jpn");
            addLanguageB("Kashmiri", "kas");
            addLanguageB("Kongo", "kon");
            addLanguageB("Korean", "kor");
            addLanguageB("Latvian", "lav");
            addLanguageB("Lithuanian", "lit");
            addLanguageB("Macedonian", "mac");
            addLanguageB("Malay", "may");
            addLanguageB("Maltese", "mlt");
            addLanguageB("Maori", "mao");
            addLanguageB("Moldavian", "mol");
            addLanguageB("Mongolian", "mon");
            addLanguageB("Norwegian", "nor");
            addLanguageB("Punjabi", "pan");
            addLanguageB("Persian", "per");
            addLanguageB("Polish", "pol");
            addLanguageB("Portuguese", "por");
            addLanguageB("Romanian", "rum");
            addLanguageB("Russian", "rus");
            addLanguageB("Serbian", "srp");
            addLanguageB("Slovak", "slo");
            addLanguageB("Slovenian", "slv");
            addLanguageB("Spanish", "spa");
            addLanguageB("Swahili", "swa");
            addLanguageB("Swedish", "swe");
            addLanguageB("Thai", "tha");
            addLanguageB("Tibetan", "tib");
            addLanguageB("Turkish", "tur");
            addLanguageB("Urdu", "urd");
            addLanguageB("Ukrainian", "ukr");
            addLanguageB("Uzbek", "uzb");
            addLanguageB("Vietnamese", "vie");
            addLanguageB("Welsh", "wel");
            addLanguageB("Zhuang", "zha");
            addLanguageB("Zulu", "zul");
            addLanguageB("Zuni", "zun");

            addLanguageT("Abkhazian", "abk");
            addLanguageT("Achinese", "ace");
            addLanguageT("Acoli", "ach");
            addLanguageT("Adangme", "ada");
            addLanguageT("Adygei (Adyghe)", "ady");
            addLanguageT("Afar", "aar");
            addLanguageT("Afrihili", "afh");
            addLanguageT("Afrikaans", "afr");
            addLanguageT("Afro-Asiatic (Other)", "afa");
            addLanguageT("Akan", "aka");
            addLanguageT("Akkadian", "akk");
            addLanguageT("Albanian", "sqi");
            addLanguageT("Aleut", "ale");
            addLanguageT("Algonquian languages", "alg");
            addLanguageT("Altaic (Other)", "tut");
            addLanguageT("Apache languages", "apa");
            addLanguageT("Arabic", "ara");
            addLanguageT("Aragonese", "arg");
            addLanguageT("Aramaic", "arc");
            addLanguageT("Arapaho", "arp");
            addLanguageT("Araucanian", "arn");
            addLanguageT("Arawak", "arw");
            addLanguageT("Armenian", "hye");
            addLanguageT("Assamese", "ast");
            addLanguageT("Athapascan languages", "art");
            addLanguageT("Australian languages", "aus");
            addLanguageT("Austronesian (Other)", "map");
            addLanguageT("Avaric", "ava");
            addLanguageT("Avestan", "ave");
            addLanguageT("Awadhi", "awa");
            addLanguageT("Aymara", "aym");
            addLanguageT("Azerbaijani", "aze");
            addLanguageT("Balinese", "ban");
            addLanguageT("Bantu", "bnt");
            addLanguageT("Basque", "eus");
            addLanguageT("Belarusian", "bel");
            addLanguageT("Bosnian", "bos");
            addLanguageT("Breton", "bre");
            addLanguageT("Bulgarian", "bul");
            addLanguageT("Burmese", "mya");
            addLanguageT("Catalan", "cat");
            addLanguageT("Chinese", "zho");
            addLanguageT("Corsican", "cos");
            addLanguageT("Croatian", "hrv");
            addLanguageT("Czech", "ces");
            addLanguageT("Danish", "dan");
            addLanguageT("Dutch", "nld");
            addLanguageT("Estonian", "est");
            addLanguageT("Faroese", "fao");
            addLanguageT("Finnish", "fin");
            addLanguageT("French", "fra");
            addLanguageT("Georgian", "kat");
            addLanguageT("German", "deu");
            addLanguageT("Greek", "ell");
            addLanguageT("Hebrew", "heb");
            addLanguageT("Hindi", "hin");
            addLanguageT("Hungarian", "hun");
            addLanguageT("English", "eng");
            addLanguageT("Icelandic", "isl");
            addLanguageT("Indonesian", "ind");
            addLanguageT("Irish", "gai");
            addLanguageT("Italian", "ita");
            addLanguageT("Japanese", "jpn");
            addLanguageT("Kashmiri", "kas");
            addLanguageT("Kongo", "kon");
            addLanguageT("Korean", "kor");
            addLanguageT("Latvian", "lav");
            addLanguageT("Lithuanian", "lit");
            addLanguageT("Macedonian", "mkd");
            addLanguageT("Malay", "msa");
            addLanguageT("Maltese", "mlt");
            addLanguageT("Maori", "mri");
            addLanguageT("Moldavian", "mol");
            addLanguageT("Mongolian", "mon");
            addLanguageT("Norwegian", "nor");
            addLanguageT("Punjabi", "pan");
            addLanguageT("Persian", "fas");
            addLanguageT("Polish", "pol");
            addLanguageT("Portuguese", "por");
            addLanguageT("Romanian", "ron");
            addLanguageT("Russian", "rus");
            addLanguageT("Serbian", "srp");
            addLanguageT("Slovak", "slk");
            addLanguageT("Slovenian", "slv");
            addLanguageT("Spanish", "spa");
            addLanguageT("Swahili", "swa");
            addLanguageT("Swedish", "swe");
            addLanguageT("Thai", "tha");
            addLanguageT("Tibetan", "bod");
            addLanguageT("Turkish", "tur");
            addLanguageT("Urdu", "urd");
            addLanguageT("Ukrainian", "ukr");
            addLanguageT("Uzbek", "uzb");
            addLanguageT("Vietnamese", "vie");
            addLanguageT("Welsh", "cym");
            addLanguageT("Zhuang", "zha");
            addLanguageT("Zulu", "zul");
            addLanguageT("Zuni", "zun");
        }

		private LanguageSelectionContainer()
		{
		}

        public static string lookupISOCode(string code)
		{
            if (languagesReverseBibliographic.ContainsKey(code))
                return languagesReverseBibliographic[code];
            else if (languagesReverseTerminology.ContainsKey(code))
                return languagesReverseTerminology[code];
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
                    //if (ci.TwoLetterISOLanguageName.Length == 2) // sometimes we get 3 letter codes here, divxmux can't handle those
                        return ci.TwoLetterISOLanguageName;
                }
            }
            return null;
        }

        ///<summary>
        ///Convert the 2 char strings to the full language name
        ///</summary>
        ///
        public static string Short2FullLanguageName(string LngCode)
        {
            string Language = "";
            switch (LngCode.ToLower(System.Globalization.CultureInfo.InvariantCulture))
            {
                case "aa": Language = "Afar"; break;
                case "ab": Language = "Abkhazian"; break;
                case "af": Language = "Afrikaans"; break;
                case "am": Language = "Amharic"; break;
                case "ar": Language = "Arabic"; break;
                case "as": Language = "Assamese"; break;
                case "ay": Language = "Aymara"; break;
                case "az": Language = "Azerbaijani"; break;
                case "ba": Language = "Bashkir"; break;
                case "be": Language = "Byelorussian"; break;
                case "bg": Language = "Bulgarian"; break;
                case "bh": Language = "Bihari"; break;
                case "bi": Language = "Bislama"; break;
                case "bn": Language = "Bengali; Bangla"; break;
                case "bo": Language = "Tibetan"; break;
                case "br": Language = "Breton"; break;
                case "ca": Language = "Catalan"; break;
                case "co": Language = "Corsican"; break;
                case "cs": Language = "Czech"; break;
                case "cy": Language = "Welsh"; break;
                case "da": Language = "Danish"; break;
                case "de": Language = "German"; break;
                case "dz": Language = "Bhutani"; break;
                case "el": Language = "Greek"; break;
                case "en": Language = "English"; break;
                case "eo": Language = "Esperanto"; break;
                case "es": Language = "Spanish"; break;
                case "et": Language = "Estonian"; break;
                case "eu": Language = "Basque"; break;
                case "fa": Language = "Persian"; break;
                case "fi": Language = "Finnish"; break;
                case "fj": Language = "Fiji"; break;
                case "fo": Language = "Faroese"; break;
                case "fr": Language = "French"; break;
                case "fy": Language = "Frisian"; break;
                case "ga": Language = "Irish"; break;
                case "gd": Language = "Scots Gaelic"; break;
                case "gl": Language = "Galician"; break;
                case "gn": Language = "Guarani"; break;
                case "gu": Language = "Gujarati"; break;
                case "ha": Language = "Hausa"; break;
                case "he": Language = "Hebrew (formerly iw)"; break;
                case "hi": Language = "Hindi"; break;
                case "hr": Language = "Croatian"; break;
                case "hu": Language = "Hungarian"; break;
                case "hy": Language = "Armenian"; break;
                case "ia": Language = "Interlingua"; break;
                case "id": Language = "Indonesian (formerly in)"; break;
                case "ie": Language = "Interlingue"; break;
                case "ik": Language = "Inupiak"; break;
                case "is": Language = "Icelandic"; break;
                case "it": Language = "Italian"; break;
                case "iu": Language = "Inuktitut"; break;
                case "iw": Language = "Hebrew"; break;
                case "ja": Language = "Japanese"; break;
                case "jw": Language = "Javanese"; break;
                case "ka": Language = "Georgian"; break;
                case "kk": Language = "Kazakh"; break;
                case "kl": Language = "Greenlandic"; break;
                case "km": Language = "Cambodian"; break;
                case "kn": Language = "Kannada"; break;
                case "ko": Language = "Korean"; break;
                case "ks": Language = "Kashmiri"; break;
                case "ku": Language = "Kurdish"; break;
                case "ky": Language = "Kirghiz"; break;
                case "la": Language = "Latin"; break;
                case "ln": Language = "Lingala"; break;
                case "lo": Language = "Laothian"; break;
                case "lt": Language = "Lithuanian"; break;
                case "lv": Language = "Latvian, Lettish"; break;
                case "mg": Language = "Malagasy"; break;
                case "mi": Language = "Maori"; break;
                case "mk": Language = "Macedonian"; break;
                case "ml": Language = "Malayalam"; break;
                case "mn": Language = "Mongolian"; break;
                case "mo": Language = "Moldavian"; break;
                case "mr": Language = "Marathi"; break;
                case "ms": Language = "Malay"; break;
                case "mt": Language = "Maltese"; break;
                case "my": Language = "Burmese"; break;
                case "na": Language = "Nauru"; break;
                case "ne": Language = "Nepali"; break;
                case "nl": Language = "Dutch"; break;
                case "no": Language = "Norwegian"; break;
                case "oc": Language = "Occitan"; break;
                case "om": Language = "(Afan) Oromo"; break;
                case "or": Language = "Oriya"; break;
                case "pa": Language = "Punjabi"; break;
                case "pl": Language = "Polish"; break;
                case "ps": Language = "Pashto, Pushto"; break;
                case "pt": Language = "Portuguese"; break;
                case "qu": Language = "Quechua"; break;
                case "rm": Language = "Rhaeto-Romance"; break;
                case "rn": Language = "Kirundi"; break;
                case "ro": Language = "Romanian"; break;
                case "ru": Language = "Russian"; break;
                case "rw": Language = "Kinyarwanda"; break;
                case "sa": Language = "Sanskrit"; break;
                case "sd": Language = "Sindhi"; break;
                case "sg": Language = "Sangho"; break;
                case "sh": Language = "Serbo-Croatian"; break;
                case "si": Language = "Sinhalese"; break;
                case "sk": Language = "Slovak"; break;
                case "sl": Language = "Slovenian"; break;
                case "sm": Language = "Samoan"; break;
                case "sn": Language = "Shona"; break;
                case "so": Language = "Somali"; break;
                case "sq": Language = "Albanian"; break;
                case "sr": Language = "Serbian"; break;
                case "ss": Language = "Siswati"; break;
                case "st": Language = "Sesotho"; break;
                case "su": Language = "Sundanese"; break;
                case "sv": Language = "Swedish"; break;
                case "sw": Language = "Swahili"; break;
                case "ta": Language = "Tamil"; break;
                case "te": Language = "Telugu"; break;
                case "tg": Language = "Tajik"; break;
                case "th": Language = "Thai"; break;
                case "ti": Language = "Tigrinya"; break;
                case "tk": Language = "Turkmen"; break;
                case "tl": Language = "Tagalog"; break;
                case "tn": Language = "Setswana"; break;
                case "to": Language = "Tonga"; break;
                case "tr": Language = "Turkish"; break;
                case "ts": Language = "Tsonga"; break;
                case "tt": Language = "Tatar"; break;
                case "tw": Language = "Twi"; break;
                case "ug": Language = "Uighur"; break;
                case "uk": Language = "Ukrainian"; break;
                case "ur": Language = "Urdu"; break;
                case "uz": Language = "Uzbek"; break;
                case "vi": Language = "Vietnamese"; break;
                case "vo": Language = "Volapuk"; break;
                case "wo": Language = "Wolof"; break;
                case "xh": Language = "Xhosa"; break;
                case "yi": Language = "Yiddish (formerly ji)"; break;
                case "yo": Language = "Yoruba"; break;
                case "za": Language = "Zhuang"; break;
                case "zh": Language = "Chinese"; break;
                case "zu": Language = "Zulu"; break;
            }
            return Language;
        }
	}
}