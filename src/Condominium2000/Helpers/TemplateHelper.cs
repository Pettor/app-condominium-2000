using System.Linq;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers
{
	public class TemplateHelper
	{
		public static NewsTemplate GetNewsTemplate(ref Condominium2000Context context)
		{
			return context.NewsTemplates.FirstOrDefault(a => a.Id == Constants.NewsDbTemplateId);
		}

		public static SocietyTemplate GetSocietyTemplate(ref Condominium2000Context context)
		{
			return context.SocietyTemplates.FirstOrDefault(a => a.Id == Constants.SocietyDbTemplateId);
		}

		public static ResidentTemplate GetResidentTemplate(ref Condominium2000Context context)
		{
			return context.ResidentTemplates.FirstOrDefault(a => a.Id == Constants.ResidentDbTemplateId);
		}

		public static ContactTemplate GetContactTemplate(ref Condominium2000Context context)
		{
			return context.ContactTemplates.FirstOrDefault(a => a.Id == Constants.ContactDbTemplateId);
		}
	}
}