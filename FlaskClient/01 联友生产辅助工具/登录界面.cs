using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace HarveyZ
{
    public partial class FormLogin : Form
    {
        public static InfoObject infObj = new InfoObject();
        private Main main = new Main(infObj);
        
        #region 本地局域变量
        private bool MsgFlag = false;

        private delegate void SqlTestDelegate(string connStr);
        #endregion

        #region 窗口初始化
        public FormLogin()
        {
            InitializeComponent();
            GetMutilOpen();

            //判断是否在debug模式
            #if DEBUG
            infObj.testFlag = true;
            this.Text += "    -DEBUG";
            #endif

            FormLogin_Init(); //配置信息获取
            
            if (main.GetNewVersion())
            {
                UpdateMe.ProgUpdate(infObj.progName, infObj.updateHost + @"/download/" + infObj.progName + ".exe");
            }

            labelVersion.Text = "Ver: " + infObj.progVer;

            SqlTestDelegate sqlTestDelegate = new SqlTestDelegate(SqlTest);
            sqlTestDelegate.BeginInvoke(infObj.connWG, null, null);

            textBoxUid.Text = infObj.userId;
            textBoxUid.SelectAll();
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
                bool result = main.FormLogin_GetLogin(textBoxUid.Text, textBoxPwd.Text, out Msg);
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
        private void SqlTest(string connStr)
        {
            infObj.connFlag = infObj.sql.SQLlinkTest(connStr);
        }

        private void FormLogin_Init() //软件配置信息获取
        {
            if(!main.GetUpdateHost())
            {
                if (MessageBox.Show("获取后台服务器配置失败，请联系咨询部！", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Environment.Exit(0);
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

    public class Main
    {
        private InfoObject infObj = null;
        private VersionManeger versionManager = null;

        public Main(InfoObject _infObj)
        {
            infObj = _infObj;
            infObj.sql = new Mssql();

            GetProgInfo();
            GetRemoteFlag();
            SetConnInfo();

            InitMainIniPath();
            InitUserId();

            versionManager = new VersionManeger(infObj.connWG);
        }

        private void InitMainIniPath()
        {
            infObj.mainIniFilePath = infObj.localPath + @"/Main.ini";
        }

        private void InitUserId()
        {
            IniHelper.CheckFile(infObj.mainIniFilePath);
            infObj.userId = IniHelper.GetValue("Login", "Uid", "", infObj.mainIniFilePath);
        }
        
        private void GetProgInfo()
        {
            infObj.localPath = Directory.GetCurrentDirectory();
            infObj.progName = Application.ProductName.ToString();
            infObj.progVer = Application.ProductVersion.ToString();
        }

        private void GetRemoteFlag()
        {
            if (File.Exists(infObj.localPath + @"/Setting.ini") == true)
            {
                infObj.remoteFlag = true;
            }
        }

        private void SetConnInfo()
        {
            if (infObj.remoteFlag)
            {
                infObj.connWG = Global_Const.strConnection_WG_R;
                infObj.connYF = Global_Const.strConnection_YF_R;
                infObj.connMD = Global_Const.strConnection_MD_R;
            }
            else
            {
                infObj.connWG = Global_Const.strConnection_WG;
                infObj.connYF = Global_Const.strConnection_YF;
                infObj.connMD = Global_Const.strConnection_MD;
            }
        }

        public bool GetUpdateHost()
        {
            try
            {
                string sqlstr = "";
                if (infObj.remoteFlag)
                {
                    sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='Remote' AND Valid = 'Y'";
                }
                else
                {
                    sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='Local' AND Valid = 'Y'";
                }
                infObj.updateHost = infObj.sql.SQLselect(infObj.connWG, sqlstr).Rows[0][0].ToString();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool GetNewVersion()
        {
            string Msg;
            if (versionManager.GetNewVersion(infObj.progName, infObj.progVer, out Msg))
            {
                return true;
            }
            else
            {
                if (Msg != null)
                {
                    if (MessageBox.Show(Msg, "错误", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Environment.Exit(0);
                    }
                }
                return false;
            }
        }

        private void SetIniFileUid()
        {
            IniHelper.SetValue("Login", "Uid", infObj.userId, infObj.mainIniFilePath);
        }
        
        public bool FormLogin_GetLogin(string LoginUid, string LoginPwd, out string Msg)//登录
        {
            UserLogin userLogin = new UserLogin();
            UserLogin.UserObjectReturn userObj = new UserLogin.UserObjectReturn();
            userObj.Uid = LoginUid;
            userObj.Pwd = CEncrypt.GetMd5Str(LoginPwd);
            userLogin.Login(userObj);
            if (userObj.Status)
            {
                infObj.userPermList = userObj.Permission;

                Msg = userObj.Msg;

                infObj.userId = userObj.Uid;
                infObj.userName = userObj.Name;
                infObj.userDpt = userObj.Dpt;
                infObj.userGroup = userObj.Group;

                SetIniFileUid();

                return true;
            }
            else
            {
                Msg = userObj.Msg;
                return false;
            }
        }
    }

    public class InfoObject : InfoObjectBase
    {
        private bool _remoteFlag = false;
        private string _localPath = null;
        private string _userPwd = null;
        private bool _testFlag = false;
        private bool _connFlag = false;

        private string _mainIniFilePath = null;
        private List<string> _userPermList = new List<string> { };
        private List<string> _menuItemList = new List<string> { };

        public bool remoteFlag { get { return _remoteFlag; } set { _remoteFlag = value; } }
        public string localPath { get { return _localPath; } set { _localPath = value; } }

        public string userPwd { get { return _userPwd; } set { _userPwd = value; } }

        public bool testFlag { get { return _testFlag; } set { _testFlag = value; } }

        public bool connFlag { get { return _connFlag; } set { _connFlag = value; } }

        public string mainIniFilePath { get { return _mainIniFilePath; } set { _mainIniFilePath = value; } }
        public List<string> userPermList { get { return _userPermList; } set { _userPermList = value; } }
        public List<string> menuItemList { get { return _menuItemList; } set { _menuItemList = value; } }

    }
}
