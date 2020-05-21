using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace HarveyZ
{

    public class Json
    {
        /// <summary>
        /// 将字典类型序列化为json字符串
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="dict">要序列化的字典数据</param>
        /// <returns>json字符串</returns>
        public static string Dict2JsonNull<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict.Count == 0)
                return "";

            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }

        public static string Dict2Json(Dictionary<string, string> dict)
        {
            if (dict == null || dict.Count == 0)
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
        public static Dictionary<TKey, TValue> Json2DictNull<TKey, TValue>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<TKey, TValue>();

            Dictionary<TKey, TValue> dict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

            return dict;
        }

        public static Dictionary<string, string> Json2Dict(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<string, string>();

            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);

            return dict;
        }

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable Json2DT(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result = null;
            try
            {
                dataTable = JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception ex)
            {
                dataTable = null;
            }
            result = dataTable;
            return result;
        }

        /// <summary>
        /// DataTable数据集合 转换为 Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DT2Json(DataTable dt)
        {
            string jsonStr = null;
            try
            {
                jsonStr = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                jsonStr = null;
            }
            return jsonStr;
        }
    }
}
