using System.Linq;
using Condominium2000.Models;
using Condominium2000.Models.Context;

namespace Condominium2000.Helpers.Models
{
	public class AnnualReportHelper
	{
		public static ErrorCodes.EditErrorCodes ValidateChangedAnnualReport(AnnualReport model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				var boardMember = Context.AnnualReports.FirstOrDefault(a => a.Id == model.Id);
				// Do any validation?

				return result;
			}
		}

		public static ErrorCodes.EditErrorCodes ValidateAnnualReport(AnnualReport model)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.EditErrorCodes.Success;

				// Check if name already exists
				var dbArSv = Context.AnnualReports.FirstOrDefault(r => r.NameSv == model.NameSv);
				if (dbArSv == null)
				{
					var dbArEn = Context.AnnualReports.FirstOrDefault(r => r.NameEn == model.NameEn);
					if (dbArEn != null)
					{
						result = ErrorCodes.EditErrorCodes.InvalidObjectName;
					}
				}
				else
				{
					result = ErrorCodes.EditErrorCodes.InvalidObjectName;
				}

				return result;
			}
		}

		public static ErrorCodes.FileNameErrorCodes ValidateFileName(string fileName)
		{
			using (var Context = new Condominium2000Context())
			{
				var result = ErrorCodes.FileNameErrorCodes.Success;

				// Check that filename has valid format
				var hasValidFileFormat = true;
				char[] delimiterCharsFormat = {'.'};
				var wordParts = fileName.Split(delimiterCharsFormat);
				if (wordParts.Length > 2)
				{
					// There are more than two "." in the filename so format is invalid
					hasValidFileFormat = false;
				}

				if (hasValidFileFormat)
				{
					// Check that the filename is unique
					var hasUniqueFileName = true;
					char[] delimiterCharsPath = {'/'};
					foreach (var ar in Context.AnnualReports)
					{
						var words = ar.FilePath.Split(delimiterCharsPath);
						var filename = words.Last();

						if (filename.Equals(fileName))
						{
							hasUniqueFileName = false;
							break;
						}
					}

					if (!hasUniqueFileName)
					{
						result = ErrorCodes.FileNameErrorCodes.Duplicate;
					}
				}
				else
				{
					result = ErrorCodes.FileNameErrorCodes.InvalidFormat;
				}

				return result;
			}
		}
	}
}