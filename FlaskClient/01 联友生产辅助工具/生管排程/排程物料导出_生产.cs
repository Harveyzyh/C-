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
    public partial class 排程物料导出_生产 : Form
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

        public 排程物料导出_生产(string text = "")
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
            SetCmBoxDptTypeList();
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
            if (CheckErrData())
            {
                if (Msg.Show("存在排程上线数量与已绑定工单数量不一致，是否确定继续查询？", "提示", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    dtShow = GetShowDt();
                }
            }
            else
            {
                dtShow = GetShowDt();
            }
            if (dtShow != null)
            {
                SetPlanKid(GetPlanKid());
            }
            DtOpt.DtDateFormat(dtShow, "日期");
            DgvOpt.SetShow(DgvMain, dtShow);
            DgvOpt.SetColWidth(DgvMain, "生产单号", 180);
            UI();
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataSet.Tables.Add((DataTable)DgvMain.DataSource);
            excelObj.dataSet.Tables[0].TableName = "排程生产物料导出";
            excelObj.defauleFileName = "排程生产物料导出_" + DateTime.Now.ToString("yyyy-MM-dd");
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

        private void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if(CheckBoxAll.Checked == true)
            {
                CheckBoxNew.Checked = false;
                CheckBoxFinished.Checked = false;
            }
        }

        private void CheckBoxNew_CheckedChanged(object sender, EventArgs e)
        {
            if(CheckBoxNew.Checked == true)
            {
                CheckBoxAll.Checked = false;
                CheckBoxFinished.Checked = false;
            }
        }

        private void CheckBoxFinished_CheckedChanged(object sender, EventArgs e)
        {
            if(CheckBoxFinished.Checked == true)
            {
                CheckBoxAll.Checked = false;
                CheckBoxNew.Checked = false;
            }
        }
        #endregion

        #region 逻辑
        private void SetCmBoxDptTypeList()
        {
            string slqStr = @"Select Dpt from SC_PLAN_DPT_TYPE WHERE Type = 'Out' and Valid = 1 order by K_ID";
            DataTable dt = mssql.SQLselect(connWG, slqStr);
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    CmBoxDptType.Items.Add(dt.Rows[rowIndex][0].ToString());
                }
                CmBoxDptType.SelectedIndex = 0;
            }
        }

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

        private bool CheckErrData()
        {
            string sqlStr = @" SELECT K_ID 序号
                                    FROM WG_DB.dbo.SC_PLAN 
									LEFT JOIN COMFORT.dbo.COPTD ON RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) = SC001 
                                    LEFT JOIN COMFORT.dbo.COPTQ ON TQ001 = TD004 AND TQ002 = TD053 
                                    LEFT JOIN COMFORT.dbo.COPTC ON TC001 = TD001 AND TC002 = TD002 
                                    LEFT JOIN COMFORT.dbo.COPMA ON MA001 = TC004 
									LEFT JOIN (SELECT SC2.SC001 SC2001, SUM(SC2.SC013) SC2013 FROM WG_DB.dbo.SC_PLAN AS SC2 WHERE SC2.SC003 <= CONVERT(VARCHAR(8), GETDATE(), 112) GROUP BY SC2.SC001) AS SC2 ON SC2001 = SC001 
									LEFT JOIN (SELECT SC3.SC001 SC3001, SUM(SC3.SC013) SC3013 FROM WG_DB.dbo.SC_PLAN AS SC3 GROUP BY SC3.SC001) AS SC3 ON SC3001 = SC001 
									LEFT JOIN (SELECT SUM(TA015) TA015, TA006, RTRIM(TA026)+'-'+RTRIM(TA027)+'-'+RTRIM(TA028) TD, UDF02 FROM COMFORT.dbo.MOCTA 
                                            WHERE TA013 NOT IN ('V', 'U') GROUP BY RTRIM(TA026)+'-'+RTRIM(TA027)+'-'+RTRIM(TA028), TA006, UDF02) 
										AS MOCTA ON MOCTA.TD = SC001 AND MOCTA.UDF02 = K_ID AND MOCTA.TA006 = SC028 
                                    LEFT JOIN COMFORT.dbo.INVMB ON TD004 = MB001 
                                    WHERE 1 = 1 ";
            sqlStr += string.Format(@" AND SC003 >= '{0}' ", DtpStartDate.Value.ToString("yyyyMMdd"));
            sqlStr += string.Format(@" AND SC003 <= '{0}' ", DtpEndDate.Value.ToString("yyyyMMdd"));
            if (CmBoxDptType.Text != "全部") sqlStr += string.Format(@" AND SC023 LIKE '%{0}%' ", CmBoxDptType.Text);
            sqlStr += @"AND SC013 != ISNULL(CAST(MOCTA.TA015 AS FLOAT), 0) ";
            return mssql.SQLexist(connWG, sqlStr);
        }

        private DataTable GetPlanKid()
        {
            string sqlStr = @"SELECT DISTINCT K_ID 
                                FROM WG_DB.dbo.SC_PLAN(NOLOCK) AS SCPLAN 
                                INNER JOIN MOCTA(NOLOCK) AS MOCTA ON MOCTA.UDF02 = SCPLAN.K_ID AND SCPLAN.SC028 = MOCTA.TA006 
                                INNER JOIN MOCTB(NOLOCK) AS MOCTB ON MOCTA.TA001 = MOCTB.TB001 AND MOCTA.TA002 = MOCTB.TB002 
                                INNER JOIN INVMB(NOLOCK) AS INVMB ON INVMB.MB001 = MOCTA.TA006 
                                INNER JOIN CMSMW(NOLOCK) AS CMSMW ON CMSMW.MW001 = MOCTB.TB006 
                                WHERE 1=1 ";
            sqlStr += string.Format(@" AND SC003 >= '{0}' ", DtpStartDate.Value.ToString("yyyyMMdd"));
            sqlStr += string.Format(@" AND SC003 <= '{0}' ", DtpEndDate.Value.ToString("yyyyMMdd"));
            if (CmBoxDptType.Text != "全部") sqlStr += string.Format(@" AND SC023 LIKE '%{0}%' ", CmBoxDptType.Text);

            if (CheckBoxFinished.Checked)
            {
                sqlStr += @"AND SCPLAN.SC030 IS NOT NULL ";
            }
            if (CheckBoxNew.Checked)
            {
                sqlStr += @"AND SCPLAN.SC030 IS NULL ";
            }

            sqlStr += @"AND MOCTB.TB011 IN ('1', '2', '3', '5') 
                        AND MOCTA.TA011 != 'y' 
                        ORDER BY K_ID ";
            return mssql.SQLselect(connYF, sqlStr);
        }

        private DataTable GetShowDt()
        {
            string sqlStr = @"SELECT SCPLAN.SC003 排程日期, SCPLAN.SC023 生产车间, SCPLAN.SC001 生产单号, 
                                (CASE WHEN SC0.SC003 IS NULL THEN '新增' WHEN SC0.SC003>SCPLAN.SC003 THEN '提前，原排程日期'+SC0.SC003 WHEN SC0.SC003<SCPLAN.SC003 THEN '延后，原排程日期'+SC0.SC003 ELSE '' END) 状态, 
                                RTRIM(INVMB.MB001) 成品品号, RTRIM(INVMB.MB002) 成品品名, RTRIM(INVMB.MB003) 成品规格, INVMB.UDF12 产品系列, 
                                RTRIM(MOCTB.TB003) 物料品号, RTRIM(MOCTB.TB012) 材料品名, RTRIM(MOCTB.TB013) 材料规格, MOCTB.TB006 工艺, CAST(SUM(MOCTB.TB004) AS FLOAT) 需领料量, 
                                RTRIM(CMSMW.MW002) 工艺名称, CONVERT(VARCHAR(100), CMSMW.MW003) 组别, 
                                (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA001 ELSE PURMA1.MA001 END) 批号, 
                                (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA002 ELSE PURMA1.MA002 END) 批号说明, 
                                RTRIM(INVMB2.UDF02) 货位号
                                FROM WG_DB.dbo.SC_PLAN(NOLOCK) AS SCPLAN 
                                LEFT JOIN WG_DB.dbo.SC_PLAN_Snapshot AS SC0 ON SC0.K_ID = SCPLAN.K_ID AND SC0.SC001 = SCPLAN.SC001 AND SC0.SC000 = CONVERT(VARCHAR(8), DATEADD(DAY, -2, GETDATE()), 112)
                                INNER JOIN dbo.MOCTA(NOLOCK) AS MOCTA ON MOCTA.UDF02 = SCPLAN.K_ID AND SCPLAN.SC028 = MOCTA.TA006 
                                INNER JOIN dbo.MOCTB(NOLOCK) AS MOCTB ON MOCTA.TA001 = MOCTB.TB001 AND MOCTA.TA002 = MOCTB.TB002 
                                INNER JOIN dbo.COPTD(NOLOCK) ON RTRIM(COPTD.TD001)+'-'+RTRIM(COPTD.TD002)+'-'+RTRIM(COPTD.TD003) = SCPLAN.SC001 
                                INNER JOIN dbo.INVMB(NOLOCK) AS INVMB ON INVMB.MB001 = MOCTA.TA006 
                                INNER JOIN dbo.CMSMW(NOLOCK) AS CMSMW ON CMSMW.MW001 = MOCTB.TB006 
                                INNER JOIN dbo.INVMB(NOLOCK) AS INVMB2 ON INVMB2.MB001 = MOCTB.TB003 
                                LEFT JOIN dbo.PURMA(NOLOCK) AS PURMA1 ON PURMA1.MA001 = INVMB2.MB032 
                                LEFT JOIN dbo.PURMA(NOLOCK) AS PURMA2 ON PURMA2.MA001 = INVMB2.UDF06
                                LEFT JOIN dbo.PURMA(NOLOCK) AS PURMA3 ON PURMA3.MA001 = COPTD.UDF09 
                                WHERE 1=1 ";
            sqlStr += string.Format(@" AND SCPLAN.SC003 >= '{0}' ", DtpStartDate.Value.ToString("yyyyMMdd"));
            sqlStr += string.Format(@" AND SCPLAN.SC003 <= '{0}' ", DtpEndDate.Value.ToString("yyyyMMdd"));
            if (CmBoxDptType.Text != "全部") sqlStr += string.Format(@" AND SCPLAN.SC023 LIKE '%{0}%' ", CmBoxDptType.Text);

            if (CheckBoxFinished.Checked)
            {
                sqlStr += @"AND SCPLAN.SC030 IS NOT NULL ";
            }
            if (CheckBoxNew.Checked)
            {
                sqlStr += @"AND SCPLAN.SC030 IS NULL ";
            }

            sqlStr += @"AND MOCTB.TB011 IN ('1', '2', '3', '5') 
                        AND MOCTA.TA011 != 'y' 
                        GROUP BY SCPLAN.SC003, SCPLAN.SC023, SCPLAN.SC001, 
                            (CASE WHEN SC0.SC003 IS NULL THEN '新增' WHEN SC0.SC003>SCPLAN.SC003 THEN '提前，原排程日期'+SC0.SC003 WHEN SC0.SC003<SCPLAN.SC003 THEN '延后，原排程日期'+SC0.SC003 ELSE '' END), 
                            RTRIM(INVMB.MB001), RTRIM(INVMB.MB002), RTRIM(INVMB.MB003), INVMB.UDF12, 
                            RTRIM(MOCTB.TB003), RTRIM(MOCTB.TB012), RTRIM(MOCTB.TB013), MOCTB.TB006, RTRIM(CMSMW.MW002), 
                            CONVERT(VARCHAR(100), CMSMW.MW003), 
                            (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA001 ELSE PURMA1.MA001 END), 
                            (CASE WHEN INVMB2.MB002 IN('气压棒', '中管气压棒') AND COPTD.UDF09 IS NOT NULL AND COPTD.UDF09 != '' THEN PURMA3.MA002 ELSE PURMA1.MA002 END), 
                            RTRIM(INVMB2.UDF02)   
                        ORDER BY SCPLAN.SC003, SCPLAN.SC023, SCPLAN.SC001, RTRIM(MOCTB.TB003), MOCTB.TB006 ";
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
