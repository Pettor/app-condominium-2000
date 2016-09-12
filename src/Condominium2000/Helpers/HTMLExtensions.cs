using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Condominium2000.Helpers.Tags;
using Condominium2000.Helpers.Translator;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Condominium2000.Helpers
{
	public enum TableContent
	{
		Title,
		SubTitle,
		Content,
		HtmlContent,
		Position,
		Name,
		Caption
	}

	public static class HtmlExtensions
	{
		public static IHtmlString RenderTextWithBreak(this HtmlHelper htmlHelper, string text)
		{
			// Don't make newline when the HTML tag is a list
			const string pattern = Constants.RegexpBreakForHtml;
			const string replacement = "<br />";
			var rgx = new Regex(pattern);
			var result = rgx.Replace(text, replacement);

			return htmlHelper.Raw(result);
		}

		public static string Truncate(this HtmlHelper helper, string input, int length)
		{
			if (input.Length <= length)
			{
				return input;
			}
			return input.Substring(0, length) + "...";
		}

		public static MvcHtmlString SwitchTable(this HtmlHelper helper, string classEven, string classOdd, ref bool isEven)
		{
			var result = "<tr class=";

			if (isEven)
				result = result + classEven;
			else
				result = result + classOdd;

			isEven = !isEven;
			result = result + ">";
			return MvcHtmlString.Create(result);
		}

		#region QRCode

		/// <summary>
		///     Generates an img tag with a data uri encoded image of the QR code from the content given.
		/// </summary>
		/// <param name="html"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public static IHtmlString QrCode(this HtmlHelper html, string content)
		{
			var enc = new QrEncoder(ErrorCorrectionLevel.H);
			var code = enc.Encode(content);

			var r = new GraphicsRenderer(new FixedCodeSize(5, QuietZoneModules.Zero), Brushes.Black, Brushes.White);

			using (var ms = new MemoryStream())
			{
				r.WriteToStream(code.Matrix, ImageFormat.Png, ms);

				var image = ms.ToArray();

				return html.Raw($@"<img src=""data:image/png;base64,{Convert.ToBase64String(image)}"" alt=""{content}"" />");
			}
		}

		#endregion

		#region Language Helper

		/// <summary>
		///     Return content from Database table chosen  by user
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="obj">The class to fetch information from</param>
		/// <param name="tableContent">Which table to fetch information from</param>
		/// <returns></returns>
		public static string ReturnLanguageContent(this HtmlHelper helper, object obj, TableContent tableContent)
		{
			// Must contain space, else Exception if no method found!
			var result = " ";
			PropertyInfo prop;

			// Check language
			var lang = LanguageHelper.GetLanguage();

			// Fetch the property information depending on language
			prop = lang == LanguageHelper.Language.Sv ? obj.GetType().GetProperty(tableContent + "Sv") : obj.GetType().GetProperty(tableContent + "En");

			if (prop == null)
			{ return result; }

			// Fetch the getter method
			var getMethod = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), obj, prop.GetGetMethod());
			result = getMethod();

			return result;
		}

		#endregion

		#region Translation Helper

		/// <summary>
		///     Generate a translate Javascript
		///     REQUIRES HTML:
		///     - DIV[ajax_success]: When translation has finished
		///     - DIV[ajax_error]: When translation return error
		///     - DIV[ajax_preloader]: Div for loader during translation
		///     - FUNCTION[getObjSv]: Return object with swedish text obj { Lang, Title, SubTitle, Content }
		///     - FUNCTION[getObjEn]: Return object with english text obj { Lang, Title, SubTitle, Content }
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="translateRoute"></param>
		/// <returns></returns>
		public static HtmlString ReturnTranslationFunction(this HtmlHelper helper, string translateRoute)
		{
			var objectChanged = TranslationHelper.ObjectChangedScript();
			var ajaxInitChanged = TranslationHelper.AjaxInitScript();
			var callScript = TranslationHelper.FunctionCallScript(translateRoute);
			var getTranslateObject = TranslationHelper.GetTranslateObject();

			return
				new HtmlString("\n<script type='text/javascript'>\n" + objectChanged + "\n" + ajaxInitChanged + "\n" + callScript +
							   "\n" + getTranslateObject + "\n</script>\n");
		}

		#endregion

		#region Script Helper

		public static HtmlString ReturnPreviewWindowScript(this HtmlHelper helper, string translateRoute, string windowName)
		{
			return
				new HtmlString("MyWindow=window.open('" + translateRoute + "','" + windowName + "', 'width = " +
							   Constants.PreviewWindowWidth + ", height=" + Constants.PreviewWindowHeight + "'); return false;");
		}

		#endregion

		#region File Helper

		public static HtmlString DisplayFileFormatImage(this HtmlHelper htmlHelper, string format)
		{
			var displayClass = "society_table_file_pic_img";

			switch (format)
			{
				case "pdf":
					{
						displayClass = "society_table_file_pic_pdf";
						break;
					}

				case "word":
				case "excel":
					{
						displayClass = "society_table_file_pic_word";
						break;
					}
			}

			//<div class="society_table_file_pic_img"></div>
			return new HtmlString("<div class='" + displayClass + "'></div>");
		}

		#endregion

		#region Panel Helpers

		public static string RemovePanelTags(this HtmlHelper htmlHelper, string text)
		{
			return FindAndConvertTags.FindAndConvertLinksToNormalText(text);
		}

		#endregion

		#region ActionLink Helpers

		/// <summary>
		///     Create a action link that include language
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="obj"></param>
		/// <param name="tableContent"></param>
		/// <param name="actionName"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public static MvcHtmlString ActionLinkLanguage(this HtmlHelper htmlHelper, object obj, TableContent tableContent,
			string actionName, object htmlAttributes)
		{
			return htmlHelper.ActionLink(ReturnLanguageContent(htmlHelper, obj, tableContent), actionName, htmlAttributes);
		}

		public static MvcHtmlString ActionLinkLanguage(this HtmlHelper htmlHelper, object obj, TableContent tableContent,
			int truncateSize, string actionName, object htmlAttributes)
		{
			return htmlHelper.ActionLink(
				htmlHelper.Truncate(ReturnLanguageContent(htmlHelper, obj, tableContent), truncateSize), actionName, htmlAttributes);
		}

		public static MvcHtmlString ActionLinkLanguage(this HtmlHelper htmlHelper, object obj, TableContent tableContent,
			int truncateSize, string actionName, object routeValues, object htmlAttributes)
		{
			return htmlHelper.ActionLink(
				htmlHelper.Truncate(ReturnLanguageContent(htmlHelper, obj, tableContent), truncateSize), actionName, routeValues,
				htmlAttributes);
		}

		/// <summary>
		///     Create a action link takes the user back in history
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="linkText"></param>
		/// <returns></returns>
		public static MvcHtmlString ActionLinkBack(this HtmlHelper htmlHelper, string linkText)
		{
			return MvcHtmlString.Create(string.Format("<a onclick='javascript:history.go(-1)'>{0}</a>", linkText));
		}

		public static MvcHtmlString ActionLinkTop(this HtmlHelper htmlHelper, string linkText)
		{
			return MvcHtmlString.Create(string.Format("<div id='fl_top' onclick='window.scrollTo(0,0);'>{0}</div>", linkText));
		}

		#endregion

		#region Google Analytics

		/// <summary>
		///     Insert GA with an Urchin and domain name
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="urchin"></param>
		/// <param name="domainName"></param>
		/// <returns></returns>
		public static HtmlString ReturnAnalyticsScript(this HtmlHelper htmlHelper, string urchin, string domainName)
		{
			var sb = new StringBuilder();

			sb.Append("<script type='text/javascript'>\n");
			sb.Append(" var _gaq = _gaq || [];\n");
			sb.Append(" _gaq.push(['_setAccount', '" + urchin + "']);\n");
			sb.Append(" _gaq.push(['_setDomainName', '" + domainName + "']);\n");
			sb.Append(" _gaq.push(['_trackPageview']);\n");
			sb.Append(" (function() {\n");
			sb.Append("    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;\n");
			sb.Append(
				"    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';\n");
			sb.Append("    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);\n");
			sb.Append(" })();\n");
			sb.Append("</script>\n");

			return new HtmlString("\n" + sb);
		}

		/// <summary>
		///     Pull the urchin and domain name from Web.Config
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <returns></returns>
		public static HtmlString ReturnAnalyticsScript(this HtmlHelper htmlHelper)
		{
			// Pull values from Config
			var urchin = ConfigurationManager.AppSettings["ga-urchin"];
			var domainName = ConfigurationManager.AppSettings["ga-domainName"];
			return ReturnAnalyticsScript(htmlHelper, urchin, domainName);
		}

		#endregion
	}
}