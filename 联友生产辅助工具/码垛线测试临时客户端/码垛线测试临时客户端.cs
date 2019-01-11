using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 码垛线测试临时客户端
{
    public partial class 码垛线测试临时客户端 : Form
    {
        #region 数据库连接字
        public const string strConnection_ROBOT = "Server=192.168.0.198;initial catalog=ROBOT_TEST;user id=sa;password=COMfort123456;Connect Timeout=5";
        #endregion

        #region 局部变量
        private Mssql mssql = new Mssql();
        private bool Show_Type_Flag = true;
        private DataTable Main_dt = null;
        #endregion


        public 码垛线测试临时客户端()
        {
            InitializeComponent();
            if (!Enable())
            {
                Environment.Exit(0);
            }
            Form_MainResized_Work();
            Init();
        }

        #region 窗口大小变化设置
        private void Form_MainResized(object sender, EventArgs e)
        {
            Form_MainResized_Work();
        }

        private void Form_MainResized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width - 20;
            FormHeight = Height - 40;
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            dgv_Main.Size = new Size(FormWidth, FormHeight-panel_Title.Height-7);
        }
        #endregion

        private bool Enable()
        {
            string sqlstr = " SELECT CONTENTS FROM CONFIG..CONFIGURE WHERE CONFIG_NAME = 'ROBOT_TEST' ";
            Main_dt = mssql.SQLselect(strConnection_ROBOT, sqlstr);
            if(Main_dt != null)
            {
                if (Main_dt.Rows[0][0].ToString() == "Y")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        private void Init()
        {
            Show();
        }

        private void Show()
        {
            if(dgv_Main.DataSource != null)
            {
                dgv_Main.DataSource = null;
            }
            string sqlstr = "SELECT SC001 订单号, SC003 上线日期, SC010 名称, SC013 生产数量, ISNULL(COUNT001, 0) 已过机数量, "
                          + "ISNULL(PD002, NULL) 栈板号, (CASE SC033 WHEN 0 THEN 'N' WHEN 1 THEN 'Y' END) 完成码, SC036 纸箱尺寸码, SC037 订单类别码, PO_Class+'-' + PO_Type 订单类别名称, SC039 测试码 "
                          + "FROM SCHEDULE AS SC "
                          + "INNER JOIN SplitTypeCode ON SC037 = TypeCode "
                          + "LEFT JOIN "
                          + "(SELECT PdData.SC001 PD001, COUNT(PdData.SC001) COUNT001, PdData.MD_No PD002 FROM PdData WHERE Pd_Sta = 'OK' "
                          + "GROUP BY PdData.SC001, PdData.MD_No) AS PD ON PD001 = SC.SC001 "
                          + "WHERE 1 = 1 "
                          + "AND SC039 = 'Y' "
                          + "ORDER BY SC003, SC001, PD002, PD001";
            Main_dt = mssql.SQLselect(strConnection_ROBOT, sqlstr);
            if(Main_dt != null)
            {
                button4.Enabled = true;
                dgv_Main.DataSource = Main_dt;
                dgv_Main.Columns[0].Width = 130;
                dgv_Main.Columns[0].ReadOnly = true;
                dgv_Main.Columns[1].ReadOnly = true;
                dgv_Main.Columns[4].ReadOnly = true;
                dgv_Main.Columns[5].ReadOnly = true;
                dgv_Main.Columns[6].ReadOnly = true;
                dgv_Main.Columns[9].ReadOnly = true;
                dgv_Main.Columns[10].ReadOnly = true;
            }
            else
            {
                button4.Enabled = false;
                MessageBox.Show("没有查询到数据", "错误");
            }
        }

        private void Save()
        {
            string sqlstr = "UPDATE SCHEDULE SET SC013 = '{1}', SC036 = '{2}', SC037 = '{3}', SC010 = '{4}' WHERE SC001 = '{0}'";
            string sql_tmp = null;
            int Count = Main_dt.Rows.Count;
            int Index = 0;
            for(Index = 0; Index < Count; Index++)
            {
                sql_tmp = string.Format(sqlstr, Main_dt.Rows[Index][0], Main_dt.Rows[Index][3], Main_dt.Rows[Index][7], Main_dt.Rows[Index][8], Main_dt.Rows[Index][2]);
                mssql.SQLexcute(strConnection_ROBOT, sql_tmp);
            }
            MessageBox.Show("已保存！", "提示");
            Show();
        }

        private void Show_Type_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlstr1 = "DELETE FROM PdData WHERE SC001 IN(SELECT SC001 FROM SCHEDULE WHERE SC039 = 'Y')";
            string sqlstr2 = "UPDATE SCHEDULE SET SC033 = 0, SC003 = CONVERT(VARCHAR(20), GETDATE(), 112) WHERE SC039 = 'Y'";
            mssql.SQLexcute(strConnection_ROBOT, sqlstr1);
            mssql.SQLexcute(strConnection_ROBOT, sqlstr2);
            MessageBox.Show("订单信息已重置", "成功");
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Save();
        }
    }

    public class Mssql
    {
        /// <summary>
        /// 数据库连接测试
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
        /// <param name="CMDstr">本数据库中表示DeviceID</param>
        public int SQLexcute(string SQLstr, string CMDstr)
        {
            using (SqlConnection conn = new SqlConnection(SQLstr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(CMDstr, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    return 0;
                }
                catch (Exception es)
                {
                    MessageBox.Show("SQL Commit 出错了！\r\n" + SQLstr + "\r\n\r\n\r\n" + es.ToString(), "提示", MessageBoxButtons.OK);
                    return 1;
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
        /// <param name="SQLstr">数据库连接字符串</param>
        /// <param name="CMDstr">本数据库中表示DeviceID</param>
        public DataTable SQLselect(string SQLstr, string CMDstr)
        {
            using (SqlConnection conn = new SqlConnection(SQLstr))
            {
                try
                {
                    //conn.Open();
                    //SqlCommand cmd = new SqlCommand(CMDstr, conn);
                    DataTable dttmp = new DataTable();
                    SqlDataAdapter sdatmp = new SqlDataAdapter(CMDstr, conn);
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
                    //MessageBox.Show("执行失败(" + ex.Message + ")，请退出后重新进入！", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return null;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
    }
}
