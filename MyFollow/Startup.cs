using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyFollow.Startup))]
namespace MyFollow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
