using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ.仓储中心;
using HarveyZ.生产日报表;
using HarveyZ.生管码垛线;
using HarveyZ.生管排程;
using HarveyZ.品管;
using HarveyZ.财务;
using HarveyZ.采购;
using HarveyZ.报表;
using HarveyZ.维护ERP;

namespace HarveyZ
{
    public partial class 主界面 : Form
    {
        public static InfoObject infObj = new InfoObject();
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

            StatusBarSetItem();
        }

        #endregion

        #region 窗体权限设置
        private void FormPermission()
        {
            ItemUnvisable();
            SetPermission(FormLogin.infObj.userPermList);
            SetTestPermission();
        }

        // 测试账户才会显示的菜单
        private void SetTestPermission()
        {
            if (FormLogin.infObj.userId != "001114")
            {

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

            statusLabelWGConn.Width = 150;
            statusLabelWGConn.TextAlign = ContentAlignment.MiddleCenter;
            statusLabelSWConn.Width = 150;
            statusLabelSWConn.TextAlign = ContentAlignment.MiddleCenter;
            statusLabelLinkMode.Width = 180;
            statusLabelLinkMode.TextAlign = ContentAlignment.MiddleCenter;
            statusLabelAppVersion.Width = 180;
            statusLabelAppVersion.TextAlign = ContentAlignment.MiddleCenter;
            statusLabelIP.Width = 180;
            statusLabelIP.TextAlign = ContentAlignment.MiddleRight;

            statusLabelUser.TextAlign = ContentAlignment.MiddleLeft;
            statusLabelUser.Width = FormWidth - statusLabelWGConn.Width - statusLabelSWConn.Width - statusLabelIP.Width - statusLabelAppVersion.Width - statusLabelLinkMode.Width - 12;
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
            statusLabelUser.Text = "部门：" + FormLogin.infObj.userDpt + "    姓名：" + FormLogin.infObj.userId + "-" + FormLogin.infObj.userName;
            statusLabelIP.Text = "本机IP地址：" + IPInfo.GetIpAddress() + "  ";
            statusLabelAppVersion.Text = "软件版本：" + FormLogin.infObj.progVer;

            statusLabelWGConn.Text = FormLogin.infObj.connWgFlag ? "生产服务器：已连接" : "生产服务器：未连接";

            statusLabelSWConn.Text = FormLogin.infObj.connSwFlag ? "财务服务器：已连接" : "财务服务器：未连接";

            statusLabelLinkMode.Text = FormLogin.infObj.remoteFlag ? "连接模式：远端模式" : "连接模式：本地模式";

            statusLabelLinkMode.Text += FormLogin.infObj.testFlag ? " -- DEBUG" : "";
        }
        #endregion

        #region 菜单栏设置

        #region 菜单栏其他选项
        private void 关闭当前界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCloseWork();
        }

        private void 帮助_使用手册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            手册下载 frm = new 手册下载("帮助_使用手册", "联友生产辅助工具-使用手册.pdf");
            frm.ShowDialog();
        }
        #endregion

        #region 管理
        private void 管理_权限管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            权限管理 frm = new 权限管理("管理_权限管理");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 管理_版本发布ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            版本发布 frm = new 版本发布("管理_版本发布");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 管理_FastReport模板发布ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                FastReport模板发布 frm = new FastReport模板发布("管理_FastReport模板发布");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 管理_逻辑手册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            手册下载 frm = new 手册下载("管理_逻辑手册", "联友生产辅助工具-逻辑手册.pdf");
            frm.ShowDialog();
        }
        #endregion

        #region 仓储中心
        private void 仓储中心_扫描领料单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            扫描领料单 frm = new 扫描领料单("仓储中心_扫描领料单");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 仓储中心_录入进货单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            录入进货单 frm = new 录入进货单("仓储中心_录入进货单");
            //FormOpenInit(frm);
            frm.Show();
        }

        private void 仓储中心_录入进货单Excel导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                录入进货单_Excel导入 frm = new 录入进货单_Excel导入("仓储中心_录入进货单Excel导入");
                //FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 仓储中心_录入退货单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                录入退货单 frm = new 录入退货单("仓储中心_录入退货单");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 仓储中心_生成领料单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                生成领料单 frm = new 生成领料单("仓储中心_生成领料单");
                FormOpenInit(frm);
                frm.Show();
            }
        }
        #endregion

        #region 生产日报表
        private void 生产日报表_查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表查询 frm = new 日报表查询("生产日报表_查询");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表新增 frm = new 日报表新增("生产日报表_新增");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表修改 frm = new 日报表修改("生产日报表_修改");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_系列组别维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表维护组别系列 frm = new 日报表维护组别系列("生产日报表_系列组别维护");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_部门线别维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表部门线别维护 frm = new 日报表部门线别维护("生产日报表_部门线别维护");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生产日报表_工作组线别维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            日报表部门工作组线别维护 frm = new 日报表部门工作组线别维护("生产日报表_工作组线别维护");
            FormOpenInit(frm);
            frm.Show();
        }

        #endregion

        #region 码垛线
        private void 码垛线_排程导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                码垛线排程导入 frm = new 码垛线排程导入("码垛线_排程导入");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 码垛线_纸箱编码管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                纸箱编码管理 frm = new 纸箱编码管理("码垛线_纸箱编码管理");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 码垛线_订单类别编码管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                订单类别编码管理 frm = new 订单类别编码管理("码垛线_订单类别编码管理");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 码垛线_纸箱名称管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                纸箱名称管理 frm = new 纸箱名称管理("码垛线_纸箱名称管理");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 码垛线_客户端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                码垛线客户端 frm = new 码垛线客户端("码垛线_客户端");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 码垛线_ERP单据生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                码垛线_ERP单据生成程序 frm = new 码垛线_ERP单据生成程序("码垛线_ERP单据生成");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 码垛线_码垛线报表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                码垛线报表 frm = new 码垛线报表("码垛线_码垛线报表");
                FormOpenInit(frm);
                frm.Show();
            }
        }
        #endregion

        #region 生产排程
        private void 生管_生产排程部门管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            生产排程部门管理 frm = new 生产排程部门管理("生管_生产排程部门管理");
            FormOpenInit(frm);
            frm.Show();
        }
        private void 生管_生产排程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            生产排程 frm = new 生产排程("生管_生产排程");
            FormOpenInit(frm);
            frm.Show();
        }

        private void 生管_订单信息查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                订单信息查询 frm = new 订单信息查询("生管_订单信息查询");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 生管_排程物料导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                排程物料导出_生产 frm = new 排程物料导出_生产("生管_排程物料导出");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 生管_排程物料导出_汇总ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                排程物料导出_汇总 frm = new 排程物料导出_汇总("生管_排程物料导出_汇总");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 生管_自动LRP计划队列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                自动LRP计划队列 frm = new 自动LRP计划队列("生管_自动LRP计划队列");
                FormOpenInit(frm);
                frm.Show();
            }
        }
        #endregion

        #region 品管部
        private void 品管部_成品标签打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                成品标签打印 frm = new 成品标签打印("品管部_成品标签打印");
                FormOpenInit(frm);
                frm.Show();
            }
        }
        #endregion

        #region 采购
        private void 采购_排程物料导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                排程物料导出_采购 frm = new 排程物料导出_采购("采购_排程物料导出");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 采购_排程物料导出纸箱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                排程物料导出_采购_纸箱 frm = new 排程物料导出_采购_纸箱("采购_排程物料导出_纸箱");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 采购_批量采购数量汇总ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                批量采购数量汇总 frm = new 批量采购数量汇总("采购_批量采购数量汇总");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        #endregion

        #region 财务部
        private void 财务部_成本异常报表导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                成本异常报表导出 frm = new 成本异常报表导出("财务部_成本异常报表导出");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 财务部_刷新会计科目生产ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                刷新会计科目生产 frm = new 刷新会计科目生产("财务部_刷新会计科目生产");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 财务部_品号信息生产导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                品号信息生产导出 frm = new 品号信息生产导出("财务部_品号信息生产导出");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 财务部_品号信息税务导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                品号信息税务导出 frm = new 品号信息税务导出("财务部_品号信息税务导出");
                FormOpenInit(frm);
                frm.Show();
            }
        }
        
        private void 财务部_领退料明细生产导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                领退料明细生产导出 frm = new 领退料明细生产导出("财务部_领退料明细生产导出");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 财务部_工单明细表生产导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                工单明细生产导出 frm = new 工单明细生产导出("财务部_工单明细表生产导出");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void 财务部_收集工单工时税务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                收集工单工时税务 frm = new 收集工单工时税务("财务部_收集工单工时税务");
                FormOpenInit(frm);
                frm.Show();
            }
        }
        #endregion

        #region 报表
        private void 报表_销货信息带入库部门ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                销货信息_带入库部门_查询 frm = new 销货信息_带入库部门_查询("报表_销货信息带入库部门");
                FormOpenInit(frm);
                frm.Show();
            }
        }
        #endregion

        #region ERP
        private void ERP_客户配置维护_勾选项替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                客户配置维护_勾选项替换 frm = new 客户配置维护_勾选项替换("ERP_客户配置维护_勾选项替换");
                FormOpenInit(frm);
                frm.Show();
            }
        }

        private void ERP_客户配置维护_存在未勾选的配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(FormLogin.StopModuleOpen())
            {
                客户配置维护_存在未勾选的配置 frm = new 客户配置维护_存在未勾选的配置("ERP_客户配置维护_存在未勾选的配置");
                FormOpenInit(frm);
                frm.Show();
            }
        }
        #endregion

        #endregion
    }
}