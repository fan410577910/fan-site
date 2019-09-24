using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace FAN.Core.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            

            //configuration.GetSection("Size").Get<string>("Light");
            //md m = configuration.Get<md>();

        }

        public class md
        {
            public int Size { get; set; }
            public string Color{get;set;}
        }
    }
}
