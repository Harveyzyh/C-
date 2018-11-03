using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;

namespace 多线程_多窗口_sqlite
{ 
    public partial class Form1 : Form
    {
        Form[] frmList;

        #region 窗口大小变化设置
        private void Form_MainResized(object sender, EventArgs e)
        {
            Form_MainResized_Work();
        }

        private void Form_MainResized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width - 18;
            FormHeight = Height - 16;
            panel1.Size = new Size(FormWidth, FormHeight - 80);
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
            Form_MainResized_Work();
            
            frmList = new Form[20];
            for (int a = 0; a < 20; a++)
            {
                frmList[a] = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Excel文件|*.xlsx;*.xls";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable dttmp = null;
                try
                {
                    dttmp = Excel.ImportExcel(openFileDialog.FileName);
                    Form2 frm2 = new Form2(dttmp);
                    frm2.TopLevel = false;
                    frm2.FormBorderStyle = FormBorderStyle.None;
                    frm2.Dock = DockStyle.Fill;
                    panel1.Controls.Add(frm2);
                    frm2.Width = panel1.Width;
                    frm2.Height = panel1.Height;
                    frmList[0] = frm2;
                    frm2.Show();
                }
                catch(System.IO.IOException)
                {
                    MessageBox.Show("文件已被其他程序打开！", "Excel打开错误", MessageBoxButtons.OK);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 frm_tmp = new Form3();
            Form frm = Form_Opt.Form_Init(frm_tmp);
            panel1.Controls.Add(frm);
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            kk(frmList[0]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("name", "you ");
            dict = WebNet.WebPost("http://192.169.1.12/client", dict);
            if(dict != null)
            {
                //MessageBox.Show(dict.Keys.ToString(), "");
                string ll;
                dict.TryGetValue("name", out ll);
                MessageBox.Show(ll, "");
            }
            else
            {
                MessageBox.Show("NULL", "");
            }
        }

        private void kk(Form obj)
        {
            panel1.Controls.Remove(obj);
        }        

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dttmp = new DataTable();
            Form frm = new Form();
        }
    }
}
