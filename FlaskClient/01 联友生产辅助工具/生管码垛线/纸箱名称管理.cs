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

namespace 联友生产辅助工具.生管码垛线
{
    public partial class 纸箱名称管理 : Form
    {
        #region 本地局域变量
        private static DataTable showDtTmp = new DataTable();
        private static DataTable showDt = new DataTable();
        public static string connStrRobot = 纸箱编码管理.connStrRobot;
        private static Mssql mssql = new Mssql();
        #endregion

        #region 窗体设计
        public 纸箱名称管理()
        {
            InitializeComponent();
            FormMain_Init();
            FormMain_Resized_Work();
        }


        private void FormMain_Init() // 窗体显示初始化
        {
            ShowBoxName();
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

        private void ShowBoxName()
        {
            string sqlstr = @"SELECT BoxName 纸箱名称, Spec 工艺编码, Valid 有效码 FROM BoxNameCode ORDER BY BoxName ";
            showDt = mssql.SQLselect(connStrRobot, sqlstr);


            DgvMain.DataSource = null;
            if (showDt != null)
            {
                showDtTmp = showDt.Copy();
                DgvMain.DataSource = showDt;
                DgvOpt.SetColReadonly(DgvMain, "纸箱名称");
                DgvOpt.SetRowBackColor(DgvMain);
            }

            DgvOpt.SetColHeadMiddleCenter(DgvMain);
        }

        private void BtnReflash_Click(object sender, EventArgs e)
        {
            ShowBoxName();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            纸箱名称添加 frm = new 纸箱名称添加();
            frm.ShowDialog();
            ShowBoxName();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string sqlstr = @"UPDATE BoxNameCode SET Spec = '{1}', Valid = '{2}' WHERE BoxName = '{0}' ";
            for(int rowIndex = 0; rowIndex < showDtTmp.Rows.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < showDtTmp.Columns.Count; colIndex++)
                {
                    if(showDtTmp.Rows[rowIndex][colIndex].ToString() != DgvMain.Rows[rowIndex].Cells[colIndex].Value.ToString())
                    {
                        mssql.SQLexcute(connStrRobot, string.Format(sqlstr, DgvMain.Rows[rowIndex].Cells[0].Value.ToString(),
                             DgvMain.Rows[rowIndex].Cells[1].Value.ToString(),
                            DgvMain.Rows[rowIndex].Cells[2].Value.ToString()).Replace("'True'", "1").Replace("'False'", "0"));
                        break;
                    }
                }
            }
            ShowBoxName();
        }
    }
}
