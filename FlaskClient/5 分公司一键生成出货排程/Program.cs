using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HarveyZ;

namespace 联友中山分公司生产辅助工具
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new 主界面());
        }
    }
}
