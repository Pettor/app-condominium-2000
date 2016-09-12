using System.Linq;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Models
{
	public class SocietyHelper
	{
		public enum SubMenu
		{
			Society,
			Union,
			AnnualReport,
			AnnualMeeting
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedUnionInfo(UnionInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var ui = Context.UnionInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedSocietyInfo(SocietyInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var ui = Context.SocietyInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedAnnualReportInfo(AnnualReportInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var ar = Context.AnnualReportInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedAnnualMeetingInfo(AnnualMeetingInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var am = Context.AnnualMeetingInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}
	}
}