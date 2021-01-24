using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HarveyZ.财务
{
    public partial class 品号信息税务导出 : Form
    {
        private string connSW = FormLogin.infObj.connSW;
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public 品号信息税务导出(string text = "")
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
            excelObj.defauleFileName = "品号信息(税务)" + DateTime.Now.ToString("yyyy-MM-dd");
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
            string sqlStr = @"SELECT 
                                RTRIM(MB001) AS 品号, 
                                RTRIM(MB002) AS 品名, 
                                RTRIM(MB003) AS 规格,    
                                RTRIM(MB004) AS 单位, 
                                CAST(MB064 AS FLOAT) AS 数量, 
                                RTRIM(MB032) AS 供应商编码, 
                                RTRIM(MA002) 供应商简称,  
                                (CASE WHEN MB109 = 'Y' THEN '已核准' WHEN MB109 = 'y' THEN '尚未核准' WHEN MB109 = 'N' THEN '不准交易' END ) AS 核准状况

                                FROM INVMB
                                LEFT JOIN PURMA ON MA001 = MB032
                                ORDER BY MB109 DESC , MB025, MB001
                                ";
            DataTable dt = mssql.SQLselect(connSW, sqlStr);
            dt.TableName = "品号信息";
            return dt;
        }
        #endregion
    }
}
