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
    public class TourArtefactFilter : ApiFilter
    {
        public int? tourId { get; set; }

        public int? artefactId { get; set; } 
    }

    public class TourArtefactController : BaseApiController
    {
		private bool isTest;

		public TourArtefactController()
		{
			isTest = false;
		}

		public TourArtefactController(bool test = false)
		{
			isTest = test;
		}

		#region CRUD
		[HttpPost, Route("api/tourArtefact")]
        public TourArtefactDto Create([FromBody]TourArtefactDto dto)
        {
            try
            {
                return new TourArtefactHandler(isTest).Create(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut, Route("api/tourArtefact")]
        public TourArtefactDto Update([FromBody]TourArtefactDto dto)
        {
            try
            {
                return new TourArtefactHandler(isTest).Update(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/tourArtefact/{tourArtefactId}")]
        public TourArtefactDto GetById(int tourArtefactId)
        {
            try
            {
                return new TourArtefactHandler(isTest).GetById(tourArtefactId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/tourArtefact")]
        public List<TourArtefactDto> GetFiltered([FromUri]TourArtefactFilter filter)
        {
            try
            {
                return new TourArtefactHandler(isTest).GetFiltered(filter);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete, Route("api/tourArtefact/{tourArtefactId}")]
        public bool Delete(int tourArtefactId)
        {
            try
            {
                return new TourArtefactHandler(isTest).Delete(tourArtefactId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

    }
}