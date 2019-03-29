using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.生管码垛线
{
    public partial class 纸箱编码管理 : Form
    {
        #region 本地局域变量
        private static DataTable showDtTmp = new DataTable();
        private static DataTable showDt = new DataTable();
        private static string connStrRobot = FormLogin.Conn_ROBOT;
        private static Mssql mssql = new Mssql();
        #endregion


        #region 窗体设计
        public 纸箱编码管理()
        {
            InitializeComponent();
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
            dgvMain.Location = new Point(0, panel_Title.Height + 2);
            dgvMain.Size = new Size(FormWidth, FormHeight - panel_Title.Height - 2);
        }
        #endregion

        #endregion

        private void ShowBoxCode()
        {
            string sqlstr = @"SELECT K_ID 序号, BoxSize 纸箱尺寸, BoxCode 纸箱编码, BoxSet 纸箱码放方式, Valid 有效码 FROM BoxSizeCode ORDER BY K_ID ";
            showDt = mssql.SQLselect(connStrRobot, sqlstr);


            dgvMain.DataSource = null;
            if (showDt != null)
            {
                DtOpt.DtValid2Bool(showDt);
                showDtTmp = showDt.Copy();
                dgvMain.DataSource = showDt;
                dgvMain.Columns[0].ReadOnly = true;
                DgvOpt.SetRowColor(dgvMain);
            }
        }

        private void BtnReflash_Click(object sender, EventArgs e)
        {
            ShowBoxCode();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            for(int rowIndex = 0; rowIndex < showDtTmp.Rows.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < showDtTmp.Columns.Count; colIndex++)
                {
                    if(showDtTmp.Rows[rowIndex][colIndex] == showDt.Rows[rowIndex][colIndex])
                    {

                        break;
                    }
                }
            }
        }
    }
}
