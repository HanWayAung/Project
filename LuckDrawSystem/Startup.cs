using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LuckDrawSystem.Startup))]
namespace LuckDrawSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
