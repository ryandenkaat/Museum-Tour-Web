using Forge.Museum.API.Controllers;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;

namespace Forge.Museum.API.Tests.Controllers
{
    [TestClass]
    public class ZoneController : BaseTestClass
    {
        private API.Controllers.ZoneController _controller;

        [TestInitialize]
        public void SetupTest()
        {
            _controller = new API.Controllers.ZoneController(true);
        }

        #region Methods
        #region Create
        /// <summary>
        /// Create - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_ShouldSucceed()
        {
            //Set up dto
            ZoneDto zone = new ZoneDto()
            {
                Name = "Test",
                Description = "Test"
            };

            //Make test request
            ZoneDto zoneResult = _controller.Create(zone);

            //Assert Values
            Assert.IsNotNull(zoneResult);
            Assert.IsNotNull(zoneResult.Id);
            Assert.IsTrue(zoneResult.Id != 0);
            Assert.AreEqual(zone.Name, zoneResult.Name);
            Assert.AreEqual(zone.Description, zoneResult.Description);
            Assert.IsNotNull(zoneResult.CreatedDate);
            Assert.IsNotNull(zoneResult.ModifiedDate);
        }

        /// <summary>
        /// Create - No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_NoName()
        {
            //Set up dto
            ZoneDto zone = new ZoneDto()
            {
                Name = null,
                Description = "Test"
            };

            _controller.Create(zone);
        }

        /// <summary>
        /// Create - Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_EmptyName()
        {
            //Set up dto
            ZoneDto zone = new ZoneDto()
            {
                Name = string.Empty,
                Description = "Test"
            };

            _controller.Create(zone);
        }

        /// <summary>
        /// No Description - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoDescription()
        {
            //Set up dto
            ZoneDto zone = new ZoneDto()
            {
                Name = "Test",
                Description = null
            };

            //Make test request
            ZoneDto zoneResult = _controller.Create(zone);

            //Assert Values
            Assert.IsNotNull(zoneResult);
            Assert.IsNotNull(zoneResult.Id);
            Assert.IsTrue(zoneResult.Id != 0);
            Assert.AreEqual(zone.Description, zoneResult.Description);
        }

        /// <summary>
        /// No Image - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoImage()
        {
            //Set up dto
            ZoneDto zone = new ZoneDto()
            {
                Name = "Test",
                Description = "Test"
            };

            //Make test request
            ZoneDto zoneResult = _controller.Create(zone);

            //Assert Values
            Assert.IsNotNull(zoneResult);
            Assert.IsNotNull(zoneResult.Id);
            Assert.IsTrue(zoneResult.Id != 0);
        }
        #endregion

        #region Update
        /// <summary>
        /// Should Succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_ShouldSucceed()
        {
            //Get valid zone
            ZoneDto zone = GetZone();

            zone.Name = "Test Edit";
            zone.Description = "Test Edit";
            ZoneDto updatedZone = _controller.Update(zone);

            Assert.IsNotNull(updatedZone);
            Assert.IsNotNull(updatedZone.Id);
            Assert.AreEqual(zone.Id, updatedZone.Id);
            Assert.AreEqual(zone.Name, updatedZone.Name);
            Assert.AreEqual(zone.Description, updatedZone.Description);
        }

        /// <summary>
        /// No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_NoName()
        {
            //Get valid zone
            ZoneDto zone = GetZone();

            zone.Name = null;
            zone.Description = "Test Edit";

            _controller.Update(zone);
        }

        /// <summary>
        /// Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_EmptyName()
        {
            //Get valid zone
            ZoneDto zone = GetZone();

            zone.Name = string.Empty;
            zone.Description = "Test Edit";

            _controller.Update(zone);
        }

        /// <summary>
        /// No Description - should succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoDescription()
        {
            //Get valid zone
            ZoneDto zone = GetZone();

            zone.Name = "Test Edit";
            zone.Description = null;

            ZoneDto updatedZone = _controller.Update(zone);

            Assert.IsNotNull(updatedZone);
            Assert.IsNotNull(updatedZone.Id);
            Assert.AreEqual(zone.Id, updatedZone.Id);
            Assert.AreEqual(zone.Name, updatedZone.Name);
            Assert.AreEqual(zone.Description, updatedZone.Description);
        }

        /// <summary>
        /// No Image
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoImage()
        {
            //Get valid zone
            ZoneDto zone = GetZone();

            zone.Name = "Test Edit";
            zone.Description = "Test Edit";
   
            ZoneDto updatedZone = _controller.Update(zone);

            Assert.IsNotNull(updatedZone);
            Assert.IsNotNull(updatedZone.Id);
            Assert.AreEqual(zone.Id, updatedZone.Id);
            Assert.AreEqual(zone.Name, updatedZone.Name);
            Assert.AreEqual(zone.Description, updatedZone.Description);
        }

        /// <summary>
        /// Invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Zone.")]
        public void TestUpdate_InvalidId()
        {
            //Get valid zone
            ZoneDto zone = GetZone();

            ZoneDto updatedZone = new ZoneDto()
            {
                Id = 0,
                Name = zone.Name,
                Description = zone.Description
            };

            _controller.Update(updatedZone);
        }
        #endregion

        #region GetById
        /// <summary>
        /// Test a valid Id GetById
        /// </summary>
        [TestMethod]
        public void TestGetById_ValidId()
        {
            //Get a valid zone 
            ZoneDto validZone = GetZone();

            //Try to get this zone
            ZoneDto zoneResult = _controller.GetById(validZone.Id);

            Assert.IsNotNull(zoneResult);
            Assert.IsNotNull(zoneResult.Id);
            Assert.AreEqual(validZone.Id, zoneResult.Id);
        }

        /// <summary>
        /// Test an invalid Id GetById
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Zone.")]
        public void TestGetById_InvalidId()
        {
            _controller.GetById(0);
        }
        #endregion

        #region GetFiltered
        /// <summary>
        /// All filters - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestGetFiltered_AllFilters()
        {
            //Create Zone for getfiltered validation
            ZoneDto validZone = CreateTestZone();

            var results = _controller.GetFiltered(new ApiFilter () { isDeleted = false, numPerPage = 100, pageNumber = 0 });

            Assert.IsNotNull(results);
            Assert.IsTrue(!results.Any(m => m.IsDeleted));
            Assert.IsTrue(results.Count <= 100);
            Assert.IsTrue(results.Count == results.Distinct().Count());
        }

        /// <summary>
        /// isDeleted true
        /// </summary>
        [TestMethod]
        public void TestGetFiltered_IsDeleted()
        {
            //Create a zone to test on
            ZoneDto validZone = CreateTestZone();

            //delete for test
            _controller.Delete(validZone.Id);

            var results = _controller.GetFiltered(new ApiFilter() { isDeleted = true, numPerPage = 100, pageNumber = 0 });

            Assert.IsNotNull(results);
            Assert.IsTrue(!results.Any(m => !m.IsDeleted));
            Assert.IsTrue(results.Count <= 100);
            Assert.IsTrue(results.Count == results.Distinct().Count());
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete test with valid Id
        /// </summary>
        [TestMethod]
        public void TestDelete_ValidId()
        {
            //Get valid zone
            ZoneDto validZone = GetZone();

            //Delete zone
            bool result = _controller.Delete(validZone.Id);

            Assert.IsTrue(result);

            //Get zone for comparison
            ZoneDto zoneResult = _controller.GetById(validZone.Id);

            Assert.IsNotNull(zoneResult);
            Assert.IsNotNull(zoneResult.Id);
            Assert.AreEqual(validZone.Id, zoneResult.Id);
            Assert.IsTrue(zoneResult.IsDeleted);
        }

        /// <summary>
        /// Delete test with invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Zone.")]
        public void TestDelete_InvalidId()
        {
            _controller.Delete(0);
        }
        #endregion
        #endregion

        #region Helpers
        ZoneDto GetZone()
        {
            ApiFilter filter = new ApiFilter() { isDeleted = false, numPerPage = 1, pageNumber = 0 };
            var zones = _controller.GetFiltered(filter);

            if (zones != null && zones.Any())
            {
                return zones.First();
            }
            else
            {
                //Create a new zone for testing
                return CreateTestZone();
            }
        }

        ZoneDto CreateTestZone()
        {
            ZoneDto zone = new ZoneDto()
            {
                Name = "Test",
                Description = "Test"
            };

            zone = _controller.Create(zone);

            return zone;
        }
        #endregion
    }
}
