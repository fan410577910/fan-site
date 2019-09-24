using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.MongoDB.Host
{
    public class Global
    {
        //调度器
        private static IScheduler _scheduler = null;

        public static void Start()
        {
            int repeatcount = int.MaxValue;
            TimeSpan sleepMongoMinute = TimeSpan.FromSeconds(10);
            _scheduler = new StdSchedulerFactory().GetScheduler();
            //触发器（定义什么时间任务开始或每隔多长时间执行一次）
            SimpleTriggerImpl trigger = new SimpleTriggerImpl("trigger1", repeatcount, sleepMongoMinute);
            //ITrigger triggerCron = TriggerBuilder.Create().WithIdentity("trigger1", "group1").WithCronSchedule("0/5 * * * * ?").Build();    //5秒执行一次
            //任务
            IJobDetail job1 = JobBuilder.Create<MongoDBServer>().WithIdentity("job1", "group1").Build();
            //将任务和触发器添加到调度器中
            _scheduler.ScheduleJob(job1, trigger);
            //开始执行
            _scheduler.Start();
        }

        public static void Stop() {
            //结束任务
            _scheduler.Shutdown(true);
        }
    }
}
