using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Forge.Museum.BLL.Http;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Interfaces.DataTransferObjects.Tour;
using Forge.Museum.Web.Models;

namespace Forge.Museum.Web.Controllers
{
    public class ToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TourDtoes
        public async Task<ActionResult> Index()
        {
            var request = new HTTPrequest();

            List<TourDto> viewModel = await request.Get<List<TourDto>>("api/tour?pageNumber=0&numPerPage=500&isDeleted=false");

            return View(viewModel);
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
            if (ModelState.IsValid)
            {
                tour.CreatedDate = DateTime.Now;
                var request = new HTTPrequest();
                tour = await request.Post<TourDto>("api/tour", tour);
                return RedirectToAction("Index");
            }

            return View(tour);
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
            var request = new HTTPrequest();
            TourDto tour_editted = await request.Get<TourDto>("api/tour/" + tour.Id);
            if (ModelState.IsValid) {
                tour_editted.Name = tour.Name;
                tour_editted.Description = tour.Description;
                tour_editted.AgeGroup = tour.AgeGroup;
                tour_editted.ModifiedDate = DateTime.Now;
                await request.Put<TourDto>("api/tour", tour_editted);
                return RedirectToAction("Index");
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
            try
            {
                var request = new HTTPrequest();
                await request.Delete("api/tour/" + id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }

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
