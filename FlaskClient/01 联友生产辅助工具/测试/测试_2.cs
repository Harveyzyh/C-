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
            string json = HttpPost.HttpPost_Json(Url + @"/Client/Test/0", dict);
            if (json == null)
            {
                MessageBox.Show("null");
            }
            else
            {
                textBox1.Text = json;
                dataGridView1.DataSource = Json.Json2DT(json);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string json = "[{\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"\u5185\u9500\", \"\u7c7b\u522b\u7f16\u7801\": \"A\"}, {\"\u6709\u6548\u7801\": false, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"\u83dc\u9e1f\u6761\u7801\", \"\u7c7b\u522b\u7f16\u7801\": \"B\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"\u65e0\u7801\", \"\u7c7b\u522b\u7f16\u7801\": \"C\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"\u4eac\u4e1c\u6761\u7801\", \"\u7c7b\u522b\u7f16\u7801\": \"D\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5185\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"POP\u6761\u7801\", \"\u7c7b\u522b\u7f16\u7801\": \"E\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2201\", \"\u7c7b\u522b\u7f16\u7801\": \"F\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2214\", \"\u7c7b\u522b\u7f16\u7801\": \"G\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2204\", \"\u7c7b\u522b\u7f16\u7801\": \"H\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2203\", \"\u7c7b\u522b\u7f16\u7801\": \"I\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2202\", \"\u7c7b\u522b\u7f16\u7801\": \"J\"}, {\"\u6709\u6548\u7801\": true, \"\u8ba2\u5355\u5c5e\u6027\": \"\u5916\u9500\", \"\u8ba2\u5355\u7c7b\u522b\": \"2216\", \"\u7c7b\u522b\u7f16\u7801\": \"L\"}]";
            textBox1.Text = json;
            dataGridView1.DataSource = Json.Json2DT(json);
            DataGridViewCheckBoxColumn checkboxSynch = dataGridView1.Columns[0] as DataGridViewCheckBoxColumn;
            checkboxSynch.TrueValue = "True";
            checkboxSynch.FalseValue = "False";
        }
    }
}
