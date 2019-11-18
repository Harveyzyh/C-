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
    public partial class 玖友库存 : Form
    {
        Mssql mssql = new Mssql();
        string connYLs = Global_Const.strConnection_Y_LS;

        public 玖友库存()
        {
            InitializeComponent();
            Form_MainResized_Work();
            ShowDgv();
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

        private void ShowDgv()
        {
            string sqlstr = @"select updatedate 库存更新时间, wlno 物料品号, name 品名, spec 规格, fgskc 分公司库存, jykc 玖友库存, unit 单位, wllx 物料类型 
                            from
                            (
                            select jy.UpdateDate as updatedate, jy.wlno as wlno, ma.name as name, ma.spec as spec, ma.kcsl as fgskc, 
                            convert(numeric(10, 4), jy.kysl) as jykc, ma.unit as unit, ma.wllx as wllx from WG_DB..JY_KYSL as jy 
                            left join material as ma on jy.wlno = ma.wlno collate chinese_prc_ci_as 
                            )  as a
                            where name is not null
                            order by wlno";

            DataTable showDt = mssql.SQLselect(connYLs, sqlstr);
            if (showDt != null)
            {
                DgvMain.DataSource = showDt;
                DgvOpt.SetRowColor(DgvMain);
            }
        }
    }
}
