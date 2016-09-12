using System.Text;

namespace Condominium2000.Helpers.Translator
{
	public class TranslationHelper
	{
		public static GoogleTranslator.TranslateObject TranslateText(GoogleTranslator.TranslateObject obj)
		{
			var source = LanguageHelper.Language.Sv;
			var target = LanguageHelper.Language.En;

			if (obj.Lang == null)
			{
				return obj;
			}
			if (obj.Lang.ToLower() == "en")
			{
				source = LanguageHelper.Language.En;
				target = LanguageHelper.Language.Sv;
			}

			if (obj.Title != null)
			{
				// Translate Title
				var translator = new GoogleTranslator(source, target);
				var result = translator.TranslateText(obj.Title);
				obj.Title = translator.ParseContent(result);
			}

			if (obj.SubTitle != null)
			{
				// Translate Title
				var translator = new GoogleTranslator(source, target);
				var result = translator.TranslateText(obj.SubTitle);
				obj.SubTitle = translator.ParseContent(result);
			}

			if (obj.Content != null)
			{
				// Translate Title
				var translator = new GoogleTranslator(source, target);
				var result = translator.TranslateText(obj.Content);
				obj.Content = translator.ParseContent(result);
			}

			return obj;
		}

		public static string ObjectChangedScript()
		{
			var sb = new StringBuilder();

			sb.Append("var TitleChanged = false;\n");
			sb.Append("var SubTitleChanged = false;\n");
			sb.Append("var ContentChanged = false;\n");
			sb.Append("\n");
			sb.Append("$(document).ready(function () {\n");
			sb.Append("    $('#TitleSv').change(function () {\n");
			sb.Append("        TitleChanged = true;\n");
			sb.Append("    });\n");
			sb.Append("    $('#TitleEn').change(function () {\n");
			sb.Append("        TitleChanged = true;\n");
			sb.Append("    });\n");
			sb.Append("    $('#SubTitleSv').change(function () {\n");
			sb.Append("        SubTitleChanged = true;\n");
			sb.Append("    });\n");
			sb.Append("    $('#SubTitleEn').change(function () {\n");
			sb.Append("        SubTitleChanged = true;\n");
			sb.Append("    });\n");
			sb.Append("    $('#ContentSv').change(function () {\n");
			sb.Append("        ContentChanged = true;\n");
			sb.Append("    });\n");
			sb.Append("    $('#ContentEn').change(function () {\n");
			sb.Append("        ContentChanged = true;\n");
			sb.Append("    });\n");
			sb.Append("});\n");

			return sb.ToString();
		}

		public static string AjaxInitScript()
		{
			var sb = new StringBuilder();

			sb.Append("$(function () {\n");
			sb.Append("    $('#tabs').tabs();\n");
			// Div for AJAX
			sb.Append("    $('#ajax_success').hide(" + Constants.GuiAjaxSuccesAndErrorHideTime + ");\n");
			sb.Append("    $('#ajax_error').hide(" + Constants.GuiAjaxSuccesAndErrorHideTime + ");\n");
			sb.Append("    $('#ajax_preloader').hide(" + Constants.GuiAjaxSuccesAndErrorHideTime + ");\n");
			sb.Append("});\n");

			return sb.ToString();
		}

		public static string FunctionCallScript(string TranslateRoute)
		{
			var sb = new StringBuilder();

			sb.Append("function translateText(sourceLang) {\n");
			// Announcement is the object containing the data
			sb.Append("    var obj = null;\n");
			// Show loading animation
			sb.Append("    $('#ajax_preloader').show(" + Constants.GuiAjaxLoaderHideTime + ");\n");
			// Check target language
			sb.Append("    if (sourceLang == 'sv') {\n");
			sb.Append("        obj = getObj(sourceLang);\n");
			sb.Append("        if (obj == null) {\n");
			sb.Append("            alert('Could not get obj!');\n");
			sb.Append("            return;\n");
			sb.Append("        }\n");
			sb.Append("    }\n");
			sb.Append("    else if (sourceLang === 'en') {\n");
			sb.Append("        obj = getObj(sourceLang);\n");
			sb.Append("        if (obj == null) {\n");
			sb.Append("            alert('Could not get obj!');\n");
			sb.Append("            return;\n");
			sb.Append("        }\n");
			sb.Append("    }\n");
			// Objectify
			sb.Append("    var json = JSON.stringify(obj);\n");
			sb.Append("    $.ajax({\n");
			sb.Append("        url: '" + TranslateRoute + "',\n");
			sb.Append("        type: 'POST',\n");
			sb.Append("        dataType: 'json',\n");
			sb.Append("        data: json,\n");
			sb.Append("        contentType: 'application/json; charset=utf-8',\n");
			sb.Append("        success: function (data) {\n");
			sb.Append("            if (sourceLang == 'sv') {\n");
			// Got data, change selected language text to En
			sb.Append("                if (data.Title != null) {\n");
			sb.Append("                    $('#TitleEn').val(data.Title);\n");
			sb.Append("                }\n");
			sb.Append("                if (data.SubTitle != null) {\n");
			sb.Append("                    $('#SubTitleEn').val(data.SubTitle);\n");
			sb.Append("                }\n");
			sb.Append("                if (data.Content != null) {\n");
			sb.Append("                    $('#ContentEn').val(data.Content);\n");
			sb.Append("                }\n");
			sb.Append("            }\n");
			sb.Append("            else if (sourceLang == 'en') {\n");
			// Got data, change selected language text to Sv
			sb.Append("                if (data.Title != null) {\n");
			sb.Append("                    $('#TitleSv').val(data.Title);\n");
			sb.Append("                }\n");
			sb.Append("                if (data.SubTitle != null) {\n");
			sb.Append("                    $('#SubTitleSv').val(data.SubTitle);\n");
			sb.Append("                }\n");
			sb.Append("                if (data.Content != null) {\n");
			sb.Append("                    $('#ContentSv').val(data.Content);\n");
			sb.Append("                }\n");
			sb.Append("            }\n");
			// Hide loading animation and show success DIV
			sb.Append("            $('#ajax_preloader').hide(" + Constants.GuiAjaxLoaderHideTime + ");\n");
			sb.Append("            $('#ajax_success').show(" + Constants.GuiAjaxSuccesAndErrorEase + ").delay(" +
			          Constants.GuiAjaxSuccesAndErrorShowTime + ").hide(" + Constants.GuiAjaxSuccesAndErrorEase +
			          ");\n");
			sb.Append("        },\n");
			sb.Append("        error: function () {\n");
			// Hide loading animation and show error DIV
			sb.Append("            $('#ajax_preloader').hide(" + Constants.GuiAjaxLoaderHideTime + ");\n");
			sb.Append("            $('#ajax_error').show(" + Constants.GuiAjaxSuccesAndErrorEase + ").delay(" +
			          Constants.GuiAjaxSuccesAndErrorShowTime + ").hide(" + Constants.GuiAjaxSuccesAndErrorEase +
			          ");\n");
			sb.Append("        }\n");
			sb.Append("    });\n");
			sb.Append("}\n");

			return sb.ToString();
		}

		public static string GetTranslateObject()
		{
			var sb = new StringBuilder();

			sb.Append("function getObj(sourceLang) {\n");
			sb.Append("    var appendText = '';\n");
			sb.Append("    if (sourceLang == 'sv') {\n");
			sb.Append("        appendText = 'Sv';\n");
			sb.Append("    }\n");
			sb.Append("    else if (sourceLang == 'en') {\n");
			sb.Append("        appendText = 'En';\n");
			sb.Append("    }\n");
			sb.Append("    else {\n");
			sb.Append("        alert('Incorrect lang, sourceLang: ' + sourceLang);\n");
			sb.Append("        return { Lang: '', Title: '', SubTitle: '', Content: '' };\n");
			sb.Append("    }\n");
			sb.Append("    var lang = sourceLang;\n");
			sb.Append("    var title = '';\n");
			sb.Append("    var subTitle = '';\n");
			sb.Append("    var content = '';\n");
			// Title exists
			sb.Append("    var divId = '#Title' + appendText;\n");
			sb.Append("    if ($(divId).length) {\n");
			sb.Append("        if (TitleChanged) {\n");
			sb.Append("            title = $(divId).val();\n");
			sb.Append("        }\n");
			sb.Append("    }\n");
			// SubTitle exists
			sb.Append("    var divId = '#SubTitle' + appendText;\n");
			sb.Append("    if ($(divId).length) {\n");
			sb.Append("        if (SubTitleChanged) {\n");
			sb.Append("            subTitle = $(divId).val();\n");
			sb.Append("        }\n");
			sb.Append("    }\n");
			// Content exists
			sb.Append("    var divId = '#Content' + appendText;\n");
			sb.Append("    if ($(divId).length) {\n");
			sb.Append("        if (ContentChanged) {\n");
			sb.Append("            content = $(divId).val();\n");
			sb.Append("        }\n");
			sb.Append("    }\n");
			sb.Append("    return { Lang: lang, Title: title, SubTitle: subTitle, Content: content };\n");
			sb.Append("}\n");

			return sb.ToString();
		}
	}
}