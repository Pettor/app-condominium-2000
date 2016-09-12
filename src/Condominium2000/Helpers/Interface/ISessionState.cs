using System.Web;
using Condominium2000.Helpers.Session;

namespace Condominium2000.Helpers.Interface
{
	/// <summary>
	///     Interface for Session State
	/// </summary>
	public interface ISessionState
	{
		void Clear();
		void Delete(string key);
		object Get(string key);
		void Store(string key, object value);
	}

	/// <summary>
	///     Implement ISession interface
	/// </summary>
	public class DefaultSessionState : ISessionState
	{
		public static DefaultSessionState defaultSession;
		private readonly HttpSessionStateBase session;

		/// <summary>
		///     Get the default session state from HTTPSessionStateBase
		/// </summary>
		/// <param name="session"></param>
		public DefaultSessionState(HttpSessionStateBase session)
		{
			this.session = session;
		}

		/// <summary>
		///     Clear session
		/// </summary>
		public void Clear()
		{
			session.RemoveAll();
		}

		/// <summary>
		///     Delete key from session
		/// </summary>
		/// <param name="key"></param>
		public void Delete(string key)
		{
			session.Remove(key);
		}

		/// <summary>
		///     Get key from session
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object Get(string key)
		{
			var obj = session[key];

			if (obj == null)
			{
				SessionHelper.InitializeSession();
			}

			return obj;
		}

		/// <summary>
		///     Store value in session with key
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Store(string key, object value)
		{
			session[key] = value;
		}
	}
}