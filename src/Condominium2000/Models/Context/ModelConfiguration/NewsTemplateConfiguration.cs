using System.Data.Entity.ModelConfiguration;

namespace Condominium2000.Models.Context.ModelConfiguration
{
	public class NewsTemplateConfiguration : EntityTypeConfiguration<NewsTemplate>
	{
		internal NewsTemplateConfiguration()
		{
			HasRequired(b => b.SelectedAnnouncement)
				.WithMany()
				.HasForeignKey(b => b.AnnouncementId);
		}
	}
}