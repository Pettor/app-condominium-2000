using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Condominium2000.Controllers.Interface;
using Condominium2000.Helpers;
using Condominium2000.Helpers.Account;
using Condominium2000.Helpers.Membership;
using Condominium2000.Helpers.Models;
using Condominium2000.Models;
using Condominium2000.Models.Context;
using Condominium2000.Models.ViewModels;
using Condominium2000.Resources.Account;
using Microsoft.Web.Helpers;
using TwoFactor;
using EntityState = System.Data.Entity.EntityState;
using ErrorCodes = Condominium2000.Resources.ErrorCodes;

namespace Condominium2000.Controllers
{
	public class AccountController : BaseController
	{
		private readonly Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /Account/EditUser/5

		[Authorize]
		public ActionResult EditUser(string username)
		{
			// Must be SuperAdmin to gain access to this functionality
			if (!User.IsSuperAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			// Fetch user
			var dbUser = _db.Users.FirstOrDefault(c => c.Username == username);

			/**
			 * Fill Model
			 */
			var user = new EditUserModel();

			if (dbUser == null)
			{
				return RedirectToAction("Index", "News");
			}

			user.UserName = username;
			user.UserId = dbUser.UserId;
			user.Email = dbUser.Email;
			user.IsApproved = dbUser.IsApproved;
			user.FirstName = dbUser.FirstName;
			user.LastName = dbUser.LastName;

			return View(user);
		}

		//
		// POST: /Account/EditUser/5

		[HttpPost]
		[Authorize]
		public ActionResult EditUser(EditUserModel user)
		{
			// Must be SuperAdmin to gain access to this functionality
			if (!User.IsSuperAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				var dbUser = _db.Users.FirstOrDefault(c => c.UserId == user.UserId);
				if (dbUser != null)
				{
					// Validate the user
					var status = UserHelper.ValidateChangedUser(user);

					if (status == MembershipCreateStatus.Success)
					{
						if (TryUpdateModel(dbUser))
						{
							_db.Entry(dbUser).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditUserSuccess"] = Resources.Account.EditUser.SUCCESS_USER_SAVED;
						}
						else
						{
							ModelState.AddModelError("", Resources.Account.EditUser.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.Account.EditUser.ERROR_INTERNAL_NOT_FIND_USER);
				}
			}
			return View(user);
		}

		//
		// GET: /Account/DeleteUser/5

		[Authorize]
		public ActionResult DeleteUser(string username)
		{
			// Must be SuperAdmin to gain access to this functionality
			if (!User.IsSuperAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			// Fetch user
			var dbUser = _db.Users.FirstOrDefault(c => c.Username == username);

			if (dbUser == null)
			{
				return RedirectToAction("Index", "News");
			}

			return View(dbUser);
		}

		//
		// POST: /Account/DeleteUser/5

		[Authorize]
		[HttpPost]
		public ActionResult DeleteUser(User user)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var deleteUser = false;
			if (Roles.IsUserInRole(user.Username, RolesHelper.Roles.SuperAdmin.ToString()))
			{
				var usersInRole = Roles.GetUsersInRole(RolesHelper.Roles.SuperAdmin.ToString());
				// There has to be at least 2 users that are SuperAdmin

				if (usersInRole.Length > 1)
				{
					deleteUser = true;
				}
				else
				{
					ModelState.AddModelError("", Resources.Account.DeleteUser.ERROR_INTERNAL_LAST_SUPERADMIN);
				}
			}
			else
			{
				deleteUser = true;
			}

			if (deleteUser)
			{
				if (WebSecurity.DeleteUser(user.Username))
				{
					TempData["DeleteUserSuccess"] = Resources.Account.DeleteUser.SUCCESS_USER_DELETED;

					return RedirectToAction("AdminUsers", "Account");
				}
				ModelState.AddModelError("", Resources.Account.DeleteUser.ERROR_INTERNAL_NOT_DELETE_MODEL);
			}


			var dbUser = _db.Users.FirstOrDefault(u => u.Username == user.Username);

			return View(dbUser);
		}

		//
		// GET: /Account/AdminUsers

		[Authorize]
		public ActionResult AdminUsers()
		{
			/**
			 * Fill Model
			 */
			var viewModel = new AccountViewModel { Users = _db.Users.ToList() };

			// Get Users and Roles from Database
			return View(viewModel);
		}

		//
		// GET: /Account/LogOn

		public ActionResult LogOn()
		{
			return View();
		}

		//
		// POST: /Account/LogOn

		//private void DoLogOn(LogOnModel model, string returnUrl)
		//{
		//    try
		//    {
		//        if (ModelState.IsValid)
		//        {
		//            if (Membership.ValidateUser(model.UserName, model.Password))
		//            {
		//                var profile = TwoFactorProfile.GetByUserName(model.UserName);

		//                if (profile != null && !string.IsNullOrEmpty(profile.TwoFactorSecret))
		//                {
		//                    // Prevent the user from attempting to brute force the two factor secret.
		//                    // Without this, an attacker, if they know your password already, could try to brute
		//                    // force the two factor code. They only need to try 1,000,000 distinct codes in 3 minutes.
		//                    // This throttles them down to a managable level.
		//                    if (profile.LastLoginAttemptUtc.HasValue && profile.LastLoginAttemptUtc > DateTime.UtcNow - TimeSpan.FromSeconds(1))
		//                    {
		//                        System.Threading.Thread.Sleep(5000);
		//                    }

		//                    profile.LastLoginAttemptUtc = DateTime.UtcNow;

		//                    if (TimeBasedOneTimePassword.IsValid(profile.TwoFactorSecret, model.TwoFactorCode))
		//                    {
		//                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
		//                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
		//                        {
		//                            AsyncManager.Parameters["returnUrl"] = returnUrl;
		//                        }
		//                        else
		//                        {
		//                            AsyncManager.Parameters["action"] = "Index";
		//                            AsyncManager.Parameters["controller"] = "News";
		//                        }
		//                    }
		//                    else
		//                    {
		//                        ModelState.AddModelError("", "The two factor code is incorrect.");
		//                    }
		//                }
		//                else
		//                {
		//                    ModelState.AddModelError("", "The two factor code is incorrect.");
		//                }
		//            }
		//            else
		//            {
		//                ModelState.AddModelError("", "The user name or password provided is incorrect.");
		//            }
		//        }

		//        AsyncManager.Parameters["model"] = model;
		//    }
		//    finally
		//    {
		//        AsyncManager.OutstandingOperations.Decrement();
		//    }
		//}

		//
		// POST: /Account/LogOn

		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (Membership.ValidateUser(model.UserName, model.Password))
				{
					FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
						&& !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
					{
						return Redirect(returnUrl);
					}
					return RedirectToAction("Index", "News");
				}
				ModelState.AddModelError("", Resources.Account.LogOn.ERROR_INTERNAL_USER_PASS);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//[HttpPost]
		//public void LogOnAsync(LogOnModel model, string returnUrl)
		//{
		//    AsyncManager.OutstandingOperations.Increment();
		//    AsyncManager.Parameters["task"] = Task.Factory.StartNew(() => { DoLogOn(model, returnUrl); });
		//}

		//public ActionResult LogOnCompleted(Task task, string returnUrl, string action, string controller, LogOnModel model)
		//{
		//    try
		//    {
		//        task.Wait();
		//    }
		//    catch (AggregateException ex)
		//    {
		//        Exception baseException = ex.GetBaseException();

		//        if (baseException is OneTimePasswordException)
		//        {
		//            model = new LogOnModel();
		//            ModelState.AddModelError("", @Condominium2000.Resources.Account.TwoFactor.ERROR_ALREADY_USED);
		//        }
		//        else
		//        {
		//            throw;
		//        }
		//    }

		//    if (returnUrl != null)
		//    {
		//        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
		//        return Redirect(returnUrl);
		//    }
		//    else if (action != null && controller != null)
		//    {
		//        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
		//        return RedirectToAction(action, controller);
		//    }
		//    else
		//    {
		//        return View(model);
		//    }
		//}

		//
		// GET: /Account/LogOff

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("Index", "News");
		}

		//
		// GET: /Account/Register

		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register

		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Check Recaptcha
				if (ReCaptcha.Validate(Constants.RecaptchaPrivateKey))
				{
					// Attempt to register the user
					var createStatus = WebSecurity.Register(model.UserName, model.Password, model.Email, true, model.FirstName,
						model.LastName);

					if (createStatus == MembershipCreateStatus.Success)
					{
						// Add user to Admin role
						Roles.AddUserToRole(model.UserName, RolesHelper.Roles.Admin.ToString());

						// Don't let the user log in, it's not approved yet
						FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
						return RedirectToAction("Index", "News");
					}
					ModelState.AddModelError("", ErrorCodeToString(createStatus));
				}
				else
				{
					ModelState.AddModelError("", ErrorCodes.ERROR_RECAPTCHA_INVALID);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[Authorize]
		public ActionResult ShowTwoFactorSecret()
		{
			var secret = TwoFactorProfile.CurrentUser.TwoFactorSecret;

			if (string.IsNullOrEmpty(secret))
			{
				var buffer = new byte[9];

				using (var rng = RandomNumberGenerator.Create())
				{
					rng.GetBytes(buffer);
				}

				// Generates a 10 character string of A-Z, a-z, 0-9
				// Don't need to worry about any = padding from the
				// Base64 encoding, since our input buffer is divisible by 3
				TwoFactorProfile.CurrentUser.TwoFactorSecret =
					Convert.ToBase64String(buffer).Substring(0, 10).Replace('/', '0').Replace('+', '1');

				secret = TwoFactorProfile.CurrentUser.TwoFactorSecret;
			}

			var enc = new Base32Encoder().Encode(Encoding.ASCII.GetBytes(secret));

			return View(new TwoFactorSecret { EncodedSecret = enc });
		}

		//
		// GET: /Account/ChangeRole

		[Authorize]
		public ActionResult ChangeRole(string username)
		{
			// Must be SuperAdmin to gain access to this functionality
			if (!User.IsSuperAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (WebSecurity.ChangeRole(username))
			{
				TempData["ResetSuccess"] = Resources.Account.ChangeRole.SUCCESS_CHANGE_ROLE;
			}
			else
			{
				TempData["ResetError"] = Resources.Account.ChangeRole.ERROR_CHANGE_ROLE;
			}

			var dbUser = _db.Users.FirstOrDefault(c => c.Username == username);

			return RedirectToAction("EditUser", new { username = dbUser?.Username });
		}

		//
		// GET: /Account/ResetPassword

		[Authorize]
		public ActionResult ResetPassword(string username)
		{
			// Must be SuperAdmin to gain access to this functionality
			if (!User.IsSuperAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (WebSecurity.ResetPassword(username))
			{
				TempData["ResetSuccess"] = Resources.Account.ResetPassword.SUCCESS_PASSWORD_RESET;
			}
			else
			{
				TempData["ResetError"] = Resources.Account.ResetPassword.ERROR_RESET_PASSWORD;
			}

			var dbUser = _db.Users.FirstOrDefault(c => c.Username == username);

			return RedirectToAction("EditUser", new { username = dbUser?.Username });
		}

		//
		// GET: /Account/ChangePassword

		[Authorize]
		public ActionResult ChangePassword()
		{
			return View();
		}

		//
		// POST: /Account/ChangePassword

		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{
				// ChangePassword will throw an exception rather
				// than return false in certain failure scenarios.
				bool changePasswordSucceeded = false;
				try
				{
					var currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
					if (currentUser != null)
					{ changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword); }
				}
				catch (Exception)
				{
					changePasswordSucceeded = false;
				}

				if (changePasswordSucceeded)
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				ModelState.AddModelError("", @"The current password is incorrect or the new password is invalid.");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/ChangePasswordSuccess

		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}

		#region Status Codes

		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return UserErrorCodes.ERROR_DUPLICATE_USER_NAME;

				case MembershipCreateStatus.DuplicateEmail:
					return UserErrorCodes.ERROR_DUPLICATE_EMAIL;

				case MembershipCreateStatus.InvalidPassword:
					return UserErrorCodes.ERROR_INVALID_PASSWORD;

				case MembershipCreateStatus.InvalidEmail:
					return UserErrorCodes.ERROR_DUPLICATE_EMAIL;

				case MembershipCreateStatus.InvalidAnswer:
					return UserErrorCodes.ERROR_INVALID_ANSWER;

				case MembershipCreateStatus.InvalidQuestion:
					return UserErrorCodes.ERROR_INVALID_QUESTION;

				case MembershipCreateStatus.InvalidUserName:
					return UserErrorCodes.ERROR_INVALID_USER_NAME;

				case MembershipCreateStatus.ProviderError:
					return UserErrorCodes.ERROR_PROVIDER_ERROR;

				case MembershipCreateStatus.UserRejected:
					return UserErrorCodes.ERROR_USER_REJECTED;

				default:
					return UserErrorCodes.ERROR_DEFAULT;
			}
		}

		#endregion
	}
}