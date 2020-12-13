using System;
using System.Data;
using System.Windows.Forms;

namespace HarveyZ.生管排程
{
    public partial class 生产排程部门选择 : Form
    {
        public static string Dpt = null;
        Mssql mssql = new Mssql();
        string connWG = FormLogin.infObj.connWG;

        public 生产排程部门选择()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            string slqStr = "SELECT Dpt FROM dbo.SC_PLAN_DPT_TYPE WHERE Valid = 1 AND Type = 'In' ORDER BY K_ID";
            DataTable dt = mssql.SQLselect(connWG, slqStr);
            if (dt != null)
            {
                for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                {
                    comboBoxDpt.Items.Add(dt.Rows[rowIdx][0].ToString());
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
