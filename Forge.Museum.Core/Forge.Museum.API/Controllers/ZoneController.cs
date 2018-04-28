using Forge.Museum.API.CoreHandlers;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.Controllers
{
    public class ZoneController : BaseApiController
    {
        #region CRUD
        [HttpPost, Route("api/zone")]
        public ZoneDto Create([FromBody]ZoneDto dto)
        {
            try
            {
                return new ZoneHandler().Create(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut, Route("api/zone")]
        public ZoneDto Update([FromBody]ZoneDto dto)
        {
            try
            {
                return new ZoneHandler().Update(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/zone/{zoneId}")]
        public ZoneDto GetById(int zoneId)
        {
            try
            {
                return new ZoneHandler().GetById(zoneId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/zone")]
        public List<ZoneDto> GetFiltered([FromUri]ApiFilter filter)
        {
            try
            {
                return new ZoneHandler().GetFiltered(filter);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete, Route("api/zone/{zoneId}")]
        public bool Delete(int zoneId)
        {
            try
            {
                return new ZoneHandler().Delete(zoneId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}