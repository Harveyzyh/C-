using HarveyZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 联友生产辅助工具.生管码垛线
{
    public partial class 订单类别编码添加 : Form
    {

        private static Mssql mssql = new Mssql();
        public static string connStrRobot = 纸箱编码管理.connStrRobot;

        public 订单类别编码添加()
        {
            InitializeComponent();
            textBox1.Text = "内销";
            textBox1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlstr = @"INSERT INTO SplitTypeCode ( PO_Class, PO_Type, TypeCode) VALUES( '{0}', '{1}', '{2}')";

            string num = mssql.SQLselect(connStrRobot, " Select MAX(K_ID) +1 FROM SplitTypeCode").Rows[0][0].ToString();

            mssql.SQLexcute(connStrRobot, string.Format(sqlstr, textBox1.Text, textBox2.Text, 码垛线排程导入.Num2Char(num)));

            MessageBox.Show("已添加", "提示");
            textBox2.Text = "";
        }
    }
}
