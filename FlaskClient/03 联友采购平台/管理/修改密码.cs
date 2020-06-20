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
    public partial class 修改密码 : Form
    {
        public 修改密码()
        {
            InitializeComponent();
            TextBoxUid.Text = FormLogin.infObj.userId;
            TextBoxOldPwd.Select();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if(TextBoxNewPwd.Text == TextBoxNewPwdConfirm.Text)
            {
                string msg = "";
                if (FormLogin.infObj.userLogin.ChangePwd(FormLogin.infObj.userId, TextBoxOldPwd.Text, TextBoxNewPwd.Text, out msg))
                {
                    MessageBox.Show("密码已修改，下次登录使用新密码！", "提示", MessageBoxButtons.OK);
                    TextBoxNewPwdConfirm.Text = "";
                    TextBoxNewPwd.Text = "";
                    TextBoxOldPwd.Text = "";
                }
                else
                {
                    MessageBox.Show(msg, "错误", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("新密码输入不一致，请重新输入！", "错误", MessageBoxButtons.OK);
                TextBoxNewPwdConfirm.Text = "";
                TextBoxNewPwd.Select();
                TextBoxNewPwd.SelectAll();
            }
        }
    }
}
