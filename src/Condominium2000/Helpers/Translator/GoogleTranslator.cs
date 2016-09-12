using System;
using System.Net;
using System.Text;

namespace Condominium2000.Helpers.Translator
{
	public class GoogleTranslator
	{
		#region Constructor

		/// <summary>
		///     Initializes a new instance of the <see cref="GoogleTranslator" /> class.
		/// </summary>
		/// <param name="sourceLang"></param>
		/// <param name="targetLang"></param>
		public GoogleTranslator(LanguageHelper.Language sourceLang, LanguageHelper.Language targetLang)
		{
			SourceLanguage = sourceLang.ToString();
			TargetLanguage = targetLang.ToString();
		}

		#endregion

		#region Private Declarations

		/// <summary>
		///     The language that is going to be converted
		/// </summary>
		private string SourceLanguage { get; }

		/// <summary>
		///     The language to convert to
		/// </summary>
		private string TargetLanguage { get; }

		#endregion

		#region Public Declarations

		/// <summary>
		///     The fields that is translated
		/// </summary>
		public enum TranslateField
		{
			Title,
			SubTitle,
			Content
		}

		/// <summary>
		///     Object container for translations fields
		/// </summary>
		public class TranslateObject
		{
			public string Lang { get; set; }
			public string Title { get; set; }
			public string SubTitle { get; set; }
			public string Content { get; set; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		///     Translate the given text
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public string TranslateText(string text)
		{
			var result = "";
			var webClient = new WebClient();

			// Setup the URL parameters
			var url = Constants.TranslationUrlGoogleTranslate;

			url += string.Format("hl=en&ie=UTF8&oe=UTF8submit=Translate&langpair={0}|{1}",
				SourceLanguage,
				TargetLanguage);

			// Set text to be translated
			url += "&text=\"" + text + "\"";

			// Add headers and Encoding (important for Swedish)
			webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
			webClient.Encoding = Encoding.UTF8;

			try
			{
				// Download the result from the translate URL
				result = webClient.DownloadString(url);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Function: " + GetType().Name + @" Exception: " + e.InnerException);
			}


			return result;
		}

		/// <summary>
		///     Parse the downloaded content
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public string ParseContent(string text)
		{
			var parser = new StringParser(text);

			// Scrape the translation
			var strTranslation = string.Empty;
			if (parser.SkipToEndOf("<span id=result_box"))
			{
				// Contain each individual parsed string
				var parsedString = string.Empty;

				// Reached the result box
				while (parser.SkipToEndOf("onmouseout=\"this.style.backgroundColor='#fff'\">"))
				{
					// Extract all content to the span field
					if (parser.ExtractTo("</span>", ref parsedString))
					{
						// Remove HTML fields from the text
						strTranslation += StringParser.RemoveHtml(parsedString);
					}
				}
			}

			// Fix tags that are not correctly parsed
			strTranslation = StringParser.RemoveBadTagFormatting(strTranslation);

			// Clean up the translated string
			var startClean = 0;
			var endClean = 0;
			var i = 0;

			// Clean up unwanted character in the beginning of the string
			while (i < strTranslation.Length)
			{
				var selChar = strTranslation[i];
				if (char.IsLetterOrDigit(selChar) || selChar.Equals('[') || selChar.Equals(']'))
				{
					startClean = i;
					break;
				}
				i++;
			}
			i = strTranslation.Length - 1;

			// Clean up unwanted character in the end of the string
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

			return strTranslation.Substring(startClean, endClean - startClean + 1);
		}

		#endregion
	}
}