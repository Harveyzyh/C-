using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.仓储中心
{
    public partial class 录入进货单_采购平台 : Form
    {

        #region 局部变量设定
        Mssql mssql = new Mssql();
        private static string connYF = FormLogin.infObj.connYF;
        ERP_Create_Purtg createPurtg = new ERP_Create_Purtg(connYF);
        
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public static string Mode = null;
        public static string Title = null;
        public static string GetMain = null;
        public static string GetOther = null;
        public static string MaterielMode = null;

        private string SupplierID = null;
        private string TypeID = "3401";
        private string PositionID = "P013";
        private string MaterielID = null;
        private string LoginUid = FormLogin.infObj.userId;
        private string LoginUserGroup = FormLogin.infObj.userGroup;

        private bool MsgFlag = false;
        private bool check = false;
        #endregion

        #region 窗口初始化
        public 录入进货单_采购平台(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Init();

            DgvOpt.SetRowBackColor(DataGridView_List);
            DgvOpt.SetColHeadMiddleCenter(DataGridView_List);
        }

        private void Init()
        {
            入库单别.Text = "3401-采购入库单";
            入库仓库.Text = "P013-仓储原材料仓";

            panel_Title.Enabled = true;
            panel_Last.Enabled = false;

            条码T.Select();
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

        private void ShowUI()
        {
            if (DataGridView_List.RowCount > 0)
            {
                panel_Last.Enabled = true;
                入库单别L.Enabled = false;
                入库仓库L.Enabled = false;
                dateTimePicker1.Enabled = false;
                送货单号T.Enabled = false;
                if (check)
                {
                    buttonUpload.Enabled = true;
                }
                else
                {
                    buttonUpload.Enabled = false;
                }
            }
            if (DataGridView_List.RowCount <= 0)
            {
                panel_Last.Enabled = false;
                入库单别L.Enabled = true;
                入库仓库L.Enabled = true;
                dateTimePicker1.Enabled = true;
                送货单号T.Enabled = true;
            }
        }

        #region UI按钮
        private void 入库仓库L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "PositionID";
            Title = "仓库编号";
            //Title = "仓库编号|仓库名称";
            录入进货单_获取单据信息 frm = new 录入进货单_获取单据信息(connYF, Title, Mode);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    GetMain = frm.GetMain;
                    GetOther = frm.GetOther;

                    入库仓库.Text = GetMain + "-" + GetOther;
                    PositionID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void 入库单别L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "TypeID";
            Title = "单别";
            //Title = "单别|单据简称";
            录入进货单_获取单据信息 frm = new 录入进货单_获取单据信息(connYF, Title, Mode);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                GetMain = frm.GetMain;
                GetOther = frm.GetOther;

                if (GetMain != null)
                {
                    入库单别.Text = GetMain + "-" + GetOther;
                    TypeID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void 送货单号_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && MsgFlag == true)
            {
                MsgFlag = false;
                return;
            }
            else
            {
                MsgFlag = false;
            }

            if ((MsgFlag == true) && (e.KeyCode == Keys.Enter))
            {
                MsgFlag = false;
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                条码T.Select();
            }
        }

        private void 条码_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && MsgFlag == true)
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
                while(DataGridView_List.Rows.Count > 0)
                {
                    DataGridView_List.Rows.RemoveAt(0);
                }

                条码T.Text = 条码T.Text.ToUpper();

                if(送货单号T.Text == "")
                {
                    送货单号T.Text = 条码T.Text;
                }

                if (入库单别.Text == "" || 入库仓库.Text == "" || 送货单号T.Text == "")
                {
                    MsgFlag = true;
                    MessageBox.Show("请先查询或填写 进货单别，供应商，入库仓库，送货单号 的信息！", "错误");
                    条码T.Select();
                    条码T.SelectAll();
                }
                else
                {
                    AddData();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            while (DataGridView_List.Rows.Count > 0)
            {
                DataGridView_List.Rows.RemoveAt(0);
            }
            送货单号T.Text = "";
            ShowUI();
            GetIndex();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            Upload();
            ShowUI();
            GetIndex();
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            check = Check();
            if (!check)
            {
                MessageBox.Show("存在异常，请检查", "错误", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("无异常", "提示", MessageBoxButtons.OK);
            }
            ShowUI();
        }

        private void DataGridView_List_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            check = false;
            ShowUI();
        }
        #endregion

        #region 前端业务逻辑
        private string GetDate()
        {
            return dateTimePicker1.Value.ToString("yyyyMMdd");
        }

        private string GetFlowID(string flowId = null)
        {
            string time = mssql.SQLselect(connYF, "SELECT dbo.f_getTime(1) ").Rows[0][0].ToString();

            if (flowId == null)
            {
                flowId = "JH" + time + "0001";
            }
            else
            {
                if (mssql.SQLselect(connYF, string.Format("SELECT RTRIM(JHXA005) FROM COMFORT..JH_LYXA WHERE JHXA005 = '{0}' ", flowId)) == null)
                {
                    return GetFlowID(flowId);
                }
            }
            return flowId;
        }

        private string GetTime()
        {
            string sqlstr = "SELECT dbo.f_getTime(1) ";
            DataTable dt = mssql.SQLselect(connYF, sqlstr);
            if (dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        private float GetDsl(string MaterielID, string SupplierID)
        {
            string sqlstr = "SELECT RTRIM(TD004), RTRIM(TC004), SL FROM V_PURTD_SL_WG WHERE TD004 = '{0}' AND TC004 = '{1}' ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlstr, MaterielID, SupplierID));
            if (dt != null)
            {
                try
                {
                    return float.Parse(dt.Rows[0][2].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private bool GetMaterielExist(string MaterielID)
        {
            string sqlstr = "SELECT MB001 FROM INVMB WHERE MB001 = '{0}' ";
            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlstr, MaterielID));
            if (dt != null)
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
            if (DataGridView_List.Rows.Count > 0)
            {
                int Count = DataGridView_List.Rows.Count;
                int Index = 0;
                for (Index = 0; Index < Count; Index++)
                {
                    DataGridView_List.Rows[Index].Cells[0].Value = (Index + 1).ToString().PadLeft(3, '0');
                    DataGridView_List.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridView_List.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridView_List.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridView_List.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridView_List.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else
            {
                return;
            }
        }

        //添加数据到dgv中显示
        private void AddData()
        {
            string sqlstr = @"select d.xh 序号, d.wlno 品号, RTRIM(MB002) 品名, RTRIM(MB003) 规格, sl 数量, h.SupId 供应商编号  
                                from WG_DB.dbo.CG_SupplyHead as h 
                                inner join WG_DB.dbo.CG_SupplyDetail as d on h.SupId = d.SupId and h.SendDate = d.SendDate and h.SendVersion = d.SendVersion
                                left join COMFORT.dbo.INVMB on wlno = MB001
                                where h.SupId + '-'+ h.SendDate + '-'+ h.SendVersion = '{0}' 
                                order by d.xh";

            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlstr, 条码T.Text));

            if (dt != null)
            {
                for (int index = 0; index < dt.Rows.Count; index++)
                {

                    int row = this.DataGridView_List.Rows.Add();
                    this.DataGridView_List.Rows[row].Cells[0].Value = dt.Rows[index][0].ToString();
                    this.DataGridView_List.Rows[row].Cells[1].Value = dt.Rows[index][1].ToString();
                    this.DataGridView_List.Rows[row].Cells[2].Value = dt.Rows[index][2].ToString();
                    this.DataGridView_List.Rows[row].Cells[3].Value = dt.Rows[index][3].ToString();
                    this.DataGridView_List.Rows[row].Cells[4].Value = dt.Rows[index][4].ToString();
                    this.DataGridView_List.Rows[row].Cells[5].Value = PositionID;
                    this.DataGridView_List.Rows[row].Cells[6].Value = dt.Rows[index][5].ToString();
                    this.DataGridView_List.Rows[row].Cells[7].Value = dt.Rows[index][5].ToString();
                    this.DataGridView_List.Rows[row].Cells[8].Value = 送货单号T.Text;
                }
            }
            else
            {
                MessageBox.Show("该条码不存在", "错误");
                送货单号T.Text = "";
            }

            ShowUI();
        }

        //把数据上传至后台临时表，并生成进货单
        private void Upload()
        {
            string sql = "INSERT INTO JH_LYXA "
                        + "(COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, JHXA001, JHXA002, JHXA003, "
                        + "JHXA004, JHXA005, JHXA007, JHXA008, JHXA009, "
                        + "JHXA011, JHXA012, JHXA013, JHXA014, JHXA015, ID) "
                        + "VALUES('COMFORT', '{0}', '{1}', '{2}', 1, '{3}', '{4}', '{5}', "
                        + "'{6}', '{7}', '{8}', '{9}', '{10}', 'N', 'N', "
                        + "'{11}', '********************', '{12}', '{13}')";
            string flowId = GetFlowID();
            string Time = GetTime();
            int Count = DataGridView_List.RowCount;

            for (int Index = 0; Index < Count; Index++)
            {
                string JHXA001 = TypeID;
                string JHXA002 = DataGridView_List.Rows[0].Cells[7].Value.ToString();
                string JHXA003 = DataGridView_List.Rows[0].Cells[5].Value.ToString();
                string JHXA004 = GetDate();
                string JHXA007 = DataGridView_List.Rows[0].Cells[1].Value.ToString();
                string JHXA008 = "";
                string JHXA009 = DataGridView_List.Rows[0].Cells[4].Value.ToString();
                string JHXA013 = DataGridView_List.Rows[0].Cells[8].Value.ToString();
                string JHXA015 = DataGridView_List.Rows[0].Cells[7].Value.ToString();
                string ID = (Index + 1).ToString();
                string sqlstr = string.Format(sql, LoginUid, LoginUserGroup, Time, JHXA001, JHXA002, JHXA003,
                    JHXA004, flowId, JHXA007, JHXA008, JHXA009, JHXA013, JHXA015, ID);

                mssql.SQLexcute(connYF, sqlstr);

                DataGridView_List.Rows.Remove(DataGridView_List.Rows[0]);
            }
            string tell = "\n\r处理流水号为：" + flowId;

            //执行生成进货单程序
            //string getBackStr = "";
            string getBackStr = createPurtg.HandelDef(flowId);

            //修改采购平台单头的标志位
            UptCgPlatformFlag();

            if (getBackStr != null)
            {
                tell += "\n\r进货单号为：" + getBackStr;
            }
            else
            {
                tell += "\n\r处理失败，请把记下流水处理号并联系咨询部。";
            }
            if (MessageBox.Show(tell, "提示", MessageBoxButtons.OK) == DialogResult.OK)
            {
                MsgFlag = true;
                送货单号T.SelectAll();
                送货单号T.Select();
                panel_Last.Enabled = false;
            }
        }

        //检查数据是否正确
        private bool Check()
        {
            bool flag = true;

            for(int index=0; index< DataGridView_List.Rows.Count; index++)
            {
                float sl = 0;
                try
                {
                    sl = float.Parse(DataGridView_List.Rows[index].Cells[4].Value.ToString());
                }
                catch
                {
                    DataGridView_List.Rows[index].Cells[9].Value = "数量输入不正确";
                    flag = false;
                }

                if(DataGridView_List.Rows[index].Cells[2].Value.ToString() == "")
                {
                    DataGridView_List.Rows[index].Cells[9].Value = "品号信息不正确";
                    flag = false;
                }

                float dsl = GetDsl(DataGridView_List.Rows[index].Cells[1].Value.ToString(), DataGridView_List.Rows[index].Cells[6].Value.ToString());
                if(dsl != 0)
                {
                    if(dsl < sl)
                    {
                        DataGridView_List.Rows[index].Cells[9].Value = "可验收量为" + dsl.ToString() + "，少于需入库数量";
                        DataGridView_List.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                        flag = false;
                    }
                    else
                    {
                        DataGridView_List.Rows[index].Cells[9].Value = "";
                        DataGridView_List.Rows[index].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                else
                {
                    DataGridView_List.Rows[index].Cells[9].Value = "可验收量为0";
                    DataGridView_List.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    flag = false;
                }
            }
            return flag;
        }

        private void UptCgPlatformFlag()
        {
            string sqlstr = "update WG_DB.dbo.CG_SupplyHead set ScanFlag = 1 where SupId + '-'+ SendDate + '-'+ SendVersion = '{0}' ";
            mssql.SQLexcute(connYF, string.Format(sqlstr, 条码T.Text));
        }

        #endregion
    }
}
