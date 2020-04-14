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
    public partial class 生产日入库明细 : Form
    {
        private Mssql mssql = new Mssql();
        private string connY_Ls = 主界面.connY_Ls;

        public 生产日入库明细()
        {
            InitializeComponent();
            Form_MainResized_Work();
            DgvShow();
        }

        #region 主窗体按钮
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) //窗体上的关闭按钮
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

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
            PanelTitle.Location = new Point(2, 2);
            PanelTitle.Size = new Size(FormWidth, PanelTitle.Height);
            DgvMain.Location = new Point(2, PanelTitle.Height + 3);
            DgvMain.Size = new Size(FormWidth, FormHeight - PanelTitle.Height - 6);
        } 
        #endregion

        private void DgvShow()
        {
            string sqlstr = @"declare @day1 varchar(20), @day2 varchar(20), @day3 varchar(20)
                                set @day2 = convert(varchar(20), getdate() - 1, 112)
                                set @day1 = convert(varchar(20), getdate(), 112)

                                select ma.wlno 品号, ma.name 品名, ma.spec 规格, 
                                scdt.scdh 生产单号, convert(varchar(20), scdt.scrq, 23) 排单日期, convert(varchar(20), scdt.scjq, 23) 生产交期, scdt.sl 排单数量, 
                                (case when scrk.sl is null then 0 else scrk.sl end) - (case when sctk.sl is null then 0 else sctk.sl end) 总入库数量, 
                                (case when scdt.sl - (scrk.sl - sctk.sl) is null then scdt.sl else scdt.sl - (scrk.sl - sctk.sl) end) 欠数,
                                (case when sctk_day2.sl is null then 0 else sctk_day2.sl end) as 昨日入库量,
                                (case when sctk_day1.sl is null then 0 else sctk_day1.sl end) as 今日入库量, 
                                ma.unit 单位

                                from material as ma
                                inner join mf_wwd as scdt on scdt.wlno = ma.wlno and scdt.shbz = 1 and scdt.jsbz = 0
                                left join (
	                                select distinct scrkd.wlno, scrkd.indh as scdh, sum(scrkd.sl) as sl from tf_scrk as scrkd
	                                inner join mf_scrk as scrkt on scrkd.crkdh = scrkt.crkdh and scrkt.shbz = 1
	                                group by scrkd.wlno, scrkd.indh 
                                ) as scrk on scrk.wlno = ma.wlno and scrk.scdh = scdt.scdh
                                left join (
	                                select distinct scrkd.wlno, scrkd.indh as scdh, sum(scrkd.osl) as sl from tf_scrk as scrkd
	                                inner join mf_scrk as scrkt on scrkd.crkdh = scrkt.crkdh and scrkt.shbz = 1
	                                group by scrkd.wlno, scrkd.indh 
                                ) as sctk on sctk.wlno = ma.wlno and sctk.scdh = scdt.scdh
                                left join (
                                select distinct scrkd.wlno, scrkd.indh as scdh, convert(varchar(20), scrkt.shrq, 112) as shrq, sum(scrkd.sl) - sum(scrkd.osl) as sl from tf_scrk as scrkd
	                                inner join mf_scrk as scrkt on scrkd.crkdh = scrkt.crkdh and scrkt.shbz = 1 
	                                group by scrkd.indh, scrkd.wlno, convert(varchar(20), scrkt.shrq, 112)
                                ) as sctk_day2 on sctk_day2.wlno = ma.wlno and sctk_day2.scdh = scdt.scdh and sctk_day2.shrq = @day2
                                left join (
                                select distinct scrkd.wlno, scrkd.indh as scdh, convert(varchar(20), scrkt.shrq, 112) as shrq, sum(scrkd.sl) - sum(scrkd.osl) as sl from tf_scrk as scrkd
	                                inner join mf_scrk as scrkt on scrkd.crkdh = scrkt.crkdh and scrkt.shbz = 1 
	                                group by scrkd.indh, scrkd.wlno, convert(varchar(20), scrkt.shrq, 112)
                                ) as sctk_day1 on sctk_day1.wlno = ma.wlno and sctk_day1.scdh = scdt.scdh and sctk_day1.shrq = @day1
                                where 1=1
                                and scdt.crklx = 'SCD'
                                order by scdt.scdh";
            DataTable showDt = mssql.SQLselect(connY_Ls, sqlstr);
            DgvMain.DataSource = null;
            if (showDt != null)
            {
                DgvMain.DataSource = showDt;
                DgvOpt.SetColReadOnly(DgvMain);
                DgvOpt.SetRowColor(DgvMain);
                DgvOpt.SetColTextMiddleCenter(DgvMain);
            }
        }
    }
}
