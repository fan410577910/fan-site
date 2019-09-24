using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Entity
{
    public partial class ActiveMenu_info
    {
        //public string Description { get; set; }

        //public int ID { get; set; }

        //public string Name { get; set; }

        //public int ParentID { get; set; }

        //public string Href { get; set; }

        #region 常量
        ///<summary>
        ///活动名称表
        ///</summary>
        public const string _ACTIVE_NAME_ = "active_name";
        ///<summary>
        ///
        ///</summary>
        public const string _ID_ = "ID";
        ///<summary>
        ///活动名称
        ///</summary>
        public const string _NAME_ = "Name";
        ///<summary>
        ///活动类型描述
        ///</summary>
        public const string _DESCRIPTION_ = "Description";
        ///<summary>
        ///状态
        ///</summary>
        public const string _STATUS_ = "Status";
        ///<summary>
        ///站点ID
        ///</summary>
        public const string _SITEID_ = "SiteID";
        ///<summary>
        ///父级ID
        ///</summary>
        public const string _PARENTID_ = "ParentID";
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
        ///活动名称
        ///</summary>
        private string _name = "";
        ///<summary>
        ///活动类型描述
        ///</summary>
        private string _description = "";
        ///<summary>
        ///状态
        ///</summary>
        private int _status;
        ///<summary>
        ///站点ID
        ///</summary>
        private int _siteID;
        ///<summary>
        ///父级ID
        ///</summary>
        private int _parentID;
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
        ///活动名称
        ///</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        ///<summary>
        ///活动类型描述
        ///</summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        ///<summary>
        ///状态
        ///</summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        ///<summary>
        ///站点ID
        ///</summary>
        public int SiteID
        {
            get { return _siteID; }
            set { _siteID = value; }
        }

        ///<summary>
        ///父级ID
        ///</summary>
        public int ParentID
        {
            get { return _parentID; }
            set { _parentID = value; }
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
    }
}
