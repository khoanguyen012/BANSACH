using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcBT.Startup))]
namespace MvcBT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
