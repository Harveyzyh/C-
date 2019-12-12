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
        Mssql mssql = new Mssql();

        private static bool ConnectFlag = false;
        private static bool DoneFlag = false;

        private delegate string SqlTestDelegate(string sqlConn);
        
        public Form1()
        {
            InitializeComponent();
            //SqlTestDelegate sqlTestDelegate = SqlTest;
            //IAsyncResult asyncResult = sqlTestDelegate.BeginInvoke(connTest, null, null);
            //toolStripStatusLabel1.Text = sqlTestDelegate.EndInvoke(asyncResult);
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
            mssql.SQLexcute(connRobot, "EXEC dbo.SplitCodeUpdate");
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
            MessageBox.Show(Num2Char("676"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Num2Char("673"));
        }

        private string ChangeNum2Char(int num)
        {
            string returnStr = "";
            byte[] array = new byte[1];
            array[0] = (byte)(Convert.ToInt32(num + 64));//ASCII码强制转换二进制
            returnStr = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));

            if(returnStr == "@")
            {
                returnStr = "Z";
            }
            return returnStr;
        }

        private string Num2Char(string num2 = null)
        {
            string returnStr = "";

            if(num2 == null)
            {
                return null;
            }
            else
            {
                int num = int.Parse(num2);
                int shang = num / 26;
                int yu = num % 26;

                if( shang ==1 && yu == 0)
                {
                    returnStr += ChangeNum2Char(26);
                }
                else if( shang != 0)
                {
                    returnStr += Num2Char(shang.ToString());
                    returnStr += ChangeNum2Char(yu);
                }
                else
                {
                    returnStr += ChangeNum2Char(yu);
                }
                return returnStr;
            }
        }
    }
}
