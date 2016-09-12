using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Condominium2000.Helpers.Translator
{
	/// <summary>
	///     A class that helps you to extract information from a string.
	/// </summary>
	public class StringParser
	{
		///////////////////////////
		// Implementation (members)

		/// <summary>Content to be parsed.</summary>
		private string _mStrContent = "";

		/// <summary>Lower-cased version of content to be parsed.</summary>
		private string _mStrContentLc = "";

		/// <summary>
		///     Default constructor.
		/// </summary>
		public StringParser()
		{
		}

		/// <summary>
		///     Constructs a StringParser with specific content.
		/// </summary>
		/// <param name="strContent">The parser's content.</param>
		public StringParser
			(string strContent)
		{
			Content = strContent;
		}

		/////////////
		// Properties

		/// <summary>Gets and sets the content to be parsed.</summary>
		public string Content
		{
			get { return _mStrContent; }
			set
			{
				_mStrContent = value;
				_mStrContentLc = _mStrContent.ToLower();
				ResetPosition();
			}
		}

		/// <summary>Gets the parser's current position.</summary>
		public int Position { get; private set; }

		/////////////////
		// Static methods

		/// <summary>
		///     Retrieves the collection of HTML links in a string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <param name="strRootUrl">Root url (may be null).</param>
		/// <param name="documents">Collection of document link strings.</param>
		/// <param name="images">Collection of image link strings.</param>
		public static void GetLinks
			(string strString,
				string strRootUrl,
				ref ArrayList documents,
				ref ArrayList images)
		{
			// Remove comments and JavaScript and fix links
			strString = RemoveComments(strString);
			strString = RemoveScripts(strString);
			var parser = new StringParser(strString);
			parser.ReplaceEvery("\'", "\"");

			// Set root url
			var rootUrl = "";
			if (strRootUrl != null)
				rootUrl = strRootUrl.Trim();
			if ((rootUrl.Length > 0) && !rootUrl.EndsWith("/"))
				rootUrl += "/";

			// Extract HREF targets
			var strUrl = "";
			parser.ResetPosition();
			while (parser.SkipToEndOfNoCase("href=\""))
			{
				if (!parser.ExtractTo("\"", ref strUrl))
					continue;

				strUrl = strUrl.Trim();
				if (strUrl.Length <= 0)
					continue;

				if (strUrl.IndexOf("mailto:", StringComparison.Ordinal) != -1)
					continue;

				// Get fully qualified url (best guess)
				if (!strUrl.StartsWith("http://") && !strUrl.StartsWith("ftp://"))
				{
					try
					{
						var uriBuilder = new UriBuilder(rootUrl) { Path = strUrl };
						strUrl = uriBuilder.Uri.ToString();
					}
					catch (Exception)
					{
						strUrl = "http://" + strUrl;
					}
				}

				// Add url to document list if not already present
				if (!documents.Contains(strUrl))
					documents.Add(strUrl);
			}

			// Extract SRC targets
			parser.ResetPosition();
			while (parser.SkipToEndOfNoCase("src=\""))
			{
				if (parser.ExtractTo("\"", ref strUrl))
				{
					strUrl = strUrl.Trim();
					if (strUrl.Length > 0)
					{
						// Get fully qualified url (best guess)
						if (!strUrl.StartsWith("http://") && !strUrl.StartsWith("ftp://"))
						{
							try
							{
								var uriBuilder = new UriBuilder(rootUrl) { Path = strUrl };
								strUrl = uriBuilder.Uri.ToString();
							}
							catch (Exception)
							{
								strUrl = "http://" + strUrl;
							}
						}

						// Add url to images list if not already present
						if (!images.Contains(strUrl))
							images.Add(strUrl);
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="strString"></param>
		public static string RemoveBadTagFormatting(string strString)
		{
			const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
			var regex = new Regex(Constants.RegexpTranslatorTags, options);
			var matches = regex.Matches(strString);

			// Here we check the Match instance.
			if (matches.Count <= 0) return strString;
			for (var i = matches.Count - 1; i >= 0; i--)
			{
				// Get the match
				var match = matches[i];
				var index = match.Index;

				// Fix the formatting of the sub string
				var subStr = match.Value;
				subStr = subStr.Replace(" ", string.Empty);
				subStr = subStr.ToLower();

				// Only do this is substr has changes
				if (subStr.Equals(match.Value))
					continue;

				var sb = new StringBuilder(strString);
				if (index + subStr.Length > sb.Length)
				{
					// You could throw an exception here, or you could just
					// append to the end of the StringBuilder -- up to you.
					throw new ArgumentOutOfRangeException();
				}

				for (var j = 0; j < subStr.Length; ++j)
				{
					sb[index + j] = subStr[j];
				}
				strString = sb.ToString();

				// Remove the "remaining" char from string
				var diffLength = match.Value.Length - subStr.Length;
				if (diffLength > 0)
				{
					strString = strString.Remove(match.Index + subStr.Length, diffLength);
				}
			}
			return strString;
		}

		/// <summary>
		///     Removes all HTML comments from a string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>Comment-free version of string.</returns>
		public static string RemoveComments(string strString)
		{
			// Return comment-free version of string
			var strCommentFreeString = "";
			var strSegment = "";
			var parser = new StringParser(strString);

			while (parser.ExtractTo("<!--", ref strSegment))
			{
				strCommentFreeString += strSegment;
				if (!parser.SkipToEndOf("-->"))
					return strString;
			}

			parser.ExtractToEnd(ref strSegment);
			strCommentFreeString += strSegment;
			return strCommentFreeString;
		}

		/// <summary>
		///     Returns an unanchored version of a string, i.e. without the enclosing
		///     leftmost &lt;a...&gt; and rightmost &lt;/a&gt; tags.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>Unanchored version of string.</returns>
		public static string RemoveEnclosingAnchorTag(string strString)
		{
			var strStringLc = strString.ToLower();
			var nStart = strStringLc.IndexOf("<a", StringComparison.Ordinal);
			if (nStart == -1) return strString;
			nStart++;
			nStart = strStringLc.IndexOf(">", nStart, StringComparison.Ordinal);
			if (nStart == -1) return strString;
			nStart++;
			var nEnd = strStringLc.LastIndexOf("</a>", StringComparison.Ordinal);
			if (nEnd == -1) return strString;
			var strRet = strString.Substring(nStart, nEnd - nStart);
			return strRet;
		}

		/// <summary>
		///     Returns an unquoted version of a string, i.e. without the enclosing
		///     leftmost and rightmost double " characters.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>Unquoted version of string.</returns>
		public static string RemoveEnclosingQuotes
			(string strString)
		{
			var nStart = strString.IndexOf("\"", StringComparison.Ordinal);
			if (nStart != -1)
			{
				var nEnd = strString.LastIndexOf("\"", StringComparison.Ordinal);
				if (nEnd > nStart)
					return strString.Substring(nStart, nEnd - nStart - 1);
			}
			return strString;
		}

		/// <summary>
		///     Returns a version of a string without any HTML tags.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>Version of string without HTML tags.</returns>
		public static string RemoveHtml(string strString)
		{
			// Do some common case-sensitive replacements
			var replacements = new Hashtable
			{
				{"&nbsp;", " "},
				{"&amp;", "&"},
				{"&aring;", ""},
				{"&auml;", ""},
				{"&eacute;", ""},
				{"&iacute;", ""},
				{"&igrave;", ""},
				{"&ograve;", ""},
				{"&ouml;", ""},
				{"&quot;", @""""},
				{"&szlig;", ""},
				{"&lt;", "<"},
				{"&gt;", ">"}
			};

			var parser = new StringParser(strString);
			foreach (string key in replacements.Keys)
			{
				var val = replacements[key] as string;
				if (strString.IndexOf(key, StringComparison.Ordinal) != -1)
					parser.ReplaceEveryExact(key, val);
			}

			// Do some sequential replacements
			parser.ReplaceEveryExact("&#0", "&#");
			parser.ReplaceEveryExact("&#39;", "'");
			parser.ReplaceEveryExact("</", " <~/");
			parser.ReplaceEveryExact("<~/", "</");

			// Case-insensitive replacements
			replacements.Clear();
			replacements.Add("<br>", "\n");
			replacements.Add("<p>", " ");
			foreach (string key in replacements.Keys)
			{
				var val = replacements[key] as string;
				if (strString.IndexOf(key, StringComparison.Ordinal) != -1)
					parser.ReplaceEvery(key, val);
			}
			strString = parser.Content;

			// Remove space from tags
			var nIndex = 0;
			int nStartTag;
			while ((nStartTag = strString.IndexOf("<", nIndex, StringComparison.Ordinal)) != -1)
			{
				// Extract to start of tag
				nIndex = nStartTag + 1;

				var nEndTag = strString.IndexOf(">", nIndex, StringComparison.Ordinal);
				if (nEndTag == -1)
					break;
				nIndex = nEndTag + 1;
			}

			// Finally, reduce spaces
			parser.Content = strString;
			parser.ReplaceEveryExact("  ", " ");

			// Return the de-HTMLized string
			return strString;
		}

		/// <summary>
		///     Removes all scripts from a string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>Version of string without any scripts.</returns>
		public static string RemoveScripts(string strString)
		{
			// Get script-free version of content
			var strStringSansScripts = "";
			var strSegment = "";
			var parser = new StringParser(strString);

			while (parser.ExtractToNoCase("<script", ref strSegment))
			{
				strStringSansScripts += strSegment;
				if (!parser.SkipToEndOfNoCase("</script>"))
				{
					parser.Content = strStringSansScripts;
					return strString;
				}
			}

			parser.ExtractToEnd(ref strSegment);
			strStringSansScripts += strSegment;
			return strStringSansScripts;
		}

		/////////////
		// Operations

		/// <summary>
		///     Checks if the parser is case-sensitively positioned at the start
		///     of a string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>
		///     true if the parser is positioned at the start of the string, false
		///     otherwise.
		/// </returns>
		public bool At(string strString)
		{
			return _mStrContent.IndexOf(strString, Position, StringComparison.Ordinal) == Position;
		}

		/// <summary>
		///     Checks if the parser is case-insensitively positioned at the start
		///     of a string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>
		///     true if the parser is positioned at the start of the string, false
		///     otherwise.
		/// </returns>
		public bool AtNoCase(string strString)
		{
			strString = strString.ToLower();
			return _mStrContentLc.IndexOf(strString, Position, StringComparison.Ordinal) == Position;
		}

		/// <summary>
		///     Extracts the text from the parser's current position to the case-
		///     sensitive start of a string and advances the parser just after the
		///     string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <param name="strExtract">The extracted text.</param>
		/// <returns>true if the parser was advanced, false otherwise.</returns>
		public bool ExtractTo(string strString, ref string strExtract)
		{
			var nPos = _mStrContent.IndexOf(strString, Position, StringComparison.Ordinal);
			if (nPos == -1)
			{ return false; }

			strExtract = _mStrContent.Substring(Position, nPos - Position);
			Position = nPos + strString.Length;
			return true;
		}

		/// <summary>
		///     Extracts the text from the parser's current position to the case-
		///     insensitive start of a string and advances the parser just after the
		///     string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <param name="strExtract">The extracted text.</param>
		/// <returns>true if the parser was advanced, false otherwise.</returns>
		public bool ExtractToNoCase(string strString, ref string strExtract)
		{
			strString = strString.ToLower();
			var nPos = _mStrContentLc.IndexOf(strString, Position, StringComparison.Ordinal);
			if (nPos == -1)
			{ return false; }

			strExtract = _mStrContent.Substring(Position, nPos - Position);
			Position = nPos + strString.Length;
			return true;
		}

		/// <summary>
		///     Extracts the text from the parser's current position to the case-
		///     sensitive start of a string and position's the parser at the start
		///     of the string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <param name="strExtract">The extracted text.</param>
		/// <returns>true if the parser was advanced, false otherwise.</returns>
		public bool ExtractUntil(string strString, ref string strExtract)
		{
			var nPos = _mStrContent.IndexOf(strString, Position, StringComparison.Ordinal);
			if (nPos == -1)
			{ return false; }

			strExtract = _mStrContent.Substring(Position, nPos - Position);
			Position = nPos;
			return true;
		}

		/// <summary>
		///     Extracts the text from the parser's current position to the case-
		///     insensitive start of a string and position's the parser at the start
		///     of the string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <param name="strExtract">The extracted text.</param>
		/// <returns>true if the parser was advanced, false otherwise.</returns>
		public bool ExtractUntilNoCase(string strString, ref string strExtract)
		{
			strString = strString.ToLower();
			var nPos = _mStrContentLc.IndexOf(strString, Position, StringComparison.Ordinal);
			if (nPos == -1)
			{ return false; }

			strExtract = _mStrContent.Substring(Position, nPos - Position);
			Position = nPos;
			return true;
		}

		/// <summary>
		///     Extracts the text from the parser's current position to the end
		///     of its content and does not advance the parser's position.
		/// </summary>
		/// <param name="strExtract">The extracted text.</param>
		public void ExtractToEnd(ref string strExtract)
		{
			if (strExtract == null)
				throw new ArgumentNullException(nameof(strExtract));

			strExtract = "";
			if (Position >= _mStrContent.Length)
				return;

			var nRemainLen = _mStrContent.Length - Position;
			strExtract = _mStrContent.Substring(Position, nRemainLen);
		}

		/// <summary>
		///     Case-insensitively replaces every occurence of a string in the
		///     parser's content with another.
		/// </summary>
		/// <param name="strOccurrence">The occurrence.</param>
		/// <param name="strReplacement">The replacement string.</param>
		/// <returns>The number of occurences replaced.</returns>
		public int ReplaceEvery
			(string strOccurrence,
				string strReplacement)
		{
			// Initialize replacement process
			var nReplacements = 0;
			strOccurrence = strOccurrence.ToLower();

			// For every occurence...
			var nOccurrence = _mStrContentLc.IndexOf(strOccurrence, StringComparison.Ordinal);
			while (nOccurrence != -1)
			{
				// Create replaced substring
				var strReplacedString = _mStrContent.Substring(0, nOccurrence) + strReplacement;

				// Add remaining substring (if any)
				var nStartOfRemainingSubstring = nOccurrence + strOccurrence.Length;
				if (nStartOfRemainingSubstring < _mStrContent.Length)
				{
					var strSecondPart = _mStrContent.Substring(nStartOfRemainingSubstring,
						_mStrContent.Length - nStartOfRemainingSubstring);
					strReplacedString += strSecondPart;
				}

				// Update the original string
				_mStrContent = strReplacedString;
				_mStrContentLc = _mStrContent.ToLower();
				nReplacements++;

				// Find the next occurence
				nOccurrence = _mStrContentLc.IndexOf(strOccurrence, StringComparison.Ordinal);
			}
			return nReplacements;
		}

		/// <summary>
		///     Case sensitive version of replaceEvery()
		/// </summary>
		/// <param name="strOccurrence">The occurrence.</param>
		/// <param name="strReplacement">The replacement string.</param>
		/// <returns>The number of occurences replaced.</returns>
		public int ReplaceEveryExact(string strOccurrence, string strReplacement)
		{
			var nReplacements = 0;
			while (_mStrContent.IndexOf(strOccurrence, StringComparison.Ordinal) != -1)
			{
				_mStrContent = _mStrContent.Replace(strOccurrence, strReplacement);
				nReplacements++;
			}
			_mStrContentLc = _mStrContent.ToLower();
			return nReplacements;
		}

		/// <summary>
		///     Resets the parser's position to the start of the content.
		/// </summary>
		public void ResetPosition()
		{
			Position = 0;
		}

		/// <summary>
		///     Advances the parser's position to the start of the next case-sensitive
		///     occurence of a string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>true if the parser's position was advanced, false otherwise.</returns>
		public bool SkipToStartOf(string strString)
		{
			var bStatus = SeekTo(strString, false, false);
			return bStatus;
		}

		/// <summary>
		///     Advances the parser's position to the start of the next case-insensitive
		///     occurence of a string.
		/// </summary>
		/// <param name="strText">The string.</param>
		/// <returns>true if the parser's position was advanced, false otherwise.</returns>
		public bool SkipToStartOfNoCase(string strText)
		{
			var bStatus = SeekTo(strText, true, false);
			return bStatus;
		}

		/// <summary>
		///     Advances the parser's position to the end of the next case-sensitive
		///     occurence of a string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <returns>true if the parser's position was advanced, false otherwise.</returns>
		public bool SkipToEndOf(string strString)
		{
			var bStatus = SeekTo(strString, false, true);
			return bStatus;
		}

		/// <summary>
		///     Advances the parser's position to the end of the next case-insensitive
		///     occurence of a string.
		/// </summary>
		/// <param name="strText">The string.</param>
		/// <returns>true if the parser's position was advanced, false otherwise.</returns>
		public bool SkipToEndOfNoCase(string strText)
		{
			var bStatus = SeekTo(strText, true, true);
			return bStatus;
		}

		///////////////////////////
		// Implementation (methods)

		/// <summary>
		///     Advances the parser's position to the next occurence of a string.
		/// </summary>
		/// <param name="strString">The string.</param>
		/// <param name="bNoCase">Flag: perform a case-insensitive search.</param>
		/// <param name="bPositionAfter">Flag: position parser just after string.</param>
		/// <returns></returns>
		private bool SeekTo(string strString, bool bNoCase, bool bPositionAfter)
		{
			if (Position >= _mStrContent.Length)
				return false;

			// Find the start of the string - return if not found
			var nNewIndex = 0;
			if (bNoCase)
			{
				strString = strString.ToLower();
				nNewIndex = _mStrContentLc.IndexOf(strString, Position, StringComparison.Ordinal);
			}
			else
			{
				nNewIndex = _mStrContent.IndexOf(strString, Position, StringComparison.Ordinal);
			}
			if (nNewIndex == -1)
				return false;

			// Position the parser
			Position = nNewIndex;
			if (bPositionAfter)
				Position += strString.Length;
			return true;
		}
	}
}