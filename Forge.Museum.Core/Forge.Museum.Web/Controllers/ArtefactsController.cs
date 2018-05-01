
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
using Forge.Museum.Web.Models;
using PagedList;

namespace Forge.Museum.Web.Controllers
{
    public class ArtefactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Artefacts
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var request = new HTTPrequest();
            //request.Get<PagedList<ArtefactDto>>("api/artefact?pageNumber=1&numPerPage=500&isDeleted=false");
            List<ArtefactDto> viewModel = await request.Get<List<ArtefactDto>>("api/artefact?pageNumber=1&numPerPage=500&isDeleted=false");
            
            //return View(viewModel.ToPagedList<ArtefactDto>(pageNumber, pageSize));
            return View(viewModel);
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
        public async Task<ActionResult> Create(string name, string description, string additionalComments, DateTime acquistionDate, int length, int width, int height)
        {
            var request = new HTTPrequest();
            ArtefactDto dto = new ArtefactDto()
            {
                Name = name,
                Description = description,
                AdditionalComments = additionalComments,
                AcquisitionDate = acquistionDate,
                Measurement_Length = length,
                Measurement_Height = height,
                Measurement_Width = width,
            };

            dto = await request.Post<ArtefactDto>("api/artefact", dto);

            return View(dto);
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var request = new HTTPrequest();
                //ArtefactDto dto = await request.Get<ArtefactDto>("api/artefact/" + id);
                await request.Delete("api/artefact/" + id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
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
