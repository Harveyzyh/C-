using HarveyZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ.生产日报表
{
    public partial class 日报表获取组别系列 : Form
    {
        public string strConnection = FormLogin.infObj.connWG;

        Mssql mssql = new Mssql();


        public static bool XL_ChangeFlag = false;
        public static string XL_List = "";

        public 日报表获取组别系列(string slqStr)
        {
            InitializeComponent();
            XL_List = "";
            Init(slqStr);
        }

        private void Init(string slqStr)
        {
            DataTable dttmp = mssql.SQLselect(strConnection, slqStr);
            dataGridView1.DataSource = dttmp;
            if(dttmp != null)
            {
                dataGridView1.Columns[1].Width = 150;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.Bisque;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0, Index = 0;
            int total = dataGridView1.RowCount;
            for(Index = 0; Index < total; Index++)
            {
                if((bool)dataGridView1.Rows[Index].Cells[0].EditedFormattedValue == true)
                {
                    if(count == 0)
                    {
                        XL_List += "'";
                        XL_List += dataGridView1.Rows[Index].Cells[1].Value.ToString() + "', '";
                    }
                    else
                    {
                        XL_List += dataGridView1.Rows[Index].Cells[1].Value.ToString() + "', '";
                    }
                    count++;
                }
            }
            if(count == 0)
            {
                MessageBox.Show("至少选择一个！", "提示", MessageBoxButtons.OK);
            }
            else
            {
                XL_List = XL_List.Substring(0, XL_List.Length - 3);
                XL_ChangeFlag = true;
                this.Dispose();
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XL_ChangeFlag = false;
            this.Dispose();
            this.Close();
        }
    }
}
