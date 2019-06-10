using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友中山分公司生产辅助工具
{
    public partial class 物控登录 : Form
    {
        private static Mssql mssql = new Mssql();
        private static string connYLs = 主界面.connY_Ls;
        public static bool Flag = false;
        public static string Uid = null;

        public 物控登录()
        {
            InitializeComponent();
            Flag = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlstr = "select * from mf_zy where zydm = '{0}' and hpassword = '{1}' and bz like '%物控%' ";
            if (textBox1.Text != "" )
            {
                DataTable dt = mssql.SQLselect(connYLs, string.Format(sqlstr, textBox1.Text, textBox2.Text));
                if (dt != null)
                {
                    Flag = true;
                    Uid = textBox1.Text;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("输入账号或密码错误", "错误");
                }
            }
            else
            {
                MessageBox.Show("请输入账号！", "错误");
            }
        }
    }
}
