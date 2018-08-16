using Forge.Museum.API.CoreHandlers;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Forge.Museum.Interfaces.DataTransferObjects.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.Controllers
{
	public class StoreItemImageController : BaseApiController
	{
		#region CRUD
		[HttpPost, Route("api/storeItemImage")]
		public StoreItemImageDto Create([FromBody]StoreItemImageDto dto)
		{
			try
			{
				return new StoreItemImageHandler().Create(dto);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpPut, Route("api/storeItemImage")]
		public StoreItemImageDto Update([FromBody]StoreItemImageDto dto)
		{
			try
			{
				return new StoreItemImageHandler().Update(dto);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpGet, Route("api/storeItemImage/{storeItemImageId}")]
		public StoreItemImageDto GetById(int storeItemImageId)
		{
			try
			{
				return new StoreItemImageHandler().GetById(storeItemImageId);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpGet, Route("api/storeItemImage")]
		public List<StoreItemImageDto> GetFiltered(ApiFilter filter)
		{
			try
			{
				return new StoreItemImageHandler().GetFiltered(filter);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpDelete, Route("api/storeItemImage/{storeItemImageId}")]
		public bool Delete(int storeItemImageId)
		{
			try
			{
				return new StoreItemImageHandler().Delete(storeItemImageId);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		#endregion
	}
}