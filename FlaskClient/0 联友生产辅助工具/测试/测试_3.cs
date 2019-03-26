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
    public partial class 测试_3 : Form
    {
        WebNet webNet = new WebNet();
        Dictionary<string, string> dict = new Dictionary<string, string> { };
        List<string> MenuItem_List = FormLogin.MenuItemList;

        public 测试_3()
        {
            InitializeComponent();
        }


        #region 窗口大小变化设置
        private void FormMain_Resized(object sender, EventArgs e)
        {
            FormMain_Resized_Work();
        }

        public void FormMain_Resized_Work()
        {
            //窗框大小
            int FormWidth, FormHeight;
            FormWidth = Width;
            FormHeight = Height;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            foreach( string kk in MenuItem_List)
            {
                MessageBox.Show(kk);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
