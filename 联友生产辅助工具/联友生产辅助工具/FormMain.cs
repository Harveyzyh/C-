using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace 联友生产辅助工具
{
    public partial class FormMain : Form
    {
        public static string strConnection = Global_Const.strConnection_WG_DB;
        bool DtpFlag = false;

        #region Init
        public FormMain()
        {
            InitializeComponent();

            FormMainInit();

            Form_MainResized_Work();
        }

        private void FormMainInit()
        {
            LabelUserInfo.Text = "部门：" + Global_Var.Login_Dpt + "    姓名：" + Global_Var.Login_Name;

            码垛机ToolStripMenuItem.Visible = false;
            ButtonReportUpdateSelect.Visible = false;
            ButtonReportSelectLayout.Enabled = false;

            //部门判断、权限设置
            if (Global_Var.Login_Dpt.Substring(0, 2) == "生产")
            {
                ComboBoxReportDptType.Text = Global_Var.Login_Dpt;
                //ComboBoxReportDptType.Enabled = false;
                ComboBoxReportSelectType.Text = "全部";
                维护系列ToolStripMenuItem.Enabled = false;
            }
            else
            {
                string sqlstr = "SELECT ROLE FROM WG_DB..WG_USER WHERE U_ID = '" + Global_Var.Login_UID +"'";
                DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);
                if(dttmp != null)
                {
                    string role = dttmp.Rows[0][0].ToString();
                    Global_Var.Login_Role = role;
                    if(role == "Super")
                    {
                        ComboBoxReportDptType.Text = "全部";
                        ComboBoxReportSelectType.Text = "全部";
                    }
                    else if(role == "生管")
                    {

                        ComboBoxReportDptType.Text = "全部";
                        ComboBoxReportSelectType.Text = "全部";
                        生产部门填写ToolStripMenuItem.Enabled = false;
                        维护系列ToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        ComboBoxReportDptType.Text = "全部";
                        ComboBoxReportSelectType.Text = "全部";
                        生产部门填写ToolStripMenuItem.Enabled = false;
                        维护系列ToolStripMenuItem.Enabled = false;
                    }
                }
                else
                {
                    ComboBoxReportDptType.Text = "全部";
                    ComboBoxReportSelectType.Text = "全部";
                    生产部门填写ToolStripMenuItem.Enabled = false;
                    维护系列ToolStripMenuItem.Enabled = false;
                }
            }

            DtpFlag = false;//初始化调整时间时不执行相关操作
            
            DtpReportInputWorkDate.Value = DateTime.Now.AddDays(-1);
            DtpReportUpdateWorkDate.Value = DateTime.Now.AddDays(-1);
            DtpReportSelectStartDate.Value = DateTime.Now.AddDays(-1);
            DtpReportSelectEndDate.Value = DateTime.Now;

            DtpFlag = true;

            this.Controls.Add(panelReportSelect);
            this.Controls.Add(panelReportInput);
            this.Controls.Add(panelReportUpdate);

            panelReportSelect.Visible = false;
            panelReportInput.Visible = false;
            panelReportUpdate.Visible = false;
        }
        #endregion

        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Dispose();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
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
            FormWidth = Width - 16;
            FormHeight = Height - 16;
            LabelUserInfo.Location = new Point(2, FormHeight - 45);

            //生产日报表
            panelReportSelect.Location = new Point(0, 30);
            panelReportSelect.Size = new Size(FormWidth, FormHeight - 80);
            panelReportInput.Location = new Point(0, 30);
            panelReportInput.Size = new Size(FormWidth, FormHeight - 80);
            panelReportUpdate.Location = new Point(0, 30);
            panelReportUpdate.Size = new Size(FormWidth, FormHeight - 80);

            dataGridViewReportInput.Size = new Size(FormWidth, FormHeight - 122);
            dataGridViewReportUpdate.Size = new Size(FormWidth, FormHeight - 118);
            dataGridViewReportSelect.Size = new Size(FormWidth, FormHeight - 153);

            //码垛机

        }
        #endregion

        #region 菜单栏设置
        private void 生产部门填写_新增ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panelReportInput.Visible = true;
            panelReportUpdate.Visible = false;
            panelReportSelect.Visible = false;
        }

        private void 生产部门填写_修改ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panelReportUpdate.Visible = true;
            panelReportInput.Visible = false;
            panelReportSelect.Visible = false;
        }

        private void 生产日报表_查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelReportSelect.Visible = true;
            panelReportInput.Visible = false;
            panelReportUpdate.Visible = false;
        }

        private void 维护系列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelReportSelect.Visible = false;
            panelReportInput.Visible = false;
            panelReportUpdate.Visible = false;
            Form Modi = new FormMain_WG_Modi();
            Modi.ShowDialog();
        }

        private void 五部码垛机数据导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 码垛机_查询ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 版本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("", "");
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

            sqlstr = " INSERT INTO WG_DB..WG_USELOG (UserID, Date, ProgramName, ModuleName) VALUES('" + Global_Var.Login_UID + "', " + Global_Const.sqldatestrlong 
                   + ", '" + ProgramName + "', '" + ModuleName  + "')";

            Mssql.SQLexcute(strConnection, sqlstr);
        }
        #endregion

        #region PanelReportPublic
        private void Calculation(object sender)//总工时运算及错误字符检测
        {
            DataGridView dataGridView = (DataGridView)sender;
            int row = dataGridView.RowCount;
            int Index;
            int Number = 0, Menber = 0, PreNumber = 0;
            float PerHours = 0, StopHours = 0, TotalHours = 0, Capacity = 0;
            for (Index = 0; Index < row; Index++)
            {
                if (dataGridView.Rows[Index].Cells[2].Value.ToString() == "0" && dataGridView.Rows[Index].Cells[3].Value.ToString() == "0")
                {
                    continue;
                }
                else
                {
                    try
                    {
                        PreNumber = int.Parse(dataGridView.Rows[Index].Cells[2].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行计划数量输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[2].Value = "0";
                    }
                    try
                    {
                        Number = int.Parse(dataGridView.Rows[Index].Cells[3].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行生产数量输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[3].Value = "0";
                    }
                    try
                    {
                        Menber = int.Parse(dataGridView.Rows[Index].Cells[4].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行人数输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[4].Value = "0";
                    }
                    try
                    {
                        PerHours = float.Parse(dataGridView.Rows[Index].Cells[5].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行工时输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[5].Value = "0";
                    }
                    try
                    {
                        StopHours = float.Parse(dataGridView.Rows[Index].Cells[6].Value.ToString());
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("第" + (Index + 1).ToString() + "行停工工时输入格式错误，请重新输入！", "输入错误");
                        dataGridView.Rows[Index].Cells[6].Value = "0";
                    }

                    TotalHours = Menber * PerHours - StopHours;
                    if (TotalHours != 0)
                    {
                        Capacity = Number / TotalHours;
                        dataGridView.Rows[Index].Cells[8].Value = Capacity.ToString("f4");
                    }
                    dataGridView.Rows[Index].Cells[7].Value = TotalHours.ToString();

                }
            }
        }

        private void HeadRowLineNumber(object sender)//表头行数
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
            if(FormMain_WG.XL_List != "")
            {
                WGroup_List += "AND WGroup IN (" + FormMain_WG.XL_List + ")";
            }

            //汇总方式
            if (ComboBoxReportSelectType.Text == "全部")
            {
                sqlstr = " SELECT WorkDpt AS 生产部门, SUBSTRING(WorkDate,1,4) + '-' + SUBSTRING(WorkDate,5,2) + '-' + SUBSTRING(WorkDate,7,2) AS 生产日期, "
                   + " WGroup AS 组别, Serial AS 系列, PlanNumber AS 计划数量, "
                   + " WorkNumber AS 生产数量, Workers AS 人数, Hours AS 工时, StopHours AS 停工工时, TotalHours AS 总工时, Capacity AS 产量每人每小时, "
                   + " OrderID AS 生产单号, Remark AS 备注 FROM SC_DAILYRECORD "
                   + " WHERE 1 = 1 ";
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

                DataTable dttmp2 = Mssql.SQLselect(strConnection, sqlstr2);
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

                DataTable dttmp2 = Mssql.SQLselect(strConnection, sqlstr2);
                DataTable dttmp3 = Mssql.SQLselect(strConnection, sqlstr3);

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
                DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);
                dataGridViewReportSelect.DataSource = null;
                if(dttmp != null)
                {
                    dataGridViewReportSelect.DataSource = dttmp;

                    dataGridViewReportSelect.ReadOnly = true;

                    if (ComboBoxReportSelectType.Text == "全部")
                    {
                        dataGridViewReportSelect.Columns[3].Width = 170;
                        dataGridViewReportSelect.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        dataGridViewReportSelect.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        dataGridViewReportSelect.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        dataGridViewReportSelect.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        dataGridViewReportSelect.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        dataGridViewReportSelect.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                        dataGridViewReportSelect.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人均产能
                        dataGridViewReportSelect.Columns[11].Width = 280;//生产单号
                        dataGridViewReportSelect.Columns[12].Width = 400;//备注
                    }
                    else if (ComboBoxReportSelectType.Text == "组别-系列")
                    {
                        dataGridViewReportSelect.Columns[1].Width = 170;
                        dataGridViewReportSelect.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        dataGridViewReportSelect.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        dataGridViewReportSelect.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        dataGridViewReportSelect.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        dataGridViewReportSelect.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        dataGridViewReportSelect.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                    }
                    else if (ComboBoxReportSelectType.Text == "部门-组别-系列")
                    {
                        dataGridViewReportSelect.Columns[2].Width = 170;
                        dataGridViewReportSelect.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        dataGridViewReportSelect.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        dataGridViewReportSelect.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        dataGridViewReportSelect.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        dataGridViewReportSelect.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        dataGridViewReportSelect.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时

                    }

                    //行颜色
                    dataGridViewReportSelect.RowsDefaultCellStyle.BackColor = Color.Bisque;
                    dataGridViewReportSelect.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
                    //首列行数
                    for (int i = 0; i < this.dataGridViewReportSelect.Columns.Count; i++)
                    {
                        this.dataGridViewReportSelect.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    HeadRowLineNumber(dataGridViewReportSelect);
                    dataGridViewReportSelect.RowHeadersWidth = 60;
                    ButtonReportSelectLayout.Enabled = true;
                }
                else
                {
                    dataGridViewReportSelect.DataSource = null;
                    ButtonReportSelectLayout.Enabled = false;
                    MessageBox.Show("没有查询到数据！", "提示", MessageBoxButtons.OK);
                }
            }
            else
            {
                dataGridViewReportSelect.DataSource = null;
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
                    DataTable dttmp = (DataTable)dataGridViewReportSelect.DataSource;

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

            Form formxl = new FormMain_WG(sqlstr);
            formxl.ShowDialog();
            //if (FormMain_WG.XL_ChangeFlag)
            //{
            //    ReportInputShow(FormMain_WG.XL_List);
            //}
            formxl.Dispose();
        }
        #endregion

        #region PanelReportInput
        private void ReportInputShow(string XL_List)
        {
            if (XL_List != "")
            {
                string sqlstr = " SELECT B.WGroup AS 工作组, B.Serial AS 系列, '0' AS 计划数量, '0' AS 生产数量, '0' AS 人数, '0' AS 工时, '0' AS 停工工时, '0' AS 总工时, "
                              + " '0' AS 产量每人每小时, '' AS 生产单号, '' AS 备注 "
                              + " FROM WG_DB..SC_XL2GY AS B "
                              + " LEFT JOIN(SELECT WGroup, Serial, SUM(CONVERT(INT, WorkNumber)) AS S FROM WG_DB..SC_DAILYRECORD GROUP BY WGroup, Serial) AS A ON A.WGroup = B.WGroup AND A.Serial = B.Serial "
                              + " LEFT JOIN (SELECT WGroup, SUM(CONVERT(INT, WorkNumber)) AS S FROM WG_DB..SC_DAILYRECORD GROUP BY WGroup) AS C ON C.WGroup = B.WGroup "
                              + " WHERE B.WGroup IN("
                              + XL_List + ")"
                              + " AND B.Vaild = 'Y' "
                              + " ORDER BY C.S DESC, A.S DESC ";
                DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);
                dataGridViewReportInput.DataSource = dttmp;
                if (dttmp != null)
                {
                    dataGridViewReportInput.Columns[0].ReadOnly = true;//组别
                    dataGridViewReportInput.Columns[1].ReadOnly = true;//系列
                    dataGridViewReportInput.Columns[1].Width = 170;
                    dataGridViewReportInput.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                    dataGridViewReportInput.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                    dataGridViewReportInput.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                    dataGridViewReportInput.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                    dataGridViewReportInput.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                    dataGridViewReportInput.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                    dataGridViewReportInput.Columns[7].ReadOnly = true;//总工时
                    dataGridViewReportInput.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人均产能
                    dataGridViewReportInput.Columns[8].ReadOnly = true;//人均产能
                    dataGridViewReportInput.Columns[8].Width = 160;
                    dataGridViewReportInput.Columns[9].Width = 300;//生产单号
                    dataGridViewReportInput.Columns[10].Width = 500;//备注


                    dataGridViewReportInput.RowsDefaultCellStyle.BackColor = Color.Bisque;
                    dataGridViewReportInput.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;

                    for (int i = 0; i < this.dataGridViewReportInput.Columns.Count; i++)
                    {
                        this.dataGridViewReportInput.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    HeadRowLineNumber(dataGridViewReportInput);
                    dataGridViewReportInput.RowHeadersWidth = 60;
                    DtpFlag = true;
                }
            }
        }

        private void ButtonReportInputCommit_Click(object sender, EventArgs e)//保存
        {
            RecordUseLog("联友生产辅助工具", "生产日报表-新增");

            int row = dataGridViewReportInput.RowCount;
            int Index = 0;
            string sqlstr = "";

            string Creator = Global_Var.Login_UID;
            string Create_Date = Mssql.SQLselect(strConnection, "SELECT CONVERT(VARCHAR(20), GETDATE(), 112)").Rows[0][0].ToString();
            string WorkDpt = Global_Var.Login_Dpt;
            string WorkDate = DtpReportInputWorkDate.Value.ToString("yyyy-MM-dd");
            WorkDate = WorkDate.Split('-')[0] + WorkDate.Split('-')[1] + WorkDate.Split('-')[2];
            string Serial, Group, PlanNumber, WorkNumber, Workers, Hours, StopHours, TotalHours, Capacity, OrderID, Remark;
            for (Index = 0; Index < row; Index++)
            {
                if (dataGridViewReportInput.Rows[Index].Cells[2].Value.ToString() == "0" && dataGridViewReportInput.Rows[Index].Cells[3].Value.ToString() == "0")
                {
                    continue;
                }
                else
                {
                    Serial = dataGridViewReportInput.Rows[Index].Cells[0].Value.ToString();
                    Group = dataGridViewReportInput.Rows[Index].Cells[1].Value.ToString();
                    PlanNumber = dataGridViewReportInput.Rows[Index].Cells[2].Value.ToString();
                    WorkNumber = dataGridViewReportInput.Rows[Index].Cells[3].Value.ToString();
                    Workers = dataGridViewReportInput.Rows[Index].Cells[4].Value.ToString();
                    Hours = dataGridViewReportInput.Rows[Index].Cells[5].Value.ToString();
                    StopHours = dataGridViewReportInput.Rows[Index].Cells[6].Value.ToString();
                    TotalHours = dataGridViewReportInput.Rows[Index].Cells[7].Value.ToString();
                    Capacity = dataGridViewReportInput.Rows[Index].Cells[8].Value.ToString();
                    OrderID = dataGridViewReportInput.Rows[Index].Cells[9].Value.ToString();
                    Remark = dataGridViewReportInput.Rows[Index].Cells[10].Value.ToString();

                    sqlstr = "INSERT INTO WG_DB..SC_DAILYRECORD (Creator, Create_Date, WorkDpt, WorkDate, WGroup, Serial, PlanNumber, WorkNumber, Workers, Hours, StopHours, TotalHours, Capacity, OrderID, Remark)"
                           + "VALUES("
                           + "'" + Creator + "', "
                           + "'" + Create_Date + "', "
                           + "'" + WorkDpt + "', "
                           + "'" + WorkDate + "', "
                           + "'" + Serial + "', "
                           + "'" + Group + "', "
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
                     Mssql.SQLexcute(strConnection, sqlstr);
                }
            }
            MessageBox.Show("保存已完成！", "提示", MessageBoxButtons.OK);
        }

        private void ButtonReportInputXLSelect_Click(object sender, EventArgs e)//选择系列
        {
            //string sqlstr = "SELECT INVMB.UDF12 AS 系列 FROM COPTD "
            //              + " LEFT JOIN INVMB ON MB001 = TD004 "
            //              + "WHERE 1 = 1 "
            //              + "AND SUBSTRING(COPTD.CREATE_DATE, 1, 6) >= CONVERT(varchar(10), DATEADD(mm, -7, GETDATE()), 112) "
            //              + "AND MB001 LIKE '1%' "
            //              + "AND MB109 = 'Y' "
            //              + "AND MB005 = '1405' "
            //              + "AND INVMB.UDF12 IS NOT NULL AND INVMB.UDF12 <> '' "
            //              + "GROUP BY INVMB.UDF12 "
            //              + "ORDER BY SUM(CONVERT(INT, TD008)) DESC";

            string sqlstr = "SELECT DISTINCT WGroup AS 组别 FROM WG_DB..SC_XL2GY";

            Form formxl = new FormMain_WG(sqlstr);
            formxl.ShowDialog();
            if (FormMain_WG.XL_ChangeFlag)
            {
                ReportInputShow(FormMain_WG.XL_List);
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
                dataGridViewReportInput.DataSource = null;
                DtpFlag = false;
            }
        }
        #endregion

        #region PanelReportUpdate
        private void ReportUpdateShow(string XL_List)
        {
            string WorkDate = DtpReportUpdateWorkDate.Value.ToString("yyyy-MM-dd");
            WorkDate = WorkDate.Split('-')[0] + WorkDate.Split('-')[1] + WorkDate.Split('-')[2];
            if(XL_List != "")
            {
                string sqlstr = " SELECT B.WGroup AS 工作组, B.Serial AS 系列, B.PlanNumber AS 计划数量, B.WorkNumber AS 生产数量, B.Workers AS 人数, B.Hours AS 工时, B.StopHours AS 停工工时, B.TotalHours AS 总工时, "
                              + " B.Capacity AS 产量每人每小时, B.OrderID AS 生产单号, B.Remark AS 备注 "
                              + " FROM WG_DB..SC_DAILYRECORD AS B "
                              + " LEFT JOIN(SELECT WGroup, Serial, SUM(CONVERT(INT, WorkNumber)) AS S FROM WG_DB..SC_DAILYRECORD GROUP BY WGroup, Serial) AS A ON A.WGroup = B.WGroup AND A.Serial = B.Serial "
                              + " LEFT JOIN (SELECT WGroup, SUM(CONVERT(INT, WorkNumber)) AS S FROM WG_DB..SC_DAILYRECORD GROUP BY WGroup) AS C ON C.WGroup = B.WGroup "
                              + " WHERE B.WGroup IN("
                              + XL_List + ")";

                if (Global_Var.Login_Role != "Super")
                {
                    sqlstr += " AND B.WorkDpt =  '" + Global_Var.Login_Dpt + "' ";
                }

                sqlstr += " AND B.WorkDate = '" + WorkDate + "' ";

                sqlstr += " ORDER BY C.S DESC, A.S DESC ";
                DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);
                dataGridViewReportUpdate.DataSource = dttmp;
                if (dttmp != null)
                {
                    dataGridViewReportUpdate.Columns[0].ReadOnly = true;//组别
                    dataGridViewReportUpdate.Columns[1].ReadOnly = true;//系列
                    dataGridViewReportUpdate.Columns[1].Width = 170;
                    dataGridViewReportUpdate.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                    dataGridViewReportUpdate.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                    dataGridViewReportUpdate.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                    dataGridViewReportUpdate.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                    dataGridViewReportUpdate.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                    dataGridViewReportUpdate.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                    dataGridViewReportUpdate.Columns[7].ReadOnly = true;//总工时
                    dataGridViewReportUpdate.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人均产能
                    dataGridViewReportUpdate.Columns[8].ReadOnly = true;//人均产能
                    dataGridViewReportUpdate.Columns[8].Width = 160;
                    dataGridViewReportUpdate.Columns[9].Width = 300;//生产单号
                    dataGridViewReportUpdate.Columns[10].Width = 500;//备注


                    dataGridViewReportUpdate.RowsDefaultCellStyle.BackColor = Color.Bisque;
                    dataGridViewReportUpdate.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;

                    for (int i = 0; i < this.dataGridViewReportInput.Columns.Count; i++)
                    {
                        this.dataGridViewReportUpdate.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    HeadRowLineNumber(dataGridViewReportUpdate);
                    dataGridViewReportUpdate.RowHeadersWidth = 60;
                    DtpFlag = true;
                }
            }
        }

        private void ButtonReportUpdateSelect_Click(object sender, EventArgs e)
        {

        }

        private void ButtonReportUpdateCommit_Click(object sender, EventArgs e)
        {
            RecordUseLog("联友生产辅助工具", "生产日报表-修改");

            string sqlstr = "";
            int Index = 0;
            int row = dataGridViewReportUpdate.RowCount;
            string WorkDate = DtpReportUpdateWorkDate.Value.ToString("yyyy-MM-dd");
            WorkDate = WorkDate.Split('-')[0] + WorkDate.Split('-')[1] + WorkDate.Split('-')[2];
            string WorkDpt = Global_Var.Login_Dpt;
            string ModiFier = Global_Var.Login_UID;
            string Modi_Date = Mssql.SQLselect(strConnection, "SELECT CONVERT(VARCHAR(20), GETDATE(), 112)").Rows[0][0].ToString();
            string Serial, WGroup, PlanNumber, WorkNumber, Workers, Hours, StopHours, TotalHours, Capacity, OrderID, Remark;
            for (Index = 0; Index < row; Index++)
            {
                WGroup = dataGridViewReportUpdate.Rows[Index].Cells[0].Value.ToString();
                Serial = dataGridViewReportUpdate.Rows[Index].Cells[1].Value.ToString();
                PlanNumber = dataGridViewReportUpdate.Rows[Index].Cells[2].Value.ToString();
                WorkNumber = dataGridViewReportUpdate.Rows[Index].Cells[3].Value.ToString();
                Workers = dataGridViewReportUpdate.Rows[Index].Cells[4].Value.ToString();
                Hours = dataGridViewReportUpdate.Rows[Index].Cells[5].Value.ToString();
                StopHours = dataGridViewReportUpdate.Rows[Index].Cells[6].Value.ToString();
                TotalHours = dataGridViewReportUpdate.Rows[Index].Cells[7].Value.ToString();
                Capacity = dataGridViewReportUpdate.Rows[Index].Cells[8].Value.ToString();
                OrderID = dataGridViewReportUpdate.Rows[Index].Cells[9].Value.ToString();
                Remark = dataGridViewReportUpdate.Rows[Index].Cells[10].Value.ToString();

                sqlstr = "SELECT WGroup, Serial FROM WG_DB..SC_DAILYRECORD "
                       + "WHERE 1=1 "
                       + "AND WorkDate = '" + WorkDate + "' "
                       + "AND Serial = '" + Serial + "' "
                       + "AND WGroup = '" + WGroup + "' "
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
                if(Global_Var.Login_Role != "Super")
                {

                    sqlstr += "AND WorkDpt = '" + WorkDpt + "' ";
                }

                DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);

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
                           + "AND WorkDate = '" + WorkDate + "'"
                           + "AND Serial = '" + Serial + "'"
                           + "AND WGroup = '" + WGroup + "'";
                    if (Global_Var.Login_Role != "Super")
                    {
                        sqlstr += "AND WorkDpt = '" + WorkDpt + "'";
                    }
                    Mssql.SQLexcute(strConnection, sqlstr);
                }
            }
            MessageBox.Show("保存已完成！", "提示", MessageBoxButtons.OK);
        }

        private void ButtonReportUpdateXLSelect_Click(object sender, EventArgs e)
        {
            string sqlstr = "SELECT DISTINCT WGroup AS 组别 FROM WG_DB..SC_XL2GY";

            Form formxl = new FormMain_WG(sqlstr);
            formxl.ShowDialog();
            if (FormMain_WG.XL_ChangeFlag)
            {
                ReportUpdateShow(FormMain_WG.XL_List);
            }
            formxl.Dispose();
            DtpFlag = true;
        }

        private void DataGridViewReportUpdate_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Calculation(sender);
        }

        private void DtpReportUpdateWorkDate_ValueChanged(object sender, EventArgs e)//判断日期是否有修改
        {
            if (DtpFlag)
            {
                dataGridViewReportUpdate.DataSource = null;
                DtpFlag = false;
            }
        }
        #endregion

        #region PanelReportModi

        #endregion

        #region PanelRobotInput
        #endregion

        #region PanelRobotSelect
        #endregion

    }
}
