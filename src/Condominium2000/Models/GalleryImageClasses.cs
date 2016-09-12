using System;
using System.ComponentModel.DataAnnotations;

namespace Condominium2000.Models
{
	public class GalleryImage
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		public string ImageName { get; set; }

		public string CaptionSv { get; set; }

		public string CaptionEn { get; set; }

		[Required]
		public int FileSize { get; set; }

		[Required]
		public string FileType { get; set; }

		[Required]
		public string FilePath { get; set; }

		[Required]
		public bool IsPromoted { get; set; }

		[Required]
		public int ListPriority { get; set; }
	}
}