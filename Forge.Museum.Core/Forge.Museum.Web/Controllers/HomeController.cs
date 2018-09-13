using System.Threading.Tasks;
using Forge.Museum.BLL.Http;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects.Exhibition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forge.Museum.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var requestArtefacts = new HTTPrequest();

            //Get list of Artefacts and pass Count of Artefact Entires
            List<ArtefactDto> artefactsMasterList = await requestArtefacts.Get<List<ArtefactDto>>("api/artefact?pageNumber=0&numPerPage=999999&isDeleted=false");
            int artefactsCount = artefactsMasterList.Count;
            ViewBag.TotalArtefacts = artefactsCount;


            //Get the most recently modified Artefact and pass NAME, ID, MODIFIED DATE via ViewBag
            List<ArtefactDto> orderedArtefactMasterList = artefactsMasterList.OrderByDescending(m => m.ModifiedDate).ToList();
            ArtefactDto mostRecentArtefact = orderedArtefactMasterList.ElementAt(0);
            ViewBag.mostRecentArtefactName = mostRecentArtefact.Name;
            ViewBag.mostRecentArtefactId = mostRecentArtefact.Id;
            ViewBag.mostRecentArtefactModDate = mostRecentArtefact.ModifiedDate;


            //Get list of Artefacts and pass Count of Artefact Entires
            List<ArtefactInfoDto> artefactInfoMasterList = await GetArtefactInfo();
            int artefactInfoCount = artefactInfoMasterList.Count;
            ViewBag.TotalArtefactContentEntries = artefactInfoCount;

            //Get list of Exibitions and pass Count of Artefact Entires
            List<ExhibitionDto> exhibitionsMasterList = await GetExhibitions();
            List<ExhibitionDto> futureExhibitions = exhibitionsMasterList.Where(e => e.StartDate >= DateTime.Today).ToList().OrderBy(e => e.StartDate).ToList();
            int exhibitionCountFuture = futureExhibitions.Count();
            ExhibitionDto nextExhibition;
            if (exhibitionCountFuture > 0)
            {
                nextExhibition = futureExhibitions.ElementAt(0);
                ViewBag.FutureExhibitionCount = exhibitionCountFuture;
                ViewBag.NextExhibitionId = nextExhibition.Id;
                ViewBag.NextExhibitionName = nextExhibition.Name;
                ViewBag.NextExhibitionStart = nextExhibition.StartDate.ToShortDateString();
            } else
            {
                nextExhibition = null;
                ViewBag.FutureExhibitionCount = 0;

            }




            return View();

        }

        public ViewResult IndexAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ArtefactInfoDto>> GetArtefactInfo()
        {
            var request = new HTTPrequest();
            List<ArtefactInfoDto> artefactInfoMasterList = await request.Get<List<ArtefactInfoDto>>("api/artefactInfo?pageNumber=0&numPerPage=99999&isDeleted=false");
            return artefactInfoMasterList;
        }


        public async Task<List<ExhibitionDto>> GetExhibitions()
        {
            var request = new HTTPrequest();
            List<ExhibitionDto> exhibitionsMasterList = await request.Get<List<ExhibitionDto>>("api/exhibition?pageNumber=0&numPerPage=99999&isDeleted=false");
            return exhibitionsMasterList;
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Artefacts()
        {
            ViewBag.Message = "Artefacts.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}