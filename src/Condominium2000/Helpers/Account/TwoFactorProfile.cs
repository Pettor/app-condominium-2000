using System;
using System.Web.Profile;

namespace Condominium2000.Helpers.Account
{
	public class TwoFactorProfile : ProfileBase
	{
		public static TwoFactorProfile CurrentUser
		{
			get
			{
				var membershipUser = System.Web.Security.Membership.GetUser();
				return membershipUser != null ? GetByUserName(membershipUser.UserName) : null;
			}
		}

		public DateTime? LastLoginAttemptUtc
		{
			get
			{
				try
				{
					return (DateTime?) base["LastLoginAttemptUtc"];
				}
				catch
				{
					return null;
				}
			}
			set
			{
				base["LastLoginAttemptUtc"] = value;
				Save();
			}
		}

		public string TwoFactorSecret
		{
			get { return (string) base["TwoFactorSecret"]; }
			set
			{
				base["TwoFactorSecret"] = value;
				Save();
			}
		}

		public static TwoFactorProfile GetByUserName(string username)
		{
			return (TwoFactorProfile) Create(username);
		}
	}
}