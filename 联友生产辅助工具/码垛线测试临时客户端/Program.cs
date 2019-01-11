using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 码垛线测试临时客户端
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
            Application.Run(new 码垛线测试临时客户端());
        }
    }
}
