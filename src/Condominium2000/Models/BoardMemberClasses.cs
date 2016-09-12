using System;
using System.ComponentModel.DataAnnotations;

namespace Condominium2000.Models
{
	public class BoardMember
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateCreated { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string PositionSv { get; set; }

		[Required]
		public string PositionEn { get; set; }

		public string MobileNr { get; set; }

		public string Mail { get; set; }

		[Required]
		public bool IsOrdinary { get; set; }

		[Required]
		public int ListPriority { get; set; }
	}
}