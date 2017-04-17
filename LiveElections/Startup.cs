using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LiveElections.Startup))]
namespace LiveElections
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
