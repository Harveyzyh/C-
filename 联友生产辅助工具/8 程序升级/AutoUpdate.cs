using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using FtpSyn;

namespace AutoUpdate
{
    public partial class AutoUpdate : Form
    {
        private string[] args = new string[2];
        
        private static string ProgFileName = "ERP";
        private static string FileName = "ERP.exe";
        
        private FtpHelper m_ftp = new FtpHelper();

        public AutoUpdate(string[] arg)
        {
            InitializeComponent();
            args = arg;
            Main();
        }

        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        private void Main()
        {
           if (true)
           //if (args.Length == 2)
            {
                //UpdateUrl = args[0];
                //ProgFileName = args[1];
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
            string LocalPath = Directory.GetCurrentDirectory();

            //如果不存在就创建file文件夹
            if (Directory.Exists(LocalPath + @"\temp") == false)
            {
                Directory.CreateDirectory(LocalPath + @"\temp");
            }
            string FilePath = LocalPath + @"\temp\";
            
            label_Log.Text += "\n\r\n\r程序初始化...";
            Thread.Sleep(1000);

            label_Log.Text += "\n\r开始下载文件...";
            m_ftp.Download("ERP.exe", FilePath);
            Thread.Sleep(500);

            label_Log.Text += "\n\r下载完成";
            Thread.Sleep(500);
            
            label_Log.Text += "\n\r数据准备中...";
            Thread.Sleep(500);
            var kk = FileName.Split('.');
            string LastName = kk[kk.Length - 1];
            
            if(LastName == "exe")
            {
                label_Log.Text += "\n\r开始替换程序...";
                Thread.Sleep(500);
                string filePath1 = LocalPath + @"\temp\" + ProgFileName + ".exe";
                string filePath2 = LocalPath + @"\" + ProgFileName + ".exe";
                FileInfo fi1 = new FileInfo(filePath1);
                FileInfo fi2 = new FileInfo(filePath2);
                try
                {
                    //确认改文件已经不存在
                    fi2.Delete();
                    //开始复制文件
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
                Directory.Delete(LocalPath + @"\temp\", true);
                Thread.Sleep(1500);
                StartProcess(LocalPath + "\\" + ProgFileName + ".exe", null);
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

namespace FtpSyn
{
    public class FtpHelper
    {
        //基本设置
        static private string ftpip = "40.73.246.171";    //ftp IP地址
        static private string path = @"ftp://" + ftpip + "/";    //目标路径
        static private string username = "erpupdate";   //ftp用户名
        static private string password = "erpfiledownload";   //ftp密码

        //获取ftp上面的文件和文件夹
        public static string[] GetFileList(string dir)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(username, password);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.UseBinary = true;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    Console.WriteLine(line);
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取ftp上面的文件和文件夹：" + ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="file">ip服务器下的相对路径</param>
        /// <returns>文件大小</returns>
        public static int GetFileSize(string file)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(path + file));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(username, password);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                int dataLength = (int)request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件大小出错：" + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filePath">原路径（绝对路径）包括文件名</param>
        /// <param name="objPath">目标文件夹：服务器下的相对路径 不填为根目录</param>
        public static void FileUpLoad(string filePath, string objPath = "")
        {
            try
            {
                string url = path;
                if (objPath != "")
                    url += objPath + "/";
                try
                {

                    FtpWebRequest reqFTP = null;
                    //待上传的文件 （全路径）
                    try
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        using (FileStream fs = fileInfo.OpenRead())
                        {
                            long length = fs.Length;
                            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + fileInfo.Name));

                            //设置连接到FTP的帐号密码
                            reqFTP.Credentials = new NetworkCredential(username, password);
                            //设置请求完成后是否保持连接
                            reqFTP.KeepAlive = false;
                            //指定执行命令
                            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                            //指定数据传输类型
                            reqFTP.UseBinary = true;

                            using (Stream stream = reqFTP.GetRequestStream())
                            {
                                //设置缓冲大小
                                int BufferLength = 5120;
                                byte[] b = new byte[BufferLength];
                                int i;
                                while ((i = fs.Read(b, 0, BufferLength)) > 0)
                                {
                                    stream.Write(b, 0, i);
                                }
                                Console.WriteLine("上传文件成功");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("上传文件失败错误为" + ex.Message);
                    }
                    finally
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("上传文件失败错误为" + ex.Message);
                }
                finally
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("上传文件失败错误为" + ex.Message);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">服务器下的相对路径 包括文件名</param>
        public static void DeleteFileName(string fileName)
        {
            try
            {
                FileInfo fileInf = new FileInfo(ftpip + "" + fileName);
                string uri = path + fileName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除文件出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 新建目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public static void MakeDir(string dirName)
        {
            try
            {
                string uri = path + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建目录出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 删除目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public static void DelDir(string dirName)
        {
            try
            {
                string uri = path + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除目录出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 从ftp服务器上获得文件夹列表
        /// </summary>
        /// <param name="RequedstPath">服务器下的相对路径</param>
        /// <returns></returns>
        public static List<string> GetDirctory(string RequedstPath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = path + RequedstPath;   //目标路径 path为服务器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(line.LastIndexOf("<DIR>") + 5).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取目录出错：" + ex.Message);
            }
            return strs;
        }

        /// <summary>
        /// 从ftp服务器上获得文件列表
        /// </summary>
        /// <param name="RequedstPath">服务器下的相对路径</param>
        /// <returns></returns>
        public static List<string> GetFile(string RequedstPath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = path + RequedstPath;   //目标路径 path为服务器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(39).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件出错：" + ex.Message);
            }
            return strs;
        }
        //从ftp服务器上下载文件的功能  
        public void Download(string fileName, string downloadPath)
        {
            FtpWebRequest reqFTP;
            try
            {
                string filePath = Application.StartupPath;
                //FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                FileStream outputStream = new FileStream(downloadPath + "\\" + fileName, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
