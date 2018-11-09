using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 联友生产进度工具
{
    public partial class Form_Login : Form
    {
        CEncrypt cEncrypt = new CEncrypt();
        #region 窗口设计
        public Form_Login()
        {
            InitializeComponent();
            Form_Login_Init();
            Form_Login_CONFIG(); //配置信息获取
        }

        private void Form_Login_Init()
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

            Form_Login_TextBox_UID.Text = "ly";
            Form_Login_TextBox_PWD.Text = "ly";
        }

        private void Form_Login_Button_Login_Click(object sender, EventArgs e)//登录按钮
        {
            Form_Login_Login();
        }

        private void Form_Login_Button_Exit_Click(object sender, EventArgs e)//退出按钮
        {
            if (MessageBox.Show("是否确认退出", "提示", MessageBoxButtons.OKCancel)
                == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void Form_Login_TextBox_UID_KeyUp(object sender, KeyEventArgs e)//输入框跳转，显示测试按钮逻辑
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (Form_Login_TextBox_UID.Text == "Config") //显示配置按钮
                {
                    Form_Login_Botton_Linktest.Visible = true;

                    Form_Login_TextBox_UID.Clear();
                }
                else if (Form_Login_TextBox_UID.Text == "Close") //显示配置按钮
                {
                    Form_Login_Botton_Linktest.Visible = false;
                    Form_Login_TextBox_UID.Clear();
                }
                else
                {
                    Form_Login_Botton_Linktest.Visible = false;
                    Form_Login_TextBox_PWD.SelectAll();
                    Form_Login_TextBox_PWD.Focus();
                }
            }
        }

        private void Form_Login_TextBox_PWD_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Form_Login_Login();
            }
        }
        
        private void Form_Login_Botton_Linktest_Click(object sender, EventArgs e)
        {
            Mssql.SQLlinkTest(Global_Const.strConnection_COMFORT);
        }
        #endregion

        #region 逻辑设计
        private void Form_Login_CONFIG() //软件配置信息获取
        {
            Global_Var.Software_Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Global_Var.File_Version = Application.ProductVersion.ToString();
        }

        private void Form_Login_Login()//登录
        {
            string Login_uid = Form_Login_TextBox_UID.Text;
            string Login_pwd = Form_Login_TextBox_PWD.Text;

            Global_Var.Login_UID = Login_uid; //登录用户名赋值到全局变量

            if (Login_uid == "" || Login_pwd == "")
            {
                Form_Login_Botton_Linktest.Visible = false;

                if (MessageBox.Show("请输入账号或密码", "提示", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Form_Login_TextBox_PWD.Clear();
                    Form_Login_TextBox_UID.SelectAll();
                    Form_Login_TextBox_UID.Focus();
                }
            }
            else
            {
                Login_pwd = cEncrypt.GetMd5Str(Login_pwd); //转换成MD5值
                
                //修改密码
                //string sql = "UPDATE USERS SET PWD = '" + Login_pwd + "' WHERE USERID = '" + Login_uid + "'";
                //Mssql.SQLexcute(Global_Const.strConnection_CONFIG, sql);
                //MessageBox.Show(Login_pwd, "");

                string sqltmp = "SELECT USERID FROM USERS WHERE USERID = '" + Login_uid + "' AND PWD = '" + Login_pwd + "' ";
                DataTable dttmp = Mssql.SQLselect(Global_Const.strConnection_CONFIG, sqltmp);
                if (dttmp == null)
                {
                    if (MessageBox.Show("输入的用户名或密码错误！", "密码无效", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        Form_Login_TextBox_PWD.Clear();
                        Form_Login_TextBox_PWD.SelectAll();
                        Form_Login_TextBox_UID.Focus();
                    }
                }
                else
                {
                    dttmp.Dispose();
                    Form_Main form_main = new Form_Main();
                    form_main.Show();
                    this.Hide();
                }
            }
        }
        #endregion
    }
}
