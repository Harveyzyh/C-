using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 联友生产辅助工具.仓储中心
{
    public partial class PDA_扫描进货单 : Form
    {
        public PDA_扫描进货单()
        {
            InitializeComponent();
            show1();
        }
        private void show1()
        {
            string file = "AutoUpdate.exe";
            System.Diagnostics.FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(file);

            MessageBox.Show(fv.FileVersion);
        }
    }
}
