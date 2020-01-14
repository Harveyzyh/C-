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
    public partial class PDA_扫描进货单 : Form
    {
        #region 公用变量设定

        public static string strConnection = Global_Const.strConnection_COMFORT;
        #endregion

        #region 局部变量设定
        Mssql mssql = new Mssql();

        Dictionary<string, string> dict = new Dictionary<string, string> { };
        Dictionary<string, string> dict_get = new Dictionary<string, string> { };
        public static string Mode = null;
        public static string URL = FormLogin.HttpURL + "/Client/PDA/JH_LYXA";
        public static string Title = null;
        public static string GetMain = null;
        public static string GetOther = null;
        public static string MaterielMode = null;

        private string SupplierID = null;
        private string TypeID = "3401";
        private string PositionID = "P013";
        private string MaterielID = null;
        private string LoginUid = FormLogin.Login_Uid;
        private string LoginUserGroup = null;

        private bool MsgFlag = false;
        #endregion

        #region 窗口初始化
        public PDA_扫描进货单()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //if (FormLogin.URLTestFlag)
            //{
            //    strConnection = Global_Const.strConnection_COMFORT_TEST;
            //    label1.Text = "DEBUG模式";
            //}
            入库单别.Text = "3401-采购入库单";
            入库仓库.Text = "P013-仓储原材料仓";

            panel_Title.Enabled = true;
            panel_Last.Enabled = false;

            供应商查.Select();
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
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            DataGridView_List.Location = new Point(0, panel_Title.Height + 3);
            DataGridView_List.Size = new Size(FormWidth, FormHeight - panel_Title.Height - panel_Last.Height);
            panel_Last.Location = new Point(0, panel_Title.Height + DataGridView_List.Height);
            panel_Last.Size = new Size(FormWidth, panel_Last.Height);
        }
        #endregion

        #region UI按钮
        private void 送货单别查_Click(object sender, EventArgs e)
        {
            Mode = "TypeID";
            Title = "单别";
            //Title = "单别|单据简称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if(frm.ShowDialog() == DialogResult.Cancel)
            {
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

        private void 供应商查_Click(object sender, EventArgs e)
        {
            Mode = "SupplierID";
            Title = "供应商编号";
            //Title = "供应商编号|简称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    供应商.Text = GetMain + "-" + GetOther;
                    SupplierID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
                MsgFlag = true;
                送货单号T.Select();
                送货单号T.SelectAll();
            }
        }

        private void 入库仓库查_Click(object sender, EventArgs e)
        {
            Mode = "PositionID";
            Title = "仓库编号";
            //Title = "仓库编号|仓库名称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    入库仓库.Text = GetMain + "-" + GetOther;
                    PositionID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }
        
        private void 入库仓库L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "PositionID";
            Title = "仓库编号";
            //Title = "仓库编号|仓库名称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    入库仓库.Text = GetMain + "-" + GetOther;
                    PositionID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void 供应商L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "SupplierID";
            Title = "供应商编号";
            //Title = "供应商编号|简称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    供应商.Text = GetMain + "-" + GetOther;
                    SupplierID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
                MsgFlag = true;
                送货单号T.Select();
                送货单号T.SelectAll();
            }
        }

        private void 入库单别L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "TypeID";
            Title = "单别";
            //Title = "单别|单据简称";
            PDA_扫描进货单_获取单据信息 frm = new PDA_扫描进货单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
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
                条码T.Text = 条码T.Text.ToUpper();
                if (入库单别.Text == "" || 供应商.Text == "" || 入库仓库.Text == "" || 送货单号T.Text == "")
                {
                    MsgFlag = true;
                    MessageBox.Show("请先查询或填写 进货单别，供应商，入库仓库，送货单号 的信息！", "错误");
                    条码T.Select();
                    条码T.SelectAll();
                }
                else if(条码T.Text.Length > 9)
                {
                    if(!CheckSupplierID(条码T.Text.Substring(条码T.Text.Length-5, 5)))
                    {
                        MsgFlag = true;
                        MessageBox.Show("条码中的供应商编码与选择的不对应！请检测条码。", "错误");
                        条码T.Select();
                        条码T.SelectAll();
                    }
                    else
                    {
                        MaterielID = 条码T.Text.Substring(0, 条码T.Text.Length - 5);
                        数量T.Select();
                    }
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
            DgvOpt.SetRowColor(this.DataGridView_List);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.DataGridView_List.SelectedRows)
            {
                this.DataGridView_List.Rows.Remove(item);
            }
            条码T.SelectAll();
            条码T.Select();
            SetEnable();
            GetIndex();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            Insert();
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
            string time = mssql.SQLselect(strConnection, "SELECT (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', ''))").Rows[0][0].ToString();

            if (flowId == null)
            {
                flowId = "JH" + time + "0001";
            }
            else
            {
                if (mssql.SQLselect(strConnection, string.Format("SELECT RTRIM(JHXA005) FROM COMFORT..JH_LYXA WHERE JHXA005 = '{0}' ", flowId)) == null)
                {
                    return GetFlowID(flowId);
                }
            }
            return flowId;
        }

        private string GetTime()
        {
            string sqlstr = "SELECT (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', ''))";
            DataTable dt = mssql.SQLselect(strConnection, sqlstr);
            if(dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        private void GetUserGroup()
        {
            string sql = "SELECT MF004 FROM ADMMF WHERE MF001 = '{0}' ";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sql, LoginUid));
            if(dt != null)
            {
                LoginUserGroup = dt.Rows[0][0].ToString();
            }
            else
            {
                LoginUserGroup = "";
            }
        }

        private DataTable GetMaterielInfo(string MaterielID, string SupplierID)
        {
            string sqlstr = "SELECT RTRIM(TD004), RTRIM(MB002), RTRIM(MB003), RTRIM(TC004), PCBSum FROM VPURTD_ZYH "
                            + "INNER JOIN INVMB ON MB001 = TD004 "
                            + "WHERE TD004 = '{0}' AND TC004 = '{1}' ";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, MaterielID, SupplierID));
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
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, MaterielID));
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
                    DataGridView_List.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                                int index = this.DataGridView_List.Rows.Add();
                                this.DataGridView_List.Rows[index].Cells[1].Value = MaterielID;
                                this.DataGridView_List.Rows[index].Cells[2].Value = dt.Rows[0][1];
                                this.DataGridView_List.Rows[index].Cells[3].Value = dt.Rows[0][2];
                                this.DataGridView_List.Rows[index].Cells[4].Value = 数量T.Text;
                                this.DataGridView_List.Rows[index].Cells[5].Value = PositionID;
                                this.DataGridView_List.Rows[index].Cells[6].Value = SupplierID;
                                this.DataGridView_List.Rows[index].Cells[7].Value = SupplierID;
                                this.DataGridView_List.Rows[index].Cells[8].Value = 送货单号T.Text;
                                this.DataGridView_List.Rows[index].Cells[9].Value = MaterielID + SupplierID;
                                条码T.SelectAll();
                                条码T.Select();
                            }
                            else
                            {
                                MsgFlag = true;
                                MessageBox.Show("此条码可入库数量为" + k.ToString() + "少于输入的数量。\n\r请重新输入", "错误");
                                数量T.Text = "";
                                数量T.SelectAll();
                                数量T.Select();
                            }
                        }
                        catch
                        {
                            MsgFlag = true;
                            MessageBox.Show("查询未进货量返回错误", "错误");
                            条码T.SelectAll();
                            条码T.Select();
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
                        MessageBox.Show("品号：" + MaterielID + " 可入库数量为零，请重新输入条码", "错误");
                        条码T.SelectAll();
                        条码T.Select();
                    }
                    else
                    {
                        MsgFlag = true;
                        MessageBox.Show("品号：" + MaterielID + " 存在错误，请重新输入条码", "错误");
                        条码T.SelectAll();
                        条码T.Select();
                    }
                }
                MaterielID = null;
            }
            else
            {
                if (条码T.Text != "")
                {
                    if (!CheckSupplierID(条码T.Text.Substring(条码T.Text.Length - 5, 5)))
                    {
                        MsgFlag = true;
                        MessageBox.Show("条码中的供应商编码与选择的不对应！请检测条码。", "错误");
                        条码T.Select();
                        条码T.SelectAll();
                    }
                    else
                    {
                        MaterielID = 条码T.Text.Substring(0, 条码T.Text.Length - 5);
                        数量T.Select();
                        AddData();
                    }
                }
                else
                {
                    MsgFlag = true;
                    MessageBox.Show("没有获取到品号信息", "错误");
                    条码T.Select();
                    条码T.SelectAll();
                }
            }
            SetEnable();
        }

        private void Insert()
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
            GetUserGroup();
            int Count = DataGridView_List.RowCount;

            for (int Index = 0; Index < Count; Index++)
            {
                string JHXA001 = TypeID;
                string JHXA002 = DataGridView_List.Rows[0].Cells[7].Value.ToString();
                string JHXA003 = DataGridView_List.Rows[0].Cells[5].Value.ToString();
                string JHXA004 = GetDate();
                string JHXA007 = DataGridView_List.Rows[0].Cells[1].Value.ToString();
                string JHXA008 = DataGridView_List.Rows[0].Cells[9].Value.ToString();
                string JHXA009 = DataGridView_List.Rows[0].Cells[4].Value.ToString();
                string JHXA013 = DataGridView_List.Rows[0].Cells[8].Value.ToString();
                string JHXA015 = DataGridView_List.Rows[0].Cells[7].Value.ToString();
                string ID = (Index+1).ToString();
                string sqlstr = string.Format(sql, LoginUid, LoginUserGroup, Time, JHXA001, JHXA002, JHXA003, 
                    JHXA004, flowId, JHXA007, JHXA008, JHXA009, JHXA013, JHXA015, ID);

                mssql.SQLexcute(strConnection, sqlstr);

                DataGridView_List.Rows.Remove(DataGridView_List.Rows[0]);
            }
            string tell = "\n\r处理流水号为：" + flowId;
            //string getBackStr = Upload(FlowId, Count.ToString());
            string getBackStr = HandelDef(flowId);
            if(getBackStr != null)
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

        private string Upload(string FlowId, string Count)
        {
            Dictionary<string, string> dict = new Dictionary<string, string> { };
            dict.Add("Uid", LoginUid);
            dict.Add("Mode", "Complete");
            dict.Add("Parameter", FlowId);
            dict.Add("Data", "");
            dict.Add("RowCount", Count);
            dict = HttpPost.HttpPost_Dict(URL, dict, 120);
            string GetBack = null;
            if (dict != null)
            {
                dict.TryGetValue("Data", out GetBack);
            }
            return GetBack;
        }

        private void SetEnable()
        {
            if (DataGridView_List.RowCount > 0)
            {
                panel_Last.Enabled = true;
                入库单别查.Enabled = false;
                入库仓库查.Enabled = false;
                供应商查.Enabled = false;
                dateTimePicker1.Enabled = false;
                送货单号T.Enabled = false;
            }
            if (DataGridView_List.RowCount <= 0)
            {
                panel_Last.Enabled = false;
                入库单别查.Enabled = true;
                入库仓库查.Enabled = true;
                供应商查.Enabled = true;
                dateTimePicker1.Enabled = true;
                送货单号T.Enabled = true;
            }
        }

        private bool CheckSupplierID(string Supplier)
        {
            if(SupplierID == Supplier)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 后端业务逻辑
        private string HandelDef(string flowID)
        {
            HeadObject headObj = new HeadObject();
            headObj.FlowId = flowID;

            GetHeadInfo(headObj);
            GetHeadTG002(headObj);
            GetHeadTime(headObj);
            SetHeadInfo(headObj);
            SetDetailDef(headObj);
            SetDetailMoney(headObj);
            SetHeadMoney(headObj);
            UptJHXAInfo(headObj);
            return headObj.TG001 + '-' + headObj.TG002;
        }

        private void GetHeadTime(HeadObject headObj)
        {
            string sqlstr = "SELECT (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', ''))";
            DataTable dt = mssql.SQLselect(strConnection, sqlstr);
            if (dt != null)
            {
                headObj.Time = dt.Rows[0][0].ToString();
            }
        }

        private void GetHeadInfo(HeadObject headObj)
        {
            string sqlstr = @"SELECT DISTINCT RTRIM(JHXA.COMPANY) 公司别, RTRIM(JHXA.CREATOR) 创建人, RTRIM(JHXA.USR_GROUP) 用户组, 
                                RTRIM(JHXA001) 进货单别, RTRIM(JHXA004) 进货日期, RTRIM(JHXA002) 供应商编号, RTRIM(JHXA013) 送货单号, 
                                RTRIM(MA021) 交易币种, MG2.MG003 汇率, 
                                RTRIM(MA030) 发票种类, 
                                (CASE WHEN NOT (TC018 = '' OR TC018 IS NULL) THEN TC018 
                                ELSE (CASE WHEN MA044 ='' OR MA044 IS NULL THEN '1' ELSE MA044 END ) END) AS TC018C, 
                                RTRIM(MA003) 供应商全称, 
                                (CASE WHEN TC026 IS NULL THEN MA064 ELSE TC026 END) AS TC026C, 
                                (CASE WHEN TC027 = '' OR TC027 IS NULL THEN MA055 ELSE TC027 END) AS TC027C 
                                FROM COMFORT.dbo.JH_LYXA AS JHXA 
                                LEFT JOIN COMFORT.dbo.PURTC AS PURTC ON 1=2 
                                LEFT JOIN COMFORT.dbo.INVMB AS INVMB ON MB001=JHXA007 
                                LEFT JOIN COMFORT.dbo.PURMA AS PURMA ON MA001=JHXA002 
                                LEFT JOIN (SELECT CMSMG.MG003, CMSMG.MG001 FROM COMFORT.dbo.CMSMG 
                                INNER JOIN (SELECT MAX(MG002) MAXMG02, MG001 MAXMG01 FROM CMSMG GROUP BY MG001) AS MG 
                                ON MG.MAXMG01 = CMSMG.MG001 AND MG.MAXMG02 = CMSMG.MG002) AS MG2 ON MG2.MG001 = MA021 
                                WHERE JHXA005 IN ('{0}') AND JHXA011 = 'N' ";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, headObj.FlowId));
            if (dt != null)
            {
                headObj.Company = dt.Rows[0][0].ToString();
                headObj.Uid = dt.Rows[0][1].ToString();
                headObj.Ugroup = dt.Rows[0][2].ToString();
                headObj.TG001 = dt.Rows[0][3].ToString();
                headObj.TG003 = dt.Rows[0][4].ToString();
                headObj.TG005 = dt.Rows[0][5].ToString();
                headObj.TG006 = dt.Rows[0][6].ToString();
                headObj.TG007 = dt.Rows[0][7].ToString();
                headObj.TG008 = dt.Rows[0][8].ToString();
                headObj.TG009 = dt.Rows[0][9].ToString();
                headObj.TG010 = dt.Rows[0][10].ToString();
                headObj.TG014 = headObj.TG003;
                headObj.TG021 = dt.Rows[0][11].ToString();
                headObj.TG030 = dt.Rows[0][12].ToString();
                headObj.TG033 = dt.Rows[0][13].ToString();
            }
        }

        private void GetHeadTG002(HeadObject headObj)
        {
            string sqlstr = @"SELECT (CASE WHEN A1 IS NULL THEN A2 + '0001' ELSE A1 END ) B FROM 
                                (SELECT MAX(TG002) + 1 A1, SUBSTRING(CONVERT(VARCHAR(10), GETDATE(), 112), 3, 4) A2 
                                FROM PURTG 
                                WHERE TG001 = '{0}' AND SUBSTRING(TG002, 1, 4) = 
                                SUBSTRING(CONVERT(VARCHAR(10), GETDATE(), 112), 3, 4)) A";
            DataTable dt = mssql.SQLselect(strConnection, string.Format(sqlstr, headObj.TG001));
            if (dt != null)
            {
                headObj.TG002 = dt.Rows[0][0].ToString();
            }
        }

        private void SetHeadInfo(HeadObject headObj)
        {
            string sqlstr = @"INSERT INTO PURTG (COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, TG001, TG002, TG003, TG004, TG005, 
                                TG006, TG007, TG008, TG009, TG010, TG013, TG014, TG015, TG021, TG030, TG033, TG016, TG043, TG052) 
                                VALUES('{0}', '{1}', '{2}', '{3}', '1', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', 
                                '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '', '', '' )";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.Company, headObj.Uid, headObj.Ugroup, headObj.Time, headObj.TG001, 
                headObj.TG002, headObj.TG003, headObj.TG004, headObj.TG005, headObj.TG006, headObj.TG007, headObj.TG008, headObj.TG009, 
                headObj.TG010, headObj.TG013, headObj.TG014, headObj.TG015, headObj.TG021, headObj.TG030, headObj.TG033));
        }

        private void SetDetailDef(HeadObject headObj)
        {
            //if (headObj.TG005 == "A0263")
            //{
            //    SetDetailA0263Def(headObj);
            //}
            //else
            //{
            //    SetDetailDefaultDef(headObj);
            //}
            SetDetailDefaultDef(headObj);
        }

        private void SetDetailDefaultDef(HeadObject headObj)
        {
            DetailObject detailObj = new DetailObject();
            GetLsDt(headObj, detailObj);
            if (detailObj.LsDt != null)
            {
                for(int lsRowIndex = 0; lsRowIndex < detailObj.LsDt.Rows.Count; lsRowIndex++)
                {
                    try
                    {
                        detailObj.TH004 = detailObj.LsDt.Rows[lsRowIndex][0].ToString();
                        detailObj.TH005 = detailObj.LsDt.Rows[lsRowIndex][1].ToString();
                        detailObj.TH006 = detailObj.LsDt.Rows[lsRowIndex][2].ToString();
                        detailObj.TH008 = detailObj.LsDt.Rows[lsRowIndex][3].ToString();
                        detailObj.TH009 = detailObj.LsDt.Rows[lsRowIndex][4].ToString();
                        detailObj.Total = float.Parse(detailObj.LsDt.Rows[lsRowIndex][5].ToString());

                        detailObj.TH010 = headObj.TG005;
                        detailObj.TH014 = headObj.TG003;
                        GetSlDt(headObj, detailObj);
                        GetNumber(headObj, detailObj);
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            }

        }

        private void GetLsDt(HeadObject headObj, DetailObject detailObj)
        {
            string sqlstr = @"SELECT RTRIM(MB001), RTRIM(MB002), RTRIM(MB003), RTRIM(MB004), 
                                RTRIM(JHXA003), RTRIM(JHXA009) FROM INVMB 
                                INNER JOIN JH_LYXA ON JHXA007 = MB001 WHERE 1=1 AND JHXA005 = '{0}' ORDER BY ID";
            detailObj.LsDt = mssql.SQLselect(strConnection, string.Format(sqlstr, headObj.FlowId));
        }

        private void GetSlDt(HeadObject headObj, DetailObject detailObj)
        {
            string sqlstr = @"SELECT DISTINCT TOP 200 TD008 - TD015 - ( SELECT isnull( SUM ( TH007 ), 0 ) 
                                FROM COMFORT.dbo.PURTH PURTH WHERE TH011 = TD001 AND TH012 = TD002 AND TH013 = TD003 
                                AND TH030 = 'N' ) AS WJL, 
                                TD001, RTRIM(TD002) AS TD002, TD003, (CASE WHEN TD010 IS NULL THEN 0 ELSE TD010 END) AS TD010, TD014, RTRIM(TD020) AS TD020, 
                                RTRIM(TD022) AS TD022, RTRIM(TDC03) AS TDC03, TC024 
                                FROM COMFORT.dbo.PURTD AS PURTD 
                                LEFT JOIN COMFORT.dbo.PURTC AS PURTC ON TC001 = TD001 AND TC002 = TD002 
                                WHERE TC004 = '{0}' AND TD004 = '{1}' 
                                AND (TD008 - TD015 - ( SELECT isnull( SUM ( TH007 ), 0 ) FROM COMFORT.dbo.PURTH PURTH 
                                WHERE TH011 = TD001 AND TH012 = TD002 AND TH013 = TD003 AND TH030 = 'N' )) > 0 
                                AND TD016 = 'N' AND TC014 = 'Y' AND TC001 <> '3305' AND TC001 <> '3306' 
                                ORDER BY TC024, TD001 DESC, RTRIM(TD002), TD003 ";
            detailObj.SlDt = mssql.SQLselect(strConnection, string.Format(sqlstr, headObj.TG005, detailObj.TH004));
        }

        private void GetNumber(HeadObject headObj, DetailObject detailObj)
        {
            if (detailObj.SlDt != null && detailObj.SlDt.Rows.Count != 0)
            {
                detailObj.RowIndex++;

                detailObj.TH011 = detailObj.SlDt.Rows[0][1].ToString();
                detailObj.TH012 = detailObj.SlDt.Rows[0][2].ToString();
                detailObj.TH013 = detailObj.SlDt.Rows[0][3].ToString();
                detailObj.TH018 = Math.Round(float.Parse(detailObj.SlDt.Rows[0][4].ToString()), 6, MidpointRounding.AwayFromZero).ToString();
                detailObj.TH033 = detailObj.SlDt.Rows[0][5].ToString();
                detailObj.TH035 = detailObj.SlDt.Rows[0][6].ToString();
                detailObj.TH042 = detailObj.SlDt.Rows[0][7].ToString();
                detailObj.THC02 = detailObj.SlDt.Rows[0][8].ToString();

                if(float.Parse(detailObj.SlDt.Rows[0][0].ToString()) >= detailObj.Total)
                {
                    detailObj.TH007 = Math.Round(detailObj.Total, 6, MidpointRounding.AwayFromZero).ToString();
                    SetDetailInfo(headObj, detailObj);
                }
                else
                {
                    float sl = float.Parse(detailObj.SlDt.Rows[0][0].ToString());
                    detailObj.TH007 = Math.Round(sl, 6, MidpointRounding.AwayFromZero).ToString();
                    detailObj.Total -= sl;
                    SetDetailInfo(headObj, detailObj);
                    detailObj.SlDt.Rows.RemoveAt(0);
                    GetNumber(headObj, detailObj);
                }
            }
        }

        private void SetDetailInfo(HeadObject headObj, DetailObject detailObj)
        {
            detailObj.TH015 = detailObj.TH007;
            detailObj.TH016 = detailObj.TH007;
            detailObj.TH034 = detailObj.TH007;
            detailObj.TH064 = detailObj.TH008;
            detailObj.TH065 = detailObj.TH008;
            detailObj.TH019 = Math.Round(float.Parse(detailObj.TH007) * float.Parse(detailObj.TH018), 6, MidpointRounding.AwayFromZero).ToString();
            detailObj.TH003 = detailObj.RowIndex.ToString().PadLeft(4, '0');

            string sqlstr = @"INSERT INTO PURTH(COMPANY,CREATOR,USR_GROUP,CREATE_DATE,FLAG, 
                                TH001,TH002,TH003,TH004,TH005,TH006,TH007,TH008,TH009,TH010, 
                                TH011,TH012,TH013,TH014,TH015,TH016,TH018,TH019,TH026,TH027, 
                                TH029,TH030,TH031,TH032,TH033,TH034,TH035,TH042,TH043,TH044, 
                                TH060,TH064,TH065,TH071,TH072,THC02) 
                                VALUES('{0}','{1}','{2}','{3}',1,'{4}','{5}','{6}', 
                                '{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}', 
                                '{18}','{19}','{20}','{21}','N','{22}','N','N','N','N','{23}', 
                                '{24}','{25}','{26}','N','N','0','{27}','{28}','1','##########','{29}')";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.Company, headObj.Uid, headObj.Ugroup, headObj.Time, headObj.TG001, headObj.TG002,
                detailObj.TH003, detailObj.TH004, detailObj.TH005, detailObj.TH006, detailObj.TH007, detailObj.TH008, detailObj.TH009, detailObj.TH010,
                detailObj.TH011, detailObj.TH012, detailObj.TH013, detailObj.TH014, detailObj.TH015, detailObj.TH016, detailObj.TH018, detailObj.TH019,
                detailObj.TH027, detailObj.TH033, detailObj.TH034, detailObj.TH035, detailObj.TH042, detailObj.TH064, detailObj.TH065, detailObj.THC02));
        }

        private void SetDetailA0263Def(HeadObject headObj)
        {

        }

        private void SetDetailMoney(HeadObject headObj)
        {
            string sqlstr = @"UPDATE COMFORT.dbo.PURTH  SET 
                                TH045 = CAST(ROUND(TH019/(1+CONVERT(FLOAT, TG030)),2) AS  NUMERIC(10,2)), 
                                TH046 = CAST(ROUND(TH019 - (TH019/(1+CONVERT(FLOAT, TG030))),2) AS  NUMERIC(10,2)), 
                                TH047 = CAST(ROUND((TH019 * CONVERT(FLOAT, TG008)/(1+CONVERT(FLOAT, TG030))),2) 
                                AS  NUMERIC(10,2)), 
                                TH048 = CAST(ROUND((TH019 * CONVERT(FLOAT, TG008)) - 
                                (TH019 * CONVERT(FLOAT, TG008)/(1+CONVERT(FLOAT, TG030))),2) AS  NUMERIC(10,2)) 
                                FROM PURTH INNER JOIN COMFORT.dbo.PURTG AS PURTG ON TG001 = TH001 AND TG002 = TH002 
                                WHERE TG001= '{0}' AND TG002= '{1}' ";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.TG001, headObj.TG002));
        }
        
        private void SetHeadMoney(HeadObject headObj)
        {
            string sqlstr = @"UPDATE A SET TG017=SUMTH019,TG019=SUMTH046,TG026=SUMTH015,TG028=SUMTH045,TG031=SUMTH047, 
                                TG032=SUMTH048,TG040=SUMTH050,TG041=SUMTH052,TG053=SUMTH007,TG054=SUMTH049 
                                FROM COMFORT.dbo.PURTG A 
                                INNER JOIN (SELECT TH001,TH002,SUMTH019=SUM(TH019),SUMTH046=SUM(TH046), 
                                SUMTH007=SUM(CASE WHEN MA024='2' THEN FLOOR(TH007) ELSE TH007 END), 
                                SUMTH015=SUM(CASE WHEN MA024='2' THEN FLOOR(TH015) ELSE TH015 END),SUMTH045=SUM(TH045), 
                                SUMTH047=SUM(TH047),SUMTH048=SUM(TH048),SUMTH050=SUM(TH050),SUMTH052=SUM(TH052), 
                                SUMTH049=SUM(TH049) 
                                FROM COMFORT.dbo.PURTH 
                                INNER JOIN COMFORT.dbo.CMSMA ON 1=1 
                                GROUP BY TH001,TH002)  AS B ON A.TG001=B.TH001 AND A.TG002=B.TH002 
                                WHERE TG001= '{0}' AND TG002= '{1}' ";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.TG001, headObj.TG002));
        }

        private void UptJHXAInfo(HeadObject headObj)
        {
            string time = mssql.SQLselect(strConnection, "SELECT " + Normal.GetSysTimeStr("Long")).Rows[0][0].ToString();
            string sqlstr = @"UPDATE COMFORT.dbo.JH_LYXA SET MODIFIER='{1}', MODI_DATE='{2}', 
                                FLAG=(convert(int,COMFORT.dbo.JH_LYXA.FLAG))%999+1, JHXA011 = 'Y', UDF01 = '{3}'WHERE  JHXA005 = '{0}' ";
            mssql.SQLexcute(strConnection, string.Format(sqlstr, headObj.FlowId, headObj.Uid, time, headObj.TG001 + '-' + headObj.TG002));
        }
        #endregion
    }

    #region 数据对象基类
    class HeadObject
    {
        private string flowId = null;
        private string company = null;
        private string uid = null;
        private string ugroup = null;
        private string time = null;

        private string tg001 = null; // 单别
        private string tg002 = null; // 单号
        private string tg003 = null; // 进货日期
        private string tg004 = "01"; // 工厂编号
        private string tg005 = null; // 供应商编号
        private string tg006 = null; // 送货单号
        private string tg007 = null; // 币种
        private string tg008 = null; // 汇率
        private string tg009 = null; // 发票种类
        private string tg010 = null; // 税种
        private string tg013 = "N"; // 审核码
        private string tg014 = null;  // 单据日期=进货日期
        private string tg015 = "N";  // 更新码
		private string tg021 = null;  // 供应商全称
		private string tg030 = null;  // 增值税率
		private string tg033 = null;  // 付款条件编号

        public string FlowId { get { return flowId; } set { flowId = value; } }
        public string Company { get { return company; } set { company = value; } }
        public string Uid { get { return uid; } set { uid = value; } }
        public string Ugroup { get { return ugroup; } set { ugroup = value; } }
        public string Time { get { return time; } set { time = value; } }

        public string TG001 { get { return tg001; } set { tg001 = value; } }
        public string TG002 { get { return tg002; } set { tg002 = value; } }
        public string TG003 { get { return tg003; } set { tg003 = value; } }
        public string TG004 { get { return tg004; } }
        public string TG005 { get { return tg005; } set { tg005 = value; } }
        public string TG006 { get { return tg006; } set { tg006 = value; } }
        public string TG007 { get { return tg007; } set { tg007 = value; } }
        public string TG008 { get { return tg008; } set { tg008 = value; } }
        public string TG009 { get { return tg009; } set { tg009 = value; } }
        public string TG010 { get { return tg010; } set { tg010 = value; } }
        public string TG013 { get { return tg013; } }
        public string TG014 { get { return tg014; } set { tg014 = value; } }
        public string TG015 { get { return tg015; } }
        public string TG021 { get { return tg021; } set { tg021 = value; } }
        public string TG030 { get { return tg030; } set { tg030 = value; } }
        public string TG033 { get { return tg033; } set { tg033 = value; } }
    }

    class DetailObject
    {
        private int rowIndex = 0; //写入行序号

        private float total = 0; //需入库数量

        private DataTable lsDt = null;
        private DataTable slDt = null;

        private string th003 = null; //序号
        private string th004 = null; //品号
        private string th005 = null; //品名
        private string th006 = null; //规格
        private string th007 = null; //进货数量
        private string th008 = null; //单位
        private string th009 = null; //仓库
        private string th010 = null; //批号
        private string th011 = null; //采购单别
        private string th012 = null; //采购单号
        private string th013 = null; //采购序号
        private string th014 = null; //验收日期
        private string th015 = null; //验收数量
        private string th016 = null; //计价数量
        private string th018 = null; //原币单位今后价
        private string th019 = null; //原币进货金额
        private string th027 = "N"; //超期码
        private string th033 = null; //备注 订单信息
        private string th034 = null; //验收库存数量
        private string th035 = null; //小单位
        private string th042 = null; //项目编号
        private string th064 = null; //计价单温
        private string th065 = null; //库存单位
        private string thc02 = null; //类型

        public int RowIndex { get { return rowIndex; } set { rowIndex = value; } }
        public float Total { get { return total; } set { total = value; } }
        public DataTable LsDt { get { return lsDt; } set { lsDt = value; } }
        public DataTable SlDt { get { return slDt; } set { slDt = value; } }

        public string TH003 { get { return th003; } set { th003 = value; } }
        public string TH004 { get { return th004; } set { th004 = value; } }
        public string TH005 { get { return th005; } set { th005 = value; } }
        public string TH006 { get { return th006; } set { th006 = value; } }
        public string TH007 { get { return th007; } set { th007 = value; } }
        public string TH008 { get { return th008; } set { th008 = value; } }
        public string TH009 { get { return th009; } set { th009 = value; } }
        public string TH010 { get { return th010; } set { th010 = value; } }
        public string TH011 { get { return th011; } set { th011 = value; } }
        public string TH012 { get { return th012; } set { th012 = value; } }
        public string TH013 { get { return th013; } set { th013 = value; } }
        public string TH014 { get { return th014; } set { th014 = value; } }
        public string TH015 { get { return th015; } set { th015 = value; } }
        public string TH016 { get { return th016; } set { th016 = value; } }
        public string TH018 { get { return th018; } set { th018 = value; } }
        public string TH019 { get { return th019; } set { th019 = value; } }
        public string TH027 { get { return th027; } }
        public string TH033 { get { return th033; } set { th033 = value; } }
        public string TH034 { get { return th034; } set { th034 = value; } }
        public string TH035 { get { return th035; } set { th035 = value; } }
        public string TH042 { get { return th042; } set { th042 = value; } }
        public string TH064 { get { return th064; } set { th064 = value; } }
        public string TH065 { get { return th065; } set { th065 = value; } }
        public string THC02 { get { return thc02; } set { thc02 = value; } }

    }
    #endregion
}
