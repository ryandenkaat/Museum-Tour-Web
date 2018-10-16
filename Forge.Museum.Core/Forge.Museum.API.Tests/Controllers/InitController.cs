using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.API.Tests.Controllers
{
	[TestClass]
	public class InitController
	{
		[AssemblyInitialize]
		public static void AssemblyInit(TestContext content)
		{
			AutoMapperConfig.CreateMaps();
		}
	}
}
