using FAN.Helper;
using FAN.Remoting;
using FAN.SyncImage.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.SyncImage.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Global.Start();
            System.Console.Read();
        }
    }
    class Person
    {
        public string Name { get; set; }
        public string SayHello(string hello)
        {
            return hello;
        }
    }
}
