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
    public partial class 纸箱编码管理 : Form
    {
        #region 本地局域变量
        private static DataTable showDtTmp = new DataTable();
        private static DataTable showDt = new DataTable();
        public static string connStrRobot = FormLogin.infObj.connMD;
        private static Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        #endregion

        #region 窗体设计
        public 纸箱编码管理(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }


        private void FormMain_Init() // 窗体显示初始化
        {
            ShowBoxCode();
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

        private void ShowBoxCode()
        {
            string sqlstr = @"SELECT BoxSize 纸箱尺寸, BoxCode 纸箱编码, BoxSet 纸箱码放方式, Valid 有效码 FROM BoxSizeCode ORDER BY BoxCode, BoxSize ";
            showDt = mssql.SQLselect(connStrRobot, sqlstr);
            
            DgvMain.DataSource = null;
            if (showDt != null)
            {
                showDtTmp = showDt.Copy();
                DgvMain.DataSource = showDt;
                DgvOpt.SetColReadonly(DgvMain, "纸箱尺寸");
                DgvOpt.SetRowBackColor(DgvMain);
                DgvOpt.SetColNoSortMode(DgvMain);
                DgvMain.Columns[0].Width = 250;
                DgvMain.Columns[1].Width = 100;
            }
        }

        private void BtnReflash_Click(object sender, EventArgs e)
        {
            ShowBoxCode();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            纸箱编码添加 frm = new 纸箱编码添加();
            frm.ShowDialog();
            ShowBoxCode();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string sqlstr = @"UPDATE BoxSizeCode SET BoxCode = '{1}', BoxSet = '{2}', Valid = '{3}' WHERE BoxSize = '{0}' ";
            for(int rowIndex = 0; rowIndex < showDtTmp.Rows.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < showDtTmp.Columns.Count; colIndex++)
                {
                    string kk = DgvMain.Rows[rowIndex].Cells[colIndex].Value.ToString();
                    if (showDtTmp.Rows[rowIndex][colIndex].ToString() != DgvMain.Rows[rowIndex].Cells[colIndex].Value.ToString())
                    {
                        mssql.SQLexcute(connStrRobot, string.Format(sqlstr, DgvMain.Rows[rowIndex].Cells[0].Value.ToString(),
                            DgvMain.Rows[rowIndex].Cells[1].Value.ToString(),
                            DgvMain.Rows[rowIndex].Cells[2].Value.ToString(),
                            DgvMain.Rows[rowIndex].Cells[3].Value.ToString()).Replace("'True'", "1").Replace("'False'", "0"));
                        break;
                    }
                }
            }
            ShowBoxCode();
        }
    }
}
