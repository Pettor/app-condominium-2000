using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Condominium2000.Helpers;
using Condominium2000.Resources;
using DataAnnotationsExtensions;

namespace Condominium2000.Models
{
	public interface IContactForm
	{
		string Name { get; set; }

		string Mail { get; set; }

		string AppartmentNr { get; set; }

		SelectList CategoryList { get; set; }

		int SelectedCategory { get; set; }

		string Title { get; set; }

		string Content { get; set; }

		SelectList GetSelectList();
		string ConvertFormCategoryToLanguageString(int formCategory);
	}

	// Category enums for the forms
	public enum ErrorFormCategory
	{
		General = 1,
		Garage = 2,
		Courtyard = 3,
		Elevator = 4,
		Guestroom = 5,
		Relax = 6,
		Cellar = 7,
		Bicycleroom = 8,
		GarbageDisposal = 9,
		Noise = 10,
		Water = 11,
		WebPage = 12
	}

	public enum BoardFormCategory
	{
		General = 1,
		Board = 2,
		Mimolett = 3,
		Sublease = 4,
		Sale = 5,
		Garage = 6,
		Courtyard = 7,
		Elevator = 8,
		Guestroom = 9,
		Relax = 10,
		Cellar = 11,
		Bicycleroom = 12,
		GarbageDisposal = 13,
		Noise = 14,
		Water = 15
	}

	public enum ItFormCategory
	{
		General = 1,
		WebPage = 2,
		Forum = 3,
		BookRoom = 4
	}

	public class ErrorForm : IContactForm
	{
		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_NAME_REQUIRED")]
		public virtual string Name { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_EMAIL_REQUIRED")]
		[DisplayName(@"My Email Address")]
		[Email(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_EMAIL_INVALID")]
		public virtual string Mail { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_NAME_APPART_NR_REQUIRED")]
		public virtual string AppartmentNr { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_TITLE_REQUIRED")]
		[StringLength(100)]
		public virtual string Title { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_CONTENT_REQUIRED")]
		public virtual string Content { get; set; }

		[Required]
		public virtual SelectList CategoryList { get; set; }

		[Required]
		public virtual int SelectedCategory { get; set; }

		public SelectList GetSelectList()
		{
			var result = new Dictionary<string, string>();
			var items = Enum.GetValues(typeof(ErrorFormCategory)).Cast<ErrorFormCategory>().ToList();
			foreach (var item in items)
			{
				var enumValue = (int)item;

				// Convert the enum value to it's value
				result.Add(ConvertFormCategoryToLanguageString(enumValue), enumValue.ToString());
			}
			return new SelectList(result, "value", "key");
		}

		public string ConvertFormCategoryToLanguageString(int formCategory)
		{
			switch (formCategory)
			{
				case 1:
					return Resources.Contact.ErrorForm.ENUM_VALUE_GENERAL;

				case 2:
					return Resources.Contact.ErrorForm.ENUM_VALUE_GARAGE;

				case 3:
					return Resources.Contact.ErrorForm.ENUM_VALUE_COURTYARD;

				case 4:
					return Resources.Contact.ErrorForm.ENUM_VALUE_ELEVATOR;

				case 5:
					return Resources.Contact.ErrorForm.ENUM_VALUE_GUEST_ROOM;

				case 6:
					return Resources.Contact.ErrorForm.ENUM_VALUE_RELAX;

				case 7:
					return Resources.Contact.ErrorForm.ENUM_VALUE_CELLAR;

				case 8:
					return Resources.Contact.ErrorForm.ENUM_VALUE_BICYCLE_ROOM;

				case 9:
					return Resources.Contact.ErrorForm.ENUM_VALUE_GARBAGE_DISPOSAL;

				case 10:
					return Resources.Contact.ErrorForm.ENUM_VALUE_NOISE;

				case 11:
					return Resources.Contact.ErrorForm.ENUM_VALUE_WATER;

				case 12:
					return Resources.Contact.ErrorForm.ENUM_VALUE_WEB_PAGE;

				default:
					return "";
			}
		}

		public List<string> GetMainRecipientsFromCategory(ErrorFormCategory category)
		{
			var recipients = new List<string>();

			switch (category)
			{
				case ErrorFormCategory.General:
				case ErrorFormCategory.Noise:
				case ErrorFormCategory.WebPage:
				case ErrorFormCategory.Guestroom:
				case ErrorFormCategory.Relax:
				case ErrorFormCategory.Bicycleroom:
				case ErrorFormCategory.Cellar:
				case ErrorFormCategory.Courtyard:
				case ErrorFormCategory.Elevator:
				case ErrorFormCategory.Garage:
				case ErrorFormCategory.GarbageDisposal:
				case ErrorFormCategory.Water:
					recipients.Add(Constants.MailErrorFormTo);
					break;
			}

			return recipients;
		}
	}


	public class BoardForm : IContactForm
	{
		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_NAME_REQUIRED")]
		public virtual string Name { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_EMAIL_REQUIRED")]
		[DisplayName(@"My Email Address")]
		[Email(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_EMAIL_INVALID")]
		public virtual string Mail { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_NAME_APPART_NR_REQUIRED")]
		public virtual string AppartmentNr { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_TITLE_REQUIRED")]
		[StringLength(100)]
		public virtual string Title { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_CONTENT_REQUIRED")]
		public virtual string Content { get; set; }

		[Required]
		public virtual SelectList CategoryList { get; set; }

		[Required]
		public virtual int SelectedCategory { get; set; }

		public SelectList GetSelectList()
		{
			var result = new Dictionary<string, string>();
			var items = Enum.GetValues(typeof(BoardFormCategory)).Cast<BoardFormCategory>().ToList();
			foreach (var item in items)
			{
				var enumValue = (int)item;

				// Convert the enum value to it's value
				result.Add(ConvertFormCategoryToLanguageString(enumValue), enumValue.ToString());
			}
			return new SelectList(result, "value", "key");
		}

		public string ConvertFormCategoryToLanguageString(int formCategory)
		{
			switch (formCategory)
			{
				case 1:
					return Resources.Contact.BoardForm.ENUM_VALUE_GENERAL;

				case 2:
					return Resources.Contact.BoardForm.ENUM_VALUE_BOARD;

				case 3:
					return Resources.Contact.BoardForm.ENUM_VALUE_MIMOLETT;

				case 4:
					return Resources.Contact.BoardForm.ENUM_VALUE_SUBLEASE;

				case 5:
					return Resources.Contact.BoardForm.ENUM_VALUE_SALE;

				case 6:
					return Resources.Contact.BoardForm.ENUM_VALUE_GARAGE;

				case 7:
					return Resources.Contact.BoardForm.ENUM_VALUE_COURTYARD;

				case 8:
					return Resources.Contact.BoardForm.ENUM_VALUE_ELEVATOR;

				case 9:
					return Resources.Contact.BoardForm.ENUM_VALUE_GUEST_ROOM;

				case 10:
					return Resources.Contact.BoardForm.ENUM_VALUE_RELAX;

				case 11:
					return Resources.Contact.BoardForm.ENUM_VALUE_CELLAR;

				case 12:
					return Resources.Contact.BoardForm.ENUM_VALUE_BICYCLE_ROOM;

				case 13:
					return Resources.Contact.BoardForm.ENUM_VALUE_GARBAGE_DISPOSAL;

				case 14:
					return Resources.Contact.BoardForm.ENUM_VALUE_NOISE;

				case 15:
					return Resources.Contact.BoardForm.ENUM_VALUE_WATER;

				default:
					return "";
			}
		}

		public List<string> GetMainRecipientsFromCategory(BoardFormCategory category)
		{
			var recipients = new List<string>();

			switch (category)
			{
				case BoardFormCategory.Garage:
				case BoardFormCategory.Courtyard:
				case BoardFormCategory.Water:
				case BoardFormCategory.Bicycleroom:
				case BoardFormCategory.Board:
				case BoardFormCategory.Cellar:
				case BoardFormCategory.Elevator:
				case BoardFormCategory.GarbageDisposal:
				case BoardFormCategory.Guestroom:
				case BoardFormCategory.Mimolett:
				case BoardFormCategory.Noise:
				case BoardFormCategory.Relax:
				case BoardFormCategory.Sale:
				case BoardFormCategory.Sublease:
					recipients.Add(Constants.MailBoardFormTo);
					break;
			}

			return recipients;
		}
	}

	public class ItForm : IContactForm
	{
		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_NAME_REQUIRED")]
		public virtual string Name { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_EMAIL_REQUIRED")]
		[DisplayName(@"My Email Address")]
		[Email(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_EMAIL_INVALID")]
		public virtual string Mail { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_NAME_APPART_NR_REQUIRED")]
		public virtual string AppartmentNr { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_TITLE_REQUIRED")]
		[StringLength(100)]
		public virtual string Title { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(ValidationMessages),
			ErrorMessageResourceName = "VALIDATION_CONTENT_REQUIRED")]
		public virtual string Content { get; set; }

		[Required]
		public virtual SelectList CategoryList { get; set; }

		[Required]
		public virtual int SelectedCategory { get; set; }

		public SelectList GetSelectList()
		{
			var result = new Dictionary<string, string>();
			var items = Enum.GetValues(typeof(ItFormCategory)).Cast<ItFormCategory>().ToList();
			foreach (var item in items)
			{
				var enumValue = (int)item;

				// Convert the enum value to it's value
				result.Add(ConvertFormCategoryToLanguageString(enumValue), enumValue.ToString());
			}
			return new SelectList(result, "value", "key");
		}

		public string ConvertFormCategoryToLanguageString(int formCategory)
		{
			switch (formCategory)
			{
				case 1:
					return Resources.Contact.ITForm.ENUM_VALUE_GENERAL;

				case 2:
					return Resources.Contact.ITForm.ENUM_VALUE_WEB_PAGE;

				case 3:
					return Resources.Contact.ITForm.ENUM_VALUE_FORUM;

				case 4:
					return Resources.Contact.ITForm.ENUM_VALUE_BOOK_ROOM;

				default:
					return "";
			}
		}

		public List<string> GetMainRecipientsFromCategory(ItFormCategory category)
		{
			var recipients = new List<string>();

			switch (category)
			{
				case ItFormCategory.BookRoom:
				case ItFormCategory.Forum:
				case ItFormCategory.General:
				case ItFormCategory.WebPage:
					recipients.Add(Constants.MailItFormTo);
					break;
			}

			return recipients;
		}
	}
}