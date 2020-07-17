﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.报表
{
    public partial class 销货信息_带入库部门_查询 : Form
    {
        private string conn = FormLogin.infObj.connYF;
        private Mssql mssql = new Mssql();

        public 销货信息_带入库部门_查询()
        {
            InitializeComponent();
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DgvShow();
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataDt = (DataTable)DgvMain.DataSource;
            excelObj.defauleFileName = "销货明细_" + DateTime.Now.ToString("yyyy-MM-dd");
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

        private void DgvShow()
        {
            DgvMain.DataSource = null;
            string dateTime = dateTimePicker1.Value.ToString("yyyyMM");
            string sqlstr = @"declare @Dates varchar(20), @StartDate varchar(20), @EndDate varchar(20)
                                set @Dates = '{0}'
                                set @StartDate = @Dates + '01'
                                set @EndDate = @Dates + '31'
                                SELECT DISTINCT A1 AS 销货单号, A2 AS 客户简称, 
                                A4 AS 品号, A5 AS 品名, A6 AS 规格, B2 AS 仓库名称, 
                                A7 AS 销货数量, A3 AS 订单单号, 
                                A8 AS 销货日期, 
                                B3 AS 销货部门 
                                FROM 
                                (SELECT DISTINCT RTRIM(COPTH.TH001) + '-' + RTRIM(COPTH.TH002) + '-' + RTRIM(COPTH.TH003)AS A1, RTRIM(MA002) AS A2, 
                                RTRIM(COPTH.TH014) + '-' + RTRIM(COPTH.TH015) + '-' + RTRIM(COPTH.TH016) AS A3, RTRIM(COPTH.TH004) AS A4, 
                                RTRIM(INVMB.MB002) AS A5, RTRIM(INVMB.MB003) AS A6, CONVERT(FLOAT, TH008) AS A7, 
                                SUBSTRING(COPTG.TG003, 1, 4) + '-' + SUBSTRING(COPTG.TG003, 5, 2) + '-' + SUBSTRING(COPTG.TG003, 7, 2) AS A8 
                                FROM COPTG 
                                INNER JOIN COPTH ON COPTG.TG001 = COPTH.TH001 AND COPTG.TG002 = COPTH.TH002 
                                LEFT JOIN COPTD ON COPTD.TD001 = COPTH.TH014 AND COPTD.TD002 = COPTH.TH015 AND COPTD.TD003 = COPTH.TH016 AND COPTD.TD004 = COPTH.TH004 
                                LEFT JOIN COPTC ON COPTC.TC001 = COPTD.TD001 AND COPTC.TC002 = COPTD.TD002 
                                LEFT JOIN COPMA ON COPMA.MA001 = COPTC.TC004 
                                INNER JOIN INVMB ON INVMB.MB001 = COPTH.TH004 
                                WHERE 1=1 
                                AND COPTG.TG003 BETWEEN @StartDate AND @EndDate 
                                AND TG023 = 'Y' 
                                AND (COPTH.TH004 LIKE '1%' OR COPTH.TH004 LIKE '2%')) AS A 
                                LEFT JOIN 
                                (SELECT DISTINCT RTRIM(COPTH.TH001) + '-' + RTRIM(COPTH.TH002) + '-' + RTRIM(COPTH.TH003) AS B1, 
                                RTRIM(CMSMC.MC002) AS B2, RTRIM(CMSME.ME002) AS B3 
                                FROM COPTG 
                                LEFT JOIN COPTH ON COPTG.TG001 = COPTH.TH001 AND COPTG.TG002 = COPTH.TH002 
                                LEFT JOIN COPTD ON COPTD.TD001 = COPTH.TH014 AND COPTD.TD002 = COPTH.TH015 AND COPTD.TD003 = COPTH.TH016 AND COPTD.TD004 = COPTH.TH004 
                                LEFT JOIN MOCTA ON MOCTA.TA026 = COPTD.TD001 AND MOCTA.TA027 = COPTD.TD002 AND MOCTA.TA028 = COPTD.TD003 AND MOCTA.TA006 = COPTD.TD004 
                                LEFT JOIN MOCTG ON MOCTA.TA001 = MOCTG.TG014 AND MOCTA.TA002 = MOCTG.TG015 AND MOCTG.TG004 = COPTD.TD004 
                                LEFT JOIN MOCTF ON MOCTF.TF001 = MOCTG.TG001 AND MOCTF.TF002 = MOCTG.TG002 
                                LEFT JOIN CMSMC ON CMSMC.MC001 = MOCTG.TG010 
                                LEFT JOIN CMSME ON CMSME.ME001 = MOCTF.TF016 
                                WHERE 1=1 
                                AND MOCTA.TA011 = 'Y' 
                                ) AS B ON A1 = B1 
                                ORDER BY A1 
                                ";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlstr, dateTime));

            if (dt != null)
            {
                DgvMain.DataSource = dt;
                DgvOpt.SetColWidth(DgvMain, "销货单号", 150);
                DgvOpt.SetColWidth(DgvMain, "订单单号", 150);
                DgvOpt.SetColWidth(DgvMain, "品名", 200);
                DgvOpt.SetColWidth(DgvMain, "规格", 200);
            }
            else
            {
                Msg.Show("没有查询到数据");
            }
            UI();
        }
    }
}
