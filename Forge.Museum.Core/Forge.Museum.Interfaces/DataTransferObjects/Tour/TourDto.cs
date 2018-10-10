using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.Interfaces.DataTransferObjects.Tour
{
    public class TourDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AgeGroup AgeGroup { get; set; }
        public List<TourArtefactDto> Artefacts { get; set; }
    }
}
