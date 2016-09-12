using System.Web.Mvc;

namespace Condominium2000.Helpers.Interface
{
	public abstract class BaseViewPage : WebViewPage
	{
		public new virtual UserPrincipal User
		{
			get { return base.User as UserPrincipal; }
		}
	}

	public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
	{
		public new virtual UserPrincipal User
		{
			get { return base.User as UserPrincipal; }
		}
	}
}