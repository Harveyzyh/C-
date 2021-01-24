using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HarveyZ.财务
{
    public partial class 工单明细生产导出 : Form
    {
        private string conn = FormLogin.infObj.connYF;
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public 工单明细生产导出(string text = "")
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_Start(dateTimePicker1, dateTimePicker2);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_End(dateTimePicker1, dateTimePicker2);
        }

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
            string sqlStr = @"SELECT rtrim(TA001) 单别,rtrim(TA002) 单号,rtrim(TA003) 日期,rtrim(TA006) 产品品号,rtrim(TB003) 材料品号,
		                        rtrim(TB012) 材料品名,rtrim(TB013) 规格,rtrim(TB006) 工艺,rtrim(TB004) 需领用量,rtrim(TB005) 已领用量,rtrim(MW002) 工艺名称,
		                        rtrim(TB028) 配置方案,rtrim(TA026) 订单单别,rtrim(TA027) 订单单号,rtrim(TA028) 订单序号 FROM MOCTA 
	                        INNER JOIN MOCTB ON TA001=TB001 AND TA002=TB002		
	                        LEFT JOIN CMSMW ON TB006=MW001
	                        where TA003 BETWEEN '{0}' AND '{1}'
                                ";
            DataTable dt = mssql.SQLselect(conn, string.Format(sqlStr, dateTimePicker1.Value.ToString("yyyyMMdd"), dateTimePicker2.Value.ToString("yyyyMMdd")));
            dt.TableName = "工单明细";
            return dt;
        }
        #endregion
    }
}
