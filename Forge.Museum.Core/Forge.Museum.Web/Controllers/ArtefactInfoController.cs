﻿using System;
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
using System.Net.Http;

namespace Forge.Museum.Web.Controllers
{
    public class ArtefactInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ArtefactInfo
        public async Task<ActionResult> Index(int? artefactId)
        {
            var request = new HTTPrequest();
            ViewBag.ArtefactID = artefactId;
            List<ArtefactInfoDto> viewModel = new List<ArtefactInfoDto>();
            if (artefactId == null)
            {
                viewModel = await request.Get<List<ArtefactInfoDto>>("api/artefactInfo?pageNumber=0&numPerPage=500&isDeleted=false");
            } else
            {
                viewModel = await request.Get<List<ArtefactInfoDto>>("api/artefactInfo?artefactId="+artefactId+"&pageNumber=0&numPerPage=500&isDeleted=false");
            }
            return View(viewModel);

        }

        // GET: ArtefactInfo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = new HTTPrequest();

            ArtefactInfoDto artefactInfo = await request.Get<ArtefactInfoDto>("api/artefactInfo/" + id);
            if (artefactInfo == null)
            {
                return HttpNotFound();
            }
            return View(artefactInfo);
        }

        // GET: ArtefactInfo/Create
        public async Task<ActionResult> Create(int? artefactId)
        {
            bool artefact_Selected = artefactId.HasValue;
            ViewBag.ArteFactSelected = artefact_Selected;
            var request = new HTTPrequest();
            List<ArtefactSimpleDto> artefactsList = new List<ArtefactSimpleDto>();
            ArtefactSimpleDto artefact = new ArtefactSimpleDto();
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            // Checks if page was access from Index of all MediaFiles, or Direct from a particular artefact
            if (artefact_Selected == true)
            {
                artefact = await request.Get<ArtefactSimpleDto>("api/artefact/"+artefactId);
                if (artefact == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    artefactDropdown.Add(new SelectListItem()
                    {
                        Text = artefact.Name,
                        Value = artefact.Id.ToString()

                    });
                    ViewBag.ArtefactName = artefact.Name;
                    ViewBag.ArtefactID = artefact.Id.ToString();

                }
            } else {
                artefactsList = await request.Get<List<ArtefactSimpleDto>>("api/artefact?pageNumber=0&numPerPage=5-0&isDeleted=false");
                if (artefactsList != null && artefactsList.Any())
                {
                    foreach (var artefacts in artefactsList)
                    {
                        artefactDropdown.Add(new SelectListItem()
                        {
                            Text = artefacts.Id + ":" + artefacts.Name,
                            Value = artefacts.Id.ToString()
                        });
                    }
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
        public async Task<ActionResult> Create(ArtefactInfoDto artefactInfo, HttpPostedFileBase ArtefactInfoFile)
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
                //HttpPostedFileBase newFile = Request.Files["ArtefactInfoFile"];
                //HttpPostedFileBase newFile = ArtefactInfoFile;
                    if (ArtefactInfoFile != null && ArtefactInfoFile.ContentLength > 0)
                    {
                        artefactInfo.File = new byte[ArtefactInfoFile.ContentLength];
                        ArtefactInfoFile.InputStream.Read(artefactInfo.File, 0, ArtefactInfoFile.ContentLength);
                        string fileExtension = Path.GetExtension(ArtefactInfoFile.FileName);
                        artefactInfo.FileExtension = fileExtension;
                    if (ArtefactInfoFile == null) {
                        throw new ArgumentException("file");
                    }
                }
                var request2 = new HTTPrequest();

                artefactInfo = await request2.Post<ArtefactInfoDto>("api/artefactInfo", artefactInfo);

                return RedirectToAction("Index");
            }
            return View(artefactInfo);
        }

        // GET: ArtefactInfo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var request = new HTTPrequest();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtefactInfoDto artefactInfo = await request.Get<ArtefactInfoDto>("api/artefactInfo/" + id);

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
        public async Task<ActionResult> Edit(ArtefactInfoDto artefactInfo, HttpPostedFileBase ArtefactInfoFile)
        {
            var request = new HTTPrequest();
            ArtefactInfoDto artefactInfo_editted = await request.Get<ArtefactInfoDto>("api/artefactInfo/" + artefactInfo.Id.ToString());

            if (ModelState.IsValid)
            {
                artefactInfo_editted.Content = artefactInfo.Content;
                artefactInfo_editted.Description = artefactInfo.Description;
                artefactInfo_editted.ArtefactInfoType = artefactInfo.ArtefactInfoType;
                if (ArtefactInfoFile != null)
                {
                    HttpPostedFileBase mediaFile = Request.Files["ArtefactInfoFile"];

                    artefactInfo_editted.File = new byte[mediaFile.ContentLength];
                    mediaFile.InputStream.Read(artefactInfo_editted.File, 0, mediaFile.ContentLength);
                    string fileExtension = Path.GetExtension(mediaFile.FileName);
                    artefactInfo_editted.FileExtension = fileExtension;
                }


                artefactInfo = await request.Put<ArtefactInfoDto>("api/artefactInfo", artefactInfo_editted);
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
            var request = new HTTPrequest();
            ArtefactInfoDto artefactInfo = await request.Get<ArtefactInfoDto>("api/artefactInfo/" + id);
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
            try
            {
                var request = new HTTPrequest();
                ArtefactInfoDto artefactInfo = await request.Get<ArtefactInfoDto>("api/artefactInfo/" + id);
                artefactInfo.IsDeleted = true;
                await request.Put<ArtefactInfoDto>("api/artefactInfo", artefactInfo);
                await request.Delete("api/artefactInfo/" + id.ToString());
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
