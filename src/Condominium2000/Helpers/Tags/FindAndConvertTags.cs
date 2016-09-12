using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Condominium2000.Helpers.Tags
{
	public enum ListType
	{
		Ordered,
		Unordered
	}

	public class FindAndConvertTags
	{
		public static string FindAndConvertMap(string inputStr)
		{
			return inputStr.Replace("[map]",
				@"<iframe width=""320"" height=""240"" frameborder=""0"" scrolling=""no"" marginheight=""0"" marginwidth=""0"" class=""resident_map"" src=""https://maps.google.com/maps?f=q&amp;source=s_q&amp;hl=en&amp;geocode=&amp;q=Lindholmsall%C3%A9n+57,+G%C3%B6teborg,+Sverige&amp;aq=&amp;sll=57.708194,11.937718&amp;sspn=0.004683,0.013937&amp;ie=UTF8&amp;hq=&amp;hnear=Lindholmsall%C3%A9n+57,+417+55+G%C3%B6teborg,+Sweden&amp;ll=57.708254,11.937854&amp;spn=0.00466,0.013937&amp;t=m&amp;z=14&amp;output=embed""></iframe>");
		}

		public static string FindAndConvertParagraph(string inputStr)
		{
			return inputStr.Replace("[p]", "<p>").Replace("[/p]", "</p>");
		}

		public static string FindAndConvertBold(string inputStr)
		{
			return inputStr.Replace("[b]", "<b>").Replace("[/b]", "</b>");
		}

		public static string FindAndConvertItalic(string inputStr)
		{
			return inputStr.Replace("[i]", "<i>").Replace("[/i]", "</i>");
		}

		public static string FindAndConvertUnderline(string inputStr)
		{
			return inputStr.Replace("[u]", "<u>").Replace("[/u]", "</u>");
		}

		public static string FindAndConvertNote(string inputStr)
		{
			return inputStr.Replace("[note]", @"<font color=""red""><b>").Replace("[/note]", "</font></b>");
		}

		public static string FindAndConvertList(string inputStr, ListType listType)
		{
			var result = inputStr;
			var findListRegexp = "";

			switch (listType)
			{
				case ListType.Ordered:
				{
					findListRegexp = @"\[list\=1\]((.|\n)*?)\[/list\]";
					break;
				}

				case ListType.Unordered:
				{
					findListRegexp = @"\[list\]((.|\n)*?)\[/list\]";
					break;
				}
			}

			if (findListRegexp != null)
			{
				// Replace [list] tags
				var links = new List<string>();
				var diffIndex = 0;
				var diffIndexSummed = 0;
				foreach (Match match in Regex.Matches(result, findListRegexp))
				{
					var breakDiffIndex = 0;
					diffIndex = match.Groups[0].Index - diffIndexSummed;

					// Extract the current match
					var listString = ConvertTagToHtml.ConvertListToHtml(match.Groups[0].Value, listType);

					var sb = new StringBuilder(result);
					sb.Remove(diffIndex, match.Groups[0].Length);
					result = sb.Insert(diffIndex, listString).ToString();

					// Remove any possible breaks after list segment
					var stringEndIndex = diffIndex + listString.Length;
					var removeLength = 2;
					if (result.Length - stringEndIndex >= removeLength)
					{
						var subStr = result.Substring(stringEndIndex, removeLength);
						if (subStr.Equals("\r\n"))
						{
							result = result.Remove(stringEndIndex, removeLength);
							breakDiffIndex = removeLength;
						}
					}

					// How many characters were added / removed compared to original?
					diffIndex = match.Groups[0].Length - listString.Length;

					if (diffIndex >= 0)
					{
						diffIndex -= breakDiffIndex;
					}
					else
					{
						diffIndex += breakDiffIndex;
					}

					diffIndexSummed += diffIndex;
				}
			}

			return result;
		}

		public static string FindAndConvertLinks(string inputStr)
		{
			var result = inputStr;
			var inputStrLengt = inputStr.Length;

			// Replace [link] tags
			var links = new List<string>();
			var diffIndex = 0;
			var diffIndexSummed = 0;
			foreach (Match match in Regex.Matches(result, @"\[(link|linktab)="".*?""\]((.|\n)*?)\[\/link\]"))
			{
				diffIndex = match.Groups[0].Index - diffIndexSummed;

				// Extract the current match
				var listString = ConvertTagToHtml.ConvertLinkToHtml(match.Groups[0].Value);

				var sb = new StringBuilder(result);
				sb.Remove(diffIndex, match.Groups[0].Length);
				result = sb.Insert(diffIndex, listString).ToString();

				// How many characters were added / removed compared to original?
				diffIndexSummed += match.Groups[0].Length - listString.Length;
			}

			return result;
		}

		public static string FindAndConvertLinksToNormalText(string inputStr)
		{
			var result = inputStr;

			var diffIndex = 0;
			var diffIndexSummed = 0;
			foreach (Match match in Regex.Matches(result, @"(\<a href="".*?""\>)((.|\n)*?)(<\/a>)"))
			{
				diffIndex = match.Groups[1].Index - diffIndexSummed;

				var sb = new StringBuilder(result);
				var listString = "<b>";
				sb.Remove(diffIndex, match.Groups[1].Length);
				result = sb.Insert(diffIndex, listString).ToString();

				// How many characters were added / removed compared to original?
				diffIndexSummed += match.Groups[1].Length - listString.Length;

				// Do the same for </a> endtag
				diffIndex = match.Groups[4].Index - diffIndexSummed;

				sb = new StringBuilder(result);
				listString = "</b>";
				sb.Remove(diffIndex, match.Groups[4].Length);
				result = sb.Insert(diffIndex, listString).ToString();

				// How many characters were added / removed compared to original?
				diffIndexSummed += match.Groups[4].Length - listString.Length;
			}

			return result;
		}
	}
}