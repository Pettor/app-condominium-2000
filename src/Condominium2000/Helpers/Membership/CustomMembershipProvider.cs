using System;
using System.Linq;
using System.Web.Security;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Membership
{
	public class CustomMembershipProvider : MembershipProvider
	{
		#region Properties

		public override string ApplicationName
		{
			get { return GetType().Assembly.GetName().Name; }
			set
			{
				if (value == null)
				{ throw new ArgumentNullException(nameof(value)); }
				ApplicationName = GetType().Assembly.GetName().Name;
			}
		}

		public override int MaxInvalidPasswordAttempts => 5;

		public override int MinRequiredNonAlphanumericCharacters => 0;

		public override int MinRequiredPasswordLength => 6;

		public override int PasswordAttemptWindow => 0;

		public override MembershipPasswordFormat PasswordFormat => MembershipPasswordFormat.Hashed;

		public override string PasswordStrengthRegularExpression => string.Empty;

		public override bool RequiresUniqueEmail => true;

		#endregion

		#region Functions

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
			string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			if (string.IsNullOrEmpty(username))
			{
				status = MembershipCreateStatus.InvalidUserName;
				return null;
			}
			if (string.IsNullOrEmpty(password))
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			if (string.IsNullOrEmpty(email))
			{
				status = MembershipCreateStatus.InvalidEmail;
				return null;
			}

			var hashedPassword = Crypto.HashPassword(password);
			if (hashedPassword.Length > 128)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}

			using (var context = new Condominium2000Context())
			{
				if (context.Users.Any(usr => usr.Username == username))
				{
					status = MembershipCreateStatus.DuplicateUserName;
					return null;
				}

				if (context.Users.Any(usr => usr.Email == email))
				{
					status = MembershipCreateStatus.DuplicateEmail;
					return null;
				}

				var newUser = new User
				{
					UserId = Guid.NewGuid(),
					Username = username,
					Password = hashedPassword,
					IsApproved = isApproved,
					Email = email,
					CreateDate = DateTime.UtcNow,
					LastPasswordChangedDate = DateTime.UtcNow,
					PasswordFailuresSinceLastSuccess = 0,
					LastLoginDate = DateTime.UtcNow,
					LastActivityDate = DateTime.UtcNow,
					LastLockoutDate = DateTime.UtcNow,
					IsLockedOut = false,
					LastPasswordFailureDate = DateTime.UtcNow
				};

				context.Users.Add(newUser);
				context.SaveChanges();
				status = MembershipCreateStatus.Success;

				if (newUser.LastActivityDate != null && newUser.CreateDate != null && newUser.LastPasswordChangedDate != null &&
					newUser.LastLoginDate != null && newUser.LastLockoutDate != null)
				{
					return new MembershipUser(System.Web.Security.Membership.Provider.Name, newUser.Username, newUser.UserId,
						newUser.Email, null, null, newUser.IsApproved, newUser.IsLockedOut, newUser.CreateDate.Value,
						newUser.LastLoginDate.Value, newUser.LastActivityDate.Value, newUser.LastPasswordChangedDate.Value,
						newUser.LastLockoutDate.Value);
				}
			}

			return null;
		}

		public override bool ValidateUser(string username, string password)
		{
			if (string.IsNullOrEmpty(username))
			{
				return false;
			}
			if (string.IsNullOrEmpty(password))
			{
				return false;
			}
			using (var context = new Condominium2000Context())
			{
				var user = context.Users.FirstOrDefault(usr => usr.Username == username);
				if (user == null)
				{
					return false;
				}
				if (!user.IsApproved)
				{
					return false;
				}
				if (user.IsLockedOut)
				{
					return false;
				}
				var hashedPassword = user.Password;
				var verificationSucceeded = hashedPassword != null && Crypto.VerifyHashedPassword(hashedPassword, password);
				if (verificationSucceeded)
				{
					user.PasswordFailuresSinceLastSuccess = 0;
					user.LastLoginDate = DateTime.UtcNow;
					user.LastActivityDate = DateTime.UtcNow;
				}
				else
				{
					var failures = user.PasswordFailuresSinceLastSuccess;
					if (failures < MaxInvalidPasswordAttempts)
					{
						user.PasswordFailuresSinceLastSuccess += 1;
						user.LastPasswordFailureDate = DateTime.UtcNow;
					}
					else if (failures >= MaxInvalidPasswordAttempts)
					{
						user.LastPasswordFailureDate = DateTime.UtcNow;
						user.LastLockoutDate = DateTime.UtcNow;
						user.IsLockedOut = true;
					}
				}
				context.SaveChanges();
				if (verificationSucceeded)
				{
					return true;
				}
				return false;
			}
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			if (string.IsNullOrEmpty(username))
			{
				return null;
			}
			using (var context = new Condominium2000Context())
			{
				var user = context.Users.FirstOrDefault(usr => usr.Username == username);
				if (user == null)
				{ return null; }

				if (userIsOnline)
				{
					user.LastActivityDate = DateTime.UtcNow;
					context.SaveChanges();
				}

				if (user.LastActivityDate != null && user.CreateDate != null && user.LastPasswordChangedDate != null &&
					user.LastLoginDate != null && user.LastLockoutDate != null)
				{
					return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId, user.Email,
						null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value,
						user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value);
				}
				return null;
			}
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			if (providerUserKey is Guid)
			{
			}
			else
			{
				return null;
			}

			using (var context = new Condominium2000Context())
			{
				var user = context.Users.Find(providerUserKey);
				if (user == null)
				{ return null; }
				if (!userIsOnline)
				{
					if (user.LastActivityDate != null && user.CreateDate != null && user.LastPasswordChangedDate != null &&
						user.LastLoginDate != null && user.LastLockoutDate != null)
					{
						return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId, user.Email,
							null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value,
							user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value);
					}
				}
				user.LastActivityDate = DateTime.UtcNow;
				context.SaveChanges();

				if (user.LastActivityDate != null && user.CreateDate != null && user.LastPasswordChangedDate != null &&
					user.LastLoginDate != null && user.LastLockoutDate != null)
				{
					return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId, user.Email,
						null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value,
						user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value);
				}

				return null;
			}
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			if (string.IsNullOrEmpty(username))
			{
				return false;
			}
			if (string.IsNullOrEmpty(oldPassword))
			{
				return false;
			}
			if (string.IsNullOrEmpty(newPassword))
			{
				return false;
			}
			using (var context = new Condominium2000Context())
			{
				var user = context.Users.FirstOrDefault(usr => usr.Username == username);
				if (user == null)
				{
					return false;
				}
				var hashedPassword = user.Password;
				var verificationSucceeded = hashedPassword != null && Crypto.VerifyHashedPassword(hashedPassword, oldPassword);
				if (verificationSucceeded)
				{
					user.PasswordFailuresSinceLastSuccess = 0;
				}
				else
				{
					var failures = user.PasswordFailuresSinceLastSuccess;
					if (failures < MaxInvalidPasswordAttempts)
					{
						user.PasswordFailuresSinceLastSuccess += 1;
						user.LastPasswordFailureDate = DateTime.UtcNow;
					}
					else if (failures >= MaxInvalidPasswordAttempts)
					{
						user.LastPasswordFailureDate = DateTime.UtcNow;
						user.LastLockoutDate = DateTime.UtcNow;
						user.IsLockedOut = true;
					}
					context.SaveChanges();
					return false;
				}
				var newHashedPassword = Crypto.HashPassword(newPassword);
				if (newHashedPassword.Length > 128)
				{
					return false;
				}
				user.Password = newHashedPassword;
				user.LastPasswordChangedDate = DateTime.UtcNow;
				context.SaveChanges();
				return true;
			}
		}

		public override bool UnlockUser(string userName)
		{
			using (var context = new Condominium2000Context())
			{
				var user = context.Users.FirstOrDefault(usr => usr.Username == userName);
				if (user == null)
				{ return false; }

				user.IsLockedOut = false;
				user.PasswordFailuresSinceLastSuccess = 0;
				context.SaveChanges();
				return true;
			}
		}

		public override int GetNumberOfUsersOnline()
		{
			var dateActive =
				DateTime.UtcNow.Subtract(
					TimeSpan.FromMinutes(Convert.ToDouble(System.Web.Security.Membership.UserIsOnlineTimeWindow)));
			using (var context = new Condominium2000Context())
			{
				return context.Users.Count(usr => usr.LastActivityDate > dateActive);
			}
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			if (string.IsNullOrEmpty(username))
			{
				return false;
			}
			using (var context = new Condominium2000Context())
			{
				var user = context.Users.FirstOrDefault(usr => usr.Username == username);
				if (user == null)
				{ return false; }

				foreach (var news in user.WrittenNews)
				{
					news.WrittenBy = null;
				}

				context.Users.Remove(user);
				context.SaveChanges();
				return true;
			}
		}

		public override string GetUserNameByEmail(string email)
		{
			using (var context = new Condominium2000Context())
			{
				var user = context.Users.FirstOrDefault(usr => usr.Email == email);
				return user != null ? user.Username : string.Empty;
			}
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
			out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();
			using (var context = new Condominium2000Context())
			{
				totalRecords = context.Users.Count(usr => usr.Email == emailToMatch);
				var users =
					context.Users.Where(usr => usr.Email == emailToMatch)
						.OrderBy(usrn => usrn.Username)
						.Skip(pageIndex * pageSize)
						.Take(pageSize);
				foreach (var user in users)
				{

					if (user.LastActivityDate != null && user.CreateDate != null && user.LastPasswordChangedDate != null &&
						user.LastLoginDate != null && user.LastLockoutDate != null)
					{
						membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId,
							user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value,
							user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
					}
				}
			}
			return membershipUsers;
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
			out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();
			using (var context = new Condominium2000Context())
			{
				totalRecords = context.Users.Count(usr => usr.Username == usernameToMatch);
				var users =
					context.Users.Where(usr => usr.Username == usernameToMatch)
						.OrderBy(usrn => usrn.Username)
						.Skip(pageIndex * pageSize)
						.Take(pageSize);
				foreach (var user in users)
				{
					if (user.LastActivityDate != null && user.CreateDate != null && user.LastPasswordChangedDate != null &&
						user.LastLoginDate != null && user.LastLockoutDate != null)
					{
						membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId,
							user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value,
							user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
					}
				}
			}
			return membershipUsers;
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();
			using (var context = new Condominium2000Context())
			{
				totalRecords = context.Users.Count();
				var users = context.Users.OrderBy(usrn => usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
				foreach (var user in users)
				{
					if (user.LastActivityDate != null && user.CreateDate != null && user.LastPasswordChangedDate != null &&
						user.LastLoginDate != null && user.LastLockoutDate != null)
					{
						membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.UserId,
							user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate.Value, user.LastLoginDate.Value,
							user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
					}
				}
			}
			return membershipUsers;
		}

		#endregion

		#region Not Supported

		//CodeFirstMembershipProvider does not support password retrieval scenarios.
		public override bool EnablePasswordRetrieval => false;

		public override string GetPassword(string username, string answer)
		{
			throw new NotSupportedException("Consider using methods from WebSecurity module.");
		}

		//CodeFirstMembershipProvider does not support password reset scenarios.
		public override bool EnablePasswordReset => false;

		public override string ResetPassword(string username, string answer)
		{
			throw new NotSupportedException("Consider using methods from WebSecurity module.");
		}

		//CodeFirstMembershipProvider does not support question and answer scenarios.
		public override bool RequiresQuestionAndAnswer => false;

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
			string newPasswordAnswer)
		{
			throw new NotSupportedException("Consider using methods from WebSecurity module.");
		}

		//CodeFirstMembershipProvider does not support UpdateUser because this method is useless.
		public override void UpdateUser(MembershipUser user)
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}