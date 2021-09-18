using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Training.Startup))]
namespace Training
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
