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
    public partial class 录入客户订单 : Form
    {
        #region 公用变量设定

        public static string connYF = FormLogin.infObj.connYF;
        #endregion

        #region 局部变量设定
        private Mssql mssql = FormLogin.infObj.sql;
        ERP_Create_Purtg createPurtg = new ERP_Create_Purtg();

        public static string Mode = null;
        public static string Title = null;
        public static string GetMain = null;
        public static string GetOther = null;
        public static string MaterielMode = null;

        private string SupplierID = "";
        private string TypeID = "2201";
        private string PositionID = "301";
        private string MaterielID = null;
        private string LoginUid = FormLogin.infObj.userId;
        private string LoginUserGroup = FormLogin.infObj.userGroup;

        private DataTable mainDt = new DataTable();

        private bool MsgFlag = false;
        private bool checkFlag = false;

        #endregion

        #region 窗口初始化
        public 录入客户订单()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            订单单别.Text = "2201-客户订单";
            订单仓库.Text = "301-成品仓";

            panel_Title.Enabled = true;
            panel_Last.Enabled = false;

            mainDt.Columns.Add("序号", Type.GetType("System.String"));
            mainDt.Columns.Add("单别", Type.GetType("System.String"));
            mainDt.Columns.Add("客户编号", Type.GetType("System.String"));
            mainDt.Columns.Add("仓库", Type.GetType("System.String"));
            mainDt.Columns.Add("订单日期", Type.GetType("System.String"));
            mainDt.Columns.Add("采购单号", Type.GetType("System.String"));
            mainDt.Columns.Add("品号", Type.GetType("System.String"));
            mainDt.Columns.Add("品名", Type.GetType("System.String"));
            mainDt.Columns.Add("规格", Type.GetType("System.String"));
            mainDt.Columns.Add("单位", Type.GetType("System.String"));
            mainDt.Columns.Add("采购数量", Type.GetType("System.String"));
            mainDt.Columns.Add("预交货日", Type.GetType("System.String"));
            mainDt.Columns.Add("生产单号", Type.GetType("System.String"));
            mainDt.Columns.Add("单身备注", Type.GetType("System.String"));

            dgvMain.DataSource = mainDt;
            DgvOpt.SetColHeadMiddleCenter(dgvMain);
            DgvOpt.SetRowBackColor(dgvMain);
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

            panel_Last.Location = new Point(panel_Title.Location.X, panel_Title.Height + dgvMain.Height);
            panel_Last.Size = new Size(panel_Title.Width, panel_Last.Height);

            dgvMain.Location = new Point(panel_Title.Location.X, panel_Title.Location.Y + panel_Title.Height + 1);
            dgvMain.Size = new Size(panel_Title.Width, FormHeight - (panel_Title.Height + 1) - (panel_Last.Height + 1));
        }
        #endregion

        #region UI按钮
        private void UI()
        {
            if (dgvMain.RowCount > 0)
            {
                panel_Last.Enabled = true;
                dateTimePicker1.Enabled = false;
                订单单别L.Enabled = false;
                订单仓库L.Enabled = false;
                客户L.Enabled = false;
            }
            if (dgvMain.RowCount <= 0)
            {
                panel_Last.Enabled = false;
                dateTimePicker1.Enabled = true;
                订单单别L.Enabled = true;
                订单仓库L.Enabled = true;
                客户L.Enabled = true;
                checkFlag = false;
            }
            if (checkFlag)
            {
                buttonUpload.Enabled = true;
            }
            else
            {
                buttonUpload.Enabled = false;
            }

            float sum = 0;
            if (dgvMain.Rows.Count > 0)
            {
                for (int rowIdx = 0; rowIdx < dgvMain.Rows.Count; rowIdx++)
                {
                    sum += float.Parse(dgvMain.Rows[rowIdx].Cells["采购数量"].Value.ToString());
                }
            }
            label2.Text = "总数量：" + sum.ToString();
        }

        private void 订单仓库L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "PositionID";
            Title = "仓库编号";
            //Title = "仓库编号|仓库名称";
            录入客户订单_获取单据信息 frm = new 录入客户订单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    订单仓库.Text = GetMain + "-" + GetOther;
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
            录入客户订单_获取单据信息 frm = new 录入客户订单_获取单据信息();
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

        private void 订单单别L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "TypeID";
            Title = "单别";
            //Title = "单别|单据简称";
            录入客户订单_获取单据信息 frm = new 录入客户订单_获取单据信息();
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                if (GetMain != null)
                {
                    订单单别.Text = GetMain + "-" + GetOther;
                    TypeID = GetMain;
                    GetMain = null;
                    GetOther = null;
                }
                frm.Dispose();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dgvMain.SelectedRows)
            {
                this.dgvMain.Rows.Remove(item);
            }
            UI();
            //GetIndex();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            Upload();
            UI();
            //GetIndex();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            if (SupplierID == "" || TypeID == ""|| PositionID == "")
            {
                Msg.ShowErr("单别，仓库，客户编号 不能为空");
                UI();
                return;
            }
            else
            {
                AddData();
                UI();
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            checkFlag = Check();

            if (checkFlag)
            {
                Msg.Show("无异常");
            }
            else
            {
                Msg.ShowErr("存在异常，请检查");
            }
            UI();
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
                flowId = "DD" + time + "0001";
            }
            else
            {
                if (mssql.SQLselect(connYF, string.Format("SELECT RTRIM(XHXA005) FROM dbo.XH_LYXA WHERE XHXA005 = '{0}' ", flowId)) == null)
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
            if(dt != null)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        private void GetIndex()//重排dgv的序号并且设置部分列居中显示
        {
            if(dgvMain.Rows.Count > 0)
            {
                int Count = dgvMain.Rows.Count;
                int Index = 0;
                for(Index = 0; Index < Count; Index++)
                {
                    dgvMain.Rows[Index].Cells[0].Value = (Index + 1).ToString().PadLeft(3, '0');
                    dgvMain.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvMain.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvMain.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvMain.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else
            {
                return;
            }
        }

        private void AddData()
        {
            string sqlstr = @"SELECT RTRIM(MB002) 品名, RTRIM(MB003) 规格 FROM dbo.INVMB WHERE MB001 = '{0}' ";
            DataTable dt = ReadExcel();
            dt = DtConvert(dt);
            if(dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    DataTable dt2 = mssql.SQLselect(connYF, string.Format(sqlstr, dt.Rows[rowIndex]["品号"].ToString()));

                    DataRow dr = mainDt.NewRow();

                    dr["序号"] = (rowIndex+1).ToString();
                    dr["单别"] = TypeID;
                    dr["品号"] = dt.Rows[rowIndex]["品号"].ToString();
                    dr["采购数量"] = dt.Rows[rowIndex]["采购数量"].ToString();
                    dr["采购单号"] = dt.Rows[rowIndex]["采购单号"].ToString();
                    dr["预交货日"] = dt.Rows[rowIndex]["预交货日"].ToString();
                    dr["生产单号"] = dt.Rows[rowIndex]["生产单号"].ToString();
                    dr["单身备注"] = dt.Rows[rowIndex]["单身备注"].ToString();
                    dr["仓库"] = PositionID;
                    dr["客户编号"] = SupplierID;
                    dr["订单日期"] = GetDate();

                    if (dt2 != null)
                    {
                        dr["品名"] = dt2.Rows[0]["品名"].ToString();
                        dr["规格"] = dt2.Rows[0]["规格"].ToString();
                        //dgvMain.Rows[row].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        dr["品名"] = "品号信息异常";
                        //dgvMain.Rows[row].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    mainDt.Rows.Add(dr);
                }
            }
            //GetIndex();
        }

        private void Upload()
        {
            string sql = @"INSERT INTO DD_LYXA 
                            (COMPANY, CREATOR, USR_GROUP, CREATE_DATE, FLAG, DDXA001, DDXA002, DDXA003, DDXA004, 
                            DDXA005, DDXA006, DDXA007, DDXA008, DDXA009, DDXA010, DDXA011, DDXA012, DDXA013, 
                            DDXA014, DDXA015, ID) 
                            VALUES('COMFORT3', '{0}', '{1}', '{2}', 1, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', 
                            '{10}', '{11}', '{12}', 'N', 'N', '{13}', '{14}', '{15}', '{16}')";
            string flowId = GetFlowID();
            string Time = GetTime();
            int Count = dgvMain.RowCount;

            for (int Index = 0; Index < Count; Index++)
            {
                string XA001 = dgvMain.Rows[0].Cells["单别"].Value.ToString();
                string XA002 = dgvMain.Rows[0].Cells["客户编号"].Value.ToString();
                string XA003 = dgvMain.Rows[0].Cells["仓库"].Value.ToString();
                string XA004 = dgvMain.Rows[0].Cells["订单日期"].Value.ToString();
                string XA006 = dgvMain.Rows[0].Cells["采购单号"].Value.ToString();
                string XA007 = dgvMain.Rows[0].Cells["品号"].Value.ToString();
                string XA008 = dgvMain.Rows[0].Cells["预交货日"].Value.ToString();
                string XA009 = dgvMain.Rows[0].Cells["采购数量"].Value.ToString();
                string XA010 = dgvMain.Rows[0].Cells["生产单号"].Value.ToString();
                string XA013 = dgvMain.Rows[0].Cells["单身备注"].Value.ToString();
                string XA014 = "";
                string XA015 = "";
                string ID = (Index+1).ToString();

                string sqlstr = string.Format(sql, LoginUid, LoginUserGroup, Time, XA001, XA002, XA003, 
                    XA004, flowId, XA006, XA007, XA008, XA009, XA010, XA013, XA014, XA015, ID);
                
                mssql.SQLexcute(connYF, sqlstr);

                dgvMain.Rows.Remove(dgvMain.Rows[0]);
            }

            string tell = "\n\r处理流水号为：" + flowId;
            string sqlstrEXEC = @"EXEC dbo.P_COPTC_CREATE_WORK_WG '{0}'";
            DataTable dt = mssql.SQLselect(connYF, string.Format(sqlstrEXEC, flowId));
            if (dt != null)
            {
                if (dt.Rows[0][0].ToString() != "")
                {
                    tell += "\n\r订单单号为：" + dt.Rows[0][0].ToString();
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
        
        private bool Check()
        {
            bool flag = true;

            for (int index = 0; index < dgvMain.Rows.Count; index++)
            {
                if(dgvMain.Rows[index].Cells["品名"].Value.ToString() == "品号信息异常")
                {
                    dgvMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    flag = false;
                }
            }
            return flag;
        }

        private DataTable ReadExcel()
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.isTitleRow = true;

            if (excel.ExcelOpt(excelObj))
            {
                if (excelObj.status == true)
                {
                    DataTable dt = excelObj.dataDt;
                    return dt;
                }
                else
                {
                    Msg.Show(excelObj.msg);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private DataTable DtConvert(DataTable dt)
        {
            DataTable dtNew = null;

            //是否存在品名，采购数量列flag
            bool isPhFlag = false;
            bool isSlFlag = false;
            bool isCgdhFlag = false;
            bool isYjhrFlag = false;
            bool isScdhFlag = false;
            bool isDsbzFlag = false;

            if (dt != null)
            {
                for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                {
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace(" ", "");
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("\n", "");
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("\t", "");
                    //dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("采购数量", "订单数量");
                    //dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("数量", "订单数量");

                    string kk = dt.Columns[colIndex].ColumnName;
                    if (dt.Columns[colIndex].ColumnName == "品号") isPhFlag = true;
                    if (dt.Columns[colIndex].ColumnName == "采购数量") isSlFlag = true;
                    if (dt.Columns[colIndex].ColumnName == "生产单号") isScdhFlag = true;
                    if (dt.Columns[colIndex].ColumnName == "预交货日") isYjhrFlag = true;
                    if (dt.Columns[colIndex].ColumnName == "采购单号") isCgdhFlag = true;
                    if (dt.Columns[colIndex].ColumnName == "单身备注") isDsbzFlag = true;
                }

                if (!(isPhFlag && isSlFlag && isScdhFlag && isYjhrFlag && isCgdhFlag && isDsbzFlag))
                {
                    Msg.ShowErr("导入的表格表头不存在“品号”或“采购数量”或“生产单号”或“预交货日”或“采购单号”或“单身备注”，请检查。");
                    return null;
                }
                
                DataRow dr = null;
                dtNew = new DataTable();
                dtNew.Columns.Add("品号".ToString(), Type.GetType("System.String"));
                dtNew.Columns.Add("采购数量".ToString(), Type.GetType("System.String"));
                dtNew.Columns.Add("采购单号".ToString(), Type.GetType("System.String"));
                dtNew.Columns.Add("预交货日".ToString(), Type.GetType("System.String"));
                dtNew.Columns.Add("单身备注".ToString(), Type.GetType("System.String"));
                dtNew.Columns.Add("生产单号".ToString(), Type.GetType("System.String"));

                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    if (dt.Rows[rowIndex]["品号"].ToString() != "")
                    {
                        dr = dtNew.NewRow();
                        dr["品号"] = dt.Rows[rowIndex]["品号"].ToString();
                        dr["采购数量"] = dt.Rows[rowIndex]["采购数量"].ToString();
                        dr["采购单号"] = dt.Rows[rowIndex]["采购单号"].ToString();
                        dr["预交货日"] = dt.Rows[rowIndex]["预交货日"].ToString().Replace("-", "");
                        dr["单身备注"] = dt.Rows[rowIndex]["单身备注"].ToString();
                        dr["生产单号"] = dt.Rows[rowIndex]["生产单号"].ToString();

                        dtNew.Rows.Add(dr);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return dtNew;
        }

        #endregion
    }
}
