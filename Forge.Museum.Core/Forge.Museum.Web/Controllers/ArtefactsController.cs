
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forge.Museum.Web.Models;
using PagedList;

namespace Forge.Museum.Web.Controllers
{
    public class ArtefactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Artefacts
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var artefacts = from a in db.Artefacts
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                artefacts = artefacts.Where(s => s.ArtefactDescription.Contains(searchString)
                                       || s.ArtefactAdditionalComments.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    artefacts = artefacts.OrderByDescending(a => a.ArtefactDescription);
                    break;
                case "Date":
                    artefacts = artefacts.OrderBy(a => a.AcquisitionDate);
                    break;
                case "date_desc":
                    artefacts = artefacts.OrderByDescending(a => a.AcquisitionDate);
                    break;
                default:
                    artefacts = artefacts.OrderBy(s => s.ArtefactDescription);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(artefacts.ToPagedList(pageNumber, pageSize));
            return View(artefacts.ToList());
        }

        // GET: Artefacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artefact artefact = db.Artefacts.Find(id);
            if (artefact == null)
            {
                return HttpNotFound();
            }
            return View(artefact);
        }

        // GET: Artefacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artefacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtefactID,ArtefactDescription,ArtefactAdditionalComments,AcquisitionDate,ArtefactMeasurement_Length,ArtefactMeasurement_Width,ArtefactMeasurement_Height")] Artefact artefact)
        {
            if (ModelState.IsValid)
            {
                db.Artefacts.Add(artefact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artefact);
        }

        // GET: Artefacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artefact artefact = db.Artefacts.Find(id);
            if (artefact == null)
            {
                return HttpNotFound();
            }
            return View(artefact);
        }

        // POST: Artefacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtefactID,ArtefactDescription,ArtefactAdditionalComments,AcquisitionDate,ArtefactMeasurement_Length,ArtefactMeasurement_Width,ArtefactMeasurement_Height")] Artefact artefact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artefact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artefact);
        }

        // GET: Artefacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artefact artefact = db.Artefacts.Find(id);
            if (artefact == null)
            {
                return HttpNotFound();
            }
            return View(artefact);
        }

        // POST: Artefacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artefact artefact = db.Artefacts.Find(id);
            db.Artefacts.Remove(artefact);
            db.SaveChanges();
            return RedirectToAction("Index");
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
