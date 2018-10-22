using Forge.Museum.Interfaces.DataTransferObjects.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Http;
using Forge.Museum.Interfaces.DataTransferObjects.Common;

namespace Forge.Museum.API.Tests.Controllers
{
    [TestClass]
	public class StoreItemImageController : BaseTestClass
	{
		private API.Controllers.StoreItemImageController _controller;
        private API.Controllers.StoreItemController _storeItemController;

        [TestInitialize]
        public void SetupTest()
        {
            _controller = new API.Controllers.StoreItemImageController(true);
            _storeItemController = new API.Controllers.StoreItemController(true);
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
            StoreItemImageDto storeItemImage = new StoreItemImageDto()
            {
                Image = testImage,
                FileType = ".jpg",
                StoreItem = new StoreItemSimpleDto { Id = CreateTestStoreItem().Id, Name = CreateTestStoreItem().Name }
            };

            //Make test request
            StoreItemImageDto storeItemImageResult = _controller.Create(storeItemImage);

            //Assert Values
            Assert.IsNotNull(storeItemImageResult);
            Assert.IsNotNull(storeItemImageResult.Id);
            Assert.IsTrue(storeItemImageResult.Id != 0);
            Assert.IsNotNull(storeItemImageResult.StoreItem);


            Assert.AreEqual(storeItemImage.StoreItem.Id, storeItemImageResult.StoreItem.Id);
            Assert.AreEqual(storeItemImage.StoreItem.Name, storeItemImageResult.StoreItem.Name);

            Assert.AreEqual(storeItemImage.FileType, storeItemImageResult.FileType);
            Assert.IsNotNull(storeItemImageResult.CreatedDate);
            Assert.IsNotNull(storeItemImageResult.ModifiedDate);
        }

        /// <summary>
        /// No Image - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoImage()
        {
            //Set up dto
            StoreItemImageDto storeItem = new StoreItemImageDto()
            {
                Image = null,
                FileType = ".jpg",
                StoreItem = new StoreItemSimpleDto { Id = CreateTestStoreItem().Id, Name = CreateTestStoreItem().Name }
            };

            //Make test request
            StoreItemImageDto storeItemImageResult = _controller.Create(storeItem);

            //Assert Values
            Assert.IsNotNull(storeItemImageResult);
            Assert.IsNotNull(storeItemImageResult.Id);
            Assert.IsTrue(storeItemImageResult.Id != 0);
        }
        #endregion

        #region Update
        /// <summary>
        /// Should Succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_ShouldSucceed()
        {
            //Get valid storeItemImage
            StoreItemImageDto storeItemImage = GetStoreItemImage();
            storeItemImage.Id = 1;
            storeItemImage.Image = testImage;
            storeItemImage.FileType = ".jpg";
            storeItemImage.StoreItem = new StoreItemSimpleDto { Id = CreateTestStoreItem().Id, Name = CreateTestStoreItem().Name };

            _controller.Update(storeItemImage);
            StoreItemImageDto updatedStoreItemImage = _controller.Update(storeItemImage);

            Assert.IsNotNull(updatedStoreItemImage);
            Assert.IsNotNull(updatedStoreItemImage.Id);
            Assert.IsTrue(updatedStoreItemImage.Id != 0);
            Assert.IsNotNull(updatedStoreItemImage.StoreItem);


            Assert.AreEqual(storeItemImage.StoreItem.Id, updatedStoreItemImage.StoreItem.Id);
            Assert.AreEqual(storeItemImage.StoreItem.Name, updatedStoreItemImage.StoreItem.Name);

            Assert.AreEqual(storeItemImage.FileType, updatedStoreItemImage.FileType);
            Assert.IsNotNull(updatedStoreItemImage.CreatedDate);
            Assert.IsNotNull(updatedStoreItemImage.ModifiedDate);
        }

        /// <summary>
        /// No Image
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoImage()
        {
            //Get valid storeItemImage
            StoreItemImageDto storeItemImage = GetStoreItemImage();

            storeItemImage.FileType = "jpeg";
            storeItemImage.Image = null;
            storeItemImage.StoreItem = new StoreItemSimpleDto { Id = CreateTestStoreItem().Id, Name = CreateTestStoreItem().Name };


            StoreItemImageDto updatedStoreItemImage = _controller.Update(storeItemImage);

            Assert.IsNotNull(updatedStoreItemImage);
            Assert.IsNotNull(updatedStoreItemImage.Id);
            Assert.AreEqual(storeItemImage.Id, updatedStoreItemImage.Id);
            Assert.AreEqual(storeItemImage.StoreItem.Id, updatedStoreItemImage.StoreItem.Id);
            Assert.AreEqual(storeItemImage.StoreItem.Name, updatedStoreItemImage.StoreItem.Name);
            Assert.AreEqual(storeItemImage.FileType, updatedStoreItemImage.FileType);
        }

        /// <summary>
        /// No StoreItem - should fail 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "StoreItem is a required field.")]
        public void TestUpdate_NoStoreItem()
        {
            //Get valid storeItemImage
            StoreItemImageDto storeItemImage = GetStoreItemImage();

            storeItemImage.Id = 1;
            storeItemImage.Image = testImage;
            storeItemImage.FileType = ".jpg";
            storeItemImage.StoreItem = null;

            _controller.Update(storeItemImage);
        }
       

        /// <summary>
        /// Invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find StoreItemImage.")]
        public void TestUpdate_InvalidId()
        {
            //Get valid storeItemImage
            StoreItemImageDto storeItemImage = GetStoreItemImage();

            StoreItemImageDto updatedStoreItemImage = new StoreItemImageDto()
            {
                Id = 0,
                Image = testImage,
                FileType = ".jpg",
                StoreItem = new StoreItemSimpleDto { Id = CreateTestStoreItem().Id, Name = CreateTestStoreItem().Name }
            };

            _controller.Update(updatedStoreItemImage);
        }
        #endregion

        #region GetById
        /// <summary>
        /// Test a valid Id GetById
        /// </summary>
        [TestMethod]
        public void TestGetById_ValidId()
        {
            //Get a valid storeItemImage 
            StoreItemImageDto validStoreItemImage = GetStoreItemImage();

            //Try to get this storeItemImage
            StoreItemImageDto storeItemImageResult = _controller.GetById(validStoreItemImage.Id);

            Assert.IsNotNull(storeItemImageResult);
            Assert.IsNotNull(storeItemImageResult.Id);
            Assert.AreEqual(validStoreItemImage.Id, storeItemImageResult.Id);
        }

        /// <summary>
        /// Test an invalid Id GetById
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find StoreItemImage.")]
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
            //Create StoreItemImage for getfiltered validation
            StoreItemImageDto validStoreItemImage = CreateTestStoreItemImage();

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
            //Create a storeItemImage to test on
            StoreItemImageDto validStoreItemImage = CreateTestStoreItemImage();

            //delete for test
            _controller.Delete(validStoreItemImage.Id);

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
            //Get valid storeItemImage
            StoreItemImageDto validStoreItemImage = GetStoreItemImage();

            //Delete storeItemImage
            bool result = _controller.Delete(validStoreItemImage.Id);

            Assert.IsTrue(result);

            //Get storeItemImage for comparison
            StoreItemImageDto storeItemImageResult = _controller.GetById(validStoreItemImage.Id);

            Assert.IsNotNull(storeItemImageResult);
            Assert.IsNotNull(storeItemImageResult.Id);
            Assert.AreEqual(validStoreItemImage.Id, storeItemImageResult.Id);
            Assert.IsTrue(storeItemImageResult.IsDeleted);
        }

        /// <summary>
        /// Delete test with invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find StoreItemImage.")]
        public void TestDelete_InvalidId()
        {
            _controller.Delete(0);
        }
        #endregion
        #endregion

        #region Helpers
        StoreItemImageDto GetStoreItemImage()
        {
            ApiFilter filter = new ApiFilter() { isDeleted = false, numPerPage = 1, pageNumber = 0 };
            var storeItemImages = _controller.GetFiltered(filter);

            if (storeItemImages != null && storeItemImages.Any())
            {
                return storeItemImages.First();
            }
            else
            {
                //Create a new storeItemImage for testing
                return CreateTestStoreItemImage();
            }
        }

        StoreItemImageDto CreateTestStoreItemImage()
        {
            StoreItemSimpleDto newStoreItem = new StoreItemSimpleDto { Id = CreateTestStoreItem().Id, Name = CreateTestStoreItem().Name };
            StoreItemImageDto storeItemImage = new StoreItemImageDto()
            {
                Id = 1,
                Image = testImage,
                FileType = ".jpg",
                StoreItem = new StoreItemSimpleDto { Id = CreateTestStoreItem().Id, Name = CreateTestStoreItem().Name }

            };
            storeItemImage = _controller.Create(storeItemImage);
            return storeItemImage;
        }

        StoreItemDto CreateTestStoreItem()
        {
            StoreItemDto storeItem = new StoreItemDto()
            {
                Name = "Test",
                Description = "Test",
                Cost = 9.99,
                InStock = true,
            };
            storeItem = _storeItemController.Create(storeItem);
            return storeItem;
        }
        #endregion
    }
}
