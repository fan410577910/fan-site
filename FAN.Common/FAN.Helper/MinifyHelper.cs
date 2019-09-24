using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yahoo.Yui.Compressor;

namespace FAN.Helper
{
    /// <summary>
    /// 最小化页面工具类
    /// </summary>
    public class MinifyHelper
    {
        #region 删除注释
        /// <summary>
        /// 多行注释
        /// </summary>
        private static readonly Regex _MutilNoteRegex = new Regex(@"(?:^|\n|\r)\s*\/\*[\s\S]*?\*\/\s*(?:\r|\n|$)", RegexOptions.Compiled);
        /// <summary>
        /// 单行注释
        /// </summary>
        private static readonly Regex _SingleNoteRegex = new Regex(@"(?:^|\n|\r)\s*\/\/.*(?:\r|\n|$)", RegexOptions.Compiled);

        /// <summary>
        /// 删除注释
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string RemoveNote(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                content = _MutilNoteRegex.Replace(content, string.Empty);
                content = _SingleNoteRegex.Replace(content, string.Empty);
                return content;
            }
            return null;
        }
        #endregion

        #region 正则表达式
        /// <summary>
        /// 最小化HTML内容
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string MinifyHtml(string htmlContent)
        {
            if (!string.IsNullOrWhiteSpace(htmlContent))
            {
                htmlContent = RemoveNote(htmlContent);
                foreach (Expression expression in ExpressionCollection.Instance)
                {
                    if (expression.ReplaceHandler == null)
                    {
                        htmlContent = expression.RegularExpression.Replace(htmlContent, expression.ReplaceWith);
                    }
                    else
                    {
                        htmlContent = expression.ReplaceHandler(expression, htmlContent);
                    }
                }
                return htmlContent;
            }
            return null;
        }
        /// <summary>
        /// 去掉注释和空行
        /// </summary>
        /// <param name="jsContent"></param>
        /// <returns></returns>
        [Obsolete("过期了")]
        public static string MinifyJavaScript(string jsContent)
        {
            if (!string.IsNullOrWhiteSpace(jsContent))
            {
                try
                {
                    //注释
                    jsContent = Regex.Replace(jsContent, @"(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)", string.Empty, RegexOptions.Singleline);
                    //空行
                    jsContent = Regex.Replace(jsContent, @"\r\n\s*|\n\s*", string.Empty, RegexOptions.Multiline);
                }
                catch { }
                return jsContent;
            }
            return null;
        }
        #endregion

        #region 微软压缩
        /// <summary>
        /// 最小化css内容
        /// </summary>
        /// <param name="cssContent"></param>
        /// <returns></returns>
        public static string MinifyCss(string cssContent)
        {
            if (!string.IsNullOrWhiteSpace(cssContent))
            {
                try
                {
                    cssContent = new Minifier().MinifyStyleSheet(cssContent, new CssSettings
                    {
                        CommentMode = CssComment.None
                    });
                }
                catch { }
                return cssContent;
            }
            return null;
        }
        /// <summary>
        /// 最小化javascript内容
        /// </summary>
        /// <param name="jsContent"></param>
        /// <returns></returns>
        public static string MinifyJavascript(string jsContent)
        {
            if (!string.IsNullOrWhiteSpace(jsContent))
            {
                try
                {
                    jsContent = new Minifier().MinifyJavaScript(jsContent, new CodeSettings
                    {
                        EvalTreatment = EvalTreatment.MakeAllSafe,
                        PreserveImportantComments = false
                    });
                }
                catch { }
                return jsContent;
            }
            return null;
        }
        #endregion

        #region 雅虎压缩
        /// <summary>
        /// 压缩javascript内容
        /// </summary>
        /// <param name="jsContent"></param>
        /// <returns></returns>
        public static string CompressJavascript(string jsContent)
        {
            if (!string.IsNullOrWhiteSpace(jsContent))
            {
                //压缩该js
                try
                {
                    jsContent = new JavaScriptCompressor()
                    {
                        CompressionType = CompressionType.Standard//压缩类型
                        ,
                        Encoding = System.Text.Encoding.UTF8//编码
                        ,
                        IgnoreEval = false//大小写转换
                        ,
                        ThreadCulture = System.Globalization.CultureInfo.CurrentCulture
                    }.Compress(jsContent);
                }
                catch { }
                return jsContent;
            }
            return null;
        }
        /// <summary>
        /// 压缩css内容
        /// </summary>
        /// <param name="cssContent"></param>
        /// <returns></returns>
        public static string CompressCss(string cssContent)
        {
            if (!string.IsNullOrWhiteSpace(cssContent))
            {
                try
                {
                    cssContent = new CssCompressor().Compress(cssContent);
                }
                catch { }
                return cssContent;
            }
            return null;
        }
        #endregion

        #region ECMAScript压缩
        /// <summary>
        /// 压缩javascript内容
        /// </summary>
        /// <param name="jsContent"></param>
        /// <returns></returns>
        public static string PackJavascript(string jsContent)
        {
            if (!string.IsNullOrWhiteSpace(jsContent))
            {
                try
                {
                    jsContent = new ECMAScriptPacker().Pack(jsContent).Replace("\n", "\r\n");
                }
                catch { }
                return jsContent;
            }
            return null;
        }
        #endregion
    }
    internal class ExpressionCollection
         : List<Expression>
    {
        private static object synclock = new object();
        private static ExpressionCollection _instance;
        private static bool SupressWithWhiteSpace = false;
        internal static ExpressionCollection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        if (_instance == null)
                            _instance = new ExpressionCollection();
                    }
                }
                return _instance;
            }
        }
        private ExpressionCollection()
        {
            #region Regular Expressions
            //remove tabs
            Expression expression = new Expression(new Regex("\t", RegexOptions.Compiled | RegexOptions.Multiline));
            this.Add(expression);

            //remove space after and before tag
            Regex spaceBeforeTag = new Regex(@"\s+<", RegexOptions.Multiline | RegexOptions.Compiled);
            Regex spaceAfterTag = new Regex(@">\s+", RegexOptions.Compiled | RegexOptions.Multiline);
            List<Regex> lstExprs = new List<Regex>();
            lstExprs.Add(spaceBeforeTag);
            lstExprs.Add(spaceAfterTag);
            expression = new Expression(lstExprs);
            expression.ReplaceHandler = (expres, input) =>
            {
                input = expres.RegularExpressions[0].Replace(input, SupressWithWhiteSpace ? " <" : "<");
                input = expres.RegularExpressions[1].Replace(input, SupressWithWhiteSpace ? "> " : ">");
                return input;
            };
            this.Add(expression);

            //remove new line character
            expression = new Expression(new Regex(@"\r\n\s*|\n\s*", RegexOptions.Multiline | RegexOptions.Compiled));
            this.Add(expression);

            //remove html comments
            expression = new Expression(new Regex(@"<!--[\w\W\r\n]*?-->", RegexOptions.Multiline | RegexOptions.Compiled));
            this.Add(expression);

            //remove html comments '<!-- -->' from javascript
            expression = new Expression(new Regex(@"<script[\s\=\""\'\/\\\w]+>(<!--)?[\w\W\r\n]*?(//\s*-->)?</script>", RegexOptions.Compiled | RegexOptions.Multiline));
            expression.ReplaceHandler = (expr, input) =>
            {
                string str = string.Empty;
                string mainString = string.Empty;
                if (expr.RegularExpression.IsMatch(input))
                {
                    MatchCollection matchColl = expr.RegularExpression.Matches(input);
                    foreach (Match match in matchColl)
                    {
                        GroupCollection gc = match.Groups;
                        mainString = gc[0].Value;
                        if (gc[1].Value != string.Empty && gc[2].Value != string.Empty)
                        {
                            input = input.Replace(mainString, mainString.Replace(gc[1].Value, string.Empty).Replace(gc[2].Value, string.Empty));
                        }
                    }
                }
                return input;
            };
            base.Add(expression);
            #endregion
        }
    }
    internal delegate string Replace(Expression sender, string input);
    internal class Expression
    {
        internal Expression()
        { }

        internal Expression(Regex expr, string replaceWith = "")
        {
            RegularExpression = expr;
            ReplaceWith = replaceWith;
        }

        internal Expression(List<Regex> exprs, string replaceWith = "")
        {
            RegularExpressions = exprs;
            ReplaceWith = replaceWith;
        }

        internal Regex RegularExpression { get; private set; }

        internal List<Regex> RegularExpressions { get; private set; }

        internal string ReplaceWith { get; private set; }

        internal Replace ReplaceHandler { get; set; }
    }
    /// <summary>
	/// Packs a javascript file into a smaller area, removing unnecessary characters from the output.
	/// </summary>
    internal class ECMAScriptPacker
    {
        /// <summary>
        /// The encoding level to use. See http://dean.edwards.name/packer/usage/ for more info.
        /// </summary>
        public enum PackerEncoding { None = 0, Numeric = 10, Mid = 36, Normal = 62, HighAscii = 95 };

        private PackerEncoding encoding = PackerEncoding.Normal;
        private bool fastDecode = true;
        private bool specialChars = false;
        private bool enabled = true;

        string IGNORE = "$1";

        /// <summary>
        /// The encoding level for this instance
        /// </summary>
        public PackerEncoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        /// <summary>
        /// Adds a subroutine to the output to speed up decoding
        /// </summary>
        public bool FastDecode
        {
            get { return fastDecode; }
            set { fastDecode = value; }
        }

        /// <summary>
        /// Replaces special characters
        /// </summary>
        public bool SpecialChars
        {
            get { return specialChars; }
            set { specialChars = value; }
        }

        /// <summary>
        /// Packer enabled
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public ECMAScriptPacker()
        {
            Encoding = PackerEncoding.Normal;
            FastDecode = true;
            SpecialChars = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encoding">The encoding level for this instance</param>
        /// <param name="fastDecode">Adds a subroutine to the output to speed up decoding</param>
        /// <param name="specialChars">Replaces special characters</param>
        public ECMAScriptPacker(PackerEncoding encoding, bool fastDecode, bool specialChars)
        {
            Encoding = encoding;
            FastDecode = fastDecode;
            SpecialChars = specialChars;
        }

        /// <summary>
        /// Packs the script
        /// </summary>
        /// <param name="script">the script to pack</param>
        /// <returns>the packed script</returns>
        public string Pack(string script)
        {
            if (enabled)
            {
                script += "\n";
                script = basicCompression(script);
                if (SpecialChars)
                    script = encodeSpecialChars(script);
                if (Encoding != PackerEncoding.None)
                    script = encodeKeywords(script);
            }
            return script;
        }

        //zero encoding - just removal of whitespace and comments
        private string basicCompression(string script)
        {
            ParseMaster parser = new ParseMaster();
            // make safe
            parser.EscapeChar = '\\';
            // protect strings
            parser.Add("'[^'\\n\\r]*'", IGNORE);
            parser.Add("\"[^\"\\n\\r]*\"", IGNORE);
            // remove comments
            parser.Add("\\/\\/[^\\n\\r]*[\\n\\r]");
            parser.Add("\\/\\*[^*]*\\*+([^\\/][^*]*\\*+)*\\/");
            // protect regular expressions
            parser.Add("\\s+(\\/[^\\/\\n\\r\\*][^\\/\\n\\r]*\\/g?i?)", "$2");
            parser.Add("[^\\w\\$\\/'\"*)\\?:]\\/[^\\/\\n\\r\\*][^\\/\\n\\r]*\\/g?i?", IGNORE);
            // remove: ;;; doSomething();
            if (specialChars)
                parser.Add(";;[^\\n\\r]+[\\n\\r]");
            // remove redundant semi-colons
            parser.Add(";+\\s*([};])", "$2");
            // remove white-space
            parser.Add("(\\b|\\$)\\s+(\\b|\\$)", "$2 $3");
            parser.Add("([+\\-])\\s+([+\\-])", "$2 $3");
            parser.Add("\\s+");
            // done
            return parser.Exec(script);
        }

        WordList encodingLookup;
        private string encodeSpecialChars(string script)
        {
            ParseMaster parser = new ParseMaster();
            // replace: $name -> n, $$name -> na
            parser.Add("((\\$+)([a-zA-Z\\$_]+))(\\d*)",
                new ParseMaster.MatchGroupEvaluator(encodeLocalVars));

            // replace: _name -> _0, double-underscore (__name) is ignored
            Regex regex = new Regex("\\b_[A-Za-z\\d]\\w*");

            // build the word list
            encodingLookup = analyze(script, regex, new EncodeMethod(encodePrivate));

            parser.Add("\\b_[A-Za-z\\d]\\w*", new ParseMaster.MatchGroupEvaluator(encodeWithLookup));

            script = parser.Exec(script);
            return script;
        }

        private string encodeKeywords(string script)
        {
            // escape high-ascii values already in the script (i.e. in strings)
            if (Encoding == PackerEncoding.HighAscii) script = escape95(script);
            // create the parser
            ParseMaster parser = new ParseMaster();
            EncodeMethod encode = getEncoder(Encoding);

            // for high-ascii, don't encode single character low-ascii
            Regex regex = new Regex(
                    (Encoding == PackerEncoding.HighAscii) ? "\\w\\w+" : "\\w+"
                );
            // build the word list
            encodingLookup = analyze(script, regex, encode);

            // encode
            parser.Add((Encoding == PackerEncoding.HighAscii) ? "\\w\\w+" : "\\w+",
                new ParseMaster.MatchGroupEvaluator(encodeWithLookup));

            // if encoded, wrap the script in a decoding function
            return (script == string.Empty) ? "" : bootStrap(parser.Exec(script), encodingLookup);
        }

        private string bootStrap(string packed, WordList keywords)
        {
            // packed: the packed script
            packed = "'" + escape(packed) + "'";

            // ascii: base for encoding
            int ascii = Math.Min(keywords.Sorted.Count, (int)Encoding);
            if (ascii == 0)
                ascii = 1;

            // count: number of words contained in the script
            int count = keywords.Sorted.Count;

            // keywords: list of words contained in the script
            foreach (object key in keywords.Protected.Keys)
            {
                keywords.Sorted[(int)key] = "";
            }
            // convert from a string to an array
            StringBuilder sbKeywords = new StringBuilder("'");
            foreach (string word in keywords.Sorted)
                sbKeywords.Append(word + "|");
            sbKeywords.Remove(sbKeywords.Length - 1, 1);
            string keywordsout = sbKeywords.ToString() + "'.split('|')";

            string encode;
            string inline = "c";

            switch (Encoding)
            {
                case PackerEncoding.Mid:
                    encode = "function(c){return c.toString(36)}";
                    inline += ".toString(a)";
                    break;
                case PackerEncoding.Normal:
                    encode = "function(c){return(c<a?\"\":e(parseInt(c/a)))+" +
                        "((c=c%a)>35?String.fromCharCode(c+29):c.toString(36))}";
                    inline += ".toString(a)";
                    break;
                case PackerEncoding.HighAscii:
                    encode = "function(c){return(c<a?\"\":e(c/a))+" +
                        "String.fromCharCode(c%a+161)}";
                    inline += ".toString(a)";
                    break;
                default:
                    encode = "function(c){return c}";
                    break;
            }

            // decode: code snippet to speed up decoding
            string decode = "";
            if (fastDecode)
            {
                decode = "if(!''.replace(/^/,String)){while(c--)d[e(c)]=k[c]||e(c);k=[function(e){return d[e]}];e=function(){return'\\\\w+'};c=1;}";
                if (Encoding == PackerEncoding.HighAscii)
                    decode = decode.Replace("\\\\w", "[\\xa1-\\xff]");
                else if (Encoding == PackerEncoding.Numeric)
                    decode = decode.Replace("e(c)", inline);
                if (count == 0)
                    decode = decode.Replace("c=1", "c=0");
            }

            // boot function
            string unpack = "function(p,a,c,k,e,d){while(c--)if(k[c])p=p.replace(new RegExp('\\\\b'+e(c)+'\\\\b','g'),k[c]);return p;}";
            Regex r;
            if (fastDecode)
            {
                //insert the decoder
                r = new Regex("\\{");
                unpack = r.Replace(unpack, "{" + decode + ";", 1);
            }

            if (Encoding == PackerEncoding.HighAscii)
            {
                // get rid of the word-boundries for regexp matches
                r = new Regex("'\\\\\\\\b'\\s*\\+|\\+\\s*'\\\\\\\\b'");
                unpack = r.Replace(unpack, "");
            }
            if (Encoding == PackerEncoding.HighAscii || ascii > (int)PackerEncoding.Normal || fastDecode)
            {
                // insert the encode function
                r = new Regex("\\{");
                unpack = r.Replace(unpack, "{e=" + encode + ";", 1);
            }
            else
            {
                r = new Regex("e\\(c\\)");
                unpack = r.Replace(unpack, inline);
            }
            // no need to pack the boot function since i've already done it
            string _params = "" + packed + "," + ascii + "," + count + "," + keywordsout;
            if (fastDecode)
            {
                //insert placeholders for the decoder
                _params += ",0,{}";
            }
            // the whole thing
            return "eval(" + unpack + "(" + _params + "))\n";
        }

        private string escape(string input)
        {
            Regex r = new Regex("([\\\\'])");
            return r.Replace(input, "\\$1");
        }

        private EncodeMethod getEncoder(PackerEncoding encoding)
        {
            switch (encoding)
            {
                case PackerEncoding.Mid:
                    return new EncodeMethod(encode36);
                case PackerEncoding.Normal:
                    return new EncodeMethod(encode62);
                case PackerEncoding.HighAscii:
                    return new EncodeMethod(encode95);
                default:
                    return new EncodeMethod(encode10);
            }
        }

        private string encode10(int code)
        {
            return code.ToString();
        }

        //lookups seemed like the easiest way to do this since 
        // I don't know of an equivalent to .toString(36)
        private static string lookup36 = "0123456789abcdefghijklmnopqrstuvwxyz";

        private string encode36(int code)
        {
            string encoded = "";
            int i = 0;
            do
            {
                int digit = (code / (int)Math.Pow(36, i)) % 36;
                encoded = lookup36[digit] + encoded;
                code -= digit * (int)Math.Pow(36, i++);
            } while (code > 0);
            return encoded;
        }

        private static string lookup62 = lookup36 + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string encode62(int code)
        {
            string encoded = "";
            int i = 0;
            do
            {
                int digit = (code / (int)Math.Pow(62, i)) % 62;
                encoded = lookup62[digit] + encoded;
                code -= digit * (int)Math.Pow(62, i++);
            } while (code > 0);
            return encoded;
        }

        private static string lookup95 = "、￥ウЖ┆辈炒刀犯购患骄坷谅媚牌侨墒颂臀闲岩釉罩棕仝圮蒉哙徕沅彐玷殛腱眍镳耱篝貊鼬";

        private string encode95(int code)
        {
            string encoded = "";
            int i = 0;
            do
            {
                int digit = (code / (int)Math.Pow(95, i)) % 95;
                encoded = lookup95[digit] + encoded;
                code -= digit * (int)Math.Pow(95, i++);
            } while (code > 0);
            return encoded;
        }

        private string escape95(string input)
        {
            Regex r = new Regex("[\xa1-\xff]");
            return r.Replace(input, new MatchEvaluator(escape95Eval));
        }

        private string escape95Eval(Match match)
        {
            return "\\x" + ((int)match.Value[0]).ToString("x"); //return hexadecimal value
        }

        private string encodeLocalVars(Match match, int offset)
        {
            int length = match.Groups[offset + 2].Length;
            int start = length - Math.Max(length - match.Groups[offset + 3].Length, 0);
            return match.Groups[offset + 1].Value.Substring(start, length) +
                match.Groups[offset + 4].Value;
        }

        private string encodeWithLookup(Match match, int offset)
        {
            return (string)encodingLookup.Encoded[match.Groups[offset].Value];
        }

        private delegate string EncodeMethod(int code);

        private string encodePrivate(int code)
        {
            return "_" + code;
        }

        private WordList analyze(string input, Regex regex, EncodeMethod encodeMethod)
        {
            // analyse
            // retreive all words in the script
            MatchCollection all = regex.Matches(input);
            WordList rtrn;
            rtrn.Sorted = new StringCollection(); // list of words sorted by frequency
            rtrn.Protected = new HybridDictionary(); // dictionary of word->encoding
            rtrn.Encoded = new HybridDictionary(); // instances of "protected" words
            if (all.Count > 0)
            {
                StringCollection unsorted = new StringCollection(); // same list, not sorted
                HybridDictionary Protected = new HybridDictionary(); // "protected" words (dictionary of word->"word")
                HybridDictionary values = new HybridDictionary(); // dictionary of charCode->encoding (eg. 256->ff)
                HybridDictionary count = new HybridDictionary(); // word->count
                int i = all.Count, j = 0;
                string word;
                // count the occurrences - used for sorting later
                do
                {
                    word = "$" + all[--i].Value;
                    if (count[word] == null)
                    {
                        count[word] = 0;
                        unsorted.Add(word);
                        // make a dictionary of all of the protected words in this script
                        //  these are words that might be mistaken for encoding
                        Protected["$" + (values[j] = encodeMethod(j))] = j++;
                    }
                    // increment the word counter
                    count[word] = (int)count[word] + 1;
                } while (i > 0);
                /* prepare to sort the word list, first we must protect
                    words that are also used as codes. we assign them a code
                    equivalent to the word itself.
                   e.g. if "do" falls within our encoding range
                        then we store keywords["do"] = "do";
                   this avoids problems when decoding */
                i = unsorted.Count;
                string[] sortedarr = new string[unsorted.Count];
                do
                {
                    word = unsorted[--i];
                    if (Protected[word] != null)
                    {
                        sortedarr[(int)Protected[word]] = word.Substring(1);
                        rtrn.Protected[(int)Protected[word]] = true;
                        count[word] = 0;
                    }
                } while (i > 0);
                string[] unsortedarr = new string[unsorted.Count];
                unsorted.CopyTo(unsortedarr, 0);
                // sort the words by frequency
                Array.Sort(unsortedarr, (IComparer)new CountComparer(count));
                j = 0;
                /*because there are "protected" words in the list
                  we must add the sorted words around them */
                do
                {
                    if (sortedarr[i] == null)
                        sortedarr[i] = unsortedarr[j++].Substring(1);
                    rtrn.Encoded[sortedarr[i]] = values[i];
                } while (++i < unsortedarr.Length);
                rtrn.Sorted.AddRange(sortedarr);
            }
            return rtrn;
        }

        private struct WordList
        {
            public StringCollection Sorted;
            public HybridDictionary Encoded;
            public HybridDictionary Protected;
        }

        private class CountComparer : IComparer
        {
            HybridDictionary count;

            public CountComparer(HybridDictionary count)
            {
                this.count = count;
            }

            #region IComparer Members

            public int Compare(object x, object y)
            {
                return (int)count[y] - (int)count[x];
            }

            #endregion
        }
    }
    /// <summary>
    /// a multi-pattern parser
    /// </summary>
    internal class ParseMaster
    {
        // used to determine nesting levels
        Regex GROUPS = new Regex("\\("),
            SUB_REPLACE = new Regex("\\$"),
            INDEXED = new Regex("^\\$\\d+$"),
            ESCAPE = new Regex("\\\\."),
            QUOTE = new Regex("'"),
            DELETED = new Regex("\\x01[^\\x01]*\\x01");

        /// <summary>
        /// Delegate to call when a regular expression is found.
        /// Use match.Groups[offset + &lt;group number&gt;].Value to get
        /// the correct subexpression
        /// </summary>
        public delegate string MatchGroupEvaluator(Match match, int offset);

        private string DELETE(Match match, int offset)
        {
            return "\x01" + match.Groups[offset].Value + "\x01";
        }

        private bool ignoreCase = false;
        private char escapeChar = '\0';

        /// <summary>
        /// Ignore Case?
        /// </summary>
        public bool IgnoreCase
        {
            get { return ignoreCase; }
            set { ignoreCase = value; }
        }

        /// <summary>
        /// Escape Character to use
        /// </summary>
        public char EscapeChar
        {
            get { return escapeChar; }
            set { escapeChar = value; }
        }

        /// <summary>
        /// Add an expression to be deleted
        /// </summary>
        /// <param name="expression">Regular Expression String</param>
        public void Add(string expression)
        {
            Add(expression, string.Empty);
        }

        /// <summary>
        /// Add an expression to be replaced with the replacement string
        /// </summary>
        /// <param name="expression">Regular Expression String</param>
        /// <param name="replacement">Replacement String. Use $1, $2, etc. for groups</param>
        public void Add(string expression, string replacement)
        {
            if (replacement == string.Empty)
                add(expression, new MatchGroupEvaluator(DELETE));

            add(expression, replacement);
        }

        /// <summary>
        /// Add an expression to be replaced using a callback function
        /// </summary>
        /// <param name="expression">Regular expression string</param>
        /// <param name="replacement">Callback function</param>
        public void Add(string expression, MatchGroupEvaluator replacement)
        {
            add(expression, replacement);
        }

        /// <summary>
        /// Executes the parser
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns>parsed string</returns>
        public string Exec(string input)
        {
            return DELETED.Replace(unescape(getPatterns().Replace(escape(input), new MatchEvaluator(replacement))), string.Empty);
            //long way for debugging
            /*input = escape(input);
            Regex patterns = getPatterns();
            input = patterns.Replace(input, new MatchEvaluator(replacement));
            input = DELETED.Replace(input, string.Empty);
            return input;*/
        }

        ArrayList patterns = new ArrayList();
        private void add(string expression, object replacement)
        {
            Pattern pattern = new Pattern();
            pattern.expression = expression;
            pattern.replacement = replacement;
            //count the number of sub-expressions
            // - add 1 because each group is itself a sub-expression
            pattern.length = GROUPS.Matches(internalEscape(expression)).Count + 1;

            //does the pattern deal with sup-expressions?
            if (replacement is string && SUB_REPLACE.IsMatch((string)replacement))
            {
                string sreplacement = (string)replacement;
                // a simple lookup (e.g. $2)
                if (INDEXED.IsMatch(sreplacement))
                {
                    pattern.replacement = int.Parse(sreplacement.Substring(1)) - 1;
                }
            }

            patterns.Add(pattern);
        }

        /// <summary>
        /// builds the patterns into a single regular expression
        /// </summary>
        /// <returns></returns>
        private Regex getPatterns()
        {
            StringBuilder rtrn = new StringBuilder(string.Empty);
            foreach (object pattern in patterns)
            {
                rtrn.Append(((Pattern)pattern).ToString() + "|");
            }
            rtrn.Remove(rtrn.Length - 1, 1);
            return new Regex(rtrn.ToString(), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// Global replacement function. Called once for each match found
        /// </summary>
        /// <param name="match">Match found</param>
        private string replacement(Match match)
        {
            int i = 1, j = 0;
            Pattern pattern;
            //loop through the patterns
            while (!((pattern = (Pattern)patterns[j++]) == null))
            {
                //do we have a result?
                if (match.Groups[i].Value != string.Empty)
                {
                    object replacement = pattern.replacement;
                    if (replacement is MatchGroupEvaluator)
                    {
                        return ((MatchGroupEvaluator)replacement)(match, i);
                    }
                    else if (replacement is int)
                    {
                        return match.Groups[(int)replacement + i].Value;
                    }
                    else
                    {
                        //string, send to interpreter
                        return replacementString(match, i, (string)replacement, pattern.length);
                    }
                }
                else //skip over references to sub-expressions
                    i += pattern.length;
            }
            return match.Value; //should never be hit, but you never know
        }

        /// <summary>
        /// Replacement function for complicated lookups (e.g. Hello $3 $2)
        /// </summary>
        private string replacementString(Match match, int offset, string replacement, int length)
        {
            while (length > 0)
            {
                replacement = replacement.Replace("$" + length--, match.Groups[offset + length].Value);
            }
            return replacement;
        }

        private StringCollection escaped = new StringCollection();

        //encode escaped characters
        private string escape(string str)
        {
            if (escapeChar == '\0')
                return str;
            Regex escaping = new Regex("\\\\(.)");
            return escaping.Replace(str, new MatchEvaluator(escapeMatch));
        }

        private string escapeMatch(Match match)
        {
            escaped.Add(match.Groups[1].Value);
            return "\\";
        }

        //decode escaped characters
        private int unescapeIndex = 0;
        private string unescape(string str)
        {
            if (escapeChar == '\0')
                return str;
            Regex unescaping = new Regex("\\" + escapeChar);
            return unescaping.Replace(str, new MatchEvaluator(unescapeMatch));
        }

        private string unescapeMatch(Match match)
        {
            return "\\" + escaped[unescapeIndex++];
        }

        private string internalEscape(string str)
        {
            return ESCAPE.Replace(str, "");
        }

        //subclass for each pattern
        private class Pattern
        {
            public string expression;
            public object replacement;
            public int length;

            public override string ToString()
            {
                return "(" + expression + ")";
            }
        }
    }
}
