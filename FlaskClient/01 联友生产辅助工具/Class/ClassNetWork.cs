using Common.Helper.Crypto;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace HarveyZ
{
    #region WebPost
    public class WebClientEx : WebClient
    {
        private int timeout = 60000;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = timeout;
            return request;
        }
    }

    public class WebNet
    {
        public static Dictionary<string, string> WebPost(string url, Dictionary<string, string> dict, int timeout = 60000)
        {
            try
            {
                var client = new WebClientEx();
                client.Timeout = timeout;
                string json = Json.Dict2Json(dict);
                string response = client.UploadString(url, json);
                dict = Json.Json2Dict(response);
                return dict;
            }
            catch
            {
                return null;
            }
        }

        public static string WebPost(string url, string strin, int timeout = 60000)
        {
            string strOut = null;
            try
            {
                var client = new WebClientEx();
                client.Timeout = timeout;
                strOut = client.UploadString(url, strin);
            }
            catch (Exception es)
            {
                MessageBox.Show(es.ToString());
                strOut = null;
            }
            return strOut;
        }
    }
    #endregion

    #region HTTP Post发送
    public class HttpPost
    {
        private static AesCrypto aes16 = new AesCrypto();//字符串加密
        public static Dictionary<string, string> HttpPost_Dict(string webURL, Dictionary<string, string> dict = null, int timeout = 60)
        {
            Dictionary<string, string> dictBack = new Dictionary<string, string> { };

            string jsonin = Json.Dict2Json(dict);
            string jsonout = WebNet.WebPost(webURL, aes16.Encrypt(jsonin), timeout * 1000);
            jsonout = aes16.Decrypt(jsonout);
            dictBack = Json.Json2Dict(jsonout);
            return dictBack;
        }
        public static string HttpPost_Json(string webURL, Dictionary<string, string> dict = null, int timeout = 60)
        {
            string jsonin = Json.Dict2Json(dict);
            string jsonout = WebNet.WebPost(webURL, aes16.Encrypt(jsonin), timeout * 1000);
            return jsonout;
        }
    }
    #endregion
}