using System.Collections.Generic;

namespace Condominium2000.Models.ViewModels
{
	public class AnnouncementViewModel
	{
		public Announcement SelectedAnnouncement { get; set; }
		public IEnumerable<Announcement> Announcements { get; set; }
	}
}