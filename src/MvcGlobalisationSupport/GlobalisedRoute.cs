﻿using System.Web.Routing;

namespace MvcGlobalisationSupport
{
	public class GlobalisedRoute : Route
	{

		public const string CultureKey = "culture";

		private static string CreateCultureRoute(string unGlobalisedUrl)
		{
			return "{" + CultureKey + $"}}/{unGlobalisedUrl}";
		}

		/// <summary>
		///    Initializes a new instance of the System.Web.Routing.Route class, by using
		///    the specified URL pattern, default parameter values, and handler class.
		/// </summary>
		/// <param name="unGlobalisedUrl">The URL pattern for the route, without the culture</param>
		/// <param name="defaults"The values to use for any parameters that are missing in the URL.></param>
		public GlobalisedRoute(string unGlobalisedUrl, RouteValueDictionary defaults) :
			base(CreateCultureRoute(unGlobalisedUrl),
					defaults,
					new RouteValueDictionary(new { culture = new CultureRouteConstraint() }),
					new GlobalisationRouteHandler())
		{
		}
	}
}
