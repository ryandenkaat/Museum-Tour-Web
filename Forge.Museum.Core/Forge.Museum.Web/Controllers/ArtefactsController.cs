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
using System.IO;

namespace Forge.Museum.Web.Controllers {
    public class ArtefactsController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Artefacts
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page) {
            var request = new HTTPrequest();
            List<ArtefactDto> viewModel = await request.Get<List<ArtefactDto>>("api/artefact?pageNumber=0&numPerPage=500&isDeleted=false");
            return View(viewModel);
        }

        // GET: Artefacts/Details/5
        public async Task<ActionResult> Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            ArtefactDto artefact = await request.Get<ArtefactDto>("api/artefact/"+id);
            if (artefact == null) {
                return HttpNotFound();
            }
            return View(artefact);
        }

        // GET: Artefacts/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Artefacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ArtefactDto artefact, HttpPostedFileBase imageFile) {
            if (ModelState.IsValid) {
                var request = new HTTPrequest();
                HttpPostedFileBase imgFile = Request.Files["ImageFile"];
                if (imgFile != null) {
                    artefact.Image = new byte[imgFile.ContentLength];
                    imgFile.InputStream.Read(artefact.Image, 0, imgFile.ContentLength);
                }
                    artefact = await request.Post<ArtefactDto>("api/artefact", artefact);
                    return RedirectToAction("Index");
                }
                return View(artefact);
        }

        // GET: Artefacts/Edit/5
        public async Task<ActionResult> Edit(int? id) {
            var request = new HTTPrequest();
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtefactDto artefact = await request.Get<ArtefactDto>("api/artefact/" + id);
            if (artefact == null) {
                return HttpNotFound();
            }
            return View(artefact);
        }

        // POST: Artefacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ArtefactDto artefact) {
            var request = new HTTPrequest();
            ArtefactDto artefact_editted = await request.Get<ArtefactDto>("api/artefact/" + artefact.Id);
            if (ModelState.IsValid) {
                artefact_editted.Name = artefact.Name;
                artefact_editted.Description = artefact.Description;
                artefact_editted.Measurement_Length = artefact.Measurement_Length;
                artefact_editted.Measurement_Height = artefact.Measurement_Height;
                artefact_editted.Measurement_Width = artefact.Measurement_Width;
                artefact_editted.AdditionalComments = artefact.AdditionalComments;
                await request.Put<ArtefactDto>("api/artefact", artefact_editted);
                return RedirectToAction("Index");
            }
            return View(artefact);
        }

        // GET: Artefacts/Delete/5
        public async Task<ActionResult> Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();

            ArtefactDto artefact = await request.Get<ArtefactDto>("api/artefact/" + id);
            if (artefact == null) {
                return HttpNotFound();
            }
            return View(artefact);
        }

        // POST: Artefacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> DeleteConfirmed(int id) {
            try {
                var request = new HTTPrequest();
                await request.Delete("api/artefact/" + id);
                return RedirectToAction("Index");
            }
            catch (Exception) {

                throw;
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
