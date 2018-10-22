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
        public async Task<ActionResult> Index(int? tId, string recentAction, string recentNameT, string recentIdT, string recentNameA, string recentIdA)
        {
            if (tId.HasValue == false)
            {
                return RedirectToAction("Index", "Tours");
            }
            //Pass through most recent action details if redirected from an action
            if (recentAction != null && recentAction.Count() > 0)
            {
                ViewBag.Action = recentAction;
                ViewBag.RecentNameT = recentNameT;
                ViewBag.RecentIdT = recentIdT;
                ViewBag.RecentNameA = recentNameA;
                ViewBag.RecentIdA = recentIdA;
            }
            var request = new HTTPrequest();
            ViewBag.tourId = tId;
            TourDto tour = await request.Get<TourDto>("api/tour/"+tId);
            ViewBag.tourName = tour.Name;
            List<TourArtefactDto> toursArtefactsMasterlist = await request.Get<List<TourArtefactDto>>("api/tourArtefact?pageNumber=0&numPerPage=999999&isDeleted=false");
            List<TourArtefactDto> tourArtefactsFiltered = toursArtefactsMasterlist.Where(m => m.Tour.Id.ToString() == tour.Id.ToString()).ToList();

            //viewModel = await request.Get<List<TourDto>>("api/tour?tourId=" + tourId + "&pageNumber=0&numPerPage=500&isDeleted=false");
            List<TourArtefactDto> tourArtefactList = tourArtefactsFiltered.OrderBy(m => m.Order).ToList();
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
            // Checks Order is not Negative or Empty
            if (tourArtefact.Order < 0 || tourArtefact.Order == null)
            {
                ViewBag.OrderValidation = "Order must be a positive integer.";
                return View(tourArtefact);
            }
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

            if (tourCheck.Artefacts.Any(m => m.Order == tourArtefact.Order) && tourCheck.Artefacts.Any(m => m.Id == tourArtefact.Artefact.Id))
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
            return RedirectToAction("Index", "ToursArtefacts", new { tId = newTourArtefact.Tour.Id, recentAction = "Created", recentNameT = newTourArtefact.Tour.Name, recentIdT = newTourArtefact.Tour.Id, recentNameA = newTourArtefact.Artefact.Name, recentIdA = newTourArtefact.Artefact.Id });
        }

        // GET: TourDtoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            bool tour_Selected = id.HasValue;
            ViewBag.TourSelected = tour_Selected;

            var request = new HTTPrequest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourArtefactDto tourArtefact = await request.Get<TourArtefactDto>("api/tourArtefact/" + id);

            if (tourArtefact == null)
            {
                return HttpNotFound();
            }


            //TOUR CATEGORY 
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            tourDropdown = await PopulateTourDropdown(tour_Selected, tourArtefact.Tour.Id);
            ViewBag.TourList = tourDropdown;

            //GET Artefact and Pass through ViewBag
            var requestArtefact = new HTTPrequest();
            ArtefactSimpleDto artefact = await requestArtefact.Get<ArtefactSimpleDto>("api/artefact/" + tourArtefact.Artefact.Id);
            ViewBag.artefact = artefact;


            //ARTEFACT CATEGORY DROPDOWN
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            artefactDropdown = await PopulateArtefactDropdown();
            ViewBag.ArtefactList = artefactDropdown;


            return View(tourArtefact);
        }

        // POST: TourDtoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TourArtefactDto tourArtefact, int? tourId)
        {
            // Checks Order is not Negative or Empty
            if (tourArtefact.Order < 0 || tourArtefact.Order == null)
            {
                ViewBag.OrderValidation = "Order must be a positive integer.";
                return View(tourArtefact);
            }
            var request = new HTTPrequest();

            bool tour_Selected = tourId.HasValue;
            //TOUR CATEGORY 
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            if (tour_Selected)
            {
                tourDropdown = await PopulateTourDropdown(tour_Selected, tourId);
            }
            else
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
          
            TourArtefactDto newTourArtefact = await request.Get<TourArtefactDto>("api/tourArtefact/" + tourArtefact.Id.ToString());
            TourSimpleDto tour = await tourRequest.Get<TourSimpleDto>("api/tour/" + tourArtefact.Tour.Id);

            TourArtefactDto tourArtefactCheck = await tourRequest.Get<TourArtefactDto>("api/tourArtefact/" + tourArtefact.Id);

            if (tourCheck.Artefacts.Any(m => m.Artefact.Id != tourArtefact.Artefact.Id && m.Order != tourArtefact.Order))
            {
                ViewBag.IndexAvail = false;
                return View(newTourArtefact);

            }
            if (tourCheck.Artefacts.Any(m => m.Order == tourArtefact.Order))
            {
                ViewBag.IndexAvail = false;
                return View(newTourArtefact);
            }
            if (tourArtefact.Order == tourArtefactCheck.Order)
            {
                ViewBag.IndexAvail = true;
            }

            if (ModelState.IsValid)
            {
                newTourArtefact.Id = tourArtefact.Id;
                newTourArtefact.Tour = tour;
                newTourArtefact.Order = tourArtefact.Order;
                newTourArtefact.Artefact = await tourRequest.Get<ArtefactSimpleDto>("api/artefact/" + tourArtefact.Artefact.Id);
                newTourArtefact.ModifiedDate = DateTime.Now;
                newTourArtefact = await request.Put<TourArtefactDto>("api/tourArtefact", newTourArtefact);
                return RedirectToAction("Index", "ToursArtefacts", new { tId = tourArtefact.Tour.Id, recentAction = "Editted", recentNameT = newTourArtefact.Tour.Name, recentIdT = newTourArtefact.Tour.Id, recentNameA = newTourArtefact.Artefact.Name, recentIdA = newTourArtefact.Artefact.Id });
            }
            return View(newTourArtefact);
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
                return RedirectToAction("Index", "ToursArtefacts", new { tId = tourId, recentAction = "Deleted", recentNameT = tourArtefact.Tour.Name, recentIdT = tourArtefact.Tour.Id, recentNameA = tourArtefact.Artefact.Name, recentIdA = tourArtefact.Artefact.Id });

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
