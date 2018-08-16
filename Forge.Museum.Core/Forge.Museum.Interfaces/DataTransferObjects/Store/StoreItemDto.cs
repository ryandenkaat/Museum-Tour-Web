using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.Interfaces.DataTransferObjects.Store
{
	public class StoreItemDto : BaseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Cost { get; set; }
		public bool InStock { get; set; }
		public List<StoreItemImageDto> StoreItemImages { get; set; }
	}
}
