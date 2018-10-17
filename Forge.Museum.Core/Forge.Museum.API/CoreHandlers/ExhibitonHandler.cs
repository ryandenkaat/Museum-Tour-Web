using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using Forge.Museum.Interfaces.DataTransferObjects.Exhibition;

namespace Forge.Museum.API.CoreHandlers
{
	public class ExhibitonHandler : BaseApiHandler
	{
		public ExhibitonHandler(bool test = false) : base(test)
		{
			
		}

		#region CRUD
		public ExhibitionDto Create(ExhibitionDto dto)
		{
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            Exhibition exhibition = new Exhibition()
			{
				Name = dto.Name,
				Description = dto.Description,
				StartDate = dto.StartDate,
				FinishDate = dto.FinishDate,
				Organiser = dto.Organiser,
				Price_Adult = dto.Price_Adult,
				Price_Child = dto.Price_Child,
				Price_Concession = dto.Price_Concession,
				CreatedDate = DateTime.UtcNow,
				ModifiedDate = DateTime.UtcNow,
				IsDeleted = false
			};

			if(dto.Image != null)
			{
				exhibition.Image = dto.Image;
			}

			Db.Exhibitions.Add(exhibition);

			Db.SaveChanges();

			return Mapper.Map<ExhibitionDto>(exhibition);
		}

		public ExhibitionDto Update(ExhibitionDto dto)
		{
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            var exhibition = Db.Exhibitions.FirstOrDefault(m => m.Id == dto.Id);

			if (exhibition == null) NotFoundException();

			exhibition.Name = dto.Name;
			exhibition.Description = dto.Description;
			exhibition.StartDate = dto.StartDate;
			exhibition.FinishDate = dto.FinishDate;
			exhibition.Organiser = dto.Organiser;
			exhibition.Price_Adult = dto.Price_Adult;
			exhibition.Price_Child = dto.Price_Child;
			exhibition.Price_Concession = dto.Price_Concession;
			exhibition.ModifiedDate = DateTime.UtcNow;
			
			if(dto.Image != null)
			{
				exhibition.Image = dto.Image;
			}

			Db.SaveChanges();

			return Mapper.Map<ExhibitionDto>(dto);
		}

		public ExhibitionDto GetById(int exhibitionId)
		{
			var exhibition = Db.Exhibitions.Find(exhibitionId);

			if (exhibition == null) NotFoundException();

			return Mapper.Map<ExhibitionDto>(exhibition);
		}

		public List<ExhibitionDto> GetFiltered(ApiFilter filter)
		{
			IQueryable<Exhibition> exhibitions = Db.Exhibitions;

			if (filter.isDeleted.HasValue)
				exhibitions = exhibitions.Where(m => m.IsDeleted == filter.isDeleted.Value);

			return Mapper.Map<List<ExhibitionDto>>(exhibitions.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));
		}

		public bool Delete(int exhibitionId)
		{
			var exhibition = Db.Exhibitions.FirstOrDefault(m => m.Id == exhibitionId);

			if (exhibition == null) NotFoundException();

			exhibition.IsDeleted = true;

            Db.SaveChanges();

			return true;
		}
		#endregion

	}
}