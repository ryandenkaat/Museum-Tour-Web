using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Forge.Museum.API.Controllers;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;

namespace Forge.Museum.API.CoreHandlers
{
    public class TourHandler : BaseApiHandler
    {
        #region CRUD
        public TourDto Create(TourDto dto)
        {
            throw new NotImplementedException();
        }

        public TourDto Update(TourDto dto)
        {
            throw new NotImplementedException();
        }

        public TourDto GetById(int tourId)
        {
            throw new NotImplementedException();
        }

        public List<TourDto> GetFiltered(TourFilter filter)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int tourId)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}