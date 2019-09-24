#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  DomainRoute 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间：2014/8/14
     * 描述    : 使用ASP.NET的UrlRouting配置URL格式，支持泛域名的UrlRouting
     * =====================================================================
     * 修改时间：2014/8/14
     * 修改人  ：  
     * 版本号  ：V1.0.0.0 
     * 描述    ：使用ASP.NET的UrlRouting配置URL格式，支持泛域名的UrlRouting
*/
#endregion
using System.Collections.Generic;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;

namespace FAN.UrlRouting
{
    /// <summary>
    /// 支持泛域名的UrlRouting
    /// </summary>
    public class DomainRoute : RouteBase
    {
        #region 变量
        private string _domainName;
        private string _physicalFile;
        private string _routeUrl;
        private bool _checkPhysicalUrlAccess = false;
        private RouteValueDictionary _defaults;
        private RouteValueDictionary _constraints;
        private IList<PathSegment> _pathSegmentLists = new List<PathSegment>();
        private const string REPLACE_PATTEN = @"([\w,%]+)";
        private readonly Regex _patten = new Regex(@"\{([a-z,A-Z,0-9]+)\}", RegexOptions.Compiled);
        private int _segmentCount = 0;
        #endregion

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName">泛域名</param>
        /// <param name="routeUrl">Url路由</param>
        /// <param name="physicalFile">映射的物理文件</param>
        /// <param name="checkPhysicalUrlAccess">一个值，该值指示 ASP.NET 是否应验证用户是否有权访问物理 URL（始终会检查路由 URL）。此参数设置 System.Web.Routing.PageRouteHandler.CheckPhysicalUrlAccess</param>
        /// <param name="defaults">路由的默认值。</param>
        /// <param name="constraints">一些约束，URL 请求必须满足这些约束才能作为此路由处理。</param>

        public DomainRoute(string domainName, string routeUrl, string physicalFile, bool checkPhysicalUrlAccess, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            this._domainName = domainName.ToLower();
            this._routeUrl = routeUrl;
            this._physicalFile = physicalFile;
            this._checkPhysicalUrlAccess = checkPhysicalUrlAccess;
            this._defaults = defaults;
            this._constraints = constraints;

            IList<string> lists = SplitUrlToPathSegmentStrings(routeUrl);
            if (lists != null && lists.Count > 0)
            {
                this._segmentCount = lists.Count;
                for (int i = 0; i < lists.Count; i++)
                {
                    string strPatten = lists[i];
                    if (!string.IsNullOrWhiteSpace(strPatten) && this._patten.IsMatch(strPatten))
                    {
                        PathSegment segment = new PathSegment();
                        segment.Index = i;

                        Match match;
                        List<string> valueNames = new List<string>();
                        for (match = this._patten.Match(strPatten); match.Success; match = match.NextMatch())
                        {
                            strPatten = strPatten.Replace(match.Groups[0].Value, REPLACE_PATTEN);
                            valueNames.Add(match.Groups[1].Value);
                        }
                        segment.ValueNames = valueNames.ToArray();
                        segment.Regex = new Regex(strPatten, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        this._pathSegmentLists.Add(segment);
                    }
                }
            }
        }

        public DomainRoute(string domainName, string routeUrl, string physicalFile)
            : this(domainName, routeUrl, physicalFile, false, new RouteValueDictionary(), new RouteValueDictionary())
        {

        }
        #endregion

        #region 属性
        /// <summary>
        /// 泛域名
        /// </summary>
        public string DomainName
        {
            get { return this._domainName; }
            set { this._domainName = value; }
        }
        /// <summary>
        /// 映射的物理文件
        /// </summary>
        public string PhysicalFile
        {
            get { return this._physicalFile; }
            set { this._physicalFile = value; }
        }
        /// <summary>
        /// Url路由
        /// </summary>
        public string RouteUrl
        {
            get { return this._routeUrl; }
            set { this._routeUrl = value; }
        }
        #endregion

        #region 方法
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData result = null;
            HttpRequestBase request = httpContext.Request;
            if (request.Url.Host.ToLower().Contains(this._domainName))
            {
                string virtualPath = request.AppRelativeCurrentExecutionFilePath.Substring(2) + request.PathInfo;
                IList<string> segmentUrl = SplitUrlToPathSegmentStrings(virtualPath);
                if (segmentUrl.Count == this._segmentCount)
                {
                    PathSegment pathSegment = null;
                    string path = null;
                    bool isOK = true;
                    for (int i = 0; i < this._pathSegmentLists.Count; i++)
                    {
                        pathSegment = this._pathSegmentLists[i];
                        path = segmentUrl[pathSegment.Index];
                        if (!pathSegment.Regex.IsMatch(path))
                        {
                            isOK = false;
                            break;
                        }
                    }
                    if (isOK)
                    {
                        result = new RouteData(this, new PageRouteHandler(this._physicalFile, this._checkPhysicalUrlAccess));
                        //result.Values.Add("Domain", this._domainName);
                        Match match = null;
                        for (int i = 0; i < this._pathSegmentLists.Count; i++)
                        {
                            pathSegment = this._pathSegmentLists[i];
                            path = segmentUrl[pathSegment.Index];
                            match = pathSegment.Regex.Match(path);
                            if (pathSegment.ValueNames.Length + 1 == match.Groups.Count)
                            {
                                for (int j = 0; j < pathSegment.ValueNames.Length; j++)
                                {
                                    result.Values.Add(pathSegment.ValueNames[j], match.Groups[j + 1].Value);
                                }
                            }
                        }
                    }
                }
                segmentUrl.Clear();
                segmentUrl = null;
            }
            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return new VirtualPathData(this, this._physicalFile);
        }

        private static IList<string> SplitUrlToPathSegmentStrings(string url)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(url))
            {
                int index;
                for (int i = 0; i < url.Length; i = index + 1)
                {
                    index = url.IndexOf('/', i);
                    if (index == -1)
                    {
                        string str = url.Substring(i);
                        if (str.Length > 0)
                        {
                            list.Add(str);
                        }
                        return list;
                    }
                    string item = url.Substring(i, index - i);
                    if (item.Length > 0)
                    {
                        list.Add(item);
                    }
                    //list.Add("/");
                }
            }
            list.TrimExcess();
            return list;
        }
        #endregion

        #region 内部类
        private class PathSegment
        {
            public int Index { get; set; }
            public Regex Regex { get; set; }
            public string[] ValueNames { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// 支持泛域名的UrlRouting(与上一个相同)
    /// </summary>
    public class DomainRoute1 : Route
    {
        private string _domainName;
        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName">泛域名</param>
        /// <param name="routeUrl">Url路由</param>
        /// <param name="physicalFile">映射的物理文件</param>
        /// <param name="checkPhysicalUrlAccess">一个值，该值指示 ASP.NET 是否应验证用户是否有权访问物理 URL（始终会检查路由 URL）。此参数设置 System.Web.Routing.PageRouteHandler.CheckPhysicalUrlAccess</param>
        /// <param name="defaults">路由的默认值。</param>
        /// <param name="constraints">一些约束，URL 请求必须满足这些约束才能作为此路由处理。</param>

        public DomainRoute1(string domainName, string routeUrl, string physicalFile, bool checkPhysicalUrlAccess, RouteValueDictionary defaults, RouteValueDictionary constraints):base(routeUrl,defaults,constraints,new PageRouteHandler(physicalFile, checkPhysicalUrlAccess))
        {
            this._domainName = domainName.ToLower();            
        }
        #endregion
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            HttpRequestBase request = httpContext.Request;
            if (request.Url.Host.ToLower().Contains(this._domainName))
            {
                RouteData rd = base.GetRouteData(httpContext);
                return rd;
            }
            return null;
        }
    }


}
