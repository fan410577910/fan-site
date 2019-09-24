#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  UrlRoutingSettingConfigElementCollection 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan 
     * 创建时间：2014/11/5 11:23:46 
     * 描述    : Url路由配置元素集合
     * =====================================================================
     * 修改时间：2014/11/5 11:23:46 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.Configuration;

namespace FAN.UrlRouting.Config
{
    /// <summary>
    /// Url路由配置元素集合
    /// </summary>
    public class UrlRoutingSettingConfigElementCollection : ConfigurationElementCollection
    {
        public UrlRoutingSettingConfigElementCollection()
        {
            UrlRoutingSettingConfigElement element = this.CreateNewElement() as UrlRoutingSettingConfigElement;
            this.Add(element);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlRoutingSettingConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as UrlRoutingSettingConfigElement).RouteName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public new string AddElementName
        {
            get { return base.AddElementName; }
            set { base.AddElementName = value; }
        }

        public new string ClearElementName
        {
            get { return base.ClearElementName; }
            set { base.AddElementName = value; }
        }

        public new string RemoveElementName
        {
            get { return base.RemoveElementName; }
        }

        public new int Count
        {
            get { return base.Count; }
        }

        public UrlRoutingSettingConfigElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as UrlRoutingSettingConfigElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                    base.BaseRemoveAt(index);
                this.BaseAdd(index, value);
            }
        }

        public new UrlRoutingSettingConfigElement this[string name]
        {
            get
            {
                return base.BaseGet(name) as UrlRoutingSettingConfigElement;
            }
        }

        public int IndexOf(UrlRoutingSettingConfigElement element)
        {
            return base.BaseIndexOf(element);
        }

        public void Add(UrlRoutingSettingConfigElement element)
        {
            this.BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            base.BaseAdd(element, false);
        }

        public void Remove(UrlRoutingSettingConfigElement element)
        {
            if (base.BaseIndexOf(element) >= 0)
                base.BaseRemove(element.RouteName);
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public void Clear()
        {
            base.BaseClear();
        }
    }
}
