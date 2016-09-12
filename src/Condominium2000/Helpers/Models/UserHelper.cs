using System.Linq;
using System.Web.Security;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Models
{
	public class UserHelper
	{
		public static User GetUser(ref Condominium2000Context context, string Username)
		{
			User user = null;
			user = context.Users.FirstOrDefault(Usr => Usr.Username == Username);
			return user;
		}

		public static MembershipCreateStatus ValidateChangedUser(EditUserModel model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = MembershipCreateStatus.Success;

				var user = Context.Users.FirstOrDefault(Usr => Usr.Username == model.UserName);
				// Must not exist multiple users with same name
				if ((user != null) && (model.UserId != user.UserId))
				{
					result = MembershipCreateStatus.DuplicateUserName;
				}

				return result;
			}
		}
	}
}