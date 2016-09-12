using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace MvcGlobalisationSupport
{
	public static class CultureManager
	{
		private const string SwedishCultureName = "sv";
		private const string EnglishCultureName = "en";

		private static CultureInfo DefaultCulture => SupportedCultures[SwedishCultureName];

		private static Dictionary<string, CultureInfo> SupportedCultures { get; set; }


		private static void AddSupportedCulture(string name)
		{
			SupportedCultures.Add(name, CultureInfo.CreateSpecificCulture(name));
		}

		private static void InitializeSupportedCultures()
		{
			SupportedCultures = new Dictionary<string, CultureInfo>();
			AddSupportedCulture(SwedishCultureName);
			AddSupportedCulture(EnglishCultureName);
		}

		private static string ConvertToShortForm(string code)
		{
			return code.Substring(0, 2);
		}

		private static bool CultureIsSupported(string code)
		{
			if (string.IsNullOrWhiteSpace(code))
				return false;
			code = code.ToLowerInvariant();
			if (code.Length == 2)
				return SupportedCultures.ContainsKey(code);
			return CultureFormatChecker.FormattedAsCulture(code) && SupportedCultures.ContainsKey(ConvertToShortForm(code));
		}

		private static CultureInfo GetCulture(string code)
		{
			if (!CultureIsSupported(code))
				return DefaultCulture;
			string shortForm = ConvertToShortForm(code).ToLowerInvariant();
			return SupportedCultures[shortForm];
		}

		public static string GetCurrentCultureShortForm()
		{
			return ConvertToShortForm(Thread.CurrentThread.CurrentCulture.ToString()).ToLowerInvariant();
		}

		public static void SetCulture(string code)
		{
			var cultureInfo = GetCulture(code);
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			Thread.CurrentThread.CurrentCulture = cultureInfo;
		}

		static CultureManager()
		{
			InitializeSupportedCultures();
		}
	}
}
