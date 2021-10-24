using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoProject1.Startup))]
namespace DemoProject1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
