﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace HarveyZ.生产日报表
{
    public partial class 日报表维护组别系列 : Form
    {
        public static string strConnection = FormLogin.infObj.connWG;

        Mssql mssql = new Mssql();

        private bool newFlag = false;
        private bool editFlag = false;
        private bool delFlag = false;
        private bool outFlag = false;
        private bool lockFlag = false;
        private bool printFlag = false;

        public 日报表维护组别系列(string text = "")
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            FormLogin.infObj.userPermission.GetPermUserDetail(FormLogin.infObj.userId, this.Text, out newFlag, out editFlag, out delFlag, out outFlag, out lockFlag, out printFlag);
            Init();
            comboBox1.Text = "";
        }

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
            panel_Title.Size = new Size(FormWidth, panel_Title.Height);
            DgvMain.Location = new Point(0, panel_Title.Height + 2);
            DgvMain.Size = new Size(FormWidth, FormHeight - panel_Title.Height - 2);
        }
        #endregion


        private void Init()
        {
            getitem();
            
            string slqStr = "";
            slqStr = "SELECT WGroup AS 组别, Serial AS 系列, Valid AS 有效码 FROM dbo.SC_DRY_XL2GY ORDER BY Serial, K_ID DESC ";
            DataTable dttmp = mssql.SQLselect(strConnection, slqStr);

            DgvMain.DataSource = null;

            if (dttmp != null)
            {
                DgvMain.Rows.Clear();

                int Columns = dttmp.Columns.Count;
                int Rows = dttmp.Rows.Count;

                DgvMain.DataSource = dttmp;
                DgvOpt.SetRowBackColor(DgvMain);

                DgvMain.Columns[0].Width = 150;
                DgvMain.Columns[0].ReadOnly = true;
                DgvMain.Columns[1].Width = 350;
                DgvMain.Columns[1].ReadOnly = true;
                DgvMain.Columns[2].Width = 50;


                for (int i = 0; i < this.DgvMain.Columns.Count; i++)
                {
                    this.DgvMain.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                HeadRowLineNumber(DgvMain);
                DgvMain.RowHeadersWidth = 40;
            }
            else
            {
                MessageBox.Show("没有查询到数据！", "提示", MessageBoxButtons.OK);
            }
        }

        private void getitem()
        {
            string slqStr = "SELECT DISTINCT WGroup FROM dbo.SC_DRY_XL2GY ORDER BY WGroup";
            DataTable dttmp = mssql.SQLselect(strConnection, slqStr);
            if(dttmp != null)
            {
                comboBox1.Items.Clear();
                int Lenth = dttmp.Rows.Count;
                int Index;
                for (Index = 0; Index < Lenth; Index++)
                {
                    comboBox1.Items.Add(dttmp.Rows[Index][0].ToString());
                }
            }
        }

        private void HeadRowLineNumber(object sender)//表头行数
        {
            DataGridView dataGridView = (DataGridView)sender;
            int rowNumber = 1;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                row.HeaderCell.Value = rowNumber.ToString();
                rowNumber = rowNumber + 1;
            }
            dataGridView.AutoResizeRowHeadersWidth(
                DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
        
        private bool seach(string slqStr)
        {
            DataTable dttmp = mssql.SQLselect(strConnection, slqStr);
            if (dttmp != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void commit()
        {
            int Index, RowCount;
            string slqStr1, slqStr2;
            string WGroup, Serial, Valid;
            RowCount = DgvMain.RowCount;

            for(Index = 0; Index < RowCount; Index++)
            {
                WGroup = DgvMain.Rows[Index].Cells[0].Value.ToString();
                Serial = DgvMain.Rows[Index].Cells[1].Value.ToString();
                if((bool)DgvMain.Rows[Index].Cells[2].EditedFormattedValue == true)
                {
                    Valid = "1";
                }
                else
                {
                    Valid = "0";
                }

                slqStr1 = "SELECT Serial FROM dbo.SC_DRY_XL2GY WHERE Serial = '" + Serial + "' AND WGroup = '" + WGroup + "' AND Valid = '" +  Valid + "' ";
                if (seach(slqStr1))
                {
                    continue;
                }
                else
                {
                    slqStr2 = "UPDATE dbo.SC_DRY_XL2GY SET Valid = '" + Valid + "' WHERE Serial = '" + Serial + "' AND WGroup = '" + WGroup + "' ";
                    mssql.SQLexcute(strConnection, slqStr2);
                }
            }
        }

        private void commit_LineList()
        {
            string slqStr_insert = " INSERT INTO dbo.SC_DRY_LineList (Dpt, WGroup, Serial, Line) "
                          + " SELECT A.Dpt, B.WGroup, B.Serial, A.Line "
                          + " FROM sbo.SC_DRY_Dpt2Line AS A "
                          + " INNER JOIN dbo.SC_DRY_XL2GY AS B ON B.Valid = 1 "
                          + " AND A.Valid = 1 "
                          + " AND NOT EXISTS(SELECT 1 FROM dbo.SC_DRY_LineList AS C WHERE C.Dpt = A.Dpt "
                          + " AND C.WGroup = B.WGroup AND C.Serial = B.Serial AND C.Line = A.Line)";
            mssql.SQLexcute(strConnection, slqStr_insert);

            string slqStr_update = "UPDATE dbo.SC_DRY_LineList SET Valid=0 "
                                 + " FROM dbo.SC_DRY_LineList AS A "
                                 + " INNER JOIN dbo.SC_DRY_XL2GY AS B ON B.WGroup = A.WGroup AND B.Serial"
                                 + " WHERE B.Valid = 0 ";
            mssql.SQLexcute(strConnection, slqStr_update);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string WGroup = comboBox1.Text.Trim();
            string Serial = textBox1.Text.Trim();
            string slqStr1, slqStr2;

            slqStr1 = "SELECT Serial FROM dbo.SC_DRY_XL2GY WHERE Serial = '" + Serial + "' AND WGroup = '" + WGroup + "'";
            if (seach(slqStr1))
            {
                MessageBox.Show("组别：" + WGroup + "，系列：" + Serial + "已存在，请确认！保存失败。", "提示", MessageBoxButtons.OK);
            }
            else
            {
                if((WGroup == "") | (Serial == ""))
                {
                    MessageBox.Show("组别或系列为空，请确认！新增失败。", "提示", MessageBoxButtons.OK);
                }
                else
                {
                    slqStr2 = "INSERT INTO dbo.SC_DRY_XL2GY (WGroup, Serial) VALUES('" + WGroup + "', '" + Serial + "')";
                    mssql.SQLexcute(strConnection, slqStr2);
                    MessageBox.Show("组别：" + WGroup + "，系列：" + Serial + "新增成功！", "提示", MessageBoxButtons.OK);
                    commit();
                    commit_LineList();
                    Init();
                }
            }
            textBox1.Focus();
            textBox1.SelectAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            commit();
            Init();
            comboBox1.Text = "";
            textBox1.Text = "";
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK);
        }
    }
}
