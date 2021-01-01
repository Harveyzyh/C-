using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HarveyZ
{
    public partial class 手册下载 : Form
    {
        private string url = null;
        public 手册下载(string text, string url)
        {
            InitializeComponent();
            this.Text = text == "" ? this.Text : text;
            if(url != null)
            {
                if (url != "")
                {
                    this.url = url;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileUrl = FormLogin.infObj.updateHost + @"/downloadFile/" + this.url;
            System.Diagnostics.Process.Start(fileUrl);
        }
    }
}
