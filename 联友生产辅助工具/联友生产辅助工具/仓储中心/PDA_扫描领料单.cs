﻿using HarveyZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 联友生产辅助工具.仓储中心
{
    public partial class PDA_扫描领料单 : Form
    {
        string strConnection = Global_Const.strConnection_COMFORT;
        DataTable dttmp = null;

        Mssql mssql = new Mssql();

        string xa007 = "";
        public PDA_扫描领料单()
        {
            InitializeComponent();
            Button_Upload.Enabled = false;
        }

        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Dispose();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region 窗口大小变化设置
        private void Form_MainResized(object sender, EventArgs e)
        {
            Form_MainResized_Work();
            panel_Title.Enabled = true;
        }

        private void Form_MainResized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            DataGridView_List.Location = new Point(0, panel_Title.Height + 2);
            DataGridView_List.Size = new Size(FormWidth, FormHeight - panel_Title.Height - 2);

        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            button1_Work();
        }

        private void button1_Work()
        {
            if (TextBox_Danhao.Text != "")
            {
                string danhao_L = TextBox_Danhao.Text;
                string[] danhao_arry = danhao_L.Split('-');
                string danbie = danhao_arry[0];
                string danhao = danhao_arry[1];
                check(danbie, danhao);
            }
            else
            {
                MessageBox.Show("请录入单别单号", "错误");
                Button_Upload.Enabled = false;
                dttmp = null;
                DataGridView_List.DataSource = null;
            }
        }

        private void button1_Click_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Work();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            insertsql();
        }



        private string getTime()
        {
            
            string sqlstr = "SELECT REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(30), GETDATE(), 120), '-', ''), ':', ''), ' ', '')";
            DataTable dttmp2 = mssql.SQLselect(strConnection, sqlstr);
            if (dttmp2 != null)
            {
                return dttmp2.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        private void check(string danbie, string danhao)
        {
            if (checkMOCTC(danbie, danhao))
            {
                if (!checkLLXA(danbie, danhao))
                {
                    showList(danbie, danhao);
                }
                else
                {
                    MessageBox.Show("领料单：" + danbie + "-" + danhao + " 已扫描！");
                    TextBox_Danhao.Text = "";
                }
            }
            else
            {
                MessageBox.Show("领料单：" + danbie + "-" + danhao + " 不存在于ERP系统！");
                TextBox_Danhao.Text = "";
            }

        }

        private bool checkMOCTC(string danbie, string danhao)
        {
            string sqlstr = "SELECT TC001, TC002 FROM MOCTC WHERE TC001 = '" + danbie + "' AND TC002 = '" + danhao + "'";
            DataTable dttmp2 = mssql.SQLselect(strConnection, sqlstr);
            if (dttmp2 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkLLXA(string danbie, string danhao)
        {
            string sqlstr = "SELECT LLXA001 FROM LL_LYXA WHERE LLXA001='" + danbie + "' AND LLXA002='" + danhao + "'";
            DataTable dttmp2 = mssql.SQLselect(strConnection, sqlstr);
            if (dttmp2 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkLLXA007(string LLXA007)
        {
            string sqlstr = "SELECT LLXA007 FROM LL_LYXA WHERE LLXA007='" + LLXA007 + "'";
            DataTable dttmp2 = mssql.SQLselect(strConnection, sqlstr);
            if (dttmp2 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string getLLXA007()
        {
        	string time = getTime();
        	string long_time = "";
        	for(int index = 1; ;index ++)
        	{
        		long_time = "LL" + time + index.ToString().PadLeft(4, '0');
        		if (!checkLLXA007(long_time))
        		{
        			break;
        		}
        	}
        	return long_time;
        }

        private void showList(string danbie, string danhao)
        {
        	xa007 = getLLXA007();
            string sqlstr = "SELECT TE001 AS LLXA001,TE002 AS LLXA002,TE003 AS LLXA003,TE004 AS LLXA012,"
                + " TE017 AS pinming,TE018 AS guige,CONVERT(FLOAT, TE005) AS LLXA017,TE008 AS LLXA013,MC002,TE009 AS LLXA011,"
                + " MW002,TE010 AS LLXA015,TE011 AS LLXA009,TE012 AS LLXA010,TE014 AS LLXA018,"
                + " '" + xa007 + "' AS LLXA007 "
                + " FROM VMOCTEJ WHERE TE001 = '" + danbie + "' AND TE002 = '" + danhao + "'";
            dttmp = mssql.SQLselect(strConnection, sqlstr);
            if (dttmp != null)
            {
                DataGridView_List.DataSource = dttmp;
                Button_Upload.Enabled = true;
                label1.Text = "领料单共" + dttmp.Rows.Count.ToString() + "条！";
                Button_Upload.Select();
            }
            else
            {
                Button_Upload.Enabled = false;
                MessageBox.Show("领料单：" + danbie + "-" + danhao + " 已审核！", "错误");
            }
        }

        private void insertsql()
        {
            string sqlstr = "";
            int index;
            if (dttmp != null)
            {
                string create_date = getTime();
                string creator = "Tools";
                string xa001 = "";
                string xa002 = "";
                string xa003 = "";
                string xa009 = "";
                string xa010 = "";
                string xa011 = "";
                string xa012 = "";
                string xa013 = "";
                string xa015 = "";
                string xa017 = "";
                string xa018 = "";
                if (!checkLLXA007(xa007))
                {
                    xa007 = getLLXA007();
                }
                for (index = 0; index < dttmp.Rows.Count; index++)
                {
                    xa001 = dttmp.Rows[index][0].ToString().Trim();
                    xa002 = dttmp.Rows[index][1].ToString().Trim();
                    xa003 = dttmp.Rows[index][2].ToString().Trim();
                    xa009 = dttmp.Rows[index][12].ToString().Trim();
                    xa010 = dttmp.Rows[index][13].ToString().Trim();
                    xa011 = dttmp.Rows[index][9].ToString().Trim();
                    xa012 = dttmp.Rows[index][3].ToString().Trim();
                    xa013 = dttmp.Rows[index][7].ToString().Trim();
                    xa015 = dttmp.Rows[index][11].ToString().Trim();
                    xa017 = dttmp.Rows[index][6].ToString().Trim();
                    xa018 = dttmp.Rows[index][14].ToString().Trim();
                    sqlstr = "INSERT INTO LL_LYXA (CREATOR, CREATE_DATE, LLXA001, LLXA002, LLXA003, LLXA007, LLXA009, LLXA010, LLXA011, LLXA012, LLXA013, LLXA015, LLXA017, LLXA018) VALUES ( "
                            +  "'" +creator + "', '" + create_date + "', '" + xa001 + "', '" + xa002 + "', '" + xa003 + "', '" + xa007 + "', '" + xa009 + "','" + xa010 + "', '" + xa011 + "', '" + xa012 + "','" 
                            + xa013 + "', '" + xa015 + "', '" + xa017 + "', '" + xa018 + "')";
                    mssql.SQLexcute(strConnection, sqlstr);
                }
                DataGridView_List.DataSource = null;
                dttmp = null;
                Button_Upload.Enabled = false;
                TextBox_Danhao.Text = "";
                Lable_Danhao.Text = "";
                label3.Text = "已上传" + index.ToString() + "条记录！";
                MessageBox.Show("已上传" + index.ToString() + "条记录！", "");
            }
            else
            {
                Button_Upload.Enabled = false;
                MessageBox.Show("", "错误");
            }
        }
    }
}
