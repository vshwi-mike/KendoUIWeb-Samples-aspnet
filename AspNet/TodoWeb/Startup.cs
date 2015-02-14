using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TodoWeb.Startup))]
namespace TodoWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
