using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Forge.Museum.Web.Models
{
    public class Artefact
    {

        public int ArtefactID { get; set; }

        [Required]
        public string ArtefactDescription { get; set; }

        public string ArtefactAdditionalComments { get; set; }

        public DateTime AcquisitionDate { get; set; }

        public double ArtefactMeasurement_Length { get; set; }

        public double ArtefactMeasurement_Width { get; set; }

        public double ArtefactMeasurement_Height { get; set; }
    }
}