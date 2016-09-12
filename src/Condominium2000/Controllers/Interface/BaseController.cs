using System.Web.Mvc;
using Condominium2000.Helpers.Interface;

namespace Condominium2000.Controllers.Interface
{
	public class BaseController : Controller
	{
		protected new virtual UserPrincipal User
		{
			get { return HttpContext.User as UserPrincipal; }
		}
	}
}