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
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;
using Forge.Museum.BLL.Http;
using System.IO;

namespace Forge.Museum.Web.Controllers
{
    public class ArtefactInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ArtefactInfo
        public async Task<ActionResult> Index()
        {
            var request = new HTTPrequest();

            List<ArtefactInfo> viewModel = await request.Get<List<ArtefactInfo>>("api/artefactInfo?pageNumber=0&numPerPage=500&isDeleted=false");

            return View(viewModel);

        }

        // GET: ArtefactInfo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtefactInfo artefactInfo = await db.ArtefactInfoes.FindAsync(id);
            if (artefactInfo == null)
            {
                return HttpNotFound();
            }
            return View(artefactInfo);
        }

        // GET: ArtefactInfo/Create
        public async Task<ActionResult> Create()
        {
            var request = new HTTPrequest();
            List<ArtefactSimpleDto> artefactsList = await request.Get<List<ArtefactSimpleDto>>("api/artefact?pageNumber=0&numPerPage=5-0&isDeleted=false");

            List<SelectListItem> artefactDropdown = new List<SelectListItem>();

            if (artefactsList != null && artefactsList.Any())
            {
                foreach (var artefact in artefactsList)
                {
                    artefactDropdown.Add(new SelectListItem()
                    {
                        Text = artefact.Name,
                        Value = artefact.Id.ToString()
                    });
                }
            }

            ViewBag.ArtefactList = artefactDropdown;


            return View();
        }

        // POST: ArtefactInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ArtefactInfoDto artefactInfo)
        {

            ArtefactInfoDto newArtefactInfo = new ArtefactInfoDto();
            newArtefactInfo.Id = artefactInfo.Id;
            newArtefactInfo.ArtefactInfoType = artefactInfo.ArtefactInfoType;
            newArtefactInfo.CreatedDate = DateTime.Now;
            newArtefactInfo.ModifiedDate = DateTime.Now;
            newArtefactInfo.Description = artefactInfo.Description;
            newArtefactInfo.Content = artefactInfo.Content;

            if (ModelState.IsValid) { 
                var request = new HTTPrequest();
                HttpPostedFileBase newFile = Request.Files["ArtefactInfoFile"];

                if (newFile != null && newFile.ContentLength > 0) {

                    artefactInfo.File = new byte[newFile.ContentLength];
                    newFile.InputStream.Read(artefactInfo.File, 0, newFile.ContentLength);

                  //  var fileName = Path.GetFileName(artefactInfoFile.ToString());
                    //string fileExtension = Path.GetExtension(fileName);
                    //newArtefactInfo.File = new byte[artefactInfoFile.ContentLength];
                    //newArtefactInfo.FileExtension = fileExtension;

                   // artefactInfoFile.InputStream.Read(newArtefactInfo.File, 0, artefactInfoFile.ContentLength);
                }
                artefactInfo = await request.Post<ArtefactInfoDto>("api/artefactInfo", artefactInfo);
                return RedirectToAction("Index");
            }

            return View(artefactInfo);
        }

        // GET: ArtefactInfo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtefactInfo artefactInfo = await db.ArtefactInfoes.FindAsync(id);
            if (artefactInfo == null)
            {
                return HttpNotFound();
            }
            return View(artefactInfo);
        }

        // POST: ArtefactInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,File,FileExtension,ArtefactInfoType,Content")] ArtefactInfo artefactInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artefactInfo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(artefactInfo);
        }

        // GET: ArtefactInfo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtefactInfo artefactInfo = await db.ArtefactInfoes.FindAsync(id);
            if (artefactInfo == null)
            {
                return HttpNotFound();
            }
            return View(artefactInfo);
        }

        // POST: ArtefactInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ArtefactInfo artefactInfo = await db.ArtefactInfoes.FindAsync(id);
            db.ArtefactInfoes.Remove(artefactInfo);
            await db.SaveChangesAsync();
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
