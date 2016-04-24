using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyCustomTokenHandlerWebsite.Startup))]
namespace MyCustomTokenHandlerWebsite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
