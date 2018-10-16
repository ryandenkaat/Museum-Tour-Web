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
    public class ArtefactCategoryController : BaseApiController
    {
		private bool isTest;

		public ArtefactCategoryController()
		{
			isTest = false;
		}

		public ArtefactCategoryController(bool test = false)
		{
			isTest = test;
		}

        #region CRUD
        [HttpPost, Route("api/artefactCatergory")]
        public ArtefactCategoryDto Create([FromBody]ArtefactCategoryDto dto)
        {
            try
            {
                return new ArtefactCategoryHandler(isTest).Create(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut, Route("api/artefactCatergory")]
        public ArtefactCategoryDto Update([FromBody]ArtefactCategoryDto dto)
        {
            try
            {
                return new ArtefactCategoryHandler(isTest).Update(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/artefactCatergory/{artefactCatergoryId}")]
        public ArtefactCategoryDto GetById(int artefactCatergoryId)
        {
            try
            {
                return new ArtefactCategoryHandler(isTest).GetById(artefactCatergoryId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet, Route("api/artefactCatergory")]
        public List<ArtefactCategoryDto> GetFiltered([FromUri]ApiFilter filter)
        {
            try
            {
                return new ArtefactCategoryHandler(isTest).GetFiltered(filter);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete, Route("api/artefactCatergory/{artefactCatergoryId}")]
        public bool Delete(int artefactCatergoryId)
        {
            try
            {
                return new ArtefactCategoryHandler(isTest).Delete(artefactCatergoryId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}