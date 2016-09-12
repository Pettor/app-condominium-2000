using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Condominium2000.Models
{
	public class ResidentTemplate
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		// ResidentInfo
		public int ResidentInfoId { get; set; }
		public virtual ResidentInfo SelectedResidentInfo { get; set; }

		// QuestionsInfo
		public int QuestionsInfoId { get; set; }
		public virtual QuestionsInfo SelectedQuestionsInfo { get; set; }

		// LinksInfo
		public int LinksInfoId { get; set; }
		public virtual LinksInfo SelectedLinksInfo { get; set; }

		// BookRoomInfo
		public int BookRoomInfoId { get; set; }
		public virtual BookRoomInfo SelectedBookRoomInfo { get; set; }
	}

	public class ResidentInfo
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

	public class QuestionsInfo
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

	public class LinksInfo
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

	public class BookRoomInfo
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