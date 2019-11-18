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
        #region 定义公开全局变量
        //是否为测试模式：true: 查找测试服务器的地址进行连接，false：查找正式的服务器地址  --在Init中判断DEBUG模式来设定true
        public static bool URLTestFlag = false; 

        //公用信息
        public static WebNet webNet = new WebNet();//http post
        public static Mssql mssql = new Mssql();//MSSQL操作类

        //用户信息
        public static string Login_Uid = "";
        public static string Login_Name = "";
        public static string Login_Dpt = "";
        public static string Login_Role = "";

        //权限信息
        public static List<string> menuItemList = new List<string> { };//所有菜单栏列表
        public static List<string> userPermList = new List<string> { };//用户权限列表

        //软件信息
        public static string ProgVersion = "";
        public static string ProgName = "";

        //服务器URL
        public static string HttpURL = "";

        //数据库连接字符串
        public static string connWg = Global_Const.strConnection_WGDB;
        public static string connComfort = Global_Const.strConnection_COMFORT;
        public static string connY_Ls = Global_Const.strConnection_Y_LS;
        public static string connRobot = null;

        //数据库连接标志位
        public static bool connFlag99 = false;
        public static bool connFlag198 = false;
        public static bool connFlagY = false;
        
        
        #endregion

        #region 本地局域变量
        private bool MsgFlag = false;

        private string UpdateUrl = "";

        private delegate void SqlTestDelegate(string connStr);
        #endregion

        #region 窗口初始化
        public FormLogin()
        {
            InitializeComponent();
            GetMutilOpen();
            GetRobotDbConn();

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
                UpdateMe.ProgUpdate(ProgName, UpdateUrl);
            }

            labelVersion.Text = "Ver: " + ProgVersion;

            SqlTestDelegate sqlTestYDelegate = new SqlTestDelegate(SqlTestY);
            sqlTestYDelegate.BeginInvoke(connY_Ls, null, null);
            SqlTestDelegate sqlTest99Delegate = new SqlTestDelegate(SqlTest99);
            sqlTest99Delegate.BeginInvoke(connComfort, null, null);
            SqlTestDelegate sqlTest198Delegate = new SqlTestDelegate(SqlTest198);
            sqlTest198Delegate.BeginInvoke(connWg, null, null);
        }

        #endregion

        #region 窗口设计

        private void BtnLogin_Click(object sender, EventArgs e)//登录按钮
        {
            FormLogin_Login();
        }

        private void BtnExit_Click(object sender, EventArgs e)//退出按钮
        {
            if (MessageBox.Show("是否确认退出", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void TextBoxChanged(object sender, EventArgs e)//避免登录错误显示，鼠标点击，重新输入信息后，按下回车无反应
        {
            MsgFlag = false;
        }

        private void TextBoxUID_KeyUp(object sender, KeyEventArgs e)//输入框跳转
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

        private void TextBoxPWD_KeyUp(object sender, KeyEventArgs e)
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
            if (TextBoxUID.Text == "" )
            {
                if (MessageBox.Show("请输入账号或密码", "提示", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    MsgFlag = true;
                    FormLogin_TextBox_PWD.Text = "";
                    TextBoxUID.Focus();
                    TextBoxUID.SelectAll();
                }
            }
            else
            {
                string Msg = "";
                bool result = FormLogin_GetLogin(TextBoxUID.Text, FormLogin_TextBox_PWD.Text, out Msg);
                if (result)
                {
                    主界面 Form_main = new 主界面();
                    Form_main.Show();
                    this.Hide();
                }
                else
                {
                    if (MessageBox.Show(Msg, "登录失败", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        MsgFlag = true;
                        FormLogin_TextBox_PWD.Text = "";
                        TextBoxUID.Focus();
                        TextBoxUID.SelectAll();
                    }
                }
            }
        }
        #endregion

        #region 逻辑设计
        private void SqlTestY(string connStr)
        {
            connFlagY = mssql.SQLlinkTest(connStr);
        }
        private void SqlTest99(string connStr)
        {
            connFlag99 = mssql.SQLlinkTest(connStr);
        }
        private void SqlTest198(string connStr)
        {
            connFlag198 = mssql.SQLlinkTest(connStr);
        }

        private void GetRobotDbConn()
        {
            string sqlstr = @"SELECT ServerURL FROM WG_CONFIG WHERE ConfigName = 'MdSysDb' AND Valid = 'Y' ";
            string dbName = mssql.SQLselect(connWg, sqlstr).Rows[0][0].ToString();
            if (dbName == "ROBOT_TEST") connRobot = Global_Const.strConnection_ROBOT_TEST;
            if (dbName == "ROBOT") connRobot = Global_Const.strConnection_ROBOT;
        }

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
                var get = mssql.SQLselect(connWg, sqlstr).Rows[0][0].ToString();
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
                    var get = mssql.SQLselect(connWg, sqlstr).Rows[0][0].ToString();
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

        private bool GetNewVersion()
        {
            string Msg, Url;
            if(VersionManeger.GetNewVersion(ProgName, ProgVersion, out Msg, out Url))
            {
                UpdateUrl = HttpURL + Url + ProgName + ".exe";
                return true;
            }
            else
            {
                if(Msg != null)
                {
                    if(MessageBox.Show(Msg, "错误", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Environment.Exit(0);
                    }
                }
                return false;
            }
        }

        private void FormLogin_Init() //软件配置信息获取
        {
            HttpURL = GetHttpURL();
            //HttpURL = "http://192.168.0.197:8099";
        }
        
        private bool FormLogin_GetLogin(string LoginUid, string LoginPwd, out string Msg)//登录
        {
            UserLogin userLogin = new UserLogin();
            UserLogin.UserObjectReturn userObj = new UserLogin.UserObjectReturn();
            userObj.Uid = LoginUid;
            userObj.Pwd = CEncrypt.GetMd5Str(LoginPwd);
            userLogin.Login(userObj);
            if (userObj.Status)
            {
                Login_Uid = userObj.Uid;
                Login_Name = userObj.Name;
                Login_Dpt = userObj.Dpt;
                userPermList = userObj.Permission;
                Msg = userObj.Msg;
                return true;
            }
            else
            {
                Msg = userObj.Msg;
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
                Environment.Exit(0);
            }
        }
        #endregion
    }
}
