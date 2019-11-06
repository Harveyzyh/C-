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
    public partial class 日报表维护组别系列 : Form
    {
        public static string strConnection = 日报表新增.strConnection;

        Mssql mssql = new Mssql();
        string Login_UID = FormLogin.Login_Uid;
        string Login_Role = FormLogin.Login_Role;
        string Login_Dpt = FormLogin.Login_Dpt;

        public 日报表维护组别系列()
        {
            InitializeComponent();
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
            
            string sqlstr = "";
            sqlstr = "SELECT WGroup AS 组别, Serial AS 系列, Valid AS 有效码 FROM WG_DB..SC_XL2GY ORDER BY Serial, K_ID DESC ";
            DataTable dttmp = mssql.SQLselect(strConnection, sqlstr);

            DgvMain.DataSource = null;

            if (dttmp != null)
            {
                DgvMain.Rows.Clear();

                int Columns = dttmp.Columns.Count;
                int Rows = dttmp.Rows.Count;
                int Row_Index = 0;
                int Index;

                //for(Row_Index = 0; Row_Index < Rows; Row_Index++)
                //{
                //    Index = DataGridView_List.Rows.Add();
                //    DataGridView_List.Rows[Index].Cells[0].Value = dttmp.Rows[Row_Index][0].ToString();
                //    DataGridView_List.Rows[Index].Cells[1].Value = dttmp.Rows[Row_Index][1].ToString();
                //    if(dttmp.Rows[Row_Index][2].ToString() == "Y")
                //    {
                //        DataGridView_List.Rows[Index].Cells[2].Value = 1;
                //    }
                //    else
                //    {
                //        DataGridView_List.Rows[Index].Cells[2].Value = 0;
                //    }
                //}

                DgvMain.DataSource = dttmp;
                DgvOpt.SetRowColor(DgvMain);

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
            string sqlstr = "SELECT DISTINCT WGroup FROM WG_DB..SC_XL2GY ORDER BY WGroup";
            DataTable dttmp = mssql.SQLselect(strConnection, sqlstr);
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
        
        private bool seach(string sqlstr)
        {
            DataTable dttmp = mssql.SQLselect(strConnection, sqlstr);
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
            string sqlstr1, sqlstr2;
            string WGroup, Serial, Vaild;
            RowCount = DgvMain.RowCount;

            for(Index = 0; Index < RowCount; Index++)
            {
                WGroup = DgvMain.Rows[Index].Cells[0].Value.ToString();
                Serial = DgvMain.Rows[Index].Cells[1].Value.ToString();
                if((bool)DgvMain.Rows[Index].Cells[2].EditedFormattedValue == true)
                {
                    Vaild = "1";
                }
                else
                {
                    Vaild = "0";
                }

                sqlstr1 = "SELECT Serial FROM WG_DB..SC_XL2GY WHERE Serial = '" + Serial + "' AND WGroup = '" + WGroup + "' AND Valid = '" +  Vaild + "' ";
                if (seach(sqlstr1))
                {
                    continue;
                }
                else
                {
                    sqlstr2 = "UPDATE WG_DB..SC_XL2GY SET Valid = '" + Vaild + "' WHERE Serial = '" + Serial + "' AND WGroup = '" + WGroup + "' ";
                    mssql.SQLexcute(strConnection, sqlstr2);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string WGroup = comboBox1.Text.Trim();
            string Serial = textBox1.Text.Trim();
            string sqlstr1, sqlstr2;

            sqlstr1 = "SELECT Serial FROM WG_DB..SC_XL2GY WHERE Serial = '" + Serial + "' AND WGroup = '" + WGroup + "'";
            if (seach(sqlstr1))
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
                    sqlstr2 = "INSERT INTO WG_DB..SC_XL2GY VALUES('" + WGroup + "', '" + Serial + "', 'Y')";
                    mssql.SQLexcute(strConnection, sqlstr2);
                    MessageBox.Show("组别：" + WGroup + "，系列：" + Serial + "新增成功！", "提示", MessageBoxButtons.OK);
                    commit();
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

        #region 回传使用记录
        private void RecordUseLog(string ProgramName, string ModuleName)
        {
            string sqlstr = "";

            sqlstr = " INSERT INTO WG_DB..WG_USELOG (UserID, Date, ProgramName, ModuleName) VALUES('" + Login_UID + "', " + Normal.GetSysTimeStr("Long")
                   + ", '" + ProgramName + "', '" + ModuleName + "')";

            mssql.SQLexcute(strConnection, sqlstr);
        }
        #endregion
    }
}
