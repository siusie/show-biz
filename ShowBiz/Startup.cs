using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShowBiz.Startup))]

namespace ShowBiz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
