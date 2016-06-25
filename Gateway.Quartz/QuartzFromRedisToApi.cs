using System;
using System.Collections.Generic;
using System.Linq;
using Quartz;
using System.Threading;
using System.Threading.Tasks;
using APIRedisSql.Gateway;
using APIRedisSql.Gateway.Model;
using NLog;


namespace Gateway.Quartz
{
    public sealed class QuartzFromRedisToApi : IJob
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public async void Execute(IJobExecutionContext context)
        {
            var processer = new PushToRedisProcesser();
            Console.WriteLine($"WriteExecuteTime:{DateTime.Now}");
            while (true)
            {
                try
                {
                    await processer.ProcessAsync();
                }
                catch (AggregateException ex)
                {
                    Logger.Error(ex);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
    }

    public class PushToRedisProcesser
    {
        public async Task ProcessAsync()
        {
            List<AAGetModel> peroidLists = SqlDal.GetPeriodAAModelList();
            Console.WriteLine($"TestTask:{DateTime.Now}");
            if (peroidLists.Any())
            {
                bool pushSuccess = await Redishelper.BoolPushList<AAGetModel>("SortingList", peroidLists);
                if (pushSuccess)
                {
                    SqlDal.ChangePeriodAAModelInfoListStatusInSql(peroidLists);
                    Console.WriteLine($"ChangeStatus:{DateTime.Now}");
                }
            }
            else
            {
                Console.WriteLine($"sleep:{DateTime.Now}");
                Thread.Sleep(20000);
            }
        }
    }
}
