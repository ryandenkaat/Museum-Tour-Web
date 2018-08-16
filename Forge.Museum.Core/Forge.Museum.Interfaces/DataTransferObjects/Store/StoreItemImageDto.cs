using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.Interfaces.DataTransferObjects.Store
{
	public class StoreItemImageDto : BaseDto
	{
		public int Id { get; set; }
		public byte[] Image { get; set; }
		public string FileType { get; set; }
		public StoreItemSimpleDto StoreItem { get; set; }
	}
}
