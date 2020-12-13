using System;
using System.Data;
using System.Windows.Forms;
using HarveyZ;
using System.Collections.Generic;

namespace HarveyZ.仓储中心
{
    public partial class 生成领料单_获取单据信息 : Form
    {
        private static InfoObject infObj = null;
        private static LldGenerate generate = null;

        string connYF = FormLogin.infObj.connYF;

        List<string> gdList = null;

        DataTable dt = null;

        public 生成领料单_获取单据信息(InfoObject _infObj, LldGenerate _generate, List<string> _gdList=null)
        {
            InitializeComponent();

            infObj = _infObj;
            generate = _generate;
            gdList = _gdList;

            Init();
        }

        private void Init()
        {
            button2.Enabled = false;
            dateTimePicker1.Value = DateTime.Now.AddDays(2);

            if(gdList != null)
            {
                if (gdList.Count > 0)
                {
                    AutoGetDt();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                GetDt2();
            }
            else if (checkBox2.Checked)
            {
                GetDt3();
            }
            else
            {
                GetDt();
            }
        }

        private void AutoGetDt()
        {
            foreach (string tmp in gdList)
            {
                if(tmp != "") textBox2.Text += tmp + ",";
            }
            infObj.dpt = "";
            infObj.tradeMode = "";

            GetDt();


            if (dt != null)
            {
                DataTable dt2 = dt.Copy();
                dt2.Columns.RemoveAt(0);

                for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                {
                    bool flag = true;
                    DataRow dtRow = dt2.Rows[rowIdx];

                    for (int rowIdx2 = 0; rowIdx2 < infObj.gdDt.Rows.Count; rowIdx2++)
                    {
                        if (dtRow[0].ToString() == infObj.gdDt.Rows[rowIdx2][0].ToString() && dtRow[1].ToString() == infObj.gdDt.Rows[rowIdx2][1].ToString())
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) infObj.gdDt.ImportRow(dtRow);
                }
            }
            this.Close();
        }

        private void GetDt()
        {
            //string slqStr = "SELECT CAST(0 AS BIT) 选择, V_GetWscGd.* From V_GetWscGd "
            //        + " WHERE 部门编号 LIKE '%{1}%' AND 贸易方式 LIKE '%{2}%' "
            //        + " AND (RTRIM(工单单别) + '-' + RTRIM(工单单号) LIKE '%{0}%' OR 订单号 LIKE '%{0}%' "
            //        + " OR 椅型 LIKE '%{0}%' OR 规格 LIKE '%{0}%' OR 成品品号 LIKE '%{0}%' OR 客户编号 LIKE '%{0}%') "
            //        + " {3} ORDER BY RTRIM(工单单别) + '-' + RTRIM(工单单号)";


            string slqStr = "SELECT CAST(0 AS BIT) 选择, V_GetWscGd.* From V_GetWscGd "
                    + " WHERE 1=1  "
                    + " AND (RTRIM(工单单别) + '-' + RTRIM(工单单号) LIKE '%{0}%' OR 订单号 LIKE '%{0}%' "
                    + " OR 椅型 LIKE '%{0}%' OR 规格 LIKE '%{0}%' OR 成品品号 LIKE '%{0}%' OR 客户编号 LIKE '%{0}%') "
                    + " {3} ORDER BY RTRIM(工单单别) + '-' + RTRIM(工单单号)";

            string slqStr2 = " AND  RTRIM(工单单号) IN ({0}) ";
            string slqStr3 = "";

            foreach(string tmp in textBox2.Text.Split(','))
            {
                if (tmp != "") slqStr3 += "'" + tmp.Replace("\r\n", "").Trim() + "',";
            }
            if (textBox2.Text == "" || !textBox2.Enabled)
            {
                slqStr2 = "";
            }
            else
            {
                slqStr2 = string.Format(slqStr2, slqStr3.TrimEnd(','));
            }

            string aa = string.Format(slqStr, textBox1.Enabled ? textBox1.Text.Trim() : "",
                infObj.dpt, infObj.tradeMode, slqStr2);

            dt = infObj.sql.SQLselect(connYF, string.Format(slqStr, textBox1.Enabled ? textBox1.Text.Trim() : "", 
                infObj.dpt, infObj.tradeMode, slqStr2));
            if(dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = false;
                for(int idx=1; idx<dataGridView1.Columns.Count; idx++)
                {
                    dataGridView1.Columns[idx].ReadOnly = true;
                }
                DgvOpt.SetRowBackColor(dataGridView1);
                button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("查询工单无数据", "错误", MessageBoxButtons.OK);
                button2.Enabled = false;
            }
        }

        private void GetDt2()
        {
            //string slqStr = @"SELECT CAST(0 AS BIT) 选择, V_GetWscGd.* From V_GetWscGd 
            //                    WHERE 部门编号='{0}' AND 贸易方式='{1}' AND 审核日期 = convert(varchar(8), getdate(), 112) 
            //                    AND NOT EXISTS(SELECT 1 FROM MOCTE WHERE TE011 = 工单单别 AND TE012 = 工单单号)";


            string slqStr = @"SELECT CAST(0 AS BIT) 选择, V_GetWscGd.* From V_GetWscGd 
                                WHERE 审核日期 = convert(varchar(8), getdate(), 112) 
                                AND NOT EXISTS(SELECT 1 FROM MOCTE WHERE TE011 = 工单单别 AND TE012 = 工单单号)";

            dt = infObj.sql.SQLselect(connYF, string.Format(slqStr, infObj.dpt, infObj.tradeMode));
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = false;
                for (int idx = 1; idx < dataGridView1.Columns.Count; idx++)
                {
                    dataGridView1.Columns[idx].ReadOnly = true;
                }
                DgvOpt.SetRowBackColor(dataGridView1);
                button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("无数据", "错误", MessageBoxButtons.OK);
                button2.Enabled = false;
            }
        }

        private void GetDt3()
        {
            string slqStr = @"SELECT CAST(0 AS BIT) 选择, V_GetWscGd.* From V_GetWscGd 
                                INNER JOIN WG_DB.dbo.SC_PLAN ON SC_PLAN.K_ID = 排程序号
                                WHERE 1=1 
                                AND SC003 = '{0}'";

            dt = infObj.sql.SQLselect(connYF, string.Format(slqStr, dateTimePicker1.Value.ToString("yyyyMMdd")));
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = false;
                for (int idx = 1; idx < dataGridView1.Columns.Count; idx++)
                {
                    dataGridView1.Columns[idx].ReadOnly = true;
                }
                DgvOpt.SetRowBackColor(dataGridView1);
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

                    // 检测新勾选的是否存在原有的dt里，若存在则 pass
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                dateTimePicker1.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }
            else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            if (checkBox2.Checked)
            {
                dateTimePicker1.Enabled = true;
                checkBox1.Checked = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }
            else
            {
                dateTimePicker1.Enabled = false;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
        }
    }
}
