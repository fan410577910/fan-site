using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// http://www.jeasyui.com/documentation/index.php
    /// </summary>
    public static class EasyUI
    {
        public static IHtmlString Button(string text, string id)
        {
            return Button(text, id, null, null);
        }
        public static IHtmlString Button(string text, string id, string icon)
        {
            return Button(text, id, icon, null);
        }
        public static IHtmlString Button(string text, string id, object htmlAttributes)
        {
            return Button(text, id, null, ((IDictionary<string, object>)System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static IHtmlString Button(string text, string id, string icon, object htmlAttributes)
        {
            return Button(text, id, icon, ((IDictionary<string, object>)System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static IHtmlString Button(string text, string id, string icon, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = "<a href=\"javascript:void(0)\"></a>"
            };
            tagBuilder.MergeAttributes<string, object>(htmlAttributes);
            tagBuilder.MergeAttribute("name", id, true);
            tagBuilder.GenerateId(id);
            if (string.IsNullOrEmpty(icon))
            {
                tagBuilder.MergeAttribute("data-options", "plain:false", true);
            }
            else
            {
                tagBuilder.MergeAttribute("data-options", "plain:false,iconCls:'" + icon + "'", true);
            }
            if (htmlAttributes != null)
            {
                if (htmlAttributes.ContainsKey("class"))
                {
                    tagBuilder.AddCssClass(htmlAttributes["class"].ToString());
                }
            }
            tagBuilder.AddCssClass("easyui-linkbutton");
            tagBuilder.SetInnerText(text ?? "");
            return new HtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString PlainButton(string text, string id)
        {
            return PlainButton(text, id, null, null);
        }
        public static IHtmlString PlainButton(string text, string id, string icon)
        {
            return PlainButton(text, id, icon, null);
        }
        public static IHtmlString PlainButton(string text, string id, object htmlAttributes)
        {
            return PlainButton(text, id, null, ((IDictionary<string, object>)System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static IHtmlString PlainButton(string text, string id, string icon, object htmlAttributes)
        {
            return PlainButton(text, id, icon, ((IDictionary<string, object>)System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static IHtmlString PlainButton(string text, string id, string icon, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = "<a href=\"javascript:void(0)\"></a>"
            };
            tagBuilder.MergeAttributes<string, object>(htmlAttributes);
            tagBuilder.MergeAttribute("name", id, true);
            tagBuilder.GenerateId(id);
            if (string.IsNullOrEmpty(icon))
            {
                tagBuilder.MergeAttribute("data-options", "plain:true", true);
            }
            else
            {
                tagBuilder.MergeAttribute("data-options", "plain:true,iconCls:'" + icon + "'", true);
            }
            if (htmlAttributes != null)
            {
                if (htmlAttributes.ContainsKey("class"))
                {
                    tagBuilder.AddCssClass(htmlAttributes["class"].ToString());
                }
            }
            tagBuilder.AddCssClass("easyui-linkbutton");
            tagBuilder.SetInnerText(text ?? "");
            return new HtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString EnumFormatter(Type type, Array enums, bool isConvertValue = true)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException(type.FullName + "不是枚举类型！", "type");
            }
            StringBuilder builder = new StringBuilder("");
            builder.Append("function(value,row,index){switch(value){");
            foreach (var value in enums)
            {
                builder.Append("case " + Convert.ToInt32(value) + ":return \"" + (isConvertValue ? value.ToString() : HtmlExtensions.EnumDescriptionFor(value)) + "\";");
            }
            builder.Append("}return \"\";}");//}
            return new HtmlString(builder.ToString());
        }
        public static IHtmlString EnumFormatter<TEnum>() where TEnum : struct
        {
            return EnumFormatter(typeof(TEnum), Enum.GetValues(typeof(TEnum)));
        }
        public static IHtmlString EnumFormatter(Type type,bool isConvertValue = true)
        {
            return EnumFormatter(type, Enum.GetValues(type), isConvertValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enums">要排除的枚举值数组</param>
        /// <returns></returns>
        public static IHtmlString EnumFormatter<TEnum>(params TEnum[] enums) where TEnum : struct
        {
            return EnumFormatter(false, enums);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="isInclude">是包含还是排除</param>
        /// <param name="enums">包含or排除枚举值数组</param>
        /// <returns></returns>
        public static IHtmlString EnumFormatter<TEnum>(bool isInclude, params TEnum[] enums) where TEnum : struct
        {
            if (isInclude)
            {
                return EnumFormatter(typeof(TEnum), enums);
            }
            else
            {
                return EnumFormatter(typeof(TEnum), Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Where(p => !enums.Contains(p)).ToArray());
            }
        }

        public static IHtmlString EnumFilterData(Type type, Array enums, string firstName, int firstValue, bool isConvertValue = true)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException(type.FullName + "不是枚举类型！", "type");
            }
            StringBuilder builder = new StringBuilder("");
            builder.Append("[");
            if (firstName == null)
            {
                foreach (var value in enums)
                {
                    builder.Append("{\"text\":\"" + (isConvertValue ? value.ToString() : HtmlExtensions.EnumDescriptionFor(value)) + "\",\"value\":" + Convert.ToInt32(value) + "},");
                }
                if (enums.Length > 0)
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                }
            }
            else
            {
                builder.Append("{\"text\":\"" + firstName + "\",\"value\":" + firstValue + "}");
                foreach (var value in enums)
                {
                    builder.Append(",{\"text\":\"" + (isConvertValue ? value.ToString() : HtmlExtensions.EnumDescriptionFor(value)) + "\",\"value\":" + Convert.ToInt32(value) + "}");
                }
            }
            builder.Append("]");
            return new HtmlString(builder.ToString());
        }
        public static IHtmlString EnumFilterData<TEnum>() where TEnum : struct
        {
            return EnumFilterData(typeof(TEnum), Enum.GetValues(typeof(TEnum)), "全部", -1);
        }
        public static IHtmlString EnumFilterData<TEnum>(string firstName, int firstValue) where TEnum : struct
        {
            return EnumFilterData(typeof(TEnum), Enum.GetValues(typeof(TEnum)), "全部", -1);
        }
        public static IHtmlString EnumFilterData(Type type, bool isConvertValue = true)
        {
            return EnumFilterData(type, Enum.GetValues(type), "全部", -1, isConvertValue);
        }
        public static IHtmlString EnumFilterData(Type type, string firstName, int firstValue)
        {
            return EnumFilterData(type, Enum.GetValues(type), firstName, firstValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enums">要排除的枚举值数组</param>
        /// <returns></returns>
        public static IHtmlString EnumFilterData<TEnum>(params TEnum[] enums) where TEnum : struct
        {
            return EnumFilterData(false, enums);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="isInclude">是包含还是排除</param>
        /// <param name="enums">包含or排除枚举值数组</param>
        /// <returns></returns>
        public static IHtmlString EnumFilterData<TEnum>(bool isInclude, params TEnum[] enums) where TEnum : struct
        {
            if (isInclude)
            {
                return EnumFilterData(typeof(TEnum), enums, "全部", -1);
            }
            else
            {
                return EnumFilterData(typeof(TEnum), Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Where(p => !enums.Contains(p)).ToArray(), "全部", -1);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="isInclude">是包含还是排除</param>
        /// <param name="firstName"></param>
        /// <param name="firstValue"></param>
        /// <param name="enums">包含or排除枚举值数组</param>
        /// <returns></returns>
        public static IHtmlString EnumFilterData<TEnum>(bool isInclude, string firstName, int firstValue, params TEnum[] enums) where TEnum : struct
        {
            if (isInclude)
            {
                return EnumFilterData(typeof(TEnum), enums, "全部", -1);
            }
            else
            {
                return EnumFilterData(typeof(TEnum), Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Where(p => !enums.Contains(p)).ToArray(), firstName, firstValue);
            }
        }

        public static IHtmlString BoolFilterData(string trueString, string falseString, bool hasFirst)
        {
            StringBuilder builder = new StringBuilder("");
            builder.Append("[");
            if (hasFirst)
            {
                builder.Append("{\"text\":\"全部\",\"value\":-1},");
            }
            builder.Append("{\"text\":\"" +( trueString ?? "是") + "\",\"value\":true}");
            builder.Append(",{\"text\":\"" + (falseString ?? "否") + "\",\"value\":false}");
            builder.Append("]");
            return new HtmlString(builder.ToString());
        }
        public static IHtmlString BoolFilterData(string trueString, string falseString)
        {
            return BoolFilterData(trueString, falseString, true);
        }
        public static IHtmlString BoolFilterData()
        {
            return BoolFilterData("是", "否", true);
        }
    }
}
