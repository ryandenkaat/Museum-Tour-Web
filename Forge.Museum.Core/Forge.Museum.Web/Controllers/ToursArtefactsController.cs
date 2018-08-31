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
    public class ToursArtefactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<List<SelectListItem>> PopulateArtefactDropdown()
        {
            var request = new HTTPrequest();
            List<ArtefactSimpleDto> artefacts = await request.Get<List<ArtefactSimpleDto>>("api/artefact?pageNumber=0&numPerPage=5-0&isDeleted=false");
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            if (artefacts != null && artefacts.Any())
            {
                foreach (var category in artefacts)
                {
                    artefactDropdown.Add(new SelectListItem()
                    {
                        Text = category.Id + ": " + category.Name,
                        Value = category.Id.ToString()
                    });
                }
            }
            return artefactDropdown;
        }

        // GET: TourDtoes
        public async Task<ActionResult> Index(int? tourId)
        {
            var request = new HTTPrequest();
            ViewBag.tourId = tourId;

            TourDto tour = await request.Get<TourDto>("api/tour/"+tourId);
            //viewModel = await request.Get<List<TourDto>>("api/tour?tourId=" + tourId + "&pageNumber=0&numPerPage=500&isDeleted=false");


            List<ArtefactSimpleDto> viewModel = tour.Artefacts;
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
        public async Task<ActionResult> Create(int? tourId)
        {
            bool tour_Selected = tourId.HasValue;
            ViewBag.TourSelected = tour_Selected;
            var request = new HTTPrequest();
            List<TourDto> tourList = new List<TourDto>();
            TourDto tour = new TourDto();
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            // Checks if page was access from Index of all MediaFiles, or Direct from a particular artefact
            if (tour_Selected == true)
            {
                 tour = await request.Get<TourDto>("api/tour/" + tourId);
                if (tour == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    tourDropdown.Add(new SelectListItem()
                    {
                        Text = tour.Name,
                        Value = tour.Id.ToString()

                    });
                    ViewBag.TourName = tour.Name;
                    ViewBag.TourID = tour.Id.ToString();
                }
            }
            ViewBag.tourList = tourDropdown;
            //ARTEFACT CATEGORY DROPDOWN
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            artefactDropdown = await PopulateArtefactDropdown();
            ViewBag.ArtefactList = artefactDropdown;
            return View();
        }

        // POST: TourDtoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TourDto tour, int? tourId, ArtefactSimpleDto artefact)
        {
            var request = new HTTPrequest();

            string tourID = ViewBag.SelectedTourID;
            tour = await request.Get<TourDto>("api/tour/"+tourId);
            ArtefactSimpleDto newTourArtefact = await request.Get<ArtefactSimpleDto>("api/artefact/" + artefact.Id.ToString());
            //List<ArtefactSimpleDto> = 
            tour.Artefacts.Insert(0, newTourArtefact);
           // tour.Artefacts.Add(newTourArtefact);
            if(tourId != null && artefact.Id != null)
            {
                var request2 = new HTTPrequest();

                tour = await request2.Put<TourDto>("api/tour", tour);
                //    return RedirectToAction("Index");
                return RedirectToAction("Index", "Tours");

            }
            //return View(tour);
            return RedirectToAction("Index", "Tours");

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
