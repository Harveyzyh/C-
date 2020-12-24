using HarveyZ;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace ERP定时任务
{
    public partial class ERP定时任务 : Form
    {
        #region 公共静态变量
        //软件信息
        public static string ProgVersion = "";
        public static string ProgName = "";
        //服务器URL
        public static string HttpURL = "";
        private string UpdateUrl = "";
        #endregion

        #region 局域静态变量
        private static string connYF = Global_Const.strConnection_YF;
        public static string connWG = Global_Const.strConnection_WG;
        private Mssql mssql = new Mssql();
        private VersionManeger versionManager = new VersionManeger(connWG);
        private Logger logger = new Logger();

        private static int globalStopDate = 20220301;
        private static bool globalStopFlag = false;
        private static System.Timers.Timer mainTimer = null;
        private static System.Timers.Timer updateTimer = null;

        private static DataTable componentFileDt = new DataTable();
        #endregion

        #region 代理变量

        private delegate void DelegateMainWork();
        private delegate void DelegateUpdateWork();
        
        private DelegateUpdateWork delegateUpdateWork = null;

        private InvmbBoxSize boxSize = null;
        private BomList bomList = null;
        private AutoLrpPlan autoLrp = null;

        private ERP_BOMB05 bomb05 = null;
        private ERP_COPAB02 copab02 = null;
        private FixMocta fixMocta = null;
        private FixPurta fixPurta = null;
        private CreateScPlanSnapShot scplanSnapshot = null;
        private DeleteCoptrError deleteCoptr = null;
        #endregion

        #region 初始化
        public ERP定时任务()
        {
            InitializeComponent();
            StopModuleOpen();
            Init();
        }

        private void StopModuleOpen()
        {
            string timeYF = mssql.SQLTime(connYF, 8);
            if (int.Parse(timeYF) > globalStopDate)
            {
                Msg.ShowErr("该软件已停用，所有自动处理已停用！");
                globalStopFlag = true;
            }
        }

        private void Init()
        {
            //判断是否在debug模式
            #if DEBUG
            this.Text += "   -DEBUG";
            testEnableFlag = true;
            #endif

            //从文件详细信息中获取程序名称
            ProgName = Application.ProductName.ToString();
            ProgVersion = Application.ProductVersion.ToString();

            this.Text += "   -Ver." + ProgVersion;
            
            HttpURL = GetHttpURL();

            //自动下载组件
            componentFileDt.Columns.Add("FileName", Type.GetType("System.String"));
            componentFileDt.Columns.Add("FileVersion", Type.GetType("System.String"));
            DataRow dr = componentFileDt.NewRow();
            dr["FileName"] = "AutoUpdate.exe"; dr["FileVersion"] = "1.0.0.0";
            componentFileDt.Rows.Add(dr);

            FileVersion.JudgeFile(HttpURL + @"/download/", componentFileDt);

            
            //检查软件是否存在更新版本
            if (GetNewVersion())
            {
                UpdateMe.ProgUpdate(ProgName, UpdateUrl);
            }

            //开始工作
            WorkStart();
            logAppendText("定时任务初始化已完成!" + "   -Ver " +  ProgVersion);
            logger.Instance.WriteLog("定时任务初始化已完成!" + "   -Ver " + ProgVersion);
        }

        /// <summary>
        /// 日志写入
        /// </summary>
        /// <param name="text"></param>
        private void logAppendText(string text)
        {
            textBoxLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm\t") + text + " \r\n");
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool GetConfig(string configName, string typeName)
        {
            if (configName != null && typeName != null)
            {
                Mssql sql = new Mssql();
                string sqlStr = @"SELECT Valid FROM WG_CONFIG WHERE ConfigName = '{0}' AND Type = '{1}' AND Valid = 'Y' ";
                return sql.SQLexist(connWG, string.Format(sqlStr, configName, typeName));
            }
            else
            {
                return false;
            }
        }

        #region 软件更新检测
        private string GetHttpURL()
        {
            try
            {
                string sqlstr = "SELECT ServerURL FROM WG_CONFIG WHERE ConfigName='APP_Server' AND Type='Local' AND Valid = 'Y'";

                var get = mssql.SQLselect(connWG, sqlstr).Rows[0][0].ToString();
                return get;
            }
            catch
            {
                if (Msg.ShowErr("获取后台服务器配置失败，请联系咨询部！") == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
                return null;
            }

        }
        
        //窗口关闭
        private void ERP定时任务_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        
        /// 软件更新
        private bool GetNewVersion()
        {
            string Msg;
            if (versionManager.GetNewVersion(ProgName, ProgVersion, out Msg))
            {
                UpdateUrl = HttpURL + @"/download/" + ProgName + ".exe";
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
        #endregion
        #endregion

        #region 界面
        private void BtnAutoLrpRun_Click(object sender, EventArgs e)
        {
            if (!autoLrp.workFlag)
            {
                Thread thread = new Thread(new ThreadStart(autoLrp.MainWork));
                logAppendText("AutoLrpPlan: Work Start!");
                thread.Start();
            }
        }
        #endregion

        #region 定时器主逻辑
        private void WorkStart()
        {
            SetUpdateTimer();
            SetMainTimer();
        }
        #endregion

        #region 自动更新程序定时
        private void SetUpdateTimer()
        {
            updateTimer = new System.Timers.Timer(10 * 60 * 1000);//实例化Timer类，设置间隔时间为1000毫秒；
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(SoftwareUpdateTimerWork);//到达时间的时候执行事件；
            updateTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            updateTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void SoftwareUpdateTimerWork(object source, System.Timers.ElapsedEventArgs e) 
        {
            delegateUpdateWork = new DelegateUpdateWork(SoftwareUpdate);
            BeginInvoke(delegateUpdateWork);
        }

        private void SoftwareUpdate() 
        {
            if (!(boxSize.workFlag || autoLrp.workFlag || bomList.workFlag))
            {
                if (GetNewVersion())
                {
                    UpdateMe.ProgUpdate(ProgName, UpdateUrl);
                }
            }
        }
        #endregion

        #region 主定时器
        private void SetMainTimer()
        {
            mainTimer = new System.Timers.Timer(60 * 1000);//实例化Timer类，设置间隔时间为1000毫秒；
            mainTimer.Elapsed += new System.Timers.ElapsedEventHandler(mainTimerWork);//到达时间的时候执行事件；
            mainTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            mainTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            //实例初始化
            boxSize = new InvmbBoxSize(mssql, connYF, logger);
            bomList = new BomList(mssql, connYF, logger);
            autoLrp = new AutoLrpPlan(mssql, connYF, connWG, logger);

            scplanSnapshot = new CreateScPlanSnapShot(mssql, connWG, logger);
            fixMocta = new FixMocta(mssql, connYF, logger);
            fixPurta = new FixPurta(mssql, connYF, logger);
            bomb05 = new ERP_BOMB05(mssql, connYF, logger);
            copab02 = new ERP_COPAB02(mssql, connYF, logger);
            deleteCoptr = new DeleteCoptrError(mssql, connYF, logger);
        }

        private void mainTimerWork(object source, System.Timers.ElapsedEventArgs e)
        {
            int dayOfWeek = int.Parse(DateTime.Now.DayOfWeek.ToString("d"));
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;

            //计算低阶码
            if (hour == 0 && minute == 35)
            {
                if (!bomb05.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(bomb05.MainWork));
                    logAppendText("ERP Job BOMB05: Work Start!");
                    thread.Start();
                }
            }

            //计算层级吗
            if (hour == 1 && minute == 5)
            {
                if (!copab02.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(copab02.MainWork));
                    logAppendText("ERP Job COPAB02: Work Start!");
                    thread.Start();
                }
            }

            //更新工单单头的部门信息为审核者信息
            if (minute % 10 == 0 && hour >= 8 && hour <= 20) 
            {
                if (!fixMocta.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(fixMocta.MainWork));
                    logAppendText("Fix Mocta Department Info: Work Start!");
                    thread.Start();
                }
                else
                {
                    logAppendText("Fix Mocta Department Info: Working!");
                }
            }

            //请购单-异常修复
            if (minute % 20 == 0 && hour >= 8 && hour <= 20) 
            {
                if (!fixPurta.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(fixPurta.MainWork));
                    logAppendText("Fix Purta Info: Work Start!");
                    thread.Start();
                }
                else
                {
                    logAppendText("Fix Purta Info: Working!");
                }
            }
            //删除层级码带X
            if (hour == 0 && minute == 30)
            {
                if (!deleteCoptr.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(deleteCoptr.MainWork));
                    logAppendText("Delete Coptr Error Info: Work Start!");
                    thread.Start();
                }
            }

            //创建排程镜像
            if (hour == 23 && minute == 00)
            {
                if (!scplanSnapshot.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(scplanSnapshot.MainWork));
                    logAppendText("Create Scplan Snapshot: Work Start!");
                    thread.Start();
                }
            }

            //计算品号纸箱尺寸
            if ((dayOfWeek == 1 || dayOfWeek == 3 || dayOfWeek == 5) && hour == 1 && minute == 0)
            {
                if (!boxSize.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(boxSize.MainWork));
                    logAppendText("GetBoxSize: Work Start!");
                    thread.Start();
                }
            }

            //计算标准BOM
            if ((dayOfWeek == 1 || dayOfWeek == 3) && hour == 1 && minute == 10)
            {
                if (!bomList.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(bomList.MainWork));
                    logAppendText("GetBomList: Work Start!");
                    thread.Start();
                }
            }

            //自动跑Lrp计划
            if (minute == 0 && hour >= 8 && hour <= 22)
            {
                if (!autoLrp.workFlag)
                {
                    Thread thread = new Thread(new ThreadStart(autoLrp.MainWork));
                    logAppendText("AutoLrpPlan: Work Start!");
                    thread.Start();
                }
            }
        }
        #endregion
    }
}
