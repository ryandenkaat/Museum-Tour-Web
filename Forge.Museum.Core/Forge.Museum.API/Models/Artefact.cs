﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
    public class Artefact : Base
    {
        public Artefact()
        {
            Tours = new HashSet<Tour>();
        }

        public int Id { get; set; }
        
        [Required, StringLength(256)]
        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        [StringLength(4000)]
        public string AdditionalComments { get; set; }

        public DateTime AcquisitionDate { get; set; }

        public int Measurement_Length { get; set; }

        public int Measurement_Width { get; set; }

        public int Measurement_Height { get; set; }

        public int ArtefactStatus { get; set; }

        public virtual Zone Zone { get; set; }

        public virtual ArtefactCategory ArtefactCategory { get; set; }

        public virtual ICollection<ArtefactInfo> ArtefactInfos { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}