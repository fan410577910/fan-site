#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2018 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-TI50KE6KO4 
     * 文件名：  JsonManager 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间： 2018/5/31 20:39:56 
     * 描述    :
     * =====================================================================
     * 修改时间：2018/5/31 20:39:56 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using FAN.Helper;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Mvc;

namespace FAN.Admin.Components
{
    /// <summary>
    /// 只是支持asp.net mvc方式使用JQuery.EasyUI返回数据的操作
    /// </summary>
    public class JsonManager
    {
        private static JsonErrorType JsonErrorType = JsonErrorType.Message;
        private const string CONTENT_TYPE = "application/json";

        /// <summary>
        /// 成功实体
        ///     信息显示操作成功
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static JsonResult GetSuccess()
        {
            return GetSuccess(null, "操作成功！");
        }
        /// <summary>
        /// 成功实体
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public static JsonResult GetSuccess(string msg)
        {
            return GetSuccess(null, msg);
        }
        /// <summary>
        /// 成功实体
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static JsonResult GetSuccess(object data)
        {
            return GetSuccess(data, null);
        }
        /// <summary>
        /// 创建一个成功的json返回实体
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        /// <returns>JsonModel实体</returns>
        public static JsonResult GetSuccess(object data, string msg)
        {
            return Get(new JsonModel()
            {
                Code = 0,
                Data = data,
                Error = null,
                Success = true,
                Message = msg
            });
        }
        /// <summary>
        /// 创建一个失败的json实体
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public static JsonResult GetError(int code, string msg)
        {
            return GetError(code, msg, (string)null);
        }

        /// <summary>
        /// 创建一个失败的json实体
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="error">错误信息</param>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public static JsonResult GetError(int code, string msg, string error)
        {
            return GetError(code, msg, error == null ? null : new string[1] { error });
        }
        /// <summary>
        /// 创建一个失败的json实体
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="error">错误信息</param>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public static JsonResult GetError(int code, string msg, string[] error)
        {
            return Get(new JsonModel()
            {
                Code = code,
                Data = null,
                Error = error,
                Success = false,
                Message = msg
            });
        }
        /// <summary>
        /// 创建一个失败的json实体
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static JsonResult GetError(int code, Exception error)
        {
            return GetError(code, error == null ? null : error.Message, error);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static JsonResult GetError(int code, string msg, Exception error)
        {
            return GetError(code, msg, new string[1] { GetException(error) });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static JsonResult GetError(int code, string msg, Exception[] error)
        {
            return Get(new JsonModel()
            {
                Code = code,
                Data = null,
                Error = GetException(error),
                Success = false,
                Message = msg
            });
        }
        //
        public static JsonResult Get(JsonModel jsonModel)
        {
            if (jsonModel == null)
            {
                throw new ArgumentNullException("jsonModel");
            }
            return new JsonResult()
            {
                Data = jsonModel,
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 显示原始传入的数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult Raw(object data)
        {
            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// 显示原始传入的数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ActionResult Raw(string data)
        {
            return new ContentResult() { Content = data, ContentType = CONTENT_TYPE };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private static string GetException(Exception error)
        {
            if (error == null) return null;
            switch (JsonErrorType)
            {
                case JsonErrorType.Message:
                    return error.Message;
                case JsonErrorType.Stack:
                    return error.StackTrace;
                case JsonErrorType.Detail:
                default:
                    return error.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static string[] GetException(Exception[] errors)
        {
            if (errors == null)
            {
                return null;
            }
            string[] exceptions = new string[errors.Length];
            for (var i = 0; i < errors.Length; i++)
            {
                exceptions[i] = GetException(errors[i]);
            }
            return exceptions;
        }

        private static readonly JsonSerializerSettings _MicrosoftDateFormatSettings = new JsonSerializerSettings
        {
#if DEBUG
            Formatting = Newtonsoft.Json.Formatting.Indented,
#endif
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        /// <summary>
        /// .net数据类型序列化对象到json字符串
        /// </summary>
        /// <param name="objectValue"></param>
        /// <returns></returns>
        public static string Serialize(object objectValue)
        {
            return JsonHelper.ConvertJsonToStr(objectValue, _MicrosoftDateFormatSettings);
        }
        /// <summary>
        /// 反序列化json字符串到.net数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonString)
        {
            return JsonHelper.ConvertStrToJson<T>(jsonString);
        }
        /// <summary>
        /// 反序列化json字符串到.net数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static object Deserialize(string jsonString)
        {
            return JsonHelper.ConvertStrToObject(jsonString);
        }
        public static object Deserialize(string jsonString, string type)
        {
            return JsonHelper.ConvertStrToJson(jsonString, type);
        }
        public static object Deserialize(string jsonString, Type type)
        {
            return JsonHelper.ConvertStrToJson(jsonString, type);
        }

        /// <summary>
        /// 反序列化json字符串到匿名对象
        /// YangHuanWen  2014-06-03
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonString, T anonymousTypeObject)
        {
            return JsonHelper.ConvertStrToT<T>(jsonString, anonymousTypeObject);
        }

        /// <summary>
        /// Web Api 输出转Json
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static HttpResponseMessage WebApiJson(object @object)
        {
            return WebApiJson(@object, 0);
        }

        /// <summary>
        /// Web Api 输出转Json
        /// </summary>
        /// <param name="object"></param>
        /// <param name="code">TLZ.COM.TBDress.WebSite.Mobile.WebAPI.Code.EWebApiCode枚举类型</param>
        /// <returns></returns>
        public static HttpResponseMessage WebApiJson(object @object, int code)
        {
            string jsonStr = JsonHelper.ConvertJsonToStr(new JsonModel
            {
                Code = code,
                Data = @object,
                Error = null,
                Success = true,
                Message = null
            }, _MicrosoftDateFormatSettings);
            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(jsonStr, System.Text.Encoding.UTF8, CONTENT_TYPE),
            };
            return result;
        }

        /// <summary>
        /// Web Api 输出转Json异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static HttpResponseMessage WebApiJsonError(string message, int code)
        {
            string jsonStr = JsonHelper.ConvertJsonToStr(new JsonModel
            {
                Code = code,
                Data = null,
                Error = null,
                Success = false,
                Message = message
            }, _MicrosoftDateFormatSettings);
            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(jsonStr, System.Text.Encoding.UTF8, CONTENT_TYPE),
            };
            return result;
        }
    }

    /// <summary>
    /// json通信实体
    /// </summary>
    [Serializable]
    public class JsonModel
    {
        /// <summary>
        /// 错误码(0表示无错误)
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string[] Error { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Json数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 服务器响应时间 
        /// </summary>
        public long ResponseTicks
        {
            get
            {
                return DateTime.Now.Ticks;
            }
        }
    }

    /// <summary>
    /// Json错误类型
    /// </summary>
    public enum JsonErrorType
    {
        /// <summary>
        /// 提示消息
        /// </summary>
        Message = 0,
        /// <summary>
        /// 堆栈
        /// </summary>
        Stack = 1,
        /// <summary>
        /// 详细
        /// </summary>
        Detail = 2
    }
}