using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.CoreHandlers
{
    public class BaseApiHandler
    {
        //TODO add DB connection

        protected BaseApiHandler()
        {

        }

        protected void NotFoundException()
        {
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}