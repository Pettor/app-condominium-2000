using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Condominium2000.Models
{
	public class Question
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
		[AllowHtml]
		public string ContentSv { get; set; }

		[Required]
		[AllowHtml]
		public string ContentEn { get; set; }

		[AllowHtml]
		public string HtmlContentSv { get; set; }

		[AllowHtml]
		public string HtmlContentEn { get; set; }

		[Required]
		public bool IsFrq { get; set; }

		[Required]
		public int ListPriority { get; set; }
	}
}