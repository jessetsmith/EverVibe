using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VibeSpace.Startup))]
namespace VibeSpace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
