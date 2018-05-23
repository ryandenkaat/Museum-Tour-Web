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

        // GET: ArtefactCategorySimpleDtoes
        public async Task<ActionResult> Index()
        {
            var request = new HTTPrequest();

            List<ArtefactCategorySimpleDto> viewModel = await request.Get<List<ArtefactCategorySimpleDto>>("api/artefactCatergory");

            return View(viewModel);
        }

        // GET: ArtefactCategorySimpleDtoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();
            ArtefactCategorySimpleDto artefactCategorySimpleDto = await request.Get<ArtefactCategorySimpleDto>("api/artefactCatergory/" + id);
            if (artefactCategorySimpleDto == null)
            {
                return HttpNotFound();
            }
            return View(artefactCategorySimpleDto);
        }

        // GET: ArtefactCategorySimpleDtoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtefactCategorySimpleDtoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ArtefactCategorySimpleDto artefactCategorySimpleDto)
        {
            if (ModelState.IsValid)
            {
                db.ArtefactCategorySimpleDtoes.Add(artefactCategorySimpleDto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artefactCategorySimpleDto);
        }

        // GET: ArtefactCategorySimpleDtoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtefactCategorySimpleDto artefactCategorySimpleDto = db.ArtefactCategorySimpleDtoes.Find(id);
            if (artefactCategorySimpleDto == null)
            {
                return HttpNotFound();
            }
            return View(artefactCategorySimpleDto);
        }

        // POST: ArtefactCategorySimpleDtoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ArtefactCategorySimpleDto artefactCategorySimpleDto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artefactCategorySimpleDto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artefactCategorySimpleDto);
        }

        // GET: ArtefactCategorySimpleDtoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtefactCategorySimpleDto artefactCategorySimpleDto = db.ArtefactCategorySimpleDtoes.Find(id);
            if (artefactCategorySimpleDto == null)
            {
                return HttpNotFound();
            }
            return View(artefactCategorySimpleDto);
        }

        // POST: ArtefactCategorySimpleDtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArtefactCategorySimpleDto artefactCategorySimpleDto = db.ArtefactCategorySimpleDtoes.Find(id);
            db.ArtefactCategorySimpleDtoes.Remove(artefactCategorySimpleDto);
            db.SaveChanges();
            return RedirectToAction("Index");
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
