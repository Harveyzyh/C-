using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;
using System.Collections;

namespace HarveyZ.生管排程
{
    public partial class 生产排程部门管理 : Form
    {
        #region 本地局域变量
        private static DataTable showDt = new DataTable();
        private string connWG = FormLogin.infObj.connWG;
        private static Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;
        #endregion

        #region 窗体设计
        public 生产排程部门管理(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }
        
        private void FormMain_Init() // 窗体显示初始化
        {
            ShowDgv();
            UI();
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
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            DgvMain.Location = new Point(0, panel_Title.Height + 2);
            DgvMain.Size = new Size(FormWidth, FormHeight - panel_Title.Height - 2);
        }
        #endregion

        #endregion
        private void UI()
        {
            BtnAdd.Enabled = newFlag ? true : false;
            BtnSave.Enabled = editFlag ? true : false;
        }

        private void BtnReflash_Click(object sender, EventArgs e)
        {
            ShowDgv();
            UI();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            生产排程部门添加 frm = new 生产排程部门添加();
            frm.ShowDialog();
            ShowDgv();
            UI();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string slqStr = @"UPDATE dbo.SC_PLAN_DPT_TYPE SET Valid = '{2}', ERPDpt = '{3}', DptWorkTime = '{4}' WHERE Type = '{0}' AND Dpt = '{1}' ";
            for (int rowIndex = 0; rowIndex < showDt.Rows.Count; rowIndex++)
            {
                mssql.SQLexcute(connWG, string.Format(slqStr, DgvMain.Rows[rowIndex].Cells["类型"].Value.ToString().Replace("导入", "In").Replace("导出", "Out"),
                    DgvMain.Rows[rowIndex].Cells["排程部门"].Value.ToString(),
                    DgvMain.Rows[rowIndex].Cells["有效码"].Value.ToString().Replace("'True'", "1").Replace("'False'", "0"), 
                    DgvMain.Rows[rowIndex].Cells["ERP部门编号"].Value.ToString(), DgvMain.Rows[rowIndex].Cells["部门总生产工时"].Value.ToString()));

            }
            ShowDgv();
            UI();
        }
        private void ShowDgv()
        {
            string slqStr = @"SELECT (CASE Type WHEN 'In' THEN '导入' WHEN 'Out' THEN '导出' ELSE Type END) 类型, Dpt 排程部门, ERPDpt ERP部门编号, DptWorkTime 部门总生产工时, Valid 有效码 
                                FROM dbo.SC_PLAN_DPT_TYPE ORDER BY Type, K_ID ";
            showDt = mssql.SQLselect(connWG, slqStr);

            DgvMain.DataSource = null;
            if (showDt != null)
            {
                DgvMain.DataSource = showDt;

                DgvOpt.SetColReadonly(DgvMain, "类型");
                DgvOpt.SetColReadonly(DgvMain, "排程部门");

                DgvOpt.SetRowBackColor(DgvMain);
                DgvOpt.SetColNoSortMode(DgvMain);
                DgvOpt.SetColMiddleCenter(DgvMain);
            }
        }
    }
}
