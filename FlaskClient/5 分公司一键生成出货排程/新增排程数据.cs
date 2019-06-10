using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;

namespace 联友中山分公司生产辅助工具
{
    public partial class 新增排程数据 : Form
    {
        private static Mssql mssql = new Mssql();
        private static string connYLs = 主界面.connY_Ls;

        public static bool modeAdd = true;
        public static string date = DateTime.Now.ToString("yyyyMMdd");
        public static string scdh = "";
        public static string sl = "";
        private static string maxsl = "";
        
        public 新增排程数据()
        {
            InitializeComponent();
        }

        private void 新增排程数据_Load(object sender, EventArgs e)
        {
            if (!modeAdd)
            {
                BtnAdd.Enabled = false;
                TxbScdh.Text = scdh;
                TxbSl.Text = sl;
                DtpDate.Text = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString();
                TxbScdh.Enabled = false;
                TxbSl.Enabled = false;
                DtpDate.Enabled = false;
                ShowDgv(scdh);
            }
            else
            {
                BtnAdd.Enabled = false;
                TxbScdh.Text = "";
                TxbSl.Text = "";
                DtpDate.Value = DateTime.Now;
                TxbScdh.Enabled = true;
                TxbSl.Enabled = true;
                DtpDate.Enabled = true;
                DgvMain.DataSource = null;
            }
        }

        private void AddNewLine(string date, string scdh, string sl)
        {
            string sqlstr = @"insert into zzzz_chpc(chrq, scdh, sl) values(convert(datetime, '{0}', 112), '{1}', '{2}')";
            mssql.SQLexcute(connYLs, string.Format(sqlstr, date, scdh, sl));
            TxbSl.Text = "";
            MessageBox.Show("已新增", "提示");
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            ShowDgv(TxbScdh.Text);
            DataTable dt = 生成出货排程.GetBaseDt(TxbScdh.Text);
            if(int.Parse(dt.Rows[0][1].ToString()) > 0)
            {
                if (int.Parse(dt.Rows[0][3].ToString()) == 0)
                {
                    if (int.Parse(dt.Rows[0][4].ToString()) <= 5)
                    {
                        LabelMsg.Text = "最大可填套数为 " + dt.Rows[0][5].ToString();
                        maxsl = dt.Rows[0][5].ToString();
                        if(modeAdd) TxbSl.Enabled = true;
                    }
                    else
                    {
                        LabelMsg.Text = "不可成套";
                        TxbSl.Text = "-1";
                        TxbSl.Enabled = false;
                    }
                }
                else
                {
                    LabelMsg.Text = "不可成套";
                    TxbSl.Text = "-1";
                    TxbSl.Enabled = false;
                }
                if (modeAdd) BtnAdd.Enabled = true;
            }
            else
            {
                LabelMsg.Text = "没有未排数量";
                TxbSl.Enabled = false;
                BtnAdd.Enabled = false;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string addSl = "0";
            if (!TxbSl.Enabled)
            {
                addSl = "-1";
            }
            else
            {
                addSl = TxbSl.Text;
            }
            try
            {
                if (addSl == "-1")
                {
                    AddNewLine(DtpDate.Value.ToString("yyyyMMdd"), TxbScdh.Text, addSl);
                    BtnAdd.Enabled = false;
                }
                else
                {
                    if (int.Parse(maxsl) >= int.Parse(addSl))
                    {
                        AddNewLine(DtpDate.Value.ToString("yyyyMMdd"), TxbScdh.Text, addSl);
                        BtnAdd.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("填入数值大于最大可填套数， 请重新填入", "错误");
                    }
                }
            }
            catch
            {
                MessageBox.Show("填入正确的数值", "错误");
            }
        }

        private void ShowDgv(string scdh)
        {
            string sqlstr = @"select distinct posd.wlno 品号, posd.name 品名, posd.ys 大件产品, posd.spec 规格, posd.unit 单位, 
                                posd.dddh as 联友采购单号, posd.bz 备注, posd.csdh 生产单号, posd.ck 仓库, 
                                posd.sl 订单数量, (posd.sl - (case when chpdd.sl is null then 0 else chpdd.sl end)) as 未录入排程数量, 
                                post.cgdh as 订单号, posd.seq as 订单序号 
                                from mf_pos as post
                                inner join tf_pos as posd on post.cgdh = posd.cgdh
                                left join (
	                                select wlno, indh, inseq, sum(sl) as sl from tf_chpd group by wlno, indh, inseq
                                ) as chpdd on chpdd.wlno = posd.wlno and chpdd.indh = posd.cgdh and chpdd.inseq = posd.seq
                                left join (select '' as crkdh) as dh on 1=1
                                where 1=1
                                and post.polx = 'PA' -- 客户订单
                                and post.gysno = 'C001'
                                and post.shbz = 1
                                and posd.jsbz = 0
                                and post.jsbz = 0
                                and posd.dddh like '3301%'
                                and posd.csdh = '{0}'
                                order by posd.csdh
                                ";
            DataTable dt = mssql.SQLselect(connYLs, string.Format(sqlstr, scdh));
            DgvMain.DataSource = null;
            if(dt != null)
            {
                DgvMain.DataSource = dt;
                DgvOpt.SetRowColor(DgvMain);
            }
        }
    }
}
