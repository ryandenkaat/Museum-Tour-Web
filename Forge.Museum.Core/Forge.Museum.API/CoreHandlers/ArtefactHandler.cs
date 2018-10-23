using System;
using System.Collections.Generic;
using System.Linq;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.API.Models;
using AutoMapper;
using Forge.Museum.API.Controllers;

namespace Forge.Museum.API.CoreHandlers
{
    public class ArtefactHandler : BaseApiHandler
    {
		public ArtefactHandler(bool test = false) : base(test)
		{

		}

		#region CRUD
		public ArtefactDto Create(ArtefactDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            Artefact artefact = new Artefact
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
				ImageFileType = dto.ImageFileType,
                AdditionalComments = dto.AdditionalComments,
                AcquisitionDate = dto.AcquisitionDate,
                Measurement_Height = dto.Measurement_Height,
                Measurement_Length = dto.Measurement_Length,
                Measurement_Width = dto.Measurement_Width,
                ArtefactStatus = (int)dto.ArtefactStatus,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsDeleted = false,
				UniqueCode = CalculateUniqueCode()
            };

            //Add Zone 
            if(dto.Zone != null && dto.Zone.Id > 0)
            {
                artefact.Zone = Db.Zones.Find(dto.Zone.Id);
            }

            //Add category
            if(dto.ArtefactCategory != null && dto.ArtefactCategory.Id > 0)
            {
                artefact.ArtefactCategory = Db.ArtefactCategories.Find(dto.ArtefactCategory.Id);
            }

            Db.Artefacts.Add(artefact);

            Db.SaveChanges();

            return Mapper.Map<ArtefactDto>(artefact);
        }

        public ArtefactDto Update(ArtefactDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentNullException("Name");

            Artefact artefact = Db.Artefacts.FirstOrDefault(m => m.Id == dto.Id);

            if (artefact == null) NotFoundException();

            artefact.Name = dto.Name;
            artefact.Description = dto.Description;
            artefact.Image = dto.Image;
			artefact.ImageFileType = dto.ImageFileType;
            artefact.AdditionalComments = dto.AdditionalComments;
            artefact.AcquisitionDate = dto.AcquisitionDate;
            artefact.Measurement_Height = dto.Measurement_Height;
            artefact.Measurement_Length = dto.Measurement_Length;
            artefact.Measurement_Width = dto.Measurement_Width;
            artefact.ArtefactStatus = (int)dto.ArtefactStatus;
            artefact.IsDeleted = dto.IsDeleted;
            artefact.ModifiedDate = DateTime.UtcNow;

            //Process zone
            if(dto.Zone != null && dto.Zone.Id > 0)
            {
                artefact.Zone = Db.Zones.Find(dto.Zone.Id);
            }
            else
            {
                artefact.Zone = null;
            }

            //Process Category
            if(dto.ArtefactCategory != null && dto.ArtefactCategory.Id > 0)
            {
                artefact.ArtefactCategory = Db.ArtefactCategories.Find(dto.ArtefactCategory.Id);
            }
            else
            {
                artefact.ArtefactCategory = null;
            }

            Db.SaveChanges();

            return Mapper.Map<ArtefactDto>(artefact);
        }

        public ArtefactDto GetById(int artefactId)
        {
            Artefact artefact = Db.Artefacts.Find(artefactId);

            if (artefact == null) NotFoundException();

            return Mapper.Map<ArtefactDto>(artefact);
        }

        public List<ArtefactDto> GetFiltered(ArtefactFilter filter)
        {
            IQueryable<Artefact> artefacts = Db.Artefacts;

            if (filter.categoryId.HasValue)
                artefacts = artefacts.Where(m => m.ArtefactCategory.Id == filter.categoryId.Value);

            if (filter.zoneId.HasValue)
                artefacts = artefacts.Where(m => m.Zone.Id == filter.zoneId.Value);

            if (filter.isDeleted.HasValue)
                artefacts = artefacts.Where(m => m.IsDeleted == filter.isDeleted.Value);

            return Mapper.Map<List<ArtefactDto>>(artefacts.OrderBy(m => m.ModifiedDate).Skip(filter.pageSize * filter.page).Take(filter.pageSize));
        }

        public bool Delete(int artefactId)
        {
            Artefact artefact = Db.Artefacts.FirstOrDefault(m => m.Id == artefactId);

            if (artefact == null) NotFoundException();

            artefact.IsDeleted = true;
            artefact.ModifiedDate = DateTime.UtcNow;

            Db.SaveChanges();

            return true;
        }
		#endregion

		#region Helpers
		private string CalculateUniqueCode()
		{
			string uniqueCode;
			int nextCode = 1;

			try
			{ 
				var artefacts = Db.Artefacts;

				if (artefacts != null && artefacts.Any())
				{
					int currentMaxNum = Convert.ToInt32(Db.Artefacts.Select(m => m.UniqueCode).Max());

					nextCode = currentMaxNum + 1;
				}
			}
			catch(Exception ex)
			{
				nextCode = Db.Artefacts.Count() + 1;
			}

			var numZero = 4 - nextCode.ToString().Length;
			uniqueCode = string.Empty;
			for(var i = 1; i <= numZero; i++)
			{
				uniqueCode = uniqueCode + "0";
			}

			uniqueCode += nextCode.ToString();

			return uniqueCode;
		}
		#endregion

	}
}