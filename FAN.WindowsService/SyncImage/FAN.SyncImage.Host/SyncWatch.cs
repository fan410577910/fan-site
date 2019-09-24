
using FAN.Helper;
using System;
using System.Collections.Generic;
using System.IO;

using System.Threading;

namespace FAN.SyncImage.Host
{
    //private Timer _timer = null;
    //this._timer = new Timer(new TimerCallback(this.OnTimer), null, Timeout.Infinite, Timeout.Infinite);
    //this._timer.Change(TIMEOUT_MILLS, Timeout.Infinite);
    internal class SyncWatch : IDisposable
    {
        private FileSystemWatcher _fileSystemWatcher = null;
        public static SyncWatch Instance = new SyncWatch();
        private object SyncRoot = new object();
        public event Action<FileInfo> CreateEvent = null;
        private SyncWatch()
        {
            this._fileSystemWatcher = new FileSystemWatcher(Global.UPLOAD_PATH);
            this._fileSystemWatcher.IncludeSubdirectories = true;
            this._fileSystemWatcher.Created += _fileSystemWatcher_Created;
        }

        private void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (this.CreateEvent!=null)
            {
                FileInfo fi = new FileInfo(e.FullPath);
                if (fi!=null)
                {
                    this.CreateEvent(fi);
                }
            }
        }


        /// <summary>
        /// 开始监控
        /// </summary>
        public void StartWatch()
        {
            if (!this._fileSystemWatcher.EnableRaisingEvents)
            {
                this._fileSystemWatcher.EnableRaisingEvents = true;
            }
        }
        /// <summary>
        /// 结束监控
        /// </summary>
        public void StopWatch()
        {
            this._fileSystemWatcher.EnableRaisingEvents = false;
        }
        public void Dispose()
        {
            this.StopWatch();
            this._fileSystemWatcher.Dispose();
        }
    }





    //public class SyncWatch : IDisposable
    //{
    //    private FileSystemWatcher _fileSystemWatcher = null;
    //    private Timer _timer = null;
    //    private static readonly int UPLOAD_TYPE = 1;//FTP、...
    //    private static readonly string FTP_CONNECTION_STRING = "uploadUID=fan;uploadPWD=poxiao315;uploadHost=127.0.0.1;uploadPort=21;uploadRootPath=/testsyncimage";
    //    private static readonly string UPLOAD_PATH = @"d:\testFolder";
    //    private static readonly string COPY_PATH = @"e:\copyFolder";
    //    private static readonly int TIMEOUT_MILLS = 2000;
    //    private static readonly int BUFFER_SIZE = 1024;
    //    private List<string> _filePathList = new List<string>();
    //    public static SyncWatch Instance = new SyncWatch();
    //    private object SyncRoot = new object();
    //    private SyncWatch()
    //    {
    //        this._fileSystemWatcher = new FileSystemWatcher(UPLOAD_PATH);
    //        this._fileSystemWatcher.IncludeSubdirectories = true;
    //        this._fileSystemWatcher.Created += _fileSystemWatcher_Created;
    //        this._timer = new Timer(new TimerCallback(this.OnTimer), null, Timeout.Infinite, Timeout.Infinite);
    //    }

    //    private void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
    //    {
    //        lock (this.SyncRoot)
    //        {
    //            if (!_filePathList.Contains(e.FullPath))
    //            {
    //                _filePathList.Add(e.FullPath);
    //            }
    //        }

    //        this._timer.Change(TIMEOUT_MILLS, Timeout.Infinite);
    //    }

    //    private void OnTimer(object state)
    //    {
    //        lock (this.SyncRoot)
    //        {
    //            List<string> filePathList = new List<string>();
    //            filePathList.AddRange(this._filePathList);
    //            this._filePathList.Clear();
    //            TaskAsyncHelper.Empty.Success(task =>
    //            {
    //                foreach (string filePath in filePathList)
    //                {
    //                    this.MoveFile(filePath);
    //                }
    //            });
    //        }
    //    }
    //    /// <summary>
    //    /// 开始监控
    //    /// </summary>
    //    public void StartWatch()
    //    {
    //        if (!this._fileSystemWatcher.EnableRaisingEvents)
    //        {
    //            this._fileSystemWatcher.EnableRaisingEvents = true;
    //        }
    //    }
    //    /// <summary>
    //    /// 结束监控
    //    /// </summary>
    //    public void StopWatch()
    //    {
    //        this._fileSystemWatcher.EnableRaisingEvents = false;
    //    }
    //    /// <summary>
    //    /// 移动文件
    //    /// </summary>
    //    [MethodImpl(MethodImplOptions.Synchronized)]
    //    private void MoveFile(string fileFullName)
    //    {
    //        string copyFileName = Path.Combine(COPY_PATH, Path.GetFileName(fileFullName));

    //        if (UPLOAD_TYPE == 1)
    //        {//FTP
    //            ConnectionConfiguration ftpSetting = ConnectionStringParser.Parse(FTP_CONNECTION_STRING);
    //            FtpClient ftpClient = new FtpClient { Host = ftpSetting.UploadHost, Port = ftpSetting.UploadPort, Credentials = new NetworkCredential(ftpSetting.UploadUID, ftpSetting.UploadPWD) };
    //            string fileName = Path.GetFileName(fileFullName);
    //            string filePath = Path.GetDirectoryName(fileFullName);
    //            filePath = filePath.Replace(UPLOAD_PATH, "");
    //            string copyFullName = Path.Combine(filePath, fileName);


    //            if (!ftpClient.DirectoryExists(filePath))
    //            {
    //                ftpClient.CreateDirectory(filePath);
    //            }
    //            if (ftpClient.FileExists(filePath))
    //            {
    //                ftpClient.DeleteFile(filePath);
    //            }
    //            using (Stream stream = ftpClient.OpenWrite(copyFullName, FtpDataType.Binary))
    //            {
    //                byte[] oriBytes = File.ReadAllBytes(fileFullName);
    //                int offset = 0;
    //                int bufferSize = 0;
    //                while (offset < oriBytes.Length)
    //                {
    //                    if (oriBytes.Length - 1 - offset < BUFFER_SIZE)
    //                    {
    //                        bufferSize = oriBytes.Length - offset;
    //                    }
    //                    else
    //                    {
    //                        bufferSize = BUFFER_SIZE;
    //                    }
    //                    stream.Write(oriBytes, offset, bufferSize);
    //                    offset += bufferSize;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            //网络驱动器
    //        }
    //    }
    //    public void Dispose()
    //    {
    //        this.StopWatch();
    //        this._fileSystemWatcher.Dispose();
    //        this._timer.Dispose();
    //    }
    //}
}
