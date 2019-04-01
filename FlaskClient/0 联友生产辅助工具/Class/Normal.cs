using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HarveyZ
{
    class Normal
    {
        private static Mssql mssql = new Mssql();
        private static string Conn_WG_DB = Global_Const.strConnection_WG_DB;

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <param name="Mode">获取模式：Sort 短时间；Long 详细到时分秒</param>
        /// <returns>出现的次数</returns>
        public static string GetDbSysTime(string Mode = "Sort")
        {
            string sqlstrSort = @" SELECT CONVERT(VARCHAR(20), GETDATE(), 112) ";
            string sqlstrLong = @"SELECT (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', ''))";
            string returnStr = null;
            if(Mode == "Sort")
            {
                DataTable dt = mssql.SQLselect(Conn_WG_DB, sqlstrSort);
                if (dt != null)
                {
                    returnStr =  dt.Rows[0][0].ToString();
                }
            }
            else if(Mode == "Long")
            {
                DataTable dt = mssql.SQLselect(Conn_WG_DB, sqlstrLong);
                if (dt != null)
                {
                    returnStr =  dt.Rows[0][0].ToString();
                }
            }
            return returnStr;
        }
        
        /// <summary>
        /// 计算字符串中子串出现的次数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="substring">子串</param>
        /// <returns>出现的次数</returns>
        public static int GetSubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }

            return 0;
        }
    }

    class VersionManeger
    {
        private static Mssql mssql = new Mssql();
        private static string Conn_WG_DB = Global_Const.strConnection_WG_DB;

        public static void SetProgVersion(string ProgName, string Version)
        {
            string sqlstr = @"UPDATE WG_APP_INF SET Version = '{1}', Valid = 1 WHERE ProgName = '{0}' ";
            mssql.SQLexcute(Conn_WG_DB, string.Format(sqlstr, ProgName, Version));
        }
        public static bool GetNewVersion(string ProgName, string NowVersion, out string Msg, out string Url)
        {
            bool result = false;
            Msg = null;
            Url = null;
            if (Normal.GetSubstringCount(NowVersion, ".") == 3)
            {
                string sqlstr = @"SELECT Version FROM WG_APP_INF WHERE ProgName = '{0}' ";
                DataTable dt = mssql.SQLselect(Conn_WG_DB, string.Format(sqlstr, ProgName));
                if (dt != null)
                {
                    var NewVersionList = dt.Rows[0][0].ToString().Split('.');
                    var NowVersionList = NowVersion.Split('.');
                    for (int index = 0; index < 4; index++)
                    {
                        if (int.Parse(NewVersionList[index]) > int.Parse(NowVersionList[index]))
                        {
                            result = true;
                            //Url = "";
                            break;
                        }
                        if (int.Parse(NewVersionList[index]) < int.Parse(NowVersionList[index]))
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Msg = "传入程序名称错误";
                }
            }
            else
            {
                Msg = "传入程序版本错误";
            }
            return result;
        }
    }

    class UserManager
    {

    }
}
