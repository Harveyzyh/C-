using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ;
using System.Collections.Generic;

namespace 联友生产辅助工具.生管码垛线
{
    public partial class 码垛线客户端 : Form
    {
        #region 数据库连接字
        public string connMD = FormLogin.infObj.connMD;
        #endregion

        #region 局部变量
        private Mssql mssql = new Mssql();
        private DataTable Main_dt = null;

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;

        private bool TestFlag = false;
        private bool FindFlag = false;
        private bool InitFlag = true;

        private string FindStr = "";
        #endregion

        public 码垛线客户端(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
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
                    label1.Visible = false;
                    dateTimePicker1.Visible = false;
                }
                else//正常显示模式
                {
                    btn_Reflash.Visible = true;
                    btn_Find_Type.Visible = true;
                    btn_Find_Type.Text = "进入查找模式";
                    btn_Show_Type.Text = "显示测试订单";
                    btn_Show_Type.Visible = true;
                    label1.Visible = true;
                    dateTimePicker1.Visible = true;
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
                label1.Visible = false;
                dateTimePicker1.Visible = false;
            }
        }

        private void Dgv_Show()
        {
            if (dgv_Main.DataSource != null)
            {
                dgv_Main.DataSource = null;
            }
            string sqlstr = "SELECT SC.SC001 订单号, SC003 上线日期, "
                            + "SC010 品名, SC013 数量, SC036 纸箱编码, SC040 纸箱尺寸, "
                            + "SC037 订单编码, '' 订单类别, MD_No 栈板号, ISNULL(PDCOUNT, 0) 已过机数量, "
                            + "(CASE SC033 WHEN '1' THEN 'Y' ELSE 'N'END ) 已完成, PD2.MIXDATE 最早过机时间, PD2.MAXDATE 最迟过机时间 "
                            + "FROM SCHEDULE AS SC "
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
                    sqlstr += "AND SC.SC003 = '" + dateTimePicker1.Value.ToString("yyyyMMdd") + "' AND SC.SC039 != 'Y' ";
                }
            }

            sqlstr += "ORDER BY KEY_ID ";

            Main_dt = mssql.SQLselect(connMD, sqlstr);

            if (Main_dt != null)
            {
                DtOpt.DtDateFormat(Main_dt, "上线日期");
                dgv_Main.DataSource = Main_dt;
                DgvOpt.SetRowBackColor(dgv_Main);

                if (TestFlag)
                {
                    btn_Test_Save.Enabled = true;
                    dgv_Main.ReadOnly = false;
                    List<int> list = new List<int>();
                    list.Add(3);
                    list.Add(4);
                    list.Add(5);
                    list.Add(6);
                    list.Add(7);
                    DgvOpt.SetColWritable(dgv_Main, list);
                }
                else
                {
                    btn_Test_Save.Enabled = false;
                    dgv_Main.ReadOnly = true;
                }
                dgv_Main.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvOpt.SetColMiddleCenter(dgv_Main, "生产日期");
                DgvOpt.SetColMiddleCenter(dgv_Main, "数量");
                DgvOpt.SetColMiddleCenter(dgv_Main, "纸箱编码");
                DgvOpt.SetColMiddleCenter(dgv_Main, "纸箱尺寸");
                DgvOpt.SetColMiddleCenter(dgv_Main, "订单编码");
                DgvOpt.SetColMiddleCenter(dgv_Main, "栈板号");
                DgvOpt.SetColMiddleCenter(dgv_Main, "已过机数量");
                DgvOpt.SetColMiddleCenter(dgv_Main, "已完成");
                DgvOpt.SetColWidth(dgv_Main, "最早过机时间", 135);
                DgvOpt.SetColWidth(dgv_Main, "最迟过机时间", 135);
                DgvOpt.SetColWidth(dgv_Main, "订单号", 130);
                DgvOpt.SetColWidth(dgv_Main, "品名", 250);
            }
            else
            {
                btn_Test_Save.Enabled = false;
                MessageBox.Show("没有查询到数据", "错误");
            }
        }

        private void Save_Test()
        {
            string sqlstr = "UPDATE SCHEDULE SET SC013 = '{1}', SC010 = '{2}', SC036 = '{3}', SC037 = '{4}', SC040 = '{5}' WHERE SC001 = '{0}'";
            string sql_tmp = null;
            int Count = Main_dt.Rows.Count;
            int Index = 0;
            for (Index = 0; Index < Count; Index++)
            {
                sql_tmp = string.Format(sqlstr, Main_dt.Rows[Index]["订单号"], Main_dt.Rows[Index]["数量"], Main_dt.Rows[Index]["品名"], Main_dt.Rows[Index]["纸箱编码"], Main_dt.Rows[Index]["订单编码"], Main_dt.Rows[Index]["纸箱尺寸"]);
                mssql.SQLexcute(connMD, sql_tmp);
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
            mssql.SQLexcute(connMD, sqlstr1);
            mssql.SQLexcute(connMD, sqlstr2);
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Dgv_Show();
        }
    }
}
