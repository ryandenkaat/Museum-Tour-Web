using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Forge.Museum.API.Controllers;
using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;

namespace Forge.Museum.API.CoreHandlers
{
	public class TourHandler : BaseApiHandler
	{
		public TourHandler(bool test = false) : base(test) { }
        #region CRUD
        public TourDto Create(TourDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            Tour tour = new Tour()
			{
				Name = dto.Name,
				Description = dto.Description,
				AgeGroup = (int)dto.AgeGroup,
				ModifiedDate = DateTime.UtcNow,
				CreatedDate = DateTime.UtcNow,
				IsDeleted = false
			};

			//Handle Artefacts
			if(dto.Artefacts != null && dto.Artefacts.Any())
			{
				foreach(var artefact in dto.Artefacts)
				{
					var artefactEntity = Db.TourArtefacts.Find(artefact.Id);

					if (artefactEntity != null)
						tour.Artefacts.Add(artefactEntity);
				}
			}

			Db.Tours.Add(tour);

			Db.SaveChanges();

			return Mapper.Map<TourDto>(tour);
        }

        public TourDto Update(TourDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            Tour tour = Db.Tours.FirstOrDefault(m => m.Id == dto.Id);

			if (tour == null)
				NotFoundException();

			tour.Name = dto.Name;
			tour.Description = dto.Description;
			tour.AgeGroup = (int)dto.AgeGroup;
			tour.ModifiedDate = DateTime.UtcNow;
			tour.IsDeleted = dto.IsDeleted;

			//Handle Artefacts
			var toRemove = tour.Artefacts.Where(m => !dto.Artefacts.Any(a => a.Id == m.Id)).ToList();

			if(toRemove != null && toRemove.Any())
			{
				foreach(var artefact in toRemove)
				{
					tour.Artefacts.Remove(artefact);
				}
			}

			if (dto.Artefacts != null && dto.Artefacts.Any())
			{
				foreach (var artefact in dto.Artefacts)
				{
					var artefactEntity = Db.TourArtefacts.Find(artefact.Id);

					if (artefactEntity != null)
						tour.Artefacts.Add(artefactEntity);
				}
			}

			Db.SaveChanges();

			return Mapper.Map<TourDto>(tour);
        }

        public TourDto GetById(int tourId)
        {
			Tour tour = Db.Tours.Find(tourId);

			if (tour == null)
				NotFoundException();

			return Mapper.Map<TourDto>(tour);
        }

        public List<TourDto> GetFiltered(TourFilter filter)
        {
			IQueryable<Tour> tours = Db.Tours;

			if (filter.isDeleted.HasValue)
				tours = tours.Where(m => m.IsDeleted == filter.isDeleted.Value);

			if (filter.artefactId.HasValue)
				tours = tours.Where(m => m.Artefacts.Any(a => a.Id == filter.artefactId.Value));

			return Mapper.Map<List<TourDto>>(tours.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));
        }

        public bool Delete(int tourId)
        {
            Tour tour = Db.Tours.FirstOrDefault(m => m.Id == tourId);

            if (tour == null) NotFoundException();

            tour.IsDeleted = true;
            tour.ModifiedDate = DateTime.UtcNow;

            Db.SaveChanges();

            return true;
        }
        #endregion

    }
}