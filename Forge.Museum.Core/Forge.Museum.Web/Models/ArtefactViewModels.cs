using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Forge.Museum.Web.Models
{
    public class ArtefactsCategory
    {

        public int SelectedCategoryId { get; set; }

        [Required]
        public string ArtefactDescription { get; set; }

        public string ArtefactAdditionalComments { get; set; }

        [DataType(DataType.Date)]
        public DateTime AcquisitionDate { get; set; }

        public double ArtefactMeasurement_Length { get; set; }

        public double ArtefactMeasurement_Width { get; set; }

        public double ArtefactMeasurement_Height { get; set; }
    }
}