using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using PDA工具;

namespace HarveyZ
{
    public partial class FormLogin : Form
    {
        private bool TestFlag = false;

        #region 定义公开全局变量
        public static string Login_Uid = "";
        public static string Login_Name = "";
        public static string Login_Dpt = "";
        public static string Login_Role = "";

        public static string Software_Version = "";
        public static string File_Version = "";
        #endregion

        #region 本地局域变量
        private bool MessageboxFlag = false;
        private string HttpURL = "";
        private string ConnStr = Global_Const.strConnection_WG_DB;
        
        private string ProgFileName = "";

        private string UpdateUrl = "";
        #endregion

        CEncrypt cEncrypt = new CEncrypt();
        Mssql mssql = new Mssql();
        WebNet Webnet = new WebNet();
        
        public FormLogin()
        {
            InitializeComponent();
            GetMutilOpen();

            //FormLogin_TextBox_UID.Text = "001114";
            //FormLogin_TextBox_PWD.Text = "Venushui";

            FormLogin_Init(); //配置信息获取

            //bool UpdFlag = GetNewVersion();
            //if (UpdFlag)
            //{
            //    Update();
            //}
            
            labelVersion.Text = "Ver: " + Software_Version;

        }

        #region 程序更新
        public bool StartProcess(string filename, string[] args)
        {
            try
            {
                string s = "";
                foreach (string arg in args)
                {
                    s = s + arg + " ";
                }
                s = s.Trim();
                Process myprocess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(filename, s);
                myprocess.StartInfo = startInfo;

                //通过以下参数可以控制exe的启动方式，具体参照 myprocess.StartInfo.下面的参数，如以无界面方式启动exe等
                myprocess.StartInfo.UseShellExecute = false;
                myprocess.Start();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动应用程序时出错！原因：" + ex.Message);
            }
            return false;
        }

        private void Update()
        {
            string Path = System.IO.Directory.GetCurrentDirectory();
            try
            {
                string[] arg = new string[2];
                arg[0] = UpdateUrl;
                arg[1] = ProgFileName;
                StartProcess(Path + "\\" + "AutoUpdate.exe", arg);
                System.Environment.Exit(0);
            }
            catch(Win32Exception e)
            {
                if(MessageBox.Show("找不到更新软件，程序即将退出!", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
            
        }
        #endregion

        #region 窗口设计

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

        private void FormLogin_TextBox_UID_KeyUp(object sender, KeyEventArgs e)//输入框跳转
        {
            if ((MessageboxFlag == true) && (e.KeyCode == Keys.Enter))
            {
                MessageboxFlag = false;
                return;
            }
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                    FormLogin_TextBox_PWD.SelectAll();
                    FormLogin_TextBox_PWD.Focus();
            }
        }

        private void FormLogin_TextBox_PWD_KeyUp(object sender, KeyEventArgs e)
        {
            if ((MessageboxFlag == true) && (e.KeyCode == Keys.Enter))
            {
                MessageboxFlag = false;
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                FormLogin_Login();
            }
        }

        private void FormLogin_Login()
        {
            if (FormLogin_TextBox_UID.Text == "" )
            {
                if (MessageBox.Show("请输入账号或密码", "提示", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    MessageboxFlag = true;
                    FormLogin_TextBox_PWD.Text = "";
                    FormLogin_TextBox_UID.Focus();
                    FormLogin_TextBox_UID.SelectAll();
                }
            }
            else
            {
                bool result = FormLogin_GetLogin(FormLogin_TextBox_UID.Text, FormLogin_TextBox_PWD.Text);
                if (result)
                {
                    if(Login_Uid != "000960")
                    {
                        if (MessageBox.Show("本程序目前只允许账号为000960登录！", "登录失败", MessageBoxButtons.OK) == DialogResult.OK)
                        {
                            MessageboxFlag = true;
                            FormLogin_TextBox_PWD.Text = "";
                            FormLogin_TextBox_UID.Focus();
                            FormLogin_TextBox_UID.SelectAll();
                        }
                    }
                    else
                    {
                        PDA_LL Form_main = new PDA_LL();
                        Form_main.Show();
                        this.Hide();
                    }
                    
                }
                else
                {
                    if (MessageBox.Show("输入的用户名或密码错误！", "登录失败", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        MessageboxFlag = true;
                        FormLogin_TextBox_PWD.Text = "";
                        FormLogin_TextBox_UID.Focus();
                        FormLogin_TextBox_UID.SelectAll();
                    }
                }
            }
        }
        #endregion

        #region 逻辑设计
        private string GetHttpURL()
        {
            string sqlstr = "";
            if (TestFlag)
            {
                sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='TEST' AND Vaild = 'Y'";
            }
            else
            {
                sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='WEB' AND Vaild = 'Y'";
            }
            var get = mssql.SQLselect(ConnStr, sqlstr).Rows[0][0].ToString();
            return get;
        }

        private bool HttpURLTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("", "");
            dict = Webnet.WebPost(HttpURL + "/Client/LinkTest", dict);
            string get = "";
            if (dict != null)
            {
                dict.TryGetValue("Return", out get);
                if (get == "Yes")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool GetNewVersion()
        {
            ProgFileName = System.IO.Path.GetFileName(Application.ExecutablePath).Split('.')[0];

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ProgName", ProgFileName);
            dict.Add("Version", Software_Version);
            dict = Webnet.WebPost(HttpURL + "/Client/GetVersion", dict);
            string Mode = "";
            dict.TryGetValue("Mode", out Mode);
            if (Mode == "Yes")
            {
                return false;
            }
            else if(Mode == "New")
            {
                dict.TryGetValue("URL", out UpdateUrl);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FormLogin_Init() //软件配置信息获取
        {
            Software_Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            File_Version = Application.ProductVersion.ToString();
            Software_Version = Software_Version.Split('.')[0] + '.' + Software_Version.Split('.')[1] + '.'
                + Software_Version.Split('.')[2];
            
            if (mssql.SQLlinkTest(ConnStr))
            {
                HttpURL = GetHttpURL();
                bool get = HttpURLTest();
                if (!get)
                {
                    if (MessageBox.Show("无法连接后台服务器", "配置错误", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                if (MessageBox.Show("无法连接配置服务器", "配置错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }

        private bool FormLogin_GetLogin(string Login_uid, string Login_pwd)//登录
        {
            Login_pwd = cEncrypt.GetMd5Str(Login_pwd); //转换成MD5值

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Login_Uid", Login_uid);
            dict.Add("Login_Pwd", Login_pwd);
            dict = Webnet.WebPost(HttpURL + "/Client/UserLogin", dict);
            
            if(dict != null)
            {
                string Login_status = "";
                dict.TryGetValue("Login_Status", out Login_status);
                if(Login_status == "Y")
                {
                    dict.TryGetValue("Login_Uid", out Login_Uid);
                    dict.TryGetValue("Login_Name", out Login_Name);
                    dict.TryGetValue("Login_Role", out Login_Role);
                    dict.TryGetValue("Login_Dpt", out Login_Dpt);
                    return true;
                }
                else if(Login_status == "y")
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
        
        #region 程序不能多开设定
        private void GetMutilOpen()
        {
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
        }
        #endregion
    }
}
