using System.Runtime.CompilerServices;

namespace Condominium2000.Helpers
{
	public class ErrorCodes
	{
		[TypeForwardedFrom("System.Web, Version=2.0.0.0, Culture=Neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public enum EditErrorCodes
		{
			// Summary:
			//     The object was successfully created.
			Success = 0,
			//
			// Summary:
			//     The object was not found in the database.
			InvalidObjectName = 1,
			//
			// Summary:
			//     The provider returned an error that is not described by other Condominium2000.Helpers.EditErrorCode
			//     enumeration values.
			ProviderError = 2
		}

		[TypeForwardedFrom("System.Web, Version=2.0.0.0, Culture=Neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public enum FileNameErrorCodes
		{
			// Summary:
			//     The object was successfully created.
			Success = 0,
			//
			// Summary:
			//     Filename already exists in the database.
			Duplicate,
			//
			// Summary:
			//     The filename contain a invalid formating.
			InvalidFormat
		}

		public static string ErrorCodeToString(EditErrorCodes status)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (status)
			{
				case EditErrorCodes.InvalidObjectName:
					return Resources.ErrorCodes.ERROR_GENERAL_DUPLICATE_OBJECT_NAME;

				case EditErrorCodes.ProviderError:
					return Resources.ErrorCodes.ERROR_GENERAL_PROVIDER_ERROR;

				default:
					return Resources.ErrorCodes.ERROR_GENERAL_DEFAULT;
			}
		}

		public static string ErrorCodeToString(FileNameErrorCodes status)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (status)
			{
				case FileNameErrorCodes.Duplicate:
					return Resources.ErrorCodes.ERROR_FILENAME_DUPLICATE;

				case FileNameErrorCodes.InvalidFormat:
					return Resources.ErrorCodes.ERROR_FILENAME_INVALID_FORMAT;

				default:
					return Resources.ErrorCodes.ERROR_GENERAL_DEFAULT;
			}
		}
	}
}