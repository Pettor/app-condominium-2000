using System.Collections.Generic;
using System.Linq;
using Condominium2000.Helpers;
using Condominium2000.Models.Context;

namespace Condominium2000.Models.ViewModels
{
	public class SocietyViewModel
	{
		public SocietyViewModel()
		{
		}

		public SocietyViewModel(ref Condominium2000Context context)
		{
			// Populate all DB collections
			PopulateAnnualReports(ref context);
			PopulateAnnualMeetings(ref context);
			PopulateBoardMembers(ref context);

			// Get Template
			SocietyTemplate = TemplateHelper.GetSocietyTemplate(ref context);
		}

		public SocietyTemplate SocietyTemplate { get; set; }

		// Collections
		public IEnumerable<AnnualMeeting> AnnualMeetings { get; set; }
		public IEnumerable<AnnualReport> AnnualReports { get; set; }
		public IEnumerable<BoardMember> BoardMembers { get; set; }

		private void PopulateAnnualMeetings(ref Condominium2000Context context)
		{
			AnnualMeetings = context.AnnualMeetings.ToList();
		}

		private void PopulateAnnualReports(ref Condominium2000Context context)
		{
			AnnualReports = context.AnnualReports.ToList();
		}

		private void PopulateBoardMembers(ref Condominium2000Context context)
		{
			BoardMembers = context.BoardMembers.OrderBy(m => m.ListPriority).ToList();
		}
	}
}