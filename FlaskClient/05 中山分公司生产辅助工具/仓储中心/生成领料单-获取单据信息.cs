using System;
using System.Data;
using System.Windows.Forms;
using HarveyZ;

namespace 联友中山分公司生产辅助工具.仓储中心
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
            string sqlstr = "SELECT CAST(0 AS BIT) 选择, V_GetWscGd.* From V_GetWscGd "
                    + " WHERE 部门编号='{1}' AND 贸易方式='{2}' "
                    + " AND (RTRIM(工单单别) + '-' + RTRIM(工单单号) LIKE '%{0}%' OR 订单号 LIKE '%{0}%' "
                    + " OR 椅型 LIKE '%{0}%' OR 规格 LIKE '%{0}%' OR 成品品号 LIKE '%{0}%' OR 客户编号 LIKE '%{0}%') "
                    + " {3} ORDER BY RTRIM(工单单别) + '-' + RTRIM(工单单号)";

            string sqlstr2 = " AND  RTRIM(工单单号) IN ({0}) ";
            string sqlstr3 = "";

            foreach(string tmp in textBox2.Text.Split(','))
            {
                sqlstr3 += "'" + tmp.Replace("\r\n", "").Trim() + "',";
            }
            if (textBox2.Text == "")
            {
                sqlstr2 = "";
            }
            else
            {
                sqlstr2 = string.Format(sqlstr2, sqlstr3.TrimEnd(','));
            }


            dt = infObj.sql.SQLselect(infObj.connYF, string.Format(sqlstr, textBox1.Text.Trim(), infObj.dpt, infObj.tradeMode, sqlstr2));
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox obj = (TextBox)sender;
            obj.Text = obj.Text.Replace("\r\n", "").Replace(" ", "").Replace("，", ",").Replace(";", ",").Replace("；", ",");
        }
    }
}
