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
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            List<ArtefactDto> artefactsMasterList = await request.Get<List<ArtefactDto>>("api/artefact?pageNumber=0&numPerPage=999999&isDeleted=false");
            IEnumerable<ArtefactDto> artefactsFiltered = artefactsMasterList.ToList();
        

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
                artefactsFiltered = artefactsFiltered.Where(a => a.Name.Contains(searchString));
            }

            var artefacts = artefactsFiltered;


            switch (sortOrder)
            {
                case "id_desc":
                    artefacts = artefacts.OrderByDescending(a => a.Id);
                    break;
                case "Name":
                    artefacts = artefacts.OrderBy(a => a.Name);
                    break;
                case "name_desc":
                    artefacts = artefacts.OrderByDescending(a => a.Name);
                    break;
                case "Date":
                    artefacts = artefacts.OrderBy(a => a.AcquisitionDate);
                    break;
                case "date_desc":
                    artefacts = artefacts.OrderByDescending(a => a.AcquisitionDate);
                    break;
                case "Category":
                    artefacts = artefacts.OrderBy(a => a.ArtefactCategory.Name);
                    break;
                case "category_desc":
                    artefacts = artefacts.OrderByDescending(a => a.ArtefactCategory.Name);
                    break;
                default:
                    artefacts = artefacts.OrderBy(a => a.Id);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(artefacts.ToPagedList(pageNumber, pageSize));

            //return View(artefacts.ToList());
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
        public async Task<ActionResult> Create() {
            var request = new HTTPrequest();
            //TODO Add filter to this api call
            List<ArtefactCategorySimpleDto> artefactCateories = await request.Get<List<ArtefactCategorySimpleDto>>("api/artefactCatergory?pageNumber=0&numPerPage=5-0&isDeleted=false");

            List<SelectListItem> categoryDropdown = new List<SelectListItem>();

            if (artefactCateories != null && artefactCateories.Any())
            {
                foreach(var category in artefactCateories)
                {
                    categoryDropdown.Add(new SelectListItem()
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
                }
            }

            ViewBag.CategoryList = categoryDropdown;

            return View();
        }

        // POST: Artefacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ArtefactDto artefact, HttpPostedFileBase imageFile) {
            ArtefactDto newArtefact = new ArtefactDto();
            newArtefact.Id = artefact.Id;
            newArtefact.Name = artefact.Name;
            newArtefact.Description = artefact.Description;
            newArtefact.ModifiedDate = artefact.ModifiedDate;
            newArtefact.Measurement_Height = artefact.Measurement_Height;
            newArtefact.Measurement_Length = artefact.Measurement_Length;
            newArtefact.Measurement_Width = artefact.Measurement_Width;
            newArtefact.ArtefactCategory = artefact.ArtefactCategory;

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
