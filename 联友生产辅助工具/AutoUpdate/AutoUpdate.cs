using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
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
            ZipUnZip zipunzip = new ZipUnZip();
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
            
            if(LastName == "zip")
            {
                label_Log.Text += "\n\r开始解压...";
                Thread.Sleep(500);
                if (zipunzip.UnZip(FilePath, Path + @"\temp") == true)
                {
                    label_Log.Text += "\n\r解压成功...";

                    try
                    {
                        Directory.Delete(Path + @"\temp\", true);
                    }
                    catch(Exception es)
                    {
                        MessageBox.Show(es.ToString(), "");
                    }
                    

                    Thread.Sleep(500);
                    label_Log.Text += "\n\r更新已完成，更新程序即将退出...";
                    Thread.Sleep(1500);
                    StartProcess(Path + "\\" + ProgFileName + ".exe", null);
                    System.Environment.Exit(0);
                }
                else
                {
                    if(MessageBox.Show("解压失败，请手动解压覆盖文件，更新程序即将退出...", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Thread.Sleep(200);
                        System.Environment.Exit(0);
                    }
                }
            }
            else if(LastName == "exe")
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
    public class ZipUnZip
    {
        #region 压缩  

        /// <summary>   
        /// 递归压缩文件夹的内部方法   
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipStream">压缩输出流</param>   
        /// <param name="parentFolderName">此文件夹的上级文件夹</param>   
        /// <returns></returns>   
        private bool ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName)
        {
            bool result = true;
            string[] folders, files;
            ZipEntry ent = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();

            try
            {
                ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
                zipStream.PutNextEntry(ent);
                zipStream.Flush();

                files = Directory.GetFiles(folderToZip);
                foreach (string file in files)
                {
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)));
                    ent.DateTime = DateTime.Now;
                    ent.Size = fs.Length;

                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    ent.Crc = crc.Value;
                    zipStream.PutNextEntry(ent);
                    zipStream.Write(buffer, 0, buffer.Length);
                }

            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }

            folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
                if (!ZipDirectory(folder, zipStream, folderToZip))
                    return false;

            return result;
        }

        /// <summary>   
        /// 压缩文件夹    
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipedFile">压缩文件完整路径</param>   
        /// <param name="password">密码</param>   
        /// <returns>是否压缩成功</returns>   
        public bool ZipDirectory(string folderToZip, string zipedFile, string password)
        {
            bool result = false;
            if (!Directory.Exists(folderToZip))
                return result;

            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
            zipStream.SetLevel(6);
            if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

            result = ZipDirectory(folderToZip, zipStream, "");

            zipStream.Finish();
            zipStream.Close();

            return result;
        }

        /// <summary>   
        /// 压缩文件夹   
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipedFile">压缩文件完整路径</param>   
        /// <returns>是否压缩成功</returns>   
        public bool ZipDirectory(string folderToZip, string zipedFile)
        {
            bool result = ZipDirectory(folderToZip, zipedFile, null);
            return result;
        }

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileToZip">要压缩的文件全名</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>   
        public bool ZipFile(string fileToZip, string zipedFile, string password)
        {
            bool result = true;
            ZipOutputStream zipStream = null;
            FileStream fs = null;
            ZipEntry ent = null;

            if (!File.Exists(fileToZip))
                return false;

            try
            {
                fs = File.OpenRead(fileToZip);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                fs = File.Create(zipedFile);
                zipStream = new ZipOutputStream(fs);
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                ent = new ZipEntry(Path.GetFileName(fileToZip));
                zipStream.PutNextEntry(ent);
                zipStream.SetLevel(6);

                zipStream.Write(buffer, 0, buffer.Length);

            }
            catch
            {
                result = false;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (ent != null)
                {
                    ent = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            GC.Collect();
            GC.Collect(1);

            return result;
        }

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileToZip">要压缩的文件全名</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <returns>压缩结果</returns>   
        public bool ZipFile(string fileToZip, string zipedFile)
        {
            bool result = ZipFile(fileToZip, zipedFile, null);
            return result;
        }

        /// <summary>   
        /// 压缩文件或文件夹   
        /// </summary>   
        /// <param name="fileToZip">要压缩的路径</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>   
        public bool Zip(string fileToZip, string zipedFile, string password)
        {
            bool result = false;
            if (Directory.Exists(fileToZip))
                result = ZipDirectory(fileToZip, zipedFile, password);
            else if (File.Exists(fileToZip))
                result = ZipFile(fileToZip, zipedFile, password);

            return result;
        }

        /// <summary>   
        /// 压缩文件或文件夹   
        /// </summary>   
        /// <param name="fileToZip">要压缩的路径</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <returns>压缩结果</returns>   
        public bool Zip(string fileToZip, string zipedFile)
        {
            bool result = Zip(fileToZip, zipedFile, null);
            return result;

        }

        #endregion

        #region 解压  

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <param name="password">密码</param>   
        /// <returns>解压结果</returns>   
        public bool UnZip(string fileToUnZip, string zipedFolder, string password)
        {
            bool result = true;
            FileStream fs = null;
            ZipInputStream zipStream = null;
            ZipEntry ent = null;
            string fileName;

            if (!File.Exists(fileToUnZip))
                return false;

            if (!Directory.Exists(zipedFolder))
                Directory.CreateDirectory(zipedFolder);

            try
            {
                zipStream = new ZipInputStream(File.OpenRead(fileToUnZip));
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(ent.Name))
                    {
                        fileName = Path.Combine(zipedFolder, ent.Name);
                        fileName = fileName.Replace('/', '\\');//change by Mr.HopeGi   

                        if (fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        fs = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                                fs.Write(data, 0, data.Length);
                            else
                                break;
                        }
                    }
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            return result;
        }

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <returns>解压结果</returns>   
        public bool UnZip(string fileToUnZip, string zipedFolder)
        {
            bool result = UnZip(fileToUnZip, zipedFolder, null);
            return result;
        }

        #endregion
    }
}
