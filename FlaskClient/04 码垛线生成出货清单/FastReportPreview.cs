using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 码垛线生成并打印出货清单
{
    public partial class FastReportPreview : Form
    {
        private FastReport.Report report = new FastReport.Report();
        private string tg001 = "";
        private string tg002 = "";
        private string md_no = "";
        private string printerName = "";
        private string printFilePath = "";
        public FastReportPreview(string tg001, string tg002, string md_no, string printerName, string printFilePath)
        {
            InitializeComponent();
            this.tg001 = tg001;
            this.tg002 = tg002;
            this.md_no = md_no;
            this.printerName = printerName;
            this.printFilePath = printFilePath;

            PreviewShow();
        }

        private void PreviewShow()
        {
            report.Load(printFilePath);
            report.SetParameterValue("@TG001", tg001);
            report.SetParameterValue("@TG002", tg002);
            report.SetParameterValue("@PD_NO", md_no);
            report.PrintSettings.Printer = printerName;
            report.PrintSettings.ShowDialog = true;
            report.Preview = previewControl1;
            
            report.Prepare();
            report.ShowPrepared();
        }
    }
}
