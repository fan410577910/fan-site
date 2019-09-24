using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Console
{
    public interface ITreeData
    {
        int ParentID { get; set; }
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
    /// <summary>
    /// JQuery EasyUI里面的Tree控件数据类型
    /// </summary>
    public class TreeData
    {
        public object id { get; set; }
        public string text { get; set; }
        //状态：open 或者 closed
        public string state { get; set; }
        public bool @checked { get; set; }
        public string iconCls { get; set; }
        public object attributes { get; set; }
        public string description { get; set; }
        public List<TreeData> children { get; set; }
    }
    public class MenuInfo : ITreeData
    {
        public int ParentID { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
    public class 泛型Demo
    {
        /// <summary>
        /// 获取EasyUI Tree控件数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="treeData"></param>
        /// <param name="parentID"></param>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<TreeData> GetTreeData<T>(List<T> treeData, int parentID, int type, Func<T, int> orderFunc) where T : class,ITreeData
        {
            List<TreeData> treeList = new List<TreeData>();
            var treeItems = treeData.FindAll(t => t.ParentID == parentID).OrderBy(orderFunc).ToList();
            if (treeItems.Count == 0) return treeList;

            foreach (var treeItem in treeItems)
            {
                TreeData td = new TreeData
                {
                    id = treeItem.ID,
                    text = treeItem.Name,
                    description = treeItem.Description,
                    children = GetTreeData(treeData, treeItem.ID, type, orderFunc)
                };
                td.state = td.children.Count > 0 ? "closed" : "open";
                td.attributes = new { hasChildren = td.children.Count > 0, type = type };
                treeList.Add(td);
            }
            return treeList;

        }
        /// <summary>
        /// 字符串转换为指定类型的List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<T> SplitToList<T>(string str) where T : IConvertible
        {
            List<T> resultList = new List<T>();
            string[] arrs = str.Split(',');
            T t = default(T);

            for (int i = 0; i < arrs.Length; i++)
            {
                try
                {
                    t = (T)Convert.ChangeType(arrs[i], typeof(T));
                    resultList.Add(t);
                }
                catch (Exception ex)
                {

                }
            }


            return resultList;
        }
    }
}
