using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FAN.Helper
{
    /// <summary>
    /// XML实用类
    /// </summary>
    public class XmlHelper
    {
        #region 得到指定键名和值的XML字符串

        /// <summary>
        /// 得到指定键名和值的XML字符串
        /// </summary>
        /// <param name="Name">键名</param>
        /// <param name="Value">值</param>
        /// <returns></returns>
        public static string GetNameValue(string Name, object Value)
        {
            if (Value != null)
                return "<" + Name + ">" + System.Web.HttpUtility.HtmlEncode(Value.ToString()) + "</" + Name + ">\r\n";
            else
                return "<" + Name + "/>\r\n";
        }

        #endregion

        #region 将XML反序列成对象
        /// <summary>
        /// XML反序列成对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static object LoadSerializedObject(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        #endregion

        #region 将对象序列化成XML
        /// <summary>
        /// 将对象序列化成XML
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="filename">指定文件</param>
        public static void SerializeObject(object o, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(o.GetType());

            // Create an XmlSerializerNamespaces object.
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            // Add two namespaces with prefixes.
            ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Create an XmlTextWriter using a FileStream.
            Stream fs = new FileStream(filename, FileMode.Create);
            XmlWriter writer = new XmlTextWriter(fs, new UTF8Encoding());

            // Serialize using the XmlTextWriter.
            serializer.Serialize(writer, o, ns);
            writer.Close();
            fs.Close();
        }
        #endregion

        #region 安全的XPATH
        public static string SafeXpathString(string input)
        {
            bool sngl_qt = input.IndexOf("'") > -1;
            bool reg_qt = input.IndexOf("\"") > -1;
            string output = "";

            if (sngl_qt && reg_qt)
            {
                // Build a concat function to 
                // make our string work.
                string[] parts = input.Split('\'');
                output = "concat('";
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i > 0) output += ",\"'\",'";
                    output += parts[i] + "'";
                }
                output += ")";
            }
            else if (sngl_qt && !reg_qt)
            {
                //Wrap just single quotes with regualar quotes
                output = "\"" + input + "\"";
            }
            else
            {
                //Wrap everything else in single quotes
                output = "'" + input + "'";
            }

            return output;
        }

        //http://www.holmok.com/quotes_xpath.aspx 

        
        #endregion
    }
}
