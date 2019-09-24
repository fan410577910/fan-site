using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Search.CreateIndex.Host
{
    partial class LuceneNetBus
    {
        /// <summary>
        /// 获取Lucene目录
        /// </summary>
        /// <returns></returns>
        public static string GetLuceneDirectory()
        {
            string luceneDirectory = null;
            if (LuceneDirectory.LastIndexOf(Lucene_1_Directory) > 0)
            {
                luceneDirectory = LuceneDirectory.Replace(Lucene_1_Directory, Lucene_2_Directory);
            }
            else if (LuceneDirectory.LastIndexOf(Lucene_2_Directory) > 0)
            {
                luceneDirectory = LuceneDirectory.Replace(Lucene_2_Directory, Lucene_1_Directory);
            }
            else
            {
                luceneDirectory = IOHelper.CombinePath(LuceneDirectory, Lucene_1_Directory);
            }
            Global.Logger.LogWithTime("GetLuceneDirectory()::luceneDirectory=" + luceneDirectory, Logger.ELogLevel.Trace);
            return luceneDirectory;
        }

        public static string Change()
        {
            string message = null;
            try
            {
                string luceneDirectory = GetLuceneDirectory();
                ConfigHelper.SaveAppSettingValue(LuceneServiceConfig, LuceneNetConfig.LUCENE_DIRECTORY, luceneDirectory); //修改Lucene.Config文件
                InitConfig();
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }
            return message;
        }
    }
}
