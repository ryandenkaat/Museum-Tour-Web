using Forge.Museum.API.CoreHandlers;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.Controllers
{
    public class TourFilter : ApiFilter
    {
        public int? artefactId { get; set; }
    }

    public class TourController : BaseApiController
    {
        #region CRUD
        [HttpPost, Route("api/tour")]
        public TourDto Create([FromBody]TourDto dto)
        {
            try
            {
                return new TourHandler().Create(dto);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPut, Route("api/tour")]
        public TourDto Update([FromBody]TourDto dto)
        {
            try
            {
                return new TourHandler().Update(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/tour/{tourId}")]
        public TourDto GetById(int tourId)
        {
            try
            {
                return new TourHandler().GetById(tourId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/tour")]
        public List<TourDto> GetFiltered([FromUri]TourFilter filter)
        {
            try
            {
                return new TourHandler().GetFiltered(filter);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete, Route("api/tour/{tourId}")]
        public bool Delete(int tourId)
        {
            try
            {
                return new TourHandler().Delete(tourId);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}