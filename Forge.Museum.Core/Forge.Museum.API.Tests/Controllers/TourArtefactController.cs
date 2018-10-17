using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;
using Forge.Museum.Interfaces.Enumerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forge.Museum.API.Controllers;
using System.Web.Http;

namespace Forge.Museum.API.Tests.Controllers
{
    [TestClass]
    public class TourArtefactController : BaseTestClass
    {
        private API.Controllers.TourArtefactController _controller;

        [TestInitialize]
        public void SetupTest()
        {
            _controller = new API.Controllers.TourArtefactController(true);
        }
        #region Methods
        #region Create
        /// <summary>
        /// Create - Should Succeed
        /// </summary>
        /// 
        [TestMethod]
        public void TestCreate_ShouldSucceed()
        {
            //Set up dto
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = CreateTestTour(),
                Artefact = CreateTestArtefact(),
                Order = 5
            };

            //Make test request
            TourArtefactDto tourArtefactResult = _controller.Create(tourArtefact);

            //Assert Values
            Assert.IsNotNull(tourArtefactResult);
            Assert.IsNotNull(tourArtefactResult.Id);
            Assert.IsTrue(tourArtefactResult.Id != 0);
            Assert.AreEqual(tourArtefact.Tour, tourArtefactResult.Tour);
            Assert.AreEqual(tourArtefact.Artefact, tourArtefactResult.Artefact);
            Assert.AreEqual(tourArtefact.Order, tourArtefactResult.Order);
            Assert.IsNotNull(tourArtefactResult.CreatedDate);
            Assert.IsNotNull(tourArtefactResult.ModifiedDate);
        }

        /// <summary>
        /// Create - No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_NoName()
        {
            //Set up dto
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = CreateTestTour(),
                Artefact = CreateTestArtefact(),
                Order = 5
            };

            _controller.Create(tourArtefact);
        }

        /// <summary>
        /// Create - Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_EmptyName()
        {
            //Set up dto
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = CreateTestTour(),
                Artefact = CreateTestArtefact(),
                Order = 5
            };

            _controller.Create(tourArtefact);
        }

        /// <summary>
        /// No Description - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoDescription()
        {
            //Set up dto
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = CreateTestTour(),
                Artefact = CreateTestArtefact(),
                Order = 5
            };

            //Make test request
            TourArtefactDto tourArtefactResult = _controller.Create(tourArtefact);

            //Assert Values
            Assert.IsNotNull(tourArtefactResult);
            Assert.IsNotNull(tourArtefactResult.Id);
            Assert.IsTrue(tourArtefactResult.Id != 0);
            Assert.AreEqual(tourArtefact.Order, tourArtefactResult.Order);
            Assert.AreEqual(tourArtefact.Tour, tourArtefactResult.Tour);
            Assert.AreEqual(tourArtefact.Artefact, tourArtefactResult.Artefact);

        }

        /// <summary>
        /// No Image - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoImage()
        {
            //Set up dto
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = CreateTestTour(),
                Artefact = CreateTestArtefact(),
                Order = 5
            };

            //Make test request
            TourArtefactDto tourArtefactResult = _controller.Create(tourArtefact);

            //Assert Values
            Assert.IsNotNull(tourArtefactResult);
            Assert.IsNotNull(tourArtefactResult.Id);
            Assert.IsTrue(tourArtefactResult.Id != 0);
        }
        #endregion

        #region Update
        /// <summary>
        /// Should Succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_ShouldSucceed()
        {
            //Get valid tourArtefact
            TourArtefactDto tourArtefact = GetTourArtefact();

            tourArtefact.Tour = CreateTestTour();
            tourArtefact.Artefact = CreateTestArtefact();
            tourArtefact.Order = 5;


            TourArtefactDto updatedTourArtefact = _controller.Update(tourArtefact);

            Assert.IsNotNull(updatedTourArtefact);
            Assert.IsNotNull(updatedTourArtefact.Id);
            Assert.AreEqual(tourArtefact.Id, updatedTourArtefact.Id);
            Assert.AreEqual(tourArtefact.Tour, updatedTourArtefact.Tour);
            Assert.AreEqual(tourArtefact.Artefact, updatedTourArtefact.Artefact);
            Assert.AreEqual(tourArtefact.Order, updatedTourArtefact.Order);
        }

        /// <summary>
        /// No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_NoName()
        {
            //Get valid tourArtefact
            TourArtefactDto tourArtefact = GetTourArtefact();

            tourArtefact.Id = 1;
            tourArtefact.Tour = null;
            tourArtefact.Artefact = CreateTestArtefact();
            tourArtefact.Order = 5;

            _controller.Update(tourArtefact);
        }

        /// <summary>
        /// Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_NoTour()
        {
            //Get valid tourArtefact
            TourArtefactDto tourArtefact = GetTourArtefact();

            tourArtefact.Id = 1;
            tourArtefact.Tour = null;
            tourArtefact.Artefact = CreateTestArtefact();
            tourArtefact.Order = 5;

            _controller.Update(tourArtefact);
        }

        /// <summary>
        /// No Description - should succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoDescription()
        {
            //Get valid tourArtefact
            TourArtefactDto tourArtefact = GetTourArtefact();

            tourArtefact.Id = 1;
            tourArtefact.Tour = CreateTestTour();
            tourArtefact.Artefact = CreateTestArtefact();
            tourArtefact.Order = 5;

            TourArtefactDto updatedTourArtefact = _controller.Update(tourArtefact);

            Assert.IsNotNull(updatedTourArtefact);
            Assert.IsNotNull(updatedTourArtefact.Id);
            Assert.AreEqual(tourArtefact.Id, updatedTourArtefact.Id);
            Assert.AreEqual(tourArtefact.Tour, updatedTourArtefact.Tour);
            Assert.AreEqual(tourArtefact.Artefact, updatedTourArtefact.Artefact);
            Assert.AreEqual(tourArtefact.Order, updatedTourArtefact.Order);
        }

        /// <summary>
        /// No Image
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoImage()
        {
            //Get valid tourArtefact
            TourArtefactDto tourArtefact = GetTourArtefact();

            tourArtefact.Id = 1;
            tourArtefact.Tour = CreateTestTour();
            tourArtefact.Artefact = CreateTestArtefact();
            tourArtefact.Order = 5;

            TourArtefactDto updatedTourArtefact = _controller.Update(tourArtefact);

            Assert.IsNotNull(updatedTourArtefact);
            Assert.IsNotNull(updatedTourArtefact.Id);
            Assert.AreEqual(tourArtefact.Id, updatedTourArtefact.Id);
            Assert.AreEqual(tourArtefact.Tour, updatedTourArtefact.Tour);
            Assert.AreEqual(tourArtefact.Artefact, updatedTourArtefact.Artefact);
            Assert.AreEqual(tourArtefact.Order, updatedTourArtefact.Order);
        }

        /// <summary>
        /// Invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find TourArtefact.")]
        public void TestUpdate_InvalidId()
        {
            //Get valid tourArtefact
            TourArtefactDto tourArtefact = GetTourArtefact();

            TourArtefactDto updatedTourArtefact = new TourArtefactDto()
            {
                Id = 0,
                Tour = CreateTestTour(),
                Artefact = CreateTestArtefact(),
                Order = 5
            };

            _controller.Update(updatedTourArtefact);
        }
        #endregion

        #region GetById
        /// <summary>
        /// Test a valid Id GetById
        /// </summary>
        [TestMethod]
        public void TestGetById_ValidId()
        {
            //Get a valid tourArtefact 
            TourArtefactDto validtourArtefact = GetTourArtefact();

            //Try to get this tourArtefact
            TourArtefactDto tourArtefactResult = _controller.GetById(validtourArtefact.Id);

            Assert.IsNotNull(tourArtefactResult);
            Assert.IsNotNull(tourArtefactResult.Id);
            Assert.AreEqual(validtourArtefact.Id, tourArtefactResult.Id);
        }

        /// <summary>
        /// Test an invalid Id GetById
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find TourArtefact.")]
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
            //Create TourArtefact for getfiltered validation
            TourArtefactDto validtourArtefact = CreateTestTourArtefact();

            var results = _controller.GetFiltered(new TourArtefactFilter() { isDeleted = false, numPerPage = 100, pageNumber = 0 });

            Assert.IsNotNull(results);
            Assert.IsTrue(!results.Any(m => m.IsDeleted));
            Assert.IsTrue(results.Count <= 100);
            Assert.IsTrue(results.Any(m => m.Id == validtourArtefact.Id));
            Assert.IsTrue(results.Count == results.Distinct().Count());
        }

        /// <summary>
        /// isDeleted true
        /// </summary>
        [TestMethod]
        public void TestGetFiltered_IsDeleted()
        {
            //Create a tourArtefact to test on
            TourArtefactDto validtourArtefact = CreateTestTourArtefact();

            //delete for test
            _controller.Delete(validtourArtefact.Id);

            var results = _controller.GetFiltered(new TourArtefactFilter() { isDeleted = true, numPerPage = 100, pageNumber = 0 });

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
            //Get valid tourArtefact
            TourArtefactDto validtourArtefact = GetTourArtefact();

            //Delete tourArtefact
            bool result = _controller.Delete(validtourArtefact.Id);

            Assert.IsTrue(result);

            //Get tourArtefact for comparison
            TourArtefactDto tourArtefactResult = _controller.GetById(validtourArtefact.Id);

            Assert.IsNotNull(tourArtefactResult);
            Assert.IsNotNull(tourArtefactResult.Id);
            Assert.AreEqual(validtourArtefact.Id, tourArtefactResult.Id);
            Assert.IsTrue(tourArtefactResult.IsDeleted);
        }

        /// <summary>
        /// Delete test with invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find TourArtefact.")]
        public void TestDelete_InvalidId()
        {
            _controller.Delete(0);
        }
        #endregion
        #endregion

        #region Helpers
        TourArtefactDto GetTourArtefact()
        {
            TourArtefactFilter filter = new TourArtefactFilter() { isDeleted = false, numPerPage = 1, pageNumber = 0 };
            var tourArtefacts = _controller.GetFiltered(filter);

            if (tourArtefacts != null && tourArtefacts.Any())
            {
                return tourArtefacts.First();
            }
            else
            {
                //Create a new tourArtefact for testing
                return CreateTestTourArtefact();
            }
        }

        TourArtefactDto CreateTestTourArtefact()
        {
            TourSimpleDto newTour = CreateTestTour();
            ArtefactSimpleDto newArtefact = CreateTestArtefact();

            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Id = 7,
                Tour = newTour,
                Artefact = newArtefact,
                Order = 5
            };

            tourArtefact = _controller.Create(tourArtefact);

            return tourArtefact;
        }

        TourSimpleDto CreateTestTour()
        {
            TourSimpleDto tour = new TourSimpleDto()
            {
                Id = 0001,
                Name = "Test"
            };
            return tour;
        }

        ArtefactSimpleDto CreateTestArtefact()
        {
            ArtefactSimpleDto artefact = new ArtefactSimpleDto()
            {
                Id = 0001,
                Name = "Test",
              
            };
            return artefact;
        }
        #endregion
    }
}

