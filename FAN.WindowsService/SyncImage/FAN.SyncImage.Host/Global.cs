using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.SyncImage.Host
{
    public static class Global
    {
        public static string UPLOAD_PATH = @"d:\testFolder";
        public static void Start()
        {
            SyncImage.Start();
        }
        public static void Stop()
        {
            SyncImage.Stop();
        }
    }
}
