#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  LuceneBus 
     * 版本号：  V1.0.0.0 
     * 创建人：  Administrator 
     * 创建时间：2014/10/29 9:51:41 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/10/29 9:51:41 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.IO;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 清除网站生成的静态页面
    /// </summary>
    partial class LuceneBus
    {
        /// <summary>
        /// 清除网站生成的静态页面
        /// </summary>
        public static void ClearDirectory()
        {
            string dir = LuceneNetConfig.LuceneWebPageDirectory;
            if (Directory.Exists(dir))
            {
                foreach (string item in Directory.GetFileSystemEntries(dir, "*.*", SearchOption.TopDirectoryOnly))
                {
                    if (File.Exists(item))
                    {
                        try
                        {
                            File.Delete(item);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        DeleteFolder(item);
                    }
                }
            }
        }

        /// <summary>
        /// 删除文件和文件夹
        /// </summary>
        /// <param name="dir"></param>
        private static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir))
            {
                try
                {
                    Directory.Delete(dir, true);
                }
                catch { }
            }
        }
    }
}
