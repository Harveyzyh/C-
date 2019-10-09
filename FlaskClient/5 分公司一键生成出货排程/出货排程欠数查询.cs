using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HarveyZ;
using System.IO;

namespace 联友中山分公司生产辅助工具
{
    public partial class 出货排程欠数查询 : Form
    {
        Mssql mssql = new Mssql();
        string connYLs = 主界面.connY_Ls;

        public 出货排程欠数查询()
        {
            InitializeComponent();
            Form_MainResized_Work();
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

        private void ShowDt()
        {
            string sqlstr = @"select convert(varchar(20), chpd.chrq, 23) as 出货日期, chpd.wlno as 品号, chpd.name as 品名, chpd.spec as 规格, chpd.ys as 大小件, convert(int, chpd.sl) as 出货数量, 
                                (case when ma.kcsl is null then 0 else convert(int, ma.kcsl) end) as 分公司库存, 
                                (case when jy.kysl is null then 0 else convert(int, jy.kysl) end) as 玖友库存, 
                                '' as 累计出货数, (case when qjsl.qs is null then 0 else qjsl.qs end) as 欠数, chpd.unit as 单位

                                from (
	                                select chpdt.khno as khno, chpdd.wlno as wlno, chpdd.name as name, chpdd.spec as spec, chpdd.chrq as chrq, chpdd.unit as unit, chpdd.ys as ys, sum(chpdd.sl) as sl from mf_chpd as chpdt
	                                inner join tf_chpd as chpdd on chpdt.crkdh = chpdd.crkdh and chpdt.shbz = 1
	                                where 1=1
	                                and convert(varchar(20), chpdd.chrq, 112) >= convert(varchar(20), getdate(), 112)
	                                group by chpdt.khno, chpdd.wlno, chpdd.name, chpdd.spec, chpdd.chrq, chpdd.unit, chpdd.ys
                                ) as chpd
                                left join (
	                                select chpdd.wlno as wlno, convert(varchar(20), min(chpdd.chrq), 112) as minchrq from mf_chpd as chpdt
	                                inner join tf_chpd as chpdd on chpdt.crkdh = chpdd.crkdh and chpdt.shbz = 1
	                                where 1=1
	                                and convert(varchar(20), chpdd.chrq, 112) >= convert(varchar(20), getdate(), 112)
	                                group by chpdd.wlno 
                                ) as chpd2 on chpd2.wlno = chpd.wlno
                                left join (
	                                select chpdd.wlno as wlno, convert(int, sum(chpdd.chsl) - sum(chpdd.sl))as qs from tf_chpd as chpdd
	                                inner join mf_chpd as chpdt on chpdt.crkdh = chpdd.crkdh 
	                                where convert(varchar(20), chpdd.chrq, 112) >= '20190501'
	                                and chpdt.shbz = 1 and chpdd.jsbz = 0 and chpdt.jsbz = 0
	                                group by chpdd.wlno
                                ) as qjsl on qjsl.wlno = chpd.wlno
                                left join material as ma on ma.wlno = chpd.wlno
                                left join WG_DB..JY_KYSL as jy on jy.wlno =  chpd.wlno COLLATE Chinese_PRC_CI_AS 
                                where 1=1
                                and chpd.khno = 'C001'
                                order by chpd2.minchrq, chpd.wlno, chpd.chrq";
            DataTable dt =  mssql.SQLselect(connYLs, sqlstr);
            DgvMain.DataSource = null;
            if(dt != null)
            {
                HandelDt(dt);
                DgvMain.DataSource = dt;
                DgvOpt.SetRowColor(DgvMain);
            }
        }

        private void HandelDt(DataTable dt)
        {
            string wlno = "";
            for(int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                if(dt.Rows[rowIndex][1].ToString() != wlno)
                {
                    wlno = dt.Rows[rowIndex][1].ToString();
                    dt.Rows[rowIndex][8] = dt.Rows[rowIndex][5];
                }
                else
                {
                    dt.Rows[rowIndex][8] = int.Parse(dt.Rows[rowIndex][5].ToString()) + int.Parse(dt.Rows[rowIndex - 1][8].ToString());
                }
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            ShowDt();
        }

        private void BtnLayout_Click(object sender, EventArgs e)
        {
            Excel.Excel_Base excelObj = new Excel.Excel_Base();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Excel 2007|*.xlsx";
            saveFileDialog.FileName = "出货排程导出_" + DateTime.Now.ToString("yyyy-MM-dd");
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dttmp = (DataTable)DgvMain.DataSource;

                    excelObj.FilePath = Path.GetDirectoryName(saveFileDialog.FileName);
                    excelObj.FileName = Path.GetFileName(saveFileDialog.FileName);
                    excelObj.IsWrite = true;
                    excelObj.CellDt = dttmp;

                    Excel excel = new Excel();
                    excel.ExcelOpt(excelObj);
                    MessageBox.Show("Excel导出成功！", "提示");
                }
                catch (IOException)
                {
                    MessageBox.Show("文件保存失败,请确保该文件没被打开！", "错误");
                }
            }
        }
    }
}
