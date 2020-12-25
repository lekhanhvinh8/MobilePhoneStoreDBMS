using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MobilePhoneStoreDBMS.Startup))]
namespace MobilePhoneStoreDBMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
