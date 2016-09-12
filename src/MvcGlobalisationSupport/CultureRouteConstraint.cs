using System.Web;
using System.Web.Routing;


namespace MvcGlobalisationSupport
{
	public class CultureRouteConstraint : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
			if (!values.ContainsKey(parameterName))
				return false;
			var potentialCultureName = (string)values[parameterName];
			return CultureFormatChecker.FormattedAsCulture(potentialCultureName);
		}
	}
}
