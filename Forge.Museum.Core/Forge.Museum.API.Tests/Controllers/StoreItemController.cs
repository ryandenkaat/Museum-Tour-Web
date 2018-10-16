using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.API.Tests.Controllers
{
	[TestClass]
	public class StoreItemController : BaseTestClass
	{
		private API.Controllers.StoreItemController _controller;

		[TestInitialize]
		public void SetupTest()
		{
			_controller = new API.Controllers.StoreItemController(true);
		}
	}
}
