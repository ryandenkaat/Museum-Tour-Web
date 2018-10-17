using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Forge.Museum.API.CoreHandlers
{
    public class ZoneHandler : BaseApiHandler
    {
		public ZoneHandler(bool test = false) : base(test) { }

        #region CRUD
        public ZoneDto Create(ZoneDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            Zone zone = new Zone
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            Db.Zones.Add(zone);

            Db.SaveChanges();

            return Mapper.Map<ZoneDto>(zone);
        }

        public ZoneDto Update(ZoneDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            Zone zone = Db.Zones.FirstOrDefault(m => m.Id == dto.Id);

            if (zone == null) NotFoundException();

            zone.Name = dto.Name;
            zone.Description = dto.Description;
            zone.ModifiedDate = DateTime.UtcNow;
            zone.IsDeleted = dto.IsDeleted;

            Db.SaveChanges();

            return Mapper.Map<ZoneDto>(zone);
        }

        public ZoneDto GetById(int zoneId)
        {
            Zone zone = Db.Zones.Find(zoneId);

            if (zone == null) NotFoundException();

            return Mapper.Map<ZoneDto>(zone);
        }

        public List<ZoneDto> GetFiltered(ApiFilter filter)
        {
            IQueryable<Zone> zones = Db.Zones;

            if (filter.isDeleted.HasValue)
                zones = zones.Where(m => m.IsDeleted == filter.isDeleted.Value);

            return Mapper.Map<List<ZoneDto>>(zones.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));
        }

        public bool Delete(int zoneId)
        {
            Zone zone = Db.Zones.FirstOrDefault(m => m.Id == zoneId);

            if (zone == null) NotFoundException();

            zone.IsDeleted = true;
            zone.ModifiedDate = DateTime.UtcNow;

            Db.SaveChanges();

            return true;
        }
        #endregion
    }
}