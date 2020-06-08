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
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
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
                System.Diagnostics.Process.Start(prodPath + "\\" + "程序更新.exe", "\"" + prodName.Trim() + "\" \"" + prodVersion.Trim() + "\" \"" + prodPath + "\" \"" + _conStr + "\" \"" + pid + "\"");
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
            string Path = System.IO.Directory.GetCurrentDirectory();
            try
            {
                string[] arg = new string[2];
                arg[0] = UpdateUrl;
                arg[1] = ProgName;
                StartProcess(Path + "\\" + "AutoUpdate.exe", arg);
                System.Environment.Exit(0);
            }
            catch (Win32Exception)
            {
                if (MessageBox.Show("找不到更新软件，程序即将退出!", "错误", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }

        }
        #endregion
    }


    public class FormOpt
    {
        public Form FormInit(Form sender)
        {
            Form frm = sender;
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.WindowState = FormWindowState.Maximized;
            return frm;
        }
    }


    public class DgvOpt
    {
        public static void SetRowColor(DataGridView Dgv)
        {
            //行颜色
            Dgv.RowsDefaultCellStyle.BackColor = Color.Bisque;
            Dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
        }

        public static void SetColReadonly(DataGridView Dgv, List<int> formatList)
        {
            foreach (int listIndex in formatList)
            {
                Dgv.Columns[listIndex].ReadOnly = true;
            }
        }

        public static void SetColReadonly(DataGridView Dgv, string ColName)
        {
            List<int> formatList = new List<int>();
            for (int colIndex = 0; colIndex < Dgv.Columns.Count; colIndex++)
            {
                if (Dgv.Columns[colIndex].Name.Contains(ColName))
                {
                    formatList.Add(colIndex);
                }
            }

            foreach (int listIndex in formatList)
            {
                Dgv.Columns[listIndex].ReadOnly = true;
            }
        }
        
        public static void SetColNoSortMode(DataGridView Dgv)
        {
            for (int i = 0; i < Dgv.Columns.Count; i++)
            {
                Dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public static void SelectLastRow(DataGridView dgv = null)
        {
            int kk = dgv.RowCount;
            if (dgv.RowCount > 0 && dgv != null)
            {
                dgv.CurrentCell = dgv.Rows[dgv.RowCount - 1].Cells[0];
            }
        }
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
}
