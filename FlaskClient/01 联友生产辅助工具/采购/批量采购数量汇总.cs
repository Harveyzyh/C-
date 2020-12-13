using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace HarveyZ.采购
{
    public partial class 批量采购数量汇总 : Form
    {
        private string conn = FormLogin.infObj.connYF;
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        #region 初始化
        public 批量采购数量汇总(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            UI();
            DgvMain.ReadOnly = true;
            DgvOpt.SetColHeadMiddleCenter(DgvMain);
            DgvOpt.SetRowBackColor(DgvMain);
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
        }

        private void DtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_End(DtpStartDate, DtpEndDate);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DgvShow();
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataSet.Tables.Add((DataTable)DgvMain.DataSource);
            excelObj.dataSet.Tables[0].TableName = "批量采购数量汇总";
            excelObj.defauleFileName = "批量采购数量汇总_" + DateTime.Now.ToString("yyyy-MM-dd");
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
        private void DgvShow()
        {
            DgvMain.DataSource = null;
            string startDate = DtpStartDate.Value.ToString("yyyyMMdd");
            string endDate = DtpEndDate.Value.ToString("yyyyMMdd");

            DataTable dtBasic = GetBasicData(startDate, endDate);

            if (dtBasic != null)
            {
                int basicColCount = dtBasic.Columns.Count;

                DataTable dtShow = dtBasic.Copy();

                DataTable dtData = CheckBoxMonth.Checked ? GetCgData_Month(startDate, endDate) : GetCgData_Day(startDate, endDate);
                Normal.ConvertDate(dtData, "开单日期");

                DataConvert(dtShow, dtData);

                DgvMain.DataSource = dtShow;
                DgvOpt.SetColMiddleCenter(DgvMain, "-");
                DgvOpt.SetColMiddleCenter(DgvMain, "号");
                DgvOpt.SetColMiddleCenter(DgvMain, "单位");
                DgvOpt.SetColMiddleCenter(DgvMain, "供应商");
                DgvOpt.SetColMiddleCenter(DgvMain, "量");
                DgvOpt.SetColMiddleCenter(DgvMain, "安全库存");
            }
            else
            {
                Msg.Show("没有查询到数据");
            }
            UI();
        }
        
        private DataTable GetBasicData(string startDate, string endDate)
        {
            string slqStr = @"SELECT DISTINCT RTRIM(PURMA.MA001) 供应商编号, RTRIM(INVMB.MB001) 品号, RTRIM(INVMB.MB002) 品名, RTRIM(INVMB.MB003) 规格, 
                                RTRIM(INVMB.MB004) 单位, CAST(INVMC.MC004 AS FLOAT) 安全库存, CAST(INVMC.MC007 AS FLOAT) 库存数量
                                FROM MOCTA(NOLOCK) 
                                INNER JOIN MOCTB(NOLOCK) ON MOCTA.TA001 = MOCTB.TB001 AND MOCTA.TA002 = MOCTB.TB002 
                                INNER JOIN INVMB(NOLOCK) ON INVMB.MB001 = MOCTB.TB003 
                                INNER JOIN INVMC(NOLOCK) ON INVMB.MB001 = INVMC.MC001 AND INVMC.MC002 = INVMB.MB017 
                                LEFT JOIN PURMA(NOLOCK) ON PURMA.MA001 = INVMB.MB032 
                                WHERE 1=1 
                                AND MOCTA.TA003 BETWEEN '{0}' AND '{1}' 
                                AND INVMB.MB025 = 'P' AND INVMB.MB034 = 'R' 
                                GROUP BY RTRIM(MOCTA.TA003), RTRIM(PURMA.MA001), RTRIM(PURMA.MA002), RTRIM(INVMB.MB001), RTRIM(INVMB.MB002), RTRIM(INVMB.MB003), RTRIM(INVMB.MB004), 
	                                CAST(INVMC.MC004 AS FLOAT), CAST(INVMC.MC007 AS FLOAT) 
                                ORDER BY RTRIM(INVMB.MB001)";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, startDate, endDate));
            return dt;
        }

        private DataTable GetCgData_Day(string startDate, string endDate)
        {
            string slqStr = @"SELECT RTRIM(MOCTA.TA003) 开单日期, RTRIM(PURMA.MA001) 供应商编号, RTRIM(PURMA.MA002) 供应商, RTRIM(INVMB.MB001) 品号, RTRIM(INVMB.MB002) 品名, RTRIM(INVMB.MB003) 规格, 
                                RTRIM(INVMB.MB004) 单位, CAST(INVMC.MC004 AS FLOAT) 安全库存, CAST(INVMC.MC007 AS FLOAT) 库存数量, 
                                CAST(SUM(MOCTB.TB004) AS FLOAT) 需领数量
                                FROM MOCTA(NOLOCK) 
                                INNER JOIN MOCTB(NOLOCK) ON MOCTA.TA001 = MOCTB.TB001 AND MOCTA.TA002 = MOCTB.TB002 
                                INNER JOIN INVMB(NOLOCK) ON INVMB.MB001 = MOCTB.TB003 
                                INNER JOIN INVMC(NOLOCK) ON INVMB.MB001 = INVMC.MC001 AND INVMC.MC002 = INVMB.MB017 
                                LEFT JOIN PURMA(NOLOCK) ON PURMA.MA001 = INVMB.MB032 
                                WHERE 1=1 
                                AND MOCTA.TA003 BETWEEN '{0}' AND '{1}' 
                                AND INVMB.MB025 = 'P' AND INVMB.MB034 = 'R' 
                                GROUP BY RTRIM(MOCTA.TA003), RTRIM(PURMA.MA001), RTRIM(PURMA.MA002), RTRIM(INVMB.MB001), RTRIM(INVMB.MB002), RTRIM(INVMB.MB003), RTRIM(INVMB.MB004), 
	                                CAST(INVMC.MC004 AS FLOAT), CAST(INVMC.MC007 AS FLOAT) 
                                ORDER BY RTRIM(INVMB.MB001) ";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, startDate, endDate));
            return dt;
        }

        private DataTable GetCgData_Month(string startDate, string endDate)
        {
            string slqStr = @"SELECT LEFT(RTRIM(MOCTA.TA003), 6) 开单日期, RTRIM(PURMA.MA001) 供应商编号, RTRIM(PURMA.MA002) 供应商, RTRIM(INVMB.MB001) 品号, RTRIM(INVMB.MB002) 品名, RTRIM(INVMB.MB003) 规格, 
                                RTRIM(INVMB.MB004) 单位, CAST(INVMC.MC004 AS FLOAT) 安全库存, CAST(INVMC.MC007 AS FLOAT) 库存数量, 
                                CAST(SUM(MOCTB.TB004) AS FLOAT) 需领数量
                                FROM MOCTA(NOLOCK) 
                                INNER JOIN MOCTB(NOLOCK) ON MOCTA.TA001 = MOCTB.TB001 AND MOCTA.TA002 = MOCTB.TB002 
                                INNER JOIN INVMB(NOLOCK) ON INVMB.MB001 = MOCTB.TB003 
                                INNER JOIN INVMC(NOLOCK) ON INVMB.MB001 = INVMC.MC001 AND INVMC.MC002 = INVMB.MB017 
                                LEFT JOIN PURMA(NOLOCK) ON PURMA.MA001 = INVMB.MB032 
                                WHERE 1=1 
                                AND MOCTA.TA003 BETWEEN '{0}' AND '{1}' 
                                AND INVMB.MB025 = 'P' AND INVMB.MB034 = 'R' 
                                GROUP BY LEFT(RTRIM(MOCTA.TA003), 6), RTRIM(PURMA.MA001), RTRIM(PURMA.MA002), RTRIM(INVMB.MB001), RTRIM(INVMB.MB002), RTRIM(INVMB.MB003), RTRIM(INVMB.MB004), 
	                                CAST(INVMC.MC004 AS FLOAT), CAST(INVMC.MC007 AS FLOAT) 
                                ORDER BY RTRIM(INVMB.MB001) ";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, startDate, endDate));
            return dt;
        }

        private void DataConvert(DataTable dtShow, DataTable dtData)
        {
            if(dtShow != null)
            {
                int basicColCount = dtShow.Columns.Count;

                List<string> dateList = GetDateList(dtData);
                foreach(string tmp in dateList)
                {
                    dtShow.Columns.Add(tmp);
                }

                for(int colIndex = basicColCount; colIndex < dtShow.Columns.Count; colIndex++)
                {
                    string colName = dtShow.Columns[colIndex].ColumnName;

                    for(int rowIndex = 0; rowIndex < dtShow.Rows.Count; rowIndex++)
                    {
                        string rowName = dtShow.Rows[rowIndex]["品号"].ToString();
                        dtShow.Rows[rowIndex][colName] = GetDtValue(dtData, rowName, colName);
                    }
                }
            }
        }

        private List<string> GetDateList(DataTable dtData)
        {
            List<string> list = new List<string>();

            DataTable dt = dtData.Copy();
            DataView dv = dt.DefaultView;
            dv.Sort = "开单日期 ASC";
            dt = dv.ToTable();

            string tmp = "";

            for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                if (tmp != dt.Rows[rowIndex]["开单日期"].ToString())
                {
                    tmp = dt.Rows[rowIndex]["开单日期"].ToString();
                    list.Add(tmp);
                }
            } 
            return list;
        }

        private string GetDtValue(DataTable dtData, string wlno, string date)
        {
            string tmp = "";
            for(int rowIndex = 0; rowIndex < dtData.Rows.Count; rowIndex++)
            {
                if(dtData.Rows[rowIndex]["品号"].ToString() == wlno && dtData.Rows[rowIndex]["开单日期"].ToString() == date)
                {
                    string s1 = dtData.Rows[rowIndex]["品号"].ToString();
                    string s2 = dtData.Rows[rowIndex]["开单日期"].ToString();
                    tmp = dtData.Rows[rowIndex]["需领数量"].ToString();
                    break;
                }
                else
                {
                    tmp = "";
                }
            }
            return tmp;
        }

        #endregion
    }
}
