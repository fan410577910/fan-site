using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            const string SERVICE_NAME = "WindowsService1";
            if (args.Length > 0 && (args[0].ToLower() == "-install" || args[0].ToLower() == "-i"))
            {
                if (!ServiceIsExisted(SERVICE_NAME))
                {
                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { string.Concat(SERVICE_NAME, ".exe") });

                    ServiceController c = new ServiceController(SERVICE_NAME);
                    c.Start();
                }
            }
            else if (args.Length > 0 && (args[0].ToLower() == "-uninstall" || args[0].ToLower() == "-u"))
            {
                if (ServiceIsExisted(SERVICE_NAME))
                {
                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { "/u", string.Concat(SERVICE_NAME, ".exe") });
                }
            }
            else
            {
                System.ServiceProcess.ServiceBase[] servicesToRun = new System.ServiceProcess.ServiceBase[] { new Service1() };
                System.ServiceProcess.ServiceBase.Run(servicesToRun);
            }
        }
        /// <summary>
        /// 检查指定的服务是否存在
        /// </summary>
        /// <param name="svcName">服务名称</param>
        /// <returns></returns>
        private static bool ServiceIsExisted(string svcName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (string.Compare(s.ServiceName, svcName) == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
