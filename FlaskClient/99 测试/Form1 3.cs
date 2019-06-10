using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Helper.Crypto;
using System.Net;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;
using HarveyZ;
using System.Threading;
using Microsoft.Reporting.WinForms;
using FastReport;

namespace 测试
{
    public partial class Form1 : Form
    {
        string ConnYWGDB = Global_Const.strConnection_Y_WGDB;
        string ConnCOMFORT = Global_Const.strConnection_Y_COMFORT;
        Mssql mssql = new Mssql();

        //System.Timers.Timer t = new System.Timers.Timer(5000); //设置时间间隔为5秒
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Thread t = new Thread(new ThreadStart(GetData));
            //t.IsBackground = true;
            //t.Start();
            //this.reportViewer1.RefreshReport();
        }

        private void GetData()
        {
            var timer = new System.Timers.Timer();
            timer.Interval = 300;
            timer.Enabled = true;
            timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；  
            timer.Start();
            timer.Elapsed += (o, a) =>
            {
                SetData();
                //ShowMessage(string.Format("更新时间：" + DateTime.Now));
            };
        }

        //声明委托
        private delegate void SetDataDelegate();
        private void SetData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetDataDelegate(SetData));
            }
            else
            {
                label1.Text = "a";
                Thread.Sleep(3000);
                label1.Text = string.Format("更新时间：" + DateTime.Now);
            }
        }

        //声明委托
        private delegate void ShowMessageDelegate(string message);
        private void ShowMessage(string message)
        {
            if (this.InvokeRequired)
            {
                ShowMessageDelegate showMessageDelegate = ShowMessage;
                this.Invoke(showMessageDelegate, new object[] { message });
            }
            else
            {
                label1.Text = message;
            }
        }

        //private void TimerInit()
        //{
        //    t.Elapsed += new System.Timers.ElapsedEventHandler(TimesUp);
        //    t.AutoReset = true;
        //    t.Enabled = true;
        //    t.Start();
        //}

        private void TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            MessageBox.Show("aa");
            label1.Text = DateTime.Now.ToLocalTime().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ///----指定报表外部数据源
            DataTable dtHead = new DataTable();
            string sqlstrHead = @"
							SELECT RTRIM(COPTG.TG001)+' '+RTRIM(COPTG.TG002) AS OutId, RTRIM(COPTG.UDF04) AS TuoChe, RTRIM(COPTG.UDF03) AS ChePai, RTRIM(COPMA.MA002) AS KhJc, 
							RTRIM(SUBSTRING(COPTG.TG042,1,4)+'-'+SUBSTRING(COPTG.TG042,5,2)+'-'+SUBSTRING(COPTG.TG042,7,2)) AS GDate, RTRIM(COPTG.UDF05) AS GangKou, RTRIM(COPTG.UDF06) AS PO,
							RTRIM(COPTG.UDF02) AS GuiXing, RTRIM(COPTG.UDF01) AS GuiHao, RTRIM(COPTG.UDF08) AS YuanChanDi, RTRIM(CMSMV.MV002) AS ChuanWu 
							FROM COPTG 
							INNER JOIN COPMA ON COPMA.MA001 = COPTG.TG004 
							INNER JOIN CMSMV ON COPTG.CREATOR = CMSMV.MV001 
							WHERE RTRIM(COPTG.TG001) + '-' + RTRIM(COPTG.TG002) = '2301-18100021' ";
            dtHead = mssql.SQLselect(ConnCOMFORT, sqlstrHead);

            DataTable dtDetail = new DataTable();

            string sqlstrDetail = @"
                            SELECT COPTH.TH003 as No, RTRIM(COPTH.TH005) + CHAR(10) + COPTH.TH006 as NameSpec, COPTH.UDF02 as InId, 
                            ' ' + CONVERT(VARCHAR(10), CONVERT(FLOAT, COPTH.TH008)) + CHAR(10) + '       ' + RTRIM(COPTH.TH009) as NumUnit, 
                            '' as ActNum, COPTH.UDF03 as Describe, ' ' + RTRIM(CMSMC.MC002) + CHAR(10) + RTRIM(COPTH.TH004) as Remark, RTRIM(COPTH.UDF01) as PO  
                            FROM COPTH
                            INNER JOIN CMSMC ON COPTH.TH007 = CMSMC.MC001
                            WHERE RTRIM(TH001) + '-' + RTRIM(TH002) = '2301-18100021'";
            dtDetail = mssql.SQLselect(ConnCOMFORT, sqlstrDetail);

            ///---添加数据源
            ReportDataSource rdsHead = new ReportDataSource();
            rdsHead.Name = "DtHead";
            rdsHead.Value = dtHead;

            ReportDataSource rdsDetail = new ReportDataSource();
            rdsDetail.Name = "DtDetail";
            rdsDetail.Value = dtDetail;

            reportViewer1.LocalReport.DataSources.Add(rdsHead);
            reportViewer1.LocalReport.DataSources.Add(rdsDetail);

            string deviceInfo =
         "<DeviceInfo>" +
         "  <OutputFormat>EMF</OutputFormat>" +
         "  <PageWidth>24.1cm</PageWidth>" +
         "  <PageHeight>13.9cm</PageHeight>" +
         "  <MarginTop>0.1cm</MarginTop>" +
         "  <MarginLeft>0.1cm</MarginLeft>" +
         "  <MarginRight>0.1cm</MarginRight>" +
         "  <MarginBottom>0.1cm</MarginBottom>" +
         "</DeviceInfo>";
            Warning[] warnings;

            reportViewer1.LocalReport.ReportPath = @".\..\..\Report1.rdlc";
            reportViewer1.RefreshReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FastReport.Report report = new FastReport.Report();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
