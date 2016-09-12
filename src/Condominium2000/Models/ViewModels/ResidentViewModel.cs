using System.Collections.Generic;
using System.Linq;
using Condominium2000.Helpers;
using Condominium2000.Models.Context;

namespace Condominium2000.Models.ViewModels
{
	public class ResidentViewModel
	{
		public ResidentViewModel()
		{
		}

		public ResidentViewModel(ref Condominium2000Context context)
		{
			// Populate all DB collections
			PopulateQuestions(ref context);
			PopulateFrontPageGalleryImages(ref context);
			PopulateAllGalleryImages(ref context);
			PopulateResidentLinkCategories(ref context);

			// Get Template
			ResidentTemplate = TemplateHelper.GetResidentTemplate(ref context);
		}

		public ResidentTemplate ResidentTemplate { get; set; }

		// Collections
		public IEnumerable<Question> Questions { get; set; }
		public IEnumerable<GalleryImage> FrontPageGalleryImages { get; set; }
		public IEnumerable<GalleryImage> AllGalleryImages { get; set; }
		public IEnumerable<ResidentLinkCategory> ResidentLinkCategories { get; set; }

		private void PopulateResidentLinkCategories(ref Condominium2000Context context)
		{
			ResidentLinkCategories = context.ResidentLinkCategories.OrderBy(c => c.ListPriority).ToList();
		}

		private void PopulateQuestions(ref Condominium2000Context context)
		{
			Questions = context.Questions.OrderBy(c => c.ListPriority).ToList();
		}

		private void PopulateFrontPageGalleryImages(ref Condominium2000Context context)
		{
			FrontPageGalleryImages = context.GalleryImages.Where(gi => gi.IsPromoted).OrderBy(gi => gi.ListPriority).ToList();
		}

		private void PopulateAllGalleryImages(ref Condominium2000Context context)
		{
			AllGalleryImages = context.GalleryImages.OrderBy(gi => gi.ListPriority).ToList();
		}
	}
}