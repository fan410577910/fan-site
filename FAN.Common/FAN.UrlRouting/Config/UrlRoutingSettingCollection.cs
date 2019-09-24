#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-429236GLDJ 
     * 文件名：  UrlRoutingSettingCollection 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan 
     * 创建时间：2014/11/5 11:31:47 
     * 描述    : Url路由信息集合
     * =====================================================================
     * 修改时间：2014/11/5 11:31:47 
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAN.UrlRouting.Config
{
    /// <summary>
    /// Url路由信息集合
    /// </summary>
    public sealed class UrlRoutingSettingCollection : List<UrlRoutingSetting>
    {
        new public void Add(UrlRoutingSetting item)
        {
            if (!this.Contains(item))
                base.Add(item);
        }

        new public bool Contains(UrlRoutingSetting item)
        {
            return base.Contains(item);
        }

        new public void Remove(UrlRoutingSetting item)
        {
            if (this.Contains(item))
                base.Remove(item);
        }

        new public void Clear()
        {
            base.Clear();
        }

        new public int Count
        {
            get
            {
                return base.Count;
            }
        }

        new public UrlRoutingSetting this[int index]
        {
            get
            {
                if (index < 0 || index > this.Count)
                    throw new IndexOutOfRangeException("下标越界");
                return base[index];
            }
        }

        public UrlRoutingSetting this[string routeName]
        {
            get
            {
                UrlRoutingSetting setting = null;
                foreach (UrlRoutingSetting item in this)
                {
                    if (item.RouteName == routeName)
                    {
                        setting = item;
                        break;
                    }
                }
                return setting;
            }
        }
    }
}
