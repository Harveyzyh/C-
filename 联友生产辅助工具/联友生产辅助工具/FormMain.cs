using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HarveyZ;
using 联友生产辅助工具.FormModule;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;

namespace 联友生产辅助工具
{
    public partial class FormMain : Form
    {
        public static string strConnection = Global_Const.strConnection_WG_DB;

        #region Init
        public FormMain()
        {
            InitializeComponent();

            FormMainInit();

            Form_MainResized_Work();
        }

        private void FormMainInit()
        {
            LabelUserInfo.Text = "部门：" + FormLogin.Login_Dpt + "    姓名：" + FormLogin.Login_Uid + "-" + FormLogin.Login_Name;
            if(FormLogin.Login_Uid != "001114")
            {
                测试ToolStripMenuItem.Visible = false;
            }
        }
        #endregion

        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Dispose();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region 窗口大小变化设置
        private void Form_MainResized(object sender, EventArgs e)
        {
            Form_MainResized_Work();
        }

        private void Form_MainResized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width - 16;
            FormHeight = Height - 16;
            LabelUserInfo.Location = new Point(2, FormHeight - 40);
        }
        #endregion

        #region 菜单栏设置
        #endregion

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        /*
         以下为测试部分
             */
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //ZipUnZip zipunzip = new ZipUnZip();
            //MessageBox.Show("", "");
            //WebClient client = new WebClient();
            //string URLAddress = @"http://192.168.7.251:8099/Client/WG/Download/FtpClient.zip";
            //string receivePath = @"F:\";
            //client.DownloadFile(URLAddress, receivePath + System.IO.Path.GetFileName(URLAddress));

            //string kk = @"F:\FtpClient.zip";

            //if (zipunzip.UnZip(kk, @"F:\FtpClient") == true)
            //{
            //    MessageBox.Show("解压完成", "");
            //}
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Form_Opt frm = new Form_Opt(PDA_JH);
        }
    }
}