using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FAN.Console
{
    /// <summary>
    /// 异步变成案例
    /// .NET有三种异步编程模型：EAP、APM、TPL(重点)
    /// </summary>
    internal class AsyncDemo
    {
        private static readonly string DOWNLOAD_URL = "https://www.taobao.com";
        /// <summary>
        /// EAP:Event-based Asynchronous Pattern（基于事件的异步模型）
        /// 类似于 Ajax 中的XmlHttpRequest，send 之后并不是处理完成了，而是在 onreadystatechange 事件中再通知处理完成。
        /// 优点：简单   缺点：使用起来麻烦，尤其是多次异步操作
        /// </summary>
        public static void EAPDemo()
        {
            WebClient client = new WebClient();            
            client.DownloadStringAsync(new Uri(DOWNLOAD_URL));
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) => { System.Console.WriteLine(e.Result); };
        }
        /// <summary>
        /// APM:Asynchronous Programming Model  是.Net 旧版本中广泛使用的异步编程模型。
        /// 使用了 APM的异步方法会返回一个 IAsyncResult 对象，这个对象有一个重要的属性 AsyncWaitHandle，他是一个用来等待异步任务执行结束的一个同步信号。
        /// </summary>
        public static void APMDemo()
        {
            string path = Path.Combine( Directory.GetCurrentDirectory(),"1.txt");
            byte[] buffer = new byte[1000];
            FileStream fs = new FileStream(path, FileMode.Open);
            IAsyncResult aResult = fs.BeginRead(buffer, 0, buffer.Length,null,null);
            aResult.AsyncWaitHandle.WaitOne();            
            System.Console.WriteLine(Encoding.UTF8.GetString(buffer));
            fs.EndRead(aResult);
        }
        /// <summary>
        /// TPL:Task Parallel Library 是.Net 4.0 之后带来的新特性，更简洁方便，最常用
        /// </summary>
        public static async Task TPLDemoAsync()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "1.txt");
            FileStream fs = File.OpenRead(path);
            byte[] buffer = new byte[1000];
            int len = await fs.ReadAsync(buffer, 0, buffer.Length);
            System.Console.WriteLine("读取了" + len + "个字节");
            Thread.Sleep(3000);
            System.Console.WriteLine(Encoding.UTF8.GetString(buffer));
        }

    }
}
