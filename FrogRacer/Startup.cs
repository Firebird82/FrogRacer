using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FrogRacer.Startup))]
namespace FrogRacer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
