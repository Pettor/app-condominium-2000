// Copyright (c) 2010 Ravi Bhavnani
// License: Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx

using System;
using System.Collections.Generic;

namespace Condominium2000.Helpers.Translator
{
	/// <summary>
	///     Translates text using Google's online language tools.
	/// </summary>
	public class Translator : WebResourceProvider
	{
		#region Fields

		/// <summary>
		///     The language to translation mode map.
		/// </summary>
		private static Dictionary<string, string> _languageModeMap;

		#endregion

		#region Constructor

		/// <summary>
		///     Initializes a new instance of the <see cref="Translator" /> class.
		/// </summary>
		public Translator()
		{
			SourceLanguage = "Swedish";
			TargetLanguage = "English";
			Referer = "http://www.google.com";
		}

		#endregion

		#region Public methods

		/// <summary>
		///     Attempts to translate the text.
		/// </summary>
		public void Translate()
		{
			// Validate source and target languages
			if (string.IsNullOrEmpty(SourceLanguage) ||
			    string.IsNullOrEmpty(TargetLanguage) ||
			    SourceLanguage.Trim().Equals(TargetLanguage.Trim()))
			{
				throw new Exception("An invalid source or target language was specified.");
			}

			// Delegate to base class
			FetchResource();
		}

		#endregion

		#region Private methods

		/// <summary>
		///     Converts a language to its identifier.
		/// </summary>
		/// <param name="language">The language."</param>
		/// <returns>The identifier or <see cref="string.Empty" /> if none.</returns>
		private static string LanguageEnumToIdentifier
			(string language)
		{
			if (_languageModeMap == null)
			{
				_languageModeMap = new Dictionary<string, string>
				{
					{"Afrikaans", "af"},
					{"Albanian", "sq"},
					{"Arabic", "ar"},
					{"Belarusian", "be"},
					{"Bulgarian", "bg"},
					{"Catalan", "ca"},
					{"Chinese", "zh-CN"},
					{"Croatian", "hr"},
					{"Czech", "cs"},
					{"Danish", "da"},
					{"Dutch", "nl"},
					{"English", "en"},
					{"Estonian", "et"},
					{"Filipino", "tl"},
					{"Finnish", "fi"},
					{"French", "fr"},
					{"Galician", "gl"},
					{"German", "de"},
					{"Greek", "el"},
					{"Haitian Creole ALPHA", "ht"},
					{"Hebrew", "iw"},
					{"Hindi", "hi"},
					{"Hungarian", "hu"},
					{"Icelandic", "is"},
					{"Indonesian", "id"},
					{"Irish", "ga"},
					{"Italian", "it"},
					{"Japanese", "ja"},
					{"Korean", "ko"},
					{"Latvian", "lv"},
					{"Lithuanian", "lt"},
					{"Macedonian", "mk"},
					{"Malay", "ms"},
					{"Maltese", "mt"},
					{"Norwegian", "no"},
					{"Persian", "fa"},
					{"Polish", "pl"},
					{"Portuguese", "pt"},
					{"Romanian", "ro"},
					{"Russian", "ru"},
					{"Serbian", "sr"},
					{"Slovak", "sk"},
					{"Slovenian", "sl"},
					{"Spanish", "es"},
					{"Swahili", "sw"},
					{"Swedish", "sv"},
					{"Thai", "th"},
					{"Turkish", "tr"},
					{"Ukrainian", "uk"},
					{"Vietnamese", "vi"},
					{"Welsh", "cy"},
					{"Yiddish", "yi"}
				};
			}
			string mode;
			_languageModeMap.TryGetValue(language, out mode);
			return mode;
		}

		#endregion

		#region Properties

		/// <summary>
		///     Gets or sets the source "
		/// </summary>
		/// <value>The source "</value>
		public string SourceLanguage { get; set; }

		/// <summary>
		///     Gets or sets the target "
		/// </summary>
		/// <value>The target "</value>
		public string TargetLanguage { get; set; }

		/// <summary>
		///     Gets or sets the source text.
		/// </summary>
		/// <value>The source text.</value>
		public string SourceText { get; set; }

		/// <summary>
		///     Gets the translation.
		/// </summary>
		/// <value>The translated text.</value>
		public string Translation { get; private set; }

		/// <summary>
		///     Gets the reverse translation.
		/// </summary>
		/// <value>The reverse translated text.</value>
		public string ReverseTranslation { get; private set; }

		#endregion

		#region WebResourceProvider implementation

		/// <summary>
		///     Returns the url to be fetched.
		/// </summary>
		/// <returns>The url to be fetched.</returns>
		protected override string GetFetchUrl()
		{
			return "http://translate.google.com/translate_t";
		}

		/// <summary>
		///     Retrieves the POST data (if any) to be sent to the url to be fetched.
		///     The data is returned as a string of the form "arg=val[&arg=val]...".
		/// </summary>
		/// <returns>A string containing the POST data or null if none.</returns>
		protected override string GetPostData()
		{
			// Set translation mode
			var strPostData =
				$"hl=sv&ie=UTF8&oe=UTF8submit=Translate&langpair={LanguageEnumToIdentifier(SourceLanguage)}|{LanguageEnumToIdentifier(TargetLanguage)}";

			// Set text to be translated
			strPostData += "&text=\"" + SourceText + "\"";
			return strPostData;
		}

		public void ParseAwesome(string apa)
		{
			// Initialize the scraper
			Translation = string.Empty;
			var strContent = apa;
			var parser = new StringParser(strContent);

			// Scrape the translation
			var strTranslation = string.Empty;
			if (parser.SkipToEndOf("<span id=result_box"))
			{
				if (parser.SkipToEndOf("onmouseout=\"this.style.backgroundColor='#fff'\">"))
				{
					if (parser.ExtractTo("</span>", ref strTranslation))
					{
						strTranslation = StringParser.RemoveHtml(strTranslation);
					}
				}
			}

			#region Fix up the translation

			var startClean = 0;
			var endClean = 0;
			var i = 0;
			while (i < strTranslation.Length)
			{
				if (char.IsLetterOrDigit(strTranslation[i]))
				{
					startClean = i;
					break;
				}
				i++;
			}
			i = strTranslation.Length - 1;
			while (i > 0)
			{
				var ch = strTranslation[i];
				if (char.IsLetterOrDigit(ch) ||
				    (char.IsPunctuation(ch) && (ch != '\"')))
				{
					endClean = i;
					break;
				}
				i--;
			}
			Translation = strTranslation.Substring(startClean, endClean - startClean + 1).Replace("\"", "");

			#endregion
		}

		/// <summary>
		///     Parses the fetched content.
		/// </summary>
		protected override void ParseContent()
		{
			// Initialize the scraper
			Translation = string.Empty;
			var strContent = Content;
			var parser = new StringParser(strContent);

			// Scrape the translation
			var strTranslation = string.Empty;
			if (parser.SkipToEndOf("<span id=result_box"))
			{
				if (parser.SkipToEndOf("onmouseout=\"this.style.backgroundColor='#fff'\">"))
				{
					if (parser.ExtractTo("</span>", ref strTranslation))
					{
						strTranslation = StringParser.RemoveHtml(strTranslation);
					}
				}
			}

			#region Fix up the translation

			var startClean = 0;
			var endClean = 0;
			var i = 0;
			while (i < strTranslation.Length)
			{
				if (char.IsLetterOrDigit(strTranslation[i]))
				{
					startClean = i;
					break;
				}
				i++;
			}
			i = strTranslation.Length - 1;
			while (i > 0)
			{
				var ch = strTranslation[i];
				if (char.IsLetterOrDigit(ch) ||
				    (char.IsPunctuation(ch) && (ch != '\"')))
				{
					endClean = i;
					break;
				}
				i--;
			}
			Translation = strTranslation.Substring(startClean, endClean - startClean + 1).Replace("\"", "");

			#endregion
		}

		#endregion
	}
}