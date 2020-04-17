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

        private static string connRobot = Global_Const.strConnection_ROBOT_TEST;
        private static string connComfort = Global_Const.strConnection_COMFORT;

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
                        HttpPost.HttpPost_Dict(FormLogin.infObj.httpHost + "/Client/MaDuo/GetInfo", dict);

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

        private void btnSyncFromPlan_Click(object sender, EventArgs e)
        {
            SyncFromPlan();
        }
        #endregion

        #region 导入与数据显示
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
                                         + " SC013 AS 排程数量, "
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
                                         + " (CASE WHEN SC036 = 'NULL' THEN '未维护' ELSE SC036 END) AS 纸箱尺寸码 "
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

        #region 获取ERP订单信息,直接从WG_DB里面取数据
        private void SyncFromPlan()
        {
            label1.Visible = true;
            string sqlstr = @"DELETE FROM SCHEDULE WHERE SC003 >= CONVERT(VARCHAR(20), GETDATE(), 112) AND SC039 != 'Y' 
                                INSERT INTO SCHEDULE(CREATOR, CREATE_DATE, SC001, SC002, SC003, SC004, 
                                SC005, SC006, SC007, SC008, SC009, SC010, SC011, SC012, SC013, SC014, 
                                SC015, SC016, SC017, SC018, SC019, SC020, SC021, SC022, SC023, SC024, SC025, SC026)
                                SELECT CREATOR, CREATE_DATE, SC001, SC002, SC003, SC004, SC005, SC006, SC007, SC008, 
                                SC009, SC010, SC011, SC012, SC013, SC014, SC015, SC016, SC017, SC018, SC019, SC020, 
                                SC021, SC022, SC023, SC024, SC025, SC026 
                                FROM WG_DB..SC_PLAN
                                WHERE SC023 IN ('生产五部', '生产二部') 
                                AND SC003 >= CONVERT(VARCHAR(20), GETDATE(), 112)
                                UPDATE SCHEDULE SET SC038 = 'n' WHERE SC003 >= CONVERT(VARCHAR(20), GETDATE(), 112) AND SC039 != 'Y' ";
            mssql.SQLexcute(connRobot, sqlstr);

            SetTypeCode();

            SetBoxCode();

            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add("User", FormLogin.Login_Uid);
            //dict.Add("Mode", "Insert");
            //dict.Add("Detail", "");
            //HttpPost.HttpPost_Dict(FormLogin.HttpURL + "/Client/MaDuo/GetInfo", dict);

            label1.Visible = false;
            DgvShow(true);
            MessageBox.Show("同步工作已完成", "完成");
        }
        #endregion

        #region 获取纸箱编码
        private void SetBoxCode()
        {
            string sqlstrGet = @"SELECT SC001 FROM SCHEDULE WHERE 1=1 AND SC038 = 'y' ORDER BY KEY_ID";
            string sqlstrSet = @"UPDATE SCHEDULE SET SC038 = 'Y', SC036 = (CASE WHEN BC IS NULL THEN 'NULL' ELSE BC END)
                                    FROM SCHEDULE 
                                    LEFT JOIN (
                                    SELECT SC001 AS SC1, BoxCode AS BC FROM SCHEDULE 
                                    LEFT JOIN BoxSizeCode ON BoxSize = '{1}'
                                    ) AS A ON SC1 = SC001 WHERE SC001 = '{0}'  ";
            DataTable dt = mssql.SQLselect(connRobot, sqlstrGet);
            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    string BoxSize = GetMaxBoxSizeCode(dt.Rows[rowIndex][0].ToString());
                    mssql.SQLexcute(connRobot, string.Format(sqlstrSet, dt.Rows[rowIndex][0].ToString(), BoxSize));
                }
            }
        }

        private string GetMaxBoxSizeCode(string SC001)
        {
            string returnStr = "";

            string sqlstrGet = GetBoxNameStr();
            string sqlstr = @"SELECT DISTINCT TB013 FROM MOCTB INNER JOIN MOCTA ON TA001 = TB001 AND TA002= TB002 WHERE 1=1 {0} 
                                AND RTRIM(TA076) + '-' + RTRIM(TA077) + '-' + RTRIM(TA078) = '{1}'";

            string k = string.Format(sqlstr, sqlstrGet, SC001);
            DataTable dt = mssql.SQLselect(connComfort, string.Format(sqlstr, sqlstrGet, SC001));

            returnStr = GetBoxMaxSize(dt);

            return returnStr;
        }

        private string GetBoxNameStr()
        {
            string sqlstr = @"SELECT DISTINCT BoxName, Spec FROM BoxNameCode WHERE Valid = 1 ";

            string returnStr = null;
            DataTable dt = mssql.SQLselect(connRobot, sqlstr);
            if(dt != null)
            {
                for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    if(returnStr == null)
                    {
                        returnStr = string.Format(" (TB012 LIKE '%{0}%' AND TB006 LIKE '%{1}%' )", dt.Rows[rowIndex][0].ToString(), dt.Rows[rowIndex][1].ToString());
                    }
                    else
                    {
                        returnStr += string.Format(" OR (TB012 LIKE '%{0}%' AND TB006 LIKE '%{1}%' ) ", dt.Rows[rowIndex][0].ToString(), dt.Rows[rowIndex][1].ToString());
                    }
                }
                return string.Format( "AND ({0})", returnStr);
            }
            else
            {
                return " AND (TB012 LIKE '%纸箱%' AND TB006 = '0801') ";
            }
        }

        private string GetBoxMaxSize(DataTable dt)
        {
            DataTable dtTmp = new DataTable();
            string returnStr = null;
            dtTmp.Columns.Add("L", typeof(Int32));
            dtTmp.Columns.Add("W", typeof(Int32));
            dtTmp.Columns.Add("H", typeof(Int32));
            dtTmp.Columns.Add("V", typeof(Int32));

            if (dt != null)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    //取出规格中尺寸部分
                    string guigeStr = dt.Rows[rowIndex][0].ToString();
                    var guigeStrTmp = guigeStr.Split('/');
                    for (int i = 0; i < guigeStrTmp.Length; i++)
                    {
                        if(Normal.GetSubstringCount(guigeStrTmp[i], "*") == 2)
                        {
                            try
                            {
                                //取出尺寸Str中各个数字并求出体积
                                string sizeStr = guigeStrTmp[i];
                                var sizeTmp = sizeStr.Split('*');

                                string LStr = sizeTmp[0].Split('(')[0].Split('（')[0];
                                int L = int.Parse(LStr);
                                string WStr = sizeTmp[1].Split('(')[0].Split('（')[0];
                                int W = int.Parse(WStr);
                                string HStr = sizeTmp[2].Split('(')[0].Split('（')[0];
                                int H = int.Parse(HStr);
                                int V = L * W * H;

                                DataRow dtTmpRow = dtTmp.NewRow();
                                dtTmpRow["L"] = L;
                                dtTmpRow["W"] = W;
                                dtTmpRow["H"] = H;
                                dtTmpRow["V"] = V;
                                dtTmp.Rows.Add(dtTmpRow);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }

                //已生成临时数据，体积已求出
                if (dtTmp != null)
                {
                    int V = 0;
                    for (int rowIndex = 0; rowIndex < dtTmp.Rows.Count; rowIndex++)
                    {
                        //MessageBox.Show(dtTmp.Rows[rowIndex][0].ToString() + "*" + dtTmp.Rows[rowIndex][1].ToString() + "*" + dtTmp.Rows[rowIndex][2].ToString());
                        if (int.Parse(dtTmp.Rows[rowIndex][3].ToString()) > V)
                        {
                            V = int.Parse(dtTmp.Rows[rowIndex][3].ToString());
                            returnStr = dtTmp.Rows[rowIndex][0].ToString() + "*" + dtTmp.Rows[rowIndex][1].ToString() + "*" + dtTmp.Rows[rowIndex][2].ToString();
                        }
                    }
                    return returnStr;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 获取订单类别编码
        private void SetTypeCode()
        {
            //SetOrderInType(); //更新内销-停用
            //SetOrderOutType(); //更新外销-停用
            SetTypeCodeByProc(); //使用存储过程来更新信息
        }

        private void SetTypeCodeByProc()
        {
            mssql.SQLexcute(connRobot, "EXEC dbo.SplitCodeUpdate ");
        }

        private void SetOrderInType()
        {
            string sqlstr = @"UPDATE SCHEDULE SET SC038 = 'y', SC037 = SC37
                            FROM(
                            SELECT SC001 AS SC1, (CASE  WHEN SC017 LIKE '%无码%' THEN 'C'  WHEN SC017 LIKE '%京东条码%' THEN 'D'  WHEN SC017 LIKE '%POP条码%' THEN 'E'  ELSE 'A' END ) AS SC37 FROM SCHEDULE 
                            WHERE SC002 = '内销' AND SC038 = 'n' 
                            ) AS A WHERE SC001 = SC1";
            mssql.SQLexcute(connRobot, string.Format(sqlstr, GetOrderInTypeSqlStr()));
        }

        private string GetOrderInTypeSqlStr()
        {
            string returnStrTmp = "";
            string returnStr = "(CASE {0} ELSE 'A' END )";
            DataTable returnStrDt = mssql.SQLselect(connRobot, @"SELECT PO_Type, TypeCode FROM SplitTypeCode WHERE Valid = 1 AND PO_Class = '内销' AND PO_Type != '内销' ORDER BY K_ID");
            if(returnStrDt != null)
            {
                string returnStrTemp2 = " WHEN SC017 LIKE '%{0}%' THEN '{1}' ";
                for(int rowIndex = 0; rowIndex < returnStrDt.Rows.Count; rowIndex++)
                {
                    returnStrTmp += string.Format(returnStrTemp2, returnStrDt.Rows[rowIndex][0].ToString(), returnStrDt.Rows[rowIndex][1].ToString());
                }
                returnStr = string.Format(returnStr, returnStrTmp);
                return returnStr;
            }
            else
            {
                return " 'A'";
            }
        }

        private void SetOrderOutType()
        {
            string sqlstr = @"UPDATE SCHEDULE SET SC038 = 'y', SC037 = TypeCode
                                FROM (
                                SELECT SC001 AS SC1, TypeCode FROM SCHEDULE 
                                INNER JOIN SplitTypeCode ON PO_Type = SUBSTRING(SC001, 1, 4) 
                                WHERE SC002 = '外销' AND SC038 = 'n' AND PO_Class = '外销' 
                                ) AS A WHERE SC1 = SC001";
            mssql.SQLexcute(connRobot, sqlstr);

            string sqlstr2 = @" SELECT DISTINCT SUBSTRING(SC001, 1, 4) FROM SCHEDULE WHERE SC002 = '外销' AND SC038 = 'n'";
            DataTable dt = mssql.SQLselect(connRobot, sqlstr2);
            if (dt != null)
            {
                GetOrderOutTypeCheck(dt);
            }
        }

        private void GetOrderOutTypeCheck(DataTable dt)
        {
            if( dt != null)
            {
                for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    string num = mssql.SQLselect(connRobot, " Select MAX(K_ID) +1 FROM SplitTypeCode").Rows[0][0].ToString();
                    AddOrderType("外销", dt.Rows[rowIndex][0].ToString(), Num2Char(num));
                }
                SetOrderOutType();
            }
        }

        private void AddOrderType(string PO_Class, string PO_Type, string TypeCode)
        {
            string sqlstr = @"INSERT INTO SplitTypeCode(PO_Class, PO_Type, TypeCode, Valid) 
                              VALUES ('{0}', '{1}', '{2}', 1) ";
            mssql.SQLexcute(connRobot, string.Format(sqlstr, PO_Class, PO_Type, TypeCode));
        }

        #region Num2Char
        private static string ChangeNum2Char(int num)
        {
            string returnStr = "";
            byte[] array = new byte[1];
            array[0] = (byte)(Convert.ToInt32(num + 64));//ASCII码强制转换二进制
            returnStr = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));

            if (returnStr == "@")
            {
                returnStr = "Z";
            }
            return returnStr;
        }

        public static string Num2Char(string num2 = null)
        {
            string returnStr = "";

            if (num2 == null)
            {
                return null;
            }
            else
            {
                int num = int.Parse(num2);
                int shang = num / 26;
                int yu = num % 26;

                if (shang == 1 && yu == 0)
                {
                    returnStr += ChangeNum2Char(26);
                }
                else if (shang != 0)
                {
                    returnStr += Num2Char(shang.ToString());
                    returnStr += ChangeNum2Char(yu);
                }
                else
                {
                    returnStr += ChangeNum2Char(yu);
                }
                return returnStr;
            }
        }
        #endregion

        #endregion
    }
}
