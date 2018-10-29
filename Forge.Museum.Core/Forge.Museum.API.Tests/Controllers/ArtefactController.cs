using Forge.Museum.API.Controllers;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
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
    public class ArtefactController : BaseTestClass
    {
        private API.Controllers.ArtefactController _controller;

        [TestInitialize]
        public void SetupTest()
        {
            _controller = new API.Controllers.ArtefactController(true);
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
            ArtefactDto artefact = new ArtefactDto()
            {
                Name = "Test",
                Description = "Test",
                Measurement_Width = 1,
                Measurement_Height = 2,
                Measurement_Length = 3,
                AcquisitionDate = DateTime.Now,
                Image = testImage
            };

            //Make test request
            ArtefactDto artefactResult = _controller.Create(artefact);

            //Assert Values
            Assert.IsNotNull(artefactResult);
            Assert.IsNotNull(artefactResult.Id);
            Assert.IsTrue(artefactResult.Id != 0);
            Assert.AreEqual(artefact.Name, artefactResult.Name);
            Assert.AreEqual(artefact.Description, artefactResult.Description);
            Assert.IsNotNull(artefactResult.CreatedDate);
            Assert.IsNotNull(artefactResult.ModifiedDate);
        }

        /// <summary>
        /// Create - No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_NoName()
        {
            //Set up dto
            ArtefactDto artefact = new ArtefactDto()
            {
                Name = null,
                Description = "Test",
                Measurement_Width = 1,
                Measurement_Height = 2,
                Measurement_Length = 3,
                AcquisitionDate = DateTime.Now,
                Image = testImage
            };

            _controller.Create(artefact);
        }

        /// <summary>
        /// Create - Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_EmptyName()
        {
            //Set up dto
            ArtefactDto artefact = new ArtefactDto()
            {
                Name = string.Empty,
                Description = "Test",
                Measurement_Width = 1,
                Measurement_Height = 2,
                Measurement_Length = 3,
                AcquisitionDate = DateTime.Now,
                Image = testImage
            };

            _controller.Create(artefact);
        }

        /// <summary>
        /// No Description - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoDescription()
        {
            //Set up dto
            ArtefactDto artefact = new ArtefactDto()
            {
                Name = "Test",
                Description = null,
                Measurement_Width = 1,
                Measurement_Height = 2,
                Measurement_Length = 3,
                AcquisitionDate = DateTime.Now,
                Image = testImage
            };

            //Make test request
            ArtefactDto artefactResult = _controller.Create(artefact);

            //Assert Values
            Assert.IsNotNull(artefactResult);
            Assert.IsNotNull(artefactResult.Id);
            Assert.IsTrue(artefactResult.Id != 0);
            Assert.AreEqual(artefact.Description, artefactResult.Description);
        }

        /// <summary>
        /// No Image - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoImage()
        {
            //Set up dto
            ArtefactDto artefact = new ArtefactDto()
            {
                Name = "Test",
                Description = "Test",
                Measurement_Width = 1,
                Measurement_Height = 2,
                Measurement_Length = 3,
                AcquisitionDate = DateTime.Now,
                Image = null
            };

            //Make test request
            ArtefactDto artefactResult = _controller.Create(artefact);

            //Assert Values
            Assert.IsNotNull(artefactResult);
            Assert.IsNotNull(artefactResult.Id);
            Assert.IsTrue(artefactResult.Id != 0);
        }
        #endregion

        #region Update
        /// <summary>
        /// Should Succeed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Validation.DbEntityValidationException), "")]
        public void TestUpdate_ShouldSucceed()
        {
            //Get valid artefact
            ArtefactDto artefact = GetArtefact();

            artefact.Name = "Test Edit";
            artefact.Description = "Test Edit";
            artefact.Image = testImage;

            ArtefactDto updatedArtefact = _controller.Update(artefact);

            Assert.IsNotNull(updatedArtefact);
            Assert.IsNotNull(updatedArtefact.Id);
            Assert.AreEqual(artefact.Id, updatedArtefact.Id);
            Assert.AreEqual(artefact.Name, updatedArtefact.Name);
            Assert.AreEqual(artefact.Description, updatedArtefact.Description);
        }

        /// <summary>
        /// No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_NoName()
        {
            //Get valid artefact
            ArtefactDto artefact = GetArtefact();

            artefact.Name = null;
            artefact.Description = "Test Edit";
            artefact.Image = testImage;

            _controller.Update(artefact);
        }

        /// <summary>
        /// Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_EmptyName()
        {
            //Get valid artefact
            ArtefactDto artefact = GetArtefact();

            artefact.Name = string.Empty;
            artefact.Description = "Test Edit";
            artefact.Image = testImage;

            _controller.Update(artefact);
        }

        /// <summary>
        /// No Description - should succeed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Validation.DbEntityValidationException), "")]
        public void TestUpdate_NoDescription()
        {
            //Get valid artefact
            ArtefactDto artefact = GetArtefact();

            artefact.Name = "Test Edit";
            artefact.Description = null;
            artefact.Image = testImage;

            ArtefactDto updatedArtefact = _controller.Update(artefact);

            Assert.IsNotNull(updatedArtefact);
            Assert.IsNotNull(updatedArtefact.Id);
            Assert.AreEqual(artefact.Id, updatedArtefact.Id);
            Assert.AreEqual(artefact.Name, updatedArtefact.Name);
            Assert.AreEqual(artefact.Description, updatedArtefact.Description);
        }

        /// <summary>
        /// No Image
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Validation.DbEntityValidationException), "")]
        public void TestUpdate_NoImage()
        {
            //Get valid artefact
            ArtefactDto artefact = GetArtefact();

            artefact.Name = "Test Edit";
            artefact.Description = "Test Edit";
            artefact.Image = null;
            artefact.Measurement_Width = 1;
            artefact.Measurement_Height = 2;
            artefact.Measurement_Length = 3 ;
            artefact.AcquisitionDate = DateTime.Now;

            ArtefactDto updatedArtefact = _controller.Update(artefact);

            Assert.IsNotNull(updatedArtefact);
            Assert.IsNotNull(updatedArtefact.Id);
            Assert.AreEqual(artefact.Id, updatedArtefact.Id);
            Assert.AreEqual(artefact.Name, updatedArtefact.Name);
            Assert.AreEqual(artefact.Description, updatedArtefact.Description);
        }

        /// <summary>
        /// Invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Artefact.")]
        public void TestUpdate_InvalidId()
        {
            //Get valid artefact
            ArtefactDto artefact = GetArtefact();

            ArtefactDto updatedArtefact = new ArtefactDto()
            {
                Id = 0,
                Name = artefact.Name,
                Description = artefact.Description,
                Measurement_Height = artefact.Measurement_Height,
                Measurement_Length = artefact.Measurement_Length,
                Measurement_Width = artefact.Measurement_Width,
                AcquisitionDate = artefact.AcquisitionDate,
                Image = artefact.Image
            };

            _controller.Update(updatedArtefact);
        }
        #endregion

        #region GetById
        /// <summary>
        /// Test a valid Id GetById
        /// </summary>
        [TestMethod]
        public void TestGetById_ValidId()
        {
            //Get a valid artefact 
            ArtefactDto validArtefact = GetArtefact();

            //Try to get this artefact
            ArtefactDto artefactResult = _controller.GetById(validArtefact.Id);

            Assert.IsNotNull(artefactResult);
            Assert.IsNotNull(artefactResult.Id);
            Assert.AreEqual(validArtefact.Id, artefactResult.Id);
        }

        /// <summary>
        /// Test an invalid Id GetById
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Artefact.")]
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
            //Create Artefact for getfiltered validation
            ArtefactDto validArtefact = CreateTestArtefact();

            var results = _controller.GetFiltered(new ArtefactFilter() { isDeleted = false, numPerPage = 100, pageNumber = 0 });

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
            //Create a artefact to test on
            ArtefactDto validArtefact = CreateTestArtefact();

            //delete for test
            _controller.Delete(validArtefact.Id);

            var results = _controller.GetFiltered(new ArtefactFilter() { isDeleted = true, numPerPage = 100, pageNumber = 0 });

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
        [ExpectedException(typeof(System.Data.Entity.Validation.DbEntityValidationException), "")]
        public void TestDelete_ValidId()
        {
            //Get valid artefact
            ArtefactDto validArtefact = GetArtefact();

            //Delete artefact
            bool result = _controller.Delete(validArtefact.Id);

            Assert.IsTrue(result);

            //Get artefact for comparison
            ArtefactDto artefactResult = _controller.GetById(validArtefact.Id);

            Assert.IsNotNull(artefactResult);
            Assert.IsNotNull(artefactResult.Id);
            Assert.AreEqual(validArtefact.Id, artefactResult.Id);
            Assert.IsTrue(artefactResult.IsDeleted);
        }

        /// <summary>
        /// Delete test with invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find Artefact.")]
        public void TestDelete_InvalidId()
        {
            _controller.Delete(0);
        }
        #endregion
        #endregion

        #region Helpers
        ArtefactDto GetArtefact()
        {
            ArtefactFilter filter = new ArtefactFilter() { isDeleted = false, numPerPage = 1, pageNumber = 0 };
            var artefacts = _controller.GetFiltered(filter);

            if (artefacts != null && artefacts.Any())
            {
                return artefacts.First();
            }
            else
            {
                //Create a new artefact for testing
                return CreateTestArtefact();
            }
        }

        ArtefactDto CreateTestArtefact()
        {
            ArtefactDto artefact = new ArtefactDto()
            {
                Id = 9,
                Name = "Test",
                Description = "Test",
                Measurement_Height = 1,
                Measurement_Length = 2,
                Measurement_Width = 3,
                AcquisitionDate = DateTime.Now,
                Image = testImage,
                UniqueCode = "0009"
            };

            artefact = _controller.Create(artefact);

            return artefact;
        }
        #endregion
    }
}

