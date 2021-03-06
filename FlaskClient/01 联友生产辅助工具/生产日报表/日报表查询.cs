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
using DataGridViewAutoFilter;

namespace HarveyZ.生产日报表
{
    public partial class 日报表查询: Form
    {
        #region 局部变量
        Mssql mssql = new Mssql();
        DataGridViewFunction Get = new DataGridViewFunction();
        public static string strConnection = FormLogin.infObj.connWG;
        private static bool DtpFlag = false;

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;
        #endregion

        #region Init
        public 日报表查询(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);

            FormMainInit();

            FormMain_Resized_Work();
        }

        private void FormMainInit()
        {
            ButtonReportSelectLayout.Enabled = false;

            //部门判断、权限设置
            if (FormLogin.infObj.userDpt.Length > 2)
            {
                if (FormLogin.infObj.userDpt.Substring(0, 2) == "生产")
                {
                    ComboBoxReportDptType.Text = FormLogin.infObj.userDpt;
                    ComboBoxReportSelectType.Text = "全部";
                }
                else
                {
                    string slqStr = "SELECT ROLE FROM WG_DB..WG_USER WHERE U_ID = '" + FormLogin.infObj.userId + "'";
                    DataTable dttmp = mssql.SQLselect(strConnection, slqStr);
                    if (dttmp != null)
                    {
                        string role = dttmp.Rows[0][0].ToString();
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
            }
            else
            {
                ComboBoxReportDptType.Text = "全部";
                ComboBoxReportSelectType.Text = "全部";
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
            PanelTitle.Size = new Size(FormWidth, PanelTitle.Height);
            DgvMain.Location = new Point(0, PanelTitle.Height + 2);
            DgvMain.Size = new Size(FormWidth, FormHeight - PanelTitle.Height - 2);
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

        #region PanelReportSelect
        private void ButtonSelectSubmit_Click(object sender, EventArgs e)
        {
            bool NullFlag = false;

            string slqStr = "", sql_date = "", sql_dpt = "", WGroup_List = "";

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
                slqStr = " SELECT WorkDpt AS 生产部门, SUBSTRING(WorkDate,1,4) + '-' + SUBSTRING(WorkDate,5,2) + '-' + SUBSTRING(WorkDate,7,2) AS 生产日期, "
                   + " WGroup AS 组别, Serial AS 系列, Line AS 线别, PlanNumber AS 计划数量, "
                   + " WorkNumber AS 生产数量, Workers AS 人数, Hours AS 工时, StopHours AS 停工工时, TotalHours AS 总工时, Capacity AS 产量每人每小时, "
                   + " OrderID AS 生产单号, Remark AS 备注 FROM WG_DB..SC_DRY_DAILYRECORD "
                   + " WHERE 1 = 1 "
                   + " AND (PlanNumber <> '0' OR WorkNumber <> '0') ";
                slqStr += sql_date + sql_dpt;
                slqStr += WGroup_List;
            }
            else if(ComboBoxReportSelectType.Text == "组别-系列")
            {
                string slqStr2 = "";
                slqStr = "";
                slqStr2 = " SELECT DISTINCT WGroup FROM WG_DB..SC_DRY_DAILYRECORD ";
                slqStr2 += " WHERE 1=1 ";
                slqStr2 += sql_date + sql_dpt;
                slqStr2 += WGroup_List;

                DataTable dttmp2 = mssql.SQLselect(strConnection, slqStr2);
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
                            slqStr += " UNION ";
                        }
                        slqStr += " SELECT WGroup AS 组别, Serial AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                        slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, Hours))) AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                        slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                        slqStr += " FROM WG_DB..SC_DRY_DAILYRECORD ";
                        slqStr += " WHERE 1 = 1 ";
                        slqStr += sql_date + sql_dpt;
                        slqStr += " AND WGroup = '" + WGroup + "'";
                        slqStr += " GROUP BY WGroup, Serial ";

                        slqStr += " UNION ";
                        slqStr += " SELECT WGroup AS 组别, '小计' AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                        slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, '' AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                        slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                        slqStr += " FROM WG_DB..SC_DRY_DAILYRECORD ";
                        slqStr += " WHERE 1 = 1 ";
                        slqStr += sql_date + sql_dpt;
                        slqStr += " AND WGroup = '" + WGroup + "'";
                        slqStr += " GROUP BY WGroup";
                    }

                    slqStr += " UNION ";

                    slqStr += " SELECT '' AS 组别, '总计' AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                    slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, '' AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                    slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                    slqStr += " FROM WG_DB..SC_DRY_DAILYRECORD ";
                    slqStr += " WHERE 1 = 1 ";
                    slqStr += sql_date + sql_dpt;
                    slqStr += WGroup_List;

                    slqStr += " ORDER BY 组别 DESC ";
                }
                else
                {
                    NullFlag = true;
                }
            }
            else if(ComboBoxReportSelectType.Text == "部门-组别-系列")
            {
                string slqStr3 = "";
                string slqStr2 = "";
                slqStr = "";

                slqStr2 = " SELECT DISTINCT WGroup FROM WG_DB..SC_DRY_DAILYRECORD ";
                slqStr2 += " WHERE 1=1 ";
                slqStr2 += sql_date + sql_dpt;
                slqStr2 += WGroup_List;

                slqStr3 = " SELECT DISTINCT WorkDpt FROM WG_DB..SC_DRY_DAILYRECORD ";
                slqStr3 += " WHERE 1=1 ";
                slqStr3 += sql_date + sql_dpt;

                DataTable dttmp2 = mssql.SQLselect(strConnection, slqStr2);
                DataTable dttmp3 = mssql.SQLselect(strConnection, slqStr3);

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
                            slqStr += " UNION ";
                        }

                        slqStr += " SELECT WorkDpt AS 生产部门, WGroup AS 组别, Serial AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                        slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, Hours))) AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                        slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                        slqStr += " FROM WG_DB..SC_DRY_DAILYRECORD ";
                        slqStr += " WHERE 1 = 1 ";
                        slqStr += sql_date + sql_dpt;
                        slqStr += " AND WGroup = '" + WGroup + "'";
                        slqStr += " GROUP BY WorkDpt, WGroup, Serial ";

                        slqStr += " UNION ";
                        slqStr += " SELECT WorkDpt AS 生产部门, WGroup AS 组别, '小计' AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                        slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, '' AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                        slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                        slqStr += " FROM WG_DB..SC_DRY_DAILYRECORD ";
                        slqStr += " WHERE 1 = 1 ";
                        slqStr += sql_date + sql_dpt;
                        slqStr += " AND WGroup = '" + WGroup + "'";
                        slqStr += " GROUP BY WorkDpt, WGroup";
                    }

                    slqStr += " UNION ";

                    slqStr += " SELECT '' AS 生产部门, '' AS 组别, '总计' AS 系列, CONVERT(VARCHAR(50), SUM(CONVERT(INT, PlanNumber))) AS 计划数量, CONVERT(VARCHAR(50), SUM(CONVERT(INT, WorkNumber))) AS 生产数量, ";
                    slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(INT, Workers))) AS 人数, '' AS 工时, CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, StopHours))) AS 停工工时, ";
                    slqStr += " CONVERT(VARCHAR(50), SUM(CONVERT(FLOAT, TotalHours))) AS 总工时, CONVERT(VARCHAR(50), (SUM(CONVERT(FLOAT, Capacity))) / COUNT(Capacity)) AS 产量每人每小时 ";
                    slqStr += " FROM WG_DB..SC_DRY_DAILYRECORD ";
                    slqStr += " WHERE 1 = 1 ";
                    slqStr += sql_date + sql_dpt;
                    slqStr += WGroup_List;

                    slqStr += " ORDER BY 组别 DESC ";
                }
                else
                {
                    NullFlag = true;
                }
            }

            if (!NullFlag)
            {
                DataTable dttmp = mssql.SQLselect(strConnection, slqStr);
                DgvMain.DataSource = null;
                if(dttmp != null)
                {
                    DgvMain.DataSource = dttmp;

                    //Get.GridViewDataLoad(dttmp, DgvMain);
                    //Get.GridViewHeaderFilter(DgvMain);
                    
                    if (ComboBoxReportSelectType.Text == "全部")
                    {

                        DgvMain.Columns[3].Width = 170;
                        DgvMain.Columns[4].Width = 50;
                        DgvMain.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        DgvMain.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        DgvMain.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        DgvMain.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        DgvMain.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        DgvMain.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                        DgvMain.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人均产能
                        DgvMain.Columns[12].Width = 280;//生产单号
                        DgvMain.Columns[13].Width = 400;//备注
                    }
                    else if (ComboBoxReportSelectType.Text == "组别-系列")
                    {
                        DgvMain.Columns[1].Width = 170;
                        DgvMain.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        DgvMain.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        DgvMain.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        DgvMain.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        DgvMain.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        DgvMain.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时
                    }
                    else if (ComboBoxReportSelectType.Text == "部门-组别-系列")
                    {
                        DgvMain.Columns[2].Width = 170;
                        DgvMain.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//计划数量
                        DgvMain.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//生产数量
                        DgvMain.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//人数
                        DgvMain.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//工时
                        DgvMain.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//停工工时
                        DgvMain.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//总工时

                    }

                    //行颜色
                    DgvMain.RowsDefaultCellStyle.BackColor = Color.Bisque;
                    DgvMain.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
                    //首列行数
                    for (int i = 0; i < this.DgvMain.Columns.Count; i++)
                    {
                        this.DgvMain.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    日报表新增.HeadRowLineNumber(DgvMain);
                    DgvMain.RowHeadersWidth = 60;
                    ButtonReportSelectLayout.Enabled = true;

                }
                else
                {
                    DgvMain.DataSource = null;
                    ButtonReportSelectLayout.Enabled = false;
                    MessageBox.Show("没有查询到数据！", "提示", MessageBoxButtons.OK);
                }
            }
            else
            {
                DgvMain.DataSource = null;
                ButtonReportSelectLayout.Enabled = false;
                MessageBox.Show("没有查询到数据！", "提示", MessageBoxButtons.OK);
            }
        }

        private void ButtonReportSelectLeyout_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            
            excelObj.dataDt = (DataTable)DgvMain.DataSource;
            excelObj.defauleFileName = "生产日报表导出_" + DateTime.Now.ToString("yyyy-MM-dd");
            excelObj.isWrite = true;

            if (excel.ExcelOpt(excelObj))
            {
                if (excelObj.status)
                {
                    Msg.Show("Excel导出成功！");
                }
                else
                {
                    MessageBox.Show(excelObj.msg, "错误");
                }
            }
        }

        private void ButtonReportSelectWG_Click(object sender, EventArgs e)
        {
            string slqStr = "SELECT DISTINCT WGroup AS 组别 FROM WG_DB..SC_DRY_XL2GY";

            Form formxl = new 日报表获取组别系列(slqStr);
            formxl.ShowDialog();
            formxl.Dispose();
        }
        #endregion
    }
}