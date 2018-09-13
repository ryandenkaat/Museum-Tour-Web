using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;
using Forge.Museum.Interfaces.DataTransferObjects.Exhibition;
using Forge.Museum.Interfaces.DataTransferObjects.Store;

namespace Forge.Museum.API
{
    public class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            //Auto Map DB sets to the DataTransferObjects
            Mapper.Initialize(config =>
            {
                config.CreateMap<Artefact, ArtefactDto>();
                config.CreateMap<Artefact, ArtefactSimpleDto>();
                config.CreateMap<ArtefactInfo, ArtefactInfoDto>();
                config.CreateMap<ArtefactInfo, ArtefactInfoSimpleDto>();
                config.CreateMap<ArtefactCategory, ArtefactCategoryDto>();
                config.CreateMap<ArtefactCategory, ArtefactCategorySimpleDto>();
				config.CreateMap<Exhibition, ExhibitionDto>();
				config.CreateMap<StoreItem, StoreItemDto>();
				config.CreateMap<StoreItemImage, StoreItemImageDto>();
                config.CreateMap<Zone, ZoneDto>();
                config.CreateMap<Zone, ZoneSimpleDto>();
                config.CreateMap<Base, BaseDto>();
                config.CreateMap<Tour, TourDto>();
                config.CreateMap<Tour, TourSimpleDto>();
                config.CreateMap<TourArtefact, TourArtefactDto>();


            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}