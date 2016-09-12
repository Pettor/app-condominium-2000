using System.Web;
using Condominium2000.Helpers.Interface;

namespace Condominium2000.Helpers.Session
{
	public class SessionHelper
	{
		// Keys: Objects stores in session
		public enum Keys
		{
			Language,
			Menu
		}

		// Choices for each object stores in session
		public enum Menu
		{
			News,
			Society,
			Resident,
			Contact,
			Forum
		}

		#region Public Methods

		/// <summary>
		///     Initialize the session object and set default values for each object
		/// </summary>
		public static void InitializeSession()
		{
			// Check if session was already created
			if (DefaultSessionState.defaultSession == null)
			{
				DefaultSessionState.defaultSession =
					new DefaultSessionState(new HttpSessionStateWrapper(HttpContext.Current.Session));
			}

			// Default values
			SessionMenu = Menu.News;
		}

		#endregion

		#region Properies

		// Key variable
		public static Keys SessionKeys;

		// Menu property
		public static Menu SessionMenu
		{
			get { return (Menu)GetSessionValue(Keys.Menu); }
			set { SetSessionValue(Keys.Menu, value); }
		}

		#endregion

		#region Private Methods

		/// <summary>
		///     Get value from session using key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private static object GetSessionValue(Keys key)
		{
			if (DefaultSessionState.defaultSession == null)
			{
				InitializeSession();
			}

			return DefaultSessionState.defaultSession?.Get(KeyToString(key));
		}

		/// <summary>
		///     Set value in session using key
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		private static void SetSessionValue(Keys key, object value)
		{
			if (DefaultSessionState.defaultSession == null)
			{
				InitializeSession();
			}
			DefaultSessionState.defaultSession?.Store(KeyToString(key), value);
		}

		/// <summary>
		///     Translate key to string
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private static string KeyToString(Keys key)
		{
			return key.ToString();
		}

		#endregion
	}
}