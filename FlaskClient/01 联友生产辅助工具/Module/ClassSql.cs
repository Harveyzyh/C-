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
        /// <param name="connStr">数据库连接字</param>
        public bool SQLlinkTest(string connStr) //数据库连接测试
        {
            bool CanConnectDB = false;
            using (SqlConnection testConn = new SqlConnection(connStr))
            {
                try
                {
                    testConn.Open();
                    SqlCommand testCmd = testConn.CreateCommand();
                    testCmd.CommandTimeout = 3;
                    CanConnectDB = true;
                    testConn.Close();
                    testConn.Dispose();
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
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="slqStr">本数据库中表示DeviceID</param>
        public int SQLexcute(string connStr, string slqStr, int workCount = 0)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(slqStr, conn);
                    cmd.CommandTimeout = 1800;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    return 0;
                }
                catch (Exception es)
                {
                    workCount++;
                    if (workCount < 5)
                    {
                        return SQLexcute(connStr, slqStr, workCount);
                    }
                    else
                    {
                        MessageBox.Show("SQL Commit 出错了！\r\n" + "\r\n\r\n\r\n" + es.ToString(), "提示", MessageBoxButtons.OK);
                        return 1;
                    }
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }


        /// <summary>
        /// 数据库-查
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="slqStr">本数据库中表示DeviceID</param>
        public DataTable SQLselect(string connStr, string slqStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    DataTable dttmp = new DataTable();
                    SqlCommand cmd = new SqlCommand(slqStr, conn);
                    cmd.CommandTimeout = 6000;

                    //SqlDataAdapter sdatmp = new SqlDataAdapter(slqStr, conn);
                    SqlDataAdapter sdatmp = new SqlDataAdapter();
                    sdatmp.SelectCommand = cmd;
                    sdatmp.Fill(dttmp);
                    sdatmp.Dispose();
                    if (dttmp.Rows.Count <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        return dttmp;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("执行失败(" + ex.Message + ")，请退出后重新进入！", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return null;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }


        /// <summary>
        /// 数据库-查-是否存在
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        /// <param name="slqStr">查询语句</param>
        public bool SQLexist(string connStr, string slqStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    DataTable dttmp = new DataTable();
                    SqlDataAdapter sdatmp = new SqlDataAdapter(slqStr, conn);
                    sdatmp.Fill(dttmp);
                    sdatmp.Dispose();
                    if (dttmp.Rows.Count <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("执行失败(" + ex.Message + ")，请退出后重新进入！", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return false;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public string SQLTime(string connStr, int lenth = 0)
        {
            string slqStr = "";
            if (lenth == 0)
            {
                slqStr = @"SELECT REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(varchar(25), GETDATE(), 25), '-', ''), ' ', ''), ':', ''), '.', '') ";
            }
            else
            {
                slqStr = @"SELECT LEFT( REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(varchar(25), GETDATE(), 25), '-', ''), ' ', ''), ':', ''), '.', ''), {0}) ";
                slqStr = string.Format(slqStr, lenth);
            }
            return SQLselect(connStr, slqStr).Rows[0][0].ToString();
        }
    }
}
