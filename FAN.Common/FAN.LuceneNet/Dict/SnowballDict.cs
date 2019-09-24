#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  SnowballDict
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 词干字典
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using SF.Snowball;
using SF.Snowball.Ext;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TLZ.LuceneNet
{
    /// <summary>
    /// 词干字典
    /// </summary>
    public static class SnowballDict
    {
        private static Dictionary<string, SnowballProgram> _dictSnowball = new Dictionary<string, SnowballProgram>();
        private static Dictionary<string, string> _dictStemmer = new Dictionary<string, string>();

        static SnowballDict()
        {
            _dictSnowball.Add("DA", new DanishStemmer());//丹麦语
            _dictSnowball.Add("NL", new DutchStemmer());//荷兰语
            _dictSnowball.Add("EN", new EnglishStemmer());//英语
            _dictSnowball.Add("FI", new FinnishStemmer());//芬兰语
            _dictSnowball.Add("FR", new FrenchStemmer());//法语
            _dictSnowball.Add("DE2", new German2Stemmer());//德语2
            _dictSnowball.Add("DE", new GermanStemmer());//德语
            _dictSnowball.Add("HU", new HungarianStemmer());
            _dictSnowball.Add("IT", new ItalianStemmer());
            _dictSnowball.Add("文斯语", new LovinsStemmer());
            _dictSnowball.Add("NO", new NorwegianStemmer());
            _dictSnowball.Add("波特语", new PorterStemmer());//英语的
            _dictSnowball.Add("PT", new PortugueseStemmer());//葡萄牙语
            _dictSnowball.Add("RO", new RomanianStemmer());
            _dictSnowball.Add("RU", new RussianStemmer());//俄语
            _dictSnowball.Add("ES", new SpanishStemmer());//西班牙语
            _dictSnowball.Add("SV", new SwedishStemmer());
            _dictSnowball.Add("TR", new TurkishStemmer());//土耳其语

            _dictStemmer.Add("DA", "Danish");//丹麦语
            _dictStemmer.Add("NL", "Dutch");//荷兰语
            _dictStemmer.Add("EN", "English");//英语
            _dictStemmer.Add("FI", "Finnish");//芬兰语
            _dictStemmer.Add("FR", "French");//法语
            _dictStemmer.Add("DE2", "German2");//德语2
            _dictStemmer.Add("DE", "German");//德语
            _dictStemmer.Add("HU", "Hungarian");
            _dictStemmer.Add("IT", "Italian");
            _dictStemmer.Add("文斯语", "Lovins");
            _dictStemmer.Add("NO", "Norwegian");
            _dictStemmer.Add("波特语", "Porter");//英语的
            _dictStemmer.Add("PT", "Portuguese");//葡萄牙语
            _dictStemmer.Add("RO", "Romanian");
            _dictStemmer.Add("RU", "Russian");//俄语
            _dictStemmer.Add("ES", "Spanish");//西班牙语
            _dictStemmer.Add("SV", "Swedish");
            _dictStemmer.Add("TR", "Turkish");//土耳其语
        }
        /// <summary>
        /// 获取词干对象(线程不安全)
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        //public static SnowballProgram GetSnowball(string language)
        //{
        //    if (_dictSnowball.ContainsKey(language))
        //        return _dictSnowball[language];
        //    return null;
        //}
        /// <summary>
        /// 获取词干对象(线程安全)
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static SnowballProgram GetSnowball(string language)
        {
            SnowballProgram result = null;
            switch (language)
            {
                case "DA":
                    result = new DanishStemmer();
                    break;//丹麦语
                case "NL":
                    result = new DutchStemmer();
                    break;//荷兰语
                case "EN":
                    result = new EnglishStemmer();
                    break;//英语
                case "FI":
                    result = new FinnishStemmer();
                    break;//芬兰语
                case "FR":
                    result = new FrenchStemmer();
                    break;//法语
                case "DE2":
                    result = new German2Stemmer();
                    break;//德语2
                case "DE":
                    result = new GermanStemmer();
                    break;//德语
                case "HU":
                    result = new HungarianStemmer();
                    break;
                case "IT":
                    result = new ItalianStemmer();
                    break;
                case "文斯语":
                    result = new LovinsStemmer();
                    break;
                case "NO":
                    result = new NorwegianStemmer();
                    break;
                case "波特语":
                    result = new PorterStemmer();
                    break;//英语的
                case "PT":
                    result = new PortugueseStemmer();
                    break;//葡萄牙语
                case "RO":
                    result = new RomanianStemmer();
                    break;
                case "RU":
                    result = new RussianStemmer();
                    break;//俄语
                case "ES":
                    result = new SpanishStemmer();
                    break;//西班牙语
                case "SV":
                    result = new SwedishStemmer();
                    break;
                case "TR":
                    result = new TurkishStemmer();
                    break;//土耳其语
            }
            return result;
        }

        /// <summary>
        /// 获取词干对象
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetStemmer(string language)
        {
            if (_dictStemmer.ContainsKey(language))
                return _dictStemmer[language];
            return null;
        }

    }
}
