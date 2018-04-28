﻿using Forge.Museum.API.CoreHandlers;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.Controllers
{
    public class ArtefactInfoController : BaseApiController
    {
        #region CRUD
        [HttpPost, Route("api/artefactInfo")]
        public ArtefactInfoDto Create([FromBody]ArtefactInfoDto dto)
        {
            try
            {
                return new ArtefactInfoHandler().Create(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut, Route("api/artefactInfo")]
        public ArtefactInfoDto Update([FromBody]ArtefactInfoDto dto)
        {
            try
            {
                return new ArtefactInfoHandler().Update(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/artefactInfo/{artefactInfoId}")]
        public ArtefactInfoDto GetById(int artefactInfoId)
        {
            try
            {
                return new ArtefactInfoHandler().GetById(artefactInfoId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/artefactInfo")]
        public List<ArtefactInfoDto> GetFiltered([FromUri]ApiFilter filter, [FromUri]int? artefactId = null)
        {
            try
            {
                return new ArtefactInfoHandler().GetFiltered(filter, artefactId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete, Route("api/artefactInfo/{artefactInfoId}")]
        public bool Delete(int artefactInfoId)
        {
            try
            {
                return new ArtefactCategoryHandler().Delete(artefactInfoId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}