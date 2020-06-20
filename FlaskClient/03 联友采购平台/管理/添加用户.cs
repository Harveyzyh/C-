using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{
    public partial class 添加用户 : Form
    {
        Mssql mssql = new Mssql();
        string conn = FormLogin.infObj.connYF;

        public 添加用户()
        {
            InitializeComponent();
        }

        private void TextBoxCompanyId_Leave(object sender, EventArgs e)
        {
            LabelCompanyName.Text = GetCompanyName(TextBoxCompanyId.Text);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (LabelCompanyName.Text != "")
            {
                if (FormLogin.infObj.userLogin.AddUser(TextBoxUid.Text, TextBoxName.Text, TextBoxPwd.Text, TextBoxCompanyId.Text, out msg))
                {
                    MessageBox.Show("已保存", "提示", MessageBoxButtons.OK);
                    TextBoxCompanyId.Text = "";
                    TextBoxName.Text = "";
                    TextBoxPwd.Text = "";
                    TextBoxUid.Text = "";
                }
                else
                {
                    MessageBox.Show(msg, "错误", MessageBoxButtons.OK);
                    TextBoxUid.Select();
                    TextBoxUid.SelectAll();
                }
            }
            else
            {
                MessageBox.Show("该供应商编号不存在，请重新输入！", "错误", MessageBoxButtons.OK);
                TextBoxCompanyId.Select();
                TextBoxCompanyId.SelectAll();
            }
        }

        private string GetCompanyName(string companyId)
        {
            string sqlstr = @"SELECT RTRIM(MA002) FROM dbo.PURMA WHERE MA001 = '{0}'";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlstr, companyId));
            if (dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
