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

        private static DataTable showDt = new DataTable();

        private Mssql mssql = new Mssql();

        private static string connRobot = FormLogin.connRobot;
        private static string connComfort = FormLogin.connComfort;

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
            DgvShow(true);
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

        #region 逻辑设计
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
                if(Normal.GetSubstringCount(SC001, "-") < 2)
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

        private string Dt2SqlStr(DataTable dttmp)//数据表转成sql语句
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

            int SysTime = int.Parse(Normal.GetDbSysTime("Sort"));
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
                    
                    returnstr += SC001 + ":" + SC003 + "; ";
                    if (SC003.Contains("转"))
                    {
                        break;
                    }
                    
                    DataTable dttmp2 = mssql.SQLselect(connRobot, "SELECT SC001 FROM SCHEDULE WHERE SC001 = '" + SC001 + "'");

                    if (dttmp2 != null)
                    {
                        updatestr = "UPDATE SCHEDULE SET "
                                    + " MODIFIER = '" + FormLogin.Login_Uid + "', MODI_DATE = (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', '')), "
                                    + " SC003 = '" + SC003 + "', "
                                    + " SC038 = 'N' "
                                    + " WHERE SC001 = '" + SC001 + "' ";

                        if (0 == mssql.SQLexcute(connRobot, updatestr))
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
                        
                        if (0 == mssql.SQLexcute(connRobot, insertstr))
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
            return returnstr;
        }

        private void DgvShow(bool show) //数据库资料显示到界面
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
                                         + " (CASE WHEN SC036 = 'NULL' THEN '未维护' ELSE SC036 END) AS 纸箱尺寸码, "
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

            if (showDt != null)
            {
                showDt.Clear();
            }
            showDt = mssql.SQLselect(connRobot, show_sqlstr);

            if (showDt != null)
            {

                int row_count = 0;
                int datatable_rows = showDt.Rows.Count;

                for (; row_count < datatable_rows; row_count++)
                {
                    //日期格式调整：增加‘-’
                    showDt.Rows[row_count][0] = Normal.ConvertDate(showDt.Rows[row_count][0].ToString());
                }
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowColor(DgvMain);
            }
            else
            {
                DgvMain.DataSource = null;
            }

            if (DgvMain.ColumnCount > 0)
            {
                DgvMain.Columns[1].Width = 180; //列宽-生产单号
                DgvMain.Columns[2].Width = 30;
                DgvMain.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvMain.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvMain.Columns[5].Width = 180; //列宽-品名
                DgvMain.Columns[6].Width = 180; //列宽-保有品名
            }
        }
        #endregion

        #region 按钮
        private void button_Show_Click(object sender, EventArgs e) //刷新按钮
        {
            Button B = (Button)sender;
            string text = B.Text;
            if (text == "显示当前日期")
            {
                DgvShow(true);
                B.Text = "显示全部";
            }
            else if(text == "显示全部")
            {
                DgvShow(false);
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
                        NewTaskDelegate task = Dt2SqlStr;
                        IAsyncResult asyncResult = task.BeginInvoke(excelObj.CellDt, null, null);
                        string result = task.EndInvoke(asyncResult);
                        DgvShow(true);

                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add("User", FormLogin.Login_Uid);
                        dict.Add("Mode", "Insert");
                        dict.Add("Detail", result);
                        HttpPost.HttpPost_Dict(FormLogin.HttpURL + "/Client/MaDuo/GetInfo", dict);

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

        private void button1_Click(object sender, EventArgs e)
        {
            SyncFromPlan();
        }

        private void SyncFromPlan()
        {
            string sqlstr = @"DELETE FROM SCHEDULE WHERE SC003 >= CONVERT(VARCHAR(20), GETDATE(), 112) AND SC039 != 'Y' 
                                INSERT INTO SCHEDULE(CREATOR, CREATE_DATE, SC001, SC002, SC003, SC004, 
                                SC005, SC006, SC007, SC008, SC009, SC010, SC011, SC012, SC013, SC014, 
                                SC015, SC016, SC017, SC018, SC019, SC020, SC021, SC022, SC023, SC024, SC025, SC026)
                                SELECT CREATOR, CREATE_DATE, SC001, SC002, SC003, SC004, SC005, SC006, SC007, SC008, 
                                SC009, SC010, SC011, SC012, SC013, SC014, SC015, SC016, SC017, SC018, SC019, SC020, 
                                SC021, SC022, SC023, SC024, SC025, SC026 
                                FROM WG_DB..SC_PLAN
                                WHERE SC023 = '生产五部'
                                AND SC003 >= CONVERT(VARCHAR(20), GETDATE(), 112)
                                UPDATE SCHEDULE SET SC038 = 'n' WHERE SC003 >= CONVERT(VARCHAR(20), GETDATE(), 112) AND SC039 != 'Y' ";
            mssql.SQLexcute(connRobot, sqlstr);

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("User", FormLogin.Login_Uid);
            dict.Add("Mode", "Insert");
            dict.Add("Detail", "");
            HttpPost.HttpPost_Dict(FormLogin.HttpURL + "/Client/MaDuo/GetInfo", dict);

            //SetBoxCode();
            MessageBox.Show("同步工作已完成", "完成");

        }

        private void SetBoxCode()
        {
            string sqlstrGet = @"SELECT SC001 FROM SCHEDULE WHERE 1=1 AND SC038 = 'y' ORDER BY KEY_ID";
            string sqlstrSet = @"UPDATE SCHEDULE SET SC038 = 'Y', SC036 = '{1}' WHERE SC001 = '{0}'";
            DataTable dt = mssql.SQLselect(connRobot, sqlstrGet);
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    string BoxCode = GetMaxBoxSizeCode(dt.Rows[rowIndex][0].ToString());
                    mssql.SQLexcute(connRobot, string.Format(sqlstrSet, dt.Rows[rowIndex][0].ToString(), BoxCode));
                }
            }
        }

        private string GetMaxBoxSizeCode(string SC001)
        {
            string returnStr = "NULL";

            return returnStr;
        }
    }
}
