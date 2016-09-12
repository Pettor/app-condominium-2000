using System.Linq;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Models
{
	public class NewsHelper
	{
		public static ErrorCodes.EditErrorCodes ValidateChangedNews(News model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var news = Context.News.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}
	}
}