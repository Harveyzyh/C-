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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = Application.ProductName.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DictJson dictJson = new DictJson();
            dict.Clear();
            dict.Add("UID", "001114");
            dict.Add("Mode", "Complete");
            dict.Add("Parameter", "JH201812281329460001");
            dict.Add("Data", null);
            dict.Add("RowCount", "4");
            string jsonin = dictJson.Dict2Json(dict);
            string jsonin_enc = AesCrypto.Encrypt(jsonin);
            string jsonout = webnet.WebPost("http://192.168.1.60:80/Test/Test3", jsonin_enc);
            jsonout = AesCrypto.Decrypt(jsonout);
            textBox5.Text = jsonout;
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
}
