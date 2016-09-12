using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Condominium2000.Models
{
	public class ResidentLinkCategory
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		public string TitleSv { get; set; }

		[Required]
		public string TitleEn { get; set; }

		[Required]
		public int ListPriority { get; set; }

		public virtual ICollection<ResidentLink> ResidentLinks { get; set; }
	}

	public class ResidentLink
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[AllowHtml]
		public string ContentSv { get; set; }

		[Required]
		[AllowHtml]
		public string ContentEn { get; set; }

		public string Link { get; set; }

		public string PhoneNumber { get; set; }

		public int? ResidentLinkCategoryId { get; set; }
		public virtual ResidentLinkCategory ResidentLinkCategory { get; set; }

		[Required]
		public int ListPriority { get; set; }
	}
}