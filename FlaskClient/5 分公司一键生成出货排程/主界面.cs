using HarveyZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 联友中山分公司生产辅助工具
{
    public partial class 主界面 : Form
    {
        private string ProgName = "";
        private string ProgVersion = "";
        private static Form FormOpen = null;

        public static string connY_Ls = Global_Const.strConnection_Y_LS;

        private delegate void SqlTestDelegate(string connStr);
        private Mssql mssql = new Mssql();

        #region 窗体初始化
        public 主界面()
        {
            InitializeComponent();
            GetMutilOpen();

            //从文件详细信息中获取程序名称
            ProgName = Application.ProductName.ToString();
            ProgVersion = Application.ProductVersion.ToString();

            Form_MainResized_Work();
            this.Text += "      Ver." + ProgVersion;

            //SqlTestDelegate sqlTestYDelegate = new SqlTestDelegate(SqlTestY);
            //sqlTestYDelegate.BeginInvoke(connY_Ls, null, null);
        }

        private void SqlTestY(string connStr)
        {
            if (mssql.SQLlinkTest(connStr))
            {
                statusLabelYConn.Text = "已连接云服务器";
            }
            else
            {
               statusLabelYConn.Text = "未连接云服务器";
            }
        }
        #endregion

        #region 程序不能多开设定
        private void GetMutilOpen()
        {
            bool Exist;//定义一个bool变量，用来表示是否已经运行
                       //创建Mutex互斥对象
            System.Threading.Mutex newMutex = new System.Threading.Mutex(true, "仅一次", out Exist);
            if (Exist)//如果没有运行
            {
                newMutex.ReleaseMutex();//运行新窗体
            }
            else
            {
                MessageBox.Show("本程序已正在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);//弹出提示信息
                Environment.Exit(0);
            }
        }
        #endregion

        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Form frm = FormOpen;
                if (frm != null)
                {
                    frm.Dispose();
                }
                Environment.Exit(0);
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
            FormWidth = Width - 18;
            FormHeight = Height - 40;
            panelParent.Size = new Size(FormWidth, FormHeight - menuStrip1.Height - statusBar.Height - 1);
            //父窗体发生大小变化时，重新设置子窗体的大小
            if (FormOpen != null)
            {
                Form frm = FormOpen;
                frm.WindowState = FormWindowState.Minimized;
                frm.WindowState = FormWindowState.Maximized;
            }
        }
        #endregion

        #region 子窗体初始化
        private Form FormOpenInit(object sender)
        {
            Form frm = null;
            try
            {
                frm = (Form)sender;
                if (FormOpen != null)
                {
                    Form frmOpen = FormOpen;
                    frmOpen.Dispose();
                }
                frm = FormOpenWork(frm);
                return frm;
            }
            catch
            {
                return null;
            }
        }

        private Form FormOpenWork(Form sender)
        {
            Form frm = sender;
            FormOpen = frm;
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.WindowState = FormWindowState.Maximized;
            frm.Parent = this.panelParent;
            关闭当前界面ToolStripMenuItem.Visible = true;
            关闭当前界面ToolStripMenuItem.Text = "关闭界面(" + frm.Text + ")";
            return frm;
        }

        private void FormCloseWork()
        {
            Form frm = FormOpen;
            frm.Dispose();
            FormOpen = null;
            关闭当前界面ToolStripMenuItem.Visible = false;
            关闭当前界面ToolStripMenuItem.Text = "关闭界面";
        }
        #endregion

        #region 菜单栏其他选项
        private void 关闭当前界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCloseWork();
        }
        #endregion

        #region 菜单栏
        private void 生产入库领料明细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            生产入库领料明细 frm = new 生产入库领料明细();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日入库数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            生产日入库明细 frm = new 生产日入库明细();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 出货排程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            物控登录 frmL = new 物控登录();
            frmL.ShowDialog();
            if (物控登录.Flag)
            {
                生成出货排程 frm = new 生成出货排程();
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 物料需求量导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            联友物料需求量导出 frm = new 联友物料需求量导出();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 查询玖友库存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            玖友库存 frm = new 玖友库存();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion
    }
}
