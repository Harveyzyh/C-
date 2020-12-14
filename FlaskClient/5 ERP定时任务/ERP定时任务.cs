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

        private static int globalStopDate = 20300301;
        private static bool globalStopFlag = false;

        private static System.Timers.Timer testTimer = null;
        private static System.Timers.Timer mainTimer = null;
        private static System.Timers.Timer updateTimer = null;

        private static DataTable componentFileDt = new DataTable();
        #endregion

        #region 代理变量
        private bool testEnableFlag = false;

        private delegate void DelegateMainWork();
        private delegate void DelegateUpdateWork();
        
        private DelegateUpdateWork delegateUpdateWork = null;

        private DelegateMainWork delegateTestMainWork = null;
        private DelegateMainWork delegateBOMB05MainWork = null;
        private DelegateMainWork delegateCOPAB02MainWork = null;
        private DelegateMainWork delegateUpdateMoctaMainWork = null;
        private DelegateMainWork delegateUpdatePurtaMainWork = null;
        private DelegateMainWork delegateDeleteCoptrErrorMainWork = null;

        private DelegateMainWork delegateBoxSizeMainWork = null;
        private DelegateMainWork delegateBomListMainWork = null;
        private DelegateMainWork delegateAutoLrpMainWork = null;

        private InvmbBoxSize boxSize = null;
        private BomList bomList = null;
        private AutoLrpPlan autoLrp = null;
        #endregion

        #region 初始化
        public ERP定时任务()
        {
            InitializeComponent();
            GetMutilOpen();
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
            logAppendText("定时任务初始化已完成!");
        }

        private void logAppendText(string text)
        {
            textBoxLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm\t") + text + " \r\n");
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
        #endregion

        #region 界面
        private void BtnAutoLrpRun_Click(object sender, EventArgs e)
        {
            textBoxLog.BeginInvoke(delegateAutoLrpMainWork);
        }
        #endregion

        #region 定时器主逻辑
        private void WorkStart()
        {
            //if(testEnableFlag) SetTestTimer();
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

            //代理初始化
            delegateBOMB05MainWork = new DelegateMainWork(BOMB05MainWork);
            delegateCOPAB02MainWork = new DelegateMainWork(COPAB02MainWork);
            delegateUpdateMoctaMainWork = new DelegateMainWork(UpdateMoctaMainWork);
            delegateUpdatePurtaMainWork = new DelegateMainWork(UpdatePurtaMainWork);
            delegateDeleteCoptrErrorMainWork = new DelegateMainWork(DeleteCoptrErrorMainWork);

            delegateBoxSizeMainWork = new DelegateMainWork(BoxSizeMainWork);
            delegateBomListMainWork = new DelegateMainWork(BomListMainWork);
            delegateAutoLrpMainWork = new DelegateMainWork(AutoLrpMainWork);

            //实例初始化
            boxSize = new InvmbBoxSize(mssql, connYF, logger);
            bomList = new BomList(mssql, connYF, logger);
            autoLrp = new AutoLrpPlan(mssql, connYF, connWG, logger);
        }

        private void mainTimerWork(object source, System.Timers.ElapsedEventArgs e)
        {
            int dayOfWeek = int.Parse(DateTime.Now.DayOfWeek.ToString("d"));
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;

            //计算层级吗
            if (hour == 1 && minute == 5) textBoxLog.BeginInvoke(delegateBOMB05MainWork);
            //计算层级吗
            if (hour == 2 && minute == 5) textBoxLog.BeginInvoke(delegateCOPAB02MainWork);
            //更新工单单头的部门信息为审核者信息
            if (minute % 10 == 0 && hour >= 8 && hour <= 20) textBoxLog.BeginInvoke(delegateUpdateMoctaMainWork);
            //请购单-异常修复
            if (minute % 10 == 0  && hour >= 8 && hour <= 20) textBoxLog.BeginInvoke(delegateUpdatePurtaMainWork);
            //删除层级码带X -- 停用
            //if (hour == 1 && minute == 0) textBoxLog.BeginInvoke(delegateDeleteCoptrErrorMainWork);

            //计算品号纸箱尺寸
            if ((dayOfWeek == 1 || dayOfWeek == 3 || dayOfWeek == 5) && hour == 1 && minute == 0) textBoxLog.BeginInvoke(delegateBoxSizeMainWork);
            //计算标准BOM
            if ((dayOfWeek == 1 || dayOfWeek == 3) && hour == 1 && minute == 10) BeginInvoke(delegateBomListMainWork);
            //自动跑Lrp计划
            if (minute == 0) BeginInvoke(delegateAutoLrpMainWork);
        }

        private void BOMB05MainWork()
        {
            logAppendText("Add ERP Job BOMB05: Work Start!");
            logger.Instance.WriteLog("Add ERP Job BOMB05 Work Start!");
            string sqlStr = @"
                            DECLARE @NYR VARCHAR(20)
                            DECLARE @XH INT
                            DECLARE @JOBID VARCHAR(20)
                            DECLARE @SUBID VARCHAR(20)
                            SELECT @NYR = CONVERT(VARCHAR(20), GETDATE(), 112)

                            IF NOT EXISTS(SELECT * FROM [DSCSYS].[dbo].[JOBQUEUE] WHERE JOBNAME = 'BOMB05' AND STATUS IN ('P', 'N'))
                            BEGIN
	                            IF EXISTS(SELECT * FROM [DSCSYS].[dbo].[JOBQUEUE] WHERE SUBSTRING(JOBID, 1, 8) = @NYR )
	                            BEGIN
		                            SELECT  @XH = CONVERT(INT, SUBSTRING(MAX(JOBID), 9, 6)) 
			                            FROM [DSCSYS].[dbo].[JOBQUEUE] WHERE SUBSTRING(JOBID, 1, 8) = @NYR
	                            END
	                            ELSE 
	                            BEGIN
		                            SET @XH = 0
	                            END

	                            SELECT @JOBID = @NYR + RIGHT('000000' + CAST(@XH + 1 AS VARCHAR(6)), 6)
	                            SELECT @SUBID = @NYR + '0001'

	                            INSERT INTO [DSCSYS].[dbo].[JOBQUEUE] ([JOBID], [SUBID], [COMPANYID], [USERID], [USEDALIAS], [JOBNAME], [EXTNAME], [COMPROGID], [JOBOPTION], [GENTYPE], [GENSTATUS], 
	                            [PRIORITY], [STATUS], [PROGRESS], [DTREQUEST], [DTRECEIVE], [DTSCHEDULE], [DTSTART], [DTFINISH], [RESULT], [STYLE], [PROCESSER], [FLAG], [NOTIFY]) 
	                            VALUES (@JOBID, @SUBID, 'COMFORT', 'Robot', 'COMFORT', 'BOMB05', '', 'BOMB05S.Class1', 
	                            0x44532056415249414E54202030313030180100000C2000000100000000000000010000000C2000000100000001000000030000000C20000001000000000000000100000008000000060000000990E9623B4EF64EC154F753080000000400000073007000300031000C20000001000000000000000100000008000000070000001F7510624C006F006700876563680800000006000000630068006B004C006F0067000C20000001000000000000000100000008000000040000005C4F1A4EE5651F67080000000B000000650064005000720069006E00740044006100740065000C2000000100000001000000030000000C200000010000000000000002000000080000000100000004000800000000000000080000000000000008000000010000004E000800000000000000, 
	                            1, 1, 3, 'N', NULL, GETDATE(), GETDATE(), GETDATE(), NULL, NULL, NULL, 'B', '', 1, '')
                            END";
            mssql.SQLexcute(connYF, sqlStr);
        }

        private void COPAB02MainWork()
        {
            logAppendText("Add ERP Job COPAB02: Work Start!");
            logger.Instance.WriteLog("Add ERP Job COPAB02 Work Start!");
            string sqlStr = @"
                            DECLARE @NYR VARCHAR(20)
                            DECLARE @XH INT
                            DECLARE @JOBID VARCHAR(20)
                            DECLARE @SUBID VARCHAR(20)
                            SELECT @NYR = CONVERT(VARCHAR(20), GETDATE(), 112)

                            IF NOT EXISTS(SELECT * FROM [DSCSYS].[dbo].[JOBQUEUE] WHERE JOBNAME = 'COPAB02' AND STATUS IN ('P', 'N'))
                            BEGIN
	                            IF EXISTS(SELECT * FROM DSCSYS..JOBQUEUE WHERE SUBSTRING(JOBID, 1, 8) = @NYR )
	                            BEGIN 
		                            SELECT  @XH = CONVERT(INT, SUBSTRING(MAX(JOBID), 9, 6)) FROM DSCSYS..JOBQUEUE WHERE SUBSTRING(JOBID, 1, 8) = @NYR 
	                            END
	                            ELSE 
	                            BEGIN
		                            SET @XH = 0
	                            END

	                            SELECT @JOBID = @NYR + RIGHT('000000' + CAST(@XH + 1 AS VARCHAR(6)), 6)
	                            SELECT @SUBID = @NYR + '0001'

	                            INSERT INTO [DSCSYS]..[JOBQUEUE] ([JOBID], [SUBID], [COMPANYID], [USERID], [USEDALIAS], [JOBNAME], [EXTNAME], [COMPROGID], [JOBOPTION], [GENTYPE], [GENSTATUS], 
	                            [PRIORITY], [STATUS], [PROGRESS], [DTREQUEST], [DTRECEIVE], [DTSCHEDULE], [DTSTART], [DTFINISH], [RESULT], [STYLE], [PROCESSER], [FLAG], [NOTIFY]) 
	                            VALUES (@JOBID, @SUBID, 'COMFORT', 'Robot', 'COMFORT', 'COPAB02', '', 'COPAB02S.Class1', 
	                            0x44532056415249414E54202030313030640100000C2000000100000000000000010000000C2000000100000001000000040000000C20000001000000000000000100000008000000040000000990E962C154F753080000000400000073007000300031000C20000001000000000000000100000008000000070000000990E96242004F004D00E5651F67080000000400000073007000300032000C20000001000000000000000100000008000000070000001F7510624C006F006700876563680800000006000000630068006B004C006F0067000C20000001000000000000000100000008000000040000005C4F1A4EE5651F67080000000B000000650064005000720069006E00740044006100740065000C2000000100000001000000040000000C200000010000000000000000000000080000000100000005000C2000000100000000000000020000000800000001000000040008000000000000000800000000000000080000000100000059000800000000000000, 
	                            1, 1, 3, 'N', NULL, GETDATE(), GETDATE(), GETDATE(), NULL, NULL, NULL, 'B', '', 1, '');
                            END";
            mssql.SQLexcute(connYF, sqlStr);
        }

        private void UpdateMoctaMainWork()
        {
            logAppendText("Fix Mocta Department Info: Work Start!");
            logger.Instance.WriteLog("Fix Mocta Department Info Work Start!");
            string sqlStr = @"
                            UPDATE COMFORT.dbo.MOCTA 
                            SET TA064 = MV004, TA021 = MD001 

                            FROM COMFORT.dbo.MOCTA 

                            INNER JOIN COMFORT.dbo.CMSMV ON MV001 = TA041 

                            INNER JOIN COMFORT.dbo.CMSMD ON MD015 = MV004 AND MD001 IN ('1' ,'6' , '7') 

                            WHERE 1=1 
                            AND TA013 = 'Y'  

                            AND TA011 NOT IN ('Y', 'y') 

                            AND (TA064 != MV004 OR MD001 != TA021) 
                            AND (MD001 IS NOT NULL OR RTRIM(MD001) != '') 
                            AND (MV004 IS NOT NULL OR RTRIM(MV004) != '') ";
            mssql.SQLexcute(connYF, sqlStr);
        }

        private void UpdatePurtaMainWork()
        {
            logAppendText("Fix Purta Info: Work Start!");
            logger.Instance.WriteLog("Fix Purta Info Work Start!");
            string sqlStr = @"EXEC dbo.P_PURTA_FIX_WORK ";
            mssql.SQLexcute(connYF, sqlStr);
        }

        private void DeleteCoptrErrorMainWork()
        {
            logAppendText("Delete Coptr Error Info: Work Start!");
            logger.Instance.WriteLog("Delete Coptr Error Info Work Start!");
            string sqlStr = @"IF EXISTS (SELECT* FROM DSCSYS.dbo.JOBQUEUE WHERE JOBNAME IN ('COPAB02', 'COPAB01') AND STATUS IN ('P', 'N'))
                                BEGIN
	                                DELETE FROM [COMFORT].[dbo].[COPTR] WHERE TR003 like 'X%' 
	                                AND TR001 NOT LIKE '6%'
	                                AND TR001 NOT LIKE '7%'
	                                AND TR001 NOT LIKE '8%'
	                                AND TR001 NOT LIKE '9%'


	                                UPDATE COPTR SET TR003 = SUBSTRING(TR003, 2, LEN(TR003)) 
	                                WHERE (TR001 LIKE '6%' 
	                                OR TR001 LIKE '7%'
	                                OR TR001 LIKE '8%'
	                                OR TR001 LIKE '9%')
	                                AND TR003 LIKE 'X%'

                                END ";
            mssql.SQLexcute(connYF, sqlStr);
        }

        private void BoxSizeMainWork()
        {
            if (!boxSize.workFlag)
            {
                Thread thread = new Thread(new ThreadStart(boxSize.MainWork));
                logAppendText("GetBoxSize: Work Start!");
                thread.Start();
            }
        }

        private void AutoLrpMainWork()
        {
            if (!autoLrp.workFlag)
            {
                Thread thread = new Thread(new ThreadStart(autoLrp.MainWork));
                logAppendText("AutoLrpPlan: Work Start!");
                thread.Start();
            }
        }

        private void BomListMainWork()
        {
            if (!bomList.workFlag)
            {
                Thread thread = new Thread(new ThreadStart(bomList.MainWork));
                logAppendText("GetBomList: Work Start!");
                thread.Start();
            }
        }
        #endregion
    }
}
