using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 联友生产辅助工具.生管排程
{
    public partial class 生管排成导入导出部门选择 : Form
    {
        private static string Mode = null;
        public static string Dpt = null;

        public 生管排成导入导出部门选择(string WorkMode)
        {
            InitializeComponent();
            Mode = WorkMode;
            Init();
        }

        private void Init()
        {
            if(Mode == "导入")
            {
                comboBoxDpt.Items.Add("生产一部");
                comboBoxDpt.Items.Add("生产二部");
                comboBoxDpt.Items.Add("生产三部");
                comboBoxDpt.Items.Add("生产四部");
                comboBoxDpt.Items.Add("生产五部");
                comboBoxDpt.Items.Add("半成品");
                comboBoxDpt.Items.Add("原材料");
            }
            else if (Mode == "导出")
            {
                comboBoxDpt.Items.Add("全部");
                comboBoxDpt.Items.Add("生产一部");
                comboBoxDpt.Items.Add("生产二部");
                comboBoxDpt.Items.Add("生产三部");
                comboBoxDpt.Items.Add("生产四部");
                comboBoxDpt.Items.Add("生产五部");
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
