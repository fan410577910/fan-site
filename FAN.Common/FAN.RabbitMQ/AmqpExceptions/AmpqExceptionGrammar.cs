#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  AmqpExceptionGrammar 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/2 
     * 描述    : RabbitMQ框架
     * =====================================================================
     * 修改时间：2014/7/2
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：
*/
#endregion
using System.Collections.Generic;
using FAN.RabbitMQ.Sprache;

namespace FAN.RabbitMQ.AmqpExceptions
{
    public static class AmqpExceptionGrammar
    {
        public static Parser<ushort> Number = Parse.Number.Select(ushort.Parse);

        public static Parser<T> MakeIntegerElementParser<T>(string key) where T : AmqpExceptionIntegerValueElement, new()
        {
            return
                from k in Parse.CaseInsensitiveString(key).Token()
                from eq in Parse.Char('=')
                from value in Number
                select new T { Value = value };
        }

        static readonly Parser<IAmqpExceptionElement> codeElement = MakeIntegerElementParser<AmqpExceptionCodeElement>("code");
        static readonly Parser<IAmqpExceptionElement> classIdElement = MakeIntegerElementParser<AmqpExceptionClassIdElement>("classId");
        static readonly Parser<IAmqpExceptionElement> methodIdElement = MakeIntegerElementParser<AmqpExceptionMethodIdElement>("methodId");

        static readonly Parser<IAmqpExceptionElement> keyValueElement =
            from key in Parse.CharExcept(c => c == '=' || c == ',', "").Many().Text().Token()
            from eq in Parse.Char('=')
            from value in Parse.CharExcept(',').Many().Text()
            select new AmqpExceptionKeyValueElement(key, value);

        static readonly Parser<IAmqpExceptionElement> textElement =
            from text in Parse.CharExcept(',').Many().Text()
            select new TextElement(text);

        static readonly Parser<IAmqpExceptionElement> element = methodIdElement.Or(
            classIdElement.Or(
            codeElement.Or(
            keyValueElement.Or(
            textElement))));

        static readonly Parser<IEnumerable<IAmqpExceptionElement>> elements = element.ListDelimitedBy(',');

        static readonly Parser<AmapExceptionPreface> preface =
            from text in Parse.CharExcept(':').Many().Text()
            from colon in Parse.Char(':')
            select new AmapExceptionPreface(text);

        static readonly Parser<AmqpException> exception =
            from p in preface
            from e in elements
            select new AmqpException(p, new List<IAmqpExceptionElement>(e));

        public static AmqpException ParseExceptionString(string exceptionMessage)
        {
            return exception.Parse(exceptionMessage);
        }
    }
}