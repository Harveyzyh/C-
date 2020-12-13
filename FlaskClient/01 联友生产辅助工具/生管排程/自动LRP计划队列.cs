using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ.生管排程
{
    public partial class 自动LRP计划队列 : Form
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

        public 自动LRP计划队列(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);

            DgvOpt.SetRowBackColor(DgvMain);
        }

        private void BtnReflash_Click(object sender, EventArgs e)
        {
            DgvShow();
        }

        private void DgvShow()
        {
            string sqlStr = @"SELECT * FROM dbo.V_GetAutoErpPlanOrderDetail ORDER BY UDF12, planDd ";
            DataTable dt = mssql.SQLselect(connYF, sqlStr);
            if (dt != null)
            {
                for(int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                {
                    if (dt.Columns[colIndex].ColumnName == "planDd") dt.Columns[colIndex].ColumnName = "订单号";
                    if (dt.Columns[colIndex].ColumnName == "planId") dt.Columns[colIndex].ColumnName = "计划编号";
                    if (dt.Columns[colIndex].ColumnName == "LRPCOUNT") dt.Columns[colIndex].ColumnName = "已跑计划次数";
                    if (dt.Columns[colIndex].ColumnName == "CG") dt.Columns[colIndex].ColumnName = "已存在采购单";
                    if (dt.Columns[colIndex].ColumnName == "UDF12") dt.Columns[colIndex].ColumnName = "订单更新时间";
                    if (dt.Columns[colIndex].ColumnName == "TD004") dt.Columns[colIndex].ColumnName = "成品品号";
                    if (dt.Columns[colIndex].ColumnName == "TD053") dt.Columns[colIndex].ColumnName = "客户配置";
                }
                DgvMain.DataSource = dt;
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColMiddleCenter(DgvMain);
                DgvOpt.SetColWidth(DgvMain, "订单号", 180);
                DgvOpt.SetColWidth(DgvMain, "计划编号", 180);
                DgvOpt.SetColWidth(DgvMain, "客户配置", 250);
            }
            else
            {
                DgvMain.DataSource = null;
                Msg.Show("没有数据");
            }
        }
    }
}
