using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;
using System.IO;

namespace 联友生产辅助工具.生管排程
{
    public partial class 订单信息查询 : Form
    {
        private Mssql mssql = new Mssql();
        private string connCOMFORT = FormLogin.infObj.connYF;
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;

        public 订单信息查询(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
            FormMain_Resized_Work();
        }
        
        #region 窗口大小变化设置
        private void FormMain_Resized(object sender, EventArgs e)
        {
            FormMain_Resized_Work();
        }

        public void FormMain_Resized_Work()
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

        private void DgvShow()
        {
            string sqlstr = @"SELECT '' AS 上线日期, 
                            (CASE WHEN TC004='0118' THEN '内销' ELSE '外销' END) 订单类型, 
                            (RTRIM(COPTD.TD001) +'-'+ RTRIM(COPTD.TD002) +'-'+COPTD.TD003+(CASE WHEN COPTF.UDF51='1' THEN '(新增)' WHEN COPTF.UDF51='0' THEN '(变更)' ELSE '' END)) AS 生产单号, 
                            RTRIM(COPMA.MA002) AS 客户名称, 
                            RTRIM(COPTC.TC015) AS 注意事项, 
                            (CASE WHEN COPTF.TF003 IS NULL THEN '' WHEN COPTF.TF003 IS NOT NULL AND COPTF.TF017 = 'Y' THEN '指定结束'+':'+'变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 ELSE '变更版本号'+COPTF.TF003+'_'+COPTF.UDF11 END) AS 订单变更原因, 
                            (CASE WHEN COPTD.TD013 = '' THEN '' WHEN COPTD.TD013 IS NULL THEN '' ELSE SUBSTRING(COPTD.TD013, 1, 4) + '-' + SUBSTRING(COPTD.TD013, 5, 2) + '-' + SUBSTRING(COPTD.TD013, 7, 2) END) AS 出货日, 
                            (CASE WHEN COPTD.UDF03 = '' THEN '' WHEN COPTD.UDF03 IS NULL THEN '' ELSE SUBSTRING(COPTD.UDF03, 1, 4) + '-' + SUBSTRING(COPTD.UDF03, 5, 2) + '-' + SUBSTRING(COPTD.UDF03, 7, 2) END) AS 验货日, 
                            RTRIM(COPTD.UDF01) AS PO#, 
                            RTRIM(COPTD.TD005) AS 品名, 
                            RTRIM(COPTD.UDF08) AS 保友品名, 
                            RTRIM(COPTD.TD006) AS 规格, 
                            RTRIM(COPTD.TD008) AS 订单数量, 
                            RTRIM(COPTD.TD024) AS 赠品测试量, 
                            RTRIM(COPTD.TD053) AS 配置方案, 
                            RTRIM(COPTQ.TQ003) AS 配置方案描述, 
                            (COPTQ.UDF07+COPTD.TD020) AS 描述备注, 
                            RTRIM(COPTD.TD204) AS 柜型柜数, 
                            RTRIM(COPTC.TC035) AS 目的地, 
                            RTRIM(CMSMV.MV002) AS 业务员, 
                            RTRIM(COPTC.TC012) AS 客户单号, 
                            RTRIM(COPTD.TD014) AS 客户品号, 
                            RTRIM((CASE WHEN TC004='0118' THEN INVMB.UDF04 ELSE INVMB.UDF05 END)) AS 生产车间, 
                            RTRIM(COPTD.UDF05) AS 客户编码, 
                            RTRIM(COPTD.UDF10) AS 电商编码, 
                            (CASE WHEN COPTC.UDF09='否' THEN '' ELSE '是' END) AS 急单, 
                            SUBSTRING(COPTD.CREATE_DATE,1,12) AS 录单日期, 
                            (CASE WHEN K.TDUDF52 IS NOT NULL THEN '是' ELSE '' END) 订单日期等于BOM日期 
        
                            FROM COPTD AS COPTD 
                            Left JOIN COPTC AS COPTC On COPTD.TD001=COPTC.TC001 and COPTD.TD002=COPTC.TC002 
                            Left JOIN COPTQ AS COPTQ On COPTD.TD053=COPTQ.TQ002 and COPTD.TD004=COPTQ.TQ001 
                            Left JOIN COPMA AS COPMA On COPTC.TC004=COPMA.MA001 
                            Left JOIN CMSMV AS CMSMV On COPTC.TC006=CMSMV.MV001 
                            LEFT JOIN INVMB AS INVMB ON COPTD.TD004=INVMB.MB001 
                            Left JOIN COPTF AS COPTF On COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104 AND COPTF.TF003 = (SELECT MAX(TF003) FROM COPTF WHERE COPTD.TD001=COPTF.TF001 and COPTD.TD002=COPTF.TF002 and COPTD.TD003=COPTF.TF104) 
                            LEFT JOIN (SELECT TD001 AS TDTD001, TD002 AS TDTD002, TD003 AS TDTD003, TD013 AS TDTD013, UDF52 AS TDUDF52 FROM COPTD AS COPTD) K ON TDTD001 = TD001 AND TDTD002 = TD002 AND TDTD003 = TD003 AND CONVERT(INT, SUBSTRING(COPTD.CREATE_DATE, 1, 8)) = CONVERT(INT, TDUDF52) AND CONVERT(INT, TD013) - CONVERT(INT, TDUDF52) <=2 
                            LEFT JOIN CMSME AS CMSME ON CMSME.ME001 = INVMB.MB445 
                            WHERE 1=1 AND (COPTC.TC027 = 'Y') ";
            if (DtpStartDate.Checked) sqlstr += @"AND COPTD.UDF12 > '" + DtpStartDate.Value.ToString("yyyyMMddhhmmss") + "' ";
            if (DtpEndDate.Checked) sqlstr += @"AND COPTD.UDF12 < '" + DtpEndDate.Value.ToString("yyyyMMddhhmmss") + "' ";
            if (CmBoxType.Text == "生产三部") sqlstr += @"AND RTRIM(COPTD.TD005)LIKE '%NUM%' ";
            if (CmBoxType.Text == "原材料") sqlstr += @"AND (COPTD.TD004 LIKE '3%' OR COPTD.TD004 LIKE '4%' ) ";
            if (CmBoxType.Text == "半成品") sqlstr += @"AND COPTD.TD004 LIKE '2%' ";

            sqlstr += @"ORDER BY TD002, TD003 ";

            if (!CmBoxType.Items.Contains(CmBoxType.Text))
            {
                Msg.ShowErr("订单类型选择错误， 请重新选择");
            }
            else
            {
                DgvMain.DataSource = null;
                DataTable showDt = mssql.SQLselect(connCOMFORT, sqlstr);

                if (showDt != null)
                {
                    DgvMain.DataSource = showDt;
                    DgvOpt.SetRowBackColor(DgvMain);
                    DgvMain.Columns[2].Width = 180;
                    DgvMain.ReadOnly = true;
                    DgvMain.Columns[0].ReadOnly = false;
                    BtnOutput.Enabled = true;
                }
                else
                {
                    Msg.ShowErr("没有查询到数据");
                    BtnOutput.Enabled = false;
                }
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            DgvShow();
        }

        private void BtnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataDt = (DataTable)DgvMain.DataSource;
            excelObj.defauleFileName = "订单信息导出_" + CmBoxType.Text + "_" + DateTime.Now.ToString("yyyy-MM-dd");
            excelObj.isWrite = true;

            if (excel.ExcelOpt(excelObj))
            {
                if (excelObj.status)
                {
                    Msg.Show("Excel导出成功！");
                }
                else
                {
                    Msg.ShowErr(excelObj.msg);
                }
            }
        }

        private void CmBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DgvMain.DataSource = null;
            BtnOutput.Enabled = false;
        }
    }
}
