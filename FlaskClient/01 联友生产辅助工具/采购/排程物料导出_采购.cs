using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace HarveyZ.采购
{
    public partial class 排程物料导出_采购 : Form
    {
        private string connYF = FormLogin.infObj.connYF;
        private string connWG = FormLogin.infObj.connWG;
        private Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        #region 初始化
        public 排程物料导出_采购(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }

        private void FormMain_Init() // 窗体显示初始化
        {
            UI();
            DgvMain.ReadOnly = true;
            DgvOpt.SetColHeadMiddleCenter(DgvMain);
            DgvOpt.SetRowBackColor(DgvMain);
        }
        #endregion

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

        #region 按钮
        private void UI()
        {
            if (DgvMain.DataSource != null)
            {
                btnOutput.Enabled = true;
            }
            else
            {
                btnOutput.Enabled = false;
            }
        }

        private void DtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_Start(DtpStartDate, DtpEndDate);
            DgvMain.DataSource = null;
            UI();
        }

        private void DtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePickerOpt.DateTimeValueChange_End(DtpStartDate, DtpEndDate);
            DgvMain.DataSource = null;
            UI();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DgvShow();
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Excel excel = new Excel();
            Excel.Excel_Base excelObj = new Excel.Excel_Base();
            excelObj.dataSet.Tables.Add((DataTable)DgvMain.DataSource);
            excelObj.dataSet.Tables[0].TableName = "排程生产物料导出";
            excelObj.defauleFileName = "排程生产物料导出_" + DateTime.Now.ToString("yyyy-MM-dd");
            excelObj.isWrite = true;

            if (excel.ExcelOpt(excelObj))
            {
                if (excelObj.status)
                {
                    UpdOutputFlag((DataTable)DgvMain.DataSource);
                    Msg.Show("Excel导出成功！");
                }
                else
                {
                    Msg.Show(excelObj.msg, "错误");
                }
            }
        }
        #endregion

        #region 逻辑
        private string GetTime()
        {
            string slqStr = @"SELECT dbo.f_getTime(1) ";
            return mssql.SQLselect(connYF, slqStr).Rows[0][0].ToString();
        }

        private void UpdOutputFlag(DataTable dt)
        {
            string time = GetTime();
            string slqStr = @"UPDATE SC_PLAN SET SC031 = '{1}' WHERE K_ID = '{0}' ";
            string kidTmp = "";
            if(dt != null)
            {
                for(int index=0; index < dt.Rows.Count; index++)
                {
                    string kid = dt.Rows[index]["排程序号"].ToString();
                    if(kid != kidTmp)
                    {
                        kidTmp = kid;
                        mssql.SQLexcute(connWG, string.Format(slqStr, kid, time));
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private void DgvShow()
        {
            DgvMain.DataSource = null;
            string startDate = DtpStartDate.Value.ToString("yyyyMMdd");
            string endDate = DtpEndDate.Value.ToString("yyyyMMdd");

            DataTable dtShow = null;

            if (dtShow != null)
            {

            }
            else
            {
                Msg.ShowErr("没有查询到数据");
            }
            UI();
        }

        #endregion
    }
}
