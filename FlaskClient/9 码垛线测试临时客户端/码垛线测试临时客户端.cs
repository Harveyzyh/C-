using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 码垛线测试临时客户端
{
    public partial class 码垛线测试临时客户端 : Form
    {
        #region 数据库连接字
        public const string strConnection_ROBOT = "Server=192.168.0.198;initial catalog=ROBOT_TEST;user id=sa;password=COMfort123456;Connect Timeout=5";
        #endregion

        #region 局部变量
        private Mssql mssql = new Mssql();
        private DataTable Main_dt = null;

        private bool TestFlag = false;
        private bool FindFlag = false;
        private bool InitFlag = true;

        private string FindStr = "";
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
            Dgv_Show();
            Btn_Show_Work();
        }

        private void Btn_Show_Work()
        {
            btn_Reflash.Visible = false;
            btn_Find_Type.Visible = false;
            btn_Fine.Visible = false;
            txb_Fine.Visible = false;
            btn_Test_Add.Visible = false;
            btn_Test_Reset.Visible = false;
            btn_Test_Save.Visible = false;
            btn_Show_Type.Visible = false;

            if (InitFlag)
            {
                InitFlag = false;
                btn_Reflash.Visible = true;
                btn_Find_Type.Visible = true;
                btn_Show_Type.Visible = true;
                btn_Show_Type.Text = "显示测试订单";
            }

            if (!TestFlag)//不在测试模式
            {
                if (FindFlag)//进入查找模式
                {
                    btn_Find_Type.Visible = true;
                    btn_Find_Type.Text = "退出查找模式";
                    btn_Fine.Visible = true;
                    txb_Fine.Visible = true;
                    txb_Fine.Text = "";
                }
                else//正常显示模式
                {
                    btn_Reflash.Visible = true;
                    btn_Find_Type.Visible = true;
                    btn_Find_Type.Text = "进入查找模式";
                    btn_Show_Type.Text = "显示测试订单";
                    btn_Show_Type.Visible = true;
                }
            }
            else//显示为测试模式
            {
                btn_Reflash.Visible = true;
                //btn_Test_Add.Visible = true;
                btn_Test_Reset.Visible = true;
                btn_Test_Save.Visible = true;
                btn_Show_Type.Visible = true;
                btn_Show_Type.Text = "显示正常订单";
            }
        }

        private void Dgv_Show()
        {
            if(dgv_Main.DataSource != null)
            {
                dgv_Main.DataSource = null;
            }
            string sqlstr = "SELECT SC.SC001 订单号, SUBSTRING(SC003,1, 4) + '-' + SUBSTRING(SC003, 5, 2) + '-' + SUBSTRING(SC003, 7, 2) 生产日期, "
                            + "SC010 品名, SC013 数量, SC036 纸箱编码, "
                            + "SC037 订单编码, InTypeName + (CASE WHEN Spec IS NULL THEN '' ELSE '-' + Spec END) 订单类别, MD_No 栈板号, ISNULL(PDCOUNT, 0) 已过机数量, "
                            + "(CASE SC033 WHEN '1' THEN 'Y' ELSE 'N'END ) 已完成, PD2.MIXDATE 最早过机时间, PD2.MAXDATE 最迟过机时间 "
                            + "FROM SCHEDULE AS SC "
                            + "LEFT JOIN SplitTypeCode ON TypeCode = SC037 "
                            + "LEFT JOIN ( "
                            + "SELECT SC001, MD_No, COUNT(Pd_Sta) PDCOUNT FROM PdData "
                            + "WHERE Pd_Sta = 'OK' GROUP BY SC001, MD_No "
                            + ") AS PD ON PD.SC001 = SC.SC001 "
                            + "LEFT JOIN ( "
                            + "SELECT SC001, MIN(Pd_date) MIXDATE, MAX(Pd_date) MAXDATE FROM PdData "
                            + "WHERE Pd_Sta = 'OK' GROUP BY SC001 "
                            + ") AS PD2 ON PD2.SC001 = SC.SC001 "
                            + "WHERE 1 = 1 ";

            if (TestFlag)
            {
                sqlstr += "AND SC.SC039 = 'Y' ";
            }
            else
            {
                if (FindFlag)
                {
                    sqlstr += "AND SC.SC001 LIKE '%" + FindStr.Trim() + "%' ";
                    FindStr = "";
                }
                else
                {
                    sqlstr += "AND SC.SC003 = CONVERT(VARCHAR(20), GETDATE(), 112) AND SC.SC039 != 'Y' ";
                }
            }
            
            sqlstr += "ORDER BY KEY_ID ";

            Main_dt = mssql.SQLselect(strConnection_ROBOT, sqlstr);

            if(Main_dt != null)
            {
                dgv_Main.DataSource = Main_dt;
                DgvOpt.SetRowColor(dgv_Main);

                if (TestFlag)
                {
                    btn_Test_Save.Enabled = true;
                    dgv_Main.ReadOnly = false;
                    dgv_Main.Columns[0].Width = 130;
                    dgv_Main.Columns[0].ReadOnly = true;
                    dgv_Main.Columns[1].ReadOnly = true;
                    dgv_Main.Columns[6].ReadOnly = true;
                    dgv_Main.Columns[7].ReadOnly = true;
                    dgv_Main.Columns[8].ReadOnly = true;
                    dgv_Main.Columns[9].ReadOnly = true;
                    dgv_Main.Columns[10].ReadOnly = true;
                    dgv_Main.Columns[11].ReadOnly = true;
                }
                else
                {
                    btn_Test_Save.Enabled = false;
                    dgv_Main.ReadOnly = true;
                }
                dgv_Main.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Main.Columns[0].Width = 130;
                dgv_Main.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Main.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Main.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Main.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Main.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Main.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Main.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_Main.Columns[10].Width = 135;
                dgv_Main.Columns[11].Width = 135;
            }
            else
            {
                btn_Test_Save.Enabled = false;
                MessageBox.Show("没有查询到数据", "错误");
            }
        }

        private void Save_Test()
        {
            string sqlstr = "UPDATE SCHEDULE SET SC013 = '{1}', SC036 = '{2}', SC037 = '{3}', SC010 = '{4}' WHERE SC001 = '{0}'";
            string sql_tmp = null;
            int Count = Main_dt.Rows.Count;
            int Index = 0;
            for(Index = 0; Index < Count; Index++)
            {
                sql_tmp = string.Format(sqlstr, Main_dt.Rows[Index][0], Main_dt.Rows[Index][3], Main_dt.Rows[Index][4], Main_dt.Rows[Index][5], Main_dt.Rows[Index][2]);
                mssql.SQLexcute(strConnection_ROBOT, sql_tmp);
            }
            MessageBox.Show("已保存！", "提示");
            Dgv_Show();
        }

        private void Show_Type_Click(object sender, EventArgs e)
        {
            TestFlag = !TestFlag;
            Btn_Show_Work();
            Dgv_Show();
        }

        private void btn_Reflash_Click(object sender, EventArgs e)
        {
            Dgv_Show();
        }

        private void btn_Test_Save_Click(object sender, EventArgs e)
        {
            Save_Test();
        }

        private void btn_Find_Type_Click(object sender, EventArgs e)
        {
            FindFlag = !FindFlag;
            Btn_Show_Work();
            if (FindFlag)
            {
                dgv_Main.DataSource = null;
            }
            else
            {
                Dgv_Show();
            }
        }

        private void btn_Test_Reset_Click(object sender, EventArgs e)
        {
            string sqlstr1 = "DELETE FROM PdData WHERE SC001 IN(SELECT SC001 FROM SCHEDULE WHERE SC039 = 'Y')";
            string sqlstr2 = "UPDATE SCHEDULE SET SC033 = 0, SC003 = CONVERT(VARCHAR(20), GETDATE(), 112) WHERE SC039 = 'Y'";
            mssql.SQLexcute(strConnection_ROBOT, sqlstr1);
            mssql.SQLexcute(strConnection_ROBOT, sqlstr2);
            MessageBox.Show("订单信息已重置", "成功");
            Dgv_Show();
        }

        private void btn_Test_Add_Click(object sender, EventArgs e)
        {

        }

        private void btn_Fine_Click(object sender, EventArgs e)
        {
            FindStr = txb_Fine.Text;
            Dgv_Show();
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
