using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Condominium2000.Helpers.Membership;
using Condominium2000.Helpers.Models;
using Condominium2000.Helpers.Tags;

namespace Condominium2000.Models.Context
{
	public class SeedData
	{
		/// <summary>
		///     Contain data used for initial database
		/// </summary>
		public static void InitData(Condominium2000Context context)
		{
			// Create PINNED NEWS

			// Ann1
			const string ann1Sv = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam nisi tortor, viverra eu elementum eu, feugiat quis leo.";
			const string ann1En = @"Sed et tempor lorem, eget accumsan ex.";

			new List<Announcement>
			{
				new Announcement
				{
					Id = 1
					,
					TitleSv = "Quisque"
					,
					TitleEn = "Aliquam"
					,
					SubTitleSv = "A pulvinar sem."
					,
					SubTitleEn = "Etiam nunc lacus."
					,
					DateCreated = new DateTime(2012, 12, 27, 13, 10, 12)
					,
					ContentSv = ann1Sv
					,
					ContentEn = ann1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(ann1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(ann1En)
				}
			}.ForEach(b => context.Announcements.Add(b));

			// Create QUESTIONS

			// Question1
			const string question1Sv = @"Ut iaculis sollicitudin dignissim. Mauris enim est, pretium id diam ac, tincidunt finibus leo. Praesent sit amet mollis ligula, dapibus efficitur lacus. Nullam ligula dolor, faucibus nec nulla ac, feugiat condimentum justo. Proin ullamcorper felis ut massa volutpat tincidunt. Cras porttitor sed tortor sit amet mattis. Cras varius risus vitae enim ornare dignissim. Etiam urna felis, fringilla id dolor eu, viverra porttitor ipsum. Aliquam gravida mollis aliquam.";
			const string question1En = @"Vestibulum sit amet lorem ac purus aliquet vestibulum quis nec eros. Nam aliquet dolor viverra tristique sodales. Vivamus in pellentesque justo. Praesent quis nisl varius, luctus nulla quis, laoreet quam. Nunc blandit augue ex, sed fringilla ex luctus eu. Pellentesque sodales bibendum tellus et gravida.";

			// Question2

			const string question2Sv = @"List:
[list=1]
	[*] Aliquam1
	[*] Aliquam2
[/list]
[linktab=""http://www.link.com""]Pulvinar[/link]";

			const string question2En = @"List:
[list=1]
	[*] Aliquam1
	[*] Aliquam2
[/list]
[linktab=""http://www.link.com""]Pulvinar[/link]";


			new List<Question>
			{
				new Question
				{
					TitleSv = "Nascetur"
					,
					TitleEn = "Quisque"
					,
					ListPriority = 1
					,
					DateCreated = new DateTime(2012, 12, 27, 11, 23, 12)
					,
					ContentSv = question1Sv
					,
					ContentEn = question1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(question1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(question1En)
					,
					IsFrq = true
				},
				new Question
				{
					TitleSv = "Etiam"
					,
					TitleEn = "Praesent"
					,
					ListPriority = 2
					,
					DateCreated = new DateTime(2012, 12, 27, 11, 23, 12)
					,
					ContentSv = question2Sv
					,
					ContentEn = question2En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(question2Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(question2En)
					,
					IsFrq = false
				},
			}.ForEach(b => context.Questions.Add(b));

			// Save DB
			context.SaveChanges();

			// Create NEWS TEMPLATE
			new List<NewsTemplate>
			{
				new NewsTemplate
				{
					SelectedAnnouncement = context.Announcements.FirstOrDefault(c => c.Id == 1)
				}
			}.ForEach(b => context.NewsTemplates.Add(b));


			new List<BoardMember>
			{
				new BoardMember
				{
					IsOrdinary = true
					,
					DateCreated = DateTime.Now
					,
					Name = "Feugiat Condimentum"
					,
					PositionSv = "Vestibulum"
					,
					PositionEn = "Consequat"
					,
					Mail = "someone@mail.com"
					,
					MobileNr = "0789 - 12 34 56"
					,
					ListPriority = 1
				},
				new BoardMember
				{
					IsOrdinary = true
					,
					DateCreated = DateTime.Now
					,
					Name = "Tincidunt Finibus"
					,
					PositionSv = "Vestibulum"
					,
					PositionEn = "Consequat"
					,
					Mail = "someone2@mail.com"
					,
					MobileNr = "0789 - 98 78 56"
					,
					ListPriority = 2
				},
			}.ForEach(b => context.BoardMembers.Add(b));

			// Create UNION INFOS

			// Union1

			const string union1Sv = @"Vestibulum sit amet lorem ac purus aliquet vestibulum quis nec eros. Nam aliquet dolor viverra tristique sodales. Vivamus in pellentesque justo. Praesent quis nisl varius, luctus nulla quis, laoreet quam. Nunc blandit augue ex, sed fringilla ex luctus eu. Pellentesque sodales bibendum tellus et gravida. Quisque ac nunc interdum, venenatis erat sed, gravida nisl.";

			const string union1En =
				@"Aenean sit amet aliquet lectus, sed tempor justo. Nam et neque pulvinar, faucibus lorem convallis, commodo odio. Pellentesque nec ipsum sit amet ante pellentesque scelerisque et eu nisi. In maximus commodo ullamcorper. Donec aliquam, ipsum in condimentum malesuada, nisi ante posuere ligula, eu mattis elit turpis at justo. Aenean eu accumsan neque. Maecenas blandit pulvinar sem vel elementum.";

			new List<UnionInfo>
			{
				new UnionInfo
				{
					ContentSv = union1Sv
					,
					ContentEn = union1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(union1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(union1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.UnionInfos.Add(b));

			// Create SOCIETY INFOS
			new List<SocietyInfo>
			{
				new SocietyInfo
				{
					ContentSv = "Proin molestie hendrerit purus eget scelerisque."
					,
					ContentEn = "Quisque ornare, turpis eu suscipit fermentum."
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.SocietyInfos.Add(b));

			// Create ANNUAL MEETING INFOS

			// Meeting1

			const string meeting1Sv = @"Sed a libero auctor, gravida augue et, sollicitudin elit. Curabitur vulputate arcu nunc, eu porttitor justo lobortis pretium. Morbi sit amet placerat enim. Suspendisse eget elementum dui.";

			const string meeting1En = @"Suspendisse in iaculis risus. Aliquam finibus, turpis ut faucibus fermentum, ipsum risus viverra justo, vel bibendum tortor odio commodo massa.";

			new List<AnnualMeetingInfo>
			{
				new AnnualMeetingInfo
				{
					ContentSv = meeting1Sv
					,
					ContentEn = meeting1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(meeting1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(meeting1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.AnnualMeetingInfos.Add(b));

			// Create ANNUAL REPORT INFOS
			const string annual1Sv = @"Sed suscipit sollicitudin tortor ac mattis. Nullam egestas ipsum at urna tincidunt convallis.";
			const string annual1En = @"Nunc ullamcorper auctor libero, nec semper augue tristique ac. Etiam interdum semper elit";

			new List<AnnualReportInfo>
			{
				new AnnualReportInfo
				{
					ContentSv = annual1Sv
					,
					ContentEn = annual1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(annual1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(annual1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.AnnualReportInfos.Add(b));

			new List<AnnualReport>
			{
				new AnnualReport
				{
					NameSv = "Mauris 2011"
					,
					NameEn = "Consectetur 2011"
					,
					DateCreated = new DateTime(2013, 02, 11, 17, 23, 15)
					,
					FilePath = "/Uploads/AnnualReports/annual.pdf"
					,
					FileSize = 232476
					,
					FileType = "pdf"
				},
				new AnnualReport
				{
					NameSv = "Mauris 2012"
					,
					NameEn = "Consectetur 2012"
					,
					DateCreated = new DateTime(2013, 04, 08, 18, 33, 58)
					,
					FilePath = "/Uploads/AnnualReports/annual2.pdf"
					,
					FileSize = 2843237
					,
					FileType = "pdf"
				}
			}.ForEach(b => context.AnnualReports.Add(b));

			new List<AnnualMeeting>
			{
				new AnnualMeeting
				{
					NameSv = "Curabitur 2013"
					,
					NameEn = "Donec 2013"
					,
					DateCreated = new DateTime(2013, 04, 04, 20, 58, 18)
					,
					FilePath = "/Uploads/AnnualMeetings/annual3.pdf"
					,
					FileSize = 321520
					,
					FileType = "pdf"
				}
			}.ForEach(b => context.AnnualMeetings.Add(b));

			// Save DB
			context.SaveChanges();

			// Create test SOCIETY TEMPLATE
			new List<SocietyTemplate>
			{
				new SocietyTemplate
				{
					SelectedUnionInfo = context.UnionInfos.FirstOrDefault()
					,
					SelectedSocietyInfo = context.SocietyInfos.FirstOrDefault()
					,
					SelectedAnnualMeetingInfo = context.AnnualMeetingInfos.FirstOrDefault()
					,
					SelectedAnnualReportInfo = context.AnnualReportInfos.FirstOrDefault()
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.SocietyTemplates.Add(b));
			
			// Create QALLERY IMAGES
			new List<GalleryImage>
			{
				new GalleryImage
				{
					ImageName = "Vivamus"
					,
					CaptionSv = "Vivamus."
					,
					CaptionEn = "Pellentesque."
					,
					DateCreated = new DateTime(2012, 12, 29)
					,
					ListPriority = 4
					,
					IsPromoted = true
					,
					FilePath = "http://www.link.com/pick1.png"
					,
					FileSize = 155182
					,
					FileType = "jpg"
				},
				new GalleryImage
				{
					ImageName = "Vivamus2"
					,
					CaptionSv = "Vivamus."
					,
					CaptionEn = "Pellentesque."
					,
					DateCreated = new DateTime(2012, 12, 29)
					,
					ListPriority = 1
					,
					IsPromoted = true
					,
					FilePath = "http://www.link.com/pick2.png"
					,
					FileSize = 224649
					,
					FileType = "jpg"
				},
				new GalleryImage
				{
					ImageName = "Bathroom"
					,
					CaptionSv = "Badrum."
					,
					CaptionEn = "Bathroom."
					,
					DateCreated = new DateTime(2012, 12, 29)
					,
					ListPriority = 5
					,
					IsPromoted = true
					,
					FilePath = "http://www.Condominium2000.se/Uploads/GalleryImages/gallery_7.jpg"
					,
					FileSize = 52000
					,
					FileType = "jpg"
				}
			}.ForEach(b => context.GalleryImages.Add(b));

			// Create RESIDENT INFOS

			// Resident1

			const string resident1Sv = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam nisi tortor, viverra eu elementum eu, feugiat quis leo. Sed et tempor lorem, eget accumsan ex. Proin interdum lectus vel sollicitudin imperdiet. Duis ut elit eget dui maximus faucibus. Mauris ante diam, eleifend interdum libero non, consectetur suscipit dui.";
			const string resident1En = @"Ut iaculis sollicitudin dignissim. Mauris enim est, pretium id diam ac, tincidunt finibus leo. Praesent sit amet mollis ligula, dapibus efficitur lacus. Nullam ligula dolor, faucibus nec nulla ac, feugiat condimentum justo.";

			new List<ResidentInfo>
			{
				new ResidentInfo
				{
					ContentSv = resident1Sv
					,
					ContentEn = resident1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(resident1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(resident1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.ResidentInfos.Add(b));

			// Create QUESTION INFOS
			new List<QuestionsInfo>
			{
				new QuestionsInfo
				{
					ContentSv = "Duis ut elit eget dui maximus faucibus."
					,
					ContentEn = "Quisque et sem ut est bibendum consequat."
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.QuestionsInfos.Add(b));

			// Create LINK INFOS
			new List<LinksInfo>
			{
				new LinksInfo
				{
					ContentSv = @"Duis ut elit eget dui maximus faucibus."
					,
					ContentEn = @"Quisque et sem ut est bibendum consequat."
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.LinksInfos.Add(b));

			// Create BOOKROOM INFOS

			// Book1
			const string book1Sv = @"Ut iaculis sollicitudin dignissim. Mauris enim est, pretium id diam ac, tincidunt finibus leo. Praesent sit amet mollis ligula, dapibus efficitur lacus.";
			const string book1En = @"Vestibulum sit amet lorem ac purus aliquet vestibulum quis nec eros. Nam aliquet dolor viverra tristique sodales. Vivamus in pellentesque justo.";

			new List<BookRoomInfo>
			{
				new BookRoomInfo
				{
					ContentSv = book1Sv
					,
					ContentEn = book1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(book1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(book1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.BookRoomInfos.Add(b));

			// Save DB
			context.SaveChanges();
			// Create test RESIDENT TEMPLATE
			new List<ResidentTemplate>
			{
				new ResidentTemplate
				{
					SelectedResidentInfo = context.ResidentInfos.FirstOrDefault()
					,
					SelectedQuestionsInfo = context.QuestionsInfos.FirstOrDefault()
					,
					SelectedLinksInfo = context.LinksInfos.FirstOrDefault()
					,
					SelectedBookRoomInfo = context.BookRoomInfos.FirstOrDefault()
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.ResidentTemplates.Add(b));

			// Create RESIDENT LINK CATEGORIES
			new List<ResidentLinkCategory>
			{
				new ResidentLinkCategory
				{
					TitleSv = "Luctus nulla quis."
					,
					TitleEn = "Laoreet quam."
					,
					ListPriority = 1
					,
					DateCreated = DateTime.Now
				},
				new ResidentLinkCategory
				{
					TitleSv = "Nunc blandit augue ex."
					,
					TitleEn = "Sed fringilla ex luctus eu."
					,
					ListPriority = 2
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(c => context.ResidentLinkCategories.Add(c));
			// Save DB
			context.SaveChanges();

			// Create RESIDENT LINKS
			new List<ResidentLink>
			{
				// PROVIDERS
				new ResidentLink
				{
					Name = "Pretium"
					,
					ContentSv = "Tincidunt finibus leo."
					,
					ContentEn = "Praesent sit amet mollis ligula."
					,
					Link = "http://www.something.se"
					,
					PhoneNumber = "0770-777 000"
					,
					DateCreated = DateTime.Now
					,
					ResidentLinkCategory = context.ResidentLinkCategories.FirstOrDefault()
					,
					ListPriority = 1
				},
			}.ForEach(c => context.ResidentLinks.Add(c));

			// Contact1
			const string contact1Sv = @"Vestibulum sit amet lorem ac purus aliquet vestibulum quis nec eros. Nam aliquet dolor viverra tristique sodales. Vivamus in pellentesque justo.";
			const string contact1En = @"Quisque ac nunc interdum, venenatis erat sed, gravida nisl. Aenean sit amet aliquet lectus, sed tempor justo. Nam et neque pulvinar, faucibus lorem convallis, commodo odio.";

			new List<ContactInfo>
			{
				new ContactInfo
				{
					ContentSv = contact1Sv
					,
					ContentEn = contact1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(contact1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(contact1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.ContactInfos.Add(b));

			// Create ERROR FORM INFOS

			// Error1
			const string error1Sv = @"Sed a libero auctor, gravida augue et, sollicitudin elit. Curabitur vulputate arcu nunc, eu porttitor justo lobortis pretium. Morbi sit amet placerat enim. Suspendisse eget elementum dui.";
			const string error1En = @"Suspendisse in iaculis risus. Aliquam finibus, turpis ut faucibus fermentum, ipsum risus viverra justo, vel bibendum tortor odio commodo massa.";

			new List<ErrorFormInfo>
			{
				new ErrorFormInfo
				{
					ContentSv = error1Sv
					,
					ContentEn = error1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(error1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(error1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.ErrorFormInfos.Add(b));

			// Create BOARD FORM INFOS

			// Board1
			const string board1Sv = @"Sed suscipit sollicitudin tortor ac mattis. Nullam egestas ipsum at urna tincidunt convallis.";
			const string board1En = @"Nunc ullamcorper auctor libero, nec semper augue tristique ac. Etiam interdum semper elit.";

			new List<BoardFormInfo>
			{
				new BoardFormInfo
				{
					ContentSv = board1Sv
					,
					ContentEn = board1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(board1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(board1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.BoardFormInfos.Add(b));

			// Create IT FORM INFOS

			// IT1
			const string it1Sv = @"Sed suscipit sollicitudin tortor ac mattis. Nullam egestas ipsum at urna tincidunt convallis.";
			const string it1En = @"Nunc ullamcorper auctor libero, nec semper augue tristique ac. Etiam interdum semper elit.";

			new List<ItFormInfo>
			{
				new ItFormInfo
				{
					ContentSv = it1Sv
					,
					ContentEn = it1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(it1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(it1En)
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.ItFormInfos.Add(b));

			// Save DB
			context.SaveChanges();
			// Create test CONTACT TEMPLATE
			new List<ContactTemplate>
			{
				new ContactTemplate
				{
					SelectedContactInfo = context.ContactInfos.FirstOrDefault()
					,
					SelectedErrorFormInfo = context.ErrorFormInfos.FirstOrDefault()
					,
					SelectedBoardFormInfo = context.BoardFormInfos.FirstOrDefault()
					,
					SelectedItFormInfo = context.ItFormInfos.FirstOrDefault()
					,
					DateCreated = DateTime.Now
				}
			}.ForEach(b => context.ContactTemplates.Add(b));

			// Create the Roles
			Roles.CreateRole(RolesHelper.Roles.Admin.ToString());
			Roles.CreateRole(RolesHelper.Roles.SuperAdmin.ToString());

			// Create test ACCOUNT SUPERADMIN
			WebSecurity.Register("SuperAdmin", "abc1231!", "it@Condominium2000.se", true, "Mr.", "Admin");
			Roles.AddUserToRole("SuperAdmin", RolesHelper.Roles.SuperAdmin.ToString());

			// Save DB
			context.SaveChanges();


			// News1
			const string news1Sv = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam nisi tortor, viverra eu elementum eu, feugiat quis leo. Sed et tempor lorem, eget accumsan ex. Proin interdum lectus vel sollicitudin imperdiet. Duis ut elit eget dui maximus faucibus. Mauris ante diam, eleifend interdum libero non, consectetur suscipit dui. Mauris vestibulum enim nibh, vitae consequat est sollicitudin ac. Ut ultricies libero mauris. Quisque et sem ut est bibendum [link=""/Contact/ITForm"">consequat""]consequat[/link].";
			const string news1En = @"Quisque sed aliquam ligula, a pulvinar sem. Etiam nunc lacus, sagittis ut dolor id, elementum aliquet lectus. Curabitur rhoncus molestie vulputate. Duis nec mattis est. Cras tempor felis nec tempor sollicitudin. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Fusce a nunc rhoncus, varius orci eget, dictum orci. Interdum et malesuada fames ac ante ipsum primis in faucibus. [link=""/Contact/ITForm"">consequat""]consequat[/link].";

			// News2
			var news2Sv = @"Ut iaculis sollicitudin dignissim. Mauris enim est, pretium id diam ac, tincidunt finibus leo. Praesent sit amet mollis ligula, dapibus efficitur lacus. Nullam ligula dolor, faucibus nec nulla ac, feugiat condimentum justo. Proin ullamcorper felis ut massa volutpat tincidunt. Cras porttitor sed tortor sit amet mattis. Cras varius risus vitae enim ornare dignissim. Etiam urna felis, fringilla id dolor eu.

[note]viverra porttitor ipsum. Aliquam gravida mollis aliquam.[/note]";
			var news2En = @"Vestibulum sit amet lorem ac purus aliquet vestibulum quis nec eros. Nam aliquet dolor viverra tristique sodales. Vivamus in pellentesque justo. Praesent quis nisl varius, luctus nulla quis, laoreet quam. Nunc blandit augue ex, sed fringilla ex luctus eu. Pellentesque sodales bibendum tellus et gravida. Quisque ac nunc interdum, venenatis erat sed, gravida nisl. Aenean sit amet aliquet lectus, sed tempor justo. Nam et neque pulvinar, faucibus lorem convallis, commodo odio.

[note]viverra porttitor ipsum. Aliquam gravida mollis aliquam.[/note]";
			
			// News3
			var news3Sv = @"Aliquam

[list]
[*] aliquam1

[*] aliquam2

[*] aliquam3
[/list]";

			var news3En = @"Aliquam

[list]
[*] aliquam1

[*] aliquam2

[*] aliquam3
[/list]";

			// News4
			const string news4Sv = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam nisi tortor, viverra eu elementum eu, feugiat quis leo. Sed et tempor lorem, eget accumsan ex. Proin interdum lectus vel sollicitudin imperdiet. Duis ut elit eget dui maximus faucibus. Mauris ante diam, eleifend interdum libero non, consectetur suscipit dui. Mauris vestibulum enim nibh, vitae consequat est sollicitudin ac. Ut ultricies libero mauris. Quisque et sem ut est bibendum [link=""/Contact/ITForm"">consequat""]consequat[/link].";
			const string news4En = @"Quisque sed aliquam ligula, a pulvinar sem. Etiam nunc lacus, sagittis ut dolor id, elementum aliquet lectus. Curabitur rhoncus molestie vulputate. Duis nec mattis est. Cras tempor felis nec tempor sollicitudin. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Fusce a nunc rhoncus, varius orci eget, dictum orci. Interdum et malesuada fames ac ante ipsum primis in faucibus. [link=""/Contact/ITForm"">consequat""]consequat[/link].";

			// News5
			const string news5Sv = @"Ut iaculis sollicitudin dignissim. Mauris enim est, pretium id diam ac, tincidunt finibus leo. Praesent sit amet mollis ligula, dapibus efficitur lacus. Nullam ligula dolor, faucibus nec nulla ac, feugiat condimentum justo. Proin ullamcorper felis ut massa volutpat tincidunt. Cras porttitor sed tortor sit amet mattis. Cras varius risus vitae enim ornare dignissim. Etiam urna felis, fringilla id dolor eu.

[note]viverra porttitor ipsum. Aliquam gravida mollis aliquam.[/note]";
			const string news5En = @"Vestibulum sit amet lorem ac purus aliquet vestibulum quis nec eros. Nam aliquet dolor viverra tristique sodales. Vivamus in pellentesque justo. Praesent quis nisl varius, luctus nulla quis, laoreet quam. Nunc blandit augue ex, sed fringilla ex luctus eu. Pellentesque sodales bibendum tellus et gravida. Quisque ac nunc interdum, venenatis erat sed, gravida nisl. Aenean sit amet aliquet lectus, sed tempor justo. Nam et neque pulvinar, faucibus lorem convallis, commodo odio.

[note]viverra porttitor ipsum. Aliquam gravida mollis aliquam.[/note]";

			// News1
			const string news6Sv = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam nisi tortor, viverra eu elementum eu, feugiat quis leo. Sed et tempor lorem, eget accumsan ex. Proin interdum lectus vel sollicitudin imperdiet. Duis ut elit eget dui maximus faucibus. Mauris ante diam, eleifend interdum libero non, consectetur suscipit dui. Mauris vestibulum enim nibh, vitae consequat est sollicitudin ac. Ut ultricies libero mauris. Quisque et sem ut est bibendum [link=""/Contact/ITForm"">consequat""]consequat[/link].";
			const string news6En = @"Quisque sed aliquam ligula, a pulvinar sem. Etiam nunc lacus, sagittis ut dolor id, elementum aliquet lectus. Curabitur rhoncus molestie vulputate. Duis nec mattis est. Cras tempor felis nec tempor sollicitudin. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Fusce a nunc rhoncus, varius orci eget, dictum orci. Interdum et malesuada fames ac ante ipsum primis in faucibus. [link=""/Contact/ITForm"">consequat""]consequat[/link].";

			// News2
			const string news7Sv = @"Ut iaculis sollicitudin dignissim. Mauris enim est, pretium id diam ac, tincidunt finibus leo. Praesent sit amet mollis ligula, dapibus efficitur lacus. Nullam ligula dolor, faucibus nec nulla ac, feugiat condimentum justo. Proin ullamcorper felis ut massa volutpat tincidunt. Cras porttitor sed tortor sit amet mattis. Cras varius risus vitae enim ornare dignissim. Etiam urna felis, fringilla id dolor eu.

[note]viverra porttitor ipsum. Aliquam gravida mollis aliquam.[/note]";
			const string news7En = @"Vestibulum sit amet lorem ac purus aliquet vestibulum quis nec eros. Nam aliquet dolor viverra tristique sodales. Vivamus in pellentesque justo. Praesent quis nisl varius, luctus nulla quis, laoreet quam. Nunc blandit augue ex, sed fringilla ex luctus eu. Pellentesque sodales bibendum tellus et gravida. Quisque ac nunc interdum, venenatis erat sed, gravida nisl. Aenean sit amet aliquet lectus, sed tempor justo. Nam et neque pulvinar, faucibus lorem convallis, commodo odio.

[note]viverra porttitor ipsum. Aliquam gravida mollis aliquam.[/note]";

			// Create NEWS
			new List<News>
			{
				new News
				{
					TitleSv = "Mauris enim est"
					,
					TitleEn = "Ut iaculis sollicitudin"
					,
					WrittenBy = context.Users.FirstOrDefault(c => c.Username == "SuperAdmin")
					,
					DateCreated = new DateTime(2012, 12, 28, 15, 13, 54)
					,
					ContentSv = news1Sv
					,
					ContentEn = news1En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news1Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news1En)
				},
				new News
				{
					TitleSv = "Ut iaculis sollicitudin"
					,
					TitleEn = "Mauris enim est"
					,
					WrittenBy = context.Users.FirstOrDefault(c => c.Username == "SuperAdmin")
					,
					DateCreated = new DateTime(2013, 01, 06, 17, 58, 23)
					,
					ContentSv = news2Sv
					,
					ContentEn = news2En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news2Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news2En)
				},
				new News
				{
					TitleSv = "Sed molestie massa"
					,
					TitleEn = "posuere tristique facilisis"
					,
					WrittenBy = context.Users.FirstOrDefault(c => c.Username == "SuperAdmin")
					,
					DateCreated = new DateTime(2013, 01, 14, 18, 08, 34)
					,
					ContentSv = news3Sv
					,
					ContentEn = news3En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news3Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news3En)
				},
				new News
				{
					TitleSv = "Duis neque nisl"
					,
					TitleEn = "Ullamcorper ornare justo"
					,
					WrittenBy = null
					,
					DateCreated = new DateTime(2013, 01, 20, 13, 04, 24)
					,
					ContentSv = news4Sv
					,
					ContentEn = news4En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news4Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news4En)
				},
				new News
				{
					TitleSv = "Auctor varius erat"
					,
					TitleEn = "Integer nisi ligula"
					,
					WrittenBy = context.Users.FirstOrDefault(c => c.Username == "SuperAdmin")
					,
					DateCreated = new DateTime(2013, 02, 12, 22, 29, 11)
					,
					ContentSv = news5Sv
					,
					ContentEn = news5En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news5Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news5En)
				},
				new News
				{
					TitleSv = "Phasellus faucibus"
					,
					TitleEn = "Enim vel rhoncus"
					,
					WrittenBy = context.Users.FirstOrDefault(c => c.Username == "SuperAdmin")
					,
					DateCreated = new DateTime(2013, 02, 20, 21, 16, 42)
					,
					ContentSv = news6Sv
					,
					ContentEn = news6En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news6Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news6En)
				},
				new News
				{
					TitleSv = "Donec vitae enim"
					,
					TitleEn = "Fusce sed odio"
					,
					WrittenBy = context.Users.FirstOrDefault(c => c.Username == "SuperAdmin")
					,
					DateCreated = new DateTime(2013, 04, 04, 21, 04, 42)
					,
					ContentSv = news7Sv
					,
					ContentEn = news7En
					,
					HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news7Sv)
					,
					HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news7En)
				}
			}.ForEach(b => context.News.Add(b));

			// Save DB
			context.SaveChanges();
		}
	}
}