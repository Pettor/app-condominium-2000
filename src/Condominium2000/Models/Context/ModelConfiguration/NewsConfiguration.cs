using System.Data.Entity.ModelConfiguration;

namespace Condominium2000.Models.Context.ModelConfiguration
{
	public class NewsConfiguration : EntityTypeConfiguration<News>
	{
		internal NewsConfiguration()
		{
			HasRequired(b => b.WrittenBy)
				.WithMany()
				.HasForeignKey(b => b.Id);
		}
	}
}