using MvcGlobalisationSupport;

namespace Condominium2000.Helpers
{
	public class LanguageHelper
	{
		public enum Language
		{
			Sv,
			En
		}

		public static Language GetLanguage()
		{
			var result = Language.Sv;

			var language = CultureManager.GetCurrentCultureShortForm();

			if (language.Equals("en"))
			{
				result = Language.En;
			}

			return result;
		}

		public static void SetLanguage(Language lang)
		{
			CultureManager.SetCulture(lang.ToString());
		}
	}
}