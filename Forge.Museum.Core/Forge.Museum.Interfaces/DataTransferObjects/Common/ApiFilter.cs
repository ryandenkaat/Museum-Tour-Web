using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Museum.Interfaces.DataTransferObjects.Common
{
    public class ApiFilter
    {
        public int? pageNumber { get; set; }

        public int? numPerPage { get; set; }

        public bool? isDeleted { get; set; }

        public int page
        {
            get
            {
                if (pageNumber.HasValue && pageNumber.Value >= 0) return pageNumber.Value;
                else return 0;
            }
        }

        public int pageSize
        {
            get
            {
                if (numPerPage.HasValue && numPerPage.Value > 0) return numPerPage.Value;
                else return 20;
            }
        }

        public static readonly ApiFilter All = new ApiFilter
        {
            pageNumber = 0,
            numPerPage = 5000,
            isDeleted = false
        };
    }
}
