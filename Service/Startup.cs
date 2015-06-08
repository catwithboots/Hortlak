using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Service.Startup))]

namespace Service
{
    public class Startup
    {
        //  Hack from http://stackoverflow.com/a/17227764/19020 to load controllers in 
        //  another assembly.  Another way to do this is to create a custom assembly resolver
        Type _publicControllerType = typeof(Api.Controllers.PublicController);
        
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            //  Enable attribute based routing
            config.MapHttpAttributeRoutes();

            // Enable Cross-Origin requests globally
            //var cors = new EnableCorsAttribute("www.example.com", "*", "*");

            // Enable Cross-Origin requests
            config.EnableCors();

            appBuilder.UseWebApi(config);
        }
    }
}
