using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Condominium2000.Helpers.Interface;
using Condominium2000.Helpers.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Membership
{
	public sealed class WebSecurity
	{
		public enum MembershipLoginStatus
		{
			Success,
			Failure
		}

		// TODO: Not in clear text
		private static readonly string DefaultPassword = "abc123!";

		public static HttpContextBase Context => new HttpContextWrapper(HttpContext.Current);

		public static HttpRequestBase Request => Context.Request;

		public static HttpResponseBase Response => Context.Response;

		public static IPrincipal User => Context.User;

		public static bool IsAuthenticated => User.Identity.IsAuthenticated;

		public static MembershipCreateStatus Register(string username, string password, string email, bool isApproved,
			string firstName, string lastName)
		{
			MembershipCreateStatus createStatus;
			System.Web.Security.Membership.CreateUser(username, password, email, null, null, isApproved, null, out createStatus);

			if (createStatus != MembershipCreateStatus.Success)
			{ return createStatus; }

			using (var context = new Condominium2000Context())
			{
				var user = context.Users.FirstOrDefault(usr => usr.Username == username);
				if (user != null)
				{
					user.FirstName = firstName;
					user.LastName = lastName;
				}
				context.SaveChanges();
			}

			return createStatus;
		}

		public static MembershipLoginStatus Login(string username, string password, bool rememberMe)
		{
			if (System.Web.Security.Membership.ValidateUser(username, password))
			{
				FormsAuthentication.SetAuthCookie(username, rememberMe);
				return MembershipLoginStatus.Success;
			}
			return MembershipLoginStatus.Failure;
		}

		public static void Logout()
		{
			FormsAuthentication.SignOut();
		}

		public static MembershipUser GetUser(string username)
		{
			return System.Web.Security.Membership.GetUser(username);
		}

		public static bool ChangeRole(string username)
		{
			using (var context = new Condominium2000Context())
			{
				var result = false;

				var user = context.Users.FirstOrDefault(c => c.Username == username);
				if (user != null)
				{
					if (Roles.IsUserInRole(user.Username, RolesHelper.Roles.Admin.ToString()))
					{
						result = true;
						Roles.RemoveUserFromRole(user.Username, RolesHelper.Roles.Admin.ToString());
						Roles.AddUserToRole(user.Username, RolesHelper.Roles.SuperAdmin.ToString());
						context.SaveChanges();
					}
					else if (Roles.IsUserInRole(user.Username, RolesHelper.Roles.SuperAdmin.ToString()))
					{
						var usersInRole = Roles.GetUsersInRole(RolesHelper.Roles.SuperAdmin.ToString());
						// There has to be at least 2 users that are SuperAdmin
						if (usersInRole.Length > 1)
						{
							result = true;
							Roles.RemoveUserFromRole(user.Username, RolesHelper.Roles.SuperAdmin.ToString());
							Roles.AddUserToRole(user.Username, RolesHelper.Roles.Admin.ToString());
							context.SaveChanges();
						}
					}
				}
				return result;
			}
		}

		public static bool ChangePassword(string oldPassword, string newPassword)
		{
			var currentUser = System.Web.Security.Membership.GetUser(User.Identity.Name);
			return currentUser != null && currentUser.ChangePassword(oldPassword, newPassword);
		}

		public static bool ResetPassword(string username)
		{
			using (var context = new Condominium2000Context())
			{
				var result = false;

				var user = context.Users.FirstOrDefault(c => c.Username == username);
				if (user != null)
				{
					result = true;
					user.Password = Crypto.HashPassword(DefaultPassword);
					context.SaveChanges();
				}

				return result;
			}
		}

		public static bool DeleteUser(string username)
		{
			return System.Web.Security.Membership.DeleteUser(username);
		}

		public static List<MembershipUser> FindUsersByEmail(string email, int pageIndex, int pageSize)
		{
			int totalRecords;
			return
				System.Web.Security.Membership.FindUsersByEmail(email, pageIndex, pageSize, out totalRecords)
					.Cast<MembershipUser>()
					.ToList();
		}

		public static List<MembershipUser> FindUsersByName(string username, int pageIndex, int pageSize)
		{
			int totalRecords;
			return
				System.Web.Security.Membership.FindUsersByName(username, pageIndex, pageSize, out totalRecords)
					.Cast<MembershipUser>()
					.ToList();
		}

		public static List<MembershipUser> GetAllUsers(int pageIndex, int pageSize)
		{
			int totalRecords;
			return
				System.Web.Security.Membership.GetAllUsers(pageIndex, pageSize, out totalRecords).Cast<MembershipUser>().ToList();
		}

		public static UserPrincipal CreatePrincipal(FormsIdentity id, string name)
		{
			using (var context = new Condominium2000Context())
			{
				UserPrincipal userPrincipal = null;

				var user = context.Users.FirstOrDefault(usr => usr.Username == name);
				if (user == null)
				{ return null; }

				var roles = Roles.GetRolesForUser(user.Username).ToList();
				if (roles.Count > 0)
				{
					userPrincipal = new UserPrincipal(
						new GenericIdentity(name)
						, roles
						, user.FirstName
						, user.LastName
						, user.Username);
				}
				return userPrincipal;
			}
		}
	}
}