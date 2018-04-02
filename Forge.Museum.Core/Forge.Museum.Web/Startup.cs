using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Forge.Museum.Web.Startup))]
namespace Forge.Museum.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
