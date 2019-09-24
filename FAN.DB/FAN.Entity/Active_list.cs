using System;

namespace FAN.Entity
{
    /// <summary>
    ///活动列表
    /// </summary>
    [Serializable]
    public partial class Active_info
    {
        #region 常量
        ///<summary>
		///活动列表
		///</summary>
        public const string _ACTIVE_LIST_ = "active_list";
        ///<summary>
        ///
        ///</summary>
        public const string _ID_ = "ID";
        ///<summary>
        ///外键
        ///</summary>
        public const string _ACTIVENAMEID_ = "ActiveNameID";
        ///<summary>
        ///细则名称
        ///</summary>
        public const string _NAME_ = "Name";
        ///<summary>
        ///前台提示
        ///</summary>
        public const string _TIPS_ = "Tips";
        ///<summary>
        ///活动开始时间
        ///</summary>
        public const string _STARTTIME_ = "StartTime";
        ///<summary>
        ///活动结束时间
        ///</summary>
        public const string _ENDTIME_ = "EndTime";
        ///<summary>
        ///包含分类（品类或类目）
        ///</summary>
        public const string _SCOPEFENLEIINCLUDEJSON_ = "ScopeFenLeiIncludeJSON";
        ///<summary>
        ///排除分类（品类或类目），没有值就是没有排除的品类或类目
        ///</summary>
        public const string _SCOPEFENLEIEXCLUDEJSON_ = "ScopeFenLeiExcludeJSON";
        ///<summary>
        ///分类（类目或品类）
        ///</summary>
        public const string _SCOPELEIMUORPINLEI_ = "ScopeLeiMuOrPinLei";
        ///<summary>
        ///包含产品
        ///</summary>
        public const string _SCOPEPRODUCTINCLUDEJSON_ = "ScopeProductIncludeJSON";
        ///<summary>
        ///排除产品
        ///</summary>
        public const string _SCOPEPRODUCTEXCLUDEJSON_ = "ScopeProductExcludeJSON";
        ///<summary>
        ///产品（SPUID或SKUID）
        ///</summary>
        public const string _SCOPEPRODUCTSKUORSPU_ = "ScopeProductSKUorSPU";
        ///<summary>
        ///当前使用的优惠方式，枚举（按金额包邮，按折扣包邮；按购物车中满足条件的产品打折，按购物车中满足条件的产品减钱，按产品总价定额定价；按SKU赠送产品；满M件送N件）
        ///</summary>
        public const string _DISCOUNTTYPEUSE_ = "DiscountTypeUse";
        ///<summary>
        ///使用当前的优惠方式，要排除其他的优惠方式，如果没有值表示没有排除的优惠方式。
        ///</summary>
        public const string _DISCOUNTTYPEEXCLUDEJSON_ = "DiscountTypeExcludeJSON";
        ///<summary>
        ///活动使用平台枚举：PC，M，APP
        ///</summary>
        public const string _PLATFORMJSON_ = "PlatformJSON";
        ///<summary>
        ///活动优先级别（排序规则：值越大级别越高）
        ///</summary>
        public const string _PRIORITY_ = "Priority";
        ///<summary>
        ///规则状态枚举（正常或暂停）
        ///</summary>
        public const string _STATUS_ = "Status";
        ///<summary>
        ///创建时间
        ///</summary>
        public const string _CREATETIME_ = "CreateTime";
        ///<summary>
        ///创建用户名
        ///</summary>
        public const string _CREATEUSERNAME_ = "CreateUserName";
        ///<summary>
        ///修改时间
        ///</summary>
        public const string _UPDATETIME_ = "UpdateTime";
        ///<summary>
        ///修改用户名
        ///</summary>
        public const string _UPDATEUSERNAME_ = "UpdateUserName";
        #endregion

        #region 变量定义
        ///<summary>
        ///
        ///</summary>
        private int _iD;
        ///<summary>
        ///外键
        ///</summary>
        private int _activeNameID;
        ///<summary>
        ///细则名称
        ///</summary>
        private string _name = "";
        ///<summary>
        ///前台提示
        ///</summary>
        private string _tips = "";
        ///<summary>
        ///活动开始时间
        ///</summary>
        private DateTime _startTime = new DateTime(1900, 1, 1);
        ///<summary>
        ///活动结束时间
        ///</summary>
        private DateTime _endTime = new DateTime(1900, 1, 1);
        ///<summary>
        ///包含分类（品类或类目）
        ///</summary>
        private string _scopeFenLeiIncludeJSON = "";
        ///<summary>
        ///排除分类（品类或类目），没有值就是没有排除的品类或类目
        ///</summary>
        private string _scopeFenLeiExcludeJSON = "";
        ///<summary>
        ///分类（类目或品类）
        ///</summary>
        private int _scopeLeiMuOrPinLei;
        ///<summary>
        ///包含产品
        ///</summary>
        private string _scopeProductIncludeJSON = "";
        ///<summary>
        ///排除产品
        ///</summary>
        private string _scopeProductExcludeJSON = "";
        ///<summary>
        ///产品（SPUID或SKUID）
        ///</summary>
        private int _scopeProductSKUorSPU;
        ///<summary>
        ///当前使用的优惠方式，枚举（按金额包邮，按折扣包邮；按购物车中满足条件的产品打折，按购物车中满足条件的产品减钱，按产品总价定额定价；按SKU赠送产品；满M件送N件）
        ///</summary>
        private int _discountTypeUse;
        ///<summary>
        ///使用当前的优惠方式，要排除其他的优惠方式，如果没有值表示没有排除的优惠方式。
        ///</summary>
        private string _discountTypeExcludeJSON = "";
        ///<summary>
        ///活动使用平台枚举：PC，M，APP
        ///</summary>
        private string _platformJSON = "";
        ///<summary>
        ///活动优先级别（排序规则：值越大级别越高）
        ///</summary>
        private int _priority;
        ///<summary>
        ///规则状态枚举（正常或暂停）
        ///</summary>
        private int _status;
        ///<summary>
        ///创建时间
        ///</summary>
        private DateTime _createTime = new DateTime(1900, 1, 1);
        ///<summary>
        ///创建用户名
        ///</summary>
        private string _createUserName = "";
        ///<summary>
        ///修改时间
        ///</summary>
        private DateTime _updateTime = new DateTime(1900, 1, 1);
        ///<summary>
        ///修改用户名
        ///</summary>
        private string _updateUserName = "";
        #endregion

        #region 构造函数
        ///<summary>
        ///活动列表
        ///</summary>
        public Active_info()
        {
        }
        ///<summary>
        ///活动列表
        ///</summary>
        public Active_info
        (
            int activeNameID,
            string name,
            string tips,
            DateTime startTime,
            DateTime endTime,
            string scopeFenLeiIncludeJSON,
            string scopeFenLeiExcludeJSON,
            int scopeLeiMuOrPinLei,
            string scopeProductIncludeJSON,
            string scopeProductExcludeJSON,
            int scopeProductSKUorSPU,
            int discountTypeUse,
            string discountTypeExcludeJSON,
            string platformJSON,
            int priority,
            int status,
            DateTime createTime,
            string createUserName,
            DateTime updateTime,
            string updateUserName
        )
        {
            _activeNameID = activeNameID;
            _name = name;
            _tips = tips;
            _startTime = startTime;
            _endTime = endTime;
            _scopeFenLeiIncludeJSON = scopeFenLeiIncludeJSON;
            _scopeFenLeiExcludeJSON = scopeFenLeiExcludeJSON;
            _scopeLeiMuOrPinLei = scopeLeiMuOrPinLei;
            _scopeProductIncludeJSON = scopeProductIncludeJSON;
            _scopeProductExcludeJSON = scopeProductExcludeJSON;
            _scopeProductSKUorSPU = scopeProductSKUorSPU;
            _discountTypeUse = discountTypeUse;
            _discountTypeExcludeJSON = discountTypeExcludeJSON;
            _platformJSON = platformJSON;
            _priority = priority;
            _status = status;
            _createTime = createTime;
            _createUserName = createUserName;
            _updateTime = updateTime;
            _updateUserName = updateUserName;

        }
        #endregion

        #region 公共属性

        ///<summary>
        ///
        ///</summary>
        public int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        ///<summary>
        ///外键
        ///</summary>
        public int ActiveNameID
        {
            get { return _activeNameID; }
            set { _activeNameID = value; }
        }

        ///<summary>
        ///细则名称
        ///</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        ///<summary>
        ///前台提示
        ///</summary>
        public string Tips
        {
            get { return _tips; }
            set { _tips = value; }
        }

        ///<summary>
        ///活动开始时间
        ///</summary>
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        ///<summary>
        ///活动结束时间
        ///</summary>
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        ///<summary>
        ///包含分类（品类或类目）
        ///</summary>
        public string ScopeFenLeiIncludeJSON
        {
            get { return _scopeFenLeiIncludeJSON; }
            set { _scopeFenLeiIncludeJSON = value; }
        }

        ///<summary>
        ///排除分类（品类或类目），没有值就是没有排除的品类或类目
        ///</summary>
        public string ScopeFenLeiExcludeJSON
        {
            get { return _scopeFenLeiExcludeJSON; }
            set { _scopeFenLeiExcludeJSON = value; }
        }

        ///<summary>
        ///分类（类目或品类）
        ///</summary>
        public int ScopeLeiMuOrPinLei
        {
            get { return _scopeLeiMuOrPinLei; }
            set { _scopeLeiMuOrPinLei = value; }
        }

        ///<summary>
        ///包含产品
        ///</summary>
        public string ScopeProductIncludeJSON
        {
            get { return _scopeProductIncludeJSON; }
            set { _scopeProductIncludeJSON = value; }
        }

        ///<summary>
        ///排除产品
        ///</summary>
        public string ScopeProductExcludeJSON
        {
            get { return _scopeProductExcludeJSON; }
            set { _scopeProductExcludeJSON = value; }
        }

        ///<summary>
        ///产品（SPUID或SKUID）
        ///</summary>
        public int ScopeProductSKUorSPU
        {
            get { return _scopeProductSKUorSPU; }
            set { _scopeProductSKUorSPU = value; }
        }

        ///<summary>
        ///当前使用的优惠方式，枚举（按金额包邮，按折扣包邮；按购物车中满足条件的产品打折，按购物车中满足条件的产品减钱，按产品总价定额定价；按SKU赠送产品；满M件送N件）
        ///</summary>
        public int DiscountTypeUse
        {
            get { return _discountTypeUse; }
            set { _discountTypeUse = value; }
        }

        ///<summary>
        ///使用当前的优惠方式，要排除其他的优惠方式，如果没有值表示没有排除的优惠方式。
        ///</summary>
        public string DiscountTypeExcludeJSON
        {
            get { return _discountTypeExcludeJSON; }
            set { _discountTypeExcludeJSON = value; }
        }

        ///<summary>
        ///活动使用平台枚举：PC，M，APP
        ///</summary>
        public string PlatformJSON
        {
            get { return _platformJSON; }
            set { _platformJSON = value; }
        }

        ///<summary>
        ///活动优先级别（排序规则：值越大级别越高）
        ///</summary>
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        ///<summary>
        ///规则状态枚举（正常或暂停）
        ///</summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        ///<summary>
        ///创建时间
        ///</summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        ///<summary>
        ///创建用户名
        ///</summary>
        public string CreateUserName
        {
            get { return _createUserName; }
            set { _createUserName = value; }
        }

        ///<summary>
        ///修改时间
        ///</summary>
        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        ///<summary>
        ///修改用户名
        ///</summary>
        public string UpdateUserName
        {
            get { return _updateUserName; }
            set { _updateUserName = value; }
        }

        #endregion

        #region 重写的方法
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Active_info)
            {
                result = (obj as Active_info).ID == this.ID;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
