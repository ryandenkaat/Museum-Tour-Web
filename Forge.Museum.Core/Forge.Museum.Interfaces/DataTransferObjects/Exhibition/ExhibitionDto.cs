using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.Interfaces.DataTransferObjects.Exhibition
{
	public class ExhibitionDto : BaseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime FinishDate { get; set; }
		public string Organiser { get; set; }
		public double Price_Adult { get; set; }
		public double Price_Concession { get; set; }
		public double Price_Child { get; set; }
		public byte[] Image { get; set; }
	}
}
