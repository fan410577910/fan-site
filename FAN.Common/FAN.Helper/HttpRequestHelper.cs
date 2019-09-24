﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Net.Http;
using FAN.Helper.HttpRequests;

namespace FAN.Helper
{
    public class HttpRequestHelper
    {
        public const string CONTENT_BOUNDARY = "----------ae0cH2cH2GI3Ef1KM7GI3Ij5cH2gL6";
        public const string CONTENT_BOUNDARY_PREFIX = "--";


        public static ReqeustResult Request(string url)
        {
            return Request(url, new RequestData());
        }

        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static ReqeustResult Request(string url, RequestData requestData)
        {
            MemoryStream postStream = null;
            BinaryWriter postWriter = null;
            HttpWebResponse response = null;
            StreamReader responseStream = null;
            if (requestData == null)
            {
                return null;
            }
            Encoding requestEncoding = requestData.RequestEncoding;
            Encoding responseEncoding = requestData.ResponseEncoding;
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //UserAgent
                request.UserAgent = requestData.UserAgent;
                //ContentType
                if (requestData.ContentType == "multipart/form-data;")
                    request.ContentType = "multipart/form-data; boundary=" + CONTENT_BOUNDARY;
                else
                    request.ContentType = requestData.ContentType;

                //UserAgent
                request.Accept = requestData.Accept;
                //Connection
                request.KeepAlive = requestData.KeepAlive;

                //设置代理
                if (requestData.WebProxy != null)
                    request.Proxy = requestData.WebProxy;


                //设置过期时间
                request.Timeout = 300000;
                if (request.Timeout > 0)
                    request.Timeout = requestData.Timeout;

                //Shoespie Instagram 解决限制抓取临时方案 Yang 2018-4-11 11:10:31
                // request.Headers.Add("Cookie", "mid=WsnSsAALAAFcGbGvMbQrQ7-U-6uk; ig_pr=1; ig_vw=1920; ig_vh=464; ig_or=landscape-primary; csrftoken=Y8XBHkGwTombBiMnkWTdX3ToFanoa7cu; ds_user_id=843030856; rur=FTW; sessionid=IGSC838e962903bfb91fa2e220447cf715834ba1a376023217d013cf0ae57f0e48bd%3Avp70NXXkcKTE398XvWJK6zc8QScEYHq5%3A%7B%22_auth_user_id%22%3A843030856%2C%22_auth_user_backend%22%3A%22accounts.backends.CaseInsensitiveModelBackend%22%2C%22_auth_user_hash%22%3A%22%22%2C%22_platform%22%3A4%2C%22_token_ver%22%3A2%2C%22_token%22%3A%22843030856%3AH9VqbqFDOLGeSLTLdDz7G4Ddxkhlhzvt%3A5d4966f8736331628831679f514995ba6c4536964911c1c39c45efdf0d233786%22%2C%22last_refreshed%22%3A1523411363.3383610249%7D; 9238; shbid=9238;");

                //请求类型
                request.Method = requestData.Method == RequestMethods.Get ? "Get" : "POST";

                //初始化头部信息
                InitHeaders(request, requestData);

                //写表单数据
                postStream = new MemoryStream();
                postWriter = new BinaryWriter(postStream);
                if (requestData.FormValue != null && requestData.FormValue.Count > 0)
                {
                    if (requestData.ContentType == "multipart/form-data;")
                        WriteMultipartFormData(postWriter, requestData.FormValue, requestEncoding);
                    else
                        WriteFormData(postWriter, requestData.FormValue, requestEncoding);
                }
                request.ContentLength = postStream.Length;
                if (requestData.Method == RequestMethods.Post)
                {
                    Stream requestStream = request.GetRequestStream();
                    postStream.WriteTo(requestStream);
                }

                response = (HttpWebResponse)request.GetResponse();
                responseStream = new StreamReader(response.GetResponseStream(), responseEncoding);
                ReqeustResult result = new ReqeustResult
                {
                    Html = responseStream.ReadToEnd(),
                    StatusCode = (int)response.StatusCode
                };

                for (int i = 0; i < response.Headers.Count; i++)
                {
                    Header header = new Header
                    {
                        Key = response.Headers.Keys[i],
                        Value = response.Headers[i]
                    };
                    result.Headers.Add(header);
                }
                return result;
            }
            catch (Exception ex)
            {
                ReqeustResult result = new ReqeustResult { StatusCode = -1, Html = ex.Message };
                return result;
            }
            finally
            {
                if (postWriter != null)
                    postWriter.Close();

                if (postStream != null)
                {
                    postStream.Close();
                    postStream.Dispose();
                }
                if (response != null)
                    response.Close();

                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream.Dispose();
                }
            }
        }

        private static void WriteFormData(BinaryWriter postWriter, List<FormValue> formValues, Encoding requestEncoding)
        {
            string temp = "";
            temp = EncodeValue(formValues[0].Name, formValues[0].Value, requestEncoding);
            for (int i = 1; i < formValues.Count; i++)
            {
                temp += "&" + EncodeValue(formValues[i].Name, formValues[i].Value, requestEncoding);
            }
            postWriter.Write(GetBytes(requestEncoding, temp));
        }

        private static void WriteMultipartFormData(BinaryWriter postWriter, IEnumerable<FormValue> formValues, Encoding requestEncoding)
        {
            foreach (FormValue formValue in formValues)
            {
                if (formValue.BinaryData != null && formValue.BinaryData.Length > 0)
                {
                    postWriter.Write(GetBytes(requestEncoding, CONTENT_BOUNDARY_PREFIX, CONTENT_BOUNDARY, "\r\n", string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\";\r\n\r\n", formValue.Name, formValue.Value)));
                    postWriter.Write(formValue.BinaryData);
                    postWriter.Write(GetBytes(requestEncoding, "\r\n"));
                }
                else
                {
                    postWriter.Write(GetBytes(requestEncoding, CONTENT_BOUNDARY_PREFIX, CONTENT_BOUNDARY, "\r\n", string.Format("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n", formValue.Name)));
                    postWriter.Write(GetBytes(requestEncoding, formValue.Value));
                    postWriter.Write(GetBytes(requestEncoding, "\r\n"));
                }
            }
            postWriter.Write(GetBytes(requestEncoding, CONTENT_BOUNDARY_PREFIX, CONTENT_BOUNDARY, "--"));
        }

        private static string EncodeValue(string name, string value, Encoding encodeEncoding)
        {
            return string.Format("{0}={1}", HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(value));
        }

        private static void InitHeaders(HttpWebRequest request, RequestData requestData)
        {
            if (requestData != null && requestData.Headers != null && requestData.Headers.Count > 0)
            {
                foreach (Header header in requestData.Headers)
                {
                    InitHeaders(request, header.Key, header.Value);
                }
            }
        }

        private static void InitHeaders(HttpWebRequest request, string key, string value)
        {
            if (String.Compare(key, "Accept", StringComparison.OrdinalIgnoreCase) == 0)
                request.Accept = value;
            else if (String.Compare(key, "User-Agent", StringComparison.OrdinalIgnoreCase) == 0)
                request.UserAgent = value;
            else if (String.Compare(key, "Referer", StringComparison.OrdinalIgnoreCase) == 0)
                request.Referer = value;
            else
                request.Headers[key] = value;
        }

        /// <summary>
        /// 返回UTF8编码
        /// </summary>
        /// <param name="encoding">返回编码值</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        private static byte[] GetBytes(Encoding encoding, params string[] content)
        {
            string temp = "";
            for (int i = 0; i < content.Length; i++)
            {
                temp += content[i];
            }
            return encoding.GetBytes(temp);
        }

        private static HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="requestUri">请求地址</param>
        /// <returns>以字符串形式返回正文</returns>
        public static async System.Threading.Tasks.Task<string> Get(string requestUri)
        {
            return await _httpClient.GetStringAsync(requestUri);
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<T> Get<T>(string requestUri)
        {
            T t = default(T);
            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                t = await response.Content.ReadAsAsync<T>();
            }
            return t;
        }


        /// <summary>
        /// 使用应用程序/x-www-form-urlencoded MIME 类型编码的名称/值元组的容器。
        /// </summary>
        /// <param name="requestUri">请求地址</param>
        /// <param name="context">请求内容</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<T> Post<T>(string requestUri, FormUrlEncodedContent context)
        {
            T t = default(T);
            HttpResponseMessage response = await _httpClient.PostAsync(requestUri, context);
            if (response.IsSuccessStatusCode)
            {
                t = await response.Content.ReadAsAsync<T>();
            }
            return t;
        }

        /// <summary>
        /// 发送POST请求
        /// </summary>
        /// <param name="requestUri">请求地址</param>
        /// <param name="context">请求内容 使用 application/x-www-form-urlencoded 类型进行编码的内容提供容器</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task<string> Post(string requestUri, FormUrlEncodedContent context)
        {
            string result = null;
            HttpResponseMessage response = await _httpClient.PostAsync(requestUri, context);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }


        public static async System.Threading.Tasks.Task<bool> SendAsync(HttpRequestMessage request)
        {
            var responseMessage = await _httpClient.SendAsync(request);
            return responseMessage.StatusCode == HttpStatusCode.OK;
        }

    }//end class

}
