
using System.ComponentModel;
namespace FAN.Enum
{
    /// <summary>
    /// 本类里面用到的枚举，统一重新建，一律不允许在使用。wangyunpeng
    /// </summary>
    [System.Obsolete("本类里面用到的枚举，统一重新建，一律不允许在使用。wangyunpeng")]
    public class EnumType
    {
        /// <summary>
        /// app版本控制用
        /// </summary>
        public enum AppPlatForm
        {
            Android = 1,
            IOS = 2
        }
        public enum EAccount_UserStatus
        {
            正常 = 0,
            冻结 = 1,
            注销 = 2
        }

        public enum EShippingType
        {
            非免运费 = 0,
            免运费 = 1
        }

        /// <summary>
        /// 所有enum类别集合
        /// </summary>
        public enum EAllType
        {
            SellType,
            ProductState,
            FakeLevel,
            UserState,
            PayType,
            ExpressType,
            ProductUnit,
            UserType,
            LogType,
            ShippingMethod,
            WebUrl,
            CategoryDescType
        }
        public enum EPaymentType
        {
            Payment,
            CustomPayment
        }

        public enum EUserUploadType
        {
            /// <summary>
            /// 用户上传的评论图片
            /// </summary>
            UserUploadCommentImage,
            /// <summary>
            /// 用户上传的图片
            /// </summary>
            UserUploadImages
        }
        public enum EExpressType
        {
            标准运费,
            免运费,
            不支持,
            不显示
        }
        public enum ECurrency
        {
            /// <summary>
            /// 美元
            /// </summary>
            USD = 0,
            /// <summary>
            /// 欧元
            /// </summary>
            EUR = 1,
            /// <summary>
            /// 日元
            /// </summary>
            JPY = 2,
            /// <summary>
            /// 英镑
            /// </summary>
            GBP = 3,
            /// <summary>
            /// 澳元
            /// </summary>
            AUD = 4,
            /// <summary>
            /// 港币
            /// </summary>
            HKD = 5,
            /// <summary>
            /// 瑞士法郎
            /// </summary>
            CHF = 6,
            /// <summary>
            /// 加元
            /// </summary>
            CAD = 7
            ,
            /// <summary>
            /// 丹麦克朗
            /// </summary>
            DKK = 8
             ,
            /// <summary>
            /// 挪威克朗
            /// </summary>
            NOK = 9
             ,
            /// <summary>
            /// 瑞典克朗
            /// </summary>
            SEK = 10
             ,
            /// <summary>
            /// 墨西哥比索
            /// </summary>
            MXN = 11
        }
        public enum EWebUrl
        {
            无 = 0,
            tbdress = 1,
            oilpaintingclub = 2,
            wigsbuy = 3,
            dressbuying = 4,
            sexydonut = 5,
            dressv = 6,
            dressvenus = 7,
            faerydress = 8,
            yodress = 9,
            dressesa = 10,
            ericdress = 11,
            yuptattoo = 12,
            tidestore = 13,
            shouji = 14,//手机
            jewelhall = 15,
            bagsq = 16,
            dressyours = 17,
            shoespie = 18,
            beddinginn = 19,
            mochmoda = 20,
            wigsu = 21,
            markchic = 32

        }
        public enum ECategoryDescType
        {
            全部 = 0,
            运输 = 1,
            尺码 = 2,
            色卡 = 3,
            测量 = 4,
            FAQs = 5,
            公用描述 = 6,
            Return_Policy = 7,
            Quality_Guarantee = 8,
            Machine_Processing = 9,
            Assembling = 10,
            Tattoo_Info = 11,
            Wash_Care = 12
        }
        public enum ECategoryDescType_en
        {
            Descriptions = 0,
            Shipping_Methods = 1,
            Size_Chart = 2,
            Color_Chart = 3,
            Measure = 4,
            FAQs = 5,
            Description = 6,
            Return_Policy = 7,
            Quality_Guarantee = 8,
            Machine_Processing = 9,
            Assembling = 10,
            Tattoo_Info = 11,
            Wash_Care = 12
        }
        public enum EShippingMethod
        {
            EMS,
            DHL,
            Fedex,
            TNT,
            UPS,
            Big,
            Small
        }
        /// <summary>
        /// 买卖类别
        /// </summary>
        public enum ESellType
        {
            零售和批发 = 1,
            只零售 = 2,
            只批发 = 3
        }

        /// <summary>
        /// 产品状态
        /// </summary>
        public enum EProductState
        {
            //下架 = 0,
            //上架 = 1,
            //促销 = 2,
            //每日一款 = 4

            下架 = 0,
            上架 = 1,
            促销 = 2,
            清仓 = 3,
            每日一款或预售 = 4,
            产品测试 = 5,
            团购 = 6,
            秒杀 = 7,
            删除 = -4,
            隐藏 = -5,
            未翻译 = -6,
            待上架 = -100,
            CN隐藏产品 = -15,
            定时上架 = -70,
            未翻译兼职 = -88,
            已翻译兼职 = -86

        }

        /// <summary>
        /// 防货级别
        /// </summary>
        public enum EFakeLevel
        {
            正品 = 0,
            一级仿货 = 1,
            二级仿货 = 2,
            三级仿货 = 3
        }
        /// <summary>
        /// 用户状态
        /// </summary>
        public enum EUserState
        {
            黑名单 = 0,
            正常 = 1,
            限制登陆 = 2
        }
        /// <summary>
        /// 帐户类别
        /// </summary>
        public enum EUserType
        {
            普通帐户 = 0,
            测试帐户 = 1,
            供应商账户 = 2
        }
        /// <summary>
        /// 推荐商品类别
        /// </summary>
        public enum ERecommendedType
        {
            文章类型 = 0,
            图片类型 = 1
        }
        /// <summary>
        /// 支付类型
        /// </summary>
        public enum EEPayType
        {
            PayPal = 0,
            信用卡 = 1,
            手动确认 = 2
        }

        /// <summary>
        /// 图片类型
        /// </summary>
        public enum EPicType
        {
            产品图片 = 1,
            上传的图片 = 2,
            用户上传图片 = 3,
            用户产品评论图片 = 4,
            用户自定义上传的图片 = 5
        }
        /// <summary>
        /// 留言状态
        /// </summary>
        public enum EMessageState
        {
            未处理 = 0,
            处理中 = 1,
            已处理 = 2
        }
        public enum EProductUnit
        {
            Unit = 1,
            Lot = 2,
            Set = 3
        }
        public enum ELogType
        {
            登陆 = 0,
            登出 = 1,
            增加 = 2,
            删除 = 3,
            修改 = 4,
            审批 = 5,
            错误 = 6
        }
        public enum ECreateHTMLType
        {
            首页 = 1
        }
        public enum EProductHot
        {
            销售 = 0,
            浏览 = 1,
            分类热销 = 2
        }
        /// <summary>
        /// 用户中心消息类型
        /// </summary>
        public enum EMessageType
        {
            Product = 0,
            Order = 1,
            Disputed = 2,
            System = 3,
            Outbox = 4
        }

        public enum EEOrderState
        {
            全部 = -1,
            等待付款 = 1,
            已付款 = 2,
            备货期 = 3,
            正在补货 = 14,
            等待发货 = 15,
            已发货 = 16,
            等待确认收货 = 17,
            等待评价 = 18,
            已完成订单 = 19,
            取消订单 = 20,
            争议订单 = 21,
            待审核付款 = 22,
            已付款未确定 = 23,
            加急备货中订单 = 24,
            加急已付款订单 = 25,
            已退款 = 26
        }
        public enum EWigType
        {
            Human_Hair_Wigs1,
            Human_Hair_Wigs2,
            Human_Hair_Wigs3,
            Synthetic_Wigs1,
            Synthetic_Wigs2,
            Synthetic_Wigs3
        }

        public enum EBlogType
        {
            Article = 1,
            Picture = 2,
            Video = 3
        }

        ///// <summary>
        ///// 读取枚举的值
        ///// </summary>
        ///// <param name="type">枚举类型</param>
        ///// <param name="index">索引</param>
        ///// <returns></returns>
        public static string GetValue(EAllType type, int index)
        {
            string value = "";
            if (type == EAllType.SellType)//买卖类别
            {
                EnumType.ESellType[] us = (EnumType.ESellType[])System.Enum.GetValues(typeof(EnumType.ESellType));
                value = us[index].ToString();
            }
            else if (type == EAllType.ProductState)//产品状态
            {
                EnumType.EProductState[] us = (EnumType.EProductState[])System.Enum.GetValues(typeof(EnumType.EProductState));
                value = us[index].ToString();
            }
            else if (type == EAllType.FakeLevel)//防货级别
            {
                EnumType.EFakeLevel[] us = (EnumType.EFakeLevel[])System.Enum.GetValues(typeof(EnumType.EFakeLevel));
                value = us[index].ToString();
            }
            else if (type == EAllType.UserState)//用户状态
            {
                EnumType.EUserState[] us = (EnumType.EUserState[])System.Enum.GetValues(typeof(EnumType.EUserState));
                value = us[index].ToString();
            }
            else if (type == EAllType.PayType)//支付类型
            {
                EnumType.EEPayType[] us = (EnumType.EEPayType[])System.Enum.GetValues(typeof(EnumType.EEPayType));
                value = us[index].ToString();
            }
            else if (type == EAllType.ExpressType)//快递类型
            {
                EnumType.EExpressType[] us = (EnumType.EExpressType[])System.Enum.GetValues(typeof(EnumType.EExpressType));
                value = us[index].ToString();
            }
            else if (type == EAllType.ProductUnit)//产品单位
            {
                EnumType.EProductUnit[] us = (EnumType.EProductUnit[])System.Enum.GetValues(typeof(EnumType.EProductUnit));
                value = us[index].ToString();
            }
            else if (type == EAllType.UserType)
            {
                EnumType.EUserType[] us = (EnumType.EUserType[])System.Enum.GetValues(typeof(EnumType.EUserType));
                value = us[index].ToString();
            }
            else if (type == EAllType.LogType)
            {
                EnumType.ELogType[] us = (EnumType.ELogType[])System.Enum.GetValues(typeof(EnumType.ELogType));
                value = us[index].ToString();
            }
            else if (type == EAllType.ShippingMethod)
            {
                EnumType.EShippingMethod[] us = (EnumType.EShippingMethod[])System.Enum.GetValues(typeof(EnumType.EShippingMethod));
                value = us[index].ToString();
            }
            else if (type == EAllType.ShippingMethod)
            {
                EnumType.EShippingMethod[] us = (EnumType.EShippingMethod[])System.Enum.GetValues(typeof(EnumType.EShippingMethod));
                value = us[index].ToString();
            }
            else if (type == EAllType.WebUrl)
            {
                EnumType.EWebUrl[] us = (EnumType.EWebUrl[])System.Enum.GetValues(typeof(EnumType.EWebUrl));
                value = us[index].ToString();
            }
            else if (type == EAllType.CategoryDescType)
            {
                EnumType.ECategoryDescType[] us = (EnumType.ECategoryDescType[])System.Enum.GetValues(typeof(EnumType.ECategoryDescType));
                value = us[index].ToString();
            }
            return value;
        }
        /// <summary>
        /// 排序类型
        /// </summary>
        public enum ESortType
        {
            Recommended,
            BestSelling,
            NewArrival,
            Discount,
            LowPrice,
            HighPrice
        }

        /// <summary>
        /// 列表显示方式枚举
        /// </summary>
        public enum ERenderDisplay
        {
            grid = 1,
            list = 2
        }
        public enum EOrderTrackStep
        {
            生成订单,
            选择paypal付款,
            选择线下付款,
            选择西联,
            选择银转,
            待审核付款,
            审核付款成功,
            已付款,
            提交工厂备货,
            子订单入库,
            子订单发货,
            子订单确认收货,
            订单发货,
            确认收货,
            订单评价,
            争议订单,
            子订单已退款,
            订单已退款
        }


        public enum ELanguageType
        {
            英语 = 1,
            法语 = 2,
            西语 = 3,
            日语 = 4,
            阿拉伯语 = 5,
            德语 = 6,
            意大利语 = 7,
            葡萄牙语 = 8,
            俄语 = 9,
            挪威语 = 10,
            丹麦语 = 11,
            瑞典语 = 12,
            芬兰语 = 13,
            波兰语 = 14,
            韩语 = 15,
            荷兰语 = 16,

        }



        /// <summary>
        /// 主订单状态
        /// </summary>
        public enum EOrderState
        {
            等待付款 = 1,
            已付款 = 2,
            取消订单 = 20,
            待审核付款 = 22,
            已付款未确定 = 23

        }
        /// <summary>
        /// 子单状态
        /// </summary>
        public enum EOrderBillState
        {
            等待付款 = 1,
            已付款 = 2,
            备货期 = 3,
            部分付款 = 5,
            /// <summary>
            /// 判断所有子单
            /// </summary>
            已发货 = 16,
            取消订单 = 20,
            /// <summary>
            /// 生成采购需求开始
            /// </summary>
            已完成订单 = 19,
            待审核付款 = 22,

        }
        /// <summary>
        /// 付款方式
        /// </summary>
        public enum EPayType
        {
            CreditCart = 1,
            Paypal = 2,
            WesternUnion = 3,
            Banktransfer = 4,
            国内银行转账 = 5,
            Webmoney = 6,
            boleto = 7,
            Moneybookers = 8,
            Ideal = 9,
            Sofort = 10,
            FastPaypal = 11,
            Banktransfer_local = 12,
            Qiwi = 15,
            Ebabx_Boleto = 16,
            Ebanx_tef = 17,
            Konbini = 18
        }

        /// <summary>
        /// 订单来源
        /// </summary>
        public enum EOrderSourceState
        {
            手机订单 = 1,
            Pc订单 = 2
        }

        public enum EnumServiceStatus
        {
            未知状态 = -1,
            /// <summary>
            /// 服务未运行
            /// </summary>
            服务未运行 = 1,
            /// <summary>
            ///服务正在启动
            /// </summary>
            服务正在启动 = 2,
            /// <summary>
            ///服务正在停止
            /// </summary>
            服务正在停止 = 3,
            /// <summary>
            ///服务正在运行 
            /// </summary>
            服务正在运行 = 4,
            /// <summary>
            ///即将继续
            /// </summary>
            服务即将继续 = 5,
            /// <summary>
            ///即将暂停
            /// </summary>
            服务即将暂停 = 6,
            /// <summary>
            ///已暂停
            /// </summary>
            服务已暂停 = 7,
        }
        

        /// <summary>
        /// 主订单的状态
        /// 为了防止与之前的订单状态冲突
        /// 添加新的订单状态
        /// 使用规则  优先调取新版订单状态
        /// 若无  调用老版的状态
        /// TB_OrderOrBillState（涉及的表）
        /// </summary>
        public enum ENewOrderState
        {
            未知 = 0,
            [Description("等待付款")]
            等待付款 = 1,
            审核付款 = 2,
            已付款 = 3,
            备货中 = 4,
            部分发货 = 5,
            已发货 = 6,
            已完成 = 7,
            订单取消 = 8
        }

        /// <summary>
        /// 并且或者枚举
        /// </summary>
        public enum EAndOr
        {
            或者 = 0,
            并且 = 1
        }
        public enum NewOrderState
        {
            未知 = 0,
            等待付款 = 1,
            审核付款 = 2,
            已付款 = 3,
            备货中 = 4,
            部分发货 = 5,
            已发货 = 6,
            已完成 = 7,
            订单取消 = 8
        }
        /// <summary>
        /// 子订单的状态
        /// 使用规则  优先调取新版订单状态
        /// 若无  调用老版的状态
        /// TB_OrderOrBillState（涉及的表）
        /// </summary>
        public enum NewBillState
        {
            未知 = 0,
            等待付款 = 1,
            审核付款 = 2,
            已付款 = 3,
            备货中 = 4,
            备货中质检 = 5,
            备货中制包 = 6,
            已发货 = 7,
            已完成 = 8,
            取消 = 9
        }
    }
}
