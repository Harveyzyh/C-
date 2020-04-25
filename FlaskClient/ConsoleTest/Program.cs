using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using HarveyZ;

namespace ConsoleTest
{
    public class Program
    {
        protected void LoginTest()
        {
            string url = "https://comfort-oa.com:8099/api";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("username", "aaaaaa");
            parameters.Add("rsapwd", "111111");
            HttpWebResponse response = PostHttps(url, parameters, encoding);
            //打印返回值
            Stream stream = response.GetResponseStream();   //获取响应的字符串流
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html
            Console.WriteLine(html);
        }

        public static void Main()
        {
            Program pro = new Program();
            pro.LoginTest();
        }

        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }

        public static HttpWebResponse PostHttps(string url, Dictionary<string, string> parameters, Encoding charset)
        {
            HttpWebRequest request = null;
            CookieContainer cookie = new CookieContainer();
            //HTTPSQ请求
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.CookieContainer = cookie;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = DefaultUserAgent;
            request.KeepAlive = true;
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.9";
            //request.Headers["Cookie"] = "username=aaaaaa; Language=zh_CN";
            //如果需要POST数据   
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                buffer.Append(Json.Dict2Json(parameters));
                byte[] data = charset.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            return (HttpWebResponse)request.GetResponse();
        }
    }
}
