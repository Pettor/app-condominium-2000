using System.Linq;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Models
{
	public class ResidentHelper
	{
		public enum SubMenu
		{
			GeneraInformation,
			Questions,
			Gallery,
			Links,
			BookRoom
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedResidentInfo(ResidentInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var ri = Context.ResidentInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedQuestionsInfo(QuestionsInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var qi = Context.QuestionsInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedLinksInfo(LinksInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var li = Context.LinksInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedBookRoomInfo(BookRoomInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var bri = Context.BookRoomInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}
	}
}