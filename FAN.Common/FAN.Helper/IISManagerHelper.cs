#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：YANGHUANWEN 
     * 文件名：  IISManager 
     * 版本号：  V1.0.0.0 
     * 创建人：  杨焕文 
     * 创建时间：2014/10/27 10:41:33 
     * 描述    :IIS应用程序池辅助类
     * =====================================================================
     * 修改时间：2014/10/27 10:41:33 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Microsoft.Web.Administration;

namespace FAN.Helper
{

    /// <summary>
    /// IIS应用程序池辅助类
    /// </summary>
    public class IISManagerHelper
    {

        #region IIS 元数据库和IIS 6配置兼容性
        private const string HOST = "localhost";
        /// <summary>
        ///  取得所有应用程序池
        ///  要解决这个问题就得安装“IIS 元数据库和IIS 6配置兼容性”。
        /// “控制面板”->“程序和功能”->面板左侧“打开或关闭windows功能”->“Internet信息服务”->“Web管理工具”->“IIS 6管理兼容性”->“IIS 元数据库和IIS 6配置兼容性”。
        /// </summary>
        /// <returns></returns>
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        public static List<string> GetAppPools_IIS6()
        {
            DirectoryEntry appPools = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", HOST));
            return (from DirectoryEntry entry in appPools.Children select entry.Name).ToList();
        }
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        public static PropertyCollection GetAppPoolProperties_IIS6(string appPoolName)
        {
            PropertyCollection propertyCollection = null;
            DirectoryEntry appPools = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", HOST));
            foreach (DirectoryEntry entry in appPools.Children)
            {
                if (entry.Name == appPoolName)
                {
                    propertyCollection = entry.Properties;
                }
            }
            return propertyCollection;
        }

        /// <summary>
        /// 获取应用程序数量
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        public static int GetAppNumber_IIS6(string appName)
        {
            int appNumber;
            Hashtable hashTable = new Hashtable();
            List<string> poolList = GetAppPools_IIS6(); //获取应用程序池名称数组    
            foreach (string i in poolList) //填充哈希表key值内容
            {
                hashTable.Add(i, "");
            }
            GetPoolWeb_IIS6();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (string i in poolList)
            {
                appNumber = hashTable[i].ToString() != "" ? Convert.ToInt32(hashTable[i].ToString().Split(',').Length.ToString()) : 0;
                dic.Add(i, appNumber);
            }
            dic.TryGetValue(appName, out appNumber);
            return appNumber;

        }

        /// <summary>
        /// 获取IIS版本
        /// </summary>
        /// <returns></returns>
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        public static string GetIisVersion_IIS6()
        {
            string version = string.Empty;
            try
            {
                DirectoryEntry getEntity = new DirectoryEntry("IIS://LOCALHOST/W3SVC/INFO");
                version = getEntity.Properties["MajorIISVersionNumber"].Value.ToString();
            }
            catch (Exception se)
            {
                //说明一点:IIS5.0中没有(int)entry.Properties["MajorIISVersionNumber"].Value;属性，将抛出异常 证明版本为 5.0

            }
            return version;
        }

        /// <summary>
        /// 判断程序池是否存在
        /// </summary>
        /// <param name="appPoolName">程序池名称</param>
        /// <returns>true存在 false不存在</returns>
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        public static bool IsAppPoolExsit_IIS6(string appPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", HOST));
            foreach (DirectoryEntry entry in appPools.Children)
            {
                if (!entry.Name.Equals(appPoolName)) continue;
                result = true;
                break;
            }
            return result;
        }

        /// <summary>
        /// 创建应用程序池
        /// </summary>
        /// <param name="appPool"></param>
        /// <returns></returns>
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        public static bool CreateAppPool_IIS6(string appPool)
        {
            try
            {
                if (!IsAppPoolExsit_IIS6(appPool))
                {
                    DirectoryEntry appPools = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", HOST));
                    DirectoryEntry entry = appPools.Children.Add(appPool, "IIsApplicationPool");
                    entry.CommitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 操作应用程序池
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <param name="method">Start==启动 Recycle==回收 Stop==停止</param>
        /// <returns></returns>
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        public static bool DoAppPool_IIS6(string appPoolName, string method)
        {
            bool result = false;
            try
            {
                DirectoryEntry appPools = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", HOST));
                DirectoryEntry findPool = appPools.Children.Find(appPoolName, "IIsApplicationPool");
                findPool.Invoke(method, null);
                appPools.CommitChanges();
                appPools.Close();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 删除指定程序池
        /// </summary>
        /// <param name="appPoolName">程序池名称</param>
        /// <returns>true删除成功 false删除失败</returns>
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        public static bool DeleteAppPool_IIS6(string appPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", HOST));
            foreach (DirectoryEntry entry in appPools.Children)
            {
                if (!entry.Name.Equals(appPoolName)) continue;
                try
                {
                    entry.DeleteTree();
                    result = true;
                    break;
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 获得所有的应用程序池和对应站点
        /// </summary>
        [System.Obsolete("IIS 元数据库和IIS 6配置兼容性")]
        private static void GetPoolWeb_IIS6()
        {
            Hashtable hashTable = new Hashtable();
            List<string> poolList = GetAppPools_IIS6(); //获取应用程序池名称数组    
            foreach (string i in poolList) //填充哈希表key值内容
            {
                hashTable.Add(i, "");
            }

            DirectoryEntry root = new DirectoryEntry("IIS://localhost/W3SVC");
            foreach (DirectoryEntry website in root.Children)
            {
                if (website.SchemaClassName != "IIsWebServer") continue;
                string comment = website.Properties["ServerComment"][0].ToString();
                DirectoryEntry siteVDir = website.Children.Find("Root", "IISWebVirtualDir");
                string poolname = siteVDir.Properties["AppPoolId"][0].ToString().Trim();
                if (string.IsNullOrWhiteSpace(poolname))
                {
                    poolname = website.Properties["AppPoolId"][0].ToString().Trim();
                }
                foreach (string i in poolList)
                {
                    if (i != poolname) continue;
                    if (hashTable[i].ToString() == "")
                        hashTable[i] = comment;
                    else
                        hashTable[i] += "," + comment;
                }
            }
            root.Close();
        }
        #endregion

        #region IIS7
        /// <summary>
        /// 取得所有应用程序池 IIS7
        /// https://forums.iis.net/t/1151611.aspx
        /// </summary>
        /// <returns></returns>
        public static List<ApplicationPool> GetAppPools()
        {
            return new ServerManager().ApplicationPools.ToList();
        }

        /// <summary>
        /// 操作应用程序池
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <param name="method">Start==启动 Recycle==回收 Stop==停止</param>
        /// <returns></returns>
        public static bool DoAppPool(string appPoolName, string method)
        {
            bool result = false;
            ServerManager iisManager = new ServerManager();
            ApplicationPool appPool = iisManager.ApplicationPools[appPoolName];
            if (appPool != null)
            {
                try
                {
                    switch (method.ToLower())
                    {
                        case "stop":
                            appPool.Stop();
                            break;
                        case "start":
                            appPool.Start();
                            break;
                        case "recycle":
                            appPool.Recycle();
                            break;
                        default:
                            return false;
                    }
                    iisManager.CommitChanges();
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 创建应用程序池
        /// https://wenku.baidu.com/view/6fc31eaad1f34693daef3e28.html
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool CreateAppPool(string appPoolName, string username, string pwd)
        {
            ServerManager iisManager = new ServerManager();
            ApplicationPool appPool = iisManager.ApplicationPools.Add(appPoolName);
            appPool.AutoStart = true;
            appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
            appPool.ManagedRuntimeVersion = "v2.0";
            appPool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
            appPool.ProcessModel.UserName = username;
            appPool.ProcessModel.Password = pwd;
            iisManager.CommitChanges();
            return true;
        }

        /// <summary>
        /// 编辑应用程序池
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static bool EditAppPool(ApplicationPool application)
        {
            string appPoolName = application.Name;
            ServerManager iisManager = new ServerManager();
            ApplicationPool appPool = iisManager.ApplicationPools[appPoolName];
            if (appPool != null)
            {
                appPool.ManagedRuntimeVersion = application.ManagedRuntimeVersion;
                appPool.ManagedPipelineMode = application.ManagedPipelineMode;
                iisManager.CommitChanges();
            }
            return true;
        }


        /// <summary>
        /// 删除指定程序池
        /// </summary>
        /// <param name="appPoolName">程序池名称</param>
        /// <returns>true删除成功 false删除失败</returns>
        public static bool DeleteAppPool(string appPoolName)
        {
            ServerManager iisManager = new ServerManager();
            ApplicationPool appPool = iisManager.ApplicationPools[appPoolName];
            if (appPool != null)
            {
                iisManager.ApplicationPools.Remove(appPool);
                iisManager.CommitChanges();
            }
            return true;
        }

        /// <summary>
        /// 判断某一个站点是否存在
        /// </summary>
        /// <param name="webSiteName"></param>
        /// <returns></returns>
        public bool IsExistWebSite(string webSiteName)
        {
            ServerManager iisManager = new ServerManager();
            Site site = iisManager.Sites[webSiteName];
            return site != null;
        }

        /// <summary>
        /// 创建某一个站点
        /// </summary>
        /// <param name="webSiteName"></param>
        /// <param name="appPoolName"></param>
        /// <param name="schema"></param>
        /// <param name="port"></param>
        /// <param name="certHashString"></param>
        /// <param name="physicalPath"></param>
        public void CreateWebSite(string webSiteName, string appPoolName, string schema, int port, string certHashString, string physicalPath)
        {
            ServerManager iisManager = new ServerManager();
            Site site = null;
            if (string.Compare(schema, "http", StringComparison.OrdinalIgnoreCase) == 0)
            {
                site = iisManager.Sites.Add(webSiteName, schema, string.Format("*:{0}:", port), physicalPath);
            }
            else if (string.Compare(schema, "https", StringComparison.OrdinalIgnoreCase) == 0)
            {
                byte[] certHashs = certHashString.HashStringToByteArray();
                site = iisManager.Sites.Add(webSiteName, string.Format("*:{0}:", port), physicalPath, certHashs);
                //enable require SSL                  
                Configuration config = iisManager.GetApplicationHostConfiguration();
                ConfigurationSection accessSection = config.GetSection("system.webServer/security/access", webSiteName);
                accessSection["sslFlags"] = @"Ssl";
            }
            else
            {
                throw new Exception();//ToDo….;          
            }
            site.ServerAutoStart = true;
            site.Applications["/"].ApplicationPoolName = appPoolName;
            iisManager.CommitChanges();
        }

        /// <summary>
        /// 删除某一个站点
        /// </summary>
        /// <param name="webSiteName"></param>
        public void DeleteWebSite(string webSiteName)
        {
            ServerManager iisManager = new ServerManager();
            Site site = iisManager.Sites[webSiteName];
            iisManager.Sites.Remove(site);
            iisManager.CommitChanges();
        }
        #endregion
    }


}

