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
using DataGridViewAutoFilter;

namespace 联友生产辅助工具.中山分公司
{
    public partial class 联友物料需求量导出 : Form
    {
        DataGridViewFunction Get = new DataGridViewFunction();
        private Mssql mssql = new Mssql();
        private DataTable showDt = new DataTable();
        private string ConnLsWg = Global_Const.strConnection_Y_WGDB;
        private string GenerateDate = "";

        public 联友物料需求量导出()
        {
            InitializeComponent();
            Form_MainResized_Work();
            CheckConnFlagY();

            BtnData2Excel.Enabled = false;
            DtpPlanStartDate.Checked = false;
            DtpPlanEndDate.Checked = false;
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

        private void CheckConnFlagY()
        {
            if (!FormLogin.connFlagY)
            {
                PanelTitle.Enabled = false;
            }
        }

        private void ShowDgv()
        {
            DgvMain.DataSource = null;
            BtnData2Excel.Enabled = false;

            GenerateDate = DtpGenerateDate.Value.ToString("yyyyMMdd");

            string sqlstr = @"select distinct substring(T.GenerateDate, 1, 4) + '-' + substring(T.GenerateDate, 5, 2) + '-' + substring(T.GenerateDate, 7, 2) 数据生成日期, 
                                substring(D.PlanDate, 1, 4) + '-' + substring(D.PlanDate, 5, 2) + '-' + substring(D.PlanDate, 7, 2) 联友排程日期, 
                                /*D.WorkDpt 联友生产部门, */D.Material 联友品号, D.Name 联友品名, D.Spec 联友规格, 
								material.name 中山分公司品名, material.Spec 中山分公司规格, D.Unit 单位,  D.NeedNum 联友需求量,
								CONVERT(FLOAT, K.kysl) 玖友库存量, CONVERT(FLOAT, material.kcsl) 分公司库存量, 
								CONVERT(FLOAT, material.AQSL) 分公司安全库存, CONVERT(FLOAT, material.kysl) 分公司可用库存, 
                                CONVERT(FLOAT, material.scwrksl) 分公司未缴库数, CONVERT(FLOAT, material.wcksl) 分公司未出货数   
                                from LY_MaterialList_T as T 
                                inner join LY_MaterialList_D as D on T.GenerateDate = D.GenerateDate 
                                left join [lserp-LY].dbo.material as material on material.wlno = D.Material COLLATE Chinese_PRC_CS_AS
																left join JY_KYSL as K on K.wlno = D.Material
                                where T.Status = 'OK' ";
            sqlstr += " and T.GenerateDate = '" + DtpGenerateDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpPlanStartDate.Checked) sqlstr += " and D.PlanDate >= '" + DtpPlanStartDate.Value.ToString("yyyyMMdd") + "' ";
            if (DtpPlanEndDate.Checked) sqlstr += " and D.PlanDate <= '" + DtpPlanEndDate.Value.ToString("yyyyMMdd") + "' ";
            if (Material.Text != "") sqlstr += " and D.Material like '%" + Material.Text + "%' ";
            if (MaterialName.Text != "") sqlstr += " and (D.Name like '%" + MaterialName.Text + "%' or material.name like '%" + MaterialName.Text + "%') ";
            if (MaterialSpec.Text != "") sqlstr += " and (D.Spec like '%" + MaterialSpec.Text + "%' or material.Spec like '%" + MaterialSpec.Text + "%') ";



            sqlstr += @" order by substring(T.GenerateDate, 1, 4) + '-' + substring(T.GenerateDate, 5, 2) + '-' + substring(T.GenerateDate, 7, 2) , 
                                substring(D.PlanDate, 1, 4) + '-' + substring(D.PlanDate, 5, 2) + '-' + substring(D.PlanDate, 7, 2) , 
                                /*D.WorkDpt , */D.Material , D.Name , D.Spec ,
                                material.name , material.Spec , D.Unit , D.NeedNum ,
                               CONVERT(FLOAT, K.kysl) , CONVERT(FLOAT, material.kcsl) ,
                               CONVERT(FLOAT, material.AQSL) , CONVERT(FLOAT, material.kysl) ,
                               CONVERT(FLOAT, material.scwrksl) , CONVERT(FLOAT, material.wcksl) ";


            showDt = mssql.SQLselect(ConnLsWg, sqlstr);
            if (showDt != null)
            {
                //Get.GridViewDataLoad(showDt, DgvMain);
                //Get.GridViewHeaderFilter(DgvMain);
                DgvMain.DataSource = showDt;
                BtnData2Excel.Enabled = true;

                DgvRowAlarm(DgvMain);
            }
            else
            {
                MessageBox.Show("没有查询到数据", "错误", MessageBoxButtons.OK);
            }
        }

        private void DgvRowAlarm(DataGridView Dgv)
        {
            int colJYkysl = 0, colLYneedNum = 0, colFGSkysl = 0;
            for (int colIndex = 0; colIndex < Dgv.ColumnCount; colIndex++)
            {
                if (Dgv.Columns[colIndex].Name == "玖友库存量") { colJYkysl = colIndex; }
                if (Dgv.Columns[colIndex].Name == "联友需求量") { colLYneedNum = colIndex; }
                if (Dgv.Columns[colIndex].Name == "分公司库存量") { colFGSkysl = colIndex; }
            }

            for (int rowIndex = 0; rowIndex < Dgv.RowCount; rowIndex++)
            {
                try
                {
                    if (Dgv.Rows[rowIndex].Cells[10].Value.ToString() != "")
                    {
                        if (float.Parse(Dgv.Rows[rowIndex].Cells[colJYkysl].Value.ToString()) > 0)
                        {
                            Dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                        }
                        if (float.Parse(Dgv.Rows[rowIndex].Cells[colLYneedNum].Value.ToString()) < float.Parse(Dgv.Rows[rowIndex].Cells[colJYkysl].Value.ToString()))
                        {
                            Dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                        }
                    }
                    else if (float.Parse(Dgv.Rows[rowIndex].Cells[colLYneedNum].Value.ToString()) > float.Parse(Dgv.Rows[rowIndex].Cells[colFGSkysl].Value.ToString()))
                    {
                        Dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                catch
                {
                    continue;
                }
            }

            //行颜色
            DgvOpt.SetRowColor(DgvMain);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowDgv();
            }
        }

        private void BtnData2Excel_Click(object sender, EventArgs e)
        {
            Excel.Excel_Base excelObj = new Excel.Excel_Base();


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Excel 2007|*.xlsx";
            saveFileDialog.FileName = "联友物料需求量导出_生成日期" + GenerateDate + "_导出日期" + DateTime.Now.ToString("yyyyMMdd");
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

        private void BtnGetData_Click(object sender, EventArgs e)
        {
            ShowDgv();
        }

        private void DgvMain_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DgvRowAlarm(DgvMain);
        }
    }
}
