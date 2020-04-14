using System;
using System.Data;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.仓储中心
{
    public partial class 生成领料单_获取单据信息 : Form
    {
        Mssql mssql = new Mssql();
        string strConnection = 生成领料单.strConnection;
        DataTable dt = new DataTable();
        int Index = 0;
        
        private string Title = 生成领料单.Title;
        private string Mode = 生成领料单.Mode;

        public 生成领料单_获取单据信息()
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
            dataGridView1.Select();
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Seach_Work();
                dataGridView1.Select();
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

        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Index = dataGridView1.CurrentRow.Index;
                PDA_扫描进货单.GetMain = dt.Rows[Index][0].ToString();
                PDA_扫描进货单.GetOther = dt.Rows[Index][1].ToString();
                this.Close();
            }
        }
    }
}
