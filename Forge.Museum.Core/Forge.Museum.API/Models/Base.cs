using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.Models
{
    public class Base
    {
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

    }
}