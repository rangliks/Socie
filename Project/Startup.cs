using Microsoft.Owin;
using Owin;
using log4net;

[assembly: OwinStartupAttribute(typeof(Project.Startup))]
namespace Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
