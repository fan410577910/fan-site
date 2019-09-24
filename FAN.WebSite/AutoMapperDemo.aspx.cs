using FAN.Helper;
using FAN.WebSite.Code;
using System;
using System.Collections.Generic;

namespace FAN.WebSite
{
    public partial class AutoMapperDemo : BasePage
    {
        protected string UserEmail { get; set; }
        protected override string TemplateName
        {
            get
            {
                return "webform1.vm";
            }
        }
        public AutoMapperDemo() : base(false, true)
        {
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void InitDict()
        {
            UserAddress userAddress = new UserAddress {
                Name = "张三",
                FirstName="fname",
                LastName="lname",
                Age=10,
               CountryName="美国"
            };
            OrderAddress orderAddress = new OrderAddress() { Other="hahah"}; 
            AutoMapper.Mapper.Map(userAddress, orderAddress);


            UserAddress userAddress1 = AutoMapper.Mapper.Map<UserAddress>(userAddress);

            List<UserAddress> userAddressList = new List<UserAddress>() { new UserAddress {
                Name = "张三",
                Age=10,
                CountryName="美国"
            }};
            IEnumerable<OrderAddress> orderAddressList = AutoMapper.Mapper.Map<IEnumerable<OrderAddress>>(userAddressList);
            
            base.Dict.Add("JSON", JsonHelper.ConvertJsonToStr(orderAddress));//
            base.InitDict();
        }
    
    }
    public class UserAddress {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string CountryName { get; set; }
    }
    public class OrderAddress
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string OrderCountryName { get; set; }
        public string Other { get; set; }
             
    }
}