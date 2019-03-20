using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forge.Museum.Interfaces.DataTransferObjects.Artefact;

namespace Forge.Museum.API.Controllers
{
    public class ViewerController : Controller
    {
        public FileContentResult GetFile(int id)
        {
            ArtefactInfoDto artefactInfo = new ArtefactInfoController().GetById(id);

            return File(artefactInfo.File, System.Net.Mime.MediaTypeNames.Application.Octet, "artefact-info-" + id + ".mp4");
        }
    }
}