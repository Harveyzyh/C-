using HarveyZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FastReport.Editor;
using FastReport;
using System.IO;

namespace 码垛线_ERP单据生成程序
{
    public partial class 码垛线_ERP单据生成程序 : Form
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
        private Mssql mssql = new Mssql();
        private static string connRobot = Global_Const.strConnection_MD;
        private static string connYF = Global_Const.strConnection_YF;
        public static string connWG = Global_Const.strConnection_WG;

        private static string localPath = "";
        private static string iniFilePath = "";
        private static string iniFileName = "Setting.ini";
        private static string printFilePath = "";
        private static int printReflashTime = 10;
        private static int outXhReflashTime = 5;
        private static int outScrkReflashTime = 5;
        private static string printerName = "打印机名";
        private static string printerReloadPaperFlag = "Y";
        private static string autoPrintFlag = "N";
        private static string autoOutXhFlag = "N";
        private static string autoOutScrkFlag = "N";

        private static System.Timers.Timer timerOutXh = null;
        private static System.Timers.Timer timerOutScrk = null;
        private static System.Timers.Timer timerPrint = null;
        private static System.Timers.Timer timerUpdate = null;

        private static bool workXhFlag = false;
        private static bool workScrkFlag = false;

        private static DataTable componentFileDt = new DataTable();
        
        private delegate void TextBoxAppendText(string text);
        private delegate void DelegateXhCreateWork();
        private delegate void DelegateScrkCreateWork();
        private delegate void DelegatePrintWork();
        private delegate void DelegateUpdateWork();
        #endregion

        #region PrintPreview变量
        private static string tg001 = "";
        private static string tg002 = "";
        private static string md_no = "";
        private static string printId = "";
        private static string printFileName = "";

        #endregion

        #region 初始化
        public 码垛线_ERP单据生成程序()
        {
            InitializeComponent();
            GetMutilOpen();
            Init();
        }

        private void Init()
        {
            //判断是否在debug模式
            #if DEBUG
            this.Text += "   -DEBUG";
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
            dr = componentFileDt.NewRow();
            dr["FileName"] = "FastReport.Bars.dll"; dr["FileVersion"] = "2019.3.5.0";
            componentFileDt.Rows.Add(dr);
            dr = componentFileDt.NewRow();
            dr["FileName"] = "FastReport.dll"; dr["FileVersion"] = "2019.3.5.0";
            componentFileDt.Rows.Add(dr);
            dr = componentFileDt.NewRow();
            dr["FileName"] = "FastReport.Editor.dll"; dr["FileVersion"] = "2019.3.5.0";
            componentFileDt.Rows.Add(dr);
            //dr = componentFileDt.NewRow();
            //dr["FileName"] = "ICSharpCode.SharpZipLib.dll"; dr["FileVersion"] = "0.86.0";
            //componentFileDt.Rows.Add(dr);
            //dr = componentFileDt.NewRow();
            //dr["FileName"] = "Microsoft.CSharp.dll"; dr["FileVersion"] = "4.0.30319.1";
            //componentFileDt.Rows.Add(dr);
            //dr = componentFileDt.NewRow();
            //dr["FileName"] = "Microsoft.Office.Interop.Excel12.dll"; dr["FileVersion"] = "12.0.4518.1014";
            //componentFileDt.Rows.Add(dr);
            //dr = componentFileDt.NewRow();
            //dr["FileName"] = "NPOI.dll"; dr["FileVersion"] = "2.0.0.0";
            //componentFileDt.Rows.Add(dr);
            //dr = componentFileDt.NewRow();
            //dr["FileName"] = "NPOI.OOXML.dll"; dr["FileVersion"] = "2.0.0.0";
            //componentFileDt.Rows.Add(dr);
            //dr = componentFileDt.NewRow();
            //dr["FileName"] = "NPOI.OpenXml4Net.dll"; dr["FileVersion"] = "2.0.0.0";
            //componentFileDt.Rows.Add(dr);
            //dr = componentFileDt.NewRow();
            //dr["FileName"] = "NPOI.OpenXmlFormats.dll"; dr["FileVersion"] = "2.0.0.0";
            //componentFileDt.Rows.Add(dr);
            //dr = componentFileDt.NewRow();
            //dr["FileName"] = "NPOI.xml"; dr["FileVersion"] = "";
            //componentFileDt.Rows.Add(dr);

            FileVersion.JudgeFile(HttpURL + @"/download/", componentFileDt);

            
            //检查软件是否存在更新版本
            if (GetNewVersion())
            {
                UpdateMe.ProgUpdate(ProgName, UpdateUrl);
            }


            //获取初始化文件及打印模板目录
            localPath = Directory.GetCurrentDirectory();
            iniFilePath = localPath + @"\" + iniFileName;

            //初始化检测
            ReadIniFile();
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            btnPrintPreview.Enabled = false;

            //开始工作
            WorkStart();
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
                if (MessageBox.Show("错误", "获取后台服务器配置失败，请联系咨询部！", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
                return null;
            }

        }

        private bool GetNewVersion()
        {
            string Msg;
            if (VersionManeger.GetNewVersion(ProgName, ProgVersion, out Msg))
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

        #region 信息初始化检测
        private void ReadIniFile()
        {
            var pat = Path.GetDirectoryName(iniFilePath);
            if (Directory.Exists(pat) == false)
            {
                Directory.CreateDirectory(pat);
            }
            if (File.Exists(iniFilePath) == false)
            {
                File.Create(iniFilePath).Close();
            }

            string printReflashTimeTmp = IniHelper.GetValue("Setting", "PrintReflashTime", "null", iniFilePath);
            if (printReflashTimeTmp == "null")
            {
                IniHelper.SetValue("Setting", "PrintReflashTime", printReflashTime.ToString(), iniFilePath);
            }
            else
            {
                try
                {
                    printReflashTime = int.Parse(printReflashTimeTmp);
                }
                catch
                {
                    IniHelper.SetValue("Setting", "PrintReflashTime", printReflashTime.ToString(), iniFilePath);
                }
            }

            //销货单生成刷新时间
            string outXhReflashTimeTmp = IniHelper.GetValue("Setting", "OutXhReflashTime", "null", iniFilePath);
            if (outXhReflashTimeTmp == "null")
            {
                IniHelper.SetValue("Setting", "OutXhReflashTime", outXhReflashTime.ToString(), iniFilePath);
            }
            else
            {
                try
                {
                    outXhReflashTime = int.Parse(outXhReflashTimeTmp);
                }
                catch
                {
                    IniHelper.SetValue("Setting", "OutXhReflashTime", outXhReflashTime.ToString(), iniFilePath);
                }
            }

            //生产入库单生成刷新时间
            string outScrkReflashTimeTmp = IniHelper.GetValue("Setting", "OutScrkReflashTime", "null", iniFilePath);
            if (outScrkReflashTimeTmp == "null")
            {
                IniHelper.SetValue("Setting", "OutScrkReflashTime", outScrkReflashTime.ToString(), iniFilePath);
            }
            else
            {
                try
                {
                    outScrkReflashTime = int.Parse(outScrkReflashTimeTmp);
                }
                catch
                {
                    IniHelper.SetValue("Setting", "OutScrkReflashTime", outScrkReflashTime.ToString(), iniFilePath);
                }
            }

            //是否重载纸张
            string printerReloadPaperFlagTmp = IniHelper.GetValue("Setting", "PrinterReloadPaperFlag", "null", iniFilePath);
            if (printerReloadPaperFlagTmp == "null")
            {
                IniHelper.SetValue("Setting", "PrinterReloadPaperFlag", printerReloadPaperFlag, iniFilePath);
            }
            else
            {
                printerReloadPaperFlag = printerReloadPaperFlagTmp;
            }

            //打印机名
            string printerNameTmp = IniHelper.GetValue("Setting", "PrinterName", "null", iniFilePath);
            if (printerNameTmp == "null")
            {
                IniHelper.SetValue("Setting", "PrinterName", printerName, iniFilePath);
            }
            else
            {
                printerName = printerNameTmp;
            }

            //自动打印
            string autoPrintFlagTmp = IniHelper.GetValue("Setting", "AutoPrintFlag", "null", iniFilePath);
            if (autoPrintFlagTmp == "null")
            {
                IniHelper.SetValue("Setting", "AutoPrintFlag", autoPrintFlag, iniFilePath);
            }
            else
            {
                autoPrintFlag = autoPrintFlagTmp;
            }

            //自动生成生产入库单
            string autoOutScrkFlagTmp = IniHelper.GetValue("Setting", "AutoOutScrkFlag", "null", iniFilePath);
            if (autoOutScrkFlagTmp == "null")
            {
                IniHelper.SetValue("Setting", "AutoOutScrkFlag", autoOutScrkFlag, iniFilePath);
            }
            else
            {
                autoOutScrkFlag = autoOutScrkFlagTmp;
            }

            //自动生成销货单
            string autoOutXhFlagTmp = IniHelper.GetValue("Setting", "AutoOutXhFlag", "null", iniFilePath);
            if (autoOutXhFlagTmp == "null")
            {
                IniHelper.SetValue("Setting", "AutoOutXhFlag", autoOutXhFlag, iniFilePath);
            }
            else
            {
                autoOutXhFlag = autoOutXhFlagTmp;
            }
        }

        private void CheckReportPath()
        {
            var pat = Path.GetDirectoryName(printFilePath);
            if (Directory.Exists(pat) == false)
            {
                Directory.CreateDirectory(pat);
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

        #region 窗体UI-打印列表
        private void dgvList_Show(DataTable dt)
        {
            if(dt != null)
            {
                dgvList.DataSource = dt;
                DgvOpt.SetRowBackColor(dgvList);
                DgvOpt.SelectLastRow(dgvList);
                dgvList.ReadOnly = true;
            }
        }

        private void dgvList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //打印列表双击
            //若OutFlag = 1，则显示到tabpage[2]
            //若TG001 与TG002 不为空，则btnPrintPreview enable
            int rowIndex = dgvList.CurrentRow.Index;
            string outFlag = dgvList.Rows[rowIndex].Cells[4].Value.ToString();
            string errFlag = dgvList.Rows[rowIndex].Cells[13].Value.ToString();
            printFileName = dgvList.Rows[rowIndex].Cells[11].Value.ToString();
            printId = dgvList.Rows[rowIndex].Cells[0].Value.ToString();
            tg001 = dgvList.Rows[rowIndex].Cells[9].Value.ToString();
            tg002 = dgvList.Rows[rowIndex].Cells[10].Value.ToString();
            md_no = dgvList.Rows[rowIndex].Cells[2].Value.ToString();


            if (outFlag == "True" && errFlag == "False")
            {
                btnPrintPreview.Enabled = true;
            }
            
            dgvDetail_Show(GetDetailData());

            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }

        private DataTable GetDetailData()
        {
            label3.Text = tg001;
            label4.Text = tg002;
            label7.Text = md_no;
            label8.Text = printId;
            string sqlstr = @"SELECT COPTH.TH003 AS 序号, RTRIM(COPTH.TH004) AS 品号, RTRIM(COPTH.TH005) AS 品名, RTRIM(COPTH.TH006) AS 规格,
                                RTRIM(COPTH.TH014) + '-' + RTRIM(COPTH.TH015) + '-' + RTRIM(COPTH.TH016) AS 生产单号, 
                                CONVERT(VARCHAR(10), CONVERT(FLOAT, COPTH.TH008)) AS 数量, RTRIM(COPTH.TH009) AS 单位, 
                                RTRIM(COPTH.UDF03) AS 描述备注, RTRIM(COPTH.UDF04) AS 保友品名, RTRIM(COPTH.UDF05) AS 配置方案, RTRIM(COPTH.UDF10) AS 产品电商代码, 
                                ' ' + RTRIM(CMSMC.MC002) + CHAR(10)+ CHAR(10) + RTRIM(COPTH.TH004) AS 备注, RTRIM(COPTH.UDF01) AS PO号

                                FROM [192.168.0.99].COMFORT.dbo.COPTH
                                LEFT JOIN [192.168.0.99].COMFORT.dbo.COPTG ON TG001 = TH001 AND TG002 = TH002
                                LEFT JOIN [192.168.0.99].COMFORT.dbo.COPMA ON COPMA.MA001 = COPTG.TG004 
                                LEFT JOIN [192.168.0.99].COMFORT.dbo.CMSMC ON COPTH.TH007 = CMSMC.MC001
                                LEFT JOIN [192.168.0.99].COMFORT.dbo.CMSMV ON COPTG.CREATOR = CMSMV.MV001 
                                WHERE RTRIM(TH001) = '{0}' AND RTRIM(TH002) = '{1}'
                                ORDER BY COPTH.TH003";
            DataTable dt = mssql.SQLselect(connRobot, string.Format(sqlstr, tg001, tg002));
            return dt;
        }
        #endregion

        #region 打印列表明细
        private void GetListData() //打印列表显示信息的获取
        {
            string sqlstr = @"SELECT * FROM VPrintList 
                                ORDER BY 打印序号";
            DataTable dt = mssql.SQLselect(connRobot, sqlstr);
            dgvList_Show(dt);
        }
        #endregion

        #region 销货单明细
        private void dgvDetail_Show(DataTable dt)
        {
            dgvDetail.DataSource = null;
            if (dt != null)
            {
                dgvDetail.DataSource = dt;
                DgvOpt.SetRowBackColor(dgvDetail);
                dgvDetail.ReadOnly = true;
                Dictionary<string, int> dgvDetailColWidthDict = new Dictionary<string, int>();
                dgvDetailColWidthDict.Add("序号", 40);
                dgvDetailColWidthDict.Add("数量", 40);
                dgvDetailColWidthDict.Add("单位", 40);
                dgvDetailColWidthDict.Add("描述备注", 300);
                DgvOpt.SetColWidth(dgvDetail, dgvDetailColWidthDict);
                DgvOpt.SetColHeadMiddleCenter(dgvDetail);
                List<string> dgvDetailColMiddleList = new List<string>();
                dgvDetailColMiddleList.Add("序号");
                dgvDetailColMiddleList.Add("单位");
                dgvDetailColMiddleList.Add("数量");
                DgvOpt.SetColMiddleCenter(dgvDetail, dgvDetailColMiddleList);
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            FastReportPreview previewForm = new FastReportPreview(tg001, tg002, md_no, getfrx(printFileName));
            if(previewForm.ShowDialog() == DialogResult.Cancel)
            {
                previewForm.Dispose();
            }
        }
        #endregion

        #region 定时器主逻辑
        private void WorkStart()
        {
            SetUpdateTimer();

            if (autoOutXhFlag == "Y")
            {
                this.Text += "    -生成销货单已开启";
                SetOutXhTimer();
            }

            if (autoOutScrkFlag == "Y")
            {
                this.Text += "    -生成生产入库单已开启";
                SetOutScrkTimer();
            }

            if (autoPrintFlag == "Y")
            {
                this.Text += "    -自动打印已开启";
                SetPrintTimer();
            }
            GetListData();
        }
        #endregion

        #region 自动更新程序定时
        private void SetUpdateTimer()
        {
            timerUpdate = new System.Timers.Timer(3600 * 1000);//实例化Timer类，设置间隔时间为1000毫秒；
            timerUpdate.Elapsed += new System.Timers.ElapsedEventHandler(SoftwareUpdateTimerWork);//到达时间的时候执行事件；
            timerUpdate.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timerUpdate.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void SoftwareUpdateTimerWork(object source, System.Timers.ElapsedEventArgs e) //销货单生成定时器溢出执行
        {
            DelegateUpdateWork delegateUpdateWork = new DelegateUpdateWork(SoftwareUpdate);
            dgvList.BeginInvoke(delegateUpdateWork);
        }

        private void SoftwareUpdate() //销货单生成的主方法，定时器溢出后调用
        {
            if (!(workScrkFlag || workXhFlag))
            {
                if (GetNewVersion())
                {
                    UpdateMe.ProgUpdate(ProgName, UpdateUrl);
                }
            }
        }
        #endregion

        #region 自动打印
        private void SetPrintTimer()
        {
            timerPrint = new System.Timers.Timer(printReflashTime * 1000);//实例化Timer类，设置间隔时间为1000毫秒；
            timerPrint.Elapsed += new System.Timers.ElapsedEventHandler(PrintTimerWork);//到达时间的时候执行事件；
            timerPrint.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timerPrint.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void PrintTimerWork(object source, System.Timers.ElapsedEventArgs e) //销货单生成定时器溢出执行
        {
            PrintWork();
        }

        //获取打印格式
        private string getfrx(string fileName)
        {
            string sqlstr = @"SELECT CONTENT FROM dbo.WG_PRINT WHERE PRINT_TYPE = '码垛线销货单' AND PRINT_NAME = '{0}' ";
            string frx = mssql.SQLselect(connWG, string.Format(sqlstr, fileName)).Rows[0][0].ToString();
            return frx;
        }

        private void PrintWork() //销货单打印的主方法，定时器溢出后调用
        {
            Report report = new Report();
            string sqlstr = @"SELECT TOP 1 TG001, TG002, MD_No, PrintType, PrintId FROM ROBOT_TEST.dbo.PrintData
                                INNER JOIN ROBOT_TEST.dbo.SplitTypeCode ON TG001 = OutType
                                WHERE STATUSS = 0 AND OutFlag = 1 AND PrintFlag = 0 AND PrintingFlag = 0
                                ORDER BY Create_Date ";

            DataTable dt = mssql.SQLselect(connRobot, sqlstr);

            if (dt != null)
            {
                string tg001c = dt.Rows[0][0].ToString();
                string tg002c = dt.Rows[0][1].ToString();
                string md_noc = dt.Rows[0][2].ToString();
                string printFileName = dt.Rows[0][3].ToString();
                string printIdc = dt.Rows[0][4].ToString();

                mssql.SQLexcute(connRobot, string.Format(" UPDATE ROBOT_TEST.dbo.PrintData SET PrintingFlag = 1 WHERE PrintId = {0}",
                                                            printIdc));


                report.LoadFromString(getfrx(printFileName));
                report.SetParameterValue("@TG001", tg001c);
                report.SetParameterValue("@TG002", tg002c);
                report.SetParameterValue("@PD_NO", md_noc);
                report.PrintSettings.Printer = printerName;
                report.PrintSettings.ShowDialog = false;
                report.Print();

                mssql.SQLexcute(connRobot, string.Format(" UPDATE ROBOT_TEST.dbo.PrintData SET PrintFlag = 1, PrintDate = getdate(), PrintingFlag = 0 WHERE PrintId = {0}",
                                                            printIdc));
            }
        }
        #endregion

        #region 自动生成销货单
        private void SetOutXhTimer()
        {
            timerOutXh = new System.Timers.Timer(outXhReflashTime * 1000);//实例化Timer类，设置间隔时间为10000毫秒；
            timerOutXh.Elapsed += new System.Timers.ElapsedEventHandler(XhCreateTimerWork);//到达时间的时候执行事件；
            timerOutXh.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timerOutXh.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void XhCreateTimerWork(object source, System.Timers.ElapsedEventArgs e) //销货单生成定时器溢出执行
        {
            DelegateXhCreateWork delegateXhCreateWork = new DelegateXhCreateWork(XhCreate);
            dgvList.BeginInvoke(delegateXhCreateWork);
        }

        private void XhCreate() //销货单生成的主方法，定时器溢出后调用
        {
            XhCreateWork();
            GetListData();
        }

        private void XhCreateWork() //执行存储过程在99生成销货单
        {
            string sqlstr = @" EXEC ROBOT_TEST.dbo.COPTG_CREATE_WORK_ZYH ";
            mssql.SQLexcute(connRobot, sqlstr);
        }
        #endregion

        #region 自动生成生产入库单

        private void SetOutScrkTimer()
        {
            timerOutScrk = new System.Timers.Timer(outScrkReflashTime * 1000);//实例化Timer类，设置间隔时间为1000毫秒；
            timerOutScrk.Elapsed += new System.Timers.ElapsedEventHandler(ScrkCreateTimerWork);//到达时间的时候执行事件；
            timerOutScrk.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timerOutScrk.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void ScrkCreateTimerWork(object source, System.Timers.ElapsedEventArgs e) //销货单生成定时器溢出执行
        {
            DelegateScrkCreateWork delegateScrkCreateWork = new DelegateScrkCreateWork(ScrkCreate);
            dgvList.BeginInvoke(delegateScrkCreateWork);
        }

        private void ScrkCreate() //销货单生成的主方法，定时器溢出后调用
        {
            ScrkCreateWork();
            GetListData();
        }

        private void ScrkCreateWork() //执行存储过程在99生成销货单
        {
            //string sqlstr = @" EXEC ROBOT_TEST.dbo.COPTG_CREATE_WORK_ZYH ";
            //mssql.SQLexcute(connRobot, sqlstr);
        }
        #endregion
        
        #region 测试项

        private void button1_Click(object sender, EventArgs e)
        {
            PrintWork();
        }
        #endregion
    }
}
