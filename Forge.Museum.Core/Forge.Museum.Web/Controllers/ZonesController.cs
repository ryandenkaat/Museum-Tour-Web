using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.Web.Models;
using Forge.Museum.BLL.Http;

namespace Forge.Museum.Web.Controllers
{
    public class ZonesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Zones
        public async Task<ActionResult> Index()
        {
            var request = new HTTPrequest();

            List<ZoneDto> viewModel = await request.Get<List<ZoneDto>>("api/zone?pageNumber=0&numPerPage=500&isDeleted=false");

            return View(viewModel);
        }

        // GET: Zones/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            ZoneDto zone = await request.Get<ZoneDto>("api/zone/" + id);
            if (zone == null)
            {
                return HttpNotFound();
            }
            return View(zone);
        }

        // GET: Zones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Zones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ZoneDto zone)
        {
            if (ModelState.IsValid)
            {
                var request = new HTTPrequest();

                zone = await request.Post<ZoneDto>("api/zone", zone);

            }
            return RedirectToAction("Index");

        }

        // GET: Zones/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var request = new HTTPrequest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZoneDto zone = await request.Get<ZoneDto>("api/zone/" + id);
            if (zone == null)
            {
                return HttpNotFound();
            }
            return View(zone);
        }

        // POST: Zones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ZoneDto zone)
        {
            var request = new HTTPrequest();
            if (ModelState.IsValid)
            {
                await request.Put<ArtefactCategoryDto>("api/zone", zone);
                return RedirectToAction("Index");
            }
            return View(zone);
        }

        // GET: Zones/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            ZoneDto zone = await request.Get<ZoneDto>("api/zone/" + id);
            if (zone == null)
            {
                return HttpNotFound();
            }
            return View(zone);
        }

        // POST: Zones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var request = new HTTPrequest();
                await request.Delete("api/zone/" + id);
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
