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
        private string tg001 = "";
        private string tg002 = "";
        private string printerName = "";
        private string printFilePath = "";
        public FastReportPreview(string tg001, string tg002, string printerName, string printFilePath)
        {
            InitializeComponent();
            this.tg001 = tg001;
            this.tg002 = tg002;
            this.printerName = printerName;
            this.printFilePath = printFilePath;
        }
    }
}
