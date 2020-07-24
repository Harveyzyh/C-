using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ;
using System.Collections.Generic;
using System.Timers;

namespace 联友生产辅助工具.生管码垛线
{
    public partial class 码垛线报表 : Form
    {
        #region 静态数据设置
        private delegate string NewTaskDelegate(DataTable dttmp); //任务代理
        
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;

        private string connWG = FormLogin.infObj.connWG;
        private string connERP = FormLogin.infObj.connYF;
        private string connMD = FormLogin.infObj.connMD;
        #endregion

        #region 窗体设计
        public 码垛线报表(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            DtpEndDate.Checked = true;
            DgvShow();
            DgvOpt.SetRowBackColor(DgvMain);
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
            //int FormWidth, FormHeight;
            //FormWidth = Width;
            //FormHeight = Height;
            //PanelTitle.Size = new Size(FormWidth, PanelTitle.Height);
            //DgvMain.Location = new Point(0, PanelTitle.Height + 2);
            //DgvMain.Size = new Size(FormWidth, FormHeight - PanelTitle.Height - 2);
        }
        #endregion

        #region 界面
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
            excelObj.defauleFileName = "码垛线报表_" + DateTime.Now.ToString("yyyy-MM-dd");
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

        private void UI()
        {
            if (DgvMain.DataSource != null)
            {
                BtnOutput.Enabled = true;
            }
            else
            {
                BtnOutput.Enabled = false;
            }
        }
        #endregion

        #endregion

        #region 业务逻辑
        private void DgvShow() //数据库资料显示到界面
        {
            string sqlstrShow = @" SELECT SC.SC003 上线日期, SC.SC001 生产单号, 
	                                    CAST(SC.SC013 AS FLOAT) 上线数量, ISNULL(PD2.SL, 0) AS 上线日过机数量, ISNULL(PD1.SL, 0) AS 总过机数量, SC033 是否完成, 
	                                    SC010 品名, SC011 保友品名, SC012 规格, SC015 配置方案, SC016 配置描述, SC017 描述备注
                                    FROM dbo.SCHEDULE AS SC 
                                    LEFT JOIN (
	                                    SELECT SC001, COUNT(*) AS SL FROM dbo.PdData GROUP BY SC001 
                                    ) AS PD1 ON PD1.SC001 = SC.SC001 
                                    LEFT JOIN (
	                                    SELECT SC001, CONVERT(VARCHAR(8), Pd_date, 112) AS DT, COUNT(*) AS SL FROM  dbo.PdData GROUP BY SC001, CONVERT(VARCHAR(8), Pd_date, 112)
                                    ) AS PD2 ON PD2.SC001 = SC.SC001 AND PD2.DT = SC.SC003 
                                    WHERE 1=1 ";
            if (TxBoxOrder.Text != "") sqlstrShow += @" AND SC.SC001 LIKE '%" + TxBoxOrder.Text + "%' ";
            if (TxBoxName.Text != "") sqlstrShow += @" AND SC.SC010 LIKE '%" + TxBoxName.Text + "%' ";
            if (DtpStartDate.Checked) sqlstrShow += @" AND SC.SC003 >= '" + DtpStartDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpEndDate.Checked) sqlstrShow += @" AND SC.SC003 <= '" + DtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            sqlstrShow += " ORDER BY SC003, SC.SC001 ";
            DataTable showDt = mssql.SQLselect(connMD, sqlstrShow);

            if (showDt != null)
            {
                DtOpt.DtDateFormat(showDt, "上线日期");
                DgvMain.DataSource = showDt;
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColMiddleCenter(DgvMain, "上线数量");
                DgvOpt.SetColMiddleCenter(DgvMain, "上线日过机数量");
                DgvOpt.SetColMiddleCenter(DgvMain, "总过机数量");
                DgvOpt.SetColMiddleCenter(DgvMain, "是否完成");
                DgvOpt.SetColWidth(DgvMain, "生产单号", 150);
                DgvOpt.SetColWidth(DgvMain, "上线数量", 80);
                DgvOpt.SetColWidth(DgvMain, "是否完成", 80);
                DgvOpt.SetColWidth(DgvMain, "上线日过机数量", 120);
                DgvOpt.SetColWidth(DgvMain, "总过机数量", 100);
                DgvOpt.SetColWidth(DgvMain, "品名", 250);
            }
            else
            {
                DgvMain.DataSource = null;
            }
        }
        #endregion
    }
}
