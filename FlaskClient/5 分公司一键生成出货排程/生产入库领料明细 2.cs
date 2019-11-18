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
    public partial class 生产入库领料明细 : Form
    {
        private Mssql mssql = new Mssql();
        public 生产入库领料明细()
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
            string sqlstr = @"select (case scdt.jsbz when 1 then '是' when 0 then '否' end) 结案,  
                            ma.wlno 品号, ma.name 品名, ma.spec 规格, 
                            scdt.scdh 生产单号, convert(varchar(20), scdt.scrq, 23) 排单日期, convert(varchar(20), scdt.scjq, 23) 生产交期, scdt.sl 排单数量, 
                            (case when scrk.sl is null then 0 else scrk.sl end) - (case when sctk.sl is null then 0 else sctk.sl end) 入库数量, 
                            scdd.wlno 物料编号, scdd.name 物料名称, scdd.spec 物料规格, convert(numeric(10,4), scdd.bomsl) BOM用量, 
                            scdd.sl 需领料数量, convert(numeric(10,4), scdd.bomsl*((case when scrk.sl is null then 0 else scrk.sl end) - (case when sctk.sl is null then 0 else sctk.sl end))) 至少领用数量, 
                            (case when scnl.sl is null then 0 else scnl.sl end) - (case when sctl.sl is null then 0 else sctl.sl end) 已领数量, 
                            (case when scbl.sl is null then 0 else scbl.sl end) 补料数量, 
                            ma_wl.kcsl 库存数量, ma_wl.unit 单位

                            from material as ma
                            inner join mf_wwd as scdt on scdt.wlno = ma.wlno and scdt.shbz = 1 and scdt.wcbz = 0
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
                            inner join tf_scd as scdd on scdd.scd_no = scdt.scdh
                            inner join material as ma_wl on ma_wl.wlno = scdd.wlno
                            left join (
	                            select tf_ntl.wlno, tf_ntl.indh as scdh, sum(osl) as sl from tf_ntl
	                            inner join mf_ntl on mf_ntl.crkdh = tf_ntl.crkdh and mf_ntl.shbz = 1 and tf_ntl.crklx = 'scnl'
	                            group by tf_ntl.indh, tf_ntl.wlno 
                            ) as scnl on scnl.scdh = scdt.scdh and scnl.wlno = scdd.wlno
                            left join (
	                            select tf_ntl.wlno, tf_ntl.indh as scdh, sum(sl) as sl from tf_ntl
	                            inner join mf_ntl on mf_ntl.crkdh = tf_ntl.crkdh and mf_ntl.shbz = 1 and tf_ntl.crklx = 'sctl'
	                            group by tf_ntl.indh, tf_ntl.wlno 
                            ) as sctl on sctl.scdh = scdt.scdh and sctl.wlno = scdd.wlno
                            left join (
	                            select tf_ntl.wlno, tf_ntl.indh as scdh, sum(osl) as sl from tf_ntl
	                            inner join mf_ntl on mf_ntl.crkdh = tf_ntl.crkdh and mf_ntl.shbz = 1 and tf_ntl.crklx = 'scbl'
	                            group by tf_ntl.indh, tf_ntl.wlno 
                            ) as scbl on sctl.scdh = scdt.scdh and sctl.wlno = scdd.wlno

                            where 1=1

                            order by scdt.scdh, scdd.seq";
            DataTable dtShow = mssql.SQLselect(主界面.connY_Ls, sqlstr);
            DgvMain.DataSource = dtShow;
            DgvOpt.SetRowColor(DgvMain);
            SetRowAlarm(DgvMain);
        }

        private void SetRowAlarm(DataGridView Dgv)
        {
            int colAtLestUse = -1, colUsed = -1;
            for (int colIndex =0; colIndex < Dgv.Columns.Count; colIndex++)
            {
                if (Dgv.Columns[colIndex].Name == "至少领用数量") colAtLestUse = colIndex;
                if (Dgv.Columns[colIndex].Name == "已领数量") colUsed = colIndex;
            }
            if(colUsed != -1 && colAtLestUse != -1)
            {
                for (int rowIndex = 0; rowIndex < Dgv.Rows.Count; rowIndex++)
                {

                }
            }
        }
    }
}
