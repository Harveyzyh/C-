using Common.Helper.Crypto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using 联友生产辅助工具;


namespace HarveyZ
{
    public partial class FormLogin : Form
    {
        public static bool URLTestFlag = false; //是否为测试模式：true: 查找测试服务器的地址进行连接，false：查找正式的服务器地址  --在Init中判断DEBUG模式来设定true

        #region 定义公开全局变量
        //公用信息
        public static WebNet webNet = new WebNet();//http post
        public static AesCrypto aes16 = new AesCrypto();//字符串加密
        public static CEncrypt cEncrypt = new CEncrypt();//MD5加密
        public static Mssql mssql = new Mssql();//MSSQL操作类
        public static DictJson dictJson = new DictJson();//字典json互转类

        //用户信息
        public static string Login_Uid = "";
        public static string Login_Name = "";
        public static string Login_Dpt = "";
        public static string Login_Role = "";

        //权限信息
        public static List<string> MenuItemList = new List<string> { };//所有菜单栏列表
        public static List<string> PermItemList = new List<string> { };//用户权限列表

        //软件信息
        public static string ProgVersion = "";
        public static string ProgName = "";

        //服务器URL
        public static string HttpURL = "HTTP://192.168.31.29:80";

        //数据库连接字符串
        public static string Conn_WG_DB = Global_Const.strConnection_WG_DB;
        public static string Conn_ERP99 = Global_Const.strConnection_COMFORT;
        public static string Conn_ERP_TEST = Global_Const.strConnection_COMFORT_TEST;
        public static string Conn_ROBOT = Global_Const.strConnection_ROBOT;
        
        #endregion

        #region 本地局域变量
        private bool MsgFlag = false;

        private string UpdateUrl = "";
        #endregion

        #region Init
        public FormLogin()
        {
            InitializeComponent();
            GetMutilOpen();

            //从文件详细信息中获取程序名称
            ProgName = Application.ProductName.ToString();
            ProgVersion = Application.ProductVersion.ToString();

            //判断是否在debug模式
            #if DEBUG
            URLTestFlag = true;
            this.Text += "    -DEBUG";
            #endif

            FormLogin_Init(); //配置信息获取
            
            if (GetNewVersion())
            {
                ProgUpdate();
            }

            labelVersion.Text = "Ver: " + ProgVersion;

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

        private void FormLogin_TextBox_Changed(object sender, EventArgs e)//避免登录错误显示，鼠标点击，重新输入信息后，按下回车无反应
        {
            MsgFlag = false;
        }

        private void FormLogin_TextBox_UID_KeyUp(object sender, KeyEventArgs e)//输入框跳转
        {
            if ((MsgFlag == true) && (e.KeyCode == Keys.Enter))
            {
                MsgFlag = false;
                return;
            }
            else
            {
                MsgFlag = false;
            }
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                    FormLogin_TextBox_PWD.SelectAll();
                    FormLogin_TextBox_PWD.Focus();
            }
        }

        private void FormLogin_TextBox_PWD_KeyUp(object sender, KeyEventArgs e)
        {
            if ((MsgFlag == true) && (e.KeyCode == Keys.Enter))
            {
                MsgFlag = false;
                return;
            }
            else
            {
                MsgFlag = false;
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
                    MsgFlag = true;
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
                    主界面 Form_main = new 主界面();
                    Form_main.Show();
                    this.Hide();
                }
                else
                {
                    if (MessageBox.Show("输入的用户名或密码错误！", "登录失败", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        MsgFlag = true;
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
            try
            {
                string sqlstr = "";
                if (URLTestFlag)
                {
                    sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='TEST' AND Valid = 'Y'";
                }
                else
                {
                    sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='WEB' AND Valid = 'Y'";
                }
                var get = mssql.SQLselect(Conn_WG_DB, sqlstr).Rows[0][0].ToString();
                return get;
            }
            catch
            {
                try
                {
                    string sqlstr = "";
                    if (URLTestFlag)
                    {
                        sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='TEST' AND Vaild = 'Y'";
                    }
                    else
                    {
                        sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='WEB' AND Vaild = 'Y'";
                    }
                    var get = mssql.SQLselect(Conn_WG_DB, sqlstr).Rows[0][0].ToString();
                    return get;
                }
                catch
                {
                    if (MessageBox.Show("错误", "获取后台服务器配置失败，请联系咨询部！", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Environment.Exit(0);
                    }
                    return null;
                }
                
            }
            
        }

        private bool HttpURLTest()
        {
            
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Test", "Client");
            dict = HttpPost(HttpURL + "/Client/LinkTest", dict, 5000);
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
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ProgName", ProgName);
            dict.Add("Version", ProgVersion);
            try
            {
                dict = HttpPost(HttpURL + "/Client/VersionManager", dict);
                string Mode = "";
                dict.TryGetValue("Mode", out Mode);
                if (Mode == "Yes")
                {
                    return false;
                }
                else if (Mode == "New")
                {
                    dict.TryGetValue("URL", out UpdateUrl);
                    return true;
                }
                else if (Mode == "False")
                {
                    dict.TryGetValue("URL", out UpdateUrl);
                    if (MessageBox.Show("程序：" + ProgName + " 已停用！ 请勿再使用，谢谢！\n\r\n\r" + UpdateUrl, "错误", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Environment.Exit(0);
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("无法连接程序版本管理器", "错误", MessageBoxButtons.OK);
                return false;
            }
        }

        private void FormLogin_Init() //软件配置信息获取
        {
            HttpURL = GetHttpURL();
            bool get = HttpURLTest();
            if (!get)
            {
                if (MessageBox.Show("无法连接后台服务器\n 程序即将退出！", "配置错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }

        private bool FormLogin_GetLogin(string LoginUid, string LoginPwd)//登录
        {
            LoginPwd = cEncrypt.GetMd5Str(LoginPwd); //转换成MD5值

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Module", "UserManager");
            dict.Add("Mode", "UserLogin");
            dict.Add("Uid", LoginUid);
            dict.Add("Pwd", LoginPwd);
            dict = HttpPost(HttpURL + "/Client/UserManager", dict);
            
            if(dict != null)
            {
                string LoginStatus = "";
                string Message = "";
                dict.TryGetValue("Status", out LoginStatus);
                dict.TryGetValue("Message", out Message);
                if(LoginStatus == "Y")
                {
                    string PermStr = "";
                    dict.TryGetValue("Uid", out Login_Uid);
                    dict.TryGetValue("Name", out Login_Name);
                    dict.TryGetValue("Role", out Login_Role);
                    dict.TryGetValue("Dpt", out Login_Dpt);
                    dict.TryGetValue("Permission", out PermStr);
                    if(PermStr != null)
                    {
                        foreach(string PermStrTmp in PermStr.Trim().Split('|'))
                        {
                            PermItemList.Add(PermStrTmp);
                        }
                    }
                    return true;
                }
                else if(LoginStatus == "y")
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

        #region HTTP Post发送
        public static Dictionary<string, string> HttpPost(string webURL, Dictionary<string, string>dict, int timeout = 60000)
        {
            Dictionary<string, string> dictBack = new Dictionary<string, string> { };

            string jsonin = dictJson.Dict2Json(dict);
            string jsonin_enc = aes16.Encrypt(jsonin);
            string jsonout = webNet.WebPost(webURL, jsonin_enc, timeout);
            jsonout = aes16.Decrypt(jsonout);
            dictBack = dictJson.Json2Dict(jsonout);
            return dictBack;
        }
        #endregion

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

        private void ProgUpdate()
        {
            string Path = System.IO.Directory.GetCurrentDirectory();
            try
            {
                string[] arg = new string[2];
                arg[0] = HttpURL + UpdateUrl;
                arg[1] = ProgName;
                StartProcess(Path + "\\" + "AutoUpdate.exe", arg);
                System.Environment.Exit(0);
            }
            catch (Win32Exception e)
            {
                if (MessageBox.Show("找不到更新软件，程序即将退出!", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
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
                Environment.Exit(0);
            }
        }
        #endregion
    }
}
