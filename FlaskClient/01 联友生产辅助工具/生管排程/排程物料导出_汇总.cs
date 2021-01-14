using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ.生管排程
{
    public partial class 排程物料导出_汇总 : Form
    {
        private Mssql mssql = new Mssql();

        private string connWG = FormLogin.infObj.connWG;
        private string connYF = FormLogin.infObj.connYF;
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        private List<string> kidList = new List<string>();

        public 排程物料导出_汇总(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init()
        {
            UI();
            DgvOpt.NormalInit(DgvMain, readOnlyFlag: true, colHeadMiddleFlag: true);
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

        #region 按钮
        private void UI()
        {
            if (DgvMain.DataSource != null)
            {
                btnOutput.Enabled = true;
            }
            else
            {
                btnOutput.Enabled = false;
            }
        }

        private void DtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_Start(DtpStartDate, DtpEndDate);
            DgvMain.DataSource = null;
            UI();
        }

        private void DtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_End(DtpStartDate, DtpEndDate);
            DgvMain.DataSource = null;
            UI();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DgvOpt.SetShow(DgvMain);
            DgvMain.DataSource = null;
            DataTable dtShow = null;

            dtShow = GetShowDt();

            DtOpt.DtDateFormat(dtShow, "日期");
            DgvOpt.SetShow(DgvMain, dtShow);
            DgvOpt.SetColHeadMiddleCenter(DgvMain);
            DgvOpt.SetColMiddleCenter(DgvMain, "量");
            DgvOpt.SetColMiddleCenter(DgvMain, "库存");
            DgvOpt.SetColWidth(DgvMain, "量", 60);
            DgvOpt.SetColWidth(DgvMain, "库存", 60);
            DgvOpt.SetColWidth(DgvMain, "单位", 40);
            DgvOpt.SetColWidth(DgvMain, "品名", 200);
            DgvOpt.SetColWidth(DgvMain, "规格", 350);
            DgvOpt.SetColWidth(DgvMain, "供应商编号", 50);
            DgvOpt.SetColWidth(DgvMain, "供应商简称", 150);
            DgvOpt.SetColReadonly(DgvMain);
            UI();
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataSet.Tables.Add((DataTable)DgvMain.DataSource);
            excelObj.dataSet.Tables[0].TableName = "排程生产物料汇总导出";
            excelObj.defauleFileName = "排程生产物料汇总导出_" + DateTime.Now.ToString("yyyy-MM-dd");
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
        #endregion

        #region 逻辑
        private DataTable GetShowDt()
        {
            string sqlStr = @"SELECT 
                                材料品号, 材料品名, 材料规格, 
                                需领数量, 已领数量, 未领数量, 
                                主供应商批号库存, 次供应商批号库存, 
                                欠量,
                                ISNULL(主供应商待检量, 0) 主供应商待检量, ISNULL(次供应商待检量, 0) 次供应商待检量, 单位, 
                                主供应商编号, 主供应商简称, 次供应商编号, 次供应商简称 
                                FROM (
	                                SELECT RTRIM(INVMB2.MB001) 材料品号, RTRIM(INVMB2.MB002) 材料品名, RTRIM(INVMB2.MB003) 材料规格, RTRIM(INVMB2.MB004) 单位,
	                                CAST(SUM(TB004) AS FLOAT) 需领数量, CAST(SUM(TB005) AS FLOAT) 已领数量, CAST(SUM(TB004-TB005) AS FLOAT) 未领数量, 
	                                ISNULL(CAST(INVML.ML005 AS FLOAT), 0) 主供应商批号库存, ISNULL(CAST(INVML2.ML005 AS FLOAT), 0) 次供应商批号库存, 
	                                (CASE WHEN CAST(SUM(TB004-TB005) AS FLOAT) > (ISNULL(CAST(INVML.ML005 AS FLOAT), 0) + ISNULL(CAST(INVML2.ML005 AS FLOAT), 0)) THEN CAST(SUM(TB004-TB005) AS FLOAT) - ISNULL(CAST(INVML.ML005 AS FLOAT), 0) - ISNULL(CAST(INVML2.ML005 AS FLOAT), 0) ELSE 0 END) 欠量,
	                                RTRIM(PURMA1.MA001) 主供应商编号, RTRIM(PURMA1.MA002) 主供应商简称, RTRIM(PURMA2.MA001) 次供应商编号, RTRIM(PURMA2.MA002) 次供应商简称 
	                                FROM WG_DB.dbo.SC_PLAN(NOLOCK) AS SCPLAN 
	                                INNER JOIN dbo.MOCTA(NOLOCK) ON MOCTA.UDF02 = SCPLAN.K_ID 
	                                INNER JOIN dbo.MOCTB(NOLOCK) ON MOCTA.TA001 = MOCTB.TB001 AND MOCTA.TA002 = MOCTB.TB002 
	                                INNER JOIN dbo.COPTD(NOLOCK) ON RTRIM(COPTD.TD001)+'-'+RTRIM(COPTD.TD002)+'-'+RTRIM(COPTD.TD003) = SCPLAN.SC001 
	                                INNER JOIN dbo.INVMB(NOLOCK) ON INVMB.MB001 = COPTD.TD004 
	                                INNER JOIN dbo.INVMB(NOLOCK) AS INVMB2 ON INVMB2.MB001 = MOCTB.TB003
	                                LEFT JOIN dbo.PURMA(NOLOCK) AS PURMA1 ON PURMA1.MA001 = INVMB2.MB032 
	                                LEFT JOIN dbo.PURMA(NOLOCK) AS PURMA2 ON PURMA2.MA001 = INVMB2.UDF06
	                                LEFT JOIN dbo.PURMA(NOLOCK) AS PURMA3 ON PURMA3.MA001 = COPTD.UDF09 
	                                LEFT JOIN (
		                                SELECT ML001, ML004, SUM(ML005) AS ML005 FROM INVML WHERE ML002 NOT IN ('P06', 'P05', 'P09', 'P10', 'P11', 'P15', 'P20', 'P21') GROUP BY ML001, ML004
	                                ) AS INVML ON INVML.ML001 = INVMB2.MB001 AND INVML.ML004 = PURMA1.MA001
	                                LEFT JOIN (
		                                SELECT ML001, ML004, SUM(ML005) AS ML005 FROM INVML WHERE ML002 NOT IN ('P06', 'P05', 'P09', 'P10', 'P11', 'P15', 'P20', 'P21') GROUP BY ML001, ML004
	                                ) AS INVML2 ON INVML2.ML001 = INVMB2.MB001 AND INVML2.ML004 = PURMA2.MA001 AND PURMA1.MA001 != PURMA2.MA001

	                                WHERE 1=1
	                                AND INVMB2.MB025 IN ('P')
	                                AND INVMB2.MB034 IN ('L')";
            sqlStr += string.Format(@" AND SCPLAN.SC003 >= '{0}' ", DtpStartDate.Value.ToString("yyyyMMdd"));
            sqlStr += string.Format(@" AND SCPLAN.SC003 <= '{0}' ", DtpEndDate.Value.ToString("yyyyMMdd"));

            sqlStr += string.Format(@" AND (INVMB2.MB001 LIKE '%{0}%' OR INVMB2.MB002 LIKE '%{0}%' OR INVMB2.MB003 LIKE '%{0}%') ", TxbWl.Text);
            sqlStr += string.Format(@" AND (PURMA1.MA001 LIKE '%{0}%' OR PURMA1.MA002 LIKE '%{0}%' OR PURMA2.MA001 LIKE '%{0}%' OR PURMA2.MA002 LIKE '%{0}%')", TxbPh.Text);

            sqlStr += @"GROUP BY RTRIM(INVMB2.MB001), RTRIM(INVMB2.MB002), RTRIM(INVMB2.MB003), RTRIM(INVMB2.MB004), INVML.ML005, INVML2.ML005, RTRIM(PURMA1.MA001), RTRIM(PURMA1.MA002), RTRIM(PURMA2.MA001), RTRIM(PURMA2.MA002) ";
            if (CheckBoxQl.Checked) sqlStr += @"HAVING (CASE WHEN CAST(SUM(TB004-TB005) AS FLOAT) > (ISNULL(CAST(INVML.ML005 AS FLOAT), 0) + ISNULL(CAST(INVML2.ML005 AS FLOAT), 0)) THEN CAST(SUM(TB004-TB005) AS FLOAT) - ISNULL(CAST(INVML.ML005 AS FLOAT), 0) - ISNULL(CAST(INVML2.ML005 AS FLOAT), 0) ELSE 0 END) >0 ";

            sqlStr += @") AS A
                        LEFT JOIN (
	                        SELECT TG005, TH004, CAST(SUM(TH007) AS FLOAT) 主供应商待检量
	                        FROM PURTG INNER JOIN PURTH ON TG001 = TH001 AND TG002 = TH002 WHERE TG013 = 'N' AND TH030 = 'N' GROUP BY TG005, TH004
                        ) AS PURTG1 ON PURTG1.TH004 = 材料品号 AND PURTG1.TG005 = 主供应商编号
                        LEFT JOIN (
	                        SELECT TG005, TH004, CAST(SUM(TH007) AS FLOAT) 次供应商待检量
	                        FROM PURTG INNER JOIN PURTH ON TG001 = TH001 AND TG002 = TH002 WHERE TG013 = 'N' AND TH030 = 'N' GROUP BY TG005, TH004
                        ) AS PURTG2 ON PURTG2.TH004 = 材料品号 AND PURTG2.TG005 = 次供应商编号 AND 次供应商编号 != 主供应商编号
                        ORDER BY 主供应商编号, 次供应商编号, 材料品号";
            return mssql.SQLselect(connYF, sqlStr); ;
        }

        #endregion
    }
}
