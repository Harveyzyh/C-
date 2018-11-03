using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinfrmTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.AppendText("计算机名:" + SystemInformation.ComputerName);
            richTextBox1.AppendText(Environment.NewLine);//换行
            richTextBox1.AppendText("计算机名:" + Environment.MachineName);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("操作系统:" + Environment.OSVersion.Platform);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("版本号:" + Environment.OSVersion.VersionString);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("处理器个数:" + Environment.ProcessorCount);
            richTextBox1.AppendText(Environment.NewLine);
            //判断操作系统位数
            if (Environment.Is64BitOperatingSystem)
            {
                richTextBox1.AppendText("操作系统位数:64bit.");
            }
            else
            {
                richTextBox1.AppendText("操作系统位数:32bit.");
            }
            richTextBox1.AppendText(Environment.NewLine);
            //判断网络是否连接
            if (SystemInformation.Network)
            {
                richTextBox1.AppendText("网络连接:已连接");
            }
            else
            {
                richTextBox1.AppendText("网络连接:未连接");
            }
            richTextBox1.AppendText(Environment.NewLine);
            //判断启动模式
            if (SystemInformation.BootMode.ToString() == "Normal")
                richTextBox1.AppendText("启动模式:正常启动");
            if (SystemInformation.BootMode.ToString() == "FailSafe")
                richTextBox1.AppendText("启动模式:安全启动");
            if (SystemInformation.BootMode.ToString() == "FailSafeWithNework")
                richTextBox1.AppendText("启动方式:通过网络服务启动");
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("显示器数量:" + SystemInformation.MonitorCount);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("显示器分辨率:" + SystemInformation.PrimaryMonitorMaximizedWindowSize.Width + " x " + SystemInformation.PrimaryMonitorMaximizedWindowSize.Height);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("主显示器当前分辨率:" + SystemInformation.PrimaryMonitorSize.Width + " x " + SystemInformation.PrimaryMonitorSize.Height);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("鼠标按钮个数:" + SystemInformation.MouseButtons.ToString());//不知道怎么获取出来的是5个按钮
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("系统限定目录:" + Environment.SystemDirectory);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText("系统内存:" + Environment.SystemPageSize.ToString());
            richTextBox1.AppendText(Environment.NewLine);
        }
    }
}