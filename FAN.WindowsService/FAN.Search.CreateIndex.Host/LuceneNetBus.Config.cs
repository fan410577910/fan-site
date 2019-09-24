using FAN.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAN.LuceneNet;
namespace FAN.Search.CreateIndex.Host
{
    internal partial class LuceneNetBus
    {
        private static string LuceneDirectory = null;
        private static string LuceneDictDirectory = null;
        private static string WebStaticPageDirectory = null;
        private static string LuceneServiceConfig = null;
        private static string Lucene_1_Directory = null;
        private static string Lucene_2_Directory = null;

        static LuceneNetBus()
        {
            LuceneNetConfig.ConfigChangedEvent += LuceneNetConfig_ConfigChangedEvent;
            InitConfig();
        }

        static void LuceneNetConfig_ConfigChangedEvent()
        {
            InitConfig();
        }

        private static void InitConfig()
        {
            LuceneServiceConfig = ConfigHelper.GetAppSettingValue("LuceneServiceConfig");

            if (string.IsNullOrWhiteSpace(LuceneServiceConfig))
            {
                throw new SystemException("appSetting->LuceneServiceConfig必须设置有效值");
            }
            if (!IOHelper.IsExistFilePath(LuceneServiceConfig))
            {
                throw new SystemException(string.Format("appSetting->LuceneServiceConfig设置的文件 {0} 不存在", LuceneServiceConfig));
            }

            LuceneDirectory = ConfigHelper.GetAppSettingValue(LuceneServiceConfig, LuceneNetConfig.LUCENE_DIRECTORY);
            if (string.IsNullOrWhiteSpace(LuceneDirectory))
            {
                throw new SystemException(string.Format("{0}.config文件必须设置appSetting->{1}的值", LuceneServiceConfig, LuceneNetConfig.LUCENE_DIRECTORY));
            }
            LuceneDictDirectory = ConfigHelper.GetAppSettingValue(LuceneServiceConfig, LuceneNetConfig.LUCENE_DICT_DIRECTORY);
            if (string.IsNullOrWhiteSpace(LuceneDictDirectory))
            {
                throw new SystemException(string.Format("{0}.config文件必须设置appSetting->{1}的值", LuceneServiceConfig, LuceneNetConfig.LUCENE_DICT_DIRECTORY));
            }

            WebStaticPageDirectory = ConfigHelper.GetAppSettingValue(LuceneServiceConfig, LuceneNetConfig.LUCENE_WEBPAGE_DIRECTORY);
            if (string.IsNullOrWhiteSpace(WebStaticPageDirectory))
            {
                throw new SystemException(string.Format("{0}.config文件必须设置appSetting->{1}的值", LuceneServiceConfig, LuceneNetConfig.LUCENE_WEBPAGE_DIRECTORY));
            }

            Lucene_1_Directory = ConfigHelper.GetAppSettingValue("Lucene_1");
            if (string.IsNullOrWhiteSpace(Lucene_1_Directory))
            {
                Lucene_1_Directory = "Lucene_1";
            }

            Lucene_2_Directory = ConfigHelper.GetAppSettingValue("Lucene_2");
            if (string.IsNullOrWhiteSpace(Lucene_2_Directory))
            {
                Lucene_2_Directory = "Lucene_2";
            }
            LuceneNetConfig.LuceneDirectory = LuceneDirectory;
            LuceneNetConfig.LuceneDictDirectory = LuceneDictDirectory;
        }
    }
}
