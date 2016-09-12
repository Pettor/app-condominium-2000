using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Condominium2000.Models
{
	public class SocietyTemplate
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		// UnionInfo
		public int UnionInfoId { get; set; }
		public virtual UnionInfo SelectedUnionInfo { get; set; }

		// SocietyInfo
		public int SocietyInfoId { get; set; }
		public virtual SocietyInfo SelectedSocietyInfo { get; set; }

		// AnnualReportInfo
		public int AnnualReportInfoId { get; set; }
		public virtual AnnualReportInfo SelectedAnnualReportInfo { get; set; }

		// AnnualMeetingInfo
		public int AnnualMeetingInfoId { get; set; }
		public virtual AnnualMeetingInfo SelectedAnnualMeetingInfo { get; set; }
	}

	public class UnionInfo
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

	public class SocietyInfo
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

	public class AnnualReportInfo
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

	public class AnnualMeetingInfo
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