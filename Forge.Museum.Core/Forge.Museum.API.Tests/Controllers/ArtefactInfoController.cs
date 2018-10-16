using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.API.Tests.Controllers
{
	[TestClass]
	public class ArtefactInfoController : BaseTestClass
	{
		private API.Controllers.ArtefactInfoController _controller;

		[TestInitialize]
		public void SetupTest()
		{
			_controller = new API.Controllers.ArtefactInfoController(true);
		}


	}
}
