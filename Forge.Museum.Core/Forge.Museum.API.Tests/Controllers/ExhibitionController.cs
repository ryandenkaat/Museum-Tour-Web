
using Forge.Museum.API.Controllers;
using Forge.Museum.Interfaces.DataTransferObjects.Exhibition;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Forge.Museum.API.Tests.Controllers
{
	[TestClass]
	public class ExhibitionController : BaseTestClass
	{
		private API.Controllers.ExhibitionController _controller;

		[TestInitialize]
		public void SetupTest()
		{
			_controller = new API.Controllers.ExhibitionController(true);
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
            ExhibitionDto exhibition = new ExhibitionDto()
            {
                Name = "Test",
                Description = "Test",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Parse("01/01/2099"),
                Price_Adult = 10,
                Price_Child = 5,
                Price_Concession = 7,
                Organiser = "Test",
                Image = testImage
            };

            //Make test request
            ExhibitionDto exhibitionResult = _controller.Create(exhibition);

            //Assert Values
            Assert.IsNotNull(exhibitionResult);
            Assert.IsNotNull(exhibitionResult.Id);
            Assert.IsTrue(exhibitionResult.Id != 0);
            Assert.AreEqual(exhibition.Name, exhibitionResult.Name);
            Assert.AreEqual(exhibition.Description, exhibitionResult.Description);
            Assert.AreEqual(exhibition.StartDate, exhibitionResult.StartDate);
            Assert.AreEqual(exhibition.FinishDate, exhibitionResult.FinishDate);
            Assert.AreEqual(exhibition.Price_Adult, exhibitionResult.Price_Adult);
            Assert.AreEqual(exhibition.Price_Child, exhibitionResult.Price_Child);
            Assert.AreEqual(exhibition.Price_Concession, exhibitionResult.Price_Concession);
            Assert.AreEqual(exhibition.Organiser, exhibitionResult.Organiser);
            Assert.IsNotNull(exhibitionResult.CreatedDate);
            Assert.IsNotNull(exhibitionResult.ModifiedDate);
        }

        /// <summary>
        /// Create - No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_NoName()
        {
            //Set up dto
            ExhibitionDto exhibition = new ExhibitionDto()
            {
                Name = null,
                Description = "Test",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Parse("01/01/2099"),
                Price_Adult = 10,
                Price_Child = 5,
                Price_Concession = 7,
                Organiser = "Test",
                Image = testImage
            };

            _controller.Create(exhibition);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException), "Start Date is a required field.")]
        public void TestCreate_NoStartDate()
        {
            //Set up dto
            ExhibitionDto exhibition = new ExhibitionDto()
            {
                Name = "Test",
                Description = "Test",
                FinishDate = DateTime.Parse("01/01/2099"),
                Price_Adult = 10,
                Price_Child = 5,
                Price_Concession = 7,
                Organiser = "Test",
                Image = testImage
            };
            _controller.Create(exhibition);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException), "Finish Date is a required field.")]
        public void TestCreate_NoFinishDate()
        {
            //Set up dto
            ExhibitionDto exhibition = new ExhibitionDto()
            {
                Name = "Test",
                Description = "Test",
                StartDate = DateTime.Now,
                Price_Adult = 10,
                Price_Child = 5,
                Price_Concession = 7,
                Organiser = "Test",
                Image = testImage
            };
            _controller.Create(exhibition);
        }

        /// <summary>
        /// Create - Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_EmptyName()
        {
            //Set up dto
            ExhibitionDto exhibition = new ExhibitionDto()
            {
                Name = string.Empty,
                Description = "Test",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Parse("01/01/2099"),
                Price_Adult = 10,
                Price_Child = 5,
                Price_Concession = 7,
                Organiser = "Test",
                Image = testImage
            };

            _controller.Create(exhibition);
        }

        /// <summary>
        /// No Description - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoDescription()
        {
            //Set up dto
            ExhibitionDto exhibition = new ExhibitionDto()
            {
                Name = "Test",
                Description = null,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Parse("01/01/2099"),
                Price_Adult = 10,
                Price_Child = 5,
                Price_Concession = 7,
                Organiser = "Test",
                Image = testImage
            };

            //Make test request
            ExhibitionDto exhibitionResult = _controller.Create(exhibition);

            //Assert Values
            Assert.IsNotNull(exhibitionResult);
            Assert.IsNotNull(exhibitionResult.Id);
            Assert.IsTrue(exhibitionResult.Id != 0);
            Assert.AreEqual(exhibition.Description, exhibitionResult.Description);
        }

        /// <summary>
        /// No Image - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoImage()
        {
            //Set up dto
            ExhibitionDto exhibition = new ExhibitionDto()
            {
                Name = "Test",
                Description = "Test",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Parse("01/01/2099"),
                Price_Adult = 10,
                Price_Child = 5,
                Price_Concession = 7,
                Organiser = "Test",
                Image = null
            };

            //Make test request
            ExhibitionDto exhibitionResult = _controller.Create(exhibition);

            //Assert Values
            Assert.IsNotNull(exhibitionResult);
            Assert.IsNotNull(exhibitionResult.Id);
            Assert.IsTrue(exhibitionResult.Id != 0);
        }
        #endregion

        #region Update
        /// <summary>
        /// Should Succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_ShouldSucceed()
        {
            //Get valid exhibition
            ExhibitionDto exhibition = GetExhibition();

            exhibition.Name = "Test";
            exhibition.Description = "Test";
            exhibition.StartDate = DateTime.Now;
            exhibition.FinishDate = DateTime.Parse("01/01/2099");
            exhibition.Price_Adult = 10;
            exhibition.Price_Child = 5;
            exhibition.Price_Concession = 7;
            exhibition.Organiser = "Test";
            exhibition.Image = testImage;

            ExhibitionDto updatedExhibition = _controller.Update(exhibition);

            Assert.IsNotNull(updatedExhibition);
            Assert.IsNotNull(updatedExhibition.Id);
            Assert.AreEqual(exhibition.Id, updatedExhibition.Id);
            Assert.AreEqual(exhibition.Name, updatedExhibition.Name);
            Assert.AreEqual(exhibition.Description, updatedExhibition.Description);
            Assert.AreEqual(exhibition.StartDate, updatedExhibition.StartDate);
            Assert.AreEqual(exhibition.FinishDate, updatedExhibition.FinishDate);
            Assert.AreEqual(exhibition.Price_Adult, updatedExhibition.Price_Adult);
            Assert.AreEqual(exhibition.Price_Child, updatedExhibition.Price_Child);
            Assert.AreEqual(exhibition.Price_Concession, updatedExhibition.Price_Concession);
            Assert.AreEqual(exhibition.Organiser, updatedExhibition.Organiser);
        }

        /// <summary>
        /// No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_NoName()
        {
            //Get valid exhibition
            ExhibitionDto exhibition = GetExhibition();

            exhibition.Name = null;
            exhibition.Description = "Test";
            exhibition.StartDate = DateTime.Now;
            exhibition.FinishDate = DateTime.Parse("01/01/2099");
            exhibition.Price_Adult = 10;
            exhibition.Price_Child = 5;
            exhibition.Price_Concession = 7;
            exhibition.Organiser = "Test";
            exhibition.Image = testImage;

            _controller.Update(exhibition);
        }

        /// <summary>
        /// Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_EmptyName()
        {
            //Get valid exhibition
            ExhibitionDto exhibition = GetExhibition();

            exhibition.Name = string.Empty;
            exhibition.Description = "Test";
            exhibition.StartDate = DateTime.Now;
            exhibition.FinishDate = DateTime.Parse("01/01/2099");
            exhibition.Price_Adult = 10;
            exhibition.Price_Child = 5;
            exhibition.Price_Concession = 7;
            exhibition.Organiser = "Test";
            exhibition.Image = testImage;

            _controller.Update(exhibition);
        }

        /// <summary>
        /// No Description - should succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoDescription()
        {
            //Get valid exhibition
            ExhibitionDto exhibition = GetExhibition();

            exhibition.Name = "Test";
            exhibition.Description = null;
            exhibition.StartDate = DateTime.Now;
            exhibition.FinishDate = DateTime.Parse("01/01/2099");
            exhibition.Price_Adult = 10;
            exhibition.Price_Child = 5;
            exhibition.Price_Concession = 7;
            exhibition.Organiser = "Test";
            exhibition.Image = testImage;

            ExhibitionDto updatedExhibition = _controller.Update(exhibition);

            Assert.IsNotNull(updatedExhibition);
            Assert.IsNotNull(updatedExhibition.Id);
            Assert.AreEqual(exhibition.Id, updatedExhibition.Id);
            Assert.AreEqual(exhibition.Name, updatedExhibition.Name);
            Assert.AreEqual(exhibition.Description, updatedExhibition.Description);
            Assert.AreEqual(exhibition.StartDate, updatedExhibition.StartDate);
            Assert.AreEqual(exhibition.FinishDate, updatedExhibition.FinishDate);
            Assert.AreEqual(exhibition.Price_Adult, updatedExhibition.Price_Adult);
            Assert.AreEqual(exhibition.Price_Child, updatedExhibition.Price_Child);
            Assert.AreEqual(exhibition.Price_Concession, updatedExhibition.Price_Concession);
            Assert.AreEqual(exhibition.Organiser, updatedExhibition.Organiser);
        }

        /// <summary>
        /// No Image
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoImage()
        {
            //Get valid exhibition
            ExhibitionDto exhibition = GetExhibition();

            exhibition.Name = "Test";
            exhibition.Description = "Test";
            exhibition.StartDate = DateTime.Now;
            exhibition.FinishDate = DateTime.Parse("01/01/2099");
            exhibition.Price_Adult = 10;
            exhibition.Price_Child = 5;
            exhibition.Price_Concession = 7;
            exhibition.Organiser = "Test";
            exhibition.Image = null;


            ExhibitionDto updatedExhibition = _controller.Update(exhibition);

            Assert.IsNotNull(updatedExhibition);
            Assert.IsNotNull(updatedExhibition.Id);
            Assert.AreEqual(exhibition.Id, updatedExhibition.Id);
            Assert.AreEqual(exhibition.Name, updatedExhibition.Name);
            Assert.AreEqual(exhibition.Description, updatedExhibition.Description);
            Assert.AreEqual(exhibition.StartDate, updatedExhibition.StartDate);
            Assert.AreEqual(exhibition.FinishDate, updatedExhibition.FinishDate);
            Assert.AreEqual(exhibition.Price_Adult, updatedExhibition.Price_Adult);
            Assert.AreEqual(exhibition.Price_Child, updatedExhibition.Price_Child);
            Assert.AreEqual(exhibition.Price_Concession, updatedExhibition.Price_Concession);
            Assert.AreEqual(exhibition.Organiser, updatedExhibition.Organiser);
        }

        /// <summary>
        /// Invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Exhibition.")]
        public void TestUpdate_InvalidId()
        {
            //Get valid exhibition
            ExhibitionDto exhibition = GetExhibition();

            ExhibitionDto updatedExhibition = new ExhibitionDto()
            {
                Id = 0,
                Name = exhibition.Name,
                Description = exhibition.Description,

                Image = exhibition.Image
            };

            _controller.Update(updatedExhibition);
        }
        #endregion

        #region GetById
        /// <summary>
        /// Test a valid Id GetById
        /// </summary>
        [TestMethod]
        public void TestGetById_ValidId()
        {
            //Get a valid exhibition 
            ExhibitionDto validExhibition = GetExhibition();

            //Try to get this exhibition
            ExhibitionDto exhibitionResult = _controller.GetById(validExhibition.Id);

            Assert.IsNotNull(exhibitionResult);
            Assert.IsNotNull(exhibitionResult.Id);
            Assert.AreEqual(validExhibition.Id, exhibitionResult.Id);
        }

        /// <summary>
        /// Test an invalid Id GetById
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Exhibition.")]
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
            //Create Exhibition for getfiltered validation
            ExhibitionDto validExhibition = CreateTestExhibition();

            var results = _controller.GetFiltered(new ApiFilter() { isDeleted = false, numPerPage = 100, pageNumber = 0 });

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
            //Create a exhibition to test on
            ExhibitionDto validExhibition = CreateTestExhibition();

            //delete for test
            _controller.Delete(validExhibition.Id);

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
            //Get valid exhibition
            ExhibitionDto validExhibition = GetExhibition();

            //Delete exhibition
            bool result = _controller.Delete(validExhibition.Id);

            Assert.IsTrue(result);

            //Get exhibition for comparison
            ExhibitionDto exhibitionResult = _controller.GetById(validExhibition.Id);

            Assert.IsNotNull(exhibitionResult);
            Assert.IsNotNull(exhibitionResult.Id);
            Assert.AreEqual(validExhibition.Id, exhibitionResult.Id);
            Assert.IsTrue(exhibitionResult.IsDeleted);
        }

        /// <summary>
        /// Delete test with invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Exhibition.")]
        public void TestDelete_InvalidId()
        {
            _controller.Delete(0);
        }
        #endregion
        #endregion

        #region Helpers
        ExhibitionDto GetExhibition()
        {
            ApiFilter filter = new ApiFilter() { isDeleted = false, numPerPage = 1, pageNumber = 0 };
            var exhibitions = _controller.GetFiltered(filter);

            if (exhibitions != null && exhibitions.Any())
            {
                return exhibitions.First();
            }
            else
            {
                //Create a new exhibition for testing
                return CreateTestExhibition();
            }
        }

        ExhibitionDto CreateTestExhibition()
        {
            ExhibitionDto exhibition = new ExhibitionDto()
            {
                Name = "Test",
                Description = "Test",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Parse("01/01/2099"),
                Price_Adult = 10,
                Price_Child = 5,
                Price_Concession = 7,
                Organiser = "Test",
                Image = null
            };

            exhibition = _controller.Create(exhibition);

            return exhibition;
        }
        #endregion
    }
}
