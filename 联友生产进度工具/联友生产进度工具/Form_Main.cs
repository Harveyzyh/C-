using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace 联友生产进度工具
{
    public partial class Form_Main : Form
    {
        #region 静态数据设置

        private delegate string NewTaskDelegate(DataTable dttmp); //任务代理

        public static DataTable show_datatable = new DataTable();

        #endregion
        
        private string dt2sqlstr(DataTable dttmp)//数据表转成sql语句
        {
            int insert = 0;
            int update = 0;
            int error = 0;
            string errorstr = "";
            int row = 0;
            int row_total = dttmp.Rows.Count;
            int col_total = dttmp.Columns.Count;
            string updatestr = "";
            string insertstr = "";
            DataTable dttmp_erp = new DataTable();

            string SC003 = "";
            string SC001 = "";
            string SC013 = "";
            string SC010 = "";
            string SC011 = "";
            string SC012 = "";
            string SC025 = "";
            string SC015 = "";
            string SC016 = "";
            string SC017 = "";
            string SC024 = "";
            string SC023 = "";
            string SC026 = "";

            if(dttmp.Rows[0][1].ToString() == "上线日期")
            {
                row ++;
            }

            for (; row < row_total; row++ )
            {
                if (dttmp.Rows[row][2].ToString() == "生产单号")
                {
                    continue;
                }
                else if (dttmp.Rows[row][2].ToString() != "")
                {
                    SC001 = dttmp.Rows[row][2].ToString();
                    SC001 = SC001.Split('-')[0].Trim() + '-' + SC001.Split('-')[1].Trim() + '-' + SC001.Split('-')[2].Trim();
                    SC001 = SC001.Replace("（新增）", "").Replace("（变更）", "").Replace("（更改）", "").Replace("(新增)", "").Replace("(变更)", "").Replace("(更改)", "");

                    SC003 = dttmp.Rows[row][0].ToString().Replace("-", "");
                    if (SC003.Contains("转"))
                    {
                        break;
                    }

                    dttmp_erp = dtFromERP(SC001);

                    if(dttmp_erp.Rows.Count == 1)
                    {
                        SC013 = dttmp_erp.Rows[0][1].ToString();
                        SC010 = dttmp_erp.Rows[0][2].ToString();
                        SC011 = dttmp_erp.Rows[0][3].ToString();
                        SC012 = dttmp_erp.Rows[0][4].ToString();
                        SC025 = dttmp_erp.Rows[0][5].ToString();
                        SC015 = dttmp_erp.Rows[0][6].ToString();
                        SC016 = dttmp_erp.Rows[0][7].ToString();
                        SC017 = dttmp_erp.Rows[0][8].ToString();
                        SC024 = dttmp_erp.Rows[0][9].ToString();
                        SC023 = dttmp_erp.Rows[0][10].ToString();
                        SC026 = dttmp_erp.Rows[0][11].ToString();

                        SC010 = insertconvert("str", SC010);
                        SC011 = insertconvert("str", SC011);
                        SC012 = insertconvert("str", SC012);
                        SC013 = insertconvert("str", SC013);
                        SC015 = insertconvert("str", SC015);
                        SC017 = insertconvert("str", SC017);
                    }
                    else
                    {
                        MessageBox.Show("订单号：" + SC001 + " 存在异常！，请确认。", "错误");
                        break;
                    }
                    


                    DataTable dttmp2 = Mssql.SQLselect(Global_Const.strConnection_ROBOT, "SELECT SC001 FROM SCHEDULE WHERE SC001 = '" + SC001 + "'");

                    if (dttmp2 != null)
                    {
                        updatestr = "UPDATE SCHEDULE SET "
                                    + " MODIFIER = '" + Global_Var.Login_UID + "', MODI_DATE = " + Global_Const.sqldatestrlong + ", "
                                    + " SC003 = '" + SC003 + "', "
                                    + " SC010 = '" + SC010 + "', "
                                    + " SC011 = '" + SC011 + "', "
                                    + " SC012 = '" + SC012 + "', "
                                    + " SC013 = '" + SC013 + "', "
                                    + " SC015 = '" + SC015 + "', " 
                                    + " SC016 = '" + SC016 + "', "
                                    + " SC017 = '" + SC017 + "', "
                                    + " SC023 = '" + SC023 + "', "
                                    + " SC024 = '" + SC024 + "', "
                                    + " SC025 = '" + SC025 + "', " 
                                    + " SC026 = '" + SC026 + "'"
                                    + " WHERE SC001 = '" + SC001 + "' ";

                        if (0 == Mssql.SQLexcute(Global_Const.strConnection_ROBOT, updatestr))
                        {
                            update++;
                        }
                        else
                        {
                            error++;
                            if (errorstr == "")
                            {
                                errorstr += SC001;
                            }
                            else errorstr += "; " + SC001;
                        }
                    }
                    else
                    {
                        insertstr = "INSERT INTO SCHEDULE ( "
                                    + " CREATOR, CREATE_DATE, "
                                    + " SC001, SC003, SC010, "
                                    + " SC011, SC012, SC013, SC015, SC016, SC017, "
                                    + " SC023, SC024, SC025, SC026, SC033 )"
                                    + " VALUES( "
                                    + " '" + Global_Var.Login_UID + "', " + Global_Const.sqldatestrlong + ", "
                                    + " '" + SC001 + "', '" + SC003 + "', '" + SC010 
                                    + "', '" + SC011 + "', '" + SC012 + "', '" + SC013 + "', '" + SC015
                                    + "', '" + SC016 + "', '" + SC017 + "', '" + SC023 + "', '" + SC024 + "', '" + SC025
                                    + "', '" + SC026 + "', '0' )";
                        
                        if (0 == Mssql.SQLexcute(Global_Const.strConnection_ROBOT, insertstr))
                        {
                            insert++;
                        }
                        else
                        {
                            error++;
                            if (errorstr == "")
                            {
                                errorstr += SC001;
                            }
                            else errorstr += "; " + SC001;
                        }
                    }
                }
                else continue;
            }
            string returnstr = "新增" + insert.ToString() + "条，更新" + update.ToString() + "条，失败" + error.ToString() + "\r\n";
            if (errorstr == "")
            {
                returnstr += "\r\n\r\n";
            }
            else
            {
                returnstr +=  "失败单号为：" + errorstr;
                returnstr += "\r\n\r\n";
            }

            return returnstr;
        }

        private DataTable dtFromERP(string SC001)
        {
            DataTable dttmp = new DataTable();
            string sqlstr = "SELECT "
                          + "(RTRIM(COPTD.TD001) + '-' + RTRIM(COPTD.TD002) + '-' + RTRIM(COPTD.TD003)), " //订单号
                          + "CONVERT(INT, COPTD.TD008), " //订单数量
                          + "RTRIM(COPTD.TD005), " //品名
                          + "RTRIM(COPTD.UDF08), " //保友品名
                          + "RTRIM(COPTD.TD006), " //规格
                          + "RTRIM(COPTD.UDF10), " //电商代码
                          + "RTRIM(COPTD.TD053), " //配置方案
                          + "RTRIM(COPTQ.TQ003), " //配置描述
                          + "RTRIM(COPTD.TD020), " //描述备注
                          + "RTRIM(COPTD.UDF05), " //客户编码
                          + "(CASE WHEN TC004 = '0118' THEN RTRIM(INVMB.UDF04) ELSE RTRIM(INVMB.UDF05) END), " //生产车间
                          + "(CASE WHEN COPTC.UDF09 = '是' THEN 'Y' ELSE 'N' END) " //急单

                          + "FROM COPTD AS COPTD "
                          + "Left JOIN COPTC AS COPTC On COPTD.TD001 = COPTC.TC001 and COPTD.TD002 = COPTC.TC002 "
                          + "Left JOIN COPTQ AS COPTQ On COPTD.TD053 = COPTQ.TQ002 and COPTD.TD004 = COPTQ.TQ001 "
                          + "LEFT JOIN INVMB AS INVMB ON COPTD.TD004 = INVMB.MB001 "
                          + "WHERE 1 = 1 AND COPTC.TC027 = 'Y' AND COPTD.TD004 NOT LIKE '6%' AND COPTD.TD004 NOT LIKE '7%' "
                          + "AND RTRIM(COPTD.TD001) + '-' + RTRIM(COPTD.TD002) + '-' + RTRIM(COPTD.TD003) = '" + SC001 + "' ";
            
            dttmp = Mssql.SQLselect(Global_Const.strConnection_COMFORT, sqlstr);

            return dttmp;
        }

        private void getdatatable(bool show) //数据库资料显示到界面
        {
            string show_sqlstr = " SELECT "
                                         + " SC003 AS 上线日期, "
                                         + " SC001 AS 订单号, "
                                         + " (CASE SC026 WHEN 'Y' THEN '是' ELSE '' END ) AS 急单, "
                                         + " SC013 AS 订单数量, "
                                         + " SC010 AS 品名, "
                                         + " SC011 AS 保友品名, "
                                         + " SC012 AS 规格, "
                                         + " SC015 AS 配置方案, "
                                         + " SC016 AS 配置方案描述, "
                                         + " SC017 AS 描述备注,  "
                                         + " SC023 AS 生产车间,  "
                                         + " SC024 AS 客户编码,  "
                                         + " SC025 AS 电商编码,  "
                                         + " (CASE WHEN SC037 = 'N' THEN '未维护' ELSE SC037 END) AS 栈板分类码, "
                                         + " Type AS 栈板分类, "
                                         + " (CASE WHEN SC036 = 'NULL' THEN '未维护' ELSE SC036 END) AS 纸箱尺寸码, "
                                         + " Size AS 纸箱尺寸, "
                                         + " (CASE SC033 WHEN '0' THEN '未完成' WHEN '1' THEN '已完成' END) AS 完成状态  "
                                         + " FROM SCHEDULE "
                                         + " LEFT JOIN BoxSizeCode ON SC036 = Code "
                                         + " LEFT JOIN SplitTypeCode ON TypeCode = SC037 "
                                         + " WHERE 1=1 ";
            if (show)
            {
                show_sqlstr += " AND SC003 >= CONVERT(VARCHAR(20), GETDATE(), 112)";
            }
            show_sqlstr += " ORDER BY SC003, (CASE SC026 WHEN 'Y' THEN '是' ELSE '' END ) DESC, SC001 ";

            if (show_datatable != null)
            {
                show_datatable.Clear();
            }
            show_datatable = Mssql.SQLselect(Global_Const.strConnection_ROBOT, show_sqlstr);

            if (show_datatable != null)
            {

                int row_count = 0;
                int datatable_rows = show_datatable.Rows.Count;

                for (; row_count < datatable_rows; row_count++)
                {
                    //日期格式调整：增加‘-’
                    show_datatable.Rows[row_count][0] = dateconvert(show_datatable.Rows[row_count][0].ToString());
                }
                grd1.DataSource = show_datatable;
                grd1.RowsDefaultCellStyle.BackColor = Color.Bisque;
                grd1.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            }
            else
            {
                grd1.DataSource = null;
            }

            if (grd1.ColumnCount > 0)
            {
                grd1.Columns[1].Width = 180; //列宽-生产单号
                grd1.Columns[4].Width = 180; //列宽-品名
                grd1.Columns[5].Width = 180; //列宽-保有品名
            }
        }

        #region 数据转换
        private string dateconvert(string datestr) //显示日期转换 加 -
        {
            string returnstr = "";
            int strlenth = datestr.Length;
            if (strlenth == 14)
            {
                returnstr = datestr.Substring(0, 4) + "-" + datestr.Substring(4, 2) + "-" + datestr.Substring(6, 2) + " " 
                    + datestr.Substring(8, 2) + ":" + datestr.Substring(10, 2) + ":" + datestr.Substring(12, 2);
            }
            else if (strlenth == 8)
            {
                returnstr = datestr.Substring(0, 4) + "-" + datestr.Substring(4, 2) + "-" + datestr.Substring(6, 2);
            }
            else if ((strlenth >8 ) & (strlenth < 14))
            {
                returnstr = datestr.Substring(0, 4) + "-" + datestr.Substring(4, 2) + "-" + datestr.Substring(6, 2);
            }
            else
            {
                returnstr = datestr;
            }

            return returnstr;
        }

        private string insertconvert(string name, string insertsrt) //转SQL前部分字段转换
        {
            string returnstr = "";

            if (name == "str")
            {
                returnstr = insertsrt.Replace("^", "[^]").Replace("%", "[%]").Replace("[", "[[]").Replace("'", "‘");
            }

            return returnstr;
        }
        #endregion

        #region 窗体设计
        public Form_Main()
        {
            InitializeComponent();
            Form_Main_Init();
        }

        private void Form_Main_Init() // 窗体显示初始化
        {
            label1.Text = "";
            getdatatable(true);
            导出ToolStripMenuItem.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
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

        private void Form_MainResized(object sender, EventArgs e) //窗口大小变化
        {
            int x = 0;
            int y = 0;

            x = this.Width;
            y = this.Height;


            this.grd1.Size = new System.Drawing.Size(x-15, y-75);
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e) //菜单-文件-打开文件
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Excel文件|*.xlsx;*.xls";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                excelObj.FilePath = Path.GetDirectoryName(openFileDialog.FileName);
                excelObj.FileName = Path.GetFileName(openFileDialog.FileName);
                excelObj.IsWrite = false;
                excelObj.IsTitleRow = true;


                excel.ExcelOpt(excelObj);
                try
                {
                    NewTaskDelegate task = dt2sqlstr;
                    IAsyncResult asyncResult = task.BeginInvoke(excelObj.CellDt, null, null);
                    string result = task.EndInvoke(asyncResult);
                    getdatatable(true);
                    MessageBox.Show(result, "导入结果", MessageBoxButtons.OK);
                }
                catch (Exception es)
                {
                    if (MessageBox.Show("请截图联系资讯课！软件即将退出。\r\n" + es.ToString(), "程序出错", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }
            }
            

            
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)  //菜单-文件-导出
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) //菜单-文件-退出
        {
            if (MessageBox.Show("是否退出？", "提示",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Dispose();
                Application.Exit();
            }
        }

        private void 使用帮助ToolStripMenuItem_Click(object sender, EventArgs e) //菜单-帮助-使用帮助
        {
            MessageBox.Show("直接打开周生产进度表即可导入。\r\n", "使用帮助", MessageBoxButtons.OK);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e) //菜单-帮助-关于
        {
            MessageBox.Show("软件说明：此软件名为联友生产进度工具。\r\n\t" 
                            + "现暂于生产五部使用！\r\n\r\n" 
                            + "软件版本：" + Global_Var.Software_Version + "\r\n" 
                            + "文件版本：" + Global_Var.File_Version + "\r\n"
                            ,"关于",MessageBoxButtons.OK);
        }

        private void button1_Click(object sender, EventArgs e) //刷新按钮
        {
            Button B = (Button)sender;
            string text = B.Text;
            if (text == "显示当前日期")
            {
                getdatatable(true);
                B.Text = "显示全部";
            }
            else if(text == "显示全部")
            {
                getdatatable(false);
                B.Text = "显示当前日期";
            }
        }
        #endregion
    }
}
