using System.Collections.Generic;
using System.Linq;
using Condominium2000.Helpers;
using Condominium2000.Models.Context;
using PagedList;

namespace Condominium2000.Models.ViewModels
{
	public class NewsViewModel
	{
		/// <summary>
		///     Select what amount of news to populate the NewsViewModel with
		/// </summary>
		public enum DisplayNews
		{
			All,
			Limited,
			None
		}

		public NewsViewModel()
		{
		}

		public NewsViewModel(ref Condominium2000Context context, DisplayNews displayNews)
		{
			// Populate all DB collections
			PopulateNews(ref context, displayNews);
			PopulateFRQ(ref context);

			// Get Template
			NewsTemplate = TemplateHelper.GetNewsTemplate(ref context);

			// Select Announcement
			if (NewsTemplate != null)
			{
				SelectedAnnouncement = NewsTemplate.SelectedAnnouncement;
			}
			else
			{
				SelectedAnnouncement = new Announcement();
			}
		}

		public News SelectedNews { get; set; }
		public Announcement SelectedAnnouncement { get; set; }
		public NewsTemplate NewsTemplate { get; set; }
		public IPagedList<News> PaginationList { get; set; }

		// Collections
		public IEnumerable<News> News { get; set; }
		public IEnumerable<Question> Questions { get; set; }

		private void PopulateNews(ref Condominium2000Context Context, DisplayNews DisplayNews)
		{
			switch (DisplayNews)
			{
				case DisplayNews.All:
				{
					News = Context.News.OrderByDescending(n => n.DateCreated).ToList();
					break;
				}
				case DisplayNews.Limited:
				{
					News = Context.News.OrderByDescending(n => n.DateCreated).Take(Constants.NewsIndexNumberOfNews).ToList();
					break;
				}
				case DisplayNews.None:
				default:
				{
					// Do Nothing
					break;
				}
			}
		}

		/// <summary>
		///     Return the questions marked as FRQ
		/// </summary>
		/// <returns></returns>
		private void PopulateFRQ(ref Condominium2000Context Context)
		{
			Questions = Context.Questions.Where(q => q.IsFrq).ToList();
		}
	}
}