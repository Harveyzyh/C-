using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Services;
using HarveyZ;
using System.Collections;
using System.Web.Script.Serialization;

namespace 联友生产辅助工具.仓储中心
{
    public partial class PDA_扫描进货单_获取单据信息 : Form
    {
        Mssql mssql = new Mssql();
        string strConnection = PDA_扫描进货单.strConnection;
        DataTable dt = new DataTable();
        int Index = 0;

        //private WebNet webNet = new WebNet();
        //private Dictionary<string, string> dict = new Dictionary<string, string> { };
        //private Dictionary<string, string> dict_get = new Dictionary<string, string> { };
        private string Title = PDA_扫描进货单.Title;
        private string Mode = PDA_扫描进货单.Mode;
        //private string URL = PDA_扫描进货单.URL;

        //private string ReturnData = null;

        public PDA_扫描进货单_获取单据信息()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if(Title != null)
            {
                var TitleList = Title.Split('|');
                comboBox1.Items.AddRange(TitleList);
                comboBox1.SelectedIndex = 0;
                textBox1.Select();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Seach_Work();
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Seach_Work();
            }
        }

        private void Seach_Work()
        {
            if (Mode == "TypeID")
            {
                GetTypeID();
            }
            else if (Mode == "SupplierID")
            {
                GetSupplierID();
            }
            else if (Mode == "PositionID")
            {
                GetPositionID();
            }
        }

        private void GetTypeID()
        {
            dataGridView1.DataSource = null;
            string sqlstr = ("SELECT RTRIM(MQ001) 单别, RTRIM(MQ002) 单据名称, RTRIM(MQ019) 核对采购 "
                            + "FROM CMSMQ "
                            + "WHERE 1=1 "
                            + "AND MQ003 = '34' AND MQ004 = '2'"
                            + "AND MQ001 LIKE '%{0}%' "
                            + "ORDER BY MQ001 ");
            dt = mssql.SQLselect(strConnection, string.Format(sqlstr, textBox1.Text.Trim()));
            if(dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 200;
                dataGridView1.Columns[1].Width = 300;
            }
        }

        private void GetSupplierID()
        {
            dataGridView1.DataSource = null;
            string sqlstr = ("SELECT RTRIM(MA001) 供应商编号, RTRIM(MA002) 简称 FROM COMFORT..PURMA "
                            + "WHERE MA001 LIKE '%{0}%' "
                            + "ORDER BY MA001 ");
            dt = mssql.SQLselect(strConnection, string.Format(sqlstr, textBox1.Text.Trim()));
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 200;
                dataGridView1.Columns[1].Width = 300;
            }
        }

        private void GetPositionID()
        {
            dataGridView1.DataSource = null;
            string sqlstr = ("SELECT RTRIM(MC001) 仓库编号, RTRIM(MC002) 仓库名称, RTRIM(MC003) 工厂编号 "
                            + "FROM COMFORT..CMSMC "
                            + "WHERE MC001 LIKE '%{0}%' "
                            + "ORDER BY LEN(MC001) DESC, MC001");
            dt = mssql.SQLselect(strConnection, string.Format(sqlstr, textBox1.Text.Trim()));
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 200;
                dataGridView1.Columns[1].Width = 300;
            }
        }

        private void CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Index = dataGridView1.Rows[e.RowIndex].Index;
            PDA_扫描进货单.GetMain = dt.Rows[Index][0].ToString();
            PDA_扫描进货单.GetOther = dt.Rows[Index][1].ToString();
            this.Close();
        }
    }
}
