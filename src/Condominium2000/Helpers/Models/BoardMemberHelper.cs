using System.Linq;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Models
{
	public class BoardMemberHelper
	{
		public static ErrorCodes.EditErrorCodes ValidateChangedBoardMember(BoardMember model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var boardMember = Context.BoardMembers.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}
	}
}