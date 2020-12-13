using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastReport;

namespace HarveyZ
{
    public partial class FastReport打印预览 : Form
    {
        private DataSet ds = null;
        private string frx = "";
        private Report report = new Report();

        public FastReport打印预览(DataSet ds, string frx)
        {
            InitializeComponent();
            this.ds = ds;
            //this.ds.Tables[0].TableName = "Dict";
            this.frx = frx;

            PreviewShow();
        }

        private void PreviewShow()
        {
            previewControl1.Buttons = (PreviewButtons.Print | PreviewButtons.Close);
            report.LoadFromString(frx);
            report.RegisterData(ds);
            report.PrintSettings.ShowDialog = true;
            report.Preview = previewControl1;
            
            report.Prepare();
            report.ShowPrepared();
        }
    }
}
