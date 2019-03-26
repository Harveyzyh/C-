using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.测试
{
    public partial class 测试_2 : Form
    {
        string Url = FormLogin.HttpURL;

        public 测试_2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string >{ };
            dict.Add("1", "2");
            string json = FormLogin.HttpPost_Json(Url + @"/Client/Test/0", dict);
            textBox1.Text = json;
            dataGridView1.DataSource = Json.Json2DT(json);
            int count = 0;
            count = dataGridView1.ColumnCount;
            MessageBox.Show(count.ToString());
            if (count > 0)
            {
                for( int index = 0; index < count; index++)
                {
                    if(dataGridView1.Columns[index].HeaderText == "有效码")
                    {
                        MessageBox.Show(index.ToString());
                        for(int index2 = 0; index2 < dataGridView1.RowCount; index2++)
                        {
                            MessageBox.Show(dataGridView1.Rows[index2].Cells[index].Value.GetType().ToString());
                            dataGridView1.Columns[index] = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string json = "[{\"ascdsf\": \"1\", \"mac\": \"20:f1:7c:c5:cd:80\"}, {\"ascdsf\": \"sdfsfsdfsdf4\", \"mac\": \"20:f1:7c:c5:cd:85\"}]";
            textBox1.Text = json;
            dataGridView1.DataSource = Json.Json2DT(json);
        }
    }
}
