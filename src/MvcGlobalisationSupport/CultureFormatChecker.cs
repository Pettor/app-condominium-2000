using System.Text.RegularExpressions;

namespace MvcGlobalisationSupport
{
    public static class CultureFormatChecker
    {
        //This matches the format xx or xx-xx 
        // where x is any alpha character, case insensitive
        //The router will determine if it is a supported language
        static readonly Regex CultureRegexPattern = new Regex(@"^([a-zA-Z]{2})(-[a-zA-Z]{2})?$", RegexOptions.IgnoreCase & RegexOptions.Compiled);

        public static bool FormattedAsCulture(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;
            return CultureRegexPattern.IsMatch(code);

        }
    }
}
