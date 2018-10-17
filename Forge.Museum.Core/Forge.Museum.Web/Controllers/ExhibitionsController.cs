using Forge.Museum.BLL.Http;
using Forge.Museum.Interfaces.DataTransferObjects.Exhibition;
using Forge.Museum.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Forge.Museum.Web.Controllers
{
    [Authorize]
    public class ExhibitionsController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artefacts
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page, string recentAction, string recentName, string recentId) {
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
            List<ExhibitionDto> exhibitionMasterList = await request.Get<List<ExhibitionDto>>("api/exhibition?pageNumber=0&numPerPage=999999&isDeleted=false");
            IEnumerable<ExhibitionDto> exhibitionsFiltered = exhibitionMasterList.ToList();
        

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
                exhibitionsFiltered = exhibitionsFiltered.Where(a => a.Name.Contains(searchString));
            }

            var exhibitions = exhibitionsFiltered;


            switch (sortOrder)
            {
                case "id_desc":
                    exhibitions = exhibitions.OrderByDescending(a => a.Id);
                    break;
                case "Name":
                    exhibitions = exhibitions.OrderBy(a => a.Name);
                    break;
                case "name_desc":
                    exhibitions = exhibitions.OrderByDescending(a => a.Name);
                    break;
                default:
                    exhibitions = exhibitions.OrderBy(a => a.Id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(exhibitions.ToPagedList(pageNumber, pageSize));
        }


        // GET: Artefacts/Details/5
        public async Task<ActionResult> Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            ExhibitionDto exhibition = await request.Get<ExhibitionDto>("api/exhibition/"+id);
            if (exhibition == null) {
                return HttpNotFound();
            }
            return View(exhibition);
        }

        // GET: Artefacts/Create
        public async Task<ActionResult> Create() {
            var request = new HTTPrequest();
            return View();
        }

        // POST: Artefacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExhibitionDto exhibition, HttpPostedFileBase imageFile) {
            if (string.IsNullOrEmpty(exhibition.Name))
            {
                ViewBag.ValidationName = "Name field is required.";
                return View(exhibition);
            }
            ExhibitionDto newExhibition = new ExhibitionDto();
            newExhibition.Id = exhibition.Id;
            newExhibition.Name = exhibition.Name;
            newExhibition.Description = exhibition.Description;
            newExhibition.ModifiedDate = exhibition.ModifiedDate;
            newExhibition.StartDate = exhibition.StartDate;
            newExhibition.FinishDate = exhibition.FinishDate;
            newExhibition.Price_Adult = exhibition.Price_Adult;
            newExhibition.Price_Concession = exhibition.Price_Concession;
            newExhibition.Price_Child = exhibition.Price_Child;

            if (ModelState.IsValid) {
                var request = new HTTPrequest();
                HttpPostedFileBase imgFile = Request.Files["ImageFile"];
                if (imgFile != null) {
                    exhibition.Image = new byte[imgFile.ContentLength];
                    imgFile.InputStream.Read(exhibition.Image, 0, imgFile.ContentLength);
                    string fileExtension = Path.GetExtension(imgFile.FileName);
                    //exhibition.ImageFileType = fileExtension;
                }
                exhibition = await request.Post<ExhibitionDto>("api/exhibition", exhibition);
                return RedirectToAction("Index", new { recentAction = "Created", recentName = exhibition.Name, recentId = exhibition.Id });            }
            else {
                return View();
            }
        }

        // GET: Artefacts/Edit/5
        public async Task<ActionResult> Edit(int? id) {
            var request = new HTTPrequest();
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExhibitionDto exhibition = await request.Get<ExhibitionDto>("api/exhibition/" + id);
            if (exhibition == null) {
                return HttpNotFound();
            }

            return View(exhibition);
        }

        // POST: Artefacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ExhibitionDto exhibition, HttpPostedFileBase imageFile) {
            if (string.IsNullOrEmpty(exhibition.Name))
            {
                ViewBag.ValidationName = "Name field is required.";
                return View(exhibition);
            }
            var request = new HTTPrequest();
            ExhibitionDto exhibition_editted = await request.Get<ExhibitionDto>("api/exhibition/" + exhibition.Id);
            if (ModelState.IsValid) {
                exhibition_editted.Name = exhibition.Name;
                exhibition_editted.Description = exhibition.Description;
                exhibition_editted.Price_Adult = exhibition.Price_Adult;
                exhibition_editted.Price_Concession = exhibition.Price_Concession;
                exhibition_editted.Price_Child = exhibition.Price_Child;
                exhibition_editted.StartDate = exhibition.StartDate;
                exhibition_editted.FinishDate = exhibition.FinishDate;
                exhibition_editted.Organiser = exhibition.Organiser;
                exhibition_editted.ModifiedDate = DateTime.Now;

              //  HttpPostedFileBase imgFile = Request.Files["ImageFile"];
                if (imageFile != null) {
                    exhibition_editted.Image = new byte[imageFile.ContentLength];
                    imageFile.InputStream.Read(exhibition_editted.Image, 0, imageFile.ContentLength);
                    string fileExtension = Path.GetExtension(imageFile.FileName);
                    //exhibition_editted.ImageFileType = fileExtension;
                } else {
                    exhibition_editted.Image = exhibition_editted.Image;
                    //exhibition_editted.ImageFileType = exhibition_editted.ImageFileType;

                }
                await request.Put<ExhibitionDto>("api/exhibition", exhibition_editted);
                return RedirectToAction("Index", new { recentAction = "Editted", recentName = exhibition.Name, recentId = exhibition.Id });
            }
            return View(exhibition);
        }

        // GET: Artefacts/Delete/5
        public async Task<ActionResult> Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();

            ExhibitionDto exhibition = await request.Get<ExhibitionDto>("api/exhibition/" + id);
            if (exhibition == null) {
                return HttpNotFound();
            }
            return View(exhibition);
        }

        // POST: Artefacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> DeleteConfirmed(int id) {

            var request = new HTTPrequest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExhibitionDto exhibition = await request.Get<ExhibitionDto>("api/exhibition/" + id);
            if (exhibition == null)
            {
                return HttpNotFound();
            }
            exhibition.IsDeleted = true;
            var requestDelete = new HTTPrequest();

            exhibition = await requestDelete.Put<ExhibitionDto>("api/exhibition", exhibition);
            await request.Delete("api/exhibition/" + id.ToString());

            return RedirectToAction("Index", new { recentAction = "Deleted", recentName = exhibition.Name, recentId = exhibition.Id });

        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
