using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GEmpresasEMM.Startup))]
namespace GEmpresasEMM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
