
using Topshelf;

namespace Gateway.Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<QuartzServiceRunner>();
            });
        }
    }
}
