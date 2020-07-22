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

        private delegate void NewTaskDelegate(); //任务代理
        private NewTaskDelegate dc1 = new NewTaskDelegate(MdHandelClass.GetBoxCode);

        private static DataTable showDt = new DataTable();

        private static Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;

        private static string connMD = FormLogin.infObj.connMD;
        private static string connERP = FormLogin.infObj.connYF;

        #endregion

        #region 窗体设计
        public 码垛线排程导入(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
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

        private void btnSyncFromPlan_Click(object sender, EventArgs e)
        {
            SyncFromPlan();

            DgvShow(true);

            MessageBox.Show("同步工作已完成", "完成");
        }

        private void btnSyncBoxCode_Click(object sender, EventArgs e)
        {
            MdHandelClass.SetBefore();
            SetTypeCode();
            //MdHandelClass.GetBoxCode();
            dc1.Invoke();
            DgvShow(true);
            MessageBox.Show("更新完成", "提示");
        }

        private void btnOutput_Click(object sender, EventArgs e)
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
        #endregion

        #region 导入与数据显示
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
                                         + " (CASE WHEN SC036 = 'NULL' THEN '未维护' ELSE SC036 END) AS 纸箱尺寸码,"
                                         + " SC040 AS 纸箱尺寸 "
                                         + " FROM SCHEDULE "
                                         + " LEFT JOIN BoxSizeCode ON SC036 = BoxCode "
                                         + " LEFT JOIN SplitTypeCode ON TypeCode = SC037 "
                                         + " WHERE 1=1 AND SC039 = 'N'";
            if (show)
            {
                show_sqlstr += " AND SC003 >= CONVERT(VARCHAR(20), GETDATE(), 112)";
            }
            show_sqlstr += " ORDER BY SC003, (CASE SC026 WHEN 'Y' THEN '是' ELSE '' END ) DESC, SC001 ";

            if (showDt != null)
            {
                showDt.Clear();
            }
            showDt = mssql.SQLselect(connMD, show_sqlstr);

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
                DgvOpt.SetRowBackColor(DgvMain);
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
            mssql.SQLexcute(connMD, "EXEC dbo.MdPlanInsert ");

            SetTypeCode();

            SetBoxCode();
        }
        #endregion

        #region 获取纸箱编码
        private void SetBoxCode()
        {
            //SetBoxCodeByProc(); //使用存储过程来更新信息
            dc1.Invoke();
        }

        private void SetBoxCodeByProc()
        {
            mssql.SQLexcute(connMD, "EXEC dbo.BoxCodeUpdate ");
        }
        #endregion

        #region 获取订单类别编码
        private void SetTypeCode()
        {
            SetTypeCodeByProc(); //使用存储过程来更新信息
        }

        private void SetTypeCodeByProc()
        {
            mssql.SQLexcute(connMD, "EXEC dbo.SplitCodeUpdate ");
        }
        #endregion

        #region 其他处理类
        private static class MdHandelClass
        {
            public static void SetBefore()
            {
                string sqlstr = @"UPDATE dbo.SCHEDULE SET SC038 = 'n' WHERE SC003 >= CONVERT(VARCHAR(20), DATEADD(DAY, -1, GETDATE()), 112) AND SC039 != 'Y'";
                mssql.SQLexcute(connMD, sqlstr);
            }

            /// <summary>
            /// 获取纸箱编码
            /// </summary>
            /// <param name="beforeDay">前置天数</param>
            public static void GetBoxCode()
            {
                string sc001 = "";
                string sc040 = "";

                string boxNameStr = "";

                string sqlstrBoxName = "SELECT BoxName, Spec from BoxNameCode where Valid = 1 order by Spec";

                string sqlstrList = @"SELECT SC001 FROM dbo.SCHEDULE(NOLOCK) WHERE 1=1 AND SC039 = 'N' AND SC038 = 'y' AND SC039 != 'Y' ORDER BY SC003, SC001";

                string sqlstrMocta = @"SELECT TA001 FROM dbo.MOCTA(NOLOCK) INNER JOIN dbo.MOCTB(NOLOCK) ON TA001 = TB001 AND TA002 = TB002 WHERE RTRIM(TA026)+'-'+RTRIM(TA027)+'-'+RTRIM(TA028) = '{0}'";

                string sqlstrMoctb = @"SELECT TOP 1 ISNULL(MBU01, '') FROM dbo.MOCTA(NOLOCK) 
					                    INNER JOIN dbo.MOCTB(NOLOCK) ON TA001 = TB001 AND TA002 = TB002 
                                        LEFT JOIN dbo.INVMB(NOLOCK) ON MB001 = TB003 
					                    WHERE 1=1 AND MBU02 IS NOT NULL 
					                    AND RTRIM(TA026)+'-'+RTRIM(TA027)+'-'+RTRIM(TA028) = '{0}' 
                                        AND ({1})  
					                    ORDER BY MBU02 DESC ";

                string sqlstrUdtSc040 = @"UPDATE ROBOT_TEST.dbo.SCHEDULE SET SC040 = '{1}', SC038 = 'Y' WHERE SC001 = '{0}' ";

                string sqlstrUdtSc036 = @"UPDATE ROBOT_TEST.dbo.SCHEDULE SET SC036 = ISNULL(BoxCode, 'NULL') FROM dbo.SCHEDULE LEFT JOIN ROBOT_TEST.dbo.BoxSizeCode ON BoxSize = SC040 WHERE (SC036 = 'NULL' OR SC036 = '' OR SC036 IS NULL) ";

                //构建纸箱名称的sql语句
                DataTable dtBoxName = mssql.SQLselect(connMD, sqlstrBoxName);
                if (dtBoxName != null)
                {
                    for (int rowIndex = 0; rowIndex < dtBoxName.Rows.Count; rowIndex++) {
                        if (boxNameStr == "")
                        {
                            boxNameStr = string.Format(" (TB012 LIKE '%{0}%' AND TB006 LIKE '%{1}%') ", dtBoxName.Rows[rowIndex][0].ToString(), dtBoxName.Rows[rowIndex][1].ToString());
                        }
                        else
                        {
                            boxNameStr += string.Format("OR (TB012 LIKE '%{0}%' AND TB006 LIKE '%{1}%') ", dtBoxName.Rows[rowIndex][0].ToString(), dtBoxName.Rows[rowIndex][1].ToString());
                        }
                    }
                }
                else
                {
                    boxNameStr = " 1=1 ";
                }


                DataTable dtList = mssql.SQLselect(connMD, sqlstrList);
                if (dtList != null)
                {
                    for(int rowIndex =0; rowIndex < dtList.Rows.Count; rowIndex++)
                    {
                        sc040 = "";
                        sc001 = dtList.Rows[rowIndex][0].ToString();

                        if(mssql.SQLexist(connERP, string.Format(sqlstrMocta, sc001)))
                        {
                            DataTable dt = mssql.SQLselect(connERP, string.Format(sqlstrMoctb, sc001, boxNameStr));
                            if (dt != null)
                            {
                                sc040 = dt.Rows[0][0].ToString();
                            }
                            else
                            {
                                sc040 = "不存在纸箱";
                            }
                        }
                        else
                        {
                            sc040 = "不存在工单";
                        }

                        mssql.SQLexcute(connMD, string.Format(sqlstrUdtSc040, sc001, sc040));
                    }
                    mssql.SQLexcute(connMD, sqlstrUdtSc036);
                }
            }


            /// <summary>
            /// 获取订单分类码
            /// </summary>
            public static void GetSplitCOde()
            {

            }
        }
        #endregion
    }
}
