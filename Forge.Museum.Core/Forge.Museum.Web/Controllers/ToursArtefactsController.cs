﻿using System;
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
            List<TourDto> toursList = await request.Get<List<TourDto>>("api/artefact?pageNumber=0&numPerPage=5-0&isDeleted=false");
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            TourDto tour = new TourDto();
            if (tourSelected == true)
            {
                tour = await request.Get<TourDto>("api/tour/" + tourId);
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
            var request = new HTTPrequest();
            ViewBag.tourId = tourId;
            TourDto tour = await request.Get<TourDto>("api/tour/"+tourId);
            ViewBag.tourName = tour.Name;


            //viewModel = await request.Get<List<TourDto>>("api/tour?tourId=" + tourId + "&pageNumber=0&numPerPage=500&isDeleted=false");
            List<TourArtefactDto> artefactList = tour.Artefacts;
            ViewBag.artefactsList = artefactList;

            List<TourArtefactDto> viewModel = artefactList;
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
            ViewBag.TourID = tourId.ToString();


            //TOUR CATEGORY 
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            tourDropdown = await PopulateTourDropdown(tour_Selected, tourId);
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
        public async Task<ActionResult> Create(TourArtefactDto tourArtefact, TourDto tour, int? tourId, ArtefactSimpleDto artefact)
        {
            //TOUR CATEGORY 
            List<SelectListItem> tourDropdown = new List<SelectListItem>();
            tourDropdown = await PopulateTourDropdown(true, tourId);
            ViewBag.TourList = tourDropdown;
            //ARTEFACT DROPDOWN
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            artefactDropdown = await PopulateArtefactDropdown();
            ViewBag.ArtefactList = artefactDropdown;

            ViewBag.TourID = tourId.ToString();

           var tourRequest = new HTTPrequest();
           TourDto tourCheck = await tourRequest.Get<TourDto>("api/tour/" + tourArtefact.Tour.Id);

            TourArtefactDto newTourArtefact = new TourArtefactDto();


            if (ModelState.IsValid)
            {
                newTourArtefact.Id = tourArtefact.Id;
                newTourArtefact.Tour = tourArtefact.Tour;
                newTourArtefact.Order = tourArtefact.Order;
                newTourArtefact.CreatedDate = DateTime.Now;
                newTourArtefact.ModifiedDate = DateTime.Now;
                if (tourCheck.Artefacts.Any(m => m.Id != tourArtefact.Id && m.Order != tourArtefact.Order))
                {
                    var request = new HTTPrequest();


                    await request.Post<TourArtefactDto>("api/tourArtefact", newTourArtefact);

                    return RedirectToAction("Index");
                }
            }
            return View(newTourArtefact);

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
        public async Task<ActionResult> Delete(int? artefactId, int? tourId)
        {
            bool tour_Selected = tourId.HasValue;
            ViewBag.TourSelected = tour_Selected;
            ViewBag.originalArtefactId = artefactId;
            ViewBag.ArtefactId = artefactId;
            if (tourId == null || artefactId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //GET Tour and Pass Through ViewBag
            var request = new HTTPrequest();
            TourDto tour = await request.Get<TourDto>("api/tour/" + tourId);
            if (tour == null)
            {
                return HttpNotFound();
            } else
            {
                ViewBag.tour = tour;
                ViewBag.tourId = tour.Id;
                ViewBag.tourName = tour.Name;
            }


            //GET Artefact and Pass through ViewBag
            var requestArtefact = new HTTPrequest();
            ArtefactSimpleDto artefact = await requestArtefact.Get<ArtefactSimpleDto>("api/artefact/" + artefactId);
            if (artefact == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.artefact = artefact;
                ViewBag.artefactId = artefact.Id;
                ViewBag.artefactName = artefact.Name;
            }
            return View(tour);


        }



        // POST: TourDtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? tourId, int? artefactId)
        {
            try
            {
                var request = new HTTPrequest();
                TourDto tour = await request.Get<TourDto>("api/tour/" + tourId);
                ArtefactSimpleDto artefact = await request.Get<ArtefactSimpleDto>("api/artefact/" + artefactId);
                List<TourArtefactDto> artefactList = tour.Artefacts;

                int index = artefactList.FindIndex(a => a.Id == artefact.Id);
                artefactList.RemoveAt(index);
                tour.Artefacts = artefactList;
                await request.Put<TourDto>("api/tour", tour);
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
