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
using DataGridViewAutoFilter;

namespace 联友生产辅助工具.生产日报表
{
    public partial class 日报表查询: Form
    {
        #region 局部变量
        Mssql mssql = new Mssql();
        DataGridViewFunction Get = new DataGridViewFunction();
        string Login_UID = FormLogin.Login_Uid;
        string Login_Role = FormLogin.Login_Role;
        string Login_Dpt = FormLogin.Login_Dpt;
        public static string strConnection = Global_Const.strConnection_WG_DB;
        bool DtpFlag = false;
        #endregion

        #region Init
        public 日报表查询()
        {
            InitializeComponent();

            FormMainInit();

            FormMain_Resized_Work();
        }

        private void FormMainInit()
        {
            ButtonReportSelectLayout.Enabled = false;

            //部门判断、权限设置
            if (Login_Dpt.Substring(0, 2) == "生产")
            {
                ComboBoxReportDptType.Text = Login_Dpt;
                ComboBoxReportSelectType.Text = "全部";
            }
            else
            {
                string sqlstr = "SELECT ROLE FROM WG_DB..WG_USER WHERE U_ID = '" + Login_UID +"'";
                DataTable dttmp = mssql.SQLselect(strConnection, sqlstr);
                if (dttmp != null)
                {
                    string role = dttmp.Rows[0][0].ToString();
                    Login_Role = role;
                    if (role == "Super")
                    {
                        ComboBoxReportDptType.Text = "全部";
                        ComboBoxReportSelectType.Text = "全部";
                    }
                    else if (role == "生管")
                    {

                        ComboBoxReportDptType.Text = "全部";
                        ComboBoxReportSelectType.Text = "全部";
                    }
                    else
                    {
                        ComboBoxReportDptType.Text = "全部";
                        ComboBoxReportSelectType.Text = "全部";
                    }
                }
                else
                {
                    ComboBoxReportDptType.Text = "全部";
                    ComboBoxReportSelectType.Text = "全部";
                }
            }

            DtpFlag = false;//初始化调整时间时不执行相关操作
            
            DtpReportSelectStartDate.Value = DateTime.Now.AddDays(-1);
            DtpReportSelectEndDate.Value = DateTime.Now;

            DtpFlag = true;
        }
        #endregion

        #region 窗口大小变化设置
        private void FormMain_Resized(object sender, EventArgs e)
        {
            FormMain_Resized_Work();
        }

        public void FormMain_Resized_Work()
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

            sqlstr = " INSERT INTO WG_DB..WG_USELOG (UserID, Date, ProgramName, ModuleName) VALUES('" + Login_UID + "', " + Global_Const.sqldatestrlong 
                   + ", '" + ProgramName + "', '" + ModuleName  + "')";

            mssql.SQLexcute(strConnection, sqlstr);
        }
        #endregion

        #region PanelReportSelect
        private void ButtonSelectSubmit_Click(object sender, EventArgs e)
        {
            RecordUseLog("联友生产辅助工具", "生产日报表-查询");

            bool NullFlag = false;

            string sqlstr = "", sql_date = "", sql_dpt = "", WGroup_List = "";

            //日期范围
            if (DtpReportSelectStartDate.Checked)
            {
                string ReportSelectStartDate = DtpReportSelectStartDate.Value.ToString("yyyyMMdd");
                sql_date += " AND WorkDate >= '" + ReportSelectStartDate + "' ";
            }
            if (DtpReportSelectEndDate.Checked)
            {
                string ReportSelectEndDate = DtpReportSelectEndDate.Value.ToString("yyyyMMdd");
                sql_date += " AND WorkDate <= '" + ReportSelectEndDate + "' ";
            }

            //选中部门
            if ((ComboBoxReportDptType.Text != "全部"))
            {
                sql_dpt += " AND WorkDpt = '" + ComboBoxReportDptType.Text + "' ";
            }

            //选择组别
            if(日报表获取组别系列.XL_List != "")
            {
                WGroup_List += "AND WGroup IN (" + 日报表获取组别系列.XL_List + ")";
            }

            //汇总方式
            if (ComboBoxReportSelectType.Text == "全部")
            {
                sqlstr = " SELECT WorkDpt AS 生产部门, SUBSTRING(WorkDate,1,4) + '-' + SUBSTRING(WorkDate,5,2) + '-' + SUBSTRING(WorkDate,7,2) AS 生产日期, "
                   + " WGroup AS 组别, Serial AS 系列, Line AS 线别, PlanNumber AS 计划数量, "
                   + " WorkNumber AS 生产数量, Workers AS 人数, Hours AS 工时, StopHours AS 停工工时, TotalHours AS 总工时, Capacity AS 产量每人每小时, "
                   + " OrderID AS 生产单号, Remark AS 备注 FROM SC_DAILYRECORD "
                   + " WHERE 1 = 1 "
                   + " AND (PlanNumber <> '0' AND WorkNumber <> '0') ";
                sqlstr += sql_date + sql_dpt;
                sqlstr += WGroup_List;
            }
            else if(ComboBoxReportSelectType.Text == "组别-系列")
            {
                string sqlstr2 = "";
                sqlstr = "";
                sqlstr2 = " SELECT DISTINCT WGroup FROM WG_DB..SC_DAILYRECORD ";
                sqlstr2 += " WHERE 1=1 ";
                sqlstr2 += sql_date + sql_dpt;
                sqlstr2 += WGroup_List;

                DataTable dttmp2 = mssql.SQLselect(strConnection, sqlstr2);
                if(dttmp2 != null)
                {
                    NullFlag = false;
                    int Count = dttmp2.Rows.Count;
                    int Index;
                    for(Index = 0; Index < Count; Index++)
                    {
                        string WGroup = dttmp2.Rows[Index][0].ToString();
                        if(Index > 0)
                        {
                            sqlstr += " UNION ";
                        }
                        sqlstr += " SELECT WGroup AS 组别, Serial AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                        sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, Hours))) AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                        sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                        sqlstr += " FROM WG_DB..SC_DAILYRECORD ";
                        sqlstr += " WHERE 1 = 1 ";
                        sqlstr += sql_date + sql_dpt;
                        sqlstr += " AND WGroup = '" + WGroup + "'";
                        sqlstr += " GROUP BY WGroup, Serial ";

                        sqlstr += " UNION ";
                        sqlstr += " SELECT WGroup AS 组别, '小计' AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                        sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, '' AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                        sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                        sqlstr += " FROM WG_DB..SC_DAILYRECORD ";
                        sqlstr += " WHERE 1 = 1 ";
                        sqlstr += sql_date + sql_dpt;
                        sqlstr += " AND WGroup = '" + WGroup + "'";
                        sqlstr += " GROUP BY WGroup";
                    }

                    sqlstr += " UNION ";

                    sqlstr += " SELECT '' AS 组别, '总计' AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                    sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, '' AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                    sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                    sqlstr += " FROM WG_DB..SC_DAILYRECORD ";
                    sqlstr += " WHERE 1 = 1 ";
                    sqlstr += sql_date + sql_dpt;
                    sqlstr += WGroup_List;

                    sqlstr += " ORDER BY 组别 DESC ";
                }
                else
                {
                    NullFlag = true;
                }
            }
            else if(ComboBoxReportSelectType.Text == "部门-组别-系列")
            {
                string sqlstr3 = "";
                string sqlstr2 = "";
                sqlstr = "";

                sqlstr2 = " SELECT DISTINCT WGroup FROM WG_DB..SC_DAILYRECORD ";
                sqlstr2 += " WHERE 1=1 ";
                sqlstr2 += sql_date + sql_dpt;
                sqlstr2 += WGroup_List;

                sqlstr3 = " SELECT DISTINCT WorkDpt FROM WG_DB..SC_DAILYRECORD ";
                sqlstr3 += " WHERE 1=1 ";
                sqlstr3 += sql_date + sql_dpt;

                DataTable dttmp2 = mssql.SQLselect(strConnection, sqlstr2);
                DataTable dttmp3 = mssql.SQLselect(strConnection, sqlstr3);

                if (dttmp3 != null)
                {
                    NullFlag = false;
                    int Count = dttmp2.Rows.Count;
                    int Index;
                    for (Index = 0; Index < Count; Index++)
                    {
                        string WGroup = dttmp2.Rows[Index][0].ToString();
                        if (Index > 0)
                        {
                            sqlstr += " UNION ";
                        }

                        sqlstr += " SELECT WorkDpt AS 生产部门, WGroup AS 组别, Serial AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                        sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, Hours))) AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                        sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                        sqlstr += " FROM WG_DB..SC_DAILYRECORD ";
                        sqlstr += " WHERE 1 = 1 ";
                        sqlstr += sql_date + sql_dpt;
                        sqlstr += " AND WGroup = '" + WGroup + "'";
                        sqlstr += " GROUP BY WorkDpt, WGroup, Serial ";

                        sqlstr += " UNION ";
                        sqlstr += " SELECT WorkDpt AS 生产部门, WGroup AS 组别, '小计' AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                        sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, '' AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                        sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                        sqlstr += " FROM WG_DB..SC_DAILYRECORD ";
                        sqlstr += " WHERE 1 = 1 ";
                        sqlstr += sql_date + sql_dpt;
                        sqlstr += " AND WGroup = '" + WGroup + "'";
                        sqlstr += " GROUP BY WorkDpt, WGroup";
                    }

                    sqlstr += " UNION ";

                    sqlstr += " SELECT '' AS 生产部门, '' AS 组别, '总计' AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                    sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, '' AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                    sqlstr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                    sqlstr += " FROM WG_DB..SC_DAILYRECORD ";
                    sqlstr += " WHERE 1 = 1 ";
                    sqlstr += sql_date + sql_dpt;
                    sqlstr += WGroup_List;

                    sqlstr += " ORDER BY 组别 DESC ";
                }
                else
                {
                    NullFlag = true;
                }
            }

            if (!NullFlag)
            {
                DataTable dttmp = mssql.SQLselect(strConnection, sqlstr);
                DataGridView_List.DataSource = null;
                if(dttmp != null)
                {
                    //DataGridView_List.DataSource = dttmp;

                    Get.GridViewDataLoad(dttmp, DataGridView_List);
                    Get.GridViewHeaderFilter(DataGridView_List);

                    //DataGridView_List.ReadOnly = true;
                    
                    if (ComboBoxReportSelectType.Text == "全部")
                    {

                        DataGridView_List.Columns[3].Width = 170;
                        DataGridView_List.Columns[4].Width = 50;
                        DataGridView_List.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        DataGridView_List.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        DataGridView_List.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        DataGridView_List.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        DataGridView_List.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        DataGridView_List.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                        DataGridView_List.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人均产能
                        DataGridView_List.Columns[12].Width = 280;//生产单号
                        DataGridView_List.Columns[13].Width = 400;//备注
                    }
                    else if (ComboBoxReportSelectType.Text == "组别-系列")
                    {
                        DataGridView_List.Columns[1].Width = 170;
                        DataGridView_List.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        DataGridView_List.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        DataGridView_List.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        DataGridView_List.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        DataGridView_List.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        DataGridView_List.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                    }
                    else if (ComboBoxReportSelectType.Text == "部门-组别-系列")
                    {
                        DataGridView_List.Columns[2].Width = 170;
                        DataGridView_List.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        DataGridView_List.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        DataGridView_List.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        DataGridView_List.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        DataGridView_List.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        DataGridView_List.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时

                    }

                    //行颜色
                    DataGridView_List.RowsDefaultCellStyle.BackColor = Color.Bisque;
                    DataGridView_List.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
                    //首列行数
                    for (int i = 0; i < this.DataGridView_List.Columns.Count; i++)
                    {
                        this.DataGridView_List.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    日报表新增.HeadRowLineNumber(DataGridView_List);
                    DataGridView_List.RowHeadersWidth = 60;
                    ButtonReportSelectLayout.Enabled = true;

                }
                else
                {
                    DataGridView_List.DataSource = null;
                    ButtonReportSelectLayout.Enabled = false;
                    MessageBox.Show("没有查询到数据！", "提示", MessageBoxButtons.OK);
                }
            }
            else
            {
                DataGridView_List.DataSource = null;
                ButtonReportSelectLayout.Enabled = false;
                MessageBox.Show("没有查询到数据！", "提示", MessageBoxButtons.OK);
            }
        }

        private void ButtonReportSelectLeyout_Click(object sender, EventArgs e)
        {
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
                

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Excel 2007|*.xlsx";
            saveFileDialog.FileName = "生产日报表导出_" + DateTime.Now.ToString("yyyy-MM-dd");
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dttmp = (DataTable)DataGridView_List.DataSource;

                    excelObj.FilePath = Path.GetDirectoryName(saveFileDialog.FileName);
                    excelObj.FileName = Path.GetFileName(saveFileDialog.FileName);
                    excelObj.IsWrite = true;
                    excelObj.CellDt = dttmp;

                    Excel excel = new Excel();
                    excel.ExcelOpt(excelObj);
                    MessageBox.Show("Excel导出成功！", "提示");
                }
                catch (IOException)
                {
                    MessageBox.Show("文件保存失败,请确保改文件没被打开！", "错误");
                }
            }
        }

        private void ButtonReportSelectWG_Click(object sender, EventArgs e)
        {
            string sqlstr = "SELECT DISTINCT WGroup AS 组别 FROM WG_DB..SC_XL2GY";

            Form formxl = new 日报表获取组别系列(sqlstr);
            formxl.ShowDialog();
            //if (FormMain_WG.XL_ChangeFlag)
            //{
            //    ReportInputShow(FormMain_WG.XL_List);
            //}
            formxl.Dispose();
        }
        #endregion
    }
}