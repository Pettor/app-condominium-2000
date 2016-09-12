using System;
using System.ComponentModel.DataAnnotations;

namespace Condominium2000.Models
{
	public class AnnualMeeting
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		public string NameSv { get; set; }

		[Required]
		public string NameEn { get; set; }

		[Required]
		public int FileSize { get; set; }

		[Required]
		public string FileType { get; set; }

		[Required]
		public string FilePath { get; set; }
	}
}