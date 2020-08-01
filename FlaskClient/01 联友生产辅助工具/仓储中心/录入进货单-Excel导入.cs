using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.仓储中心
{
    public partial class 录入进货单_Excel导入 : Form
    {
        #region 局部变量设定
        Mssql mssql = new Mssql();
        static string connYF = FormLogin.infObj.connYF;
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
        public 录入进货单_Excel导入(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Init();
        }

        private void Init()
        {
            入库单别.Text = "3401-采购入库单";
            入库仓库.Text = "P013-仓储原材料仓";

            panel_Title.Enabled = true;
            panel_Last.Enabled = false;

            DgvOpt.SetRowBackColor(dgvMain);
            DgvOpt.SetColHeadMiddleCenter(dgvMain);
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
        private void ShowUI()
        {
            if (dgvMain.RowCount > 0)
            {
                panel_Last.Enabled = true;
                入库单别L.Enabled = false;
                入库仓库L.Enabled = false;
                供应商L.Enabled = false;
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
            if (dgvMain.RowCount <= 0)
            {
                panel_Last.Enabled = false;
                入库单别L.Enabled = true;
                入库仓库L.Enabled = true;
                供应商L.Enabled = true;
                dateTimePicker1.Enabled = true;
                送货单号T.Enabled = true;
            }
        }

        private void 入库仓库L_MouseClick(object sender, MouseEventArgs e)
        {
            Mode = "PositionID";
            Title = "仓库编号";
            //Title = "仓库编号|仓库名称";
            录入进货单_获取单据信息 frm = new 录入进货单_获取单据信息(connYF, Title, Mode);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                GetMain = frm.GetMain;
                GetOther = frm.GetOther;

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
            录入进货单_获取单据信息 frm = new 录入进货单_获取单据信息(connYF, Title, Mode);
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                GetMain = frm.GetMain;
                GetOther = frm.GetOther;

                if (GetMain != null)
                {
                    供应商Lable.Text = frm.GetMain + "-" + GetOther;
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
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dgvMain.SelectedRows)
            {
                this.dgvMain.Rows.Remove(item);
            }
            ShowUI();
            GetIndex();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            Upload();
            ShowUI();
            GetIndex();
        }

        private void btnCheck_Click(object sender, EventArgs e)
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

        private void btnInput_Click(object sender, EventArgs e)
        {
            if (入库单别.Text == "" || 供应商Lable.Text == "" || 入库仓库.Text == "" || 送货单号T.Text == "")
            {
                MsgFlag = true;
                MessageBox.Show("请先查询或填写 进货单别，供应商，入库仓库，送货单号 的信息！", "错误");
            }
            else
            {
                AddData();
            }
        }

        private void btnLayoutDefault_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("品号", Type.GetType("System.String"));
            dt.Columns.Add("品名", Type.GetType("System.String"));
            dt.Columns.Add("规格", Type.GetType("System.String"));
            dt.Columns.Add("数量", Type.GetType("System.String"));
            dt.TableName = "模板";

            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.isWrite = true;
            excelObj.defauleFileName = "录入进货单-Excel导入-模板";
            excelObj.dataDt = dt;

            if (excel.ExcelOpt(excelObj))
            {
                if (excelObj.status)
                {
                    Msg.Show("已保存模板");
                }
                else
                {
                    Msg.ShowErr(excelObj.msg);
                }
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
                    DgvOpt.SetColMiddleCenter(dgvMain, 0);
                    DgvOpt.SetColMiddleCenter(dgvMain, 4);
                    DgvOpt.SetColMiddleCenter(dgvMain, 5);
                    DgvOpt.SetColMiddleCenter(dgvMain, 6);
                    DgvOpt.SetColMiddleCenter(dgvMain, 7);
                }
            }
            else
            {
                return;
            }
        }

        private void AddData()
        {
            string sqlstr = @"SELECT RTRIM(MB002) 品名, RTRIM(MB003) 规格 FROM COMFORT.dbo.INVMB WHERE MB001 = '{0}' ";
            DataTable dt = ReadExcel();
            dt = DtConvert(dt);
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    DataTable dt2 = mssql.SQLselect(connYF, string.Format(sqlstr, dt.Rows[rowIndex]["品号"].ToString()));

                    int row = dgvMain.Rows.Add();
                    dgvMain.Rows[row].Cells["序号"].Value = "";
                    dgvMain.Rows[row].Cells["品号"].Value = dt.Rows[rowIndex]["品号"].ToString();
                    dgvMain.Rows[row].Cells["数量"].Value = dt.Rows[rowIndex]["数量"].ToString();
                    dgvMain.Rows[row].Cells["仓库"].Value = PositionID;
                    dgvMain.Rows[row].Cells["批号"].Value = SupplierID;
                    dgvMain.Rows[row].Cells["供应商"].Value = SupplierID;
                    dgvMain.Rows[row].Cells["送货单号"].Value = 送货单号T.Text;

                    if (dt2 != null)
                    {
                        dgvMain.Rows[row].Cells["品名"].Value = dt2.Rows[0]["品名"].ToString();
                        dgvMain.Rows[row].Cells["规格"].Value = dt2.Rows[0]["规格"].ToString();
                        dgvMain.Rows[row].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        dgvMain.Rows[row].Cells["品名"].Value = "品号信息异常";
                        dgvMain.Rows[row].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }

            GetIndex();
            ShowUI();
        }

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
            int Count = dgvMain.RowCount;

            for (int Index = 0; Index < Count; Index++)
            {
                string JHXA001 = TypeID;
                string JHXA002 = dgvMain.Rows[0].Cells["批号"].Value.ToString();
                string JHXA003 = dgvMain.Rows[0].Cells["仓库"].Value.ToString();
                string JHXA004 = GetDate();
                string JHXA007 = dgvMain.Rows[0].Cells["品号"].Value.ToString();
                string JHXA008 = "";
                string JHXA009 = dgvMain.Rows[0].Cells["数量"].Value.ToString();
                string JHXA013 = dgvMain.Rows[0].Cells["送货单号"].Value.ToString();
                string JHXA015 = dgvMain.Rows[0].Cells["供应商"].Value.ToString();
                string ID = (Index+1).ToString();
                string sqlstr = string.Format(sql, LoginUid, LoginUserGroup, Time, JHXA001, JHXA002, JHXA003, 
                    JHXA004, flowId, JHXA007, JHXA008, JHXA009, JHXA013, JHXA015, ID);

                mssql.SQLexcute(connYF, sqlstr);

                dgvMain.Rows.Remove(dgvMain.Rows[0]);
            }
            string tell = "\n\r处理流水号为：" + flowId;
            string getBackStr = "";
            getBackStr = createPurtg.HandelDef(flowId);
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

        private float GetDsl(string MaterielID, string SupplierID)
        {
            string sqlstr = "SELECT RTRIM(TD004), RTRIM(TC004), SL FROM V_PURTD_SL_WG WHERE TD004 = '{0}' AND TC004 = '{1}' ";
            DataTable dt = mssql.SQLselect(FormLogin.infObj.connYF, string.Format(sqlstr, MaterielID, SupplierID));
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

        private bool Check()
        {
            bool flag = true;

            for (int index = 0; index < dgvMain.Rows.Count; index++)
            {
                float sl = 0;
                try
                {
                    sl = float.Parse(dgvMain.Rows[index].Cells["数量"].Value.ToString());
                }
                catch
                {
                    dgvMain.Rows[index].Cells["异常"].Value = "数量输入不正确";
                    flag = false;
                }

                if (dgvMain.Rows[index].Cells["品名"].Value.ToString() == "")
                {
                    dgvMain.Rows[index].Cells["异常"].Value = "品号信息不正确";
                    flag = false;
                }

                float dsl = GetDsl(dgvMain.Rows[index].Cells[1].Value.ToString(), dgvMain.Rows[index].Cells[6].Value.ToString());
                if (dsl != 0)
                {
                    if (dsl < sl)
                    {
                        dgvMain.Rows[index].Cells[9].Value = "可验收量为" + dsl.ToString() + "，少于需入库数量";
                        dgvMain.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                        flag = false;
                    }
                    else
                    {
                        dgvMain.Rows[index].Cells[9].Value = "";
                        dgvMain.Rows[index].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                else
                {
                    dgvMain.Rows[index].Cells[9].Value = "无相关采购单";
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

            if (dt != null)
            {
                for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                {
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace(" ", "");
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("\n", "");
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("\t", "");
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("采购数量", "数量");
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("送货数量", "数量");
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("送货数", "数量");
                    dt.Columns[colIndex].ColumnName = dt.Columns[colIndex].ColumnName.Replace("送货量", "数量");

                    if (dt.Columns[colIndex].ColumnName == "品号") isPhFlag = true;
                    if (dt.Columns[colIndex].ColumnName == "数量") isSlFlag = true;
                }

                if (!(isPhFlag && isSlFlag))
                {
                    MessageBox.Show("导入的表格表头不存在“品号”或“数量”，请检查。", "错误", MessageBoxButtons.OK);
                    return null;
                }

                dt.DefaultView.Sort = "品号 ASC";

                dt = dt.DefaultView.ToTable();

                string pn = "";
                DataRow dr = null;
                dtNew = new DataTable();
                dtNew.Columns.Add(new DataColumn("品号".ToString(), typeof(string)));
                dtNew.Columns.Add(new DataColumn("数量".ToString(), typeof(float)));

                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    if (dt.Rows[rowIndex]["品号"].ToString() != "")
                    {
                        if (pn != dt.Rows[rowIndex]["品号"].ToString())
                        {
                            dr = dtNew.NewRow();
                            pn = dt.Rows[rowIndex]["品号"].ToString();
                            dr["品号"] = pn;
                            try
                            {
                                dr["数量"] = float.Parse(dt.Rows[rowIndex]["数量"].ToString());
                                dtNew.Rows.Add(dr);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        else
                        {
                            try
                            {
                                float sl  = float.Parse(dt.Rows[rowIndex]["数量"].ToString());
                                dtNew.Rows[dtNew.Rows.Count - 1]["数量"] = float.Parse(dtNew.Rows[dtNew.Rows.Count - 1]["数量"].ToString()) + sl;
                            }
                            catch
                            {
                                continue;
                            }
                        }
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
