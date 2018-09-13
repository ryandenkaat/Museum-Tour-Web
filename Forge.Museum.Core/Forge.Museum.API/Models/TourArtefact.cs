using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
    public class TourArtefact : Base
    {
        public int Id { get; set; }

        public int Order { get; set; }

        [Required]
        public virtual Artefact Artefact { get; set; }

        [Required]
        public virtual Tour Tour { get; set; }
    }
}