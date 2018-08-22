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

namespace Forge.Museum.Web.Controllers
{
    public class ArtefactCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ArtefactCategoryDtoes
        public async Task<ActionResult> Index()
        {
            var request = new HTTPrequest();

            List<ArtefactCategoryDto> viewModel = await request.Get<List<ArtefactCategoryDto>>("api/artefactCatergory?pageNumber=0&numPerPage=500&isDeleted=false");

            return View(viewModel);
        }

        // GET: ArtefactCategoryDtoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            ArtefactCategoryDto category = await request.Get<ArtefactCategoryDto>("api/artefactCatergory/" + id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: ArtefactCategoryDtoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtefactCategoryDtoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ArtefactCategoryDto category, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                var request = new HTTPrequest();

                HttpPostedFileBase imgFile = Request.Files["ImageFile"];
                if (imgFile != null)
                {
                    category.Image = new byte[imgFile.ContentLength];
                    imgFile.InputStream.Read(category.Image, 0, imgFile.ContentLength);
                }

                category = await request.Post<ArtefactCategoryDto>("api/artefactCatergory", category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: ArtefactCategoryDtoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var request = new HTTPrequest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtefactCategoryDto category = await request.Get<ArtefactCategoryDto>("api/artefactCatergory/" + id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: ArtefactCategoryDtoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ArtefactCategoryDto category, HttpPostedFileBase imageFile)
        {
            var request = new HTTPrequest();
            ArtefactCategoryDto category_editted = await request.Get<ArtefactCategoryDto>("api/artefactCatergory/" + category.Id);
            if (ModelState.IsValid) {
                category_editted.Name = category.Name;
                category_editted.Description = category.Description;
                category_editted.ModifiedDate = DateTime.Now;
                HttpPostedFileBase imgFile = Request.Files["ImageFile"];

                if (imageFile != null) {
                    category_editted.Image = new byte[imageFile.ContentLength];
                    imageFile.InputStream.Read(category_editted.Image, 0, imageFile.ContentLength);
                } else {
                    category_editted.Image = category_editted.Image;
                }

                await request.Put<ArtefactCategoryDto>("api/artefactCatergory", category_editted);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: ArtefactCategoryDtoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            ArtefactCategoryDto category = await request.Get<ArtefactCategoryDto>("api/artefactCatergory/" + id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: ArtefactCategoryDtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var request = new HTTPrequest();
                await request.Delete("api/artefactCatergory/" + id);
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
