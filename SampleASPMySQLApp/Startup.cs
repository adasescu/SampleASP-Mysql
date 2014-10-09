using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleASPMySQLApp.Startup))]
namespace SampleASPMySQLApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
