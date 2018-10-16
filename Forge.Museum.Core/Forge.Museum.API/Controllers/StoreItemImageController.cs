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
		private bool isTest;

		public StoreItemImageController()
		{
			isTest = false;
		}

		public StoreItemImageController(bool test = false)
		{
			isTest = test;
		}

		#region CRUD
		[HttpPost, Route("api/storeItemImage")]
		public StoreItemImageDto Create([FromBody]StoreItemImageDto dto)
		{
			try
			{
				return new StoreItemImageHandler(isTest).Create(dto);
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
				return new StoreItemImageHandler(isTest).Update(dto);
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
				return new StoreItemImageHandler(isTest).GetById(storeItemImageId);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpGet, Route("api/storeItemImage")]
		public List<StoreItemImageDto> GetFiltered([FromUri] ApiFilter filter)
		{
			try
			{
				return new StoreItemImageHandler(isTest).GetFiltered(filter);
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
				return new StoreItemImageHandler(isTest).Delete(storeItemImageId);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		#endregion
	}
}