using Forge.Museum.Interfaces.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.Interfaces.DataTransferObjects.Artefact
{
    public class ArtefactInfoDto : BaseDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public byte[] File { get; set; }
		public string FileExtension { get; set; }
        public ArtefactInfoType ArtefactInfoType { get; set; }
        public string Content { get; set; }
        public ArtefactSimpleDto Artefact { get; set; }
    }
}
