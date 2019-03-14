using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HarveyZ;
using 联友生产辅助工具.仓储中心;
using 联友生产辅助工具.生产日报表;
using 联友生产辅助工具.生管码垛线;
using 联友生产辅助工具.生管排程;
using 联友生产辅助工具.玖友;
using 联友生产辅助工具.管理;
using 联友生产辅助工具.测试;
using System.Collections;
using System.Web.Script.Serialization;
using Common.Helper.Crypto;

namespace 联友生产辅助工具
{
    public partial class 主界面 : Form
    {
        #region 静态变量
        private static Form FormOpen = null;
        public static List<string> MenuItem_List = new List<string> { };//菜单栏列表
        #endregion

        #region 窗体初始化
        public 主界面()
        {
            InitializeComponent();
            
            LabelUserInfo.Text = "部门：" + FormLogin.Login_Dpt + "    姓名：" + FormLogin.Login_Uid + "-" + FormLogin.Login_Name;
            IPInfo ipinfo = new IPInfo();
            LabelIPInfo.Text = "本机IP地址：" + ipinfo.GetIpAddress() + "  ";

            关闭当前界面ToolStripMenuItem.Visible = false;

            FormPermission();

            Form_MainResized_Work();
            this.Text += "      Ver." + FormLogin.ProgVersion;
        }

        #endregion

        #region 窗体权限设置
        private void FormPermission()
        {
            SetTestPermission();

            ItemUnvisable();

            List<string> list = new List<string> { };
            if (FormLogin.Login_Role != "Super")
            {
                if(FormLogin.Login_Dpt.Substring(0, 2) == "生产")
                {
                    list.Add("生产日报表_新增");
                    list.Add("生产日报表_修改");
                    list.Add("生产日报表_查询");
                }
                if(FormLogin.Login_Dpt == "工程部")
                {
                    list.Add("码垛线_纸箱编码管理");
                    list.Add("码垛线_订单类别编码管理");
                    list.Add("码垛线_排程导入");
                }
                if(FormLogin.Login_Role == "生管")
                {
                    list.Add("生产日报表_查询");
                    list.Add("生产日报表_部门线别维护");
                    list.Add("生产日报表_系列组别维护");
                    list.Add("码垛线_排程导入");
                }
                if(FormLogin.Login_Uid == "000960" || FormLogin.Login_Uid == "001165")
                {
                    list.Add("仓储中心_扫描领料单");
                }
                if (FormLogin.Login_Uid == "000068" || FormLogin.Login_Uid == "001161")
                {
                    list.Add("仓储中心_扫描进货单");
                }
                if (FormLogin.Login_Uid == "000807")
                {
                    list.Add("生产日报表_查询");
                }
                if (FormLogin.Login_Uid == "000946")
                {
                    list.Add("生产日报表_查询");
                }
                
                SetPermission(list);
            }
            else
            {
                SetPermission(MenuItem_List);
            }
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
            ItemVisable(list);
        }

        private void ItemVisable(List<string> ItemList)
        {
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem> { };
            foreach(string Item in ItemList)
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
                if (con.Name == "关闭当前界面ToolStripMenuItem")
                {
                    continue;
                }
                else if (con.Name == "帮助ToolStripMenuItem")
                {
                    continue;
                }
                else if(con.Name == "测试ToolStripMenuItem")
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
                MenuItem_List.Add(con.Name.Replace("ToolStripMenuItem", ""));
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
            FormWidth = Width - 20;
            FormHeight = Height - 40;
            LabelUserInfo.Location = new Point(2, FormHeight - LabelUserInfo.Height);
            LabelIPInfo.Location = new Point(FormWidth - LabelIPInfo.Width, FormHeight - LabelIPInfo.Height);
            panelParent.Size = new Size(FormWidth, FormHeight - menuStrip1.Height - LabelUserInfo.Height - 7);
            //父窗体发生大小变化时，重新设置子窗体的大小
            if(FormOpen != null)
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
            frm.Dispose();
            FormOpen = null;
            关闭当前界面ToolStripMenuItem.Visible = false;
            关闭当前界面ToolStripMenuItem.Text = "关闭界面";
        }
        #endregion

        #region 测试部分
        private void 测试_1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new 测试_码垛线排程导入();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 测试_2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
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

        #region 其他选项
        private void 关闭当前界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCloseWork();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("关于", "关于", MessageBoxButtons.OK);
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
        #endregion

        #region PDA工具
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
        #endregion

        #region 生管排程
        private void 生管_电子排程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            生管电子排程 frm = new 生管电子排程();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion

        #region 玖友
        private void 玖友_查询物料需求量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            玖友查询物料需求量 frm = new 玖友查询物料需求量();
            FormOpenInit(frm);
            frm.Show();
        }

        private void 玖友_查询生产排程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            生管电子排程 frm = new 生管电子排程();
            FormOpenInit(frm);
            frm.Show();
        }
        #endregion

        #endregion 
    }
}