using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System;

namespace ConnectionParser
{
    using System.Collections.Generic;
    using UpdateConfiguration = Func<ConnectionConfiguration, ConnectionConfiguration>;

    public static class ConnectionStringGrammar
    {
        public static Parser<string> Text = Parse.CharExcept(';').Many().Text();
        public static Parser<int> Number = Parse.Number.Select(int.Parse);

        public static Parser<bool> Bool =
            (Parse.String("true").Or(Parse.String("false"))).Text().Select(x => x == "true");

        public static Parser<UpdateConfiguration> Part = new List<Parser<UpdateConfiguration>>
        {
            // add new connection string parts here
            BuildKeyValueParser("downHost", Text, c => c.DownHost),
            BuildKeyValueParser("downPort", Number, c => c.DownPort),
            BuildKeyValueParser("downRootPath", Text, c => c.DownRootPath),
            BuildKeyValueParser("downUID", Text, c => c.DownUID),
            BuildKeyValueParser("downPWD", Text, c => c.DownPWD),
            BuildKeyValueParser("uploadHost", Text, c => c.UploadHost),
            BuildKeyValueParser("uploadPort", Number, c => c.UploadPort),
            BuildKeyValueParser("uploadRootPath", Text, c => c.UploadRootPath),
            BuildKeyValueParser("uploadUID", Text, c => c.UploadUID),
            BuildKeyValueParser("uploadPWD", Text, c => c.UploadPWD)
        }.Aggregate((a, b) => a.Or(b));

        public static Parser<IEnumerable<UpdateConfiguration>> ConnectionStringBuilder = Part.ListDelimitedBy(';');

        public static Parser<UpdateConfiguration> BuildKeyValueParser<T>(
            string keyName,
            Parser<T> valueParser,
            Expression<Func<ConnectionConfiguration, T>> getter)
        {
            return
                from key in Parse.CaseInsensitiveString(keyName).Token()
                from separator in Parse.Char('=')
                from value in valueParser
                select (UpdateConfiguration)(c =>
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
            var memberEx = getter.Body as MemberExpression;

            var property = memberEx.Member as PropertyInfo;

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
