using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;

namespace HarveyZ
{
    public class Msg
    {
        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="msg">信息内容</param>
        public static void Show(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButtons.OKCancel);
        }

        public static void Show(string msg, string title)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OKCancel);
        }

        public static void Show(string msg, string title, MessageBoxButtons btn)
        {
            MessageBox.Show(msg, title, btn);
        }

        public static void Show(string msg, string title, MessageBoxButtons btn, MessageBoxIcon icon)
        {
            MessageBox.Show(msg, title, btn, icon);
        }

        public static void ShowErr(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButtons.OKCancel);
        }

        public static void ShowErr(string msg, MessageBoxButtons btn)
        {
            MessageBox.Show(msg, "错误", btn);
        }

        public static void ShowErr(string msg, MessageBoxButtons btn, MessageBoxIcon icon)
        {
            MessageBox.Show(msg, "错误", btn, icon);
        }
    }

    public class IPInfo
    {
        public static string GetIpAddress()
        {
            string addr = "";
            string ip = "."; //用于过滤ip
            int index = 0;
            IPHostEntry localhost = Dns.GetHostEntry(Dns.GetHostName());    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址

            foreach(IPAddress localaddr in localhost.AddressList)
            {
                index = localaddr.ToString().IndexOf(ip);
                if (index >= 0)
                {
                    addr = localaddr.ToString();
                }
            }

            return addr;
        }
    }

    public class CEncrypt
    {
        public static string GetMd5Str(string str)     //取得字符串str被MD5加密算法加密后的字符串
        {
            string resultstr = "";
            MD5 myMD5 = MD5.Create();
            Byte[] mybyte;
            mybyte = myMD5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            for (int itmp = 0; itmp < mybyte.Length; itmp++)
            {
                resultstr += mybyte[itmp].ToString("X");
            }
            return resultstr;
        }
    }
    
    public class UpdateMe
    {
        #region ftp文件下载更新
        string _conStr;
        public UpdateMe(string conStr)
        {
            _conStr = conStr;
        }

        public void Update()
        {
            string prodName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.Trim();
            string prodVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string prodPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            Process p = Process.GetCurrentProcess();
            string pid = p.Id.ToString();
            bool _needUpdateFlag = false;

            using (SqlConnection conSqlSvr = new SqlConnection(_conStr))
            {
                conSqlSvr.Open();
                string sqlVersion = "select ProgVersion from daruenfa.dbo.ProgInf Where (ProgName='" + prodName + "' or ProgName='" + prodName + ".exe')";
                DataTable dt = null;// CConDB.GetDataFromSqlSvr(sqlVersion, conSqlSvr);
                if (dt == null)
                {
                    _needUpdateFlag = false;
                }
                else if (dt.Rows[0]["ProgVersion"].ToString().Trim() == prodVersion.Trim())
                {
                    _needUpdateFlag = false;
                }
                else
                {
                    _needUpdateFlag = true;
                }
                conSqlSvr.Close();
                conSqlSvr.Dispose();
            }
            if (_needUpdateFlag)
            {
                Process.Start(prodPath + "\\" + "程序更新.exe", "\"" + prodName.Trim() + "\" \"" + prodVersion.Trim() + "\" \"" + prodPath + "\" \"" + _conStr + "\" \"" + pid + "\"");
            }
        }
        #endregion


        #region http文件下载更新
        private static bool StartProcess(string filename, string[] args)
        {
            try
            {
                string s = "";
                foreach (string arg in args)
                {
                    s = s + arg + " ";
                }
                s = s.Trim();
                Process myprocess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(filename, s);
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

        public static void ProgUpdate(string ProgName, string UpdateUrl)
        {
            string Path = Directory.GetCurrentDirectory();
            try
            {
                string[] arg = new string[2];
                arg[0] = UpdateUrl;
                arg[1] = ProgName;
                StartProcess(Path + "\\" + "AutoUpdate.exe", arg);
                Environment.Exit(0);
            }
            catch (Win32Exception)
            {
                if (MessageBox.Show("找不到更新软件，程序即将退出!", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }

        }
        #endregion
    }

    public class HttpDownloadFile
    {
        public static void Download(string url, string fileName)
        {
            WebClient client = new WebClient();
            string URLAddress = url;
            string path = Directory.GetCurrentDirectory();

            if (Directory.Exists(path + @"\temp") == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path + @"\temp");
            }
            string FilePath = path + @"\temp\" + Path.GetFileName(URLAddress);

            //下载文件
            client.DownloadFile(URLAddress, FilePath);

            var kk = System.IO.Path.GetFileName(URLAddress).Split('.');
            string LastName = kk[kk.Length - 1];
            string filePath1 = path + @"\temp\" + fileName;
            string filePath2 = path + @"\" + fileName;
            FileInfo fi1 = new FileInfo(filePath1);
            FileInfo fi2 = new FileInfo(filePath2);
            try
            {

                //Ensure that the target does not exist.
                fi2.Delete();

                //Copy the file.
                fi1.CopyTo(filePath2, true);
            }
            catch
            {
                if (MessageBox.Show("程序替换失败，请联系资讯部！\r\n更新程序即将退出！", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
    
    public class FileVersion
    {
        public static void JudgeFile(string url, DataTable fileDt)
        {
            string path = Directory.GetCurrentDirectory();
            if (Directory.Exists(path + @"\temp") == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path + @"\temp");
            }

            foreach (DataRow dr in fileDt.Rows)
            {
                GetFile(url, path, dr["FileName"].ToString(), dr["FileVersion"].ToString());
            }
            Directory.Delete(path + @"\temp\", true);
        }

        private static void GetFile(string url, string path, string fileName, string fileVersion)
        {
            if (File.Exists(path + @"\" + fileName))
            {
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(path + @"\" + fileName);
                //产品版本不一致
                if (info.ProductVersion != fileVersion)
                {
                    HttpDownloadFile.Download(url + fileName, fileName);
                }
            }
            else
            {
                HttpDownloadFile.Download(url + fileName, fileName);
            }
        }
    }
    
    public class FileOpt
    {
        public static bool OpenFile(string filter, out string filePath, out string fileName)
        {
            filePath = "";
            fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = filter;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = Path.GetDirectoryName(openFileDialog.FileName);
                fileName = Path.GetFileName(openFileDialog.FileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool OpenFile(string filter, out string fullPath)
        {
            fullPath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = filter;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fullPath = openFileDialog.FileName;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool OpenFile(string filter, string initDir, out string filePath, out string fileName)
        {
            filePath = "";
            fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = initDir;
            openFileDialog.Filter = filter;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = Path.GetDirectoryName(openFileDialog.FileName);
                fileName = Path.GetFileName(openFileDialog.FileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SaveFile(string filter, out string filePath, out string fileName)
        {
            filePath = "";
            fileName = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FilterIndex = 1;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = Path.GetDirectoryName(saveFileDialog.FileName);
                fileName = Path.GetFileName(saveFileDialog.FileName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ReadFileGetContent(string filePath)
        {
            string rtnStr = "";
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(fs);
                rtnStr = reader.ReadToEnd();
            }
            catch (Exception)
            {
                rtnStr = "";
            }
            return rtnStr;
        }
    }
    
    public class DgvOpt
    {
        #region 行背景颜色
        /// <summary>
        /// 设置Dgv中单双行背景颜色不一致
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        public static void SetRowBackColor(DataGridView dgv)
        {
            //行颜色
            dgv.RowsDefaultCellStyle.BackColor = Color.Bisque;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
        }
        #endregion

        #region 列只读
        /// <summary>
        /// 设置Dgv的列宽度，以单个列序号传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="col">单个列序号</param>
        public static void SetColReadonly(DataGridView dgv, int col)
        {
            if (dgv != null)
            {
                dgv.Columns[col].ReadOnly = true;
            }
        }

        /// <summary>
        /// 设置Dgv列只读，以列序号List传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="colList">列序号List</param>
        public static void SetColReadonly(DataGridView dgv, List<int> colList)
        {
            //获取所有列序号
            List<int> dgvList = new List<int>();
            for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
            {
                dgvList.Add(colIndex);
            }

            if (dgv != null)
            {
                foreach (int col in colList)
                {
                    SetColReadonly(dgv, col);
                }

                //其余列设置为可写
                //SetColWritable(dgv, ListOpt.ListDif(dgvList, colList));
            }
        }

        /// <summary>
        /// 设置所有列均为只读
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        public static void SetColReadonly(DataGridView dgv)
        {
            if (dgv != null)
            {
                List<int> dgvList = new List<int>();
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    dgvList.Add(colIndex);
                }

                SetColReadonly(dgv, dgvList);
            }
        }

        /// <summary>
        /// 设置Dgv列只读，以单个列名称传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="colName">单个列名</param>
        public static void SetColReadonly(DataGridView dgv, string colName)
        {
            if (dgv != null)
            {
                List<int> colList = new List<int>();
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    if (dgv.Columns[colIndex].Name.Contains(colName))
                    {
                        colList.Add(colIndex);
                    }
                }

                SetColReadonly(dgv, colList);
            }
        }

        /// <summary>
        /// 设置Dgv的列只读，以列名称List传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="nameList">列名List</param>
        public static void SetColReadonly(DataGridView dgv, List<string> nameList)
        {
            if (dgv != null)
            {
                foreach (string colName in nameList)
                {
                    SetColReadonly(dgv, colName);
                }
            }
        }
        #endregion

        #region 列可写
        public static void SetColWritable(DataGridView dgv, int col)
        {
            if(dgv != null)
            {
                dgv.Columns[col].ReadOnly = false;
            }
        }

        public static void SetColWritable(DataGridView dgv, List<int> colList)
        {
            //获取所有列序号
            List<int> dgvList = new List<int>();
            for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
            {
                dgvList.Add(colIndex);
            }
            if (dgv != null)
            {
                //先将所有列readonly
                SetColReadonly(dgv, dgvList);

                foreach (int col in colList)
                {
                    SetColWritable(dgv, col);
                }

                //其余列设置为只读
                //SetColReadonly(dgv, ListOpt.ListDif(dgvList, colList));
            }
        }

        public static void SetColWritable(DataGridView dgv)
        {
            if (dgv != null)
            {
                List<int> dgvList = new List<int>();
                for(int colIndex =0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    dgvList.Add(colIndex);
                }
                SetColWritable(dgv, dgvList);
            }
        }

        /// <summary>
        /// 设置Dgv列可写，以单个列名称传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="colName">单个列名</param>
        public static void SetColWritable(DataGridView dgv, string colName)
        {
            if (dgv != null)
            {
                List<int> colList = new List<int>();
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    if (dgv.Columns[colIndex].Name.Contains(colName))
                    {
                        colList.Add(colIndex);
                    }
                }

                SetColWritable(dgv, colList);
            }
        }

        /// <summary>
        /// 设置Dgv的列可写，以列名称List传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="nameList">列名List</param>
        public static void SetColWritable(DataGridView dgv, List<string> nameList)
        {
            if (dgv != null)
            {
                foreach (string colName in nameList)
                {
                    SetColWritable(dgv, colName);
                }
            }
        }
        #endregion

        #region 列宽度
        /// <summary>
        /// 设置Dgv的列宽度，以单个列序号传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="col">列序号</param>
        /// <param name="width">宽度</param>
        public static void SetColWidth(DataGridView dgv, int col, int width)
        {
            if (dgv != null && width > 0)
            {
                dgv.Columns[col].Width = width;
            }
        }

        /// <summary>
        /// 设置Dgv的列宽度，以单个列名称传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="colName">列名称</param>
        /// <param name="width">宽度</param>
        public static void SetColWidth(DataGridView dgv, string colName, int width)
        {
            if (dgv != null && width > 0)
            {
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    if (dgv.Columns[colIndex].Name.Contains(colName))
                    {
                        SetColWidth(dgv, colIndex, width);
                    }
                }
            }
        }

        /// <summary>
        /// 设置Dgv的列宽度，以列序号字典传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="dict">字典(列序号，宽度)</param>
        public static void SetColWidth(DataGridView dgv, Dictionary<int, int> dict)
        {
            if (dgv != null && dict != null)
            {
                foreach (KeyValuePair<int, int> kv in dict)
                {
                    SetColWidth(dgv, kv.Key, kv.Value);
                }
            }
        }

        /// <summary>
        /// 设置Dgv的列宽度，以列名称字典传入
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        /// <param name="dict">字典(列名称，宽度)</param>
        public static void SetColWidth(DataGridView dgv, Dictionary<string, int> dict)
        {
            if(dgv != null && dict != null)
            {
                foreach(KeyValuePair<string, int> kv in dict)
                {
                    SetColWidth(dgv, kv.Key, kv.Value);
                }
            }
        }

        #endregion

        #region 列居中显示
        public static void SetColHeadMiddleCenter(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SetColNoSortMode(dgv);
        }

        public static void SetColMiddleCenter(DataGridView dgv, int col)
        {
            dgv.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public static void SetColMiddleCenter(DataGridView dgv, List<int> colList)
        {
            if (dgv != null)
            {
                foreach (int col in colList)
                {
                    SetColMiddleCenter(dgv, col);
                }
            }
        }

        public static void SetColMiddleCenter(DataGridView dgv)
        {
            if (dgv != null)
            {
                List<int> dgvList = new List<int>();
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    dgvList.Add(colIndex);
                }
                SetColMiddleCenter(dgv, dgvList);
            }
        }

        public static void SetColMiddleCenter(DataGridView dgv, string name)
        {
            if(dgv != null)
            {
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    if (dgv.Columns[colIndex].Name.Contains(name))
                    {
                        SetColMiddleCenter(dgv, colIndex);
                    }
                }
            }
        }

        public static void SetColMiddleCenter(DataGridView dgv, List<string> nameList)
        {
            if(dgv != null)
            {
                foreach (string name in nameList)
                {
                    SetColMiddleCenter(dgv, name);
                }
            }
        }
        #endregion

        #region 列靠右显示
        public static void SetColMiddleRight(DataGridView dgv, int col)
        {
            dgv.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public static void SetColMiddleRight(DataGridView dgv, List<int> colList)
        {
            if (dgv != null)
            {
                foreach (int col in colList)
                {
                    SetColMiddleRight(dgv, col);
                }
            }
        }

        public static void SetColMiddleRight(DataGridView dgv)
        {
            if (dgv != null)
            {
                List<int> dgvList = new List<int>();
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    dgvList.Add(colIndex);
                }
                SetColMiddleRight(dgv, dgvList);
            }
        }

        public static void SetColMiddleRight(DataGridView dgv, string name)
        {
            if (dgv != null)
            {
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    if (dgv.Columns[colIndex].Name.Contains(name))
                    {
                        SetColMiddleRight(dgv, colIndex);
                    }
                }
            }
        }

        public static void SetColMiddleRight(DataGridView dgv, List<string> nameList)
        {
            if (dgv != null)
            {
                foreach (string name in nameList)
                {
                    SetColMiddleRight(dgv, name);
                }
            }
        }
        #endregion

        #region 列靠左显示
        public static void SetColMiddleLeft(DataGridView dgv, int col)
        {
            dgv.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        public static void SetColMiddleLeft(DataGridView dgv, List<int> colList)
        {
            if (dgv != null)
            {
                foreach (int col in colList)
                {
                    SetColMiddleLeft(dgv, col);
                }
            }
        }

        public static void SetColMiddleLeft(DataGridView dgv)
        {
            if (dgv != null)
            {
                List<int> dgvList = new List<int>();
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    dgvList.Add(colIndex);
                }
                SetColMiddleLeft(dgv, dgvList);
            }
        }

        public static void SetColMiddleLeft(DataGridView dgv, string name)
        {
            if (dgv != null)
            {
                for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
                {
                    if (dgv.Columns[colIndex].Name.Contains(name))
                    {
                        SetColMiddleLeft(dgv, colIndex);
                    }
                }
            }
        }

        public static void SetColMiddleLeft(DataGridView dgv, List<string> nameList)
        {
            if (dgv != null)
            {
                foreach (string name in nameList)
                {
                    SetColMiddleLeft(dgv, name);
                }
            }
        }
        #endregion

        /// <summary>
        /// 设置所有列不需要排序
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        public static void SetColNoSortMode(DataGridView dgv)
        {
            if (dgv != null)
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }

        /// <summary>
        /// 设置Dgv选中最后一行
        /// </summary>
        /// <param name="dgv">传入需要处理的Dgv</param>
        public static void SelectLastRow(DataGridView dgv = null)
        {
            int kk = dgv.RowCount;
            if (dgv.RowCount > 0 && dgv != null)
            {
                dgv.CurrentCell = dgv.Rows[dgv.RowCount - 1].Cells[0];
            }
        }

        #region 行颜色
        public static void SetRowFontColor(DataGridView dgv, int row, Color color)
        {
            if (dgv != null)
            {
                dgv.Rows[row].DefaultCellStyle.ForeColor = color;
            }
        }

        public static void SetRowFontColor(DataGridView dgv, List<int> rowList, Color color)
        {
            if (dgv != null)
            {
                foreach(int row in rowList)
                {
                    SetRowFontColor(dgv, row, color);
                }
            }
        }
        #endregion
    }
    
    public class DtOpt
    {
        public static void DtDateFormat(DataTable Dt, List<int> FormatList)
        {
            if (Dt != null)
            {
                for (int rowIndex = 0; rowIndex < Dt.Rows.Count; rowIndex++)
                {
                    foreach (int listIndex in FormatList)
                    {
                        Dt.Rows[rowIndex][listIndex] = Normal.ConvertDate(Dt.Rows[rowIndex][listIndex].ToString());
                    }
                }
            }
        }

        public static void DtDateFormat(DataTable Dt, string ColName)
        {
            if (Dt != null)
            {
                List<int> formatList = new List<int>();
                for (int colIndex = 0; colIndex < Dt.Columns.Count; colIndex++)
                {
                    if (Dt.Columns[colIndex].ColumnName.Contains(ColName))
                    {
                        formatList.Add(colIndex);
                    }
                }

                for (int rowIndex = 0; rowIndex < Dt.Rows.Count; rowIndex++)
                {
                    foreach (int listIndex in formatList)
                    {
                        Dt.Rows[rowIndex][listIndex] = Normal.ConvertDate(Dt.Rows[rowIndex][listIndex].ToString());
                    }
                }
            }
        }

        public static DataTable DtDifferent(DataTable dt1, DataTable dt2)
        {
            DataTable resultDt = new DataTable();
            return resultDt;
        }
    }

    public class ListOpt
    {

        /// <summary>
        /// 对比两个List的差异
        /// </summary>
        /// <param name="list1">被对比List，多项</param>
        /// <param name="list2">对比Lis， 少项t</param>
        /// <returns></returns>
        public static List<int> ListDif(List<int> list1, List<int> list2)
        {
            List<int> list3 = new List<int>();
            foreach (int item1 in list1)
            {
                if (!list2.Contains(item1))
                {
                    list3.Add(item1);
                }
            }
            return list3;
        }

        /// <summary>
        /// 对比两个List的差异
        /// </summary>
        /// <param name="list1">被对比List，多项</param>
        /// <param name="list2">对比Lis， 少项t</param>
        /// <returns></returns>
        public static List<string> ListDif(List<string> list1, List<string> list2)
        {
            List<string> list3 = new List<string>();
            foreach (string item1 in list1)
            {
                if (!list2.Contains(item1))
                {
                    list3.Add(item1);
                }
            }
            return list3;
        }
    }
}
