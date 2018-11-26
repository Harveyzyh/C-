using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 联友生产辅助工具
{
    public partial class FormLogin : Form
    {
        //数据库连接字
        public static string strConnection = Global_Const.strConnection_COMFORT;
        CEncrypt cEncrypt = new CEncrypt();

        #region 窗口设计
        public FormLogin()
        {
            InitializeComponent();
            FormLogin_Init();
            FormLogin_CONFIG(); //配置信息获取
        }

        private void FormLogin_Init()
        {
            #region 程序不能多开设定
            bool Exist;//定义一个bool变量，用来表示是否已经运行
                       //创建Mutex互斥对象
            System.Threading.Mutex newMutex = new System.Threading.Mutex(true, "仅一次", out Exist);
            if (Exist)//如果没有运行
            {
                newMutex.ReleaseMutex();//运行新窗体
            }
            else
            {
                MessageBox.Show("本程序已正在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);//弹出提示信息
                this.Close();//关闭当前窗体
            }
            #endregion
        }

        private void FormLogin_Button_Login_Click(object sender, EventArgs e)//登录按钮
        {
            FormLogin_Login();
        }

        private void FormLogin_Button_Exit_Click(object sender, EventArgs e)//退出按钮
        {
            if (MessageBox.Show("是否确认退出", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void FormLogin_TextBox_UID_KeyUp(object sender, KeyEventArgs e)//输入框跳转，显示测试按钮逻辑
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (FormLogin_TextBox_UID.Text == "Config") //显示配置按钮
                {
                    FormLogin_Botton_Linktest.Visible = true;

                    FormLogin_TextBox_UID.Clear();
                }
                else if (FormLogin_TextBox_UID.Text == "Close") //显示配置按钮
                {
                    FormLogin_Botton_Linktest.Visible = false;
                    FormLogin_TextBox_UID.Clear();
                }
                else
                {
                    FormLogin_Botton_Linktest.Visible = false;
                    FormLogin_TextBox_PWD.SelectAll();
                    FormLogin_TextBox_PWD.Focus();
                }
            }
        }

        private void FormLogin_TextBox_PWD_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FormLogin_Login();
            }
        }

        private void FormLogin_Botton_Linktest_Click(object sender, EventArgs e)
        {
            Mssql.SQLlinkTest(strConnection);
        }
        #endregion

        #region 逻辑设计
        private void FormLogin_CONFIG() //软件配置信息获取
        {
            Global_Var.Software_Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Global_Var.File_Version = Application.ProductVersion.ToString();
        }

        private void FormLogin_Login()//登录
        {
            string Login_uid = FormLogin_TextBox_UID.Text;
            string Login_pwd = FormLogin_TextBox_PWD.Text;

            Global_Var.Login_UID = Login_uid; //登录用户名赋值到全局变量

            if (Login_uid == "" || Login_pwd == "")
            {
                FormLogin_Botton_Linktest.Visible = false;

                if (MessageBox.Show("请输入账号或密码", "提示", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    FormLogin_TextBox_PWD.Text = "";
                    FormLogin_TextBox_UID.Focus();
                    FormLogin_TextBox_UID.SelectAll();
                }
            }
            else
            {
                //Login_pwd = cEncrypt.GetMd5Str(Login_pwd); //转换成MD5值
                string sqlstr = "SELECT RTRIM(MV001), RTRIM(MV002), RTRIM(ME002) FROM CMSME "
                                + "INNER JOIN CMSMV ON MV004 = ME001 "
                                + "WHERE MV001 = '" + Login_uid.Trim() + "' ";
                DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);

                if (dttmp == null)
                {
                    if (MessageBox.Show("输入的用户名或密码错误！", "密码无效", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        FormLogin_TextBox_PWD.Text = "";
                        FormLogin_TextBox_UID.Focus();
                        FormLogin_TextBox_UID.SelectAll();
                    }
                }
                else
                {
                    Global_Var.Login_UID = dttmp.Rows[0][0].ToString();
                    Global_Var.Login_Name = dttmp.Rows[0][1].ToString();
                    Global_Var.Login_Dpt = dttmp.Rows[0][2].ToString();

                    dttmp.Dispose();
                    日报表 form_main = new 日报表();
                    form_main.Show();
                    this.Hide();
                }
            }
        }
        #endregion
    }
}
