using System;
using System.Windows.Forms;
using HarveyZ;
using System.Threading;
using System.Threading.Tasks;

namespace 测试
{
    public partial class Form1 : Form
    {
        string connYWGDB = Global_Const.strConnection_Y_WGDB;
        string connCOMFORT = Global_Const.strConnection_Y_COMFORT;
        string connRobot = Global_Const.strConnection_ROBOT;
        string connTest = "Server=192.168.20.188;initial catalog=lserp-JY;user id=sa;password=lsdnkj;Connect Timeout=5";
        Mssql mssql = new Mssql();

        private static bool ConnectFlag = false;
        private static bool DoneFlag = false;

        private delegate string SqlTestDelegate(string sqlConn);
        
        public Form1()
        {
            InitializeComponent();
            SqlTestDelegate sqlTestDelegate = SqlTest;
            IAsyncResult asyncResult = sqlTestDelegate.BeginInvoke(connTest, null, null);
            toolStripStatusLabel1.Text = sqlTestDelegate.EndInvoke(asyncResult);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //toolStripStatusLabel1.Text = "ceshi";
            toolStripStatusLabel1.Width = 500;
            toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolStripStatusLabel2.AutoSize = false;
            toolStripStatusLabel2.Width = 200;
            toolStripStatusLabel2.Text = "未连接";
        }

        
        protected void button1_Click(object sender, EventArgs e)
        {

        }

        private string SqlTest(string connStr)
        {
            toolStripStatusLabel2.Text = "未连接";
            ConnectFlag = mssql.SQLlinkTest(connStr);
            toolStripStatusLabel2.Text = "已连接";
            return "20";
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
