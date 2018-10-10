using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forge.Museum.API.Models;
using Forge.Museum.Web.Models;
using Forge.Museum.Interfaces.DataTransferObjects.Store;
using Forge.Museum.BLL.Http;
using System.IO;
using System.Net.Http;
using PagedList;

namespace Forge.Museum.Web.Controllers
{
    [Authorize]
    public class StoreItemImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: storeItemImage
        public async Task<ActionResult> Index(int? storeItemId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            var requestStoreItem = new HTTPrequest();
            var requestStoreItemImageList = new HTTPrequest();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "description_desc" : "Description";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";

            ViewBag.StoreItemID = storeItemId;
            // Retrives a list of All Artefacts if user has not navaigated through a particular Artefact Id 
            List<StoreItemImageDto> storeItemImageMasterList = new List<StoreItemImageDto>();
            if (storeItemId == null)
            {
                storeItemImageMasterList = await requestStoreItemImageList.Get<List<StoreItemImageDto>>("api/storeItemImage?pageNumber=0&numPerPage=500&isDeleted=false");
            } else
            {
                storeItemImageMasterList = await requestStoreItemImageList.Get<List<StoreItemImageDto>>("api/storeItemImage?storeItemId=" + storeItemId + "&pageNumber=0&numPerPage=500&isDeleted=false");
                StoreItemDto storeItem = await requestStoreItem.Get<StoreItemDto>("api/storeItem/" + storeItemId);
                ViewBag.StoreItemName = storeItem.Name;
            }
            IEnumerable<StoreItemImageDto> storeItemImagesFiltered = storeItemImageMasterList.ToList();

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
                storeItemImagesFiltered = storeItemImagesFiltered.Where(a => a.Id.Equals(searchString));
            }

            var storeItemsContent = storeItemImagesFiltered;

            switch (sortOrder)
            {
                case "id_desc":
                    storeItemsContent = storeItemsContent.OrderByDescending(a => a.Id);
                    break;
                default:
                    storeItemsContent = storeItemsContent.OrderBy(a => a.Id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(storeItemsContent.ToPagedList(pageNumber, pageSize));
        }

        // GET: storeItemImage/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();

            StoreItemImageDto storeItemImage = await request.Get<StoreItemImageDto>("api/storeItemImage/" + id);
            if (storeItemImage == null)
            {
                return HttpNotFound();
            }
            return View(storeItemImage);
        }

        // GET: storeItemImage/Create
        public async Task<ActionResult> Create(int? storeItemId)
        {
            bool storeItem_Selected = storeItemId.HasValue;
            ViewBag.StoreItemSelected = storeItem_Selected;
            var request = new HTTPrequest();
            List<StoreItemSimpleDto> storeItemsList = new List<StoreItemSimpleDto>();
            StoreItemSimpleDto storeItem = new StoreItemSimpleDto();
            List<SelectListItem> storeItemDropdown = new List<SelectListItem>();
            // Checks if page was access from Index of all MediaFiles, or Direct from a particular storeItem
            if (storeItem_Selected == true)
            {
                storeItem = await request.Get<StoreItemSimpleDto>("api/storeItem/" + storeItemId);
                if (storeItem == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    storeItemDropdown.Add(new SelectListItem()
                    {
                        Text = storeItem.Name,
                        Value = storeItem.Id.ToString()

                    });
                    ViewBag.StoreItemName = storeItem.Name;
                    ViewBag.StoreItemID = storeItem.Id.ToString();

                }
            } else {
                storeItemsList = await request.Get<List<StoreItemSimpleDto>>("api/storeItem?pageNumber=0&numPerPage=5-0&isDeleted=false");
                if (storeItemsList != null && storeItemsList.Any())
                {
                    foreach (var storeItems in storeItemsList)
                    {
                        storeItemDropdown.Add(new SelectListItem()
                        {
                            Text = storeItems.Id + ":" + storeItems.Name,
                            Value = storeItems.Id.ToString()
                        });
                    }
                }
            }

            ViewBag.StoreItemList = storeItemDropdown;
            return View();
        }

        // POST: storeItemImage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StoreItemImageDto storeItemImage, HttpPostedFileBase storeItemImageFile)
        {

            StoreItemImageDto newstoreItemImage = new StoreItemImageDto();
            newstoreItemImage.Id = storeItemImage.Id;
            newstoreItemImage.StoreItem = storeItemImage.StoreItem;
            newstoreItemImage.CreatedDate = DateTime.Now;
            newstoreItemImage.ModifiedDate = DateTime.Now;

            if (ModelState.IsValid) { 
                var request = new HTTPrequest();
                //HttpPostedFileBase newFile = Request.Files["storeItemImageFile"];
                //HttpPostedFileBase newFile = storeItemImageFile;
                    if (storeItemImageFile != null && storeItemImageFile.ContentLength > 0)
                    {
                        storeItemImage.Image = new byte[storeItemImageFile.ContentLength];
                        storeItemImageFile.InputStream.Read(storeItemImage.Image, 0, storeItemImageFile.ContentLength);
                        string fileExtension = Path.GetExtension(storeItemImageFile.FileName);
                        storeItemImage.FileType = fileExtension;
                    if (storeItemImageFile == null) {
                        throw new ArgumentException("file");
                    }
                }

                await request.Post<StoreItemImageDto>("api/storeItemImage", storeItemImage);

                return RedirectToAction("Index");
            }
            return View(storeItemImage);
        }

        // GET: storeItemImage/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var request = new HTTPrequest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItemImageDto storeItemImage = await request.Get<StoreItemImageDto>("api/storeItemImage/" + id);

            if (storeItemImage == null)
            {
                return HttpNotFound();
            }
            return View(storeItemImage);
        }

        // POST: storeItemImage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StoreItemImageDto storeItemImage, HttpPostedFileBase storeItemImageFile)
        {
            var request = new HTTPrequest();
            StoreItemImageDto storeItemImage_editted = await request.Get<StoreItemImageDto>("api/storeItemImage/" + storeItemImage.Id.ToString());

            if (ModelState.IsValid)
            {
                if (storeItemImageFile != null)
                {
                    HttpPostedFileBase mediaFile = Request.Files["storeItemImageFile"];

                    storeItemImage_editted.Image = new byte[mediaFile.ContentLength];
                    mediaFile.InputStream.Read(storeItemImage_editted.Image, 0, mediaFile.ContentLength);
                    string fileExtension = Path.GetExtension(mediaFile.FileName);
                    storeItemImage_editted.FileType = fileExtension;
                }


                storeItemImage = await request.Put<StoreItemImageDto>("api/storeItemImage", storeItemImage_editted);
                return RedirectToAction("Index");
            }
            return View(storeItemImage);
        }

        // GET: storeItemImage/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            StoreItemImageDto storeItemImage = await request.Get<StoreItemImageDto>("api/storeItemImage/" + id);
            if (storeItemImage == null)
            {
                return HttpNotFound();
            }
            return View(storeItemImage);
        }

        // POST: storeItemImage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var request = new HTTPrequest();
                StoreItemImageDto storeItemImage = await request.Get<StoreItemImageDto>("api/storeItemImage/" + id);
                storeItemImage.IsDeleted = true;
                await request.Put<StoreItemImageDto>("api/storeItemImage", storeItemImage);
                await request.Delete("api/storeItemImage/" + id.ToString());
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
