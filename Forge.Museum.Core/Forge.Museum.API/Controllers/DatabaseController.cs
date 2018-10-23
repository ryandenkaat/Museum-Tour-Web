using Forge.Museum.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.Controllers
{
	public class DatabaseController : BaseApiController
	{
		[HttpGet]
		[Route("api/initialize")]
		public bool DbInitialize([FromUri]bool isTest = false)
		{
			string connectionType = "DefaultConnection";

			if(isTest)
			{
				connectionType = "TestConnection";
			}

			var db = new ApplicationDbContext(connectionType);
			db.Database.Initialize(false);

			return true;
		}

		[HttpGet]
		[Route("api/{tsn}/exists")]
		public bool DbExists([FromUri]bool isTest = false)
		{
			string connectionType = "DefaultConnection";

			if (isTest)
			{
				connectionType = "TestConnection";
			}

			var db = new ApplicationDbContext(connectionType);

			return db.Database.Exists();
		}
	}
}