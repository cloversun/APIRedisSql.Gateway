using System.Web.Http;
using Owin;

namespace APIRedisSql.Gateway
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(config);
        }
    }
}
