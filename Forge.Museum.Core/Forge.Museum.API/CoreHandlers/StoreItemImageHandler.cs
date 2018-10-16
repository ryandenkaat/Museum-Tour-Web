using AutoMapper;
using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Forge.Museum.Interfaces.DataTransferObjects.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.CoreHandlers
{
	public class StoreItemImageHandler : BaseApiHandler
	{
		public StoreItemImageHandler(bool test = false) : base(test) { }

		#region CRUD
		public StoreItemImageDto Create(StoreItemImageDto dto)
		{
			StoreItemImage image = new StoreItemImage()
			{
				Image = dto.Image,
				FileType = dto.FileType,
				StoreItem = Db.StoreItems.Find(dto.StoreItem.Id),
				CreatedDate = DateTime.UtcNow,
				ModifiedDate = DateTime.UtcNow,
				IsDeleted = false
			};

			Db.StoreItemImages.Add(image);

			Db.SaveChanges();

			return Mapper.Map<StoreItemImageDto>(image);
		}

		public StoreItemImageDto Update(StoreItemImageDto dto)
		{
			StoreItemImage image = Db.StoreItemImages.Include("StoreItem").FirstOrDefault(m => m.Id == dto.Id);

			if (image == null) NotFoundException();

			image.Image = dto.Image;
			image.FileType = dto.FileType;
			image.StoreItem = Db.StoreItems.Find(dto.StoreItem.Id);
			image.ModifiedDate = DateTime.UtcNow;
			image.IsDeleted = dto.IsDeleted;

			Db.SaveChanges();

			return Mapper.Map<StoreItemImageDto>(image);
		}

		public StoreItemImageDto GetById(int storeItemImageId)
		{
			StoreItemImage image = Db.StoreItemImages.Find(storeItemImageId);

			if (image == null) NotFoundException();

			return Mapper.Map<StoreItemImageDto>(image);
		}

		public List<StoreItemImageDto> GetFiltered(ApiFilter filter)
		{
			IQueryable<StoreItemImage> images = Db.StoreItemImages;

			if (filter.isDeleted.HasValue)
				images = images.Where(m => m.IsDeleted == filter.isDeleted.Value);

			return Mapper.Map<List<StoreItemImageDto>>(images.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));

		}

		public bool Delete(int storeItemImageId)
		{
			StoreItemImage image = Db.StoreItemImages.Include("StoreItem").FirstOrDefault(m => m.Id == storeItemImageId);

			if (image == null) NotFoundException();

			image.IsDeleted = true;

			Db.SaveChanges();

			return true;
		}

		#endregion

	}
}