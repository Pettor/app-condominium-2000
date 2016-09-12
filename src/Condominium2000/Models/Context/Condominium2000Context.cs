using System.Data.Entity;
using Condominium2000.Models.Context.ModelConfiguration;

namespace Condominium2000.Models.Context
{
	public class Condominium2000Context : DbContext
	{
		/// <summary>
		///     A new context using the Connection string from Web.config (important for Code First to work)
		/// </summary>
		public Condominium2000Context() : base("Condominium2000Context")
		{
			// Set the database intializer
			// BE CAREFUL, WRONG USAGE COULD DELETE DATABASE CONTENT! 

			// Use to update the existing database
			//Database.SetInitializer(new MigrateDatabaseToLatestVersion<Condominium2000Context, Configuration>());

			// Use to initialize the database the first time (run two times if parse error when trying to delete existing information)
			Database.SetInitializer(new Condominium2000ContextInitializer());
		}

		/// <summary>
		///     NEWS
		/// </summary>
		public DbSet<News> News { get; set; }

		public DbSet<NewsTemplate> NewsTemplates { get; set; }
		public DbSet<Announcement> Announcements { get; set; }

		/// <summary>
		///     SOCIETY
		/// </summary>
		public DbSet<SocietyTemplate> SocietyTemplates { get; set; }

		public DbSet<AnnualMeeting> AnnualMeetings { get; set; }
		public DbSet<AnnualReport> AnnualReports { get; set; }
		public DbSet<BoardMember> BoardMembers { get; set; }

		// Info objects
		public DbSet<UnionInfo> UnionInfos { get; set; }
		public DbSet<SocietyInfo> SocietyInfos { get; set; }
		public DbSet<AnnualReportInfo> AnnualReportInfos { get; set; }
		public DbSet<AnnualMeetingInfo> AnnualMeetingInfos { get; set; }

		/// <summary>
		///     RESIDENT
		/// </summary>
		public DbSet<ResidentTemplate> ResidentTemplates { get; set; }

		public DbSet<GalleryImage> GalleryImages { get; set; }
		public DbSet<ResidentLinkCategory> ResidentLinkCategories { get; set; }
		public DbSet<ResidentLink> ResidentLinks { get; set; }

		// Info objects
		public DbSet<ResidentInfo> ResidentInfos { get; set; }
		public DbSet<QuestionsInfo> QuestionsInfos { get; set; }
		public DbSet<LinksInfo> LinksInfos { get; set; }
		public DbSet<BookRoomInfo> BookRoomInfos { get; set; }

		/// <summary>
		///     CONTACT
		/// </summary>
		public DbSet<ContactTemplate> ContactTemplates { get; set; }

		// Info objects
		public DbSet<ContactInfo> ContactInfos { get; set; }
		public DbSet<ErrorFormInfo> ErrorFormInfos { get; set; }
		public DbSet<BoardFormInfo> BoardFormInfos { get; set; }
		public DbSet<ItFormInfo> ItFormInfos { get; set; }

		/// <summary>
		///     GENERAL
		/// </summary>
		public DbSet<Question> Questions { get; set; }

		/// <summary>
		///     Account
		/// </summary>
		public DbSet<User> Users { get; set; }

		public DbSet<Role> Roles { get; set; }

		/// <summary>
		///     Set up configuration for the Database
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new NewsTemplateConfiguration());
			modelBuilder.Configurations.Add(new SocietyTemplateConfiguration());
		}

		/// <summary>
		///     Create a new Context (Database) with Code First
		/// </summary>
		public class Condominium2000ContextInitializer : DropCreateDatabaseAlways<Condominium2000Context>
		{
			/// <summary>
			///     Seed the database with new information when model has changed
			/// </summary>
			/// <param name="context"></param>
			protected override void Seed(Condominium2000Context context)
			{
				SeedData.InitData(context);
			}
		}
	}
}