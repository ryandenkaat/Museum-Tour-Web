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
	public class ArtefactCategoryController : BaseTestClass
	{
		private Forge.Museum.API.Controllers.ArtefactCategoryController _controller;

		[TestInitialize]
		public void SetupTest()
		{
			_controller = new API.Controllers.ArtefactCategoryController(true);
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
			ArtefactCategoryDto category = new ArtefactCategoryDto()
			{
				Name = "Test",
				Description = "Test",
				Image = testImage
			};

			//Make test request
			ArtefactCategoryDto categoryResult = _controller.Create(category);

			//Assert Values
			Assert.IsNotNull(categoryResult);
			Assert.IsNotNull(categoryResult.Id);
			Assert.IsTrue(categoryResult.Id != 0);
			Assert.AreEqual(category.Name, categoryResult.Name);
			Assert.AreEqual(category.Description, categoryResult.Description);
			Assert.IsNotNull(categoryResult.CreatedDate);
			Assert.IsNotNull(categoryResult.ModifiedDate);
		}

		/// <summary>
		/// Create - No Name
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
		public void TestCreate_NoName()
		{
			//Set up dto
			ArtefactCategoryDto category = new ArtefactCategoryDto()
			{
				Name = null,
				Description = "Test",
				Image = testImage
			};

			_controller.Create(category);
		}

		/// <summary>
		/// Create - Empty Name
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
		public void TestCreate_EmptyName()
		{
			//Set up dto
			ArtefactCategoryDto category = new ArtefactCategoryDto()
			{
				Name = string.Empty,
				Description = "Test",
				Image = testImage
			};

			_controller.Create(category);
		}

		/// <summary>
		/// No Description - Should Succeed
		/// </summary>
		[TestMethod]
		public void TestCreate_NoDescription()
		{
			//Set up dto
			ArtefactCategoryDto category = new ArtefactCategoryDto()
			{
				Name = "Test",
				Description = null,
				Image = testImage
			};

			//Make test request
			ArtefactCategoryDto categoryResult = _controller.Create(category);

			//Assert Values
			Assert.IsNotNull(categoryResult);
			Assert.IsNotNull(categoryResult.Id);
			Assert.IsTrue(categoryResult.Id != 0);
			Assert.AreEqual(category.Description, categoryResult.Description);
		}

		/// <summary>
		/// No Image - Should Succeed
		/// </summary>
		[TestMethod]
		public void TestCreate_NoImage()
		{
			//Set up dto
			ArtefactCategoryDto category = new ArtefactCategoryDto()
			{
				Name = "Test",
				Description = "Test",
				Image = null
			};

			//Make test request
			ArtefactCategoryDto categoryResult = _controller.Create(category);

			//Assert Values
			Assert.IsNotNull(categoryResult);
			Assert.IsNotNull(categoryResult.Id);
			Assert.IsTrue(categoryResult.Id != 0);
		}
		#endregion

		#region Update
		/// <summary>
		/// Should Succeed
		/// </summary>
		[TestMethod]
		public void TestUpdate_ShouldSucceed()
		{
			//Get valid category
			ArtefactCategoryDto category = GetArtefactCategory();

			category.Name = "Test Edit";
			category.Description = "Test Edit";
			category.Image = testImage;

			ArtefactCategoryDto updatedCategory = _controller.Update(category);

			Assert.IsNotNull(updatedCategory);
			Assert.IsNotNull(updatedCategory.Id);
			Assert.AreEqual(category.Id, updatedCategory.Id);
			Assert.AreEqual(category.Name, updatedCategory.Name);
			Assert.AreEqual(category.Description, updatedCategory.Description);
		}

		/// <summary>
		/// No Name
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
		public void TestUpdate_NoName()
		{
			//Get valid category
			ArtefactCategoryDto category = GetArtefactCategory();

			category.Name = null;
			category.Description = "Test Edit";
			category.Image = testImage;

			_controller.Update(category);
		}

		/// <summary>
		/// Empty Name
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "Name is a required field.")]
		public void TestUpdate_EmptyName()
		{
			//Get valid category
			ArtefactCategoryDto category = GetArtefactCategory();

			category.Name = string.Empty;
			category.Description = "Test Edit";
			category.Image = testImage;

			_controller.Update(category);
		}

		/// <summary>
		/// No Description - should succeed
		/// </summary>
		[TestMethod]
		public void TestUpdate_NoDescription()
		{
			//Get valid category
			ArtefactCategoryDto category = GetArtefactCategory();

			category.Name = "Test Edit";
			category.Description = null;
			category.Image = testImage;

			ArtefactCategoryDto updatedCategory = _controller.Update(category);

			Assert.IsNotNull(updatedCategory);
			Assert.IsNotNull(updatedCategory.Id);
			Assert.AreEqual(category.Id, updatedCategory.Id);
			Assert.AreEqual(category.Name, updatedCategory.Name);
			Assert.AreEqual(category.Description, updatedCategory.Description);
		}

		/// <summary>
		/// No Image
		/// </summary>
		[TestMethod]
		public void TestUpdate_NoImage()
		{
			//Get valid category
			ArtefactCategoryDto category = GetArtefactCategory();

			category.Name = "Test Edit";
			category.Description = "Test Edit";
			category.Image = null;

			ArtefactCategoryDto updatedCategory = _controller.Update(category);

			Assert.IsNotNull(updatedCategory);
			Assert.IsNotNull(updatedCategory.Id);
			Assert.AreEqual(category.Id, updatedCategory.Id);
			Assert.AreEqual(category.Name, updatedCategory.Name);
			Assert.AreEqual(category.Description, updatedCategory.Description);
		}

		/// <summary>
		/// Invalid Id
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(HttpResponseException), "Could not find Artefact Category.")]
		public void TestUpdate_InvalidId()
		{
			//Get valid category
			ArtefactCategoryDto category = GetArtefactCategory();

			ArtefactCategoryDto updateCategory = new ArtefactCategoryDto()
			{
				Id = 0,
				Name = category.Name,
				Description = category.Description,
				Image = category.Image
			};

			_controller.Update(updateCategory);
		}
		#endregion

		#region GetById
		/// <summary>
		/// Test a valid Id GetById
		/// </summary>
		[TestMethod]
		public void TestGetById_ValidId()
		{
			//Get a valid artefact category
			ArtefactCategoryDto validCategory = GetArtefactCategory();

			//Try to get this category
			ArtefactCategoryDto categoryResult = _controller.GetById(validCategory.Id);

			Assert.IsNotNull(categoryResult);
			Assert.IsNotNull(categoryResult.Id);
			Assert.AreEqual(validCategory.Id, categoryResult.Id);
		}

		/// <summary>
		/// Test an invalid Id GetById
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(HttpResponseException), "Could not find Artefact Category.")]
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
			//Create Category for getfiltered validation
			ArtefactCategoryDto validCategory = CreateTestArtefactCategory();

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
			//Create a category to test on
			ArtefactCategoryDto validCategory = CreateTestArtefactCategory();

			//delete for test
			_controller.Delete(validCategory.Id);

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
			//Get valid category
			ArtefactCategoryDto validCategory = GetArtefactCategory();

			//Delete category
			bool result = _controller.Delete(validCategory.Id);

			Assert.IsTrue(result);

			//Get Category for comparison
			ArtefactCategoryDto categoryResult = _controller.GetById(validCategory.Id);

			Assert.IsNotNull(categoryResult);
			Assert.IsNotNull(categoryResult.Id);
			Assert.AreEqual(validCategory.Id, categoryResult.Id);
			Assert.IsTrue(categoryResult.IsDeleted);
		}

		/// <summary>
		/// Delete test with invalid Id
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(HttpResponseException), "Could not find Artefact Category.")]
		public void TestDelete_InvalidId()
		{
			_controller.Delete(0);
		}
		#endregion
		#endregion

		#region Helpers
		ArtefactCategoryDto GetArtefactCategory()
		{
			ApiFilter filter = new ApiFilter() { isDeleted = false, numPerPage = 1, pageNumber = 0 };
			var categories = _controller.GetFiltered(filter);

			if(categories != null && categories.Any())
			{
				return categories.First();
			}
			else
			{
				//Create a new category for testing
				return CreateTestArtefactCategory();
			}
		}

		ArtefactCategoryDto CreateTestArtefactCategory()
		{
			ArtefactCategoryDto category = new ArtefactCategoryDto()
			{
				Name = "Test",
				Description = "Test",
				Image = testImage
			};

			category = _controller.Create(category);

			return category;
		}
		#endregion
	}
}
