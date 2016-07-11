using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventDemoProject.Startup))]
namespace EventDemoProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
