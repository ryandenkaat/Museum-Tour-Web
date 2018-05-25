﻿using System;
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
        public async Task<ActionResult> Create(ArtefactCategoryDto category, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                var request = new HTTPrequest();
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
        public async Task<ActionResult> Edit(ArtefactCategoryDto category)
        {
            var request = new HTTPrequest();
            if (ModelState.IsValid)
            {
                await request.Put<ArtefactCategoryDto>("api/artefactCatergory", category);
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
