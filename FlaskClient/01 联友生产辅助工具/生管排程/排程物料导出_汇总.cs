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
                    UpdOutputFlag();
                    Msg.Show("Excel导出成功！");
                }
                else
                {
                    Msg.Show(excelObj.msg, "错误");
                }
            }
        }

        private void CheckBoxJh_CheckedChanged(object sender, EventArgs e)
        {
            if(CheckBoxJh.Checked == true)
            {
                CheckBoxJhDl.Checked = false;
            }
        }

        private void CheckBoxJhDl_CheckedChanged(object sender, EventArgs e)
        {
            if(CheckBoxJhDl.Checked == true)
            {
                CheckBoxJh.Checked = false;
            }
        }
        #endregion

        #region 逻辑

        private string GetTime()
        {
            string slqStr = @"SELECT LEFT(dbo.f_getTime(1), 14) ";
            return mssql.SQLselect(connYF, slqStr).Rows[0][0].ToString();
        }

        private void UpdOutputFlag()
        {
            string time = GetTime();
            string slqStr = @"UPDATE SC_PLAN SET SC030 = '{1}' WHERE K_ID = '{0}' ";
            string kidTmp = "";
            if (kidList.Count > 0)
            {
                foreach(string kid in kidList)
                {
                    if (kid != kidTmp)
                    {
                        kidTmp = kid;
                        mssql.SQLexcute(connWG, string.Format(slqStr, kid, time));
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private DataTable GetShowDt()
        {
            string sqlStr = @"SELECT 
                                SC003 上线日期, RTRIM(INVMB2.MB001) 材料品号, RTRIM(INVMB2.MB002) 材料品名, RTRIM(INVMB2.MB003) 材料规格, 
                                CAST(SUM(TB004) AS FLOAT) 需领数量, CAST(SUM(TB005) AS FLOAT) 已领数量, CAST(SUM(TB004-TB005) AS FLOAT) 未领数量, 
                                ISNULL(CAST(SUM(PURTG.TH015) AS FLOAT), 0) 进货数量, 
                                (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA001 ELSE PURMA1.MA001 END) 批号, 
                                (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA002 ELSE PURMA1.MA002 END) 批号说明
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
	                                SELECT PURTG.UDF02 AS UDF02, PURTG.TG005, PURTH.TH004, PURTH.TH015 
	                                FROM PURTG INNER JOIN dbo.PURTH ON PURTG.TG001 = PURTH.TH001 AND PURTG.TG002 = PURTH.TH002 AND PURTG.TG013 = 'Y' AND PURTH.TH030 = 'Y'
                                ) AS PURTG ON PURTG.UDF02 = SCPLAN.SC003 AND PURTG.TH004 = MOCTB.TB003
	                                AND PURTG.TG005 = (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA001 ELSE PURMA1.MA001 END) 
                                WHERE 1=1
                                AND INVMB2.MB025 IN ('P')
                                AND INVMB2.MB034 IN ('L')";
            sqlStr += string.Format(@" AND SCPLAN.SC003 >= '{0}' ", DtpStartDate.Value.ToString("yyyyMMdd"));
            sqlStr += string.Format(@" AND SCPLAN.SC003 <= '{0}' ", DtpEndDate.Value.ToString("yyyyMMdd"));

            sqlStr += string.Format(@" AND (INVMB2.MB001 LIKE '%{0}%' OR INVMB2.MB002 LIKE '%{0}%' OR INVMB2.MB003 LIKE '%{0}%') ", TxbWl.Text);
            sqlStr += string.Format(@" AND ((CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA001 ELSE PURMA1.MA001 END) LIKE '%{0}%'
                                        	OR (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA002 ELSE PURMA1.MA002 END) LIKE '%{0}%')", TxbPh.Text);

            sqlStr += @"GROUP BY SC003, RTRIM(INVMB2.MB001), RTRIM(INVMB2.MB002), RTRIM(INVMB2.MB003), 
                            (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA001 ELSE PURMA1.MA001 END), 
                            (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA002 ELSE PURMA1.MA002 END) ";
            if (CheckBoxJh.Checked) sqlStr += @"HAVING ISNULL(CAST(SUM(PURTG.TH015) AS FLOAT), 0) = 0 ";
            if (CheckBoxJhDl.Checked) sqlStr += @"HAVING ISNULL(CAST(SUM(PURTG.TH015) AS FLOAT), 0) < SUM(TB004-TB005) ";

            sqlStr += @"ORDER BY SC003, 
                            (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA001 ELSE PURMA1.MA001 END),
                            RTRIM(INVMB2.MB001), RTRIM(INVMB2.MB002), RTRIM(INVMB2.MB003)";
            return mssql.SQLselect(connYF, sqlStr); ;
        }

        private void SetPlanKid(DataTable dt)
        {
            kidList.Clear();
            if(dt != null)
            {
                for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    kidList.Add(dt.Rows[rowIndex]["K_ID"].ToString());
                }
            }
        }

        #endregion
    }
}
