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
using PagedList;


namespace Forge.Museum.Web.Controllers
{
    [Authorize]
    public class ZonesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Zones
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page, string recentAction, string recentName, string recentId)
        {
            var request = new HTTPrequest();
            //Pass through most recent action details if redirected from an action
            if (recentAction != null && recentAction.Count() > 0)
            {
                ViewBag.Action = recentAction;
                ViewBag.RecentName = recentName;
                ViewBag.RecentId = recentId;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            List<ZoneDto> zoneMasterList = await request.Get<List<ZoneDto>>("api/zone?pageNumber=0&numPerPage=500&isDeleted=false");
            IEnumerable<ZoneDto> zonesFiltered = zoneMasterList.ToList();

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
                zonesFiltered = zonesFiltered.Where(a => a.Name.Contains(searchString));
            }


            var zones = zonesFiltered;

            switch (sortOrder)
            {
                case "id_desc":
                    zones = zones.OrderByDescending(a => a.Id);
                    break;
                case "Name":
                    zones = zones.OrderBy(a => a.Name);
                    break;
                case "name_desc":
                    zones = zones.OrderByDescending(a => a.Name);
                    break;
                default:
                    zones = zones.OrderBy(a => a.Id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(zones.ToPagedList(pageNumber, pageSize));
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
        public async Task<ActionResult> Create()
        {
            var request = new HTTPrequest();
            return View();
        }

        // POST: Zones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ZoneDto zone)
        {
            // Checks Name is not Null or Empty
            if (string.IsNullOrEmpty(zone.Name))
            {
                ViewBag.ValidationName = "Name field is required.";
                return View(zone);
            }

            if (ModelState.IsValid && (zone.Name != null))
            {
                var request = new HTTPrequest();
                zone = await request.Post<ZoneDto>("api/zone", zone);
                return RedirectToAction("Index", new { recentAction = "Created", recentName = zone.Name, recentId = zone.Id });
            } else
            {
                var request = new HTTPrequest();
                ViewBag.Action = null;

                return View();
            }
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
            // Checks Name is not Null or Empty
            if (string.IsNullOrEmpty(zone.Name))
            {
                ViewBag.ValidationName = "Name field is required.";
                return View(zone);
            }

            var request = new HTTPrequest();
            if (ModelState.IsValid)
            {
                await request.Put<ArtefactCategoryDto>("api/zone", zone);
                return RedirectToAction("Index", new { recentAction = "Editted", recentName = zone.Name, recentId = zone.Id });
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
                ZoneDto zone = await request.Get<ZoneDto>("api/zone/" + id);
                await request.Delete("api/zone/" + id);
                return RedirectToAction("Index", new { recentAction = "Deleted", recentName = zone.Name, recentId = zone.Id });
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
