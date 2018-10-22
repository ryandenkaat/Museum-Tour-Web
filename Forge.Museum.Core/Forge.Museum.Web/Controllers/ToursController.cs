using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Forge.Museum.BLL.Http;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;
using Forge.Museum.Web.Models;
using PagedList;
using System.Windows.Input;

namespace Forge.Museum.Web.Controllers
{
    [Authorize]
    public class ToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TourDtoes
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page, string recentAction, string recentName, string recentId)
        {
            var request = new HTTPrequest();

            if (recentAction != null && recentAction.Count() > 0)
            {
                ViewBag.Action = recentAction;
                ViewBag.RecentName = recentName;
                ViewBag.RecentId = recentId;

            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.AgeSortParm = sortOrder == "Age Group" ? "age_desc" : "Age Group";

            List<TourDto> tourMasterList = await request.Get<List<TourDto>>("api/tour?pageNumber=0&numPerPage=500&isDeleted=false");
            IEnumerable<TourDto> toursFiltered = tourMasterList.ToList();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                toursFiltered = toursFiltered.Where(a => a.Name.Contains(searchString));
            }


            var tours = toursFiltered;

            switch (sortOrder)
            {
                case "id_desc":
                    tours = tours.OrderByDescending(a => a.Id);
                    break;
                case "Name":
                    tours = tours.OrderBy(a => a.Name);
                    break;
                case "name_desc":
                    tours = tours.OrderByDescending(a => a.Name);
                    break;
                case "Age Group":
                    tours = tours.OrderBy(a => a.AgeGroup.ToString());
                    break;
                case "age_desc":
                    tours = tours.OrderByDescending(a => a.AgeGroup.ToString());
                    break;
                default:
                    tours = tours.OrderBy(a => a.Id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tours.ToPagedList(pageNumber, pageSize));
        }

        // GET: TourDtoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            TourDto tour = await request.Get<TourDto>("api/tour/" + id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // GET: TourDtoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TourDtoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TourDto tour, HttpPostedFileBase imageFile)
        {
            if (string.IsNullOrEmpty(tour.Name))
            {
                ViewBag.ValidationName = "Name field is required.";
                return View(tour);
            }

            if (ModelState.IsValid)
            {
                tour.CreatedDate = DateTime.Now;
                var request = new HTTPrequest();
                tour = await request.Post<TourDto>("api/tour", tour);
                return RedirectToAction("Index", new { recentAction = "Created", recentName = tour.Name, recentId = tour.Id });
            }

            return View();
        }

        // GET: TourDtoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var request = new HTTPrequest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourDto tour = await request.Get<TourDto>("api/tour/" + id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            List<ArtefactSimpleDto> artefactsList = new List<ArtefactSimpleDto>();
            ArtefactSimpleDto artefact = new ArtefactSimpleDto();

            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            artefactsList = await request.Get<List<ArtefactSimpleDto>>("api/artefact?pageNumber=0&numPerPage=5-0&isDeleted=false");
            if (artefactsList != null && artefactsList.Any())
            {
                foreach (var artefacts in artefactsList)
                {
                    artefactDropdown.Add(new SelectListItem()
                    {
                        Text = artefacts.Id + ":" + artefacts.Name,
                        Value = artefacts.Id.ToString()
                    });
                }
            }

            ViewBag.ArtefactList = artefactDropdown;
            return View(tour);
        }

        // POST: TourDtoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TourDto tour, HttpPostedFileBase imageFile)
        {
            if (string.IsNullOrEmpty(tour.Name))
            {
                ViewBag.ValidationName = "Name field is required.";
                return View(tour);
            }
            var request = new HTTPrequest();
            TourDto tour_editted = await request.Get<TourDto>("api/tour/" + tour.Id);
            if (ModelState.IsValid) {
                tour_editted.Name = tour.Name;
                tour_editted.Description = tour.Description;
                tour_editted.AgeGroup = tour.AgeGroup;
                tour_editted.ModifiedDate = DateTime.Now;
                await request.Put<TourDto>("api/tour", tour_editted);
                return RedirectToAction("Index", new { recentAction = "Editted", recentName = tour.Name, recentId = tour.Id });
            }
            return View(tour);
        }

        // GET: TourDtoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            TourDto tour = await request.Get<TourDto>("api/tour/" + id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: TourDtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
                var request = new HTTPrequest();
                TourDto tour = await request.Get<TourDto>("api/tour/" + id);
                tour.IsDeleted = true;
                await request.Delete("api/tour/" + id.ToString());
                return RedirectToAction("Index", new { recentAction = "Deleted", recentName = tour.Name, recentId = tour.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
