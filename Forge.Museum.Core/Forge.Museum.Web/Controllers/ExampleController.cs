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

        public async Task<ActionResult> GetApiExample()
        {
            try
            {
                var request = new HTTPrequest();
                List<ArtefactDto> vm = await request.Get<List<ArtefactDto>>("api/artefact?pageNumber=0&numPerPage=500&isDeleted=false");
                return View(vm);
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<ActionResult> PostApiExample(string name, string description)
        {
            try
            {
                var request = new HTTPrequest();

                ArtefactDto dto = new ArtefactDto()
                {
                    Name = name,
                    Description = description,
                    //More properties here .....
                };

                dto = await request.Post<ArtefactDto>("api/artefact", dto);

                return View(dto);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}