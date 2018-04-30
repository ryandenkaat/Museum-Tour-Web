using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Forge.Museum.BLL.Http
{
    public class HTTPrequest
    {
        private string APIurl = WebConfigurationManager.AppSettings["APIurl"];
    }
}
