﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.API.Tests.Controllers
{
	[TestClass]
	public class ArtefactController : BaseTestClass
	{
		private API.Controllers.ArtefactController _controller;

		[TestInitialize]
		public void SetupTest()
		{
			_controller = new API.Controllers.ArtefactController(true);
		}


	}
}
