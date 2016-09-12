using System.Text.RegularExpressions;

namespace Condominium2000.Helpers
{
	public class FileHelper
	{
		public enum FileFormat
		{
			Other,
			Word,
			Excel,
			Pdf,
			Png,
			Jpg,
			Gif
		}

		public enum FileType
		{
			Other,
			Img,
			Application
		}

		/// <summary>
		///     Get the fileformat from content type
		/// </summary>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public static FileFormat GetFileFormatFromContentType(string contentType)
		{
			var result = FileFormat.Other;
			char[] delimiterChars = {'/'};

			var words = contentType.Split(delimiterChars);

			var format = "";
			if (words[0] != null)
			{
				format = words[1];
			}

			// Search for known fileformat patterns
			Regex.Match(format, "wordprocessingml|spreadsheetml|pdf|png|jpg|jpeg|gif", RegexOptions.Compiled);

			switch (format)
			{
				case "vnd.openxmlformats-officedocument.wordprocessingml.document":
				{
					result = FileFormat.Word;
					break;
				}
				case "vnd.openxmlformats-officedocument.spreadsheetml.sheet":
				{
					result = FileFormat.Excel;
					break;
				}
				case "pdf":
				{
					result = FileFormat.Pdf;
					break;
				}
				case "png":
				{
					result = FileFormat.Png;
					break;
				}
				case "jpg":
				case "jpeg":
				{
					result = FileFormat.Jpg;
					break;
				}
				case "gif":
				{
					result = FileFormat.Gif;
					break;
				}
			}

			return result;
		}

		/// <summary>
		///     Get the filetype from content type
		/// </summary>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public static FileType GetFileTypeFromContentType(string contentType)
		{
			var result = FileType.Other;
			char[] delimiterChars = {'/'};

			var words = contentType.Split(delimiterChars);

			var type = "";
			if (words[0] != null)
			{
				type = words[0];
			}

			switch (type)
			{
				case "application":
				{
					result = FileType.Application;
					break;
				}

				case "image":
				{
					result = FileType.Img;
					break;
				}
			}

			return result;
		}

		/// <summary>
		///     Pretty format content size
		/// </summary>
		/// <param name="contentSize"></param>
		/// <returns></returns>
		public static string DisplaySize(int contentSize)
		{
			string result;
			var sizeInKb = contentSize/1024;

			// Size is 1mb or more
			if (sizeInKb >= 1024)
			{
				result = sizeInKb/1024 + "MB";
			}
			else
			{
				result = sizeInKb + "KB";
			}

			return result;
		}
	}
}