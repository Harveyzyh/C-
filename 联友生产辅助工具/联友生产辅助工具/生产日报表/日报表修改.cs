﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HarveyZ;

namespace 联友生产辅助工具.生产日报表
{
    public partial class 日报表修改 : Form
    {
        Mssql mssql = new Mssql();
        string Login_UID = FormLogin.Login_Uid;
        string Login_Role = FormLogin.Login_Role;
        string Login_Dpt = FormLogin.Login_Dpt;
        public static string strConnection = 日报表新增.strConnection;
        bool DtpFlag = false;

        #region Init
        public 日报表修改()
        {
            InitializeComponent();

            FormMainInit();

            Form_MainResized_Work();
        }

        private void FormMainInit()
        {
            DtpFlag = false;//初始化调整时间时不执行相关操作
            DtpReportUpdateWorkDate.Value = DateTime.Now.AddDays(-1);
            DtpFlag = true;

            ButtonReportUpdateCommit.Enabled = false;
        }
        #endregion

        #region 窗口大小变化设置
        private void Form_MainResized(object sender, EventArgs e)
        {
            Form_MainResized_Work();
        }

        private void Form_MainResized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            DataGridView_List.Location = new Point(0, panel_Title.Height + 2);
            DataGridView_List.Size = new Size(FormWidth, FormHeight - panel_Title.Height - 2);
        }
        #endregion

        #region 回传使用记录
        private void RecordUseLog(string ProgramName, string ModuleName)
        {
            string sqlstr = "";

            sqlstr = " INSERT INTO WG_DB..WG_USELOG (UserID, Date, ProgramName, ModuleName) VALUES('" + Login_UID + "', " + Global_Const.sqldatestrlong 
                   + ", '" + ProgramName + "', '" + ModuleName  + "')";

            mssql.SQLexcute(strConnection, sqlstr);
        }
        #endregion

        #region PanelReportUpdate
        private void ReportUpdateShow(string XL_List)
        {
            string WorkDate = DtpReportUpdateWorkDate.Value.ToString("yyyy-MM-dd");
            WorkDate = WorkDate.Split('-')[0] + WorkDate.Split('-')[1] + WorkDate.Split('-')[2];
            if(XL_List != "")
            {
                string sqlstr = " SELECT B.WGroup AS 工作组, B.Serial AS 系列, Line AS 生产线别, B.PlanNumber AS 计划数量, B.WorkNumber AS 生产数量, B.Workers AS 人数, B.Hours AS 工时, B.StopHours AS 停工工时, B.TotalHours AS 总工时, "
                              + " B.Capacity AS 产量每人每小时, B.OrderID AS 生产单号, B.Remark AS 备注 "
                              + " FROM WG_DB..SC_DAILYRECORD AS B "
                              + " LEFT JOIN(SELECT WGroup, Serial, SUM(CONVERT(INT, WorkNumber)) AS S FROM WG_DB..SC_DAILYRECORD GROUP BY WGroup, Serial) AS A ON A.WGroup = B.WGroup AND A.Serial = B.Serial "
                              + " LEFT JOIN (SELECT WGroup, SUM(CONVERT(INT, WorkNumber)) AS S FROM WG_DB..SC_DAILYRECORD GROUP BY WGroup) AS C ON C.WGroup = B.WGroup "
                              + " WHERE B.WGroup IN("
                              + XL_List + ")";

                if (Login_Role != "Super")
                {
                    sqlstr += " AND B.WorkDpt =  '" + Login_Dpt + "' ";
                }

                sqlstr += " AND B.WorkDate = '" + WorkDate + "' ";

                sqlstr += " ORDER BY C.S DESC, A.S DESC ";
                DataTable dttmp = mssql.SQLselect(strConnection, sqlstr);
                DataGridView_List.DataSource = dttmp;
                if (dttmp != null)
                {
                    DataGridView_List.Columns[0].ReadOnly = true;//组别
                    DataGridView_List.Columns[1].ReadOnly = true;//系列
                    DataGridView_List.Columns[1].Width = 170;
                    DataGridView_List.Columns[2].ReadOnly = true;//线别
                    DataGridView_List.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DataGridView_List.Columns[2].Width = 50;
                    DataGridView_List.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                    DataGridView_List.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                    DataGridView_List.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                    DataGridView_List.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                    DataGridView_List.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                    DataGridView_List.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                    DataGridView_List.Columns[8].ReadOnly = true;//总工时
                    DataGridView_List.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人均产能
                    DataGridView_List.Columns[9].ReadOnly = true;//人均产能
                    DataGridView_List.Columns[9].Width = 160;
                    DataGridView_List.Columns[10].Width = 300;//生产单号
                    DataGridView_List.Columns[11].Width = 500;//备注


                    DataGridView_List.RowsDefaultCellStyle.BackColor = Color.Bisque;
                    DataGridView_List.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;

                    for (int i = 0; i < this.DataGridView_List.Columns.Count; i++)
                    {
                        this.DataGridView_List.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    日报表新增.HeadRowLineNumber(DataGridView_List);
                    DataGridView_List.RowHeadersWidth = 60;
                    DtpFlag = true;
                    ButtonReportUpdateCommit.Enabled = true;
                }
            }
        }

        private void ButtonReportUpdateCommit_Click(object sender, EventArgs e)
        {
            RecordUseLog("联友生产辅助工具", "生产日报表-修改");

            string sqlstr = "";
            int Index = 0;
            int row = DataGridView_List.RowCount;
            string WorkDate = DtpReportUpdateWorkDate.Value.ToString("yyyy-MM-dd");
            WorkDate = WorkDate.Split('-')[0] + WorkDate.Split('-')[1] + WorkDate.Split('-')[2];
            string WorkDpt = Login_Dpt;
            string ModiFier = Login_UID;
            string Modi_Date = mssql.SQLselect(strConnection, "SELECT CONVERT(VARCHAR(20), GETDATE(), 112)").Rows[0][0].ToString();
            string Serial, WGroup, Line, PlanNumber, WorkNumber, Workers, Hours, StopHours, TotalHours, Capacity, OrderID, Remark;
            for (Index = 0; Index < row; Index++)
            {
                WGroup = DataGridView_List.Rows[Index].Cells[0].Value.ToString();
                Serial = DataGridView_List.Rows[Index].Cells[1].Value.ToString();
                Line = DataGridView_List.Rows[Index].Cells[2].Value.ToString();
                PlanNumber = DataGridView_List.Rows[Index].Cells[3].Value.ToString();
                WorkNumber = DataGridView_List.Rows[Index].Cells[4].Value.ToString();
                Workers = DataGridView_List.Rows[Index].Cells[5].Value.ToString();
                Hours = DataGridView_List.Rows[Index].Cells[6].Value.ToString();
                StopHours = DataGridView_List.Rows[Index].Cells[7].Value.ToString();
                TotalHours = DataGridView_List.Rows[Index].Cells[8].Value.ToString();
                Capacity = DataGridView_List.Rows[Index].Cells[9].Value.ToString();
                OrderID = DataGridView_List.Rows[Index].Cells[10].Value.ToString();
                Remark = DataGridView_List.Rows[Index].Cells[11].Value.ToString();

                sqlstr = "SELECT WGroup, Serial FROM WG_DB..SC_DAILYRECORD "
                       + "WHERE 1=1 "
                       + "AND WorkDate = '" + WorkDate + "' "
                       + "AND Serial = '" + Serial + "' "
                       + "AND WGroup = '" + WGroup + "' "
                       + "AND Line = '" + Line + "' "
                       + "AND PlanNumber = '" + PlanNumber + "' "
                       + "AND WorkNumber = '" + WorkNumber + "' "
                       + "AND Workers = '" + Workers + "' "
                       + "AND Hours = '" + Hours + "' "
                       + "AND StopHours = '" + StopHours + "' "
                       + "AND TotalHours = '" + TotalHours + "' "
                       + "AND Capacity = '" + Capacity + "' "
                       + "AND OrderID = '" + OrderID + "' "
                       + "AND Remark = '" + Remark + "' "
                       + "";
                if(Login_Role != "Super")
                {

                    sqlstr += "AND WorkDpt = '" + WorkDpt + "' ";
                }

                DataTable dttmp = mssql.SQLselect(strConnection, sqlstr);

                if (dttmp != null)
                {
                    continue;
                }
                else
                {
                    sqlstr = "UPDATE WG_DB..SC_DAILYRECORD SET "
                           + "ModiFier = '" + ModiFier + "', "
                           + "Modi_Date = '" + Modi_Date + "', "
                           + "PlanNumber = '" + PlanNumber + "', "
                           + "WorkNumber = '" + WorkNumber + "', "
                           + "Workers = '" + Workers + "', "
                           + "Hours = '" + Hours + "', "
                           + "StopHours = '" + StopHours + "', "
                           + "TotalHours = '" + TotalHours + "', "
                           + "Capacity = '" + Capacity + "', "
                           + "OrderID = '" + OrderID + "', "
                           + "Remark = '" + Remark + "' "
                           + "WHERE 1=1 "
                           + "AND WorkDate = '" + WorkDate + "' "
                           + "AND Serial = '" + Serial + "' "
                           + "AND WGroup = '" + WGroup + "' "
                           + "AND Line = '" + Line + "' ";
                    if (Login_Role != "Super")
                    {
                        sqlstr += "AND WorkDpt = '" + WorkDpt + "' ";
                    }
                    mssql.SQLexcute(strConnection, sqlstr);
                }
            }
            MessageBox.Show("保存已完成！", "提示", MessageBoxButtons.OK);
            ButtonReportUpdateCommit.Enabled = false;
        }

        private void ButtonReportUpdateXLSelect_Click(object sender, EventArgs e)
        {
            string sqlstr = "SELECT DISTINCT WGroup AS 组别 FROM WG_DB..SC_XL2GY";

            Form formxl = new 日报表获取组别系列(sqlstr);
            formxl.ShowDialog();
            if (日报表获取组别系列.XL_ChangeFlag)
            {
                ReportUpdateShow(日报表获取组别系列.XL_List);
            }
            formxl.Dispose();
            DtpFlag = true;
        }

        private void DataGridViewReportUpdate_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            日报表新增.Calculation(sender);
        }

        private void DtpReportUpdateWorkDate_ValueChanged(object sender, EventArgs e)//判断日期是否有修改
        {
            if (DtpFlag)
            {
                DataGridView_List.DataSource = null;
                DtpFlag = false;
            }
        }
        #endregion
    }
}