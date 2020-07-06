using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HarveyZ;
using System.Collections.Generic;
using System.Timers;

namespace 联友生产辅助工具.生管排程
{
    public partial class 生产电子排程 : Form
    {
        #region 静态数据设置
        private delegate string NewTaskDelegate(DataTable dttmp); //任务代理
        
        private Mssql mssql = new Mssql();

        private static string connWG = FormLogin.infObj.connWG;
        private static string connERP = FormLogin.infObj.connYF;
        #endregion

        #region 窗体设计
        public 生产电子排程()
        {
            InitializeComponent();
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            BtnOutput.Enabled = false;
            DtpEndDate.Checked = false;
            DgvShow();
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
        private bool CheckDt(DataTable dt)
        {
            if(dt.Columns.Count != 28)
            {
                MessageBox.Show("导入文件格式有异常", "错误");
                return false;
            }

            int rowTotal = dt.Rows.Count;
            string SC001 = "", SC003 = "";
            string Msg = "";
            for (int rowIndex = 0; rowIndex < rowTotal; rowIndex++)
            {
                if (dt.Rows[rowIndex][2].ToString() == "生产单号")
                {
                    continue;
                }

                if (dt.Rows[rowIndex][13].ToString() == "") dt.Rows[rowIndex][13] = "0";

                SC001 = dt.Rows[rowIndex][2].ToString();
                SC003 = dt.Rows[rowIndex][0].ToString();

                if(Normal.GetSubstringCount(SC001, "-") != 2 || SC003 == "")
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

        private int GetNowDate()
        {
            string sqlstr = @"SELECT isnull(LEFT(dbo.f_getTime(1), 8), 0) ";
            return int.Parse(mssql.SQLselect(connERP, sqlstr).Rows[0][0].ToString());
        }

        private int GetLastDate()
        {
            //限定日期
            //获取最后天期限需要增加的天数
            int dateAdd = 365;
            string sqlstr = @"SELECT isnull(CONVERT(VARCHAR(8), DATEADD(DAY, {0}, GETDATE()), 112), 0) ";
            return int.Parse(mssql.SQLselect(connERP, string.Format(sqlstr, dateAdd)).Rows[0][0].ToString());
        }

        private string GetInsertDt(DataTable dt)//数据表转成sql语句
        {
            DataTable dtInsert = new DataTable();
            dtInsert.Columns.Add("生产单号", Type.GetType("System.String"));
            dtInsert.Columns.Add("上线日期", Type.GetType("System.String"));
            dtInsert.Columns.Add("数量", Type.GetType("System.String"));
            dtInsert.Columns.Add("测试量", Type.GetType("System.String"));
            dtInsert.Columns.Add("生产车间", Type.GetType("System.String"));
            DataRow dtRowTmp = null;
            int rowIndex = 0;
            int row_total = dt.Rows.Count;
            int col_total = dt.Columns.Count;
            string returnstr = "";

            string SC003 = "", SC001 = "";

            int SysDate = GetNowDate();
            int LastDate = GetLastDate();
            int WorkTime = 0;
            
            for (; rowIndex < row_total; rowIndex++ )
            {
                //限定日期
                try
                {
                    SC003 = dt.Rows[rowIndex][0].ToString().Replace("-", "").Replace("/", "");
                    WorkTime = int.Parse(SC003);
                    if(WorkTime < SysDate || WorkTime > LastDate)
                    {
                        continue;
                    }
                }
                catch
                {
                    continue;
                }

                if (dt.Rows[rowIndex][2].ToString() == "生产单号")
                {
                    continue;
                }
                else if (dt.Rows[rowIndex][2].ToString() != "")
                {
                    SC001 = dt.Rows[rowIndex][2].ToString();

                    SC001 = SC001.Replace("（新增）", "").Replace("（变更）", "").Replace("（更改）", "").Replace("(新增)", "").
                        Replace("(变更)", "").Replace("(更改)", "").Replace("(变更）", "").Replace("(新增）", "").Replace("(更改）", "");

                    SC001 = SC001.Split('-')[0].Trim() + '-' + SC001.Split('-')[1].Trim() + '-' + SC001.Split('-')[2].Trim();

                    dtRowTmp = dtInsert.NewRow();
                    dtRowTmp[0] = SC001;
                    dtRowTmp[1] = SC003;
                    dtRowTmp[2] = dt.Rows[rowIndex][12].ToString().Split('/')[0];
                    dtRowTmp[3] = dt.Rows[rowIndex][13].ToString().Split('/')[0];
                    dtRowTmp[4] = dt.Rows[rowIndex][22].ToString();
                    dtInsert.Rows.Add(dtRowTmp);
                }
                else continue;
            }

            if (dtInsert != null && dtInsert.Rows.Count != 0)
            {
                SqlInsertWork(WorkDate: (SysDate).ToString(), dt: dtInsert);
                UptInfo();
            }
            else
            {
                MessageBox.Show("导入异常，找不到没有可导入数据", "错误");
            }

            return returnstr;
        }

        private void SqlInsertWork(string WorkDate, DataTable dt)
        {
            string sqlstrDel = @"DELETE FROM WG_DB..SC_PLAN WHERE SC003 >= '{0}' AND SC023 = '{1}'";
            string sqlstrInsert = @"INSERT INTO WG_DB..SC_PLAN (CREATOR, CREATE_DATE, SC001, SC003, SC013, SC014, SC023) 
                                    VALUES ('{0}', (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', '')), 
                                    '{1}', '{2}', '{3}', '{4}', '{5}') ";
            mssql.SQLexcute(connWG, string.Format(sqlstrDel, WorkDate, dt.Rows[0][4].ToString()));
            for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                mssql.SQLexcute(connWG, string.Format(sqlstrInsert, FormLogin.infObj.userId, dt.Rows[rowIndex][0], dt.Rows[rowIndex][1], 
                    dt.Rows[rowIndex][2], dt.Rows[rowIndex][3], dt.Rows[rowIndex][4]));
            }
        }
        
        private void UptInfo()
        {
            string sqlstr = @"UPDATE WG_DB.dbo.SC_PLAN SET 
                                SC002 = X00, SC004 = X01, SC005 = X02, SC006 = X03, SC007 = X04, SC008 = X05, 
                                SC009 = X06, SC010 = X07, SC011 = X08, SC012 = X09, SC015 = X10, 
                                SC016 = X11, SC017 = X12, SC018 = X13, SC019 = X14, SC020 = X15, SC021 = X16, 
                                SC022 = X17, SC024 = X18, SC025 = X19, SC026 = X20, SC027 = X21, SC028 = X22 
                                FROM WG_DB.dbo.SC_PLAN
                                INNER JOIN (
	                                SELECT 
	                                RTRIM(COPTD.TD001) + '-' + RTRIM(COPTD.TD002) + '-' + RTRIM(COPTD.TD003) AS DD,
	                                (CASE WHEN TC004='0118' THEN '内销' ELSE '外销' END) AS X00,
	                                RTRIM(COPMA.MA002) AS X01,
	                                RTRIM(COPTC.TC015) AS X02,
	                                RTRIM((CASE WHEN COPTF.TF003 IS NULL THEN '' WHEN COPTF.TF003 IS NOT NULL AND COPTF.TF017 = 'Y' 
		                                THEN '指定结束'+':'+'变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 ELSE '变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 END)) AS X03,
	                                RTRIM((CASE WHEN COPTD.TD013 = '' THEN '' WHEN COPTD.TD013 IS NULL THEN '' ELSE COPTD.TD013 END)) AS X04,
	                                RTRIM((CASE WHEN COPTD.UDF03 = '' THEN '' WHEN COPTD.UDF03 IS NULL THEN '' ELSE COPTD.UDF03 END)) AS X05,
	                                RTRIM(COPTD.UDF01) AS X06,
	                                RTRIM(COPTD.TD005) AS X07,
	                                RTRIM(COPTD.UDF08) AS X08,
	                                RTRIM(COPTD.TD006) AS X09,
	                                RTRIM(COPTD.TD053) AS X10,
	                                RTRIM(COPTQ.TQ003) AS X11,
	                                RTRIM((COPTQ.UDF07+COPTD.TD020)) AS X12, 
	                                RTRIM(COPTD.TD204) AS X13, 
	                                RTRIM(COPTC.TC035) AS X14,
	                                RTRIM(CMSMV.MV002) AS X15, 
	                                RTRIM(COPTC.TC012) AS X16, 
	                                RTRIM(COPTD.TD014) AS X17,
	                                RTRIM(COPTD.UDF05) AS X18,
	                                RTRIM(COPTD.UDF10) AS X19,
	                                RTRIM((CASE WHEN COPTC.UDF09='否' THEN '' ELSE '是' END)) AS X20,
	                                SUBSTRING(COPTD.CREATE_DATE,1,14) AS X21, 
	                                RTRIM(COPTD.TD004) AS X22 
	                                FROM COMFORT.dbo.COPTD AS COPTD 
	                                Left JOIN COMFORT.dbo.COPTC AS COPTC On COPTD.TD001=COPTC.TC001 and COPTD.TD002=COPTC.TC002 
	                                Left JOIN COMFORT.dbo.COPTQ AS COPTQ On COPTD.TD053=COPTQ.TQ002 and COPTD.TD004=COPTQ.TQ001 
	                                Left JOIN COMFORT.dbo.COPMA AS COPMA On COPTC.TC004=COPMA.MA001 
	                                Left JOIN COMFORT.dbo.CMSMV AS CMSMV On COPTC.TC006=CMSMV.MV001 
	                                LEFT JOIN COMFORT.dbo.INVMB AS INVMB ON COPTD.TD004=INVMB.MB001 
	                                Left JOIN COMFORT.dbo.COPTF AS COPTF On COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104 
		                                AND COPTF.TF003 = (SELECT MAX(TF003) FROM COMFORT.dbo.COPTF  
			                                WHERE COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104) 
	                                LEFT JOIN COMFORT.dbo.CMSME AS CMSME ON CMSME.ME001 = INVMB.MB445 
                                ) AS A ON SC001 = DD
                                WHERE (SC028 IS NULL OR SC028 ='') ";
            mssql.SQLexcute(connWG, sqlstr);
        }

        private void DgvShow() //数据库资料显示到界面
        {
            List<string> dptList = new List<string>{"生产一部", "生产二部", "生产三部", "生产四部", "生产五部" };

            string sqlstrShow = @" SELECT SUBSTRING(CREATE_DATE, 1, 8) 导入日期, SC003 上线日期, SC023 生产车间, SC001 订单号, SC002 订单类型, SC010 品名, 
                                    SC012 规格, SC011 保友品名, SC013 订单数量, SC014 赠品测试量, SC015 配置方案, 
                                    SC016 配置方案描述, SC017 描述备注, SC004 客户名称, SC005 注意事项, 
                                    SC006 变更原因, SC007 出货日期, SC008 验货日期, SC009 PO#, SC018 柜型柜数, SC019 目的地, 
                                    SC024 客户编码, SC025 电商编码, SC026 急单, SC027 订单日期, SC028 品号 
                                    FROM WG_DB..SC_PLAN WHERE 1 = 1 ";
            if (TxBoxOrder.Text != "") sqlstrShow += @" AND SC001 LIKE '%" + TxBoxOrder.Text + "%' ";
            if (TxBoxName.Text != "") sqlstrShow += @" AND SC010 LIKE '%" + TxBoxName.Text + "%' ";
            if (DtpStartDate.Checked) sqlstrShow += @" AND SC003 >= '" + DtpStartDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpEndDate.Checked) sqlstrShow += @" AND SC003 <= '" + DtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            if (dptList.Contains(CmBoxDptType.Text)) sqlstrShow += @" AND SC023 = '" + CmBoxDptType.Text + "' ";
            sqlstrShow += " ORDER BY SUBSTRING(CREATE_DATE, 1, 8), SC003, SC001 ";
            DataTable showDt = mssql.SQLselect(connWG, sqlstrShow);

            if (showDt != null)
            {
                DtOpt.DtDateFormat(showDt, "日期");
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvMain.Columns[3].Width = 180;
                BtnOutput.Enabled = true;
            }
            else
            {
                DgvMain.DataSource = null;
                BtnOutput.Enabled = false;
            }
        }

        private void SetInputDtDpt(string Dpt, DataTable dt)
        {
            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                if (dt.Rows[rowIndex][2].ToString() == "生产单号")
                {
                    continue;
                }
                dt.Rows[rowIndex][22] = Dpt;
            }
        }
        #endregion

        #region 按钮
        private void BtnShow_Click(object sender, EventArgs e) //刷新按钮
        {
            DgvShow();
        }

        private void BtnInput_Click(object sender, EventArgs e)
        {
            Form formInput = new 生管排程导入导出部门选择("导入");
            formInput.ShowDialog();
            string Dpt = 生管排程导入导出部门选择.Dpt;
            if (Dpt != null)
            {
                Form frm = new 生产排程导入中();
                try
                {
                    Excel excel = new Excel();
                    Excel.Excel_Base excelObj = new Excel.Excel_Base();

                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    openFileDialog.Filter = "Excel文件|*.xlsx;*.xls";
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.FilterIndex = 1;
                   


                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        frm.Show();

                        excelObj.filePath = Path.GetDirectoryName(openFileDialog.FileName);
                        excelObj.fileName = Path.GetFileName(openFileDialog.FileName);
                        excelObj.isWrite = false;
                        excelObj.isTitleRow = true;

                        excel.ExcelOpt(excelObj);

                        if (CheckDt(excelObj.cellDt))
                        {
                            SetInputDtDpt(Dpt, excelObj.cellDt);
                            GetInsertDt(excelObj.cellDt);
                            
                            MessageBox.Show("导入成功", "提示");
                            DgvShow();
                        }
                    }
                }
                catch(Exception es)
                {
                    MessageBox.Show(es.ToString());
                }
                finally
                {
                    frm.Dispose();
                }
            }
        }

        private void BtnOutput_Click(object sender, EventArgs e)
        {
            Excel.Excel_Base excelObj = new Excel.Excel_Base();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Excel 2007|*.xlsx";
            saveFileDialog.FileName = "生产排程导出_" + DateTime.Now.ToString("yyyy-MM-dd");
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    excelObj.filePath = Path.GetDirectoryName(saveFileDialog.FileName);
                    excelObj.fileName = Path.GetFileName(saveFileDialog.FileName);
                    excelObj.isWrite = true;

                    excelObj.cellDt = (DataTable)DgvMain.DataSource;

                    Excel excel = new Excel();
                    excel.ExcelOpt(excelObj);
                    MessageBox.Show("Excel导出成功！", "提示");
                }
                catch (IOException)
                {
                    MessageBox.Show("文件保存失败,请确保该文件没被打开！", "错误");
                }
            }
        }
        #endregion
    }
}
