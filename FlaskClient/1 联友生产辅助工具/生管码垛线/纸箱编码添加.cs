﻿using HarveyZ;
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
    public partial class 纸箱编码添加 : Form
    {

        private static Mssql mssql = new Mssql();
        public static string connStrRobot = 纸箱编码管理.connStrRobot;

        public 纸箱编码添加()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlstr = @"INSERT INTO BoxSizeCode ( BoxSize, BoxCode, BoxSet) VALUES( '{0}', '{1}', '{2}')";
            mssql.SQLexcute(connStrRobot, string.Format(sqlstr, textBox1.Text, textBox2.Text, textBox3.Text));
            MessageBox.Show("已添加", "提示");
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }
    }
}
