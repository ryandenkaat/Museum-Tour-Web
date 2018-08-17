using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
    public partial class ArtefactInfo 
    {
        public int Id { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        public byte[] File { get; set; }

		public string FileExtension { get; set; }

        public int ArtefactInfoType { get; set; }

        public string Content { get; set; }


    }
}