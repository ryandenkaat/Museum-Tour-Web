using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
    public class Tour : Base
    {
        public Tour()
        {
            Artefacts = new HashSet<TourArtefact>();
        }

        public int Id { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        public int AgeGroup { get; set; }

        public virtual ICollection<TourArtefact> Artefacts { get; set; }
    }
}