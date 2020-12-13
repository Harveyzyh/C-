using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HarveyZ
{
    public class Mssql
    {
        /// <summary>
        /// 数据库连接测试，超时时间为3秒
        /// </summary>
        /// <param name="strConnection">数据库连接字</param>
        public bool SQLlinkTest(string strConnection) //数据库连接测试
        {
            bool CanConnectDB = false;
            using (SqlConnection testConnection = new SqlConnection(strConnection))
            {
                try
                {
                    testConnection.Open();
                    SqlCommand testCmd = testConnection.CreateCommand();
                    testCmd.CommandTimeout = 3;
                    CanConnectDB = true;
                    testConnection.Close();
                    testConnection.Dispose();
                }
                catch { }
                if (CanConnectDB)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 数据库-增，改，删
        /// </summary>
        /// <param name="SQLstr">数据库连接字符串</param>
        /// <param name="SqlStr">本数据库中表示DeviceID</param>
        public int SQLexcute(string ConnStr, string SqlStr, int workCount = 0)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SqlStr, conn);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                return 0;
            }

        }


        /// <summary>
        /// 数据库-查
        /// </summary>
        /// <param name="SQLstr">数据库连接字符串</param>
        /// <param name="CMDstr">本数据库中表示DeviceID</param>
        public DataTable SQLselect(string SQLstr, string CMDstr)
        {
            using (SqlConnection conn = new SqlConnection(SQLstr))
            {
                DataTable dttmp = new DataTable();
                SqlDataAdapter sdatmp = new SqlDataAdapter(CMDstr, conn);
                sdatmp.Fill(dttmp);
                sdatmp.Dispose();
                conn.Close();
                conn.Dispose();
                if (dttmp.Rows.Count <= 0)
                {
                    return null;
                }
                else
                {
                    return dttmp;
                }
            }
        }


        /// <summary>
        /// 数据库-查-是否存在
        /// </summary>
        /// <param name="SQLstr">数据库连接字符串</param>
        /// <param name="CMDstr">查询语句</param>
        public bool SQLexist(string SQLstr, string CMDstr)
        {
            using (SqlConnection conn = new SqlConnection(SQLstr))
            {
                DataTable dttmp = new DataTable();
                SqlDataAdapter sdatmp = new SqlDataAdapter(CMDstr, conn);
                sdatmp.Fill(dttmp);
                sdatmp.Dispose();
                conn.Close();
                conn.Dispose();
                if (dttmp.Rows.Count <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string SQLTime(string SQLstr, int lenth = 0)
        {
            string sqlstr = "";
            if (lenth == 0)
            {
                sqlstr = @"SELECT REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(varchar(25), GETDATE(), 25), '-', ''), ' ', ''), ':', ''), '.', '') ";
            }
            else
            {
                sqlstr = @"SELECT LEFT( REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(varchar(25), GETDATE(), 25), '-', ''), ' ', ''), ':', ''), '.', ''), {0}) ";
                sqlstr = string.Format(sqlstr, lenth);
            }
            return SQLselect(SQLstr, sqlstr).Rows[0][0].ToString();
        }
    }
}
