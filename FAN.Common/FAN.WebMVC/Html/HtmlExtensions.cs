using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Web.Mvc.Html
{
    public static class HtmlExtensions
    {
        //
        public static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;
            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null) { realModelType = underlyingType; }
            return realModelType;
        }
        public static string EnumDescriptionFor<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public static IHtmlString EnumDescriptionFor<TEnum>(this HtmlHelper helper, TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((attributes != null) && (attributes.Length > 0))
                return new HtmlString(attributes[0].Description);
            else
                return new HtmlString("");
        }
        public static IHtmlString EnumEachFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Action<TEnum, string, StringBuilder> each)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            StringBuilder sb = new StringBuilder("");
            foreach (var item in values)
            {
                each(item, EnumDescriptionFor(item), sb);
            }
            return new HtmlString(sb.ToString());
        }
        public static IHtmlString EnumEachFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Action<int, TEnum, string, StringBuilder> each)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            StringBuilder sb = new StringBuilder("");
            int i = 0;
            foreach (var item in values)
            {
                each(i, item, EnumDescriptionFor(item), sb);
                i++;
            }
            return new HtmlString(sb.ToString());
        }
        public static IHtmlString EnumEachFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Func<TEnum, string, StringBuilder, bool> each)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            StringBuilder sb = new StringBuilder("");
            foreach (var item in values)
            {
                bool r = each(item, EnumDescriptionFor(item), sb);
                if (!r) break;
            }
            return new HtmlString(sb.ToString());
        }
        public static IHtmlString EnumEachFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Func<int, TEnum, string, StringBuilder, bool> each)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            StringBuilder sb = new StringBuilder("");
            int i = 0;
            foreach (var item in values)
            {
                bool r = each(i, item, EnumDescriptionFor(item), sb);
                if (!r) break;
                i++;
            }
            return new HtmlString(sb.ToString());
        }
        public static IHtmlString EnumEachFor<TEnum>(this HtmlHelper htmlHelper, Action<TEnum, string, StringBuilder> each)
        {
            Type enumType = typeof(TEnum);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            StringBuilder sb = new StringBuilder("");
            foreach (var item in values)
            {
                each(item, EnumDescriptionFor(item), sb);
            }
            return new HtmlString(sb.ToString());
        }
        public static IHtmlString EnumEachFor<TEnum>(this HtmlHelper htmlHelper, Action<int, TEnum, string, StringBuilder> each)
        {
            Type enumType = typeof(TEnum);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            StringBuilder sb = new StringBuilder("");
            int i = 0;
            foreach (var item in values)
            {
                each(i, item, EnumDescriptionFor(item), sb);
                i++;
            }
            return new HtmlString(sb.ToString());
        }
        public static IHtmlString EnumEachFor<TEnum>(this HtmlHelper htmlHelper, Func<TEnum, string, StringBuilder, bool> each)
        {
            Type enumType = typeof(TEnum);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            StringBuilder sb = new StringBuilder("");
            foreach (var item in values)
            {
                bool r = each(item, EnumDescriptionFor(item), sb);
                if (!r) break;
            }
            return new HtmlString(sb.ToString());
        }
        public static IHtmlString EnumEachFor<TEnum>(this HtmlHelper htmlHelper, Func<int, TEnum, string, StringBuilder, bool> each)
        {
            Type enumType = typeof(TEnum);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();
            StringBuilder sb = new StringBuilder("");
            int i = 0;
            foreach (var item in values)
            {
                bool r = each(i, item, EnumDescriptionFor(item), sb);
                if (!r) break;
                i++;
            }
            return new HtmlString(sb.ToString());
        }
        
        //使用这个方法会把日期类型的属性值变成Date(\\d)的格式。
        //public static IHtmlString Json(this HtmlHelper htmlHelper, object input)
        //{
        //    if (input != null)
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        serializer.MaxJsonLength = Int32.MaxValue;
        //        return htmlHelper.Raw(serializer.Serialize(input));
        //    }
        //    return htmlHelper.Raw("{}");
        //}

        public static IHtmlString Json(this HtmlHelper htmlHelper, object input)
        {
            if (input != null)
            {
                return htmlHelper.Raw(JsonConvert.SerializeObject(input));
            }
            return htmlHelper.Raw("{}");
        }

        public static string SubString(this HtmlHelper helper, string input, int len)
        {
            if (input == null)
                return string.Empty;
            if (input.Length < len)
                return input;
            else
                return input.Substring(0, len) + "...";
        }
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            return id.Replace('[', '_').Replace(']', '_');
        }
        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static IHtmlString Help(this HtmlHelper helper, WebViewPage viewPage, bool debug = false)
        {
            string url = viewPage.Context.Request.Url.LocalPath;

            return helper.Raw("<a title='本页帮助' href='javascript:;' style=\"position:fixed;top:1px;right:0px; z-index:3;display:block; background-color:rgba(0,0,0,0.5);color:#fff; border:solid 1px #666;padding:2px;\" target='_blank' onclick=\"erp.tabShow('帮助中心','/Help/Help/Go?url="
                + url
                + "')\">本页帮助</a>" + (debug ? "<a href='javascript:;' style=\"position:fixed;top:1px;right:55px; z-index:3;display:block; background-color:rgba(0,0,0,0.5);color:#fff;border:solid 1px #666;padding:2px;\" onclick=\"erp.alert('" + url + "')\">获取URL</a>" : ""));
        }
    }
}
