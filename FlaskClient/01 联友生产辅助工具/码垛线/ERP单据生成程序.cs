using HarveyZ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FastReport;
using System.IO;

namespace 联友生产辅助工具.生管码垛线
{
    public partial class 码垛线_ERP单据生成程序 : Form
    {
        #region 公共静态变量
        //软件信息
        public static string ProgVersion = "";
        public static string ProgName = "";
        //服务器URL
        public static string HttpURL = "";
        private string UpdateUrl = "";
        #endregion

        #region 局域静态变量
        private Mssql mssql = new Mssql();
        private static string connRobot = Global_Const.strConnection_MD;
        private static string connYF = Global_Const.strConnection_YF;
        public static string connWG = Global_Const.strConnection_WG;
        
        private static string printFilePath = "";

        private static DataTable componentFileDt = new DataTable();
        #endregion

        #region PrintPreview变量
        private static string tg001 = "";
        private static string tg002 = "";
        private static string md_no = "";
        private static string printId = "";
        private static string printFileName = "";

        #endregion

        #region 初始化
        public 码垛线_ERP单据生成程序(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            //FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
            Init();
            FormMain_Resized_Work();
        }

        private void Init()
        {
            GetListData();
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

        #region 窗体UI-打印列表
        private void dgvList_Show(DataTable dt)
        {
            DgvMain.DataSource = null;
            int rowIndex = -1;
            if (DgvMain.DataSource != null)
            {
                rowIndex = DgvMain.CurrentRow.Index;
            }

            if(dt != null)
            {
                DgvMain.DataSource = dt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvMain.ReadOnly = true;
            }

            DgvOpt.SelectLastRow(DgvMain, rowIndex);

            DgvOpt.SetColHeadMiddleCenter(DgvMain);

            DgvOpt.SetColMiddleCenter(DgvMain, "打印序号");
            DgvOpt.SetColMiddleCenter(DgvMain, "栈板号");
            DgvOpt.SetColMiddleCenter(DgvMain, "箱数");
            DgvOpt.SetColMiddleCenter(DgvMain, "正在打印");
            DgvOpt.SetColMiddleCenter(DgvMain, "已打印");
            DgvOpt.SetColMiddleCenter(DgvMain, "异常");
            DgvOpt.SetColMiddleCenter(DgvMain, "已生成生产入库单");
            DgvOpt.SetColMiddleCenter(DgvMain, "已生成销货单");

            DgvOpt.SetColWidth(DgvMain, "打印序号", 60);
            DgvOpt.SetColWidth(DgvMain, "栈板号", 60);
            DgvOpt.SetColWidth(DgvMain, "箱数", 40);
            DgvOpt.SetColWidth(DgvMain, "正在打印", 60);
            DgvOpt.SetColWidth(DgvMain, "已打印", 60);
            DgvOpt.SetColWidth(DgvMain, "异常", 40);
            DgvOpt.SetColWidth(DgvMain, "时间", 120);
            DgvOpt.SetColWidth(DgvMain, "已生成", 120);
            DgvOpt.SetColWidth(DgvMain, "销货单别", 60);
            DgvOpt.SetColWidth(DgvMain, "生产入库单别", 90);

        }

        private void dgvList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = DgvMain.CurrentRow.Index;
            string xhOutFlag = DgvMain.Rows[rowIndex].Cells["已生成销货单"].Value.ToString();
            string errFlag = DgvMain.Rows[rowIndex].Cells["异常"].Value.ToString();
            printFileName = DgvMain.Rows[rowIndex].Cells["销货单据名称"].Value.ToString();
            printId = DgvMain.Rows[rowIndex].Cells["打印序号"].Value.ToString();
            tg001 = DgvMain.Rows[rowIndex].Cells["销货单别"].Value.ToString();
            tg002 = DgvMain.Rows[rowIndex].Cells["销货单号"].Value.ToString();
            md_no = DgvMain.Rows[rowIndex].Cells["栈板号"].Value.ToString();


            if (xhOutFlag == "True" && errFlag == "False")
            {
                btnPrintPreview.Enabled = true;
            }
            
            dgvDetail_Show(GetDetailData());

            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }

        private DataTable GetDetailData()
        {
            label3.Text = tg001;
            label4.Text = tg002;
            label7.Text = md_no;
            label8.Text = printId;
            string sqlstr = @"SELECT COPTH.TH003 AS 序号, RTRIM(COPTH.TH004) AS 品号, RTRIM(COPTH.TH005) AS 品名, RTRIM(COPTH.TH006) AS 规格,
                                RTRIM(COPTH.TH014) + '-' + RTRIM(COPTH.TH015) + '-' + RTRIM(COPTH.TH016) AS 生产单号, 
                                CONVERT(VARCHAR(10), CONVERT(FLOAT, COPTH.TH008)) AS 数量, RTRIM(COPTH.TH009) AS 单位, 
                                RTRIM(COPTH.UDF03) AS 描述备注, RTRIM(COPTH.UDF04) AS 保友品名, RTRIM(COPTH.UDF05) AS 配置方案, RTRIM(COPTH.UDF10) AS 产品电商代码, 
                                ' ' + RTRIM(CMSMC.MC002) + CHAR(10)+ CHAR(10) + RTRIM(COPTH.TH004) AS 备注, RTRIM(COPTH.UDF01) AS PO号

                                FROM [192.168.0.99].COMFORT.dbo.COPTH
                                LEFT JOIN [192.168.0.99].COMFORT.dbo.COPTG ON TG001 = TH001 AND TG002 = TH002
                                LEFT JOIN [192.168.0.99].COMFORT.dbo.COPMA ON COPMA.MA001 = COPTG.TG004 
                                LEFT JOIN [192.168.0.99].COMFORT.dbo.CMSMC ON COPTH.TH007 = CMSMC.MC001
                                LEFT JOIN [192.168.0.99].COMFORT.dbo.CMSMV ON COPTG.CREATOR = CMSMV.MV001 
                                WHERE RTRIM(TH001) = '{0}' AND RTRIM(TH002) = '{1}'
                                ORDER BY COPTH.TH003";
            DataTable dt = mssql.SQLselect(connRobot, string.Format(sqlstr, tg001, tg002));
            return dt;
        }
        #endregion

        #region 打印列表明细
        private void GetListData() //打印列表显示信息的获取
        {
            string sqlstr = @"SELECT * FROM VPrintList 
                                WHERE (CONVERT(VARCHAR(20), 创建时间, 112) between '{0}' and '{1}' 
                                OR CONVERT(VARCHAR(20), 销货单生成时间, 112) between '{0}' and '{1}')
                                ORDER BY 打印序号";
            DataTable dt = mssql.SQLselect(connRobot, string.Format(sqlstr, DtpStart.Value.ToString("yyyyMMdd"), DtpEnd.Value.ToString("yyyyMMdd")));
            dgvList_Show(dt);
        }
        #endregion

        #region 销货单明细预览
        private void dgvDetail_Show(DataTable dt)
        {
            dgvDetail.DataSource = null;
            if (dt != null)
            {
                dgvDetail.DataSource = dt;
                DgvOpt.SetRowBackColor(dgvDetail);
                dgvDetail.ReadOnly = true;
                Dictionary<string, int> dgvDetailColWidthDict = new Dictionary<string, int>();
                dgvDetailColWidthDict.Add("序号", 40);
                dgvDetailColWidthDict.Add("数量", 40);
                dgvDetailColWidthDict.Add("单位", 40);
                dgvDetailColWidthDict.Add("描述备注", 300);
                DgvOpt.SetColWidth(dgvDetail, dgvDetailColWidthDict);
                DgvOpt.SetColHeadMiddleCenter(dgvDetail);
                List<string> dgvDetailColMiddleList = new List<string>();
                dgvDetailColMiddleList.Add("序号");
                dgvDetailColMiddleList.Add("单位");
                dgvDetailColMiddleList.Add("数量");
                DgvOpt.SetColMiddleCenter(dgvDetail, dgvDetailColMiddleList);
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            ERP单据预览 previewForm = new ERP单据预览(tg001, tg002, md_no, printId, getfrx(printFileName));
            if(previewForm.ShowDialog() == DialogResult.Cancel)
            {
                previewForm.Dispose();
            }
        }
        #endregion

        //获取打印格式
        private string getfrx(string fileName)
        {
            string sqlstr = @"SELECT CONTENT FROM dbo.WG_PRINT WHERE PRINT_TYPE = '码垛线销货单' AND PRINT_NAME = '{0}' ";
            string frx = mssql.SQLselect(connWG, string.Format(sqlstr, fileName)).Rows[0][0].ToString();
            return frx;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            GetListData();
        }
    }
}
