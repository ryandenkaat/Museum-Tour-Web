using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Forge.Museum.Interfaces.DataTransferObjects.Store;

namespace Forge.Museum.API.CoreHandlers
{
	public class StoreItemHandler : BaseApiHandler
	{
		public StoreItemHandler(bool test = false) : base(test) { }

		#region CRUD
		public StoreItemDto Create(StoreItemDto dto)
		{
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            StoreItem storeItem = new StoreItem()
			{
				Name = dto.Name,
				Description = dto.Description,
				Cost = dto.Cost,
				InStock = dto.InStock,
				CreatedDate = DateTime.UtcNow,
				ModifiedDate = DateTime.UtcNow,
				IsDeleted = false
			};

			Db.StoreItems.Add(storeItem);

			Db.SaveChanges();

			return Mapper.Map<StoreItemDto>(storeItem);
		}

		public StoreItemDto Update(StoreItemDto dto)
		{
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            StoreItem storeItem = Db.StoreItems.FirstOrDefault(m => m.Id == dto.Id);

			if (storeItem == null) NotFoundException();

			storeItem.Name = dto.Name;
			storeItem.Description = dto.Description;
			storeItem.Cost = dto.Cost;
			storeItem.InStock = dto.InStock;
			storeItem.IsDeleted = dto.IsDeleted;
			storeItem.ModifiedDate = dto.ModifiedDate;

			Db.SaveChanges();

			return Mapper.Map<StoreItemDto>(storeItem);
		}

		public StoreItemDto GetById(int storeItemId)
		{
			StoreItem storeItem = Db.StoreItems.Find(storeItemId);

			if (storeItem == null) NotFoundException();

			return Mapper.Map<StoreItemDto>(storeItem);
		}

		public List<StoreItemDto> GetFiltered(ApiFilter filter)
		{
			IQueryable<StoreItem> storeItems = Db.StoreItems;

			if (filter.isDeleted.HasValue)
				storeItems = storeItems.Where(m => m.IsDeleted == filter.isDeleted.Value);

			return Mapper.Map<List<StoreItemDto>>(storeItems.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));
		}

		public bool Delete(int storeItemId)
		{
			StoreItem storeItem = Db.StoreItems.FirstOrDefault(m => m.Id == storeItemId);

			if (storeItem == null) NotFoundException();

			storeItem.IsDeleted = true;

			Db.SaveChanges();

			return true;
		}
		#endregion

	}
}