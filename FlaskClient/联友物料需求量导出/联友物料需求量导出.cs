using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HarveyZ;
using System.IO;

namespace 联友物料需求量导出
{
    public partial class 联友物料需求量导出 : Form
    {
        private Mssql mssql = new Mssql();
        private DataTable showDt = new DataTable();
        private string connStrWg = "Server=40.73.246.171;initial catalog=WG_DB;user id=sa;password=DGlsdnkj168;Connect Timeout=5";
        private string CreateDate = "";

        public 联友物料需求量导出()
        {
            InitializeComponent();
            Form_MainResized_Work();

            BtnData2Excel.Enabled = false;
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
            FormWidth = Width - 20;
            FormHeight = Height - 40;
            PanelTitle.Size = new Size(FormWidth, PanelTitle.Height);
            DgvList.Location = new Point(2, PanelTitle.Height + 2);
            DgvList.Size = new Size(FormWidth, FormHeight - PanelTitle.Height - 2);
        }
        #endregion

        private void BtnGetData_Click(object sender, EventArgs e)
        {
            DgvList.DataSource = null;
            BtnData2Excel.Enabled = false;

            string sqlstr = @"select substring(T.CreateDate, 1, 4) + '-' + substring(T.CreateDate, 5, 2) + '-' + substring(T.CreateDate, 7, 2) 生成日期, 
                                substring(D.PlanDate, 1, 4) + '-' + substring(D.PlanDate, 5, 2) + '-' + substring(D.PlanDate, 7, 2) 排程日期, 
                                D.Material 品号, D.MaterialName 品名, D.MaterialSpec 规格, D.NeedNum 需求量, D.Unit 单位, 
                                material.kcsl 库存数量 

                                from LY_MaterialList_T as T 
                                inner join LY_MaterialList_D as D on T.CreateDate = D.CreateDate 
                                left join [lserp-LY].dbo.material as material on material.wlno = D.Material COLLATE Chinese_PRC_CS_AS
                                where T.Status = 'OK' 
                                and T.CreateDate = '20190329' 
                                order by T.CreateDate, D.PlanDate, D.Material ";

            CreateDate = DtpDate.Value.ToString("yyyyMMdd");

            showDt = mssql.SQLselect(connStrWg, string.Format(sqlstr, CreateDate));
            if(showDt != null)
            {
                DgvList.DataSource = showDt;
                BtnData2Excel.Enabled = true;

                //行颜色
                DgvOpt.SetRowColor(DgvList);
            }
            else
            {
                MessageBox.Show("没有查询到数据", "错误", MessageBoxButtons.OK);
            }
        }

        private void BtnData2Excel_Click(object sender, EventArgs e)
        {
            Excel.Excel_Base excelObj = new Excel.Excel_Base();


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Excel 2007|*.xlsx";
            saveFileDialog.FileName = "联友物料需求量导出_生成日期" + CreateDate + "_导出日期" + DateTime.Now.ToString("yyyyMMdd");
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dttmp = (DataTable)DgvList.DataSource;

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
