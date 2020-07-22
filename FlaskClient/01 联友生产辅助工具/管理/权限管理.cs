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
        static Mssql mssql = new Mssql();
        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        public 权限管理(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
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
            DgvUser.DataSource = FormLogin.infObj.userLogin.GetUserInfo();
            DgvOpt.SetRowBackColor(DgvUser);
        }

        private void ShowUserPerm(string U_ID)
        {
            DgvMain.DataSource = null;
            BtnSave.Enabled = false;
            BtnReset.Enabled = false;
            DataTable showDt = FormLogin.infObj.userPermission.ShowUserPerm(U_ID);
            if (showDt != null)
            {
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowBackColor(DgvMain);
                DgvOpt.SetColHeadMiddleCenter(DgvMain);
                DgvOpt.SetColReadonly(DgvMain, "权限名");
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
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("PermName", Type.GetType("System.String"));
                dt.Columns.Add("New", Type.GetType("System.String"));
                dt.Columns.Add("Edit", Type.GetType("System.String"));
                dt.Columns.Add("Del", Type.GetType("System.String"));
                dt.Columns.Add("Out", Type.GetType("System.String"));
                dt.Columns.Add("Lock", Type.GetType("System.String"));
                for (int rowIndex = 0; rowIndex < saveDt.Rows.Count; rowIndex++)
                {
                    if (saveDt.Rows[rowIndex]["有效码"].Equals(true))
                    {
                        dr = dt.NewRow();
                        dr["PermName"] = saveDt.Rows[rowIndex]["权限名"].ToString();
                        dr["New"] = saveDt.Rows[rowIndex]["新增"].Equals(true) ? "1" : "0";
                        dr["Edit"] = saveDt.Rows[rowIndex]["编辑"].Equals(true) ? "1" : "0";
                        dr["Del"] = saveDt.Rows[rowIndex]["删除"].Equals(true) ? "1" : "0";
                        dr["Out"] = saveDt.Rows[rowIndex]["输出"].Equals(true) ? "1" : "0";
                        dr["Lock"] = saveDt.Rows[rowIndex]["锁定"].Equals(true) ? "1" : "0";
                        dt.Rows.Add(dr);
                    }
                }
                FormLogin.infObj.userPermission.SetPermUser(TextBoxU_ID.Text, dt);
                MessageBox.Show("保存成功");
            }
        }

        private void BtnSetBasePerm_Click(object sender, EventArgs e)
        {
            FormLogin.infObj.userPermission.SetPermBase(FormLogin.infObj.menuItemList);
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
            if (FormLogin.infObj.userLogin.SetPasswdReset(TextBoxU_ID.Text))
            {
                MessageBox.Show("已设置！", "提示", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("设置失败，稍后重试！", "提示", MessageBoxButtons.OK);
            }
            ShowUser();
        }
    }
}
