using System.Web.Http;
using NLog;

namespace APIRedisSql.Gateway.Controller
{
    public abstract class GatewayControllerBase : ApiController
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
    }
}

