using Forge.Museum.Interfaces.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.Interfaces.DataTransferObjects.Artefact
{
    public class ArtefactDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
		public string ImageFileType { get; set; }
        public string AdditionalComments { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public int Measurement_Length { get; set; }
        public int Measurement_Width { get; set; }
        public int Measurement_Height { get; set; }
		public string UniqueCode { get; set; }
        public ArtefactStatus ArtefactStatus { get; set; }
        public ZoneSimpleDto Zone { get; set; }
        public ArtefactCategorySimpleDto ArtefactCategory { get; set; }
        public List<ArtefactInfoSimpleDto> ArtefactInfos { get; set; }
    }
}
