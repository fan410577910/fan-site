﻿#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice © 2010-2014 TideBuy.com All rights reserved .
     * ========================================================================
     * 机器名称： 
     * 文件名：  RabbitHutch 
     * 版本号：  V1.0.0.0 
     * 创建人：  王云鹏 
     * 创建时间：2014/7/2 
     * 描述    : RabbitMQ框架
     * =====================================================================
     * 修改时间：2014/7/2
     * 修改人  ：  
     * 版本号  ： V1.0.0.0 
     * 描述    ：调用RabbitMQ里面的功能都从这里面出
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;

namespace FAN.RabbitMQ
{
    using System.Linq.Expressions;
    using System.Reflection;
    using FAN.RabbitMQ.Sprache;
    using UpdateConfiguration = Func<ConnectionConfiguration, ConnectionConfiguration>;

    public static class ConnectionStringGrammar
    {
        public static Parser<string> Text = Parse.CharExcept(';').Many().Text();
        public static Parser<ushort> Number = Parse.Number.Select(ushort.Parse);

        public static Parser<bool> Bool =
            (Parse.String("true").Or(Parse.String("false"))).Text().Select(x => x == "true");

        public static Parser<HostConfiguration> Host =
            from host in Parse.Char(c => c != ':' && c != ';' && c != ',', "host").Many().Text()
            from port in Parse.Char(':').Then(_ => Number).Or(Parse.Return((ushort)0))
            select new HostConfiguration { Host = host, Port = port };

        public static Parser<IEnumerable<HostConfiguration>> Hosts = Host.ListDelimitedBy(',');

        private static Uri result;
        public static Parser<Uri> AMQP = Parse.CharExcept(';').Many().Text().Where(x => Uri.TryCreate(x, UriKind.Absolute, out result)).Select(_ => new Uri(_));

        public static Parser<UpdateConfiguration> Part = new List<Parser<UpdateConfiguration>>
        {
            // add new connection string parts here
            BuildKeyValueParser("amqp", AMQP, c => c.AMQPConnectionString),
            BuildKeyValueParser("host", Hosts, c => c.Hosts),
            BuildKeyValueParser("port", Number, c => c.Port),
            BuildKeyValueParser("virtualHost", Text, c => c.VirtualHost),
            BuildKeyValueParser("requestedHeartbeat", Number, c => c.RequestedHeartbeat),
            BuildKeyValueParser("username", Text, c => c.UserName),
            BuildKeyValueParser("password", Text, c => c.Password),
            BuildKeyValueParser("prefetchcount", Number, c => c.PrefetchCount),
            BuildKeyValueParser("timeout", Number, c => c.Timeout),
            BuildKeyValueParser("publisherConfirms", Bool, c => c.PublisherConfirms),
            BuildKeyValueParser("persistentMessages", Bool, c => c.PersistentMessages),
            BuildKeyValueParser("cancelOnHaFailover", Bool, c => c.CancelOnHaFailover),
            BuildKeyValueParser("product", Text, c => c.Product),
            BuildKeyValueParser("platform", Text, c => c.Platform)
        }.Aggregate((a, b) => a.Or(b));

        public static Parser<UpdateConfiguration> AMQPAlone =
            AMQP.Select(_ => (Func<ConnectionConfiguration, ConnectionConfiguration>)(configuration
                                                                                       =>
            {
                configuration.AMQPConnectionString = _;
                return configuration;
            }));

        public static Parser<IEnumerable<UpdateConfiguration>> ConnectionStringBuilder = Part.ListDelimitedBy(';').Or(AMQPAlone.Once());

        public static Parser<UpdateConfiguration> BuildKeyValueParser<T>(
            string keyName,
            Parser<T> valueParser,
            Expression<Func<ConnectionConfiguration, T>> getter)
        {
            return
                from key in Parse.CaseInsensitiveString(keyName).Token()
                from separator in Parse.Char('=')
                from value in valueParser
                select (Func<ConnectionConfiguration, ConnectionConfiguration>)(c =>
                {
                    CreateSetter(getter)(c, value);
                    return c;
                });
        }

        public static Action<ConnectionConfiguration, T> CreateSetter<T>(Expression<Func<ConnectionConfiguration, T>> getter)
        {
            return CreateSetter<ConnectionConfiguration, T>(getter);
        }

        /// <summary>
        /// Stolen from SO:
        /// http://stackoverflow.com/questions/4596453/create-an-actiont-to-set-a-property-when-i-am-provided-with-the-linq-expres
        /// </summary>
        /// <typeparam name="TContaining"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="getter"></param>
        /// <returns></returns>
        public static Action<TContaining, TProperty> CreateSetter<TContaining, TProperty>(Expression<Func<TContaining, TProperty>> getter)
        {
            Preconditions.CheckNotNull(getter, "getter");

            var memberEx = getter.Body as MemberExpression;

            Preconditions.CheckNotNull(memberEx, "getter", "Body is not a member-expression.");

            var property = memberEx.Member as PropertyInfo;

            Preconditions.CheckNotNull(property, "getter", "Member is not a property.");
            Preconditions.CheckTrue(property.CanWrite, "getter", "Member is not a writeable property.");

            return (Action<TContaining, TProperty>)
                Delegate.CreateDelegate(typeof(Action<TContaining, TProperty>),
                    property.GetSetMethod());
        }

        public static IEnumerable<T> Cons<T>(this T head, IEnumerable<T> rest)
        {
            yield return head;
            foreach (var item in rest)
                yield return item;
        }

        public static Parser<IEnumerable<T>> ListDelimitedBy<T>(this Parser<T> parser, char delimiter)
        {
            return
                from head in parser
                from tail in Parse.Char(delimiter).Then(_ => parser).Many()
                select head.Cons(tail);
        }
    }
}
