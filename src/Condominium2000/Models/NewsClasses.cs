using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Condominium2000.Models
{
	public class News
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string TitleSv { get; set; }

		[Required]
		public string TitleEn { get; set; }

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

		public virtual User WrittenBy { get; set; }
	}

	public class NewsTemplate
	{
		[Key]
		public int Id { get; set; }

		public int AnnouncementId { get; set; }

		public virtual Announcement SelectedAnnouncement { get; set; }
	}
}