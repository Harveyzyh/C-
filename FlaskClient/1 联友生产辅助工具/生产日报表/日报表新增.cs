using System;
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
    public partial class 日报表新增 : Form
    {
        Mssql mssql = new Mssql();
        string Login_UID = FormLogin.Login_Uid;
        string Login_Role = FormLogin.Login_Role;
        string Login_Dpt = FormLogin.Login_Dpt;
        public static string strConnection = Global_Const.strConnection_WGDB;
        bool DtpFlag = false;

        #region Init
        public 日报表新增()
        {
            InitializeComponent();

            FormMainInit();

            Form_MainResized_Work();
        }

        private void FormMainInit()
        {
            DtpFlag = false;//初始化调整时间时不执行相关操作
            DtpReportInputWorkDate.Value = DateTime.Now.AddDays(-1);
            DtpFlag = true;

            ButtonReportInputCommit.Enabled = false;
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
            panel_Tile.Size = new Size(FormWidth, panel_Tile.Height);
            DataGridView_List.Location = new Point(0, panel_Tile.Height + 2);
            DataGridView_List.Size = new Size(FormWidth, FormHeight - panel_Tile.Height - 2);

        }
        #endregion
        
        #region 格式化
        private void TextInputNumber_KeyPress(object sender, KeyPressEventArgs e)//允许输入浮点型
        {
            //如果不是输入数字就不让输入
            if (e.KeyChar != 8 && e.KeyChar != 9 && e.KeyChar != 46 && !Char.IsDigit(e.KeyChar))//回车，Tab，小数点，数字
            {
                e.Handled = true;
            }
        }

        private void TextInputInt_KeyPress(object sender, KeyPressEventArgs e)//只能输入整形
        {
            //如果不是输入数字就不让输入
            if (e.KeyChar != 8 && e.KeyChar != 9 && !Char.IsDigit(e.KeyChar))//回车，Tab，数字
            {
                e.Handled = true;
            }
        }

        private void TextInputDate_KeyUp(object sender, KeyEventArgs e)//日期格式化
        {
            int Lenth;
            TextBox tbox = (TextBox)sender;
            Lenth = tbox.Text.Length;
            if (Lenth == 4 || Lenth == 7)
            {
                tbox.Text += "-";
                tbox.Select(Lenth + 1, 0);
            }
            if (Lenth > 10)
            {
                if (MessageBox.Show("日期超长，请重新输入正确的日期！", "提示", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    tbox.Text = "";
                }
            }
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)//按回车自动跳下一个选框
        {
            if (e.KeyData == Keys.Enter)
            {
                //panelInput2.SelectNextControl(ActiveControl, true, true, false, true);
            }
        }
        #endregion

        #region 回传使用记录
        private void RecordUseLog(string ProgramName, string ModuleName)
        {
            string sqlstr = "";

            sqlstr = " INSERT INTO WG_DB..WG_USELOG (UserID, Date, ProgramName, ModuleName) VALUES('" + Login_UID + "', " + Normal.GetSysTimeStr("Long")
                   + ", '" + ProgramName + "', '" + ModuleName  + "')";

            mssql.SQLexcute(strConnection, sqlstr);
        }
        #endregion

        #region PanelReportPublic
        public static void Calculation(object sender)//总工时运算及错误字符检测
        {
            DataGridView dataGridView = (DataGridView)sender;
            int row = dataGridView.RowCount;
            int Index;
            int Number = 0, Menber = 0, PreNumber = 0;
            float PerHours = 0, StopHours = 0, TotalHours = 0, Capacity = 0;
            for (Index = 0; Index < row; Index++)
            {
                if (dataGridView.Rows[Index].Cells[3].Value.ToString() == "0" && dataGridView.Rows[Index].Cells[4].Value.ToString() == "0")
                {
                    dataGridView.Rows[Index].Cells[5].Value = "0";
                    dataGridView.Rows[Index].Cells[6].Value = "0";
                    dataGridView.Rows[Index].Cells[7].Value = "0";
                    dataGridView.Rows[Index].Cells[8].Value = "0";
                    dataGridView.Rows[Index].Cells[9].Value = "0";
                    dataGridView.Rows[Index].Cells[10].Value = "";
                    dataGridView.Rows[Index].Cells[11].Value = "";
                }
                else
                {
                    try
                    {
                        PreNumber = int.Parse(dataGridView.Rows[Index].Cells[3].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行计划数量输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[3].Value = "0";
                    }
                    try
                    {
                        Number = int.Parse(dataGridView.Rows[Index].Cells[4].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行生产数量输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[4].Value = "0";
                    }
                    try
                    {
                        Menber = int.Parse(dataGridView.Rows[Index].Cells[5].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行人数输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[5].Value = "0";
                    }
                    try
                    {
                        PerHours = float.Parse(dataGridView.Rows[Index].Cells[6].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行工时输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[6].Value = "0";
                    }
                    try
                    {
                        StopHours = float.Parse(dataGridView.Rows[Index].Cells[7].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行停工工时输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[7].Value = "0";
                    }

                    TotalHours = Menber * PerHours - StopHours;
                    if (TotalHours != 0)
                    {
                        Capacity = Number / TotalHours;
                        dataGridView.Rows[Index].Cells[9].Value = Capacity.ToString("f4");
                    }
                    dataGridView.Rows[Index].Cells[8].Value = TotalHours.ToString();

                }
            }
        }

        public static void HeadRowLineNumber(object sender)//表头行数
        {
            DataGridView dataGridView = (DataGridView)sender;
            int rowNumber = 1;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
            dataGridView.AutoResizeRowHeadersWidth(
                DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
        #endregion

        #region PanelReportInput
        private void ReportInputShow(string XL_List)
        {
            if (XL_List != "")
            {
                string sqlstr = " SELECT A.WGroup AS 工作组, A.Serial AS 系列, A.Line AS 生产线别,  "
                              + " '0' AS 计划数量, '0' AS 生产数量, '0' AS 人数, '0' AS 工时, '0' AS 停工工时, '0' AS 总工时, "
                              + " '0' AS 产量每人每小时, '' AS 生产单号, '' AS 备注 "
                              + " FROM WG_DB..SC_LineList AS A"
                              + " LEFT JOIN(SELECT WorkDpt, WGroup, Serial, SUM(CONVERT(INT, WorkNumber)) AS S FROM WG_DB..SC_DAILYRECORD GROUP BY WorkDpt, WGroup, Serial) "
                              + " AS B ON A.WGroup = B.WGroup Collate Chinese_PRC_CS_AS AND A.Serial = B.Serial Collate Chinese_PRC_CS_AS AND A.Dpt = B.WorkDpt Collate Chinese_PRC_CS_AS "
                              + " LEFT JOIN (SELECT WGroup, SUM(CONVERT(INT, WorkNumber)) AS S FROM WG_DB..SC_DAILYRECORD GROUP BY WGroup) AS C ON C.WGroup = B.WGroup Collate Chinese_PRC_CS_AS "
                              + " WHERE 1 = 1 "
                              + " AND A.Dpt = '" + Login_Dpt + "' "
                              + " AND A.Valid = 1 "
                              + " AND A.WGroup IN(" + XL_List + ") "
                              + " ORDER BY A.Serial, A.WGroup, A.Line, C.S DESC, B.S DESC "; 
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

                    //for (int i = 0; i < this.DataGridView_List.Columns.Count; i++)
                    //{
                    //    this.DataGridView_List.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //}
                    //HeadRowLineNumber(DataGridView_List);
                    DataGridView_List.RowHeadersWidth = 30;
                    ButtonReportInputCommit.Enabled = true;
                    DtpFlag = true;
                }
            }
        }

        private void ButtonReportInputCommit_Click(object sender, EventArgs e)//保存
        {
            int row = DataGridView_List.RowCount;
            int Index = 0;
            string sqlstr = "";

            string Creator = Login_UID;
            string Create_Date = mssql.SQLselect(strConnection, "SELECT CONVERT(VARCHAR(20), GETDATE(), 112)").Rows[0][0].ToString();
            string WorkDpt = Login_Dpt;
            string WorkDate = DtpReportInputWorkDate.Value.ToString("yyyy-MM-dd");
            WorkDate = WorkDate.Split('-')[0] + WorkDate.Split('-')[1] + WorkDate.Split('-')[2];
            string Serial, Group, PlanNumber, WorkNumber, Workers, Hours, StopHours, TotalHours, Capacity, OrderID, Remark, Line;
            for (Index = 0; Index < row; Index++)
            {
                if (DataGridView_List.Rows[Index].Cells[3].Value.ToString() == "0" && DataGridView_List.Rows[Index].Cells[4].Value.ToString() == "0")
                {
                    continue;
                }
                else
                {
                    Serial = DataGridView_List.Rows[Index].Cells[0].Value.ToString();
                    Group = DataGridView_List.Rows[Index].Cells[1].Value.ToString();
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

                    sqlstr = "INSERT INTO WG_DB..SC_DAILYRECORD (Creator, Create_Date, WorkDpt, WorkDate, WGroup, Serial, Line, PlanNumber, WorkNumber, Workers, Hours, StopHours, TotalHours, Capacity, OrderID, Remark)"
                           + "VALUES("
                           + "'" + Creator + "', "
                           + "'" + Create_Date + "', "
                           + "'" + WorkDpt + "', "
                           + "'" + WorkDate + "', "
                           + "'" + Serial + "', "
                           + "'" + Group + "', "
                           + "'" + Line + "', "
                           + "'" + PlanNumber + "', "
                           + "'" + WorkNumber + "', "
                           + "'" + Workers + "', "
                           + "'" + Hours + "', "
                           + "'" + StopHours + "', "
                           + "'" + TotalHours + "', "
                           + "'" + Capacity + "', "
                           + "'" + OrderID + "', "
                           + "'" + Remark + "' "
                           + ")";
                     mssql.SQLexcute(strConnection, sqlstr);
                }
            }
            MessageBox.Show("保存已完成！", "提示", MessageBoxButtons.OK);
            ButtonReportInputCommit.Enabled = false;
        }

        private void ButtonReportInputXLSelect_Click(object sender, EventArgs e)//选择系列
        {
            string sqlstr = "SELECT DISTINCT WGroup AS 组别 FROM WG_DB..SC_XL2GY";

            Form formxl = new 日报表获取组别系列(sqlstr);
            formxl.ShowDialog();
            if (日报表获取组别系列.XL_ChangeFlag)
            {
                ReportInputShow(日报表获取组别系列.XL_List);
            }
            formxl.Dispose();
            
        }

        private void DataGridViewReportInput_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Calculation(sender);
        }

        private void DtpReportInputWorkDate_ValueChanged(object sender, EventArgs e)//判断日期是否有修改
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