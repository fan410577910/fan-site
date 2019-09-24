#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：YANGHUANWEN 
     * 文件名：  ImageHelper 
     * 版本号：  V1.0.0.0 
     * 创建人：  Administrator 
     * 创建时间：2014/10/13 18:26:52 
     * 描述    :
     * =====================================================================
     * 修改时间：2014/10/13 18:26:52 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;

namespace FAN.Helper
{
    public class ImageHelper
    {

        /// <summary>
        /// 获取产品图片列表下标
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static int GetImageIndex(string image)
        {
            int idx = 1;
            if (string.IsNullOrEmpty(image))
                return idx;
            if (image.EndsWith(".jpg") == false)
                return idx;
            if (image.IndexOf("_", StringComparison.Ordinal) == -1)
                return idx;
            image = image.Replace(".jpg", "");
            image = image.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[1];
            idx = int.Parse(image);
            return idx;

        }

        /// <summary>
        /// tidebuy所有网站图片水印统一从这里出（婚纱离婚站除外）
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <param name="watermark"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string GetImageUrl(string originalUrl, string watermark, string width, string height)
        {
            string url = null;
            if (!string.IsNullOrEmpty(originalUrl) && RegexHelper.IsUrl(originalUrl))
            {
                //wangyunpeng 增加webp,暂时不能使用，主要是多数浏览器还不支持 + 图片的CDN缓存webp格式会有问题。
                //if (originalUrl.LastIndexOf(".jpg", StringComparison.CurrentCultureIgnoreCase) > 0)
                //{
                //    originalUrl += ".page.webp";
                //}
                Uri uri = new Uri(originalUrl);
                List<string> segmentList = uri.Segments.ToList();
                if (segmentList.Count > 1)
                {
                    if (!string.IsNullOrWhiteSpace(watermark))
                    {
                        segmentList.Insert(segmentList.Count - 1, watermark + "/");
                    }
                    List<string> parameterList = new List<string>(2);
                    int w = TypeParseHelper.StrToInt32(width);
                    int h = TypeParseHelper.StrToInt32(height);
                    if (w > 0 || h > 0)
                    {
                        parameterList.Add(w.ToString());
                        parameterList.Add(h.ToString());
                    }
                    if (string.IsNullOrWhiteSpace(watermark) && parameterList.Count > 0)
                    {
                        segmentList.Insert(segmentList.Count - 1, string.Join("-", parameterList) + "/");
                    }
                    parameterList.Clear();
                    parameterList = null;
                    UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port);
                    uriBuilder.Path = string.Join("", segmentList);
                    url = uriBuilder.Uri.ToString();
                    segmentList.Clear();
                }
                segmentList = null;
            }
            else
            {
                url = string.Empty;
            }
            return url;
        }
        /// <summary>
        /// 包含婚纱礼服类目的站点需要加水印和尺寸
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <param name="watermark"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string GetImageUrlByWedding(string originalUrl, string watermark, string width, string height)
        {
            string url = null;
            if (!string.IsNullOrEmpty(originalUrl) && RegexHelper.IsUrl(originalUrl))
            {
                Uri uri = new Uri(originalUrl);
                List<string> segmentList = uri.Segments.ToList();
                if (segmentList.Count > 1)
                {
                    if (!string.IsNullOrWhiteSpace(watermark))
                    {
                        segmentList.Insert(segmentList.Count - 1, watermark + "/");
                    }
                    List<string> parameterList = new List<string>(2);
                    int w = TypeParseHelper.StrToInt32(width);
                    int h = TypeParseHelper.StrToInt32(height);
                    if (w > 0 || h > 0)
                    {
                        parameterList.Add(w.ToString());
                        parameterList.Add(h.ToString());
                    }
                    if (parameterList.Count > 0) //eric加上图片水印和尺寸同时截图。wangyunpeng.2016-10-22。2017-3-30，取消使用。
                    {
                        segmentList.Insert(segmentList.Count - 1, string.Join("-", parameterList) + "/");
                    }
                    parameterList.Clear();
                    parameterList = null;
                    UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port);
                    uriBuilder.Path = string.Join("", segmentList);
                    url = uriBuilder.Uri.ToString();
                    segmentList.Clear();
                }
                segmentList = null;
            }
            else
            {
                url = string.Empty;
            }
            return url;
        }
    }
}
