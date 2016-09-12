using System;
using System.Linq;
using System.Web.Security;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Models.Providers
{
	public class CustomRoleProvider : RoleProvider
	{
		public override string ApplicationName
		{
			get { return GetType().Assembly.GetName().Name; }
			set { ApplicationName = GetType().Assembly.GetName().Name; }
		}

		public override bool RoleExists(string roleName)
		{
			if (string.IsNullOrEmpty(roleName))
			{
				return false;
			}
			using (var Context = new Condominium2000Context())
			{
				Role Role = null;
				Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
				if (Role != null)
				{
					return true;
				}
				return false;
			}
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			if (string.IsNullOrEmpty(username))
			{
				return false;
			}
			if (string.IsNullOrEmpty(roleName))
			{
				return false;
			}
			using (var Context = new Condominium2000Context())
			{
				User User = null;
				User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
				if (User == null)
				{
					return false;
				}
				var Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
				if (Role == null)
				{
					return false;
				}
				return User.Roles.Contains(Role);
			}
		}

		public override string[] GetAllRoles()
		{
			using (var Context = new Condominium2000Context())
			{
				return Context.Roles.Select(Rl => Rl.RoleName).ToArray();
			}
		}

		public override string[] GetUsersInRole(string roleName)
		{
			if (string.IsNullOrEmpty(roleName))
			{
				return null;
			}
			using (var Context = new Condominium2000Context())
			{
				Role Role = null;
				Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
				if (Role != null)
				{
					return Role.Users.Select(Usr => Usr.Username).ToArray();
				}
				return null;
			}
		}

		public override string[] GetRolesForUser(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				return null;
			}
			using (var Context = new Condominium2000Context())
			{
				User User = null;
				User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
				if (User != null)
				{
					return User.Roles.Select(Rl => Rl.RoleName).ToArray();
				}
				return null;
			}
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			if (string.IsNullOrEmpty(roleName))
			{
				return null;
			}

			if (string.IsNullOrEmpty(usernameToMatch))
			{
				return null;
			}

			using (var Context = new Condominium2000Context())
			{
				return (from Rl in Context.Roles
					from Usr in Rl.Users
					where Rl.RoleName == roleName && Usr.Username.Contains(usernameToMatch)
					select Usr.Username).ToArray();
			}
		}

		public override void CreateRole(string roleName)
		{
			if (!string.IsNullOrEmpty(roleName))
			{
				using (var Context = new Condominium2000Context())
				{
					Role Role = null;
					Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
					if (Role == null)
					{
						var NewRole = new Role
						{
							RoleId = Guid.NewGuid(),
							RoleName = roleName
						};
						Context.Roles.Add(NewRole);
						Context.SaveChanges();
					}
				}
			}
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			if (string.IsNullOrEmpty(roleName))
			{
				return false;
			}
			using (var Context = new Condominium2000Context())
			{
				Role Role = null;
				Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
				if (Role == null)
				{
					return false;
				}
				if (throwOnPopulatedRole)
				{
					if (Role.Users.Any())
					{
						return false;
					}
				}
				else
				{
					Role.Users.Clear();
				}
				Context.Roles.Remove(Role);
				Context.SaveChanges();
				return true;
			}
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			using (var Context = new Condominium2000Context())
			{
				var Users = Context.Users.Where(Usr => usernames.Contains(Usr.Username)).ToList();
				var Roles = Context.Roles.Where(Rl => roleNames.Contains(Rl.RoleName)).ToList();
				foreach (var user in Users)
				{
					foreach (var role in Roles)
					{
						if (!user.Roles.Contains(role))
						{
							user.Roles.Add(role);
						}
					}
				}
				Context.SaveChanges();
			}
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			using (var Context = new Condominium2000Context())
			{
				foreach (var username in usernames)
				{
					var us = username;
					var user = Context.Users.FirstOrDefault(U => U.Username == us);
					if (user != null)
					{
						foreach (var roleName in roleNames)
						{
							var rl = roleName;
							var role = user.Roles.FirstOrDefault(R => R.RoleName == rl);
							if (role != null)
							{
								user.Roles.Remove(role);
							}
						}
					}
				}
				Context.SaveChanges();
			}
		}
	}
}