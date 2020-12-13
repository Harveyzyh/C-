using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友中山分公司生产辅助工具.销售管理
{
    public partial class 录入销货单 : Form
    {
        #region 公用变量设定

        public static string connStr = FormLogin.infObj.connYF;
        #endregion

        #region 局部变量设定
        private Mssql mssql = FormLogin.infObj.sql;
        ERP_Create_Purtg createPurtg = new ERP_Create_Purtg();

        public static string Mode = null;
        public static string Title = null;
        public static string GetMain = null;
        public static string GetOther = null;
        public static string MaterielMode = null;

        private string SupplierID = null;
        private string TypeID = "2301";
        private string PositionID = "301";
        private string MaterielID = null;
        private string LoginUid = FormLogin.infObj.userId;
        private string LoginUserGroup = FormLogin.infObj.userGroup;

        private bool MsgFlag = false;
        #endregion

        #region 窗口初始化
        public 录入销货单()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            销货单别.Text = "2301-销货单";
            销货仓库.Text = "301-成品仓";

            panel_Title.Enabled = true;
            panel_Last.Enabled = false;
            
        }
        #endregion

        #region 窗口大小变化设置
        private void FormMain_Resized(object sender, EventArgs e)
        {
            FormMain_Resized_Work();
        }

        private void FormMain_Resized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            panel_Title.Location = new Point(1, 1);
            panel_Title.Size = new Size(FormWidth - 2, panel_Title.Height);

            panel_Last.Location = new Point(panel_Title.Location.X, panel_Title.Height + DataGridView_List.Height);
            panel_Last.Size = new Size(panel_Title.Width, panel_Last.Height);

            DataGridView_List.Location = new Point(panel_Title.Location.X, panel_Title.Location.Y + panel_Title.Height + 1);
            DataGridView_List.Size = new Size(panel_Title.Width, FormHeight - (panel_Title.Height + 1) - (panel_Last.Height + 1));
        }
        #endregion

        #region UI按钮
        private void 销货仓库L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "PositionID";
            Title = "仓库编号";
            //Title = "仓库编号|仓库名称";
            录入销货单_获取单据信息 frm = new 录入销货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    销货仓库.Text = GetMain + "-" + GetOther;
                    PositionID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void 客户L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "CustmerID";
            Title = "客户编号";
            //Title = "供应商编号|简称";
            录入销货单_获取单据信息 frm = new 录入销货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    客户.Text = GetMain + "-" + GetOther;
                    SupplierID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
                MsgFlag = true;
            }
        }

        private void 销货单别L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "TypeID";
            Title = "单别";
            //Title = "单别|单据简称";
            录入销货单_获取单据信息 frm = new 录入销货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    销货单别.Text = GetMain + "-" + GetOther;
                    TypeID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void 品号_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && MsgFlag == true)
            {
                MsgFlag = false;
                return;
            }
            else
            {
                MsgFlag = false;
            }

            if (e.KeyCode == Keys.Enter)
            {
                品号T.Text = 品号T.Text.ToUpper();
                if (销货单别.Text == "" || 客户.Text == "" || 销货仓库.Text == "")
                {
                    MsgFlag = true;
                    MessageBox.Show("请先查询或填写 销货单别，客户，销货仓库 的信息！", "错误");
                    品号T.Select();
                    品号T.SelectAll();
                }
                else if(品号T.Text.Length > 9)
                {
                    MaterielID = 品号T.Text;
                    数量T.Select();
                }
                if(数量T.Text == "")
                {
                    数量T.Text = "1";
                    数量T.SelectAll();
                }
                else
                {
                    数量T.SelectAll();
                }
            }
        }

        private void 数量_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && MsgFlag == true)
            {
                MsgFlag = false;
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    float.Parse(数量T.Text);
                    AddData();
                    GetIndex();
                }
                catch
                {
                    MsgFlag = true;
                    MessageBox.Show("数量输入错误！", "错误");
                    数量T.SelectAll();
                }
            }
            selectLastRow(this.DataGridView_List);
            DgvOpt.SetRowBackColor(this.DataGridView_List);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.DataGridView_List.SelectedRows)
            {
                this.DataGridView_List.Rows.Remove(item);
            }
            品号T.SelectAll();
            品号T.Select();
            SetEnable();
            GetIndex();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            Upload();
            SetEnable();
            GetIndex();
        }

        private void selectLastRow(DataGridView dgv = null)
        {
            int kk = dgv.RowCount;
            if (dgv.RowCount > 0 && dgv != null)
            {
                dgv.CurrentCell = dgv.Rows[dgv.RowCount - 1].Cells[0];
            }
        }
        #endregion

        #region 前端业务逻辑
        private string GetDate()
        {
            return dateTimePicker1.Value.ToString("yyyyMMdd");
        }

        private string GetFlowID(string flowId = null)
        {
            string time = mssql.SQLselect(connStr, "SELECT dbo.f_getTime(1) ").Rows[0][0].ToString();

            if (flowId == null)
            {
                flowId = "XH" + time + "0001";
            }
            else
            {
                if (mssql.SQLselect(connStr, string.Format("SELECT RTRIM(XHXA005) FROM dbo.XH_LYXA WHERE XHXA005 = '{0}' ", flowId)) == null)
                {
                    return GetFlowID(flowId);
                }
            }
            return flowId;
        }

        private string GetTime()
        {
            string sqlstr = "SELECT dbo.f_getTime(1) ";
            DataTable dt = mssql.SQLselect(connStr, sqlstr);
            if(dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        private DataTable GetMaterielInfo(string MaterielID, string SupplierID)
        {
            string sqlstr = @"SELECT RTRIM(TD004), RTRIM(MB002), RTRIM(MB003), RTRIM(TC004), SL FROM V_COPTD_SL_WG 
                                INNER JOIN INVMB ON MB001 = TD004 
                                WHERE TD004 = '{0}' AND TC004 = '{1}' ";
            DataTable dt = mssql.SQLselect(connStr, string.Format(sqlstr, MaterielID, SupplierID));
            if(dt != null)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        private bool GetMaterielExist(string MaterielID)
        {
            string sqlstr = "SELECT MB001 FROM INVMB WHERE MB001 = '{0}' ";
            DataTable dt = mssql.SQLselect(connStr, string.Format(sqlstr, MaterielID));
            if(dt != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetIndex()//重排dgv的序号并且设置部分列居中显示
        {
            if(DataGridView_List.Rows.Count > 0)
            {
                int Count = DataGridView_List.Rows.Count;
                int Index = 0;
                for(Index = 0; Index < Count; Index++)
                {
                    DataGridView_List.Rows[Index].Cells[0].Value = (Index + 1).ToString().PadLeft(3, '0');
                    DataGridView_List.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridView_List.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridView_List.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridView_List.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else
            {
                return;
            }
        }

        private void AddData()
        {
            if (MaterielID != null)
            {
                DataTable dt = GetMaterielInfo(MaterielID, SupplierID);
                if(dt != null)
                {
                    float shuliang = -2;
                    try
                    {
                        shuliang = float.Parse(数量T.Text);
                        try
                        {
                            float k = float.Parse(dt.Rows[0][4].ToString());
                            if (k >= shuliang)
                            {
                                //检查是否存在重复的品号
                                bool repeatFlag = false;
                                if (DataGridView_List.Rows.Count > 0) {
                                    for (int rowIdx = 0; rowIdx < DataGridView_List.Rows.Count; rowIdx ++)
                                    {
                                        if(MaterielID == DataGridView_List.Rows[rowIdx].Cells[1].Value.ToString())
                                        {
                                            repeatFlag = true;
                                        }
                                    }
                                }

                                if (!repeatFlag)//把数据写入dgv
                                {
                                    int index = this.DataGridView_List.Rows.Add();
                                    this.DataGridView_List.Rows[index].Cells[1].Value = MaterielID;
                                    this.DataGridView_List.Rows[index].Cells[2].Value = dt.Rows[0][1];
                                    this.DataGridView_List.Rows[index].Cells[3].Value = dt.Rows[0][2];
                                    this.DataGridView_List.Rows[index].Cells[4].Value = 数量T.Text;
                                    this.DataGridView_List.Rows[index].Cells[5].Value = PositionID;
                                    this.DataGridView_List.Rows[index].Cells[6].Value = SupplierID;
                                    
                                }
                                else
                                {
                                    MessageBox.Show("品号：" + MaterielID + "已存在待处理数据中，品号不能重复", "错误", MessageBoxButtons.OK);
                                }
                                品号T.SelectAll();
                                品号T.Select();
                            }
                            else
                            {
                                MsgFlag = true;
                                MessageBox.Show("此品号可交量为" + k.ToString() + "少于输入的数量。\n\r请重新输入", "错误");
                                数量T.Text = "";
                                数量T.SelectAll();
                                数量T.Select();
                            }
                        }
                        catch
                        {
                            MsgFlag = true;
                            MessageBox.Show("查询可交量量返回错误", "错误");
                            品号T.SelectAll();
                            品号T.Select();
                        }
                    }
                    catch
                    {
                        MsgFlag = true;
                        MessageBox.Show("数量输入错误", "错误");
                        数量T.Select();
                        数量T.SelectAll();
                    }
                }
                else
                {
                    if (GetMaterielExist(MaterielID))
                    {
                        MsgFlag = true;
                        MessageBox.Show("品号：" + MaterielID + " 可交量为零，请重新输入品号", "错误");
                        品号T.SelectAll();
                        品号T.Select();
                    }
                    else
                    {
                        MsgFlag = true;
                        MessageBox.Show("品号：" + MaterielID + " 存在错误，请重新输入品号", "错误");
                        品号T.SelectAll();
                        品号T.Select();
                    }
                }
                MaterielID = null;
            }
            else
            {
                if (品号T.Text != "")
                {
                    MaterielID = 品号T.Text;
                    数量T.Select();
                    AddData();
                }
                else
                {
                    MsgFlag = true;
                    MessageBox.Show("没有获取到品号信息", "错误");
                    品号T.Select();
                    品号T.SelectAll();
                }
            }
            SetEnable();
        }

        private void Upload()
        {
            string sql = "INSERT INTO XH_LYXA "
                        + "(COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, XHXA001, XHXA002, XHXA003, "
                        + "XHXA004, XHXA005, XHXA007, XHXA008, XHXA009, "
                        + "XHXA011, XHXA012, XHXA013, XHXA014, XHXA015, ID) "
                        + "VALUES('COMFORT3', '{0}', '{1}', '{2}', 1, '{3}', '{4}', '{5}', "
                        + "'{6}', '{7}', '{8}', '{9}', '{10}', 'N', 'N', "
                        + "'{11}', '********************', '{12}', '{13}')";
            string flowId = GetFlowID();
            string Time = GetTime();
            int Count = DataGridView_List.RowCount;

            for (int Index = 0; Index < Count; Index++)
            {
                string JHXA001 = TypeID;
                string JHXA002 = DataGridView_List.Rows[0].Cells[6].Value.ToString();
                string JHXA003 = DataGridView_List.Rows[0].Cells[5].Value.ToString();
                string JHXA004 = GetDate();
                string JHXA007 = DataGridView_List.Rows[0].Cells[1].Value.ToString();
                string JHXA008 = "";
                string JHXA009 = DataGridView_List.Rows[0].Cells[4].Value.ToString();
                string JHXA013 = "";
                string JHXA015 = "";
                string ID = (Index+1).ToString();
                string sqlstr = string.Format(sql, LoginUid, LoginUserGroup, Time, JHXA001, JHXA002, JHXA003, 
                    JHXA004, flowId, JHXA007, JHXA008, JHXA009, JHXA013, JHXA015, ID);

                mssql.SQLexcute(connStr, sqlstr);

                DataGridView_List.Rows.Remove(DataGridView_List.Rows[0]);
            }
            string tell = "\n\r处理流水号为：" + flowId;
            string sqlstrEXEC = @"EXEC dbo.P_COPTG_CREATE_WORK_WG '{0}'";
            DataTable dt = mssql.SQLselect(connStr, string.Format(sqlstrEXEC, flowId));
            if(dt != null)
            {
                if(dt.Rows[0][0].ToString() != "")
                {
                    tell += "\n\r销货单号为：" + dt.Rows[0][0].ToString();
                }
                if (dt.Rows[0][1].ToString() != "")
                {
                    tell += "\n\r发生错误，信息为：" + dt.Rows[0][1].ToString();
                }
            }

            if (MessageBox.Show(tell, "提示", MessageBoxButtons.OK) == DialogResult.OK)
            {
                MsgFlag = true;
                panel_Last.Enabled = false;
            }
        }

        private void SetEnable()
        {
            if (DataGridView_List.RowCount > 0)
            {
                panel_Last.Enabled = true;
                dateTimePicker1.Enabled = false;
                销货单别L.Enabled = false;
                销货仓库L.Enabled = false;
                客户L.Enabled = false;
            }
            if (DataGridView_List.RowCount <= 0)
            {
                panel_Last.Enabled = false;
                dateTimePicker1.Enabled = true;
                销货单别L.Enabled = true;
                销货仓库L.Enabled = true;
                客户L.Enabled = true;
            }

            float sum = 0;
            if (DataGridView_List.Rows.Count > 0)
            {
                for (int rowIdx = 0; rowIdx < DataGridView_List.Rows.Count; rowIdx++)
                {
                    sum += float.Parse(DataGridView_List.Rows[rowIdx].Cells[4].Value.ToString());
                }
            }
            label2.Text = "总数量：" + sum.ToString();
        }
        #endregion
    }
}
