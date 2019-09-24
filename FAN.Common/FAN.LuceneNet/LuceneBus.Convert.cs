#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  LuceneBus.Convert 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/28
     * 描述    : 
     * =====================================================================
     * 修改时间：2014/7/28
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TLZ.LuceneNet
{
    partial class LuceneBus
    {
        /// <summary>
        /// 将对象转成Lucene里面的Document类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="columnFields"></param>
        /// <returns></returns>
        public static Document Convert(object obj, ColumnField[] columnFields)
        {
            Document document = null;
            if (obj != null)
            {
                document = new Document();
                Type type = obj.GetType();
                foreach (ColumnField columnField in columnFields)
                {
                    if (columnField.IsCustomScore)
                    {//为文档增加自定义评分列
                        NumericField numericField = new NumericField(columnField.Column, columnField.Store, true);
                        numericField.SetIntValue(1);
                        document.Add(numericField);
                        columnField.Type = FieldType.INT32;
                        continue;
                    }
                    Lucene.Net.Documents.Field.Store store = columnField.Store;

                    string fieldName = columnField.Column;
                    PropertyInfo propertyInfo = type.GetProperty(fieldName);
                    if (propertyInfo != null)
                    {
                        object value = propertyInfo.GetValue(obj, null);
                        if (value != null)
                        {
                            if (columnField.Index == Field.Index.ANALYZED//分词建索引
                                || columnField.Index == Field.Index.ANALYZED_NO_NORMS)//分词建索引，不支持权重
                            {
                                Field field = new Field(fieldName, value.ToString(), store, columnField.Index);
                                field.Boost = columnField.Boost;
                                document.Add(field);
                                columnField.Type = FieldType.STRING;
                            }
                            else
                            {
                                string typeName = value.GetType().Name;
                                if (typeName == FieldType.SINGLE)
                                {
                                    NumericField numericField = new NumericField(fieldName, store, true);
                                    numericField.SetFloatValue(System.Convert.ToSingle(value));
                                    numericField.Boost = columnField.Boost;
                                    document.Add(numericField);
                                    columnField.Type = FieldType.SINGLE;
                                }
                                else if (typeName == FieldType.DOUBLE || typeName == FieldType.DECIMAL)
                                {
                                    NumericField numericField = new NumericField(fieldName, store, true);
                                    numericField.SetDoubleValue(System.Convert.ToDouble(value));
                                    numericField.Boost = columnField.Boost;
                                    document.Add(numericField);
                                    columnField.Type = FieldType.DOUBLE;
                                }
                                else if (typeName == FieldType.INT32)
                                {
                                    NumericField numericField = new NumericField(fieldName, store, true);
                                    numericField.SetIntValue(System.Convert.ToInt32(value));
                                    numericField.Boost = columnField.Boost;
                                    document.Add(numericField);
                                    columnField.Type = FieldType.INT32;
                                }
                                else if (typeName == FieldType.INT64)
                                {
                                    NumericField numericField = new NumericField(fieldName, store, true);
                                    numericField.SetLongValue(System.Convert.ToInt64(value));
                                    numericField.Boost = columnField.Boost;
                                    document.Add(numericField);
                                    columnField.Type = FieldType.INT64;
                                }
                                else if (typeName == FieldType.DATETIME)
                                {
                                    NumericField numericField = new NumericField(fieldName, store, true);
                                    DateTime dateTime = System.Convert.ToDateTime(value);
                                    numericField.SetLongValue(dateTime.Ticks);
                                    numericField.Boost = columnField.Boost;
                                    document.Add(numericField);
                                    columnField.Type = FieldType.DATETIME;
                                }
                                else
                                {
                                    Field field = new Field(fieldName, value.ToString(), store, columnField.Index);
                                    field.Boost = columnField.Boost;
                                    document.Add(field);
                                    columnField.Type = FieldType.STRING;
                                }
                            }
                        }
                    }
                }
            }
            return document;
        }
        /// <summary>
        /// 将集合对象转成Lucene里面的Document类型的集合对象
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="columnFields"></param>
        /// <returns></returns>
        public static List<Document> Convert(List<object> objectList, ColumnField[] columnFields)
        {
            List<Document> documentList = null;
            if (objectList != null)
            {
                documentList = new List<Document>(objectList.Count);
                foreach (object obj in objectList)
                {
                    Document document = Convert(obj, columnFields);
                    documentList.Add(document);
                }
            }
            return documentList;
        }

        /// <summary>
        /// 将Luncene中查询出来的文档列表转换成为对象列表
        /// </summary>
        /// <param name="documentList"></param>
        /// <returns></returns>
        public static List<object> Convert(Dictionary<Document, ScoreDoc> documentList)
        {
            List<object> objectList = new List<object>();
            if (documentList != null)
            {
                Dictionary<Document, ScoreDoc>.KeyCollection keys = documentList.Keys;
                foreach (Document document in keys)
                {
                    IList<IFieldable> fieldList = document.GetFields();
                    Type type = DynamicClassEx.BuildType("DocumentType");
                    List<DynamicClassEx.CustomerPropertyInfo> lcpi = new List<DynamicClassEx.CustomerPropertyInfo>();
                    foreach (IFieldable field in fieldList)
                    {
                        DynamicClassEx.CustomerPropertyInfo cpi = new DynamicClassEx.CustomerPropertyInfo("System.Object", field.Name);
                        lcpi.Add(cpi);
                    }
                    type = DynamicClassEx.AddProperty(type, lcpi);

                    objectList.Add(Convert(type, fieldList));
                }
            }
            return objectList;
        }

        private static object Convert(Type type, IList<IFieldable> fieldList)
        {
            int columnCount = fieldList.Count;
            object obj = DynamicClassEx.CreateInstance(type);
            foreach (IFieldable field in fieldList)
            {
                string columnName = field.Name;
                string value = field.StringValue;
                DynamicClassEx.SetPropertyValue(obj, columnName, value);
            }
            return obj;
        }

        public static Dictionary<T, ScoreDoc> Convert<T>(Dictionary<Document, ScoreDoc> documentDict)
        {
            Dictionary<T, ScoreDoc> dict = null;
            if (documentDict != null)
            {
                dict = new Dictionary<T, ScoreDoc>(documentDict.Count);
                Dictionary<Document, ScoreDoc>.KeyCollection keys = documentDict.Keys;
                Type type = typeof(T);
                foreach (Document document in keys)
                {
                    if(document==null)
                    {
                        continue;
                    }
                    T @object = Activator.CreateInstance<T>();
                    IList<IFieldable> fieldList = document.GetFields();
                    foreach (IFieldable field in fieldList)
                    {
                        string value = field.StringValue;
                        if (!string.IsNullOrEmpty(value))
                        {
                            string fieldName = field.Name;
                            PropertyInfo propertyInfo = null;
                            try
                            {
                                propertyInfo = type.GetProperty(fieldName);
                            }
                            catch (Exception)
                            {
                                propertyInfo = null;
                            }
                            if (propertyInfo != null)
                            {
                                string propertyTypeName = propertyInfo.PropertyType.Name;
                                try
                                {
                                    switch (propertyTypeName)
                                    {
                                        case FieldType.DATETIME:
                                            propertyInfo.SetValue(@object, new DateTime(System.Convert.ToInt64(value)), null);
                                            break;
                                        case FieldType.DECIMAL:
                                            propertyInfo.SetValue(@object, System.Convert.ToDecimal(value), null);
                                            break;
                                        case FieldType.DOUBLE:
                                            propertyInfo.SetValue(@object, System.Convert.ToDouble(value), null);
                                            break;
                                        case FieldType.INT32:
                                            propertyInfo.SetValue(@object, System.Convert.ToInt32(value), null);
                                            break;
                                        case FieldType.INT64:
                                            propertyInfo.SetValue(@object, System.Convert.ToInt64(value), null);
                                            break;
                                        case FieldType.SINGLE:
                                            propertyInfo.SetValue(@object, System.Convert.ToSingle(value), null);
                                            break;
                                        case FieldType.STRING:
                                        default:
                                            propertyInfo.SetValue(@object, value, null);
                                            break;
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    }
                    dict.Add(@object, documentDict[document]);
                }
            }
            return dict;
        }
    }
}
