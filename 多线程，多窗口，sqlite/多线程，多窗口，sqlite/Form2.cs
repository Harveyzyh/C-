using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


    public partial class Form2 : Form
    {
        public Form2(DataTable dttmp)
        {
            InitializeComponent();
            dataGridView1.DataSource = dttmp;
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

            dataGridView1.Size = new Size(FormWidth, FormHeight);
        }
        #endregion
    }

