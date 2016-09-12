using System.Data.Entity.ModelConfiguration;

namespace Condominium2000.Models.Context.ModelConfiguration
{
	public class SocietyTemplateConfiguration : EntityTypeConfiguration<SocietyTemplate>
	{
		internal SocietyTemplateConfiguration()
		{
			// UnionInfo
			HasRequired(b => b.SelectedUnionInfo)
				.WithMany()
				.HasForeignKey(b => b.UnionInfoId);

			// SocietyInfo
			HasRequired(b => b.SelectedSocietyInfo)
				.WithMany()
				.HasForeignKey(b => b.SocietyInfoId);

			// AnnualReportInfo
			HasRequired(b => b.SelectedAnnualReportInfo)
				.WithMany()
				.HasForeignKey(b => b.AnnualReportInfoId);

			// AnnualMeetingInfo
			HasRequired(b => b.SelectedAnnualMeetingInfo)
				.WithMany()
				.HasForeignKey(b => b.AnnualMeetingInfoId);
		}
	}
}