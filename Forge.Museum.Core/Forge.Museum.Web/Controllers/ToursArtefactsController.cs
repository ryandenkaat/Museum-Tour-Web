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
                foreach (var artefact in artefacts)
                {
                    artefactDropdown.Add(new SelectListItem()
                    {
                        Text = artefact.Id + ": " + artefact.Name,
                        Value = artefact.Id.ToString()
                    });
                }
            }
            return artefactDropdown;
        }

        public async Task<List<SelectListItem>> PopulateTourDropdown(bool tourSelected, int? tourId)
        {
            var request = new HTTPrequest();
            List<TourSimpleDto> toursList = await request.Get<List<TourSimpleDto>>("api/tour?pageNumber=0&numPerPage=5-0&isDeleted=false");
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            TourSimpleDto tour = new TourSimpleDto();
            if (tourSelected == true)
            {
                tour = await request.Get<TourSimpleDto>("api/tour/" + tourId);
                if (tour != null)
                {
                    tourDropdown.Add(new SelectListItem()
                    {
                        Text = tour.Id + ":" + tour.Name,
                        Value = tour.Id.ToString()

                    });
                    ViewBag.TourName = tour.Name;
                    ViewBag.TourID = tour.Id.ToString();
                }
            } else
            {
                foreach (var t in toursList)
                {
                    tourDropdown.Add(new SelectListItem()
                    {
                        Text = t.Id + ": " + t.Name,
                        Value = t.Id.ToString()
                    });
                }
            }
            return tourDropdown;
        
        }
    

        // GET: TourDtoes
        public async Task<ActionResult> Index(int? tourId)
        {
            if(tourId.HasValue == false)
            {
                return RedirectToAction("Index", "Tours");

            }
            var request = new HTTPrequest();
            ViewBag.tourId = tourId;
            TourDto tour = await request.Get<TourDto>("api/tour/"+tourId);
            ViewBag.tourName = tour.Name;
            List<TourArtefactDto> toursArtefactsMasterlist = await request.Get<List<TourArtefactDto>>("api/tourArtefact?pageNumber=0&numPerPage=999999&isDeleted=false");
            

            //viewModel = await request.Get<List<TourDto>>("api/tour?tourId=" + tourId + "&pageNumber=0&numPerPage=500&isDeleted=false");
            List<TourArtefactDto> tourArtefactList = toursArtefactsMasterlist.OrderBy(m => m.Order).ToList();
            ViewBag.tourArtefactList = tourArtefactList;

            List<TourArtefactDto> viewModel = tourArtefactList;
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


            List<TourSimpleDto> toursList = new List<TourSimpleDto>();
            TourSimpleDto tour = new TourSimpleDto();
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            // Checks if page was access from Index of all MediaFiles, or Direct from a particular artefact
            if (tour_Selected == true)
            {
                tour = await request.Get<TourSimpleDto>("api/tour/" + tourId);
                if (tour == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    tourDropdown.Add(new SelectListItem()
                    {
                        Text = tour.Id + ": " + tour.Name,
                        Value = tour.Id.ToString()

                    });
                    ViewBag.TourName = tour.Name;
                    ViewBag.TourID = tour.Id.ToString();

                }
            }
            else
            {
                toursList = await request.Get<List<TourSimpleDto>>("api/tour?pageNumber=0&numPerPage=5-0&isDeleted=false");
                if (toursList != null && toursList.Any())
                {
                    foreach (var tours in toursList)
                    {
                        tourDropdown.Add(new SelectListItem()
                        {
                            Text = tours.Id + ": " + tours.Name,
                            Value = tours.Id.ToString()
                        });
                    }
                }
            }

            ViewBag.TourList = tourDropdown;
            //ARTEFACT DROPDOWN
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
        public async Task<ActionResult> Create(TourArtefactDto tourArtefact, int? tourId)
        {
            bool tour_Selected = tourId.HasValue;
            //TOUR CATEGORY 
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            if (tour_Selected)
            {
                tourDropdown = await PopulateTourDropdown(tour_Selected, tourId);
            } else
            {
                tourDropdown = await PopulateTourDropdown(tour_Selected, null);
            }
            ViewBag.TourList = tourDropdown;
            //ARTEFACT DROPDOWN
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            artefactDropdown = await PopulateArtefactDropdown();
            ViewBag.ArtefactList = artefactDropdown;

            ViewBag.TourID = tourId.ToString();

           var tourRequest = new HTTPrequest();
           TourDto tourCheck = await tourRequest.Get<TourDto>("api/tour/" + tourArtefact.Tour.Id);

            TourArtefactDto newTourArtefact = new TourArtefactDto();
            if (tourCheck.Artefacts.Any(m => m.Artefact.Id != tourArtefact.Artefact.Id && m.Order != tourArtefact.Order))
            {
                ViewBag.IndexAvail = false;
                return View(newTourArtefact);

            }
            if(tourCheck.Artefacts.Any(m => m.Order == tourArtefact.Order))
            {
                ViewBag.IndexAvail = false;
                return View(newTourArtefact);
            }

            newTourArtefact.Artefact = await tourRequest.Get<ArtefactSimpleDto>("api/artefact/" + tourArtefact.Artefact.Id);
                    TourSimpleDto tour = await tourRequest.Get<TourSimpleDto>("api/tour/" + tourArtefact.Tour.Id);
                    newTourArtefact.Tour = tour;        
                    newTourArtefact.Order = tourArtefact.Order;
                    newTourArtefact.CreatedDate = DateTime.Now;
                    newTourArtefact.ModifiedDate = DateTime.Now;

                    var request = new HTTPrequest();


                     await request.Post<TourArtefactDto>("api/tourArtefact", newTourArtefact);

            return RedirectToAction("Index", "ToursArtefacts", new { tourId = tour.Id });

        }

        // GET: TourDtoes/Edit/5
        public async Task<ActionResult> Edit(int? artefactId, int? tourId)
        {
            bool tour_Selected = tourId.HasValue;
            ViewBag.TourSelected = tour_Selected;
            ViewBag.originalArtefactId = artefactId;

            ViewBag.ArtefactId = artefactId;
            var request = new HTTPrequest();
            if (artefactId == null || tourId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourDto tour = await request.Get<TourDto>("api/tour/" + tourId);
            if (tour == null)
            {
                return HttpNotFound();
            }

            //TOUR CATEGORY 
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            tourDropdown = await PopulateTourDropdown(tour_Selected, tourId);
            ViewBag.TourList = tourDropdown;

            //GET Artefact and Pass through ViewBag
            var requestArtefact = new HTTPrequest();
            ArtefactSimpleDto artefact = await requestArtefact.Get<ArtefactSimpleDto>("api/artefact/" + artefactId);
            ViewBag.artefact = artefact;


            //ARTEFACT CATEGORY DROPDOWN
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            artefactDropdown = await PopulateArtefactDropdown();
            ViewBag.ArtefactList = artefactDropdown;


            return View(tour);
        }

        // POST: TourDtoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TourDto tour, int? tourId, ArtefactSimpleDto artefact, int? originalArtefactId)
        {
            var request = new HTTPrequest();
            TourDto tour_editted = await request.Get<TourDto>("api/tour/" + tourId);
            TourArtefactDto newArtefact = await request.Get<TourArtefactDto>("api/artefact/" + artefact.Id);

            TourArtefactDto originalArtefact = await request.Get<TourArtefactDto>("api/artefact/" + originalArtefactId);

            List<TourArtefactDto> artefactList = tour_editted.Artefacts;
            int index = artefactList.FindIndex(a => a.Id == originalArtefact.Id);
            artefactList.RemoveAt(index);
            artefactList.Insert(index, newArtefact);
            tour_editted.Artefacts = artefactList;
            await request.Put<TourDto>("api/tour", tour_editted);


            if (ModelState.IsValid) {
                tour_editted.Artefacts = artefactList;
                tour_editted.ModifiedDate = DateTime.Now;
                await request.Put<TourDto>("api/tour", tour_editted);
                return RedirectToAction("Index");
            }
            //return View(tour);
            return RedirectToAction("Index", "ToursArtefacts", new { tourId = tourId });

        }

        // GET: TourDtoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Tours");

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            TourArtefactDto tourArtefact = await request.Get<TourArtefactDto>("api/tourArtefact/" + id);
            ViewBag.TourArtefactId = id;
            if (tourArtefact == null)
            {
                return HttpNotFound();
            }
            return View(tourArtefact);


        }



        // POST: TourDtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
                try
                {
                    var request = new HTTPrequest();
                    TourArtefactDto tourArtefact = await request.Get<TourArtefactDto>("api/tourArtefact/" + id);
                    tourArtefact.IsDeleted = true;
                    int tourId = tourArtefact.Tour.Id;
                //    await request.Put<TourArtefactDto>("api/tourArtefact", tourArtefact);
                await request.Delete("api/tourArtefact/" + id.ToString());
                    return RedirectToAction("Index", "ToursArtefacts", new { tourId = tourId });
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
