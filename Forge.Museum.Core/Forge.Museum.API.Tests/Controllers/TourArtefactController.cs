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
        private API.Controllers.TourController _tourController;
        private API.Controllers.ArtefactController _artefactController;

        [TestInitialize]
        public void SetupTest()
        {
            _controller = new API.Controllers.TourArtefactController(true);
            _tourController = new API.Controllers.TourController(true);
            _artefactController = new API.Controllers.ArtefactController(true);
            ArtefactDto artefact = CreateTestArtefact();
            ArtefactSimpleDto artefactSimple = new ArtefactSimpleDto() { Id = artefact.Id };
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
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = new TourSimpleDto { Id = CreateTestTour().Id, Name = CreateTestTour().Name },
                Artefact = new ArtefactSimpleDto { Id = CreateTestArtefact().Id, Name = CreateTestArtefact().Name },
                Order = 5
            };

            //Make test request
            TourArtefactDto tourArtefactResult = _controller.Create(tourArtefact);

            //Assert Values
            Assert.IsNotNull(tourArtefactResult);
            Assert.IsNotNull(tourArtefactResult.Id);
            Assert.IsTrue(tourArtefactResult.Id != 0);
            Assert.IsNotNull(tourArtefactResult.Tour);
            Assert.IsNotNull(tourArtefactResult.Artefact);

            Assert.AreEqual(tourArtefact.Tour.Id, tourArtefactResult.Tour.Id);
            Assert.AreEqual(tourArtefact.Tour.Name, tourArtefactResult.Tour.Name);
            Assert.AreEqual(tourArtefact.Artefact.Id, tourArtefactResult.Artefact.Id);
            Assert.AreEqual(tourArtefact.Artefact.Name, tourArtefactResult.Artefact.Name);

            Assert.AreEqual(tourArtefact.Order, tourArtefactResult.Order);
            Assert.IsNotNull(tourArtefactResult.CreatedDate);
            Assert.IsNotNull(tourArtefactResult.ModifiedDate);
        }

        /// <summary>
        /// Create - No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Order is a required field.")]
        public void TestCreate_InvalidOrder()
        {
            //Set up dto
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = new TourSimpleDto { Id = CreateTestTour().Id, Name = CreateTestTour().Name },
                Artefact = new ArtefactSimpleDto { Id = CreateTestArtefact().Id, Name = CreateTestArtefact().Name },
                Order = -16,
                };

            _controller.Create(tourArtefact);
        }

        /// <summary>
        /// Create - No Tour
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Tour is a required field.")]
        public void TestCreate_NoTour()
        {
            //Set up dto
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = null,
                Artefact = new ArtefactSimpleDto { Id = CreateTestArtefact().Id, Name = CreateTestArtefact().Name },
                Order = 7,
            };
            _controller.Create(tourArtefact);
        }

        /// <summary>
        /// Create - No Artefact
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Artefact is a required field.")]
        public void TestCreate_NoArtefact()
        {
            //Set up dto
            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Tour = new TourSimpleDto { Id = CreateTestTour().Id, Name = CreateTestTour().Name },
                Artefact = null,
                Order = -16,
            };

            _controller.Create(tourArtefact);
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
            tourArtefact.Id = 1;
            tourArtefact.Tour = new TourSimpleDto { Id = CreateTestTour().Id, Name = CreateTestTour().Name };
            tourArtefact.Artefact = new ArtefactSimpleDto { Id = CreateTestArtefact().Id, Name = CreateTestArtefact().Name };
            tourArtefact.Order = 7;

            _controller.Update(tourArtefact);
            TourArtefactDto updatedTourArtefact = _controller.Update(tourArtefact);

            Assert.IsNotNull(updatedTourArtefact);
            Assert.IsNotNull(updatedTourArtefact.Id);
            Assert.IsNotNull(updatedTourArtefact.Tour.Id);
            Assert.IsNotNull(updatedTourArtefact.Artefact.Id);


            Assert.AreEqual(tourArtefact.Artefact.Name, updatedTourArtefact.Artefact.Name);
            Assert.AreEqual(tourArtefact.Order, updatedTourArtefact.Order);
        }

        /// <summary>
        /// No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Order is a required field.")]
        public void TestUpdate_InvalidOrder()
        {
            //Get valid tourArtefact
            TourArtefactDto tourArtefact = GetTourArtefact();

            tourArtefact.Id = 1;
            tourArtefact.Tour = new TourSimpleDto { Id = CreateTestTour().Id, Name = CreateTestTour().Name };
            tourArtefact.Artefact = new ArtefactSimpleDto { Id = CreateTestArtefact().Id, Name = CreateTestArtefact().Name };
            tourArtefact.Order = -5;
            _controller.Update(tourArtefact);
        }

        /// <summary>
        /// No Tour - should fail 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Tour is a required field.")]
        public void TestUpdate_NoTour()
        {
            //Get valid tourArtefact
            TourArtefactDto tourArtefact = GetTourArtefact();

            tourArtefact.Id = 1;
            tourArtefact.Tour = null;
            tourArtefact.Artefact = new ArtefactSimpleDto { Id = CreateTestArtefact().Id, Name = CreateTestArtefact().Name };
            tourArtefact.Order = 5;

            _controller.Update(tourArtefact);
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
                Tour = new TourSimpleDto { Id = CreateTestTour().Id, Name = CreateTestTour().Name },
                Artefact = new ArtefactSimpleDto { Id = CreateTestArtefact().Id, Name = CreateTestArtefact().Name },
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
            TourSimpleDto newTour = new TourSimpleDto { Id = CreateTestTour().Id, Name = CreateTestTour().Name };
            ArtefactSimpleDto newArtefact = new ArtefactSimpleDto { Id = CreateTestArtefact().Id, Name = CreateTestArtefact().Name };


            TourArtefactDto tourArtefact = new TourArtefactDto()
            {
                Id = 1,
                Tour = newTour,
                Artefact = newArtefact,
                Order = 5
            };

            tourArtefact = _controller.Create(tourArtefact);

            return tourArtefact;
        }

        TourDto CreateTestTour()
        {
            TourDto tour = new TourDto()
            {
                Id = 1,
                Description = "Test",
                AgeGroup = AgeGroup.Adult,
                Name = "Test"
            };
            tour = _tourController.Create(tour);

            return tour;
        }
      
        ArtefactDto CreateTestArtefact()
        {
            ArtefactDto artefact = new ArtefactDto()
            {
                Name = "Test",
                Description = "Test",
                Measurement_Height = 1,
                Measurement_Length = 2,
                Measurement_Width = 3,
                AcquisitionDate = DateTime.Now,
                Image = testImage
            };
            artefact = _artefactController.Create(artefact);

            return artefact;
        }
        #endregion
    }
}

