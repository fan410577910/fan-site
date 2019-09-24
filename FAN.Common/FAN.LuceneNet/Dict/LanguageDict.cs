#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  LanguageDic 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/12/29 10:00:23 
     * 描述    : 获取多语言信息
     * =====================================================================
     * 修改时间：2014/12/29 10:00:23 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TLZ.LuceneNet
{
    public class LanguageDict
    {
        public const string LANGUAGE_FILE_NAME = "LanguageDict.xml";
        private static List<Language> _LanguageList = null;
        private static string _applicationPath = null;
        static LanguageDict()
        {
            InitDict();
        }

        private static void InitDict()
        {
            _applicationPath = Path.Combine(LuceneNetConfig.LuceneDictDirectory, LANGUAGE_FILE_NAME);
            if (File.Exists(_applicationPath))
            {
                Encoding encoding = EncodingType.GetType(_applicationPath);
                try
                {
                    _LanguageList = SerializeHelper.XmlDeserialize<List<Language>>(_applicationPath, encoding);
                }
                catch
                {
                    _LanguageList = new List<Language>();
                }
            }
        }
        public static void SaveLanguageXML(List<Language> customScoreInfoList)
        {
            SaveLanguageXML(_applicationPath, customScoreInfoList);
        }
        public static void SaveLanguageXML(string path, List<Language> customScoreInfoList)
        {
            try
            {
                SerializeHelper.XmlSerialize(path, customScoreInfoList);
            }
            catch (Exception ex)
            {
                throw new Exception("保存" + LANGUAGE_FILE_NAME + "失败了！" + ex.Message);
            }
        }
        /// <summary>
        /// 根据所有语言信息
        /// </summary>
        public static List<Language> LanguageList
        {
            get { return LanguageDict._LanguageList; }
        }
    }
}
