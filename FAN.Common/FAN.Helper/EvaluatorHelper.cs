using System;
using System.CodeDom.Compiler;

using System.Reflection;

namespace FAN.Helper
{
    /// <summary>
    /// 动态求值
    /// 必须采用实例方法不能改为静态方法调用不然仁林那边高并发获取运费计算那里会出错！！！！2018-02-06.wangyunpeng.yanghuanwen.
    /// </summary>
    public class EvaluatorHelper
    {
        /// <summary>
        /// 计算结果，性能不高，支持公式。
        /// </summary>
        /// <param name="statement">表达式,如"1+2+3+4"</param>
        /// <returns>结果</returns>
        public string Eval(string statement)
        {
            //return _evaluatorType.InvokeMember(
            //            "Eval",
            //            BindingFlags.InvokeMethod,
            //            null,
            //            _evaluator,
            //            new object[] { statement }
            //         );
            return _evaluatorFunc(statement);
        }

        public EvaluatorHelper()
        {
            //构造JScript的编译驱动代码
            CodeDomProvider provider = CodeDomProvider.CreateProvider("JScript");
            CompilerParameters parameters = new CompilerParameters() { GenerateInMemory = true };
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, _jscriptSource);
            Assembly assembly = results.CompiledAssembly;

            _evaluatorType = assembly.GetType("Evaluator");
            _evaluator = Activator.CreateInstance(_evaluatorType);
            _evaluatorFunc = (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), _evaluator, "Eval", false);
        }

        private readonly object _evaluator = null;
        private readonly Type _evaluatorType = null;
        private readonly Func<string, string> _evaluatorFunc = null;

        /// <summary>
        /// JScript代码
        /// </summary>
        private readonly string _jscriptSource =
             @"class Evaluator
               {
                   public function Eval(expr : String) : String 
                   { 
                      return eval(expr); 
                   }
               }";


        private delegate TResult Func<in T1, out TResult>(T1 arg1);

        /// <summary>
        /// https://www.cnblogs.com/songxingzhu/p/6737618.html
        /// https://github.com/davideicardi/DynamicExpresso
        /// 高性能版本，支持公式计算。wangyunpeng。2018-02-07
        /// </summary>
        /// <param name="statement">支持公式计算</param>
        /// <returns></returns>
        public static string Calc(string statement)
        {
            object @object = new DynamicExpresso.Interpreter().Eval(statement);
            if (@object == null)
            {
                return string.Empty;
            }
            else
            {
                return @object.ToString();
            }
        }
        /// <summary>
        /// https://www.cnblogs.com/liweis/p/6703314.html?utm_source=itdadao&utm_medium=referral
        /// 高性能版本，不支持公式计算。wangyunpeng。2018-02-06
        /// </summary>
        /// <param name="statement">不支持公式计算</param>
        /// <returns></returns>
        //public static string Exp(string statement)
        //{
        //    TypeRegistry types = new TypeRegistry();
        //    types.RegisterDefaultTypes();
        //    CompiledExpression expression = new CompiledExpression(statement) { TypeRegistry = types };
        //    object result = expression.Eval();
        //    return result.ToString();
        //}
    }
}
