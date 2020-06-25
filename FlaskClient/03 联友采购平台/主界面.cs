using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HarveyZ;
using 联友采购平台.供应商;

namespace HarveyZ
{
    public partial class 主界面 : Form
    {
        public static InfoObject infObj = new InfoObject();
        public static string companyName = "";
        #region 静态变量
        private static Form FormOpen = null;
        private static List<string> menuIgnoreList = new List<string> {
            "关闭当前界面",
            "帮助",
            "此用户没有任何权限"
        };
        #endregion

        #region 窗体初始化
        public 主界面()
        {
            InitializeComponent();

            infObj.userId = FormLogin.infObj.userId;
            infObj.userName = FormLogin.infObj.userName;
            infObj.userDpt = FormLogin.infObj.userDpt;

            关闭当前界面ToolStripMenuItem.Visible = false;

            FormPermission();

            Form_MainResized_Work();

            this.Text = FormLogin.infObj.progName + "      Ver." + FormLogin.infObj.progVer;
            if (FormLogin.infObj.testFlag)
            {
                this.Text += "     -DEBUG";
            }

            companyName = GetCompanyName(FormLogin.infObj.userDpt);

            StatusBarSetItem();
        }

        #endregion

        #region 窗体权限设置
        private void FormPermission()
        {
            ItemUnvisable();
            
            FormLogin.infObj.userPermList.Add("供应商_录入送货单");
            FormLogin.infObj.userPermList.Add("管理_修改密码");

            SetPermission(FormLogin.infObj.userPermList);
            SetTestPermission();
        }

        private void SetTestPermission()
        {

        }

        private void SetPermission(List<string> list)
        {
            if (list == null || list.Count == 0)
            {
                此用户没有任何权限ToolStripMenuItem.Visible = true;
            }
            ItemVisable(list);
        }

        private void ItemVisable(List<string> ItemList)
        {
            if (ItemList != null)
            {
                List<ToolStripMenuItem> list = new List<ToolStripMenuItem> { };
                foreach (string Item in ItemList)
                {
                    foreach (ToolStripMenuItem con in MainMenuStrip.Items)
                    {
                        list.Clear();
                        ItemVisableWork(con, Item + "ToolStripMenuItem", list);

                        if (list.Count > 0)
                        {
                            list.Reverse();
                            foreach (ToolStripMenuItem m in list)
                            {
                                m.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        private bool ItemVisableWork(ToolStripMenuItem con, string item, List<ToolStripMenuItem> list)
        {
            if (con.DropDownItems.Count > 0)
            {
                foreach (ToolStripMenuItem con2 in con.DropDownItems)
                {

                    if (ItemVisableWork(con2, item, list))
                    {
                        list.Add(con);
                        return true;
                    }
                }
                return false;
            }
            else
            {
                if (con.Name == item)
                {
                    list.Add(con);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void ItemUnvisable()
        {
            foreach (ToolStripMenuItem con in MainMenuStrip.Items)
            {
                if (menuIgnoreList.Contains(con.Name.Replace("ToolStripMenuItem", "")))
                {
                    continue;
                }
                else
                {
                    ItemUnvisableWork(con);
                }
            }
        }

        private void ItemUnvisableWork(ToolStripMenuItem con)
        {
            if (con.DropDownItems.Count > 0)
            {
                foreach (ToolStripMenuItem con2 in con.DropDownItems)
                {
                    ItemUnvisableWork(con2);
                }
                con.Visible = false;
            }
            else
            {
                con.Visible = false;
                FormLogin.infObj.menuItemList.Add(con.Name.Replace("ToolStripMenuItem", ""));
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
            
            statusLabelLocalConn.Width = 150;
            statusLabelLocalConn.TextAlign = ContentAlignment.MiddleCenter;
            statusLabelIP.Width = 180;
            statusLabelIP.TextAlign = ContentAlignment.MiddleRight;

            statusLabelUser.TextAlign = ContentAlignment.MiddleLeft;
            statusLabelUser.Width = FormWidth - statusLabelLocalConn.Width - statusLabelIP.Width - 12;
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
            frm.Close();
            frm.Dispose();
            FormOpen = null;
            关闭当前界面ToolStripMenuItem.Visible = false;
            关闭当前界面ToolStripMenuItem.Text = "关闭界面";
        }
        #endregion

        #region 状态栏
        private void StatusBarSetItem()
        {
            statusLabelUser.Text = "公司：" + FormLogin.infObj.userDpt + '-' + companyName + "    姓名：" + FormLogin.infObj.userId + "-" + FormLogin.infObj.userName;
            statusLabelIP.Text = "本机IP地址：" + IPInfo.GetIpAddress() + "  ";
            if (FormLogin.infObj.connFlag)
            {
                statusLabelLocalConn.Text = "联友服务器：已连接";
            }
            else
            {
                statusLabelLocalConn.Text = "联友服务器：未连接";
            }
        }

        //额外补充信息，补充供应商名称
        private string GetCompanyName(string companyId)
        {
            if (companyId == "LY")
            {
                return "联友办公家具有限公司";
            }
            else
            {
                string sqlstr = @"SELECT RTRIM(MA003) FROM dbo.PURMA WHERE MA001 = '{0}'";
                DataTable dt = FormLogin.infObj.sql.SQLselect(FormLogin.infObj.connYF, string.Format(sqlstr, companyId));
                if (dt != null)
                {
                    return dt.Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region 菜单栏设置

        #region 菜单栏其他选项
        private void 关闭当前界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCloseWork();
        }
        #endregion

        #region 管理
        private void 管理_权限管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            权限管理 frm = new 权限管理();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 管理_用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            用户管理 frm = new 用户管理();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 管理_修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            修改密码 frm = new 修改密码();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 管理_添加用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            添加用户 frm = new 添加用户();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion

        #region 供应商
        private void 供应商_录入送货单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            录入供应商送货单 frm = new 录入供应商送货单();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion

        #endregion
    }
}