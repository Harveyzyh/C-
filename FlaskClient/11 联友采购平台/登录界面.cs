using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using 联友采购平台;


namespace HarveyZ
{
    public partial class 登录界面 : Form
    {
        public static InfoObject infObj = new InfoObject();
        private Main main = new Main(infObj);
        
        #region 定义公开全局变量
        //是否为测试模式：true: 查找测试服务器的地址进行连接，false：查找正式的服务器地址  --在Init中判断DEBUG模式来设定true
        public static bool URLTestFlag = false; 

        //公用信息
        public static WebNet webNet = new WebNet();//http post

        //用户信息
        public static string Login_Uid = "";
        public static string Login_Name = "";
        public static string Login_Dpt = "";
        public static string Login_Role = "";

        //权限信息
        public static List<string> menuItemList = new List<string> { };//所有菜单栏列表
        public static List<string> userPermList = new List<string> { };//用户权限列表

        //数据库连接字符串
        //public static string connWg = Global_Const.strConnection_WGDB;

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
        public 登录界面()
        {
            InitializeComponent();
            GetMutilOpen();

            //判断是否在debug模式
            #if DEBUG
            infObj.testFlag = true;
            this.Text += "    -DEBUG";
            #endif

            FormLogin_Init(); //配置信息获取
            
            if (GetNewVersion())
            {
                UpdateMe.ProgUpdate(infObj.progName, UpdateUrl);
            }

            labelVersion.Text = "Ver: " + infObj.progVer;
            
            SqlTestDelegate sqlTest99Delegate = new SqlTestDelegate(SqlTest99);
            sqlTest99Delegate.BeginInvoke(infObj.connComfort, null, null);
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

        private void textBoxChanged(object sender, EventArgs e)//避免登录错误显示，鼠标点击，重新输入信息后，按下回车无反应
        {
            MsgFlag = false;
        }

        private void textBoxUid_KeyUp(object sender, KeyEventArgs e)//输入框跳转
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
                    textBoxPwd.SelectAll();
                    textBoxPwd.Focus();
            }
        }

        private void textBoxPwd_KeyUp(object sender, KeyEventArgs e)
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
            if (textBoxUid.Text == "" )
            {
                if (MessageBox.Show("请输入账号或密码", "提示", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    MsgFlag = true;
                    textBoxPwd.Text = "";
                    textBoxUid.Focus();
                    textBoxUid.SelectAll();
                }
            }
            else
            {
                string Msg = "";
                bool result = FormLogin_GetLogin(textBoxUid.Text, textBoxPwd.Text, out Msg);
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
                        textBoxPwd.Text = "";
                        textBoxUid.Focus();
                        textBoxUid.SelectAll();
                    }
                }
            }
        }
        #endregion

        #region 逻辑设计
        private void SqlTest99(string connStr)
        {
            connFlag99 = infObj.sql.SQLlinkTest(connStr);
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
                var get = infObj.sql.SQLselect(infObj.connWg, sqlstr).Rows[0][0].ToString();
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
                    var get = infObj.sql.SQLselect(infObj.connWg, sqlstr).Rows[0][0].ToString();
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
            if(VersionManeger.GetNewVersion(infObj.progName, infObj.progVer, out Msg, out Url))
            {
                UpdateUrl = infObj.httpHost + Url + infObj.progName + ".exe";
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
            bool flag = main.GetHttpURL();
            if(!flag)
            {
                if (MessageBox.Show("错误", "获取后台服务器配置失败，请联系咨询部！", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
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

        private void textBoxUid_Leave(object sender, EventArgs e)
        {
            infObj.userId = textBoxUid.Text;
        }

        private void textBoxPwd_Leave(object sender, EventArgs e)
        {
            infObj.userPwd = textBoxPwd.Text;
        }
    }

    public class Main
    {
        private InfoObject infObj = null;

        public Main(InfoObject _infObj)
        {
            infObj = _infObj;
            infObj.sql = new Mssql();
            infObj.connWg = Global_Const.strConnection_WGDB;
            infObj.connComfort = Global_Const.strConnection_COMFORT;

            GetProgInfo();
        }

        public void GetProgInfo()
        {
            infObj.progName = Application.ProductName.ToString();
            infObj.progVer = Application.ProductVersion.ToString();
        }

        public bool GetHttpURL()
        {
            try
            {
                string sqlstr = "";
                if (infObj.testFlag)
                {
                    sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='TEST' AND Valid = 'Y'";
                }
                else
                {
                    sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='WEB' AND Valid = 'Y'";
                }
                infObj.httpHost = infObj.sql.SQLselect(infObj.connWg, sqlstr).Rows[0][0].ToString();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }

    public class InfoObject : InfoObjectBase
    {
        private string _userPwd = null;
        private string _httpHost = null;
        private bool _testFlag = false;
        private string _progName = null;
        private string _progVer = null;
        private string _connWg = null;
        private string _connComfort = null;

        public string userPwd { get { return _userPwd; } set { _userPwd = value; } }
        public string httpHost { get { return _httpHost; } set { _httpHost = value; } }
        public bool testFlag { get { return _testFlag; } set { _testFlag = value; } }

        public string connWg { get { return _connWg; } set { _connWg = value; } }
        public string connComfort { get { return _connComfort; } set { _connComfort = value; } }

        public string progName { get { return _progName; } set { _progName = value; } }
        public string progVer { get { return _progVer; } set { _progVer = value; } }
    }
}
