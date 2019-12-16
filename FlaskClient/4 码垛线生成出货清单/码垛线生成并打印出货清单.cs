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
using FastReport.DevComponents;
using System.IO;

namespace 码垛线生成并打印出货清单
{
    public partial class 码垛线生成并打印出货清单 : Form
    {
        #region 局域静态变量
        private Mssql mssql = new Mssql();
        private static string connRobot = Global_Const.strConnection_ROBOT_TEST;
        private static string connComfort = Global_Const.strConnection_COMFORT;

        private static string localPath = "";
        private static string iniFilePath = "";
        private static string iniFileName = "Setting.ini";
        private static string printFilePath = "";
        private static int printReflashTime = 10;
        private static int outReflashTime = 5;
        private static string printerName = "打印机名";
        private static string printerReloadPaperFlag = "Y";
        private static string autoPrintFlag = "N";
        private static string autoOutFlag = "N";

        private static System.Timers.Timer timerOut = null;
        private static System.Timers.Timer timerPrint = null;


        private delegate void TextBoxAppendText(string text);
        private delegate void DelegateCOPTGCreateWork();
        private delegate void DelegateCOPTGPrintWork();
        #endregion

        #region PrintPreview变量
        private static string tg001 = "";
        private static string tg002 = "";
        private static string md_no = "";
        private static string printId = "";
        private static string printFileName = "";

        #endregion

        #region 初始化
        public 码垛线生成并打印出货清单()
        {
            InitializeComponent();
            GetMutilOpen();
            Init();
        }

        private void Init()
        {
            localPath = System.IO.Directory.GetCurrentDirectory();
            iniFilePath = localPath + @"\" + iniFileName;
            printFilePath = localPath + @"\ReportFile\";

            ReadIniFile();
            CheckReportPath();
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            btnPrintPreview.Enabled = false;

            WorkStart();

            GetListData();
        }

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

            string outReflashTimeTmp = IniHelper.GetValue("Setting", "OutReflashTime", "null", iniFilePath);
            if (outReflashTimeTmp == "null")
            {
                IniHelper.SetValue("Setting", "OutReflashTime", outReflashTime.ToString(), iniFilePath);
            }
            else
            {
                try
                {
                    outReflashTime = int.Parse(outReflashTimeTmp);
                }
                catch
                {
                    IniHelper.SetValue("Setting", "OutReflashTime", outReflashTime.ToString(), iniFilePath);
                }
            }

            string printerReloadPaperFlagTmp = IniHelper.GetValue("Setting", "PrinterReloadPaperFlag", "null", iniFilePath);
            if (outReflashTimeTmp == "null")
            {
                IniHelper.SetValue("Setting", "PrinterReloadPaperFlag", printerReloadPaperFlag, iniFilePath);
            }
            else
            {
                printerReloadPaperFlag = printerReloadPaperFlagTmp;
            }

            string printerNameTmp = IniHelper.GetValue("Setting", "PrinterName", "null", iniFilePath);
            if (printerNameTmp == "null")
            {
                IniHelper.SetValue("Setting", "PrinterName", printerName, iniFilePath);
            }
            else
            {
                printerName = printerNameTmp;
            }

            string autoPrintFlagTmp = IniHelper.GetValue("Setting", "AutoPrintFlag", "null", iniFilePath);
            if (autoPrintFlagTmp == "null")
            {
                IniHelper.SetValue("Setting", "AutoPrintFlag", autoPrintFlag, iniFilePath);
            }
            else
            {
                autoPrintFlag = autoPrintFlagTmp;
            }

            string autoOutFlagTmp = IniHelper.GetValue("Setting", "AutoOutFlag", "null", iniFilePath);
            if (autoOutFlagTmp == "null")
            {
                IniHelper.SetValue("Setting", "AutoOutFlag", autoOutFlag, iniFilePath);
            }
            else
            {
                autoOutFlag = autoOutFlagTmp;
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

        #region 窗体UI-打印列表
        private void dgvList_Show(DataTable dt)
        {
            if(dt != null)
            {
                dgvList.DataSource = dt;
                DgvOpt.SetRowColor(dgvList);
                dgvList_SelectLastRow();
                dgvList.ReadOnly = true;
            }
        }

        private void dgvList_SelectLastRow()
        {
            dgvList.CurrentCell = dgvList.Rows[dgvList.RowCount - 1].Cells[0];
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

            GetDetailData();

            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }

        #endregion

        #region 窗体UI-销货单明细
        private void dgvDetail_Show(DataTable dt)
        {
            if (dt != null)
            {
                dgvDetail.DataSource = dt;
                DgvOpt.SetRowColor(dgvDetail);
                //dgvDetail_SelectLastRow();
                dgvDetail.ReadOnly = true;
            }
        }

        private void dgvDetail_SelectLastRow()
        {
            dgvDetail.CurrentCell = dgvDetail.Rows[dgvDetail.RowCount - 1].Cells[0];
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            FastReportPreview previewForm = new FastReportPreview(tg001, tg002, md_no, printerName, @"ReportFile/" + printFileName + @".frx");
            if(previewForm.ShowDialog() == DialogResult.Cancel)
            {
                previewForm.Dispose();
            }
        }
        #endregion

        #region 主逻辑
        private void WorkStart()
        {
            if (autoOutFlag == "Y")
            {
                this.Text += "    -生成销货单已开启";
                SetOutTimer();
            }
            if (autoPrintFlag == "Y")
            {
                this.Text += "    -自动打印已开启";
                SetPrintTimer();
            }
        }

        #region 订单生成并显示打印列表信息
        private void SetOutTimer()
        {
            timerOut = new System.Timers.Timer(outReflashTime * 1000);//实例化Timer类，设置间隔时间为10000毫秒；
            timerOut.Elapsed += new System.Timers.ElapsedEventHandler(COPTGCreateTimerWork);//到达时间的时候执行事件；
            timerOut.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timerOut.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void COPTGCreateTimerWork(object source, System.Timers.ElapsedEventArgs e) //销货单生成定时器溢出执行
        {
            //TextBoxAppendText textBoxAppendText = new TextBoxAppendText(TextBoxAppend);
            //this.textBox1.BeginInvoke(textBoxAppendText, "Create   " + DateTime.Now.ToString("yyyy-mm-dd hh:MM:ss"));
            DelegateCOPTGCreateWork delegateCreateWork = new DelegateCOPTGCreateWork(COPTGCreateWork);
            dgvList.BeginInvoke(delegateCreateWork);
        }

        private void COPTGCreateWork() //销货单生成的主方法，定时器溢出后调用
        {
            COPTGCreateProcWork();
            GetListData();
        }

        private void GetListData() //打印列表显示信息的获取
        {
            string sqlstr = @"SELECT * FROM VPrintList 
                                ORDER BY 打印序号";
            DataTable dt = mssql.SQLselect(connRobot, sqlstr);
            dgvList_Show(dt);
        }

        private void COPTGCreateProcWork() //执行存储过程在99生成销货单
        {
            string sqlstr = @" EXEC ROBOT_TEST.dbo.COPTG_CREATE_WORK_ZYH ";
            mssql.SQLexcute(connRobot, sqlstr);
        }

        private void GetDetailData()
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
            dgvDetail_Show(dt);
        }
        #endregion

        #region 订单打印
        private void SetPrintTimer()
        {
            timerPrint = new System.Timers.Timer(printReflashTime * 1000);//实例化Timer类，设置间隔时间为10000毫秒；
            timerPrint.Elapsed += new System.Timers.ElapsedEventHandler(COPTGPrintTimerWork);//到达时间的时候执行事件；
            timerPrint.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timerPrint.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void COPTGPrintTimerWork(object source, System.Timers.ElapsedEventArgs e) //销货单生成定时器溢出执行
        {
            //DelegateCOPTGPrintWork delegatePrintWork = new DelegateCOPTGPrintWork(COPTGPrintWork);
            COPTGPrintWork();
        }

        private void COPTGPrintWork() //销货单打印的主方法，定时器溢出后调用
        {
            FastReport.Report report = new FastReport.Report();

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
                string printFileNamec = dt.Rows[0][3].ToString();
                string printIdc = dt.Rows[0][4].ToString();

                mssql.SQLexcute(connRobot, string.Format(" UPDATE ROBOT_TEST.dbo.PrintData SET PrintingFlag = 1 WHERE PrintId = {0}", 
                                                            printIdc));


                report.Load(@"ReportFile/" + printFileNamec + @".frx");
                report.SetParameterValue("@TG001", tg001c);
                report.SetParameterValue("@TG002", tg002c);
                report.SetParameterValue("@PD_NO", md_noc);
                report.PrintSettings.Printer = printerName;
                report.PrintSettings.ShowDialog = false;
                //report.Show();
                report.Print();

                mssql.SQLexcute(connRobot, string.Format(" UPDATE ROBOT_TEST.dbo.PrintData SET PrintFlag = 1, PrintDate = getdate(), PrintingFlag = 0 WHERE PrintId = {0}", 
                                                            printIdc));
            }
        }
        #endregion
        
        #region 测试项

        private void TextBoxAppend(string text) //插入字符串到textbox1
        {
            textBox1.AppendText(text + "\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            COPTGPrintWork();
        }
        #endregion

        #endregion
    }
}
