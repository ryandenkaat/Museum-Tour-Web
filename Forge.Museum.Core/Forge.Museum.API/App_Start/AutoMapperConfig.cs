using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Forge.Museum.API.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects;

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
                config.CreateMap<Zone, ZoneDto>();
                config.CreateMap<Zone, ZoneSimpleDto>();
                config.CreateMap<Base, BaseDto>();
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}