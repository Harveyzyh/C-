using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Web.Script.Serialization;
using HarveyZ;
using 联友生产辅助工具.仓储中心;
using 联友生产辅助工具.生产日报表;
using 联友生产辅助工具.生管码垛线;
using 联友生产辅助工具.生管排程;
using 联友生产辅助工具.管理;
using 联友生产辅助工具.测试;

namespace 联友生产辅助工具
{
    public partial class 主界面 : Form
    {
        public static InfoObject infObj = new InfoObject();
        #region 静态变量
        private static Form FormOpen = null;
        private static List<string> menuIgnoreList = new List<string> {
            "关闭当前界面",
            "帮助",
            "测试",
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
            
            StatusBarSetItem();
        }

        #endregion

        #region 窗体权限设置
        private void FormPermission()
        {
            ItemUnvisable();
            SetPermission(FormLogin.userPermList);
            SetTestPermission();
        }
        
        private void SetTestPermission()
        {
            if (FormLogin.Login_Uid != "001114")
            {
                测试ToolStripMenuItem.Visible = false;
            }
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
            if(con.DropDownItems.Count > 0)
            {
                foreach(ToolStripMenuItem con2 in con.DropDownItems)
                {
                    ItemUnvisableWork(con2);
                }
                con.Visible = false;
            }
            else
            {
                con.Visible = false;
                FormLogin.menuItemList.Add(con.Name.Replace("ToolStripMenuItem", ""));
            }
        }

        #endregion

        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Form frm = FormOpen;
                if(frm != null)
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
            if(FormOpen != null)
            {
                Form frm = FormOpen;
                frm.WindowState = FormWindowState.Minimized;
                frm.WindowState = FormWindowState.Maximized;
            }

            //statusLabelYunConn.Width = 150;
            //statusLabelYunConn.TextAlign = ContentAlignment.MiddleCenter;
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
                if(FormOpen != null)
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
            statusLabelUser.Text = "部门：" + FormLogin.Login_Dpt + "    姓名：" + FormLogin.Login_Uid + "-" + FormLogin.Login_Name;
            statusLabelIP.Text = "本机IP地址：" + IPInfo.GetIpAddress() + "  ";
            if (FormLogin.connFlag99)
            {
                statusLabelLocalConn.Text = "联友服务器：已连接";
            }
            else
            {
                statusLabelLocalConn.Text = "联友服务器：未连接";
            }
        }
        #endregion

        #region 测试部分
        private void 测试_1ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 测试_2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new 测试_2();
            FormOpenInit(frm);
            frm.Visible = true;
        }

        private void 测试_3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new 测试_3();
            FormOpenInit(frm);
            frm.Visible = true;
        }

        public DataTable Json2Dt(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                MessageBox.Show("0");
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                MessageBox.Show("0.5");
                if (arrayList.Count > 0)
                {
                    MessageBox.Show("1");
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        MessageBox.Show("2");
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            MessageBox.Show("3");
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            result = dataTable;
            return result;
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
            用户管理 frm = new 用户管理();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 管理_用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            用户管理 frm = new 用户管理();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion

        #region 仓储中心
        private void 仓储中心_扫描领料单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PDA_扫描领料单 frm = new PDA_扫描领料单();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 仓储中心_扫描进货单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PDA_扫描进货单 frm = new PDA_扫描进货单();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 仓储中心_扫描进货单采购平台ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            扫描进货单_采购平台 frm = new 扫描进货单_采购平台();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 仓储中心_生成领料单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            生成领料单 frm = new 生成领料单();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion

        #region 生产日报表
        private void 生产日报表_查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表查询 frm = new 日报表查询();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表新增 frm = new 日报表新增();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表修改 frm = new 日报表修改();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_系列组别维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表维护组别系列 frm = new 日报表维护组别系列();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_部门线别维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表部门线别维护 frm = new 日报表部门线别维护();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_工作组线别维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表部门工作组线别维护 frm = new 日报表部门工作组线别维护();
            FormOpenInit(frm);
            frm.Show();
        }

        #endregion

        #region 码垛线
        private void 码垛线_排程导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            码垛线排程导入 frm = new 码垛线排程导入();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 码垛线_纸箱编码管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            纸箱编码管理 frm = new 纸箱编码管理();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 码垛线_订单类别编码管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            订单类别编码管理 frm = new 订单类别编码管理();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 码垛线_纸箱名称管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            纸箱名称管理 frm = new 纸箱名称管理();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion

        #region 生产排程
        private void 生管_电子排程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            生产电子排程 frm = new 生产电子排程();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生管_订单信息查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            订单信息查询 frm = new 订单信息查询();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion

        #endregion
    }

    public class InfoObject : InfoObjectBase
    {

    }
}