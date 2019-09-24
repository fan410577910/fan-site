using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FAN.Helper
{
    public static class CsvHelper
    {
        /// <summary>
        /// 获取CSV导入的数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName">文件名称(.csv不用加)</param>
        /// <returns></returns>
        public static List<T> GetCsvList<T>(string filePath, string fileName)
        {
            List<T> list = new List<T>();
            try
            {
                DataTable data = GetCsvTable(filePath, fileName);
                list = data.ToList<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 获取CSV导入的数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName">文件名称(.csv不用加)</param>
        /// <returns></returns>
        public static List<T> GetExcelList<T>(string filePath, string fileName)
        {
            List<T> list = new List<T>();
            try
            {
                DataTable data = GetExcelTable(filePath, fileName);
                list = data.ToList<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 获取CSV导入的数据
        /// </summary>
        /// <param name="filePath">CSV全路径需要.csv</param>
        /// <returns></returns>
        public static List<T> GetCsvList<T>(string filePath)
        {
            List<T> list = new List<T>();
            try
            {
                DataTable data = OpenCsv(filePath);
                //DataTable data = getCsvData(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
                list = data.ToList<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// CSV文件导入Table
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetExcelTable(string filePath, string fileName)
        {
            DataTable data = new DataTable();
            string connString = @"Driver={Microsoft Excel Driver (*.xls)};DBQ=" + filePath + ";Extensions=xls;";
            try
            {
                using (OdbcConnection odbcConn = new OdbcConnection(connString))
                {
                    odbcConn.Open();
                    OdbcCommand oleComm = new OdbcCommand
                    {
                        Connection = odbcConn,
                        CommandText = "select * from [" + fileName + "$]"
                    };
                    OdbcDataAdapter adapter = new OdbcDataAdapter(oleComm);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds, fileName);

                    data = ds.Tables[0];
                    odbcConn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        /// <summary>
        /// CSV文件导入Table
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetCsvTable(string filePath, string fileName)
        {
            DataTable data = new DataTable();
            string connString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + filePath + ";Extensions=asc,csv,tab,txt;";
            try
            {
                using (OdbcConnection odbcConn = new OdbcConnection(connString))
                {
                    odbcConn.Open();

                    OdbcCommand oleComm = new OdbcCommand
                    {
                        Connection = odbcConn,
                        CommandText = "select * from [" + fileName + "#csv]"
                    };
                    OdbcDataAdapter adapter = new OdbcDataAdapter(oleComm);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds, fileName);
                    data = ds.Tables[0];
                    odbcConn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        #region 读取Csv文件
        public static DataTable getCsvData(string pCsvpath, string pCsvname)
        {
            try
            {
                pCsvname = pCsvname.Replace(".", "#");
                DataSet dsCsvData = new DataSet();

                OleDbConnection OleCon = new OleDbConnection();
                OleDbCommand OleCmd = new OleDbCommand();
                OleDbDataAdapter OleDa = new OleDbDataAdapter();

                OleCon.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pCsvpath + ";Extended Properties='Text;FMT=Delimited(,);HDR=YES;IMEX=1';";
                OleCon.Open();
                DataTable dts1 = OleCon.GetSchema("Tables");
                DataTable dts2 = OleCon.GetSchema("Columns");
                OleCmd.Connection = OleCon;
                OleCmd.CommandText = "select * from [" + pCsvname + "] where 1=1";
                OleDa.SelectCommand = OleCmd;
                OleDa.Fill(dsCsvData, "Csv");
                OleCon.Close();

                return dsCsvData.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="filePath">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCsv(string filePath)
        {
            Encoding encoding = Encoding.UTF8;
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(fs, encoding);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        if (i == 36)
                        {

                        }
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {

                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (j == 36)
                        {

                        }
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        /// <summary>
        /// DataTable To List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static List<T> ToList<T>(this DataTable dt)
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            List<PropertyInfo> plist = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i].ToString(), null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }
    }
}
