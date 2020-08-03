using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友生产辅助工具.生管排程
{
    public partial class 生产排程修改 : Form
    {
        private string connWG = FormLogin.infObj.connWG;
        Mssql mssql = new Mssql();

        private string index, mode;
        public static string indexRtn = "";

        public 生产排程修改(string mode, string index, string dd, string ph, string pm, string gg, string pz, string rq, string bm, string sl)
        {
            InitializeComponent();

            textBoxDd.ReadOnly = true;
            this.mode = mode;
            this.index = index;
            indexRtn = index;

            label1.Text = "排程序号：" + index;
            textBoxDd.Text =  dd;
            label3.Text = "品号：" + ph;
            label4.Text = "品名：" + pm;
            label5.Text = "规格：" + gg;
            label6.Text = "配置方案：" + pz;
            dateTimePicker1.Text = rq;
            comboBoxDpt.Text = bm;
            textBoxSl.Text = sl;
            
            Init();

            if (mode == "New")
            {
                label1.Visible = false;
                textBoxDd.ReadOnly = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
            }
            if (mode == "Edit")
            {
                label1.Visible = true;
                textBoxDd.ReadOnly = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (mode == "Edit") BtnSaveEditWork();
            else if (mode == "New") BtnSaveNewWork();
            else this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Init()
        {
            string sqlstr = "SELECT Dpt FROM dbo.SC_PLAN_DPT_TYPE WHERE Valid = 1 AND Type = 'In' ORDER BY K_ID";
            DataTable dt = mssql.SQLselect(connWG, sqlstr);
            if (dt != null)
            {
                for (int rowIdx = 0; rowIdx < dt.Rows.Count; rowIdx++)
                {
                    comboBoxDpt.Items.Add(dt.Rows[rowIdx][0].ToString());
                }
            }

            if (comboBoxDpt.SelectedItem == null) comboBoxDpt.SelectedIndex = 0;
        }

        private void BtnSaveNewWork()
        {
            if (textBoxDd.Text != "")
            {
                string sqlstr = @"INSERT INTO WG_DB.dbo.SC_PLAN (CREATOR, CREATE_DATE, K_ID, SC001, SC003, SC013, SC014, SC023) 
                                VALUES ('{0}', LEFT(COMFORT.dbo.f_getTime(1), 14), (SELECT ISNULL(MAX(K_ID), 0) + 1 FROM WG_DB.dbo.SC_PLAN), 
                                '{1}', '{2}', {3}, 0, '{4}') ";
                string sqlstr2 = @"SELECT ISNULL(MAX(K_ID), -1) FROM dbo.SC_PLAN WHERE SC001 = '{0}' AND SC003 = '{1}' AND SC013 = {2} AND SC023 = '{3}' ";
                try
                {
                    float sl = float.Parse(textBoxSl.Text);
                    Msg.Show(string.Format(sqlstr, FormLogin.infObj.userId, textBoxDd.Text, dateTimePicker1.Value.ToString("yyyyMMdd"), textBoxSl.Text, comboBoxDpt.SelectedItem.ToString()));
                    mssql.SQLexcute(connWG, string.Format(sqlstr, FormLogin.infObj.userId, textBoxDd.Text, dateTimePicker1.Value.ToString("yyyyMMdd"), textBoxSl.Text, comboBoxDpt.SelectedItem.ToString()));
                    indexRtn = mssql.SQLselect(connWG, string.Format(sqlstr2, textBoxDd.Text, dateTimePicker1.Value.ToString("yyyyMMdd"), textBoxSl.Text, comboBoxDpt.SelectedItem.ToString())).Rows[0][0].ToString();

                    this.Close();
                }
                catch
                {
                    Msg.ShowErr("数量输入不正确");
                }
            }
            else
            {
                Msg.ShowErr("订单号不能为空");
            }
        }

        private void BtnSaveEditWork()
        {
            string sqlstr = @"UPDATE dbo.SC_PLAN SET SC003 = '{2}', SC013 = {3}, SC023 = '{4}' WHERE K_ID = '{0}' AND SC001 = '{1}'";
            try
            {
                float sl = float.Parse(textBoxSl.Text);
                mssql.SQLexcute(connWG, string.Format(sqlstr, index, textBoxDd.Text, dateTimePicker1.Value.ToString("yyyyMMdd"), textBoxSl.Text, comboBoxDpt.SelectedItem.ToString()));
                this.Close();
            }
            catch
            {
                Msg.ShowErr("数量输入不正确");
            }
        }
    }
}
