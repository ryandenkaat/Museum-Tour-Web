using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
	public class StoreItem : Base
	{
		public StoreItem()
		{
			StoreItemImages = new HashSet<StoreItemImage>();
		}

		public int Id { get; set; }

		[StringLength(256)]
		public string Name { get; set; }

		[StringLength(4000)]
		public string Description { get; set; }

		public double Cost { get; set; }

		public bool InStock { get; set; }

		public virtual ICollection<StoreItemImage> StoreItemImages { get; set; }
	}
}