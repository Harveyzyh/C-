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

        private static System.Timers.Timer timerOut = null;
        private static System.Timers.Timer timerPrint = null;


        private delegate void TextBoxAppendText(string text);
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

            WorkStart();
            tabControl1.SelectedTab = tabControl1.TabPages[0];
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
            int rowIndex = dgvList.CurrentRow.Index;
        }

        #endregion

        #region 窗体UI-销货单明细
        private void dgvDetail_Show(DataTable dt)
        {
            if (dt != null)
            {
                dgvDetail.DataSource = dt;
                DgvOpt.SetRowColor(dgvDetail);
                dgvDetail_SelectLastRow();
                dgvDetail.ReadOnly = true;
            }
        }

        private void dgvDetail_SelectLastRow()
        {
            dgvDetail.CurrentCell = dgvDetail.Rows[dgvDetail.RowCount - 1].Cells[0];
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 逻辑
        private void WorkStart()
        {
            SetOutTimer();
            SetPrintTimer();
        }

        private void SetOutTimer()
        {
            timerOut = new System.Timers.Timer(outReflashTime * 1000);//实例化Timer类，设置间隔时间为10000毫秒；
            timerOut.Elapsed += new System.Timers.ElapsedEventHandler(COPTGCreateWork);//到达时间的时候执行事件；
            timerOut.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timerOut.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void SetPrintTimer()
        {
            timerPrint = new System.Timers.Timer(printReflashTime * 1000);//实例化Timer类，设置间隔时间为10000毫秒；
            timerPrint.Elapsed += new System.Timers.ElapsedEventHandler(COPTGPrintWork);//到达时间的时候执行事件；
            timerPrint.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timerPrint.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }

        private void COPTGCreateWork(object source, System.Timers.ElapsedEventArgs e)
        {
            TextBoxAppendText textBoxAppendText = new TextBoxAppendText(TextBoxAppend);
            this.textBox1.BeginInvoke(textBoxAppendText, "Create   " + DateTime.Now.ToString("yyyy-mm-dd hh:MM:ss"));
        }

        private void COPTGPrintWork(object source, System.Timers.ElapsedEventArgs e)
        {
            TextBoxAppendText textBoxAppendText = new TextBoxAppendText(TextBoxAppend);
            this.textBox1.BeginInvoke(textBoxAppendText, "Print   " + DateTime.Now.ToString("yyyy-mm-dd hh:MM:ss"));
        }

        private void TextBoxAppend(string text)
        {
            textBox1.AppendText(text + "\n");
        }
        #endregion
    }
}
