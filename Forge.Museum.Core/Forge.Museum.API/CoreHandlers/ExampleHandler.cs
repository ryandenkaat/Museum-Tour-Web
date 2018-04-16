using Forge.Museum.Interfaces.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forge.Museum.API.CoreHandlers
{
    public class ExampleHandler : BaseApiHandler
    {
        //Example handler method
        public ExampleDto GetExampleDto(int exampleId)
        {
            //Usually the database processing will occur here...


            ExampleDto response = new ExampleDto()
            {
                Id = 1,
                Name = "Some name",
                CreatedDate = DateTime.Now.ToLocalTime(),
                ModifiedDate = DateTime.Now.ToLocalTime(),
                IsDeleted = false
            };

            return response;
        }
    }
}