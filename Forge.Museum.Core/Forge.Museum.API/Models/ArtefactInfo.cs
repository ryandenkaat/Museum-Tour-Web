using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
    public class ArtefactInfo : Base
    {
        public int Id { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        public byte[] File { get; set; }

		public string FileExtension { get; set; }

        public int ArtefactInfoType { get; set; }

        public string Content { get; set; }

        [Required]
        public virtual Artefact Artefact { get; set; }
    }
}