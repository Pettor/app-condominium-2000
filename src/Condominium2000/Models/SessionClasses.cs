namespace Condominium2000.Models
{
	public class SessionObject
	{
		public enum Language
		{
			Swedish,
			English
		}

		public enum Menu
		{
			News,
			Society,
			Resident,
			Contact,
			Forum
		}

		public Language SessionLang;

		public Menu SessionMenu;

		public SessionObject()
		{
			SessionMenu = Menu.News;
			SessionLang = Language.Swedish;
		}
	}
}