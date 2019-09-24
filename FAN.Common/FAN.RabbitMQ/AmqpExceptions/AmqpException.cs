#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  AmqpException 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/2 
     * 描述    : AMQP协议通用异常类型,RabbitMQ也是遵循AMQP协议开发出来的通用中间件，本类定义AMQP协议里的错误信息，转成用C#表示的异常类型
     * =====================================================================
     * 修改时间：2014/7/2
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ： 
*/
#endregion
using System.Collections.Generic;
using System.Linq;
/*
 * AMQP协议通用异常类型
 * RabbitMQ也是遵循AMQP协议开发出来的通用中间件，本类定义AMQP协议里的错误信息，转成用C#表示的异常类型
 */
namespace FAN.RabbitMQ.AmqpExceptions
{
    public class AmqpException
    {
        public AmapExceptionPreface Preface { get; private set; }
        public IList<IAmqpExceptionElement> Elements { get; private set; }

        public AmqpException(AmapExceptionPreface preface, IList<IAmqpExceptionElement> elements)
        {
            Preface = preface;
            Elements = elements;
        }

        private int GetElement<T>() where T : AmqpExceptionIntegerValueElement
        {
            return Elements.OfType<T>().Select(x => x.Value).SingleOrDefault();
        }

        public int Code { get { return GetElement<AmqpExceptionCodeElement>(); } }
        public int ClassId { get { return GetElement<AmqpExceptionClassIdElement>(); } }
        public int MethodId { get { return GetElement<AmqpExceptionMethodIdElement>(); } }

        public static ushort ConnectionClosed = 320;
    }

    public class AmapExceptionPreface
    {
        public string Text { get; private set; }

        public AmapExceptionPreface(string text)
        {
            Text = text;
        }
    }

    public interface IAmqpExceptionElement { }

    public class TextElement : IAmqpExceptionElement
    {
        public string Text { get; private set; }

        public TextElement(string text)
        {
            Text = text;
        }
    }

    public class AmqpExceptionKeyValueElement : IAmqpExceptionElement
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public AmqpExceptionKeyValueElement(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public abstract class AmqpExceptionIntegerValueElement : IAmqpExceptionElement
    {
        public int Value { get; set; }
    }

    public class AmqpExceptionCodeElement : AmqpExceptionIntegerValueElement { }
    public class AmqpExceptionClassIdElement : AmqpExceptionIntegerValueElement { }
    public class AmqpExceptionMethodIdElement : AmqpExceptionIntegerValueElement { }
}