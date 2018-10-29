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
using System.Net.Http;
using PagedList;

namespace Forge.Museum.Web.Controllers { 
[Authorize]
    public class ArtefactInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        public async Task<List<SelectListItem>> PopulateArtefactDropdown()
        {
            var request = new HTTPrequest();
            List<ArtefactSimpleDto> artefacts = await request.Get<List<ArtefactSimpleDto>>("api/artefact?pageNumber=0&numPerPage=5-0&isDeleted=false");
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            if (artefacts != null && artefacts.Any())
            {
                foreach (var category in artefacts)
                {
                    artefactDropdown.Add(new SelectListItem()
                    {
                        Text = category.Id + ": " + category.Name,
                        Value = category.Id.ToString()
                    });
                }
            }
            return artefactDropdown;
        }



        // GET: ArtefactInfo
        public async Task<ActionResult> Index(int? artefactId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            var requestArtefact = new HTTPrequest();
            var requestArtefactsInfoList = new HTTPrequest();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "description_desc" : "Description";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";

            ViewBag.ArtefactID = artefactId;
            // Retrives a list of All Artefacts if user has not navaigated through a particular Artefact Id 
            List<ArtefactInfoDto> artefactContentMasterList = new List<ArtefactInfoDto>();
            if (artefactId == null)
            {
                artefactContentMasterList = await requestArtefactsInfoList.Get<List<ArtefactInfoDto>>("api/artefactInfo?pageNumber=0&numPerPage=500&isDeleted=false");
            } else
            {
                artefactContentMasterList = await requestArtefactsInfoList.Get<List<ArtefactInfoDto>>("api/artefactInfo?artefactId="+artefactId+"&pageNumber=0&numPerPage=500&isDeleted=false");
                ArtefactDto artefact = await requestArtefact.Get<ArtefactDto>("api/artefact/" + artefactId);
                ViewBag.ArtefactName = artefact.Name;
            }
            IEnumerable<ArtefactInfoDto> artefactContentFiltered = artefactContentMasterList.ToList();

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
                artefactContentFiltered = artefactContentFiltered.Where(a => a.Description.Contains(searchString));
            }

            var artefactsContent = artefactContentFiltered;

            switch (sortOrder)
            {
                case "id_desc":
                    artefactsContent = artefactsContent.OrderByDescending(a => a.Id);
                    break;
                case "Description":
                    artefactsContent = artefactsContent.OrderBy(a => a.Description);
                    break;
                case "description_desc":
                    artefactsContent = artefactsContent.OrderByDescending(a => a.Description);
                    break;
                case "Name":
                    artefactsContent = artefactsContent.OrderBy(a => a.Artefact.Name);
                    break;
                case "name_desc":
                    artefactsContent = artefactsContent.OrderByDescending(a => a.Artefact.Name);
                    break;
                case "Type":
                    artefactsContent = artefactsContent.OrderBy(a => a.ArtefactInfoType);
                    break;
                case "type_desc":
                    artefactsContent = artefactsContent.OrderByDescending(a => a.ArtefactInfoType);
                    break;
                default:
                    artefactsContent = artefactsContent.OrderBy(a => a.Id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(artefactsContent.ToPagedList(pageNumber, pageSize));
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
                        Text = artefact.Id + ": " + artefact.Name,
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
                            Text = artefacts.Id + ": " + artefacts.Name,
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
            newArtefactInfo.Artefact = artefactInfo.Artefact;
            newArtefactInfo.ArtefactInfoType = artefactInfo.ArtefactInfoType;
            newArtefactInfo.CreatedDate = DateTime.Now;
            newArtefactInfo.ModifiedDate = DateTime.Now;
            newArtefactInfo.Description = artefactInfo.Description;
            // need to parse apostrophes out
            newArtefactInfo.Content = artefactInfo.Content;

            // Checks Name is not Null or Empty
            if (artefactInfo.Artefact.Id.ToString() == null || string.IsNullOrEmpty(artefactInfo.Artefact.Id.ToString()))
            {
                ViewBag.ValidationName = "Artefact is required.";
                return View(artefactInfo);
            }

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

                await request.Post<ArtefactInfoDto>("api/artefactInfo", artefactInfo);

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

            //ARTEFACT CATEGORY DROPDOWN
            List<SelectListItem> artefactDropdown = new List<SelectListItem>();
            artefactDropdown = await PopulateArtefactDropdown();
            ViewBag.ArtefactList = artefactDropdown;

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

            // Checks Name is not Null or Empty
            if (artefactInfo.Artefact.Id.ToString() == null || string.IsNullOrEmpty(artefactInfo.Artefact.Id.ToString()))
            {
                ViewBag.ValidationName = "Artefact is required.";
                return View(artefactInfo);
            }

            if (ModelState.IsValid)
            {
                artefactInfo_editted.Artefact = artefactInfo.Artefact;
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
