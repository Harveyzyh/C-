using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Helper.Crypto;
using System.Net;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;

namespace 测试
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        private static WebNet webnet = new WebNet();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string json = "[{\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"\u5185\u9500\", \"\u7c7b\u522b\u7f16\u7801\": \"A\"}, {\"\u6709\u6548\u7801\": false, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"\u83dc\u9e1f\u6761\u7801\", \"\u7c7b\u522b\u7f16\u7801\": \"B\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"\u65e0\u7801\", \"\u7c7b\u522b\u7f16\u7801\": \"C\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"\u4eac\u4e1c\u6761\u7801\", \"\u7c7b\u522b\u7f16\u7801\": \"D\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"POP\u6761\u7801\", \"\u7c7b\u522b\u7f16\u7801\": \"E\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2201\", \"\u7c7b\u522b\u7f16\u7801\": \"F\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2214\", \"\u7c7b\u522b\u7f16\u7801\": \"G\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2204\", \"\u7c7b\u522b\u7f16\u7801\": \"H\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2203\", \"\u7c7b\u522b\u7f16\u7801\": \"I\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2202\", \"\u7c7b\u522b\u7f16\u7801\": \"J\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2216\", \"\u7c7b\u522b\u7f16\u7801\": \"L\"}]";


            DataTableJson table2json = new DataTableJson();
            DataTable dt = DataTableJson.Json2DT(json);
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }

    #region WebPost
    public class WebClientEx : WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = 30000;
            return request;
        }
    }

    public class WebNet
    {
        public string  WebPost(string url, string strin)
        {
            try
            {
                var client = new WebClientEx();
                string strout = client.UploadString(url, strin);
                return strout;
            }
            catch
            {
                return null;
            }
        }
    }
    #endregion

    public class DictJson
    {
        /// <summary>
        /// 将字典类型序列化为json字符串
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="dict">要序列化的字典数据</param>
        /// <returns>json字符串</returns>
        public string Dict2Json(Dictionary<string, string> dict)
        {
            if (dict.Count == 0)
                return "";
            string jsonStr = JsonConvert.SerializeObject(dict, new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            return jsonStr;
        }

        /// <summary>
        /// 将json字符串反序列化为字典类型
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>字典数据</returns>
        public Dictionary<string, string> Json2Dict(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<string, string>();
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
            return dict;

        }
    }

    public class DataTableJson
    {
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public DataTable Json2DT2(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                if (current != "data")
                                    dataTable.Columns.Add(current, dictionary[current].GetType());
                                else
                                {
                                    ArrayList list = dictionary[current] as ArrayList;
                                    foreach (Dictionary<string, object> dic in list)
                                    {
                                        foreach (string key in dic.Keys)
                                        {
                                            dataTable.Columns.Add(key, dic[key].GetType());
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        //Rows
                        string root = "";
                        foreach (string current in dictionary.Keys)
                        {
                            if (current != "data")
                                root = current;
                            else
                            {
                                ArrayList list = dictionary[current] as ArrayList;
                                foreach (Dictionary<string, object> dic in list)
                                {
                                    DataRow dataRow = dataTable.NewRow();
                                    dataRow[root] = dictionary[root];
                                    foreach (string key in dic.Keys)
                                    {
                                        dataRow[key] = dic[key];
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception es)
            {
                MessageBox.Show(es.ToString());
            }
            result = dataTable;
            return result;
        }


        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable Json2DT(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.ToString());
            }
            result = dataTable;
            return result;
        }
    }
}
