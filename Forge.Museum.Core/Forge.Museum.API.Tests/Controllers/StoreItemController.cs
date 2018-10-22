using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forge.Museum.API.Controllers;
using Forge.Museum.Interfaces.DataTransferObjects.Store;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using System.Web.Http;

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

        #region Methods
        #region Create
        /// <summary>
        /// Create - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_ShouldSucceed()
        {
            //Set up dto
            StoreItemDto storeItem = new StoreItemDto()
            {
                Name = "Test",
                Description = "Test",
                Cost = 9.99,
                InStock = true
            };

            //Make test request
            StoreItemDto storeItemResult = _controller.Create(storeItem);

            //Assert Values
            Assert.IsNotNull(storeItemResult);
            Assert.IsNotNull(storeItemResult.Id);
            Assert.IsTrue(storeItemResult.Id != 0);
            Assert.AreEqual(storeItem.Name, storeItemResult.Name);
            Assert.AreEqual(storeItem.Description, storeItemResult.Description);
            Assert.AreEqual(storeItem.Cost, storeItemResult.Cost);
            Assert.AreEqual(storeItem.InStock, storeItemResult.InStock);
            Assert.IsNotNull(storeItemResult.CreatedDate);
            Assert.IsNotNull(storeItemResult.ModifiedDate);
        }

        /// <summary>
        /// Create - No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_NoName()
        {
            //Set up dto
            StoreItemDto storeItem = new StoreItemDto()
            {
                Name = null,
                Description = "Test",
                Cost = 9.99,
                InStock = true
            };

            _controller.Create(storeItem);
        }

        /// <summary>
        /// Create - Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestCreate_EmptyName()
        {
            //Set up dto
            StoreItemDto storeItem = new StoreItemDto()
            {
                Name = string.Empty,
                Description = "Test",
                Cost = 9.99,
                InStock = true
            };

            _controller.Create(storeItem);
        }

        /// <summary>
        /// No Description - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoDescription()
        {
            //Set up dto
            StoreItemDto storeItem = new StoreItemDto()
            {
                Name = "Test",
                Description = null,
                Cost = 9.99,
                InStock = true
            };

            //Make test request
            StoreItemDto storeItemResult = _controller.Create(storeItem);

            //Assert Values
            Assert.IsNotNull(storeItemResult);
            Assert.IsNotNull(storeItemResult.Id);
            Assert.IsTrue(storeItemResult.Id != 0);
            Assert.AreEqual(storeItem.Description, storeItemResult.Description);
        }

        /// <summary>
        /// No Image - Should Succeed
        /// </summary>
        [TestMethod]
        public void TestCreate_NoImage()
        {
            //Set up dto
            StoreItemDto storeItem = new StoreItemDto()
            {
                Name = "Test",
                Description = "Test",
                Cost = 9.99,
                InStock = true
            };

            //Make test request
            StoreItemDto storeItemResult = _controller.Create(storeItem);

            //Assert Values
            Assert.IsNotNull(storeItemResult);
            Assert.IsNotNull(storeItemResult.Id);
            Assert.IsTrue(storeItemResult.Id != 0);
        }
        #endregion

        #region Update
        /// <summary>
        /// Should Succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_ShouldSucceed()
        {
            //Get valid storeItem
            StoreItemDto storeItem = GetStoreItem();

            storeItem.Name = "Test";
            storeItem.Description = "Test";
            storeItem.Cost = 9.99;
            storeItem.InStock = true;

            StoreItemDto updatedStoreItem = _controller.Update(storeItem);

            Assert.IsNotNull(updatedStoreItem);
            Assert.IsNotNull(updatedStoreItem.Id);
            Assert.AreEqual(storeItem.Id, updatedStoreItem.Id);
            Assert.AreEqual(storeItem.Name, updatedStoreItem.Name);
            Assert.AreEqual(storeItem.Description, updatedStoreItem.Description);
            Assert.AreEqual(storeItem.Cost, updatedStoreItem.Cost);
            Assert.AreEqual(storeItem.InStock, updatedStoreItem.InStock);
        }

        /// <summary>
        /// No Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_NoName()
        {
            //Get valid storeItem
            StoreItemDto storeItem = GetStoreItem();

            storeItem.Name = null;
            storeItem.Description = "Test";
            storeItem.Cost = 9.99;
            storeItem.InStock = true;


            _controller.Update(storeItem);
        }

        /// <summary>
        /// Empty Name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
        public void TestUpdate_EmptyName()
        {
            //Get valid storeItem
            StoreItemDto storeItem = GetStoreItem();

            storeItem.Name = string.Empty;
            storeItem.Description = "Test";
            storeItem.Cost = 9.99;
            storeItem.InStock = true;


            _controller.Update(storeItem);
        }

        /// <summary>
        /// No Description - should succeed
        /// </summary>
        [TestMethod]
        public void TestUpdate_NoDescription()
        {
            //Get valid storeItem
            StoreItemDto storeItem = GetStoreItem();

            storeItem.Name = "Test";
            storeItem.Description = null;
            storeItem.Cost = 9.99;
            storeItem.InStock = true;

            StoreItemDto updatedStoreItem = _controller.Update(storeItem);

            Assert.IsNotNull(updatedStoreItem);
            Assert.IsNotNull(updatedStoreItem.Id);
            Assert.AreEqual(storeItem.Id, updatedStoreItem.Id);
            Assert.AreEqual(storeItem.Name, updatedStoreItem.Name);
            Assert.AreEqual(storeItem.Description, updatedStoreItem.Description);
            Assert.AreEqual(storeItem.Cost, updatedStoreItem.Cost);
            Assert.AreEqual(storeItem.InStock, updatedStoreItem.InStock);
        }

        #endregion

        #region GetById
        /// <summary>
        /// Test a valid Id GetById
        /// </summary>
        [TestMethod]
        public void TestGetById_ValidId()
        {
            //Get a valid storeItem 
            StoreItemDto validStoreItem = GetStoreItem();

            //Try to get this storeItem
            StoreItemDto storeItemResult = _controller.GetById(validStoreItem.Id);

            Assert.IsNotNull(storeItemResult);
            Assert.IsNotNull(storeItemResult.Id);
            Assert.AreEqual(validStoreItem.Id, storeItemResult.Id);
        }

        /// <summary>
        /// Test an invalid Id GetById
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find StoreItem.")]
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
            //Create StoreItem for getfiltered validation
            StoreItemDto validStoreItem = CreateTestStoreItem();

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
            //Create a storeItem to test on
            StoreItemDto validStoreItem = CreateTestStoreItem();

            //delete for test
            _controller.Delete(validStoreItem.Id);

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
            //Get valid storeItem
            StoreItemDto validStoreItem = GetStoreItem();

            //Delete storeItem
            bool result = _controller.Delete(validStoreItem.Id);

            Assert.IsTrue(result);

            //Get storeItem for comparison
            StoreItemDto storeItemResult = _controller.GetById(validStoreItem.Id);

            Assert.IsNotNull(storeItemResult);
            Assert.IsNotNull(storeItemResult.Id);
            Assert.AreEqual(validStoreItem.Id, storeItemResult.Id);
            Assert.IsTrue(storeItemResult.IsDeleted);
        }

        /// <summary>
        /// Delete test with invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Could not find StoreItem.")]
        public void TestDelete_InvalidId()
        {
            _controller.Delete(0);
        }
        #endregion
        #endregion

        #region Helpers
        StoreItemDto GetStoreItem()
        {
            ApiFilter filter = new ApiFilter() { isDeleted = false, numPerPage = 1, pageNumber = 0 };
            var storeItems = _controller.GetFiltered(filter);

            if (storeItems != null && storeItems.Any())
            {
                return storeItems.First();
            }
            else
            {
                //Create a new storeItem for testing
                return CreateTestStoreItem();
            }
        }

        StoreItemDto CreateTestStoreItem()
        {
            StoreItemDto storeItem = new StoreItemDto()
            {
                Name = "Test",
                Description = "Test",
                Cost = 9.99,
                InStock = true
            };

            storeItem = _controller.Create(storeItem);

            return storeItem;
        }
        #endregion
    }
}
