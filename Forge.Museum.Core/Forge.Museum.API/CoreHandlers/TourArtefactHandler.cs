using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Forge.Museum.API.Controllers;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;

namespace Forge.Museum.API.CoreHandlers
{
    public class TourArtefactHandler : BaseApiHandler
    {
        public TourArtefactDto GetById(int tourArtefactId)
        {
            throw new NotImplementedException();
        }

        public List<TourArtefactDto> GetFiltered(TourArtefactFilter filter)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object artefactId)
        {
            throw new NotImplementedException();
        }

        public TourArtefactDto Update(TourArtefactDto dto)
        {
            throw new NotImplementedException();
        }

        public TourArtefactDto Create(TourArtefactDto dto)
        {
            throw new NotImplementedException();
        }
    }
}