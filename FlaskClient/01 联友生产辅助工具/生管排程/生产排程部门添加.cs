using System;
using System.Windows.Forms;

namespace HarveyZ.生管排程
{
    public partial class 生产排程部门添加 : Form
    {

        private static Mssql mssql = new Mssql();
        private string connWG = FormLogin.infObj.connWG;

        public 生产排程部门添加()
        {
            InitializeComponent();
            ComboBoxDptType.Items.Add("导入");
            ComboBoxDptType.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                Msg.ShowErr("排程部门不能为空");
            }
            else
            {
                string slqStr2 = @"SELECT 1 FROM dbo.SC_PLAN_DPT_TYPE WHERE Type = 'In' AND Dpt = '{0}' ";
                if (mssql.SQLexist(connWG, string.Format(slqStr2, textBox2.Text)))
                {
                    Msg.ShowErr("当前信息已存在，保存失败");
                }
                else
                {
                    string slqStr = @"INSERT INTO dbo.SC_PLAN_DPT_TYPE (K_ID, [Type], Dpt, ERPDpt, Valid) VALUES((SELECT MAX(K_ID)+1 FROM dbo.SC_PLAN_DPT_TYPE WHERE [Type] = '{0}'), '{0}', '{1}', '{2}', 1) ";
                    mssql.SQLexcute(connWG, string.Format(slqStr, ComboBoxDptType.Text.Replace("导入", "In").Replace("导出", "Out"), textBox2.Text, textBox1.Text));
                    Msg.Show("已添加");
                    textBox2.Text = "";
                }
            }
        }
    }
}
