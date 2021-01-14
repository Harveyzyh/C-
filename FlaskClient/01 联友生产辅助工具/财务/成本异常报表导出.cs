using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace HarveyZ.财务
{
    public partial class 成本异常报表导出 : Form
    {
        private string conn = FormLogin.infObj.connYF;
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public 成本异常报表导出(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
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
            string dtp = dateTimePicker1.Value.ToString("yyyyMM");
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();

            DataSet ds = new DataSet();
            GetData(ds, dtp);

            excelObj.dataSet = ds;
            excelObj.defauleFileName = "成本异常" + dateTimePicker1.Value.ToString("yyyy-MM");
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

        private void GetData(DataSet ds, string date)
        {
            ds.Tables.Add(GetData1(date));
            ds.Tables.Add(GetData2(date));
            ds.Tables.Add(GetData3(date));
            ds.Tables.Add(GetData4(date));
            ds.Tables.Add(GetData5(date));
            ds.Tables.Add(GetData6(date));
            ds.Tables.Add(GetData7(date));
            ds.Tables.Add(GetData8(date));
            ds.Tables.Add(GetData9(date));
            ds.Tables.Add(GetData10(date));
            ds.Tables.Add(GetData11(date));
        }

        private DataTable GetNulData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("无异常");
            DataRow dr = dt.NewRow();
            dr["无异常"] = "";
            dt.Rows.Add(dr);
            return dt;
        }

        #region sql脚本内容
        //进货单本币税前<>发票本币税前
        private DataTable GetData1(string date)
        {
            string slqStr = @"SELECT TA003,TB008,TB001,TB002,TB003,TB005,TB006,TB007,TB017,TH047
                                FROM ACPTA LEFT JOIN ACPTB ON TA001=TB001 AND TA002=TB002 
                                LEFT JOIN PURTH ON TB005=TH001 AND TB006=TH002 AND TB007=TH003
                                WHERE SUBSTRING(TA003,1,6)='202009' AND SUBSTRING(TB008,1,6)='{0}'
                                AND TA024='Y' AND TA079='1' --AND TB004='1'
                                AND TB017<>TH047 
                                ORDER BY TB001,TB002,TB003";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "进货单本币税前<>发票本币税前";
            return dt;
        }

        //进货单本币税前<>本币税前-价差
        private DataTable GetData2(string date)
        {
            string slqStr = @"SELECT TA003,TB008,TB001,TB002,TB003,TB005,TB006,TB007,TB017,TB055,TH047
                                FROM ACPTA LEFT JOIN ACPTB ON TA001=TB001 AND TA002=TB002 
                                LEFT JOIN PURTH ON TB005=TH001 AND TB006=TH002 AND TB007=TH003
                                WHERE SUBSTRING(TA003,1,6)='{0}' AND SUBSTRING(TB008,1,6)<>'{0}'
                                AND TA024='Y' AND TA079='1' --AND TB004='1'
                                AND (TB017-TB055)<>TH047 
                                ORDER BY TB001,TB002,TB003";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "进货单本币税前<>本币税前-价差";
            return dt;
        }

        //进货单本币税前<>成本要素档金额
        private DataTable GetData3(string date)
        {
            string slqStr = @"SELECT LH001,TH001,TH002,TH003,TH004,TH005,TH006,TH047 本币税前,SUM(LH008) 成本要素档金额
                                FROM PURTH INNER JOIN INVLH 
                                ON TH001=LH002 AND TH002=LH003 AND TH003=LH004  
                                WHERE LH001='{0}'
                                GROUP BY LH001,TH001,TH001,TH002,TH003,TH004,TH005,TH006,TH047 
                                HAVING TH047<>SUM(LH008)
                                ORDER BY PURTH.TH001,TH002,TH003";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "进货单本币税前<>成本要素档金额";
            return dt;
        }

        //进货单本币税前<>LA013
        private DataTable GetData4(string date)
        {
            string slqStr = @"SELECT LA001,LA006,LA007,LA008,TH047 本币税前,LA013 库存交易档金额
                                FROM PURTH INNER JOIN INVLA
                                ON TH001=LA006 AND TH002=LA007 AND TH003=LA008  
                                WHERE SUBSTRING(LA004,1,6)='{0}'
                                AND TH047<>LA013";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "进货单本币税前<>LA013";
            return dt;
        }

        //退货单本币税前<>发票本币税前
        private DataTable GetData5(string date)
        {
            string slqStr = @"SELECT TA003,TB008,TB001,TB002,TB003,TB005,TB006,TB007,TB017,TJ032
                                FROM ACPTA LEFT JOIN ACPTB ON TA001=TB001 AND TA002=TB002 
                                LEFT JOIN PURTJ ON TB005=TJ001 AND TB006=TJ002 AND TB007=TJ003
                                WHERE SUBSTRING(TA003,1,6)='{0}' AND SUBSTRING(TB008,1,6)='{0}'
                                AND TA024='Y' AND TA079='1' --AND TB004='1'
                                AND (TB017+TJ032)<>'0'
                                ORDER BY TB001,TB002,TB003";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "退货单本币税前<>发票本币税前";
            return dt;
        }

        //退货单本币税前<>价差-本币税前
        private DataTable GetData6(string date)
        {
            string slqStr = @"SELECT TA003,TB008,TB001,TB002,TB003,TB005,TB006,TB007,TB017,TB055,TJ032
                                FROM ACPTA LEFT JOIN ACPTB ON TA001=TB001 AND TA002=TB002 
                                LEFT JOIN PURTJ ON TB005=TJ001 AND TB006=TJ002 AND TB007=TJ003
                                WHERE SUBSTRING(TA003,1,6)='{0}' AND SUBSTRING(TB008,1,6)<>'{0}'
                                AND TA024='Y' AND TA079='1' --AND TB004='1'
                                AND (TB055-TB017)<>TJ032 
                                ORDER BY TB001,TB002,TB003";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "退货单本币税前<>价差-本币税前";
            return dt;
        }

        //退货单本币税前<>成本要素档金额
        private DataTable GetData7(string date)
        {
            string slqStr = @"SELECT LH001,TJ001,TJ002,TJ003,TJ004,TJ005,TJ006,TJ032 本币税前,SUM(LH008) 成本要素档金额
                                FROM PURTJ INNER JOIN INVLH ON TJ001=LH002 AND TJ002=LH003 AND TJ003=LH004  
                                WHERE LH001='{0}'
                                GROUP BY LH001,TJ001,TJ001,TJ002,TJ003,TJ004,TJ005,TJ006,TJ032 
                                HAVING TJ032<>SUM(LH008)
                                ORDER BY PURTJ.TJ001,TJ002,TJ003";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "退货单本币税前<>成本要素档金额";
            return dt;
        }

        //退货单本币税前<>LA013
        private DataTable GetData8(string date)
        {
            string slqStr = @"SELECT LA001,LA006,LA007,LA008,TJ032 本币税前,LA013 库存交易档金额
                                FROM PURTJ INNER JOIN INVLA ON TJ001=LA006 AND TJ002=LA007 AND TJ003=LA008  
                                WHERE SUBSTRING(LA004,1,6)='{0}'
                                AND TJ032<>LA013";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "退货单本币税前<>LA013";
            return dt;
        }

        //其他单据不存在成本要素档
        private DataTable GetData9(string date)
        {
            string slqStr = @"SELECT LA006,LA007,LA008,LA001,LA004,LA013,LH002,LH003,LH004
                                FROM INVLA LEFT JOIN INVLH ON LH002=LA006 AND LH003=LA007 AND LH004=LA008
                                WHERE SUBSTRING(LA004,1,6)='{0}' AND LA013<>0 AND LH002 IS NULL
                                ORDER BY LA006,LA007,LA008 ";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "其他单据不存在成本要素档";
            return dt;
        }

        //是否存在当月暂估当月开票数据
        private DataTable GetData10(string date)
        {
            string date2 = date.Substring(2, 4);
            string slqStr = @"SELECT TQ001,TQ002,TQ003,TQ010,TQ011,TQ012 FROM ACPTQ
                                WHERE EXISTS (SELECT 1 FROM ACPTB WHERE TQ010=TB005 AND TQ011=TB006 AND TQ012=TB007 AND TB012='Y')
                                AND TQ001='7G01' AND TQ002='{1}0001' 

                                SELECT TB001,TB002,TB003,TB004,TB005,TB006,TB007
                                FROM ACPTA LEFT JOIN ACPTB ON TA001=TB001 AND TA002=TB002 
                                WHERE SUBSTRING(TA003,1,6)='{0}' AND TA024='Y' AND TB004='1'
                                AND EXISTS (SELECT 1 FROM ACPTQ WHERE TQ010=TB005 AND TQ011=TB006 AND TQ012=TB007 AND TQ001='7G01' AND TQ002='{1}0001')

                                SELECT TB001,TB002,TB003,TQ001,TQ002,TQ003,TQ010,TQ011,TQ012
                                FROM ACPTQ INNER JOIN ACPTB ON TQ010=TB005 AND TQ011=TB006 AND TQ012=TB007
                                WHERE TQ001='7G01' AND TQ002='{1}0001' AND TB012='Y'
                                ORDER BY TB001,TB002,TB003,TQ001,TQ002,TQ003,TQ010,TQ011,TQ012 ";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date, date2));
            if (dt == null) dt = GetNulData();
            dt.TableName = "是否存在当月暂估当月开票数据";
            return dt;
        }

        //退料单成本要素异常
        private DataTable GetData11(string date)
        {
            string slqStr = @"SELECT LA006,LA007,LA008,LA001,LA004,LA013,LH002,LH003,LH004
                            FROM INVLA 
                            LEFT JOIN INVLH ON LH002=LA006 AND LH003=LA007 AND LH004=LA008
                            WHERE SUBSTRING(LA004,1,6)='{0}'
                            AND LA006 LIKE '56%'
                            GROUP BY LA006,LA007,LA008,LA001,LA004,LA013,LH002,LH003,LH004 HAVING COUNT(*) > 1
                            ORDER BY LA006,LA007,LA008 ";
            DataTable dt = mssql.SQLselect(conn, string.Format(slqStr, date));
            if (dt == null) dt = GetNulData();
            dt.TableName = "退料单成本要素异常";
            return dt;
        }
        #endregion
    }
}
