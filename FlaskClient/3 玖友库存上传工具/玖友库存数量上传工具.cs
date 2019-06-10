using System;
using System.Data;
using System.Windows.Forms;
using HarveyZ;
using System.Threading;

namespace 玖友库存数量上传工具
{
    public partial class 玖友库存数量上传工具 : Form
    {
        private static string connJY = Global_Const.strConnection_JY;
        private static string connYWG = Global_Const.strConnection_Y_WGDB;
        private static Mssql mssql = new Mssql();
        
        System.Timers.Timer t = new System.Timers.Timer(900000); //设置时间间隔为15分钟

        private void GetData()
        {
            var timer = new System.Timers.Timer();
            timer.Interval = 900000;
            timer.Enabled = true;
            timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；  
            timer.Start();
            timer.Elapsed += (o, a) =>
            {
                SetData();
            };
        }

        //声明委托
        private delegate void UploadDelegate();
        private void SetData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UploadDelegate(Upload));
            }
            else
            {
                
            }
        }

        public 玖友库存数量上传工具()
        {
            InitializeComponent();
        }

        private void 玖友库存数量上传工具_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(GetData));
            t.IsBackground = true;
            t.Start();
        }

        private void DgvShow()
        {
            string sqlstr = @"select distinct khwlno as wlno, name, spec, convert(float, kcsl) kysl from material
                                left join khlh on khlh.wlno = material.wlno 
                                where 1=1 
                                and material.wllx = '成品'  and khno = 'C001'
                                and khwlno is not null and rtrim(khwlno) != '' 
                                order by khwlno ";

            DataTable showDt = mssql.SQLselect(connJY, sqlstr);
            dataGridView1.DataSource = showDt;
        }

        private void Upload()
        {
            textBox1.Text += "开始作业：" + DateTime.Now + "\n";

            string sqlstrGet = @" select distinct khwlno as wlno, name, spec, convert(float, kcsl) kysl from material
                                left join khlh on khlh.wlno = material.wlno 
                                where 1=1 
                                and material.wllx = '成品' and khno = 'C001'
                                and khwlno is not null and rtrim(khwlno) != '' 
                                order by khwlno ";

            string sqlstrFind = @" select wlno from JY_KYSL where wlno = '{0}' ";
            string sqlstrUpt = @" update JY_KYSL set kysl = '{1}', UpdateDate = '{2}' where wlno = '{0}' ";
            string sqlstrIns = @" insert into JY_KYSL (wlno, kysl, UpdateDate ) Values ('{0}', '{1}', '{2}')";

            DataTable getDt = mssql.SQLselect(connJY, sqlstrGet);

            if (getDt != null)
            {
                string UpdateDate = "";
                try
                {
                    UpdateDate = mssql.SQLselect(connYWG, @"SELECT (CONVERT(VARCHAR(20), GETDATE(), 112) + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 24), ':', ''))").Rows[0][0].ToString();


                    for (int rowIndex = 0; rowIndex < getDt.Rows.Count; rowIndex++)
                    {
                        if (mssql.SQLexist(connYWG, string.Format(sqlstrFind, getDt.Rows[rowIndex][0].ToString())))
                        {
                            mssql.SQLexcute(connYWG, string.Format(sqlstrUpt, getDt.Rows[rowIndex][0].ToString(), getDt.Rows[rowIndex][3].ToString(), UpdateDate));
                        }
                        else
                        {
                            mssql.SQLexcute(connYWG, string.Format(sqlstrIns, getDt.Rows[rowIndex][0].ToString(), getDt.Rows[rowIndex][3].ToString(), UpdateDate));
                        }
                    }
                    textBox1.Text += "上传完成：" + DateTime.Now + "\n";
                }
                catch (Exception es)
                {
                    textBox1.Text += "上传失败：" + DateTime.Now + "\n" + es.ToString() + "\n";
                }
            }
            else
            {
                textBox1.Text += "没有数据：" + DateTime.Now + "\n";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Upload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DgvShow();
        }
    }
}
