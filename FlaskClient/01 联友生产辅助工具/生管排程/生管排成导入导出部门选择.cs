using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.生管排程
{
    public partial class 生管排程导入导出部门选择 : Form
    {
        private static string Mode = null;
        public static string Dpt = null;
        Mssql mssql = new Mssql();
        string connWGDB = FormLogin.infObj.connMD;

        public 生管排程导入导出部门选择(string WorkMode)
        {
            InitializeComponent();
            Mode = WorkMode;
            Init();
        }

        private void Init()
        {
            if(Mode == "导入")
            {
                string sqlstr = "SELECT Dpt FROM SC_PLAN_DPT_TYPE WHERE Valid = 1 AND Type = 'In' ORDER BY [Index]";
                DataTable dt = mssql.SQLselect(connWGDB, sqlstr);
                if(dt != null)
                {
                    for(int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                    {
                        comboBoxDpt.Items.Add(dt.Rows[rowIdx][0].ToString());
                    }
                }
            }
            else if (Mode == "导出")
            {
                string sqlstr = "SELECT Dpt FROM SC_PLAN_DPT_TYPE WHERE Valid = 1 AND Type = 'Out' ORDER BY [Index]";
                DataTable dt = mssql.SQLselect(connWGDB, sqlstr);
                if (dt != null)
                {
                    for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                    {
                        comboBoxDpt.Items.Add(dt.Rows[rowIdx][0].ToString());
                    }
                }
            }
            Dpt = null;
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            if (comboBoxDpt.Text != "")
            {
                Dpt = comboBoxDpt.Text;
                this.Dispose();
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择部门", "错误");
            }
        }
    }
}
