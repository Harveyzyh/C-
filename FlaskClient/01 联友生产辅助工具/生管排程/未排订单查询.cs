using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HarveyZ.生管排程
{
    public partial class 未排订单查询 : Form
    {
        #region 静态数据设置
        
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

        #region 窗体设计
        public 未排订单查询(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            SetContextMenuStripTypeList();
            BtnOutput.Enabled = false;
            DtpEndDate.Checked = false;
            DgvShow();
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

        private void BtnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataDt = (DataTable)DgvMain.DataSource;
            excelObj.defauleFileName = "未排订单导出_" + DateTime.Now.ToString("yyyy-MM-dd");
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
            if (e.ClickedItem.Text == "增加至排程")
            {
                contextMenuStrip_DgvMain_ItemClicked_New();
            }
            else
            {
                contextMenuStrip_DgvMain_ItemClicked_Select(e.ClickedItem.Text);
            }
        }
        #endregion

        #region 逻辑
        private void SetContextMenuStripTypeList()
        {
            contextMenuStrip_DgvMain.Items.Add("查询当前订单号");
            contextMenuStrip_DgvMain.Items.Add("查询当前生产单号");
            contextMenuStrip_DgvMain.Items.Add("查询当前品名");
            if (newFlag) contextMenuStrip_DgvMain.Items.Add("增加至排程");
        }

        //补全基本信息
        private void UptInfo()
        {
            string slqStr = @"UPDATE WG_DB.dbo.SC_PLAN SET 
                                SC002 = X00, SC004 = X01, SC005 = X02, SC006 = X03, SC007 = X04, SC008 = X05, 
                                SC009 = X06, SC010 = X07, SC011 = X08, SC012 = X09, SC015 = X10, 
                                SC016 = X11, SC017 = X12, SC018 = X13, SC019 = X14, SC020 = X15, SC021 = X16, 
                                SC022 = X17, SC024 = X18, SC025 = X19, SC026 = X20, SC027 = X21, SC028 = X22
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
	                                RTRIM(COPTD.TD004) AS X22 
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

        private void DgvShow() //数据库资料显示到界面
        {
            string slqStrShow = @" SELECT RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) AS 订单号, RTRIM(MA002) AS 客户名称, RTRIM(TD004) AS 品号, RTRIM(TD005) AS 品名, RTRIM(TD006) AS 规格, 
                                    CAST(TD008 AS FLOAT) AS 订单数量, RTRIM(TD053) AS 配置方案, TD013 预交货日, COPTD.UDF12 AS 订单更新时间
                                    FROM COMFORT.dbo.COPTD 
                                    INNER JOIN COMFORT.dbo.COPTC ON TC001 = TD001 AND TC002 = TD002 AND TC027 = 'Y' AND TD016 NOT IN ('Y', 'y') AND TD008-TD009 > 0 
                                    INNER JOIN COMFORT.dbo.INVMB ON TD004 = MB001 AND MB025 IN ('M')
                                    INNER JOIN COMFORT.dbo.COPMA ON MA001 = TC004 
                                    LEFT JOIN WG_DB.dbo.SC_PLAN ON RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) = SC001 
                                    WHERE 1=1
                                    AND SC001 IS NULL 
                                    AND LEFT(COPTD.UDF12, 8) >= '20200301'
                                    AND TC001 NOT IN ('2217')";
            if (TxBoxOrder.Text != "") slqStrShow += @" AND RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) LIKE '%" + TxBoxOrder.Text + "%' ";
            if (TxBoxName.Text != "") slqStrShow += @" AND RTRIM(TD005) LIKE '%" + TxBoxName.Text + "%' ";
            if (DtpStartDate.Checked) slqStrShow += @" AND LEFT(COPTD.UDF12, 8) >= '" + DtpStartDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpEndDate.Checked) slqStrShow += @" AND LEFT(COPTD.UDF12, 8) <= '" + DtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            slqStrShow += " ORDER BY COPTD.UDF12, RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) ";
            DataTable showDt = mssql.SQLselect(connWG, slqStrShow);

            if (showDt != null)
            {

                DtOpt.DtDateFormat(showDt, "时间");
                DtOpt.DtDateFormat(showDt, "预交货日");
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColMiddleCenter(DgvMain, "数量");
                DgvOpt.SetColWidth(DgvMain, "订单号", 180);
                DgvOpt.SetColWidth(DgvMain, "品名", 180);
                DgvOpt.SetColWidth(DgvMain, "订单更新时间", 180);


                float ddSum = 0;
                labelDdSlSum.Text = "订单总数量：" + ddSum.ToString();
                
            }
            else
            {
                labelDdSlSum.Text = "订单总数量：0";
                DgvMain.DataSource = null;
            }
        }

        private void contextMenuStrip_DgvMain_ItemClicked_New()
        {
            int rowIndex = DgvMain.CurrentCell.RowIndex;

            生产排程修改 frm = new 生产排程修改("Add", "", DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString());
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                frm.Dispose();
                if (生产排程修改.saveFlag)
                {
                    UptInfo();
                    Msg.Show("订单：" + DgvMain.Rows[rowIndex].Cells["订单号"].Value.ToString() + "已增加至排成");
                    DgvShow();
                }
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
        #endregion
    }
}
