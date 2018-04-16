using Forge.Museum.API.CoreHandlers;
using Forge.Museum.Interfaces.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.Controllers
{
    public class ExampleController : BaseApiController
    {
        #region CRUD
        //Example method
        //Http method type, Route to the endpoint
        [HttpGet, Route("api/example/{exampleId}")]
        public ExampleDto GetExampleDto(int exampleId)
        {
            try
            {
                return new ExampleHandler().GetExampleDto(exampleId);
            }
            catch(Exception ex)
            {
                //TODO log to some log file/db
                throw;
            }
        }

        #endregion

        #region Extensions

        #endregion
    }
}