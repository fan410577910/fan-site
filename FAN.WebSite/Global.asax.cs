using AutoMapper;
using AutoMapper.Configuration;
using FAN.Admin.App_Start;
using FAN.Helper;
using FAN.WebSite.Code;
using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Routing;

namespace FAN.WebSite
{
    public class Global : HttpApplication
    {
        private static readonly MapperConfigurationExpression MAPPER_CONFIGURATION = new MapperConfigurationExpression();

        void Application_Start(object sender, EventArgs e)
        {
            UrlRoutingBus.RegisterRoutes(RouteTable.Routes);
            //添加对象映射关系
            this.CreateMaps();
        }

        /// <summary>
        /// 添加对象映射关系
        /// </summary>
        private void CreateMaps()
        {
            //https://www.cnblogs.com/fred-bao/p/5700776.html
            //https://www.cnblogs.com/caoyc/p/6367828.html
            //MAPPER_CONFIGURATION.CreateMap<UserAddress, OrderAddress>().ReverseMap():对象间可以相互替换
            //f=>f.Condition(src=>src.Age>=0 && src.Age<=200)   符合条件才映射属性
            //f=>f.Ignore():忽略当前属性
            MAPPER_CONFIGURATION.CreateMap<UserAddress, OrderAddress>()
                .ForMember(d => d.OrderCountryName, f => f.MapFrom(src => src.CountryName))//设置名称映射
                .ForMember(d => d.Name, f => f.MapFrom(src => src.FirstName + " " + src.LastName))//映射
                                                                                                  //.ForMember(d => d.OrderCountryName, f => f.NullSubstitute("China!!"));
                                                                                                  //.ForMember(d=>d.Age,f=>f.Condition(s=>s.Age<60));//符合条件才映射属性
                                                                                                  //.ForMember(d => d.Age, f => f.Ignore());//排除映射属性

            ;
            //类型转换
            MAPPER_CONFIGURATION.CreateMap<int, string>().ConvertUsing(item => item.ToString());
            MAPPER_CONFIGURATION.CreateMap<string, int>().ConvertUsing(item => TypeParseHelper.StrToInt32(item));


            MAPPER_CONFIGURATION.CreateMap<UserAddress, UserAddress>();
            MAPPER_CONFIGURATION.CreateMap<OrderAddress, OrderAddress>();
            AutoMapper.Mapper.Initialize(MAPPER_CONFIGURATION);

        }
        public override void Init()
        {
            base.PreSendRequestHeaders += this.Global_PreSendRequestHeaders;
            base.Init();
        }
        void Global_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (sender is HttpApplication)
            {
                (sender as HttpApplication).Context.Response.Headers.Remove("Server");
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string a = "";
        }
    }

}