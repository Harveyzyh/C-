using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ;

namespace HarveyZ.生产日报表
{
    public partial class 日报表修改 : Form
    {
        Mssql mssql = new Mssql();
        public static string strConnection = FormLogin.infObj.connWG;
        bool DtpFlag = false;

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        #region Init
        public 日报表修改(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);

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

        #region PanelReportUpdate
        private void ReportUpdateShow(string XL_List)
        {
            string WorkDate = DtpReportUpdateWorkDate.Value.ToString("yyyy-MM-dd");
            WorkDate = WorkDate.Split('-')[0] + WorkDate.Split('-')[1] + WorkDate.Split('-')[2];
            if(XL_List != "")
            {
                string slqStr = " SELECT B.WGroup AS 工作组, B.Serial AS 系列, Line AS 生产线别, B.PlanNumber AS 计划数量, B.WorkNumber AS 生产数量, B.Workers AS 人数, B.Hours AS 工时, B.StopHours AS 停工工时, B.TotalHours AS 总工时, "
                              + " B.Capacity AS 产量每人每小时, B.OrderID AS 生产单号, B.Remark AS 备注 "
                              + " FROM dbo.SC_DRY_DAILYRECORD AS B "
                              + " LEFT JOIN(SELECT WGroup, Serial, SUM(CONVERT(INT, WorkNumber)) AS S FROM dbo.SC_DRY_DAILYRECORD GROUP BY WGroup, Serial) AS A ON A.WGroup = B.WGroup AND A.Serial = B.Serial "
                              + " LEFT JOIN (SELECT WGroup, SUM(CONVERT(INT, WorkNumber)) AS S FROM dbo.SC_DRY_DAILYRECORD GROUP BY WGroup) AS C ON C.WGroup = B.WGroup "
                              + " WHERE B.WGroup IN("
                              + XL_List + ")";

                if (FormLogin.infObj.userDpt.Substring(0, 2) == "生产")
                {
                    slqStr += " AND B.WorkDpt =  '" + FormLogin.infObj.userDpt + "' ";
                }

                slqStr += " AND B.WorkDate = '" + WorkDate + "' ";

                slqStr += " ORDER BY B.Serial, B.WGroup, C.S DESC, A.S DESC ";
                DataTable dttmp = mssql.SQLselect(strConnection, slqStr);
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


                    DgvOpt.SetRowBackColor(DataGridView_List);
                    DataGridView_List.RowHeadersWidth = 30;
                    DtpFlag = true;
                    ButtonReportUpdateCommit.Enabled = true;
                }
            }
        }

        private void ButtonReportUpdateCommit_Click(object sender, EventArgs e)
        {
            string slqStr = "";
            int Index = 0;
            int row = DataGridView_List.RowCount;
            string WorkDate = DtpReportUpdateWorkDate.Value.ToString("yyyy-MM-dd");
            WorkDate = WorkDate.Split('-')[0] + WorkDate.Split('-')[1] + WorkDate.Split('-')[2];
            string WorkDpt = FormLogin.infObj.userDpt;
            string ModiFier = FormLogin.infObj.userId;
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

                slqStr = "SELECT WGroup, Serial FROM dbo.SC_DRY_DAILYRECORD "
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
                if(FormLogin.infObj.userDpt.Substring(0, 2) == "生产")
                {

                    slqStr += "AND WorkDpt = '" + WorkDpt + "' ";
                }

                DataTable dttmp = mssql.SQLselect(strConnection, slqStr);

                if (dttmp != null)
                {
                    continue;
                }
                else
                {
                    slqStr = "UPDATE dbo.SC_DRY_DAILYRECORD SET "
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
                    if (FormLogin.infObj.userDpt.Substring(0, 2) == "生产")
                    {
                        slqStr += "AND WorkDpt = '" + WorkDpt + "' ";
                    }
                    mssql.SQLexcute(strConnection, slqStr);
                }
            }
            MessageBox.Show("保存已完成！", "提示", MessageBoxButtons.OK);
            ButtonReportUpdateCommit.Enabled = false;
        }

        private void ButtonReportUpdateXLSelect_Click(object sender, EventArgs e)
        {
            string slqStr = "SELECT DISTINCT WGroup AS 组别 FROM dbo.SC_DRY_XL2GY";

            Form formxl = new 日报表获取组别系列(slqStr);
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