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
    public partial class 订单类别编码管理 : Form
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
        public 订单类别编码管理(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag);
            FormMain_Init();
            FormMain_Resized_Work();
        }


        private void FormMain_Init() // 窗体显示初始化
        {
            ShowTypeSplitCode();
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

        private void ShowTypeSplitCode()
        {
            //string sqlstr = @"SELECT K_ID 序号, PO_Class 订单类别, PO_Type 描述备注, TypeCode 类别代码, Valid 有效码 FROM SplitTypeCode ORDER BY K_ID ";
            string sqlstr = @"Select InType 订单单别, InTypeName 订单单别名称, OutType 销货单别, OutTypeName 销货单别名称, SplitType 区分方式, Spec 描述备注, TypeCode 订单类别码, Remark 备注, PrintType 打印模板文件名 from SplitTypeCode";
            showDt = mssql.SQLselect(connStrRobot, sqlstr);


            DgvMain.DataSource = null;
            if (showDt != null)
            {
                showDtTmp = showDt.Copy();
                DgvMain.DataSource = showDt;
                //DgvOpt.SetColReadonly(DgvMain, "序号");
                //DgvOpt.SetColReadonly(DgvMain, "订单类别");
                //DgvOpt.SetColReadonly(DgvMain, "类别代码");
                //DgvOpt.SetColReadonly(DgvMain, "描述备注");
                DgvMain.ReadOnly = true;
                DgvOpt.SetRowBackColor(DgvMain);
            }

            DgvOpt.SetColNoSortMode(DgvMain);
        }

        private void BtnReflash_Click(object sender, EventArgs e)
        {
            ShowTypeSplitCode();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            订单类别编码添加 frm = new 订单类别编码添加();
            frm.ShowDialog();
            ShowTypeSplitCode();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string sqlstr = @"UPDATE SplitTypeCode SET Valid = '{3}' WHERE PO_Class = '{0}' AND PO_Type = '{1}' AND TypeCode = '{2}' ";
            for(int rowIndex = 0; rowIndex < showDtTmp.Rows.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < showDtTmp.Columns.Count; colIndex++)
                {
                    if(showDtTmp.Rows[rowIndex][colIndex].ToString() != DgvMain.Rows[rowIndex].Cells[colIndex].Value.ToString())
                    {
                        mssql.SQLexcute(connStrRobot, string.Format(sqlstr, DgvMain.Rows[rowIndex].Cells[1].Value.ToString(),
                            DgvMain.Rows[rowIndex].Cells[2].Value.ToString(),
                            DgvMain.Rows[rowIndex].Cells[3].Value.ToString(), 
                            DgvMain.Rows[rowIndex].Cells[4].Value.ToString()).Replace("'True'", "1").Replace("'False'", "0"));
                        break;
                    }
                }
            }
            ShowTypeSplitCode();
        }
    }
}
