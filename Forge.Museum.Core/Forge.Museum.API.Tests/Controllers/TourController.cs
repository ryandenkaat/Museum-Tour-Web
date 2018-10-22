using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Forge.Museum.API.Controllers;
using System.Web.Http;
using Forge.Museum.Interfaces.Enumerators;

namespace Forge.Museum.API.Tests.Controllers
{
	[TestClass]
	public class TourController : BaseTestClass
	{
		private API.Controllers.TourController _controller;

		[TestInitialize]
		public void SetupTest()
		{
			_controller = new API.Controllers.TourController(true);
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
            TourDto tour = new TourDto()
            {
                Name = "Test",
                Description = "Test",
                AgeGroup = AgeGroup.Adult
            };

            //Make test request
            TourDto tourResult = _controller.Create(tour);

            //Assert Values
            Assert.IsNotNull(tourResult);
            Assert.IsNotNull(tourResult.Id);
            Assert.IsTrue(tourResult.Id != 0);
            Assert.AreEqual(tour.Name, tourResult.Name);
            Assert.AreEqual(tour.Description, tourResult.Description);
            Assert.IsNotNull(tourResult.CreatedDate);
            Assert.IsNotNull(tourResult.ModifiedDate);
        }

        /// <summary>
        /// Create - No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_NoName()
        {
            //Set up dto
            TourDto tour = new TourDto()
            {
                Name = null,
                Description = "Test",
                AgeGroup = AgeGroup.Adult
            };

            _controller.Create(tour);
        }

        /// <summary>
        /// Create - Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_EmptyName()
        {
            //Set up dto
            TourDto tour = new TourDto()
            {
                Name = string.Empty,
                Description = "Test",
                AgeGroup = AgeGroup.Adult
            };

            _controller.Create(tour);
        }

        /// <summary>
        /// No Description - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoDescription()
        {
            //Set up dto
            TourDto tour = new TourDto()
            {
                Name = "Test",
                Description = null,
                AgeGroup = AgeGroup.Adult
            };

            //Make test request
            TourDto tourResult = _controller.Create(tour);

            //Assert Values
            Assert.IsNotNull(tourResult);
            Assert.IsNotNull(tourResult.Id);
            Assert.IsTrue(tourResult.Id != 0);
            Assert.AreEqual(tour.Description, tourResult.Description);
        }

        /// <summary>
        /// No Image - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoImage()
        {
            //Set up dto
            TourDto tour = new TourDto()
            {
                Name = "Test",
                Description = "Test",
                AgeGroup = AgeGroup.Adult
            };

            //Make test request
            TourDto tourResult = _controller.Create(tour);

            //Assert Values
            Assert.IsNotNull(tourResult);
            Assert.IsNotNull(tourResult.Id);
            Assert.IsTrue(tourResult.Id != 0);
        }
        #endregion

        #region Update
        /// <summary>
        /// Should Succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_ShouldSucceed()
        {
            //Get valid tour
            TourDto tour = GetTour();

            tour.Name = "Test Edit";
            tour.Description = "Test Edit";
            tour.AgeGroup = AgeGroup.Adult;

            TourDto updatedTour = _controller.Update(tour);

            Assert.IsNotNull(updatedTour);
            Assert.IsNotNull(updatedTour.Id);
            Assert.AreEqual(tour.Id, updatedTour.Id);
            Assert.AreEqual(tour.Name, updatedTour.Name);
            Assert.AreEqual(tour.Description, updatedTour.Description);
        }

        /// <summary>
        /// No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_NoName()
        {
            //Get valid tour
            TourDto tour = GetTour();

            tour.Name = null;
            tour.Description = "Test Edit";
            tour.AgeGroup = AgeGroup.Adult;

            _controller.Update(tour);
        }

        /// <summary>
        /// Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_EmptyName()
        {
            //Get valid tour
            TourDto tour = GetTour();

            tour.Name = string.Empty;
            tour.Description = "Test Edit";
            tour.AgeGroup = AgeGroup.Adult;

            _controller.Update(tour);
        }

        /// <summary>
        /// No Description - should succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoDescription()
        {
            //Get valid tour
            TourDto tour = GetTour();

            tour.Name = "Test Edit";
            tour.Description = null;
            tour.AgeGroup = AgeGroup.Adult;

            TourDto updatedTour = _controller.Update(tour);

            Assert.IsNotNull(updatedTour);
            Assert.IsNotNull(updatedTour.Id);
            Assert.AreEqual(tour.Id, updatedTour.Id);
            Assert.AreEqual(tour.Name, updatedTour.Name);
            Assert.AreEqual(tour.Description, updatedTour.Description);
        }

        /// <summary>
        /// No Image
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoImage()
        {
            //Get valid tour
            TourDto tour = GetTour();

            tour.Name = "Test Edit";
            tour.Description = "Test Edit";
            tour.AgeGroup = AgeGroup.Adult;

            TourDto updatedTour = _controller.Update(tour);

            Assert.IsNotNull(updatedTour);
            Assert.IsNotNull(updatedTour.Id);
            Assert.AreEqual(tour.Id, updatedTour.Id);
            Assert.AreEqual(tour.Name, updatedTour.Name);
            Assert.AreEqual(tour.Description, updatedTour.Description);
        }

        /// <summary>
        /// Invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Tour.")]
        public void TestUpdate_InvalidId()
        {
            //Get valid tour
            TourDto tour = GetTour();

            TourDto updatedTour = new TourDto()
            {
                Id = 0,
                Name = tour.Name,
                Description = tour.Description,
                AgeGroup = AgeGroup.Adult
            };

            _controller.Update(updatedTour);
        }
        #endregion

        #region GetById
        /// <summary>
        /// Test a valid Id GetById
        /// </summary>
        [TestMethod]
        public void TestGetById_ValidId()
        {
            //Get a valid tour 
            TourDto validTour = GetTour();

            //Try to get this tour
            TourDto tourResult = _controller.GetById(validTour.Id);

            Assert.IsNotNull(tourResult);
            Assert.IsNotNull(tourResult.Id);
            Assert.AreEqual(validTour.Id, tourResult.Id);
        }

        /// <summary>
        /// Test an invalid Id GetById
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Tour.")]
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
            //Create Tour for getfiltered validation
            TourDto validTour = CreateTestTour();

            var results = _controller.GetFiltered(new TourFilter() { isDeleted = false, numPerPage = 100, pageNumber = 0 });

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
            //Create a tour to test on
            TourDto validTour = CreateTestTour();

            //delete for test
            _controller.Delete(validTour.Id);

            var results = _controller.GetFiltered(new TourFilter() { isDeleted = true, numPerPage = 100, pageNumber = 0 });

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
            //Get valid tour
            TourDto validTour = GetTour();

            //Delete tour
            bool result = _controller.Delete(validTour.Id);

            Assert.IsTrue(result);

            //Get tour for comparison
            TourDto tourResult = _controller.GetById(validTour.Id);

            Assert.IsNotNull(tourResult);
            Assert.IsNotNull(tourResult.Id);
            Assert.AreEqual(validTour.Id, tourResult.Id);
            Assert.IsTrue(tourResult.IsDeleted);
        }

        /// <summary>
        /// Delete test with invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Tour.")]
        public void TestDelete_InvalidId()
        {
            _controller.Delete(0);
        }
        #endregion
        #endregion

        #region Helpers
        TourDto GetTour()
        {
            TourFilter filter = new TourFilter() { isDeleted = false, numPerPage = 1, pageNumber = 0 };
            var tours = _controller.GetFiltered(filter);

            if (tours != null && tours.Any())
            {
                return tours.First();
            }
            else
            {
                //Create a new tour for testing
                return CreateTestTour();
            }
        }

        TourDto CreateTestTour()
        {
            TourDto tour = new TourDto()
            {
                Name = "Test",
                Description = "Test",
                AgeGroup = AgeGroup.Adult
            };

            tour = _controller.Create(tour);

            return tour;
        }
        #endregion
    }
}
