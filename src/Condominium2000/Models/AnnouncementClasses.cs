using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Condominium2000.Helpers;
using Condominium2000.Models.Context;

namespace Condominium2000.Models
{
	public class Announcement
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string TitleSv { get; set; }

		[Required]
		public string TitleEn { get; set; }

		[Required]
		public string SubTitleSv { get; set; }

		[Required]
		public string SubTitleEn { get; set; }

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

	public class EditSelectedAnnouncementModel
	{
		public EditSelectedAnnouncementModel()
		{
		}

		public EditSelectedAnnouncementModel(ref Condominium2000Context context)
		{
			PopulateNewsTemplates(ref context);

			// Select Announcement
			var selAnn = TemplateHelper.GetNewsTemplate(ref context).SelectedAnnouncement;
			if (selAnn != null)
			{
				SelectedAnnouncementId = selAnn.Id;
			}
			else
			{
				SelectedAnnouncementId = 0;
			}
		}

		[Required]
		public int SelectedAnnouncementId { get; set; }

		public List<Announcement> Announcements { get; set; }

		private void PopulateNewsTemplates(ref Condominium2000Context context)
		{
			Announcements = context.Announcements.OrderByDescending(a => a.DateCreated).ToList();
		}
	}
}