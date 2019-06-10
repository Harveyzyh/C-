using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.管理
{
    public partial class 用户管理 : Form
    {
        public 用户管理()
        {
            InitializeComponent();
            FormMain_Resized_Work();
            BtnSave.Enabled = false;
            ShowUser();
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
            DgvMain.Size = new Size(FormWidth / 2 -1, FormHeight - PanelTitle.Height - 2);
            DgvUser.Location = new Point(DgvMain.Width + 1, PanelTitle.Height + 2);
            DgvUser.Size = new Size(FormWidth / 2 - 1, FormHeight - PanelTitle.Height - 2);
        }

        #endregion

        private void ShowUser()
        {
            string connWG = Global_Const.strConnection_WGDB;
            Mssql mssql = new Mssql();
            string sqlstr = @"SELECT U_ID, U_NAME, DPT, FLAG, TYPE FROM WG_USER ORDER BY K_ID ";
            DgvUser.DataSource = mssql.SQLselect(connWG, sqlstr);
            DgvOpt.SetRowColor(DgvUser);
        }

        private void ShowUserPerm(string U_ID)
        {
            DgvMain.DataSource = null;
            DataTable showDt = UserPermission.ShowUserPerm(U_ID);
            if (showDt != null)
            {
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowColor(DgvMain);
                BtnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("没有查找到信息");
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            ShowUserPerm(TextBoxU_ID.Text);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable saveDt = (DataTable)DgvMain.DataSource;
            if (saveDt != null)
            {
                List<string> userPermList = new List<string> { };
                for (int rowIndex = 0; rowIndex < saveDt.Rows.Count; rowIndex++)
                {
                    if (saveDt.Rows[rowIndex][0].Equals(true))
                    {
                        userPermList.Add(saveDt.Rows[rowIndex][1].ToString());
                    }
                }
                UserPermission.SetPermUser(TextBoxU_ID.Text, userPermList);
                MessageBox.Show("保存成功");
            }
        }

        private void BtnSetBasePerm_Click(object sender, EventArgs e)
        {
            UserPermission.SetPermBase(FormLogin.menuItemList);
            MessageBox.Show("基础权限信息保存成功");
        }

        private void TextBoxU_ID_TextChanged(object sender, EventArgs e)
        {
            DgvMain.DataSource = null;
            BtnSave.Enabled = false;
        }
    }
}
