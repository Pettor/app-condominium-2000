using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Condominium2000.Models
{
	public class ContactTemplate
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		// ContactInfo
		public int ContactInfoId { get; set; }
		public virtual ContactInfo SelectedContactInfo { get; set; }

		// ErrorFormInfo
		public int ErrorFormInfoId { get; set; }
		public virtual ErrorFormInfo SelectedErrorFormInfo { get; set; }

		// BoardFormInfo
		public int BoardFormInfoId { get; set; }
		public virtual BoardFormInfo SelectedBoardFormInfo { get; set; }

		// ITFormInfo
		public int ItFormInfoId { get; set; }
		public virtual ItFormInfo SelectedItFormInfo { get; set; }
	}

	public class ContactInfo
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		public string ContentSv { get; set; }

		[Required]
		public string ContentEn { get; set; }

		[AllowHtml]
		public string HtmlContentSv { get; set; }

		[AllowHtml]
		public string HtmlContentEn { get; set; }
	}

	public class ErrorFormInfo
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		[AllowHtml]
		public string ContentSv { get; set; }

		[Required]
		[AllowHtml]
		public string ContentEn { get; set; }

		[AllowHtml]
		public string HtmlContentSv { get; set; }

		[AllowHtml]
		public string HtmlContentEn { get; set; }
	}

	public class BoardFormInfo
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		[AllowHtml]
		public string ContentSv { get; set; }

		[Required]
		[AllowHtml]
		public string ContentEn { get; set; }

		[AllowHtml]
		public string HtmlContentSv { get; set; }

		[AllowHtml]
		public string HtmlContentEn { get; set; }
	}

	public class ItFormInfo
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		[AllowHtml]
		public string ContentSv { get; set; }

		[Required]
		[AllowHtml]
		public string ContentEn { get; set; }

		[AllowHtml]
		public string HtmlContentSv { get; set; }

		[AllowHtml]
		public string HtmlContentEn { get; set; }
	}
}