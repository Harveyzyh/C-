using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ;
using System.Collections.Generic;
using System.Timers;

namespace 联友生产辅助工具.生管排程
{
    public partial class 生产排程 : Form
    {
        #region 静态数据设置
        private delegate string NewTaskDelegate(DataTable dttmp); //任务代理
        
        private Mssql mssql = new Mssql();

        private string connWG = FormLogin.infObj.connWG;
        private string connERP = FormLogin.infObj.connYF;
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;
        #endregion

        #region 窗体设计
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
            BtnOutput.Enabled = false;
            DtpEndDate.Checked = false;
            DgvShow();
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
        }

        private void BtnShow_Click(object sender, EventArgs e) //刷新按钮
        {
            DgvShow();
            UI();
        }

        private void BtnInput_Click(object sender, EventArgs e)
        {
            Form formInput = new 生管排程导入导出部门选择("导入");
            formInput.ShowDialog();
            string Dpt = 生管排程导入导出部门选择.Dpt;
            if (Dpt != null)
            {
                Form frm = new 生产排程导入中();
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
                            GetInsertDt(excelObj.dataDt);
                            MessageBox.Show("导入成功", "提示");
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
                    MessageBox.Show(es.ToString());
                }
                finally
                {
                    frm.Dispose();
                }
            }
        }

        private void BtnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataDt = (DataTable)DgvMain.DataSource;
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

        private void DgvMain_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (contextMenuStrip_DgvMain.Items.Count > 0)
                {
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
        }

        private void contextMenuStrip_DgvMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuStrip_DgvMain.Visible = false;
            if (e.ClickedItem.Text == "增加")
            {
                contextMenuStrip_DgvMain_ItemClicked_New();
            }
            if (e.ClickedItem.Text == "修改")
            {
                contextMenuStrip_DgvMain_ItemClicked_Edit();
            }
            if (e.ClickedItem.Text == "删除")
            {
                contextMenuStrip_DgvMain_ItemClicked_Delete();
            }
        }
        #endregion

        #region 逻辑
        private void SetContextMenuStripTypeList()
        {
            if (newFlag) contextMenuStrip_DgvMain.Items.Add("增加");
            if (editFlag) contextMenuStrip_DgvMain.Items.Add("修改");
            if (delFlag) contextMenuStrip_DgvMain.Items.Add("删除");
        }

        private void SetCmBoxDptTypeList()
        {
            string sqlstr = @"Select Dpt from SC_PLAN_DPT_TYPE WHERE Type = 'Out' and Valid = 1 order by K_ID";
            DataTable dt = mssql.SQLselect(connWG, sqlstr);
            if (dt != null)
            {
                for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    CmBoxDptType.Items.Add(dt.Rows[rowIndex][0].ToString());
                }
                CmBoxDptType.SelectedIndex = 0;
            }
        }

        private int GetNowDate()
        {
            string sqlstr = @"SELECT isnull(LEFT(dbo.f_getTime(1), 8), 0) ";
            return int.Parse(mssql.SQLselect(connERP, sqlstr).Rows[0][0].ToString());
        }

        private string GetInsertDt(DataTable dt)//数据表转成sql语句
        {
            DataTable dtInsert = new DataTable();
            dtInsert.Columns.Add("生产单号", Type.GetType("System.String"));
            dtInsert.Columns.Add("上线日期", Type.GetType("System.String"));
            dtInsert.Columns.Add("数量", Type.GetType("System.String"));
            dtInsert.Columns.Add("测试量", Type.GetType("System.String"));
            dtInsert.Columns.Add("生产车间", Type.GetType("System.String"));
            DataRow dtRowTmp = null;
            int rowIndex = 0;
            int row_total = dt.Rows.Count;
            int col_total = dt.Columns.Count;
            string returnstr = "";

            string SC003 = "", SC001 = "";

            int SysDate = GetNowDate();
            int WorkTime = 0;
            
            for (; rowIndex < row_total; rowIndex++ )
            {
                //限定日期
                try
                {
                    SC003 = dt.Rows[rowIndex][0].ToString().Replace("-", "").Replace("/", "");
                    WorkTime = int.Parse(SC003);
                    if(WorkTime < SysDate)
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

                    SC001 = SC001.Replace("（新增）", "").Replace("（变更）", "").Replace("（更改）", "").Replace("(新增)", "").
                        Replace("(变更)", "").Replace("(更改)", "").Replace("(变更）", "").Replace("(新增）", "").Replace("(更改）", "");

                    SC001 = SC001.Split('-')[0].Trim() + '-' + SC001.Split('-')[1].Trim() + '-' + SC001.Split('-')[2].Trim();

                    dtRowTmp = dtInsert.NewRow();
                    dtRowTmp[0] = SC001;
                    dtRowTmp[1] = SC003;
                    dtRowTmp[2] = dt.Rows[rowIndex][12].ToString().Split('/')[0];
                    dtRowTmp[3] = dt.Rows[rowIndex][13].ToString().Split('/')[0];
                    dtRowTmp[4] = dt.Rows[rowIndex][22].ToString();
                    dtInsert.Rows.Add(dtRowTmp);
                }
                else continue;
            }

            if (dtInsert != null && dtInsert.Rows.Count != 0)
            {
                SqlInsertWork(WorkDate: (SysDate).ToString(), dt: dtInsert);
                UptInfo();
            }
            else
            {
                MessageBox.Show("导入异常，找不到没有可导入数据", "错误");
            }

            return returnstr;
        }

        private void SqlInsertWork(string WorkDate, DataTable dt)
        {
            string sqlstrDel = @"DELETE FROM WG_DB.dbo.SC_PLAN WHERE SC003 >= '{0}' AND SC023 = '{1}'";
            string sqlstrInsert = @"INSERT INTO WG_DB.dbo.SC_PLAN (CREATOR, CREATE_DATE, K_ID, SC001, SC003, SC013, SC014, SC023) 
                                    VALUES ('{0}', LEFT(COMFORT.dbo.f_getTime(1), 14), (SELECT ISNULL(MAX(K_ID), 0) + 1 FROM WG_DB.dbo.SC_PLAN), 
                                    '{1}', '{2}', '{3}', '{4}', '{5}') ";
            mssql.SQLexcute(connWG, string.Format(sqlstrDel, WorkDate, dt.Rows[0][4].ToString()));
            for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                mssql.SQLexcute(connWG, string.Format(sqlstrInsert, FormLogin.infObj.userId, dt.Rows[rowIndex][0], dt.Rows[rowIndex][1], 
                    dt.Rows[rowIndex][2], dt.Rows[rowIndex][3], dt.Rows[rowIndex][4]));
            }
        }
        
        //补全基本信息
        private void UptInfo()
        {
            string sqlstr = @"UPDATE WG_DB.dbo.SC_PLAN SET 
                                SC002 = X00, SC004 = X01, SC005 = X02, SC006 = X03, SC007 = X04, SC008 = X05, 
                                SC009 = X06, SC010 = X07, SC011 = X08, SC012 = X09, SC015 = X10, 
                                SC016 = X11, SC017 = X12, SC018 = X13, SC019 = X14, SC020 = X15, SC021 = X16, 
                                SC022 = X17, SC024 = X18, SC025 = X19, SC026 = X20, SC027 = X21, SC028 = X22, SC029 = X23 
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
            mssql.SQLexcute(connWG, sqlstr);
        }

        private void DgvShow(int index = 0) //数据库资料显示到界面
        {
            string sqlstrShow = @" SELECT K_ID 序号, SC003 上线日期, SC023 生产车间, SC001 订单号, SC002 订单类型, SC010 品名, 
                                    SC012 规格, SC011 保友品名, SC013 上线数量, ISNULL(CAST(TD008 AS FLOAT), 0) ERP订单数量, ISNULL(SC2013, 0) 至今天已排数量, ISNULL(SC3013, 0) 总已排数量,
									(CASE COPTD.UDF04 WHEN '1' THEN '1.内销' WHEN '2' THEN '2.一般贸易' WHEN '3' THEN '3.合同' ELSE COPTD.UDF04 END) 贸易方式, SC028 品号, SC015 配置方案, 
                                    SC016 配置方案描述, SC017 描述备注, SC004 客户名称, SC005 注意事项, 
                                    SC006 变更原因, SC007 出货日期, SC008 验货日期, SC009 PO#, SC018 柜型柜数, SC019 目的地, 
                                    SC024 客户编码, SC025 电商编码, SC026 急单, SC027 订单日期 
                                    FROM WG_DB.dbo.SC_PLAN 
									LEFT JOIN COMFORT.dbo.COPTD ON RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) = SC001
									LEFT JOIN (SELECT SC2.SC001 SC2001, SUM(SC2.SC013) SC2013 FROM WG_DB.dbo.SC_PLAN AS SC2 WHERE SC2.SC003 <= CONVERT(VARCHAR(8), GETDATE(), 112) GROUP BY SC2.SC001) AS SC2 ON SC2001 = SC001
									LEFT JOIN (SELECT SC3.SC001 SC3001, SUM(SC3.SC013) SC3013 FROM WG_DB.dbo.SC_PLAN AS SC3 GROUP BY SC3.SC001) AS SC3 ON SC3001 = SC001 
                                    WHERE 1 = 1 ";
            if (TxBoxOrder.Text != "") sqlstrShow += @" AND SC001 LIKE '%" + TxBoxOrder.Text + "%' ";
            if (TxBoxName.Text != "") sqlstrShow += @" AND SC010 LIKE '%" + TxBoxName.Text + "%' ";
            if (DtpStartDate.Checked) sqlstrShow += @" AND SC003 >= '" + DtpStartDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpEndDate.Checked) sqlstrShow += @" AND SC003 <= '" + DtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            if (CmBoxDptType.Text != "全部") sqlstrShow += @" AND SC023 = '" + CmBoxDptType.Text + "' ";
            if (CheckBoxShowSlBig.Checked) sqlstrShow += @"AND SC3013 > ISNULL(CAST(TD008 AS FLOAT), 0) ";
            if (CheckBoxShowSlSmall.Checked) sqlstrShow += @"AND SC3013 < ISNULL(CAST(TD008 AS FLOAT), 0) ";
            sqlstrShow += " ORDER BY SC003, SC001 ";
            DataTable showDt = mssql.SQLselect(connWG, sqlstrShow);

            if (showDt != null)
            {

                DtOpt.DtDateFormat(showDt, "日期");
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColMiddleCenter(DgvMain, "数量");
                DgvOpt.SetColWidth(DgvMain, "订单号", 180);

                //计算合计，因为排序不一样，会出现订单数量的重复，需要重新排序去重
                float sxSum = 0;
                float ddSum = 0;
                DataTable sumDt = showDt.Copy();
                DataView sumDv = sumDt.DefaultView;
                sumDv.Sort = "订单号 ASC";
                sumDt = sumDv.ToTable();
                string ddTmp = "";
                for (int rowIndex = 0; rowIndex < sumDt.Rows.Count; rowIndex++)
                {
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
                }
                labelSxSlSum.Text = "上线总数量：" + sxSum.ToString();
                labelDdSlSum.Text = "订单总数量：" + ddSum.ToString();


                //总已排数量>ERP订单量，红字
                for(int rowIndex = 0; rowIndex < DgvMain.Rows.Count; rowIndex++)
                {
                    try
                    {
                        if (float.Parse(DgvMain.Rows[rowIndex].Cells["总已排数量"].Value.ToString()) > float.Parse(DgvMain.Rows[rowIndex].Cells["ERP订单数量"].Value.ToString()))
                        {
                            DgvMain.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        }
                        else if (float.Parse(DgvMain.Rows[rowIndex].Cells["总已排数量"].Value.ToString()) < float.Parse(DgvMain.Rows[rowIndex].Cells["ERP订单数量"].Value.ToString()))
                        {
                            DgvMain.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        }
                        else
                        {
                            DgvMain.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
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
                labelCount.Text = "总行数：" + DgvMain.Rows.Count.ToString();
            }
            else
            {
                labelSxSlSum.Text = "上线总数量：0";
                labelDdSlSum.Text = "订单总数量：0";
                labelCount.Text = "总行数：0";
                DgvMain.DataSource = null;
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

        private void contextMenuStrip_DgvMain_ItemClicked_New()
        {
            string index = "";
            生产排程修改 frm = new 生产排程修改("New", "", "", "", "", "", "", "", "", "");
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                index = 生产排程修改.indexRtn;
                UptInfo();
                frm.Dispose();
                int i = 0;
                if (int.TryParse(index, out i))
                {
                    DgvShow(i);
                }
                else
                {
                    DgvShow();
                }
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked_Edit()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string index = "";
            生产排程修改 frm = new 生产排程修改("Edit", DgvMain.Rows[rowIndex].Cells["序号"].Value.ToString(), DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString(),
                DgvMain.Rows[rowIndex].Cells["品号"].Value.ToString(), DgvMain.Rows[rowIndex].Cells["品名"].Value.ToString(), DgvMain.Rows[rowIndex].Cells["规格"].Value.ToString(),
                DgvMain.Rows[rowIndex].Cells["配置方案"].Value.ToString(), DgvMain.Rows[rowIndex].Cells["上线日期"].Value.ToString(), DgvMain.Rows[rowIndex].Cells["生产车间"].Value.ToString(),
                DgvMain.Rows[rowIndex].Cells["上线数量"].Value.ToString());
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                index = 生产排程修改.indexRtn;
                frm.Dispose();
                DgvShow(int.Parse(index));
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked_Delete()
        {
            string sqlstr = @"DELETE FROM dbo.SC_PLAN WHERE K_ID = '{0}' AND SC001 = '{1}' AND SC003 = '{2}' AND SC013 = '{3}' AND SC023 = '{4}'";
            int rowIndex = DgvMain.CurrentCell.RowIndex;
            string msg = string.Format("是否确认删除当前行！\n\n序号：'{0}'\n订单号：'{1}'\n上线日期'{2}'\n生产车间：'{3}'\n上线数量：'{4}'",
                    DgvMain.Rows[rowIndex].Cells["序号"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                    DgvMain.Rows[rowIndex].Cells["生产车间"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["上线数量"].Value.ToString());

            if (Msg.Show(msg) == DialogResult.OK)
            {
                mssql.SQLexcute(connWG, string.Format(sqlstr, DgvMain.Rows[rowIndex].Cells["序号"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["上线日期"].Value.ToString().Replace("-", ""),
                    DgvMain.Rows[rowIndex].Cells["上线数量"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["生产车间"].Value.ToString()
                    ));
                DgvShow();
            }
        }
        #endregion
    }
}
