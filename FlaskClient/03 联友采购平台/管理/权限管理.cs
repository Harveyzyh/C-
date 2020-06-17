using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{
    public partial class 权限管理 : Form
    {
        static string conn = FormLogin.infObj.connCG;
        static Mssql mssql = new Mssql();
        public 权限管理()
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
            
            string sqlstr = @"SELECT U_ID 账号, U_NAME 用户名, DPT 部门, FLAG 不允许重置密码, TYPE 账号来源类型 FROM WG_USER ORDER BY K_ID ";
            DgvUser.DataSource = mssql.SQLselect(conn, sqlstr);
            DgvOpt.SetRowColor(DgvUser);
        }

        private void ShowUserPerm(string U_ID)
        {
            DgvMain.DataSource = null;
            BtnSave.Enabled = false;
            BtnReset.Enabled = false;
            DataTable showDt = UserPermission.ShowUserPerm(U_ID);
            if (showDt != null)
            {
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowColor(DgvMain);
                BtnSave.Enabled = true;
                BtnReset.Enabled = true;
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
            UserPermission.SetPermBase(FormLogin.infObj.menuItemList);
            MessageBox.Show("基础权限信息保存成功");
        }

        private void TextBoxU_ID_TextChanged(object sender, EventArgs e)
        {
            DgvMain.DataSource = null;
            BtnSave.Enabled = false;
            BtnReset.Enabled = false;
        }

        private void DgvUser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string userId = "";
            if(DgvUser.RowCount > 0)
            {
                userId = DgvUser.Rows[DgvUser.CurrentRow.Index].Cells[0].Value.ToString();
                TextBoxU_ID.Text = userId;
                ShowUserPerm(userId);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            string sqlstr = @"UPDATE WG_USER SET FLAG = 'N' WHERE U_ID = '{0}' ";
            mssql.SQLexcute(conn, string.Format(sqlstr, TextBoxU_ID.Text));
            ShowUser();
            MessageBox.Show("已设置", "提示", MessageBoxButtons.OK);
        }
    }
}
