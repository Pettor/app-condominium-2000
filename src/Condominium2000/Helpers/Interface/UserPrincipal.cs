using System.Collections.Generic;
using System.Security.Principal;
using Condominium2000.Helpers.Models;

namespace Condominium2000.Helpers.Interface
{
	public class UserPrincipal : IPrincipal
	{
		private readonly List<string> _roles;

		/// <summary>
		///     Create the custom IPrincipal
		///     Used to get custom User information in views
		/// </summary>
		/// <param name="ident"></param>
		/// <param name="roles"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="userName"></param>
		public UserPrincipal(IIdentity ident, List<string> roles, string firstName, string lastName, string userName)
		{
			Identity = ident;
			_roles = roles;
			FirstName = firstName;
			LastName = lastName;
			UserName = userName;
		}

		// Custom properties
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserName { get; set; }

		// Required properties of IPrincipal
		public IIdentity Identity { get; set; }

		/// <summary>
		///     Check if user is in specified role
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool IsInRole(string role)
		{
			return _roles.Contains(role);
		}

		public bool IsSuperAdmin()
		{
			return _roles.Contains(RolesHelper.Roles.SuperAdmin.ToString());
		}

		public bool IsAdmin()
		{
			return _roles.Contains(RolesHelper.Roles.Admin.ToString()) || _roles.Contains(RolesHelper.Roles.SuperAdmin.ToString());
		}
	}
}