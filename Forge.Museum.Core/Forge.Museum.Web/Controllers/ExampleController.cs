using Forge.Museum.BLL.Http;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Forge.Museum.Web.Controllers
{
    public class ExampleController : Controller
    {
        public ExampleController()
        {

        }

        public ActionResult Index()
        {
            ViewBag.Title = "Example";

            ExampleViewModel vm = new ExampleViewModel()
            {
                ExampleProperty = 69,
                ExampleString = "Hello World."
            };

            //This will return the view called "index.cshtml" under the Example folder in Views
            return View(vm);
        }

        public async Task<ActionResult> ApiExample()
        {
            try
            {
                var request = new HTTPrequest();

                List<ArtefactDto> vm = await request.Get<List<ArtefactDto>>("api/artefact?pageNumber=0&numPerPage=500&isDeleted=false");
            }
            catch(Exception ex)
            {
                throw;
            }


            return View(vm);
        }
    }
}