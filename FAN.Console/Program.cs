using FAN.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FAN.Console
{
    [Serializable]
    public class User_address
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Name4 { get; set; }
        public string Name5 { get; set; }
        public string Name6 { get; set; }
        public Action<ShopCartInnerProductModel, int> func1 { get; set; }
    }
    public enum EShopCart_ValidType
    { }

    /// <summary>
    /// 购物车页面里面的每一个购车产品
    /// </summary>
    [Serializable]
    public class ShopCartInnerProductModel
    {
        public ShopCartInnerProductModel()
        {
            FavorateState = false;
        }
        /// <summary>
        /// 购物车ID
        /// </summary>
        public int ShopCartID { get; set; }
        public int PID { get; set; }
        public int SPUID { get; set; }
        public int SKUID { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public DateTime InToCartTime { get; set; }
        /// <summary>
        /// 产品的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 产品的标题URL
        /// </summary>
        public string TitleURL { get; set; }
        /// <summary>
        /// 图片的URL
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 产品单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 产品单价(美金)
        /// </summary>
        public decimal USDUnitPrice { get; set; }
        /// <summary>
        /// 产品总价
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SellPrice { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MarketPrice { get; set; }
        /// <summary>
        /// 产品的最终价格
        /// </summary>
        public decimal LastPrice { get; set; }
        /// <summary>
        /// 购买的数量
        /// </summary>
        public int BuyCount { get; set; }
        /// <summary>
        /// 产品必选区未解析
        /// </summary>
        public string SpecificationCode { get; set; }
        /// <summary>
        /// 产品必选区已解析
        /// </summary>
        public string SpecificationText { get; set; }
        /// <summary>
        /// 必选区描述文本 
        /// </summary>
        public string Required { get; set; }
        /// <summary>
        /// 是否在页面显示（赠品可以在后台设置不显示）
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 用户留言
        /// </summary>
        public string LeaveMessage { get; set; }
        /// <summary>
        /// 购物车拼接的购买数量部分的Html
        /// </summary>
        public string ShopCartCount { get; set; }
        /// <summary>
        /// 购物车中商品的类型枚举
        /// </summary>
        public int ItemType { get; set; }
        /// <summary>
        ///定额定价活动的分组ID
        /// </summary>
        [JsonIgnore]
        public int GroupID { get; set; }
        /// <summary>
        /// 包装重量(参考值) [单位:KG]
        /// </summary>
        public decimal PackageWeight { get; set; }
        /// <summary>
        /// 包装重量(参考值) [单位:g]
        /// </summary>
        public int PackageWeightG { get; set; }
        /// <summary>
        /// 多语言ID  根据单个购物车的产品显示
        /// </summary>
        [JsonIgnore]
        public int CultureID { get; set; }

        public string ProductCultrueURl { get; set; }

        /// <summary>
        /// 活动规则ID（product_sku_activepriceformula的主键）
        /// </summary>
        [JsonIgnore]
        public int ActiveRuleID { get; set; }
        /// <summary>
        /// 单位类型(厘米：0，英尺：1) 
        /// </summary>
        public int UnitSize { get; set; }
        /// <summary>
        /// 同图片色必选区值
        /// </summary>
        [JsonIgnore]
        public string SameImageColor { get; set; }
        /// <summary>
        /// 产品同图片色图片URL
        /// </summary>
        public string ProductSameImageUrl { get; set; }
        /// <summary>
        /// 闪购ID
        /// </summary>
        public int PriceFlashID { get; set; }
        /// <summary>
        /// 每日特价ID
        /// </summary>
        public int PriceDailySpecialID { get; set; }
        /// <summary>
        /// 是否包邮
        /// </summary>
        public int IsFreeShipping { get; set; }
        /// <summary>
        /// 是否被收藏
        /// </summary>
        public int IsFavorite { get; set; }
        /// <summary>
        /// 类目ID
        /// </summary>
        public string LeiMuIds { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 叶级类目ID
        /// </summary>
        public string LeiMuLeafIds { get; set; }
        /// <summary>
        /// 是否需要免费包装盒
        /// value:(0:否,1:是 )
        /// </summary>
        [JsonIgnore]
        public int IsFreePackaging { get; set; }
        /// <summary>
        /// 加价购活动ID
        /// </summary>
        public int PriceIncreaseID { get; set; }

        ///// <summary>
        ///// 失效原因（0:有效、1：下架、2：sku失效、3：产品库存为0）
        ///// </summary>
        public EShopCart_ValidType ValidType { get; set; }

        /// <summary>
        /// 是否有效（如果不是上架或定时上架则为false）
        /// </summary>
        [Obsolete("统一使用ValidType")]
        public bool IsValid { get; set; }

        /// <summary>
        /// 是否有效sku
        /// </summary>
        [Obsolete("统一使用ValidType")]
        public bool IsValidSku { get; set; }
        /// <summary>
        /// 预售活动提示信息
        /// </summary>
        public string PromotionTip { get; set; }
        /// <summary>
        /// 预售活动结束天数
        /// </summary>
        public int PromotionEndDays { get; set; }
        /// <summary>
        /// 预售类型 0区间预售、1集赞预售
        /// </summary>
        public int PromotionPresaleType { get; set; }
        /// <summary>
        /// 预售活动集赞数 or 购买人数
        /// </summary>
        public int PromotionPresaleValue { get; set; }
        /// <summary>
        /// 是否显示APPOnly图标（如果APP价格比其他平台价格低则为true）
        /// </summary>
        public bool IsShowAppOnly { get; set; }
        public decimal AppPrice { get; set; }
        /// <summary>
        /// 购物车商品收藏状态
        /// </summary>
        public bool FavorateState { set; get; }

        /// <summary>
        /// 购物车最大修改数量
        /// </summary>
        public int MaxBuyCount { set; get; }
        /// <summary>
        /// 最小起订量
        /// </summary>
        public int MinBuyCount { set; get; }
        /// <summary>
        /// 活动商品库存（闪购、每日特价）
        /// </summary>
        public int PromotionProductStock { get; set; }
        /// <summary>
        /// 商品库存
        /// </summary>
        public int ProductStock { get; set; }
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool IsSelected { get; set; } = true;

    }
    public class Special_User_address : User_address
    {
        public string SpecialName { get; set; }
    }
    /// <summary>
    /// 筛选抽象基类
    /// </summary>
    [Serializable]
    public class FieldFilter
    {
        public FieldFilter()
        {
            this.Sort = 0;
            this.Close = true;
        }

        public bool Close { get; set; }

        public int Sort { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段过滤先后顺序
        /// </summary>
        internal int FieldWherePriority { get; set; }
        /// <summary>
        /// 字段排序先后顺序
        /// </summary>
        internal int FieldOrderByPriority { get; set; }

    }

    public class IDFieldFilter<T> : FieldFilter where T : struct
    {
        public IDFieldFilter() { }
        public IDFieldFilter(T value) { this.Value = value; }
        public IDFieldFilter(T? value) { this.Value = value; }

        public T? Value { get; set; }

        #region Operator
        public static implicit operator IDFieldFilter<T>(T value)
        {
            return new IDFieldFilter<T>(value);
        }
        public static implicit operator IDFieldFilter<T>(T? value)
        {
            return new IDFieldFilter<T>(value);
        }
        public static implicit operator T(IDFieldFilter<T> value)
        {
            return value.Value.Value;
        }
        public static implicit operator T? (IDFieldFilter<T> value)
        {
            return value.Value;
        }

        #endregion

    }

    public class FilterBase
    { }
    public class SignActiveDataFilter : FilterBase
    {
        public IDFieldFilter<int> ID { get; set; }
    }

    public class MyList : List<string> { }
    public abstract class AbstractExpression
    {
        public abstract string ToString();
    }
    public class HttpContex
    {
        public string _request = null;
        public string _response = null;
        public string Request => this._request;
        public string Response => this._response;
    }
    public delegate Task RequestDelegate(HttpContex context);

    public enum EAddress_Load_Flags
    {
        加载产品 = 0,
        加载配送方式 = 1,
        加载支付方式 = 2,
        加载活动 = 4
    }
    class Program
    {
        private static bool _isStop = false;
        private static object lockObj = new object();
        static void Main(string[] args)
        {
            #region 泛型Demo
            //List<MenuInfo> menuInfoList = new List<MenuInfo>();
            //menuInfoList.Add(new MenuInfo { ID=1,ParentID=0,Name="1",Description="1级"});
            //menuInfoList.Add(new MenuInfo { ID = 2, ParentID = 0, Name = "2", Description = "1级" });
            //menuInfoList.Add(new MenuInfo { ID = 3, ParentID = 0, Name = "3", Description = "1级" });
            //menuInfoList.Add(new MenuInfo { ID = 4, ParentID = 1, Name = "4", Description = "2级" });
            //menuInfoList.Add(new MenuInfo { ID = 5, ParentID = 1, Name = "5", Description = "2级" });
            //menuInfoList.Add(new MenuInfo { ID = 6, ParentID = 4, Name = "6", Description = "3级" });
            //List<TreeData> treeData = 泛型Demo.GetTreeData(menuInfoList, 0, 3,t=>t.ID);


            //List<bool> bList = 泛型Demo.SplitToList<bool>("true,false,true,false");
            #endregion

            long t = DateTime.Now.Ticks;
            Thread.Sleep(3000);
            System.Console.WriteLine((DateTime.Now - new DateTime(t)).TotalMilliseconds); 

            
            System.Console.ReadKey(); //strList.Cast
        }
        static void Test<T>(IMyComparable<T> t1, T t2)
        {
            //省略
        }



        public static string BaseString()
        {
            string clientId = "xishuai";
            string clientSecret = "123";
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret));
        }
        private static void test(int id)
        {
            int id1 = id;
        }
        static void ThreadDemo()
        {
            int count = 0;
            Thread t1 = new Thread(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    lock (lockObj)
                    {
                        int temp = count;
                        Thread.Sleep(1);
                        count = temp + 1;

                    }
                    System.Console.WriteLine("t1-" + count);

                }
            });
            t1.Start();//开始执行
                       //t1.IsBackground = true;//后台线程
                       //t1.Priority = ThreadPriority.Highest;//线程优先级
                       //bool isAlive = t1.IsAlive;//线程是否在执行
                       //t1.Join();//当前线程等待t1线程执行结束，然后在运行当前线程
                       //t1.Interrupt();//唤醒Sleep中的线程，Sleep方法会抛出ThreadInterruptedException异常，需要catch异常并处理
                       //t1.Abort();//终止t1线程，并抛出ThreadAbortException异常



            Thread t2 = new Thread(() =>
            {

                for (int i = 0; i < 100; i++)
                {
                    lock (lockObj)
                    {
                        int temp = count;
                        Thread.Sleep(1);
                        count = temp + 1;

                    }
                    System.Console.WriteLine("t2-" + count);
                }
            });
            t2.Start();
        }
        /// <summary>
        /// 信号ManualResetEvent  案例
        /// </summary>
        static void ManualResetEventDemo()
        {
            //信号：线程通讯工具，信号提供了线程通讯的机制。开门（Set）、关门（Reset）、等着开门（WaitOne）
            //AutoResetEvent:
            ManualResetEvent mre = new ManualResetEvent(false);//false 表示“初始状态为关门”
            Thread t3 = new Thread(() =>
            {
                System.Console.WriteLine("开始等着开门");
                mre.WaitOne();
                //WaitHandle.WaitAll(WaitHandler[] waitHandlers);//等待所有信号变成“开门状态”
                //WaitHandle.WaitAny(WaitHandler[] waitHandlers);//等待任意一个信号变成“开门状态”
                System.Console.WriteLine("终于等到你");
            });
            t3.Start();
            System.Console.WriteLine("按任意键开门");
            System.Console.ReadKey();
            mre.Set();//开门
            //mre.Reset();//关门
        }
        /// <summary>
        /// 信号AutoResetEvent  案例
        /// </summary>
        static void AutoResetEventDemo()
        {
            //AutoResetEvent与ManualResetEvent的区别是 WaitOne()后自动关门
            AutoResetEvent are = new AutoResetEvent(false);
            Thread t1 = new Thread(() =>
            {
                while (true)
                {
                    System.Console.WriteLine("开始等着开门");
                    are.WaitOne();
                    System.Console.WriteLine("终于等到你");
                }
            });
            t1.Start();
            System.Console.WriteLine("按任意键开门");
            System.Console.ReadKey();
            are.Set();//开门
            System.Console.WriteLine("按任意键开门");
            System.Console.ReadKey();
            are.Set();
            System.Console.WriteLine("按任意键开门");
            System.Console.ReadKey();
            are.Set();
            System.Console.ReadKey();
        }
    }


    public interface IMyComparable<T>
    {
        int Compare(T other);
    }

    public class Employee : IMyComparable<Employee>
    {
        public string Name { get; set; }
        public int Compare(Employee other)
        {
            return Name.CompareTo(other.Name);
        }
    }

    public class Programmer : Employee, IMyComparable<Programmer>
    {
        public int Compare(Programmer other)
        {
            return Name.CompareTo(other.Name);
        }
    }

    public class Manager : Employee
    {
    }


    public class Person : IComparable<Person>, IDisposable
    {
        public int Type { get; set; }

        public int CompareTo(Person p)
        {
            if (this.Type > p.Type)
            {
                return 1;
            }
            else if (this.Type == p.Type)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    public class TypeCompare : IComparer<Person>
    {
        private readonly static List<int> typeSorts = new List<int> { 5, 1, 3 };
        public int Compare(Person x, Person y)
        {
            if (typeSorts.IndexOf(x.Type) > typeSorts.IndexOf(y.Type))
            {
                return 1;
            }
            else if (typeSorts.IndexOf(x.Type) == typeSorts.IndexOf(y.Type))
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }

    public class Parent
    {
        public virtual void Say()
        {

            System.Console.WriteLine("Parent Say");
        }
    }
    public class Child : Parent
    {
        public new void Say()
        {
            base.Say();
            System.Console.WriteLine("Child Say");
        }
    }
}
