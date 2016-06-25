using System;
using Microsoft.Owin.Hosting;

namespace APIRedisSql.Gateway
{
    public class GatewayApplication
    {
        private readonly string _url;

        public GatewayApplication(string url)
        {
            this._url = url;
        }

        protected IDisposable Application;

        public void Start()
        {
            Application = WebApp.Start<Startup>(_url);
        }

        public void Stop()
        {
            Application.Dispose();
        }
    }
}
