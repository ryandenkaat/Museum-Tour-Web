using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Forge.Museum.API.Controllers;
using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;

namespace Forge.Museum.API.CoreHandlers
{
    public class TourArtefactHandler : BaseApiHandler
    {
		public TourArtefactHandler(bool test = false) : base(test) { }

		public TourArtefactDto Create(TourArtefactDto dto)
		{
            if (dto.Order < 0) throw new ArgumentNullException("Order must be a position Number");

            var tour = Db.Tours.Find(dto.Tour.Id);

			if(tour == null || (tour.Artefacts != null && tour.Artefacts.Any(m => m.Order == dto.Order)))
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}

			TourArtefact tourArtefact = new TourArtefact()
			{
				Order = dto.Order,
				Artefact = Db.Artefacts.Find(dto.Artefact.Id),
				Tour = Db.Tours.Find(dto.Tour.Id),
				CreatedDate = DateTime.UtcNow,
				ModifiedDate = DateTime.UtcNow,
				IsDeleted = false
			};

			Db.TourArtefacts.Add(tourArtefact);

			Db.SaveChanges();

			return Mapper.Map<TourArtefactDto>(tourArtefact);
		}

		public TourArtefactDto Update(TourArtefactDto dto)
		{
            if (dto.Order<0) throw new ArgumentNullException("Order must be a position Number");

            var tour = Db.Tours.Find(dto.Tour.Id);

			if (tour == null || (tour.Artefacts == null)) // && tour.Artefacts.Any(m => m.Id != dto.Id && m.Order == dto.Order)))
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}

            TourArtefact tourArtefact = Db.TourArtefacts.Include("Artefact").Include("Tour").FirstOrDefault(m => m.Id == dto.Id);

			if(tourArtefact == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			tourArtefact.Order = dto.Order;
			tourArtefact.ModifiedDate = DateTime.UtcNow;

			Db.SaveChanges();

			return Mapper.Map<TourArtefactDto>(tourArtefact);
		}

		public TourArtefactDto GetById(int tourArtefactId)
        {
			TourArtefact tourArtefact = Db.TourArtefacts.Find(tourArtefactId);

			if(tourArtefact == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			return Mapper.Map<TourArtefactDto>(tourArtefact);
        }

        public List<TourArtefactDto> GetFiltered(TourArtefactFilter filter)
        {
			IQueryable<TourArtefact> tourArtefacts = Db.TourArtefacts;

			if(filter.isDeleted.HasValue)
			{
				tourArtefacts = tourArtefacts.Where(m => m.IsDeleted == filter.isDeleted.Value);
			}

			if(filter.artefactId.HasValue)
			{
				tourArtefacts = tourArtefacts.Where(m => m.Artefact.Id == filter.artefactId.Value);
			}

			if(filter.tourId.HasValue)
			{
				tourArtefacts = tourArtefacts.Where(m => m.Tour.Id == filter.tourId.Value);
			}

			return Mapper.Map<List<TourArtefactDto>>(tourArtefacts.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));
        }

        public bool Delete(int artefactId)
        {
			TourArtefact tourArtefact = Db.TourArtefacts.Include("Artefact").Include("Tour").FirstOrDefault(m => m.Id == artefactId);

			if (tourArtefact == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			tourArtefact.IsDeleted = true;

			Db.SaveChanges();

			return true;
        }
    }
}