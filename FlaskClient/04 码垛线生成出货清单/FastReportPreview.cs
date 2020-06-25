using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastReport;

namespace 码垛线生成并打印出货清单
{
    public partial class FastReportPreview : Form
    {
        private string tg001 = "";
        private string tg002 = "";
        private string md_no = "";
        private string printerName = "";
        private string frx = "";
        private Report report = new Report();
        public FastReportPreview(string tg001, string tg002, string md_no, string frx)
        {
            InitializeComponent();
            this.tg001 = tg001;
            this.tg002 = tg002;
            this.md_no = md_no;
            this.frx = frx;

            PreviewShow();
        }

        private void PreviewShow()
        {
            previewControl1.Buttons = (PreviewButtons.Print);
            report.LoadFromString(frx);
            report.SetParameterValue("@TG001", tg001);
            report.SetParameterValue("@TG002", tg002);
            report.SetParameterValue("@PD_NO", md_no);
            report.PrintSettings.ShowDialog = true;
            report.Preview = previewControl1;
            
            report.Prepare();
            report.ShowPrepared();
        }
    }
}
