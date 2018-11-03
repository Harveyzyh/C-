using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 联友生产辅助工具
{
    public partial class FormMain_WG_Modi : Form
    {
        public static string strConnection = FormMain.strConnection;

        public FormMain_WG_Modi()
        {
            InitializeComponent();
            Init();
            comboBox1.Text = "";
        }

        private void Init()
        {

            //comboBox1.Text = "";
            getitem();


            string sqlstr = "";
            sqlstr = "SELECT WGroup AS 组别, Serial AS 系列, Vaild AS 有效码 FROM WG_DB..SC_XL2GY ORDER BY K_ID DESC ";
            DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);

            dataGridView1.DataSource = null;

            if (dttmp != null)
            {
                dataGridView1.Rows.Clear();

                int Columns = dttmp.Columns.Count;
                int Rows = dttmp.Rows.Count;
                int Row_Index = 0;
                int Index;

                for(Row_Index = 0; Row_Index < Rows; Row_Index++)
                {
                    Index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[Index].Cells[0].Value = dttmp.Rows[Row_Index][0].ToString();
                    dataGridView1.Rows[Index].Cells[1].Value = dttmp.Rows[Row_Index][1].ToString();
                    if(dttmp.Rows[Row_Index][2].ToString() == "Y")
                    {
                        dataGridView1.Rows[Index].Cells[2].Value = 1;
                    }
                    else
                    {
                        dataGridView1.Rows[Index].Cells[2].Value = 0;
                    }
                }




                dataGridView1.Columns[0].Width = 150;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].Width = 350;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].Width = 50;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.Bisque;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;


                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                HeadRowLineNumber(dataGridView1);
                dataGridView1.RowHeadersWidth = 40;
            }
            else
            {
                MessageBox.Show("没有查询到数据！", "提示", MessageBoxButtons.OK);
            }
        }

        private void getitem()
        {
            string sqlstr = "SELECT DISTINCT WGroup FROM WG_DB..SC_XL2GY ORDER BY WGroup";
            DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);
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
            DataTable dttmp = Mssql.SQLselect(strConnection, sqlstr);
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
            RowCount = dataGridView1.RowCount;

            for(Index = 0; Index < RowCount; Index++)
            {
                WGroup = dataGridView1.Rows[Index].Cells[0].Value.ToString();
                Serial = dataGridView1.Rows[Index].Cells[1].Value.ToString();
                if((bool)dataGridView1.Rows[Index].Cells[2].EditedFormattedValue == true)
                {
                    Vaild = "Y";
                }
                else
                {
                    Vaild = "N";
                }

                sqlstr1 = "SELECT Serial FROM WG_DB..SC_XL2GY WHERE Serial = '" + Serial + "' AND WGroup = '" + WGroup + "' AND Vaild = '" +  Vaild + "' ";
                if (seach(sqlstr1))
                {
                    continue;
                }
                else
                {
                    sqlstr2 = "UPDATE WG_DB..SC_XL2GY SET Vaild = '" + Vaild + "' WHERE Serial = '" + Serial + "' AND WGroup = '" + WGroup + "' ";
                    Mssql.SQLexcute(strConnection, sqlstr2);
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
                    Mssql.SQLexcute(strConnection, sqlstr2);
                    MessageBox.Show("组别：" + WGroup + "，系列：" + Serial + "新增成功！", "提示", MessageBoxButtons.OK);
                    commit();
                    Init();
                }
            }
            textBox1.Focus();
            textBox1.SelectAll();
            RecordUseLog("联友生产辅助工具", "生产日报表-维护组别系列");
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

            sqlstr = " INSERT INTO WG_DB..WG_USELOG (UserID, Date, ProgramName, ModuleName) VALUES('" + Global_Var.Login_UID + "', " + Global_Const.sqldatestrlong
                   + ", '" + ProgramName + "', '" + ModuleName + "')";

            Mssql.SQLexcute(strConnection, sqlstr);
        }
        #endregion
    }
}
