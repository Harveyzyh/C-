using System;
using System.Data;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.仓储中心
{
    public partial class 生成领料单_获取单据信息 : Form
    {
        private static InfoObject infObj = null;
        private static LldGenerate generate = null;

        DataTable dt = null;

        public 生成领料单_获取单据信息(InfoObject _infObj, LldGenerate _generate)
        {
            InitializeComponent();

            infObj = _infObj;
            generate = _generate;

            Init();
        }

        private void Init()
        {
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetDt();
        }

        private void GetDt()
        {
            dt = infObj.sql.SQLselect(infObj.connStr, string.Format(infObj.getGdSqlStr, textBox1.Text.Trim(), infObj.dpt, infObj.tradeMode));
            if(dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = false;
                for(int idx=1; idx<dataGridView1.Columns.Count; idx++)
                {
                    dataGridView1.Columns[idx].ReadOnly = true;
                }
                DgvOpt.SetRowColor(dataGridView1);
                button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("无数据", "错误", MessageBoxButtons.OK);
                button2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt2 = dt.Copy();
            dt2.Columns.RemoveAt(0);

            for(int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
            {
                if(dt.Rows[rowIdx][0].ToString() == "True")
                {
                    bool flag = true;
                    DataRow dtRow = dt2.Rows[rowIdx];

                    // 检车新勾选的是否存在原有的dt里，若存在则 pass
                    for(int rowIdx2=0; rowIdx2 < infObj.gdDt.Rows.Count; rowIdx2++)
                    {
                        if (dtRow[0].ToString() == infObj.gdDt.Rows[rowIdx2][0].ToString() && dtRow[1].ToString() == infObj.gdDt.Rows[rowIdx2][1].ToString())
                        {
                            flag = false;
                            break;
                        }
                    }
                    if(flag) infObj.gdDt.ImportRow(dtRow);
                }
            }
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                GetDt();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                {
                    dt.Rows[rowIdx][0] = !(bool)dt.Rows[rowIdx][0];
                }
                DgvOpt.SelectLastRow(dataGridView1);
            }

            e.Handled = true;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            dt.Rows[dataGridView1.CurrentRow.Index][0] = !(bool)dt.Rows[dataGridView1.CurrentRow.Index][0];
        }
    }
}
