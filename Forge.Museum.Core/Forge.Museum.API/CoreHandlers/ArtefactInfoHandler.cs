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
    public class ArtefactInfoHandler : BaseApiHandler
    {
		public ArtefactInfoHandler(bool test = false) : base(test)
		{
			
		}

        #region CRUD
        public ArtefactInfoDto Create(ArtefactInfoDto dto)
        {
            ArtefactInfo artefactInfo = new ArtefactInfo
            {
                Description = dto.Description,
                File = dto.File,
				FileExtension = dto.FileExtension,
                ArtefactInfoType = (int)dto.ArtefactInfoType,
                Content = dto.Content,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsDeleted = false,
                Artefact = Db.Artefacts.Find(dto.Artefact.Id)
            };

            Db.ArtefactInfos.Add(artefactInfo);

            Db.SaveChanges();

            return Mapper.Map<ArtefactInfoDto>(artefactInfo);
        }

        public ArtefactInfoDto Update(ArtefactInfoDto dto)
        {
            ArtefactInfo artefactInfo = Db.ArtefactInfos.Include("Artefact").FirstOrDefault(m => m.Id == dto.Id);

            if (artefactInfo == null) NotFoundException();

            artefactInfo.Description = dto.Description;
            artefactInfo.File = dto.File;
			artefactInfo.FileExtension = dto.FileExtension;
            artefactInfo.ArtefactInfoType = (int)dto.ArtefactInfoType;
            artefactInfo.Content = dto.Content;
            artefactInfo.ModifiedDate = DateTime.UtcNow;
            artefactInfo.IsDeleted = dto.IsDeleted;
            artefactInfo.Artefact = Db.Artefacts.Find(dto.Artefact.Id);

            Db.SaveChanges();

            return Mapper.Map<ArtefactInfoDto>(artefactInfo);
        }

        public ArtefactInfoDto GetById(int artefactInfoId)
        {
            ArtefactInfo artefactInfo = Db.ArtefactInfos.Find(artefactInfoId);

            if (artefactInfo == null) NotFoundException();

            return Mapper.Map<ArtefactInfoDto>(artefactInfo);
        }

        public List<ArtefactInfoDto> GetFiltered(ApiFilter filter, int? artefactId)
        {
            IQueryable<ArtefactInfo> infos = Db.ArtefactInfos;

            if (artefactId.HasValue)
                infos = infos.Where(m => m.Artefact.Id == artefactId.Value);

            if (filter.isDeleted.HasValue)
                infos = infos.Where(m => m.IsDeleted == filter.isDeleted.Value);

            return Mapper.Map<List<ArtefactInfoDto>>(infos.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));
        }

        public bool Delete(int artefactInfoId)
        {
            ArtefactInfo artefactInfo = Db.ArtefactInfos.Include("Artefact").FirstOrDefault(m => m.Id == artefactInfoId);

            if (artefactInfo == null) NotFoundException();

            artefactInfo.IsDeleted = true;
            artefactInfo.ModifiedDate = DateTime.Now;

            Db.SaveChanges();

            return true;
        }
        #endregion
    }
}