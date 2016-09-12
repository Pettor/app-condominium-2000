using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace Condominium2000.Models
{
	public class Role
	{
		[Key]
		public virtual Guid RoleId { get; set; }

		[Required]
		public virtual string RoleName { get; set; }

		public virtual string Description { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}

	public class User
	{
		[Key]
		public virtual Guid UserId { get; set; }

		[Required]
		public virtual string Username { get; set; }

		[Required]
		public virtual string Email { get; set; }

		[Required, DataType(DataType.Password)]
		public virtual string Password { get; set; }

		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }

		[DataType(DataType.MultilineText)]
		public virtual string Comment { get; set; }

		public virtual bool IsApproved { get; set; }
		public virtual bool IsAnonymous { get; set; }
		public virtual int PasswordFailuresSinceLastSuccess { get; set; }
		public virtual DateTime? LastPasswordFailureDate { get; set; }
		public virtual DateTime? LastActivityDate { get; set; }
		public virtual DateTime? LastLockoutDate { get; set; }
		public virtual DateTime? LastLoginDate { get; set; }
		public virtual string ConfirmationToken { get; set; }
		public virtual DateTime? CreateDate { get; set; }
		public virtual bool IsLockedOut { get; set; }
		public virtual DateTime? LastPasswordChangedDate { get; set; }
		public virtual string PasswordVerificationToken { get; set; }
		public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

		public virtual ICollection<Role> Roles { get; set; }

		public virtual ICollection<News> WrittenNews { get; set; }
	}

	public class ChangePasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = @"Current password")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = @"The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = @"New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = @"Confirm new password")]
		[Compare("NewPassword", ErrorMessage = @"The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class EditUserModel
	{
		[Required]
		public virtual Guid UserId { get; set; }

		[Required]
		public string UserName { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string Email { get; set; }

		public bool IsApproved { get; set; }
	}

	public class LogOnModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = @"Password")]
		public string Password { get; set; }

		[Display(Name = @"Remember me?")]
		public bool RememberMe { get; set; }

		//public string TwoFactorCode { get; set; }
		//[Display(Name = "Google Authenticator Code")]

		//[Required]
	}

	public class RegisterModel
	{
		[Required]
		[Display(Name = @"User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = @"Email address")]
		[Email]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = @"The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = @"Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = @"Confirm password")]
		[Compare("Password", ErrorMessage = @"The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = @"First Name")]
		public string FirstName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = @"Last Name")]
		public string LastName { get; set; }
	}

	public class TwoFactorSecret
	{
		public string EncodedSecret { get; set; }
	}
}