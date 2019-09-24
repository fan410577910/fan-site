#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2018 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-TI50KE6KO4 
     * 文件名：  BasePage 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间： 2018/5/12 15:40:51 
     * 描述    :
     * =====================================================================
     * 修改时间：2018/5/12 15:40:51 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using FAN.Helper;
using FAN.Nvelocity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace FAN.WebSite.Code
{
    public abstract class BasePage:Page
    {
        #region 变量 & 属性
        protected Dictionary<string, object> _dict = new Dictionary<string, object>(); 
        /// <summary>
        /// 是否使用本地缓存（如果不使用本地缓存则使用redis缓存）
        /// </summary>
        protected bool IsUseLocalCache { get; set; }
        /// <summary>
        /// 是否生成静态页面(如果静态页没有数据，需要设置成false)
        /// </summary>
        protected bool IsGenerateFile { get; set; }
        protected Dictionary<string, object> Dict
        {
            get { return this._dict; }
        }
        protected abstract string TemplateName { get; }
        #endregion
        #region constructor
        public BasePage(bool isGenerateFile,bool isUseLocalCache) : base()
        {
            this.IsGenerateFile = isGenerateFile;
            this.IsUseLocalCache = isUseLocalCache;
        }
        public BasePage(bool isGenerateFile): this(isGenerateFile, true)
        {
        }
        #endregion
        protected override void OnLoad(EventArgs e)
        {
            this.InitDict();            
            NVelocityBus.Print(base.Request,base.Response,this.TemplateName, this._dict, this.IsGenerateFile, this.IsUseLocalCache);
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            this._dict.Clear();
            this._dict = null;
        }
        /// <summary>
        /// 根据目录设置当前目录里所有文件的版本  字典键值为（父级目录名（只包含最近一级的父级类目名称）_文件名_文件扩展名）
        /// </summary>
        /// <param name="staticFilePath">要获取的静态文件上级目录名</param>
        private void SetStaticFilesVersion(string staticFilePath)
        {
            //获取静态文件地址
            FileInfo[] staticFileInfos = IOHelper.GetFileInfos(staticFilePath, "*.*", SearchOption.AllDirectories);
            if (staticFileInfos == null)
            {
                return;

            }
            StringBuilder sb = new StringBuilder();
            foreach (FileInfo staticFileInfo in staticFileInfos)
            {
                //拼接key
                if (staticFileInfo.Directory != null)
                {
                    sb.Append(staticFileInfo.Directory.Name);
                }
                sb.Append("_");
                sb.Append(IOHelper.GetFileNameWithoutExtension(staticFileInfo.Name));
                sb.Append("_");
                sb.Append(staticFileInfo.Extension.Replace(".", string.Empty));
                string fileNameKey = sb.ToString().Replace('.', '_').ToUpper();
                sb.Clear();
                //判断是否存在key，加入最后修改时间
                if (!this._dict.Keys.Contains(fileNameKey))
                {
                    this._dict.Add(fileNameKey, "_" + staticFileInfo.LastWriteTime.ToString("yyyyMMddHHmmss"));
                }
            }
            sb = null;
            Array.Clear(staticFileInfos, 0, staticFileInfos.Length);
            staticFileInfos = null;
        }
        protected virtual void InitDict()
        {
            //静态文件项目域名
            this.Dict.Add("STATIC_HOST", ConfigSetting.STATIC_HOST);
            //静态文件时间戳
            this.SetStaticFilesVersion(ConfigSetting.STATICFILE_PATH_SCRIPT);
            this.SetStaticFilesVersion(ConfigSetting.STATICFILE_PATH_CSS);

        }



    }
}