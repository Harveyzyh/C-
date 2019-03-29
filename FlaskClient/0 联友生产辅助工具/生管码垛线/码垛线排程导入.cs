using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ;
using System.Collections.Generic;

namespace 联友生产辅助工具.生管码垛线
{
    public partial class 码垛线排程导入 : Form
    {
        #region 静态数据设置

        private delegate string NewTaskDelegate(DataTable dttmp); //任务代理

        public static DataTable show_datatable = new DataTable();

        public Mssql mssql = new Mssql();

        private static string strConnertion = Global_Const.strConnection_ROBOT;

        #endregion

        #region 窗体设计
        public 码垛线排程导入()
        {
            InitializeComponent();
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            getdatatable(true);
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
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            DataGridView_List.Location = new Point(0, panel_Title.Height + 2);
            DataGridView_List.Size = new Size(FormWidth, FormHeight - panel_Title.Height - 2);
        }
        #endregion

        #endregion

        private string GetTime()
        {
            string Time = "";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Mode", "Sort");
            try
            {
                dict = FormLogin.HttpPost_Dict(FormLogin.HttpURL + "/Client/GetTime", dict);
                dict.TryGetValue("Time", out Time);
                string Time2 = (int.Parse(Time) + 2).ToString();
            }
            catch
            {
                MessageBox.Show("无法获取系统日期", "错误", MessageBoxButtons.OK);
            }
            return Time;
        }

        /// <summary>
        /// 计算字符串中子串出现的次数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="substring">子串</param>
        /// <returns>出现的次数</returns>
        static int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }

            return 0;
        }

        private bool CheckDt(DataTable Dt)
        {
            int rowTotal = Dt.Rows.Count;
            string SC001 = "";
            string Msg = "";
            for (int rowIndex = 0; rowIndex < rowTotal; rowIndex++)
            {
                if (Dt.Rows[rowIndex][2].ToString() == "生产单号")
                {
                    continue;
                }
                SC001 = Dt.Rows[rowIndex][2].ToString();
                if(SubstringCount(SC001, "-") < 2)
                {
                    Msg += (rowIndex + 1).ToString() + ",";
                }
            }
            if (Msg == "") return true;
            else
            {
                MessageBox.Show("导入文件中行：" + Msg + "有异常，请检查", "导入失败", MessageBoxButtons.OK);
                return false;
            }
        }

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
            string returnstr = "";

            string SC003 = "";
            string SC001 = "";

            int SysTime = int.Parse(GetTime());
            int WorkTime = 0;

            if(dttmp.Rows[0][1].ToString() == "上线日期")
            {
                row ++;
            }

            for (; row < row_total; row++ )
            {
                //限定日期
                try
                {
                    SC003 = dttmp.Rows[row][0].ToString().Replace("-", "");
                    WorkTime = int.Parse(SC003);
                    if(WorkTime < SysTime)
                    {
                        continue;
                    }
                }
                catch
                {
                    continue;
                }


                if (dttmp.Rows[row][2].ToString() == "生产单号")
                {
                    continue;
                }
                else if (dttmp.Rows[row][2].ToString() != "")
                {
                    SC001 = dttmp.Rows[row][2].ToString();
                    SC001 = SC001.Split('-')[0].Trim() + '-' + SC001.Split('-')[1].Trim() + '-' + SC001.Split('-')[2].Trim();
                    SC001 = SC001.Replace("（新增）", "").Replace("（变更）", "").Replace("（更改）", "").Replace("(新增)", "").Replace("(变更)", "").Replace("(更改)", "");

                    //SC003 = dttmp.Rows[row][0].ToString().Replace("-", "");
                    returnstr += SC001 + ":" + SC003 + "; ";
                    if (SC003.Contains("转"))
                    {
                        break;
                    }
                    
                    DataTable dttmp2 = mssql.SQLselect(strConnertion, "SELECT SC001 FROM SCHEDULE WHERE SC001 = '" + SC001 + "'");

                    if (dttmp2 != null)
                    {
                        updatestr = "UPDATE SCHEDULE SET "
                                    + " MODIFIER = '" + FormLogin.Login_Uid + "', MODI_DATE = (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', '')), "
                                    + " SC003 = '" + SC003 + "', "
                                    + " SC038 = 'N' "
                                    + " WHERE SC001 = '" + SC001 + "' ";

                        if (0 == mssql.SQLexcute(strConnertion, updatestr))
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
                        insertstr = "INSERT INTO SCHEDULE (CREATOR, CREATE_DATE, SC001, SC003) VALUES ( "
                                  + "'" + FormLogin.Login_Uid + "', (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', '')), "
                                  + "'" + SC001 + "', '" + SC003 + "')";
                        
                        if (0 == mssql.SQLexcute(strConnertion, insertstr))
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
            //string returnstr = "新增" + insert.ToString() + "条，更新" + update.ToString() + "条，失败" + error.ToString() + "\r\n";
            //if (errorstr == "")
            //{
            //    returnstr += "\r\n\r\n";
            //}
            //else
            //{
            //    returnstr +=  "失败单号为：" + errorstr;
            //    returnstr += "\r\n\r\n";
            //}

            return returnstr;
        }

        private void getdatatable(bool show) //数据库资料显示到界面
        {
            string show_sqlstr = " SELECT DISTINCT "
                                         + " SC003 AS 上线日期, "
                                         + " SC001 AS 订单号, "
                                         + " (CASE SC026 WHEN 'Y' THEN '是' ELSE '' END ) AS 急单, "
                                         + " SC013 AS 订单数量, "
                                         + " SC002 AS 订单类型, "
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
                                         //+ " PO_Type AS 栈板分类, "
                                         + " (CASE WHEN SC036 = 'NULL' THEN '未维护' ELSE SC036 END) AS 纸箱尺寸码, "
                                         //+ " BoxSize AS 纸箱尺寸, "
                                         + " (CASE SC033 WHEN '0' THEN '未完成' WHEN '1' THEN '已完成' END) AS 完成状态  "
                                         + " FROM SCHEDULE "
                                         + " LEFT JOIN BoxSizeCode ON SC036 = BoxCode "
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
            show_datatable = mssql.SQLselect(strConnertion, show_sqlstr);

            if (show_datatable != null)
            {

                int row_count = 0;
                int datatable_rows = show_datatable.Rows.Count;

                for (; row_count < datatable_rows; row_count++)
                {
                    //日期格式调整：增加‘-’
                    show_datatable.Rows[row_count][0] = dateconvert(show_datatable.Rows[row_count][0].ToString());
                }
                DataGridView_List.DataSource = show_datatable;
                DataGridView_List.RowsDefaultCellStyle.BackColor = Color.Bisque;
                DataGridView_List.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            }
            else
            {
                DataGridView_List.DataSource = null;
            }

            if (DataGridView_List.ColumnCount > 0)
            {
                DataGridView_List.Columns[1].Width = 180; //列宽-生产单号
                DataGridView_List.Columns[2].Width = 30;
                DataGridView_List.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DataGridView_List.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DataGridView_List.Columns[5].Width = 180; //列宽-品名
                DataGridView_List.Columns[6].Width = 180; //列宽-保有品名
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

        #region 按钮
        private void button_Show_Click(object sender, EventArgs e) //刷新按钮
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

        private void button_Input_Click(object sender, EventArgs e)
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
                
                if(excelObj.Status == "Yes" && CheckDt(excelObj.CellDt))
                {
                    try
                    {
                        NewTaskDelegate task = dt2sqlstr;
                        IAsyncResult asyncResult = task.BeginInvoke(excelObj.CellDt, null, null);
                        string result = task.EndInvoke(asyncResult);
                        getdatatable(true);

                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add("User", FormLogin.Login_Uid);
                        dict.Add("Mode", "Insert");
                        dict.Add("Detail", result);
                        FormLogin.HttpPost_Dict(FormLogin.HttpURL + "/Client/MaDuo/GetInfo", dict);

                        MessageBox.Show("已提交至后台服务器", "导入结果", MessageBoxButtons.OK);
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
        }
        #endregion
    }
}
