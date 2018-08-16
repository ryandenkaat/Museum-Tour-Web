using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
	public class StoreItemImage : Base
	{
		public int Id { get; set; }

		public byte[] Image { get; set; }
		
		public string FileType { get; set; }

		[Required]
		public virtual StoreItem StoreItem { get; set; }
	}
}