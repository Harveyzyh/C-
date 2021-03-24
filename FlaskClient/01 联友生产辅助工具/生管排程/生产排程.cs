using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ.品管;
using System.Collections.Generic;

namespace HarveyZ.生管排程
{
    public partial class 生产排程 : Form
    {
        #region 静态数据设置
        private delegate string NewTaskDelegate(DataTable dttmp); //任务代理
        
        private Mssql mssql = new Mssql();

        private string connWG = FormLogin.infObj.connWG;
        private string connYF = FormLogin.infObj.connYF;
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;
        #endregion

        #region 窗体初始化
        public 生产排程(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            CmBoxShowType.SelectedIndex = 0;
            SetCmBoxDptTypeList();
            SetContextMenuStripTypeList();
            UI();
        }

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
            PanelTitle.Size = new Size(FormWidth, PanelTitle.Height);
            DgvMain.Location = new Point(0, PanelTitle.Height + 2);
            DgvMain.Size = new Size(FormWidth, FormHeight - PanelTitle.Height - 2);
        }
        #endregion

        #endregion

        #region 按钮
        private void UI()
        {
            if (newFlag)
            {
                BtnInput.Enabled = true;
            }
            else
            {
                BtnInput.Enabled = false;
            }
            if (DgvMain.DataSource != null && outFlag)
            {
                BtnOutput.Enabled = true;
            }
            else
            {
                BtnOutput.Enabled = false;
            }
            if (DgvMain.DataSource != null && editFlag && CmBoxShowType.Text != "未排订单")
            {
                BtnSetIndex.Enabled = true;
            }
            else
            {
                BtnSetIndex.Enabled = false;
            }

            if (CmBoxDptType.Text == "全部")
            {
                labelDptWorkTime.Visible = false;
                TxBoxDptWorkTime.Visible = false;
                labelWorkTime.Visible = false;
            }
            else
            {
                labelDptWorkTime.Visible = true;
                TxBoxDptWorkTime.Visible = true;
                labelWorkTime.Visible = true;
                TxBoxDptWorkTime.Text = GetDptWorkTime();
                //labelWorkTime.Text = "排程总工时：0";
            }

            if (CmBoxShowType.Text == "未排订单")
            {
                DtpStartWorkDate.Enabled = false;
                DtpEndWorkDate.Enabled = false;
                CmBoxDptType.Enabled = false;
                TxBoxDptWorkTime.Enabled = false;
                //DtpStartDdDate.Checked = true;
                //DtpEndDdDate.Checked = true;
                panelStatusType.Enabled = false;
            }
            else
            {
                panelStatusType.Enabled = true;
                DtpStartWorkDate.Enabled = true;
                DtpEndWorkDate.Enabled = true;
                CmBoxDptType.Enabled = true;
                TxBoxDptWorkTime.Enabled = true;
            }
        }

        private void BtnShow_Click(object sender, EventArgs e) //刷新按钮
        {
            if (CmBoxShowType.Text != "未排订单")
            {
                DgvShow();
            }
            else
            {
                DgvShow2();
            }
            UI();

            if (DgvMain.DataSource == null)
            {
                Msg.Show("没有数据！");
            }
        }

        private void BtnInput_Click(object sender, EventArgs e)
        {
            Form frmDpt = new 生产排程部门选择();
            frmDpt.ShowDialog();
            string Dpt = 生产排程部门选择.Dpt;
            if (Dpt != null)
            {
                try
                {
                    Excel excel = new Excel();
                    Excel.Excel_Base excelObj = new Excel.Excel_Base();
                    excelObj.isTitleRow = true;
                    excelObj.isWrite = false;

                    if (excel.ExcelOpt(excelObj))
                    {

                        if (excelObj.status == true)
                        {
                            SetInputDtDpt(Dpt, excelObj.dataDt);
                            //GetInsertDt(excelObj.dataDt);
                            生产排程导入 frmInput = new 生产排程导入(GetInsertDt(excelObj.dataDt));
                            frmInput.ShowDialog();
                            UptInfo();
                            DgvShow();
                        }
                        else
                        {
                            Msg.Show(excelObj.msg);
                        }
                    }
                }
                catch (Exception es)
                {
                    Msg.Show(es.ToString());
                }
            }
        }

        private void BtnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataSet.Tables.Add((DataTable)DgvMain.DataSource);
            excelObj.dataSet.Tables[0].TableName = "生产排程";
            excelObj.defauleFileName = "生产排程导出_" + DateTime.Now.ToString("yyyy-MM-dd");
            excelObj.isWrite = true;

            if (excel.ExcelOpt(excelObj))
            {
                if (excelObj.status)
                {
                    Msg.Show("Excel导出成功！");
                }
                else
                {
                    Msg.Show(excelObj.msg, "错误");
                }
            }
        }
        
        private void BtnSetIndex_Click(object sender, EventArgs e)
        {
            if (FormLogin.StopModuleOpen())
            {
                SetGdPcIndex();
                Msg.Show("已同步");
                DgvShow();
                UI();
            }
        }

        private void DtpStartWorkDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_Start(DtpStartWorkDate, DtpEndWorkDate);
        }

        private void DtpEndWorkDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_End(DtpStartWorkDate, DtpEndWorkDate);
        }

        private void DtpStartDdDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_Start(DtpStartDdDate, DtpEndDdDate);
        }

        private void DtpEndDdDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_End(DtpStartDdDate, DtpEndDdDate);
        }

        private void CmBoxDptType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DgvMain.DataSource = null;
            UI();
        }

        private void CmBoxShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DgvMain.DataSource = null;
            UI();
        }

        private void TxBoxOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DgvShow();
            }
        }

        private void TxBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DgvShow();
            }
        }

        private void DgvMain_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip_DgvMain.Items.Clear();
                SetContextMenuStripTypeList();
                // 显示定位
                if (e.RowIndex >= 0)
                {
                    contextMenuStrip_DgvMain.Visible = true;
                    DgvMain.ClearSelection();
                    DgvMain.Rows[e.RowIndex].Selected = true;
                    DgvMain.CurrentCell = DgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    contextMenuStrip_DgvMain.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuStrip_DgvMain.Visible = false;
            if (e.ClickedItem.Text == "复制")
            {
                Clipboard.SetText(DgvMain.Rows[DgvMain.CurrentCell.RowIndex].Cells[DgvMain.CurrentCell.ColumnIndex].Value.ToString());
            }
            else if (e.ClickedItem.Text == "增加")
            {
                contextMenuStrip_DgvMain_ItemClicked_New();
            }
            else if (e.ClickedItem.Text == "修改")
            {
                contextMenuStrip_DgvMain_ItemClicked_Edit();
            }
            else if (e.ClickedItem.Text == "删除")
            {
                contextMenuStrip_DgvMain_ItemClicked_Delete();
            }
            else if (e.ClickedItem.Text == "打印成品标签")
            { 
                if (FormLogin.StopModuleOpen())
                {
                    contextMenuStrip_DgvMain_ItemClicked_PrintLable();
                }
            }
            else if (e.ClickedItem.Text == "查看工单信息")
            {
                if (FormLogin.StopModuleOpen())
                {
                    contextMenuStrip_DgvMain_ItemClicked_ShowGd();
                }
            }
            else
            {
                contextMenuStrip_DgvMain_ItemClicked_Select(e.ClickedItem.Text);
            }
        }

        private void CheckBoxStatus_CheckedChanged(object sender, EventArgs e)
        {
            DgvMain.DataSource = null;
            UI();
        }
        #endregion

        #region 逻辑

        #region 右键按钮处理逻辑
        private void SetContextMenuStripTypeList()
        {
            if (printFlag && CmBoxShowType.Text != "未排订单") contextMenuStrip_DgvMain.Items.Add("打印成品标签");
            if (editFlag && CmBoxShowType.Text != "未排订单") contextMenuStrip_DgvMain.Items.Add("查看工单信息");
            contextMenuStrip_DgvMain.Items.Add("复制");
            contextMenuStrip_DgvMain.Items.Add("查询当前订单号");
            contextMenuStrip_DgvMain.Items.Add("查询当前生产单号");
            contextMenuStrip_DgvMain.Items.Add("查询当前品名");

            if (newFlag) contextMenuStrip_DgvMain.Items.Add("增加");
            if (newFlag && CmBoxShowType.Text != "未排订单") contextMenuStrip_DgvMain.Items.Add("修改");
            if (delFlag && CmBoxShowType.Text != "未排订单") contextMenuStrip_DgvMain.Items.Add("删除");
        }

        private void contextMenuStrip_DgvMain_ItemClicked_New()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string index = "";
            生产排程修改 frm = new 生产排程修改("New", "", DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString());
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                index = 生产排程修改.indexRtn;
                frm.Dispose();
                if (生产排程修改.saveFlag)
                {
                    UptInfo();
                    int i = 0;
                    if (int.TryParse(index, out i))
                    {
                        if (CmBoxShowType.Text != "未排订单")
                        {
                            DgvShow(i);
                        }
                        else
                        {
                            DgvShow2();
                        }
                    }
                    else
                    {
                        if (CmBoxShowType.Text != "未排订单")
                        {
                            DgvShow();
                        }
                        else
                        {
                            DgvShow2();
                        }
                    }
                }
                UI();
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked_Edit()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string index = "";
            生产排程修改 frm = new 生产排程修改("Edit", DgvMain.Rows[rowIndex].Cells["序号"].Value.ToString(), DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString());
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                index = 生产排程修改.indexRtn;
                frm.Dispose();
                if (生产排程修改.saveFlag)
                {
                    DgvShow(int.Parse(index));
                }
            }
            UI();
        }

        private void contextMenuStrip_DgvMain_ItemClicked_Delete()
        {
            string slqStr = @"DELETE FROM dbo.SC_PLAN WHERE K_ID = '{0}' AND SC001 = '{1}' AND SC003 = '{2}' AND SC013 = '{3}' AND SC023 = '{4}'";
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string msg = string.Format("是否确认删除当前行！\n\n序号：'{0}'\n订单号：'{1}'\n上线日期'{2}'\n生产车间：'{3}'\n上线数量：'{4}'",
                    DgvMain.Rows[rowIndex].Cells["序号"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                    DgvMain.Rows[rowIndex].Cells["生产车间"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["上线数量"].Value.ToString());

            if (Msg.Show(msg) == DialogResult.OK)
            {
                mssql.SQLexcute(connWG, string.Format(slqStr, DgvMain.Rows[rowIndex].Cells["序号"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                    DgvMain.Rows[rowIndex].Cells["上线数量"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["生产车间"].Value.ToString()
                    ));
                DgvShow();
                UI();
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked_Select(string item)
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            if (item == "查询当前订单号")
            {
                TxBoxOrder.Text = DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString();
                DgvShow();
            }
            if (item == "查询当前生产单号")
            {
                string dd = DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString();
                if (dd.Split('-').Length == 3)
                {
                    TxBoxOrder.Text = dd.Split('-')[0] + "-" + dd.Split('-')[1];
                    DgvShow();
                }
            }
            if (item == "查询当前品名")
            {
                TxBoxName.Text = DgvMain.Rows[rowIndex].Cells["品名"].Value.ToString();
                DgvShow();
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked_PrintLable()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            成品标签打印 frm = new 成品标签打印("品管部_成品标签打印", DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString(), DgvMain.Rows[rowIndex].Cells["上线数量"].Value.ToString());
            frm.ShowDialog();
        }

        private void contextMenuStrip_DgvMain_ItemClicked_ShowGd()
        {
            DataGridViewRow dr = DgvMain.Rows[DgvMain.CurrentCell.RowIndex];
            工单信息管理 frm = new 工单信息管理("排程_工单管理", dr);
            frm.ShowDialog();
        }
        #endregion

        #region 数据初始化逻辑
        private void SetCmBoxDptTypeList()
        {
            string slqStr = @"Select Dpt from SC_PLAN_DPT_TYPE WHERE Type = 'Out' and Valid = 1 order by K_ID";
            DataTable dt = mssql.SQLselect(connWG, slqStr);
            if (dt != null)
            {
                for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    CmBoxDptType.Items.Add(dt.Rows[rowIndex][0].ToString());
                }
                CmBoxDptType.SelectedIndex = 0;
            }
        }

        private void SetGdPcIndex()
        {
            string slqStr = @"UPDATE MOCTA SET MOCTA.UDF02 = SC_PLAN.K_ID
                                FROM WG_DB..SC_PLAN 
                                LEFT JOIN MOCTA ON RTRIM(TA026)+'-'+RTRIM(TA027)+'-'+RTRIM(TA028) = SC001 AND TA015 = SC013 AND TA006 = SC028 AND TA013 NOT IN ('V', 'U') AND TA011 NOT IN ('y') 
                                WHERE 1=1
                                AND NOT EXISTS(SELECT 1 FROM WG_DB..SC_PLAN AS SC2 WHERE SC_PLAN.SC001 = SC2.SC001 GROUP BY SC2.SC001, SC2.SC003, SC2.SC013, SC2.SC023 HAVING COUNT(*) > 1)
                                AND NOT EXISTS(SELECT 1 FROM MOCTA AS TA2 WHERE RTRIM(TA2.TA026)+'-'+RTRIM(TA2.TA027)+'-'+RTRIM(TA2.TA028) = SC001 AND TA2.TA015 = SC013 AND TA2.TA006 = SC028 GROUP BY RTRIM(TA2.TA026)+'-'+RTRIM(TA2.TA027)+'-'+RTRIM(TA2.TA028), TA015 HAVING COUNT(*) > 1)
                                AND TA001 = '5101'
                                AND (MOCTA.UDF02 IS NULL OR MOCTA.UDF02 != SC_PLAN.K_ID)
                                AND SC003 >= '20200901'";
            if (TxBoxOrder.Text != "") slqStr += @" AND SC001 LIKE '%" + TxBoxOrder.Text + "%' ";
            if (TxBoxName.Text != "") slqStr += @" AND SC010 LIKE '%" + TxBoxName.Text + "%' ";
            if (DtpStartWorkDate.Checked) slqStr += @" AND SC003 >= '" + DtpStartWorkDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpEndWorkDate.Checked) slqStr += @" AND SC003 <= '" + DtpEndWorkDate.Value.ToString("yyyyMMdd") + "' ";
            mssql.SQLexcute(connYF, slqStr);
        }

        private int GetNowDate()
        {
            string slqStr = @"SELECT isnull(LEFT(dbo.f_getTime(1), 8), 0) ";
            return int.Parse(mssql.SQLselect(connYF, slqStr).Rows[0][0].ToString());
        }

        private string GetDptWorkTime() //获取部门生产工时
        {
            string slqStr = @"SELECT DptWorkTime 部门生产工时 From SC_PLAN_DPT_TYPE WHERE Type = 'In' AND Dpt = '{0}' ";
            DataTable dt = mssql.SQLselect(connWG, string.Format(slqStr, CmBoxDptType.Text));
            if (dt != null)
            {
                return dt.Rows[0]["部门生产工时"].ToString();
            }
            else
            {
                return "0";
            }
        }
        #endregion

        #region 导入导出数据
        private DataTable GetInsertDt(DataTable dt)//数据表转成sql语句
        {
            DataTable dtInsert = new DataTable();
            dtInsert.Columns.Add("生产单号", Type.GetType("System.String"));
            dtInsert.Columns.Add("上线日期", Type.GetType("System.String"));
            dtInsert.Columns.Add("上线数量", Type.GetType("System.String"));
            //dtInsert.Columns.Add("测试量", Type.GetType("System.String"));
            dtInsert.Columns.Add("生产车间", Type.GetType("System.String"));
            DataRow dtRowTmp = null;
            int rowIndex = 0;
            int row_total = dt.Rows.Count;
            int col_total = dt.Columns.Count;

            string SC003 = "", SC001 = "";

            int SysDate = GetNowDate();
            //导入日期偏移量
            int dayOffset = 365;
            int SysDate2 = int.Parse(DateTime.ParseExact(SysDate.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddDays(dayOffset).ToString("yyyyMMdd"));
            int WorkTime = 0;
            
            for (; rowIndex < row_total; rowIndex++ )
            {
                //限定日期
                try
                {
                    SC003 = dt.Rows[rowIndex][0].ToString().Replace("-", "").Replace("/", "");
                    WorkTime = int.Parse(SC003);
                    if(WorkTime < SysDate || WorkTime >= SysDate2)
                    {
                        continue;
                    }
                }
                catch
                {
                    continue;
                }

                if (dt.Rows[rowIndex][2].ToString() == "生产单号")
                {
                    continue;
                }
                else if (dt.Rows[rowIndex][2].ToString() != "")
                {
                    SC001 = dt.Rows[rowIndex][2].ToString();

                    SC001 = SC001.Split('(')[0].ToString().Split('（')[0].ToString();

                    SC001 = SC001.Split('-')[0].Trim() + '-' + SC001.Split('-')[1].Trim() + '-' + SC001.Split('-')[2].Trim();

                    dtRowTmp = dtInsert.NewRow();
                    dtRowTmp["生产单号"] = SC001;
                    dtRowTmp["上线日期"] = SC003;
                    dtRowTmp["上线数量"] = dt.Rows[rowIndex][12].ToString().Split('/')[0];
                    //dtRowTmp["测试量"] = dt.Rows[rowIndex][13].ToString().Split('/')[0];
                    dtRowTmp["生产车间"] = dt.Rows[rowIndex][22].ToString();
                    dtInsert.Rows.Add(dtRowTmp);
                }
                else continue;
            }

            if (dtInsert != null && dtInsert.Rows.Count != 0)
            {
                try
                {
                    for(int rowIndex2 = 0; rowIndex2 < dtInsert.Rows.Count; rowIndex2++)
                    {
                        int kk = int.Parse(dtInsert.Rows[rowIndex2]["上线数量"].ToString());
                    }
                return dtInsert;
                }
                catch
                {
                    Msg.ShowErr("导入异常，数量列存在异常，请检查！");
                    return null;
                }
            }
            else
            {
                Msg.ShowErr("导入异常，找不到没有可导入数据");
                return null;
            }
        }

        /// <summary>
        /// 根据界面选择的部门，修改dataDt中的部门列
        /// </summary>
        /// <param name="Dpt">传入的部门</param>
        /// <param name="dt">需要处理的dt</param>
        private void SetInputDtDpt(string Dpt, DataTable dt)
        {
            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                if (dt.Rows[rowIndex][2].ToString() == "生产单号")
                {
                    continue;
                }
                dt.Rows[rowIndex][22] = Dpt;
            }
        }

        ///补全基本信息
        private void UptInfo()
        {
            string slqStr = @"UPDATE WG_DB.dbo.SC_PLAN SET 
                                SC002 = X00, SC004 = X01, SC005 = X02, SC006 = X03, SC007 = X04, SC008 = X05, 
                                SC009 = X06, SC010 = X07, SC011 = X08, SC012 = X09, SC015 = X10, 
                                SC016 = X11, SC017 = X12, SC018 = X13, SC019 = X14, SC020 = X15, SC021 = X16, 
                                SC022 = X17, SC024 = X18, SC025 = X19, SC026 = X20, SC027 = X21, SC028 = X22, SC029 = LEFT(dbo.f_getTime(1), 12)  
                                FROM WG_DB.dbo.SC_PLAN
                                INNER JOIN (
	                                SELECT 
	                                RTRIM(COPTD.TD001) + '-' + RTRIM(COPTD.TD002) + '-' + RTRIM(COPTD.TD003) AS DD,
	                                (CASE WHEN TC004='0118' THEN '内销' ELSE '外销' END) AS X00,
	                                RTRIM(COPMA.MA002) AS X01,
	                                RTRIM(COPTC.TC015) AS X02,
	                                RTRIM((CASE WHEN COPTF.TF003 IS NULL THEN '' WHEN COPTF.TF003 IS NOT NULL AND COPTF.TF017 = 'Y' 
		                                THEN '指定结束'+':'+'变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 ELSE '变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 END)) AS X03,
	                                RTRIM((CASE WHEN COPTD.TD013 = '' THEN '' WHEN COPTD.TD013 IS NULL THEN '' ELSE COPTD.TD013 END)) AS X04,
	                                RTRIM((CASE WHEN COPTD.UDF03 = '' THEN '' WHEN COPTD.UDF03 IS NULL THEN '' ELSE COPTD.UDF03 END)) AS X05,
	                                RTRIM(COPTD.UDF01) AS X06,
	                                RTRIM(COPTD.TD005) AS X07,
	                                RTRIM(COPTD.UDF08) AS X08,
	                                RTRIM(COPTD.TD006) AS X09,
	                                RTRIM(COPTD.TD053) AS X10,
	                                RTRIM(COPTQ.TQ003) AS X11,
	                                RTRIM((COPTQ.UDF07+COPTD.TD020)) AS X12, 
	                                RTRIM(COPTD.TD204) AS X13, 
	                                RTRIM(COPTC.TC035) AS X14,
	                                RTRIM(CMSMV.MV002) AS X15, 
	                                RTRIM(COPTC.TC012) AS X16, 
	                                RTRIM(COPTD.TD014) AS X17,
	                                RTRIM(COPTD.UDF05) AS X18,
	                                RTRIM(COPTD.UDF10) AS X19,
	                                RTRIM((CASE WHEN COPTC.UDF09='否' THEN '' ELSE '是' END)) AS X20,
	                                SUBSTRING(COPTD.CREATE_DATE,1,14) AS X21, 
	                                RTRIM(COPTD.TD004) AS X22, 
                                    RTRIM(COPTQ.UDF07) AS X23 
	                                FROM COMFORT.dbo.COPTD AS COPTD 
	                                Left JOIN COMFORT.dbo.COPTC AS COPTC On COPTD.TD001=COPTC.TC001 and COPTD.TD002=COPTC.TC002 
	                                Left JOIN COMFORT.dbo.COPTQ AS COPTQ On COPTD.TD053=COPTQ.TQ002 and COPTD.TD004=COPTQ.TQ001 
	                                Left JOIN COMFORT.dbo.COPMA AS COPMA On COPTC.TC004=COPMA.MA001 
	                                Left JOIN COMFORT.dbo.CMSMV AS CMSMV On COPTC.TC006=CMSMV.MV001 
	                                LEFT JOIN COMFORT.dbo.INVMB AS INVMB ON COPTD.TD004=INVMB.MB001 
	                                Left JOIN COMFORT.dbo.COPTF AS COPTF On COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104 
		                                AND COPTF.TF003 = (SELECT MAX(TF003) FROM COMFORT.dbo.COPTF  
			                                WHERE COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104) 
	                                LEFT JOIN COMFORT.dbo.CMSME AS CMSME ON CMSME.ME001 = INVMB.MB445 
                                ) AS A ON SC001 = DD
                                WHERE (SC028 IS NULL OR SC028 ='') ";
            mssql.SQLexcute(connWG, slqStr);
        }

        #endregion

        #region 数据显示

        private bool GetPlanOvertime(string kid)
        {
            string sqlStr = @"SELECT 1
                                FROM WG_DB.dbo.SC_PLAN AS SC1
                                WHERE 1=1
                                AND EXISTS(
	                                SELECT 1 FROM WG_DB.dbo.SC_PLAN AS SC2 
	                                INNER JOIN COMFORT.dbo.COPTD ON RTRIM(TD001)+'-'+RTRIM(TD002)+'-'+RTRIM(TD003) = SC2.SC001
	                                WHERE SC2.K_ID != SC1.K_ID 
                                    AND SC2.SC023 != SC1.SC023
	                                AND SC2.K_ID = {0} 
	                                AND SC1.SC001 LIKE (RTRIM(TD001)+'-'+RTRIM(TD002)+'-%')
	                                AND (DATEADD(DAY, 3, CONVERT(DATE, SC2.SC003, 112)) < CONVERT(DATE, SC1.SC003, 112) OR CONVERT(DATE, SC1.SC003, 112) < DATEADD(DAY, -3, CONVERT(DATE, SC2.SC003, 112)))
                                )";
            return mssql.SQLexist(connYF, string.Format(sqlStr, kid));
        }

        /// <summary>
        /// 对datatable作数据整理
        /// </summary>
        /// <param name="dt"></param>
        private void ShowDtConvert(DataTable dt)
        {
            //更新状态列
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    //if (GetPlanOvertime(dt.Rows[rowIndex]["序号"].ToString()))
                    //{
                    //    dt.Rows[rowIndex]["状态"] += "上线日期跨度超过3天，";
                    //}

                    if (dt.Rows[rowIndex]["生产车间N"].ToString() == "")
                    {
                        dt.Rows[rowIndex]["状态"] += "新增，";
                    }
                    else
                    {
                        if (dt.Rows[rowIndex]["上线日期N"].ToString() != dt.Rows[rowIndex]["上线日期"].ToString())
                        {
                            if (dt.Rows[rowIndex]["上线日期N"].ToString() != "")
                            {
                                int dateBack = int.Parse(dt.Rows[rowIndex]["上线日期N"].ToString().Replace("-", ""));
                                int dateNew = int.Parse(dt.Rows[rowIndex]["上线日期"].ToString().Replace("-", ""));

                                DateTime dateB = DateTime.ParseExact(dateBack.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                                DateTime dateN = DateTime.ParseExact(dateNew.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                                int compNum = dateB.Subtract(dateN).Days;

                                if (dateBack < dateNew)
                                {
                                    dt.Rows[rowIndex]["状态"] += string.Format("上线延后{0}天，", (-1*compNum).ToString());
                                }
                                if (dateBack > dateNew)
                                {
                                    dt.Rows[rowIndex]["状态"] += string.Format("上线提前{0}天，", compNum.ToString());
                                }
                            }
                        }
                        if (dt.Rows[rowIndex]["生产车间"].ToString() != dt.Rows[rowIndex]["生产车间N"].ToString() && dt.Rows[rowIndex]["生产车间N"].ToString() != "")
                        {
                            dt.Rows[rowIndex]["状态"] += "部门变更，";
                        }
                        if (dt.Rows[rowIndex]["上线数量"].ToString() != dt.Rows[rowIndex]["上线数量N"].ToString() && dt.Rows[rowIndex]["生产车间N"].ToString() != "")
                        {
                            dt.Rows[rowIndex]["状态"] += "数量变更，";
                        }
                        dt.Rows[rowIndex]["状态"] = dt.Rows[rowIndex]["状态"].ToString().TrimEnd('，');
                    }
                }
            }
            dt.Columns.Remove(dt.Columns["生产车间N"]);
            dt.Columns.Remove(dt.Columns["上线数量N"]);
            dt.Columns.Remove(dt.Columns["上线日期N"]);
        }

        /// <summary>
        /// 状态类型勾选项，返回sql语句
        /// </summary>
        /// <returns></returns>
        private DataTable GetStatusTypeSelectDt(DataTable dt)
        {
            DataTable dt2 = dt.Copy();
            DataView dv = dt2.DefaultView;

            if (CheckBoxStatusTypeEmpty.Checked)
            {
                if (dv.RowFilter == "")
                {
                    dv.RowFilter += @"状态 = '' ";
                }
                else
                {
                    dv.RowFilter += @" OR 状态 = '' ";
                }
            }
            if (CheckBoxStatusTypeNew.Checked)
            {
                if (dv.RowFilter == "")
                {
                    dv.RowFilter += @"状态 LIKE '%新增%' ";
                }
                else
                {
                    dv.RowFilter += @" OR 状态 LIKE '%新增%' ";
                }
            }
            if (CheckBoxStatusTypeDateEarly.Checked)
            {
                if (dv.RowFilter == "")
                {
                    dv.RowFilter += @"状态 LIKE '%上线提前%' ";
                }
                else
                {
                    dv.RowFilter += @" OR 状态 LIKE '%上线提前%' ";
                }
            }
            if (CheckBoxStatusTypeDateDelay.Checked)
            {
                if (dv.RowFilter == "")
                {
                    dv.RowFilter += @"状态 LIKE '%上线延后%' ";
                }
                else
                {
                    dv.RowFilter += @" OR 状态 LIKE '%上线延后%' ";
                }
            }
            if (CheckBoxStatusTypeSlChange.Checked)
            {
                if (dv.RowFilter == "")
                {
                    dv.RowFilter += @"状态 LIKE '%数量变更%' ";
                }
                else
                {
                    dv.RowFilter += @" OR 状态 LIKE '%数量变更%' ";
                }
            }
            if (CheckBoxStatusTypeDptChange.Checked)
            {
                if (dv.RowFilter == "")
                {
                    dv.RowFilter += @"状态 LIKE '%部门变更%' ";
                }
                else
                {
                    dv.RowFilter += @" OR 状态 LIKE '%部门变更%' ";
                }
            }
            if (CheckBoxStatusTypePlanOverTime.Checked)
            {
                if (dv.RowFilter == "")
                {
                    dv.RowFilter += @"状态 LIKE '%上线日期跨度%' ";
                }
                else
                {
                    dv.RowFilter += @" OR 状态 LIKE '%上线日期跨度%' ";
                }
            }

            dt2 = dv.ToTable();
            return dt2;
        }

        /// <summary>
        /// 已排订单的显示
        /// </summary>
        /// <param name="index"></param>
        private void DgvShow(int index = 0) //数据库资料显示到界面
        {
            string sqlStrShow = @" SELECT SC1.K_ID 序号, '' AS 状态, SC1.SC003 上线日期, SC1.SC023 生产车间, SC1.SC001 订单号, SC1.SC002 订单类型, RTRIM(COPTD.TD005) 品名, 
                                    RTRIM(COPTD.TD006) 规格, RTRIM(COPTD.UDF08) 保友品名, SC1.SC013 上线数量, ISNULL(CAST(TD008 AS FLOAT), 0) ERP订单数量, ISNULL(SC2013, 0) 至今天已排数量, 
                                    ISNULL(SC3013, 0) 总已排数量, ISNULL(CAST(MOCTA.TA015 AS FLOAT), 0) 绑定工单生产数量, 
                                    CAST(ISNULL(INVMB.UDF51 * SC1.SC013, 0) AS FLOAT) 生产工时, 
                                    CAST(ISNULL(COPTG.TH008, 0) AS FLOAT) 销货单数量, 
									(CASE COPTD.UDF04 WHEN '1' THEN '1.内销' WHEN '2' THEN '2.一般贸易' WHEN '3' THEN '3.合同' ELSE COPTD.UDF04 END) 贸易方式, 
                                    (CASE TD016 WHEN 'Y' THEN 'Y.自动结束' WHEN 'y' THEN 'y.指定结束' ELSE '' END) 结束, 
                                    RTRIM(TD004) 品号, RTRIM(TD053) 配置方案, 
                                    RTRIM(COPTQ.TQ003) 配置方案描述, RTRIM((COPTQ.UDF07+COPTD.TD020)) 描述备注, RTRIM(MA002) 客户名称, RTRIM(COPTC.TC015) 注意事项, 
                                    SC1.SC006 变更原因, RTRIM((CASE WHEN COPTD.TD013 = '' THEN '' WHEN COPTD.TD013 IS NULL THEN '' ELSE COPTD.TD013 END)) 出货日期, 
                                    RTRIM((CASE WHEN COPTC.UDF09='否' THEN '' ELSE '是' END)) 急单, RTRIM(COPTD.UDF12) 订单时间, SC1.SC029 排程时间,   
                                    SC1.SC030 生产物料导出时间, SC1.SC031 采购物料导出时间, 
                                    ISNULL(SC0.SC013, -1) 上线数量N, ISNULL(SC0.SC023, '') 生产车间N, ISNULL(SC0.SC003, '') 上线日期N  

                                    FROM WG_DB.dbo.SC_PLAN AS SC1
                                    LEFT JOIN WG_DB.dbo.SC_PLAN_Snapshot AS SC0 ON SC0.K_ID = SC1.K_ID AND SC0.SC001 = SC1.SC001 AND SC0.SC000 = CONVERT(VARCHAR(8), DATEADD(DAY, -2, GETDATE()), 112)
									LEFT JOIN COMFORT.dbo.COPTD ON RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) = SC1.SC001 
                                    LEFT JOIN COMFORT.dbo.COPTQ ON TQ001 = TD004 AND TQ002 = TD053 
                                    LEFT JOIN COMFORT.dbo.COPTC ON TC001 = TD001 AND TC002 = TD002 
                                    LEFT JOIN COMFORT.dbo.COPMA ON MA001 = TC004 
									LEFT JOIN (SELECT SC2.SC001 SC2001, SUM(SC2.SC013) SC2013 FROM WG_DB.dbo.SC_PLAN AS SC2 WHERE SC2.SC003 <= CONVERT(VARCHAR(8), GETDATE(), 112) GROUP BY SC2.SC001) AS SC2 ON SC2001 = SC1.SC001 
									LEFT JOIN (SELECT SC3.SC001 SC3001, SUM(SC3.SC013) SC3013 FROM WG_DB.dbo.SC_PLAN AS SC3 GROUP BY SC3.SC001) AS SC3 ON SC3001 = SC1.SC001 
									LEFT JOIN (SELECT SUM(TA015) TA015, TA006, RTRIM(TA026)+'-'+RTRIM(TA027)+'-'+RTRIM(TA028) TD, UDF02 FROM COMFORT.dbo.MOCTA 
                                            WHERE TA013 NOT IN ('V', 'U') GROUP BY RTRIM(TA026)+'-'+RTRIM(TA027)+'-'+RTRIM(TA028), TA006, UDF02) 
										AS MOCTA ON MOCTA.TD = SC1.SC001 AND MOCTA.UDF02 = SC1.K_ID AND MOCTA.TA006 = SC1.SC028 
                                    LEFT JOIN (SELECT SUM(TH008) TH008, TH004, RTRIM(TH014)+'-'+RTRIM(TH015)+'-'+RTRIM(TH016) TD FROM COMFORT.dbo.COPTG 
	                                    INNER JOIN COMFORT.dbo.COPTH ON TH001 = TG001 AND TH002 = TG002 
	                                    WHERE TG023 NOT IN ('V', 'U') GROUP BY TH004, RTRIM(TH014)+'-'+RTRIM(TH015)+'-'+RTRIM(TH016)) AS COPTG ON COPTG.TD = SC1.SC001
                                    LEFT JOIN COMFORT.dbo.INVMB ON TD004 = MB001 
                                    WHERE 1 = 1 ";
            if (TxBoxOrder.Text != "") sqlStrShow += string.Format(@" AND SC1.SC001 LIKE '%{0}%' ", TxBoxOrder.Text);
            if (TxBoxName.Text != "") sqlStrShow += string.Format(@" AND SC1.SC010 LIKE '%{0}%' ", TxBoxName.Text);
            if (TxBoxWlno.Text != "") sqlStrShow += string.Format(@" AND SC1.SC028 LIKE '%{0}%' ", TxBoxWlno.Text);
            if (DtpStartWorkDate.Checked) sqlStrShow += string.Format(@" AND SC1.SC003 >= '{0}' ", DtpStartWorkDate.Value.ToString("yyyyMMdd"));
            if (DtpEndWorkDate.Checked) sqlStrShow += string.Format(@" AND SC1.SC003 <= '{0}' ", DtpEndWorkDate.Value.ToString("yyyyMMdd"));
            if (CmBoxDptType.Text != "全部") sqlStrShow += string.Format(@" AND SC1.SC023 LIKE '%{0}%' ", CmBoxDptType.Text);
            if (CmBoxShowType.Text == "总已排数量>ERP订单数量") sqlStrShow += @"AND SC3013 > ISNULL(CAST(TD008 AS FLOAT), 0) ";
            if (CmBoxShowType.Text == "总已排数量<ERP订单数量") sqlStrShow += @"AND SC3013 < ISNULL(CAST(TD008 AS FLOAT), 0) ";
            if (CmBoxShowType.Text == "上线数量不等于绑定工单产量") sqlStrShow += @"AND SC1.SC013 != ISNULL(CAST(MOCTA.TA015 AS FLOAT), 0) ";
            if (CmBoxShowType.Text == "已排但存在变更订单") sqlStrShow += @"AND COPTD.UDF12 > SC1.SC029 ";

            sqlStrShow += " ORDER BY SC1.SC003, SC1.SC001 ";
            DataTable showDt = mssql.SQLselect(connWG, sqlStrShow);

            if (showDt != null)
            {
                DtOpt.DtDateFormat(showDt, "时间");
                DtOpt.DtDateFormat(showDt, "日期");

                ShowDtConvert(showDt);
                showDt = GetStatusTypeSelectDt(showDt);

                DgvMain.DataSource = showDt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColMiddleCenter(DgvMain, "序号");
                DgvOpt.SetColMiddleCenter(DgvMain, "数量");
                DgvOpt.SetColMiddleCenter(DgvMain, "工时");
                DgvOpt.SetColWidth(DgvMain, "订单号", 180);
                DgvOpt.SetColWidth(DgvMain, "品名", 180);
                DgvOpt.SetColWidth(DgvMain, "订单类型", 50);
                DgvOpt.SetColWidth(DgvMain, "急单", 50);
                DgvOpt.SetColWidth(DgvMain, "数量", 80);
                DgvOpt.SetColWidth(DgvMain, "工时", 80);
                DgvOpt.SetColWidth(DgvMain, "规格", 180);
                DgvOpt.SetColWidth(DgvMain, "保友品名", 90);
                DgvOpt.SetColMiddleCenter(DgvMain, "订单类型");
                DgvOpt.SetColWidth(DgvMain, "时间", 120);

                //计算数量
                float sxSum = 0;
                float ddSum = 0;
                float cpSum = 0;
                float zdSum = 0;
                float bdSum = 0;
                float fsSum = 0;
                float tzSum = 0;
                float workTimeSum = 0;

                DataTable sumDt = showDt.Copy();
                DataView sumDv = sumDt.DefaultView;
                sumDv.Sort = "订单号 ASC";
                sumDt = sumDv.ToTable();

                string ddTmp = "";
                for (int rowIndex = 0; rowIndex < sumDt.Rows.Count; rowIndex++)
                {
                    //获取ERP订单数量的重复，存在订单拆分的情况，所以需要重新排序去重
                    try
                    {
                        sxSum += float.Parse(sumDt.Rows[rowIndex]["上线数量"].ToString());
                        if (ddTmp != sumDt.Rows[rowIndex]["订单号"].ToString())
                        {
                            ddTmp = sumDt.Rows[rowIndex]["订单号"].ToString();
                            ddSum += float.Parse(sumDt.Rows[rowIndex]["ERP订单数量"].ToString());
                        }
                    }
                    catch
                    {
                        continue;
                    }
                    
                    //获取各(半)成品数量
                    if(sumDt.Rows[rowIndex]["品号"].ToString().Substring(0, 1) == "1")
                    {
                        cpSum += float.Parse(sumDt.Rows[rowIndex]["上线数量"].ToString()); ;
                    }
                    else if (sumDt.Rows[rowIndex]["品号"].ToString().Substring(0, 1) == "2")
                    {
                        if (sumDt.Rows[rowIndex]["品名"].ToString().Substring(0, 2) == "座垫")
                        {
                            zdSum += float.Parse(sumDt.Rows[rowIndex]["上线数量"].ToString()); ;
                        }
                        if (sumDt.Rows[rowIndex]["品名"].ToString().Substring(0, 2) == "背垫")
                        {
                            bdSum += float.Parse(sumDt.Rows[rowIndex]["上线数量"].ToString()); ;
                        }
                        if (sumDt.Rows[rowIndex]["品名"].ToString().Substring(0, 2) == "扶手")
                        {
                            fsSum += float.Parse(sumDt.Rows[rowIndex]["上线数量"].ToString()); ;
                        }
                        if (sumDt.Rows[rowIndex]["品名"].ToString().Substring(0, 2) == "头枕")
                        {
                            tzSum += float.Parse(sumDt.Rows[rowIndex]["上线数量"].ToString());
                        }
                    }

                    //获取排程生产总工时
                    workTimeSum += float.Parse(sumDt.Rows[rowIndex]["生产工时"].ToString());
                }
                labelSxSlSum.Text = "上线总数量：" + sxSum.ToString();
                labelDdSlSum.Text = "订单总数量：" + ddSum.ToString();
                labelCpSumSl.Text = "成品总数量：" + cpSum.ToString();
                labelBcpZdSumSl.Text = "座垫组立数量：" + zdSum.ToString();
                labelBcpBdSumSl.Text = "背垫组立数量：" + bdSum.ToString();
                labelBcpFsSumSl.Text = "扶手组立数量：" + fsSum.ToString();
                labelBcpTzSumSl.Text = "头枕组立数量：" + tzSum.ToString();

                labelWorkTime.Text = "排程总工时：" + workTimeSum.ToString();


                
                for (int rowIndex = 0; rowIndex < DgvMain.Rows.Count; rowIndex++)
                {
                    try
                    {
                        //总已排数量>ERP订单量，红字
                        if (float.Parse(DgvMain.Rows[rowIndex].Cells["总已排数量"].Value.ToString()) > float.Parse(DgvMain.Rows[rowIndex].Cells["ERP订单数量"].Value.ToString()))
                        {
                            DgvMain.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Purple;
                        }
                        else if (float.Parse(DgvMain.Rows[rowIndex].Cells["总已排数量"].Value.ToString()) < float.Parse(DgvMain.Rows[rowIndex].Cells["ERP订单数量"].Value.ToString()))
                        {
                            DgvMain.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        }
                        else
                        {
                            DgvMain.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        }

                        if (DgvMain.Rows[rowIndex].Cells["结束"].Value.ToString() == "y.指定结束")
                        {
                            DgvMain.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        }
                        if(DgvMain.Rows[rowIndex].Cells["状态"].Value.ToString() != "")
                        {
                            DgvMain.Rows[rowIndex].Cells["状态"].Style.BackColor = Color.YellowGreen;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                // 定位到传入的序号
                for (int rowIndex = 0; rowIndex < DgvMain.Rows.Count; rowIndex++)
                {
                    if(DgvMain.Rows[rowIndex].Cells["序号"].Value.ToString() == index.ToString())
                    {
                        DgvOpt.SelectLastRow(DgvMain, rowIndex);
                        break;
                    }
                }
                labelRowCount.Text = "总行数：" + DgvMain.Rows.Count.ToString();
            }
            else
            {
                labelSxSlSum.Text = "上线总数量：0";
                labelDdSlSum.Text = "订单总数量：0";
                labelRowCount.Text = "总行数：0";
                labelCpSumSl.Text = "成品总数量：0";
                labelBcpZdSumSl.Text = "座垫组立数量：0";
                labelBcpBdSumSl.Text = "背垫组立数量：0";
                labelBcpFsSumSl.Text = "扶手组立数量：0";
                labelBcpTzSumSl.Text = "头枕组立数量：0";
                labelWorkTime.Text = "排程总工时：0";
                DgvMain.DataSource = null;
            }
        }

        /// <summary>
        /// 未排订单的显示
        /// </summary>
        private void DgvShow2()
        {
            string sqlStrShow = @"SELECT RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) AS 订单号, RTRIM(MA002) AS 客户名称, RTRIM(TD004) AS 品号, RTRIM(TD005) AS 品名, RTRIM(TD006) AS 规格, 
                                    CAST(TD008 AS FLOAT) AS 订单数量, RTRIM(TD053) AS 配置方案, TD013 预交货日, COPTD.UDF12 AS 订单时间
                                    FROM COMFORT.dbo.COPTD 
                                    INNER JOIN COMFORT.dbo.COPTC ON TC001 = TD001 AND TC002 = TD002 AND TC027 = 'Y' AND TD016 NOT IN ('Y', 'y') AND TD008-TD009 > 0 
                                    INNER JOIN COMFORT.dbo.INVMB ON TD004 = MB001 AND MB025 IN ('M')
                                    INNER JOIN COMFORT.dbo.COPMA ON MA001 = TC004 
                                    LEFT JOIN WG_DB.dbo.SC_PLAN ON RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) = SC001 
                                    WHERE 1=1
                                    AND SC001 IS NULL 
                                    AND LEFT(COPTD.UDF12, 8) >= '20200301'
                                    AND TC001 NOT IN ('2217') ";
            if (TxBoxOrder.Text != "") sqlStrShow += string.Format(@" AND RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) LIKE '%{0}%' ", TxBoxOrder.Text);
            if (TxBoxName.Text != "") sqlStrShow += string.Format(@" AND TD005 LIKE '%{0}%' ", TxBoxName.Text);
            if (TxBoxWlno.Text != "") sqlStrShow += string.Format(@" AND TD004 LIKE '%{0}%' ", TxBoxWlno.Text);
            if (DtpStartDdDate.Checked) sqlStrShow += string.Format(@"AND LEFT(COPTD.UDF12, 8) >= '{0}' ", DtpStartDdDate.Value.ToString("yyyyMMdd"));
            if (DtpEndDdDate.Checked) sqlStrShow += string.Format(@"AND LEFT(COPTD.UDF12, 8) <= '{0}' ", DtpEndDdDate.Value.ToString("yyyyMMdd"));
            sqlStrShow += " ORDER BY COPTD.UDF12, RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) ";
            DataTable showDt = mssql.SQLselect(connWG, sqlStrShow);

            if (showDt != null)
            {
                DtOpt.DtDateFormat(showDt, "时间");
                DtOpt.DtDateFormat(showDt, "日期");
                DtOpt.DtDateFormat(showDt, "预交货日");
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColMiddleCenter(DgvMain, "数量");
                DgvOpt.SetColMiddleCenter(DgvMain, "工时");
                DgvOpt.SetColWidth(DgvMain, "订单号", 180);
                DgvOpt.SetColWidth(DgvMain, "品名", 180);
                DgvOpt.SetColWidth(DgvMain, "订单类型", 50);
                DgvOpt.SetColWidth(DgvMain, "急单", 50);
                DgvOpt.SetColWidth(DgvMain, "数量", 80);
                DgvOpt.SetColWidth(DgvMain, "工时", 80);
                DgvOpt.SetColWidth(DgvMain, "规格", 180);
                DgvOpt.SetColWidth(DgvMain, "时间", 120);

                //计算数量
                float sxSum = 0;
                float ddSum = 0;
                float cpSum = 0;
                float zdSum = 0;
                float bdSum = 0;
                float fsSum = 0;
                float tzSum = 0;
                float workTimeSum = 0;

                DataTable sumDt = showDt.Copy();
                DataView sumDv = sumDt.DefaultView;
                sumDv.Sort = "订单号 ASC";
                sumDt = sumDv.ToTable();

                //string ddTmp = "";
                for (int rowIndex = 0; rowIndex < sumDt.Rows.Count; rowIndex++)
                {
                    //获取各(半)成品数量
                    if (sumDt.Rows[rowIndex]["品号"].ToString().Substring(0, 1) == "1")
                    {
                        cpSum += float.Parse(sumDt.Rows[rowIndex]["订单数量"].ToString()); ;
                    }
                    else if (sumDt.Rows[rowIndex]["品号"].ToString().Substring(0, 1) == "2")
                    {
                        if (sumDt.Rows[rowIndex]["品名"].ToString().Substring(0, 2) == "座垫")
                        {
                            zdSum += float.Parse(sumDt.Rows[rowIndex]["订单数量"].ToString()); ;
                        }
                        if (sumDt.Rows[rowIndex]["品名"].ToString().Substring(0, 2) == "背垫")
                        {
                            bdSum += float.Parse(sumDt.Rows[rowIndex]["订单数量"].ToString()); ;
                        }
                        if (sumDt.Rows[rowIndex]["品名"].ToString().Substring(0, 2) == "扶手")
                        {
                            fsSum += float.Parse(sumDt.Rows[rowIndex]["订单数量"].ToString()); ;
                        }
                        if (sumDt.Rows[rowIndex]["品名"].ToString().Substring(0, 2) == "头枕")
                        {
                            tzSum += float.Parse(sumDt.Rows[rowIndex]["订单数量"].ToString());
                        }
                    }
                }
                labelSxSlSum.Text = "上线总数量：" + sxSum.ToString();
                labelDdSlSum.Text = "订单总数量：" + ddSum.ToString();
                labelCpSumSl.Text = "成品总数量：" + cpSum.ToString();
                labelBcpZdSumSl.Text = "座垫组立数量：" + zdSum.ToString();
                labelBcpBdSumSl.Text = "背垫组立数量：" + bdSum.ToString();
                labelBcpFsSumSl.Text = "扶手组立数量：" + fsSum.ToString();
                labelBcpTzSumSl.Text = "头枕组立数量：" + tzSum.ToString();

                labelWorkTime.Text = "排程总工时：" + workTimeSum.ToString();
                
                labelRowCount.Text = "总行数：" + DgvMain.Rows.Count.ToString();
            }
            else
            {
                labelSxSlSum.Text = "上线总数量：0";
                labelDdSlSum.Text = "订单总数量：0";
                labelRowCount.Text = "总行数：0";
                labelCpSumSl.Text = "成品总数量：0";
                labelBcpZdSumSl.Text = "座垫组立数量：0";
                labelBcpBdSumSl.Text = "背垫组立数量：0";
                labelBcpFsSumSl.Text = "扶手组立数量：0";
                labelBcpTzSumSl.Text = "头枕组立数量：0";
                labelWorkTime.Text = "排程总工时：0";
                DgvMain.DataSource = null;
            }
        }
        #endregion

        #endregion
    }
}
