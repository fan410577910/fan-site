using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConnectionParser
{
    /// <summary>
    /// FTP信息
    /// </summary>
    public sealed class ConnectionConfiguration
    {
        /// <summary>
        /// 自定义配置名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 下载用户名称
        /// </summary>
        public string DownUID { get; set; }
        /// <summary>
        /// 下载用户密码
        /// </summary>

        public string DownPWD { get; set; }
        /// <summary>
        /// 下载服务器地址
        /// </summary>
        public string DownHost { get; set; }
        /// <summary>
        /// 下载服务器端口
        /// </summary>
        public int DownPort { get; set; }
        /// <summary>
        /// 下载服务器根目录
        /// </summary>
        public string DownRootPath { get; set; }
        /// <summary>
        /// 上传用户名称
        /// </summary>
        public string UploadUID { get; set; }
        /// <summary>
        /// 上传用户密码
        /// </summary>
        public string UploadPWD { get; set; }
        /// <summary>
        /// 上传服务器地址
        /// </summary>
        public string UploadHost { get; set; }
        /// <summary>
        /// 上传服务器端口
        /// </summary>
        public int UploadPort { get; set; }
        /// <summary>
        /// 上传服务器根目录
        /// </summary>
        public string UploadRootPath { get; set; }

        public ConnectionConfiguration()
        {

        }
        public ConnectionConfiguration(string name, string downUID, string downPWD, string downHost, int downPort, string downRootPath, string uploadUID, string uploadPWD, string uploadHost, int uploadPort, string uploadRootPath)
        {
            this.Name = name;
            this.DownUID = downUID;
            this.DownPWD = downPWD;
            this.DownHost = downHost;
            this.DownPort = downPort;
            this.DownRootPath = downRootPath;
            this.UploadUID = uploadUID;
            this.UploadPWD = uploadPWD;
            this.UploadHost = uploadHost;
            this.UploadPort = uploadPort;
            this.UploadRootPath = uploadRootPath;
        }
    }

}
