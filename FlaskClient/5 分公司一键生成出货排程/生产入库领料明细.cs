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
    public partial class 生产入库领料明细 : Form
    {
        private Mssql mssql = new Mssql();
        public 生产入库领料明细()
        {
            InitializeComponent();
            Form_MainResized_Work();
            dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
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
            string StartDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            string EndDate = dateTimePicker2.Value.ToString("yyyyMMdd");

            string sqlstr = @" select * from dbo.v_scntbl where 1=1 ";

            sqlstr += @" and [排单日期] between '" + StartDate + "' and '" + EndDate + "' ";
            sqlstr += @" and [生产单号] like '%" + TxbScdh.Text.Trim() + "%'" ;

            sqlstr += @" order by [生产单号], [物料编号]"; //通过视图查询，有问题修改视图即可
            DataTable dtShow = mssql.SQLselect(主界面.connY_Ls, sqlstr);
            DgvMain.DataSource = dtShow;
            DgvOpt.SetRowColor(DgvMain);
            SetRowAlarm(DgvMain);

            if(dtShow != null)
            {
                DgvMain.Columns[0].Width = 50;
                DgvMain.Columns[1].Width = 50;
            }
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
                    if (float.Parse(Dgv.Rows[rowIndex].Cells[colAtLestUse].Value.ToString()) > float.Parse(Dgv.Rows[rowIndex].Cells[colUsed].Value.ToString()))
                    {
                        Dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void BtnLayout_Click(object sender, EventArgs e)
        {
            Excel.Excel_Base excelObj = new Excel.Excel_Base();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Excel 2007|*.xlsx";
            saveFileDialog.FileName = "生产入库领料明细_" + DateTime.Now.ToString("yyyy-MM-dd");
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

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            DgvShow();
        }
    }
}
