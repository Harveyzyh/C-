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
    public partial class 未排订单查询 : Form
    {
        #region 静态数据设置
        
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
        #endregion

        #region 逻辑
        private void DgvShow() //数据库资料显示到界面
        {
            string sqlstrShow = @" SELECT RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) AS 订单号, RTRIM(MA002) AS 客户名称, RTRIM(TD004) AS 品号, RTRIM(TD005) AS 品名, RTRIM(TD006) AS 规格, 
                                    CAST(TD008 AS FLOAT) AS 订单数量, RTRIM(TD053) AS 配置方案, COPTD.UDF12 AS 订单更新时间
                                    FROM COMFORT.dbo.COPTD 
                                    INNER JOIN COMFORT.dbo.COPTC ON TC001 = TD001 AND TC002 = TD002 AND TC027 = 'Y' AND TD016 NOT IN ('Y', 'y') AND TD008-TD009 > 0 
                                    INNER JOIN COMFORT.dbo.INVMB ON TD004 = MB001 AND MB025 IN ('M')
                                    INNER JOIN COMFORT.dbo.COPMA ON MA001 = TC004 
                                    LEFT JOIN WG_DB.dbo.SC_PLAN ON RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) = SC001 
                                    WHERE 1=1
                                    AND SC001 IS NULL 
                                    AND LEFT(COPTD.UDF12, 8) >= '20200301'
                                    AND TC001 NOT IN ('2217')";
            if (TxBoxOrder.Text != "") sqlstrShow += @" AND RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) LIKE '%" + TxBoxOrder.Text + "%' ";
            if (TxBoxName.Text != "") sqlstrShow += @" AND RTRIM(TD005) LIKE '%" + TxBoxName.Text + "%' ";
            if (DtpStartDate.Checked) sqlstrShow += @" AND LEFT(COPTD.UDF12, 8) >= '" + DtpStartDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpEndDate.Checked) sqlstrShow += @" AND LEFT(COPTD.UDF12, 8) <= '" + DtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            sqlstrShow += " ORDER BY COPTD.UDF12, RTRIM(TD001) + '-' + RTRIM(TD002) + '-' + RTRIM(TD003) ";
            DataTable showDt = mssql.SQLselect(connWG, sqlstrShow);

            if (showDt != null)
            {

                DtOpt.DtDateFormat(showDt, "时间");
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColMiddleCenter(DgvMain, "数量");
                DgvOpt.SetColWidth(DgvMain, "订单号", 180);
                DgvOpt.SetColWidth(DgvMain, "品名", 180);
                DgvOpt.SetColWidth(DgvMain, "订单更新时间", 180);

                ////计算合计，因为排序不一样，会出现订单数量的重复，需要重新排序去重
                //float sxSum = 0;
                float ddSum = 0;
                //DataTable sumDt = showDt.Copy();
                //DataView sumDv = sumDt.DefaultView;
                //sumDv.Sort = "订单号 ASC";
                //sumDt = sumDv.ToTable();
                //string ddTmp = "";
                //for (int rowIndex = 0; rowIndex < sumDt.Rows.Count; rowIndex++)
                //{
                //    try
                //    {
                //        sxSum += float.Parse(sumDt.Rows[rowIndex]["上线数量"].ToString());
                //        if (ddTmp != sumDt.Rows[rowIndex]["订单号"].ToString())
                //        {
                //            ddTmp = sumDt.Rows[rowIndex]["订单号"].ToString();
                //            ddSum += float.Parse(sumDt.Rows[rowIndex]["订单数量"].ToString());
                //        }
                //    }
                //    catch
                //    {
                //        continue;
                //    }
                //}
                labelDdSlSum.Text = "订单总数量：" + ddSum.ToString();
                
            }
            else
            {
                labelDdSlSum.Text = "订单总数量：0";
                DgvMain.DataSource = null;
            }
        }
        #endregion
    }
}
