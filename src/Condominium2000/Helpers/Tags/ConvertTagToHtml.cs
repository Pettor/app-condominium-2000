using System.Linq;
using System.Text.RegularExpressions;

namespace Condominium2000.Helpers.Tags
{
	public class ConvertTagToHtml
	{
		public static string ConvertInputToHtml(string inputString)
		{
			var result = inputString;

			if (inputString != null)
			{
				// Paragraph [p] --> <p>
				result = FindAndConvertTags.FindAndConvertParagraph(result);

				// Bold [b] --> <b>
				result = FindAndConvertTags.FindAndConvertBold(result);

				// Bold [i] --> <i>
				result = FindAndConvertTags.FindAndConvertItalic(result);

				// Bold [u] --> <u>
				result = FindAndConvertTags.FindAndConvertUnderline(result);

				// Bold [note] --> Bold and Red
				result = FindAndConvertTags.FindAndConvertNote(result);

				// Unordered list [list] --> <ul>
				result = FindAndConvertTags.FindAndConvertList(result, ListType.Unordered);

				// Ordered list [list=1] --> <ol>
				result = FindAndConvertTags.FindAndConvertList(result, ListType.Ordered);

				// [link|linktab] --> <a href>
				result = FindAndConvertTags.FindAndConvertLinks(result);

				// [map] --> <iframe ... googlemaps>
				result = FindAndConvertTags.FindAndConvertMap(result);

				// Breaks \r\n --> <br />
				var pattern = "\r\n";
				var replacement = "<br />";
				var rgx = new Regex(pattern);
				result = rgx.Replace(result, replacement);
			}

			return result;
		}

		/// <summary>
		///     Input MUST be a tag of type [list] ... text ... [/list]
		///     If start or endtag doesn't match function will not work
		/// </summary>
		/// <param name="listString"></param>
		/// <param name="listType"></param>
		/// <returns></returns>
		public static string ConvertListToHtml(string listString, ListType listType)
		{
			var finalString = listString;
			string htmlStartTag = null;
			string htmlEndTag = null;
			string replaceTag = null;

			// Different html tags depending on the listtype
			switch (listType)
			{
				case ListType.Ordered:
					{
						htmlStartTag = "<ol>";
						htmlEndTag = "</ol>";
						replaceTag = "[list=1]";
						break;
					}

				case ListType.Unordered:
					{
						htmlStartTag = "<ul>";
						htmlEndTag = "</ul>";
						replaceTag = "[list]";
						break;
					}
			}

			// If not valid values, return original string
			if (htmlStartTag == null)
			{ return finalString; }

			// Remove the end tag (it will be added later)
			listString = listString.Replace("[/list]", string.Empty);
			listString = listString.Trim('\r', '\n', ' ');

			// Split all list items [*]
			var words = Regex.Split(listString, @"\[\*\]", RegexOptions.Compiled).ToList();
			if (words.Count > 1)
			{
				// Insert ordinary HTML list tags
				for (var i = 1; i < words.Count; i++)
				{
					var word = words[i];
					word = word.Insert(0, "<li>");
					word = word.Insert(word.Length, "</li>");
					word = word.Trim('\r', '\n', ' ');
					words[i] = word;
				}

				words[0] = words[0].Trim('\r', '\n', ' ');
			}

			finalString = "";
			foreach (var word in words)
			{
				finalString += word;
			}

			// Insert and replace tags with HTML tags
			finalString = finalString.Insert(finalString.Length, htmlEndTag);
			finalString = finalString.Replace(replaceTag, htmlStartTag);

			return finalString;
		}

		public static string ConvertLinkToHtml(string linkString)
		{
			var result = linkString;

			// Split on " to get the webadress
			var linkUrlSplit = Regex.Split(linkString, @"\""", RegexOptions.Compiled).ToList();

			// A correctly parsed adress should have at least two hits
			if (linkUrlSplit.Count >= 2)
			{
				var linkUrl = linkUrlSplit[1];
				var linkText = "";
				var haveTab = false;

				// Now get the link text
				var linkTextMatches = Regex.Matches(result, @"\[(link|linktab)="".*""\]((.|\n)*?)\[\/link\]");
				if ((linkTextMatches.Count == 1) && (linkTextMatches[0].Groups.Count == 4))
				{
					// Get the linkText
					linkText = linkTextMatches[0].Groups[2].Value;

					// Check if link should open in new tab
					if (linkTextMatches[0].Groups[1].Value.Equals("linktab"))
					{
						haveTab = true;
					}
				}

				// Create the HTML tag
				result = @"<a href=""" + linkUrl + @"""" + (haveTab ? @" target=""_blank""" : "") + ">" + linkText + "</a>";
			}

			return result;
		}
	}
}