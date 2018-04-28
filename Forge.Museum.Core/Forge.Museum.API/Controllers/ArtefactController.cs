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
    public class ArtefactFilter : ApiFilter
    {
        public int? categoryId { get; set; }
        public int? zoneId { get; set; }
    }

    public class ArtefactController : BaseApiController
    {
        #region CRUD
        [HttpPost, Route("api/artefact")]
        public ArtefactDto Create([FromBody]ArtefactDto dto)
        {
            try
            {
                return new ArtefactHandler().Create(dto);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPut, Route("api/artefact")]
        public ArtefactDto Update([FromBody]ArtefactDto dto)
        {
            try
            {
                return new ArtefactHandler().Update(dto);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/artefact/{artefactId}")]
        public ArtefactDto GetById(int artefactId)
        {
            try
            {
                return new ArtefactHandler().GetById(artefactId);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/artefact")]
        public List<ArtefactDto> GetFiltered([FromUri]ArtefactFilter filter)
        {
            try
            {
                return new ArtefactHandler().GetFiltered(filter);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpDelete, Route("api/artefact/{artefactId}")]
        public bool Delete(int artefactId)
        {
            try
            {
                return new ArtefactHandler().Delete(artefactId);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}