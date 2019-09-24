using FAN.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 本类通过枚举类型的变量值，完整select option和input type="radio"的html创建
    /// </summary>
    public static class SelectExtensions
    {
        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "请选择...", Value = "" } };
        /// <summary>
        /// 使用强类型方式创建input type="checkbox"标签
        /// @Html.EnumCheckBoxListFor(model => model.PermissionType)
        /// wangyp
        /// 2014-12-16
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumCheckBoxListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression) where TEnum : class
        {
            return EnumCheckBoxListFor(htmlHelper, expression, null);
        }
        /// <summary>
        /// 使用强类型方式创建input type="checkbox"标签
        /// wangyp
        /// 2014-12-16
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumCheckBoxListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes) where TEnum : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = HtmlExtensions.GetNonNullableModelType(metadata);
            if (!enumType.IsArray || !metadata.ModelType.IsArray)
            {
                throw new Exception("Model的属性必须为数组类型");
            }
            enumType = enumType.GetElementType();
            if (!enumType.IsEnum)
            {
                throw new Exception("Model的属性必须为枚举类型的数组类型");
            }
            int[] models = (int[])metadata.Model;
            StringBuilder buffter = new StringBuilder();
            Array enums = Enum.GetValues(enumType);
            foreach (Enum p in enums)
            {
                TagBuilder tagBuilder = new TagBuilder("input");
                tagBuilder.GenerateId(metadata.PropertyName + p.GetValue().ToString());
                tagBuilder.MergeAttributes<string, object>(htmlAttributes);
                tagBuilder.MergeAttribute("name", metadata.PropertyName, true);
                tagBuilder.MergeAttribute("type", "checkbox");
                tagBuilder.MergeAttribute("value", p.GetValue().ToString());
                if (models.Contains(p.GetValue()))
                {
                    tagBuilder.MergeAttribute("checked", "checked");
                }
                buffter.AppendFormat("<label>" + tagBuilder.ToString() + p.GetName() + "</label>");
            }
            Array.Clear(models, 0, models.Length);
            models = null;
            return new MvcHtmlString(buffter.ToString());
        }
        /// <summary>
        /// 使用弱类型方式创建input type="checkbox"标签
        /// @this.Html.EnumCheckBoxListFor(Model.Platforms);
        /// wangyp
        /// 2014-12-16
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static IHtmlString EnumCheckBoxListFor<TEnum>(this HtmlHelper htmlHelper, TEnum[] enums) where TEnum : struct
        {
            return EnumCheckBoxListFor<TEnum>(htmlHelper, enums, null);
        }
        /// <summary>
        /// 使用弱类型方式创建input type="checkbox"标签
        /// wangyp
        /// 2014-12-16
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="enums"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString EnumCheckBoxListFor<TEnum>(this HtmlHelper htmlHelper, TEnum[] enums, IDictionary<string, object> htmlAttributes) where TEnum : struct
        {
            return htmlHelper.EnumEachFor(new Action<TEnum, string, System.Text.StringBuilder>((platform, text, sb) =>
            {
                Type enumType = typeof(TEnum);
                TagBuilder tagBuilder = new TagBuilder("input");
                tagBuilder.GenerateId(enumType.Name + Convert.ToInt32(platform).ToString());
                tagBuilder.MergeAttributes<string, object>(htmlAttributes);
                tagBuilder.MergeAttribute("name", enumType.Name, true);
                tagBuilder.MergeAttribute("type", "checkbox");
                tagBuilder.MergeAttribute("value", Convert.ToInt32(platform).ToString());
                if (enums.Contains(platform))
                {
                    tagBuilder.MergeAttribute("checked", "checked");
                }
                sb.AppendFormat("<label>" + tagBuilder.ToString() + platform + "</label>");
            }));
        }
        /// <summary>
        /// 使用强类型方式创建input type="radio"标签
        /// @this.Html.EnumRadioButtonListFor(p => p.Render)
        /// wangyp
        /// 2014-12-16
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumRadioButtonListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression) where TEnum : struct
        {
            return EnumRadioButtonListFor(htmlHelper, expression, null);
        }
        /// <summary>
        /// 使用强类型方式创建input type="radio"标签
        /// @this.Html.EnumRadioButtonListFor(p => p.Render)
        /// wangyp
        /// 2014-12-16
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumRadioButtonListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes) where TEnum : struct
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = HtmlExtensions.GetNonNullableModelType(metadata);

            StringBuilder buffter = new StringBuilder();
            Array enums = Enum.GetValues(enumType);
            foreach (Enum p in enums)
            {
                TagBuilder tagBuilder = new TagBuilder("input");
                tagBuilder.GenerateId(metadata.PropertyName + p.GetValue().ToString());
                tagBuilder.MergeAttributes<string, object>(htmlAttributes);
                tagBuilder.MergeAttribute("name", metadata.PropertyName, true);
                tagBuilder.MergeAttribute("type", "radio");
                tagBuilder.MergeAttribute("value", p.GetValue().ToString());
                if (p.Equals(metadata.Model))
                {
                    tagBuilder.MergeAttribute("checked", "checked");
                }
                buffter.AppendFormat("<label>" + tagBuilder.ToString() + p.GetName() + "</label>");
            }
            return new MvcHtmlString(buffter.ToString());
        }

        /// <summary>
        /// @Html.EnumDropDownListFor(model => model.PermissionType)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression) where TEnum : struct
        {
            return EnumDropDownListFor(htmlHelper, expression, null);
        }
        /// <summary>
        /// @Html.EnumDropDownListFor(model => model.PermissionType)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes) where TEnum : struct
        {
            return EnumDropDownListFor(htmlHelper, expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes) where TEnum : struct
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = HtmlExtensions.GetNonNullableModelType(metadata);

            StringBuilder buffter = new StringBuilder("");
            if (metadata.IsNullableValueType)
                buffter.AppendFormat("\n<option value=\"\" selected=\"selected\">请选择...</option>");
            Enum.GetValues(enumType).Cast<TEnum>().ToList().ForEach(p =>
                buffter.AppendFormat("\n<option value=\"{0}\"{2}>{1}</option>",
                    Convert.ToInt32(p).ToString(),
                    HtmlExtensions.EnumDescriptionFor(p),
                    p.Equals(metadata.Model) ? " selected=\"selected\"" : ""
                )
             );


            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = buffter.ToString()
            };
            tagBuilder.MergeAttributes<string, object>(htmlAttributes);
            tagBuilder.GenerateId(metadata.PropertyName);
            tagBuilder.MergeAttribute("name", metadata.PropertyName, true);
            return new MvcHtmlString(tagBuilder.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enums"></param>
        /// <param name="isInclude">true表示仅包含enums的值，false表示不包含enums的值</param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, TEnum[] enums, bool isInclude, bool isConvertValue = true) where TEnum : struct
        {
            return EnumDropDownListFor(htmlHelper, expression, enums, isInclude, null, isConvertValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enums"></param>
        /// <param name="isInclude">true表示仅包含enums的值，false表示不包含enums的值</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, TEnum[] enums, bool isInclude, object htmlAttributes) where TEnum : struct
        {
            return EnumDropDownListFor(htmlHelper, expression, enums, isInclude, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enums"></param>
        /// <param name="isInclude">true表示仅包含enums的值，false表示不包含enums的值</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, TEnum[] enums, bool isInclude, IDictionary<string, object> htmlAttributes, bool isConvertValue = true) where TEnum : struct
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = HtmlExtensions.GetNonNullableModelType(metadata);

            StringBuilder buffter = new StringBuilder("");
            if (metadata.IsNullableValueType)
                buffter.AppendFormat("\n<option value=\"\" selected=\"selected\">请选择...</option>");
            if (isInclude)
            {
                Array.ForEach(enums, p =>
                      buffter.AppendFormat("\n<option value=\"{0}\"{2}>{1}</option>",
                          Convert.ToInt32(p).ToString(),
                          isConvertValue ? p.ToString() : HtmlExtensions.EnumDescriptionFor(p),
                          p.Equals(metadata.Model) ? " selected=\"selected\"" : ""
                      )
                   );
            }
            else
            {
                Array.ForEach(Enum.GetValues(enumType).Cast<TEnum>().Where(p => !enums.Contains(p)).ToArray(), p =>
                    buffter.AppendFormat("\n<option value=\"{0}\"{2}>{1}</option>",
                        Convert.ToInt32(p).ToString(),
                        isConvertValue ? p.ToString() : HtmlExtensions.EnumDescriptionFor(p),
                        p.Equals(metadata.Model) ? " selected=\"selected\"" : ""
                    )
                 );
            }

            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = buffter.ToString()
            };
            tagBuilder.MergeAttributes<string, object>(htmlAttributes);
            tagBuilder.GenerateId(metadata.PropertyName);
            tagBuilder.MergeAttribute("name", metadata.PropertyName, true);
            return new MvcHtmlString(tagBuilder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="tEnum"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="tEnums"></param>
        /// <param name="isInclude">true表示仅包含enums的值，false表示不包含enums的值</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, TEnum tEnum, string id, string name, TEnum[] tEnums, bool isInclude, IDictionary<string, object> htmlAttributes) where TEnum : struct
        {
            var enums = Enum.GetValues(tEnum.GetType()).Cast<TEnum>().ToArray();

            StringBuilder buffter = new StringBuilder("");
            if (isInclude)
            {
                Array.ForEach(tEnums, p =>
                      buffter.AppendFormat("\n<option value=\"{0}\"{2}>{1}</option>",
                          Convert.ToInt32(p).ToString(),
                          HtmlExtensions.EnumDescriptionFor(p),
                          p.Equals(tEnum) ? " selected=\"selected\"" : ""
                      )
                   );
            }
            else
            {
                Array.ForEach(enums.Where(p => !tEnums.Contains(p)).ToArray(), p =>
                    buffter.AppendFormat("\n<option value=\"{0}\"{2}>{1}</option>",
                        Convert.ToInt32(p).ToString(),
                        HtmlExtensions.EnumDescriptionFor(p),
                        p.Equals(tEnum) ? " selected=\"selected\"" : ""
                    )
                 );
            }
            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = buffter.ToString()
            };
            tagBuilder.MergeAttributes<string, object>(htmlAttributes);
            tagBuilder.GenerateId(id);
            tagBuilder.MergeAttribute("name", name, true);
            return new MvcHtmlString(tagBuilder.ToString());
        }
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, TEnum tEnum, string id, string name, TEnum[] tEnums, bool isInclude, object htmlAttributes) where TEnum : struct
        {
            return EnumDropDownList(htmlHelper, tEnum, id, name, tEnums, isInclude, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, TEnum tEnum, string id, string name, TEnum[] tEnums, object htmlAttributes) where TEnum : struct
        {
            return EnumDropDownList(htmlHelper, tEnum, id, name, tEnums, true, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }


        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, TEnum tEnum, string id, string name) where TEnum : struct
        {
            return EnumDropDownList(htmlHelper, tEnum, id, name, null);
        }
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, TEnum tEnum, string id, string name, object htmlAttributes) where TEnum : struct
        {
            return EnumDropDownList(htmlHelper, tEnum, id, name, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, TEnum tEnum, string id, string name, IDictionary<string, object> htmlAttributes) where TEnum : struct
        {
            var enums = Enum.GetValues(tEnum.GetType()).Cast<TEnum>().ToArray();
            StringBuilder buffter = new StringBuilder("");
            Array.ForEach(enums, p =>
                  buffter.AppendFormat("\n<option value=\"{0}\"{2}>{1}</option>",
                    Convert.ToInt32(p).ToString(),
                    HtmlExtensions.EnumDescriptionFor(p),
                    p.Equals(tEnum) ? " selected=\"selected\"" : ""
                  )
               );
            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = buffter.ToString()
            };
            tagBuilder.MergeAttributes<string, object>(htmlAttributes);
            tagBuilder.GenerateId(id);
            tagBuilder.MergeAttribute("name", name, true);
            return new MvcHtmlString(tagBuilder.ToString());
        }

    }
}
