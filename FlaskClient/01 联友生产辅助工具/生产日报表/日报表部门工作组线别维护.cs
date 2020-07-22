using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.生产日报表
{
    public partial class 日报表部门工作组线别维护 : Form
    {
        Mssql mssql = new Mssql();
        public static string strConnection = FormLogin.infObj.connWG;

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;

        public 日报表部门工作组线别维护(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
            Form_MainResized_Work();
            ButtonReflash_Work();
        }

        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region 窗口大小变化设置
        private void Form_MainResized(object sender, EventArgs e)
        {
            Form_MainResized_Work();
        }

        private void Form_MainResized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
            PanelTitle.Location = new Point(2, 2);
            PanelTitle.Size = new Size(FormWidth, PanelTitle.Height);
            DgvMain.Location = new Point(2, PanelTitle.Height + 3);
            DgvMain.Size = new Size(FormWidth, FormHeight - PanelTitle.Height - 6);
        }
        #endregion

        #region 窗体按钮
        private void ButtonReflash_Click(object sender, EventArgs e)
        {
            ButtonReflash_Work();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            ButtonSave_Work();
        }
        #endregion

        #region 逻辑处理
        private void ButtonReflash_Work()
        {
            string sqlstr_insert = " INSERT INTO dbo.SC_DRY_LineList (Dpt, WGroup, Serial, Line) "
                          + " SELECT A.Dpt, B.WGroup, B.Serial, A.Line "
                          + " FROM dbo.SC_DRY_Dpt2Line AS A "
                          + " INNER JOIN dbo.SC_DRY_XL2GY AS B ON B.Valid = 1 "
                          + " AND A.Valid = 1 "
                          + " AND NOT EXISTS(SELECT 1 FROM dbo.SC_DRY_LineList AS C WHERE C.[Dpt] = A.[Dpt] "
                          + " AND C.[WGroup] = B.[WGroup] AND C.[Serial] = B.[Serial] AND C.[Line] = A.[Line])";
            mssql.SQLexcute(strConnection, sqlstr_insert);


            string sqlstr_select = "";
            if (FormLogin.infObj.userDpt.Substring(0, 2) == "生产")
            {
                sqlstr_select = " SELECT A.Dpt 生产部门, A.WGroup 工作组, A.Serial 系列, A.Line 线别, A.Valid 有效码 FROM dbo.SC_DRY_LineList AS A "
                              + " INNER JOIN dbo.SC_DRY_XL2GY AS B ON A.WGroup = B.WGroup AND A.Serial = B.Serial AND B.Valid = 1 "
                              + " WHERE 1 = 1 "
                              + " AND Dpt = '" + FormLogin.infObj.userDpt + "' ";
            }
            else
            {
                sqlstr_select = " SELECT A.Dpt 生产部门, A.WGroup 工作组, A.Serial 系列, A.Line 线别, A.Valid 有效码 FROM dbo.SC_DRY_LineList AS A "
                              + " INNER JOIN dbo.SC_DRY_XL2GY AS B ON A.WGroup = B.WGroup AND A.Serial = B.Serial AND B.Valid = 1 ";
            }
            
            DataTable dt = mssql.SQLselect(strConnection, sqlstr_select);
            DgvMain.DataSource = null;
            if (dt != null)
            {
                DgvMain.DataSource = dt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvMain.ReadOnly = false;
                DgvMain.Columns[0].ReadOnly = true;
                DgvMain.Columns[1].ReadOnly = true;
                DgvMain.Columns[2].ReadOnly = true;
                DgvMain.Columns[3].ReadOnly = true;
                DgvMain.Columns[4].ReadOnly = false;
            }
        }

        private void ButtonSave_Work()
        {
            string boolStr = "";
            string sqlstr_update = "";
            string sqlstr_update_tmp = " UPDATE dbo.SC_DRY_LineList SET Valid = {4} WHERE Dpt = '{0}' AND WGroup = '{1}' AND Serial = '{2}' AND Line = '{3}'; ";

            for (int rowIndex = 0;rowIndex < DgvMain.RowCount; rowIndex++)
            {

                if (DgvMain.Rows[rowIndex].Cells[4].Value.ToString() == "True") boolStr = "1";
                else if (DgvMain.Rows[rowIndex].Cells[4].Value.ToString() == "False") boolStr = "0";

                sqlstr_update += string.Format(sqlstr_update_tmp, DgvMain.Rows[rowIndex].Cells[0].Value.ToString(),
                DgvMain.Rows[rowIndex].Cells[1].Value.ToString(), DgvMain.Rows[rowIndex].Cells[2].Value.ToString(),
                DgvMain.Rows[rowIndex].Cells[3].Value.ToString(), boolStr);

            }
            mssql.SQLexcute(strConnection, sqlstr_update);
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK);
            ButtonReflash_Work();
        }
        #endregion
    }
}
