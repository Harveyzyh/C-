using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{
    class Normal
    {
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
        
        public static string ConvertDate(string datestr) //显示日期转换 加 -
        {
            string returnstr = "";
            int strlenth = datestr.Length;
            if (strlenth == 14)
            {
                returnstr = datestr.Substring(0, 4) + "-" + datestr.Substring(4, 2) + "-" + datestr.Substring(6, 2) + " "
                    + datestr.Substring(8, 2) + ":" + datestr.Substring(10, 2) + ":" + datestr.Substring(12, 2);
            }
            else if (strlenth == 8)
            {
                returnstr = datestr.Substring(0, 4) + "-" + datestr.Substring(4, 2) + "-" + datestr.Substring(6, 2);
            }
            else if ((strlenth > 8) & (strlenth < 14))
            {
                returnstr = datestr.Substring(0, 4) + "-" + datestr.Substring(4, 2) + "-" + datestr.Substring(6, 2);
            }
            else
            {
                returnstr = datestr;
            }

            return returnstr;
        }

        public static string ConverSqlStr(string SqlStr, string Type = "str") //转SQL前部分字段转换
        {
            string returnstr = "";

            if (Type == "str")
            {
                returnstr = SqlStr.Replace("^", "[^]").Replace("%", "[%]").Replace("[", "[[]").Replace("'", "‘");
            }

            return returnstr;
        }
    }

    class VersionManeger
    {
        private static Mssql mssql = new Mssql();
        private static string conn = null;

        public VersionManeger(string _conn = "")
        {
            conn = _conn;
        }

        public void SetProgVersion(string ProgName, string Version)
        {
            string sqlstr = @"UPDATE WG_APP_INF SET Version = '{1}', Valid = 1 WHERE ProgName = '{0}' ";
            mssql.SQLexcute(conn, string.Format(sqlstr, ProgName, Version));
        }

        public bool GetNewVersion(string ProgName, string NowVersion, out string Msg)
        {
            bool result = false;
            Msg = null;
            if (Normal.GetSubstringCount(NowVersion, ".") == 3)
            {
                string sqlstr = @"SELECT Version, Valid FROM WG_APP_INF WHERE ProgName = '{0}'";
                DataTable dt = mssql.SQLselect(conn, string.Format(sqlstr, ProgName));
                if (dt != null)
                {
                    bool valid = dt.Rows[0][1].Equals(true) ? true : false;
                    if (valid)
                    {
                        var NewVersionList = dt.Rows[0][0].ToString().Split('.');
                        var NowVersionList = NowVersion.Split('.');
                        for (int index = 0; index < 4; index++)
                        {
                            if (int.Parse(NewVersionList[index]) > int.Parse(NowVersionList[index]))
                            {
                                result = true;
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
                        Msg = "程序： " + ProgName + " 已停用，如有疑问，请联系资讯部，谢谢！";
                    }
                }
                else
                {
                    Msg = "新版本检测：传入程序名称错误";
                }
            }
            else
            {
                Msg = "新版本检测：传入程序版本错误";
            }
            return result;
        }
    }


}
