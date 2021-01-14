using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HarveyZ.财务
{
    public partial class 领退料明细生产导出 : Form
    {
        private string conn = FormLogin.infObj.connYF;
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public 领退料明细生产导出(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {

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
        }
        #endregion

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();

            DataSet ds = new DataSet();
            GetData(ds);

            excelObj.dataSet = ds;
            excelObj.defauleFileName = "领退料明细(生产)导出" + DateTime.Now.ToString("yyyy-MM-dd");
            excelObj.isWrite = true;

            if (excel.ExcelOpt(excelObj))
            {
                if (excelObj.status)
                {
                    Msg.Show("Excel导出成功！");
                }
                else
                {
                    Msg.Show(excelObj.msg, "错误");
                }
            }
        }

        private void GetData(DataSet ds)
        {
            ds.Tables.Add(GetData1());
        }

        #region sql脚本内容
        //品号信息
        private DataTable GetData1()
        {
            string sqlStr = @"SELECT RTRIM(LA001) 品号, RTRIM(MB002) 品名, RTRIM(MB003) 规格, LA004 日期, LA006 单别, RTRIM(LA007) 单号, LA008 序号, RTRIM(LA009) 仓库, 
                                CONVERT(FLOAT, LA011) 单据交易库存数量, CONVERT(FLOAT, LA012) 单据单位成本, LA013 金额, LA017 金额材料, 
                                RTRIM(LA016) 批号, RTRIM(PURMA.MA002) 供应商, RTRIM(LA024) 对象, 
                                RTRIM(TC001) + '-' + RTRIM(TC002) + '-' + RTRIM(TD003) 订单号, TC004 订单客户编号, COPMA.MA002 客户名称
                                FROM INVLA
                                INNER JOIN INVMB ON MB001 = LA001
                                INNER JOIN PURMA ON PURMA.MA001 = LA016
                                LEFT JOIN MOCTA ON RTRIM(TA001) + '-' + RTRIM(TA002) = LA024
                                LEFT JOIN COPTC ON TA026 = TC001 AND RTRIM(TA027) = RTRIM(TC002) 
                                LEFT JOIN COPTD ON TA026 = TD001 AND RTRIM(TA027) = RTRIM(TD002) AND TA028 = TD003
                                LEFT JOIN COPMA ON COPMA.MA001 = TC004
                                WHERE 1=1
                                AND LA004 BETWEEN '{0}' AND '{1}'
                                AND (LA006 LIKE '5%' OR LA006 LIKE '1%')
                                ORDER BY LA004, LA006, LA007, LA008
                                ";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlStr, dateTimePicker1.Value.ToString("yyyyMMdd"), dateTimePicker2.Value.ToString("yyyyMMdd")));
            dt.TableName = "领退料明细";
            return dt;
        }
        #endregion
    }
}
