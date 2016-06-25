using System.Configuration;
using Topshelf;

namespace APIRedisSql.Gateway
{
    class Program
    {
        private static readonly string GatewayBaseUri = ConfigurationManager.AppSettings["Gateway.BaseUri"];

        private static int Main()
        {
            System.Threading.ThreadPool.SetMinThreads(500, 50);
            var exitCode = HostFactory.Run(host =>
            {
                host.Service<GatewayApplication>(service =>
                {
                    service.ConstructUsing(() => new GatewayApplication(GatewayBaseUri));
                    service.WhenStarted(a => a.Start());
                    service.WhenStopped(a => a.Stop());
                });

                host.UseNLog();
            });
            return (int)exitCode;
        }
    }
}

