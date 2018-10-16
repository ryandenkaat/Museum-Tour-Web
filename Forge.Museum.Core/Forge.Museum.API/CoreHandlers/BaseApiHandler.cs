using Forge.Museum.API.Models;
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
        protected ApplicationDbContext Db;

        protected BaseApiHandler()
        {
            Db = new ApplicationDbContext();
        }

		protected BaseApiHandler(bool test = false)
		{
			if(test)
			{
				Db = new ApplicationDbContext("TestConnection");
			}
			else 
			{
				Db = new ApplicationDbContext();
			}
		}

        protected BaseApiHandler(ApplicationDbContext context)
        {
            Db = context;
        }

        protected void NotFoundException()
        {
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}