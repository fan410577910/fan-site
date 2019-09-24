#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  TranslatorHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/12/30 11:30:05 
     * 描述    : 翻译辅助类-google翻译，从老站拿过来改造一下。
     * =====================================================================
     * 修改时间：2014/12/30 11:30:05 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using FAN.Helper.HttpRequests;

namespace FAN.Helper
{
    /// <summary>
    /// 翻译辅助类
    /// </summary>
    public static class TranslatorHelper
    {
        private static readonly int _TranslatorSleepTime = 0;
        public static string BingClientId = string.Empty;
        public static string BingClientSecret = string.Empty;
        public static string GoogleApiKey = string.Empty;
        /// <summary>
        /// 1 为Google 2 为bing 3为Baidu
        /// </summary>
        private static readonly int TranslatorUserMethod = 0;
        static TranslatorHelper()
        {
            _TranslatorSleepTime = Math.Max(TypeParseHelper.StrToInt32(ConfigHelper.GetAppSettingValue("_TranslatorSleepTime")), 1);
            TranslatorUserMethod = Math.Max(TypeParseHelper.StrToInt32(ConfigHelper.GetAppSettingValue("TranslatorUserMethod")), 1);
            //每个站单独配置Bing的秘钥接口 add by Yang 2016-8-29 11:16:22
            BingClientId = ConfigHelper.GetAppSettingValue("BING_CLIENT_ID");
            BingClientSecret = ConfigHelper.GetAppSettingValue("BING_CLIENT_SECRET");
            if (string.IsNullOrWhiteSpace(BingClientId))
            {
                BingClientId = "TBWEBSITE";//YangHw个人账号(づ｡◕‿‿◕｡)づ
            }
            if (string.IsNullOrWhiteSpace(BingClientSecret))
            {
                BingClientSecret = "0MpRNAZCz09CIFmcO9hSe8NY+nYpHyIznSz2DneD1tA=";
            }
            GoogleApiKey = ConfigHelper.GetAppSettingValue("GOOGLE_API_KEY");
            if (_TranslatorSleepTime < 1)
                _TranslatorSleepTime = 1;
            if (TranslatorUserMethod < 1)
                TranslatorUserMethod = 1;
        }
        /// <summary>
        /// 执行翻译,默认源语言是英语
        /// </summary>
        /// <param name="text"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string Translator(string text, string to)
        {
            return Translator(text, "en", to);
        }
        public static string Translator(string text, string from, string to)
        {
            if (TranslatorUserMethod == 1)
            {
                text = Google.Translate(text, from, to);
            }
            else if (TranslatorUserMethod == 2)
            {
                text = BingTranslator.Translate(text, from, to.ToLower());
            }
            else if (TranslatorUserMethod == 3)
            {
                text = Baidu.Translate(text, from, to);
            }
            return text;
        }

        #region 谷歌翻译
        /// <summary>
        /// Google翻译
        /// </summary>
        public static class Google
        {
            /// <summary>
            /// 判断字符串中是否包含html标记
            /// </summary>
            private static readonly Regex _RegexHtmlTag = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            /// <summary>
            /// 默认使用的源语言
            /// </summary>
            private const string SOURCE_LANGUAGE = "auto";
            /// <summary>
            /// 翻译
            /// </summary>
            /// <param name="sourceText">源词</param>
            /// <param name="targetLanguageCode">要翻译的语言</param>
            /// <returns></returns>
            public static string Translate(string sourceText, string targetLanguageCode)
            {
                //google 翻译有一个很奇怪的bug，就是有些单词或者句子中包含大写字母时，有些语言会翻译不出来，顾全部强制用小写来翻译。
                return Translate(sourceText, SOURCE_LANGUAGE, targetLanguageCode);
            }
            /// <summary>
            /// 翻译
            /// </summary>
            /// <param name="sourceText">源词</param>
            /// <param name="sourceLanguageCode">源词的语言</param>
            /// <param name="targetLanguageCode">要翻译的语言</param>
            /// <returns></returns>
            public static string Translate(string sourceText, string sourceLanguageCode, string targetLanguageCode)
            {
                string targetText = string.Empty;
                if (string.IsNullOrEmpty(sourceText))
                {
                    return targetText;
                }
                sourceText = sourceText.ToLower().Replace("&nbsp;", " ");
                sourceLanguageCode = sourceLanguageCode.ToLower();
                targetLanguageCode = targetLanguageCode.ToLower();
                StringBuilder sb = new StringBuilder();
                MatchCollection matchCollection = _RegexHtmlTag.Matches(sourceText);
                int matchCollectionCount = matchCollection.Count;
                if (matchCollectionCount > 0)
                {
                    int offset = 0;
                    Match match = null;
                    for (int i = 0; i < matchCollectionCount; i++)
                    {
                        match = matchCollection[i];
                        if (offset != match.Index)
                        {
                            targetText = GoogleTranslate(sourceText.Substring(offset, match.Index - offset), sourceLanguageCode, targetLanguageCode);
                            sb.Append(targetText);
                        }
                        sb.Append(match.Value);
                        offset = match.Index + match.Length;
                    }
                    if (offset != sourceText.Length - 1)
                    {
                        targetText = GoogleTranslate(sourceText.Substring(offset), sourceLanguageCode, targetLanguageCode);
                        sb.Append(targetText);
                    }
                    match = null;
                }
                else
                {
                    targetText = GoogleTranslate(sourceText, sourceLanguageCode, targetLanguageCode);
                    sb.Append(targetText);
                }
                matchCollection = null;
                targetText = sb.ToString();
                sb.Clear();
                sb = null;
                return targetText;
            }

            /// <summary>
            /// 执行翻译请求
            /// </summary>
            /// <param name="sourceText"></param>
            /// <param name="sourceLanguageCode"></param>
            /// <param name="targetLanguageCode"></param>
            /// <returns></returns>
            private static string TranslateDo(string sourceText, string sourceLanguageCode, string targetLanguageCode)
            {
                string targetText = string.Empty;
                if (string.IsNullOrEmpty(sourceText))
                {
                    return targetText;
                }
                const string URL = "http://translate.google.cn/translate_a/single?client=t&sl={0}&tl={1}&hl=en&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&dt=at&ie=UTF-8&oe=UTF-8&otf=2&ssel=0&tsel=0&tk=519406|838853&q=";
                string url = string.Format(URL, sourceLanguageCode, targetLanguageCode);
                string json = null, parameter = null;
                JArray jArray = null;
                StringBuilder sb = new StringBuilder();
                List<string> list = sourceText.Length > 800 ? SplitStatement(sourceText) : new List<string>() { sourceText };
                foreach (string item in list)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    parameter = HttpUtility.UrlEncode(item);
                    try
                    {
                        json = WebAPIHelper.Get(string.Concat(url, parameter));
                        if (!string.IsNullOrEmpty(json))
                        {
                            jArray = JsonHelper.ConvertStrToJson<JArray>(json);
                            if (jArray != null && jArray[0] != null)
                            {
                                foreach (JArray strings in jArray[0])
                                {
                                    sb.Append(strings[0]);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    System.Threading.Thread.Sleep(TranslatorHelper._TranslatorSleepTime);
                }
                list.Clear();
                list = null;
                targetText = sb.ToString();
                sb.Clear();
                sb = null;
                return targetText;
            }


            /// <summary>
            /// 最新Google翻译 
            /// add by Yang 2016年12月23日13:19:05
            /// https://cloud.google.com/translate/docs/reference/rest#query_parameters
            /// </summary>
            /// <param name="sourceText">需要翻译的文本</param>
            /// <param name="sourceLanguageCode">输入语言 选填，https://cloud.google.com/translate/docs/languages </param>
            /// <param name="targetLanguageCode">输出语言 必填，https://cloud.google.com/translate/docs/languages </param>
            /// <returns></returns>
            private static string GoogleTranslate(string sourceText, string sourceLanguageCode, string targetLanguageCode)
            {
                StringBuilder sb = new StringBuilder();
                string targetText = string.Empty;
                if (string.IsNullOrEmpty(sourceText))
                {
                    return targetText;
                }
                const string URL = "https://translation.googleapis.com/language/translate/v2?key={0}&source={1}&target={2}&q=";
                //sourceLanguageCode 暂时固定写死为en modify by Yang 2017年9月4日10:52:08
                string url = string.Format(URL, GoogleApiKey, "en", targetLanguageCode);
                string parameter = null;
                List<string> list = sourceText.Length > 800 ? SplitStatement(sourceText) : new List<string> { sourceText };
                foreach (string item in list)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    parameter = HttpUtility.UrlEncode(item);
                    try
                    {
                        ReqeustResult request = HttpRequestHelper.Request(string.Concat(url, parameter));
                        if (!string.IsNullOrWhiteSpace(request.Html))
                        {
                            var obj = new
                            {
                                data = new
                                {
                                    translations =
                                         new[]
                                        {
                                             new
                                             {
                                                translatedText = "",
                                             }
                                        }
                                }
                            };
                            var googleTransResult = JsonConvert.DeserializeAnonymousType(request.Html, obj);
                            if (googleTransResult.data != null)
                            {
                                if (googleTransResult.data.translations != null)
                                {
                                    //targetText = googleTransResult.data.translations[0].translatedText;
                                    sb.Append(googleTransResult.data.translations[0].translatedText);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    System.Threading.Thread.Sleep(TranslatorHelper._TranslatorSleepTime);
                }
                list.Clear();
                list = null;
                targetText = sb.ToString();
                sb.Clear();
                sb = null;
                return targetText;
            }

            /// <summary>
            /// 按标点符号分割返回字符串数组
            /// </summary>
            /// <param name="sourceText">要分割的句子</param>
            /// <returns>按标点符号分割返回字符串数组</returns>
            private static List<string> SplitStatement(string sourceText)
            {
                List<string> list = null;
                if (sourceText.Length < 3)
                {
                    list = new List<string>() { sourceText };
                }
                else
                {
                    list = new List<string>();
                    int offsetStart = 0, offsetEnd = 0;
                    while ((offsetStart = sourceText.IndexOfAny(new[] { '!', '.', '?', /*',', ';', ':'*/ }, offsetEnd)) > -1)
                    {
                        list.Add(string.Concat(sourceText.Substring(offsetEnd, offsetStart - offsetEnd), sourceText[offsetStart]));
                        offsetEnd = offsetStart + 1;
                    }
                    if (offsetEnd < sourceText.Length)
                    {
                        list.Add(sourceText.Substring(offsetEnd, sourceText.Length - offsetEnd));
                    }
                }
                return list;
            }
        }
        #endregion

        #region Bing翻译

        public class BingTranslator
        {
            public static string Translate(string text, string from, string to)
            {
                string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(text) + "&to=" + to;
                string translation = GetResponseByUri(uri);
                if (translation == text)
                {
                    //可能源语言不是英语的.检查一下源语言
                    string urlDetect = "http://api.microsofttranslator.com/V2/Http.svc/Detect?text=" + HttpUtility.UrlEncode(text) + "";
                    string detect = GetResponseByUri(urlDetect);
                    uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(text) + "&from=" + detect + "&to=" + to;
                    translation = GetResponseByUri(uri);
                }
                return translation;
            }

            public static string GetResponseByUri(string uri)
            {
                AdmAccessToken admToken;
                string headerValue = "";
                AdmAuthentication admAuth = new AdmAuthentication(BingClientId, BingClientSecret);
                try
                {
                    admToken = admAuth.GetAccessToken();
                    headerValue = "Bearer " + admToken.access_token;
                }
                catch (WebException)
                {
                }
                string response = "";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers.Add("Authorization", headerValue);
                WebResponse webResponse = null;
                try
                {
                    webResponse = httpWebRequest.GetResponse();
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                        response = (string)dcs.ReadObject(stream);
                    }
                }
                catch (Exception ex)
                {
                    string aa = ex.Message;
                }
                finally
                {
                    if (webResponse != null)
                    {
                        webResponse.Close();
                        webResponse = null;
                    }
                }
                return response;
            }
        }
        #endregion

        #region 百度翻译
        public static class Baidu
        {
            private static readonly string appid = "20160708000024859";                                        // appid
            private static readonly string key = "2gH17Xp3c1L4jkBp8bQl";                                       // 密钥

            /// <summary>
            /// 百度翻译
            /// </summary>
            /// <param name="text">要翻译的文本</param>
            /// <param name="from">源语言 例如 en 更多简写参考 http://api.fanyi.baidu.com/api/trans/product/apidoc </param>
            /// <param name="to">目标语言 例如 zh</param>
            /// <returns></returns>
            public static string Translate(string text, string from, string to)
            {
                string result = text;
                if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                {
                    return result;
                }
                string body = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(text));
                string salt = "1";                                                                                // 随机数
                string sign = MD5Util.Md5(string.Concat(appid, body, salt, key));                                 // 签名
                body = HttpUtility.UrlEncode(body);                                                    // 生成签名之后，发送HTTP请求之前
                string url = string.Format("http://api.fanyi.baidu.com/api/trans/vip/translate?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}", body, from.ToLower(), to.ToLower(), appid, salt, sign.ToLower());
                ReqeustResult reqeustResult = HttpRequestHelper.Request(url, new RequestData() { Method = RequestMethods.Get });
                //根据API返回的Json格式 构建匿名对象
                var obj = new
                {
                    from = "",
                    to = "",
                    trans_result =
                        new[]
                        {
                                new
                                {
                                    src= "",
                                    dst = ""
                                }
                        }
                };
                var baiduTransResult = JsonConvert.DeserializeAnonymousType(reqeustResult.Html, obj);
                //判断是否正确的返回翻译结果
                if (baiduTransResult.trans_result != null)
                {
                    result = baiduTransResult.trans_result[0].dst;
                }
                return result;
            }

        }
        #endregion
    }
    public class AdmAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
    }

    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string clientId;
        private string cientSecret;
        private string requestParams;

        public AdmAuthentication(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.cientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            this.requestParams = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
        }

        public AdmAccessToken GetAccessToken()
        {
            return this.HttpPost(DatamarketAccessUri, this.requestParams);
        }

        private AdmAccessToken HttpPost(string dataMarketAccessUri, string requestParams)
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(dataMarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(requestParams);
            webRequest.ContentLength = bytes.Length;
            using (Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }
}
