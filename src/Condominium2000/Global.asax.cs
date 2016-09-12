using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Condominium2000.Helpers.Interface;
using Condominium2000.Helpers.Membership;
using Condominium2000.Helpers.Session;
using MvcGlobalisationSupport;

namespace Condominium2000
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			const string defautlRouteUrl = "{controller}/{action}/{id}";
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			var defaultRouteValueDictionary = new RouteValueDictionary(
				new { controller = "News", action = "Index", id = UrlParameter.Optional });

			var defaultRoute = new Route(defautlRouteUrl
				, defaultRouteValueDictionary
				, new MvcRouteHandler());

			routes.Add("DefaultGlobalised"
				, new GlobalisedRoute(defaultRoute.Url, defaultRoute.Defaults));

			routes.Add("Default"
				, new Route(defautlRouteUrl
					, defaultRouteValueDictionary, new MvcRouteHandler()));
		}

		protected void Session_Start(object sender, EventArgs e)
		{
			// Create default session object (static) and set default values
			SessionHelper.InitializeSession();
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			// For AJAX Json request
			ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
		}

		protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
		{
			if (HttpContext.Current.User != null)
			{
				if (HttpContext.Current.User.Identity.IsAuthenticated)
				{
					if (HttpContext.Current.User.Identity is FormsIdentity)
					{
						// Get Forms Identity From Current User
						var id = (FormsIdentity)HttpContext.Current.User.Identity;

						// Create a custom Principal Instance and assign to Current User (with caching)
						var principal = (UserPrincipal)HttpContext.Current.Cache.Get(id.Name);
						if (principal == null)
						{
							// Create and populate your Principal object with the needed data and Roles.
							principal = WebSecurity.CreatePrincipal(id, id.Name);
							if (principal != null)
							{
								HttpContext.Current.Cache.Add(
									id.Name,
									principal,
									null,
									Cache.NoAbsoluteExpiration,
									new TimeSpan(0, 30, 0),
									CacheItemPriority.Default,
									null);
							}
						}

						HttpContext.Current.User = principal;
					}
				}
			}
		}
	}
}