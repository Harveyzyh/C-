using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;
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
            //GetMutilOpen();

            //判断是否在debug模式
            #if DEBUG
            infObj.testFlag = true;
            #endif
            
            if (infObj.testFlag)
            {
                this.Text += "     -DEBUG";
            }

            if (infObj.remoteFlag)
            {
                this.Text += "     -Remote";
            }

            FormLogin_Init(); //配置信息获取

            labelVersion.Text = "Ver: " + infObj.progVer;

            SqlTestDelegate sqlTestDelegateWG = new SqlTestDelegate(SqlTestWG);
            sqlTestDelegateWG.BeginInvoke(infObj.connWG, null, null);

            SqlTestDelegate sqlTestDelegateSW = new SqlTestDelegate(SqlTestSW);
            sqlTestDelegateSW.BeginInvoke(infObj.connSW, null, null);

            textBoxUid.Text = infObj.userId;
            textBoxUid.SelectAll();

            //添加组件列表
            DataRow dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "AutoUpdate.exe"; dr["FileVersion"] = "1.0.0.0";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "ICSharpCode.SharpZipLib.dll";dr["FileVersion"] = "0.86.0";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "Microsoft.CSharp.dll"; dr["FileVersion"] = "4.0.30319.1";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "Microsoft.Office.Interop.Excel12.dll"; dr["FileVersion"] = "12.0.4518.1014";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "NPOI.dll"; dr["FileVersion"] = "2.0.0.0";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "NPOI.OOXML.dll"; dr["FileVersion"] = "2.0.0.0";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "NPOI.OpenXml4Net.dll"; dr["FileVersion"] = "2.0.0.0";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "NPOI.OpenXmlFormats.dll"; dr["FileVersion"] = "2.0.0.0";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "NPOI.xml"; dr["FileVersion"] = "";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "FastReport.Bars.dll"; dr["FileVersion"] = "2019.3.5.0";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "FastReport.dll"; dr["FileVersion"] = "2019.3.5.0";
            infObj.componentFileDt.Rows.Add(dr);
            dr = infObj.componentFileDt.NewRow();
            dr["FileName"] = "FastReport.Editor.dll"; dr["FileVersion"] = "2019.3.5.0";
            infObj.componentFileDt.Rows.Add(dr);

            FileVersion.JudgeFile(infObj.updateHost+ @"/download/", infObj.componentFileDt);

            //更新程序
            if (main.GetNewVersion())
            {
                UpdateMe.ProgUpdate(infObj.progName, infObj.updateHost + @"/download/" + infObj.progName + ".exe");
            }
        }
        
        public static bool StopModuleOpen()
        {
            if (infObj.globalStopFlag)
            {
                Msg.ShowErr("该功能已于" + Normal.ConvertDate(infObj.globalStopDate) + "停用");
                return false;
            }
            else
            {
                return true;
            }
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
        private void SqlTestWG(string connStr)
        {
            infObj.connWgFlag = infObj.sql.SQLlinkTest(connStr);
        }

        private void SqlTestSW(string connStr)
        {
            infObj.connSwFlag = infObj.sql.SQLlinkTest(connStr);
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
    }

    public class Main
    {
        private InfoObject infObj = null;
        VersionManeger versionManager = null;

        public Main(InfoObject _infObj)
        {
            infObj = _infObj;
            infObj.sql = new Mssql();

            GetProgInfo();
            GetRemoteFlag();
            SetConnInfo();
            SetModuleOpenFlag();

            InitMainIniPath();
            InitUserId();

            infObj.systemType = "ERP";

            versionManager = new VersionManeger(infObj.connWG);
            infObj.userPermission = new UserPermission(infObj.connWG, infObj.systemType);
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
                infObj.connSW = Global_Const.strConnection_SW_R;
            }
            else
            {
                infObj.connWG = Global_Const.strConnection_WG;
                infObj.connYF = Global_Const.strConnection_YF;
                infObj.connMD = Global_Const.strConnection_MD;
                infObj.connSW = Global_Const.strConnection_SW;
            }
        }

        /// <summary>
        /// 设置标志位
        /// 当服务器日期大于Object中设定的日期，设定StopFlag为true
        /// </summary>
        private void SetModuleOpenFlag()
        {
            string timeYF = infObj.sql.SQLTime(infObj.connYF, 8);
            string timeMD = infObj.sql.SQLTime(infObj.connMD, 8);
            if (int.Parse(timeYF) > int.Parse(infObj.globalStopDate) || int.Parse(timeMD) > int.Parse(infObj.globalStopDate))
            {
                infObj.globalStopFlag = true;
            }
            else
            {
                infObj.globalStopFlag = false;
            }
        }

        /// <summary>
        /// 获取服务器的更新URL
        /// </summary>
        /// <returns>True,False</returns>
        public bool GetUpdateHost()
        {
            try
            {
                string slqStr = "";
                if (infObj.remoteFlag)
                {
                    slqStr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='Remote' AND Valid = 'Y'";
                }
                else
                {
                    slqStr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='Local' AND Valid = 'Y'";
                }
                infObj.updateHost = infObj.sql.SQLselect(infObj.connWG, slqStr).Rows[0][0].ToString();
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
            infObj.userLogin = new ERP_UserLogin(FormLogin.infObj.connWG, FormLogin.infObj.connYF);
            ERP_UserLogin.UserObjectReturn userObj = new ERP_UserLogin.UserObjectReturn();
            userObj.Uid = LoginUid;
            userObj.Pwd = LoginPwd;
            infObj.userLogin.Login(userObj);
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
        private bool _connWgFlag = false;
        private bool _connSwFlag = false;

        private string _systemType = null;

        private string _mainIniFilePath = null;
        private List<string> _userPermList = new List<string> { };
        private List<string> _menuItemList = new List<string> { };

        private DataTable _componentFileDt = new DataTable();

        private ERP_UserLogin _userLogin = null;
        private UserPermission _userPermission = null;

        /// <summary>
        /// Class InfoObject 初始化
        /// </summary>
        public InfoObject()
        {
            _componentFileDt.Columns.Add("FileName", Type.GetType("System.String"));
            _componentFileDt.Columns.Add("FileVersion", Type.GetType("System.String"));
        }

        /// <summary>
        /// 是否为远端登录flag
        /// </summary>
        public bool remoteFlag { get { return _remoteFlag; } set { _remoteFlag = value; } }

        /// <summary>
        /// 应用程序所处的绝对路径
        /// </summary>
        public string localPath { get { return _localPath; } set { _localPath = value; } }

        public string userPwd { get { return _userPwd; } set { _userPwd = value; } }

        public bool testFlag { get { return _testFlag; } set { _testFlag = value; } }

        public bool connWgFlag { get { return _connWgFlag; } set { _connWgFlag = value; } }

        public bool connSwFlag { get { return _connSwFlag; } set { _connSwFlag = value; } }

        /// <summary>
        /// 软件所处于的模式，区分ERP和Client
        /// </summary>
        public string systemType { get { return _systemType; } set { _systemType = value; } }

        /// <summary>
        /// 初始化ini文件所在目录，绝对路径
        /// </summary>
        public string mainIniFilePath { get { return _mainIniFilePath; } set { _mainIniFilePath = value; } }

        /// <summary>
        /// 用户拥有的权限
        /// </summary>
        public List<string> userPermList { get { return _userPermList; } set { _userPermList = value; } }

        /// <summary>
        /// 菜单栏的所有项目
        /// </summary>
        public List<string> menuItemList { get { return _menuItemList; } set { _menuItemList = value; } }

        /// <summary>
        /// 应用程序必须组件的存储dt
        /// </summary>
        public DataTable componentFileDt { get { return _componentFileDt; } set { _componentFileDt = value; } }

        /// <summary>
        /// 用户登录的模块
        /// </summary>
        public ERP_UserLogin userLogin { get { return _userLogin; } set { _userLogin = value; } }

        /// <summary>
        /// 用户权限模块
        /// </summary>
        public UserPermission userPermission { get { return _userPermission; } set { _userPermission = value; } }

    }
}
