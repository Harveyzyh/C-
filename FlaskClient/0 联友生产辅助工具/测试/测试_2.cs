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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string json = "[{\"id\":\"00e58d51\",\"mac\":\"20:f1:7c:c5:cd:80\"}, {\"id\":\"00e58d51\",\"mac\":\"20:f1:7c:c5:cd:85\"}]";
            textBox1.Text = json;
            dataGridView1.DataSource = Json.Json2DT(json);
        }
    }
}
