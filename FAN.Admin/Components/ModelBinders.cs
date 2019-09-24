#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2010-2018 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称：USER-TI50KE6KO4 
     * 文件名：  ModelBinders 
     * 版本号：  V1.0.0.0 
     * 创建人：  fan
     * 创建时间： 2018/5/31 8:55:18 
     * 描述    :
     * =====================================================================
     * 修改时间：2018/5/31 8:55:18 
     * 修改人  ： Administrator
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using FAN.Entity;
using FAN.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FAN.Admin.Components
{
    /// <summary>
    /// 绑定对象的添加时间、添加用户名/更新时间、更新用户名
    /// 本类只能用在增加、修改。
    /// </summary>
    public class DMLModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Type modelType = bindingContext.ModelType;
            object obj = Activator.CreateInstance(modelType);
            PropertyInfo[] properties = modelType.GetProperties();
            object idValue = null;
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                Type propertyType = property.PropertyType;
                ValueProviderResult providerResult = bindingContext.ValueProvider.GetValue(propertyName);
                object objValue = null;
                if (providerResult != null)
                {
                    objValue = providerResult.ConvertTo(propertyType);
                }
                if (objValue != null)
                {
                    property.SetValue(obj, objValue);
                }
                if (propertyName == "ID")
                {
                    idValue = objValue;
                }
            }
            if (obj is IEntity)
            {
                IEntity entity = obj as IEntity;
                if (idValue != null && TypeParseHelper.StrToInt32(idValue) > 0)
                {
                    entity.UpdateTime = DateTime.Now;
                    entity.UpdateUserName = "zhangsan";
                }
                else
                {
                    entity.CreateTime = DateTime.Now;
                    entity.CreateUserName = "zhangsan";
                }
            }

            return obj;
        }
    }

    /// <summary>
    /// JSON序列化对象
    /// </summary>
    /// <typeparam name="T">泛指当前传递的对象类型</typeparam>
    public class JsonBinder<T> : System.Web.Mvc.IModelBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext">controller上下文</param>
        /// <param name="bindingContext">提交数据模型的上下文</param>
        /// <returns>object 表示通过Json字符串序列化的对象对象实例</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object @object = null;
            //从请求中获取提交的参数数据,ModelName名称为当前方法名称 
            string json = controllerContext.HttpContext.Request.Params[bindingContext.ModelName] as string;
            if (json != null)
            {
                Type type = typeof(T);
                //提交参数是对象 
                if (json.StartsWith("{") && json.EndsWith("}"))
                {
                    @object = JsonConvert.DeserializeObject<T>(json);
                    //@object = new JsonSerializer().Deserialize(JObject.Parse(json).CreateReader(), type);
                }
                //提交参数是数组 
                else if (json.StartsWith("[") && json.EndsWith("]"))
                {
                    @object = JsonConvert.DeserializeObject<List<T>>(json);
                }
                //提交参数是字符串
                else
                {
                    @object = json;
                }
            }
            return @object;
        }
    }

    /// <summary>
    /// 字符串处理类
    /// </summary>
    public class StringModelBuilder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object value = base.BindModel(controllerContext, bindingContext);
            if (value is string)
            {
                value = value.ToString().Trim();
            }
            return value;
        }
    }

    /// <summary>
    /// 绑定对象的添加时间、添加用户名
    /// </summary>
    [Obsolete("替换，用DMLModelBinder就可以")]
    public class CreateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object obj = base.BindModel(controllerContext, bindingContext);
            Type objType = obj.GetType();
            PropertyInfo createTimeProperty = objType.GetProperty("CreateTime");
            if (createTimeProperty != null)
            {
                createTimeProperty.SetValue(obj, DateTime.Now);
            }
            PropertyInfo userNameProperty = objType.GetProperty("CreateUserName");
            if (userNameProperty != null)
            {
                userNameProperty.SetValue(obj, "zhangsan");
            }
            return obj;
        }
    }

    /// <summary>
    /// 绑定对象的更新时间、更新用户名
    /// </summary>
    [Obsolete("替换，用DMLModelBinder就可以")]
    public class UpdateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object obj = base.BindModel(controllerContext, bindingContext);
            Type objType = obj.GetType();
            PropertyInfo updateTimeProperty = objType.GetProperty("UpdateTime");
            if (updateTimeProperty != null)
            {
                updateTimeProperty.SetValue(obj, DateTime.Now);
            }
            PropertyInfo updateUserNameProperty = objType.GetProperty("UpdateUserName");
            if (updateUserNameProperty != null)
            {
                updateUserNameProperty.SetValue(obj, "zhangsan");
            }
            return obj;
        }
    }

}