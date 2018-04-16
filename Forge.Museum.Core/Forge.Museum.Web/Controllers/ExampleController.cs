using Forge.Museum.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}