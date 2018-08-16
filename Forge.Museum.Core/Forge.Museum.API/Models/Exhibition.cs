using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
	public class Exhibition : Base
	{
		public int Id { get; set; }

		[StringLength(256)]
		public string Name { get; set; }

		[StringLength(4000)]
		public string Description { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime FinishDate { get; set; }

		[StringLength(256)]
		public string Organiser { get; set; }

		public double Price_Adult { get; set; }

		public double Price_Concession { get; set; }

		public double Price_Child { get; set; }

		public byte[] Image { get; set; }
	}
}