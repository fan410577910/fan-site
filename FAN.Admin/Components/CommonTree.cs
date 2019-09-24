#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2018 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-TI50KE6KO4 
     * 文件名：  CommonTree 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间： 2018/7/18 14:17:41 
     * 描述    :
     * =====================================================================
     * 修改时间：2018/7/18 14:17:41 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using FAN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace FAN.Admin.Components
{
    public class CommonTree
    {
        /// <summary>
        /// 获取树形菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="treeData">返回类型</param>
        /// <param name="text">默认text</param>
        /// <param name="iconCls"></param>
        /// <returns></returns>
        public static List<TreeData> GetTreeData<T>(List<T> treeData, string text, string iconCls = "icon-category", Expression<Func<T, int>> orderLambda = null, bool isAsc = false) where T : ITreeData
        {
            List<TreeData> tree = new List<TreeData>
            {
                new TreeData
                {
                    children = BuildTree(treeData, 0,orderLambda,isAsc),
                    iconCls = iconCls,
                    id = 0,
                    state = "open",
                    text = text,
                    attributes = new { hasChildren = true }
                }
            };
            return tree;
        }


        /// <summary>
        /// 获取树形菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="treeData"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static List<TreeData> BuildTree<T>(List<T> treeData, int parentId, Expression<Func<T, int>> orderLambda, bool isAsc) where T : ITreeData
        {
            List<TreeData> treeList = new List<TreeData>();
            IQueryable<T> treeItems = treeData.Where(p => p.ParentID == parentId).AsQueryable();
            if (orderLambda != null)
            {
                if (isAsc)
                {
                    treeItems = treeItems.OrderBy(orderLambda);
                }
                else
                {
                    treeItems = treeItems.OrderByDescending(orderLambda);
                }
            }
            if (!treeItems.Any()) return treeList;
            TreeData tree = null;
            foreach (T item in treeItems)
            {
                tree = new TreeData
                {
                    id = item.ID,
                    text = item.Name,
                    children = BuildTree(treeData, item.ID, orderLambda, isAsc),
                    description = item.Description
                };
                tree.state = tree.children.Count > 0 ? "closed" : "open";
                tree.attributes = new { hasChildren = tree.children.Count > 0,description= item.Description };
                treeList.Add(tree);
            }
            return treeList;
        }
        
    }
}