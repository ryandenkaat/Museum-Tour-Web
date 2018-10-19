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
using Forge.Museum.Interfaces.DataTransferObjects.Store;
using Forge.Museum.Web.Models;
using PagedList;
using System.IO;
using Forge.Museum.Web.Controllers;

namespace Forge.Museum.Web.Controllers {
    [Authorize]
    public class StoreItemsController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Artefacts
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
            ViewBag.CostSortParm = sortOrder == "Cost" ? "cost_desc" : "Cost";
            ViewBag.StockedSortParm = sortOrder == "Stocked" ? "stocked_desc" : "Stocked";
            List<StoreItemDto> storeItemsMasterList = await request.Get<List<StoreItemDto>>("api/storeItem?pageNumber=0&numPerPage=9999&isDeleted=false");
            IEnumerable<StoreItemDto> storeItemsFiltered = storeItemsMasterList.ToList();
        

            if(searchString != null)
            {
                page = 1;
            } else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;


            if (!String.IsNullOrEmpty(searchString))
            {
                storeItemsFiltered = storeItemsFiltered.Where(a => a.Name.Contains(searchString));
            }

            var storeItems = storeItemsFiltered;


            switch (sortOrder)
            {
                case "id_desc":
                    storeItems = storeItems.OrderByDescending(a => a.Id);
                    break;
                case "Name":
                    storeItems = storeItems.OrderBy(a => a.Name);
                    break;
                case "name_desc":
                    storeItems = storeItems.OrderByDescending(a => a.Name);
                    break;
                case "Cost":
                    storeItems = storeItems.OrderBy(a => a.Cost);
                    break;
                case "cost_desc":
                    storeItems = storeItems.OrderByDescending(a => a.Cost);
                    break;
                case "Stocked":
                    storeItems = storeItems.OrderBy(a => a.InStock);
                    break;
                case "stocked_desc":
                    storeItems = storeItems.OrderByDescending(a => a.InStock);
                    break;
                default:
                    storeItems = storeItems.OrderBy(a => a.Id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(storeItems.ToPagedList(pageNumber, pageSize));
        }


        // GET: Artefacts/Details/5
        public async Task<ActionResult> Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            StoreItemDto storeItem = await request.Get<StoreItemDto>("api/storeItem/"+id);
            if (storeItem == null) {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // GET: Artefacts/Create
        public ActionResult Create()
        {
            var request = new HTTPrequest();

            return View();
        }

        // POST: Artefacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StoreItemDto storeItem, HttpPostedFileBase imageFile) {
            // Checks Name is not Null or Empty
            if (string.IsNullOrEmpty(storeItem.Name))
            {
                ViewBag.ValidationName = "Name field is required.";
                return View(storeItem);
            }
            StoreItemDto newStoreItem = new StoreItemDto();
            newStoreItem.Id = storeItem.Id;
            newStoreItem.Name = storeItem.Name;
            newStoreItem.Description = storeItem.Description;
            newStoreItem.ModifiedDate = storeItem.ModifiedDate;
            newStoreItem.InStock = storeItem.InStock;
            newStoreItem.Cost = storeItem.Cost;

            if (ModelState.IsValid) {
                var request = new HTTPrequest();
                storeItem = await request.Post<StoreItemDto>("api/storeItem", storeItem);
                return RedirectToAction("Index", new { recentAction = "Created", recentName = storeItem.Name, recentId = storeItem.Id });
            } else {
                var request = new HTTPrequest();
                return View();
            }
        }

        // GET: Artefacts/Edit/5
        public async Task<ActionResult> Edit(int? id) {
            var request = new HTTPrequest();
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StoreItemDto storeItem = await request.Get<StoreItemDto>("api/storeItem/" + id);
            if (storeItem == null) {
                return HttpNotFound();
            }

            return View(storeItem);
        }

        // POST: Artefacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StoreItemDto storeItem, HttpPostedFileBase imageFile) {
            // Checks Name is not Null or Empty
            if (string.IsNullOrEmpty(storeItem.Name))
            {
                ViewBag.ValidationName = "Name field is required.";
                return View(storeItem);
            }
            var request = new HTTPrequest();
            StoreItemDto storeItem_editted = await request.Get<StoreItemDto>("api/storeItem/" + storeItem.Id);
            if (ModelState.IsValid) {
                storeItem_editted.Name = storeItem.Name;
                storeItem_editted.Description = storeItem.Description;
                storeItem_editted.ModifiedDate = storeItem.ModifiedDate;
                storeItem_editted.InStock = storeItem.InStock;
                storeItem_editted.Cost = storeItem.Cost;
                storeItem_editted.ModifiedDate = DateTime.Now;

                await request.Put<StoreItemDto>("api/storeItem", storeItem_editted);
                return RedirectToAction("Index", new { recentAction = "Editted", recentName = storeItem.Name, recentId = storeItem.Id });
            }
            return View(storeItem);
        }

        // GET: Artefacts/Delete/5
        public async Task<ActionResult> Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();

            StoreItemDto storeItem = await request.Get<StoreItemDto>("api/storeItem/" + id);
            if (storeItem == null) {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // POST: Artefacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> DeleteConfirmed(int id) {
            try {
                var request = new HTTPrequest();
                StoreItemDto storeItem = await request.Get<StoreItemDto>("api/storeItem/" + id);

                await request.Delete("api/storeItem/" + id);
                return RedirectToAction("Index", new { recentAction = "Deleted", recentName = storeItem.Name, recentId = storeItem.Id });
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
