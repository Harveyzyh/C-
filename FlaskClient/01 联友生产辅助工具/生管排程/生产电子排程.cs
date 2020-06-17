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

        private static string Conn_WG_DB = FormLogin.infObj.connMD;
        private static string Conn_ERP = FormLogin.infObj.connYF;
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

            int SysTime = int.Parse(Normal.GetDbSysTime("Sort"));
            int WorkTime = 0;
            
            for (; rowIndex < row_total; rowIndex++ )
            {
                //限定日期
                try
                {
                    SC003 = dt.Rows[rowIndex][0].ToString().Replace("-", "");
                    WorkTime = int.Parse(SC003);
                    if(WorkTime < SysTime || WorkTime > (int.Parse(DateTime.Now.AddDays(15).ToString("yyyyMMdd"))))
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
                    SC001 = SC001.Split('-')[0].Trim() + '-' + SC001.Split('-')[1].Trim() + '-' + SC001.Split('-')[2].Trim();
                    SC001 = SC001.Replace("（新增）", "").Replace("（变更）", "").Replace("（更改）", "").Replace("(新增)", "").Replace("(变更)", "").Replace("(更改)", "");

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
                SqlInsertWork(WorkDate: (SysTime).ToString(), dt: dtInsert);
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
            string sqlstrDel = @"DELETE FROM SC_PLAN WHERE SC003 >= '{0}' AND SC023 = '{1}'";
            string sqlstrInsert = @"INSERT INTO SC_PLAN (CREATOR, CREATE_DATE, SC001, SC003, SC013, SC014, SC023) 
                                    VALUES ('{0}', (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', '')), 
                                    '{1}', '{2}', '{3}', '{4}', '{5}') ";
            mssql.SQLexcute(Conn_WG_DB, string.Format(sqlstrDel, WorkDate, dt.Rows[0][4].ToString()));
            for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                mssql.SQLexcute(Conn_WG_DB, string.Format(sqlstrInsert, FormLogin.infObj.userId, dt.Rows[rowIndex][0], dt.Rows[rowIndex][1], 
                    dt.Rows[rowIndex][2], dt.Rows[rowIndex][3], dt.Rows[rowIndex][4]));
            }
        }
        
        private void UptInfo()
        {
            string sqlstrFindErp = @" SELECT 
                                        (CASE WHEN TC004='0118' THEN '内销' ELSE '外销' END) AS 订单类型,
                                        RTRIM(COPMA.MA002) AS 客户名称,
                                        RTRIM(COPTC.TC015) AS 注意事项,
                                        RTRIM((CASE WHEN COPTF.TF003 IS NULL THEN '' WHEN COPTF.TF003 IS NOT NULL AND COPTF.TF017 = 'Y' 
	                                        THEN '指定结束'+':'+'变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 ELSE '变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 END)) AS 订单变更原因,
                                        RTRIM((CASE WHEN COPTD.TD013 = '' THEN '' WHEN COPTD.TD013 IS NULL THEN '' ELSE COPTD.TD013 END)) AS 出货日,
                                        RTRIM((CASE WHEN COPTD.UDF03 = '' THEN '' WHEN COPTD.UDF03 IS NULL THEN '' ELSE COPTD.UDF03 END)) AS 验货日,
                                        RTRIM(COPTD.UDF01) AS PO#,
                                        RTRIM(COPTD.TD005) AS 品名,
                                        RTRIM(COPTD.UDF08) AS 保友品名,
                                        RTRIM(COPTD.TD006) AS 规格,
                                        RTRIM(COPTD.TD053) AS 配置方案,
                                        RTRIM(COPTQ.TQ003) AS 配置方案描述,
                                        RTRIM((COPTQ.UDF07+COPTD.TD020)) AS 描述备注, 
                                        RTRIM(COPTD.TD204) AS 柜型柜数, 
                                        RTRIM(COPTC.TC035) AS 目的地,
                                        RTRIM(CMSMV.MV002) AS 业务员, 
                                        RTRIM(COPTC.TC012) AS 客户单号, 
                                        RTRIM(COPTD.TD014) AS 客户品号,
                                        RTRIM(COPTD.UDF05) AS 客户编码,
                                        RTRIM(COPTD.UDF10) AS 电商代码,
                                        RTRIM((CASE WHEN COPTC.UDF09='否' THEN '' ELSE '是' END)) AS 急单,
                                        SUBSTRING(COPTD.CREATE_DATE,1,14) AS 录单日期, 
                                        RTRIM(COPTD.TD004) AS 品号 

                                        FROM COPTD AS COPTD 
                                        Left JOIN COPTC AS COPTC On COPTD.TD001=COPTC.TC001 and COPTD.TD002=COPTC.TC002 
                                        Left JOIN COPTQ AS COPTQ On COPTD.TD053=COPTQ.TQ002 and COPTD.TD004=COPTQ.TQ001 
                                        Left JOIN COPMA AS COPMA On COPTC.TC004=COPMA.MA001 
                                        Left JOIN CMSMV AS CMSMV On COPTC.TC006=CMSMV.MV001 
                                        LEFT JOIN INVMB AS INVMB ON COPTD.TD004=INVMB.MB001 
                                        Left JOIN COPTF AS COPTF On COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104 
	                                        AND COPTF.TF003 = (SELECT MAX(TF003) FROM COPTF 
		                                        WHERE COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104) 
                                        LEFT JOIN CMSME AS CMSME ON CMSME.ME001 = INVMB.MB445 
                                        WHERE RTRIM(COPTD.TD001) + '-' + RTRIM(COPTD.TD002) + '-' + RTRIM(COPTD.TD003) = '{0}' ";

            string sqlstrFindWg = @" SELECT SC001 FROM SC_PLAN WHERE (SC028 IS NULL OR SC028 ='') ";
            string sqlstrUpt = @" UPDATE SC_PLAN SET SC002 = '{1}', SC004 = '{2}', SC005 = '{3}', SC006 = '{4}', SC007 = '{5}', SC008 = '{6}', 
                                    SC009 = '{7}', SC010 = '{8}', SC011 = '{9}', SC012 = '{10}', SC015 = '{11}', 
                                    SC016 = '{12}', SC017 = '{13}', SC018 = '{14}', SC019 = '{15}', SC020 = '{16}', SC021 = '{17}', 
                                    SC022 = '{18}', SC024 = '{19}', SC025 = '{20}', SC026 = '{21}', SC027 = '{22}', SC028 = '{23}' 
                                    WHERE SC001 = '{0}' ";
            DataTable dtWg = mssql.SQLselect(Conn_WG_DB, sqlstrFindWg);
            if(dtWg != null)
            {
                for(int rowIndex = 0; rowIndex < dtWg.Rows.Count; rowIndex++)
                {
                    DataTable dtErp = mssql.SQLselect(Conn_ERP, string.Format(sqlstrFindErp, dtWg.Rows[rowIndex][0].ToString()));
                    if(dtErp != null)
                    {
                        mssql.SQLexcute(Conn_WG_DB, string.Format(sqlstrUpt, dtWg.Rows[rowIndex][0].ToString(), 
                            dtErp.Rows[0][0].ToString().Replace("'", "''"), dtErp.Rows[0][1].ToString().Replace("'", "''"), 
                            dtErp.Rows[0][2].ToString().Replace("'", "''"), dtErp.Rows[0][3].ToString().Replace("'", "''"), 
                            dtErp.Rows[0][4].ToString().Replace("'", "''"), dtErp.Rows[0][5].ToString().Replace("'", "''"), 
                            dtErp.Rows[0][6].ToString().Replace("'", "''"), dtErp.Rows[0][7].ToString().Replace("'", "''"),
                            dtErp.Rows[0][8].ToString().Replace("'", "''"), dtErp.Rows[0][9].ToString().Replace("'", "''"), 
                            dtErp.Rows[0][10].ToString().Replace("'", "''"), dtErp.Rows[0][11].ToString().Replace("'", "''"),
                            dtErp.Rows[0][12].ToString().Replace("'", "''"), dtErp.Rows[0][13].ToString().Replace("'", "''"), 
                            dtErp.Rows[0][14].ToString().Replace("'", "''"), dtErp.Rows[0][15].ToString().Replace("'", "''"),
                            dtErp.Rows[0][16].ToString().Replace("'", "''"), dtErp.Rows[0][17].ToString().Replace("'", "''"), 
                            dtErp.Rows[0][18].ToString().Replace("'", "''"), dtErp.Rows[0][19].ToString().Replace("'", "''"),
                            dtErp.Rows[0][20].ToString().Replace("'", "''"), dtErp.Rows[0][21].ToString().Replace("'", "''"), 
                            dtErp.Rows[0][22].ToString().Replace("'", "''")));
                    }
                }
            }
        }

        private void DgvShow() //数据库资料显示到界面
        {
            List<string> dptList = new List<string>{"生产一部", "生产二部", "生产三部", "生产四部", "生产五部" };

            string sqlstrShow = @" SELECT SUBSTRING(CREATE_DATE, 1, 8) 导入日期, SC003 上线日期, SC023 生产车间, SC001 订单号, SC002 订单类型, SC010 品名, 
                                    SC012 规格, SC011 保友品名, SC013 订单数量, SC014 赠品测试量, SC015 配置方案, 
                                    SC016 配置方案描述, SC017 描述备注, SC004 客户名称, SC005 注意事项, 
                                    SC006 变更原因, SC007 出货日期, SC008 验货日期, SC009 PO#, SC018 柜型柜数, SC019 目的地, 
                                    SC024 客户编码, SC025 电商编码, SC026 急单, SC027 订单日期, SC028 品号 
                                    FROM SC_PLAN WHERE 1 = 1 ";
            if (TxBoxOrder.Text != "") sqlstrShow += @" AND SC001 LIKE '%" + TxBoxOrder.Text + "%' ";
            if (TxBoxName.Text != "") sqlstrShow += @" AND SC010 LIKE '%" + TxBoxName.Text + "%' ";
            if (DtpStartDate.Checked) sqlstrShow += @" AND SC003 >= '" + DtpStartDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpEndDate.Checked) sqlstrShow += @" AND SC003 <= '" + DtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            if (dptList.Contains(CmBoxDptType.Text)) sqlstrShow += @" AND SC023 = '" + CmBoxDptType.Text + "' ";
            sqlstrShow += " ORDER BY SUBSTRING(CREATE_DATE, 1, 8), SC003, SC001 ";
            DataTable showDt = mssql.SQLselect(Conn_WG_DB, sqlstrShow);

            if (showDt != null)
            {
                DtOpt.DtDateFormat(showDt, "日期");
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowColor(DgvMain);
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

                        excelObj.FilePath = Path.GetDirectoryName(openFileDialog.FileName);
                        excelObj.FileName = Path.GetFileName(openFileDialog.FileName);
                        excelObj.IsWrite = false;
                        excelObj.IsTitleRow = true;

                        excel.ExcelOpt(excelObj);

                        if (CheckDt(excelObj.CellDt))
                        {
                            SetInputDtDpt(Dpt, excelObj.CellDt);
                            GetInsertDt(excelObj.CellDt);
                            
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
                    excelObj.FilePath = Path.GetDirectoryName(saveFileDialog.FileName);
                    excelObj.FileName = Path.GetFileName(saveFileDialog.FileName);
                    excelObj.IsWrite = true;

                    excelObj.CellDt = (DataTable)DgvMain.DataSource;

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
