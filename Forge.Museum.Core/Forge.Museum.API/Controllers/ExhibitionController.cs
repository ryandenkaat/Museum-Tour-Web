using Forge.Museum.API.CoreHandlers;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Forge.Museum.Interfaces.DataTransferObjects.Exhibition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.Controllers
{
	public class ExhibitionController : BaseApiController
	{
		#region CRUD
		[HttpPost, Route("api/exhibition")]
		public ExhibitionDto Create([FromBody]ExhibitionDto dto)
		{
			try
			{
				return new ExhibitonHandler().Create(dto);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpPut, Route("api/exhibition")]
		public ExhibitionDto Update([FromBody]ExhibitionDto dto)
		{
			try
			{
				return new ExhibitonHandler().Update(dto);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpGet, Route("api/exhibition/{exhibitionId}")]
		public ExhibitionDto GetById(int exhibitionId)
		{
			try
			{
				return new ExhibitonHandler().GetById(exhibitionId);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpGet, Route("api/exhibition")]
		public List<ExhibitionDto> GetFiltered([FromUri]ApiFilter filter)
		{
			try
			{
				return new ExhibitonHandler().GetFiltered(filter);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpDelete, Route("api/exhibition/{exhibitionId}")]
		public bool Delete(int exhibitionId)
		{
			try
			{
				return new ExhibitonHandler().Delete(exhibitionId);
			}
			catch(Exception ex)
			{
				throw;
			}
		}
		#endregion
	}
}