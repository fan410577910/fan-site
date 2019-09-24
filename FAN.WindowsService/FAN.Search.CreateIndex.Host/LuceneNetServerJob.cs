using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Search.CreateIndex.Host
{
    public class LuceneNetServerJob:IJob
    {
            public void Execute(IJobExecutionContext context)
            {
                try
                {
                    this.Start();
                }
                catch (Exception ex)
                {
                    //Global.Logger.LogWithTime(Global.SERVICE_NAME + ".LuceneNetBusServerJob.Execute(IJobExecutionContext context)服务出异常,错误信息：" + ex, Logger.ELogLevel.Error);
                }
            }

            public void Start()
            {
                List<string> cultureList = new List<string>() { "EN","JA","CN"};
                LuceneNetBus.Build(cultureList);
            }
    }
}
