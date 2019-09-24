using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.FtpClient;
using System.Runtime.CompilerServices;
using System.Collections;
using ConnectionParser;
using System.IO;
using FAN.Helper;

namespace FAN.SyncImage.Host
{
    public class SyncImage
    {
        private static readonly int UPLOAD_TYPE = 1;//FTP、...
        private static readonly string FTP_CONNECTION_STRING = "uploadUID=fan;uploadPWD=poxiao315;uploadHost=127.0.0.1;uploadPort=21;uploadRootPath=/testsyncimage";
        private static readonly string COPY_PATH = @"e:\copyFolder";
        private static readonly int TIMEOUT_MILLS = 2000;
        private static readonly int BUFFER_SIZE = 1024;

        public static void Start()
        {
            SyncWatch.Instance.StartWatch();
            SyncWatch.Instance.CreateEvent += SyncWatch_CreateEvent;
        }

        public static void Stop() {
            SyncWatch.Instance.StopWatch();
        }

        private static  void SyncWatch_CreateEvent(FileInfo fi) {
            TaskAsyncHelper.Empty.Success(task =>
            {
                MoveFile(fi);
            });
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void MoveFile(FileInfo fi)
        {
            string fileFullName = fi.FullName;
            string copyFileName = Path.Combine(COPY_PATH, Path.GetFileName(fileFullName));

            if (UPLOAD_TYPE == 1)
            {//FTP
                ConnectionConfiguration ftpSetting = ConnectionStringParser.Parse(FTP_CONNECTION_STRING);
                FtpClient ftpClient = new FtpClient { Host = ftpSetting.UploadHost, Port = ftpSetting.UploadPort, Credentials = new NetworkCredential(ftpSetting.UploadUID, ftpSetting.UploadPWD) };
                string fileName = Path.GetFileName(fileFullName);
                string filePath = Path.GetDirectoryName(fileFullName);
                filePath = filePath.Replace(Global.UPLOAD_PATH, "");
                string copyFullName = Path.Combine(filePath, fileName);


                if (!ftpClient.DirectoryExists(filePath))
                {
                    ftpClient.CreateDirectory(filePath);
                }
                if (ftpClient.FileExists(filePath))
                {
                    ftpClient.DeleteFile(filePath);
                }
                using (Stream stream = ftpClient.OpenWrite(copyFullName, FtpDataType.Binary))
                {
                    byte[] oriBytes = File.ReadAllBytes(fileFullName);
                    int offset = 0;
                    int bufferSize = 0;
                    while (offset < oriBytes.Length)
                    {
                        if (oriBytes.Length - 1 - offset < BUFFER_SIZE)
                        {
                            bufferSize = oriBytes.Length - offset;
                        }
                        else
                        {
                            bufferSize = BUFFER_SIZE;
                        }
                        stream.Write(oriBytes, offset, bufferSize);
                        offset += bufferSize;
                    }
                }
            }
            else
            {
                //网络驱动器
            }
        }
    }
}
