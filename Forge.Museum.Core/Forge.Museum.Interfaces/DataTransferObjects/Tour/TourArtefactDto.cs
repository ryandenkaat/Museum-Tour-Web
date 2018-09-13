using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.Interfaces.DataTransferObjects.Tour
{
    public class TourArtefactDto : BaseDto
    {

        public int Id { get; set; }

        public int Order { get; set; }

        public ArtefactSimpleDto Artefact { get; set; }

        public TourSimpleDto Tour { get; set; }

    }
}
