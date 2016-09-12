using System.Linq;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Models
{
	public class ContactHelper
	{
		public enum SubMenu
		{
			Contact,
			ErrorForm,
			BoardForm,
			ITForm
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedContactInfo(ContactInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var ci = Context.ContactInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedErrorFormInfo(ErrorFormInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var efi = Context.ErrorFormInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedBoardFormInfo(BoardFormInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var bfi = Context.BoardFormInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateChangedITFormInfo(ItFormInfo model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var ifi = Context.ItFormInfos.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateErrorForm(ErrorForm errorForm)
		{
			return ErrorCodes.EditErrorCodes.Success;
		}

		public static ErrorCodes.EditErrorCodes ValidateITForm(ItForm itForm)
		{
			return ErrorCodes.EditErrorCodes.Success;
		}

		public static ErrorCodes.EditErrorCodes ValidateBoardForm(BoardForm boardForm)
		{
			return ErrorCodes.EditErrorCodes.Success;
		}
	}
}