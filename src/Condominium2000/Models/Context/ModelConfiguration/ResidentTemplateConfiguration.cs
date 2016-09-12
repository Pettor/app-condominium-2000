using System.Data.Entity.ModelConfiguration;

namespace Condominium2000.Models.Context.ModelConfiguration
{
	public class ResidentTemplateConfiguration : EntityTypeConfiguration<ResidentTemplate>
	{
		internal ResidentTemplateConfiguration()
		{
			// UnionInfo
			HasRequired(b => b.SelectedResidentInfo)
				.WithMany()
				.HasForeignKey(b => b.ResidentInfoId);

			// SocietyInfo
			HasRequired(b => b.SelectedQuestionsInfo)
				.WithMany()
				.HasForeignKey(b => b.QuestionsInfoId);

			// AnnualReportInfo
			HasRequired(b => b.SelectedLinksInfo)
				.WithMany()
				.HasForeignKey(b => b.LinksInfoId);

			// AnnualMeetingInfo
			HasRequired(b => b.SelectedBookRoomInfo)
				.WithMany()
				.HasForeignKey(b => b.BookRoomInfoId);
		}
	}

	public class ResidentLinkConfiguration : EntityTypeConfiguration<ResidentLink>
	{
		internal ResidentLinkConfiguration()
		{
			// ResidentLink
			HasOptional(i => i.ResidentLinkCategory)
				.WithMany(u => u.ResidentLinks)
				.HasForeignKey(i => i.ResidentLinkCategoryId);
		}
	}
}