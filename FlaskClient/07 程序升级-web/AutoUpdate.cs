using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace AutoUpdate
{
    public partial class AutoUpdate : Form
    {
        private string[] args = new string[2];

        private string UpdateUrl = "";
        private string ProgFileName = "";


        public AutoUpdate(string[] arg)
        {
            InitializeComponent();
            args = arg;
            Main();
        }

        private void Main()
        {
            if (args.Length == 2)
            {
                UpdateUrl = args[0];
                ProgFileName = args[1];
                System.Timers.Timer tm = new System.Timers.Timer(1500);
                tm.Enabled = true;
                tm.Elapsed += new ElapsedEventHandler(Work);
                tm.AutoReset = false;
            }
            else
            {
                if (MessageBox.Show("参数传入错误，程序即将退出", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
        }
        
        private void Work(object source, ElapsedEventArgs e)
        {
            WebClient client = new WebClient();
            string URLAddress = UpdateUrl;
            string Path = Directory.GetCurrentDirectory();

            if (Directory.Exists(Path + @"\temp") == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(Path + @"\temp");
            }
            string FilePath = Path + @"\temp\" + System.IO.Path.GetFileName(URLAddress);


            label_Log.Text += "\n\r\n\r程序初始化...";
            Thread.Sleep(1000);

            label_Log.Text += "\n\r开始下载文件...";
            client.DownloadFile(URLAddress, FilePath);
            Thread.Sleep(500);

            label_Log.Text += "\n\r下载完成";
            Thread.Sleep(500);
            
            label_Log.Text += "\n\r数据准备中...";
            Thread.Sleep(500);
            var kk = System.IO.Path.GetFileName(URLAddress).Split('.');
            string LastName = kk[kk.Length - 1];

            //if(LastName == "zip")
            //{
            //    label_Log.Text += "\n\r开始解压...";
            //    Thread.Sleep(500);
            //    if (zipunzip.UnZip(FilePath, Path + @"\temp") == true)
            //    {
            //        label_Log.Text += "\n\r解压成功...";

            //        try
            //        {
            //            Directory.Delete(Path + @"\temp\", true);
            //        }
            //        catch(Exception es)
            //        {
            //            MessageBox.Show(es.ToString(), "");
            //        }


            //        Thread.Sleep(500);
            //        label_Log.Text += "\n\r更新已完成，更新程序即将退出...";
            //        Thread.Sleep(1500);
            //        StartProcess(Path + "\\" + ProgFileName + ".exe", null);
            //        System.Environment.Exit(0);
            //    }
            //    else
            //    {
            //        if(MessageBox.Show("解压失败，请手动解压覆盖文件，更新程序即将退出...", "错误", MessageBoxButtons.OK) == DialogResult.OK)
            //        {
            //            Thread.Sleep(200);
            //            System.Environment.Exit(0);
            //        }
            //    }
            //}
            //else 
            if (LastName == "exe")
            {
                label_Log.Text += "\n\r开始替换程序...";
                Thread.Sleep(500);
                string filePath1 = Path + @"\temp\" + ProgFileName + ".exe";
                string filePath2 = Path + @"\" + ProgFileName + ".exe";
                FileInfo fi1 = new FileInfo(filePath1);
                FileInfo fi2 = new FileInfo(filePath2);
                try
                {

                    //Ensure that the target does not exist.
                    fi2.Delete();

                    //Copy the file.
                    fi1.CopyTo(filePath2, true);
                    label_Log.Text += "\n\r替换程序已完成...";
                    Thread.Sleep(500);
                }
                catch
                {
                    if(MessageBox.Show("程序替换失败，请联系资讯部！\r\n更新程序即将退出！", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Environment.Exit(0);
                    }
                }

                Thread.Sleep(500);
                label_Log.Text += "\n\r更新已完成，更新程序即将退出...";
                Directory.Delete(Path + @"\temp\", true);
                Thread.Sleep(1500);
                StartProcess(Path + "\\" + ProgFileName + ".exe", null);
                Environment.Exit(0);
            }
        }

        public bool StartProcess(string filename, string[] args)
        {
            try
            {
                Process myprocess = new Process();
                ProcessStartInfo startInfo = null;
                if (args != null)
                {
                    string s = "";
                    foreach (string arg in args)
                    {
                        s = s + arg + " ";
                    }
                    s = s.Trim();
                    startInfo = new ProcessStartInfo(filename, s);
                }
                else
                {
                    startInfo = new ProcessStartInfo(filename);
                }
                myprocess.StartInfo = startInfo;

                //通过以下参数可以控制exe的启动方式，具体参照 myprocess.StartInfo.下面的参数，如以无界面方式启动exe等
                myprocess.StartInfo.UseShellExecute = false;
                myprocess.Start();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动应用程序时出错！原因：" + ex.Message);
            }
            return false;
        }
    }
}
