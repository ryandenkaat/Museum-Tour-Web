﻿using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Forge.Museum.API.CoreHandlers
{
    public class ArtefactCategoryHandler : BaseApiHandler
    {
        #region CRUD
        public ArtefactCategoryDto Create(ArtefactCategoryDto dto)
        {
            ArtefactCategory category = new ArtefactCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            //TODO add processing for adding artefacts on create

            Db.ArtefactCategories.Add(category);

            Db.SaveChanges();

            return Mapper.Map<ArtefactCategoryDto>(category);
        }

        public ArtefactCategoryDto Update(ArtefactCategoryDto dto)
        {
            ArtefactCategory category = Db.ArtefactCategories.FirstOrDefault(m => m.Id == dto.Id);

            if (category == null) NotFoundException();

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.Image = dto.Image;
            category.ModifiedDate = DateTime.UtcNow;
            category.IsDeleted = dto.IsDeleted;

            //TODO add processsing for adding artefacts on update

            Db.SaveChanges();

            return Mapper.Map<ArtefactCategoryDto>(category);
        }

        public ArtefactCategoryDto GetById(int artefactCategoryId)
        {
            ArtefactCategory category = Db.ArtefactCategories.Find(artefactCategoryId);

            if (category == null) NotFoundException();

            return Mapper.Map<ArtefactCategoryDto>(category);
        }

        public List<ArtefactCategoryDto> GetFiltered(ApiFilter filter)
        {
            IQueryable<ArtefactCategory> categories = Db.ArtefactCategories;

            if (filter.isDeleted.HasValue)
                categories = categories.Where(m => m.IsDeleted == filter.isDeleted.Value);

            return Mapper.Map<List<ArtefactCategoryDto>>(categories.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));
        }

        public bool Delete(int artefactCategoryId)
        {
            ArtefactCategory category = Db.ArtefactCategories.FirstOrDefault(m => m.Id == artefactCategoryId);

            if (category == null) NotFoundException();

            category.IsDeleted = true;
            category.ModifiedDate = DateTime.UtcNow;

            return true;
        }
        #endregion
    }
}