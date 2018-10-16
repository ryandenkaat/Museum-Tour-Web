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
	public class StoreItemController : BaseApiController
	{
		private bool isTest;

		public StoreItemController()
		{
			isTest = false;
		}

		public StoreItemController(bool test = false)
		{
			isTest = test;
		}

		#region CRUD
		[HttpPost, Route("api/storeItem")]
		public StoreItemDto Create([FromBody]StoreItemDto dto)
		{
			try
			{
				return new StoreItemHandler(isTest).Create(dto);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		[HttpPut, Route("api/storeItem")]
		public StoreItemDto Update([FromBody]StoreItemDto dto)
		{
			try
			{
				return new StoreItemHandler(isTest).Update(dto);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpGet, Route("api/storeItem/{storeItemId}")]
		public StoreItemDto GetById(int storeItemId)
		{
			try
			{
				return new StoreItemHandler(isTest).GetById(storeItemId);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpGet, Route("api/storeItem")]
		public List<StoreItemDto> GetFiltered([FromUri]ApiFilter filter)
		{
			try
			{
				return new StoreItemHandler(isTest).GetFiltered(filter);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpDelete, Route("api/storeItem/{storeItemId}")]
		public bool Delete(int storeItemId)
		{
			try
			{
				return new StoreItemHandler(isTest).Delete(storeItemId);
			}
			catch(Exception ex)
			{
				throw;
			}
		}
		#endregion


	}
}