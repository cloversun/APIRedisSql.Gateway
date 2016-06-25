using System;
using Quartz;
using Quartz.Impl;
using Topshelf;

namespace Gateway.Quartz
{
    public class QuartzServiceRunner : ServiceControl
    {

        public bool Start(HostControl hostControl)
        {
            try
            {
                //1.首先创建一个作业调度池
                ISchedulerFactory schedf = new StdSchedulerFactory();
                IScheduler sched = schedf.GetScheduler();
                //2.创建出来一个具体的作业
                IJobDetail job = JobBuilder.Create<QuartzFromRedisToApi>().Build();
                //3.创建并配置一个触发器
                ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(4).WithRepeatCount(0)).Build();
                //4.加入作业调度池中
                sched.ScheduleJob(job, trigger);
                //5.开始运行
                sched.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}

